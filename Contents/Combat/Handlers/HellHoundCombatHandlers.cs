using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Contents.Enemies;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 classes/scenes/areas/mountain/HellHound.hx 与 InfestedHellhound.hx 的招式
    public static class HellHoundCombatHandlers
    {
        [Register("coc:hell_hound_fire")]
        public static void Fire(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.HasStatusEffect(StatusEffectIds.Blind))
            {
                context.AppendLog("Scenes.Hellhound.HellhoundFire.BlindFireMiss".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.AppendLog("Scenes.Hellhound.HellhoundFire.FireWaveEvaded".Localized());

                return;
            }

            context.AppendLog("Scenes.Hellhound.HellhoundFire.FireWaveHit".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.WeaponAttack + 5);

            player.HealthChange(-damage);

            player.TakeLustDamage(20 + player.Sensitivity / 10f, true);
        }

        [Register("coc:hell_hound_scent")]
        public static void Scent(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (player.HasStatusEffect(StatusEffectIds.NoFlee))
            {
                if (enemy.Speed >= 100)
                {
                    Fire(context, ability);

                    return;
                }

                context.AppendLog("Scenes.Hellhound.HellhoundScent.ScentCircling".Localized());

                enemy.Speed = 100;
            }
            else
            {
                context.AppendLog("Scenes.Hellhound.HellhoundScent.ScentGround".Localized());

                enemy.Speed += 40;

                player.CreateStatusEffect(StatusEffectIds.NoFlee, 0, 0, 0, 0);
            }
        }

        // 原版 InfestedHellhound.hx:13 hellHoundWormCannon
        [Register("coc:infested_hellhound_worm_cannon")]
        public static void WormCannon(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.AppendLog("Scenes.Infestedhellhound.HellHoundWormCannon.WormCannonFire".Localized());

            context.AppendLog("Scenes.Infestedhellhound.HellHoundWormCannon.BlankLine".Localized());

            if (context.Random.Next(2) == 0)
            {
                context.AppendLog("Scenes.Infestedhellhound.HellHoundWormCannon.JismSplatters".Localized());

                player.TakeLustDamage(5 + player.Libido / 20f, true);

                if (player.HasStatusEffect(StatusEffectIds.Infested) && player.Cocks.Count > 0)
                {
                    context.AppendLog("Scenes.Infestedhellhound.HellHoundWormCannon.OwnWormLead".Localized());

                    context.AppendLog("Scenes.Infestedhellhound.HellHoundWormCannon.OwnWormsExpelled".Localized());

                    if (player.Balls > 0)
                        context.AppendLog("Scenes.Infestedhellhound.HellHoundWormCannon.BallsHeavier".Localized());
                    else
                        context.AppendLog("Scenes.Infestedhellhound.HellHoundWormCannon.WormsSettle".Localized());

                    player.TakeLustDamage(10, true);
                }
                else if (player.Cocks.Count > 0)
                {
                    context.AppendLog("Scenes.Infestedhellhound.HellHoundWormCannon.WormsBrushAway".Localized());

                    player.TakeLustDamage(5 + player.Corruption / 20f, true);
                }
                else if (player.HasVagina())
                    context.AppendLog("Scenes.Infestedhellhound.HellHoundWormCannon.WormsDropGround".Localized());
            }
            else
                context.AppendLog("Scenes.Infestedhellhound.HellHoundWormCannon.GushSidestepped".Localized());
        }
    }
}