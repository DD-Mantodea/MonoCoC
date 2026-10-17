using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System.Linq;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class AmilyCombatHandlers
    {

        public const string VenomSource = "AmilyVenom";

        [Register("coc:amily_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability) =>

            CombatController.Instance!.Resolver.EnemyBasicAttack();

        [Register("coc:amily_double_attack")]
        public static void DoubleAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            for (var i = 0; i < 2; i++)
                CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        [Register("coc:amily_dart_go")]
        public static void DartGo(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.Amily.AmilyDartGo.AmilyDashesAt".Localized());

                return;
            }

            context.Append("Combat.Amily.AmilyDartGo.AmilyDashesAt2".Localized());

            ApplyVenom(context);

            if (player.Speed <= 2 && player.Strength <= 2)
            {
                context.Append("Combat.Amily.AmilyDartGo.YouVeBecome".Localized());

                player.TakeDamage(8999);
            }
        }

        [Register("coc:amily_concentration")]
        public static void Concentration(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Amily.AmilyConcentration.AmilyTakesA".Localized());

            context.Enemy.CreateStatusEffect(StatusEffectIds.Concentration, 0, 0, 0, 0);
        }

        [Register("coc:amily_can_concentrate")]
        public static bool CanConcentrate(CombatContext context, CombatState state) =>

            !context.Enemy.HasStatusEffect(StatusEffectIds.Concentration);

        public static void ApplyVenom(CombatContext context)
        {
            var player = context.Player;

            var strengthLoss = CurrentLoss(player, GameStats.Strength) + 2 + context.Random.Next(5);

            var speedLoss = CurrentLoss(player, GameStats.Speed) + 2 + context.Random.Next(5);

            var source = VenomStatusSource();

            player.Stats.RemoveFrom(source);

            player.Stats.AddModifier(GameStats.Strength, ModifierOperation.Addition, () => -strengthLoss, source);

            player.Stats.AddModifier(GameStats.Speed, ModifierOperation.Addition, () => -speedLoss, source);

            CombatCalculator.AddCombatBuff(player, GameStats.Strength, 0, VenomSource, VenomStatus());
        }

        public static float CurrentLoss(Creature creature, Identifier stat)
        {
            var source = VenomStatusSource();

            return -creature.Stats.From(source).Where(modifier => modifier.Stat.Id == stat).Sum(modifier => modifier.Resolve());
        }

        public static StatusSource VenomStatusSource() =>

            new(new Identifier("coc", "combat_buff_" + VenomSource.ToLowerInvariant().Replace(' ', '_')), VenomSource);

        public static Identifier VenomStatus() => new("coc", "amily_venom");
    }
}