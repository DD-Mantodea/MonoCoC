using System;
using MonoCoC.Contents.Datas.Vanilla;
using System.Linq;
using System.Collections.Generic;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Contents.Enemies;

namespace MonoCoC.Contents.Combat
{
    public class CombatController
    {
        public static CombatController? Instance;

        public static bool InCombat;

        public CombatContext Context = null!;

        public CombatState State = new();

        public CombatAiInterpreter Interpreter = null!;

        public CombatResolver Resolver = null!;

        public CombatReward Reward = null!;

        public enum AbilityActor
        {
            Player,

            Enemy
        }

        public const int PlayerSlot = 0;

        public readonly record struct AbilityUsage(int Round, AbilityActor Actor, int ActorSlot, Identifier? ActorId, Identifier Ability);

        public readonly Dictionary<Enemy, int> enemySlots = [];

        public int nextEnemySlot = 1;

        public virtual int SlotOf(Enemy enemy)
        {
            if (enemySlots.TryGetValue(enemy, out var slot))
                return slot;

            slot = nextEnemySlot++;

            enemySlots[enemy] = slot;

            return slot;
        }

        public readonly List<AbilityUsage> abilityLog = [];

        public IReadOnlyList<AbilityUsage> AbilityLog => abilityLog;

        public Identifier? CurrAbilityUsed { get; set; }

        /// <summary>原版 CombatAbilities.hx:2684 Power Through：眩晕且满足条件时给出恢复选项。</summary>
        private bool _stunRecoveryOffered;

        public Identifier? LastAbilityUsed => abilityLog.Count > 0 ? abilityLog[^1].Ability : null;

        public virtual void LogAbility(Identifier ability, AbilityActor actor, int actorSlot, Identifier? actorId = null)
        {
            abilityLog.Add(new AbilityUsage(State.Round, actor, actorSlot, actorId, ability));

            if (actor == AbilityActor.Player)
                CurrAbilityUsed = ability;
        }

        public virtual bool PlayerUsed(Identifier ability) =>

            abilityLog.Exists(e => e.Actor == AbilityActor.Player && e.Ability == ability);

        public virtual bool LustVictory() =>

            Context.Enemy.Lust >= Context.Enemy.MaxLust && !Context.Enemy.IgnoreLust;

        public virtual bool EnemySlotUsed(int slot, Identifier ability) =>

            abilityLog.Exists(e => e.Actor == AbilityActor.Enemy && e.ActorSlot == slot && e.Ability == ability);

        public virtual bool EnemyUsed(Identifier enemyId, Identifier ability) =>

            abilityLog.Exists(e => e.Actor == AbilityActor.Enemy && e.ActorId == enemyId && e.Ability == ability);

        public virtual List<AbilityUsage> UsageBySlot(int slot) => abilityLog.FindAll(e => e.ActorSlot == slot);

        public virtual bool AnyoneUsed(Identifier ability) => abilityLog.Exists(e => e.Ability == ability);

        public virtual List<AbilityUsage> RoundUsage(int round) => abilityLog.FindAll(e => e.Round == round);

        public virtual void Start()
        {
            Instance = this;

            abilityLog.Clear();

            enemySlots.Clear();

            nextEnemySlot = 1;

            CurrAbilityUsed = null;

            InCombat = true;

            // 原版 Combat.hx:181：每场战斗重置时之盾的使用标记
            Context.Player.Shield?.Save.SetBool("used", false);

            Context.Enemy.Health = Context.Enemy.MaxHealth;

            Context.Enemy.Lust = 0;

            CombatRangeData.ResetDistance(Context.Enemy);

            _ = CombatAbilityHandlerRegistry.IsMetRegistered;

            _ = CombatConditionRegistry.IsRegistered;

            Interpreter = new CombatAiInterpreter
            {
                Context = Context,

                State = State
            };

            Resolver = new CombatResolver(Context, State);

            Reward = new CombatReward(Context);

            Menu();
        }

        public virtual void ShowMenu() => Menu();

