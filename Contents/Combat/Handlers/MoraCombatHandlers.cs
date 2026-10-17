using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class MoraCombatHandlers
    {

        [Register("coc:mora_attack")]
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
                context.Append("Combat.Mora.Attack.Dodged".Localized());

                return;
            }

            context.Append("Combat.Mora.Attack.Hit".Localized());

            if (context.Random.Next(100) < enemy.GetBaseCritChance())
                context.Append("Combat.Mora.Attack.Critical".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            player.HealthChange(-damage);

            if (damage > 0 && context.Random.Next(2) == 0)
                player.CreateStatusEffect(StatusEffectIds.IzmaBleed, 1 + context.Random.Next(4), 1 + context.Random.Next(2), 0, 0);

            if (damage > 0)
                AfflictFrostbite(context);
        }

        [Register("coc:mora_mist_attack")]
        public static void MistAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var stacks = AfflictFrostbite(context);

            context.Append("Combat.Mora.Mist.Lead".Localized());

            player.HealthChange(-(20 + 10 * stacks));
        }

        [Register("coc:mora_surge_attack")]
        public static void SurgeAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var stacks = AfflictFrostbite(context);

            context.Append("Combat.Mora.Surge.Lead".Localized());

            player.HealthChange(-(20 + 10 * stacks));
        }

        [Register("coc:mora_shroud_attack")]
        public static void ShroudAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Mora.Shroud.Lead".Localized());

            var mora = context.EnemyAs<MoraEnemy>();

            mora?.Shrouded += Math.Max(GameSettings.Difficulty + 1, 0) + 1;
        }

        [Register("coc:mora_despair_attack")]
        public static void DespairAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var weakenedArmor = false;

            if (player.Lust < player.MaxLust)
                foreach (var stat in new[] { "str", "spe", "tou", "inte" })
                    DebuffStat(context, stat);

            var armorId = player.Armor.Id;

            if (armorId == ArmorIds.DivineArmor || armorId == ArmorIds.GooArmor
                || armorId == ArmorIds.TentacleBarkBehavior || armorId == ArmorIds.VineArmor)
            {
                CombatCalculator.AddArmorBonus(player, -5);

                weakenedArmor = true;
            }

            context.Append("Combat.Mora.Despair.Lead".Localized());

            if (weakenedArmor)
                context.Append(player.Armor.Id == ArmorIds.GooArmor

                    ? "Combat.Mora.Despair.WeakenedValeria".Localized()

                    : "Combat.Mora.Despair.WeakenedArmor".Localized());

            context.Append("Combat.Mora.Despair.Tail".Localized());
        }

        public static void DebuffStat(CombatContext context, string stat)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var playerValue = StatValue(player, stat);

            var enemyValue = StatValue(enemy, stat);

            var lustRatio = (player.MaxLust - player.Lust) / player.MaxLust;

            var amount = (float)Math.Floor((playerValue - enemyValue) * lustRatio);

            if (amount > 0)
                CombatCalculator.AddCombatBuff(player, CombatCalculator.StatByName(stat), -amount, "MoraDespair");
        }

        public static float StatValue(Creature creature, string stat) => stat switch
        {
            "str" => creature.Strength,

            "spe" => creature.Speed,

            "tou" => creature.Toughness,

            _ => creature.Intelligence
        };

        public static int AfflictFrostbite(CombatContext context)
        {
            var player = context.Player;

            var effect = player.StatusEffectByType(StatusEffectIds.Frostbite);

            if (effect == null)
                effect = player.CreateStatusEffect(StatusEffectIds.Frostbite);

            if (context.Random.Next(2) == 0)
                effect.Values[0] += 1;

            if (effect.Values[0] < 1)
                effect.Values[0] = 1;

            return (int)effect.Values[0];
        }
    }
}