using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class MinotaurKingCombatHandlers
    {

        public static float BaseDamage(Enemy enemy) => enemy.Strength + enemy.WeaponAttack;

        [Register("coc:minotaur_king_backhand")]
        public static void Backhand(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.MinotaurKing.Backhand.FeintingWithHis".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, BaseDamage(enemy));

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            var reason = avoid.DodgeReason;

            if (damage <= 0 || reason is "Speed" or "Flexibility" or "Unhindered")
                context.Append("Combat.MinotaurKing.Backhand.LuckilyYouDodge".Localized());
            else if (reason == "Evade")
                context.Append("Combat.MinotaurKing.Backhand.LuckilyYouEvade".Localized());
            else if (reason == "Misdirection")
                context.Append("Combat.MinotaurKing.Backhand.LuckilyYouMisdirect".Localized());
            else
            {
                context.Append("Combat.MinotaurKing.Backhand.DamnThatHurts".Localized());

                context.DealDamageToPlayer(damage);
            }
        }

        [Register("coc:minotaur_king_headbutt")]
        public static void Headbutt(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.MinotaurKing.Headbutt.TheBruteGrowls".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, BaseDamage(enemy) / 2f);

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            var reason = avoid.DodgeReason;

            if (damage <= 0 || reason is "Speed" or "Flexibility" or "Unhindered" or "Blind")
                context.Append("Combat.MinotaurKing.Headbutt.LuckilyYouDodge".Localized());
            else if (reason == "Evade")
                context.Append("Combat.MinotaurKing.Headbutt.LuckilyYouEvade".Localized());
            else if (reason == "Misdirection")
                context.Append("Combat.MinotaurKing.Headbutt.LuckilyYouMisdirect".Localized());
            else
            {
                ((MinotaurKingEnemy)enemy).LastRoundStun = true;

                context.Append("Combat.MinotaurKing.Headbutt.HeImpactsWith".Localized());

                context.DealDamageToPlayer(damage);

                if (player.Stun(0, 100))
                    context.Append("Combat.MinotaurKing.Headbutt.YouReLeft".Localized());
            }
        }

        [Register("coc:minotaur_king_dickslap")]
        public static void Dickslap(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var king = (MinotaurKingEnemy)context.Enemy;

            context.Append("Combat.MinotaurKing.Dickslap.BeforeYouCan".Localized());

            if (king.Orgasms > 0)
            {
                context.Append("Combat.MinotaurKing.Dickslap.StrandsOfHis".Localized());

                if (player.HasPerk(PerkIds.MinotaurCumAddict))
                    context.Append("Combat.MinotaurKing.Dickslap.GodLike".Localized());

                context.Append("Combat.MinotaurKing.Dickslap.SpunkHangFrom".Localized());

                context.Append(player.HasPerk(PerkIds.MinotaurCumAddict)

                    ? "Combat.MinotaurKing.Dickslap.Delicious".Localized()

                    : "Combat.MinotaurKing.Dickslap.WhyDidYou".Localized());
            }

            player.TakeLustDamage(15 + player.Libido / 20f, true);
        }

        [Register("coc:minotaur_king_battleaxe")]
        public static void Battleaxe(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.MinotaurKing.Battleaxe.TheMinotaurKing".Localized());

            const float armorIgnore = 20f;

            var percent = CombatCalculator.DamagePercent(context, player);

            var reduction = (100f - percent) * (1f - armorIgnore * 0.01f) * 0.01f;

            var damage = BaseDamage(enemy) * (1f - reduction) *

                CombatStatusModifiers.DamageTakenMultiplier(player);

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (damage <= 0 || avoid.AttackFailed)
                context.Append("Combat.MinotaurKing.Battleaxe.InTimeTo".Localized());
            else
            {
                context.Append("Combat.MinotaurKing.Battleaxe.ByTheTime".Localized());

                context.DealDamageToPlayer(damage);
            }
        }

        [Register("coc:minotaur_king_mino_pheromones")]
        public static void MinoPheromones(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.MinotaurKing.MinoPheromones.TheMinotaurSmiles".Localized());

            if (context.Random.Next(3) == 0)
            {
                context.Append("Combat.MinotaurKing.MinoPheromones.SlappingIntoYour".Localized());

                if (player.LustPercent > 75)
                {
                    context.Append("Combat.MinotaurKing.MinoPheromones.SwallowingItInto".Localized());

                    player.TakeLustDamage(15 + player.Libido / 10f, true);
                }
                else
                {
                    context.Append("Combat.MinotaurKing.MinoPheromones.FeelingYourHeart".Localized());

                    player.TakeLustDamage(7.5f + player.Libido / 20f, true);
                }
            }
            else
                context.Append("Combat.MinotaurKing.MinoPheromones.RightPastYour".Localized());

            context.Append("Combat.MinotaurKing.MinoPheromones.TheAnimalisticScent".Localized());

            player.TakeLustDamage(15 + player.Libido / 20f, true);

            if (player.HasPerk(PerkIds.MinotaurCumAddict) ||
                CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) >= 2)
            {
                context.Append(context.Random.Next(2) == 0

                    ? "Combat.MinotaurKing.MinoPheromones.YouShiverWith".Localized()

                    : "Combat.MinotaurKing.MinoPheromones.YouGroanAnd".Localized());

                player.TakeLustDamage(5 + context.Random.Next(5), true);
            }
        }

        public static void CowslutTeaseCocks(CombatContext context)
        {
            var player = context.Player;

            if (!player.HasCock())
                return;

            var sel = context.Random.Next(3);

            if (sel == 0)
            {
                context.Append("Combat.MinotaurKing.CowslutTeaseCocks.ExcelliaCrawlsOver".Localized());

                context.Append(player.HasTailInsteadOfLegs()

                    ? "Combat.MinotaurKing.CowslutTeaseCocks.Text".Localized()

                    : "Combat.MinotaurKing.CowslutTeaseCocks.Text".Localized());

                context.Append("Combat.MinotaurKing.CowslutTeaseCocks.AReminderOf".Localized());
            }
            else if (sel == 1)
                context.Append("Combat.MinotaurKing.CowslutTeaseCocks.TurningAboutThe".Localized());
            else
                context.Append("Combat.MinotaurKing.CowslutTeaseCocks.ExcelliaRisesUp".Localized());

            player.TakeLustDamage(5, true);
        }
    }
}