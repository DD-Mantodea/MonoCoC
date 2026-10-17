using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Utils;
using MonoCoC.Contents.Followers;
using MonoCoC.Contents.Scenes.Vanilla.Places;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<MilkWaifuScene>("coc:milk_waifu")]
    public class MilkWaifuScene : Scene
    {

        public static SaveContent Save => SaveContent(SceneIds.Npcs.MilkWaifu);

        public string debugName => !string.IsNullOrEmpty(milkName) ? milkName : "Milk Slave";

        public string debugHint => "";

        public string milkName { get => CoC.Instance.Flags.Get(GameFlagIds.MilkName); set => CoC.Instance.Flags.Set(GameFlagIds.MilkName, value); }

        public override void Entry()
        {
            MilkSlave();
        }

        public void Reset()
        {

            Save.SetInt("talkedSlut", 0);
        }

        public bool MilkSlave()
        {

            return milkName.Length > 0;
        }

        public bool MilkSlaveAtCamp()
        {

            return MilkSlave() && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmBathGirl) == 0;
        }

        public void ArriveWithLacticWaifuAtCamp()
        {
            ClearOutput();

            OutputText("Scenes.Milkwaifu.ArriveWithLacticWaifuAtCamp.SlowGoingHavingTo".Localized());

            OutputText("Scenes.Milkwaifu.ArriveWithLacticWaifuAtCamp.SeeingThePool".Localized());

            OutputText("Scenes.Milkwaifu.ArriveWithLacticWaifuAtCamp.SpeakingOfWhich".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), NameZeMilkBath);

        }

        public void NameZeMilkBath()
        {
            ClearOutput();

            var input = "" ;

            if (input == "")
            {
                OutputText("Scenes.Milkwaifu.NameZeMilkBath.MustGiveHerA".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), NameZeMilkBath);

                return ;
            }

            milkName = input;

            if (milkName == "Bath Slut")
                OutputText("Scenes.Milkwaifu.NameZeMilkBath.FuckItBathSlut".Localized());
            else
                if (milkName == "Biscuit")
                    OutputText("Scenes.Milkwaifu.NameZeMilkBath.MayAsWell".Localized());
                else
                {
                    OutputText("Scenes.Milkwaifu.NameZeMilkBath.CrouchDownBeside".Localized());

                    OutputText("Scenes.Milkwaifu.NameZeMilkBath.HesitantlyRepeats".Localized());
                }

            if (SceneInstance<RathazulScene>.Get().FollowerRathazul() && SceneInstance<RathazulScene>.Get().mixologyXP >= 16)
            {
                OutputText("Scenes.Milkwaifu.NameZeMilkBath.FinishDecidingWhat".Localized());

                OutputText("Scenes.Milkwaifu.NameZeMilkBath.ShoutsFlailingAround".Localized());

                OutputText("Scenes.Milkwaifu.NameZeMilkBath.WhatDraggedIn".Localized());

                OutputText("Scenes.Milkwaifu.NameZeMilkBath.MewlsReachingBack".Localized());

                OutputText("Scenes.Milkwaifu.NameZeMilkBath.SaysShakilyKneeling".Localized());

                OutputText("Scenes.Milkwaifu.NameZeMilkBath.ReachesIntoRobes".Localized());

                OutputText("Scenes.Milkwaifu.NameZeMilkBath.HobblesOffBack".Localized());

                OutputText("Scenes.Milkwaifu.NameZeMilkBath.MumblesShiftingTeats".Localized());

                Player.CreateKeyItem("Super Reducto", 0, 0, 0, 0);

                OutputText("Scenes.Milkwaifu.NameZeMilkBath.SuperReducto".Localized());
            }
            else
            {
                OutputText("Scenes.Milkwaifu.NameZeMilkBath.FinishDecidingWhatAgain".Localized());

                OutputText("Scenes.Milkwaifu.NameZeMilkBath.EitherWayYouFigure".Localized());
            }

            ReturnToCamp(1);
        }

        public void Ratducto()
        {
            ClearOutput();

            OutputText("Scenes.Milkwaifu.Ratducto.OldRatApproaches".Localized());

            OutputText("Scenes.Milkwaifu.Ratducto.ShoutsFlailingAround".Localized());

            OutputText("Scenes.Milkwaifu.Ratducto.WhatDraggedIn".Localized());

            OutputText("Scenes.Milkwaifu.Ratducto.MewlsReachingBack".Localized());

            OutputText("Scenes.Milkwaifu.Ratducto.SaysShakilyKneeling".Localized());

            OutputText("Scenes.Milkwaifu.Ratducto.ReachesIntoRobes".Localized());

            OutputText("Scenes.Milkwaifu.Ratducto.HobblesOffBack".Localized());

            OutputText("Scenes.Milkwaifu.Ratducto.MumblesShiftingTeats".Localized());

            Player.CreateKeyItem("Super Reducto", 0, 0, 0, 0);

            OutputText("Scenes.Milkwaifu.Ratducto.SuperReducto".Localized());

            Go<CampScene>();
        }

        public void MilkyMenu()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmBathGirl) == 0)
            {
                OutputText("Scenes.Milkwaifu.MilkyMenu.WanderOverToPool".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 0)
                    OutputText("Scenes.Milkwaifu.MilkyMenu.LyingFaceDown".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 1)
                        OutputText("Scenes.Milkwaifu.MilkyMenu.ArmsCrossedUnder".Localized());
                    else
                        OutputText("Scenes.Milkwaifu.MilkyMenu.LongTannedLegs".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 0)
                {
                    OutputText("Scenes.Milkwaifu.MilkyMenu.ShakesOutOfBoob".Localized());

                    OutputText("Scenes.Milkwaifu.MilkyMenu.BathTime".Localized());
                }
                else
                {
                    OutputText("Scenes.Milkwaifu.MilkyMenu.SmilesAsYouPick".Localized());

                    OutputText("Scenes.Milkwaifu.MilkyMenu.SomethingYouNeed".Localized());
                }

            if ((CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmBathGirl) == 0 && CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionStarted) == 1) && (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 0))
                    OutputText("Scenes.Milkwaifu.MilkyMenu.AlthoughMassiveLactation".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmBathGirl) == 1 && CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 0)
                OutputText("Scenes.Milkwaifu.MilkyMenu.YoudHaveToFind".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), MilkWriteFuAppearance);

            AddButton(1, "Common.Button.Talk".Localized(), MilkTryTalking);

            if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) < 2)
                AddButton(5, "Common.Button.MilkBath".Localized(), MilkBathTime);

            if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) > 0)
                AddRowButton(1, "Milk Time!", NyanCatMilkTime);

            if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) > 0 && Player.Lust >= 33 && Player.HasCock())
                AddRowButton(1, "Titfuck", TitFuckDatMilkSlut);

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmBathGirl) == 0 && CoC.Instance.Flags.Get(GameFlagIds.MilkSize) > 0 && CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionStarted) == 1)
                AddButton(10, "Common.Button.FarmWork".Localized(), SendToFarm);

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmBathGirl) == 1 && CoC.Instance.Flags.Get(GameFlagIds.MilkSize) > 0)
                AddButton(10, "Common.Button.GoCamp".Localized(), BackToCamp);

            if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) < 2 && Player.HasKeyItem("Super Reducto"))
                AddRowButton(2, "Reducto", SuperReductoUsage);

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmBathGirl) == 0)
                AddButton(14, "Common.Button.Back".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmBathGirl) == 1)
                AddButton(14, "Common.Button.Back".Localized(), null );
        }

        public void SendToFarm()
        {
            ClearOutput();

            OutputText("Scenes.Milkwaifu.SendToFarm.DescribeToLakeAnd".Localized());

            OutputText("Scenes.Milkwaifu.SendToFarm.SaysCautiously".Localized());

            if (Player.Corruption >= 90)
                OutputText("Scenes.Milkwaifu.SendToFarm.DarklyButDeliciously".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmBathGirl, 1);

            ReturnToCamp(1);
        }

        public void BackToCamp()
        {
            ClearOutput();

            OutputText("Scenes.Milkwaifu.BackToCamp.TellHerBrowCrinkles".Localized());

            OutputText("Scenes.Milkwaifu.BackToCamp.WipesHandsBeforeWalking".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmBathGirl, 0);

            DoNext(FarmCorruption.RootScene);
        }

        public void MilkWriteFuAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Milkwaifu.MilkWriteFuAppearance.FiveFootFiveHuman".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) >= 2)
                OutputText("Scenes.Milkwaifu.MilkWriteFuAppearance.OverSuppleBody".Localized());

            OutputText("Scenes.Milkwaifu.MilkWriteFuAppearance.HumanFaceWith".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 0)
                OutputText("Scenes.Milkwaifu.MilkWriteFuAppearance.MostDistinguishingFeature".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 1)
                    OutputText("Scenes.Milkwaifu.MilkWriteFuAppearance.ThoughYouveReduced".Localized());
                else
                {
                    OutputText("Scenes.Milkwaifu.MilkWriteFuAppearance.ThanksToIntervention".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Milkwaifu.MilkWriteFuAppearance.OrCockWord".Localized());

                    OutputText("Scenes.Milkwaifu.MilkWriteFuAppearance.BetweenPillowyBosoms".Localized());
                }

            OutputText("Scenes.Milkwaifu.MilkWriteFuAppearance.BetweenLitheLegs".Localized());

            if (FarmCorruption.hasTattoo("milky"))
            {
                OutputText("Scenes.Milkwaifu.MilkWriteFuAppearance.ParagraphBreak".Localized());

                if (FarmCorruption.milkyFullTribalTats())
                    OutputText("Scenes.Milkwaifu.MilkWriteFuAppearance.CoveredHeadToToe".Localized());
                else
                    if (FarmCorruption.numMilkyButterflyTats() == 4)
                        OutputText("Scenes.Milkwaifu.MilkWriteFuAppearance.CoveredButterflies".Localized());
                    else
                    {
                        if (FarmCorruption.numTattoos("milky") > 1)
                            OutputText("Scenes.Milkwaifu.MilkWriteFuAppearance.FollowingTattoos".Localized());
                        else
                            OutputText("Scenes.Milkwaifu.MilkWriteFuAppearance.SheHasWord".Localized());

                        if (CoC.Instance.Flags.Has(GameFlagIds.MilkyTattooCollarbone))
                            OutputText("Scenes.Milkwaifu.MilkWriteFuAppearance.CollarboneTattoo".Localized(
                                CoC.Instance.Flags.Get(GameFlagIds.MilkyTattooCollarbone)
                            ));

                        if (CoC.Instance.Flags.Has(GameFlagIds.MilkyTattooShoulders))
                            OutputText("Scenes.Milkwaifu.MilkWriteFuAppearance.ShouldersTattoo".Localized(
                                CoC.Instance.Flags.Get(GameFlagIds.MilkyTattooShoulders)
                            ));

                        if (CoC.Instance.Flags.Has(GameFlagIds.MilkyTattooLowerback))
                            OutputText("Scenes.Milkwaifu.MilkWriteFuAppearance.LowerBackTattoo".Localized(
                                CoC.Instance.Flags.Get(GameFlagIds.MilkyTattooLowerback)
                            ));

                        if (CoC.Instance.Flags.Has(GameFlagIds.MilkyTattooButt))
                            OutputText("Scenes.Milkwaifu.MilkWriteFuAppearance.ButtTattoo".Localized(
                                CoC.Instance.Flags.Get(GameFlagIds.MilkyTattooButt)
                            ));
                    }
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), MilkyMenu);
        }

        public void SuperReductoUsage()
        {
            ClearOutput();

            OutputText("Scenes.Milkwaifu.SuperReductoUsage.TellThatItsAbout".Localized());

            OutputText("Scenes.Milkwaifu.SuperReductoUsage.SayCrouchingDown".Localized());

            OutputText("Scenes.Milkwaifu.SuperReductoUsage.IfByNothingElse".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 0)
            {
                OutputText("Scenes.Milkwaifu.SuperReductoUsage.TitsAreHuge".Localized());

                OutputText("Scenes.Milkwaifu.SuperReductoUsage.ShiftAroundInLap".Localized());

                OutputText("Scenes.Milkwaifu.SuperReductoUsage.CriesOutWhimpering".Localized());

                OutputText("Scenes.Milkwaifu.SuperReductoUsage.SayLeaningAround".Localized());

                OutputText("Scenes.Milkwaifu.SuperReductoUsage.ExperimentallyTriesToStand".Localized());

                OutputText("Scenes.Milkwaifu.SuperReductoUsage.GroansClutchingHead".Localized());
            }
            else
            {
                OutputText("Scenes.Milkwaifu.SuperReductoUsage.TitsFitForCow".Localized());

                if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaFollower())
                {
                    OutputText("Scenes.Milkwaifu.SuperReductoUsage.EnoughToGiveIsabella".Localized());

                    if (SceneInstance<MarbleScene>.Get().MarbleFollower())
                        OutputText("Scenes.Milkwaifu.SuperReductoUsage.AndMarbleWord".Localized());

                    OutputText("Scenes.Milkwaifu.SuperReductoUsage.PauseWord".Localized());
                }
                else
                    if (SceneInstance<MarbleScene>.Get().MarbleFollower())
                        OutputText("Scenes.Milkwaifu.SuperReductoUsage.GiveMarblePause".Localized());

                OutputText("Scenes.Milkwaifu.SuperReductoUsage.MuchHappierNow".Localized());

                OutputText("Scenes.Milkwaifu.SuperReductoUsage.ShiftInLapAgain".Localized());

                OutputText("Scenes.Milkwaifu.SuperReductoUsage.CriesOutWhimperingAgain".Localized());

                OutputText("Scenes.Milkwaifu.SuperReductoUsage.ByTimeYouSee".Localized());

                OutputText("Scenes.Milkwaifu.SuperReductoUsage.NearlyFallsCatches".Localized());

                OutputText("Scenes.Milkwaifu.SuperReductoUsage.LetHerCry".Localized());

                OutputText("Scenes.Milkwaifu.SuperReductoUsage.SurprisinglyEloquent".Localized());

                OutputText("Scenes.Milkwaifu.SuperReductoUsage.GroansBreathesSmiling".Localized());

                OutputText("Scenes.Milkwaifu.SuperReductoUsage.TellHerPleasure".Localized());

                OutputText("Scenes.Milkwaifu.SuperReductoUsage.ApproachesClearlySeen".Localized());

                OutputText("Scenes.Milkwaifu.SuperReductoUsage.HelpIntoNewClothes".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.MilkSize, CoC.Instance.Flags.Get(GameFlagIds.MilkSize) + 1);

            ReturnToCamp(1);
        }

        public void MilkBathTime()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Milkwaifu.MilkBathTime.TusslingHairTell".Localized());

            OutputText("Scenes.Milkwaifu.MilkBathTime.TremblingAnticipation".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 0)
                OutputText("Scenes.Milkwaifu.MilkBathTime.TeatLikeWord".Localized());
            else
                OutputText("Scenes.Milkwaifu.MilkBathTime.ProminentWord".Localized());

            OutputText("Scenes.Milkwaifu.MilkBathTime.AreolaBeadWithWhite".Localized());

            OutputText("Scenes.Milkwaifu.MilkBathTime.PearlyFluidFills".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 0)
                OutputText("Scenes.Milkwaifu.MilkBathTime.GiganticWord".Localized());
            else
                OutputText("Scenes.Milkwaifu.MilkBathTime.CowLikeWord".Localized());

            OutputText("Scenes.Milkwaifu.MilkBathTime.BreastsAsYouWatch".Localized());

            OutputText("Scenes.Milkwaifu.MilkBathTime.CloseEyesAndMassage".Localized());

            if (Player.Gender == Gender.Male)
                OutputText(Player.MultiCockDescriptLight());
            else
                if (Player.Gender == Gender.Female)
                    OutputText("Scenes.Milkwaifu.MilkBathTime.VaginaWord".Localized());
                else
                    if (Player.Gender == Gender.Hermaphrodite)
                        OutputText("Scenes.Milkwaifu.MilkBathTime.CocksAndVagina".Localized());
                    else
                        OutputText("Scenes.Milkwaifu.MilkBathTime.ButtWord".Localized());

            OutputText("Scenes.Milkwaifu.MilkBathTime.ResistUrgeToTouch".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 0)
                OutputText("Scenes.Milkwaifu.MilkBathTime.StillMammothMilkers".Localized());

            OutputText("Scenes.Milkwaifu.MilkBathTime.PartingSlightlyDusky".Localized());

            Player.HasGottenWashed();

            OutputText("Scenes.Milkwaifu.MilkBathTime.PullsBackLickingLips".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 0)
                OutputText("Scenes.Milkwaifu.MilkBathTime.ShiftBreastsBulk".Localized());
            else
                OutputText("Scenes.Milkwaifu.MilkBathTime.StrugglingToHerFeet".Localized());

            OutputText("Scenes.Milkwaifu.MilkBathTime.SentencePeriod".Localized());

            if (Player.Lust >= 33)
                OutputText("Scenes.Milkwaifu.MilkBathTime.CouldProbablyMasturbate".Localized());

            OutputText("Scenes.Milkwaifu.MilkBathTime.WhatDoYouDo".Localized());

            ClearButtons();

            AddButton(1, "Common.Button.MilkGirl".Localized(), PullInZeMilkGirl);

            if ((int)Player.Gender > 0 && Player.Lust >= 33)
                AddButton(0, "Common.Button.Drinknfap".Localized(), DrinkAndFapturbate);

            var count = 0;

            if (SceneInstance<SophieFollowerScene>.Get().SophieFollower())
                count += 1;

            if (FollowerRegistry.HasFollower(FollowerIds.JojoFollower))
                count += 1;

            if (SceneInstance<LatexGirlScene>.Get().LatexGooAtCamp())
                count += 1;

            if (CoC.Instance.Flags.Get(GameFlagIds.ValariaAtCamp) == 1 || Player.Armor.Id == ArmorIds.GooArmor)
                count += 1;

            if (SceneInstance<AmilyScene>.Get().AmilyFollower() && !SceneInstance<AmilyScene>.Get().AmilyCorrupt())
                count += 1;

            if (SceneInstance<HelScene>.Get().FollowerHel())
                count += 1;

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaFollowerStatus) == 1)
                count += 1;

            if (SceneInstance<EmberScene>.Get().FollowerEmber())
                count += 1;

            if (SceneInstance<KihaFollowerScene>.Get().FollowerKiha())
                count += 1;

            if (count >= 3)
            {
                OutputText("Scenes.Milkwaifu.MilkBathTime.FriendsCouldJoin".Localized());

                AddButton(2, "Common.Button.CommBath".Localized(), CommunalBath);
            }

            AddButton(4, "Common.Button.Relax".Localized(), RelaxWithMilkWaifu);
        }

        public void RelaxWithMilkWaifu()
        {
            ClearOutput();

            OutputText("Scenes.Milkwaifu.RelaxWithMilkWaifu.SitInTub".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Sensitivity / 10)));

            Player.ChangeFatigue(-34);

            ReturnToCamp(1);
        }

        public void CommunalBath()
        {
            ClearOutput();

            ShowImage("communal-bath");

            OutputText("Scenes.Milkwaifu.CommunalBath.RelaxInTub".Localized());

            if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaFollower())
            {
                OutputText("Scenes.Milkwaifu.CommunalBath.MomentLaterToweringForm".Localized());

                if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaAccent())
                    OutputText("Scenes.Milkwaifu.CommunalBath.CouldUseBathAccent".Localized());
                else
                    OutputText("Scenes.Milkwaifu.CommunalBath.CouldUseBathPlain".Localized());
            }

            if (SceneInstance<SophieFollowerScene>.Get().SophieFollower())
                OutputText("Scenes.Milkwaifu.CommunalBath.SophieCheerily".Localized());

            if (FollowerRegistry.HasFollower(FollowerIds.JojoFollower))
                OutputText("Scenes.Milkwaifu.CommunalBath.MonkJojoApproaches".Localized());

            if (SceneInstance<LatexGirlScene>.Get().LatexGooAtCamp())
                OutputText("Scenes.Milkwaifu.CommunalBath.WaveOverEbony".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ValariaAtCamp) == 1 || Player.Armor.Id == ArmorIds.GooArmor)
            {
                OutputText("Scenes.Milkwaifu.CommunalBath.GooeyValeriaMaterializes".Localized());

                SceneInstance<ValeriaScene>.Get().FeedValeria(100);
            }

            if (SceneInstance<AmilyScene>.Get().AmilyFollower() && !SceneInstance<AmilyScene>.Get().AmilyCorrupt())
                OutputText("Scenes.Milkwaifu.CommunalBath.MouseGirlAmily".Localized());

            if (SceneInstance<HelScene>.Get().FollowerHel())
                OutputText("Scenes.Milkwaifu.CommunalBath.GleefulShoutHel".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaFollowerStatus) == 1)
                OutputText("Scenes.Milkwaifu.CommunalBath.DidntEvenNoticeIzma".Localized());

            if (SceneInstance<EmberScene>.Get().FollowerEmber())
                OutputText("Scenes.Milkwaifu.CommunalBath.EmberApproachesPool".Localized(
                    SceneInstance<EmberScene>.Get().EmberMF("He", "She"),

                    SceneInstance<EmberScene>.Get().EmberMF("himself", "herself"),

                    SceneInstance<EmberScene>.Get().EmberMF("his", "her"),

                    SceneInstance<EmberScene>.Get().EmberMF("He", "She")
                ));

            if (SceneInstance<KihaFollowerScene>.Get().FollowerKiha())
            {
                OutputText("Scenes.Milkwaifu.CommunalBath.KihaWanders".Localized());

                if (silly && Player.Strength > 80 && Player.Speed > 80)
                    OutputText("Scenes.Milkwaifu.CommunalBath.WithoutWarningGrabKiha".Localized());
            }

            OutputText("Scenes.Milkwaifu.CommunalBath.SurroundedByFriends".Localized());

            OutputText("Scenes.Milkwaifu.CommunalBath.WithoutPromptingShifts".Localized());

            OutputText("Scenes.Milkwaifu.CommunalBath.CarryOnTogether".Localized());

            Player.RefillHunger(50);

            Player.ChangeFatigue(-40);

            Player.HealthChange(Player.MaxHealth * .33f);

            ReturnToCamp(1);
        }

        public void PullInZeMilkGirl()
        {
            ClearOutput();

            OutputText("Scenes.Milkwaifu.PullInZeMilkGirl.CallOutToBefore".Localized());

            OutputText("Scenes.Milkwaifu.PullInZeMilkGirl.FlashMischievousGrin".Localized());

            if (Player.BiggestBreastSize() >= 7)
                OutputText("Scenes.Milkwaifu.PullInZeMilkGirl.PressingDeepInto".Localized());

            OutputText("Scenes.Milkwaifu.PullInZeMilkGirl.SentencePeriod".Localized());

            OutputText("Scenes.Milkwaifu.PullInZeMilkGirl.MoansOnTheEdge".Localized());

            if (Player.Gender == Gender.Male)
                OutputText(Player.MultiCockDescriptLight());
            else
                if (Player.Gender == Gender.Female)
                    OutputText("Scenes.Milkwaifu.PullInZeMilkGirl.VaginaWord".Localized());
                else
                    if (Player.Gender == Gender.Hermaphrodite)
                        OutputText("Scenes.Milkwaifu.PullInZeMilkGirl.CocksAndVagina".Localized());
                    else
                        OutputText("Scenes.Milkwaifu.PullInZeMilkGirl.SexlessCrotch".Localized());

            OutputText("Scenes.Milkwaifu.PullInZeMilkGirl.StillBurningHotly".Localized());

            OutputText("Scenes.Milkwaifu.PullInZeMilkGirl.AfterTimePush".Localized());

            OutputText("Scenes.Milkwaifu.PullInZeMilkGirl.GaspsWhenBrushAgainst".Localized());

            if (Player.Corruption < 70)
                OutputText("Scenes.Milkwaifu.PullInZeMilkGirl.YoureQuickToPull".Localized());
            else
                OutputText("Scenes.Milkwaifu.PullInZeMilkGirl.GrinningWithLustyFervor".Localized());

            OutputText("Scenes.Milkwaifu.PullInZeMilkGirl.BeforeYouGetFar".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Milkwaifu.PullInZeMilkGirl.OneOfMilkGirls".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Milkwaifu.PullInZeMilkGirl.OtherHandCupping".Localized(
                        NumberUtils.Num2Text(Player.Balls)
                    ));

                OutputText("Scenes.Milkwaifu.PullInZeMilkGirl.LeaningOverTitanicTeats".Localized());
            }

            if (Player.HasVagina())
            {
                OutputText("Scenes.Milkwaifu.PullInZeMilkGirl.HandsShiftDownwards".Localized());

                if (Player.BiggestBreastSize() >= 1)
                {
                    OutputText("Scenes.Milkwaifu.PullInZeMilkGirl.CupsYourBreasts".Localized());

                    if (Player.LactationQ() >= 100)
                        OutputText("Scenes.Milkwaifu.PullInZeMilkGirl.SpurtOfMilkEscapes".Localized());
                }
            }

            ClearButtons();

            AddButton(4, "Scenes.ErlKing.HowDareYou.ButtonDont".Localized(), DontFuckTheMilkWaifu);

            if (Player.HasCock())
                AddButton(0, "Common.Button.FuckHer".Localized(), FuckTheMilkWaifu);

            if (Player.HasVagina() && Player.BiggestBreastSize() >= 3)
                AddButton(1, "Common.Button.Lesbofuck".Localized(), BeARugMunchingMilkDyke);
        }

        public void DontFuckTheMilkWaifu()
        {
            ClearOutput();

            OutputText("Scenes.Milkwaifu.DontFuckTheMilkWaifu.AllowGirlToContinue".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Sensitivity / 10)));

            Player.HealthChange(Player.MaxHealth * .33f);

            Player.ChangeFatigue(-20);

            ReturnToCamp(1);
        }

        public void FuckTheMilkWaifu()
        {
            ClearOutput();

            OutputText("Scenes.Milkwaifu.FuckTheMilkWaifu.TurnAroundAndPull".Localized());

            OutputText("Scenes.Milkwaifu.FuckTheMilkWaifu.SubmergedBeneathSea".Localized());

            if (Player.CockArea(0) < 20)
                OutputText("Scenes.Milkwaifu.FuckTheMilkWaifu.PushingFewInches".Localized());
            else
                if (Player.CockArea(0) < 50)
                    OutputText("Scenes.Milkwaifu.FuckTheMilkWaifu.HiltingInOneStroke".Localized());
                else
                    OutputText("Scenes.Milkwaifu.FuckTheMilkWaifu.CockGainingEntrance".Localized());

            OutputText("Scenes.Milkwaifu.FuckTheMilkWaifu.PrickBuriedInHer".Localized());

            OutputText("Scenes.Milkwaifu.FuckTheMilkWaifu.SheMoans".Localized());

            OutputText("Scenes.Milkwaifu.FuckTheMilkWaifu.PickUpThePace".Localized());

            OutputText("Scenes.Milkwaifu.FuckTheMilkWaifu.AllowGirlToContinue".Localized());

            OutputText("Scenes.Milkwaifu.FuckTheMilkWaifu.EntireBodyRefreshed".Localized());

            Player.Orgasm("Dick");

            Player.HealthChange(Player.MaxHealth * .33f);

            ReturnToCamp(1);
        }

        public void BeARugMunchingMilkDyke()
        {
            ClearOutput();

            OutputText("Scenes.Milkwaifu.BeARugMunchingMilkDyke.TurnAroundInMilky".Localized());

            if (Player.LactationQ() >= 200)
                OutputText("Scenes.Milkwaifu.BeARugMunchingMilkDyke.DrawingOutTrickle".Localized());

            OutputText("Scenes.Milkwaifu.BeARugMunchingMilkDyke.HandSlipsUpBody".Localized());

            if (Player.LactationQ() >= 200)
                OutputText("Scenes.Milkwaifu.BeARugMunchingMilkDyke.MilkingWithSkill".Localized());
            else
                OutputText("Scenes.Milkwaifu.BeARugMunchingMilkDyke.SentencePeriod".Localized());

            OutputText("Scenes.Milkwaifu.BeARugMunchingMilkDyke.LeanBackAgainstRim".Localized());

            if (Player.LactationQ() >= 200)
                OutputText("Scenes.Milkwaifu.BeARugMunchingMilkDyke.MilkyWord".Localized());

            OutputText("Scenes.Milkwaifu.BeARugMunchingMilkDyke.TeatsIncredibleWays".Localized());

            OutputText("Scenes.Milkwaifu.BeARugMunchingMilkDyke.SoonFeelStrange".Localized());

            if (Player.LactationQ() >= 200)
                OutputText("Scenes.Milkwaifu.BeARugMunchingMilkDyke.SprayingMilkOverYourself".Localized());

            OutputText("Scenes.Milkwaifu.BeARugMunchingMilkDyke.RunFingersThrough".Localized());

            OutputText("Scenes.Milkwaifu.BeARugMunchingMilkDyke.EntireBodyRefreshed".Localized());

            Player.Orgasm("Tits");

            Player.HealthChange(Player.MaxHealth * .33f);

            ReturnToCamp(1);
        }

        public void DrinkAndFapturbate()
        {
            ClearOutput();

            OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.CallOutToEbony".Localized());

            OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.WhinesStillMeek".Localized());

            OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.RollsShouldersSending".Localized());

            OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.TakingNippleIn".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 0)
                OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.ADinnerPlate".Localized());
            else
                OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.AnOpenPalm".Localized());

            OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.FarMoreExciting".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.IdlyStrokePuffyLust".Localized());
            else
                OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.IdlyStrokeTurgidMass".Localized());

            OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.InadvertentlyDrawing".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.DoesLittleToHide".Localized());

            OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.SentencePeriod".Localized());

            Player.RefillHunger(50);

            OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.ExcitedMoanWorms".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
            {
                OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.ThrobsPainfullyInHand".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.MakeSureToFondle".Localized());

                OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.WithOffHandRub".Localized());
            }
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.PussyJuicesMix".Localized());
                else
                {
                    OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.ThrobsPainfullySo".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.FondleMembersAgain".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.WithOffhandCradle".Localized());
                }

            OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.SprayOfWarmth".Localized());

            OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.TubDangerouslyFull".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.ReleaseGooeyLoad".Localized());

                if (Player.CumQ() >= 1500)
                    OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.EveryTorrentOfSeed".Localized());

                if (Player.CumQ() >= 4000)
                    OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.TubFluidContents".Localized());
            }

            if (Player.HasVagina())
                OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.JuicedUpCunnyFeels".Localized());

            OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.DrawnOutLowCoo".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 0)
                OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.DragsGiganticTits".Localized());
            else
                OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.StumblesAwayTenderly".Localized());

            OutputText("Scenes.Milkwaifu.DrinkAndFapturbate.SmirkingSexuallySated".Localized());

            Player.Orgasm("Generic");

            Player.RefillHunger(30);

            Player.ChangeFatigue(-33);

            ReturnToCamp(1);
        }

        public void NyanCatMilkTime()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 1)
            {
                OutputText("Scenes.Milkwaifu.NyanCatMilkTime.SayGivingPlayfulSwat".Localized());

                OutputText("Scenes.Milkwaifu.NyanCatMilkTime.OnceDisrobed".Localized());

                OutputText("Scenes.Milkwaifu.NyanCatMilkTime.MatterOfMoments".Localized());

                if (SceneInstance<SophieFollowerScene>.Get().SophieFollower())
                    OutputText("Scenes.Milkwaifu.NyanCatMilkTime.AsYouMilk".Localized());

                if (SceneInstance<KihaFollowerScene>.Get().FollowerKiha())
                    OutputText("Scenes.Milkwaifu.NyanCatMilkTime.FeelingBeingWatched".Localized());

                if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaFollower())
                {
                    if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaAccent())
                        OutputText("Scenes.Milkwaifu.NyanCatMilkTime.VhatAreYouDoing".Localized());
                    else
                        OutputText("Scenes.Milkwaifu.NyanCatMilkTime.WhatAreYouDoing".Localized());

                    OutputText("Scenes.Milkwaifu.NyanCatMilkTime.LookUpFromTask".Localized());

                    if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaAccent())
                        OutputText("Scenes.Milkwaifu.NyanCatMilkTime.ZingWord".Localized());
                    else
                        OutputText("Scenes.Milkwaifu.NyanCatMilkTime.ThingWord".Localized());

                    OutputText("Scenes.Milkwaifu.NyanCatMilkTime.LetIsabellaShowYou".Localized());

                    if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaAccent())
                        OutputText("Scenes.Milkwaifu.NyanCatMilkTime.EsIstWord".Localized());
                    else
                        OutputText("Scenes.Milkwaifu.NyanCatMilkTime.ItsWord".Localized());

                    OutputText("Scenes.Milkwaifu.NyanCatMilkTime.DoneRoughlyPushed".Localized());
                }

                if (!SceneInstance<IsabellaFollowerScene>.Get().IsabellaFollower() || !SceneInstance<SophieFollowerScene>.Get().SophieFollower())
                    OutputText("Scenes.Milkwaifu.NyanCatMilkTime.TakesGoodLongWhile".Localized());
                else
                    OutputText("Scenes.Milkwaifu.NyanCatMilkTime.ThoughDidntIntendFor".Localized());

                Player.ChangeFatigue(-50);

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Sensitivity / 10)));

                ReturnToCamp(1);
            }
            else
            {
                OutputText("Scenes.Milkwaifu.NyanCatMilkTime.SitYourselfDownWith".Localized());

                OutputText("Scenes.Milkwaifu.NyanCatMilkTime.WhimpersAsYouSqueeze".Localized());

                OutputText("Scenes.Milkwaifu.NyanCatMilkTime.EachCaressAndSqueeze".Localized());

                OutputText("Scenes.Milkwaifu.NyanCatMilkTime.CriesSquirming".Localized());

                if (Player.HasCockThatFits(50))
                    OutputText("Scenes.Milkwaifu.NyanCatMilkTime.ThenAgainMaybeHike".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Normal".Localized(), FinishMilkNormall);

                AddButton(1, "Common.Button.Suckle".Localized(), SuckleDatMilk);

                if (Player.HasCockThatFits(50))
                    AddButton(2, "Common.Button.Fuck".Localized(), FuckDatMilkSlat);
            }
        }

        public void FinishMilkNormall()
        {
            ClearOutput();

            OutputText("Scenes.Milkwaifu.FinishMilkNormall.WhisperSoothingWords".Localized());

            OutputText("Scenes.Milkwaifu.FinishMilkNormall.ExplosiveBoobgasm".Localized());

            OutputText("Scenes.Milkwaifu.FinishMilkNormall.SaysSimply".Localized());

            Player.ChangeFatigue(-50);

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Sensitivity / 10)));

            ReturnToCamp(1);
        }

        public void SuckleDatMilk()
        {
            ClearOutput();

            OutputText("Scenes.Milkwaifu.SuckleDatMilk.ReleaseBreastsGiving".Localized());

            OutputText("Scenes.Milkwaifu.SuckleDatMilk.ResponseToGentleSuckles".Localized());

            OutputText("Scenes.Milkwaifu.SuckleDatMilk.OverFewShortMinutes".Localized());

            OutputText("Scenes.Milkwaifu.SuckleDatMilk.OrgasmAsPowerfulAs".Localized());

            OutputText("Scenes.Milkwaifu.SuckleDatMilk.BoobgasmSubsides".Localized());

            Player.SlimeFeed();

            Player.RefillHunger(50);

            Player.ChangeFatigue(-50);

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Sensitivity / 10)));

            ReturnToCamp(1);
        }

        public void FuckDatMilkSlat()
        {
            ClearOutput();

            var x = Player.CockThatFits(50);

            OutputText("Scenes.Milkwaifu.FuckDatMilkSlat.SittingBehindMilkyGirl".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Milkwaifu.FuckDatMilkSlat.OneHandDriftsDown".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Milkwaifu.FuckDatMilkSlat.ShiftForwardSlightly".Localized(
                Player.CockDescript(x),

                Player.CockHead(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Milkwaifu.FuckDatMilkSlat.ByTheTimeHilted".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Milkwaifu.FuckDatMilkSlat.FeelOrgasmMounting".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Milkwaifu.FuckDatMilkSlat.DickFinallyDepleted".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Player.ChangeFatigue(-10);

            ReturnToCamp(1);
        }

        public void TitFuckDatMilkSlut()
        {
            ClearOutput();

            OutputText("Scenes.Milkwaifu.TitFuckDatMilkSlut.SitBesideAndPush".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 2)
                OutputText("Scenes.Milkwaifu.TitFuckDatMilkSlut.PullOffShirtBra".Localized());
            else
                OutputText("Scenes.Milkwaifu.TitFuckDatMilkSlut.SinkFingersIntoUdder".Localized());

            OutputText("Scenes.Milkwaifu.TitFuckDatMilkSlut.CaressingSensitiveBreasts".Localized());

            OutputText("Scenes.Milkwaifu.TitFuckDatMilkSlut.BeforeYouCanThrust".Localized());

            OutputText("Scenes.Milkwaifu.TitFuckDatMilkSlut.GrabShouldersForSupport".Localized());

            OutputText("Scenes.Milkwaifu.TitFuckDatMilkSlut.CoaxedByOral".Localized());

            if (!Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Milkwaifu.TitFuckDatMilkSlut.StraddlingShoulders".Localized());
            else
                OutputText("Scenes.Milkwaifu.TitFuckDatMilkSlut.WrappingYourselfAround".Localized());

            OutputText("Scenes.Milkwaifu.TitFuckDatMilkSlut.DeepIntoHerMouth".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Milkwaifu.TitFuckDatMilkSlut.ShiveringAsTongueDarts".Localized());

            OutputText("Scenes.Milkwaifu.TitFuckDatMilkSlut.SentencePeriod".Localized());

            OutputText("Scenes.Milkwaifu.TitFuckDatMilkSlut.JustAsYouSettle".Localized());

            OutputText("Scenes.Milkwaifu.TitFuckDatMilkSlut.OneLastSurge".Localized());

            Player.Orgasm("Tits");

            OutputText("Scenes.Milkwaifu.TitFuckDatMilkSlut.SighContentedly".Localized());

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Player.ChangeFatigue(-10);

            ReturnToCamp(1);
        }

        public void MilkTryTalking()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 0)
            {
                OutputText("Scenes.Milkwaifu.MilkTryTalking.PeacefullyOnTheMat".Localized());

                OutputText("Scenes.Milkwaifu.MilkTryTalking.ShiftsAtSoundOf".Localized());

                OutputText("Scenes.Milkwaifu.MilkTryTalking.ThatsNotWhatYoure".Localized());

                OutputText("Scenes.Milkwaifu.MilkTryTalking.DoubtfulYoureGoing".Localized());

                Save.SetInt("talkedSlut", 1);

                ClearButtons();

                AddNextButton("Common.Button.Bath".Localized(), MilkBathTime);

                AddNextButton("Common.Button.Leave".Localized(), MilkTalkLeave);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 1)
                {
                    OutputText("Scenes.Milkwaifu.MilkTryTalking.TheresASpark".Localized());

                    OutputText("Scenes.Milkwaifu.MilkTryTalking.SaysThoughTheLook".Localized());

                    OutputText("Scenes.Milkwaifu.MilkTryTalking.VisiblyUncomfortable".Localized());

                    OutputText("Scenes.Milkwaifu.MilkTryTalking.DisappointingButConsole".Localized());

                    Save.SetInt("talkedSlut", 2);

                    ClearButtons();

                    AddNextButton("Common.Button.Bath".Localized(), MilkBathTime);

                    AddNextButton("Common.Button.Leave".Localized(), MilkTalkLeave);
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 2 && Save.GetInt("talkedSlut", 0) < 2)
                    {
                        OutputText("Scenes.Milkwaifu.MilkTryTalking.NowRegainedControl".Localized());

                        OutputText("Scenes.Milkwaifu.MilkTryTalking.ThankYouForAsking".Localized());

                        OutputText("Scenes.Milkwaifu.MilkTryTalking.AfterTheLastWord".Localized());

                        Save.SetInt("talkedSlut", 3);

                        ClearButtons();

                        AddNextButton("Common.Button.Great".Localized(), () => MilkTalkAnswer(0));

                        AddNextButton("Common.Button.Fine".Localized(), () => MilkTalkAnswer(1));

                        AddNextButton("Common.Button.Awful".Localized(), () => MilkTalkAnswer(2));

                        AddNextButton("Common.Button.Horny".Localized(), () => MilkTalkAnswer(3));
                    }
                    else
                        MilkTalkMenu();
        }

        public void MilkTalkAnswer(int answer)
        {
            ClearOutput();

            switch (answer)
            {
                case 0:
                {
                OutputText("Scenes.Milkwaifu.MilkTalkAnswer.NoComplaints".Localized());

                OutputText("Scenes.Milkwaifu.MilkTalkAnswer.GrinsWidelyShowingOff".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Milkwaifu.MilkTalkAnswer.FeelPrettyAverage".Localized());

                OutputText("Scenes.Milkwaifu.MilkTalkAnswer.DoesntQuiteKnowHow".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Milkwaifu.MilkTalkAnswer.DontKnowWhetherTo".Localized());

                OutputText("Scenes.Milkwaifu.MilkTalkAnswer.SlaveGirlAdoptsConcerned".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Milkwaifu.MilkTalkAnswer.FeelingIncreasinglyHot".Localized());

                OutputText("Scenes.Milkwaifu.MilkTalkAnswer.BlushesButDoesntBack".Localized());
                }

                break;
            }

            OutputText("Scenes.Milkwaifu.MilkTalkAnswer.ConversationStalls".Localized());

            OutputText("Scenes.Milkwaifu.MilkTalkAnswer.SheSlipsOff".Localized());

            ReturnToCamp(1);
        }

        public void MilkTalkLeave()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 0)
                OutputText("Scenes.Milkwaifu.MilkTalkLeave.LeaveConfusedGirlAlone".Localized(
                    Player.Corruption > 50 ? "What a waste of View.Time. Y" : "While you may never know what the " + (CoC.Instance.Flags.Get(GameFlagIds.SandWitchesFriendly) > 0 ? "demons" : "sand witches") + " did to [bathgirlname], you're sure there's more to her than this. In any case, y"
                ));
            else
            {
                OutputText("Scenes.Milkwaifu.MilkTalkLeave.GiveBriefPatOn".Localized());

                OutputText("Scenes.Milkwaifu.MilkTalkLeave.SheSays".Localized());

                OutputText("Scenes.Milkwaifu.MilkTalkLeave.MostArticulateThing".Localized());
            }

            ReturnToCamp(1);
        }

        public void MilkTalkMenu()
        {
            ClearOutput();

            OutputText("Scenes.Milkwaifu.MilkTalkMenu.NowCanActuallyTalk".Localized());

            ClearButtons();

            AddNextButton("Common.Button.HerPast".Localized(), MilkTalkPast);

            AddNextButton("CharCreation.GenericStyleCustomizeMenu.Button6".Localized(), MilkTalkBreasts);

            AddNextButton("Common.Button.Slave".Localized(), MilkTalkSlave);

            AddButton(14, "Common.Button.Back".Localized(), () => MilkyMenu());
        }

        public void MilkTalkPast()
        {
            ClearOutput();

            OutputText("Scenes.Milkwaifu.MilkTalkPast.NowMostlyLucid".Localized());

            OutputText("Scenes.Milkwaifu.MilkTalkPast.AtYourQuestion".Localized());

            OutputText("Scenes.Milkwaifu.MilkTalkPast.ShiversAndThenSays".Localized());

            OutputText("Scenes.Milkwaifu.MilkTalkPast.SheAsks".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Yes".Localized(), () => MilkTalkPastAnswer(0));

            AddNextButton("Common.Button.No".Localized(), () => MilkTalkPastAnswer(1));

            AddNextButton("Common.Button.DontKnow".Localized(), () => MilkTalkPastAnswer(2));
        }

        public void MilkTalkPastAnswer(int answer)
        {
            ClearOutput();

            switch (answer)
            {
                case 0:
                {
                OutputText("Scenes.Milkwaifu.MilkTalkPastAnswer.FromSoundOfIt".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Milkwaifu.MilkTalkPastAnswer.TheresNoOtherWay".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Milkwaifu.MilkTalkPastAnswer.TellHerNoJudge".Localized());
                }

                break;
            }

            OutputText("Scenes.Milkwaifu.MilkTalkPastAnswer.NodsNotEntirelyConvinced".Localized());

            OutputText("Scenes.Milkwaifu.MilkTalkPastAnswer.ButThatsAboutIt".Localized());

            OutputText("Scenes.Milkwaifu.MilkTalkPastAnswer.AWorthwhileDream".Localized());

            MilkTalkMenu();
        }

        public void MilkTalkBreasts()
        {
            ClearOutput();

            OutputText("Scenes.Milkwaifu.MilkTalkBreasts.UndergoingSignificantChanges".Localized());

            OutputText("Scenes.Milkwaifu.MilkTalkBreasts.AlmostCutsYouOff".Localized());

            OutputText("Scenes.Milkwaifu.MilkTalkBreasts.CertainlyUnderstandable".Localized());

            OutputText("Scenes.Milkwaifu.MilkTalkBreasts.ShoutsFaceColoring".Localized());

            OutputText("Scenes.Milkwaifu.MilkTalkBreasts.SaysGivingBosom".Localized());

            MilkTalkMenu();
        }

        public void MilkTalkSlave()
        {
            ClearOutput();

            OutputText("Scenes.Milkwaifu.MilkTalkSlave.WellTheresNoReason".Localized());

            OutputText("Scenes.Milkwaifu.MilkTalkSlave.BlurtsOutSurprised".Localized());

            OutputText("Scenes.Milkwaifu.MilkTalkSlave.ExplainSinceYouBought".Localized());

            OutputText("Scenes.Milkwaifu.MilkTalkSlave.SaysSomewhatAbashed".Localized());

            OutputText("Scenes.Milkwaifu.MilkTalkSlave.WellYoureGlad".Localized());

            MilkTalkMenu();
        }

    }
}