        protected virtual void Menu()
        {

            var endOfRound = Context.EndOfRoundOverride;

            Context.EndOfRoundOverride = null;

            var log = Context.Log.ToArray();

            Context.Log.Clear();

            Context.Clear();

            Context.ClearButtons();

            if (log.Length > 0)
                Context.Write(string.Join("\n", log) + "\n\n");

            Context.Write("Combat.Menu.Turn".Localized(State.Round + 1));

            Context.Write("\n" + "Combat.Menu.Fighting".Localized(Context.Enemy.ShortName));

            CombatScene.RefreshStats(Context);

            if (Context.Player.HasStatusEffect(StatusEffectIds.TFSupercharging))
            {
                Context.ClearButtons();

                Context.AddButton(0, "Combat.Terra.Supernova.GatherButton".Localized(), GatherSupernovaEnergy);

                return;
            }

            if (_stunRecoveryOffered)
            {
                _stunRecoveryOffered = false;

                Context.Write("Combat.Status.TooStunned".Localized() + "\n");

                Context.ClearButtons();

                Context.AddButton(0, "Combat.PowerThrough.Option".Localized(), PowerThroughRecover);

                Context.AddButton(1, "Combat.PowerThrough.Wait".Localized(), StunWait);

                return;
            }

            if (!string.IsNullOrEmpty(Context.IntroDescription))
                Context.Write("\n" + Context.IntroDescription);
            else if (!string.IsNullOrEmpty(Context.Enemy.Description))
                Context.Write("\n" + Context.Enemy.Description);

            var ratio = Context.Enemy.MaxHealth <= 0 ? 1 : Context.Enemy.Health / Context.Enemy.MaxHealth;

            var condition = ratio switch
            {
                >= 1 => "Combat.Menu.Condition.Perfect".Localized(),

                > 0.75f => "Combat.Menu.Condition.Fine".Localized(),

                > 0.5f => "Combat.Menu.Condition.Wounded".Localized(),

                > 0.25f => "Combat.Menu.Condition.Hurt".Localized(),

                _ => "Combat.Menu.Condition.Dying".Localized()
            };

            var pronoun = Context.Enemy.Gender == Gender.Female ? "she" : Context.Enemy.Gender == Gender.Male ? "he" : "it";

            Context.Write("\n\n" + "Combat.Menu.Status".Localized(Context.Enemy.ShortName));

            Context.Write("\n" + "Combat.Menu.See".Localized(pronoun, condition));

            if (Context.Enemy.Lust >= Context.Enemy.MaxLust)
                Context.Write("\n" + "Combat.Menu.Overwhelmed".Localized(pronoun));

            if (endOfRound != null)
            {
                Context.AddButton(14, "Common.Button.Next".Localized(), endOfRound);

                return;
            }

            if (IsPlayerBound())
            {
                Context.Write("\n" + "Combat.Menu.Bound".Localized());

                Context.AddButton(0, "Combat.Button.Struggle".Localized(), Struggle);

                Context.AddButton(1, "Combat.Button.Wait".Localized(), Wait);

                return;
            }

            if (Context.Player.HasStatusEffect(StatusEffectIds.SentinelPhysicalDisabled))
                Context.AddButtonDisabled(0, "Combat.Button.Attack".Localized(), "Combat.Disabled.PhysicalSealed".Localized());
            else
                Context.AddButton(0, "Combat.Button.Attack".Localized(), Attack);

            if (Context.Player.HasStatusEffect(StatusEffectIds.SentinelNoTease))
                Context.AddButtonDisabled(1, "Combat.Button.Tease".Localized(), "Combat.Disabled.TeaseSealed".Localized());
            else
                Context.AddButton(1, "Combat.Button.Tease".Localized(), Tease);

            if (Context.Player.HasStatusEffect(StatusEffectIds.SentinelOmniSilence))
                Context.AddButtonDisabled(2, "Combat.Button.Spells".Localized(), "Combat.Disabled.MagicSealed".Localized());
            else if (CountAbilities(SpellSchools) == 0)
                Context.AddButtonDisabled(2, "Combat.Button.Spells".Localized(), "Combat.Disabled.NoSpells".Localized());
            else
                Context.AddButton(2, "Combat.Button.Spells".Localized(), Spells);

            if (Context.Player.HasStatusEffect(StatusEffectIds.SentinelPhysicalDisabled))
                Context.AddButtonDisabled(5, "Combat.Button.PhysicalSpecials".Localized(), "Combat.Disabled.PhysicalSealed".Localized());
            else if (AvailableAbilities(CombatAbilityCategory.Physical).Count == 0)
                Context.AddButtonDisabled(5, "Combat.Button.PhysicalSpecials".Localized(), "Combat.Disabled.NoPhysicalSpecials".Localized());
            else
                Context.AddButton(5, "Combat.Button.PhysicalSpecials".Localized(), PhysicalSpecials);

            if (Context.Player.HasStatusEffect(StatusEffectIds.SentinelOmniSilence))
                Context.AddButtonDisabled(6, "Combat.Button.MagicalSpecials".Localized(), "Combat.Disabled.MagicSealed".Localized());
            else if (AvailableAbilities(CombatAbilityCategory.Magical).Count == 0)
                Context.AddButtonDisabled(6, "Combat.Button.MagicalSpecials".Localized(), "Combat.Disabled.NoMagicalSpecials".Localized());
            else
                Context.AddButton(6, "Combat.Button.MagicalSpecials".Localized(), MagicalSpecials);

            Context.AddButton(3, "Combat.Button.Items".Localized(), Items);

            Context.AddButton(8, "Combat.Button.Fantasize".Localized(), Fantasize);

            if (Context.Enemy.Distance == CombatDistance.Distant)
                Context.AddButtonDisabled(10, "Combat.Button.Distance".Localized(), "Combat.Disabled.DistanceFar".Localized());
            else if (!Context.Player.CanMove())
                Context.AddButtonDisabled(10, "Combat.Button.Distance".Localized(), "Combat.Disabled.CannotMove".Localized());
            else
                Context.AddButton(10, "Combat.Button.Distance".Localized(), Distance);

            if (Context.Enemy.Distance == CombatDistance.Melee)
                Context.AddButtonDisabled(11, "Combat.Button.Approach".Localized(), "Combat.Disabled.ApproachClose".Localized());
            else if (!Context.Player.CanMove())
                Context.AddButtonDisabled(11, "Combat.Button.Approach".Localized(), "Combat.Disabled.CannotMove".Localized());
            else
                Context.AddButton(11, "Combat.Button.Approach".Localized(), Approach);

            if (Context.CanFlee)
                Context.AddButton(4, "Combat.Button.Run".Localized(), Run);
            else
                Context.AddButtonDisabled(4, "Combat.Button.Run".Localized(), "Combat.Disabled.CannotFlee".Localized());

            Context.AddButton(7, "Combat.Button.Wait".Localized(), Wait);
        }

