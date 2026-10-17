using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class JellyFishGirlCombatHandlers
    {

        [Register("coc:jelly_fish_girl_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = true,

                DoBlock = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                enemy.OutputAttack(context, 0);

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            player.HealthChange(-damage);

            enemy.OutputAttack(context, damage);
        }

        [Register("coc:jelly_fish_girl_jelly_grapple")]
        public static void JellyGrappleAbility(CombatContext context, CombatAbilityDefinition ability) =>

            JellyGrapple(context);

        public static void JellyGrapple(CombatContext context)
        {
            var player = context.Player;

            context.Append("Combat.JellyFishGirl.JellyGrapple.RibbonLikeTendrils".Localized());

            if (context.Random.Next(2) == 0 || context.Random.Next(100) < player.Strength * 1.25f)
            {
                context.Append("Combat.JellyFishGirl.JellyGrapple.NoMatterHow".Localized());

                if (!player.HasStatusEffect(StatusEffectIds.Grappled))
                    player.CreateStatusEffect(StatusEffectIds.Grappled, 0, 0);
            }
            else
                context.Append("Combat.JellyFishGirl.JellyGrapple.FortunatelyWithA".Localized());
        }

        [Register("coc:jelly_fish_girl_molest_player")]
        public static void MolestPlayer(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var grappled = (int)player.StatusEffectValue(StatusEffectIds.Grappled, 1);

            if (grappled > 1)
                context.Append("Combat.JellyFishGirl.MolestPlayer.TheJellyGirl".Localized());
            else if (grappled == 1)
                context.Append("Combat.JellyFishGirl.MolestPlayer.WhileLookingAt".Localized());
            else
            {
                if (context.PlayerWaited)
                    context.Append("Combat.JellyFishGirl.MolestPlayer.YouConserveYour".Localized());
                else
                    context.Append("Combat.JellyFishGirl.MolestPlayer.TheMoreYou".Localized());

                context.Append("Combat.JellyFishGirl.MolestPlayer.TheJellyGirl2".Localized());
            }

            var lustDamage = 3 + context.Random.Next(4) +

                (player.Sensitivity + player.Corruption + player.Libido) / 100f;

            var lustMulti = context.Random.Next(6) + 2.5f * grappled;

            player.TakeLustDamage(lustDamage * lustMulti);

            if (grappled < 2)
                player.ChangeStatusValue(StatusEffectIds.Grappled, 1, 1);
        }

        public static void Struggle(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (!player.HasStatusEffect(StatusEffectIds.Grappled))
                return;

            context.Clear();

            if (context.Random.Next(3) == 0 || context.Random.Next(100) < player.Strength * 1.25f)
            {
                context.Append("Combat.JellyFishGirl.Struggle.WrithingAgainstThe".Localized());

                player.RemoveStatusEffect(StatusEffectIds.Grappled);

                enemy.HealthChange(-(player.Strength / 10f + context.Random.Next(10)));
            }
            else
                context.Append("Combat.JellyFishGirl.Struggle.YourStrugglesManage".Localized());
        }
    }
}