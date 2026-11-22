using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class FrostGiantCombatHandlers
    {

        [Register("coc:frost_giant_giant_attack_punch")]
        public static void GiantAttackPunch(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var giantStr = enemy.Strength;

            var giantSpe = Math.Max(1, (int)enemy.Speed);

            context.AppendLog("Combat.FrostGiant.Punch.Lead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.AppendLog("Combat.FrostGiant.Punch.Miss".Localized());

                return;
            }

            if (context.Random.Next(Math.Max(1, (int)player.Speed + 40)) < giantSpe)
            {
                context.AppendLog("Combat.FrostGiant.Punch.HitFull".Localized());

                var damage = CombatCalculator.ReduceDamage(context, player, giantStr + 50 + context.Random.Next(100));

                if (damage < 40)
                    damage = 40;

                player.HealthChange(-damage);
            }
            else
            {
                context.AppendLog("Combat.FrostGiant.Punch.HitGlance".Localized());

                player.HealthChange(-CombatCalculator.ReduceDamage(context, player, context.Random.Next(Math.Max(1, (int)giantStr))));
            }
        }

        [Register("coc:frost_giant_giant_grab")]
        public static void GiantGrab(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var giantSpe = Math.Max(1, (int)enemy.Speed);

            context.AppendLog("Combat.FrostGiant.Grab.Lead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {

                var barely = context.Random.Next(Math.Max(1, (int)player.Speed)) < giantSpe;

                context.AppendLog(barely

                    ? "Combat.FrostGiant.Grab.DodgeBarely".Localized()

                    : "Combat.FrostGiant.Grab.DodgeAbly".Localized());

                enemy.HealthChange(-(10 + context.Random.Next(20)));

                context.AppendLog("Combat.FrostGiant.Grab.GiantStunned".Localized());

                enemy.CreateStatusEffect(StatusEffectIds.Stunned, duration: 1);

                return;
            }

            context.AppendLog("Combat.FrostGiant.Grab.Hit".Localized());

            player.CreateStatusEffect(StatusEffectIds.GiantGrabbed, duration: 2);
        }

        public static void GiantGrabStruggle(CombatContext context)
        {
            if (context.Random.Next(100) >= context.Player.Strength || context.Random.Next(10) == 0)
            {
                GiantGrabFail(context);

                return;
            }

            GiantGrabSuccess(context);
        }

        public static void GiantGrabSuccess(CombatContext context)
        {
            var player = context.Player;

            MonoCoC.UI.Components.Views.GameView.Current.ClearOutput();

            var str = player.Strength;

            var key = str >= 100 ? "Combat.FrostGiant.GrabSuccess.Str100"

                : str >= 80 ? "Combat.FrostGiant.GrabSuccess.Str80"

                : str >= 60 ? "Combat.FrostGiant.GrabSuccess.Str60"

                : str >= 40 ? "Combat.FrostGiant.GrabSuccess.Str40"

                : str >= 20 ? "Combat.FrostGiant.GrabSuccess.Str20"

                : "Combat.FrostGiant.GrabSuccess.StrLow";

            context.AppendLog(key.Localized());

            player.RemoveStatusEffect(StatusEffectIds.GiantGrabbed);
        }

        public static void GiantGrabFail(CombatContext context, bool struggle = true)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var giantStr = enemy.Strength;

            var str = player.Strength;

            MonoCoC.UI.Components.Views.GameView.Current.ClearOutput();

            if (struggle)
            {
                var key = str >= 80 ? "Combat.FrostGiant.GrabFail.Str80"

                    : str >= 60 ? "Combat.FrostGiant.GrabFail.Str60"

                    : str >= 40 ? "Combat.FrostGiant.GrabFail.Str40"

                    : str >= 20 ? "Combat.FrostGiant.GrabFail.Str20"

                    : "Combat.FrostGiant.GrabFail.StrLow";

                context.AppendLog(key.Localized());

                if (str >= 60 && str < 80)
                    GiantStrengthLoss(context, 1);
                else if (str >= 40 && str < 60)
                    GiantStrengthLoss(context, 2);

                if (str < 40 && !player.HasPerk(PerkIds.Juggernaut))
                {
                    var damage = str >= 20

                        ? 10 + context.Random.Next(Math.Max(1, (int)(giantStr * 0.5f)))

                        : 20 + context.Random.Next(Math.Max(1, (int)(giantStr * 0.75f)));

                    player.HealthChange(-CombatCalculator.ReduceDamage(context, player, damage));
                }
            }

            if (SceneInstance<CeraphScene>.Get().HasBondage())
            {
                context.AppendLog("Combat.FrostGiant.GrabFail.Bondage".Localized());

                player.Lust = Math.Min(player.MaxLust, player.Lust + 5);
            }

            context.AppendLog("[pg]");

            switch (context.Random.Next(6))
            {
                case 0:
                case 1:
                case 2:
                    context.AppendLog("Combat.FrostGiant.GrabFail.Taunt".Localized());

                    player.RemoveStatusEffect(StatusEffectIds.GiantGrabbed);

                    player.HealthChange(-CombatCalculator.ReduceDamage(context, player, 10 + context.Random.Next(Math.Max(1, (int)(giantStr * 0.5f)))));

                    return;

                case 3:
                case 4:
                    context.AppendLog("Combat.FrostGiant.GrabFail.GroundPound1".Localized());

                    context.AppendLog("Combat.FrostGiant.GrabFail.GroundPound2".Localized());

                    player.RemoveStatusEffect(StatusEffectIds.GiantGrabbed);

                    player.HealthChange(-CombatCalculator.ReduceDamage(context, player, 100 + context.Random.Next(Math.Max(1, (int)(giantStr * 1.5f)))));

                    return;

                default:
                    context.AppendLog("Combat.FrostGiant.GrabFail.Throw".Localized());

                    player.RemoveStatusEffect(StatusEffectIds.GiantGrabbed);

                    player.CreateStatusEffect(StatusEffectIds.Stunned, duration: 1 + context.Random.Next(2));

                    var throwDamage = (float)(20 + context.Random.Next(Math.Max(1, (int)giantStr)));

                    throwDamage = CombatCalculator.ReduceDamage(context, player, throwDamage);

                    if (throwDamage < 20)
                        throwDamage = 20;

                    player.HealthChange(-throwDamage);

                    return;
            }
        }

        public static void GiantStrengthLoss(CombatContext context, int magnitude)
        {
            var player = context.Player;

            var effect = player.StatusEffectByType(StatusEffectIds.GiantStrLoss);

            if (effect == null)
                effect = player.CreateStatusEffect(StatusEffectIds.GiantStrLoss, magnitude);
            else
                effect.Values[0] += magnitude;

            CombatCalculator.AddCombatBuff(player, GameStats.Strength, -effect.Values[0], "GiantStrLoss");
        }

        [Register("coc:frost_giant_giant_boulder_throw")]
        public static void GiantBoulderThrow(CombatContext context, CombatAbilityDefinition ability)
        {
            context.AppendLog("Combat.FrostGiant.Boulder.Throw1".Localized());

            context.AppendLog("Combat.FrostGiant.Boulder.Throw2".Localized());

            if (!context.Player.HasStatusEffect(StatusEffectIds.GiantBoulder))
                context.Player.CreateStatusEffect(StatusEffectIds.GiantBoulder);
        }

        public static void GiantBoulderHit(CombatContext context, int mode = 0)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            MonoCoC.UI.Components.Views.GameView.Current.ClearOutput();

            context.AppendLog(mode switch
            {
                0 => "Combat.FrostGiant.Boulder.Hit0".Localized(),

                1 => "Combat.FrostGiant.Boulder.Hit1".Localized(),

                _ => "Combat.FrostGiant.Boulder.Hit2".Localized()
            });

            player.RemoveStatusEffect(StatusEffectIds.GiantBoulder);

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength * 2 + 100 + context.Random.Next(250));

            if (damage < 200)
                damage = 200;

            player.HealthChange(-damage);

            context.AppendLog("[pg]");
        }

        public static void GiantBoulderMiss(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            MonoCoC.UI.Components.Views.GameView.Current.ClearOutput();

            context.AppendLog("Combat.FrostGiant.Boulder.Miss".Localized());

            player.RemoveStatusEffect(StatusEffectIds.GiantBoulder);

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, 10 + context.Random.Next(Math.Max(1, (int)(enemy.Strength / 2)))));

            context.AppendLog("[pg]");
        }

        public static void GiantBoulderFantasize(CombatContext context)
        {
            var player = context.Player;

            context.AppendLog("Combat.FrostGiant.Boulder.FantasizeBase".Localized());

            if (player.Cocks.Count > 0)
            {
                if (player.BiggestCockArea() >= 24)
                {
                    context.AppendLog("Combat.FrostGiant.Boulder.FantasizeBigCock".Localized());

                    context.AppendLog(player.Cocks.Count > 1

                        ? "Combat.FrostGiant.Boulder.FantasizeMulti".Localized()

                        : "Combat.FrostGiant.Boulder.FantasizeSingle".Localized());

                    context.AppendLog("Combat.FrostGiant.Boulder.FantasizeBigCockNose".Localized());
                }
                else
                    context.AppendLog("Combat.FrostGiant.Boulder.FantasizeSmallCock".Localized());
            }
            else
                context.AppendLog("Combat.FrostGiant.Boulder.FantasizeNoCock".Localized());

            if (player.HasStatusEffect(StatusEffectIds.GiantBoulder))
                GiantBoulderHit(context, 1);
        }
    }
}