        public virtual void Attack()
        {
            Resolver.PlayerAttack();

            AfterPlayerAction();
        }

        public static readonly Identifier TeaseAbilityId = CombatAbilityIds.Tease;

        public static readonly Identifier FantasizeAbilityId = CombatAbilityIds.Fantasize;

        public virtual void Tease()
        {
            LogAbility(TeaseAbilityId, AbilityActor.Player, PlayerSlot);

            Resolver.PlayerTease();

            AfterPlayerAction();
        }

        public virtual void Items() => Scenes.Vanilla.InventoryScene.InventoryMenu(Menu, AfterItem);

        public virtual void Spells() => MagicMenu(null);

        public virtual void PhysicalSpecials() => AbilityListMenu(CombatAbilityCategory.Physical, "P. Specials", Menu);

        public virtual void MagicalSpecials() => AbilityListMenu(CombatAbilityCategory.Magical, "M. Specials", Menu);

        protected virtual int CountAbilities(IEnumerable<CombatAbilityCategory> categories)
        {
            var total = 0;

            foreach (var category in categories)
                total += AvailableAbilities(category).Count;

            return total;
        }

        protected virtual void MagicMenu(CombatAbilityCategory? school)
        {
            Context.Clear();

            Context.ClearButtons();

            if (school == null)
            {
                Context.Write("Combat.Menu.Spells".Localized());

                var slot = 0;

                foreach (var category in SpellSchools)
                {
                    var label = SchoolName(category);

                    if (AvailableAbilities(category).Count == 0)
                        Context.AddButtonDisabled(slot, label, "Combat.Disabled.NoSpellsSchool".Localized());
                    else
                        Context.AddButton(slot, label, () => MagicMenu(category));

                    slot++;
                }

                Context.AddButton(14, "Combat.Button.Back".Localized(), Menu);

                return;
            }

            AbilityListMenu(school.Value, SchoolName(school.Value), () => MagicMenu(null));
        }

        protected virtual void AbilityListMenu(CombatAbilityCategory category, string title, Action back)
        {
            Context.Clear();

            Context.ClearButtons();

            var abilities = AvailableAbilities(category);

            Context.Write($"<b>{title}</b>\n\n");

            for (var index = 0; index < abilities.Count && index < 12; index++)
            {
                var ability = abilities[index];

                var label = $"{ability.NameKey.Localized()} ({ability.Cost})";

                // 原版 CombatAbility.hx:277：只有开启「技能冷却」模式时才检查冷却
            if (Settings.ModeSettings.Instance.Cooldowns && State.IsOnCooldown(ability.Id))
                {
                    Context.AddButtonDisabled(index, label,

                        "Combat.Disabled.OnCooldown".Localized(State.CooldownOf(ability.Id)));

                    continue;
                }

                if (ability.Cost > 0 && !Context.Player.HasFatigue(ability.Cost, ability.Fatigue))
                {
                    Context.AddButtonDisabled(index, label, "Combat.Disabled.TooTired".Localized());

                    continue;
                }

                if (!string.IsNullOrEmpty(ability.DisabledWhen.Path) &&
                    Context.Player.HasStatusEffect(ability.DisabledWhen))
                {
                    Context.AddButtonDisabled(index, label,

                        ability.DisabledTooltip == "" ? "Combat.Disabled.CannotUse".Localized() : ability.DisabledTooltip.Localized());

                    continue;
                }

                Context.Write($"{ability.NameKey.Localized()}: {ability.DescriptionKey.Localized()}\n");

                Context.AddButton(index, label, () => UseAbility(ability));
            }

            Context.AddButton(14, "Combat.Button.Back".Localized(), back);
        }

