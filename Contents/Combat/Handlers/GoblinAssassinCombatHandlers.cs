using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class GoblinAssassinCombatHandlers
    {

        [Register("coc:goblin_assassin_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        [Register("coc:goblin_assassin_goblin_drug_attack")]
        public static void GoblinDrugAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var color = context.Random.Next(5) switch
            {
                0 => "red",

                1 => "green",

                2 => "blue",

                3 => "white",

                _ => "black"
            };

            if (color == "blue")
            {
                context.Append("Combat.GoblinAssassin.GoblinDrugAttack.OnePulls".Localized(
                    Utils.Utils.CapitalizeFirstLetter(enemy.Article), enemy.ShortName));

                if (enemy.HealthRatio() < 1)
                {
                    context.Append("Combat.GoblinAssassin.GoblinDrugAttack.SheLooksTo".Localized());

                    enemy.HealthChange(enemy.MaxHealth / 4);
                }
                else
                    context.Append("Combat.GoblinAssassin.GoblinDrugAttack.ThereDoesnT".Localized());

                return;
            }

            context.Append("Combat.GoblinAssassin.GoblinDrugAttack.OneUncorks".Localized(
                Utils.Utils.CapitalizeFirstLetter(enemy.Article), enemy.ShortName, color));

            if ((player.HasPerk(PerkIds.Evade) && context.Random.Next(10) <= 3)
                || context.Random.Next(100) < player.Speed / 5)
            {
                context.Append("Combat.GoblinAssassin.GoblinDrugAttack.YouNarrowlyAvoid".Localized());

                return;
            }

            if (player.Shield != null && player.Shield.Is(ItemIds.DragonShield)
                && context.Random.Next(2) == 0)
            {
                context.Append("Combat.GoblinAssassin.GoblinDrugAttack.TheLiquidHits".Localized());

                return;
            }

            switch (color)
            {
                case "red":
                    context.Append("Combat.GoblinAssassin.GoblinDrugAttack.TheRedFluids".Localized());

                    if (!player.HasStatusEffect(StatusEffectIds.TemporaryHeat))
                        player.CreateStatusEffect(StatusEffectIds.TemporaryHeat, 0, 1, 0, 0);

                    break;

                case "green":
                    context.Append("Combat.GoblinAssassin.GoblinDrugAttack.TheGreenishFluids".Localized());

                    if (!player.HasStatusEffect(StatusEffectIds.Poison))
                        player.CreateStatusEffect(StatusEffectIds.Poison, 0, 1, 0, 0);

                    break;

                case "white":
                    context.Append("Combat.GoblinAssassin.GoblinDrugAttack.YouTryTo".Localized());

                    if (!player.HasStatusEffect(StatusEffectIds.NoFlee))
                        player.CreateStatusEffect(StatusEffectIds.NoFlee, 0, 0, 0, 0);

                    break;

                case "black":
                    context.Append("Combat.GoblinAssassin.GoblinDrugAttack.TheBlackFluid".Localized());

                    player.ChangeFatigue(10 + context.Random.Next(25));

                    break;
            }
        }

        [Register("coc:goblin_assassin_lust_needle")]
        public static void LustNeedle(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.GoblinAssassin.LustNeedle.WithASwift".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            float lust;

            if (avoid.AttackFailed)
            {
                context.Append("Combat.GoblinAssassin.LustNeedle.YouVeAlready".Localized());

                lust = context.Random.Next(Math.Max(1, (int)(player.Libido / 10))) + 4;
            }
            else
            {
                context.Append("Combat.GoblinAssassin.LustNeedle.YouReFar".Localized());

                lust = context.Random.Next(Math.Max(1, (int)(player.Libido / 4))) + 20;
            }

            player.TakeLustDamage(lust);
        }

        [Register("coc:goblin_assassin_dual_shot")]
        public static void DualShot(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.GoblinAssassin.DualShot.TheAssassinThrows".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.GoblinAssassin.DualShot.YouJumpBackwards".Localized());

                return;
            }

            context.Append("Combat.GoblinAssassin.DualShot.BeforeYouCan".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player,

                enemy.Strength + enemy.WeaponAttack + 40);

            player.TakeDamage(damage);
        }

        [Register("coc:goblin_assassin_goblin_explosion")]
        public static void GoblinExplosion(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.GoblinAssassin.GoblinExplosion.WithoutASecond".Localized());

            context.Append("Combat.GoblinAssassin.GoblinExplosion.YouShieldYourself".Localized());

            var damage = 25 + context.Random.Next(75);

            context.MonsterDamageType = DamageTypeIds.Fire;

            context.DealDamageToPlayer(damage);
        }
    }
}