using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 classes/scenes/monsters/PlaceholderDodgyMonster.hx 的四个招式。
    public static class PlaceholderDodgyMonsterCombatHandlers
    {
        // 原版 eAttack()（PlaceholderDodgyMonster.hx:100-105）：
        // 匕首没在手（tossedDagger != ""）时先挂 Attacks=2 再 super.eAttack()，也就是连打两下。
        // C# 的 Resolver.EnemyBasicAttack 不读 Attacks 状态，所以这里直接调用两次。
        [Register("coc:placeholder_dodgy_monster_attack")]
        private static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = (PlaceholderDodgyMonsterEnemy)context.Enemy;

            var attacks = enemy.TossedDagger != "" ? 2 : 1;

            for (var i = 0; i < attacks; i++)
                CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        // 原版 dodgeExec()（PlaceholderDodgyMonster.hx:112-123）：咬脖子。
        [Register("coc:placeholder_dodgy_monster_dodge_exec")]
        private static void DodgeExec(CombatContext context, CombatAbilityDefinition ability)
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

            // 原版 player.reduceDamage(str * 2 + rand(60), this, 0.7)：第三参是 armorIgnore=0.7，
            // C# 的 CombatCalculator.ReduceDamage 没有 armorIgnore 参数，按完整护甲减免计算。
            var damage = CombatCalculator.ReduceDamage(context, player,
                enemy.Strength * 2 + context.Random.Next(60));

            player.TakeDamage(damage);

            // 原版 if (player.bleed(this, 3, 3)) ...
            if (player.Bleed(enemy, 3, 3))
                context.Append("Combat.PlaceholderDodgyMonster.DodgeExec.BloodGushesOut".Localized());
        }

        // 原版 tossDagger()（PlaceholderDodgyMonster.hx:82-98）：远程飞刀。
        [Register("coc:placeholder_dodgy_monster_toss_dagger")]
        private static void TossDagger(CombatContext context, CombatAbilityDefinition ability)
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

                    // 原版 player.takeDamage(calcDamage() * 0.75)，calcDamage = reduceDamage(str + weaponAttack)
                    var damage = CombatCalculator.ReduceDamage(context, player,
                        enemy.Strength + enemy.WeaponAttack) * 0.75f;

                    player.TakeDamage(damage);

                    enemy.TossedDagger = "hit";
                    break;
                }
            }
        }

        // 原版 retrieveDagger()（PlaceholderDodgyMonster.hx:46-80）：把插在玩家身上的匕首取回来。
        // 原版结尾只有两行 TODO（:77-79），没有把 tossedDagger 清空——照抄这个行为。
        [Register("coc:placeholder_dodgy_monster_retrieve_dagger")]
        private static void RetrieveDagger(CombatContext context, CombatAbilityDefinition ability)
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

                        if (player.HasPerk(VanillaPerks.Resolute))
                            context.Append("Combat.PlaceholderDodgyMonster.RetrieveDagger.YouGritThrough".Localized());
                        else
                            context.Append("Combat.PlaceholderDodgyMonster.RetrieveDagger.YouReOverwhelmed".Localized());
                    }
                    else
                    {
                        context.Append("Combat.PlaceholderDodgyMonster.RetrieveDagger.HeTakesThe".Localized());

                        if (player.HasPerk(VanillaPerks.Resolute))
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
