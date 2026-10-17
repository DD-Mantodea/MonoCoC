using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 classes/scenes/monsters/GoblinShaman.hx 的法术招式；
    // goblinDrugAttack / goblinTeaseAttack 继承自 Goblin.hx（short=="goblin shaman" 分支）。
    public static class GoblinShamanCombatHandlers
    {
        // 原版 spellMultiplier()（GoblinShaman.hx:58-62）
        private static float Multiplier(CombatContext context) =>
            1f + 0.5f * context.Player.NewGamePlusMod();

        // 原版 whitefire()（GoblinShaman.hx:15-20）；AI 表里同一招重复出现两次，共用一个实现。
        [Register("coc:goblin_shaman_whitefire")]
        [Register("coc:goblin_shaman_whitefire2")]
        private static void Whitefire(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            context.Append("Combat.GoblinShaman.Whitefire.TheGoblinNarrows".Localized());

            var damage = enemy.Intelligence + Random.Shared.Next(50) * Multiplier(context);

            context.MonsterDamageType = Identifier.Parse("coc:fire");
            context.DealDamageToPlayer(damage);
        }

        // 原版 blind()（GoblinShaman.hx:22-30）
        [Register("coc:goblin_shaman_blind")]
        private static void Blind(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.GoblinShaman.Blind.TheGoblinGlares".Localized());

            if (Random.Shared.Next(Math.Max(1, (int)player.Intelligence / 5)) <= 4)
            {
                player.CreateStatusEffect(VanillaStatusEffects.Blind, 1 + Random.Shared.Next(3), 0, 0, 0);
                context.Append("Combat.GoblinShaman.Blind.YouAreBlinded".Localized());
            }
            else
            {
                context.Append("Combat.GoblinShaman.Blind.YouManageTo".Localized());
            }
        }

        // 原版 arouse()（GoblinShaman.hx:32-36）
        [Register("coc:goblin_shaman_arouse")]
        private static void Arouse(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.GoblinShaman.Arouse.SheMakesA".Localized());

            var lust = (int)((enemy.Intelligence / 10f) + (player.Libido / 10f)
                + Random.Shared.Next(10) * Multiplier(context));

            player.TakeLustDamage(lust);
        }

        // 原版 chargeweapon()（GoblinShaman.hx:38-41）
        [Register("coc:goblin_shaman_chargeweapon")]
        private static void ChargeWeapon(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.GoblinShaman.Chargeweapon.TheGoblinUtters".Localized());

            context.Enemy.CreateStatusEffect(
                VanillaStatusEffects.ChargeWeapon, 25 * Multiplier(context), 0, 0, 0);
        }

        // 原版 heal()（GoblinShaman.hx:43-48）
        [Register("coc:goblin_shaman_heal")]
        private static void Heal(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            context.Append("Combat.GoblinShaman.Heal.SheFocusesOn".Localized());

            // 原版 GoblinShaman.hx:45-46：amt = Std.int(Std.int(10 + inte/2 + rand(inte/3)) * spellMultiplier())
            // 注意内层先截断再乘，最后再截断一次（与 Haxe 两个 Std.int 一致）。
            var inner = (int)(10 + enemy.Intelligence / 2f
                + Random.Shared.Next(Math.Max(1, (int)(enemy.Intelligence / 3))));

            var amount = (int)(inner * Multiplier(context));

            // 原版 game.combat.getHealText(amt)（Combat.hx:1383-1388）
            context.Append("Combat.GoblinShaman.Heal.SheFlushesWith".Localized()
                CombatCalculator.GetHealText(amount));

            enemy.HealthChange(amount);
        }

        // 原版 might()（GoblinShaman.hx:50-56）
        [Register("coc:goblin_shaman_might")]
        private static void Might(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            context.Append("Combat.GoblinShaman.Might.SheFlushesDrawing".Localized());
            context.Append("Combat.GoblinShaman.Might.TheRushOf".Localized());

            var boost = 20 * Multiplier(context);

            enemy.CreateStatusEffect(VanillaStatusEffects.Might, boost, boost, 0, 0);
            enemy.Strength += boost;
            enemy.Toughness += boost;
        }

        // 原版 Goblin.goblinDrugAttack() 的 goblin shaman 分支（Goblin.hx:8-111）：
        // temp2 = rand(2)（只有红/绿），multiplier = 1 + 0.4 + NG+*0.5。
        [Register("coc:goblin_shaman_goblin_drug_attack")]
        private static void GoblinDrugAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;
            var multiplier = 1.4f + player.NewGamePlusMod() * 0.5f;

            var color = Random.Shared.Next(2) == 0 ? "red" : "green";

            context.Append("Combat.GoblinShaman.DrugThrow".Localized()
                MonoCoC.Utils.Utils.CapitalizeFirstLetter(enemy.Article), enemy.ShortName, color);

            // 原版 dodge 判定：(Evade && rand(10)<=3) || rand(100) < player.spe/5
            if ((player.HasPerk(VanillaPerks.Evade) && Random.Shared.Next(10) <= 3)
                || Random.Shared.Next(100) < player.Speed / 5)
            {
                context.Append("Combat.GoblinShaman.DrugDodged".Localized());

                return;
            }

            if (player.Shield != null && player.Shield.Is(VanillaItems.DragonShield)
                && Random.Shared.Next(2) == 0)
            {
                context.Append("Combat.GoblinShaman.DrugShield".Localized());

                return;
            }

            if (color == "red")
            {
                context.Append("Combat.GoblinShaman.DrugRed".Localized());

                if (!player.HasStatusEffect(VanillaStatusEffects.TemporaryHeat))
                    player.CreateStatusEffect(VanillaStatusEffects.TemporaryHeat, 0, multiplier, 0, 0);
            }
            else
            {
                context.Append("Combat.GoblinShaman.DrugGreen".Localized());

                if (!player.HasStatusEffect(VanillaStatusEffects.Poison))
                    player.CreateStatusEffect(VanillaStatusEffects.Poison, 0, multiplier, 0, 0);
            }
        }

        // 原版 Goblin.goblinTeaseAttack() 的 goblin shaman 分支（Goblin.hx:113-172）：
        // 三选一台词 + lustDmg*1.6。
        [Register("coc:goblin_shaman_goblin_tease_attack")]
        private static void GoblinTeaseAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            var capitalA = MonoCoC.Utils.Utils.CapitalizeFirstLetter(enemy.Article);

            var line = Random.Shared.Next(3) switch
            {
                0 => "Combat.GoblinShaman.TeaseLeather".Localized(capitalA, enemy.ShortName),
                1 => "Combat.GoblinShaman.TeaseFlexibility".Localized(capitalA, enemy.ShortName),
                _ => "Combat.GoblinShaman.TeaseAss".Localized(capitalA, enemy.ShortName)
            };

            context.Append(line);

            var lust = (int)((Random.Shared.Next(Math.Max(1, (int)(player.Libido / 10))) + 8) * 1.6f);

            context.Append("Combat.GoblinShaman.TeaseAfter".Localized());
            player.TakeLustDamage(lust);
            context.Append("Combat.GoblinShaman.TeaseParagraph".Localized());
        }

        // 原版 eAttack：普通攻击（Monster.eAttack 的通用结算）。
        [Register("coc:goblin_shaman_attack")]
        private static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        // ── AI 条件：原版 performCombatAction 里 actionChoices.add 的第三个参数 ──

        [Register("coc:goblin_shaman_lust_below_50")]
        private static bool LustBelow50(CombatContext context, CombatState state) =>
            context.Enemy.Lust < 50;

        [Register("coc:goblin_shaman_lust_above_50")]
        private static bool LustAbove50(CombatContext context, CombatState state) =>
            context.Enemy.Lust > 50;

        [Register("coc:goblin_shaman_lust_above_60")]
        private static bool LustAbove60(CombatContext context, CombatState state) =>
            context.Enemy.Lust > 60;

        // 原版 blind 条件：lust < 50 && !player.hasStatusEffect(StatusEffects.Blind)
        [Register("coc:goblin_shaman_can_blind")]
        private static bool CanBlind(CombatContext context, CombatState state) =>
            context.Enemy.Lust < 50 && !context.Player.HasStatusEffect(VanillaStatusEffects.Blind);
    }
}
