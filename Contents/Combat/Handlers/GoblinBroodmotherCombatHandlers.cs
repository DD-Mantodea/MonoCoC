using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class GoblinBroodmotherCombatHandlers
    {

        [Register("coc:goblin_broodmother_goblin_drug_attack")]
        public static void GoblinDrugAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var multiplier = 1f + player.NewGamePlusMod() * 0.5f;

            var color = context.Random.Next(2) == 0 ? "red" : "green";

            var who = enemy.Article + enemy.ShortName;

            who = char.ToUpperInvariant(who[0]) + who[1..];

            context.Append("Combat.GoblinBroodmother.Drug.Throw".Localized(who, color));

            if ((player.HasPerk(PerkIds.Evade) && context.Random.Next(10) <= 3) ||
                context.Random.Next(100) < player.Speed / 5)
            {
                context.Append("Combat.GoblinBroodmother.Drug.Dodge".Localized());

                return;
            }

            if (player.Shield.Is(ItemIds.DragonShield) && context.Random.Next(2) == 0)
            {
                context.Append("Combat.GoblinBroodmother.Drug.Absorbed".Localized());

                return;
            }

            if (color == "red")
            {
                context.Append("Combat.GoblinBroodmother.Drug.Red".Localized());

                if (!player.HasStatusEffect(StatusEffectIds.TemporaryHeat))
                    player.CreateStatusEffect(StatusEffectIds.TemporaryHeat, 0, multiplier, 0, 0);
            }
            else
            {
                context.Append("Combat.GoblinBroodmother.Drug.Green".Localized());

                if (!player.HasStatusEffect(StatusEffectIds.Poison))
                    player.CreateStatusEffect(StatusEffectIds.Poison, 0, multiplier, 0, 0);
            }
        }

        [Register("coc:goblin_broodmother_goblin_tease_attack")]
        public static void GoblinTeaseAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var lust = context.Random.Next(Math.Max(1, (int)(player.Libido / 10))) + 8;

            player.TakeLustDamage(lust);

            context.Append("Combat.GoblinBroodmother.Tease.Display".Localized());
        }

        [Register("coc:goblin_broodmother_attack")]
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
    }
}