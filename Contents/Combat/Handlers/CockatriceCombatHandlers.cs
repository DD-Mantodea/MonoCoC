using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class CockatriceCombatHandlers
    {
        public const int SpellCostCompulsion = 20;

        public const int SpellCostSandAttack = 15;

        public const int SpellCostTailSwipe = 25;

        [Register("coc:cockatrice_compulsion")]
        public static void Compulsion(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.Fatigue > 100 - SpellCostCompulsion)
            {
                Attack(context, ability);

                return;
            }

            context.Append(enemy.CanFly() ? "Combat.Cockatrice.Compulsion.LeadFly".Localized() : "Combat.Cockatrice.Compulsion.Lead".Localized());

            enemy.Fatigue += SpellCostCompulsion;

            if (player.Intelligence / 5f + context.Random.Next(20) >= 24)
            {
                context.Append("Combat.Cockatrice.Compulsion.Resisted".Localized());

                return;
            }

            if (player.HasPerk(PerkIds.BasiliskResistance) || player.CanUseStare() || player.HasKeyItem("Laybans"))
            {
                context.Append("Combat.Cockatrice.Compulsion.Immune".Localized());

                return;
            }

            context.Append("Combat.Cockatrice.Compulsion.Hit".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + 3);

            CombatCalculator.AddCombatBuff(player, GameStats.Speed, -20, "BasiliskSlow", StatusEffectIds.BasiliskSlow);

            player.CreateStatusEffect(StatusEffectIds.BasiliskSlow, 20);

            player.CreateStatusEffect(StatusEffectIds.BasiliskCompulsion);

            CoC.Instance.Flags.Set(GameFlagIds.BasiliskResistanceTracker,

                CoC.Instance.Flags.Get(GameFlagIds.BasiliskResistanceTracker) + 2);
        }

        [Register("coc:cockatrice_sand_attack")]
        public static void SandAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.Fatigue > 100 - SpellCostSandAttack)
            {
                Attack(context, ability);

                return;
            }

            context.Append(enemy.CanFly() ? "Combat.Cockatrice.SandAttack.LeadFly".Localized() : "Combat.Cockatrice.SandAttack.Lead".Localized());

            enemy.Fatigue += SpellCostSandAttack;

            if (player.Speed / 5f + context.Random.Next(20) < 28)
            {
                if (context.Random.Next(2) == 0)
                {
                    context.Append("Combat.Cockatrice.SandAttack.Hit".Localized());

                    player.CreateStatusEffect(StatusEffectIds.Blind, 2 + context.Random.Next(3));
                }
                else
                    context.Append("Combat.Cockatrice.SandAttack.Blocked".Localized());
            }
            else
                context.Append("Combat.Cockatrice.SandAttack.Dodged".Localized());
        }

        [Register("coc:cockatrice_tail_swipe")]
        public static void TailSwipe(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.Fatigue > 100 - SpellCostTailSwipe)
            {
                Attack(context, ability);

                return;
            }

            context.Append("Combat.Cockatrice.TailSwipe.Lead".Localized());

            enemy.Fatigue += SpellCostTailSwipe;

            var damage = (int)(enemy.Strength + 20 - context.Random.NextDouble() * (player.Toughness + player.ArmorDefense));

            if (damage < 0)
                damage = 0;

            player.HealthChange(-damage);

            if (damage == 0)
                context.Append("Combat.Cockatrice.TailSwipe.Harmless".Localized());
        }

        [Register("coc:cockatrice_cocka_tease")]
        public static void CockaTease(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append(context.Random.Next(2) == 0

                ? "Combat.Cockatrice.Tease.Rump".Localized()

                : "Combat.Cockatrice.Tease.Lean".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + 12 + context.Random.Next(Math.Max(1, (int)(player.Libido / 8))));
        }

        [Register("coc:cockatrice_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = true, DoBlock = true
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
    }
}