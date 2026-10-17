using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:5344 bashCalc / :5366 shieldBash
    public static class PlayerShieldBashHandlers
    {
        private static readonly Identifier ShieldMastery = new("coc", "shield");

        [Register("coc:has_shield_mastery")]
        public static bool HasShieldMastery(CombatContext context, CombatState state)
        {
            var player = context.Player;

            return player.Shield != Shield.Nothing && player.MasteryLevel(ShieldMastery) >= 1;
        }

        private static int BashDamage(CombatContext context, Player player)
        {
            var damage = 10f + player.Strength / 1.5f + context.Random.Next((int)MathF.Max(0, player.Strength / 2));

            damage += player.Shield.Block * 2 * player.MasteryLevel(ShieldMastery);

            if (player.HasPerk(PerkIds.ShieldSlam))
                damage *= 1.2f;

            if (damage < 0)
                damage = 5;

            damage *= player.PhysMod();

            damage = CombatCalculator.GlobalMod(player, damage);

            damage = CombatCalculator.ReduceDamage(context, context.Enemy, damage);

            return Math.Max(1, (int)damage);
        }

        [Register("coc:special_shield_bash")]
        public static void ShieldBash(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Special.ShieldBash.Ready".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = player, Defender = enemy,

                DoDodge = true, DoParry = false, DoBlock = false,

                ToHitChance = player.StandardDodgeFunc(enemy),
            });

            if (avoid.AttackFailed)
            {
                var speedDelta = enemy.Speed - player.Speed;

                if (speedDelta >= 8 && speedDelta < 20)
                    context.Append("Combat.Special.ShieldBash.DodgeFast".Localized());
                else if (speedDelta >= 20)
                    context.Append("Combat.Special.ShieldBash.DodgeDextrous".Localized());
                else
                    context.Append("Combat.Special.ShieldBash.DodgeNarrow".Localized());

                player.MasteryXP(ShieldMastery, 5 + context.Random.Next(31));

                return;
            }

            var damage = BashDamage(context, player);

            var chance = (int)(100 / (2 + enemy.StatusEffectValue(StatusEffectIds.TimesBashed, 0)));

            if (chance < 10)
                chance = 10;

            var final = enemy.ModifyIncomingDamage(context, damage, CombatDamageKind.Melee);

            enemy.HealthChange(-final);

            enemy.HandleDamaged(context, final, CombatDamageKind.Melee);

            context.Append("Combat.Special.ShieldBash.Hit".Localized((int)final));

            if (enemy is VolcanicGolemEnemy)
            {
                if (enemy.HasStatusEffect(StatusEffectIds.VolcanicFistProblem))
                {
                    if (!enemy.HasStatusEffect(StatusEffectIds.VolcanicFrenzy))
                    {
                        context.Append("Combat.Special.ShieldBash.GolemStun1".Localized());

                        context.Append("Combat.Special.ShieldBash.GolemStun2".Localized());

                        enemy.CreateStatusEffect(StatusEffectIds.Stunned, 1, 0, 0, 0);

                        enemy.ArmorDefense = 0;
                    }
                    else
                    {
                        context.Append("Combat.Special.ShieldBash.GolemFrenzy".Localized());

                        BreakArmor(enemy);
                    }

                    context.Append("[pg]");

                    return;
                }

                context.Append("Combat.Special.ShieldBash.GolemPlates".Localized());

                context.Append("[pg]");

                return;
            }

            if (enemy.Stun(1, chance))
            {
                context.Append("Combat.Special.ShieldBash.Stunned".Localized());

                var increment = player.HasPerk(PerkIds.ShieldSlam) ? 0.5f : 1f;

                if (!enemy.HasStatusEffect(StatusEffectIds.TimesBashed))
                    enemy.CreateStatusEffect(StatusEffectIds.TimesBashed, increment, 0, 0, 0);
                else
                    enemy.AddStatusValue(StatusEffectIds.TimesBashed, 0, increment);
            }

            context.Append("[pg]");

            player.MasteryXP(ShieldMastery, 5 + context.Random.Next(21));
        }

        private static void BreakArmor(Enemy enemy)
        {
            if (enemy.HasStatusEffect(StatusEffectIds.VolcanicArmorRed))
            {
                enemy.AddStatusValue(StatusEffectIds.VolcanicArmorRed, 0, 3);

                enemy.AddStatusValue(StatusEffectIds.VolcanicArmorRed, 1, 1);
            }
            else
                enemy.CreateStatusEffect(StatusEffectIds.VolcanicArmorRed, 3, 1, 0, 0);

            enemy.ArmorDefense = MathF.Max(0, enemy.ArmorDefense - 150);
        }
    }
}