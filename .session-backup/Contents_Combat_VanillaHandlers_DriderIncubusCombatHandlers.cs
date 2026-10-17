using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using System.Linq;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 classes.scenes.dungeons.lethicesKeep.DriderIncubus 的整回合逻辑（DriderIncubus.hx:80-415）。
    // 原版 performCombatAction 一回合里会「先结算回血/掉血 → 刺击 + 一次子招式 →（哥布林已放走时）再加哥布林行动」，
    // 所以 C# 这边用一个 Round handler 把整套流程跑完（ChooseCombatAction 固定返回这一招）。
    public static class DriderIncubusCombatHandlers
    {
        private static DriderIncubusEnemy Drider(CombatContext context) => (DriderIncubusEnemy)context.Enemy;

        // 原版伤害公式里都乘 (1 + player.newGamePlusMod() * 0.3)
        private static float Ngp(Player player) => 1f + player.NewGamePlusMod() * 0.3f;

        // 原版 Utils.rand(max:Float) = Std.int(random * max)
        private static int Rand(CombatContext context, float max) => (int)(context.Random.NextDouble() * max);

        [Register("coc:drider_incubus_round")]
        private static void Round(CombatContext context, CombatAbilityDefinition ability)
        {
            var drider = Drider(context);

            // 原版 performCombatAction()（DriderIncubus.hx:80-98）
            drider.CombatRound += 1;

            if (drider.Lust100 < 65 && drider.Hp100 < 33)
                GainHpAndLust(context);
            else if (drider.Lust100 >= 65 && drider.Hp100 >= 33)
                DropHpAndLust(context);

            if (context.Random.Next(100) > drider.Lust + 10f)
            {
                SpearStrike(context);

                context.Append("Combat.DriderIncubus.PerformCombatAction.Text".Localized());

                PerformPhysicalAttack(context);
            }
            else
            {
                PerformLustAttack(context);
            }

            if (drider.GoblinFree)
            {
                context.Append("Combat.DriderIncubus.PerformCombatAction.Text".Localized());

                GoblinAI(context);
            }
        }

        // 原版 gainHpAndLust()（DriderIncubus.hx:135-158）：回 10% 最大血 + 8 欲望，按 _hpGains 换台词。
        private static void GainHpAndLust(CombatContext context)
        {
            var drider = Drider(context);

            drider.HealthChange(drider.MaxHealth * 0.1f);

            drider.Lust += 8;

            context.Append(drider.HpGains switch
            {
                0 => "Combat.DriderIncubus.GainHpAndLust.Text".Localized(),
                1 => "Combat.DriderIncubus.GainHpAndLust.Text2".Localized(),
                2 => "Combat.DriderIncubus.GainHpAndLust.Text3".Localized(),
                _ => "Combat.DriderIncubus.GainHpAndLust.Text4".Localized()
            });

            drider.HpGains += 1;

            context.Append("Combat.DriderIncubus.GainHpAndLust.TheDemonGestures".Localized());

            // 原版这里的 lust100 是「加完 8 点之后」的值
            if (drider.Lust100 > 65)
            {
                context.Append(drider.GoblinFree
                    ? "Combat.DriderIncubus.GainHpAndLust.HisDickIs".Localized()
                    : "Combat.DriderIncubus.GainHpAndLust.HisBallsAre".Localized());

                context.Append("Combat.DriderIncubus.GainHpAndLust.YouDoubtHe".Localized());
            }

            context.Append("Combat.DriderIncubus.GainHpAndLust.Text5".Localized());
        }

        // 原版 dropHpAndLust()（DriderIncubus.hx:160-164）：掉 8% 最大血、欲望 -10。
        private static void DropHpAndLust(CombatContext context)
        {
            var drider = Drider(context);

            drider.HealthChange(-(drider.MaxHealth * 0.08f));

            drider.Lust -= 10;

            context.Append("Combat.DriderIncubus.DropHpAndLust.TheDemonSnarls".Localized());
        }

        // 原版 spearStrike()（DriderIncubus.hx:166-182）：
        //   伤害 = (str + weaponAttack) * 0.4 * NG+；可闪/可招架（不能格挡）。
        private static void SpearStrike(CombatContext context)
        {
            var player = context.Player;
            var drider = Drider(context);

            context.Append("Combat.DriderIncubus.SpearStrike.TheDriderRears".Localized());

            var damage = (drider.Strength + drider.WeaponAttack) * 0.4f * Ngp(player);

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = drider, Defender = player,
                DoDodge = true, DoParry = true, DoBlock = false
            });

            var reason = avoid.DodgeReason;

            if (damage <= 0 || reason is "Speed" or "Flexibility" or "Unhindered")
                context.Append("Combat.DriderIncubus.SpearStrike.YouBarelySlide".Localized());
            else if (reason == "Evade")
                context.Append("Combat.DriderIncubus.SpearStrike.YouEvadeThe".Localized());
            else if (reason == "Misdirection")
                context.Append("Combat.DriderIncubus.SpearStrike.UsingYourSkills".Localized());
            else if (avoid.Outcome == AvoidOutcome.Parry)
                context.Append("Combat.DriderIncubus.SpearStrike.YouDeftlyParry".Localized());
            else
            {
                context.Append("Combat.DriderIncubus.SpearStrike.TheWeaponBites".Localized());

                context.DealDamageToPlayer(damage);
            }
        }

        // 原版 performPhysicalAttack()（DriderIncubus.hx:100-110）：
        //   每 6 回合（以及第 3 回合）强制 stunningSpear；否则在 [web, kick, kick, doubleStrike, doubleStrike]
        //   （疲劳未满时再加 bite）里等权重抽。
        private static void PerformPhysicalAttack(CombatContext context)
        {
            var drider = Drider(context);

            if (drider.CombatRound >= 3 && (drider.CombatRound % 6 == 0 || drider.CombatRound == 3))
            {
                StunningSpear(context);

                return;
            }

            var options = new Action[]
            {
                () => SpiderMorphWebAttack(context),
                () => Kick(context),
                () => Kick(context),
                () => DoubleStrike(context),
                () => DoubleStrike(context)
            }.ToList();

            // 原版 fatigue100 < 100（疲劳没满才可能出 bite）
            if (drider.Fatigue < drider.MaxFatigue())
                options.Add(() => Bite(context));

            options[context.Random.Next(options.Count)]();
        }

        // 原版 performLustAttack()（DriderIncubus.hx:112-125）：
        //   每 6 回合（以及第 3 回合）强制 constrictingThoughts；
        //   否则 [arouseSpell, arouseSpell] + 条件追加 taintedMind / purpleHaze。
        private static void PerformLustAttack(CombatContext context)
        {
            var player = context.Player;
            var drider = Drider(context);

            if (drider.CombatRound >= 3 && (drider.CombatRound % 6 == 0 || drider.CombatRound == 3))
            {
                ConstrictingThoughts(context);

                return;
            }

            var options = new Action[]
            {
                () => ArouseSpell(context),
                () => ArouseSpell(context)
            }.ToList();

            if (!player.HasStatusEffect(DriderIncubusEnemy.TaintedMindId) && !drider.SeenResolute)
                options.Add(() => TaintedMind(context));

            if (!drider.SeenResolute)
                options.Add(() => PurpleHaze(context));

            options[context.Random.Next(options.Count)]();
        }

        // 原版 goblinAI()（DriderIncubus.hx:127-133）：哥布林的四种行动，未涂油时再加 babyOilMeUp。
        private static void GoblinAI(CombatContext context)
        {
            var drider = Drider(context);

            var options = new Action[]
            {
                () => GoblinHandjob(context),
                () => GoblinTongueGrapple(context),
                () => BootyTwerking(context),
                () => WebNipplechain(context)
            }.ToList();

            if (!drider.GoblinOiled)
                options.Add(() => BabyOilMeUp(context));

            options[context.Random.Next(options.Count)]();
        }

        // 原版 bite()（DriderIncubus.hx:184-216）：
        //   先疲劳 +20；玩家眩晕、或速度 <= 1 且 Web >= 2 时「白给」被咬（必定命中）；
        //   否则走闪避判定。命中挂 DriderIncubusVenom。
        private static void Bite(CombatContext context)
        {
            var player = context.Player;
            var drider = Drider(context);

            drider.Fatigue += 20;

            if (player.HasStatusEffect(VanillaStatusEffects.Stunned) ||
                (player.Speed <= 1 && player.StatusEffectValue(VanillaStatusEffects.Web, 0) >= 2))
            {
                ApplyVenom(context);

                context.Append("Combat.DriderIncubus.Bite.TakingHisTime".Localized());

                if (player.Strength <= 25)
                    context.Append("Combat.DriderIncubus.Bite.ItSGetting".Localized());
                else if (player.Strength <= 1)
                    context.Append("Combat.DriderIncubus.Bite.YouDonT".Localized());

                context.Append("Combat.DriderIncubus.Bite.WhenHePulls".Localized());

                context.Append(player.Strength > 25
                    ? "Combat.DriderIncubus.Bite.Text".Localized()
                    : "Combat.DriderIncubus.Bite.Text2".Localized());

                return;
            }

            context.Append("Combat.DriderIncubus.Bite.TwistingOverThe".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = drider, Defender = player,
                DoDodge = true, DoParry = false, DoBlock = false
            });

            var reason = avoid.DodgeReason;

            if (reason == "Misdirection")
            {
                context.Append("Combat.DriderIncubus.Bite.YouMisdirectHis".Localized());
            }
            else if (reason == "Evade")
            {
                context.Append("Combat.DriderIncubus.Bite.YouEvadeHis".Localized());
            }
            else if (reason is "Speed" or "Flexibility" or "Unhindered")
            {
                context.Append("Combat.DriderIncubus.Bite.YouAvoidHis".Localized());
            }
            else
            {
                context.Append("Combat.DriderIncubus.Bite.ThoseNeedleLike".Localized());
                context.Append("Combat.DriderIncubus.Bite.HeFlashesA".Localized());

                ApplyVenom(context);
            }
        }

        // 原版 DriderIncubusVenomDebuff（DriderIncubusVenomDebuff.hx:16-20）：
        //   永久 -5 力量 + 战斗内 -30 力量（CombatBuff 只在第一次挂上时生效，所以用 HasStatusEffect 把关）。
        private static void ApplyVenom(CombatContext context)
        {
            var player = context.Player;

            if (player.HasStatusEffect(DriderIncubusEnemy.DriderIncubusVenomId))
                return;

            player.CreateStatusEffect(DriderIncubusEnemy.DriderIncubusVenomId);

            player.ApplyStats(new DynStat(VanillaStats.Strength, -5f));

            CombatCalculator.AddCombatBuff(player, VanillaStats.Strength, -30, "DriderIncubusVenom",
                DriderIncubusEnemy.DriderIncubusVenomId);
        }

        // 原版 kick()（DriderIncubus.hx:218-234）：闪避判定后按闪避原因分文案；命中才结算伤害。
        private static void Kick(CombatContext context)
        {
            var player = context.Player;
            var drider = Drider(context);

            context.Append("Combat.DriderIncubus.Kick.WhileYouRe".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = drider, Defender = player,
                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.DodgeReason == "Misdirection")
            {
                context.Append("Combat.DriderIncubus.Kick.YouTwistOut".Localized());

                return;
            }

            if (avoid.DodgeReason != null)
            {
                context.Append("Combat.DriderIncubus.Kick.YouTwistOut2".Localized());

                return;
            }

            var damage = (drider.Strength + drider.WeaponAttack + 25 - Rand(context, player.Toughness)) * Ngp(player);

            if (damage > 0)
            {
                context.Append("Combat.DriderIncubus.Kick.YouGoFlying".Localized());

                context.DealDamageToPlayer(damage);
            }
            else
            {
                context.Append("Combat.DriderIncubus.Kick.YouSuccessfullyManage".Localized());
            }
        }

        // 原版 stunningSpear()（DriderIncubus.hx:236-261）：
        //   注意原版最后三句（YouHearThe / Bone / ButYouManage）在 if 外面，所以无论是否命中都会输出，
        //   照抄这个（疑似 bug 的）文本顺序。
        private static void StunningSpear(CombatContext context)
        {
            var player = context.Player;
            var drider = Drider(context);

            context.Append("Combat.DriderIncubus.StunningSpear.TwirlingHisWeapon".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = drider, Defender = player,
                DoDodge = true, DoParry = false, DoBlock = false
            });

            var reason = avoid.DodgeReason;

            if (reason is "Speed" or "Flexibility" or "Unhindered")
            {
                context.Append("Combat.DriderIncubus.StunningSpear.YouDuckIn".Localized());
            }
            else if (reason == "Misdirection")
            {
                context.Append("Combat.DriderIncubus.StunningSpear.YouWereAlready".Localized());
            }
            else if (reason == "Evade")
            {
                context.Append("Combat.DriderIncubus.StunningSpear.YouLeanIn".Localized());
            }
            else
            {
                var damage = (drider.Strength + drider.WeaponAttack - 25 - Rand(context, player.Toughness)) * Ngp(player);

                if (damage > 0)
                {
                    context.Append("Combat.DriderIncubus.StunningSpear.YouDonT".Localized());
                    context.Append("Combat.DriderIncubus.StunningSpear.Bone".Localized());
                    context.Append("Combat.DriderIncubus.StunningSpear.Text".Localized());

                    // 原版 player.stun(0, 100)
                    if (player.Stun(0, 100))
                        context.Append("Combat.DriderIncubus.StunningSpear.YouReLeft".Localized());

                    context.DealDamageToPlayer(damage);
                }

                context.Append("Combat.DriderIncubus.StunningSpear.YouHearThe".Localized());
                context.Append("Combat.DriderIncubus.StunningSpear.Bone".Localized());
                context.Append("Combat.DriderIncubus.StunningSpear.ButYouManage".Localized());
            }
        }

        // 原版 doubleStrike()（DriderIncubus.hx:263-279）：
        //   伤害 = str + weaponAttack + 10 - rand(player.tou)（注意这条没有 NG+ 系数）。
        private static void DoubleStrike(CombatContext context)
        {
            var player = context.Player;
            var drider = Drider(context);

            context.Append("Combat.DriderIncubus.DoubleStrike.HeSkittersForward".Localized());

            var damage = drider.Strength + drider.WeaponAttack + 10 - Rand(context, player.Toughness);

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = drider, Defender = player,
                DoDodge = true, DoParry = true, DoBlock = false
            });

            var reason = avoid.DodgeReason;

            if (damage <= 0 || reason is "Speed" or "Flexibility" or "Unhindered")
                context.Append("Combat.DriderIncubus.DoubleStrike.YouBarelySlide".Localized());
            else if (reason == "Evade")
                context.Append("Combat.DriderIncubus.DoubleStrike.YouEvadeThe".Localized());
            else if (reason == "Misdirection")
                context.Append("Combat.DriderIncubus.DoubleStrike.UsingYourSkills".Localized());
            else if (avoid.Outcome == AvoidOutcome.Parry)
                context.Append("Combat.DriderIncubus.DoubleStrike.YouParryThe".Localized());
            else
            {
                context.Append("Combat.DriderIncubus.DoubleStrike.TheWeaponBites".Localized());

                context.DealDamageToPlayer(damage);
            }
        }

        // 原版 spiderMorphWebAttack()（AbstractSpiderMorph.hx:30-49）：
        //   C# 没有 AbstractSpiderMorph，这里按原版 WebDebuff 的语义实现：
        //   命中则挂 Web 状态，每次命中再叠 -25 速度（WebDebuff.apply 忽略 firstTime，每层都生效）。
        private static void SpiderMorphWebAttack(CombatContext context)
        {
            var player = context.Player;
            var drider = Drider(context);

            context.Append("Combat.DriderIncubus.SpiderMorphWebAttack.Text".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = drider, Defender = player,
                DoDodge = true, DoParry = false, DoBlock = true
            });

            if (avoid.AttackFailed)
                return;

            if (!player.HasStatusEffect(VanillaStatusEffects.Web))
            {
                context.Append("Combat.DriderIncubus.SpiderMorphWebAttack.HitFirst".Localized());

                player.CreateStatusEffect(VanillaStatusEffects.Web, 3);
            }
            else
            {
                context.Append("Combat.DriderIncubus.SpiderMorphWebAttack.HitMore".Localized());

                player.ChangeStatusValue(VanillaStatusEffects.Web, 0, 1);
            }

            CombatCalculator.AddCombatBuff(player, VanillaStats.Speed, -25, "web", VanillaStatusEffects.Web);
        }

        // 原版 arouseSpell()（DriderIncubus.hx:281-301）：欲望伤害 = lib/10 + cor/10 + 15。
        private static void ArouseSpell(CombatContext context)
        {
            var player = context.Player;

            context.Append("Combat.DriderIncubus.ArouseSpell.TheDemonicDrider".Localized());
            context.Append("Combat.DriderIncubus.ArouseSpell.AnInvisibleForce".Localized());

            if (player.Cocks.Count > 0 || player.Vaginas.Count > 0)
            {
                context.Append("Combat.DriderIncubus.ArouseSpell.MakingYour".Localized());

                context.Append(player.Cocks.Count > 0
                    ? "Combat.DriderIncubus.ArouseSpell.Text".Localized()
                    : "Combat.DriderIncubus.ArouseSpell.Text2".Localized());

                context.Append("Combat.DriderIncubus.ArouseSpell.AcheToBe".Localized());
            }

            context.Append("Combat.DriderIncubus.ArouseSpell.Text3".Localized());

            player.TakeLustDamage(player.Libido / 10f + player.Corruption / 10f + 15, true);

            context.Append("Combat.DriderIncubus.ArouseSpell.YourBodyRebels".Localized());

            context.Append(player.Lust100 < 100
                ? "Combat.DriderIncubus.ArouseSpell.ButTheEffect".Localized()
                : "Combat.DriderIncubus.ArouseSpell.Text4".Localized());
        }

        // 原版 taintedMind()（DriderIncubus.hx:303-309）：挂 4 回合 TaintedMind。
        private static void TaintedMind(CombatContext context)
        {
            var player = context.Player;

            context.Append("Combat.DriderIncubus.TaintedMind.HeClapsHis".Localized());

            if (player.Corruption <= 33)
                context.Append("Combat.DriderIncubus.TaintedMind.WhatDidHe".Localized());

            player.CreateStatusEffect(DriderIncubusEnemy.TaintedMindId, 4);
        }

        // 原版 taintedMindAttackAttempt()（DriderIncubus.hx:311-314）：TaintedMind 期间攻击被换成自摸。
        public static void TaintedMindAttackAttempt(CombatContext context)
        {
            context.Append("Combat.DriderIncubus.TaintedMindAttackAttempt.YouReadyAn".Localized());

            context.Player.TakeLustDamage(15, true);
        }

        // 原版 constrictingThoughts()（DriderIncubus.hx:316-326）：
        //   player.stun(0, 100) 成功（= 被眩晕）才吃欲望伤害；否则标记 _seenResolute。
        private static void ConstrictingThoughts(CombatContext context)
        {
            var player = context.Player;
            var drider = Drider(context);

            context.Append("Combat.DriderIncubus.ConstrictingThoughts.TheCorruptedArachnid".Localized());
            context.Append("Combat.DriderIncubus.ConstrictingThoughts.DisjointedEroticThoughts".Localized());

            if (!player.Stun(0, 100))
            {
                drider.SeenResolute = true;

                context.Append("Combat.DriderIncubus.ConstrictingThoughts.YouMarshalYour".Localized());
            }
            else
            {
                context.Append("Combat.DriderIncubus.ConstrictingThoughts.TheIntensityOverwhelms".Localized());

                player.TakeLustDamage(player.Libido / 15f + player.Corruption / 15f + 15, true);
            }
        }

        // 原版 purpleHaze()（DriderIncubus.hx:328-341）：
        //   Resolute perk 直接免疫（并标记 _seenResolute）；否则 66% 概率抵抗；
        //   失败则 +25 欲望、挂 PurpleHaze(2~3) 与同层数的 Blind。
        private static void PurpleHaze(CombatContext context)
        {
            var player = context.Player;

            context.Append("Combat.DriderIncubus.PurpleHaze.TheDriderGestures".Localized());

            if (player.HasPerk(VanillaPerks.Resolute) || context.Random.NextDouble() * 100 < 66)
            {
                if (player.HasPerk(VanillaPerks.Resolute))
                    Drider(context).SeenResolute = true;

                context.Append("Combat.DriderIncubus.PurpleHaze.YouFlexYour".Localized());

                return;
            }

            context.Append("Combat.DriderIncubus.PurpleHaze.YouConcentrateTo".Localized());

            player.TakeLustDamage(25, true);

            var duration = 2 + context.Random.Next(2);

            player.CreateStatusEffect(DriderIncubusEnemy.PurpleHazeId, duration);
            player.CreateStatusEffect(VanillaStatusEffects.Blind, duration);
        }

        // 原版 goblinHandjob()（DriderIncubus.hx:343-346）：纯台词。
        private static void GoblinHandjob(CombatContext context)
        {
            context.Append("Combat.DriderIncubus.GoblinHandjob.SomehowTheGoblin".Localized());
            context.Append("Combat.DriderIncubus.GoblinHandjob.UnfortunatelyForHer".Localized());
        }

        // 原版 goblinTongueGrapple()（DriderIncubus.hx:348-356）：自己欲望 +5。
        // 原版 `if (lust100 <= 100)` 恒真，所以 else 分支是死代码，照抄。
        private static void GoblinTongueGrapple(CombatContext context)
        {
            var drider = Drider(context);

            context.Append("Combat.DriderIncubus.GoblinTongueGrapple.TheDeterminedGoblin".Localized());

            drider.Lust += 5;

            if (drider.Lust100 <= 100)
                context.Append("Combat.DriderIncubus.GoblinTongueGrapple.TheDriderSkitters".Localized());
            else
                context.Append("Combat.DriderIncubus.GoblinTongueGrapple.TheDriderAttempts".Localized());
        }

        // 原版 bootyTwerking()（DriderIncubus.hx:358-361）：自己欲望 +5。
        private static void BootyTwerking(CombatContext context)
        {
            context.Append("Combat.DriderIncubus.BootyTwerking.TheGoblinGives".Localized());

            Drider(context).Lust += 5;
        }

        // 原版 webNipplechain()（DriderIncubus.hx:363-372）：第一次/之后两种文本，自己欲望 +5。
        private static void WebNipplechain(CombatContext context)
        {
            var drider = Drider(context);

            context.Append("Combat.DriderIncubus.WebNipplechain.EveryTimeShe".Localized());

            if (!drider.GoblinWebChain)
            {
                context.Append("Combat.DriderIncubus.WebNipplechain.SheComesAway".Localized());

                drider.GoblinWebChain = true;
            }
            else
            {
                context.Append("Combat.DriderIncubus.WebNipplechain.TheGoblinSlave".Localized());
            }

            drider.Lust += 5;
        }

        // 原版 babyOilMeUp()（DriderIncubus.hx:379-384）：自己欲望 +7、玩家 +7。
        private static void BabyOilMeUp(CombatContext context)
        {
            var player = context.Player;
            var drider = Drider(context);

            context.Append("Combat.DriderIncubus.BabyOilMeUp.DartingIntoThe".Localized());
            context.Append("Combat.DriderIncubus.BabyOilMeUp.SheDancesAnd".Localized());

            drider.Lust += 7;

            player.TakeLustDamage(7, true);
        }

        // 原版 freeGoblin()（DriderIncubus.hx:386-394）：放走哥布林（由 Lethice's Keep 场景的按钮调用）。
        // 场景未移植，所以现在没有调用点；原版末尾的 game.combat.startMonsterTurn() 在 C# 的
        // CombatController 里没有对应公开入口，这里只做状态与文本。
        public static void FreeGoblin(CombatContext context)
        {
            var drider = Drider(context);

            context.Clear();

            drider.GoblinFree = true;

            context.Append("Combat.DriderIncubus.FreeGoblin.YouLungeIn".Localized());
            context.Append("Combat.DriderIncubus.FreeGoblin.YouReForced".Localized());
            context.Append("Combat.DriderIncubus.FreeGoblin.TheGreenSlut".Localized());
            context.Append("Combat.DriderIncubus.FreeGoblin.WellMaybeShe".Localized());
        }
    }
}
