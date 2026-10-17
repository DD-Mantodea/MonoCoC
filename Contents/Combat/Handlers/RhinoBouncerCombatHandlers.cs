using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class RhinoBouncerCombatHandlers
    {

        [Register("coc:rhino_bouncer_charge_start")]
        public static void ChargeStart(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.RhinoBouncer.ChargeStart.TheRhinoMorph".Localized());

            context.Append("Combat.RhinoBouncer.ChargeStart.HeCurlsHis".Localized());

            if (context.Enemy is RhinoBouncerEnemy rhino)
                rhino.Charging = true;
        }

        [Register("coc:rhino_bouncer_charge_grab")]
        public static void ChargeGrab(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.RhinoBouncer.ChargeGrab.TheRhinoMorph".Localized());

            var weightRatio = player.WeightRating();

            if ((float)(Random.Shared.NextDouble() * weightRatio) < enemy.Strength * 1.2f)
                switch (Random.Shared.Next(3))
                {
                    case 0:
                        Headbutt(context);

                        break;

                    case 1:
                        Backbreaker(context);

                        break;

                    default:
                        Piledriver(context);

                        break;
                }
            else
            {
                context.Append("Combat.RhinoBouncer.ChargeGrab.TheMassiveBeast".Localized());

                context.Append("Combat.RhinoBouncer.ChargeGrab.HeSaysPanting".Localized());
            }

            CombatRangeData.CloseDistance(enemy);
        }

        public static void Headbutt(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.RhinoBouncer.Headbutt.TheMassiveBeast".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength * 2);

            player.TakeDamage(damage);

            context.Append("Combat.RhinoBouncer.Headbutt.HeReleasesYou".Localized());

            if (player.Stun(2, 75))
                context.Append("Combat.RhinoBouncer.Headbutt.YouTryTo".Localized());
            else
                context.Append("Combat.RhinoBouncer.Headbutt.ToHisSurprise".Localized());
        }

        public static void Backbreaker(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.RhinoBouncer.Backbreaker.TheMassiveBeast".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength * 1.5f);

            player.TakeDamage(damage);

            context.Append("Combat.RhinoBouncer.Backbreaker.HePushesYou".Localized());

            var status = player.StatusEffectByType(RhinoBouncerEnemy.BackbrokenStatus);

            if (status == null)
            {
                player.CreateStatusEffect(RhinoBouncerEnemy.BackbrokenStatus, 3);

                CombatCalculator.AddCombatBuff(player, GameStats.Speed, -25, "Backbroken",

                    RhinoBouncerEnemy.BackbrokenStatus);
            }
            else

                status.Values[0] = 3;

            context.Append("Combat.RhinoBouncer.Backbreaker.YourFeelNumb".Localized());
        }

        public static void Piledriver(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.RhinoBouncer.Piledriver.TheMassiveBeast".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength * 2.5f);

            context.Append("Combat.RhinoBouncer.Piledriver.YourHeadCrashes".Localized());

            player.TakeDamage(damage);

            if (player.Health <= 0)
                context.Append("Combat.RhinoBouncer.Piledriver.UnfortunatelyYouCannot".Localized());
        }

        [Register("coc:rhino_bouncer_drop_kick")]
        public static void DropKick(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.RhinoBouncer.DropKick.TakesADeep".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = true,

                ToHitChance = player.StandardDodgeFunc(enemy, -25)
            });

            switch (avoid.Outcome)
            {
                case AvoidOutcome.Dodge:
                    context.Append("Combat.RhinoBouncer.DropKick.Dodged".Localized());

                    break;

                case AvoidOutcome.Block:
                    context.Append("Combat.RhinoBouncer.DropKick.Blocked".Localized());

                    break;

                case AvoidOutcome.Hit:
                    context.Append("Combat.RhinoBouncer.DropKick.TheMountainOf".Localized());

                    var damage = CombatCalculator.ReduceDamage(context, player,

                        enemy.Strength + enemy.WeaponAttack) * 2;

                    player.TakeDamage(damage);

                    CombatRangeData.MoveMonsterDistant(enemy);

                    break;
            }

            context.Append("Combat.RhinoBouncer.DropKick.TooHeavyAnd".Localized());

            enemy.Stun(1, 100, 100, false);
        }

        [Register("coc:rhino_bouncer_at_distant")]
        public static bool AtDistant(CombatContext context, CombatState state) =>

            context.Enemy.Distance == CombatDistance.Distant;

        [Register("coc:rhino_bouncer_charging")]
        public static bool Charging(CombatContext context, CombatState state) =>

            context.Enemy is RhinoBouncerEnemy rhino && rhino.Charging;
    }
}