using MonoCoC.Attributes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Internals;
using MonoCoC.Utils;
using MonoCoC.Contents.Scenes.Vanilla.Places;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<VapulaScene>("coc:vapula")]
    public class VapulaScene : Scene, ITimeAware
    {

        public static SaveContent Save => SaveContent(SceneIds.Npcs.Vapula);

        public override void Entry()
        {
            VapulaSlave();
        }

        public bool TimeChange()
        {
            if (View.Time.Hours > 23)
            {
                if (VapulaSlave() && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmVapula) == 0)
                    if (CoC.Instance.Flags.Get(GameFlagIds.VapulaHaremFuck) == 0)
                        CoC.Instance.Flags.Set(GameFlagIds.VapulaDaysSinceFed, CoC.Instance.Flags.Get(GameFlagIds.VapulaDaysSinceFed) + 1);
                    else
                        CoC.Instance.Flags.Set(GameFlagIds.VapulaDaysSinceFed, 0);

                var follower = CoC.Instance.Flags.Get(GameFlagIds.VapulaFollower);

                if (Math.Abs(follower - 0.5f) < 0.001f || Math.Abs(follower - 1.5f) < 0.001f)
                    CoC.Instance.Flags.Set(GameFlagIds.VapulaFollower, follower + 1);

                CoC.Instance.Flags.Set(GameFlagIds.DaysSinceLastDemonDealings, CoC.Instance.Flags.Get(GameFlagIds.DaysSinceLastDemonDealings) + 1);
            }

            if (VapulaSlave()
                && !Player.HasKeyItem(DullahanScene.DemonicStrapOn)

                && Player.Gender == Gender.Female

                && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmVapula) == 0)
            {
                VapulaGivesPCAPresent();

                return true;
            }

            return false;
        }

        public bool TimeChangeLarge()
        {
            if (CoC.Instance.Flags.Get(GameFlagIds.VapulaFollower) >= 2.5f
                && View.Time.Hours == 6

                && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmVapula) == 0)
            {
                FemaleVapulaRecruitmentPartII();

                return true;
            }

            if (View.Time.Hours == 2
                && VapulaSlave()

                && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmVapula) == 0

                && CoC.Instance.Flags.Get(GameFlagIds.VapulaDaysSinceFed) >= 5

                && (Player.HasCock()

                    || (Player.HasKeyItem(DullahanScene.DemonicStrapOn) && Player.HasVagina())))
            {
                VapulaForceFeeds();

                return true;
            }

            return false;
        }

        public bool VapulaSlave()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.VapulaFollower) == 1;
        }

        public bool VapulaAtCamp()
        {

            return VapulaSlave() && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmVapula) == 0;
        }

        public void FemaleVapulaRecruitmentPartII()
        {

            OutputText("Scenes.Vapula.FemaleVapulaRecruitmentPartII.AwokenFingersThighs".Localized());

            OutputText("Scenes.Vapula.FemaleVapulaRecruitmentPartII.FoundYouMistress".Localized());

            OutputText("Scenes.Vapula.FemaleVapulaRecruitmentPartII.RubEyesAcceptThing".Localized());

            OutputText("Scenes.Vapula.FemaleVapulaRecruitmentPartII.NotManyStrapOns".Localized());

            OutputText("Scenes.Vapula.FemaleVapulaRecruitmentPartII.WellDoneMySlave".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.VapulaFollower, 1);

            CoC.Instance.Flags.Set(GameFlagIds.VapulaHaremFuck, 1);

            Player.CreateKeyItem("Demonic Strap-On", 0, 0, 0, 0);

            OutputText("Scenes.Vapula.FemaleVapulaRecruitmentPartII.GainedDemonicStrapOn".Localized());

            Go<CampScene>();
        }

        public void VapulaGivesPCAPresent()
        {

            OutputText("Scenes.Vapula.VapulaGivesPCAPresent.WalksUpFrowns".Localized());

            OutputText("Scenes.Vapula.VapulaGivesPCAPresent.WalksAwayDiscipline".Localized());

            Player.CreateKeyItem("Demonic Strap-On", 0, 0, 0, 0);

            OutputText("Scenes.Vapula.VapulaGivesPCAPresent.DemonicStrapAgain".Localized());
        }

        public void VapulaSlaveFlavorText()
        {

            if (CampSleep.IsForceSleepTime())
            {
                OutputText("Scenes.Vapula.VapulaSlaveFlavorText.ZeroParagraph".Localized(
                    RandomUtils.Choice(["Vapula is splayed out on the dirt, sleeping in an unflattering position.", "Your succubus slave, Vapula, has passed out near [if (builtcabin){the door of your cabin|your bedroll}]. You surmise she meant to jump you for some sex before you went to sleep, but fell asleep herself instead."])
                ));

                return ;
            }

            var choice = Random.Shared.Next(11);

            if (choice == 0)
                OutputText("Scenes.Vapula.VapulaSlaveFlavorText.RestingBedOfLeaves".Localized());

            if (choice == 1)
                OutputText("Scenes.Vapula.VapulaSlaveFlavorText.StrokingHerselfFuriously".Localized());

            if (choice == 2)
                if (CoC.Instance.Flags.Get(GameFlagIds.VapulaHaremFuck) == 0)
                    choice = 3;
                else
                    OutputText("Scenes.Vapula.VapulaSlaveFlavorText.CurrentlyAbsentImps".Localized());

            if (choice == 3)
                OutputText("Scenes.Vapula.VapulaSlaveFlavorText.DrinkingVialOfMilk".Localized());

            if (choice == 4)
            {
                OutputText("Scenes.Vapula.VapulaSlaveFlavorText.ToyingWith".Localized());

                if (Player.HasKeyItem("Deluxe Dildo"))
                    OutputText("Scenes.Vapula.VapulaSlaveFlavorText.YourDeluxeDildo".Localized());
                else
                    OutputText("Scenes.Vapula.VapulaSlaveFlavorText.ADildoWord".Localized());

                OutputText("Scenes.Vapula.VapulaSlaveFlavorText.PumpingFastSheCan".Localized());
            }

            if (choice == 5)
                if (CoC.Instance.Flags.Get(GameFlagIds.VapulaHaremFuck) == 0 || !SceneInstance<SophieBimboScene>.Get().BimboSophie())
                    choice = 6;
                else
                    OutputText("Scenes.Vapula.VapulaSlaveFlavorText.FingeringSophie".Localized());

            if (choice == 6)
                if (CoC.Instance.Flags.Get(GameFlagIds.VapulaHaremFuck) == 0 || CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) > 0 || !SceneInstance<IzmaScene>.Get().IzmaFollower())
                    choice = 7;
                else
                    OutputText("Scenes.Vapula.VapulaSlaveFlavorText.BusyWithIzma".Localized());

            if (choice == 7)
                if (CoC.Instance.Flags.Get(GameFlagIds.VapulaHaremFuck) == 0 || !SceneInstance<JojoScene>.Get().CampCorruptJojo())
                    choice = 8;
                else
                {
                    OutputText("Scenes.Vapula.VapulaSlaveFlavorText.ScreamsOutsideCamp".Localized());

                    if (SceneInstance<JojoScene>.Get().TentacleJojo())
                        OutputText("Scenes.Vapula.VapulaSlaveFlavorText.TentacledWord".Localized());

                    OutputText("Scenes.Vapula.VapulaSlaveFlavorText.MousePetFun".Localized());
                }

            if (choice == 8)
                if (CoC.Instance.Flags.Get(GameFlagIds.VapulaHaremFuck) == 0 || !SceneInstance<AmilyScene>.Get().AmilyFollowerCorrupt())
                    choice = 9;
                else
                    OutputText("Scenes.Vapula.VapulaSlaveFlavorText.CaressingAmily".Localized());

            if (choice == 9)
                if (!SceneInstance<RathazulScene>.Get().FollowerRathazul())
                    choice = 10;
                else
                    OutputText("Scenes.Vapula.VapulaSlaveFlavorText.ConversationRathazul".Localized());

            if (choice == 10)
                OutputText("Scenes.Vapula.VapulaSlaveFlavorText.RestingButtplug".Localized());
        }

        public void MouseWaifuFreakout(bool amily = false, bool jojo = false)
        {
            ClearOutput();

            if (amily)
            {
                OutputText("Scenes.Vapula.MouseWaifuFreakout.AmilyWord".Localized());

                if (jojo)
                    OutputText("Scenes.Vapula.MouseWaifuFreakout.AndWord".Localized());
            }

            if (jojo)
                OutputText("Scenes.Vapula.MouseWaifuFreakout.JojoWord".Localized());

            OutputText("Scenes.Vapula.MouseWaifuFreakout.WalkWord".Localized());

            if (!(amily && jojo))
                OutputText("Scenes.Vapula.MouseWaifuFreakout.PluralS".Localized());

            OutputText("Scenes.Vapula.MouseWaifuFreakout.UpToYouWorried".Localized());

            if (amily || jojo)
                OutputText("Scenes.Vapula.MouseWaifuFreakout.WhatIsThisDreaming".Localized());

            OutputText("Scenes.Vapula.MouseWaifuFreakout.RelaxMyDemon".Localized());

            if (amily || jojo)
                OutputText("Scenes.Vapula.MouseWaifuFreakout.ThisIsInsaneFighting".Localized());

            OutputText("Scenes.Vapula.MouseWaifuFreakout.ExplainRenegadeFearsLethice".Localized());

            if (amily || jojo)
                OutputText("Scenes.Vapula.MouseWaifuFreakout.StillASuccubus".Localized());

            if (Player.Intelligence < (60 + (Player.CorAdjustedDown() / 2)))
            {
                if (Player.Libido >= 50)
                    OutputText("Scenes.Vapula.MouseWaifuFreakout.LongAsSheSwallows".Localized());

                OutputText("Scenes.Vapula.MouseWaifuFreakout.TheWord".Localized());

                if (jojo && amily)
                    OutputText("Scenes.Vapula.MouseWaifuFreakout.MiceAreWord".Localized());
                else
                    OutputText("Scenes.Vapula.MouseWaifuFreakout.MouseIsWord".Localized());

                OutputText("Scenes.Vapula.MouseWaifuFreakout.RavingWord".Localized());

                if (jojo)
                {
                    OutputText("Scenes.Vapula.MouseWaifuFreakout.ImLeavingJojoSays".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.JojoMoveInDisabled, 1);

                    Player.RemoveStatusEffect(StatusEffectIds.JojoNightWatch);

                    Player.RemoveStatusEffect(StatusEffectIds.PureCampJojo);
                }

                if (amily)
                {
                    OutputText("Scenes.Vapula.MouseWaifuFreakout.AmilyShakesHeadChanged".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 0);

                    CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 0);

                    if (Player.Pregnancy.Type == PregnancyIds.Amily)
                        Player.KnockUp(PregnancyIds.Mouse, Player.Pregnancy.Incubation, 100, 1);

                    CoC.Instance.Flags.Set(GameFlagIds.AmilyCorruptFlipout, 1);

                    CoC.Instance.Flags.Set(GameFlagIds.AmilyCampCorruptionFreaked, 0);
                }

                if (!Player.IsPureEnough(70))
                    OutputText("Scenes.Vapula.MouseWaifuFreakout.FuckOffHotterSlut".Localized());

                OutputText("Scenes.Vapula.MouseWaifuFreakout.ParagraphBreak".Localized());

                if (jojo)
                    OutputText("Scenes.Vapula.MouseWaifuFreakout.JojoWordAgain".Localized());

                if (amily && jojo)
                    OutputText("Scenes.Vapula.MouseWaifuFreakout.AndWordAgain".Localized());

                if (amily)
                    OutputText("Scenes.Vapula.MouseWaifuFreakout.AmilyWordAgain".Localized());

                if (amily && jojo)
                    OutputText("Scenes.Vapula.MouseWaifuFreakout.HaveWord".Localized());
                else
                    OutputText("Scenes.Vapula.MouseWaifuFreakout.HasWord".Localized());

                OutputText("Scenes.Vapula.MouseWaifuFreakout.MovedOutWord".Localized());

                OutputText("Scenes.Vapula.MouseWaifuFreakout.PastTimeBroughtAround".Localized());

                Go<CampScene>();
            }
            else
            {
                OutputText("Scenes.Vapula.MouseWaifuFreakout.AssureKeepHerAway".Localized());

                if (amily)
                    OutputText("Scenes.Vapula.MouseWaifuFreakout.AmilyWordThird".Localized());

                if (amily && jojo)
                    OutputText("Scenes.Vapula.MouseWaifuFreakout.AndWordThird".Localized());

                if (jojo)
                    OutputText("Scenes.Vapula.MouseWaifuFreakout.JojoWordThird".Localized());

                OutputText("Scenes.Vapula.MouseWaifuFreakout.MakeSureStayAway".Localized());

                if (jojo)
                {
                    OutputText("Scenes.Vapula.MouseWaifuFreakout.FineCloseWatch".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.KeptPureJojoOverVapula, 1);
                }

                if (amily)
                {
                    OutputText("Scenes.Vapula.MouseWaifuFreakout.FineDontComeNear".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.KeptPureAmilyOverVapula, 1);
                }
            }

            Go<CampScene>();
        }

        public void CallSlaveVapula(bool output = true)
        {
            ClearOutput();

            if (output)
                if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmVapula) == 0)
                {
                    OutputText("Scenes.Vapula.CallSlaveVapula.GentlyTapShoulder".Localized());

                    OutputText("Scenes.Vapula.CallSlaveVapula.WantSomeFun".Localized());
                }
                else
                {
                    OutputText("Scenes.Vapula.CallSlaveVapula.MoochesOverKicksImp".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmVapulaGibsMilk) == 1)
                    {
                        CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmVapulaGibsMilk, 2);

                        OutputText("Scenes.Vapula.CallSlaveVapula.WordlesslyHoldOutHand".Localized());

                        Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.SuccubiMilk), 1);

