using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:3786-3864 biteCalc / bite
    public static class PlayerSharkBiteHandlers
    {
        [Register("coc:has_shark_teeth")]
        public static bool HasSharkTeeth(CombatContext context, CombatState state) =>

            context.Player.Body.Get<FacePart>().Id.Path == "shark_teeth";

        [Register("coc:special_shark_bite")]
        public static void SharkBite(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.Id.Path == "worms")
            {
                context.AppendLog("Combat.Special.SharkBite.Worms".Localized());

                return;
            }

            if (enemy.HasPerk(PerkIds.BiteImmune))
            {
                context.AppendLog("Combat.Special.SharkBite.BiteImmune".Localized());

                player.HealthChange(-5);

                return;
            }

            context.AppendLog("Combat.Special.SharkBite.Intro".Localized());

            if (player.HasStatusEffect(StatusEffectIds.Blind))
                context.AppendLog("Combat.Special.SharkBite.Blind".Localized());

            var damage = (player.Strength + 45) * player.PhysMod();

            damage = CombatCalculator.BodyMod(player, damage);

            damage = CombatCalculator.GlobalMod(player, damage);

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = player, Defender = enemy,

                DoDodge = true, DoParry = false, DoBlock = false,
            });

            if (avoid.AttackFailed)
            {
                var speedDelta = enemy.Speed - player.Speed;

                if (speedDelta < 8)
                    context.AppendLog("Combat.Special.SharkBite.DodgeNarrow".Localized());
                else if (speedDelta < 20)
                    context.AppendLog("Combat.Special.SharkBite.DodgeFast".Localized());
                else
                    context.AppendLog("Combat.Special.SharkBite.DodgeDextrous".Localized());

                context.AppendLog("[pg]");

                return;
            }

            var reduced = CombatCalculator.ReduceDamage(context, enemy, damage);

            if (reduced <= 0)
            {
                reduced = 0;

                context.AppendLog("Combat.Special.SharkBite.Blocked".Localized());
            }
            else if (reduced < 10)
                context.AppendLog("Combat.Special.SharkBite.Low".Localized());
            else if (reduced < 20)
                context.AppendLog("Combat.Special.SharkBite.Medium".Localized());
            else if (reduced < 30)
                context.AppendLog("Combat.Special.SharkBite.High".Localized());
            else
                context.AppendLog("Combat.Special.SharkBite.Mutilate".Localized());

            if (reduced > 0 && enemy.Bleed(player))
                context.AppendLog("Combat.Special.SharkBite.Bleed".Localized());

            CombatCalculator.DamageEnemy(context, reduced, CombatDamageKind.Melee);

            context.AppendLog("[pg]");
        }
    }
}