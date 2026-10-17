using MonoCoC.Attributes;

using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Lake
{
    [Register<CalluScene>("coc:callu")]
    public class CalluScene : Scene
    {

        public static string FaerieHairColor = "";

        public static string FaerieOutfit = "";

        public static readonly Dictionary<string, string> outfits = new()
{
    ["nude"] = "bare, exposed body",
    ["straps"] = "solid black stockings and leather chest-wraps",
    ["leaf"] = "leafy wrappings",
    ["petal"] = "chest-wrap and tiny skirt of vibrant petals",
};

        public override void Entry()
        {
            CalluSprite();
        }

        public void CalluSprite()
        {

            ShowImage(Player.HasFur() ? "callu" : "callu-nofur");
        }

        public void ExecEncounter()
        {
            ClearOutput();

            CalluSprite();

            CoC.Instance.Flags.Set(GameFlagIds.MetOttergirl, CoC.Instance.Flags.Get(GameFlagIds.MetOttergirl) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.MetOttergirl) == 1)
            {
                OutputText("Scenes.Calluscene.ExecEncounter.ExploresLakeside".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) != 2)
                    OutputText("Scenes.Calluscene.ExecEncounter.CrystalClear".Localized());
                else
                    OutputText("Scenes.Calluscene.ExecEncounter.SlightlyClouded".Localized());

                OutputText("Scenes.Calluscene.ExecEncounter.FishDart".Localized());

                OutputText("Scenes.Calluscene.ExecEncounter.WatchesFish".Localized());

                OutputText("Scenes.Calluscene.ExecEncounter.ApproachesFigure".Localized((noFur ? "smooth and human-like" : "covered with damp, brown fur")));

                ShowImage("callu-intro");

                if (Player.IsChild())
                {
                    OutputText("Scenes.Calluscene.ExecEncounter.StopsFewFeet".Localized());

                    OutputText("Scenes.Calluscene.ExecEncounter.CantHelpFeeling".Localized());

                    OutputText("Scenes.Calluscene.ExecEncounter.RepliesSimply".Localized());

                    OutputText("Scenes.Calluscene.ExecEncounter.FeelingShe".Localized());

                    if (silly)
                        OutputText("Scenes.Calluscene.ExecEncounter.QuickThought".Localized(
                             Player.Mf("him?", "her?")
                        ));

                    OutputText("Scenes.Calluscene.ExecEncounter.SitsNextToHer".Localized());

                    OutputText("Scenes.Calluscene.ExecEncounter.SoftVoiceChimes".Localized());

                    OutputText("Scenes.Calluscene.ExecEncounter.StaysAndWatches".Localized());

                    if (Player.HasCock())
                        if (Player.BiggestCockLength() <= 5)
                            OutputText("Scenes.Calluscene.ExecEncounter.GettingErect".Localized());
                        else
                            OutputText("Scenes.Calluscene.ExecEncounter.CannotHideErection".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Calluscene.ExecEncounter.BeginsGettingWet".Localized());
                    else
                        if (!Player.HasCock() && !Player.HasVagina())
                            OutputText("Scenes.Calluscene.ExecEncounter.FortunatelyNothing".Localized());

                    OutputText("Scenes.Calluscene.ExecEncounter.SentencePeriod".Localized());

                    OutputText("Scenes.Calluscene.ExecEncounter.ReactionNoticed".Localized());

                    OutputText("Scenes.Calluscene.ExecEncounter.WellPrompt".Localized());
                }
                else
                {
                    OutputText("Scenes.Calluscene.ExecEncounter.StopsFewFeetAgain".Localized());

                    OutputText("Scenes.Calluscene.ExecEncounter.EyebrowRaised".Localized());

                    OutputText("Scenes.Calluscene.ExecEncounter.RepliesSimplyAgain".Localized());

                    OutputText("Scenes.Calluscene.ExecEncounter.ThatsIt".Localized());

                    if (silly)
                        OutputText("Scenes.Calluscene.ExecEncounter.NoTragicBackstory".Localized());

                    OutputText("Scenes.Calluscene.ExecEncounter.DontEvenKnow".Localized());

                    OutputText("Scenes.Calluscene.ExecEncounter.SoftVoiceChimesAgain".Localized());

                    OutputText("Scenes.Calluscene.ExecEncounter.HowAboutIt".Localized());
                }
            }
            else
            {
                ShowImage("callu-repeat");

                if (Player.IsChild())
                {
                    OutputText("Scenes.Calluscene.ExecEncounter.ExplorationsLead".Localized());

                    OutputText("Scenes.Calluscene.ExecEncounter.SitsNextHerAgain".Localized());

                    OutputText("Scenes.Calluscene.ExecEncounter.WellAreYouAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Calluscene.ExecEncounter.ExplorationsLeadAgain".Localized());

                    OutputText("Scenes.Calluscene.ExecEncounter.SitsNextHerThird".Localized());

                    OutputText("Scenes.Calluscene.ExecEncounter.WellAreYouAgain".Localized());
                }
            }

            ClearButtons();

            AddButton(0, "Common.Button.FuckHer".Localized(), null);

            AddButton(1, "Common.Button.Facesitting".Localized(), null);

            if (Player.Lust >= 33)
            {
                if (Player.HasCock())
                    if (Player.ShortestCockLength() > 48)
                        OutputText("Scenes.Calluscene.ExecEncounter.UnfortunatelyNot".Localized());
                    else
                        AddButton(0, "Common.Button.FuckHer".Localized(), OttergirlLikesDongs);

                AddButton(1, "Common.Button.Facesitting".Localized(), OttersForGals);
            }
            else
                OutputText("Scenes.Calluscene.ExecEncounter.NotArousedEnough".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MetOttergirl) > 1)
            {
                AddButton(2, "Common.Button.GetFish".Localized(), GetSomeFishYaFatty);

                AddNextButton("Common.Button.JustFish".Localized(), CalluJustFish);
            }

            AddButton(14, "Common.Button.Leave".Localized(), AvoidZeOtterPussy);
        }

        public void OttergirlLikesDongs()
        {
            ClearOutput();

            int temp = 0;

            int x, y;

            if (Player.IsChild())
            {
                ShowImage("callu-sex-fuck");

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.AgreeSlySmile".Localized((noFur ? "" : " amongst the sparse brown fur")));

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.CantHelpMove".Localized(
                     Player.Mf("gentleman", "lady")
                ));

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.EagerlyStrips".Localized());

                y = -1;

                temp = 0;

                while (temp < Player.CockTotal())
                {
                    if (Player.Cocks[temp].Length < 48)
                        if (y < 0)
                            y = temp;
                        else
                            if (Player.Cocks[temp].Length > Player.Cocks[y].Length)
                                y = temp;

                    temp += 1;
                }

                if (y < 0)
                    y = Player.SmallestCockIndex();

                x = y;

                if (Player.Cocks[x].Length < 6)
                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.TinyCockRemark".Localized());
                else
                    if (Player.Cocks[x].Length < 10)
                        OutputText("Scenes.Calluscene.OttergirlLikesDongs.CockRemark".Localized());
                    else
                        if (Player.Cocks[x].Length < 24)
                            OutputText("Scenes.Calluscene.OttergirlLikesDongs.OversizedCockRemark".Localized());
                        else
                            OutputText("Scenes.Calluscene.OttergirlLikesDongs.EyesGoingWide".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.StretchesAcross".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.SlenderFingers".Localized(
                     Player.CockHead(x)
                ));

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.HeyHoneyLickin".Localized());

                if (Player.Cocks[x].Length < 6)
                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.LilPecker".Localized());
                else
                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.BadBoyAgain".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.SlipRightIn".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.TongueLipsDance".Localized(
                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.CloseAttention".Localized());

                if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.ToYourFlare".Localized());
                else
                    if (Player.HasKnot(x))
                        OutputText("Scenes.Calluscene.OttergirlLikesDongs.ToTheBase".Localized());
                    else
                        if (Player.Cocks[x].Id == BodyPartTypes.Demon)
                            OutputText("Scenes.Calluscene.OttergirlLikesDongs.DemonicNodules".Localized());
                        else
                            if (Player.Cocks[x].Id == BodyPartTypes.Anemone)
                                OutputText("Scenes.Calluscene.OttergirlLikesDongs.WrigglingTentacles".Localized());
                            else
                                OutputText("Scenes.Calluscene.OttergirlLikesDongs.SensitiveSpot".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.LavishingAttention".Localized());

                if (Player.Balls > 0 && Player.HasVagina())
                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.BallsAndCuntAgain".Localized());
                else
                    if (Player.Balls > 0)
                        OutputText("Scenes.Calluscene.OttergirlLikesDongs.BallsWord".Localized());
                    else
                        if (Player.HasVagina())
                            OutputText("Scenes.Calluscene.OttergirlLikesDongs.CuntWord".Localized());
                        else
                            OutputText("Scenes.Calluscene.OttergirlLikesDongs.AssWord".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.DribbleDownForm".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.SeveralMinutes".Localized(
                     Player.CockDescript(x),

                     Player.CockHead(x)
                ));

                if (Player.Cocks[x].Length < 10)
                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.ComesToRest".Localized());
                else
                    if (Player.Cocks[x].Length < 24)
                        OutputText("Scenes.Calluscene.OttergirlLikesDongs.DevouringEntireCock".Localized());
                    else
                        OutputText("Scenes.Calluscene.OttergirlLikesDongs.ExcruciatingProcess".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.SentencePeriod".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.WithYourEntire".Localized());

                if (Player.Cocks[x].Thickness >= 3)
                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.ImpressiveWord".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.GirthSettlesDown".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.StillFeel".Localized(
                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.NothingElseToDo".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.HighPitchedSound".Localized(
                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.DoesntHaveToSuffer".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.RearSwingsSpins".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.BodyArches".Localized());

                if (Player.CumQ() >= 250)
                {
                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.SoMuchOfIt".Localized());

                    if (Player.CumQ() < 500)
                        OutputText("Scenes.Calluscene.OttergirlLikesDongs.BeginsToDribble".Localized());
                    else
                        if (Player.CumQ() < 1000)
                            OutputText("Scenes.Calluscene.OttergirlLikesDongs.BeginsToSpray".Localized());
                        else
                            if (Player.CumQ() < 2000)
                                OutputText("Scenes.Calluscene.OttergirlLikesDongs.StomachVisibly".Localized());
                            else
                                OutputText("Scenes.Calluscene.OttergirlLikesDongs.StomachInflates".Localized());

                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.SentencePeriodAgain".Localized());
                }

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.WombGreedily".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.CalluPicks".Localized());

                if (Player.CumQ() >= 2000)
                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.DespiteBelly".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.ConstantCadence".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.ThreeMoreMinutes".Localized());

                if (Player.HasKnot(x))
                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.HeldByKnot".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.SlipsBikini".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.PullsFromBag".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.TakesMinuteRecover".Localized());
            }
            else
            {
                OutputText("Scenes.Calluscene.OttergirlLikesDongs.AgreeSlySmileAgain".Localized((noFur ? "" : " amongst the sparse brown fur")));

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.MovesToGrope".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.StrangelyStrips".Localized());

                y = -1;

                temp = 0;

                while (temp < Player.CockTotal())
                {
                    if (Player.Cocks[temp].Length < 48)
                        if (y < 0)
                            y = temp;
                        else
                            if (Player.Cocks[temp].Length > Player.Cocks[y].Length)
                                y = temp;

                    temp += 1;
                }

                if (y < 0)
                    y = Player.SmallestCockIndex();

                x = y;

                if (Player.Cocks[x].Length < 6)
                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.TinyCockRemarkAgain".Localized());
                else
                    if (Player.Cocks[x].Length < 10)
                        OutputText("Scenes.Calluscene.OttergirlLikesDongs.CockRemarkAgain".Localized());
                    else
                        if (Player.Cocks[x].Length < 24)
                            OutputText("Scenes.Calluscene.OttergirlLikesDongs.OversizedCockRemarkAgain".Localized());
                        else
                            OutputText("Scenes.Calluscene.OttergirlLikesDongs.EyesGoingWideAgain".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.StretchesAcrossAgain".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.SlenderFingersAgain".Localized(
                     Player.CockHead(x)
                ));

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.HeyDarlinLickin".Localized());

                if (Player.Cocks[x].Length < 6)
                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.LittleWannaBeCock".Localized());
                else
                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.BadBoyAgain".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.SlipRightInAgain".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.TongueLipsDanceAgain".Localized(
                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.CloseAttentionAgain".Localized());

                if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.ToYourFlareAgain".Localized());
                else
                    if (Player.HasKnot(x))
                        OutputText("Scenes.Calluscene.OttergirlLikesDongs.ToTheBaseAgain".Localized());
                    else
                        if (Player.Cocks[x].Id == BodyPartTypes.Demon)
                            OutputText("Scenes.Calluscene.OttergirlLikesDongs.DemonicNodulesAgain".Localized());
                        else
                            if (Player.Cocks[x].Id == BodyPartTypes.Anemone)
                                OutputText("Scenes.Calluscene.OttergirlLikesDongs.WrigglingTentaclesAgain".Localized());
                            else
                                OutputText("Scenes.Calluscene.OttergirlLikesDongs.SensitiveSpotAgain".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.LavishingAttentionAgain".Localized());

                if (Player.Balls > 0 && Player.HasVagina())
                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.BallsAndCuntAgain".Localized());
                else
                    if (Player.Balls > 0)
                        OutputText("Scenes.Calluscene.OttergirlLikesDongs.BallsWordAgain".Localized());
                    else
                        if (Player.HasVagina())
                            OutputText("Scenes.Calluscene.OttergirlLikesDongs.CuntWordAgain".Localized());
                        else
                            OutputText("Scenes.Calluscene.OttergirlLikesDongs.AssWordAgain".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.DribbleDownFormAgain".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.SeveralMinutesAgain".Localized(
                     Player.CockDescript(x),

                     Player.CockHead(x)
                ));

                if (Player.Cocks[x].Length < 10)
                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.ComesToRestAgain".Localized());
                else
                    if (Player.Cocks[x].Length < 24)
                        OutputText("Scenes.Calluscene.OttergirlLikesDongs.DevouringEntireCockAgain".Localized());
                    else
                        OutputText("Scenes.Calluscene.OttergirlLikesDongs.ExcruciatingProcessAgain".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.PeriodBreak".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.WithYourEntireAgain".Localized());

                if (Player.Cocks[x].Thickness >= 3)
                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.ImpressiveWordAgain".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.GirthSettlesDownAgain".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.StillFeelAgain".Localized(
                     Player.CockDescript(x)
                ));

                if (Player.Cocks[x].Id == BodyPartTypes.Harpy)
                    ShowImage("callu-sex-avian");
                else
                    if (Player.Cocks[x].Id == BodyPartTypes.Kangaroo)
                        ShowImage("callu-sex-kangaroo");
                    else
                        if (Player.Cocks[x].Id == new Identifier("coc", "tentacle"))
                            ShowImage("callu-sex-tentacle");
                        else
                            if (Player.Cocks[x].Id == BodyPartTypes.Anemone)
                                ShowImage("callu-sex-anemone");
                            else
                                if (Player.Cocks[x].Id == BodyPartTypes.Echidna)
                                    ShowImage("callu-sex-echidna");
                                else
                                    if (Player.Cocks[x].Id == BodyPartTypes.Lizard)
                                        ShowImage("callu-sex-reptile");
                                    else
                                        if (Player.Cocks[x].Id == BodyPartTypes.Dragon)
                                            ShowImage("callu-sex-dragon");
                                        else
                                            if (Player.Cocks[x].Id == BodyPartTypes.Demon)
                                                ShowImage("callu-sex-demon");
                                            else
                                                if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                                                    ShowImage("callu-sex-horse");
                                                else
                                                    if (Player.Cocks[x].Id == BodyPartTypes.Human)
                                                        ShowImage("callu-sex-human");
                                                    else
                                                        if (Player.Cocks[x].Id == BodyPartTypes.Human)
                                                            ShowImage("callu-sex-human");
                                                        else
                                                            if (Player.Cocks[x].Id == BodyPartTypes.Cat)
                                                                ShowImage("callu-sex-cat");
                                                            else
                                                                if (new[] { BodyPartTypes.Dog, BodyPartTypes.Wolf, BodyPartTypes.Fox }.Contains(Player.Cocks[x].Id))
                                                                    ShowImage("callu-sex-dog");
                                                                else
                                                                    ShowImage("callu-sex-fuck");

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.NothingElseAgain".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.HighPitchedSoundAgain".Localized(
                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.DoesntSufferAgain".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.RearSwingsSpinsAgain".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.BodyArchesAgain".Localized());

                if (Player.CumQ() >= 250)
                {
                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.SoMuchAgain".Localized());

                    if (Player.CumQ() < 500)
                        OutputText("Scenes.Calluscene.OttergirlLikesDongs.BeginsToDribbleAgain".Localized());
                    else
                        if (Player.CumQ() < 1000)
                            OutputText("Scenes.Calluscene.OttergirlLikesDongs.BeginsToSprayAgain".Localized());
                        else
                            if (Player.CumQ() < 2000)
                                OutputText("Scenes.Calluscene.OttergirlLikesDongs.StomachVisiblyAgain".Localized());
                            else
                                OutputText("Scenes.Calluscene.OttergirlLikesDongs.StomachInflatesAgain".Localized());

                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.SentencePeriodThird".Localized());
                }

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.WombGreedilyAgain".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.CalluPicksAgain".Localized());

                if (Player.CumQ() >= 2000)
                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.DespiteBellyAgain".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.ConstantCadenceAgain".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.ThreeMoreMinutesAgain".Localized());

                if (Player.HasKnot(x))
                    OutputText("Scenes.Calluscene.OttergirlLikesDongs.HeldByKnotAgain".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.SlipsBikiniAgain".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.PullsFromBagAgain".Localized());

                OutputText("Scenes.Calluscene.OttergirlLikesDongs.TakesMinuteRecoverAgain".Localized());
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            DoNext(() => GainFishFillet());
        }

        public void OttersForGals()
        {
            ClearOutput();

            if (Player.IsChild())
            {
                ShowImage("callu-sex-facesit");

                OutputText("Scenes.Calluscene.OttersForGals.AgreeSlySmile".Localized((noFur ? "" : " amongst the sparse brown fur")));

                OutputText("Scenes.Calluscene.OttersForGals.MovesToGrope".Localized(
                     Player.Mf("gentleman", "lady")
                ));

                OutputText("Scenes.Calluscene.OttersForGals.EagerlyStrips".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.LeansDown".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Calluscene.OttersForGals.CunnyWord".Localized());
                else
                    OutputText("Scenes.Calluscene.OttersForGals.PuckerWord".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.FeelCompelled".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.TongueLipsDance".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Calluscene.OttersForGals.ClitWord".Localized());
                else
                    OutputText("Scenes.Calluscene.OttersForGals.UnnaturalCrotch".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.DownToPucker".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.TastyMinutes".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.KnowFullWell".Localized());

                if (Player.HasLongTongue())
                    OutputText("Scenes.Calluscene.OttersForGals.ExtendsTongue".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.BetterPartHour".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.LookingDown".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.ShiversAsDives".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Calluscene.OttersForGals.MoistCunny".Localized());
                else
                    OutputText("Scenes.Calluscene.OttersForGals.BumholeWord".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.LipsLikeMagic".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.GrabsHold".Localized());

                if (Player.HasFuckableNipples())
                    OutputText("Scenes.Calluscene.OttersForGals.SlipsFinger".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.TenderTongue".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Calluscene.OttersForGals.LipService".Localized());
                else
                    OutputText("Scenes.Calluscene.OttersForGals.CaveDive".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.CantHoldOut".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.PleasureBursts".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Calluscene.OttersForGals.SnatchClenches".Localized());
                else
                    OutputText("Scenes.Calluscene.OttersForGals.AssholeSpasms".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.BlissCollapse".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.SeveralOrgasmsLater".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.BlondeOtterSets".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.TakesMinuteRecover".Localized());
            }
            else
            {
                OutputText("Scenes.Calluscene.OttersForGals.AgreeSlySmileAgain".Localized((noFur ? "" : " amongst the sparse brown fur")));

                OutputText("Scenes.Calluscene.OttersForGals.MovesToGropeAgain".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.StrangelyStrips".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.LeansDownAgain".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Calluscene.OttersForGals.FoldsWord".Localized());
                else
                    OutputText("Scenes.Calluscene.OttersForGals.PuckerWordAgain".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.FeelCompelledAgain".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.TongueLipsDanceAgain".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Calluscene.OttersForGals.ClitWordAgain".Localized());
                else
                    OutputText("Scenes.Calluscene.OttersForGals.UnnaturalCrotchAgain".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.DownToPuckerAgain".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.TastyMinutesAgain".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.KnowFullWellAgain".Localized());

                ShowImage("callu-sex-facesit");

                if (Player.HasLongTongue())
                    OutputText("Scenes.Calluscene.OttersForGals.ExtendsTongueAgain".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.BetterPartHourAgain".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.GrinningDown".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.StraddlesFace".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Calluscene.OttersForGals.QuiveringQuim".Localized());
                else
                    OutputText("Scenes.Calluscene.OttersForGals.RumpWord".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.LipsLikeMagicAgain".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.GrabsHoldAgain".Localized());

                if (Player.HasFuckableNipples())
                    OutputText("Scenes.Calluscene.OttersForGals.SlipsFingerAgain".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.TenderTongueAgain".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Calluscene.OttersForGals.LipServiceAgain".Localized());
                else
                    OutputText("Scenes.Calluscene.OttersForGals.CaveDiveAgain".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.CantHoldOutAgain".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.PleasureBurstsAgain".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Calluscene.OttersForGals.SnatchClenchesAgain".Localized());
                else
                    OutputText("Scenes.Calluscene.OttersForGals.AssholeSpasmsAgain".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.BlissCollapseAgain".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.SeveralOrgasmsLaterAgain".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.BlondeOtterSetsAgain".Localized());

                OutputText("Scenes.Calluscene.OttersForGals.TakesSeveralMinutes".Localized());
            }

            Player.Orgasm("VaginalAnal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            DoNext(() => GainFishFillet());
        }

        public void AvoidZeOtterPussy()
        {
            ClearOutput();

            ShowImage("callu-avoid");

            OutputText("Scenes.Calluscene.AvoidZeOtterPussy.ShakesHeadExplains".Localized());

            OutputText("Scenes.Calluscene.AvoidZeOtterPussy.SitInSilence".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void GetSomeFishYaFatty()
        {
            ClearOutput();

            ShowImage("callu-fishing");

            OutputText("Scenes.Calluscene.GetSomeFishYaFatty.TellsShesHungry".Localized());

            OutputText("Scenes.Calluscene.GetSomeFishYaFatty.RetrievesFish".Localized());

            OutputText("Scenes.Calluscene.GetSomeFishYaFatty.ThanksFood".Localized());

            DoNext(() => GainFishFillet());
        }

        public void GainFishFillet()
        {
            ClearOutput();

            ShowImage("item-fishFillet");

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.FishFillet), 1);

ReturnToCamp(1);
        }

        public void CalluJustFish()
        {
            ClearOutput();

            OutputText("Scenes.Calluscene.CalluJustFish.AsksIfFishing".Localized());

            OutputText("Scenes.Calluscene.CalluJustFish.EyebrowCocked".Localized());

            OutputText("Scenes.Calluscene.CalluJustFish.JustFish".Localized());

            OutputText("Scenes.Calluscene.CalluJustFish.LetsRodDown".Localized());

            OutputText("Scenes.Calluscene.CalluJustFish.TrueToWord".Localized());

            OutputText("Scenes.Calluscene.CalluJustFish.TossesPole".Localized());

            OutputText("Scenes.Calluscene.CalluJustFish.FishesAround".Localized());

            OutputText("Scenes.Calluscene.CalluJustFish.DoesAsInstructed".Localized());

            OutputText("Scenes.Calluscene.CalluJustFish.NodsShifts".Localized());

            OutputText("Scenes.Calluscene.CalluJustFish.NothingToDo".Localized());

            OutputText("Scenes.Calluscene.CalluJustFish.NotWorstSeen".Localized());

            OutputText("Scenes.Calluscene.CalluJustFish.TakesCompliment".Localized());

            OutputText("Scenes.Calluscene.CalluJustFish.UntilBite".Localized());

            OutputText("Scenes.Calluscene.CalluJustFish.AsksWithDegree".Localized());

            OutputText("Scenes.Calluscene.CalluJustFish.DoesAndFollows".Localized());

            OutputText("Scenes.Calluscene.CalluJustFish.WithSplash".Localized());

            OutputText("Scenes.Calluscene.CalluJustFish.LooksUpAt".Localized());

            OutputText("Scenes.Calluscene.CalluJustFish.GlancesSky".Localized());

            OutputText("Scenes.Calluscene.CalluJustFish.FinishedFillet".Localized());

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.FishFillet), 1);

ReturnToCamp(1);
        }

    }
}