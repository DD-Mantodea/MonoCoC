using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:2101-2115 / :6841-6872
    public static class PlayerAimedShotHandlers
    {
        private static readonly Identifier FirearmMastery = new("coc", "Firearm");

        [Register("coc:can_aimed_shot")]
        public static bool CanAimedShot(CombatContext context, CombatState state)
        {
            var player = context.Player;

            return player.Weapon.IsFirearm() && player.MasteryLevel(FirearmMastery) >= 2;
        }

        [Register("coc:special_aimed_shot")]
        public static void AimedShot(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Special.AimedShot.Intro".Localized());

            var distant = enemy.Distance == CombatDistance.Distant;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = player, Defender = enemy,

                DoDodge = true, DoParry = false, DoBlock = false,

                ToHitChance = player.StandardDodgeFunc(enemy, distant ? 25 : -50),
            });

            if (avoid.AttackFailed)
            {
                context.Append(distant

                    ? "Combat.Special.AimedShot.MissFar".Localized()

                    : "Combat.Special.AimedShot.MissClose".Localized());

                return;
            }

            context.Append("Combat.Special.AimedShot.Hit".Localized());

            var damage = CombatCalculator.PhysicalDamage(context, player, enemy) * 1.5f;

            CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Ranged);

            player.MasteryXP(FirearmMastery, 10 + context.Random.Next(21));
        }
    }
}