CallSlaveVapula();

                        return ;
                    }
                }

            ClearButtons();

            AddNextButton("Common.Button.Appearance".Localized(), FapulaFapfapfapAppearance);

            AddNextButton("Common.Button.Talk".Localized(), TalkToVapulaForSomeReason);

            AddNextButton("Common.Button.Sex".Localized(), VapulaSexMenu);

            AddButton(5, "Common.Button.Feed".Localized(), FeedVapulaACupOfJizz);

            AddButton(6, "Common.Button.FeedDildo".Localized(), ChixFeedVapulaBlehblehIVantToZuckYourSpooo);

            AddButton(7, "Common.Button.Spank".Localized(), SpankVapulaLikeABoss);

            if (CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionStarted) == 1)
                if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmVapula) == 0)
                    AddButton(10, "Common.Button.FarmWork".Localized(), SendToFarm);
                else
                    AddNextButton("Common.Button.GoCamp".Localized(), BackToCamp);

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmVapula) == 1)
                if (CoC.Instance.Flags.Get(GameFlagIds.FollowerProductionVapula) == 0)
                    AddButton(11, "Common.Button.HarvestMilk".Localized(), HarvestMilk);
                else
                    AddNextButton("Common.Button.StopHarvest".Localized(), StopHarvest);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.Liddellium)) && (CoC.Instance.Flags.Get(GameFlagIds.LiddelliumFlag) >= 0))
                AddRowButton(2, "Strange Potion", IdLiddellium);

            AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmVapula) == 1)
                AddButton(14, "Common.Button.Back".Localized(), null );

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));
        }

        public void SendToFarm()
        {
            ClearOutput();

            OutputText("Scenes.Vapula.SendToFarm.TellFarmPresent".Localized());

            OutputText("Scenes.Vapula.SendToFarm.GoodOneMasterFarm".Localized());

            OutputText("Scenes.Vapula.SendToFarm.HerWorkingSuccubusLesson".Localized());

            OutputText("Scenes.Vapula.SendToFarm.ArguesStandFirm".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmVapula, 1);

            ReturnToCamp(1);
        }

        public void BackToCamp()
        {
            ClearOutput();

            OutputText("Scenes.Vapula.BackToCamp.HeadBackToCamp".Localized());

            OutputText("Scenes.Vapula.BackToCamp.YessssNoRegret".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmVapula, 0);

            DoNext(FarmCorruption.RootScene);
        }

        public void HarvestMilk()
        {
            ClearOutput();

            OutputText("Scenes.Vapula.HarvestMilk.WantMilkingMachine".Localized());

            OutputText("Scenes.Vapula.HarvestMilk.OohThatsWhy".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FarmUpgradesRefinery) == 0)
                OutputText("Scenes.Vapula.HarvestMilk.BeatDisgustingThing".Localized());
            else
                OutputText("Scenes.Vapula.HarvestMilk.InsistBeatWorking".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerProductionVapula, 1);

            DoNext(FarmCorruption.RootScene);
        }

        public void StopHarvest()
        {
            ClearOutput();

            OutputText("Scenes.Vapula.StopHarvest.StopProducingMilk".Localized());

            OutputText("Scenes.Vapula.StopHarvest.WholeIdeaDrinkMore".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerProductionVapula, 0);

            DoNext(FarmCorruption.RootScene);
        }

        public void FapulaFapfapfapAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Vapula.FapulaFapfapfapAppearance.SuccubusSixFootOne".Localized());

            OutputText("Scenes.Vapula.FapulaFapfapfapAppearance.JigglyHCupBreasts".Localized());

            OutputText("Scenes.Vapula.FapulaFapfapfapAppearance.PinkWetPussy".Localized());

            OutputText("Scenes.Vapula.FapulaFapfapfapAppearance.TightAssholeBetweenCheeks".Localized());

            if (FarmCorruption.hasTattoo("vapula"))
            {
                OutputText("Scenes.Vapula.FapulaFapfapfapAppearance.ParagraphBreak".Localized());

                if (FarmCorruption.vapulaFullTribalTats())
                    OutputText("Scenes.Vapula.FapulaFapfapfapAppearance.TattoosHeadToTail".Localized());
                else
                {
                    if (FarmCorruption.numTattoos("vapula") > 1)
                        OutputText("Scenes.Vapula.FapulaFapfapfapAppearance.FollowingTattoosEmblazoned".Localized());
                    else
                        OutputText("Scenes.Vapula.FapulaFapfapfapAppearance.SheHasWord".Localized());

                    if (CoC.Instance.Flags.Has(GameFlagIds.VapulaTattooCollarbone))
                        OutputText("Scenes.Vapula.FapulaFapfapfapAppearance.ZeroNewline".Localized(
                            CoC.Instance.Flags.Get(GameFlagIds.VapulaTattooCollarbone)
                        ));

                    if (CoC.Instance.Flags.Has(GameFlagIds.VapulaTattooShoulders))
                        OutputText("Scenes.Vapula.FapulaFapfapfapAppearance.ZeroNewlineAgain".Localized(
                            CoC.Instance.Flags.Get(GameFlagIds.VapulaTattooShoulders)
                        ));

                    if (CoC.Instance.Flags.Has(GameFlagIds.VapulaTattooLowerback))
                        OutputText("Scenes.Vapula.FapulaFapfapfapAppearance.ZeroNewlineThird".Localized(
                            CoC.Instance.Flags.Get(GameFlagIds.VapulaTattooLowerback)
                        ));

                    if (CoC.Instance.Flags.Has(GameFlagIds.VapulaTattooButt))
                        OutputText("Scenes.Vapula.FapulaFapfapfapAppearance.ZeroNewlineFourth".Localized(
                            CoC.Instance.Flags.Get(GameFlagIds.VapulaTattooButt)
                        ));
                }
            }

            CallSlaveVapula(false);
        }

        public void TalkToVapulaForSomeReason()
        {
            ClearOutput();

            OutputText("Scenes.Vapula.TalkToVapulaForSomeReason.WantToTalkRare".Localized(
                Player.Mf("studs", "girls")
            ));

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.VapulaHaremFuck) == 0)
            {
                OutputText("Scenes.Vapula.TalkToVapulaForSomeReason.WontFuckOthers".Localized());

                AddButton(0, "Common.Button.Fuckfollowers".Localized(), ToggleVapulaHaremFucks);
            }
            else
            {
                OutputText("Scenes.Vapula.TalkToVapulaForSomeReason.WillFuckWillingPartners".Localized());

                AddButton(0, "Common.Button.DontFuck".Localized(), ToggleVapulaHaremFucks);
            }

            if (Player.HasStatusEffect(StatusEffectIds.RepeatSuccubi))
                if (CoC.Instance.Flags.Get(GameFlagIds.VapulaThreesomes) == 0)
                {
                    OutputText("Scenes.Vapula.TalkToVapulaForSomeReason.WontAssistCerulean".Localized());

                    AddButton(1, "Common.Button.AssistOn".Localized(), ToggleCeruleanVapulaAssist);
                }
                else
                {
                    OutputText("Scenes.Vapula.TalkToVapulaForSomeReason.CallCeruleanAssist".Localized());

                    AddButton(1, "Common.Button.AssistOff".Localized(), ToggleCeruleanVapulaAssist);
                }

            AddButton(14, "Common.Button.Back".Localized(), () => CallSlaveVapula());
        }

        public void ToggleCeruleanVapulaAssist()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.VapulaThreesomes) == 0)
            {
                OutputText("Scenes.Vapula.ToggleCeruleanVapulaAssist.InformSuccubusJoinNights".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.VapulaThreesomes, 1);
            }
            else
            {
                OutputText("Scenes.Vapula.ToggleCeruleanVapulaAssist.InformDontMeddle".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.VapulaThreesomes, 0);
            }

            TalkToVapulaForSomeReason();
        }

        public void ToggleVapulaHaremFucks()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.VapulaHaremFuck) == 1)
            {
                OutputText("Scenes.Vapula.ToggleVapulaHaremFucks.FedUpFuckingEveryone".Localized());

                OutputText("Scenes.Vapula.ToggleVapulaHaremFucks.VapulaProtestsFeed".Localized());

                OutputText("Scenes.Vapula.ToggleVapulaHaremFucks.AssureCumProduction".Localized());

                OutputText("Scenes.Vapula.ToggleVapulaHaremFucks.PossessiveMasterVeryWell".Localized());

                OutputText("Scenes.Vapula.ToggleVapulaHaremFucks.PretendNotUnderstand".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.VapulaHaremFuck, 0);
            }
            else
            {
                OutputText("Scenes.Vapula.ToggleVapulaHaremFucks.DontCareAnymore".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.VapulaHaremFuck, 1);
            }

            TalkToVapulaForSomeReason();
        }

        public void VapulaSexMenu()
        {

            ClearButtons();

            AddNextButton("Scenes.Akbal.AkbalDefeated.ButtonForceLick".Localized(), VapulaForceLick);

            AddNextButton("Common.Button.FuckHer".Localized(), VapulaVaginal);

            AddNextButton("Common.Button.Threesome".Localized(), VapulaThreesomeMenu);

            AddButton(14, "Common.Button.Back".Localized(), () => CallSlaveVapula());
        }

        public void FeedVapulaACupOfJizz()
        {
            ClearOutput();

            OutputText("Scenes.Vapula.FeedVapulaACupOfJizz.RewardGoodSlut".Localized());

            OutputText("Scenes.Vapula.FeedVapulaACupOfJizz.MinistrationsTickling".Localized());

            if (Player.Balls > 0)
                OutputText(Player.BallsDescriptLight());
            else
                OutputText("Scenes.Vapula.FeedVapulaACupOfJizz.GroinWord".Localized());

            OutputText("Scenes.Vapula.FeedVapulaACupOfJizz.SucksLikeWhore".Localized());

            OutputText("Scenes.Vapula.FeedVapulaACupOfJizz.WorksOnItHours".Localized());

            OutputText("Scenes.Vapula.FeedVapulaACupOfJizz.StaminaOverwhelmedSavagely".Localized(
                NumberUtils.Num2Text(Player.Cocks[0].Length)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Vapula.FeedVapulaACupOfJizz.YourBallsChurn".Localized());
            else
                OutputText("Scenes.Vapula.FeedVapulaACupOfJizz.YourCockTwitches".Localized());

            OutputText("Scenes.Vapula.FeedVapulaACupOfJizz.ReleaseStickySpooge".Localized());

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Vapula.FeedVapulaACupOfJizz.UntilSixMonthsPregnant".Localized());

            OutputText("Scenes.Vapula.FeedVapulaACupOfJizz.CumFlowEbbStops".Localized(
                Player.CockDescript(0)
            ));

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, .5f));

            CoC.Instance.Flags.Set(GameFlagIds.VapulaDaysSinceFed, 0);

            CoC.Instance.Flags.Set(GameFlagIds.VapulaTeaseCount, 0);

            ReturnToCamp(1);
        }

        public void VapulaThreesomeMenu()
        {
            ClearOutput();

            OutputText("Scenes.Vapula.VapulaThreesomeMenu.WhoDoYouInvite".Localized());

            ClearButtons();

            if (Player.HasCock() || (Player.HasVagina() && Player.HasKeyItem("Demonic Strap-On")))
            {
                if (SceneInstance<AmilyScene>.Get().AmilyFollowerCorrupt() && Player.HasCock())
                    AddButton(0, "Common.Button.Amily".Localized(), VapulaAndAmilyThreesome);

                if (SceneInstance<CeraphFollowerScene>.Get().CeraphIsFollower() && Player.HasCock())
                    AddButton(1, "Npcs.Ceraph.Name".Localized(), VapulaCeraphThreesome);

                if (SceneInstance<SophieBimboScene>.Get().BimboSophie())
                    AddButton(2, "Common.Button.Sophie".Localized(), VapulaSophieThreesomeSelect);

                if (SceneInstance<JojoScene>.Get().CampCorruptJojo())
                    AddButton(3, "Common.Button.Jojo".Localized(), VapulaJojoThreesomes);

                if (SceneInstance<IzmaScene>.Get().IzmaFollower() && CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                    AddButton(4, "Common.Button.Izma".Localized(), VapulaAndIzmaThreeSome);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.OwcaUnlocked) > 0)
                AddNextButton("Common.Button.Rebecc".Localized(), VapulaRebeccAbuse);

            AddButton(14, "Common.Button.Back".Localized(), () => CallSlaveVapula());
        }

        public void VapulaCeraphThreesome()
        {
            ClearOutput();

            OutputText("Scenes.Vapula.VapulaCeraphThreesome.SummonCeraphTalisman".Localized(
                Player.Mf("Master", "Mistress")
            ));

            if (Player.HasCock())
                OutputText("Scenes.Vapula.VapulaCeraphThreesome.DickWord".Localized());
            else
                OutputText("Scenes.Vapula.VapulaCeraphThreesome.PussyWord".Localized());

            OutputText("Scenes.Vapula.VapulaCeraphThreesome.DeservesSpecialTreatment".Localized());

            OutputText("Scenes.Vapula.VapulaCeraphThreesome.HowTakeThem".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.PleaseBoth".Localized(), VapulaCeraphThreesomePleaseBoth);

            AddButton(1, "Common.Button.AnalTrain".Localized(), VapulaAndCeraphButtfuckTrainYeehaw);
        }

        public void VapulaCeraphThreesomePleaseBoth()
        {
            ClearOutput();

            var x = Player.CockThatFits(100);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.OrderTurnAroundDicking".Localized());

            OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.PushVapulaLegsSpread".Localized(
                Player.CockDescript(x),

                Player.Mf("Master", "Mistress"),

                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.NoiseSlapRump".Localized(
                Player.Mf("Master", "Mistress"),

                Player.CockDescript(x)
            ));

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.SwellingBellyHordeImps".Localized());

            OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.SentencePeriod".Localized());

            OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.YouFeelYour".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.BallsTwitchPulsate".Localized(
                    Player.CockDescript(x)
                ));
            else
                OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.YourTwitchPulsate".Localized(
                    Player.CockDescript(x)
                ));

            OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.EssenceIntoOmnibus".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.AnotherSlapShutUp".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.BallsHittingButthole".Localized());
            else
                OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.ThuddingCushyButtCheeks".Localized());

            OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.PurpleSexGoddessSqueals".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.DecidingPutJunkUse".Localized());

            if (Player.CockTotal() > 2)
                OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.ExtraRoomThirdMember".Localized());

            if (Player.CockTotal() > 3)
                OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.CeraphMoistNetherLips".Localized());

            if (Player.CockTotal() > 4)
                OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.ExtraJunkNoHole".Localized());

            if (Player.CockTotal() < 4)
                OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.KeepThrustingFastRough".Localized());

            OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.KeepFuckingMoanAlternatively".Localized());

            OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.CantKeepUp".Localized());

            OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.NotDoneWithSluts".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.ExpertlyJerksEachRod".Localized());

            OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.ObviouslyUsedToPractice".Localized());

            OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.EnjoyingCeraphBlowjob".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.TellVapulaJoinCeraph".Localized());

            OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.ThoughtsDrownedPleasure".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.PluralS".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.GirlsKeepFondlingBalls".Localized());

            OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.ExtraTinglesPleasure".Localized());

            OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.SweetMinistrationsTooMuch".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.PluralSAgain".Localized());

            OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.LipsMilk".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.ItWord".Localized());
            else
                OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.ThemWord".Localized());

            OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.OfAllWord".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.ItsWord".Localized());
            else
                OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.TheirWord".Localized());

            OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.JunkEmptyPopOut".Localized());

            OutputText("Scenes.Vapula.VapulaCeraphThreesomePleaseBoth.SatisfiedKissBothSluts".Localized(
                Player.Mf("Master", "Mistress")
            ));

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            if (Player.LibidoPercent > 10)
                Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            if (Player.LibidoPercent > 50)
                Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            if (Player.LibidoPercent > 70)
                Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            CoC.Instance.Flags.Set(GameFlagIds.VapulaDaysSinceFed, 0);

            CoC.Instance.Flags.Set(GameFlagIds.VapulaTeaseCount, 0);

            ReturnToCamp(1);
        }

        public void VapulaAndCeraphButtfuckTrainYeehaw()
        {
            ClearOutput();

            var x = Player.CockThatFits(100);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Vapula.VapulaAndCeraphButtfuckTrainYeehaw.DisposeOmnibusFrontLine".Localized());

            OutputText("Scenes.Vapula.VapulaAndCeraphButtfuckTrainYeehaw.PushIntoCeraphAnal".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            if (silly)
                OutputText("Scenes.Vapula.VapulaAndCeraphButtfuckTrainYeehaw.ScreamingButtFuck".Localized());
            else
                OutputText("Scenes.Vapula.VapulaAndCeraphButtfuckTrainYeehaw.GroaningRavagePucker".Localized());

            OutputText("Scenes.Vapula.VapulaAndCeraphButtfuckTrainYeehaw.PeriodBreak".Localized());

            OutputText("Scenes.Vapula.VapulaAndCeraphButtfuckTrainYeehaw.BetweenThrustsOrderImitate".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Vapula.VapulaAndCeraphButtfuckTrainYeehaw.CeraphPoorThingCant".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Vapula.VapulaAndCeraphButtfuckTrainYeehaw.PumpMoreImpatient".Localized(
                Player.Mf("man", "herm")
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Vapula.VapulaAndCeraphButtfuckTrainYeehaw.RelentlesslySmashGroin".Localized());

            OutputText("Scenes.Vapula.VapulaAndCeraphButtfuckTrainYeehaw.EntiretyCrotchDrenched".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Vapula.VapulaAndCeraphButtfuckTrainYeehaw.StaminaDecreasingPressure".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Vapula.VapulaAndCeraphButtfuckTrainYeehaw.ClenchesAnusBabyBatter".Localized());

            OutputText("Scenes.Vapula.VapulaAndCeraphButtfuckTrainYeehaw.ExtraPressureColonCums".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Vapula.VapulaAndCeraphButtfuckTrainYeehaw.DumpButtslutsSplash".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, 1f));

            CoC.Instance.Flags.Set(GameFlagIds.VapulaDaysSinceFed, 0);

            CoC.Instance.Flags.Set(GameFlagIds.VapulaTeaseCount, 0);

            ReturnToCamp(1);
        }

        public void VapulaSophieThreesomeSelect()
        {

            if (Player.HasCock())
                VapulaSophieThreeSome();
            else
                VapulaSophieThreesomeCunts();
        }

        public void VapulaSophieThreeSome()
        {
            ClearOutput();

            var x = Player.CockThatFits(100);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Vapula.VapulaSophieThreeSome.CallSophieInsistently".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreeSome.HushTellFollow".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreeSome.VapulaCommentsAirBrained".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreeSome.IgnoringKeepArguing".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreeSome.LaughReassureLipstick".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreeSome.VapulaNodsChallenge".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreeSome.GrabFuckpillowsSandwich".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockArea(x) > 75)
                OutputText("Scenes.Vapula.VapulaSophieThreeSome.FilledEntiretyColon".Localized());
            else
                OutputText("Scenes.Vapula.VapulaSophieThreeSome.CockBuriedToHilt".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreeSome.BulgeGrindsButt".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Vapula.VapulaSophieThreeSome.StillNotBreakingKiss".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Vapula.VapulaSophieThreeSome.ShameToWasteTunnel".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreeSome.VapulaTakingCareGroin".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Vapula.VapulaSophieThreeSome.BallsAndMassages".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreeSome.AssWord".Localized());

            if (Player.Cocks.Count > 2)
                OutputText("Scenes.Vapula.VapulaSophieThreeSome.ExtraFlexibilityJerksThird".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreeSome.HardNipplesPoke".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreeSome.ExpertStimulationDrains".Localized());

            if (Player.CockTotal() < 3)
                OutputText("Scenes.Vapula.VapulaSophieThreeSome.HastilyPullOut".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreeSome.WaveWord".Localized());

            if (Player.CockTotal() >= 3)
                OutputText("Scenes.Vapula.VapulaSophieThreeSome.OneOfWord".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreeSome.YourStillPulsingDick".Localized());

            if (Player.CockTotal() >= 3)
                OutputText("Scenes.Vapula.VapulaSophieThreeSome.PluralS".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreeSome.SprayingPurpleBody".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreeSome.AllDoneDismount".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Vapula.VapulaSophieThreeSome.NoddingHeadsLeafBed".Localized());

            SceneInstance<SophieScene>.Get().LuststickApplication(10);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

            CoC.Instance.Flags.Set(GameFlagIds.VapulaDaysSinceFed, 0);

            CoC.Instance.Flags.Set(GameFlagIds.VapulaTeaseCount, 0);

            if (Player.Lust < 50)
                Player.Lust = 50;

            ReturnToCamp(1);
        }

        public void VapulaSophieThreesomeCunts()
        {
            ClearOutput();

            OutputText("Scenes.Vapula.VapulaSophieThreesomeCunts.CallSophieStrapOn".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreesomeCunts.SighsConsternation".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreesomeCunts.HushTellFollowAgain".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreesomeCunts.CommentsLazilyFingering".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreesomeCunts.ComplainingBlowLoad".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreesomeCunts.GoOnArch".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreesomeCunts.LicksLipsSmirks".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreesomeCunts.GrabFuckpillowsDildo".Localized(
                Appearance.AllBreastsDescript(Player),

                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Vapula.VapulaSophieThreesomeCunts.StillNotBreakingDildo".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreesomeCunts.ShiftsAttentionBreasts".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreesomeCunts.ExpertStimulationDrainsAgain".Localized());

            OutputText("Scenes.Vapula.VapulaSophieThreesomeCunts.AtLastAllDone".Localized(
                Player.VaginaDescript(0)
            ));

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

            CoC.Instance.Flags.Set(GameFlagIds.VapulaDaysSinceFed, 0);

            CoC.Instance.Flags.Set(GameFlagIds.VapulaTeaseCount, 0);

            ReturnToCamp(1);
        }

        public void VapulaAndAmilyThreesome()
        {
            ClearOutput();

            var x = Player.CockThatFits(100);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Vapula.VapulaAndAmilyThreesome.YellAmilyPurplishBlur".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Vapula.VapulaAndAmilyThreesome.OhButNotAlone".Localized());

            OutputText("Scenes.Vapula.VapulaAndAmilyThreesome.ThisIsVapulaMistress".Localized());

            OutputText("Scenes.Vapula.VapulaAndAmilyThreesome.MousyLooksDownIntimidated".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Vapula.VapulaAndAmilyThreesome.OrderHandsKnees".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Vapula.VapulaAndAmilyThreesome.DroolingPussyPrepared".Localized());

            OutputText("Scenes.Vapula.VapulaAndAmilyThreesome.SmirksShovesTumescent".Localized());

            OutputText("Scenes.Vapula.VapulaAndAmilyThreesome.GrabButtcheeksStretch".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Vapula.VapulaAndAmilyThreesome.TongueMagicalEffects".Localized());

            OutputText("Scenes.Vapula.VapulaAndAmilyThreesome.SlidesDeeperPreCum".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Vapula.VapulaAndAmilyThreesome.MadTriangleBreaks".Localized());

            OutputText("Scenes.Vapula.VapulaAndAmilyThreesome.AmilyDrinksTransformed".Localized());

            if (SceneInstance<AmilyScene>.Get().AmilyFurry())
                OutputText("Scenes.Vapula.VapulaAndAmilyThreesome.FurShrinkingRosySkin".Localized());

            OutputText("Scenes.Vapula.VapulaAndAmilyThreesome.ByLethiceReplica".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
            {
                OutputText("Scenes.Vapula.VapulaAndAmilyThreesome.HoarseBreathlessNotOver".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Vapula.VapulaAndAmilyThreesome.KeepsSuckingBalls".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.CumQ() >= 1500)
                    OutputText("Scenes.Vapula.VapulaAndAmilyThreesome.StickyGlobsWhiteGoo".Localized());

                OutputText("Scenes.Vapula.VapulaAndAmilyThreesome.OrgasmFadingImitate".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));
            }

            OutputText("Scenes.Vapula.VapulaAndAmilyThreesome.PatAmilyExcellentButtslut".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

            CoC.Instance.Flags.Set(GameFlagIds.VapulaDaysSinceFed, 0);

            CoC.Instance.Flags.Set(GameFlagIds.VapulaTeaseCount, 0);

            ReturnToCamp(1);
        }

        public void VapulaAssistsCeruleanSuccubus()
        {

            var x = Player.CockThatFits(100);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.ThatNight".Localized());

            OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.WakeVapulaGuest".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.AhThereYouAre".Localized());

            OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.LittleSurprisedMouthFull".Localized());

            OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.NotWorryPatHead".Localized(
                Player.Mf("man", "herm"),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.InvitationJumpYou".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.DropsFluidDrooling".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.RidingFerociouslyClamping".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.SuddenlyIdeaSecondCock".Localized());
            else
                OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.BlueDemonessLicks".Localized());

            OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.BothHellGirlsRiding".Localized());

            OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.DrunkSuccubiJuices".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.AvailableHoleSecondPenis".Localized());

            if (Player.CockTotal() > 2)
                OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.FeelsGoodTongueRedundant".Localized());

            OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.KeepPoundingCoating".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.PackTightSnatch".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.AndBlueLoversMouth".Localized());

                OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.LikewiseWord".Localized());
            }

            OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.JismSpilledBackside".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.AbruptlyPopOut".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.AskVapulaAgrees".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.EnclosingHeadCleavage".Localized());

            OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.ThatWasFunParty".Localized());

            OutputText("Scenes.Vapula.VapulaAssistsCeruleanSuccubus.KnowYouDefinitelyWill".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.VapulaDaysSinceFed, 0);

            CoC.Instance.Flags.Set(GameFlagIds.VapulaTeaseCount, 0);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Strength, Random.Shared.Next(2)), new DynStat(GameStats.Toughness, Random.Shared.Next(2)), new DynStat(GameStats.Speed, Random.Shared.Next(2)), new DynStat(GameStats.Intelligence, Random.Shared.Next(2)), new DynStat(GameStats.Corruption, 2.5f));

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.CeruleanPotion), 1);
        }

        public void VapulaJojoThreesomes()
        {
            ClearOutput();

            OutputText("Scenes.Vapula.VapulaJojoThreesomes.CallOutJungleSlut".Localized());

            OutputText("Scenes.Vapula.VapulaJojoThreesomes.WalkUpSlapWhimper".Localized());

            OutputText("Scenes.Vapula.VapulaJojoThreesomes.JojoMutterTerrified".Localized());

            OutputText("Scenes.Vapula.VapulaJojoThreesomes.AwwwLittlePetCrush".Localized());

            OutputText("Scenes.Vapula.VapulaJojoThreesomes.RightHeLikesMe".Localized());

            OutputText("Scenes.Vapula.VapulaJojoThreesomes.ShouldBeAshamedLust".Localized());

            OutputText("Scenes.Vapula.VapulaJojoThreesomes.KeepTauntingPoorMouse".Localized());

            OutputText("Scenes.Vapula.VapulaJojoThreesomes.HowUseFucktoy".Localized());

            OutputText("Scenes.Vapula.VapulaJojoThreesomes.CouldFuckInFront".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Vapula.VapulaJojoThreesomes.InitiateButtFuckTrain".Localized());

            ClearButtons();

            if (Player.HasCock() || (Player.HasVagina() && Player.HasKeyItem("Demonic Strap-On")))
                AddButton(0, "Common.Button.TeaseJojo".Localized(), NTRSomeJojos);

            if (Player.HasCock())
                AddButton(1, "Common.Button.Buttfucktrain".Localized(), JojoButtFuckTrain);
        }

        public void NTRSomeJojos()
        {
            ClearOutput();

            if (Player.HasCock())
            {
                var x = Player.CockThatFits(100);

                if (x < 0)
                    x = Player.SmallestCockIndex();

                OutputText("Scenes.Vapula.NTRSomeJojos.AdmitSmokingHotBody".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.JojoShakesHeadDenial".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.PunctuateNaturalGrope".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.ReachKneadTwinOrbs".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.TurnHeadPulpyLips".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.DartHandWetPussy".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.GrabWaistHipsThighs".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Vapula.NTRSomeJojos.RunHandsWholeBody".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Vapula.NTRSomeJojos.DecideAddDepravity".Localized());

                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Vapula.NTRSomeJojos.HornySlutCapacity".Localized());
                }
                else
                    OutputText("Scenes.Vapula.NTRSomeJojos.ParagraphBreak".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.PushBothHoles".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.StillLookingKeepTaunting".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.UnableToThinkProperly".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Vapula.NTRSomeJojos.PluralS".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.BeingFilledWithCock".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.IncapableTeasingJojoWeeps".Localized());

                if (SceneInstance<JojoScene>.Get().TentacleJojo())
                    OutputText("Scenes.Vapula.NTRSomeJojos.TentaclesFlyingTowardAss".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.HisWord".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.NoWord".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.StartledByCommand".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.DontCumBeforeMe".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.TooScaredRodentNods".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Vapula.NTRSomeJojos.ExtraJunkExplodes".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.AnalWallsMilk".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Vapula.NTRSomeJojos.MouseSlutSoiledOrder".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Vapula.NTRSomeJojos.BothCompletelyEmpty".Localized());
            }
            else
            {
                OutputText("Scenes.Vapula.NTRSomeJojos.CallJungleSlutAgain".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.SlapWhimperAgain".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.JojoMutterTerrifiedAgain".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.LittlePetCrushAgain".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.HeLikesMeAgain".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.ShouldBeAshamedWantonly".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.TauntPoorMouseAgain".Localized(
                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Vapula.NTRSomeJojos.SmokingHotBodyAgain".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.JojoDenialAgain".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.PunctuateNaturalGropeAgain".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.KneadTwinOrbsAgain".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.PulpyLipsKissAgain".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.DeliciousTastedSuccubus".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.KissVapulaAgain".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.FingerWetPussyAgain".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.GrabHipsThighsAgain".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.HandsWholeBodyAgain".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.WickedWantonTightAnus".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.StillTauntingAgain".Localized());

                if (SceneInstance<JojoScene>.Get().TentacleJojo())
                    OutputText("Scenes.Vapula.NTRSomeJojos.TentaclesFlyingAssAgain".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.HisWordAgain".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.NoWordAgain".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.StartledByCommandAgain".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.CumAfterMeAgain".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.RodentNodsAgain".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.HaventGivenAuthorization".Localized());

                OutputText("Scenes.Vapula.NTRSomeJojos.LaughAtSubmissivePosture".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.VapulaDaysSinceFed, 0);

            CoC.Instance.Flags.Set(GameFlagIds.VapulaTeaseCount, 0);

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, 4f));

            ReturnToCamp(1);
        }

        public void JojoButtFuckTrain()
        {
            ClearOutput();

            var x = Player.CockThatFits(100);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Vapula.JojoButtFuckTrain.TellJojoLuckyDay".Localized());

            OutputText("Scenes.Vapula.JojoButtFuckTrain.SmokingHotBodyThird".Localized());

            OutputText("Scenes.Vapula.JojoButtFuckTrain.JojoDenialThird".Localized());

            OutputText("Scenes.Vapula.JojoButtFuckTrain.TellVapulaPlaceHerself".Localized());

            OutputText("Scenes.Vapula.JojoButtFuckTrain.RememberTemporaryUse".Localized());

            OutputText("Scenes.Vapula.JojoButtFuckTrain.JojoMumblesFewWords".Localized());

            OutputText("Scenes.Vapula.JojoButtFuckTrain.LetsGetGoing".Localized());

            OutputText("Scenes.Vapula.JojoButtFuckTrain.SeizePressTipSphincter".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            if (Player.CockArea(x) > 100)
                OutputText("Scenes.Vapula.JojoButtFuckTrain.FeelInternalOrgans".Localized(
                    Player.CockDescript(x)
                ));

            OutputText("Scenes.Vapula.JojoButtFuckTrain.PumpHimFull".Localized());

            if (SceneInstance<JojoScene>.Get().TentacleJojo())
            {
                OutputText("Scenes.Vapula.JojoButtFuckTrain.BetweenButtAssaultsOrder".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Vapula.JojoButtFuckTrain.SoftKnockEntrance".Localized());
            }

            var n1 = -1;

            var n2 = -1;

            var n3 = -1;

            var temp = Player.CockTotal();

            while (temp > 0)
            {
                temp--;

                if (Player.Cocks[temp].Id == new Identifier("coc", "tentacle") && temp != x)
                {
                    if (n1 == -1)
                        n1 = temp;
                    else
                        if (n2 == -1)
                            n2 = temp;
                        else
                            if (n3 == -1)
                                n3 = temp;

                    break;
                }
            }

            if (n1 >= 0)
                OutputText("Scenes.Vapula.JojoButtFuckTrain.DeliciouslyDepravedIdea".Localized(
                    Player.CockDescript(n1),

                    Player.CockDescript(n1),

                    Player.CockDescript(x)
                ));

            if (n2 >= 0)
                OutputText("Scenes.Vapula.JojoButtFuckTrain.FreeHolesFillMore".Localized(
                    Player.CockDescript(n2),

                    Player.CockDescript(n2)
                ));

            if (n3 >= 0)
                OutputText("Scenes.Vapula.JojoButtFuckTrain.VapulaAlreadySucking".Localized(
                    Player.CockDescript(n3),

                    Player.CockDescript(x),

                    Player.CockDescript(n3),

                    Player.CockDescript(n3)
                ));

            if (n1 >= 0)
            {
                OutputText("Scenes.Vapula.JojoButtFuckTrain.PoundHarderEver".Localized());

                if (SceneInstance<JojoScene>.Get().TentacleJojo())
                    OutputText("Scenes.Vapula.JojoButtFuckTrain.EnjoyCrazyDisplay".Localized());
            }

            OutputText("Scenes.Vapula.JojoButtFuckTrain.WishBlissfulStateLast".Localized());

            if (n1 >= 0)
                OutputText("Scenes.Vapula.JojoButtFuckTrain.AndVapulaLoveTunnel".Localized());

            if (n2 >= 0)
                OutputText("Scenes.Vapula.JojoButtFuckTrain.AndThroatWord".Localized());

            if (n3 >= 0)
                OutputText("Scenes.Vapula.JojoButtFuckTrain.AndJojosToo".Localized());

            OutputText("Scenes.Vapula.JojoButtFuckTrain.FeelJojoRelease".Localized());

            if (SceneInstance<JojoScene>.Get().TentacleJojo())
                OutputText("Scenes.Vapula.JojoButtFuckTrain.TentaclesDrenchEveryHole".Localized());

            OutputText("Scenes.Vapula.JojoButtFuckTrain.VapulaOrgasmKicking".Localized());

            OutputText("Scenes.Vapula.JojoButtFuckTrain.SpendMinutesEcstasy".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Vapula.JojoButtFuckTrain.WakeUpJojoGone".Localized(
                Player.CockDescript(x)
            ));

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, 2f));

            CoC.Instance.Flags.Set(GameFlagIds.VapulaDaysSinceFed, 0);

            CoC.Instance.Flags.Set(GameFlagIds.VapulaTeaseCount, 0);

            ReturnToCamp(1);
        }

        public void VapulaAndIzmaThreeSome()
        {
            ClearOutput();

            if (Player.HasCock())
                VapulaAndIzma(false);
            else
                VapulaAndIzma(true);
        }

        public void VapulaAndIzma(bool girls = true)
        {
            ClearOutput();

            var x = Player.CockThatFits(100);

            if (x < 0)
                x = Player.SmallestCockIndex();

            if (!girls)
            {
                OutputText("Scenes.Vapula.VapulaAndIzma.CallBetaTigershark".Localized());

                OutputText("Scenes.Vapula.VapulaAndIzma.GentlyExplainExtraFun".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Vapula.VapulaAndIzma.RelaxRailPucker".Localized());

                OutputText("Scenes.Vapula.VapulaAndIzma.ImNotUsedTo".Localized());

                OutputText("Scenes.Vapula.VapulaAndIzma.AsIfChoiceBeta".Localized());

                OutputText("Scenes.Vapula.VapulaAndIzma.IzmaTriesStruggle".Localized());

                OutputText("Scenes.Vapula.VapulaAndIzma.KnewHowLongWanting".Localized());

                OutputText("Scenes.Vapula.VapulaAndIzma.TauntsDontSeemAffect".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Vapula.VapulaAndIzma.RoarTriumphForceInside".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Vapula.VapulaAndIzma.MindTornPleasure".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Vapula.VapulaAndIzma.GiveTigersharkAnotherThrust".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Vapula.VapulaAndIzma.NoddingPullOutOwn".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Vapula.VapulaAndIzma.StareIzmaEyeContact".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));
            }
            else
            {
                OutputText("Scenes.Vapula.VapulaAndIzma.CallBetaTigersharkAgain".Localized());

                OutputText("Scenes.Vapula.VapulaAndIzma.GentlyExplainGorilla".Localized());

                OutputText("Scenes.Vapula.VapulaAndIzma.WaitWhatDoing".Localized());

                OutputText("Scenes.Vapula.VapulaAndIzma.RelaxRailPuckerAgain".Localized());

                OutputText("Scenes.Vapula.VapulaAndIzma.NotUsedToAgain".Localized());

                OutputText("Scenes.Vapula.VapulaAndIzma.NoChoiceBetaAgain".Localized());

                OutputText("Scenes.Vapula.VapulaAndIzma.IzmaTriesStruggleAgain".Localized());

                OutputText("Scenes.Vapula.VapulaAndIzma.YouSighFitMuscular".Localized());

                OutputText("Scenes.Vapula.VapulaAndIzma.TauntsNoEffectAgain".Localized());

                OutputText("Scenes.Vapula.VapulaAndIzma.RoarTriumphArtificialLength".Localized());

                OutputText("Scenes.Vapula.VapulaAndIzma.SkilledSuccubusNodsClamping".Localized(
                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Vapula.VapulaAndIzma.MindTornAgain".Localized());

                OutputText("Scenes.Vapula.VapulaAndIzma.WantCumFromButt".Localized());

                OutputText("Scenes.Vapula.VapulaAndIzma.ExtraStimulationEnough".Localized());

                OutputText("Scenes.Vapula.VapulaAndIzma.NoddingPullDildoOut".Localized(
                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Vapula.VapulaAndIzma.ThreeGoAtIt".Localized());

                OutputText("Scenes.Vapula.VapulaAndIzma.HappilyDevoursAll".Localized(
                    Player.VaginaDescript(0)
                ));
            }

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, 2f));

            CoC.Instance.Flags.Set(GameFlagIds.VapulaDaysSinceFed, 0);

            CoC.Instance.Flags.Set(GameFlagIds.VapulaTeaseCount, 0);

            ReturnToCamp(1);
        }

        public void ChixFeedVapulaBlehblehIVantToZuckYourSpooo()
        {
            ClearOutput();

            OutputText("Scenes.Vapula.ChixFeedVapulaBlehblehIVantToZuckYourSpooo.ProduceDemonicDildo".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Feed".Localized(), ChicksFeedVapula);

            if (CoC.Instance.Flags.Get(GameFlagIds.VapulaHaremFuck) == 0)
                AddButton(1, "Common.Button.Tease".Localized(), TeaseVapula);
            else
                AddButton(1, "Common.Button.Tease".Localized(), null);
        }

        public void TeaseVapula()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.VapulaHaremFuck) == 0 && (CoC.Instance.Flags.Get(GameFlagIds.VapulaDaysSinceFed) == 0))
            {
                OutputText("Scenes.Vapula.TeaseVapula.DecideNotGiveEasily".Localized(
                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Vapula.TeaseVapula.OpenEyesBitingLip".Localized());

                OutputText("Scenes.Vapula.TeaseVapula.YouAskHowBadly".Localized());

                OutputText("Scenes.Vapula.TeaseVapula.SheRepliesVeryBadly".Localized());

                OutputText("Scenes.Vapula.TeaseVapula.PerhapsDescribeIt".Localized());

                OutputText("Scenes.Vapula.TeaseVapula.SwallowsThirstyHungryHorny".Localized());

                OutputText("Scenes.Vapula.TeaseVapula.HowInterestingAllSuccubae".Localized());

                OutputText("Scenes.Vapula.TeaseVapula.GritsVapulaFix".Localized());

                OutputText("Scenes.Vapula.TeaseVapula.WatchesCockRecede".Localized());

                OutputText("Scenes.Vapula.TeaseVapula.WaitNoDidntMean".Localized());

                OutputText("Scenes.Vapula.TeaseVapula.TurnStrideAwayLaughing".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Sensitivity / 10)));

                CoC.Instance.Flags.Set(GameFlagIds.VapulaTeaseCount, CoC.Instance.Flags.Get(GameFlagIds.VapulaTeaseCount) + 1);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.VapulaHaremFuck) == 0 && CoC.Instance.Flags.Get(GameFlagIds.VapulaDaysSinceFed) == 1)
                {
                    OutputText("Scenes.Vapula.TeaseVapula.SmileOnceAgainPosition".Localized());

                    OutputText("Scenes.Vapula.TeaseVapula.BreathingHeavilyStruggling".Localized());

                    OutputText("Scenes.Vapula.TeaseVapula.TeaseHungryByNow".Localized());

                    OutputText("Scenes.Vapula.TeaseVapula.SheRepliesWetlyRub".Localized());

                    OutputText("Scenes.Vapula.TeaseVapula.HardMilkingDick".Localized());

                    OutputText("Scenes.Vapula.TeaseVapula.VapulaGrowlsInsightCruelty".Localized());

                    OutputText("Scenes.Vapula.TeaseVapula.PurrStoppingMinistrations".Localized());

                    if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Hoofed || Player.IsTaur())
                        OutputText("Scenes.Vapula.TeaseVapula.WorshipFeetCleanHooves".Localized());
                    else
                        if (Player.HasTailInsteadOfLegs())
                            OutputText("Scenes.Vapula.TeaseVapula.WorshipScalesBuff".Localized());
                        else
                            if (Player.IsDrider())
                                OutputText("Scenes.Vapula.TeaseVapula.WorshipEggDump".Localized());
                            else
                                if (Player.IsGoo())
                                    OutputText("Scenes.Vapula.TeaseVapula.WorshipEngulfEvery".Localized());
                                else
                                    if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Dog || Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Cat || Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Lizard || Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Kangaroo || Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Fox)
                                        OutputText("Scenes.Vapula.TeaseVapula.WorshipFeetClaws".Localized());
                                    else
                                        if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Bee || Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.ChitinousSpiderLegs)
                                            OutputText("Scenes.Vapula.TeaseVapula.WorshipFeetBoots".Localized());
                                        else
                                            OutputText("Scenes.Vapula.TeaseVapula.WorshipFeetThemClean".Localized());

                    OutputText("Scenes.Vapula.TeaseVapula.SpeaksFeverishlyLaugh".Localized());

                    OutputText("Scenes.Vapula.TeaseVapula.PauseRunFertileImagination".Localized(
                        Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Vapula.TeaseVapula.GuessFindOutTomorrow".Localized());

                    OutputText("Scenes.Vapula.TeaseVapula.TurnWalkAwayWail".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.VapulaTeaseCount, CoC.Instance.Flags.Get(GameFlagIds.VapulaTeaseCount) + 1);
                }
                else
                {
                    OutputText("Scenes.Vapula.TeaseVapula.OnceAgainBuckleUp".Localized());

                    OutputText("Scenes.Vapula.TeaseVapula.AnswerOpensMouth".Localized());

                    OutputText("Scenes.Vapula.TeaseVapula.SurgesForwardsManners".Localized());

                    OutputText("Scenes.Vapula.TeaseVapula.SwallowingHiltSavagely".Localized());

                    OutputText("Scenes.Vapula.TeaseVapula.DecidingWorthAdvantage".Localized());

                    if (Player.HasTailInsteadOfLegs())
                        OutputText("Scenes.Vapula.TeaseVapula.SlideTailCoils".Localized());
                    else
                        if (Player.IsTaur() || Player.IsGoo() || Player.IsDrider())
                            OutputText("Scenes.Vapula.TeaseVapula.SitMonstrousHalf".Localized());
                        else
                            OutputText("Scenes.Vapula.TeaseVapula.OpenLegsWrapAround".Localized());

                    OutputText("Scenes.Vapula.TeaseVapula.WorksOnItHours".Localized(
                        Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Vapula.TeaseVapula.StaminaSavagelyReward".Localized());

                    OutputText("Scenes.Vapula.TeaseVapula.HazeStrapOnReward".Localized());

                    OutputText("Scenes.Vapula.TeaseVapula.BadFilthyCumslutStern".Localized());

                    OutputText("Scenes.Vapula.TeaseVapula.TrainMeSendAway".Localized());

                    Player.Orgasm("Generic");

                    CoC.Instance.Flags.Set(GameFlagIds.VapulaDaysSinceFed, 0);

                    CoC.Instance.Flags.Set(GameFlagIds.VapulaTeaseCount, 0);
                }

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            ReturnToCamp(1);
        }

        public void ChicksFeedVapula()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.VapulaTeaseCount) > 1)
                OutputText("Scenes.Vapula.ChicksFeedVapula.PushPinkEndHarness".Localized());
            else
                OutputText("Scenes.Vapula.ChicksFeedVapula.TellRewardGoodSlut".Localized());

            OutputText("Scenes.Vapula.ChicksFeedVapula.VoraciouslySlurpsLaugh".Localized());

            OutputText("Scenes.Vapula.ChicksFeedVapula.EvidentlyOtherIdeas".Localized());

            OutputText("Scenes.Vapula.ChicksFeedVapula.ShakeHeadInWonder".Localized());

            OutputText("Scenes.Vapula.ChicksFeedVapula.WorksHoursAgain".Localized());

            OutputText("Scenes.Vapula.ChicksFeedVapula.HazeUndoingStrapOn".Localized());

            ReturnToCamp(1);

            Player.Orgasm("Vaginal");

            CoC.Instance.Flags.Set(GameFlagIds.VapulaDaysSinceFed, 0);

            CoC.Instance.Flags.Set(GameFlagIds.VapulaTeaseCount, 0);
        }

        public void VapulaForceFeeds()
        {

            if (Player.HasVagina() && Player.HasKeyItem("Demonic Strap-On") && !Player.HasCock())
            {
                OutputText("Scenes.Vapula.VapulaForceFeeds.InTheNight".Localized());

                OutputText("Scenes.Vapula.VapulaForceFeeds.ArousalBloomsDreams".Localized(
                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Vapula.VapulaForceFeeds.WolfishGroanHilt".Localized());

                if (Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Vapula.VapulaForceFeeds.SlideTailCoilsAgain".Localized());
                else
                    if (Player.IsTaur() || Player.IsGoo() || Player.IsDrider())
                        OutputText("Scenes.Vapula.VapulaForceFeeds.SitMonstrousHalfAgain".Localized());
                    else
                        OutputText("Scenes.Vapula.VapulaForceFeeds.LegsWrapAroundAgain".Localized());

                OutputText("Scenes.Vapula.VapulaForceFeeds.WorksHoursThird".Localized(
                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Vapula.VapulaForceFeeds.StaminaSavagelyAgain".Localized());

                OutputText("Scenes.Vapula.VapulaForceFeeds.HazeStrapOnAgain".Localized());

                Player.Orgasm("Vaginal");

                Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

                CoC.Instance.Flags.Set(GameFlagIds.VapulaDaysSinceFed, 0);

                CoC.Instance.Flags.Set(GameFlagIds.VapulaTeaseCount, 0);

                CoC.Instance.Flags.Set(GameFlagIds.VapulaEarnedASpank, 1);
            }
            else
                if (Player.HasCock())
                {
                    var x = Player.CockThatFits(100);

                    if (x < 0)
                        x = Player.SmallestCockIndex();

                    OutputText("Scenes.Vapula.VapulaForceFeeds.AwakenedByTingle".Localized());

                    OutputText("Scenes.Vapula.VapulaForceFeeds.WorksHoursFourth".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Vapula.VapulaForceFeeds.StaminaSavagelyThird".Localized(
                        Player.CockDescript(x)
                    ));

                    if (Player.Balls > 0)
                        OutputText("Scenes.Vapula.VapulaForceFeeds.YourBallsChurnAgain".Localized());
                    else
                        OutputText("Scenes.Vapula.VapulaForceFeeds.YourCockTwitchesAgain".Localized(
                            Player.CockDescript(x)
                        ));

                    OutputText("Scenes.Vapula.VapulaForceFeeds.ReleaseStickySpoogeAgain".Localized());

                    OutputText("Scenes.Vapula.VapulaForceFeeds.LipsKeepSqueezingJunk".Localized());

                    if (Player.CumQ() >= 1500)
                        OutputText("Scenes.Vapula.VapulaForceFeeds.SixMonthsPregnantAgain".Localized(
                            Player.CockDescript(x)
                        ));

                    Player.Orgasm("Dick");

                    Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

                    CoC.Instance.Flags.Set(GameFlagIds.VapulaDaysSinceFed, 0);

                    CoC.Instance.Flags.Set(GameFlagIds.VapulaTeaseCount, 0);

                    CoC.Instance.Flags.Set(GameFlagIds.VapulaEarnedASpank, 1);
                }

            Go<CampScene>();
        }

        public void SpankVapulaLikeABoss()
        {
            ClearOutput();

            OutputText("Scenes.Vapula.SpankVapulaLikeABoss.WithoutWarningGrabWrists".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Vapula.SpankVapulaLikeABoss.HealthySurgeBlood".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Vapula.SpankVapulaLikeABoss.TelltaleMoistnessNethers".Localized());

            OutputText("Scenes.Vapula.SpankVapulaLikeABoss.SentencePeriod".Localized());

            OutputText("Scenes.Vapula.SpankVapulaLikeABoss.CommandBeStillWhimpering".Localized());

            OutputText("Scenes.Vapula.SpankVapulaLikeABoss.VapulaCriesLemmeGo".Localized());

            OutputText("Scenes.Vapula.SpankVapulaLikeABoss.WellHandLiftPalm".Localized());

            if (CampScene.CompanionsCount() > 2)
                OutputText("Scenes.Vapula.SpankVapulaLikeABoss.DrawingEyesCompanions".Localized());
            else
                if (CampScene.CompanionsCount() == 2)
                    OutputText("Scenes.Vapula.SpankVapulaLikeABoss.DrawingAttentionCompanion".Localized());
                else
                    OutputText("Scenes.Vapula.SpankVapulaLikeABoss.EchoingSentinelRocks".Localized());

            OutputText("Scenes.Vapula.SpankVapulaLikeABoss.TailGoesWildFrenzy".Localized());

            OutputText("Scenes.Vapula.SpankVapulaLikeABoss.ThisTimeCocksReleases".Localized());

            OutputText("Scenes.Vapula.SpankVapulaLikeABoss.GrowlAdmireReddening".Localized());

            if (Player.HasPerk(PerkIds.Sadist))
                OutputText("Scenes.Vapula.SpankVapulaLikeABoss.DishingOutSufferingHot".Localized());

            OutputText("Scenes.Vapula.SpankVapulaLikeABoss.ScrunchesEyesShut".Localized());

            OutputText("Scenes.Vapula.SpankVapulaLikeABoss.InterruptScoldingSwore".Localized());

            OutputText("Scenes.Vapula.SpankVapulaLikeABoss.AttackBottomBarrage".Localized());

            OutputText("Scenes.Vapula.SpankVapulaLikeABoss.SuccubusPantingGasping".Localized());

            OutputText("Scenes.Vapula.SpankVapulaLikeABoss.AppealingOfferDefeatPoint".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Vapula.SpankVapulaLikeABoss.MalenessWord".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Vapula.SpankVapulaLikeABoss.VaginaWord".Localized());
                else
                    OutputText("Scenes.Vapula.SpankVapulaLikeABoss.TinglingAnusWord".Localized());

            OutputText("Scenes.Vapula.SpankVapulaLikeABoss.BringPalmBackDown".Localized());

            OutputText("Scenes.Vapula.SpankVapulaLikeABoss.ShesGoingToCum".Localized());

            OutputText("Scenes.Vapula.SpankVapulaLikeABoss.DemonessFlopsOff".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 7)));

            if (Player.HasPerk(PerkIds.Sadist))
                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 7)));

            CoC.Instance.Flags.Set(GameFlagIds.VapulaEarnedASpank, 0);

            ReturnToCamp(1);
        }

        public void IdLiddellium()
        {

            OutputText("Scenes.Vapula.IdLiddellium.RetrieveOddPhial".Localized());

            OutputText("Scenes.Vapula.IdLiddellium.WorriesEvidentSeverity".Localized());

            OutputText("Scenes.Vapula.IdLiddellium.ShuddersBeggingForgiveness".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.LiddelliumFlag, -1);

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get());
        }

        public void VapulaRebeccAbuse()
        {
            ClearOutput();

            OutputText("Scenes.Vapula.VapulaRebeccAbuse.StandsSubservientEager".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse.SackOfMeatDemons".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse.IdiotWasYou".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse.MustSenseAnger".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse.FreezesPetrifiedOwca".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse.FortunatelyHaveIdea".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse.WalkBackSilent".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse.HesitantlyTakesThem".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse.RepliesPickingOutfit".Localized(
                Player.Tallness < 70 ? "Though it's a little small for her, it draws attention to her curves like nothing else. At least the town won't be looking at her face." : (Player.Tallness > 78 ? "Though it's a little big for her, hopefully the loose fabric will help mask her true identity." : "You thought this was the right fit, but you suppose anything must feel odd after wearing nearly nothing for years.")
            ));

            OutputText("Scenes.Vapula.VapulaRebeccAbuse.HaveToDoRebecc".Localized());

            VapulaRebeccAbuse2();
        }

        public void VapulaRebeccAbuse2()
        {
            ClearOutput();

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.DisguisedSetOffPlains".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.SomethingDrawsOutHead".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.WorkWayOtherSide".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.StomachTumblesRebeccsHome".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.DoorOpensInstant".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.NeedsNoAnswerDirty".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.HardDoAnythingMilk".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.InDistanceDoorOpens".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.SheLeavesLieBack".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.FeelsLikeHoursHero".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.SightOutsideBathroom".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.PausesNibblingThroat".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.SheepGirlWatches".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.OfCourseLittleToy".Localized(
                Player.HasTailInsteadOfLegs() ? "up your underbelly" : (Player.IsDrider() ? "up your chitinous body" : (Player.IsGoo() ? "into your slime" : "up your thigh"))
            ));

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.HopefullyAssumesAnger".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.NoNeedForViolence".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.ShuddersInArmsWoolly".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.MomentInterruptedYanks".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.RemainStillAppearance".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.AndThatsItIncapable".Localized(
                Player.IsDrider() ? "pin her to the ground with your arachnid legs" : "place your " + (Player.HasTailInsteadOfLegs() ? "[if (isNaga) {serpentine|long}] tail" : (Player.IsGoo() ? "gelatinous base" : "knee") + " on her back")
            ));

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.TrulyYoursPlush".Localized(
                Player.IsGoo() || Player.HasTailInsteadOfLegs() ? "press your weight into her" : "dig your " + (Player.IsDrider() ? "leg" : "knee") + " into her back"
            ));

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.HidesFaceCallSuccubus".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.SlightPartLipsClaim".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.GroanEscapesRollsNipple".Localized(
                Player.Vaginas[0].ClitLength > 2 ? "Your engorged, inhuman clitoris rising out of its hood" : (Player.Vaginas[0].Wetness >= 4 ? "The damp spot spreading across Rebecc's back" : "The way you shiver with every shaky breath")
            ));

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.TinyWhimperThumb".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse2.JustThePersonMind".Localized());

            VapulaRebeccAbuse3();
        }

        public void VapulaRebeccAbuse3()
        {

            OutputText("Scenes.Vapula.VapulaRebeccAbuse3.BeneathMotionlessSobs".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse3.AnswerComesQuickly".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse3.BodyShakesYankDress".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse3.BarelyControlPress".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse3.FewSecondsToRest".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse3.WipeFingersClean".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse3.HonestDeliciousTerror".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse3.ShudderingHeatAching".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse3.PleasantSilencePleading".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse3.TensesUpTracesBones".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse3.MoveAsOne".Localized(
                Player.HasTailInsteadOfLegs() ? "grip falters" : (Player.IsGoo() ? "form trembles" : "legs shake")
            ));

            OutputText("Scenes.Vapula.VapulaRebeccAbuse3.NotEnoughNeedMore".Localized());

            OutputText("Scenes.Vapula.VapulaRebeccAbuse3.ShiverIntoArmsBreath".Localized(
                Player.IsGoo() ? "core" : (Player.Body.Get<SkinPart>().Id == BodyPartTypes.Plain ? "skin" : "nerves")
            ));

            OutputText("Scenes.Vapula.VapulaRebeccAbuse3.WordlesslyHelpsSingleLeg".Localized(
                Player.HasTailInsteadOfLegs() ? "tail" : (Player.IsGoo() ? "body" : "foot")
            ));

            OutputText("Scenes.Vapula.VapulaRebeccAbuse3.BeforeLeavingSheep".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 8f));

            CoC.Instance.Flags.Set(GameFlagIds.OwcaUnlocked, -1);

            ReturnToCamp(1);
        }

        public void VapulaForceLick()
        {
            ClearOutput();

            OutputText("Scenes.Vapula.VapulaForceLick.NotHereToFeed".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.VapulaHaremFuck) > 0)
            {
                OutputText("Scenes.Vapula.VapulaForceLick.LooksLikeWantsProtest".Localized());

                OutputText("Scenes.Vapula.VapulaForceLick.SurprisinglyWithoutReluctance".Localized());
            }
            else
            {
                OutputText("Scenes.Vapula.VapulaForceLick.NeedSqueezeTighter".Localized());

                OutputText("Scenes.Vapula.VapulaForceLick.AsYouCommandMutters".Localized());

                OutputText("Scenes.Vapula.VapulaForceLick.AsPerhapsExpectedDispleasure".Localized());
            }

            OutputText("Scenes.Vapula.VapulaForceLick.FilledAromaSnatch".Localized());

            OutputText("Scenes.Vapula.VapulaForceLick.HastyMessyRabidDog".Localized());

            OutputText("Scenes.Vapula.VapulaForceLick.SeekingEyeContactLessening".Localized());

            OutputText("Scenes.Vapula.VapulaForceLick.WorkingWellGrowingClose".Localized());

            OutputText("Scenes.Vapula.VapulaForceLick.PursuitHigherPleasureShove".Localized());

            OutputText("Scenes.Vapula.VapulaForceLick.SweatingPantingRideFace".Localized());

            OutputText("Scenes.Vapula.VapulaForceLick.AtLastDismountLips".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Vapula.VapulaForceLick.DrunkenlyHeavesPuddle".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.VapulaDaysSinceFed) > 0)
                    CoC.Instance.Flags.Set(GameFlagIds.VapulaDaysSinceFed, CoC.Instance.Flags.Get(GameFlagIds.VapulaDaysSinceFed) - 1);
            }
            else
                OutputText("Scenes.Vapula.VapulaForceLick.DrunkenlyRisesKissMons".Localized());

            Player.Orgasm("Vaginal");

            ReturnToCamp(1);
        }

        public void VapulaVaginal()
        {
            ClearOutput();

            OutputText("Scenes.Vapula.VapulaVaginal.ExamineAdmiringHips".Localized());

            OutputText("Scenes.Vapula.VapulaVaginal.ExpectantWrithesUnderGaze".Localized());

            OutputText("Scenes.Vapula.VapulaVaginal.DragOutOrStart".Localized());

            ClearButtons();

            AddNextButton("Common.Button.DemeanHer".Localized(), () => VapulaVaginal2(true));

            AddNextButton("Common.Button.GoForIt".Localized(), () => VapulaVaginal2(false));
        }

        public void VapulaVaginal2(bool choice)
        {
            ClearOutput();

            if (choice)
            {
                OutputText("Scenes.Vapula.VapulaVaginal2.MovingHandPropCock".Localized());

                OutputText("Scenes.Vapula.VapulaVaginal2.JustLetMe".Localized());

                OutputText("Scenes.Vapula.VapulaVaginal2.InstantlyCutOffReprimanding".Localized());

                OutputText("Scenes.Vapula.VapulaVaginal2.TellGrovelServile".Localized());

                OutputText("Scenes.Vapula.VapulaVaginal2.ShouldNeverBeenPresumptuous".Localized());
            }
            else
            {
                OutputText("Scenes.Vapula.VapulaVaginal2.FigureRaringToGo".Localized());

                OutputText("Scenes.Vapula.VapulaVaginal2.GiveMeYourCock".Localized());

                OutputText("Scenes.Vapula.VapulaVaginal2.LookInEyesPitiful".Localized());
            }

            OutputText("Scenes.Vapula.VapulaVaginal2.SheTalksWalkAround".Localized());

            OutputText("Scenes.Vapula.VapulaVaginal2.HookThumbsStretchLips".Localized());

            OutputText("Scenes.Vapula.VapulaVaginal2.EagerRockBackControl".Localized(
                Player.Tallness > 60 ? "putting your full weight into her as you " + (Player.Tallness > 70 ? "engulf her in your smothering embrace, the much smaller succubus completely covered by your body" : "press tight against the lusty demon, flattening her completely") : "pressing your head tight against her as you wrap your arms around her middle"
            ));

            OutputText("Scenes.Vapula.VapulaVaginal2.DelightedRoughTreatment".Localized());

            OutputText("Scenes.Vapula.VapulaVaginal2.YesLethiceAmazing".Localized());

            OutputText("Scenes.Vapula.VapulaVaginal2.GruntRamIntoHer".Localized());

            OutputText("Scenes.Vapula.VapulaVaginal2.JustPleaseCum".Localized());

            OutputText("Scenes.Vapula.VapulaVaginal2.RelentRequestSurgeFills".Localized(
                Player.CumQ() < 500 ? "your cock continues to spurt until the last drop has been drained" : (Player.CumQ() < 1000 ? "you flood her completely, causing your semen to overflow her cunt and spill out onto the ground below" : "you absolutely inundate her loaded cunt, your jizz flows freely from it and splattering her thighs"),

                Player.CumQ() < 500 ? "delighted" : (Player.CumQ() < 1000 ? "overjoyed" : "completely blissed-out")
            ));

            OutputText("Scenes.Vapula.VapulaVaginal2.FinallyFreeLastSplurt".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, .5f), new DynStat(GameStats.Libido, .5f));

            CoC.Instance.Flags.Set(GameFlagIds.VapulaDaysSinceFed, 0);

            ReturnToCamp(1);
        }

    }
}