        protected static readonly CombatAbilityCategory[] SpellSchools =

        [
            CombatAbilityCategory.WhiteMagic,

            CombatAbilityCategory.BlackMagic,

            CombatAbilityCategory.GrayMagic
        ];

        protected static string SchoolName(CombatAbilityCategory category) => category switch
        {
            CombatAbilityCategory.WhiteMagic => "Combat.School.White".Localized(),

            CombatAbilityCategory.BlackMagic => "Combat.School.Black".Localized(),

            _ => "Combat.School.Gray".Localized()
        };

        protected virtual List<CombatAbilityDefinition> AvailableAbilities(CombatAbilityCategory category)
        {
            var result = new List<CombatAbilityDefinition>();

            foreach (var id in CombatAbilityRegistry.Abilities.Ids)
            {
                var ability = CombatAbilityRegistry.Get(id);

                if (ability == null || ability.Category != category)
                    continue;

                var hasKnownGate = !string.IsNullOrEmpty(ability.KnownWhen.Path);

                var hasConditionGate = !string.IsNullOrEmpty(ability.AvailableWhen.Path);

                if (!hasKnownGate && !hasConditionGate)
                    continue;

                if (hasKnownGate && !Context.Player.HasStatusEffect(ability.KnownWhen))
                    continue;

                if (hasConditionGate && !CombatConditionRegistry.IsMet(ability.AvailableWhen, Context, State))
                    continue;

                result.Add(ability);
            }

            result.Sort((left, right) => string.CompareOrdinal(left.Id.Path, right.Id.Path));

            return result;
        }

        protected virtual void UseAbility(CombatAbilityDefinition ability)
        {

            if (!CombatStatusHooks.BeforePlayerAbilityUse(Context, ability))
            {
                AfterPlayerAction();

                return;
            }

            LogAbility(ability.Id, AbilityActor.Player, PlayerSlot);

            Context.Player.ChangeFatigue(ability.Cost, ability.Fatigue);

            if (Settings.ModeSettings.Instance.Cooldowns)
                State.SetCooldown(ability.Id, ability.Cooldown);

            if (!string.IsNullOrEmpty(ability.Handler.Path) &&
                CombatAbilityHandlerRegistry.TryGet(ability.Handler, out var handler))

                handler(Context, ability);
            else
                Context.Write("Combat.Disabled.NoHandler".Localized(ability.Id.Path));

            AfterPlayerAction();
        }

        public virtual void Fantasize()
        {
            LogAbility(FantasizeAbilityId, AbilityActor.Player, PlayerSlot);

            var player = Context.Player;

            var lust = 5 + Context.Random.Next((int)(player.Libido / 8 + player.Corruption / 8));

            if (Context.Enemy is Enemies.Vanilla.FrostGiantEnemy
                && player.HasStatusEffect(StatusEffectIds.GiantBoulder))
            {
                var giantLust = 10 + Context.Random.Next(Math.Max(1, (int)(player.Libido / 5 + player.Corruption / 8)));

                player.Lust = Math.Min(player.MaxLust, player.Lust + giantLust);

                Context.WriteAction("Combat.Fantasy.LustGain".Localized(giantLust));

                Handlers.FrostGiantCombatHandlers.GiantBoulderFantasize(Context);

                if (player.Lust >= player.MaxLust)
                {
                    State.Outcome = CombatOutcome.LustDefeat;

                    Finish();

                    return;
                }

                EnemyTurn();

                return;
            }

            Context.Enemy.OutputDefaultFantasy(Context, lust);

            player.Lust = Math.Min(player.MaxLust, player.Lust + lust);

            Context.WriteAction("Combat.Fantasy.LustGain".Localized(lust));

            if (player.Lust >= player.MaxLust)
            {
                State.Outcome = CombatOutcome.LustDefeat;

                Finish();

                return;
            }

            AfterPlayerAction();
        }

        public virtual void AfterItem() => EnemyTurn();

        public virtual bool IsPlayerBound() =>

            Context.Player.HasStatusEffect(StatusEffectIds.TentacleBind)

