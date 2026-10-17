using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 classes.scenes.dungeons.lethicesKeep.MinotaurKing 的招式（MinotaurKing.hx:125-270）。
    // 选招见 MinotaurKingEnemy.ChooseCombatAction（原版 performCombatAction 是手写的）。
    public static class MinotaurKingCombatHandlers
    {
        // 原版 eBaseDamage() = str + weaponAttack（Monster.hx eBaseDamage，imp 那边也是这么说的）。
        private static float BaseDamage(Enemy enemy) => enemy.Strength + enemy.WeaponAttack;

        // 原版 backhand()（MinotaurKing.hx:125-139）：
        //   闪避原因按 EVASION_* 分四种文案；注意原版的「躲开」集合里没有 EVASION_BLIND，
        //   也就是说失明的闪避原因会落进「被打中」分支——照抄。
        [Register("coc:minotaur_king_backhand")]
        private static void Backhand(CombatContext context, CombatAbilityDefinition ability)
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
            {
                context.Append("Combat.MinotaurKing.Backhand.LuckilyYouDodge".Localized());
            }
            else if (reason == "Evade")
            {
                context.Append("Combat.MinotaurKing.Backhand.LuckilyYouEvade".Localized());
            }
            else if (reason == "Misdirection")
            {
                context.Append("Combat.MinotaurKing.Backhand.LuckilyYouMisdirect".Localized());
            }
            else
            {
                context.Append("Combat.MinotaurKing.Backhand.DamnThatHurts".Localized());

                context.DealDamageToPlayer(damage);
            }
        }

        // 原版 headbutt()（MinotaurKing.hx:141-160）：
        //   半伤害；闪避集合比 backhand 多一个 EVASION_BLIND；命中后标记 _lastRoundStun 并尝试眩晕玩家。
        [Register("coc:minotaur_king_headbutt")]
        private static void Headbutt(CombatContext context, CombatAbilityDefinition ability)
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
            {
                context.Append("Combat.MinotaurKing.Headbutt.LuckilyYouDodge".Localized());
            }
            else if (reason == "Evade")
            {
                context.Append("Combat.MinotaurKing.Headbutt.LuckilyYouEvade".Localized());
            }
            else if (reason == "Misdirection")
            {
                context.Append("Combat.MinotaurKing.Headbutt.LuckilyYouMisdirect".Localized());
            }
            else
            {
                ((MinotaurKingEnemy)enemy).LastRoundStun = true;

                context.Append("Combat.MinotaurKing.Headbutt.HeImpactsWith".Localized());

                context.DealDamageToPlayer(damage);

                // 原版 player.stun(0, 100)
                if (player.Stun(0, 100))
                    context.Append("Combat.MinotaurKing.Headbutt.YouReLeft".Localized());
            }
        }

        // 原版 dickslap()（MinotaurKing.hx:162-177）：纯欲望伤害，只有在玩家还晕着时才出。
        [Register("coc:minotaur_king_dickslap")]
        private static void Dickslap(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var king = (MinotaurKingEnemy)context.Enemy;

            context.Append("Combat.MinotaurKing.Dickslap.BeforeYouCan".Localized());

            if (king.Orgasms > 0)
            {
                context.Append("Combat.MinotaurKing.Dickslap.StrandsOfHis".Localized());

                if (player.HasPerk(VanillaPerks.MinotaurCumAddict))
                    context.Append("Combat.MinotaurKing.Dickslap.GodLike".Localized());

                context.Append("Combat.MinotaurKing.Dickslap.SpunkHangFrom".Localized());

                context.Append(player.HasPerk(VanillaPerks.MinotaurCumAddict)
                    ? "Combat.MinotaurKing.Dickslap.Delicious".Localized()
                    : "Combat.MinotaurKing.Dickslap.WhyDidYou".Localized());
            }

            player.TakeLustDamage(15 + player.Libido / 20f, true);
        }

        // 原版 battleaxe()（MinotaurKing.hx:179-192）：
        //   伤害 = reduceDamage(eBaseDamage(), this, armorIgnore: 20)——用 20% 破甲；
        //   C# 的 CombatCalculator.ReduceDamage 没有破甲参数，所以这里按原版公式手算：
        //   减免比例 = (100 - damagePercent) * (1 - armorIgnore/100)，与 ReduceDamage 等价（ignore = 0 时）。
        [Register("coc:minotaur_king_battleaxe")]
        private static void Battleaxe(CombatContext context, CombatAbilityDefinition ability)
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
            {
                context.Append("Combat.MinotaurKing.Battleaxe.InTimeTo".Localized());
            }
            else
            {
                context.Append("Combat.MinotaurKing.Battleaxe.ByTheTime".Localized());

                context.DealDamageToPlayer(damage);
            }
        }

        // 原版 minoPheromones()（MinotaurKing.hx:206-230）：
        //   1/3 糊到脸（玩家 lust100 > 75 再多 15+lib/10，否则 7.5+lib/20），
        //   然后固定 15+lib/20；牛头人精液成瘾（或成瘾状态 >= 2）再随机追加 5+rand(5)。
        [Register("coc:minotaur_king_mino_pheromones")]
        private static void MinoPheromones(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.MinotaurKing.MinoPheromones.TheMinotaurSmiles".Localized());

            if (context.Random.Next(3) == 0)
            {
                context.Append("Combat.MinotaurKing.MinoPheromones.SlappingIntoYour".Localized());

                if (player.Lust100 > 75)
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
            {
                context.Append("Combat.MinotaurKing.MinoPheromones.RightPastYour".Localized());
            }

            context.Append("Combat.MinotaurKing.MinoPheromones.TheAnimalisticScent".Localized());

            player.TakeLustDamage(15 + player.Libido / 20f, true);

            if (player.HasPerk(VanillaPerks.MinotaurCumAddict) ||
                CoC.Instance.Flags.Get(VanillaFlags.MinotaurCumAddictionState) >= 2)
            {
                context.Append(context.Random.Next(2) == 0
                    ? "Combat.MinotaurKing.MinoPheromones.YouShiverWith".Localized()
                    : "Combat.MinotaurKing.MinoPheromones.YouGroanAnd".Localized());

                player.TakeLustDamage(5 + context.Random.Next(5), true);
            }
        }

        // 原版 cowslutTeaseCocks()（MinotaurKing.hx:245-264）：
        //   Excellia 的挑逗演出，只有玩家有屌时才生效；每次 +5 欲望。
        // 注意：这个函数在 Haxe 全 src 里没有调用点（死代码），C# 也没有对应钩子，先原样移植备用。
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
                    : "Combat.MinotaurKing.CowslutTeaseCocks.Text2".Localized());

                context.Append("Combat.MinotaurKing.CowslutTeaseCocks.AReminderOf".Localized());
            }
            else if (sel == 1)
            {
                context.Append("Combat.MinotaurKing.CowslutTeaseCocks.TurningAboutThe".Localized());
            }
            else
            {
                context.Append("Combat.MinotaurKing.CowslutTeaseCocks.ExcelliaRisesUp".Localized());
            }

            player.TakeLustDamage(5, true);
        }
    }
}
