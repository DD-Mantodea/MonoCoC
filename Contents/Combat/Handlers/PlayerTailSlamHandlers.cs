using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:5253-5305 tailslamcalc / tailSlamAttack
    public static class PlayerTailSlamHandlers
    {
        [Register("coc:has_dragon_tail")]
        public static bool HasDragonTail(CombatContext context, CombatState state) =>

            context.Player.Body.Get<TailPart>().Id.Path == "draconic";

        [Register("coc:special_tail_slam")]
        public static void TailSlam(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = player, Defender = enemy,

                DoDodge = true, DoParry = false, DoBlock = false,
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.Special.TailSlam.Miss".Localized());

                return;
            }

            context.Append(enemy.Plural

                ? "Combat.Special.TailSlam.HitPlural".Localized()

                : "Combat.Special.TailSlam.Hit".Localized());

            var damage = 10 + player.Strength / 1.1f;

            if (damage < 0)
                damage = 5;

            damage *= player.PhysMod();

            damage = CombatCalculator.BodyMod(player, damage);

            damage = CombatCalculator.GlobalMod(player, damage);

            CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Melee);

            var chance = (int)(100 / (2 + enemy.StatusEffectValue(StatusEffectIds.TimesBashed, 0)));

            if (chance < 10)
                chance = 10;

            if (enemy.Stun(1, chance))
            {
                context.Append("Combat.Special.TailSlam.Stun".Localized());

                if (!enemy.HasStatusEffect(StatusEffectIds.TimesBashed))
                    enemy.CreateStatusEffect(StatusEffectIds.TimesBashed, 1, 0, 0, 0);
                else
                    enemy.AddStatusValue(StatusEffectIds.TimesBashed, 0, 1);
            }

            if (enemy.Bleed(player))
                context.Append(enemy.Plural

                    ? "Combat.Special.TailSlam.BleedPlural".Localized()

                    : "Combat.Special.TailSlam.Bleed".Localized());
            else if (enemy is LivingStatueEnemy)
                context.Append("Combat.Special.TailSlam.NoBleedStatue".Localized());

            context.Append("[pg]");
        }
    }
}