            || Context.Player.HasStatusEffect(StatusEffectIds.GiantGrabbed)

            || Context.Player.HasStatusEffect(StatusEffectIds.ScorpGrabbed);

        public virtual void Struggle()
        {
            Context.Enemy.Struggle(Context);

            AfterPlayerAction();
        }

        public virtual void EndLustLoss()
        {
            if (State.Outcome != null)
                return;

            State.Outcome = CombatOutcome.LustDefeat;

            Finish();
        }

        public virtual void Wait()
        {

            Context.PlayerWaited = true;

            if (!Context.Enemy.ReactWrapper(Context, CombatReactionContext.PlayerWaited))
            {
                AfterPlayerAction();

                return;
            }

            Context.Player.ChangeFatigue(-10);

            // 原版 Combat.hx:770：爪姿态 + 爪精通 5 级时，等待升级为潜伏
            if (Context.Player.HasStatusEffect(StatusEffectIds.ClawStance)
                && Context.Player.MasteryLevel(new Identifier("coc", "Claw")) >= 5)
            {
                Context.Log.Add("Combat.Wait.Lurk".Localized());

                Context.Player.CreateStatusEffect(StatusEffectIds.Lurk);
            }
            else
                Context.Log.Add("Combat.Wait.Recover".Localized());

            AfterPlayerAction();
        }

        public virtual void Distance()
        {
            Context.Player.ChangeFatigue(10);

            Context.Log.Add("Combat.Distance.Self".Localized());

            if (!Context.Enemy.ReactWrapper(Context, CombatReactionContext.Distanced))
            {
                AfterPlayerAction();

                return;
            }

            if (TryMove(CombatDistance.Distant))
                Context.Log.Add("Combat.Distance.Success".Localized(Context.Enemy.ShortName));
            else
                Context.Log.Add("Combat.Distance.Fail".Localized(Context.Enemy.ShortName));

            AfterPlayerAction();
        }

        public virtual void Approach()
        {
            Context.Player.ChangeFatigue(10);

            Context.Log.Add("Combat.Approach.Self".Localized());

            if (!Context.Enemy.ReactWrapper(Context, CombatReactionContext.Approached))
            {
                AfterPlayerAction();

                return;
            }

            if (TryMove(CombatDistance.Melee))
                Context.Log.Add("Combat.Approach.Success".Localized(Context.Enemy.ShortName));
            else
                Context.Log.Add((Context.Enemy.Gender switch
                {
                    Gender.Female => "Combat.Approach.FailFemale",

                    Gender.Male => "Combat.Approach.FailMale",

                    _ => "Combat.Approach.FailNeuter"
                }).Localized(Context.Enemy.ShortName));

            AfterPlayerAction();
        }

        protected virtual bool TryMove(CombatDistance destination)
        {
            var enemy = Context.Enemy;

            var success = Context.Random.NextDouble() * 100 < Context.Player.MovementChance(enemy) ||

                !enemy.ShouldMove(Opposite(destination), true) ||

                !enemy.CanMove();

            if (!success)
                return false;

            if (destination == CombatDistance.Distant)
                CombatRangeData.MoveDistantSafe(enemy);
            else
                CombatRangeData.CloseDistance(enemy);

            return true;
        }

        public static CombatDistance Opposite(CombatDistance distance) =>

            distance == CombatDistance.Distant ? CombatDistance.Melee : CombatDistance.Distant;

        public virtual void Run()
        {
            if (CombatStatusTicker.PreventsFlee(Context))
            {
                Context.Log.Add("You try to flee but get stuck in the sticky white goop surrounding you.");

                EnemyTurn();

                return;
            }

            if (Context.Enemy.OnPlayerRunAttempt(Context))
            {
                EnemyTurn();

                return;
            }

            if (Context.Enemy.GuaranteesEscape(Context))
            {
                var forced = Context.Enemy.RunCheck(Context);

                if (!string.IsNullOrEmpty(forced))
                    Context.Log.Add(forced);

                State.Outcome = CombatOutcome.Fled;

                Finish();

                return;
            }

            if (Context.CanFlee && Context.Random.NextDouble() < CombatCalculator.EscapeChance(Context))
            {

                var custom = Context.Enemy.RunCheck(Context);

                if (!string.IsNullOrEmpty(custom))
                    Context.Log.Add(custom);

                State.Outcome = CombatOutcome.Fled;

                Finish();

                return;
            }

            Context.Log.Add("You fail to escape.");

            EnemyTurn();
        }

        protected virtual void AfterPlayerAction()
        {
            if (Resolver.CheckEnd())
            {
                Finish();

                return;
            }

            EnemyTurn();
        }

