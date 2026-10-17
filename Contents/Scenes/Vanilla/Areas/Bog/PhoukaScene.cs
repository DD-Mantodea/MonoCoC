using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.World;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Pregnancy;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Bog
{
    [Register<PhoukaScene>("coc:phouka")]
    public class PhoukaScene : Scene, ITimeAware
    {

        internal static int phoukaForm = PHOUKA_FORM_FAERIE;

        internal const int PHOUKA_FORM_FAERIE = 0;

        internal const int PHOUKA_FORM_BUNNY = 1;

        internal const int PHOUKA_FORM_GOAT = 2;

        internal const int PHOUKA_FORM_HORSE = 3;

        public static readonly Identifier FaeriePregnancy = new("coc", "faerie");

        public const int FaerieIncubation = 200;

        public override void Entry()
        {
            PhoukaEncounter();
        }

        public string PhoukaNameText(string known, string unknown)
        {

            return CoC.Instance.Flags.Get(GameFlagIds.PhoukaLore) == 0 ? unknown : known;
        }

        public string PhoukaName()
        {

            return PhoukaNameText("phouka", "faerie creature");
        }

        public void PhoukaEncounter()
        {

            phoukaForm = PHOUKA_FORM_FAERIE;

            var choiceChance = 0;

            if (CoC.Instance.Flags.Get(GameFlagIds.PhoukaEncounterStatus) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.TreacleMineYearDone, DateTime.Now.Year - 1);

                CoC.Instance.Flags.Set(GameFlagIds.BirthsPhouka, 0);

                CoC.Instance.Flags.Set(GameFlagIds.BirthsFaerie, 0);
            }
            else
                if ((DateTime.Now.Month > 5) && (DateTime.Now.Month < 12))
                    choiceChance = Random.Shared.Next(7);
                else
                    choiceChance = Random.Shared.Next(6);

            if (choiceChance < 2)
                PhoukaFaerieFire();
            else
                if (choiceChance < 5)
                    PhoukaStuck();
                else
                    PhoukaMasturbating();
        }

        public void PhoukaFaerieFire()
        {
            ClearOutput();

            ShowImage("phouka-encounter");

            OutputText("Scenes.Phoukascene.PhoukaFaerieFire.WadingThroughBog".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PhoukaEncounterStatus) != 0)
            {
                OutputText("Scenes.Phoukascene.PhoukaFaerieFire.RecallLastEncounter".Localized());

                OutputText("Scenes.Phoukascene.PhoukaFaerieFire.BestLeaveBogAgain".Localized());
            }
            else
            {
                OutputText("Scenes.Phoukascene.PhoukaFaerieFire.SeemsMovingAround".Localized());

                if (Player.HasKeyItem("Traveler's Guide"))
                {
                    OutputText("Scenes.Phoukascene.PhoukaFaerieFire.WarningFromTraveler".Localized());

                    OutputText("Scenes.Phoukascene.PhoukaFaerieFire.BestLeaveBogAgain".Localized());
                }
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.PhoukaLore) > 0)
                Player.UnlockCodexEntry(GameFlagIds.CodexEntryPhouka, true, true);

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.PhoukaEncounterStatus) != 0 || Player.HasKeyItem("Traveler's Guide"))
                AddButton(1, "Common.Button.BackAway".Localized(), PhoukaFaerieFireLeave);

            AddButton(0, "Common.Button.Follow".Localized(), PhoukaStuck);
        }

        public void PhoukaFaerieFireLeave()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.PhoukaEncounterStatus) == 0)
                OutputText("Scenes.Phoukascene.PhoukaFaerieFireLeave.DecideBestNot".Localized());
            else
                OutputText("Scenes.Phoukascene.PhoukaFaerieFireLeave.MomentYouDont".Localized(
                     PhoukaNameText("phouka", "creatures")
                ));

            DoNext(() => ReturnToCamp(1));
        }

        public void PhoukaStuck()
        {
            ClearOutput();

            var hasAlcohol = Player.HasItem(ItemRegistry.Create(ConsumableIds.Whisky), 1);

            ShowImage("monster-phouka");

            OutputText("Scenes.Phoukascene.PhoukaStuck.FollowFirmerGround".Localized());

            OutputText("Scenes.Phoukascene.PhoukaStuck.TryToLeap".Localized());

            OutputText("Scenes.Phoukascene.PhoukaStuck.FeelSomething".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Phoukascene.PhoukaStuck.TailIsWord".Localized());
            else
                OutputText("Scenes.Phoukascene.PhoukaStuck.LegsAreWord".Localized());

            OutputText("Scenes.Phoukascene.PhoukaStuck.SoonTangledWeb".Localized());

            OutputText("Scenes.Phoukascene.PhoukaStuck.AttackerMeltThen".Localized(
                 PhoukaName(),

                 PhoukaName()
            ));

            if (hasAlcohol)
                OutputText("Scenes.Phoukascene.PhoukaStuck.SuspectOffer".Localized(
                     PhoukaName()
                ));

            if (CoC.Instance.Flags.Get(GameFlagIds.PhoukaEncounterStatus) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.PhoukaEncounterStatus, 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.PhoukaLore) > 0)
                Player.UnlockCodexEntry(GameFlagIds.CodexEntryPhouka, true, true);

            ClearButtons();

            if (hasAlcohol)
                AddButton(0, "Common.Button.OfferDrink".Localized(), PhoukaStuckOfferWhiskey);
            else
                AddButton(0, "Common.Button.OfferDrink".Localized(), null);

            AddButton(1, "Common.Button.Talk".Localized(), PhoukaTalk);

            AddButton(2, "Common.Button.Fight".Localized(), PhoukaStartFight);
        }

        public void PhoukaStuckOfferWhiskey()
        {
            ClearOutput();

            ShowImage("item-whiskey");

            OutputText("Scenes.Phoukascene.PhoukaStuckOfferWhiskey.PullOutBottle".Localized(
                 PhoukaName()
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.PhoukaLore) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.PhoukaLore, 1);

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryPhouka, true, false);

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.Whisky), 1);

            OutputText("Scenes.Phoukascene.PhoukaStuckOfferWhiskey.ZipsOverTo".Localized());

            phoukaForm = PHOUKA_FORM_BUNNY;

            if ((Player.Pregnancy.Incubation == 0) && (Player.ButtPregnancy.Incubation == 0))
            {
                ClearButtons();

                AddButton(1, "Scenes.Lumberjack.Button.Drink".Localized(), PhoukaDrinkAccept);
            }
            else
                PhoukaDrinkWhilePregnant(true);
        }

        public void PhoukaMasturbating()
        {
            ClearOutput();

            OutputText("Scenes.Phoukascene.PhoukaMasturbating.WanderingThroughBog".Localized(
                 PhoukaNameText("phouka", "small black faerie"),

                 PhoukaName(),

                 PhoukaName()
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.PhoukaLore) > 0)
                Player.UnlockCodexEntry(GameFlagIds.CodexEntryPhouka, true, true);

            DoNext(() => ReturnToCamp(1));
        }

        public void PhoukaStartFight()
        {

            OutputText("Scenes.Phoukascene.PhoukaStartFight.CantMoveLower".Localized());

            var phouka = EnemyRegistry.Create(EnemyIds.Phouka);

            phouka.ShortName = PhoukaName();

            CombatScene.Start(phouka, () => Go<CampScene>());
        }

        public void PhoukaStartFightSilenced()
        {

            Combat.Handlers.WebSilenceHandlers.Apply(Player);

            var phouka = EnemyRegistry.Create(EnemyIds.Phouka);

            phouka.ShortName = PhoukaName();

            CombatScene.Start(phouka, () => Go<CampScene>(), canFlee: false);
        }

        public void PhoukaTalk()
        {
            ClearOutput();

            ShowImage("monster-phouka");

            OutputText("Scenes.Phoukascene.PhoukaTalk.YouAskTheWord".Localized(
                 PhoukaName()
            ));

            if (Player.Level < 10)
                OutputText("Scenes.Phoukascene.PhoukaTalk.PerhapsSomeWay".Localized());
            else
                OutputText("Scenes.Phoukascene.PhoukaTalk.IfHeReallyWants".Localized());

            if (((Math.Clamp(Player.Corruption + Player.CorruptionTolerance(), 0f, 100f) + Random.Shared.Next(125) + (Player.Level - 10)) * 4) <= 110)
            {
                OutputText("Scenes.Phoukascene.PhoukaTalk.OpenMouthToSay".Localized());

                if (Player.Corruption < 34)
                    OutputText("Scenes.Phoukascene.PhoukaTalk.VileLittleMonster".Localized());
                else
                    if (Player.Corruption < 67)
                        OutputText("Scenes.Phoukascene.PhoukaTalk.PervertedWord".Localized(
                             PhoukaName()
                        ));
                    else
                        OutputText("Scenes.Phoukascene.PhoukaTalk.FuckableLittleWord".Localized(
                             PhoukaName()
                        ));

                OutputText("Scenes.Phoukascene.PhoukaTalk.MustNotFeelLike".Localized());

                var phouka = EnemyRegistry.Create(EnemyIds.Phouka);

                phouka.ShortName = PhoukaName();

                CombatScene.Start(phouka, () => Go<CampScene>());

                Player.CreateStatusEffect(StatusEffectIds.WebSilence, 0, 0, 0, 0);

            }
            else
            {
                OutputText("Scenes.Phoukascene.PhoukaTalk.TheWord".Localized(
                     PhoukaName()
                ));

                var __sw0_1 = Player.Level < 10 ? Random.Shared.Next(8) : Random.Shared.Next(12);

                if (__sw0_1 == 0 || __sw0_1 == 1 || __sw0_1 == 2 || __sw0_1 == 3 || __sw0_1 == 4 || __sw0_1 == 5 || __sw0_1 == 6)
                {
                    OutputText("Scenes.Phoukascene.PhoukaTalk.ConsidersForMoment".Localized(
                         PhoukaName(),

                         PhoukaName()
                    ));

                    if (Player.HasVagina())
                        OutputText("Scenes.Phoukascene.PhoukaTalk.MaybeYerIntaBunnies".Localized());
                    else
                        OutputText("Scenes.Phoukascene.PhoukaTalk.SayStart".Localized());

                    OutputText("Scenes.Phoukascene.PhoukaTalk.CouldChangeIntaGoat".Localized());

                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Phoukascene.PhoukaTalk.SayEnd".Localized());

                        OutputText("Scenes.Phoukascene.PhoukaTalk.OneMoreYeLike".Localized());
                    }
                    else
                        OutputText("Scenes.Phoukascene.PhoukaTalk.IfItAintTough".Localized());

                    ClearButtons();

                    PhoukaSexAddStandardMenuChoices();

                    AddNextButton("Common.Button.Fight".Localized(), PhoukaStartFight);
                }

                if (__sw0_1 == 7 || __sw0_1 == 8)
                {
                    OutputText("Scenes.Phoukascene.PhoukaTalk.TakesLongLook".Localized());

                    OutputText("Scenes.Phoukascene.PhoukaTalk.WriggleAgainstBonds".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.PhoukaLore) == 0)
                        CoC.Instance.Flags.Set(GameFlagIds.PhoukaLore, 1);

                    Player.UnlockCodexEntry(GameFlagIds.CodexEntryPhouka, true, false);

                    DoNext(() => ReturnToCamp(1));
                }

                if (true )
                {
                    OutputText("Scenes.Phoukascene.PhoukaTalk.BuzzesWideCircle".Localized());

                    OutputText("Scenes.Phoukascene.PhoukaTalk.TroubleWithThe".Localized(PhoukaName(), (noFur ? "rabbit-like human, with long ears and furry limbs, nearly four feet tall" : "large, black furred bunny, nearly four feet tall with a very human face and hands"), PhoukaName(), PhoukaName()));

                    if (CoC.Instance.Flags.Get(GameFlagIds.PhoukaLore) == 0)
                        CoC.Instance.Flags.Set(GameFlagIds.PhoukaLore, 1);

                    phoukaForm = PHOUKA_FORM_BUNNY;

                    if ((Player.Pregnancy.Incubation == 0) && (Player.ButtPregnancy.Incubation == 0))
                    {
                        OutputText("Scenes.Phoukascene.PhoukaTalk.PoursSomeClear".Localized());

                        Player.UnlockCodexEntry(GameFlagIds.CodexEntryPhouka, true, false);

                        ClearButtons();

                        AddButton(0, "Common.Button.Refuse".Localized(), PhoukaDrinkRefuse);

                        AddButton(1, "Scenes.Lumberjack.Button.Drink".Localized(), PhoukaDrinkAccept);
                    }
                    else
                    {
                        Player.UnlockCodexEntry(GameFlagIds.CodexEntryPhouka, true, false);

                        PhoukaDrinkWhilePregnant(false);
                    }
                }
            }
        }

        public void PhoukaDrinkWhilePregnant(bool playerOfferedTheBooze)
        {

            if ((Player.Pregnancy.Incubation <= 100) || (Player.ButtPregnancy.Incubation <= 100))
                OutputText("Scenes.Phoukascene.PhoukaDrinkWhilePregnant.OfferingFullCup".Localized());

            var acceptable = PhoukaWhiskeyAcceptable();

            if (acceptable < 0)
            {
                OutputText("Scenes.Phoukascene.PhoukaDrinkWhilePregnant.ThinkAboutUnborn".Localized((acceptable <= -3 ? "ren" : "")));

                if (acceptable == -4)
                    OutputText("Scenes.Phoukascene.PhoukaDrinkWhilePregnant.OneCraves".Localized());

                OutputText("Scenes.Phoukascene.PhoukaDrinkWhilePregnant.SureSomeElders".Localized());
            }
            else
            {
                OutputText("Scenes.Phoukascene.PhoukaDrinkWhilePregnant.PutOneHand".Localized());

                if (acceptable <= 2)
                    OutputText("Scenes.Phoukascene.PhoukaDrinkWhilePregnant.BabyNeedsKicks".Localized((acceptable == 2 ? "distended colon" : "womb")));
                else
                    OutputText("Scenes.Phoukascene.PhoukaDrinkWhilePregnant.BabiesNeedKicks".Localized());

                OutputText("Scenes.Phoukascene.PhoukaDrinkWhilePregnant.WhatsUpForGrabs".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Refuse".Localized(), (playerOfferedTheBooze ? PhoukaDrinkRefuseAfterOffer : PhoukaDrinkRefuse));

            if (acceptable != 0)
                AddButton(1, "Scenes.Lumberjack.Button.Drink".Localized(), PhoukaDrinkAccept);
        }

        public void PhoukaDrinkAccept()
        {
            ClearOutput();

            ShowImage("item-whiskey");

            OutputText("Scenes.Phoukascene.PhoukaDrinkAccept.AcceptCupAndGive".Localized());

            OutputText("Scenes.Phoukascene.PhoukaDrinkAccept.ByTheTime".Localized());

            Player.SlimeFeed();

            switch (PhoukaWhiskeyDrink())
            {
                case 0:
                {
                Player.ApplyStats(new DynStat(GameStats.Lust, Random.Shared.Next(5) + 1), new DynStat(GameStats.Corruption, Random.Shared.Next(2) + 1));
                }

                break;

                case 1:
                {
                OutputText("Scenes.Phoukascene.PhoukaDrinkAccept.WarmFeelingBelly".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Phoukascene.PhoukaDrinkAccept.AtFirstBaby".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Phoukascene.PhoukaDrinkAccept.QueasyWantTo".Localized());
                }

                break;
            }

            CoC.Instance.Flags.Set(GameFlagIds.PregnancyCorruption, CoC.Instance.Flags.Get(GameFlagIds.PregnancyCorruption) + 1);

            PhoukaWhiskeyAddStatus();

            if (Player.Toughness < 30)
            {
                OutputText("Scenes.Phoukascene.PhoukaDrinkAccept.QuicklyEndUpDrunk".Localized());

                ClearButtons();

                PhoukaSexAddStandardMenuChoices();
            }

            if (Player.Toughness < 70)
                OutputText("Scenes.Phoukascene.PhoukaDrinkAccept.LooksLikePhouka".Localized());
            else
                OutputText("Scenes.Phoukascene.PhoukaDrinkAccept.CanHandleYour".Localized());

            OutputText("Scenes.Phoukascene.PhoukaDrinkAccept.SoonBothStart".Localized());

            PhoukaDrinkTalk(false);
        }

        public void PhoukaDrinkRefuse()
        {
            ClearOutput();

            if (Player.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation <= 100)
            {
                OutputText("Scenes.Phoukascene.PhoukaDrinkRefuse.RabbitMorphSitsHeavily".Localized());

                ClearButtons();

                PhoukaSexAddStandardMenuChoices();

                AddButton(14, "Common.Button.Leave".Localized(), PhoukaDrinkRefusePregnantLeave);
            }
            else
            {
                OutputText("Scenes.Phoukascene.PhoukaDrinkRefuse.DamnPrude".Localized());

                if ((Player.LustPercent > 66) || ((Player.Corruption > 50) && (Player.Lust > 33)))
                {
                    OutputText("Scenes.Phoukascene.PhoukaDrinkRefuse.BeginToExtract".Localized());

                    ClearButtons();

                    PhoukaSexAddStandardMenuChoices();
                }
                else
                    ClearButtons();

                AddButton(14, "Common.Button.Leave".Localized(), PhoukaDrinkRefuseLeave);
            }
        }

        public void PhoukaDrinkRefuseLeave()
        {
            ClearOutput();

            OutputText("Scenes.Phoukascene.PhoukaDrinkRefuseLeave.ExtractFromMud".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void PhoukaDrinkRefusePregnantLeave()
        {
            ClearOutput();

            OutputText("Scenes.Phoukascene.PhoukaDrinkRefusePregnantLeave.BlackBunnyWaves".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void PhoukaDrinkRefuseAfterOffer()
        {
            ClearOutput();

            OutputText("Scenes.Phoukascene.PhoukaDrinkRefuseAfterOffer.LooksDisappointedDowns".Localized());

            PhoukaDrinkTalk(true);
        }

        public void PhoukaDrinkTalk(bool playerNotDrinking)
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.PhoukaLore) == 1)
            {
                CoC.Instance.Flags.Set(GameFlagIds.PhoukaLore, CoC.Instance.Flags.Get(GameFlagIds.PhoukaLore) + 1);

                OutputText("Scenes.Phoukascene.PhoukaDrinkTalk.FromTheStories".Localized());

                OutputText("Scenes.Phoukascene.PhoukaDrinkTalk.NowadaysSpend".Localized());

                OutputText("Scenes.Phoukascene.PhoukaDrinkTalk.AlcoholSettingDeeper".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.PhoukaLore) == 2)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.PhoukaLore, CoC.Instance.Flags.Get(GameFlagIds.PhoukaLore) + 1);

                    OutputText("Scenes.Phoukascene.PhoukaDrinkTalk.PhoukaSomething".Localized());

                    OutputText("Scenes.Phoukascene.PhoukaDrinkTalk.HardestPart".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.PhoukaLore) == 3)
                    {
                        CoC.Instance.Flags.Set(GameFlagIds.PhoukaLore, CoC.Instance.Flags.Get(GameFlagIds.PhoukaLore) + 1);

                        OutputText("Scenes.Phoukascene.PhoukaDrinkTalk.OnceFeelingWarm".Localized());

                        OutputText("Scenes.Phoukascene.PhoukaDrinkTalk.FaeriesAre".Localized());

                        OutputText("Scenes.Phoukascene.PhoukaDrinkTalk.GrabsAndShakes".Localized(
                             Player.Mf("laddie", "lassie")
                        ));

                        OutputText("Scenes.Phoukascene.PhoukaDrinkTalk.SettlesBackDown".Localized());
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.PhoukaLore) == 4)
                        {
                            OutputText("Scenes.Phoukascene.PhoukaDrinkTalk.AskWhatThey".Localized());

                            OutputText("Scenes.Phoukascene.PhoukaDrinkTalk.LaughsButSense".Localized());
                        }

            if (!playerNotDrinking && (Player.Toughness < 70))
            {
                OutputText("Scenes.Phoukascene.PhoukaDrinkTalk.WhenBottleRunsOut".Localized((noFur ? "" : "the swirling mess of black fur ")));

                ClearButtons();

                AddButton(0, "Common.Button.Leave".Localized(), PhoukaDrinkLeave);
            }
            else
            {
                if (playerNotDrinking)
                    OutputText("Scenes.Phoukascene.PhoukaDrinkTalk.StoriesContinue".Localized());
                else
                    OutputText("Scenes.Phoukascene.PhoukaDrinkTalk.DrinkAnotherCup".Localized());

                OutputText("Scenes.Phoukascene.PhoukaDrinkTalk.LittleGiggle".Localized((noFur ? "skin" : "fur")));

                phoukaForm = PHOUKA_FORM_FAERIE;

                if (Player.Lust < 33)
                    OutputText("Scenes.Phoukascene.PhoukaDrinkTalk.LooksLikeNeedsA".Localized());
                else
                {
                    OutputText("Scenes.Phoukascene.PhoukaDrinkTalk.LookAtSleeping".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Phoukascene.PhoukaDrinkTalk.BeginningToSwell".Localized((Player.HasVagina() ? " and your [vagina] starting to moisten" : "")));
                    else
                        if (Player.HasVagina())
                            OutputText("Scenes.Phoukascene.PhoukaDrinkTalk.StartingToMoisten".Localized());
                        else
                            OutputText("Scenes.Phoukascene.PhoukaDrinkTalk.SphincterTwitch".Localized());
                }

                ClearButtons();

                AddButton(0, "Common.Button.Leave".Localized(), () => PhoukaDrinkLeave());

                if (Player.Lust >= 33)
                {
                    AddButton(1, "Common.Button.RideFaerie".Localized(), PhoukaSexFaerieNonCombat);

                    if (Player.HasCock())
                        AddButton(2, "Common.Button.FuckFaerie".Localized(), PhoukaSexFaerieFemaleNonCombat);
                }
            }
        }

        public void PhoukaDrinkLeave()
        {

            if (Player.Lust < 33)
                OutputText("Scenes.Phoukascene.PhoukaDrinkLeave.IfBitHornier".Localized());
            else
                OutputText("Scenes.Phoukascene.PhoukaDrinkLeave.IfYouWanted".Localized());

            OutputText("Scenes.Phoukascene.PhoukaDrinkLeave.CouldTakeAdvantage".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void PhoukaHalloween()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.TreacleMineYearDone, DateTime.Now.Year);

            OutputText("Scenes.Phoukascene.PhoukaHalloween.OnceAgainWading".Localized());

            OutputText("Scenes.Phoukascene.PhoukaHalloween.SneakUpOnSource".Localized());

            OutputText("Scenes.Phoukascene.PhoukaHalloween.LookOverSeeTwo".Localized());

            if (Player.Gender == Gender.Female)
                OutputText("Scenes.Phoukascene.PhoukaHalloween.GoatMorphPhouka".Localized());

            OutputText("Scenes.Phoukascene.PhoukaHalloween.WhiteHatPullsOut".Localized());

            OutputText("Scenes.Phoukascene.PhoukaHalloween.BellowsPassedAround".Localized());

            if (Player.Strength < 40)
                OutputText("Scenes.Phoukascene.PhoukaHalloween.VeryDifficultTo".Localized());
            else
                if (Player.Strength < 80)
                    OutputText("Scenes.Phoukascene.PhoukaHalloween.TreaclePumping".Localized());
                else
                    OutputText("Scenes.Phoukascene.PhoukaHalloween.DontKnowWhy".Localized());

            OutputText("Scenes.Phoukascene.PhoukaHalloween.AfterMostTake".Localized());

            if (Player.Intelligence > 80)
                OutputText("Scenes.Phoukascene.PhoukaHalloween.AboutToAcceptWhen".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PhoukaLore) > 0)
                Player.UnlockCodexEntry(GameFlagIds.CodexEntryPhouka, true, true);

            ClearButtons();

            AddButton(0, "Common.Button.Taste".Localized(), PhoukaHalloweenTreacleEater);

            if (Player.Intelligence > 80)
                AddButton(1, "Common.Button.BackAway".Localized(), PhoukaHalloweenLeave);
        }

        public void PhoukaHalloweenLeave()
        {

            OutputText("Scenes.Phoukascene.PhoukaHalloweenLeave.BetterOffNot".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void PhoukaHalloweenTreacleEater()
        {
            ClearOutput();

            ShowImage("phouka-adventure");

            OutputText("Scenes.Phoukascene.PhoukaHalloweenTreacleEater.TakeCupAndSqueeze".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void PhoukaLeaveOnLustWin(bool newScreen = true)
        {
            ClearOutput();

            if (!(newScreen))
                OutputText("Scenes.Phoukascene.PhoukaLeaveOnLustWin.ParagraphBreak".Localized());

            OutputText("Scenes.Phoukascene.PhoukaLeaveOnLustWin.LustyContinuesToPound".Localized(
                 PhoukaName()
            ));

            CoC.Instance.Player.ClearCombatStatuses();

            CombatController.Instance?.Reward.Award();

            CombatScene.CleanupAfterCombat();
        }

        public void PhoukaPlayerWins(bool hpVictory)
        {
            ClearOutput();

            if (hpVictory)
            {
                OutputText("Scenes.Phoukascene.PhoukaPlayerWins.SeriouslyInjuredStumbles".Localized(
                     PhoukaName()
                ));

                CoC.Instance.Player.ClearCombatStatuses();

                CombatController.Instance?.Reward.Award();

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.Phoukascene.PhoukaPlayerWins.CollapsesToGround".Localized(
                     PhoukaName()
                ));

                if (phoukaForm != PHOUKA_FORM_FAERIE)
                {
                    OutputText("Scenes.Phoukascene.PhoukaPlayerWins.WispsOfBlack".Localized());

                    if (phoukaForm == PHOUKA_FORM_BUNNY)
                        OutputText("Scenes.Phoukascene.PhoukaPlayerWins.BunnyMorph".Localized());
                    else
                        if (phoukaForm == PHOUKA_FORM_GOAT)
                            OutputText("Scenes.Phoukascene.PhoukaPlayerWins.GoatMorph".Localized());
                        else
                            if (phoukaForm == PHOUKA_FORM_HORSE)
                                OutputText("Scenes.Phoukascene.PhoukaPlayerWins.Stallion".Localized());

                    OutputText("Scenes.Phoukascene.PhoukaPlayerWins.ShrunkBackDown".Localized(
                         PhoukaNameText("phouka", "faerie")
                    ));

                    phoukaForm = PHOUKA_FORM_FAERIE;
                }

                if (Player.Lust < 33)
                {
                    OutputText("Scenes.Phoukascene.PhoukaPlayerWins.ThreatDealtWith".Localized());

                    PhoukaLeaveOnLustWin(false);
                }
                else
                {
                    OutputText("Scenes.Phoukascene.PhoukaPlayerWins.WhileBusyExtract".Localized(
                         PhoukaName()
                    ));

                    if (Player.Corruption < 34)
                        OutputText("Scenes.Phoukascene.PhoukaPlayerWins.CouldSateYourself".Localized());
                    else
                        if (Player.Corruption < 67)
                            OutputText("Scenes.Phoukascene.PhoukaPlayerWins.CouldTakeAdvantage".Localized());
                        else
                            OutputText("Scenes.Phoukascene.PhoukaPlayerWins.SoEasyTo".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Leave".Localized(), () => PhoukaLeaveOnLustWin());

                    AddButton(1, "Common.Button.AnalRide".Localized(), PhoukaSexFaeriePostCombat);

                    if (Player.HasCock())
                        AddButton(2, "Common.Button.FuckFaerie".Localized(), PhoukaSexFaerieFemalePostCombat);

                    if (Player.HasMultiTails())
                        AddNextButton("Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());
                }
            }
        }

        public void VaginalBirth()
        {

            ShowImage("birth-phouka");

            OutputText("Scenes.Phoukascene.VaginalBirth.FeelSomethingGiveWay".Localized());

            if (Player.Vaginas.Count == 0)
            {
                OutputText("Scenes.Phoukascene.VaginalBirth.PainfulPressure".Localized());

                Player.CreateVagina();
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.PregnancyCorruption) > 6)
            {
                OutputText("Scenes.Phoukascene.VaginalBirth.BellyBeginsDeflate".Localized());

                OutputText("Scenes.Phoukascene.VaginalBirth.DespitePain".Localized(
                     PhoukaName()
                ));

                Player.Fertility -= 18;

                if (Player.Fertility < 5)
                    Player.Fertility = 5;

                Player.ChangeFatigue(75);

                CoC.Instance.Flags.Set(GameFlagIds.BirthsPhouka, CoC.Instance.Flags.Get(GameFlagIds.BirthsPhouka) + 1);

                Player.Orgasm("Vaginal");
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.PregnancyCorruption) > 0)
                {
                    OutputText("Scenes.Phoukascene.VaginalBirth.BellyBeginsDeflateAgain".Localized());

                    OutputText("Scenes.Phoukascene.VaginalBirth.ThenYouFeelIt".Localized());

                    OutputText("Scenes.Phoukascene.VaginalBirth.SeemsAlmostFull".Localized(
                         PhoukaName()
                    ));

                    OutputText("Scenes.Phoukascene.VaginalBirth.LayOnGroundExhausted".Localized());

                    Player.Fertility -= 8;

                    if (Player.Fertility < 5)
                        Player.Fertility = 5;

                    Player.ChangeFatigue(50);

                    CoC.Instance.Flags.Set(GameFlagIds.BirthsPhouka, CoC.Instance.Flags.Get(GameFlagIds.BirthsPhouka) + 1);
                }
                else
                {
                    OutputText("Scenes.Phoukascene.VaginalBirth.BellyBeginsDeflateThird".Localized());

                    OutputText("Scenes.Phoukascene.VaginalBirth.FullGrownFaerieLooks".Localized());

                    OutputText("Scenes.Phoukascene.VaginalBirth.FaerieGirlStartsFly".Localized());

                    Player.ChangeFatigue(5);

                    CoC.Instance.Flags.Set(GameFlagIds.BirthsFaerie, CoC.Instance.Flags.Get(GameFlagIds.BirthsFaerie) + 1);
                }
        }

        public bool UpdateVaginalPregnancy()
        {

            if (Player.Pregnancy.Incubation == 170)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.PregnancyCorruption) > 0)
                    OutputText("Scenes.Phoukascene.UpdateVaginalPregnancy.StillSolidAnd".Localized());
                else
                    OutputText("Scenes.Phoukascene.UpdateVaginalPregnancy.StillSolidAndAgain".Localized());

                return true;
            }

            if (Player.Pregnancy.Incubation == 140)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.PregnancyCorruption) > 0)
                    OutputText("Scenes.Phoukascene.UpdateVaginalPregnancy.BitSofter".Localized());
                else
                    OutputText("Scenes.Phoukascene.UpdateVaginalPregnancy.BitSofterAgain".Localized());

                return true;
            }

            if (Player.Pregnancy.Incubation == 100)
            {
                OutputText("Scenes.Phoukascene.UpdateVaginalPregnancy.FeelsLikeIts".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.PregnancyCorruption) > 0)
                    OutputText("Scenes.Phoukascene.UpdateVaginalPregnancy.ThatPartMoreComfortable".Localized());
                else
                    OutputText("Scenes.Phoukascene.UpdateVaginalPregnancy.MuchMoreComfortable".Localized());

                OutputText("Scenes.Phoukascene.UpdateVaginalPregnancy.NoticedThisPregnancy".Localized());

                return true;
            }

            if (Player.Pregnancy.Incubation == 60)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.PregnancyCorruption) > 6)
                    OutputText("Scenes.Phoukascene.UpdateVaginalPregnancy.WhateverUncleanSpawn".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.PregnancyCorruption) > 0)
                        OutputText("Scenes.Phoukascene.UpdateVaginalPregnancy.WhateverUncleanSpawnAgain".Localized());
                    else
                        OutputText("Scenes.Phoukascene.UpdateVaginalPregnancy.WhateverKindOfLife".Localized());

                return true;
            }

            if (Player.Pregnancy.Incubation == 36)
            {
                OutputText("Scenes.Phoukascene.UpdateVaginalPregnancy.EvenThoughBellyRemains".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.PregnancyCorruption) > 6)
                    OutputText("Scenes.Phoukascene.UpdateVaginalPregnancy.DespiteSmallSize".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.PregnancyCorruption) > 0)
                        OutputText("Scenes.Phoukascene.UpdateVaginalPregnancy.DespiteRelativelySmall".Localized());
                    else
                        OutputText("Scenes.Phoukascene.UpdateVaginalPregnancy.DespiteBeingPregnant".Localized());

                return true;
            }

            return false;
        }

        public void PhoukaSexAddStandardMenuChoices()
        {

            AddNextButton("Common.Button.Bunny".Localized(), PhoukaSexBunnyChoice);

            AddNextButton("Common.Button.Horse".Localized(), PhoukaSexHorseChoice);

            AddNextButton("Common.Button.Goat".Localized(), PhoukaSexGoatChoice);
        }

        public void PhoukaSexBunny(bool postCombat, bool lustLoss)
        {
            ClearOutput();

            if (postCombat)
            {
                OutputText("Scenes.Phoukascene.PhoukaSexBunny.AsYouCollapseThe".Localized(
                     PhoukaName()
                ));

                if (phoukaForm == PHOUKA_FORM_BUNNY)
                    OutputText("Scenes.Phoukascene.PhoukaSexBunny.HopsOverStands".Localized());
                else
                    OutputText("Scenes.Phoukascene.PhoukaSexBunny.BeginsToShapeshift".Localized((noFur ? "skinned" : "furred")));

                OutputText("Scenes.Phoukascene.PhoukaSexBunny.WatchBunnyS".Localized());

                if (lustLoss)
                    OutputText("Scenes.Phoukascene.PhoukaSexBunny.MindOvercomeWithLust".Localized());
                else
                    OutputText("Scenes.Phoukascene.PhoukaSexBunny.BodyTooBruised".Localized());

                OutputText("Scenes.Phoukascene.PhoukaSexBunny.ResistAnyFurther".Localized());
            }
            else
                if (Player.Corruption <= 50)
                {
                    OutputText("Scenes.Phoukascene.PhoukaSexBunny.MightNotBeWisest".Localized(
                         PhoukaName(),

                         PhoukaName()
                    ));

                    if (phoukaForm != PHOUKA_FORM_BUNNY)
                        OutputText("Scenes.Phoukascene.PhoukaSexBunny.MorphsIntoRabbit".Localized());

                    OutputText("Scenes.Phoukascene.PhoukaSexBunny.HopefullyHellBeGentle".Localized());
                }
                else
                {
                    OutputText("Scenes.Phoukascene.PhoukaSexBunny.CantThinkOfAny".Localized(
                         PhoukaName(),

                         PhoukaName()
                    ));

                    if (phoukaForm != PHOUKA_FORM_BUNNY)
                        OutputText("Scenes.Phoukascene.PhoukaSexBunny.ShortSeries".Localized());
                }

            phoukaForm = PHOUKA_FORM_BUNNY;

            ShowImage("phouka-sex-bunny");

            OutputText("Scenes.Phoukascene.PhoukaSexBunny.BunnyAdvances".Localized());

            if ((Player.LustPercent > 80) || (Player.Corruption > 50))
                OutputText("Scenes.Phoukascene.PhoukaSexBunny.CanOnlyMoan".Localized());
            else
                OutputText("Scenes.Phoukascene.PhoukaSexBunny.PowerlessToStop".Localized());

            OutputText("Scenes.Phoukascene.PhoukaSexBunny.LargeFeetTake".Localized(PhoukaName(), (noFur ? "" : "Shivers run up your spine as his fur tickles your [if (tailLeg) {scales from the top to the tip of your tail|legs, back, butt and thighs}]. ")));

            if (Player.Vaginas.Count > 0 && Player.Vaginas[0].Virgin)
                OutputText("Scenes.Phoukascene.PhoukaSexBunny.ScreamAsCock".Localized(
                     PhoukaName()
                ));
            else
                if (postCombat && (Player.Vaginas[0].Looseness < 2))
                    OutputText("Scenes.Phoukascene.PhoukaSexBunny.NearlyPassOut".Localized(
                         PhoukaName()
                    ));
                else
                    if (!postCombat || (Player.Corruption > 50))
                        OutputText("Scenes.Phoukascene.PhoukaSexBunny.InvaderLengthens".Localized(
                             PhoukaName()
                        ));

            Player.CuntChange(50, true);

            OutputText("Scenes.Phoukascene.PhoukaSexBunny.StartsToUseHis".Localized());

            PhoukaSexPregnate(postCombat);
        }

        public void PhoukaSexBunnyChoice()
        {

            PhoukaSexBunny(false, false);
        }

        public void PhoukaSexGoat(bool postCombat, bool lustLoss)
        {
            ClearOutput();

            if (postCombat)
            {
                if (lustLoss)
                    OutputText("Scenes.Phoukascene.PhoukaSexGoat.PantingWithNeed".Localized(
                         PhoukaName()
                    ));
                else
                    OutputText("Scenes.Phoukascene.PhoukaSexGoat.BodyFallsToGround".Localized(
                         PhoukaName()
                    ));

                if (phoukaForm != PHOUKA_FORM_GOAT)
                    OutputText("Scenes.Phoukascene.PhoukaSexGoat.AsYouLayThere".Localized(
                        PhoukaName(),

                        noFur ? "skinned" : "furred"
                    ));
            }
            else
            {
                OutputText("Scenes.Phoukascene.PhoukaSexGoat.TellWilling".Localized(PhoukaName(), (noFur ? "skinned" : "furred")));

                if (Player.Corruption <= 50)
                    OutputText("Scenes.Phoukascene.PhoukaSexGoat.SteelYourself".Localized());
                else
                    OutputText("Scenes.Phoukascene.PhoukaSexGoat.PutOnAShow".Localized());
            }

            phoukaForm = PHOUKA_FORM_GOAT;

            ShowImage("phouka-sex-goat");

            if (Player.HasVagina())
                OutputText("Scenes.Phoukascene.PhoukaSexGoat.SaysTheGoatWrapping".Localized());
            else
                OutputText("Scenes.Phoukascene.PhoukaSexGoat.SaysTheGoat".Localized());

            if (postCombat)
                OutputText("Scenes.Phoukascene.PhoukaSexGoat.MountsYouImmediately".Localized());
            else
                OutputText("Scenes.Phoukascene.PhoukaSexGoat.RubsCockheadAgainst".Localized());

            OutputText("Scenes.Phoukascene.PhoukaSexGoat.FrontHooves".Localized());

            if (postCombat)
                OutputText("Scenes.Phoukascene.PhoukaSexGoat.ShowsNoInterest".Localized(
                     PhoukaName()
                ));
            else
                OutputText("Scenes.Phoukascene.PhoukaSexGoat.OnceHasAdjusted".Localized());

            OutputText("Scenes.Phoukascene.PhoukaSexGoat.OnceCockIsIn".Localized());

            Player.ButtChange(45, true, true, false);

            OutputText("Scenes.Phoukascene.PhoukaSexGoat.CockExpandedFull".Localized(
                 PhoukaName(),

                 PhoukaName()
            ));

            if (postCombat)
                OutputText("Scenes.Phoukascene.PhoukaSexGoat.WaitsOnlyA".Localized());
            else
                OutputText("Scenes.Phoukascene.PhoukaSexGoat.RealizingHaventYet".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, Random.Shared.Next(2) + (postCombat ? 1 : 3)));

            Player.Orgasm("Anal");

            if (postCombat)
                OutputText("Scenes.Phoukascene.PhoukaSexGoat.WhileRecoveringGoat".Localized());

            OutputText("Scenes.Phoukascene.PhoukaSexGoat.BeginsToDissolve".Localized(
                 PhoukaName()
            ));

            if (postCombat)
            {
                OutputText("Scenes.Phoukascene.PhoukaSexGoat.WorkToPullFree".Localized());

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.Phoukascene.PhoukaSexGoat.CollectClothesScramble".Localized());

                ReturnToCamp(1);
            }
        }

        public void PhoukaSexGoatChoice()
        {

            PhoukaSexGoat(false, false);
        }

        public void PhoukaSexHorse(bool postCombat, bool lustLoss)
        {
            ClearOutput();

            if (postCombat)
            {
                OutputText("Scenes.Phoukascene.PhoukaSexHorse.AsYouCollapseThe".Localized(
                     PhoukaName()
                ));

                if (phoukaForm == PHOUKA_FORM_HORSE)
                    OutputText("Scenes.Phoukascene.PhoukaSexHorse.CirclesInSlowTrot".Localized());
                else
                    OutputText("Scenes.Phoukascene.PhoukaSexHorse.BeginsToShapeshift".Localized());
            }
            else
                if (Player.Corruption <= 50)
                    OutputText("Scenes.Phoukascene.PhoukaSexHorse.PerhapsLandStarting".Localized(
                         PhoukaName(),

                         PhoukaName()
                    ));
                else
                    OutputText("Scenes.Phoukascene.PhoukaSexHorse.MoreYouThinkAbout".Localized(
                         PhoukaName(),

                         PhoukaName()
                    ));

            phoukaForm = PHOUKA_FORM_HORSE;

            OutputText("Scenes.Phoukascene.PhoukaSexHorse.CloserInspectionCentaur".Localized());

            if (!postCombat || lustLoss)
                OutputText("Scenes.Phoukascene.PhoukaSexHorse.ConsideringFire".Localized());
            else
                OutputText("Scenes.Phoukascene.PhoukaSexHorse.HardlyInCondition".Localized());

            OutputText("Scenes.Phoukascene.PhoukaSexHorse.StallionAdvances".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Phoukascene.PhoukaSexHorse.WalksUpBehind".Localized(
                     PhoukaName()
                ));
            else
            {
                if (Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Phoukascene.PhoukaSexHorse.WalksInFront".Localized());
                else
                    OutputText("Scenes.Phoukascene.PhoukaSexHorse.WalksUpBehindAgain".Localized());

                OutputText("Scenes.Phoukascene.PhoukaSexHorse.HearTheSay".Localized(
                     PhoukaName()
                ));
            }

            OutputText("Scenes.Phoukascene.PhoukaSexHorse.ConvincedGoing".Localized());

            ShowImage("phouka-sex-horse");

            if (Player.Vaginas.Count > 0 && Player.Vaginas[0].Virgin)
                OutputText("Scenes.Phoukascene.PhoukaSexHorse.ScreamAsCock".Localized(
                     PhoukaName(),

                     PhoukaName()
                ));
            else
                if (postCombat && (Player.Vaginas[0].Looseness < 3))
                    OutputText("Scenes.Phoukascene.PhoukaSexHorse.NearlyPassOut".Localized(
                         PhoukaName()
                    ));
                else
                    if (!postCombat || (Player.Corruption > 50))
                        OutputText("Scenes.Phoukascene.PhoukaSexHorse.InvaderThrustsDeeper".Localized(
                             PhoukaName()
                        ));

            Player.CuntChange(80, true);

            OutputText("Scenes.Phoukascene.PhoukaSexHorse.IfPregnantHeadSmack".Localized());

            if ((Player.LustPercent > 80) || (Player.Corruption > 50))
                OutputText("Scenes.Phoukascene.PhoukaSexHorse.PantAndMoanWaiting".Localized());
            else
            {
                OutputText("Scenes.Phoukascene.PhoukaSexHorse.WhimperInPain".Localized(
                     PhoukaName()
                ));

                if (Player.IsTaur())
                    OutputText("Scenes.Phoukascene.PhoukaSexHorse.StretchedAndFull".Localized());
                else
                    OutputText("Scenes.Phoukascene.PhoukaSexHorse.SoStretchedThat".Localized());
            }

            OutputText("Scenes.Phoukascene.PhoukaSexHorse.CumStuffedBalls".Localized(
                 PhoukaName()
            ));

            PhoukaSexPregnate(postCombat);
        }

        public void PhoukaSexHorseChoice()
        {

            PhoukaSexHorse(false, false);
        }

        public void PhoukaSexFaerie(bool postCombat)
        {
            ClearOutput();

            OutputText("Scenes.Phoukascene.PhoukaSexFaerie.DoesntLookLike".Localized(
                 PhoukaName(),

                 PhoukaName(),

                 PhoukaName()
            ));

            if (Player.IsTaur())
                OutputText("Scenes.Phoukascene.PhoukaSexFaerie.KnowCantReally".Localized());
            else
                OutputText("Scenes.Phoukascene.PhoukaSexFaerie.NotWantingHimThink".Localized(
                     PhoukaName()
                ));

            if (Player.HasVagina())
            {
                OutputText("Scenes.Phoukascene.PhoukaSexFaerie.VaginaPeriodBreak".Localized());

                Player.CuntChange(20, true);

                ShowImage("phouka-sex");

                OutputText("Scenes.Phoukascene.PhoukaSexFaerie.GettingCloserAndCloser".Localized(
                     PhoukaName()
                ));

                PhoukaSexPregnate(true);
            }
            else
            {
                OutputText("Scenes.Phoukascene.PhoukaSexFaerie.ButtPeriodBreak".Localized());

                Player.ButtChange(20, true);

                ShowImage("phouka-sex-goat");

                OutputText("Scenes.Phoukascene.PhoukaSexFaerie.LovingTheFeeling".Localized(
                     PhoukaName()
                ));

                if (Player.IsTaur())
                    OutputText("Scenes.Phoukascene.PhoukaSexFaerie.SighInFrustration".Localized(
                         PhoukaName()
                    ));
                else
                    OutputText("Scenes.Phoukascene.PhoukaSexFaerie.NotYetSatisfied".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Phoukascene.PhoukaSexFaerie.FiresSeveralBlasts".Localized());

                OutputText("Scenes.Phoukascene.PhoukaSexFaerie.SphincterClenches".Localized());

                OutputText("Scenes.Phoukascene.PhoukaSexFaerie.AboutToViolently".Localized(
                     PhoukaName(),

                     PhoukaName()
                ));

                Player.ApplyStats(new DynStat(GameStats.Corruption, Random.Shared.Next(2) + 3));

                if (postCombat)
                {
                    CoC.Instance.Player.ClearCombatStatuses();

                    CombatController.Instance?.Reward.Award();
                }
                else
                    DoNext(() => ReturnToCamp(1));
            }

            CombatScene.CleanupAfterCombat();
        }

        public void PhoukaSexFaerieNonCombat()
        {

            PhoukaSexFaerie(false);
        }

        public void PhoukaSexFaeriePostCombat()
        {

            PhoukaSexFaerie(true);
        }

        public void PhoukaSexFaerieFemale(bool postCombat)
        {
            ClearOutput();

            OutputText("Scenes.Phoukascene.PhoukaSexFaerieFemale.DoesntLookLike".Localized(
                 PhoukaName()
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.PhoukaLore) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.PhoukaLore, 1);

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryPhouka, true, true);

            OutputText("Scenes.Phoukascene.PhoukaSexFaerieFemale.PointOutToHim".Localized());

            OutputText("Scenes.Phoukascene.PhoukaSexFaerieFemale.ConcentratesAndSee".Localized());

            OutputText("Scenes.Phoukascene.PhoukaSexFaerieFemale.LooksLittleUncomfortable".Localized());

            OutputText("Scenes.Phoukascene.PhoukaSexFaerieFemale.TellHimWantMore".Localized());

            OutputText("Scenes.Phoukascene.PhoukaSexFaerieFemale.WhenPhoukaSpeaks".Localized());

            OutputText("Scenes.Phoukascene.PhoukaSexFaerieFemale.AskHowSheIntends".Localized());

            OutputText("Scenes.Phoukascene.PhoukaSexFaerieFemale.WhenItsOver".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.FreedVala) == 1) || (CoC.Instance.Flags.Get(GameFlagIds.TimesPcDefeatedVala) > 0) || (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedValaInDungeon) > 0) || (CoC.Instance.Flags.Get(GameFlagIds.InvestigatedValaAfterZetazDefeated) > 0))
                OutputText("Scenes.Phoukascene.PhoukaSexFaerieFemale.RemindsYouQuite".Localized());
            else
                OutputText("Scenes.Phoukascene.PhoukaSexFaerieFemale.StillLooksMoreHuman".Localized());

            ShowImage("phouka-sex-faerie");

            OutputText("Scenes.Phoukascene.PhoukaSexFaerieFemale.MostImportantThing".Localized());

            if (Player.BiggestCockLength() < 8)
                OutputText("Scenes.Phoukascene.PhoukaSexFaerieFemale.PushLastInch".Localized(
                     (int)(Player.BiggestCockLength())
                ));
            else
                if (Player.BiggestCockLength() < 15)
                    OutputText("Scenes.Phoukascene.PhoukaSexFaerieFemale.FeedLastInch".Localized(
                         (int)(Player.BiggestCockLength())
                    ));
                else
                {
                    OutputText("Scenes.Phoukascene.PhoukaSexFaerieFemale.DriveAsMuch".Localized());

                    if (Player.Corruption <= 50)
                        OutputText("Scenes.Phoukascene.PhoukaSexFaerieFemale.StillFeelsAmazing".Localized());
                    else
                        OutputText("Scenes.Phoukascene.PhoukaSexFaerieFemale.NotGoodEnough".Localized());
                }

            OutputText("Scenes.Phoukascene.PhoukaSexFaerieFemale.FillHerWithSpurt".Localized());

            if (Player.Corruption <= 33)
                OutputText("Scenes.Phoukascene.PhoukaSexFaerieFemale.CertainlyDontWant".Localized());
            else
                if (Player.Corruption <= 66)
                    OutputText("Scenes.Phoukascene.PhoukaSexFaerieFemale.LaughAndRemind".Localized());
                else
                    OutputText("Scenes.Phoukascene.PhoukaSexFaerieFemale.WhyShouldntOther".Localized());

            OutputText("Scenes.Phoukascene.PhoukaSexFaerieFemale.SatisfiedMarchBack".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, Random.Shared.Next(2) + 3));

            if (postCombat)
            {
                CoC.Instance.Player.ClearCombatStatuses();

                CombatController.Instance?.Reward.Award();

                CombatScene.CleanupAfterCombat();
            }
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void PhoukaSexFaerieFemaleNonCombat()
        {

            PhoukaSexFaerieFemale(false);
        }

        public void PhoukaSexFaerieFemalePostCombat()
        {

            PhoukaSexFaerieFemale(true);
        }

        public void PhoukaSexPregnate(bool postCombat)
        {

            if (Player.IsPregnant())
            {
                if (phoukaForm == PHOUKA_FORM_HORSE)
                    OutputText("Scenes.Phoukascene.PhoukaSexPregnate.ConstantPressureAgainst".Localized(
                         PhoukaName()
                    ));
                else
                    OutputText("Scenes.Phoukascene.PhoukaSexPregnate.GallonsBlastAgainst".Localized());

                if (Player.Corruption > 50)
                    OutputText("Scenes.Phoukascene.PhoukaSexPregnate.FeelingAllSeed".Localized());
                else
                    OutputText("Scenes.Phoukascene.PhoukaSexPregnate.ThickGrayCumLeaks".Localized());

                if (phoukaForm == PHOUKA_FORM_FAERIE)
                    OutputText("Scenes.Phoukascene.PhoukaSexPregnate.NowThatYouveGot".Localized());

                PhoukaSexPregnateEnd(postCombat);

                return ;
            }

            if (phoukaForm == PHOUKA_FORM_HORSE)
                OutputText("Scenes.Phoukascene.PhoukaSexPregnate.ConstantPressureBuilding".Localized(
                     PhoukaName()
                ));
            else
                OutputText("Scenes.Phoukascene.PhoukaSexPregnate.GallonsForcePast".Localized(
                     PhoukaName()
                ));

            Player.KnockUp(FaeriePregnancy, FaerieIncubation);

            if (phoukaForm == PHOUKA_FORM_FAERIE)
                OutputText("Scenes.Phoukascene.PhoukaSexPregnate.MayHaveCumBut".Localized(
                     PhoukaName(),

                     PhoukaName(),

                     PhoukaName()
                ));

            if ((!postCombat) || (Player.Corruption > 50))
            {
                if (phoukaForm == PHOUKA_FORM_HORSE)
                    OutputText("Scenes.Phoukascene.PhoukaSexPregnate.CertainlyGotOffBut".Localized(
                         PhoukaName(),

                         PhoukaName()
                    ));
                else
                {
                    OutputText("Scenes.Phoukascene.PhoukaSexPregnate.ThankfullyBeginsToRub".Localized(
                         PhoukaName()
                    ));

                    if (postCombat)
                        OutputText("Scenes.Phoukascene.PhoukaSexPregnate.BastardLaughs".Localized(
                             PhoukaName()
                        ));
                    else
                        OutputText("Scenes.Phoukascene.PhoukaSexPregnate.PerhapsHesBeing".Localized());
                }
            }
            else
            {
                if (phoukaForm == PHOUKA_FORM_HORSE)
                    OutputText("Scenes.Phoukascene.PhoukaSexPregnate.FeelTheLift".Localized(
                         PhoukaName()
                    ));
                else
                    OutputText("Scenes.Phoukascene.PhoukaSexPregnate.AddInsultInjury".Localized(
                         PhoukaName()
                    ));

                OutputText("Scenes.Phoukascene.PhoukaSexPregnate.AsHeGetsOff".Localized());
            }

            if (phoukaForm == PHOUKA_FORM_FAERIE)
                OutputText("Scenes.Phoukascene.PhoukaSexPregnate.WholeBodyDeflates".Localized(
                     PhoukaName()
                ));
            else
                if (phoukaForm == PHOUKA_FORM_HORSE)
                    OutputText("Scenes.Phoukascene.PhoukaSexPregnate.BlackFurredHorseSteps".Localized());
                else
                    if (phoukaForm == PHOUKA_FORM_BUNNY)
                        OutputText("Scenes.Phoukascene.PhoukaSexPregnate.BlackBunnyPullsOut".Localized((noFur ? "skinned" : "furred")));
                    else
                        OutputText("Scenes.Phoukascene.PhoukaSexPregnate.HeWord".Localized());

            if (postCombat)
                OutputText("Scenes.Phoukascene.PhoukaSexPregnate.RummagesAroundGemPouch".Localized());

            OutputText("Scenes.Phoukascene.PhoukaSexPregnate.TakesToTheAir".Localized());

            if (Player.IsPregnant())
            {
                var isFirstTime = (CoC.Instance.Flags.Get(GameFlagIds.BirthsPhouka) == 0) && (CoC.Instance.Flags.Get(GameFlagIds.BirthsFaerie) == 0);

                CoC.Instance.Flags.Set(GameFlagIds.PregnancyCorruption, 2);

                ShowImage("phouka-adventure");

                OutputText("Scenes.Phoukascene.PhoukaSexPregnate.AboutToLiftYourself".Localized((isFirstTime ? "an unusual" : "a familiar"), PhoukaName()));

                if (CoC.Instance.Flags.Get(GameFlagIds.PhoukaLore) == 0)
                    CoC.Instance.Flags.Set(GameFlagIds.PhoukaLore, 1);

                if (!postCombat || (Player.Corruption > 50))
                    OutputText("Scenes.Phoukascene.PhoukaSexPregnate.YerRealPrizeSlut".Localized());

                OutputText("Scenes.Phoukascene.PhoukaSexPregnate.GainsMoreAltitude".Localized());

                Player.UnlockCodexEntry(GameFlagIds.CodexEntryPhouka, true, true);

                if (isFirstTime)
                    OutputText("Scenes.Phoukascene.PhoukaSexPregnate.WonderHowLong".Localized());
                else
                    OutputText("Scenes.Phoukascene.PhoukaSexPregnate.AtLeastYouKnow".Localized());
            }
            else
                OutputText("Scenes.Phoukascene.PhoukaSexPregnate.WhenYouPullUp".Localized(
                     PhoukaName()
                ));

            PhoukaSexPregnateEnd(postCombat);
        }

        public void PhoukaSexPregnateEnd(bool postCombat)
        {

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Corruption, Random.Shared.Next(2) + (postCombat && (phoukaForm != PHOUKA_FORM_FAERIE) ? 1 : 3)));

            if (phoukaForm == PHOUKA_FORM_FAERIE)
            {
                OutputText("Scenes.Phoukascene.PhoukaSexPregnateEnd.SatisfiedForNow".Localized(
                     PhoukaName()
                ));

                if (Player.Corruption > 50)
                    OutputText("Scenes.Phoukascene.PhoukaSexPregnateEnd.ButEitherWay".Localized());

                if (postCombat)
                {
                    CoC.Instance.Player.ClearCombatStatuses();

                    CombatController.Instance?.Reward.Award();

                    CombatScene.CleanupAfterCombat();
                }
                else
                    DoNext(() => ReturnToCamp(1));
            }
            else
            {
                if (postCombat && Player.Gems > 0)
                    OutputText("Scenes.Phoukascene.PhoukaSexPregnateEnd.WhileRecoveringReaches".Localized(
                         PhoukaName()
                    ));

                OutputText("Scenes.Phoukascene.PhoukaSexPregnateEnd.NowThatItsFinished".Localized(
                     PhoukaName()
                ));

                OutputText("Scenes.Phoukascene.PhoukaSexPregnateEnd.YouWord".Localized());

                if (phoukaForm == PHOUKA_FORM_HORSE && !Player.IsTaur())
                    OutputText("Scenes.Phoukascene.PhoukaSexPregnateEnd.WaitForThrobbingPain".Localized());

                if (postCombat)
                {
                    OutputText("Scenes.Phoukascene.PhoukaSexPregnateEnd.PullFreeFromCool".Localized());

                    CombatScene.CleanupAfterCombat();
                }
                else
                {
                    OutputText("Scenes.Phoukascene.PhoukaSexPregnateEnd.CollectClothesBegin".Localized());

                    DoNext(() => ReturnToCamp(1));
                }

                if (Player.Corruption <= 50)
                    OutputText("Scenes.Phoukascene.PhoukaSexPregnateEnd.TrudgeBackToCamp".Localized());
                else
                    if (phoukaForm == PHOUKA_FORM_HORSE && Player.IsTaur())
                        OutputText("Scenes.Phoukascene.PhoukaSexPregnateEnd.QuiteTheRide".Localized());
                    else
                        OutputText("Scenes.Phoukascene.PhoukaSexPregnateEnd.NotWhatYouExpected".Localized());
            }
        }

        public static int PhoukaWhiskeyAcceptable()
        {
            if (Player.Pregnancy.Incubation == 0)
            {
                if (Player.ButtPregnancy.Incubation == 0)
                    return 0;

                if (Player.ButtPregnancy.Type == PregnancyIds.Satyr)
                    return 2;

                return -2;
            }

            if (Player.ButtPregnancy.Incubation == 0)
            {
                if (Player.Pregnancy.Type == PregnancyIds.Satyr || Player.Pregnancy.Type == FaeriePregnancy)
                    return 1;

                return -1;
            }

            var wombBabyLikesAlcohol = Player.Pregnancy.Type == PregnancyIds.Satyr || Player.Pregnancy.Type == FaeriePregnancy;

            var colonBabyLikesAlcohol = Player.ButtPregnancy.Type == PregnancyIds.Satyr;

            if (wombBabyLikesAlcohol && colonBabyLikesAlcohol)
                return 3;

            if (!wombBabyLikesAlcohol && !colonBabyLikesAlcohol)
                return -3;

            return -4;
        }

        public static int PhoukaWhiskeyDrink()
        {
            if (Player.Pregnancy.Incubation == 0 && Player.ButtPregnancy.Incubation == 0)
                return 0;

            if (Player.Pregnancy.Type == FaeriePregnancy)
            {
                var corruption = CoC.Instance.Flags.Get(GameFlagIds.PregnancyCorruption);

                if (corruption == 0)
                    return 2;

                if (corruption < 0)
                    return 3;
            }

            return 1;
        }

        public static void PhoukaWhiskeyAddStatus()
        {
            var libidoChange = Player.Libido + 25 > 100 ? (int)(100 - Player.Libido) : 25;

            var sensChange = Player.Sensitivity < 10 ? (int)Player.Sensitivity : 10;

            var speedChange = Player.Speed < 20 ? (int)Player.Speed : 20;

            var intChange = Player.Intelligence < 20 ? (int)Player.Intelligence : 20;

            if (Player.HasStatusEffect(StatusEffectIds.PhoukaWhiskeyAffect))
            {
                var drinksSoFar = (int)Player.StatusEffectValue(StatusEffectIds.PhoukaWhiskeyAffect, 1);

                if (drinksSoFar < 4)
                    Player.ChangeStatusValue(StatusEffectIds.PhoukaWhiskeyAffect, 0, 8 - (2 * drinksSoFar));
                else
                    Player.ChangeStatusValue(StatusEffectIds.PhoukaWhiskeyAffect, 0, 1);

                Player.ChangeStatusValue(StatusEffectIds.PhoukaWhiskeyAffect, 1, 1);

                Player.ChangeStatusValue(StatusEffectIds.PhoukaWhiskeyAffect, 2, (256 * libidoChange) + sensChange);

                Player.ChangeStatusValue(StatusEffectIds.PhoukaWhiskeyAffect, 3, (256 * speedChange) + intChange);

                OutputText("Scenes.Phoukascene.PhoukaDrinkAccept.OhItTastesSoGood".Localized());

                Player.ApplyStats(
                    new DynStat(GameStats.Libido, libidoChange),

                    new DynStat(GameStats.Sensitivity, -sensChange),

                    new DynStat(GameStats.Speed, -speedChange),

                    new DynStat(GameStats.Intelligence, -intChange));
            }
            else
            {
                Player.CreateStatusEffect(
                    StatusEffectIds.PhoukaWhiskeyAffect,

                    8,

                    1,

                    (256 * libidoChange) + sensChange,

                    (256 * speedChange) + intChange);

                Player.ApplyStats(
                    new DynStat(GameStats.Libido, libidoChange),

                    new DynStat(GameStats.Sensitivity, -sensChange),

                    new DynStat(GameStats.Speed, -speedChange),

                    new DynStat(GameStats.Intelligence, -intChange));
            }

            StatScreenRefresh();
        }

        // 原版 PhoukaScene.hx:24 timeChange()：每小时扣一小时酒劲，归零时结算恢复属性
        public bool TimeChange()
        {
            if (Player.StatusEffectValue(StatusEffectIds.PhoukaWhiskeyAffect, 0) > 0)
            {
                Player.ChangeStatusValue(StatusEffectIds.PhoukaWhiskeyAffect, 0, -1);

                if (Player.StatusEffectValue(StatusEffectIds.PhoukaWhiskeyAffect, 0) <= 0)
                {
                    PhoukaWhiskeyExpires();

                    return true;
                }
            }

            return false;
        }

        public bool TimeChangeLarge() => false;

        // 原版 PhoukaWhiskey.hx:138 phoukaWhiskeyExpires()
        public static void PhoukaWhiskeyExpires()
        {
            var status = StatusEffectIds.PhoukaWhiskeyAffect;

            var numDrunk = (int)Player.StatusEffectValue(status, 1);

            var libidoSens = (int)Player.StatusEffectValue(status, 2);

            var intSpeed = (int)Player.StatusEffectValue(status, 3);

            var sensChange = libidoSens & 255;

            var libidoChange = (libidoSens - sensChange) / 256;

            var intChange = intSpeed & 255;

            var speedChange = (intSpeed - intChange) / 256;

            Player.ApplyStats(new DynStatsOptions(Scale: false),

                new DynStat(GameStats.Libido, -libidoChange),

                new DynStat(GameStats.Sensitivity, sensChange),

                new DynStat(GameStats.Speed, speedChange),

                new DynStat(GameStats.Intelligence, intChange));

            Player.RemoveStatusEffect(status);

            if (numDrunk > 3)
                OutputText("Scenes.Phoukascene.PhoukaWhiskey.ExpiresVeryDrunk".Localized());
            else if (numDrunk > 1)
                OutputText("Scenes.Phoukascene.PhoukaWhiskey.ExpiresDrunk".Localized());
            else
                OutputText("Scenes.Phoukascene.PhoukaWhiskey.ExpiresSober".Localized());

            StatScreenRefresh();
        }
    }
}