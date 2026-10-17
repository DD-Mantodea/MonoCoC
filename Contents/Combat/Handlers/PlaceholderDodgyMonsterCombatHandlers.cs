using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class PlaceholderDodgyMonsterCombatHandlers
    {

        [Register("coc:placeholder_dodgy_monster_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = (PlaceholderDodgyMonsterEnemy)context.Enemy;

            var attacks = enemy.TossedDagger != "" ? 2 : 1;

            for (var i = 0; i < attacks; i++)
                CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        [Register("coc:placeholder_dodgy_monster_dodge_exec")]
        public static void DodgeExec(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = (PlaceholderDodgyMonsterEnemy)context.Enemy;

            context.Append("Combat.PlaceholderDodgyMonster.DodgeExec.MovesAwayFrom".Localized());

            if (player.Strength > 75
                && context.Random.Next(Math.Max(1, (int)player.Strength)) > 70

                && player.Toughness > 80)
            {
                context.Append("Combat.PlaceholderDodgyMonster.DodgeExec.YouFightThrough".Localized());

                return;
            }

            context.Append("Combat.PlaceholderDodgyMonster.DodgeExec.OverwhelmedByThe".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player,

                enemy.Strength * 2 + context.Random.Next(60));

            player.TakeDamage(damage);

            if (player.Bleed(enemy, 3, 3))
                context.Append("Combat.PlaceholderDodgyMonster.DodgeExec.BloodGushesOut".Localized());
        }

        [Register("coc:placeholder_dodgy_monster_toss_dagger")]
        public static void TossDagger(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = (PlaceholderDodgyMonsterEnemy)context.Enemy;

            context.Append("Combat.PlaceholderDodgyMonster.TossDagger.HeSaysFrustrated".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = true
            });

            switch (avoid.Outcome)
            {
                case AvoidOutcome.Block:
                    context.Append("Combat.PlaceholderDodgyMonster.TossDagger.Blocked".Localized());

                    enemy.TossedDagger = "blocked";

                    break;

                case AvoidOutcome.Dodge:
                    context.Append("Combat.PlaceholderDodgyMonster.TossDagger.Dodged".Localized());

                    enemy.TossedDagger = "dodged";

                    break;

                case AvoidOutcome.Hit:
                {
                    context.Append("Combat.PlaceholderDodgyMonster.TossDagger.YouDonT".Localized());

                    var damage = CombatCalculator.ReduceDamage(context, player,

                        enemy.Strength + enemy.WeaponAttack) * 0.75f;

                    player.TakeDamage(damage);

                    enemy.TossedDagger = "hit";

                    break;
                }
            }
        }

        // 原版 PlaceholderDodgyMonster.hx:77-79 结尾只有两条空注释，并没有把 tossedDagger 清空——照抄这个行为。
        [Register("coc:placeholder_dodgy_monster_retrieve_dagger")]
        public static void RetrieveDagger(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = (PlaceholderDodgyMonsterEnemy)context.Enemy;

            switch (enemy.TossedDagger)
            {
                case "hit":
                    context.Append("Combat.PlaceholderDodgyMonster.RetrieveDagger.LungesForwardSeemingly".Localized());

                    if (!PlaceholderDodgyMonsterEnemy.ShookDogHand)
                    {
                        context.Append("Combat.PlaceholderDodgyMonster.RetrieveDagger.YourShoulderBurns".Localized());

                        if (player.HasPerk(PerkIds.Resolute))
                            context.Append("Combat.PlaceholderDodgyMonster.RetrieveDagger.YouGritThrough".Localized());
                        else
                            context.Append("Combat.PlaceholderDodgyMonster.RetrieveDagger.YouReOverwhelmed".Localized());
                    }
                    else
                    {
                        context.Append("Combat.PlaceholderDodgyMonster.RetrieveDagger.HeTakesThe".Localized());

                        if (player.HasPerk(PerkIds.Resolute))
                            context.Append("Combat.PlaceholderDodgyMonster.RetrieveDagger.HeDecidesTo".Localized());
                        else
                            context.Append("Combat.PlaceholderDodgyMonster.RetrieveDagger.HeDecidesTo2".Localized());
                    }

                    player.Bleed(enemy);

                    break;

                case "dodged":
                    context.Append("Combat.PlaceholderDodgyMonster.RetrieveDagger.LungesForwardSeemingly2".Localized());

                    break;

                case "blocked":
                    context.Append("Combat.PlaceholderDodgyMonster.RetrieveDagger.LungesForwardSeemingly3".Localized());

                    context.Append("Combat.PlaceholderDodgyMonster.RetrieveDagger.HeAnalyzesHis".Localized());

                    break;
            }
        }
    }
}