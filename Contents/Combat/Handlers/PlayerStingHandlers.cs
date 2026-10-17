using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:4930-5004 stingerCalc / playerStinger
    public static class PlayerStingHandlers
    {
        [Register("coc:has_bee_abdomen")]
        public static bool HasBeeAbdomen(CombatContext context, CombatState state) =>

            context.Player.Body.Get<TailPart>().Id.Path == "bee_abdomen";

        [Register("coc:not_enough_venom")]
        public static bool NotEnoughVenom(CombatContext context, CombatState state) =>

            context.Player.Body.Get<TailPart>().Venom < 33;

        [Register("coc:special_sting")]
        public static void Sting(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var tail = player.Body.Get<TailPart>();

            if (tail.Venom < 33)
            {
                context.Append("Combat.Special.Sting.NoVenom".Localized());

                return;
            }

            if (enemy.Id.Path == "worms")
            {
                context.Append("Combat.Special.Sting.Worms".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = player, Defender = enemy,

                DoDodge = true, DoParry = false, DoBlock = true, DoFatigue = true,
            });

            if (avoid.AttackFailed)
            {
                if (avoid.Outcome == AvoidOutcome.Dodge)
                    context.Append(context.Random.Next(2) == 0

                        ? "Combat.Special.Sting.DodgeNarrow".Localized()

                        : "Combat.Special.Sting.DodgeFast".Localized());
                else if (avoid.Outcome == AvoidOutcome.Block)
                    context.Append("Combat.Special.Sting.Block".Localized());

                return;
            }

            if (enemy.ArmorDefense - player.Level >= 10 && context.Random.Next(4) > 0)
            {
                context.Append("Combat.Special.Sting.Armor".Localized());

                return;
            }

            context.Append("Combat.Special.Sting.Hit".Localized());

            context.Append(enemy.Plural

                ? "Combat.Special.Sting.HitPlural".Localized()

                : "Combat.Special.Sting.HitSingular".Localized());

            var damage = 35f + (player.Libido / 10 > 0 ? context.Random.Next((int)(player.Libido / 10)) : 0);

            if (player.Level < 10)
                damage += player.Level * 3;
            else if (player.Level < 20)
                damage += 30 + (player.Level - 10) * 2;
            else if (player.Level < 30)
                damage += 50 + (player.Level - 20);
            else
                damage += 60;

            damage = CombatCalculator.BodyMod(player, damage);

            damage *= enemy.LustVulnerability;

            enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + damage);

            if (!enemy.HasStatusEffect(StatusEffectIds.LustVenom))
                enemy.CreateStatusEffect(StatusEffectIds.LustVenom, 0, 0, 0, 0);

            context.Append("[pg]");

            tail.Venom -= 25;
        }
    }
}