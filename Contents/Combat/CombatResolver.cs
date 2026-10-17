using MonoCoC.Extensions;
using System;
using System.Linq;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Items;

namespace MonoCoC.Contents.Combat
{
    public class CombatResolver(CombatContext context, CombatState state)
    {
        public CombatContext Context { get; } = context;

        public CombatState State { get; } = state;

        public virtual void PlayerAttack()
        {
            // 原版 Combat.hx:654 normalAttack()：普通攻击会激活潜伏加成（value1 置 1）
            if (Context.Player.HasStatusEffect(Datas.Vanilla.StatusEffectIds.Lurk)
                && Context.Player.StatusEffectValue(Datas.Vanilla.StatusEffectIds.Lurk, 0) <= 0)

                Context.Player.ChangeStatusValue(Datas.Vanilla.StatusEffectIds.Lurk, 0, 1);

            // 原版 IceWeapon.hx:120 preAttack()：攻击前先结算融化，武器碎掉就取消本次攻击
            if (Context.Player.Weapon.Behavior is Items.Behaviors.Vanilla.IceWeaponBehavior
                && !Items.Behaviors.Vanilla.IceWeaponBehavior.Melt(Context.Player, true,

                    text => Context.Write(text)))

                return;

            var result = CombatCalculator.Avoid(Context, new AvoidDamageParameters
            {
                Attacker = Context.Player,

                Defender = Context.Enemy,

                DoFatigue = true
            });

            switch (result.Outcome)
            {
                case AvoidOutcome.Dodge:
                    Context.WriteAction("Combat.Resolve.PlayerMiss".Localized());

                    return;

                case AvoidOutcome.Parry:
                    Context.WriteAction("Combat.Resolve.EnemyParries".Localized(Context.Enemy.ShortName));

                    return;

                case AvoidOutcome.Block:
                    Context.WriteAction("Combat.Resolve.EnemyBlocks".Localized(Context.Enemy.ShortName));

                    return;

                case AvoidOutcome.Counter:
                    Context.WriteAction("Combat.Resolve.EnemyCounters".Localized(Context.Enemy.ShortName));

                    return;
            }

            if (!Context.Enemy.ReactWrapper(Context, CombatReactionContext.WhenAttacked))
                return;

            var damage = CombatCalculator.PhysicalDamage(Context, Context.Player, Context.Enemy);

            // 原版 Combat.hx 的 game.combat.damage：SandTrap 等敌人可在 WhenAttacked 里放大本次伤害
            if (Context.EnemyDamageTakenMultiplier != 1f)
            {
                damage *= Context.EnemyDamageTakenMultiplier;

                Context.EnemyDamageTakenMultiplier = 1f;
            }

            CombatCalculator.DamageEnemy(Context, damage, CombatDamageKind.Melee);

            // 原版 WeaponLib 各武器的 get_effects()：命中后附加效果
            WeaponEffects.ApplyAfterHit(Context);

            // 原版 UnarmedClaws.hx:69 get_effects()：自然武器爪击有概率造成流血
            if (Context.Player.Weapon.Id == Weapon.Claws.Id)
            {
                var (chance, intensity) = Items.Behaviors.Vanilla.UnarmedClawsBehavior.Bleed(Context.Player);

                if (chance > 0 && Context.Random.Next(100) < chance)
                    Context.Enemy.Bleed(Context.Player, 3, intensity);
            }

            CombatCalculator.ApplyLeech(Context.Player, damage);

            Context.WriteAction("Combat.Resolve.HitEnemy".Localized(Context.Enemy.ShortName, damage));
        }

        public virtual void PlayerTease()
        {
            // 原版 CombatTeases.hx:41-58 挑逗的三个守卫分支（致盲 / 封印 / 特定敌人），
            // 命中时直接输出「无效」并结束，不进入伤害计算。
            var enemy = Context.Enemy;

            var player = Context.Player;

            if (enemy.HasStatusEffect(Datas.Vanilla.StatusEffectIds.Blind))
            {
                Context.Write("Combat.Tease.Blind".Localized());

                return;
            }

            if (player.HasStatusEffect(Datas.Vanilla.StatusEffectIds.Sealed)
                && (int)player.StatusEffectValue(Datas.Vanilla.StatusEffectIds.Sealed, 1) == 1)
            {
                Context.Write("Combat.Tease.Sealed".Localized());

                return;
            }

            if (enemy.ShortName == "Sirius, a naga hypnotist")
            {
                Context.Write("Combat.Tease.Sirius".Localized());

                return;
            }

            if (enemy.ShortName == "outsider")
            {
                Context.Write("Combat.Tease.Outsider".Localized());

                return;
            }

            var damage = CombatCalculator.TeaseDamage(Context, Context.Player);

            Context.Enemy.Teased(damage);

            Context.Enemy.OnTeased(Context, damage);
        }

