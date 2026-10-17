using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.StatusEffects;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:5313-5340 tailSlapCalc / tailSlapAttack
    public static class PlayerTailSlapHandlers
    {
        [Register("coc:has_salamander_tail")]
        public static bool HasSalamanderTail(CombatContext context, CombatState state) =>

            context.Player.Body.Get<TailPart>().Id.Path == "salamander";

        [Register("coc:effect_on_fire_round")]
        public static void OnFireRound(CombatContext context, Creature host, StatusEffect status)
        {
            var damage = (int)status.Values[0];

            host.HealthChange(-damage);

            context.Append("Combat.Effect.OnFire.Burning".Localized());
        }

        [Register("coc:special_tail_slap")]
        public static void TailSlap(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Special.TailSlap.Intro".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = player, Defender = enemy,

                DoDodge = true, DoParry = false, DoBlock = false,
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.Special.TailSlap.Miss".Localized());

                return;
            }

            context.Append(enemy.Plural

                ? "Combat.Special.TailSlap.HitPlural".Localized()

                : "Combat.Special.TailSlap.Hit".Localized());

            var damage = CombatCalculator.BodyMod(player, player.Strength + 20) * 1.5f;

            damage = InfernoModifiers.CalcInfernoMod(context, damage);

            var pierce = Math.Clamp((int)(40 * enemy.FireResistance - 20), 0, 40);

            damage = CombatCalculator.ReduceDamage(context, enemy, damage) * (1 + pierce / 100f);

            CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Melee);

            var inferno = CombatController.Instance!.State.FireMagicCumulated;

            if (inferno > 0)
            {
                context.Append("Combat.Special.TailSlap.Burn".Localized());

                enemy.CreateStatusEffect(StatusEffectIds.OnFire, InfernoModifiers.CalcInfernoMod(context, 40), 0, 0, 0, 1 + inferno);
            }
        }
    }
}