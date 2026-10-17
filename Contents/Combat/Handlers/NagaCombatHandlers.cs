using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 classes/scenes/areas/desert/Naga.hx 的三个招式
    public static class NagaCombatHandlers
    {
        [Register("coc:naga_poison_bite")]
        public static void PoisonBite(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Scenes.Naga.NagaPoisonBiteAttack.CobraFangBite".Localized());

            if (!player.HasStatusEffect(StatusEffectIds.NagaVenom))
            {
                context.Append("Scenes.Naga.NagaPoisonBiteAttack.VenomBlurFirst".Localized());

                player.CreateStatusEffect(StatusEffectIds.NagaVenom, 0, 0, 0, 0);

                // 原版 NagaVenomDebuff.hx:15：首次附加 Speed -3
                CombatCalculator.AddCombatBuff(player, GameStats.Speed, -3, "NagaVenom", StatusEffectIds.NagaVenom);
            }
            else
            {
                context.Append("Scenes.Naga.NagaPoisonBiteAttack.VenomBlurIntensify".Localized());

                player.ChangeStatusValue(StatusEffectIds.NagaVenom, 0, 1);

                // 原版 NagaVenomDebuff.hx:15：后续每次 Speed -2
                var stacks = (int)player.StatusEffectValue(StatusEffectIds.NagaVenom, 0);

                CombatCalculator.AddCombatBuff(player, GameStats.Speed, -(3 + 2 * stacks), "NagaVenom", StatusEffectIds.NagaVenom);
            }

            // 原版 NagaVenomDebuff.hx:16-18：毒液同时持续造成伤害
            player.HealthChange(-(5 + context.Random.Next(5)));
        }

        [Register("coc:naga_constrict")]
        public static void Constrict(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Scenes.Naga.NagaConstrict.CoilBind".Localized());

            player.CreateStatusEffect(StatusEffectIds.NagaBind, 0, 0, 0, 0);

            if (!player.HasPerk(PerkIds.Juggernaut))
                player.HealthChange(-(2 + context.Random.Next(4)));

            // 原版 CombatAbilities.hx:3983：石魔拳头插地时，缠绕可以尝试扯掉它的手臂
            var enemy = context.Enemy;

            if (enemy is Enemies.Vanilla.VolcanicGolemEnemy)
                if (enemy.HasStatusEffect(StatusEffectIds.VolcanicFistProblem))
                {
                    context.Append("Combat.VolcanicGolem.ConstrictWrap".Localized());

                    if (player.Strength > 80 && !enemy.HasStatusEffect(StatusEffectIds.VolcanicFrenzy))
                    {
                        context.Append("Combat.VolcanicGolem.ConstrictRipArm".Localized());

                        enemy.CreateStatusEffect(StatusEffectIds.VolcanicWeapRed, 3, 0, 0, 0);

                        enemy.WeaponAttack -= 40;

                        if (enemy.WeaponAttack <= 0)
                            enemy.WeaponAttack = 10;
                    }
                    else
                        context.Append("Combat.VolcanicGolem.ConstrictTooWeak".Localized());
                }
                else
                    context.Append("Combat.VolcanicGolem.ConstrictSwat".Localized());
        }

        [Register("coc:naga_tail_whip")]
        public static void TailWhip(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Scenes.Naga.NagaTailWhip.TailWhipWindUp".Localized());

            if (player.HasPerk(PerkIds.Evade) && context.Random.Next(6) == 0)
            {
                context.Append("Scenes.Naga.NagaTailWhip.TailWhipEvaded".Localized());

                return;
            }

            if (player.HasPerk(PerkIds.Misdirection) && context.Random.Next(100) < 10)
            {
                context.Append("Scenes.Naga.NagaTailWhip.TailWhipMisdirected".Localized());

                return;
            }

            if (player.Speed > context.Random.Next(300))
            {
                context.Append("Scenes.Naga.NagaTailWhip.TailWhipJumpDodge".Localized());

                return;
            }

            context.Append("Scenes.Naga.NagaTailWhip.TailSlamHit".Localized());

            var damage = 10f;

            if (player.ArmorDefense < 10)
                damage += 10 - player.ArmorDefense;

            damage += context.Random.Next(3);

            player.HealthChange(-damage);
        }
    }
}