        public virtual void EnemyAction(CombatAbilityDefinition ability)
        {
            if (!string.IsNullOrEmpty(ability.Handler.Path) &&
                CombatAbilityHandlerRegistry.TryGet(ability.Handler, out var handler))
            {
                var previousType = Context.MonsterDamageType;

                Context.MonsterDamageType = ability.DamageType;

                handler(Context, ability);

                Context.MonsterDamageType = previousType;

                return;
            }

            var result = Context.Enemy.PlayerAvoids(Context);

            switch (result.Outcome)
            {
                case AvoidOutcome.Dodge:
                    Context.WriteAction("Combat.Resolve.EnemyMiss".Localized(Context.Enemy.ShortName));

                    return;

                case AvoidOutcome.Parry:
                    Context.WriteAction("Combat.Resolve.Parry".Localized(Context.Enemy.ShortName));

                    return;

                case AvoidOutcome.Block:
                    Context.WriteAction("Combat.Resolve.Block".Localized(Context.Enemy.ShortName));

                    return;

                case AvoidOutcome.Counter:
                    Context.WriteAction("Combat.Resolve.Counter".Localized(Context.Enemy.ShortName));

                    return;
            }

            if (ability.Type == CombatAbilityType.Tease)
            {
                var lust = CombatCalculator.TeaseDamage(Context, Context.Enemy, ability.Power);

                Context.Player.Lust = Math.Min(Context.Player.MaxLust, Context.Player.Lust + lust);

                Context.WriteAction("Combat.Resolve.EnemyTease".Localized(Context.Enemy.ShortName, lust));

                return;
            }

            var damage = CombatCalculator.PhysicalDamage(Context, Context.Enemy, Context.Player, ability.Power);

            damage = Context.Enemy.ModifyOutgoingDamage(Context, damage);

            Context.Player.HealthChange(-damage);

            Context.WriteAction("Combat.Resolve.EnemyHit".Localized(Context.Enemy.ShortName, damage));

            if (Context.Player.HasStatusEffect(Datas.Vanilla.StatusEffectIds.TFMoltenPlate)
                && ability.Range is CombatRange.Melee or CombatRange.ChargingMelee or CombatRange.FlyingMelee)
            {
                var lava = Handlers.PlayerTerraFormHandlers.MoltenPlateLava(Context.Player, false);

                var selfBurn = Handlers.PlayerTerraFormHandlers.MoltenPlateLava(Context.Player, true);

                Context.Append("Combat.Terra.MoltenPlate.React".Localized());

                Context.Append((Context.Enemy.Plural

                    ? "Combat.Terra.MoltenPlate.BurnPlural"

                    : "Combat.Terra.MoltenPlate.Burn").Localized());

                Context.Enemy.HealthChange(-lava);

                Context.Append("Combat.Terra.MoltenPlate.Damage".Localized((int)lava));

                Context.Append("Combat.Terra.MoltenPlate.SelfBurn".Localized());

                Context.Player.HealthChange(-selfBurn);
            }
        }

        public virtual void EnemyBasicAttack()
        {
            EnemyAction(new CombatAbilityDefinition
            {
                NameKey = "Combat.Ability.BasicAttack.Name",

                Type = CombatAbilityType.Physical,

                Power = 5
            });
        }

        public virtual void EnemyLustAttack() => EnemyAction(new CombatAbilityDefinition
        {
            NameKey = "Combat.Ability.BasicAttack.Name",

            Type = CombatAbilityType.Tease,

            Power = 5
        });

        public virtual bool CheckEnd()
        {

            if (Context.AllEnemiesDefeated || Context.Enemy.Health <= 0 && Context.Enemies.Count <= 1)
            {
                State.Outcome = CombatOutcome.HpVictory;

                return true;
            }

            var allLustFull = Context.Enemies.Count > 0 && Context.Enemies.All(e => e.Lust >= e.MaxLust);

            if (Context.Enemies.Count > 1 ? allLustFull : Context.Enemy.Lust >= Context.Enemy.MaxLust)
            {
                State.Outcome = CombatOutcome.LustVictory;

                return true;
            }

            if (Context.Player.Health <= 0)
            {
                State.Outcome = CombatOutcome.HpDefeat;

                return true;
            }

            if (Context.Player.Lust >= Context.Player.MaxLust)
            {
                State.Outcome = CombatOutcome.LustDefeat;

                return true;
            }

            return false;
        }

        protected virtual bool Hit(Creatures.Creature attacker, Creatures.Creature defender) =>

            CombatCalculator.Hit(Context, attacker, defender);
    }
}