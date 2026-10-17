using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 GoblinBroodmother 的招式：goblinDrugAttack / goblinTeaseAttack 继承自 Goblin，
    // eAttack 是基底 Monster.eAttack（战报走 Enemy.OutputAttack）。
    // 依据：src/classes/scenes/monsters/Goblin.hx:8-197、src/classes/scenes/quests/urtaQuest/GoblinBroodmother.hx:21-27。
    public static class GoblinBroodmotherCombatHandlers
    {
        // 原版 Goblin.goblinDrugAttack（Goblin.hx:8-111）在母巢地精身上的实际分支：
        // short 不是 assassin/shaman/warrior/elder/Tamani，所以 multiplier = 1 + NG+×0.5；
        // temp2 = Utils.rand(2)，只可能掷出 red / green，蓝色回血分支对她永远不成立。
        [Register("coc:goblin_broodmother_goblin_drug_attack")]
        private static void GoblinDrugAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;
            var multiplier = 1f + player.NewGamePlusMod() * 0.5f;
            var color = context.Random.Next(2) == 0 ? "red" : "green";
            var who = enemy.Article + enemy.ShortName;
            who = char.ToUpperInvariant(who[0]) + who[1..];

            context.Append("Combat.GoblinBroodmother.Drug.Throw".Localized(who, color));

            if ((player.HasPerk(VanillaPerks.Evade) && context.Random.Next(10) <= 3) ||
                context.Random.Next(100) < player.Speed / 5)
            {
                context.Append("Combat.GoblinBroodmother.Drug.Dodge".Localized());

                return;
            }

            if (player.Shield.Is(VanillaItems.DragonShield) && context.Random.Next(2) == 0)
            {
                context.Append("Combat.GoblinBroodmother.Drug.Absorbed".Localized());

                return;
            }

            if (color == "red")
            {
                context.Append("Combat.GoblinBroodmother.Drug.Red".Localized());

                if (!player.HasStatusEffect(VanillaStatusEffects.TemporaryHeat))
                    player.CreateStatusEffect(VanillaStatusEffects.TemporaryHeat, 0, multiplier, 0, 0);
            }
            else
            {
                context.Append("Combat.GoblinBroodmother.Drug.Green".Localized());

                if (!player.HasStatusEffect(VanillaStatusEffects.Poison))
                    player.CreateStatusEffect(VanillaStatusEffects.Poison, 0, multiplier, 0, 0);
            }
        }

        // 原版 Goblin.goblinTeaseAttack（Goblin.hx:113-172）在母巢地精身上：
        // short 不匹配任何一段专属措辞，所以只有末尾那句通用描写 + 欲望伤害。
        // lustDmg = Utils.rand(player.lib / 10) + 8；short 不是 assassin/warrior/shaman/Tamani/elder，无倍率。
        [Register("coc:goblin_broodmother_goblin_tease_attack")]
        private static void GoblinTeaseAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var lust = context.Random.Next(Math.Max(1, (int)(player.Libido / 10))) + 8;

            player.TakeLustDamage(lust);

            context.Append("Combat.GoblinBroodmother.Tease.Display".Localized());
        }

        // 原版 Monster.eAttack（Monster.hx:940-963）：命中后玩家受伤，战报走 outputAttack。
        // 与 Amarok/Akbal 等既有移植一致，用 Avoid + ReduceDamage + Enemy.OutputAttack。
        [Register("coc:goblin_broodmother_attack")]
        private static void Attack(CombatContext context, CombatAbilityDefinition ability)
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
