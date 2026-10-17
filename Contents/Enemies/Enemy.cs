using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies
{
    public enum CombatDamageKind
    {
        Melee,

        Ranged,

        Magical
    }

    public abstract class Enemy : Creature, IIdentifiable
    {
        public Identifier Id { get; set; }

        public EnemyDefinition? Definition;

        public Identifier? AiId;

        public bool Charged;

        public AvoidDamageParameters NormalAttackAvoid = new();

        public virtual void ApplyDefinitionEquipment(EnemyDefinition definition)
        {
            if (PickEquipment(definition.Weapons) is { } weapon)
                WeaponName = weapon.Item.Path;

            if (PickEquipment(definition.Armors) is { } armor)
                ArmorName = armor.Item.Path;

            if (PickEquipment(definition.Shields) is { } shield)
                ShieldName = shield.Item.Path;

            if (definition.WeaponAttack is { } weaponAttack)
                WeaponAttack = weaponAttack;

            if (definition.ArmorDefense is { } armorDefense)
                ArmorDefense = armorDefense;

            if (definition.ShieldBlock is { } shieldBlock)
                ShieldBlock = shieldBlock;

            if (definition.FireResistance is { } fireResistance)
                FireResistance = fireResistance;
        }

        public static EnemyEquipment? PickEquipment(List<EnemyEquipment> candidates)
        {
            if (candidates.Count == 0)
                return null;

            var total = 0f;

            foreach (var candidate in candidates)
                total += Math.Max(0f, candidate.Weight);

            if (total <= 0f)
                return candidates[0];

            var roll = Random.Shared.NextDouble() * total;

            foreach (var candidate in candidates)
            {
                roll -= Math.Max(0f, candidate.Weight);

                if (roll < 0f)
                    return candidate;
            }

            return candidates[^1];
        }

        public virtual void Configure(EnemyDefinition definition)
        {
            Definition = definition;

            NormalAttackAvoid = new AvoidDamageParameters
            {
                DoDodge = definition.AttackAvoid.Dodge,

                DoParry = definition.AttackAvoid.Parry,

                DoBlock = definition.AttackAvoid.Block,

                DoCounter = definition.AttackAvoid.Counter,

                DoFatigue = definition.AttackAvoid.Fatigue
            };

            if (Body == null)
                Body = BodyParts.Body.Create();

            ShortName = definition.NameKey == "" ? definition.Id.Path : definition.NameKey.Localized();

            Level = definition.Level;

            Strength = definition.Strength;

            Toughness = definition.Toughness;

            Speed = definition.Speed;

            Intelligence = definition.Intelligence;

            Libido = definition.Libido;

            Sensitivity = definition.Sensitivity;

            Corruption = definition.Corruption;

            if (definition.BonusHealth > 0)
                Stats.AddModifier(GameStats.MaxHealth, ModifierOperation.Addition,

                    () => definition.BonusHealth, new TemporarySource(Guid.NewGuid()));

            Health = definition.Health > 0 ? definition.Health : MaxHealth;

            Lust = definition.Lust;

            Gems = definition.Gems;

            Experience = definition.Experience;

            if (!string.IsNullOrEmpty(definition.AiId.Path))
                AiId = definition.AiId;
        }

        public virtual void PerformCombatAction()
        {
        }

        /// <summary>原版 runCheck() 返回特殊文本时通常意味着必定能逃脱（例如石魔蓄力/护盾期间）。</summary>
        public virtual bool GuaranteesEscape(CombatContext context) => false;

        public Action<bool>? OnDefeated;

        public Action<bool>? OnWon;

        public bool TookAction;

    public bool MissFirstRound;

        public virtual void HandleDamaged(CombatContext context, float damage, CombatDamageKind kind)
        {
        }

        public virtual bool React(CombatContext context, CombatReactionContext reaction) => true;

        public virtual bool ReactWrapper(CombatContext context, CombatReactionContext reaction) =>

            context.Player.HasStatusEffect(StatusEffectIds.TimeFrozen) || React(context, reaction);

        public virtual void Struggle(CombatContext context)
        {
        }

        public virtual bool HandleStun(CombatContext context)
        {
            context.WriteAction("Combat.Hook.Stunned".Localized());

            var stunned = StatusEffectByType(StatusEffectIds.Stunned);

            if (stunned != null)
                if (stunned.Values[0] <= 0)
                    RemoveStatusEffect(StatusEffectIds.Stunned);
                else
                    stunned.Values[0] -= 1;

            RemoveStatusEffect(StatusEffectIds.Uber);

            return false;
        }

        public virtual bool HandleFear(CombatContext context)
        {
            if (HasPerk(PerkIds.FearImmune))
            {
                context.WriteAction("Combat.Hook.FearImmune".Localized());

                RemoveStatusEffect(StatusEffectIds.Fear);

                return true;
            }

            var fear = StatusEffectByType(StatusEffectIds.Fear);

            if (fear == null || fear.Values[0] <= 0)
            {
                RemoveStatusEffect(StatusEffectIds.Fear);

                context.WriteAction("Combat.Hook.FearRecovered".Localized());
            }
            else
            {
                fear.Values[0] -= 1;

                context.WriteAction("Combat.Hook.FearShivering".Localized());
            }

            return false;
        }

        public virtual string? RunCheck(CombatContext context) => null;

        public virtual bool BeforeCombatAction(CombatContext context)
        {
            // 原版 ConstrictedDebuff.hx：被束缚的敌人每回合只能挣扎
            if (HasStatusEffect(StatusEffectIds.Constricted))
            {
                context.Append("Combat.VineGrab.Struggle".Localized());

                if (StatusEffectValue(StatusEffectIds.Constricted, 0) <= 0)
                {
                    context.Append("Combat.VineGrab.ReleaseText".Localized());

                    RemoveStatusEffect(StatusEffectIds.Constricted);
                }
                else
                {
                    context.Append("Combat.VineGrab.StruggleFail".Localized());

                    AddStatusValue(StatusEffectIds.Constricted, 0, -1);
                }

                return true;
            }

            return false;
        }

        public virtual int HandleCombatLossText(CombatContext context, bool inDungeon, int gemsLost)
        {
            context.WriteAction(inDungeon

                ? "Combat.Loss.Dungeon".Localized()

                : "Combat.Loss.Overworld".Localized());

            if (context.Player.Gems > 1)
                context.Append((inDungeon

                    ? "Combat.Loss.MissingDungeon"

                    : "Combat.Loss.Missing").Localized(gemsLost));
            else if (context.Player.Gems == 1)
                context.Append((inDungeon

                    ? "Combat.Loss.OnlyGemDungeon"

                    : "Combat.Loss.OnlyGem").Localized());
            else
                context.Append("Combat.Loss.None".Localized());

            return 8;
        }

        public virtual void OutputDefaultFantasy(CombatContext context, float lustDamage)
        {
            if (Balls > 0 && BallSize >= 10 && context.Random.Next(2) == 0)
            {
                context.WriteAction("Combat.Fantasy.Balls".Localized());

                context.WriteAction("Combat.Fantasy.BallsFuller".Localized());

                HoursSinceCum += 50;
            }
            else if (BiggestBreastSize() >= 6 && context.Random.Next(2) == 0)
                context.WriteAction("Combat.Fantasy.Breasts".Localized());
            else if (BiggestLactation() >= 6 && context.Random.Next(2) == 0)
                context.WriteAction("Combat.Fantasy.Lactation".Localized());
            else
                context.WriteAction("Combat.Fantasy.Default".Localized());

            if (lustDamage >= 20)
                context.WriteAction("Combat.Fantasy.Vivid".Localized());
        }

        public virtual AvoidResult PlayerAvoids(CombatContext context) =>

            CombatCalculator.Avoid(context, NormalAttackAvoid.For(this, context.Player));

        public virtual void CombatRoundUpdate(CombatContext context)
        {
            // 原版 Monster.hx:1838 CoonWhip：破甲持续时间结束时恢复护甲
            if (HasStatusEffect(StatusEffectIds.CoonWhip))
                if (StatusEffectValue(StatusEffectIds.CoonWhip, 1) <= 0)
                {
                    ArmorDefense += StatusEffectValue(StatusEffectIds.CoonWhip, 0);

                    if (this is Vanilla.VolcanicGolemEnemy && ArmorDefense >= 300)
                        ArmorDefense = 300;

                    context.Append("Combat.Special.TailWhip.WearsOff".Localized());

                    RemoveStatusEffect(StatusEffectIds.CoonWhip);
                }
                else
                {
                    AddStatusValue(StatusEffectIds.CoonWhip, 1, -1);

                    context.Append("Combat.Special.TailWhip.StillReduced".Localized());
                }
        }

        public virtual bool HandleAwardItemText(CombatContext context, Identifier item) => false;

        public virtual float TotalExperience(float playerLevel = -1)
        {
            var player = CoC.Instance?.Player;

            var multiplier = 1f + (player?.Perkv1(PerkIds.AscensionWisdom) ?? 0f) * 0.1f;

            if (playerLevel < 0)
                playerLevel = player?.PotentialLevel() ?? 0;

            var difference = playerLevel - Level;

            difference = difference <= 2 ? 0 : difference - 2;

            if (difference > 4)
                difference = 4;

            difference = (5 - difference) * 20f / 100f;

            if (playerLevel - Level > 10)
                return 1;

            return AdditionalExperience + (BaseExperience() + BonusExperience()) * difference * multiplier;
        }

        public virtual float BaseExperience()
        {
            float[] scale =

            [
                200, 10, 20, 30, 40, 50, 55, 60, 66, 75,

                83, 85, 92, 100, 107, 115, 118, 121, 128, 135,

                145
            ];

            var level = (int)Math.Round(Level);

            return level < 1 || level >= scale.Length ? 200 : scale[level];
        }

        public virtual float BonusExperience()
        {
            float[] scale = [200, 10, 20, 30, 40, 50, 55, 58, 66, 75, 83, 85, 85, 86, 92, 94, 96, 98, 99, 101, 107];

            var level = (int)Math.Round(Level);

            if (level < 1 || level >= scale.Length)
                return Random.Shared.Next(130);

            return Random.Shared.Next((int)scale[level]);
        }

        public bool PrefersRanged;

        public int MoveCooldown;

        public new virtual bool CanMove() =>

            base.CanMove() && !HasStatusEffect(StatusEffectIds.TimeFrozen);

        public virtual int InitiativeValue()
        {
            var diff = (float)Math.Round((CoC.Instance?.Player?.Level ?? 0) - Level);

            diff += 15 * (1 - Math.Max(HealthRatio(), 0.1f));

            diff += 10 * LustRatio();

            return (int)Math.Round(diff);
        }

        public virtual bool ShouldMove(CombatDistance newPos, bool forceAction = false)
        {
            if (IsFlying || MoveCooldown != 0 || HasPerk(PerkIds.ImmovableObject))
                return false;

            return newPos switch
            {
                CombatDistance.Distant => PrefersRanged && (InitiativeValue() > 15 || forceAction),

                _ => !PrefersRanged
            };
        }

        public virtual bool ShouldWait() => Fatigue >= MaxFatigue() * 0.85f;

        /// <summary>掉落表；默认取数据定义，实例相关分支（如 Minotaur 的 hasAxe）由子类覆盖。</summary>
        public virtual IReadOnlyList<EnemyLoot> Loot => Definition?.Loot ?? [];

        public string? DescriptionOverride { get; set; }

        public virtual string Description => DescriptionOverride ?? (Definition == null || Definition.DescriptionKey == ""

            ? ""

            : Definition.DescriptionKey.Localized());

        public Identifier DefeatScene => Definition?.DefeatScene ?? default;

        public Identifier VictoryScene => Definition?.VictoryScene ?? default;

        public Identifier FleeScene => Definition?.FleeScene ?? default;

        public string Article
        {
            get
            {

                if (ArticleOverride != null)
                    return ArticleOverride;

                var article = Definition?.Article ?? "";

                return article.Length == 0 || article.EndsWith(' ') ? article : article + " ";
            }
        }

        public string? ArticleOverride;

        public string CapitalA => Utils.Utils.CapitalizeFirstLetter(Article);

        public virtual float HPRatio() => MaxHealth <= 0 ? 0f : Health / MaxHealth;

        public string Possessive => ShortName.EndsWith("s") ? "'" : "'s";

        public virtual bool Plural => Definition?.Plural ?? false;

        public string Pronoun1 => Definition?.Pronoun1 is { Length: > 0 } p1 ? p1 : HasCock() ? "he" : HasVagina() ? "she" : "it";

        public string Pronoun2 => Definition?.Pronoun2 is { Length: > 0 } p2 ? p2 : HasCock() ? "him" : HasVagina() ? "her" : "it";

        public string Pronoun3 => Definition?.Pronoun3 is { Length: > 0 } p3 ? p3 : HasCock() ? "his" : HasVagina() ? "her" : "its";

        public virtual void OutputAttack(CombatContext context, float damage)
        {
            var player = context.Player;

            var armorName = player.Armor != Armor.Nothing ? player.Armor.Name : player.ArmorName;

            if (damage <= 0)
            {
                if (player.HasStatusEffect(StatusEffectIds.CounterAB))
                    context.Append("Combat.Monster.Attack.ParryCounter".Localized());
                else if (context.Random.Next(Math.Max(1, (int)(player.ArmorDefense + player.Toughness))) < player.ArmorDefense)
                    context.Append("Combat.Monster.Attack.Absorbed".Localized(WeaponVerb, armorName));
                else
                    context.Append("Combat.Monster.Attack.Deflected".Localized());
            }
            else if (damage < 6)
                context.Append("Combat.Monster.Attack.Glancing".Localized());
            else if (damage < 11)
                context.Append("Combat.Monster.Attack.Wound".Localized());
            else if (damage < 21)
                context.Append("Combat.Monster.Attack.Stagger".Localized(Pronoun3, WeaponVerb));
            else
                context.Append("Combat.Monster.Attack.Mutilate".Localized(Pronoun3, WeaponVerb));

            if (damage > 0 && CoC.Instance.Flags.Get(GameFlagIds.EnemyCritical) > 0)
                context.Append("Combat.Monster.Attack.Critical".Localized());

            context.Append("Combat.Monster.Attack.Damage".Localized((int)damage));

            if (HasPerk(PerkIds.BrutalBlows) && Strength > 75)
            {
                if (player.ArmorDefense > 0)
                    context.Append("Combat.Monster.Attack.BrutalBlows".Localized());

                var effect = player.StatusEffectByType(StatusEffectIds.BrutalBlows)

                    ?? player.CreateStatusEffect(StatusEffectIds.BrutalBlows);

                if (effect.Values[1] <= 0)
                    effect.Values[1] = player.ArmorDefense;

                effect.Values[0] += 1;

                player.ArmorDefense = Math.Max(0, effect.Values[1] * MathF.Pow(0.75f, effect.Values[0]));
            }

            if (HasPerk(PerkIds.SeverTendons) && context.Random.Next(100) <= 25)
            {
                context.Append("Combat.Monster.Attack.SeverTendons".Localized());

                var effect = player.StatusEffectByType(StatusEffectIds.SeverTendons)

                    ?? player.CreateStatusEffect(StatusEffectIds.SeverTendons);

                effect.Values[0] += 5;

                CombatCalculator.AddCombatBuff(player, GameStats.Strength, -effect.Values[0], "SeverTendons");

                CombatCalculator.AddCombatBuff(player, GameStats.Speed, -effect.Values[0], "SeverTendons");
            }
        }

        public virtual Identifier? ForcedAbility(CombatContext context) => null;

        public virtual void ScaleToLevel(int levelDelta = 0)
        {
            if (levelDelta == 0)
                levelDelta = (int)(CoC.Instance.Player.Level - Level);

            if (levelDelta <= 0)
                return;

            var source = new TemporarySource(Guid.NewGuid());

            var scaleStr = Strength * levelDelta * 0.025f;

            var scaleInte = Intelligence * levelDelta * 0.05f;

            var scaleSpe = Speed * levelDelta * 0.05f;

            var scaleTou = Toughness * levelDelta * 0.05f;

            var baseMaxHealth = MaxHealth;

            Stats.AddModifier(GameStats.Strength,

                ModifierOperation.Addition, () => scaleStr, source);

            Stats.AddModifier(GameStats.Intelligence,

                ModifierOperation.Addition, () => scaleInte, source);

            Stats.AddModifier(GameStats.Speed,

                ModifierOperation.Addition, () => scaleSpe, source);

            Stats.AddModifier(GameStats.Toughness,

                ModifierOperation.Addition, () => scaleTou, source);

            WeaponAttack *= 1 + levelDelta * 0.025f;

            Stats.AddModifier(GameStats.MaxHealth,

                ModifierOperation.Addition, () => baseMaxHealth * (1 + levelDelta * 0.0125f), source);

            Level = CoC.Instance.Player.Level;

            Health = MaxHealth;
        }

        public virtual bool Defeated(bool hpVictory) => false;

        public virtual bool Won(bool hpVictory) => false;

        public virtual CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter) =>

            interpreter.Choose();

        public virtual float ModifyIncomingDamage(CombatContext context, float damage, CombatDamageKind kind) =>

            damage;

        public virtual float ModifyOutgoingDamage(CombatContext context, float damage) => damage;

        public virtual bool OnPlayerRunAttempt(CombatContext context) => false;

        public virtual void OnTeased(CombatContext context, float lustDelta) =>

            context.WriteAction("Combat.Resolve.TeaseEnemy".Localized(ShortName, lustDelta));

        public virtual bool CustomCombatMenu(CombatController controller, CombatContext context) => false;

        public virtual bool DescribeAttacked(CombatContext context, float damage) => false;

        public virtual void HandleConstricted(CombatContext context) { }

        public virtual bool HandlePlayerWait(CombatContext context) => false;

        public virtual bool HandleSpellResistance(CombatContext context, Identifier spell) => false;

        public virtual bool ModifyRunOutcome(CombatContext context, bool escaped) => false;

        public virtual DefeatResult ResolveDefeated(CombatContext context, bool hpVictory) => DefeatResult.Ended;

        public virtual void RunFail(CombatContext context) { }

        public virtual void RunSuccess(CombatContext context) { }

        public virtual void TickStatuses(CombatContext context) { }
    }
}