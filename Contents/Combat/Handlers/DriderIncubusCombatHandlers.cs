using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using System.Linq;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class DriderIncubusCombatHandlers
    {
        public static DriderIncubusEnemy Drider(CombatContext context) => (DriderIncubusEnemy)context.Enemy;

        public static float Ngp(Player player) => 1f + player.NewGamePlusMod() * 0.3f;

        public static int Rand(CombatContext context, float max) => (int)(context.Random.NextDouble() * max);

        [Register("coc:drider_incubus_round")]
        public static void Round(CombatContext context, CombatAbilityDefinition ability)
        {
            var drider = Drider(context);

            drider.CombatRound += 1;

            if (drider.LustPercent < 65 && drider.HpPercent < 33)
                GainHpAndLust(context);
            else if (drider.LustPercent >= 65 && drider.HpPercent >= 33)
                DropHpAndLust(context);

            if (context.Random.Next(100) > drider.Lust + 10f)
            {
                SpearStrike(context);

                context.AppendLog("Combat.DriderIncubus.PerformCombatAction.Text".Localized());

                PerformPhysicalAttack(context);
            }
            else
                PerformLustAttack(context);

            if (drider.GoblinFree)
            {
                context.AppendLog("Combat.DriderIncubus.PerformCombatAction.Text".Localized());

                GoblinAI(context);
            }
        }

        public static void GainHpAndLust(CombatContext context)
        {
            var drider = Drider(context);

            drider.HealthChange(drider.MaxHealth * 0.1f);

            drider.Lust += 8;

            context.AppendLog(drider.HpGains switch
            {
                0 => "Combat.DriderIncubus.GainHpAndLust.Text".Localized(),

                1 => "Combat.DriderIncubus.GainHpAndLust.Text".Localized(),

                2 => "Combat.DriderIncubus.GainHpAndLust.SoStubborn".Localized(),

                _ => "Combat.DriderIncubus.GainHpAndLust.WhyWontYouGiveIn".Localized()
            });

            drider.HpGains += 1;

            context.AppendLog("Combat.DriderIncubus.GainHpAndLust.TheDemonGestures".Localized());

            if (drider.LustPercent > 65)
            {
                context.AppendLog(drider.GoblinFree

                    ? "Combat.DriderIncubus.GainHpAndLust.HisDickIs".Localized()

                    : "Combat.DriderIncubus.GainHpAndLust.HisBallsAre".Localized());

                context.AppendLog("Combat.DriderIncubus.GainHpAndLust.YouDoubtHe".Localized());
            }

            context.AppendLog("Combat.DriderIncubus.GainHpAndLust.ParagraphBreak".Localized());
        }

        public static void DropHpAndLust(CombatContext context)
        {
            var drider = Drider(context);

            drider.HealthChange(-(drider.MaxHealth * 0.08f));

            drider.Lust -= 10;

            context.AppendLog("Combat.DriderIncubus.DropHpAndLust.TheDemonSnarls".Localized());
        }

        public static void SpearStrike(CombatContext context)
        {
            var player = context.Player;

            var drider = Drider(context);

            context.AppendLog("Combat.DriderIncubus.SpearStrike.TheDriderRears".Localized());

            var damage = (drider.Strength + drider.WeaponAttack) * 0.4f * Ngp(player);

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = drider, Defender = player,

                DoDodge = true, DoParry = true, DoBlock = false
            });

            var reason = avoid.DodgeReason;

            if (damage <= 0 || reason is "Speed" or "Flexibility" or "Unhindered")
                context.AppendLog("Combat.DriderIncubus.SpearStrike.YouBarelySlide".Localized());
            else if (reason == "Evade")
                context.AppendLog("Combat.DriderIncubus.SpearStrike.YouEvadeThe".Localized());
            else if (reason == "Misdirection")
                context.AppendLog("Combat.DriderIncubus.SpearStrike.UsingYourSkills".Localized());
            else if (avoid.Outcome == AvoidOutcome.Parry)
                context.AppendLog("Combat.DriderIncubus.SpearStrike.YouDeftlyParry".Localized());
            else
            {
                context.AppendLog("Combat.DriderIncubus.SpearStrike.TheWeaponBites".Localized());

                context.DealDamageToPlayer(damage);
            }
        }

        public static void PerformPhysicalAttack(CombatContext context)
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

            if (drider.Fatigue < drider.MaxFatigue())
                options.Add(() => Bite(context));

            options[context.Random.Next(options.Count)]();
        }

        public static void PerformLustAttack(CombatContext context)
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

        public static void GoblinAI(CombatContext context)
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

        public static void Bite(CombatContext context)
        {
            var player = context.Player;

            var drider = Drider(context);

            drider.Fatigue += 20;

            if (player.HasStatusEffect(StatusEffectIds.Stunned) ||
                (player.Speed <= 1 && player.StatusEffectValue(StatusEffectIds.Web, 0) >= 2))
            {
                ApplyVenom(context);

                context.AppendLog("Combat.DriderIncubus.Bite.TakingHisTime".Localized());

                if (player.Strength <= 25)
                    context.AppendLog("Combat.DriderIncubus.Bite.ItSGetting".Localized());
                else if (player.Strength <= 1)
                    context.AppendLog("Combat.DriderIncubus.Bite.YouDonT".Localized());

                context.AppendLog("Combat.DriderIncubus.Bite.WhenHePulls".Localized());

                context.AppendLog(player.Strength > 25

                    ? "Combat.DriderIncubus.Bite.Text".Localized()

                    : "Combat.DriderIncubus.Bite.Text".Localized());

                return;
            }

            context.AppendLog("Combat.DriderIncubus.Bite.TwistingOverThe".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = drider, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            var reason = avoid.DodgeReason;

            if (reason == "Misdirection")
                context.AppendLog("Combat.DriderIncubus.Bite.YouMisdirectHis".Localized());
            else if (reason == "Evade")
                context.AppendLog("Combat.DriderIncubus.Bite.YouEvadeHis".Localized());
            else if (reason is "Speed" or "Flexibility" or "Unhindered")
                context.AppendLog("Combat.DriderIncubus.Bite.YouAvoidHis".Localized());
            else
            {
                context.AppendLog("Combat.DriderIncubus.Bite.ThoseNeedleLike".Localized());

                context.AppendLog("Combat.DriderIncubus.Bite.HeFlashesA".Localized());

                ApplyVenom(context);
            }
        }

        public static void ApplyVenom(CombatContext context)
        {
            var player = context.Player;

            if (player.HasStatusEffect(DriderIncubusEnemy.DriderIncubusVenomId))
                return;

            player.CreateStatusEffect(DriderIncubusEnemy.DriderIncubusVenomId);

            player.ApplyStats(new DynStat(GameStats.Strength, -5f));

            CombatCalculator.AddCombatBuff(player, GameStats.Strength, -30, "DriderIncubusVenom",

                DriderIncubusEnemy.DriderIncubusVenomId);
        }

        public static void Kick(CombatContext context)
        {
            var player = context.Player;

            var drider = Drider(context);

            context.AppendLog("Combat.DriderIncubus.Kick.WhileYouRe".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = drider, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.DodgeReason == "Misdirection")
            {
                context.AppendLog("Combat.DriderIncubus.Kick.YouTwistOut".Localized());

                return;
            }

            if (avoid.DodgeReason != null)
            {
                context.AppendLog("Combat.DriderIncubus.Kick.YouTwistOut2".Localized());

                return;
            }

            var damage = (drider.Strength + drider.WeaponAttack + 25 - Rand(context, player.Toughness)) * Ngp(player);

            if (damage > 0)
            {
                context.AppendLog("Combat.DriderIncubus.Kick.YouGoFlying".Localized());

                context.DealDamageToPlayer(damage);
            }
            else
                context.AppendLog("Combat.DriderIncubus.Kick.YouSuccessfullyManage".Localized());
        }

        public static void StunningSpear(CombatContext context)
        {
            var player = context.Player;

            var drider = Drider(context);

            context.AppendLog("Combat.DriderIncubus.StunningSpear.TwirlingHisWeapon".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = drider, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            var reason = avoid.DodgeReason;

            if (reason is "Speed" or "Flexibility" or "Unhindered")
                context.AppendLog("Combat.DriderIncubus.StunningSpear.YouDuckIn".Localized());
            else if (reason == "Misdirection")
                context.AppendLog("Combat.DriderIncubus.StunningSpear.YouWereAlready".Localized());
            else if (reason == "Evade")
                context.AppendLog("Combat.DriderIncubus.StunningSpear.YouLeanIn".Localized());
            else
            {
                var damage = (drider.Strength + drider.WeaponAttack - 25 - Rand(context, player.Toughness)) * Ngp(player);

                if (damage > 0)
                {
                    context.AppendLog("Combat.DriderIncubus.StunningSpear.YouDonT".Localized());

                    context.AppendLog("Combat.DriderIncubus.StunningSpear.Bone".Localized());

                    context.AppendLog("Combat.DriderIncubus.StunningSpear.Text".Localized());

                    if (player.Stun(0, 100))
                        context.AppendLog("Combat.DriderIncubus.StunningSpear.YouReLeft".Localized());

                    context.DealDamageToPlayer(damage);
                }

                context.AppendLog("Combat.DriderIncubus.StunningSpear.YouHearThe".Localized());

                context.AppendLog("Combat.DriderIncubus.StunningSpear.Bone".Localized());

                context.AppendLog("Combat.DriderIncubus.StunningSpear.ButYouManage".Localized());
            }
        }

        public static void DoubleStrike(CombatContext context)
        {
            var player = context.Player;

            var drider = Drider(context);

            context.AppendLog("Combat.DriderIncubus.DoubleStrike.HeSkittersForward".Localized());

            var damage = drider.Strength + drider.WeaponAttack + 10 - Rand(context, player.Toughness);

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = drider, Defender = player,

                DoDodge = true, DoParry = true, DoBlock = false
            });

            var reason = avoid.DodgeReason;

            if (damage <= 0 || reason is "Speed" or "Flexibility" or "Unhindered")
                context.AppendLog("Combat.DriderIncubus.DoubleStrike.YouBarelySlide".Localized());
            else if (reason == "Evade")
                context.AppendLog("Combat.DriderIncubus.DoubleStrike.YouEvadeThe".Localized());
            else if (reason == "Misdirection")
                context.AppendLog("Combat.DriderIncubus.DoubleStrike.UsingYourSkills".Localized());
            else if (avoid.Outcome == AvoidOutcome.Parry)
                context.AppendLog("Combat.DriderIncubus.DoubleStrike.YouParryThe".Localized());
            else
            {
                context.AppendLog("Combat.DriderIncubus.DoubleStrike.TheWeaponBites".Localized());

                context.DealDamageToPlayer(damage);
            }
        }

        public static void SpiderMorphWebAttack(CombatContext context)
        {
            var player = context.Player;

            var drider = Drider(context);

            context.AppendLog("Combat.DriderIncubus.SpiderMorphWebAttack.Text".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = drider, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = true
            });

            if (avoid.AttackFailed)
                return;

            if (!player.HasStatusEffect(StatusEffectIds.Web))
            {
                context.AppendLog("Combat.DriderIncubus.SpiderMorphWebAttack.HitFirst".Localized());

                player.CreateStatusEffect(StatusEffectIds.Web, 3);
            }
            else
            {
                context.AppendLog("Combat.DriderIncubus.SpiderMorphWebAttack.HitMore".Localized());

                player.ChangeStatusValue(StatusEffectIds.Web, 0, 1);
            }

            CombatCalculator.AddCombatBuff(player, GameStats.Speed, -25, "web", StatusEffectIds.Web);
        }

        public static void ArouseSpell(CombatContext context)
        {
            var player = context.Player;

            context.AppendLog("Combat.DriderIncubus.ArouseSpell.TheDemonicDrider".Localized());

            context.AppendLog("Combat.DriderIncubus.ArouseSpell.AnInvisibleForce".Localized());

            if (player.Cocks.Count > 0 || player.Vaginas.Count > 0)
            {
                context.AppendLog("Combat.DriderIncubus.ArouseSpell.MakingYour".Localized());

                context.AppendLog(player.Cocks.Count > 0

                    ? "Combat.DriderIncubus.ArouseSpell.Text".Localized()

                    : "Combat.DriderIncubus.ArouseSpell.Text".Localized());

                context.AppendLog("Combat.DriderIncubus.ArouseSpell.AcheToBe".Localized());
            }

            context.AppendLog("Combat.DriderIncubus.ArouseSpell.Dot".Localized());

            player.TakeLustDamage(player.Libido / 10f + player.Corruption / 10f + 15, true);

            context.AppendLog("Combat.DriderIncubus.ArouseSpell.YourBodyRebels".Localized());

            context.AppendLog(player.LustPercent < 100

                ? "Combat.DriderIncubus.ArouseSpell.ButTheEffect".Localized()

                : "Combat.DriderIncubus.ArouseSpell.Dot".Localized());
        }

        public static void TaintedMind(CombatContext context)
        {
            var player = context.Player;

            context.AppendLog("Combat.DriderIncubus.TaintedMind.HeClapsHis".Localized());

            if (player.Corruption <= 33)
                context.AppendLog("Combat.DriderIncubus.TaintedMind.WhatDidHe".Localized());

            player.CreateStatusEffect(DriderIncubusEnemy.TaintedMindId, 4);
        }

        public static void TaintedMindAttackAttempt(CombatContext context)
        {
            context.AppendLog("Combat.DriderIncubus.TaintedMindAttackAttempt.YouReadyAn".Localized());

            context.Player.TakeLustDamage(15, true);
        }

        public static void ConstrictingThoughts(CombatContext context)
        {
            var player = context.Player;

            var drider = Drider(context);

            context.AppendLog("Combat.DriderIncubus.ConstrictingThoughts.TheCorruptedArachnid".Localized());

            context.AppendLog("Combat.DriderIncubus.ConstrictingThoughts.DisjointedEroticThoughts".Localized());

            if (!player.Stun(0, 100))
            {
                drider.SeenResolute = true;

                context.AppendLog("Combat.DriderIncubus.ConstrictingThoughts.YouMarshalYour".Localized());
            }
            else
            {
                context.AppendLog("Combat.DriderIncubus.ConstrictingThoughts.TheIntensityOverwhelms".Localized());

                player.TakeLustDamage(player.Libido / 15f + player.Corruption / 15f + 15, true);
            }
        }

        public static void PurpleHaze(CombatContext context)
        {
            var player = context.Player;

            context.AppendLog("Combat.DriderIncubus.PurpleHaze.TheDriderGestures".Localized());

            if (player.HasPerk(PerkIds.Resolute) || context.Random.NextDouble() * 100 < 66)
            {
                if (player.HasPerk(PerkIds.Resolute))
                    Drider(context).SeenResolute = true;

                context.AppendLog("Combat.DriderIncubus.PurpleHaze.YouFlexYour".Localized());

                return;
            }

            context.AppendLog("Combat.DriderIncubus.PurpleHaze.YouConcentrateTo".Localized());

            player.TakeLustDamage(25, true);

            var duration = 2 + context.Random.Next(2);

            player.CreateStatusEffect(DriderIncubusEnemy.PurpleHazeId, duration);

            player.CreateStatusEffect(StatusEffectIds.Blind, duration);
        }

        public static void GoblinHandjob(CombatContext context)
        {
            context.AppendLog("Combat.DriderIncubus.GoblinHandjob.SomehowTheGoblin".Localized());

            context.AppendLog("Combat.DriderIncubus.GoblinHandjob.UnfortunatelyForHer".Localized());
        }

        public static void GoblinTongueGrapple(CombatContext context)
        {
            var drider = Drider(context);

            context.AppendLog("Combat.DriderIncubus.GoblinTongueGrapple.TheDeterminedGoblin".Localized());

            drider.Lust += 5;

            if (drider.LustPercent <= 100)
                context.AppendLog("Combat.DriderIncubus.GoblinTongueGrapple.TheDriderSkitters".Localized());
            else
                context.AppendLog("Combat.DriderIncubus.GoblinTongueGrapple.TheDriderAttempts".Localized());
        }

        public static void BootyTwerking(CombatContext context)
        {
            context.AppendLog("Combat.DriderIncubus.BootyTwerking.TheGoblinGives".Localized());

            Drider(context).Lust += 5;
        }

        public static void WebNipplechain(CombatContext context)
        {
            var drider = Drider(context);

            context.AppendLog("Combat.DriderIncubus.WebNipplechain.EveryTimeShe".Localized());

            if (!drider.GoblinWebChain)
            {
                context.AppendLog("Combat.DriderIncubus.WebNipplechain.SheComesAway".Localized());

                drider.GoblinWebChain = true;
            }
            else
                context.AppendLog("Combat.DriderIncubus.WebNipplechain.TheGoblinSlave".Localized());

            drider.Lust += 5;
        }

        public static void BabyOilMeUp(CombatContext context)
        {
            var player = context.Player;

            var drider = Drider(context);

            context.AppendLog("Combat.DriderIncubus.BabyOilMeUp.DartingIntoThe".Localized());

            context.AppendLog("Combat.DriderIncubus.BabyOilMeUp.SheDancesAnd".Localized());

            drider.Lust += 7;

            player.TakeLustDamage(7, true);
        }

        public static void FreeGoblin(CombatContext context)
        {
            var drider = Drider(context);

            MonoCoC.UI.Components.Views.GameView.Current.ClearOutput();

            drider.GoblinFree = true;

            context.AppendLog("Combat.DriderIncubus.FreeGoblin.YouLungeIn".Localized());

            context.AppendLog("Combat.DriderIncubus.FreeGoblin.YouReForced".Localized());

            context.AppendLog("Combat.DriderIncubus.FreeGoblin.TheGreenSlut".Localized());

            context.AppendLog("Combat.DriderIncubus.FreeGoblin.WellMaybeShe".Localized());
        }
    }
}