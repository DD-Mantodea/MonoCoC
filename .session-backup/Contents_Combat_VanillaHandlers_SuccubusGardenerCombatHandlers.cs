using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 classes.scenes.dungeons.lethicesKeep.SuccubusGardener 的招式与阶段效果（SuccubusGardener.hx:132-385）。
    // 选招见 SuccubusGardenerEnemy.BeforeCombatAction / ChooseCombatAction。
    public static class SuccubusGardenerCombatHandlers
    {
        private static SuccubusGardenerEnemy Gardener(CombatContext context) =>
            (SuccubusGardenerEnemy)context.Enemy;

        // 原版 vineHeal()（SuccubusGardener.hx:132-174）：
        //   标记 VineHealUsed；未眩晕 → 「自己倒进藤蔓里喝」，眩晕 → 「被藤蔓强行灌」；
        //   回满血、lustVuln +0.3、fatigue +5（到 100 就再也用不出回血）。
        [Register("coc:succubus_gardener_vine_heal")]
        private static void VineHeal(CombatContext context, CombatAbilityDefinition ability)
        {
            var gardener = Gardener(context);

            if (!gardener.HasStatusEffect(SuccubusGardenerEnemy.VineHealUsedId))
                gardener.CreateStatusEffect(SuccubusGardenerEnemy.VineHealUsedId);

            if (!gardener.HasStatusEffect(VanillaStatusEffects.Stunned))
            {
                context.Append("Combat.SuccubusGardener.VineHeal.TippingHerselfBackward".Localized());

                if (gardener.LustVulnerability <= 0.15f)
                    context.Append("Combat.SuccubusGardener.VineHeal.YouArenT".Localized());
                else if (gardener.LustVulnerability <= 0.45f)
                    context.Append("Combat.SuccubusGardener.VineHeal.SheSDefinitely".Localized());
                else
                    context.Append("Combat.SuccubusGardener.VineHeal.ThereSNo".Localized());

                if (gardener.LustVulnerability <= 0.3f)
                    context.Append("Combat.SuccubusGardener.VineHeal.WhateverIsIn".Localized());
            }
            else
            {
                context.Append("Combat.SuccubusGardener.VineHeal.ActingOnSome".Localized());

                if (gardener.LustVulnerability <= 0.15f)
                    context.Append("Combat.SuccubusGardener.VineHeal.AreSlightlyGlassy".Localized());
                else if (gardener.LustVulnerability <= 0.45f)
                    context.Append("Combat.SuccubusGardener.VineHeal.AreEvenGlassier".Localized());
                else
                    context.Append("Combat.SuccubusGardener.VineHeal.AreDilatedAnd".Localized());

                if (gardener.LustVulnerability <= 0.3f)
                    context.Append("Combat.SuccubusGardener.VineHeal.WhateverIsIn".Localized());
            }

            gardener.Health = gardener.MaxHealth;
            gardener.LustVulnerability += 0.3f;
            gardener.Fatigue += 5;

            if (gardener.Fatigue >= 100)
            {
                context.Append("Combat.SuccubusGardener.VineHeal.ItAppearsThat".Localized());

                gardener.Fatigue = 100;
            }
        }

        // 原版 tentagrapple()（SuccubusGardener.hx:176-190）：
        //   自挂 10 回合抓取冷却；命中则给玩家挂 Tentagrappled。
        [Register("coc:succubus_gardener_tentagrapple")]
        private static void Tentagrapple(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var gardener = Gardener(context);

            gardener.CreateStatusEffect(SuccubusGardenerEnemy.TentagrappleCooldownId, 10);

            context.Append("Combat.SuccubusGardener.Tentagrapple.AWebOf".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = gardener, Defender = player,
                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.SuccubusGardener.Tentagrapple.YouSlipAside".Localized());
            }
            else
            {
                context.Append("Combat.SuccubusGardener.Tentagrapple.YouTwistTo".Localized());

                player.CreateStatusEffect(SuccubusGardenerEnemy.TentagrappledId);
            }
        }

        // 原版 sapSpeed()（SuccubusGardener.hx:349-357）：给玩家挂减速（原版 CombatBuff：Speed -20%）。
        [Register("coc:succubus_gardener_sap_speed")]
        private static void SapSpeed(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.SuccubusGardener.SapSpeed.CuppingHerBreasts".Localized());
            context.Append("Combat.SuccubusGardener.SapSpeed.TheLacticAdhesive".Localized());

            if (!player.HasStatusEffect(SuccubusGardenerEnemy.GardenerSapSpeedId))
            {
                player.CreateStatusEffect(SuccubusGardenerEnemy.GardenerSapSpeedId);

                // 原版 GardenerSapSpeedDebuff：buffHost(Spe(-host.spe * 0.2))
                CombatCalculator.AddCombatBuff(player, VanillaStats.Speed, -player.Speed * 0.2f,
                    "GardenerSapSpeed", SuccubusGardenerEnemy.GardenerSapSpeedId);
            }
        }

        // 原版 sicem()（SuccubusGardener.hx:250-269）：
        //   10 次独立闪避判定，每次命中累计 2 + rand(1+lib/20) + rand(1+sens/20) 点欲望伤害。
        //   原版是 `if (damage >= 0)`——damage 初值 0，所以「全部躲开」的分支其实永远走不到，照抄。
        [Register("coc:succubus_gardener_sicem")]
        private static void Sicem(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.SuccubusGardener.Sicem.JigglingOhSo".Localized());

            var damage = 0f;

            for (var i = 0; i < 10; i++)
            {
                var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
                {
                    Attacker = enemy, Defender = player,
                    DoDodge = true, DoParry = false, DoBlock = false
                });

                if (avoid.AttackHit)
                {
                    damage += 2f +
                        (int)(context.Random.NextDouble() * (1 + player.Libido / 20f)) +
                        (int)(context.Random.NextDouble() * (1 + player.Sensitivity / 20f));
                }
            }

            if (damage >= 0)
            {
                context.Append("Combat.SuccubusGardener.Sicem.TheSinuousPlant".Localized());

                player.TakeLustDamage(damage, true);
            }
            else
            {
                context.Append("Combat.SuccubusGardener.Sicem.TheSinuousPlant2".Localized());
            }
        }

        // 原版 corruptiveShower()（SuccubusGardener.hx:271-287）：命中就给玩家挂/叠加 3 层植物精液 DoT。
        [Register("coc:succubus_gardener_corruptive_shower")]
        private static void CorruptiveShower(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.SuccubusGardener.CorruptiveShower.TheSuccubusLifts".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,
                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.SuccubusGardener.CorruptiveShower.SomehowYouManage".Localized());

                return;
            }

            if (!player.HasStatusEffect(SuccubusGardenerEnemy.ShowerDotEffectId))
                player.CreateStatusEffect(SuccubusGardenerEnemy.ShowerDotEffectId, 3);
            else
                player.ChangeStatusValue(SuccubusGardenerEnemy.ShowerDotEffectId, 0, 3);

            context.Append("Combat.SuccubusGardener.CorruptiveShower.YouTryYour".Localized());
        }

        // 原版 lustAuraCast()（SuccubusGardener.hx:359-367）：
        //   第一次只是自挂 LustAura；之后每次立刻给玩家 8 + (lib/20 + cor/25) 点欲望。
        [Register("coc:succubus_gardener_lust_aura")]
        private static void LustAuraCast(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var gardener = Gardener(context);

            context.Append("Combat.SuccubusGardener.LustAuraCast.TheDemonessBlinks".Localized());

            if (gardener.HasStatusEffect(VanillaStatusEffects.LustAura))
            {
                context.Append("Combat.SuccubusGardener.LustAuraCast.YourEyesCross".Localized());

                player.TakeLustDamage(8 + (int)(player.Libido / 20f + player.Corruption / 25f), true);
            }
            else
            {
                gardener.CreateStatusEffect(VanillaStatusEffects.LustAura);
            }
        }

        // 原版 taunt()（SuccubusGardener.hx:333-335）：纯台词，无数值。
        [Register("coc:succubus_gardener_taunt")]
        private static void Taunt(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.SuccubusGardener.Taunt.TheGreenThumbed".Localized());
        }

        // 原版 motorboat()（SuccubusGardener.hx:337-347）：
        //   按双方身高差加一句，纯台词，无数值。
        [Register("coc:succubus_gardener_motorboat")]
        private static void Motorboat(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var gardener = Gardener(context);

            context.Append("Combat.SuccubusGardener.Motorboat.TheDemonessGrowls".Localized());

            if (player.Tallness <= gardener.Tallness - 6)
                context.Append("Combat.SuccubusGardener.Motorboat.SomehowShortEnough".Localized());
            else if (player.Tallness >= gardener.Tallness + 6)
                context.Append("Combat.SuccubusGardener.Motorboat.SomehowTallEnough".Localized());

            context.Append("Combat.SuccubusGardener.Motorboat.TheyReSo".Localized());
            context.Append("Combat.SuccubusGardener.Motorboat.YourFoeGiggles".Localized());
        }

        // 原版 tasteTheEcstasy()（SuccubusGardener.hx:369-385）：
        //   力量判定 rand(str - 30) + 30 > 敌人 str 即为「失败」→ 吞精：欲望 + 腐蚀 +5。
        [Register("coc:succubus_gardener_taste_the_ecstasy")]
        private static void TasteTheEcstasy(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.SuccubusGardener.TasteTheEcstasy.ThreeTentaclesStab".Localized());
            context.Append("Combat.SuccubusGardener.TasteTheEcstasy.OneOfThose".Localized());

            // 原版 Utils.rand(player.str - 30)（负数时会得到 0 或负数，与 C# 截断取整一致）
            var roll = (int)(context.Random.NextDouble() * (player.Strength - 30)) + 30;

            if (roll > enemy.Strength)
            {
                context.Append("Combat.SuccubusGardener.TasteTheEcstasy.ItHangsThere".Localized());
                context.Append("Combat.SuccubusGardener.TasteTheEcstasy.YouBarelyEven".Localized());

                player.TakeLustDamage(8 + (int)(player.Libido / 20f + player.Corruption / 25f), true);

                // 原版 game.dynStats(Cor(5))
                player.ApplyStats(new DynStat(VanillaStats.Corruption, 5f));
            }
            else
            {
                context.Append("Combat.SuccubusGardener.TasteTheEcstasy.ThatSAll".Localized());
                context.Append("Combat.SuccubusGardener.TasteTheEcstasy.ShePouts".Localized());
            }
        }

        // 原版 struggle()（SuccubusGardener.hx:192-220）：玩家被藤蔓抓住时的挣扎判定。
        public static void Struggle(CombatContext context)
        {
            var player = context.Player;
            var gardener = Gardener(context);

            if (!player.HasStatusEffect(SuccubusGardenerEnemy.TentagrappledId))
                return;

            context.Clear();

            var numRounds = (int)player.StatusEffectValue(SuccubusGardenerEnemy.TentagrappledId, 0);

            // 原版 Utils.rand(player.str) > this.str / (1 + numRounds / 2)
            if ((int)(context.Random.NextDouble() * player.Strength) > gardener.Strength / (1 + numRounds / 2f))
            {
                context.Append("Combat.SuccubusGardener.Struggle.YouScrabbleDesperately".Localized());

                player.RemoveStatusEffect(SuccubusGardenerEnemy.TentagrappledId);
            }
            else
            {
                context.Append("Combat.SuccubusGardener.Struggle.YouScrabbleDesperately2".Localized());

                context.Append(context.Random.Next(2) == 0
                    ? "Combat.SuccubusGardener.Struggle.ButTheVines".Localized()
                    : "Combat.SuccubusGardener.Struggle.YouReIntimately".Localized());

                player.ChangeStatusValue(SuccubusGardenerEnemy.TentagrappledId, 0, 1);

                if (!player.HasPerk(VanillaPerks.Juggernaut) && gardener.ArmorPerk != "Heavy")
                    player.HealthChange(-(75 + context.Random.Next(15)));

                player.TakeLustDamage(3 + context.Random.Next(3), true);

                // 原版 game.ceraphScene.hasBondage()：被 Ceraph 捆着会额外爽一下
                if (SceneInstance<CeraphScene>.Get().HasBondage())
                    player.TakeLustDamage(7, true);
            }

            gardener.TookAction = true;
        }

        // 原版 squeeze()（SuccubusGardener.hx:236-248）：react(PlayerWaited) 里「放弃挣扎 → 被收紧」。
        public static void Squeeze(CombatContext context)
        {
            var player = context.Player;
            var gardener = Gardener(context);

            context.Clear();

            context.Append(context.Random.Next(2) == 0
                ? "Combat.SuccubusGardener.Squeeze.TheVinesEncircling".Localized()
                : "Combat.SuccubusGardener.Squeeze.YouReIntimately".Localized());

            player.ChangeStatusValue(SuccubusGardenerEnemy.TentagrappledId, 0, 1);

            if (!player.HasPerk(VanillaPerks.Juggernaut) && gardener.ArmorPerk != "Heavy")
                player.HealthChange(-(75 + context.Random.Next(15)));

            player.TakeLustDamage(3 + context.Random.Next(3), true);
        }

        // 原版 showerDotEffect()（SuccubusGardener.hx:289-331）：植物精液 DoT 的每回合结算。
        // 由 SuccubusGardenerEnemy.BeforeCombatAction 调用。
        // 未移植：原版结尾会 append game.combat.getLustText(lustDamage)（"(+N)" 的欲望增量标记），
        // C# 没有对应 helper，其它敌人 handler 也都不输出这个标记，这里保持一致省略。
        public static void ShowerDotEffect(CombatContext context)
        {
            var player = context.Player;

            var lustDamage = 2 + context.Random.Next(2);

            player.TakeLustDamage(lustDamage, false);

            player.ChangeStatusValue(SuccubusGardenerEnemy.ShowerDotEffectId, 0, -1);

            if (player.Lust100 < 50)
            {
                context.Append("Combat.SuccubusGardener.ShowerDotEffect.TheTentaclesSex".Localized());
            }
            else if (player.Lust100 < 60)
            {
                context.Append("Combat.SuccubusGardener.ShowerDotEffect.YouTryTo".Localized());

                context.Append(player.Corruption < 50
                    ? "Combat.SuccubusGardener.ShowerDotEffect.YouReAshamed".Localized()
                    : "Combat.SuccubusGardener.ShowerDotEffect.YouReA".Localized());

                context.Append("Combat.SuccubusGardener.ShowerDotEffect.ThatItS".Localized());
            }
            else if (player.Lust100 < 70)
            {
                context.Append("Combat.SuccubusGardener.ShowerDotEffect.YouGroanAt".Localized());
            }
            else if (player.Lust100 < 80)
            {
                context.Append("Combat.SuccubusGardener.ShowerDotEffect.YouWhimperAs".Localized());
            }
            else if (player.Lust100 < 90)
            {
                context.Append("Combat.SuccubusGardener.ShowerDotEffect.YouShudderIn".Localized());
            }
            else if (player.Lust100 < 100)
            {
                context.Append("Combat.SuccubusGardener.ShowerDotEffect.OhhhhYouRe".Localized());

                if (player.HasCock())
                {
                    context.Append("Combat.SuccubusGardener.ShowerDotEffect.EvenWhileYour"
                        .Localized(player.Cocks.Count > 1 ? "s are" : " is"));
                }
                else if (player.HasVagina())
                {
                    context.Append("Combat.SuccubusGardener.ShowerDotEffect.SoakedTwatThreatens"
                        .Localized(player.Wetness() == 4 ? "drown" : "further soak"));
                }
                else
                {
                    context.Append("Combat.SuccubusGardener.ShowerDotEffect.Text".Localized());
                }
            }
            else
            {
                context.Append("Combat.SuccubusGardener.ShowerDotEffect.OhhhFuckThere".Localized());
            }

            if (player.StatusEffectValue(SuccubusGardenerEnemy.ShowerDotEffectId, 0) < 0 &&
                player.Lust100 < 100)
            {
                player.RemoveStatusEffect(SuccubusGardenerEnemy.ShowerDotEffectId);

                context.Append("Combat.SuccubusGardener.ShowerDotEffect.TheLustInducing".Localized());
            }
        }
    }
}
