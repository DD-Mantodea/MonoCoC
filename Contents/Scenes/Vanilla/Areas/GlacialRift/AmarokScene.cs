using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.GlacialRift
{
    [Register<AmarokScene>("coc:amarok")]
    public class AmarokScene : Scene
    {

        public override void Entry()
        {

        }

        public void AmarokChowTime()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AmarokLosses, CoC.Instance.Flags.Get(GameFlagIds.AmarokLosses) + 1);

            if (Player.Health <= 0)
            {
                OutputText("Scenes.Amarokscene.AmarokChowTime.LegsGiveOut".Localized());

                AmarokFlagCheck();
            }
            else
            {
                OutputText("Scenes.Amarokscene.AmarokChowTime.CollapsesSnow".Localized());

                AmarokFlagCheck();
            }
        }

        public void AmarokFlagCheck()
        {

            if (Player.HasStatusEffect(StatusEffectIds.Infested))
                AmarokNoEat();
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AmarokLosses) >= 5)
                    AmarokBadEnd();
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmarokLosses) < 5)
                        AmarokChompo();
        }

        public void AmarokLustDefeatFemaleAdult()
        {
            ClearOutput();

            OutputText("Scenes.Amarokscene.AmarokLustDefeatFemaleAdult.TackledOver".Localized());

            OutputText("Scenes.Amarokscene.AmarokLustDefeatFemaleAdult.NoMatterScramble".Localized());

            Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true);

            OutputText("Scenes.Amarokscene.AmarokLustDefeatFemaleAdult.CanineMember".Localized());

            OutputText("Scenes.Amarokscene.AmarokLustDefeatFemaleAdult.PicksPace".Localized());

            OutputText("Scenes.Amarokscene.AmarokLustDefeatFemaleAdult.WolfShifts".Localized());

            if (Player.Vaginas[0].Looseness < 4)
                OutputText("Scenes.Amarokscene.AmarokLustDefeatFemaleAdult.CockThrobs".Localized((Player.Vaginas[0].Wetness < 3 ? "squirting" : "splattering")));
            else
                OutputText("Scenes.Amarokscene.AmarokLustDefeatFemaleAdult.KnotSlides".Localized((Player.Vaginas[0].Wetness < 3 ? " slick " : " copious ")));

            OutputText("Scenes.Amarokscene.AmarokLustDefeatFemaleAdult.WolfRagged".Localized());

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();
        }

        public void AmarokLustDefeatFemaleChild()
        {
            ClearOutput();

            OutputText("Scenes.Amarokscene.AmarokLustDefeatFemaleChild.FeelingDazed".Localized());

            OutputText("Scenes.Amarokscene.AmarokLustDefeatFemaleChild.PadsCircles".Localized(
                 Player.ClothedOrNakedLower(" You hear the harsh tearing noise of your [armor] being removed from your body and", " You feel ")
            ));

            OutputText("Scenes.Amarokscene.AmarokLustDefeatFemaleChild.StaysStill".Localized());

            OutputText("Scenes.Amarokscene.AmarokLustDefeatFemaleChild.MouthOpen".Localized());

            OutputText("Scenes.Amarokscene.AmarokLustDefeatFemaleChild.TinglyFeeling".Localized());

            OutputText("Scenes.Amarokscene.AmarokLustDefeatFemaleChild.UrgencyAway".Localized());

            OutputText("Scenes.Amarokscene.AmarokLustDefeatFemaleChild.FinishedRamming".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 30f));

            CombatScene.CleanupAfterCombat();
        }

        public void AmarokLustDefeatMaleAdult()
        {
            ClearOutput();

            OutputText("Scenes.Amarokscene.AmarokLustDefeatMaleAdult.BucklesOver".Localized(
                 Player.ClothedOrNakedLower(" the wolf latches at the bottom of your [armor], rending them from your body.", " you hear the sound of his sniffing by your back.")
            ));

            OutputText("Scenes.Amarokscene.AmarokLustDefeatMaleAdult.ColdSnout".Localized());

            OutputText("Scenes.Amarokscene.AmarokLustDefeatMaleAdult.ClenchesTeeth".Localized());

            OutputText("Scenes.Amarokscene.AmarokLustDefeatMaleAdult.SheerWeight".Localized());

            OutputText("Scenes.Amarokscene.AmarokLustDefeatMaleAdult.FinallyGotten".Localized());

            if (Player.Anal.Looseness <= 3)
                OutputText("Scenes.Amarokscene.AmarokLustDefeatMaleAdult.CringeGroan".Localized());
            else
                OutputText("Scenes.Amarokscene.AmarokLustDefeatMaleAdult.BreathesDeeply".Localized());

            OutputText("Scenes.Amarokscene.AmarokLustDefeatMaleAdult.GrowlingKnots".Localized());

            OutputText("Scenes.Amarokscene.AmarokLustDefeatMaleAdult.SeveralThrusts".Localized());

            if (Player.CumQ() <= 150)
                OutputText("Scenes.Amarokscene.AmarokLustDefeatMaleAdult.WhiteBackdrop".Localized());
            else
                OutputText("Scenes.Amarokscene.AmarokLustDefeatMaleAdult.CumVisible".Localized());

            OutputText("Scenes.Amarokscene.AmarokLustDefeatMaleAdult.HumiliationIsnt".Localized());

            Player.Orgasm("Anal");

            Player.ButtChange(20, true);

            CombatScene.CleanupAfterCombat();
        }

        public void AmarokLustDefeatMaleHermChild()
        {
            ClearOutput();

            OutputText("Scenes.Amarokscene.AmarokLustDefeatMaleHermChild.FeelingDazed".Localized());

            OutputText("Scenes.Amarokscene.AmarokLustDefeatMaleHermChild.PadsCircles".Localized(
                 Player.ClothedOrNakedLower(" You hear the harsh tearing noise of your [armor] being removed from your body and", " You feel ")
            ));

            OutputText("Scenes.Amarokscene.AmarokLustDefeatMaleHermChild.StaysStill".Localized());

            OutputText("Scenes.Amarokscene.AmarokLustDefeatMaleHermChild.MouthOpen".Localized());

            OutputText("Scenes.Amarokscene.AmarokLustDefeatMaleHermChild.TinglyFeeling".Localized());

            OutputText("Scenes.Amarokscene.AmarokLustDefeatMaleHermChild.UrgencyAway".Localized());

            OutputText("Scenes.Amarokscene.AmarokLustDefeatMaleHermChild.FinishedRamming".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 30f));

            CombatScene.CleanupAfterCombat();
        }

        public void AmarokVictoryMale()
        {
            ClearOutput();

            OutputText("Scenes.Amarokscene.AmarokVictoryMale.AdmitsDefeat".Localized());

            OutputText("Scenes.Amarokscene.AmarokVictoryMale.HoweverDecide".Localized(
                 Player.ClothedOrNakedLower(" pressing against the inside of your [armor] ", " visibly hard and throbbing ")
            ));

            OutputText("Scenes.Amarokscene.AmarokVictoryMale.GivesAlpha".Localized());

            OutputText("Scenes.Amarokscene.AmarokVictoryMale.LaysHeadBack".Localized());

            if (Player.BiggestCockLength() < 15)
                OutputText("Scenes.Amarokscene.AmarokVictoryMale.TakesLength".Localized());
            else
                OutputText("Scenes.Amarokscene.AmarokVictoryMale.TakesAsMuch".Localized());

            OutputText("Scenes.Amarokscene.AmarokVictoryMale.StomachSinks".Localized());

            if (Player.BiggestCockLength() < 4)
                OutputText("Scenes.Amarokscene.AmarokVictoryMale.SinksCock".Localized());
            else
                if (Player.BiggestCockLength() < 10)
                    OutputText("Scenes.Amarokscene.AmarokVictoryMale.BitesLips".Localized());
                else
                    OutputText("Scenes.Amarokscene.AmarokVictoryMale.SinksTip".Localized());

            OutputText("Scenes.Amarokscene.AmarokVictoryMale.GripsFur".Localized());

            OutputText("Scenes.Amarokscene.AmarokVictoryMale.DoggyStyle".Localized());

            OutputText("Scenes.Amarokscene.AmarokVictoryMale.AssSqueezes".Localized((Player.Balls > 0 ? " your [balls] tightening" : " your taint tensing")));

            OutputText("Scenes.Amarokscene.AmarokVictoryMale.ThrustsIn".Localized());

            if (Player.CumQ() < 300)
                OutputText("Scenes.Amarokscene.AmarokVictoryMale.GrabsPushes".Localized());
            else
                OutputText("Scenes.Amarokscene.AmarokVictoryMale.GrabsPushesAgain".Localized());

            OutputText("Scenes.Amarokscene.AmarokVictoryMale.BothFall".Localized());

            Player.Orgasm("Penis");

            CombatScene.CleanupAfterCombat();
        }

        public void AmarokNoEat()
        {
            ClearOutput();

            OutputText("Scenes.Amarokscene.AmarokNoEat.MuchSurprise".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void AmarokChompo()
        {
            ClearOutput();

            ShowImage("amarok-chompo");

            OutputText("Scenes.Amarokscene.AmarokChompo.SoftBark".Localized());

            OutputText("Scenes.Amarokscene.AmarokChompo.AwakensLater".Localized());

            OutputText("Scenes.Amarokscene.AmarokChompo.SpotsArmor".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void AmarokBadEnd()
        {
            ClearOutput();

            ShowImage("badend-amarok");

            OutputText("Scenes.Amarokscene.AmarokBadEnd.BeenHereBefore".Localized());

            OutputText("Scenes.Amarokscene.AmarokBadEnd.OtherPlans".Localized());

            OutputText("Scenes.Amarokscene.AmarokBadEnd.WrithesGrasp".Localized(
                Player.IsGoo() ? "core throbs frantically in your chest" : "blood starts to pound in your ears",

                Player.IsGoo() ? "wriggle out from" : "get " + (Player.HasTailInsteadOfLegs() ? "your tail" : "a foot"),

                Player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().LegCount > 1 ? "kick" : "knock",

                Player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().LegCount > 2 ? "roll your legs up" : "manage to move your body"
            ));

            OutputText("Scenes.Amarokscene.AmarokBadEnd.StopsThinking".Localized());

            OutputText("Scenes.Amarokscene.AmarokBadEnd.LinesPain".Localized(
                 Player.ArmorName
            ));

            GameOver();
        }

        public void AmarokBlowjob()
        {
            ClearOutput();

            OutputText("Scenes.Amarokscene.AmarokBlowjob.FeelsGood".Localized());

            OutputText("Scenes.Amarokscene.AmarokBlowjob.MuskyScent".Localized());

            OutputText("Scenes.Amarokscene.AmarokBlowjob.HeadSinks".Localized());

            OutputText("Scenes.Amarokscene.AmarokBlowjob.ConsciousnessFades".Localized());

            Player.SlimeFeed();

            Player.RefillHunger(30);

            CombatScene.CleanupAfterCombat();
        }

        public void WinAgainstAmarok(bool hpVictory)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AmarokLosses, CoC.Instance.Flags.Get(GameFlagIds.AmarokLosses) - 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.AmarokLosses) < 0)
                CoC.Instance.Flags.Set(GameFlagIds.AmarokLosses, 0);

            ClearButtons();

            OutputText("Scenes.Amarokscene.WinAgainstAmarok.CollapsesUnable".Localized());

            if (Player.Lust >= 33)
                OutputText("Scenes.Amarokscene.WinAgainstAmarok.HaventDealt".Localized());

            AddButton(0, "Common.Button.PenetrateHim".Localized(), AmarokVictoryMale);

            AddButton(1, "Scenes.Dullahan.AskDullSex.ButtonBlowjob".Localized(), AmarokBlowjob);

            AddNextButton("Common.Button.GetMounted".Localized(), GetMounted);

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void AmarokFacefuck()
        {
            ClearOutput();

            OutputText("Scenes.Amarokscene.AmarokFacefuck.CompletelyPowerless".Localized());

            OutputText("Scenes.Amarokscene.AmarokFacefuck.ConfusionShort".Localized());

            OutputText("Scenes.Amarokscene.AmarokFacefuck.MenacingGrowl".Localized());

            OutputText("Scenes.Amarokscene.AmarokFacefuck.KnowsWhatWants".Localized());

            OutputText("Scenes.Amarokscene.AmarokFacefuck.StruggleKeep".Localized());

            OutputText("Scenes.Amarokscene.AmarokFacefuck.SlamsWithoutWarning".Localized());

            OutputText("Scenes.Amarokscene.AmarokFacefuck.FeelsLikeHours".Localized());

            OutputText("Scenes.Amarokscene.AmarokFacefuck.RegainedAccess".Localized());

            OutputText("Scenes.Amarokscene.AmarokFacefuck.LegLifts".Localized());

            OutputText("Scenes.Amarokscene.AmarokFacefuck.LiquidCools".Localized());

            Player.SlimeFeed();

            Player.RefillHunger(30);

            CombatScene.CleanupAfterCombat();
        }

        public void GetMounted()
        {
            ClearOutput();

            OutputText("Scenes.Amarokscene.GetMounted.QuiveringAmarok".Localized());

            OutputText("Scenes.Amarokscene.GetMounted.BitingCold".Localized());

            OutputText("Scenes.Amarokscene.GetMounted.BeforeLose".Localized());

            Player.CuntChange(12, true, true, false);

            OutputText("Scenes.Amarokscene.GetMounted.FillsDoggy".Localized());

            OutputText("Scenes.Amarokscene.GetMounted.GruntsForces".Localized());

            OutputText("Scenes.Amarokscene.GetMounted.NowImmobile".Localized());

            OutputText("Scenes.Amarokscene.GetMounted.TimeBlur".Localized());

            Player.Orgasm("Vaginal");

            Player.SlimeFeed();

            CombatScene.CleanupAfterCombat();
        }

    }
}