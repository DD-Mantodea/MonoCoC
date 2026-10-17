using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class GoblinShamanCombatHandlers
    {

        public static float Multiplier(CombatContext context) =>

            1f + 0.5f * context.Player.NewGamePlusMod();

        [Register("coc:goblin_shaman_whitefire")]
        [Register("coc:goblin_shaman_whitefire_two")]
        public static void Whitefire(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            context.Append("Combat.GoblinShaman.Whitefire.TheGoblinNarrows".Localized());

            var damage = enemy.Intelligence + Random.Shared.Next(50) * Multiplier(context);

            context.MonsterDamageType = DamageTypeIds.Fire;

            context.DealDamageToPlayer(damage);
        }

        [Register("coc:goblin_shaman_blind")]
        public static void Blind(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.GoblinShaman.Blind.TheGoblinGlares".Localized());

            if (Random.Shared.Next(Math.Max(1, (int)player.Intelligence / 5)) <= 4)
            {
                player.CreateStatusEffect(StatusEffectIds.Blind, 1 + Random.Shared.Next(3), 0, 0, 0);

                context.Append("Combat.GoblinShaman.Blind.YouAreBlinded".Localized());
            }
            else
                context.Append("Combat.GoblinShaman.Blind.YouManageTo".Localized());
        }

        [Register("coc:goblin_shaman_arouse")]
        public static void Arouse(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.GoblinShaman.Arouse.SheMakesA".Localized());

            var lust = (int)((enemy.Intelligence / 10f) + (player.Libido / 10f)

                + Random.Shared.Next(10) * Multiplier(context));

            player.TakeLustDamage(lust);
        }

        [Register("coc:goblin_shaman_chargeweapon")]
        public static void ChargeWeapon(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.GoblinShaman.ChargeWeapon.TheGoblinUtters".Localized());

            context.Enemy.CreateStatusEffect(
                StatusEffectIds.ChargeWeapon, 25 * Multiplier(context), 0, 0, 0);
        }

        [Register("coc:goblin_shaman_heal")]
        public static void Heal(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            context.Append("Combat.GoblinShaman.Heal.SheFocusesOn".Localized());

            var inner = (int)(10 + enemy.Intelligence / 2f

                + Random.Shared.Next(Math.Max(1, (int)(enemy.Intelligence / 3))));

            var amount = (int)(inner * Multiplier(context));

            context.Append("Combat.GoblinShaman.Heal.SheFlushesWith".Localized(
                CombatCalculator.GetHealText(amount)));

            enemy.HealthChange(amount);
        }

        [Register("coc:goblin_shaman_might")]
        public static void Might(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            context.Append("Combat.GoblinShaman.Might.SheFlushesDrawing".Localized());

            context.Append("Combat.GoblinShaman.Might.TheRushOf".Localized());

            var boost = 20 * Multiplier(context);

            enemy.CreateStatusEffect(StatusEffectIds.Might, boost, boost, 0, 0);

            enemy.Strength += boost;

            enemy.Toughness += boost;
        }

        [Register("coc:goblin_shaman_goblin_drug_attack")]
        public static void GoblinDrugAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var multiplier = 1.4f + player.NewGamePlusMod() * 0.5f;

            var color = Random.Shared.Next(2) == 0 ? "red" : "green";

            context.Append("Combat.GoblinShaman.DrugThrow".Localized(
                Utils.Utils.CapitalizeFirstLetter(enemy.Article), enemy.ShortName, color));

            if ((player.HasPerk(PerkIds.Evade) && Random.Shared.Next(10) <= 3)
                || Random.Shared.Next(100) < player.Speed / 5)
            {
                context.Append("Combat.GoblinShaman.DrugDodged".Localized());

                return;
            }

            if (player.Shield != null && player.Shield.Is(ItemIds.DragonShield)
                && Random.Shared.Next(2) == 0)
            {
                context.Append("Combat.GoblinShaman.DrugShield".Localized());

                return;
            }

            if (color == "red")
            {
                context.Append("Combat.GoblinShaman.DrugRed".Localized());

                if (!player.HasStatusEffect(StatusEffectIds.TemporaryHeat))
                    player.CreateStatusEffect(StatusEffectIds.TemporaryHeat, 0, multiplier, 0, 0);
            }
            else
            {
                context.Append("Combat.GoblinShaman.DrugGreen".Localized());

                if (!player.HasStatusEffect(StatusEffectIds.Poison))
                    player.CreateStatusEffect(StatusEffectIds.Poison, 0, multiplier, 0, 0);
            }
        }

        [Register("coc:goblin_shaman_goblin_tease_attack")]
        public static void GoblinTeaseAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var capitalA = Utils.Utils.CapitalizeFirstLetter(enemy.Article);

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

        [Register("coc:goblin_shaman_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        [Register("coc:goblin_shaman_lust_below_fifty")]
        public static bool LustBelow50(CombatContext context, CombatState state) =>

            context.Enemy.Lust < 50;

        [Register("coc:goblin_shaman_lust_above_fifty")]
        public static bool LustAbove50(CombatContext context, CombatState state) =>

            context.Enemy.Lust > 50;

        [Register("coc:goblin_shaman_lust_above_sixty")]
        public static bool LustAbove60(CombatContext context, CombatState state) =>

            context.Enemy.Lust > 60;

        [Register("coc:goblin_shaman_can_blind")]
        public static bool CanBlind(CombatContext context, CombatState state) =>

            context.Enemy.Lust < 50 && !context.Player.HasStatusEffect(StatusEffectIds.Blind);
    }
}