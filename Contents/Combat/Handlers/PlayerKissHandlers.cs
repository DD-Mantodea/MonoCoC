using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:5078-5198 kissAttack
    public static class PlayerKissHandlers
    {
        [Register("coc:has_lust_stick")]
        public static bool HasLustStick(CombatContext context, CombatState state) =>

            context.Player.HasStatusEffect(StatusEffectIds.LustStickApplied);

        [Register("coc:special_kiss")]
        public static void Kiss(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (player.HasStatusEffect(StatusEffectIds.Blind))
            {
                context.Append("Combat.Special.Kiss.Blind".Localized());

                return;
            }

            context.Append(("Combat.Special.Kiss.Intro" + (context.Random.Next(6) + 1)).Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = player, Defender = enemy,

                DoDodge = true, DoParry = false, DoBlock = false,
            });

            if (avoid.AttackFailed)
            {
                var dodge = context.Random.Next(3);

                if (dodge == 0)
                    context.Append("Combat.Special.Kiss.Dodge1".Localized());
                else if (dodge == 1)
                    context.Append((enemy.Plural

                        ? "Combat.Special.Kiss.Dodge2Plural"

                        : "Combat.Special.Kiss.Dodge2").Localized());
                else
                    context.Append((enemy.Plural

                        ? "Combat.Special.Kiss.Dodge3Plural"

                        : "Combat.Special.Kiss.Dodge3").Localized());

                return;
            }

            if (enemy.LustVulnerability <= 0 || !enemy.HasCock())
            {
                context.Append((enemy.Plural

                    ? "Combat.Special.Kiss.NoEffectPlural"

                    : "Combat.Special.Kiss.NoEffect").Localized());

                return;
            }

            var roll = context.Random.Next(4);

            var damage = roll switch
            {
                1 => 15f,

                2 => 20f,

                3 => 30f,

                _ => 12f,
            };

            context.Append((roll switch
            {
                1 => enemy.Plural ? "Combat.Special.Kiss.Success1Plural" : "Combat.Special.Kiss.Success1",

                2 => enemy.Plural ? "Combat.Special.Kiss.Success2Plural" : "Combat.Special.Kiss.Success2",

                3 => enemy.Plural ? "Combat.Special.Kiss.Success3Plural" : "Combat.Special.Kiss.Success3",

                _ => "Combat.Special.Kiss.Success4",
            }).Localized());

            if (!enemy.HasStatusEffect(StatusEffectIds.Luststick))
                enemy.CreateStatusEffect(StatusEffectIds.Luststick, 0, 0, 0, 0);
            else
                enemy.AddStatusValue(StatusEffectIds.Luststick, 1, damage / 10f);

            enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + enemy.LustVulnerability * damage);

            context.Append("[pg]");
        }
    }
}