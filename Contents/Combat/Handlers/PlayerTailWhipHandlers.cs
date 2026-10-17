using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:5203-5255 tailWhipAttack
    public static class PlayerTailWhipHandlers
    {
        private static readonly string[] WhipTails = ["shark", "lizard", "kangaroo", "raccoon", "ferret"];

        [Register("coc:has_whip_tail")]
        public static bool HasWhipTail(CombatContext context, CombatState state) =>

            Array.IndexOf(WhipTails, context.Player.Body.Get<TailPart>().Id.Path) >= 0;

        [Register("coc:special_tail_whip")]
        public static void TailWhip(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy is VolcanicGolemEnemy)
            {
                var blindMiss = player.HasStatusEffect(StatusEffectIds.Blind) && context.Random.Next(2) == 0;

                var speedMiss = enemy.Speed - player.Speed > 0

                    && (int)(context.Random.NextDouble() * ((enemy.Speed - player.Speed) / 4 + 80)) > 80;

                if (blindMiss || speedMiss)
                {
                    context.Append("Combat.Special.TailWhip.Miss".Localized());

                    context.Append("[pg]");

                    return;
                }

                context.Append("Combat.Special.TailWhip.GolemHit".Localized());

                context.Append("[pg]");

                enemy.CreateStatusEffect(StatusEffectIds.CoonWhip, 150, 2, 0, 0);

                enemy.ArmorDefense -= 150;

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = player, Defender = enemy,

                DoDodge = true, DoParry = false, DoBlock = false,
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.Special.TailWhip.Miss".Localized());

                context.Append("[pg]");

                return;
            }

            context.Append(enemy.Plural

                ? "Combat.Special.TailWhip.HitPlural".Localized()

                : "Combat.Special.TailWhip.Hit".Localized());

            if (!enemy.HasStatusEffect(StatusEffectIds.CoonWhip))
                enemy.CreateStatusEffect(StatusEffectIds.CoonWhip, 0, 0, 0, 0);

            var temp = (int)Math.Round(enemy.ArmorDefense * 0.75f);

            while (temp > 0 && enemy.ArmorDefense >= 1)
            {
                enemy.ArmorDefense -= 1;

                enemy.AddStatusValue(StatusEffectIds.CoonWhip, 0, 1);

                temp -= 1;
            }

            var duration = 2;

            if (player.Body.Get<TailPart>().Id.Path == "raccoon")
                duration += 2;

            if (player.HasPerk(PerkIds.NaturalWeapons))
                duration += 1;

            enemy.AddStatusValue(StatusEffectIds.CoonWhip, 1, duration);

            context.Append("[pg]");
        }
    }
}