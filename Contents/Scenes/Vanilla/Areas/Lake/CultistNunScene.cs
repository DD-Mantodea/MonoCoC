using MonoCoC.Attributes;

using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Lake
{
    [Register<CultistNunScene>("coc:cultist_nun")]
    public class CultistNunScene : Scene
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
            ExecEncounter();
        }

        public void ExecEncounter()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.MetCultistNun) == 1)
                MeetCultistNunAgain();
            else
                MeetCultistNun();
        }

        public void MeetCultistNun()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.MetCultistNun, 1);

            OutputText("Scenes.Cultistnunscene.MeetCultistNun.LakesShore".Localized());

            OutputText("Scenes.Cultistnunscene.MeetCultistNun.DetailsClear".Localized());

            if (Player.IsReligious())
                OutputText("Scenes.Cultistnunscene.MeetCultistNun.DefinitelyNot".Localized());

            OutputText("Scenes.Cultistnunscene.MeetCultistNun.WalkingMoments".Localized());

            OutputText("Scenes.Cultistnunscene.MeetCultistNun.WorldBeautiful".Localized());

            OutputText("Scenes.Cultistnunscene.MeetCultistNun.ScratchHead".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Marae".Localized(), () => AnswerNun(0));

            AddButton(1, "Common.Button.FalseGods".Localized(), () => AnswerNun(1));

            AddButton(2, "Common.Button.NotReligious".Localized(), () => AnswerNun(2));
        }

        public void MeetCultistNunAgain()
        {
            ClearOutput();

            OutputText("Scenes.Cultistnunscene.MeetCultistNunAgain.LakesShore".Localized());

            OutputText("Scenes.Cultistnunscene.MeetCultistNunAgain.GoodToSeeAgain".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PrayedWithNun) == 1 && CoC.Instance.Flags.Get(GameFlagIds.TookNaughtyHabit) == 0)
                OutputText("Scenes.Cultistnunscene.MeetCultistNunAgain.NodsChest".Localized());

            ClearButtons();

            if ((CoC.Instance.Flags.Get(GameFlagIds.PrayedWithNun) == 1 || CoC.Instance.Flags.Get(GameFlagIds.DefiledNun) == 1) && CoC.Instance.Flags.Get(GameFlagIds.TookNaughtyHabit) == 0)
                AddNextButton("Common.Button.TakeHabit".Localized(), () => InventoryScene.TakeItem(ArmorIds.NaughtyNunsHabit, JoinNunFinish, JoinNunMaybeLater));

            if (CoC.Instance.Flags.Get(GameFlagIds.PrayedWithNun) == 0 || CoC.Instance.Flags.Get(GameFlagIds.DefiledNun) == 0)
                AddNextButton("Common.Button.Pray".Localized(), JoinNun);

            if (CoC.Instance.Flags.Get(GameFlagIds.DefiledNun) == 0 && Player.Corruption > 60 && !Player.IsGenderless())
                AddNextButton("Common.Button.DefileHer".Localized(), DefileHer);

            AddNextButton("Common.Button.No".Localized(), DenyPrayer);
        }

        public void DefileHer()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.DefiledNun, 1);

            OutputText("Scenes.Cultistnunscene.DefileHer.ScoffsPrayers".Localized());

            OutputText("Scenes.Cultistnunscene.DefileHer.ForcefullyPushes".Localized());

            OutputText("Scenes.Cultistnunscene.DefileHer.SeesBetween".Localized());

            OutputText("Scenes.Cultistnunscene.DefileHer.CanTMove".Localized());

            if (Player.HasCock())
                GetDefiledCock();
            else
                GetDefiledVagene();
        }

        public void GetDefiledCock()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Cultistnunscene.GetDefiledCock.TriesApproach".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock.NunGetsUp".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock.GrabsCock".Localized());

            if (Player.Cocks[x].Length < 6)
                OutputText("Scenes.Cultistnunscene.GetDefiledCock.PoorExample".Localized());
            else
                if (Player.Cocks[x].Length > 15)
                    OutputText("Scenes.Cultistnunscene.GetDefiledCock.PrideGreatest".Localized());
                else
                    OutputText("Scenes.Cultistnunscene.GetDefiledCock.GreatExample".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock.TakesMouth".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Anemone)
                OutputText("Scenes.Cultistnunscene.GetDefiledCock.TongueTingles".Localized());
            else
                if (Player.Cocks[x].Id == new Identifier("coc", "tentacle"))
                    OutputText("Scenes.Cultistnunscene.GetDefiledCock.CorruptedBeasts".Localized());
                else
                    if (Player.Cocks[x].Id == BodyPartTypes.Demon)
                        OutputText("Scenes.Cultistnunscene.GetDefiledCock.GirlCumDrop".Localized());
                    else
                        if (Player.Cocks[x].Id != BodyPartTypes.Human)
                            OutputText("Scenes.Cultistnunscene.GetDefiledCock.BestialCock".Localized());
                        else
                            OutputText("Scenes.Cultistnunscene.GetDefiledCock.GladNotCorrupt".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock.BeginsPleasuring".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock.ThumbsLower".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock.SuddenlyStops".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock.BaptismPrayer".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock.StuckWondering".Localized());

            DoNext(() => GetDefiledCock2());
        }

        public void GetDefiledCock2()
        {
            ClearOutput();

            Player.Orgasm();

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Cultistnunscene.GetDefiledCock2.SwiftMovement".Localized((Player.Cocks[x].Length <= 16 ? "your entire cock" : "the first sixteen inches of your cock")));

            OutputText("Scenes.Cultistnunscene.GetDefiledCock2.RemainsStill".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock2.BreathesReinserts".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock2.LegsShake".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock2.WetSounds".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock2.NotYetSinner".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock2.UnableHump".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock2.RosaryTies".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock2.ProceedsStroke".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock2.NothingHappens".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock2.BreathsDesperation".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock2.SemenPurified".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock2.NotBotherThinking".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock2.BringsHandDown".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock2.StillUnableMove".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock2.ShovesCross".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock2.ThrobsPowerfully".Localized(
                 Player.LowMedHighCum("each contraction launching a small rope of cum onto the nun's face. She dutifully takes it all, unfazed.[pg][say: I will work hard for you, Lord. From this baptism, I can see I have lacked in my devotion.]", "each contraction painting her body with thick ropes of cum. She dutifully takes it all, unfazed, cum pooling on her breasts. [say: Thank you, Lord, for this baptism. I shall strive to do even better next time.]", "each contraction covering her body with obscene amounts of cum. Despite the absurd volume of your ejaculation, she remains still, unfazed. Cum pools on her torso, sliding down the smooth satin fabric in great globs. She licks her lips, removing just enough of your copious cum for her to speak. [say: Thank you, Lord, for this divine blessing. This baptism is evidence I have been righteous, and will strive to be even more so.]")
            ));

            OutputText("Scenes.Cultistnunscene.GetDefiledCock2.RemovesCross".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledCock2.UnsureWantHit".Localized());

            DoNext(() => GetDefiledEnd());
        }

        public void GetDefiledVagene()
        {
            ClearOutput();

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene.TriesApproach".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene.NunGetsUp".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene.FaceContorts".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene.NotMyPosition".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene.ApproachesSwaying".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene.WhyCarryYourself".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene.BringsHandBetween".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene.LostLikeYou".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene.WhisperingPrayer".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene.ShovesDildo".Localized());

            DoNext(() => GetDefiledVagene2());
        }

        public void GetDefiledVagene2()
        {
            ClearOutput();

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene2.DarkVoid".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene2.SpendsMoments".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene2.LooksPanicking".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene2.MovesTowards".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene2.LostDesperate".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene2.HeadsTowardsCock".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene2.UnsureWhatToDo".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene2.HandTingles".Localized());

            var wetText = Player.TextByWetnessVagina("You feel your pussy moistening, challenging its usual dryness due to the sheer magnificence of the cock you're servicing.", "Your pussy tingles, lubricating itself and dripping with expectation, unable to contain its desire to be filled by this perfect cock.", "Your pussy twitches, several strings of lube sliding and dripping out, begging to be filled by this perfect cock.", "Your pussy twitches, a small gush of girl cum showing just how much it needs to be filled by this perfect cock.", "Your pussy twitches, a small gush of girl cum showing just how much it needs to be filled by this perfect cock.", "Your already soaked pussy gushes a torrent of girl cum, drenching your [legs] with your liquid desire, betraying your unbearable need to be filled by this perfect cock.");

            OutputText(wetText);

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene2.OpensMouth".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene2.ClosesLips".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene2.EveryThrustPace".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene2.PaceBecomesRough".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene2.BroughtOutHaze".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene2.AlignsDick".Localized());

            DoNext(() => GetDefiledVagene3());
        }

        public void GetDefiledVagene3()
        {
            ClearOutput();

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene3.BackAtLake".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene3.SmilesAgain".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene3.UnbearableTease".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene3.FigureHilts".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene3.MomentLaterPace".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene3.SeveralMinutes".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene3.RopeDivineCum".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene3.GodOffers".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledVagene3.Vanishes".Localized());
        }

        public void GetDefiledEnd()
        {
            ClearOutput();

            OutputText("Scenes.Cultistnunscene.GetDefiledEnd.WakeUpNun".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledEnd.LooksForNun".Localized());

            OutputText("Scenes.Cultistnunscene.GetDefiledEnd.RegardlessFaith".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, -5f));

            InventoryScene.TakeItem(ItemRegistry.Create(ArmorIds.NaughtyNunsHabit), GetDefiledTake, () => ReturnToCamp(1));
        }

        public void GetDefiledTake()
        {

            CoC.Instance.Flags.Set(GameFlagIds.TookNaughtyHabit, 1);

            DoNext(() => ReturnToCamp(2));
        }

        public void AnswerNun(int answer = 0)
        {
            ClearOutput();

            OutputText("Scenes.Cultistnunscene.AnswerNun.TakesSigh".Localized());

            switch (answer)
            {
                case 0:
                {
                OutputText("Scenes.Cultistnunscene.AnswerNun.LordManyForms".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Cultistnunscene.AnswerNun.OfCourseChild".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Cultistnunscene.AnswerNun.DifficultFaith".Localized());
                }

                break;
            }

            OutputText("Scenes.Cultistnunscene.AnswerNun.BreathesSighs".Localized());

            OutputText("Scenes.Cultistnunscene.AnswerNun.JoinPrayer".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Sure".Localized(), JoinNun);

            AddButton(1, "Common.Button.No".Localized(), DenyPrayer);

            if (Player.Corruption > 60)
                AddButton(2, "Common.Button.DefileHer".Localized(), DefileHer);
        }

        public void DenyPrayer()
        {
            ClearOutput();

            OutputText("Scenes.Cultistnunscene.DenyPrayer.DontCareReligion".Localized());

            OutputText("Scenes.Cultistnunscene.DenyPrayer.UnaffectedRejection".Localized());

            OutputText("Scenes.Cultistnunscene.DenyPrayer.ContinuesSilence".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void JoinNun()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.PrayedWithNun, 1);

            OutputText("Scenes.Cultistnunscene.JoinNun.SoundsGood".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNun.ThankYouJoining".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNun.WaitsSilence".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNun.BlessingCreation".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNun.Nod".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNun.CreationReaches".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNun.NodAgain".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNun.CreationGrows".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNun.NotSureWhat".Localized());

            DoNext(() => JoinNun2());
        }

        public void JoinNun2()
        {
            ClearOutput();

            OutputText("Scenes.Cultistnunscene.JoinNun2.FidgetsSpot".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNun2.LargeDoesGrow".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNun2.WhatWord".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNun2.RubsThighs".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNun2.ItThrobs".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNun2.WhatTheHell".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNun2.WhenItBursts".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNun2.ScreamsThrows".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNun2.GetsUpDistancing".Localized());

            DoNext(() => JoinNunStay());
        }

        public void JoinNunStay()
        {
            ClearOutput();

            OutputText("Scenes.Cultistnunscene.JoinNunStay.BePatient".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunStay.FeelYourTouch".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunStay.NewSurge".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunStay.YesISeeIt".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunStay.MoansLoudly".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunStay.KneelsAgain".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunStay.ThankRemaining".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunStay.ServeHimWord".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunStay.NotDifficult".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunStay.RubsThighsAgain".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunStay.SpreadTheWord".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), JoinNunJoinReligion);

            AddButton(1, "Common.Button.No".Localized(), JoinNunFuckOffMormons);
        }

        public void JoinNunFuckOffMormons()
        {
            ClearOutput();

            OutputText("Scenes.Cultistnunscene.JoinNunFuckOffMormons.RaisesHands".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunFuckOffMormons.LooksDisappointed".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunFuckOffMormons.WhateverShrugging".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void JoinNunJoinReligion()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

            OutputText("Scenes.Cultistnunscene.JoinNunJoinReligion.DespiteRude".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunJoinReligion.SmilesSerenely".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunJoinReligion.TakesChest".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunJoinReligion.TakesHabit".Localized());

            InventoryScene.TakeItem(ItemRegistry.Create(ArmorIds.NaughtyNunsHabit), JoinNunFinish, JoinNunMaybeLater);
        }

        public void JoinNunFinish()
        {
            ClearOutput();

            OutputText("Scenes.Cultistnunscene.JoinNunFinish.TakesHabitBeams".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunFinish.PartWonders".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunFinish.ThankAgain".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunFinish.FineByYou".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TookNaughtyHabit, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void JoinNunMaybeLater()
        {
            ClearOutput();

            OutputText("Scenes.Cultistnunscene.JoinNunMaybeLater.CantTakeHabit".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunMaybeLater.IUnderstand".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunMaybeLater.Nod".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunMaybeLater.FindingAnother".Localized());

            OutputText("Scenes.Cultistnunscene.JoinNunMaybeLater.FineByYou".Localized());

            DoNext(() => ReturnToCamp(1));
        }

    }
}