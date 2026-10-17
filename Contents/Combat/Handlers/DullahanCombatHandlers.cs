using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;
using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class DullahanCombatHandlers
    {
        [Register("coc:dullahan_attack")]
        public static void BasicAttack(CombatContext context, CombatAbilityDefinition ability) =>

            CombatController.Instance!.Resolver.EnemyBasicAttack();

        public static bool IsRude => (CoC.Instance?.Flags.Get(GameFlagIds.DullahanRude) ?? 0) == 2;

        [Register("coc:dullahan_flurry")]
        [Register("coc:dullahan_flurry_attack")]
        public static void Flurry(CombatContext context, CombatAbilityDefinition ability)
        {
            context.WriteAction("Combat.Dullahan.Flurry.Intro".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = context.Player,

                DoDodge = true,

                DoParry = true,

                DoBlock = true,

                DoFatigue = false
            });

            if (avoid.AttackFailed)
            {
                context.Append(ReactionText(avoid));

                return;
            }

            context.Append("Combat.Dullahan.Flurry.Feint".Localized());

            var attacks = 2 + context.Random.Next(2);

            if (IsRude)
                attacks += 4;

            var damage = 0f;

            while (attacks > 0)
            {
                damage += context.Enemy.Strength + context.Random.Next(50);

                damage = CombatCalculator.ReduceDamage(context, context.Player, damage);

                attacks--;
            }

            context.Player.HealthChange(-damage);

            context.Append("Combat.Dullahan.Flurry.Damage".Localized(damage));
        }

        public static string ReactionText(AvoidResult avoid)
        {
            if (avoid.Outcome == AvoidOutcome.Block)
                return "Combat.Dullahan.Flurry.Block".Localized();

            if (avoid.Outcome == AvoidOutcome.Parry)
                return "Combat.Dullahan.Flurry.Parry".Localized();

            return avoid.DodgeReason switch
            {
                "Blind" => "Combat.Dullahan.Flurry.Blind".Localized(),

                "Speed" => "Combat.Dullahan.Flurry.Speed".Localized(),

                "Evade" => "Combat.Dullahan.Flurry.Evade".Localized(),

                "Misdirection" => "Combat.Dullahan.Flurry.Misdirection".Localized(),

                "Flexibility" => "Combat.Dullahan.Flurry.Flexibility".Localized(),

                _ => "Combat.Dullahan.Flurry.Unhandled".Localized()
            };
        }

        [Register("coc:dullahan_horror")]
        public static void Horror(CombatContext context, CombatAbilityDefinition ability)
        {
            context.WriteAction("Combat.Dullahan.Horror.Intro".Localized());

            context.WriteAction(IsRude

                ? "Combat.Dullahan.Horror.LineRude".Localized()

                : "Combat.Dullahan.Horror.LineNormal".Localized());

            if (context.Random.Next((int)Math.Max(1, context.Player.Intelligence)) > 50)
            {
                context.WriteAction("Combat.Dullahan.Horror.Resisted".Localized());

                return;
            }

            context.WriteAction("Combat.Dullahan.Horror.Failed".Localized());

            context.Player.CreateStatusEffect(StatusEffectIds.Whispered);
        }

        [Register("coc:dullahan_pommel_bash")]
        public static void PommelBash(CombatContext context, CombatAbilityDefinition ability)
        {
            context.WriteAction("Combat.Dullahan.Pommel.Intro".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = context.Player,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
            {
                context.WriteAction("Combat.Dullahan.Pommel.Dodged".Localized());

                return;
            }

            context.WriteAction("Combat.Dullahan.Pommel.Hit".Localized());

            var damage = CombatCalculator.ReduceDamage(context, context.Player, context.Enemy.Strength);

            context.Player.HealthChange(-damage);

            if (context.Player.Stun(1, 100))
                context.Append("Combat.Dullahan.Pommel.Stunned".Localized());
        }

        [Register("coc:dullahan_disarm")]
        public static void Disarm(CombatContext context, CombatAbilityDefinition ability)
        {
            context.WriteAction("Combat.Dullahan.Disarm.Intro".Localized());

            var player = context.Player;

            if (context.Random.Next((int)Math.Max(1, player.Strength)) <
                context.Random.Next((int)context.Enemy.Strength + 30))
            {
                context.WriteAction("Combat.Dullahan.Disarm.Success".Localized());

                context.WriteAction("Combat.Dullahan.Disarm.Taunt".Localized());

                player.Disarm(player.Weapon);

                context.Enemy.WeaponAttack = 15;

                context.Enemy.WeaponName = "saber pommel";

                context.Enemy.WeaponVerb = "bash";

                context.Enemy.Speed += 20;

                return;
            }

            context.WriteAction("Combat.Dullahan.Disarm.Failed".Localized());
        }

            [Register("coc:player_disarmed")]
        public static bool PlayerDisarmed(CombatContext context, CombatState _) => context.Player.IsDisarmed();

        [Register("coc:player_not_disarmed")]
        public static bool PlayerNotDisarmed(CombatContext context, CombatState _) => !context.Player.IsDisarmed();

        [Register("coc:dullahan_disarm_ready")]
        public static bool DullahanDisarmReady(CombatContext context, CombatState _) => context.Player.CanDisarm() &&

                    (CoC.Instance?.Flags.Get(GameFlagIds.DullahanRude) ?? 0) == 2;

        [Register("coc:dullahan_horror_ready")]
        public static bool DullahanHorrorReady(CombatContext context, CombatState _) => !context.Player.IsDisarmed() && context.Enemy.HealthRatio() < 0.6f;

}
}