        protected virtual void TickBleeding(Creature creature, string who)
        {
            if (!creature.IsBleeding())
                return;

            var damage = creature.BleedDamage();

            if (damage > 0)
            {
                creature.HealthChange(-damage);

                Context.Append("Combat.Bleed.Tick".Localized(damage));
            }

            creature.ChangeStatusValue(StatusEffectIds.IzmaBleed, 0, -1);

            if (creature.StatusEffectValue(StatusEffectIds.IzmaBleed, 0) <= 0)
                creature.RemoveStatusEffect(StatusEffectIds.IzmaBleed);
        }

        protected virtual void ApplyBleedingRound()
        {
            TickBleeding(Context.Player, "player");

            TickBleeding(Context.Enemy, "enemy");
        }

        protected virtual bool IsEnemyIncapacitated()
        {
            if (Context.Enemy.HasStatusEffect(StatusEffectIds.Stunned) &&
                !Context.Enemy.HandleStun(Context))

                return true;

            if (Context.Enemy.HasStatusEffect(StatusEffectIds.Fear) &&
                !Context.Enemy.HandleFear(Context))

                return true;

            return false;
        }

        public virtual void EnemyTurn()
        {
            State.PlayerTurn = false;

            ApplyBleedingRound();

            // 原版 Combat.hx:2650：TimeFrozen 期间敌人不行动，玩家继续行动
            if (Context.Player.HasStatusEffect(StatusEffectIds.TimeFrozen))
            {
                var frozen = Context.Player.StatusEffectByType(StatusEffectIds.TimeFrozen);

                frozen?.Duration -= 1;

                if (frozen == null || frozen.Duration <= 0)
                {
                    Context.Player.RemoveStatusEffect(StatusEffectIds.TimeFrozen);

                    Context.Append("Combat.Special.FreezeTime.Fade".Localized());
                }
                else
                {
                    State.TickCooldowns();

                    State.Round++;

                    State.PlayerTurn = true;

                    if (Resolver.CheckEnd())
                    {
                        Finish();

                        return;
                    }

                    Menu();

                    return;
                }
            }

            // 原版 CombatAbilities.hx:6297 tfScorchCheck：战场火焰灼烧行动的敌人
            if (Context.Player.HasStatusEffect(StatusEffectIds.TFScorch)
                && Context.Enemy.Health > 0)
            {
                Context.Append("Combat.Terra.Scorch.Burn".Localized());

                var scorch = Handlers.PlayerTerraFormHandlers.ScorchDamage(Context);

                CombatCalculator.DamageEnemy(Context, scorch, CombatDamageKind.Magical);
            }

            foreach (var extra in Context.Enemies.Where(e => e != Context.Enemy && e.Health > 0 && e.Lust < e.MaxLust).ToList())
            {
                var savedTarget = Context.Enemy;

                Context.Enemy = extra;

                extra.TookAction = false;

                extra.CombatRoundUpdate(Context);

                if (!extra.TookAction && !IsEnemyIncapacitated() && !extra.BeforeCombatAction(Context))
                {
                    LogAbility(new Identifier("coc", "basic_attack"), AbilityActor.Enemy, SlotOf(extra), extra.Id);

                    Resolver.EnemyBasicAttack();
                }

                extra.TookAction = false;

                Context.Enemy = savedTarget;

                if (Resolver.CheckEnd())
                {
                    Finish();

                    return;
                }
            }

            Context.Enemy.CombatRoundUpdate(Context);

            if (!Context.Enemy.TookAction && !IsEnemyIncapacitated() && !Context.Enemy.BeforeCombatAction(Context))
            {

                var forced = Context.Enemy.ForcedAbility(Context);

                if (forced != null)
                {
                    var forcedAbility = CombatAbilityRegistry.Get(forced.Value);

                    if (forcedAbility != null)
                    {
                        LogAbility(forcedAbility.Id, AbilityActor.Enemy, SlotOf(Context.Enemy), Context.Enemy.Id);

                        Resolver.EnemyAction(forcedAbility);
                    }
                    else
                        Resolver.EnemyBasicAttack();
                }
                else
                {
                    var action = Context.Enemy.ChooseCombatAction(Context, Interpreter);

                    if (action != null)
                    {
                        var ability = CombatAbilityRegistry.Get(action.Ability);

                        if (ability != null)
                        {
                            LogAbility(ability.Id, AbilityActor.Enemy, SlotOf(Context.Enemy), Context.Enemy.Id);

                            Interpreter.Execute(action);

                            Resolver.EnemyAction(ability);
                        }
                        else
                            Resolver.EnemyBasicAttack();
                    }
                    else
                        Resolver.EnemyBasicAttack();
                }
            }

            Context.Enemy.TookAction = false;

            Context.PlayerWaited = false;

            State.TickCooldowns();

            CombatStatusTicker.Tick(Context);

            State.Round++;

            CurrAbilityUsed = null;

            State.PlayerTurn = true;

            if (Resolver.CheckEnd())
            {
                Finish();

                return;
            }

            if (PlayerIncapacitated())
            {
                EnemyTurn();

                return;
            }

            Menu();
        }

