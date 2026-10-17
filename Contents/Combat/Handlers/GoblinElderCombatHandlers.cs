using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class GoblinElderCombatHandlers
    {
        [Register("coc:goblin_elder_goblin_drug_attack")]
        public static void GoblinDrugAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var multiplier = 2f + player.NewGamePlusMod() * 0.5f;

            var color = Random.Shared.Next(2) == 0 ? "red" : "green";

            context.Append("Combat.GoblinElder.DrugThrow".Localized(enemy.ShortName, color));

            if (Random.Shared.Next(100) < player.Speed / 5)
            {
                context.Append("Combat.GoblinElder.DrugDodged".Localized());

                return;
            }

            if (player.Shield != null && player.Shield.Is(ItemIds.DragonShield) && Random.Shared.Next(2) == 0)
            {
                context.Append("Combat.GoblinElder.DrugShield".Localized());

                return;
            }

            if (color == "red")
            {
                context.Append("Combat.GoblinElder.DrugHeat".Localized());

                if (!player.HasStatusEffect(StatusEffectIds.TemporaryHeat))
                    player.CreateStatusEffect(StatusEffectIds.TemporaryHeat, 0, multiplier, 0, 0);
            }
            else
            {
                context.Append("Combat.GoblinElder.DrugPoison".Localized());

                if (!player.HasStatusEffect(StatusEffectIds.Poison))
                    player.CreateStatusEffect(StatusEffectIds.Poison, 0, multiplier, 0, 0);
            }
        }

        [Register("coc:goblin_elder_goblin_tease_attack")]
        [Register("coc:goblin_tease_attack")]
        public static void GoblinTeaseAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var det = Random.Shared.Next(3);

            context.Append(det switch
            {
                0 => "Combat.GoblinElder.TeaseBoneClad".Localized(context.Enemy.ShortName),

                1 => "Combat.GoblinElder.TeaseFlexibility".Localized(context.Enemy.ShortName),

                _ => "Combat.GoblinElder.TeaseAss".Localized(context.Enemy.ShortName)
            });

            var lust = (Random.Shared.Next(Math.Max(1, (int)context.Player.Libido / 10)) + 8) * 2;

            context.Player.TakeLustDamage(lust);

            context.Append("Combat.GoblinElder.TeaseAfter".Localized());
        }

        [Register("coc:goblin_elder_attack")]
        [Register("coc:goblin_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }
    }
}