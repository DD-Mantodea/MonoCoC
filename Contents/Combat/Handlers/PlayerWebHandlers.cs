using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:5007-5070 webbingCost / PCWebAttack
    public static class PlayerWebHandlers
    {
        private static int WebbingCost(Player player) =>

            player.HasPerk(PerkIds.NaturalWeapons) ? 25 : 33;

        [Register("coc:has_spider_abdomen")]
        public static bool HasSpiderAbdomen(CombatContext context, CombatState state) =>

            context.Player.Body.Get<TailPart>().Id.Path == "spider_abdomen";

        [Register("coc:not_enough_webbing")]
        public static bool NotEnoughWebbing(CombatContext context, CombatState state) =>

            context.Player.Body.Get<TailPart>().Venom < WebbingCost(context.Player);

        [Register("coc:special_web")]
        public static void Web(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var tail = player.Body.Get<TailPart>();

            if (tail.Venom < WebbingCost(player))
            {
                context.AppendLog("Combat.Special.Web.NoWebbing".Localized());

                return;
            }

            tail.Venom -= WebbingCost(player);

            if (player.HasStatusEffect(StatusEffectIds.Blind))
                context.AppendLog("Combat.Special.Web.Blind".Localized());
            else
                context.AppendLog("Combat.Special.Web.Intro".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = player, Defender = enemy,

                DoDodge = true, DoParry = false, DoBlock = true, DoFatigue = true,
            });

            if (avoid.AttackFailed)
            {
                if (avoid.Outcome == AvoidOutcome.Dodge)
                    context.AppendLog("Combat.Special.Web.Miss".Localized());
                else if (avoid.Outcome == AvoidOutcome.Block)
                    context.AppendLog("Combat.Special.Web.Block".Localized());

                return;
            }

            if (enemy.Speed < 1)
                context.AppendLog(enemy.Plural

                    ? "Combat.Special.Web.OverwebPlural".Localized()

                    : "Combat.Special.Web.Overweb".Localized());
            else
            {
                context.AppendLog("Combat.Special.Web.Hit".Localized());

                enemy.Speed = Math.Max(0, enemy.Speed - 45);
            }

            context.AppendLog("[pg]");
        }
    }
}