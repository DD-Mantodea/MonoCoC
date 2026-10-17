using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 classes/scenes/monsters/GoblinAssassin.hx 的五个招式。
    public static class GoblinAssassinCombatHandlers
    {
        // 原版 eAttack()：普通攻击（Monster.eAttack 的通用结算）。
        [Register("coc:goblin_assassin_attack")]
        private static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        // 原版 goblinDrugAttack()（GoblinAssassin.hx:8-78）：rand(5) 决定颜色，蓝色自愈，其余投掷。
        [Register("coc:goblin_assassin_goblin_drug_attack")]
        private static void GoblinDrugAttack(CombatContext context, CombatAbilityDefinition ability)
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

            // 蓝色：自己喝，回 1/4 上限血。
            if (color == "blue")
            {
                context.Append("Combat.GoblinAssassin.GoblinDrugAttack.01Pulls".Localized()
                    MonoCoC.Utils.Utils.CapitalizeFirstLetter(enemy.Article), enemy.ShortName);

                if (enemy.HealthRatio() < 1)
                {
                    context.Append("Combat.GoblinAssassin.GoblinDrugAttack.SheLooksTo".Localized());

                    enemy.HealthChange(enemy.MaxHealth / 4);
                }
                else
                {
                    context.Append("Combat.GoblinAssassin.GoblinDrugAttack.ThereDoesnT".Localized());
                }

                return;
            }

            context.Append("Combat.GoblinAssassin.GoblinDrugAttack.01Uncorks".Localized()
                MonoCoC.Utils.Utils.CapitalizeFirstLetter(enemy.Article), enemy.ShortName, color);

            // 原版 dodge 判定：(Evade && rand(10)<=3) || rand(100) < player.spe/5
            if ((player.HasPerk(VanillaPerks.Evade) && context.Random.Next(10) <= 3)
                || context.Random.Next(100) < player.Speed / 5)
            {
                context.Append("Combat.GoblinAssassin.GoblinDrugAttack.YouNarrowlyAvoid".Localized());

                return;
            }

            if (player.Shield != null && player.Shield.Is(VanillaItems.DragonShield)
                && context.Random.Next(2) == 0)
            {
                context.Append("Combat.GoblinAssassin.GoblinDrugAttack.TheLiquidHits".Localized());

                return;
            }

            switch (color)
            {
                case "red":
                    context.Append("Combat.GoblinAssassin.GoblinDrugAttack.TheRedFluids".Localized());

                    if (!player.HasStatusEffect(VanillaStatusEffects.TemporaryHeat))
                        player.CreateStatusEffect(VanillaStatusEffects.TemporaryHeat, 0, 1, 0, 0);

                    break;

                case "green":
                    context.Append("Combat.GoblinAssassin.GoblinDrugAttack.TheGreenishFluids".Localized());

                    if (!player.HasStatusEffect(VanillaStatusEffects.Poison))
                        player.CreateStatusEffect(VanillaStatusEffects.Poison, 0, 1, 0, 0);

                    break;

                case "white":
                    context.Append("Combat.GoblinAssassin.GoblinDrugAttack.YouTryTo".Localized());

                    if (!player.HasStatusEffect(VanillaStatusEffects.NoFlee))
                        player.CreateStatusEffect(VanillaStatusEffects.NoFlee, 0, 0, 0, 0);

                    break;

                case "black":
                    context.Append("Combat.GoblinAssassin.GoblinDrugAttack.TheBlackFluid".Localized());

                    player.ChangeFatigue(10 + context.Random.Next(25));

                    break;
            }
        }

        // 原版 lustNeedle()（GoblinAssassin.hx:81-96）：只可闪避；命中大幅涨欲望。
        [Register("coc:goblin_assassin_lust_needle")]
        private static void LustNeedle(CombatContext context, CombatAbilityDefinition ability)
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

        // 原版 dualShot()（GoblinAssassin.hx:99-112）：只可闪避；命中造成 str + weaponAttack + 40。
        [Register("coc:goblin_assassin_dual_shot")]
        private static void DualShot(CombatContext context, CombatAbilityDefinition ability)
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

        // 原版 goblinExplosion()（GoblinAssassin.hx:115-122）：火焰伤害 25 + rand(75)。
        [Register("coc:goblin_assassin_goblin_explosion")]
        private static void GoblinExplosion(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.GoblinAssassin.GoblinExplosion.WithoutASecond".Localized());
            context.Append("Combat.GoblinAssassin.GoblinExplosion.YouShieldYourself".Localized());

            var damage = 25 + context.Random.Next(75);

            context.MonsterDamageType = Identifier.Parse("coc:fire");
            context.DealDamageToPlayer(damage);
        }
    }
}