        private void GatherSupernovaEnergy()
        {
            Handlers.PlayerSupernovaHandlers.GatherEnergy(Context);

            EnemyTurn();
        }

        private void PowerThroughRecover()
        {
            Context.Clear();

            Context.Write("Combat.PowerThrough.Intro".Localized());

            Context.Player.RemoveStatusEffect(StatusEffectIds.Stunned);

            Context.Player.ChangeFatigue(15);

            Menu();
        }

        private void StunWait()
        {
            _stunRecoveryOffered = false;

            Context.Player.RemoveStatusEffect(StatusEffectIds.Stunned);

            EnemyTurn();
        }

        protected virtual bool PlayerIncapacitated()
        {
            var skipped = false;

            if (Context.Player.HasStatusEffect(StatusEffectIds.Stunned))
            {
                // 原版 Power Through：Resolute 且坚韧 >75 时可以花 15 体力立刻恢复
                if (Context.Player.HasPerk(PerkIds.Resolute) && Context.Player.Toughness > 75)
                {
                    _stunRecoveryOffered = true;

                    return false;
                }

                Context.Write("Combat.Status.TooStunned".Localized());

                Context.Player.RemoveStatusEffect(StatusEffectIds.Stunned);

                skipped = true;
            }

            if (Context.Player.HasStatusEffect(StatusEffectIds.Whispered))
            {
                Context.Write("Combat.Status.TooAddled".Localized());

                Context.Player.RemoveStatusEffect(StatusEffectIds.Whispered);

                skipped = true;
            }

            if (Context.Player.HasStatusEffect(StatusEffectIds.Confusion))
            {
                Context.Write("Combat.Status.TooConfused".Localized());

                Context.Player.RemoveStatusEffect(StatusEffectIds.Confusion);

                skipped = true;
            }

            return skipped;
        }

        protected virtual void ApplyCombatLoss()
        {
            var player = Context.Player;

            var inDungeon = Dungeons.DungeonSession.InDungeon;

            if (inDungeon)
                Dungeons.DungeonSession.LastCombatLost = true;

            var gemsLost = (int)Math.Round(Context.Random.Next(10) + 1 + Math.Floor(Context.Enemy.Level / 2f));

            if (inDungeon)
                gemsLost += (int)(20 + Math.Floor(Context.Enemy.Level * 2f));

            if (gemsLost > player.Gems)
                gemsLost = player.Gems;

            var hours = Context.Enemy.HandleCombatLossText(Context, inDungeon, gemsLost);

            player.LoseGems(gemsLost);

            if (hours > 0)
                Scene.View.Time.PassTime(hours);
        }

        public virtual void ClearStatuses()
        {
            Context.Player.ClearCombatStatuses();

            CombatCalculator.ClearLinkedBuffs(Context.Player);

            if (Context.Enemy != null)
                CombatCalculator.ClearLinkedBuffs(Context.Enemy);
        }

