using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 JellyFishGirl 的招式（src/classes/scenes/places/swim/JellyFishGirl.hx:23-84）。
    public static class JellyFishGirlCombatHandlers
    {
        // 原版 Monster.eAttack（Monster.hx:940-963）。
        [Register("coc:jelly_fish_girl_attack")]
        private static void Attack(CombatContext context, CombatAbilityDefinition ability)
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

        // 原版 jellyGrapple（JellyFishGirl.hx:40-49）：1/2 或 str×1.25% 缠住玩家。
        // 也供 Enemy.React(Distanced) 直接调用。
        [Register("coc:jelly_fish_girl_jelly_grapple")]
        private static void JellyGrappleAbility(CombatContext context, CombatAbilityDefinition ability) =>
            JellyGrapple(context);

        public static void JellyGrapple(CombatContext context)
        {
            var player = context.Player;

            context.Append("Combat.JellyFishGirl.JellyGrapple.RibbonLikeTendrils".Localized());

            if (context.Random.Next(2) == 0 || context.Random.Next(100) < player.Strength * 1.25f)
            {
                context.Append("Combat.JellyFishGirl.JellyGrapple.NoMatterHow".Localized());

                if (!player.HasStatusEffect(VanillaStatusEffects.Grappled))
                    player.CreateStatusEffect(VanillaStatusEffects.Grappled, 0, 0);
            }
            else
            {
                context.Append("Combat.JellyFishGirl.JellyGrapple.FortunatelyWithA".Localized());
            }
        }

        // 原版 molestPlayer（JellyFishGirl.hx:51-72）：按 Grappled 层数选文本，欲望随层数放大。
        [Register("coc:jelly_fish_girl_molest_player")]
        private static void MolestPlayer(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var grappled = (int)player.StatusEffectValue(VanillaStatusEffects.Grappled, 1);

            if (grappled > 1)
            {
                context.Append("Combat.JellyFishGirl.MolestPlayer.TheJellyGirl".Localized());
            }
            else if (grappled == 1)
            {
                context.Append("Combat.JellyFishGirl.MolestPlayer.WhileLookingAt".Localized());
            }
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
                player.ChangeStatusValue(VanillaStatusEffects.Grappled, 1, 1);
        }

        // 原版 struggle（JellyFishGirl.hx:23-37）：1/3 或 str×1.25% 成功脱身，并反伤这只水母娘。
        public static void Struggle(CombatContext context)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            if (!player.HasStatusEffect(VanillaStatusEffects.Grappled))
                return;

            context.Clear();

            if (context.Random.Next(3) == 0 || context.Random.Next(100) < player.Strength * 1.25f)
            {
                context.Append("Combat.JellyFishGirl.Struggle.WrithingAgainstThe".Localized());

                player.RemoveStatusEffect(VanillaStatusEffects.Grappled);

                enemy.HealthChange(-(player.Strength / 10f + context.Random.Next(10)));
            }
            else
            {
                context.Append("Combat.JellyFishGirl.Struggle.YourStrugglesManage".Localized());
            }
        }
    }
}
