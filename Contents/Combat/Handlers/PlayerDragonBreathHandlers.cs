using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:1330-1400 / :2808-2924：Dragon Breath 与两个条件
    public static class PlayerDragonBreathHandlers
    {
        [Register("coc:has_dragonfire")]
        public static bool HasDragonfire(CombatContext context, CombatState state) =>

            context.Player.HasPerk(PerkIds.Dragonfire);

        [Register("coc:dragon_breath_cooldown")]
        public static bool DragonBreathCooldown(CombatContext context, CombatState state) =>

            context.Player.HasStatusEffect(StatusEffectIds.DragonBreathCooldown);

        [Register("coc:spell_dragon_breath")]
        public static void DragonBreath(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            player.CreateStatusEffect(StatusEffectIds.DragonBreathCooldown, 0, 0, 0, 0);

            context.Append("Combat.DragonBreath.Roar".Localized());

            InfernoModifiers.InfernoDisplay(context);

            var damage = player.Level * 8 + 25 + context.Random.Next(10);

            damage = InfernoModifiers.CalcInfernoMod(context, damage);

            if (player.HasStatusEffect(StatusEffectIds.DragonBreathBoost))
            {
                damage *= 1.5f;

                player.RemoveStatusEffect(StatusEffectIds.DragonBreathBoost);
            }

            if (enemy.HasStatusEffect(StatusEffectIds.Sandstorm))
                damage *= 0.2f;

            damage *= enemy.FireResistance;

            if (enemy is LivingStatueEnemy)
            {
                context.Append("Combat.DragonBreath.LivingStatue".Localized());

                InfernoModifiers.BoostInferno(context);

                return;
            }

            if (enemy is VolcanicGolemEnemy)
            {
                context.Append("Combat.DragonBreath.GolemNoCare".Localized());

                if (enemy.HasStatusEffect(StatusEffectIds.VolcanicFistProblem))
                    if (!enemy.HasStatusEffect(StatusEffectIds.VolcanicFrenzy))
                    {
                        context.Append("Combat.DragonBreath.GolemStun1".Localized());

                        context.Append("Combat.DragonBreath.GolemStun2".Localized());

                        enemy.CreateStatusEffect(StatusEffectIds.Stunned, 1, 0, 0, 0);

                        enemy.ArmorDefense = 0;
                    }
                    else
                    {
                        context.Append("Combat.DragonBreath.GolemFrenzy".Localized());

                        if (enemy.HasStatusEffect(StatusEffectIds.VolcanicArmorRed))
                        {
                            enemy.AddStatusValue(StatusEffectIds.VolcanicArmorRed, 0, 3);

                            enemy.AddStatusValue(StatusEffectIds.VolcanicArmorRed, 1, 1);
                        }
                        else
                            enemy.CreateStatusEffect(StatusEffectIds.VolcanicArmorRed, 3, 1, 0, 0);

                        enemy.ArmorDefense = MathF.Max(0, enemy.ArmorDefense - 150);
                    }

                InfernoModifiers.BoostInferno(context);

                return;
            }

            context.Append("Combat.DragonBreath.Intro".Localized());

            if (enemy.HasStatusEffect(StatusEffectIds.Sandstorm))
                context.Append("Combat.DragonBreath.Sandstorm".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = player, Defender = enemy,

                DoDodge = true, DoParry = false, DoBlock = false,

                ToHitChance = player.StandardDodgeFunc(enemy, 10),
            });

            if (avoid.AttackFailed)
                context.Append("Combat.DragonBreath.Miss".Localized());
            else
            {
                if (enemy.Stun(1, 100))
                    context.Append("Combat.DragonBreath.Stunned".Localized());
                else
                    context.Append("Combat.DragonBreath.Resolute".Localized(enemy.Plural ? "are" : "is"));

                CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Magical);

                context.Append("[pg-]");
            }

            InfernoModifiers.BoostInferno(context);
        }
    }
}