        public virtual void Finish()
        {
            InCombat = false;

            Context.Player.ClearCombatStatuses();

            if (Context.Player != null)
                CombatCalculator.ClearLinkedBuffs(Context.Player);

            if (Context.Enemy != null)
                CombatCalculator.ClearLinkedBuffs(Context.Enemy);

            Context.Player?.ArmorDefense = Context.Player.Armor.DefenseFor(Context.Player);

            CombatScene.LastEnemy = Context.Enemy;

            CombatScene.LastOutcome = State.Outcome;

            var log = Context.Log.ToArray();

            Context.Log.Clear();

            Context.Clear();

            if (log.Length > 0)
                Context.Write(string.Join("\n", log) + "\n\n");

            Context.ClearButtons();

            UI.Components.Views.GameView.Instance?.HideCombatStats();

            var hpVictory = State.Outcome is CombatOutcome.HpVictory;

            switch (State.Outcome)
            {
                case CombatOutcome.HpVictory:
                case CombatOutcome.LustVictory:
                    Reward.Award();

                    if (Context.Enemy.Defeated(hpVictory))
                        return;

                    break;

                case CombatOutcome.HpDefeat:
                case CombatOutcome.LustDefeat:
                    ApplyCombatLoss();

                    if (Context.Enemy.Won(hpVictory))
                        return;

                    break;
            }

            Context.Write(State.Outcome switch
            {
                CombatOutcome.HpVictory => "You win the fight.",

                CombatOutcome.LustVictory => "Your opponent gives in to lust.",

                CombatOutcome.HpDefeat => "You are defeated.",

                CombatOutcome.LustDefeat => "You are overwhelmed by lust.",

                _ => "You escape."
            });

            if (State.Outcome is CombatOutcome.HpVictory or CombatOutcome.LustVictory &&
                Context.Enemy.OnDefeated != null)
            {
                var callback = Context.Enemy.OnDefeated;

                Context.AddButton(14, "Combat.Button.Continue".Localized(), () =>
                {
                    State.Outcome = null;

                    var previous = Instance;

                    callback(hpVictory);

                    if (Instance == previous)
                        Instance = null;
                });

                return;
            }

            if (State.Outcome is CombatOutcome.HpDefeat or CombatOutcome.LustDefeat &&
                Context.Enemy.OnWon != null)
            {
                var callback = Context.Enemy.OnWon;

                Context.AddButton(14, "Combat.Button.Continue".Localized(), () =>
                {
                    State.Outcome = null;

                    var previous = Instance;

                    callback(hpVictory);

                    if (Instance == previous)
                        Instance = null;
                });

                return;
            }

            var outcomeScene = State.Outcome switch
            {
                CombatOutcome.HpVictory or CombatOutcome.LustVictory => Context.Enemy.DefeatScene,

                CombatOutcome.HpDefeat or CombatOutcome.LustDefeat => Context.Enemy.VictoryScene,

                _ => Context.Enemy.FleeScene
            };

            if (!string.IsNullOrEmpty(scenePath(outcomeScene)))
            {
                Context.AddButton(14, "Combat.Button.Continue".Localized(), () =>
                {
                    State.Outcome = null;

                    Instance = null;

                    Scene.Go(SceneRegistry.CreateOrDefault(outcomeScene));
                });

                return;
            }

            Context.AddButton(14, "Combat.Button.Continue".Localized(), () =>
            {
                State.Outcome = null;

                Instance = null;

                Context.ReturnToCamp();
            });
        }

        public static string scenePath(Identifier id) => id.Path ?? "";
    }

    public class CombatReward(CombatContext context)
    {
        public CombatContext Context { get; } = context;

        public virtual void Award()
        {
            Context.Player.Gems += Context.Enemy.Gems;

            var experience = Context.Enemy.Experience > 0 ? Context.Enemy.Experience : Context.Enemy.TotalExperience();

            Context.Player.Experience += experience;

            Context.Write("Combat.Reward.Gems".Localized(Context.Enemy.Gems, experience));

            if (Context.Enemy.Definition?.ChainLoot == true)
            {
                AwardChain(Context.Enemy.Loot);

                return;
            }

            if (Context.Enemy.Definition?.WeightedLoot == true)
            {
                AwardWeighted(Context.Enemy.Loot);

                return;
            }

            foreach (var loot in Context.Enemy.Loot)
            {
                if (Context.Random.NextDouble() > loot.Chance)
                    continue;

                Give(loot);
            }
        }

        public virtual void AwardWeighted(IReadOnlyList<EnemyLoot> loot)
        {
            var total = 0f;

            foreach (var entry in loot)
                total += Math.Max(0f, entry.Weight);

            if (total <= 0)
                return;

            var roll = Context.Random.NextDouble() * total;

            foreach (var entry in loot)
            {
                roll -= Math.Max(0f, entry.Weight);

                if (roll > 0)
                    continue;

                if (!string.IsNullOrEmpty(entry.Item.Path))
                    Give(entry);

                return;
            }
        }

        public virtual void AwardChain(IReadOnlyList<EnemyLoot> loot)
        {
            if (loot.Count == 0)
                return;

            for (var index = 0; index < loot.Count; index++)
            {
                var entry = loot[index];

                var isLast = index == loot.Count - 1;

                if (isLast || Context.Random.NextDouble() <= entry.Chance)
                {
                    Give(entry);

                    return;
                }
            }
        }

        public virtual void Give(EnemyLoot loot)
        {
            var quantity = loot.Min + Context.Random.Next(Math.Max(1, loot.Max - loot.Min + 1));

            var item = ItemRegistry.Create(loot.Item);

            Context.Player.Inventory.Add(item, quantity);

            if (!Context.Enemy.HandleAwardItemText(Context, loot.Item))

                if (quantity == 1)
                    Context.Write("Combat.Reward.LootOne".Localized(item.LongName));
                else
                    Context.Write("Combat.Reward.LootMany".Localized(quantity, item.LongName));
        }
    }
}