using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Npcs;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Configuration;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<IsabellaScene>("coc:isabella")]
    public class IsabellaScene : Scene
    {

        public const int OFFSPRING_HUMAN_BOYS = 1;

        public const int OFFSPRING_HUMAN_GIRLS = 2;

        public const int OFFSPRING_HUMAN_HERMS = 3;

        public const int OFFSPRING_COWGIRLS = 4;

        public const int OFFSPRING_COWFUTAS = 5;

        public const int ISABELLA_PREGNANCY_LAST_STAGE = 240;

        public NpcDefinition Definition => NpcRegistry.Get(NpcIds.Isabella)!;

        public NpcState State => Player.GetNpcState(Definition);

        private PregnancyStore? _pregnancy;

        public PregnancyStore Pregnancy => _pregnancy ??= NpcPregnancy.StoreFor(Definition, State);

        public static double Floor(double value) => Math.Floor(value);

        public static bool IsabellaAccent() => CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) > 0 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) >= 100;

        public static bool isabellaFollower() => CoC.Instance.Flags.Get(GameFlagIds.IsabellaFollowerAccepted) > 0;

        public static readonly List<int> isabellaOffspringData = [];

        public override void Entry()
        {

        }

        public float TinyPenis()
        {

            float littleBabyPenis = 6;

            if (Player.IsChild())
                littleBabyPenis -= 1;

            if ((OptionStore.Instance.Get("lowStandards", 0) > 0))
                littleBabyPenis += 3;

            return littleBabyPenis;
        }

        public int TotalIsabellaChildren()
        {

            return isabellaOffspringData.Count / 2;
        }

        public int GetIsabellaChildType(int type)
        {

            var count = 0;

            var i = 0;

            while (i < isabellaOffspringData.Count)
            {
                if (isabellaOffspringData[i] > 3)
                    isabellaOffspringData[i] -= 2;

                if (isabellaOffspringData[i] == type)
                    count += 1;

                i += 2;
            }

            return count;
        }

        public string GetIsabellaChildGender(int i = 0, string boy = "boy", string girl = "girl", string futa = "na")
        {

            switch (isabellaOffspringData[i * 2])
            {
                case 1:
                {
                return boy;
                }

                case 2:
                {
                return girl;
                }

                case 3:
                {
                return futa == "na" ? girl : futa;
                }

                case 4:
                {
                return girl;
                }

                case 5:
                {
                return futa == "na" ? girl : futa;
                }
            }

            if (true )
                return "[pg]Error! Invalid child or gender: i = " + i + ", switch = " + isabellaOffspringData[i * 2] + "[pg]";
        }

        public int GetIsabellaChildAge(int i = 0)
        {

            return View.Time.Days - isabellaOffspringData[i * 2 + 1];
        }

        public int ChooseRandomIsabellaChild(int[] types)
        {

            var matchingChildList = new List<int>();

            var i = 0;

            while (i < isabellaOffspringData.Count)
            {
                if (types.IndexOf(isabellaOffspringData[i]) >= 0)
                    matchingChildList.Add((int)Floor(i / 2));

                i += 2;
            }

            return matchingChildList[Random.Shared.Next(matchingChildList.Count)];
        }

        public void IsabellaGreeting()
        {
            ClearOutput();

            Action? suck = null;

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaCampApproached) == 0)
            {
                OutputText("Scenes.Isabellascene.IsabellaGreeting.HighGrassWarblingVoice".Localized());

                ClearButtons();

                AddButton(0, "Combat.Button.Approach".Localized(), IsabellaGreetingFirstTime);

                SetExitButton("Common.Button.Back".Localized());

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaAngryAtPcCounter) > 0)
            {
                OutputText("Scenes.Isabellascene.IsabellaGreeting.UnintendedCampArrival".Localized());

                var isabellaEnemy = EnemyRegistry.Create(new Identifier("coc", "isabella"));

            isabellaEnemy.OnDefeated = _ => DefeatIsabella();

            isabellaEnemy.OnWon = _ => VictoryLactation69();

            CombatScene.Start(isabellaEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()), canFlee: true);

                if (!isabellaFollower())
                    SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(-4);

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaMet) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.IsabellaMet, CoC.Instance.Flags.Get(GameFlagIds.IsabellaMet) + 1);

                OutputText("Scenes.Isabellascene.IsabellaGreeting.StumbleIntoClearing".Localized());

                OutputText("Scenes.Isabellascene.IsabellaGreeting.EvenSeatedOccupantImposing".Localized());

                OutputText("Scenes.Isabellascene.IsabellaGreeting.CowgirlNotCompletely".Localized());

                if (Player.HasStatusEffect(StatusEffectIds.Marble))
                    OutputText("Scenes.Isabellascene.IsabellaGreeting.LikeMarbleHorned".Localized());
                else
                    OutputText("Scenes.Isabellascene.IsabellaGreeting.StubbyHornsFloppyEars".Localized());

                OutputText("Scenes.Isabellascene.IsabellaGreeting.TannedMilkChocolateSkin".Localized((noFur ? "just above the knees " : "")));

                if (Player.Tallness > 78)
                {
                    OutputText("Scenes.Isabellascene.IsabellaGreeting.NarrowsEyebrowsSizesForm".Localized());

                    OutputText("Scenes.Isabellascene.IsabellaGreeting.ExplainInterruptedGetOut".Localized());

                    OutputText("Scenes.Isabellascene.IsabellaGreeting.TakeAbackAtReaction".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.TryToTalk".Localized(), TryToTalkDownAngryCow);

                    AddButton(1, "Common.Button.Fight".Localized(), UnwelcomeFightCowGal);

                    AddButton(14, "Common.Button.Leave".Localized(), LeaveAngryIzzy);
                }
                else
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaMetShortPc) < 0)
                        CoC.Instance.Flags.Set(GameFlagIds.IsabellaMetShortPc, CoC.Instance.Flags.Get(GameFlagIds.IsabellaMetShortPc) + 1);

                    OutputText("Scenes.Isabellascene.IsabellaGreeting.BrownEyesSoften".Localized());

                    OutputText("Scenes.Isabellascene.IsabellaGreeting.ExchangeIntroductions".Localized());

                    if (Player.HasCock())
                    {
                        OutputText("Scenes.Isabellascene.IsabellaGreeting.SniffsAirGlancesGroin".Localized());

                        if (Player.ShortestCockLength() <= TinyPenis())
                        {
                            OutputText("Scenes.Isabellascene.IsabellaGreeting.EyesFogWithLust".Localized());

                            suck = IzzyGivesSmallWangsFreeOral;
                        }
                        else
                            OutputText("Scenes.Isabellascene.IsabellaGreeting.EyesCloseDisappointed".Localized());
                    }

                    ClearButtons();

                    AddButton(0, "Common.Button.Talk".Localized(), TalkWithIsabella);

                    AddButton(1, "Scenes.Lumberjack.Button.Drink".Localized(), NomOnMommaIzzysTits);

                    AddButton(2, "Common.Button.GetLicked".Localized(), suck);

                    AddButton(3, "Common.Button.Fight".Localized(), FightIsabella);

                    SetExitButton("Common.Button.Back".Localized());
                }

                return ;
            }
            else
                if (Player.Tallness > 78 && CoC.Instance.Flags.Get(GameFlagIds.IsabellaOkayWithTallFolks) == 0)
                {
                    OutputText("Scenes.Isabellascene.IsabellaGreeting.StumbleFamiliarCamp".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.TryToTalk".Localized(), TryToTalkDownAngryCow);

                    AddButton(1, "Common.Button.Fight".Localized(), FightIsabella);

                    AddButton(14, "Common.Button.Leave".Localized(), LeaveAngryIzzy);

                    return ;
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaOkayWithTallFolks) > 0 && CoC.Instance.Flags.Get(GameFlagIds.IsabellaMetShortPc) == 0 && Player.Tallness <= 78)
                    {
                        CoC.Instance.Flags.Set(GameFlagIds.IsabellaMetShortPc, CoC.Instance.Flags.Get(GameFlagIds.IsabellaMetShortPc) + 1);

                        OutputText("Scenes.Isabellascene.IsabellaGreeting.StumbleIntoCampAgain".Localized());

                        if (Player.HasCock())
                        {
                            OutputText("Scenes.Isabellascene.IsabellaGreeting.NoseTwitchesAnd".Localized());

                            if (Player.ShortestCockLength() <= TinyPenis())
                            {
                                OutputText("Scenes.Isabellascene.IsabellaGreeting.GlanceAtSmallBulge".Localized());

                                suck = IzzyGivesSmallWangsFreeOral;
                            }
                            else
                            {
                                OutputText("Scenes.Isabellascene.IsabellaGreeting.GlancesDownAtYour".Localized());

                                if (Player.Cocks[Player.ShortestCockIndex()].Length < 20)
                                    OutputText("Scenes.Isabellascene.IsabellaGreeting.LargeWord".Localized());
                                else
                                    OutputText("Scenes.Isabellascene.IsabellaGreeting.GiganticWord".Localized());

                                OutputText("Scenes.Isabellascene.IsabellaGreeting.SighsAboutSize".Localized());
                            }

                            OutputText("Scenes.Isabellascene.IsabellaGreeting.ParagraphBreak".Localized());
                        }

                        OutputText("Scenes.Isabellascene.IsabellaGreeting.CheeksPinkApology".Localized());

                        OutputText("Scenes.Isabellascene.IsabellaGreeting.WhatToDoToday".Localized());
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaMetShortPc) > 0 && CoC.Instance.Flags.Get(GameFlagIds.IsabellaOkayWithTallFolks) == 0 && Player.Tallness > 78)
                        {
                            OutputText("Scenes.Isabellascene.IsabellaGreeting.StrideIntoHerCamp".Localized());

                            Player.ApplyStats(new DynStat(GameStats.Lust, 10 + Random.Shared.Next(10)));

                            OutputText("Scenes.Isabellascene.IsabellaGreeting.ComplainsCheckingMeOut".Localized());

                            CoC.Instance.Flags.Set(GameFlagIds.IsabellaOkayWithTallFolks, CoC.Instance.Flags.Get(GameFlagIds.IsabellaOkayWithTallFolks) + 1);

                            if (Player.HasCock())
                            {
                                OutputText("Scenes.Isabellascene.IsabellaGreeting.SniffsGivesCrotchGlance".Localized());

                                if (Player.ShortestCockLength() > TinyPenis())
                                    OutputText("Scenes.Isabellascene.IsabellaGreeting.BeforeSighingWistfully".Localized());
                                else
                                {
                                    OutputText("Scenes.Isabellascene.IsabellaGreeting.OffersUndressing".Localized());

                                    suck = IzzyGivesSmallWangsFreeOral;
                                }
                            }
                        }
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaCampDisabled) == 0 && CoC.Instance.Flags.Get(GameFlagIds.IsabellaFollowerAccepted) == 0 && 0 >= (50 + CoC.Instance.Flags.Get(GameFlagIds.IsabellaTimesOfferedFollower) * 15))
                            {
                                SceneInstance<IsabellaFollowerScene>.Get().IsabellaMoovesInGreeting();

                                return ;
                            }
                            else
                            {
                                if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaMetShortPc) < 0)
                                    CoC.Instance.Flags.Set(GameFlagIds.IsabellaMetShortPc, CoC.Instance.Flags.Get(GameFlagIds.IsabellaMetShortPc) + 1);

                                OutputText("Scenes.Isabellascene.IsabellaGreeting.FamiliarVoiceLilting".Localized());

                                OutputText("Scenes.Isabellascene.IsabellaGreeting.WatchHerCurvyBody".Localized());

                                OutputText("Scenes.Isabellascene.IsabellaGreeting.FinishesSongSmiles".Localized());

                                if (Player.HasCock())
                                {
                                    OutputText("Scenes.Isabellascene.IsabellaGreeting.SniffAtYourLegs".Localized());

                                    if (Player.ShortestCockLength() > TinyPenis())
                                        OutputText("Scenes.Isabellascene.IsabellaGreeting.SighingWistfully".Localized());
                                    else
                                    {
                                        OutputText("Scenes.Isabellascene.IsabellaGreeting.LicksLipsRightSize".Localized());

                                        suck = IzzyGivesSmallWangsFreeOral;
                                    }
                                }
                            }

            ClearButtons();

            AddButton(0, "Common.Button.Talk".Localized(), TalkWithIsabella);

            AddButton(1, "Scenes.Lumberjack.Button.Drink".Localized(), NomOnMommaIzzysTits);

            AddButton(2, "Common.Button.GetLicked".Localized(), suck);

            AddButton(3, "Common.Button.FightRape".Localized(), FightIsabella);

            AddButton(4, "Common.Button.OfferOral".Localized(), VolunteerToSlurpCowCunt);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void Menu()
        {

            OutputText("Scenes.Isabellascene.Menu.YouUnintentionallyWindUp".Localized());

            var isabellaEnemy = EnemyRegistry.Create(new Identifier("coc", "isabella"));

            isabellaEnemy.OnDefeated = _ => DefeatIsabella();

            isabellaEnemy.OnWon = _ => VictoryLactation69();

            CombatScene.Start(isabellaEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()), canFlee: true);

            if (!isabellaFollower())
                SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(-4);

            return ;
        }

        public void IsabellaGreetingFirstTime()
        {

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaCampApproached, 1);

            IsabellaGreeting();
        }

        public void LeaveAngryIzzy()
        {
            ClearOutput();

            OutputText("Scenes.Isabellascene.LeaveAngryIzzy.LeavingCrazyCow".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void UnwelcomeFightCowGal()
        {
            ClearOutput();

            OutputText("Scenes.Isabellascene.UnwelcomeFightCowGal.ReadyWeaponFightingPose".Localized());

            if (!isabellaFollower())
                SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(-5);

            var isabellaEnemy = EnemyRegistry.Create(new Identifier("coc", "isabella"));

            isabellaEnemy.OnDefeated = _ => DefeatIsabella();

            isabellaEnemy.OnWon = _ => VictoryLactation69();

            CombatScene.Start(isabellaEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()), canFlee: true);

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaAngryAtPcCounter, CoC.Instance.Flags.Get(GameFlagIds.IsabellaAngryAtPcCounter) + 72);
        }

        public void FightIsabella()
        {
            ClearOutput();

            if (!isabellaFollower())
                SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(-5);

            OutputText("Scenes.Isabellascene.FightIsabella.ReadyWeaponSmirk".Localized());

            var isabellaEnemy = EnemyRegistry.Create(new Identifier("coc", "isabella"));

            isabellaEnemy.OnDefeated = _ => DefeatIsabella();

            isabellaEnemy.OnWon = _ => VictoryLactation69();

            CombatScene.Start(isabellaEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()), canFlee: true);

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaAngryAtPcCounter, CoC.Instance.Flags.Get(GameFlagIds.IsabellaAngryAtPcCounter) + 72);
        }

        public void TryToTalkDownAngryCow()
        {
            ClearOutput();

            if (Player.Intelligence < 25)
            {
                OutputText("Scenes.Isabellascene.TryToTalkDownAngryCow.WontBeLeaving".Localized());

                OutputText("Scenes.Isabellascene.TryToTalkDownAngryCow.ReplyBlottedThunderingHooves".Localized());

                var isabellaEnemy = EnemyRegistry.Create(new Identifier("coc", "isabella"));

            isabellaEnemy.OnDefeated = _ => DefeatIsabella();

            isabellaEnemy.OnWon = _ => VictoryLactation69();

            CombatScene.Start(isabellaEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()), true, true);

                if (!isabellaFollower())
                    SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(-2);

            }
            else
                if (Player.Intelligence < 50)
                {
                    OutputText("Scenes.Isabellascene.TryToTalkDownAngryCow.StartExplainReasonsStuttering".Localized());

                    if (!isabellaFollower())
                        SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(-2);

                    var isabellaEnemy = EnemyRegistry.Create(new Identifier("coc", "isabella"));

            isabellaEnemy.OnDefeated = _ => DefeatIsabella();

            isabellaEnemy.OnWon = _ => VictoryLactation69();

            CombatScene.Start(isabellaEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()), canFlee: true);
                }
                else
                    if (Player.Intelligence < 75)
                    {
                        OutputText("Scenes.Isabellascene.TryToTalkDownAngryCow.WellExplainedNoEffect".Localized());

                        if (!isabellaFollower())
                            SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(-2);

                        var isabellaEnemy = EnemyRegistry.Create(new Identifier("coc", "isabella"));

            isabellaEnemy.OnDefeated = _ => DefeatIsabella();

            isabellaEnemy.OnWon = _ => VictoryLactation69();

            CombatScene.Start(isabellaEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()), canFlee: true);
                    }
                    else
                    {
                        if (!Player.Weapon.IsAttached())
                            OutputText("Scenes.Isabellascene.TryToTalkDownAngryCow.TossAsideWeaponAnd".Localized());
                        else
                            OutputText("Scenes.Isabellascene.TryToTalkDownAngryCow.YouWord".Localized());

                        OutputText("Scenes.Isabellascene.TryToTalkDownAngryCow.HandsUpPeaceGesture".Localized());

                        OutputText("Scenes.Isabellascene.TryToTalkDownAngryCow.SitDownTellTale".Localized());

                        OutputText("Scenes.Isabellascene.TryToTalkDownAngryCow.MoistureGlimmeringBrownEyes".Localized());

                        OutputText("Scenes.Isabellascene.TryToTalkDownAngryCow.SmileGladMadeFriend".Localized());

                        DoNext(() => ReturnToCamp(1));

                        if (!isabellaFollower())
                            SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(10);

                        CoC.Instance.Flags.Set(GameFlagIds.IsabellaOkayWithTallFolks, CoC.Instance.Flags.Get(GameFlagIds.IsabellaOkayWithTallFolks) + 1);
                    }
        }

        public void NomOnMommaIzzysTits()
        {
            ClearOutput();

            int x = Player.CockThatFits(20);

            if (Player.Tallness > 78 || Player.IsTaur())
            {
                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.FaceLightsUp".Localized());

                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.ApproachSalivatingTasteMilk".Localized());

                if (Player.HasCock())
                {
                    OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.YourArmorTents".Localized());

                    if (Player.BiggestCockArea() > 40)
                        OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.ConstrainingEachCock".Localized());
                    else
                        OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.VisiblyFromEachCock".Localized());
                }

                if (Player.HasVagina())
                {
                    if (Player.HasCock())
                        OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.EvenYFragment".Localized());
                    else
                        OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.YFragment".Localized());

                    OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.OurPlaceholderFragment".Localized(
                        Player.VaginaDescript(0)
                    ));

                    if (Player.Wetness() < 3)
                        OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.GlistensWith".Localized());
                    else
                        if (Player.Wetness() < 5)
                            OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.DripsWord".Localized());
                        else
                            OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.StreamsWord".Localized());

                    OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.MoistureReactingEroticVision".Localized());
                }

                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.WeightyChestHeaves".Localized());

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.PullsTightlyStretchedAccent".Localized());
                else
                    OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.PullsTightlyStretchedPlain".Localized());

                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.AreolaeLargeTwoInches".Localized());

                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.OtherHandGrabbing".Localized());

                if (Player.Horns.Id.Path != "none")
                    OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.YourHorns".Localized());
                else
                    OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.BackOfYourHead".Localized());

                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.FaceInMilkSpouts".Localized());

                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.AbsorbedLoseTime".Localized());

                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.YankedToHerFace".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.TwitchesDripsIntensityKiss".Localized(
                        Player.SMultiCockDesc()
                    ));
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.AchesNeedDesireIntensity".Localized(
                            Player.VaginaDescript(0)
                        ));

                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.KissDoesntLast".Localized());

                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.MilkOnUnusedTit".Localized());

                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.ScreamBetweenSwallows".Localized());

                if (Player.Body.Get<WingsPart>().Id != BodyPartTypes.None)
                    OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.AndStrokeYourWings".Localized());

                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.LullingPeacefulRelaxation".Localized());

                if (Player.HasCock() && x >= 0)
                {
                    OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.WakeAfterEroticDreams".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaSleepRapeCounter) == 0)
                        OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.SleepFuckedQuestion".Localized());
                    else
                        OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.SleepFuckedAgain".Localized());

                    Player.Orgasm("Dick");

                    Player.ChangeFatigue(-50);

                    CoC.Instance.Flags.Set(GameFlagIds.IsabellaSleepRapeCounter, CoC.Instance.Flags.Get(GameFlagIds.IsabellaSleepRapeCounter) + 1);
                }
                else
                {
                    OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.SnuggledInBlankets".Localized());

                    if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                        OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.StandUpStretchAccent".Localized());
                    else
                        OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.StandUpStretchPlain".Localized());

                    Player.ChangeFatigue(-65);
                }
            }
            else
            {
                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.QuiteThirsty".Localized());

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.InLuckAccent".Localized());
                else
                    OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.InLuckPlain".Localized());

                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.UnlacingCorset".Localized());

                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.LongPregnantPause".Localized((noFur ? "" : " the short fur that sprouts from")));

                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.ShiftPositionCradle".Localized());

                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.ThirstAndAura".Localized());

                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.NuzzleAndDrink".Localized());

                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.NoisesNoEffect".Localized());

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                {
                    OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.PantsPullBackAccent".Localized());

                    if (!isabellaFollower())
                        OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.VayBackAccent".Localized());
                    else
                        OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.AusUndUberAccent".Localized());
                }
                else
                {
                    OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.PantsPullBackPlain".Localized());

                    if (!isabellaFollower())
                        OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.WayBackPlain".Localized());
                    else
                        OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.OutAndAboutPlain".Localized());
                }

                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.OooohGroansPressesMouth".Localized());

                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.DuskyFleshGrinding".Localized());

                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.SometimeLaterBurpSnuggle".Localized());

                if (Player.HasCock() && x >= 0)
                {
                    OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.WakeInBlanketSprawl".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaSleepRapeCounter) == 0)
                    {
                        OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.SleepRapeQuestion".Localized());

                        if (Player.Balls > 0)
                            OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.BallsFeel".Localized());
                        else
                            OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.BodyFeels".Localized());

                        OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.EmptyAndSated".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.SleepFuckedAgainExclaim".Localized());

                        if (View.Time.Hours < 12)
                            OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.DayWord".Localized());
                        else
                            if (View.Time.Hours < 4)
                                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.AfternoonWord".Localized());
                            else
                                OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.EveningWord".Localized());

                        OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.SentencePeriod".Localized());
                    }

                    Player.Orgasm("Dick");

                    Player.ChangeFatigue(-50);

                    CoC.Instance.Flags.Set(GameFlagIds.IsabellaSleepRapeCounter, CoC.Instance.Flags.Get(GameFlagIds.IsabellaSleepRapeCounter) + 1);
                }
                else
                {
                    if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                        OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.BlanketPileAccent".Localized());
                    else
                        OutputText("Scenes.Isabellascene.NomOnMommaIzzysTits.BlanketPilePlain".Localized());

                    Player.ChangeFatigue(-65);
                }
            }

            if (!isabellaFollower())
                SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(4);
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaMilkedYet) > 0)
                    CoC.Instance.Flags.Set(GameFlagIds.IsabellaMilkedYet, 0);

            Player.SlimeFeed();

            Player.RefillHunger(40);

            if (Random.Shared.Next(2) == 0)
                OutputText(Player.ModThickness(75, 4));

            if (Random.Shared.Next(2) == 0 && Player.HasVagina())
            {

            }

            DoNext(() => ReturnToCamp(1));
        }

        public void IzzyGivesSmallWangsFreeOral()
        {
            ClearOutput();

            var x = Player.SmallestCockIndex();

            OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.YouSpace".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.BlushAndOfferLick".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.BlushOfferLick".Localized());
                else
                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.TraceBulgeLewdly".Localized(
                        Player.CockDescript(x)
                    ));

            OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.BlushesHardRoseCheeks".Localized());

            OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.UndoingArmor".Localized(
                Player.AssDescript(),

                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.RestOfPackage".Localized());

                if (Player.BiggestCockArea() > 50)
                {
                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.ShovesLargerLessDesirable".Localized());

                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.SuffixS".Localized());

                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.ToTheSideAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.PushesTheExtra".Localized());

                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.SuffixSAgain".Localized());

                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.ToTheSideAgain".Localized());
                }
            }
            else
                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.TremblingWeaklyStrongFingers".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.SquealsCuteAccent".Localized(
                    Player.CockDescript(x)
                ));
            else
                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.SquealsCutePlain".Localized(
                    Player.CockDescript(x)
                ));

            OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.PullsDownNeckline".Localized(
                Player.CockDescript(x),

                Player.CockHead(x),

                Player.CockHead(x)
            ));

            OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.CaressesUnderbelly".Localized(
                Player.CockDescript(x)
            ));

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.IzabellaKnowsAccent".Localized());
            else
                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.IzabellaKnowsPlain".Localized());

            OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.EmphasizeWellEndowedFlexible".Localized(
                Player.CockHead(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.FeelsDamnedGoodGroan".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.BlushingAshamedWantonBehavior".Localized());
            else
                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.SighingBlissfullyAbsorbedFeel".Localized());

            OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.StrengthEbbsCorkscrewTongue".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.HybridBetweenVaginaBlowjob".Localized());

            if (Player.Sensitivity < 50)
            {
                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.MoanHappilyHipsRocking".Localized(
                    Player.CockDescript(x),

                    Player.CockHead(x)
                ));

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.PullsBackHornsAccent".Localized(
                        Player.Mf("boy", "maid")
                    ));
                else
                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.PullsBackHornsPlain".Localized(
                        Player.Mf("boy", "maid")
                    ));

                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.ReturnsGustoSnaring".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.CaressesTwitchingOrbs".Localized());
                else
                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.ProbesVaginaEngorgedLips".Localized());

                        if (Player.GetClitLength() < 3)
                            OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.GentleSqueezes".Localized());
                        else
                            OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.FirmPumps".Localized());

                        OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.SentencePeriod".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.RunsFingerFromThe".Localized());

                        if (Player.Cocks.Exists(c => c.SheathOn))
                            OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.SheathWord".Localized());
                        else
                            OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.BaseWord".Localized());

                        OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.OfPlaceholderTeasingAsshole".Localized(
                            Player.CockDescript(x)
                        ));
                    }

                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.LatchOntoHorns".Localized());

                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.DoesntProtestForcePositions".Localized());

                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.ContinuesUnabated".Localized());

                if (Player.Cocks.Exists(c => c.SheathOn))
                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.SheathWordAgain".Localized());
                else
                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.BodyWord".Localized());

                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.InevitableRelease".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.PullsTongueTightSqueezing".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.FatWord".Localized());

                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.CumSqueezeUrethra".Localized());

                if (Player.CumQ() < 100)
                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.SpurtsWord".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.TorrentsWord".Localized());
                    else
                        OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.EruptionsWord".Localized());

                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.JismSplatterSpasms".Localized());

                if (Player.CumQ() < 250)
                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.EmptyLoadOnTongue".Localized());
                else
                    if (Player.CumQ() < 500)
                        OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.EmptyLoadInBelly".Localized());
                    else
                        if (Player.CumQ() < 1500)
                            OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.HugeBatchesGurgling".Localized());
                        else
                            OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.FillBellyAndThroat".Localized());

                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.ParagraphBreak".Localized());

                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.LicksLipsLegsSoaked".Localized());

                if (Player.IsGoo())
                    Player.SlimeFeed();

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                {
                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.ContentedMooAccent".Localized());

                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.HelpsIntoArmorAccent".Localized());
                }
                else
                {
                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.ContentedMooPlain".Localized());

                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.HelpsIntoArmorPlain".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.FightTheHeaven".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.Cocks.Exists(c => c.SheathOn))
                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.AroundYourSheath".Localized());
                else
                    if (Player.Balls > 0)
                        OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.AboveYourBalls".Localized());
                    else
                        OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.AroundYourBase".Localized());

                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.KeepsPositionRockingMovements".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.HandStrokingBallsMilking".Localized());
                else
                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.NeglectedAfterthought".Localized(
                            Player.CockDescript(1)
                        ));
                    else
                        OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.StrokeToMilkLoad".Localized());

                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.ParagraphBreakAgain".Localized());

                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.TongueTightAgain".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.FatWordAgain".Localized());

                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.CumSqueezeUrethraAgain".Localized());

                if (Player.CumQ() < 100)
                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.SpurtsWordAgain".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.TorrentsWordAgain".Localized());
                    else
                        OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.EruptionsWordAgain".Localized());

                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.JismSplatterSpasmsAgain".Localized());

                if (Player.CumQ() < 250)
                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.EmptyLoadTongueAgain".Localized());
                else
                    if (Player.CumQ() < 500)
                        OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.EmptyLoadBellyAgain".Localized());
                    else
                        if (Player.CumQ() < 1500)
                            OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.HugeBatchesGurglingAgain".Localized());
                        else
                            OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.FillBellyThroatAgain".Localized());

                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.ParagraphBreakThird".Localized());

                OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.LicksLipsLegsAgain".Localized());

                if (Player.IsGoo())
                    Player.SlimeFeed();

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                {
                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.WagglesTongueAccent".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.LooksDownDisappointmentAccent".Localized(
                        Player.CockDescript(x),

                        Player.Mf("boy", "maid")
                    ));
                }
                else
                {
                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.WagglesTonguePlain".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Isabellascene.IzzyGivesSmallWangsFreeOral.LooksDownDisappointmentPlain".Localized(
                        Player.CockDescript(x),

                        Player.Mf("boy", "maid")
                    ));
                }
            }

            if (!isabellaFollower())
                SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(2);

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));
        }

        public void VolunteerToSlurpCowCunt()
        {
            ClearOutput();

            OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.InterestedInTastingHer".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.PreemptsLickZereAccent".Localized());
            else
                OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.PreemptsLickTherePlain".Localized());

            OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.PullsTrappingsClothingHigher".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.HesitantlyLeanCloserInches".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.LeanInchesAway".Localized());
                else
                    OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.DiveIntoSlickSnatch".Localized());

            OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.SwallowLoveButton".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
            {
                OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.GoodAtThisAccent".Localized());

                if (Player.Tallness < 60)
                    OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.LittleWord".Localized());
                else
                    if (Player.Tallness > 80)
                        OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.BigWord".Localized());

                OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.NoEncouragementNeededAccent".Localized());

                OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.LickMommaClittyAccent".Localized());
            }
            else
            {
                OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.GoodAtThisPlain".Localized());

                if (Player.Tallness < 60)
                    OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.LittleWordAgain".Localized());
                else
                    if (Player.Tallness > 80)
                        OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.BigWordAgain".Localized());

                OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.NoEncouragementNeededPlain".Localized());

                OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.LickMommaClittyPlain".Localized());
            }

            if (Player.Corruption < 33)
                OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.BlushHopeTalkingDirty".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.WonderIfNotDirty".Localized());
                else
                    OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.DespiteKnowingIntoMoment".Localized());

            OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.SplatterDownYourBack".Localized());

            OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.AccentedVoiceCriesOctaves".Localized());

            OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.LastPushSmearing".Localized());

            if (!(CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.VonderfulLickerAccent".Localized());
            else
                OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.WonderfulLickerPlain".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

            if (Player.HasCock())
            {
                OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.GlancesBackAtCrotch".Localized());

                if (Player.ShortestCockLength() > TinyPenis())
                    OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.BeforeSighingWistfully".Localized());
                else
                {
                    OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.OffersToUndress".Localized());

                    if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                        OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.ReturnZeFavorAccent".Localized());
                    else
                        OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.ReturnTheFavorPlain".Localized());

                    AddButton(0, "Common.Button.Yes".Localized(), IzzyGivesSmallWangsFreeOral);

                    AddButton(1, "Common.Button.No".Localized(), () => ReturnToCamp(1));

                    return ;
                }
            }

            SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(2);

            if (!isabellaFollower() || !Player.HasVagina() || Player.BiggestBreastSize() < 1)
                DoNext(() => ReturnToCamp(1));
            else
            {
                if (!(CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.SeeingArdentDesirePlain".Localized());
                else
                    OutputText("Scenes.Isabellascene.VolunteerToSlurpCowCunt.SeeingArdentDesireAccent".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.GetLicked".Localized(), () => { });

                SetExitButton("Common.Button.Back".Localized());
            }
        }

        public void IsabellaWinsAndSpanks()
        {
            ClearOutput();

            if (Player.Tallness <= 78)
            {
                if (Player.Health < 1)
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.CollapseSenselessDamage".Localized());
                else
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.CollapseMasturbating".Localized());

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.HandLiftsOverKnee".Localized((noFur ? "just above her knee" : "at the midpoint of her thigh")));

                if (Player.Lust >= Player.MaxLust)
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.TryToHumpRefused".Localized());

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.SmackImpactPlaceholderPain".Localized(
                    Player.AssDescript()
                ));

                if (Player.HasPerk(PerkIds.Masochist))
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.AndPleasureAgain".Localized());

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.NextBlowOtherCheek".Localized());

                if (Player.HasPerk(PerkIds.Masochist))
                {
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.GaspAnd".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.SquirtPreCum".Localized());
                    else
                        if (Player.HasVagina())
                            OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.SlickYourThighs".Localized());
                        else
                            OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.Tremble".Localized());

                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.WithMasochisticPleasure".Localized());
                }

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.ParagraphBreak".Localized());

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.ActingLikeDemonsAccent".Localized());
                else
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.ActingLikeDemonsPlain".Localized());

                if (Player.HasPerk(PerkIds.Masochist))
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.HowManyMoreSmacks".Localized());
                else
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.PiteouslyAgain".Localized());

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.CocksElbowAnotherSpank".Localized());

                if (Player.HasPerk(PerkIds.Masochist))
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.SwoonMoanWigglingRump".Localized());

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.PicksUpPaceScolding".Localized());

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.SmackCrackSlapLesson".Localized());

                if (!Player.HasPerk(PerkIds.Masochist))
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.DrawsScreamPastConsciousness".Localized());
                else
                {
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.PushesPastYourLimit".Localized());

                    if (Player.HasCock())
                    {
                        OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.CumLikeCannonKnee".Localized());

                        if (Player.CumQ() >= 50)
                            OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.MoisteningTheFur".Localized());

                        if (Player.CumQ() >= 250)
                            OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.DrippingDownToHooves".Localized());

                        if (Player.CumQ() >= 1000)
                            OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.CreatedPuddleUnderneath".Localized());

                        OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.SentencePeriod".Localized());
                    }

                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.YourVaginaQuiversAnd".Localized());

                        if (Player.Wetness() < 5)
                            OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.DripsAgain".Localized());
                        else
                            OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.GirlCumOnHoof".Localized());

                        OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.SentencePeriodAgain".Localized());
                    }

                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.FeelingClimaxingFromPain".Localized());
                }

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.ParagraphBreakAgain".Localized());

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.SomeTimeLaterBold".Localized());

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.CrackEyesNoisySwallowing".Localized(
                    Player.AssDescript()
                ));

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.ForgetEarlierNightAccent".Localized());
                else
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.ForgetEarlierNightPlain".Localized());

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.InSpiteOfJudgment".Localized());

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.SleepBruisedBelly".Localized());

                Player.RefillHunger(30);

                Player.ChangeFatigue(-40);

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 4f));

                if (Player.HasPerk(PerkIds.Masochist))
                    Player.Orgasm("Generic");
            }
            else
            {
                if (Player.Health < 1)
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.CollapseSenselessAgain".Localized());
                else
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.CollapseMasturbatingAgain".Localized());

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.PropsAssInAir".Localized());

                if (Player.Lust >= Player.MaxLust)
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.MasturbateRefused".Localized());

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.SmackPainAgain".Localized(
                    Player.AssDescript()
                ));

                if (Player.HasPerk(PerkIds.Masochist))
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.AndPleasureAgain".Localized());

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.OtherCheekBlowAgain".Localized());

                if (Player.HasPerk(PerkIds.Masochist))
                {
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.GaspAndAgain".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.SquirtPreCumAgain".Localized());
                    else
                        if (Player.HasVagina())
                            OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.SlickYourThighsAgain".Localized());
                        else
                            OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.TrembleAgain".Localized());

                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.WithMasochisticPleasureAgain".Localized());
                }

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.ParagraphBreakThird".Localized());

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.DemonsAccentAgain".Localized());
                else
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.DemonsPlainAgain".Localized());

                if (!Player.HasPerk(PerkIds.Masochist))
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.PiteouslyAgain".Localized());
                else
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.MoreSmacksAgain".Localized());

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.AnotherSpankAgain".Localized());

                if (Player.HasPerk(PerkIds.Masochist))
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.WiggleRumpAgain".Localized());

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.PicksUpPaceAgain".Localized());

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.SmackLessonAgain".Localized());

                if (Player.HasPerk(PerkIds.Masochist))
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.PastConsciousnessAgain".Localized());
                else
                {
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.PastLimitAgain".Localized());

                    if (Player.HasCock())
                    {
                        OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.CumLikeCannon".Localized());

                        if (Player.CumQ() > 100)
                            OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.MoisteningYourHair".Localized());

                        if (Player.CumQ() > 250)
                            OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.DrippingDownToGround".Localized());

                        if (Player.CumQ() > 1000)
                            OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.CreatedPuddleAroundYourself".Localized());

                        OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.SentencePeriodThird".Localized());
                    }
                    else
                        if (Player.HasVagina())
                        {
                            OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.VaginaQuiversAgain".Localized());

                            if (Player.Wetness() < 5)
                                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.DripsAgain".Localized());
                            else
                                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.GirlCumOnDirt".Localized());

                            OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.SentencePeriodFourth".Localized());
                        }

                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.ClimaxFromPainAgain".Localized());
                }

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.ParagraphBreakFourth".Localized());

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.SomeTimeLaterAgain".Localized());

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.NoisySwallowingAgain".Localized(
                    Player.AssDescript()
                ));

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.JustLieZereAccent".Localized());
                else
                    OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.JustLieTherePlain".Localized());

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.SpiteJudgmentAgain".Localized());

                OutputText("Scenes.Isabellascene.IsabellaWinsAndSpanks.SleepBruisedBellyAgain".Localized());

                Player.RefillHunger(30);

                Player.ChangeFatigue(-40);

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 4f));

                if (Player.HasPerk(PerkIds.Masochist))
                    Player.Orgasm("Generic");
            }

            if ((Player.HasCock()) && (Player.ShortestCockLength() <= TinyPenis()))
                {
                    DoNext(() => IsabellaPostSpankFeedSex());

                    return ;
                }

            CombatScene.CleanupAfterCombat();
        }

        public void IsabellaRapesYouWithHerAss()
        {
            ClearOutput();

            int x = Player.CockThatFits(38);

            if (x < 0)
                x = 0;

            if (Player.Health < 1)
                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.CollapseSenselessDamage".Localized());
            else
                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.CollapseMasturbating".Localized());

            OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.HoofOnChest".Localized());

            if (Player.Lust >= Player.MaxLust)
            {
                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.TwitchingHardnessSmileCoos".Localized());

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.TaughtLessonAccent".Localized());
                else
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.TaughtLessonPlain".Localized());
            }
            else
            {
                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.BronzedSkinCaresses".Localized());

                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.AlreadyWantLessonAccent".Localized());
                else
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.AlreadyWantLessonPlain".Localized());
            }

            OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.WhatsNextForYou".Localized(
                Player.CockHead(x)
            ));

            if (Player.CockTotal() > 1)
                if (Player.CockTotal() > 2)
                {
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.ReachesDownSwatPlaceholders".Localized(
                        Appearance.CockNoun(BodyPartTypes.Human)
                    ));

                    if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                        OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.VantManyPenisesAccent".Localized());
                    else
                        OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.WantManyPenisesPlain".Localized());
                }
                else
                {
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.ReachesDownStrokePlaceholder".Localized(
                        Player.CockDescript(x)
                    ));

                    if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                        OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.VhyEagerAccent".Localized());
                    else
                        OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.WhyEagerPlain".Localized());
                }

            OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.ParagraphBreak".Localized());

            OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.DropsLowerMashingPucker".Localized(
                Player.CockHead(x)
            ));

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.NeinBeingPunishedAccent".Localized());
            else
                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.NoBeingPunishedPlain".Localized());

            OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.LeakingOverHerAss".Localized());

            OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.AssSqueezingBouncing".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.NotFairCryOut".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.ApologizeFromHeartAccent".Localized());
            else
                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.ApologizeFromHeartPlain".Localized());

            OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.FocusLimitedPleasure".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.TighteningSackPullsBalls".Localized());

            OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.EasesUpDenying".Localized());

            OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.HardItHurtsTease".Localized());

            OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.ShakesHeadDisdain".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.SorryCantCumAccent".Localized());
            else
                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.SorryCantCumPlain".Localized());

            OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.StickOutLowerLip".Localized());

            if (Player.HasCockThatFits(38))
                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.LittleWord".Localized());
            else
                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.NastyWord".Localized());

            OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.HardSwollenPromise".Localized());

            OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.GrinsLikeCatCream".Localized());

            if (Player.HasCockThatFits(38))
            {
                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.SlidesPlumpAss".Localized(
                    Player.CockHead(x),

                    Player.CockDescript(x)
                ));

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.SmushingAgainstOtherDick".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.SuffixS".Localized());

                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.SentencePeriodParagraph".Localized());

                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.RocksUpAndDown".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.CantHoldBack".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.MusclesTugYourSack".Localized());
                else
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.MusclesClenchProstate".Localized());

                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.BlastFirstRopes".Localized());

                if (!Player.Cocks.Exists(c => c.SheathOn))
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.BaseWord".Localized());
                else
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.SheathWord".Localized());

                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.SqueezeNextDollop".Localized());

                if (Player.CumQ() >= 1000)
                {
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.GruntDiscomfort".Localized());

                    if (Player.CumQ() >= 1500)
                        OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.WontLetUpCries".Localized(
                            Player.CockDescript(x)
                        ));

                    if (Player.CumQ() >= 2500)
                        OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.DroolingSpunk".Localized());
                }

                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.SentencePeriod".Localized());

                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.ChestGlazedPinnedPrick".Localized());

                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.SuffixSAgain".Localized());

                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.TrappedUnderSweatyBody".Localized());
                }

                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.ParagraphBreakAgain".Localized());

                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.PullsOffGoopey".Localized());
            }
            else
            {
                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.SlidesAllWayDown".Localized());

                if (!Player.Cocks.Exists(c => c.SheathOn))
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.BaseWordAgain".Localized());
                else
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.SheathWordAgain".Localized());

                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.LowMoanFlexThighs".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.IgnoredCheeksCrush".Localized());
                else
                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.IgnoredCheeksCrushAgain".Localized());

                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.ParagraphBreakThird".Localized());

                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.CumBrutallyHard".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.SackPullsTight".Localized());
                else
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.BodyViolinString".Localized());

                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.PressureBuildsShooting".Localized());

                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.SemenOnChest".Localized());

                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.SuffixSThird".Localized());

                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.WeakerAlmostAfterthought".Localized());
                }

                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.KeepSquirtingEmpty".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.HandyWorkSmeared".Localized());

                if (Player.Balls == 0)
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.CrotchWord".Localized());
                else
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.BallsWord".Localized());

                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.PoolingAroundLegsGround".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.RopesDripCrimsonLocks".Localized());

                if (Player.CumQ() >= 2000)
                    OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.SpoogeSoaksBothVolume".Localized());

                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.ParagraphBreakFourth".Localized());

                OutputText("Scenes.Isabellascene.IsabellaRapesYouWithHerAss.WatchesTensionDrain".Localized());
            }

            if (!isabellaFollower())
                SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(4);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

            CombatScene.CleanupAfterCombat();
        }

        public void IsabellaPostSpankFeedSex()
        {
            ClearOutput();

            var x = Player.SmallestCockIndex();

            OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.SquishSquishSquishBold".Localized());

            OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.WaaaGroanCrackingEyes".Localized());

            OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.MilkSwollenMelons".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.TinyInsanePricks".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.BoyDontMoveAccent".Localized());
            else
                OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.BoyDontMovePlain".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.OnYourChest".Localized());
            else
                OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.InBetweenYourPlaceholder".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

            OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.PushingTorsoMudReeks".Localized());

            OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.TweakYourNipple".Localized());

            if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Human)
                OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.SmallerWord".Localized());
            else
                OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.LongerWord".Localized());

            OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.SlipperySmoothnessCowOrgan".Localized());

            OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.StruggleWithPleasure".Localized(
                Player.CockDescript(x)
            ));

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.MoooJaaaaAccent".Localized());
            else
                OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.MoooYessssPlain".Localized());

            OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.WhiteBurstSpouts".Localized());

            OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.PussyTightensClampingSmaller".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CumQ() < 50)
                OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.SpurtIntoConstricting".Localized());
            else
                if (Player.CumQ() < 250)
                    OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.SpurtingSuddenlyTightCunny".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.BurstIntoCunny".Localized());
                    else
                        if (Player.CumQ() < 2000)
                            OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.BurstIntoWomb".Localized());
                        else
                            OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.ExplodingConstrictingCuntWaves".Localized());

            if (Player.CumQ() >= 10000)
                OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.LeaksIntoMud".Localized());

            OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.ParagraphBreak".Localized());

            OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.RollsOffPanting".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.GoodBoyMannersAccent".Localized());
            else
                OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.GoodBoyMannersPlain".Localized());

            OutputText("Scenes.Isabellascene.IsabellaPostSpankFeedSex.ClimbsWoozyLegs".Localized());

            if (!isabellaFollower())
                SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(3);

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void IsabellaDefeats()
        {

            if (CombatController.Instance!.Context.Enemy.StatusEffectValue(StatusEffectIds.Spar, 0) <= 1)
            {
                if (Player.HasCock() && Random.Shared.Next(2) == 0)
                    IsabellaRapesYouWithHerAss();
                else
                    IsabellaWinsAndSpanks();
            }
            else
                CombatScene.CleanupAfterCombat();
        }

        public void DefeatIsabella()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.StatusEffectValue(StatusEffectIds.Spar, 0) != 2)
            {
                OutputText("Scenes.Isabellascene.DefeatIsabella.YouGiveThe".Localized());

                if (CombatController.Instance!.Context.Enemy.Health < 1)
                    OutputText("Scenes.Isabellascene.DefeatIsabella.DamageDazedAgain".Localized());
                else
                    OutputText("Scenes.Isabellascene.DefeatIsabella.ArousalAddledAgain".Localized());

                OutputText("Scenes.Isabellascene.DefeatIsabella.PushSlumpsDefeated".Localized());

                if (CombatController.Instance!.Context.Enemy.Health < 1)
                    OutputText("Scenes.Isabellascene.DefeatIsabella.Recuperate".Localized());
                else
                    OutputText("Scenes.Isabellascene.DefeatIsabella.TakeCareOfNeeds".Localized());

                CombatScene.CleanupAfterCombat();

                return ;
            }

            OutputText("Scenes.Isabellascene.DefeatIsabella.YouPushThe".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Isabellascene.DefeatIsabella.DamageDazedAgain".Localized());
            else
                OutputText("Scenes.Isabellascene.DefeatIsabella.ArousalAddledAgain".Localized());

            OutputText("Scenes.Isabellascene.DefeatIsabella.HandSinksInTits".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Isabellascene.DefeatIsabella.GrFragment".Localized());
            else
                OutputText("Scenes.Isabellascene.DefeatIsabella.MFragment".Localized());

            OutputText("Scenes.Isabellascene.DefeatIsabella.OansFragment".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
            {
                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellascene.DefeatIsabella.VhyLikeThemAccent".Localized());
                else
                    OutputText("Scenes.Isabellascene.DefeatIsabella.WhyLikeThemPlain".Localized());
            }
            else
                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                    OutputText("Scenes.Isabellascene.DefeatIsabella.DidntVantDrinkAccent".Localized());
                else
                    OutputText("Scenes.Isabellascene.DefeatIsabella.DidntWantDrinkPlain".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Isabellascene.DefeatIsabella.ActsHaughtyNipplesTenting".Localized());
            else
                OutputText("Scenes.Isabellascene.DefeatIsabella.NeedsMilkingDampness".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Lactation69".Localized(), null);

            AddButton(1, "Common.Button.Buttsex".Localized(), null);

            AddButton(2, "Common.Button.Sixtynine".Localized(), null);

            AddButton(3, "Common.Button.Vaginal".Localized(), null);

            AddButton(4, "Common.Button.BigTitfuck".Localized(), null);

            AddButton(5, "Common.Button.SmallTitfuck".Localized(), null);

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);

            if (Player.Lust < 33)
                return ;

            var hasCock = Player.HasCock();

            AddButton(0, "Common.Button.Lactation69".Localized(), VictoryLactation69);

            AddButton(1, "Common.Button.Buttsex".Localized(), PCVictoryOnIsabellaButtsex);

            AddButton(2, "Common.Button.Sixtynine".Localized(), VictoryAgainstIzzzzzySixtyNine);

            AddButton(3, "Common.Button.Vaginal".Localized(), VaginalProdNPokeIsabella);

            AddButton(4, "Common.Button.BigTitfuck".Localized(), TooBigVictoryTittyFuckingFuntimesWithMilk);

            AddButton(5, "Common.Button.SmallTitfuck".Localized(), TinyVictoryTittyFuckingFuntimesWithMilk);
        }

        public void VictoryLactation69()
        {
            ClearOutput();

            OutputText("Scenes.Isabellascene.VictoryLactation69.MentionedMilkPlaceholderFull".Localized(
                Appearance.AllBreastsDescript(Player)
            ));

            OutputText("Scenes.Isabellascene.VictoryLactation69.IsabellaMumbles".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.VictoryLactation69.MooMilkNiAccent".Localized());
            else
                OutputText("Scenes.Isabellascene.VictoryLactation69.MooMilkNiPlain".Localized());

            OutputText("Scenes.Isabellascene.VictoryLactation69.BovineTongueWhets".Localized());

            OutputText("Scenes.Isabellascene.VictoryLactation69.UnbiddenDropsMilkNipples".Localized());

            if (Player.BreastRows.Count > 1)
                OutputText("Scenes.Isabellascene.VictoryLactation69.TopRowOf".Localized());

            OutputText("Scenes.Isabellascene.VictoryLactation69.BreastsOnHeadHorns".Localized());

            OutputText("Scenes.Isabellascene.VictoryLactation69.LatchesOnSuckling".Localized());

            OutputText("Scenes.Isabellascene.VictoryLactation69.LostControlGivingUp".Localized());

            OutputText("Scenes.Isabellascene.VictoryLactation69.AirThickWithLust".Localized());

            OutputText("Scenes.Isabellascene.VictoryLactation69.FollowHerLead".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Isabellascene.VictoryLactation69.SqueezeEachCock".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Isabellascene.VictoryLactation69.RubPuffyLips".Localized());
                else
                    OutputText("Scenes.Isabellascene.VictoryLactation69.RubTaintPucker".Localized());

            OutputText("Scenes.Isabellascene.VictoryLactation69.ParagraphBreak".Localized());

            OutputText("Scenes.Isabellascene.VictoryLactation69.SugaryTasteCutOff".Localized());

            OutputText("Scenes.Isabellascene.VictoryLactation69.BelliesFillingConflictDistant".Localized());

            if (Player.BreastRows.Count > 1)
                OutputText("Scenes.Isabellascene.VictoryLactation69.PlastersCrimsonHair".Localized(
                    Player.BreastDescript(1)
                ));

            OutputText("Scenes.Isabellascene.VictoryLactation69.HipsBuckWritheSquelching".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Isabellascene.VictoryLactation69.PumpingOwnQuim".Localized());
            else
                if (Player.HasCock())
                    OutputText("Scenes.Isabellascene.VictoryLactation69.PumpingEachCock".Localized());
                else
                    OutputText("Scenes.Isabellascene.VictoryLactation69.FingeringAssholeMuscularHole".Localized());

            OutputText("Scenes.Isabellascene.VictoryLactation69.ShuddersHeadToToe".Localized());

            OutputText("Scenes.Isabellascene.VictoryLactation69.GuzzleOrgasmicCream".Localized());

            if (Player.HasCock())
                OutputText(Player.MultiCockDescriptLight());
            else
                if (Player.HasVagina())
                    OutputText(Player.VaginaDescript());
                else
                    OutputText(Player.AssholeDescript());

            OutputText("Scenes.Isabellascene.VictoryLactation69.CrushingJigglingBreast".Localized());

            if (Player.HasFuckableNipples())
                OutputText("Scenes.Isabellascene.VictoryLactation69.ThrustsTongueNipplePassage".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                if (Player.Wetness() == 5)
                    OutputText("Scenes.Isabellascene.VictoryLactation69.JuicesAfterthought".Localized());
                else
                    if (Player.Wetness() > 3)
                        OutputText("Scenes.Isabellascene.VictoryLactation69.JuicesDripVaginaAfterthought".Localized());
                    else
                        OutputText("Scenes.Isabellascene.VictoryLactation69.ClenchingVaginaAfterthought".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Isabellascene.VictoryLactation69.FewMorePumpsHard".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Isabellascene.VictoryLactation69.BallsPullingTightGroin".Localized());

                OutputText("Scenes.Isabellascene.VictoryLactation69.WarmthSpreadsSpunk".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Isabellascene.VictoryLactation69.SquirtForAges".Localized());
            }

            if (Player.Gender == Gender.Female)
            {
                OutputText("Scenes.Isabellascene.VictoryLactation69.FingeringAccomplishes".Localized());

                if (Player.Wetness() >= 4)
                    OutputText("Scenes.Isabellascene.VictoryLactation69.JuiceSquirtsHoleSplattering".Localized());
                else
                    if (Player.Wetness() >= 2)
                        OutputText("Scenes.Isabellascene.VictoryLactation69.GirlcumDripsHoleAbsorbing".Localized());
                    else
                        OutputText("Scenes.Isabellascene.VictoryLactation69.GirlcumSoaksLips".Localized());

                OutputText("Scenes.Isabellascene.VictoryLactation69.ClitPulsesThumb".Localized());
            }

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Isabellascene.VictoryLactation69.AssholeFingeringFruition".Localized());

            OutputText("Scenes.Isabellascene.VictoryLactation69.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Isabellascene.VictoryLactation69.RollSidewaysSore".Localized());

            if (!isabellaFollower())
                OutputText("Scenes.Isabellascene.VictoryLactation69.OfHerCamp".Localized());

            OutputText("Scenes.Isabellascene.VictoryLactation69.LookEachOtherOver".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.VictoryLactation69.ZatVasNiceAccent".Localized());
            else
                OutputText("Scenes.Isabellascene.VictoryLactation69.ThatWasNicePlain".Localized());

            OutputText("Scenes.Isabellascene.VictoryLactation69.NodBlissedOutCradling".Localized());

            if (!isabellaFollower())
                SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(8);

            Player.SlimeFeed();

            Player.ChangeStatusValue(StatusEffectIds.Feeder, 0, 1);

            Player.ChangeStatusValue(StatusEffectIds.Feeder, 2, 0);

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaAngryAtPcCounter, 0);

            Player.Orgasm("Tits");

            CombatScene.CleanupAfterCombat();
        }

        public void PCVictoryOnIsabellaButtsex()
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            var y = Player.CockThatFits2(CombatController.Instance!.Context.Enemy.AnalCapacity());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.MilkHuhWontDo".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.Horny".Localized());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.RollOverAllFours".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.HeavingHeftyBody".Localized());
            else
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.PullsHandsAway".Localized());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.CircleLikePredator".Localized());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.ExposeEachCock".Localized());

            if (Player.LustPercent < 70)
            {
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.Allowing".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.ItWord".Localized());
                else
                    OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.ThemWord".Localized());

                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.HardenPrecipitouslyTurgidFlesh".Localized());
            }
            else
            {
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.WindTeasingHardnessLength".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.ItSeems".Localized());
                else
                    OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.TheySeem".Localized());

                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.GrowHarderTumescentEngorging".Localized());
            }

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.LooksBackFearLust".Localized());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.IsabellaMoans".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.ForgettingWoundsNakedBody".Localized());
            else
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.LustilyBeggingPenetrated".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.PutItInsideAccent".Localized());
            else
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.PutItInsidePlain".Localized());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.TalkDirtyCringe".Localized());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.GrabTailYank".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.ProbablyBestSuitedTask".Localized());

                if (y >= 0)
                    OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.IntoHerCowCooch".Localized(
                        Player.CockDescript(y)
                    ));

                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.SentencePeriodSpace".Localized());
            }

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.EyesWidenSuddenPressure".Localized(
                Player.CockHead(x)
            ));

            if (y >= 0)
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.OtherPressingAgainstHoles".Localized(
                    Player.CockHead(y)
                ));
            else
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.PressingAgainstDarkPucker".Localized());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.VoicesMooDiscomfortSlap".Localized(
                Player.CockDescript(x)
            ));

            if (y >= 0)
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.SheathsOtherMember".Localized());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.SentencePeriodParagraphBreak".Localized());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.RedheadShuddersClamping".Localized(
                Player.CockHead()
            ));

            if (y >= 0)
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.SuffixS".Localized());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.ClapEchoLandmark".Localized());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.MilkDripsAudibly".Localized());

            if (y >= 0)
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.MembersWord".Localized());
            else
                OutputText(Player.CockDescript(x));

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.LessDickCrushingTension".Localized());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.PullHairTangles".Localized());

            if (y >= 0)
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.ContractsAndSqueezes".Localized(
                    Player.CockDescript(y)
                ));
            else
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.GirlJuiceOnLegs".Localized());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.RisingMooWarning".Localized());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.TwitchingSpasmsPassage".Localized());

            if (y >= 0)
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.SuffixSAgain".Localized());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.ShootFireworksPleasureSpine".Localized());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.CumBuildsInBalls".Localized());

            if (y < 0)
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.NeverImpregnateOrificeInstinct".Localized());
            else
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.HalfLoadWasted".Localized());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.HeatWellsUp".Localized());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.RedHairsSlipThrough".Localized());

            if (y >= 0)
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.EmulatesItsBrother".Localized(
                    Player.CockDescript(y)
                ));

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.OrgasmWindingDown".Localized());

            if (Player.CumQ() >= 750)
            {
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.PackingHerBottom".Localized());

                if (y >= 0)
                    OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.AndDrippyPuss".Localized());

                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.BalloonHerBackDoor".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.CumQ() < 2000)
                    OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.BellyDistendsSlightly".Localized());
                else
                    OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.WhiteGooSprays".Localized());
            }

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.ParagraphBreak".Localized());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.SmackHerRump".Localized());

            if (y >= 0)
                OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.SuffixSThird".Localized());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.SlumpsDownRumpUp".Localized());

            OutputText("Scenes.Isabellascene.PCVictoryOnIsabellaButtsex.DressedAndProud".Localized());

            if (!isabellaFollower())
                SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(3);

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void VictoryAgainstIzzzzzySixtyNine()
        {
            ClearOutput();

            int x = Player.CockThatFits(38);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.ArentInterestedDifferentRelief".Localized());

            OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.BeforeYouAreThe".Localized());

            if (!Player.HasStatusEffect(StatusEffectIds.Edryn))
                OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.PlumpestJuiciestPussyLips".Localized());
            else
                OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.PlumpestJuiciestVulvaEdryn".Localized());

            OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.DoesntSurpriseGlistenParting".Localized());

            OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.TwistSinuouslySlidingHips".Localized(
                Player.AssDescript()
            ));

            if (Player.Gender == Gender.Female)
                OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.NuzzlePlaceholderSmearingVulva".Localized(
                    Player.VaginaDescript(0)
                ));
            else
                if (Player.CockThatFits(38) != -1)
                    OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.SwallowWholePlyingTongue".Localized(
                        Player.CockDescript(x)
                    ));
                else
                    OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.LickPlaceholderFondlingLong".Localized(
                        Player.CockDescript(x)
                    ));

            OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.SentencePeriodParagraphBreak".Localized());

            OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.BovineBeautyPausesAttentions".Localized());

            if (Player.Gender == Gender.Female)
            {
                OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.MuffDivingGusto".Localized());

                if (Player.GetClitLength() >= 5)
                    OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.StrugglingCockLikeAppendage".Localized());
                else
                    if (Player.GetClitLength() >= 2)
                        OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.SucklingLargeButtonInhalations".Localized());
                    else
                        OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.DevouringTinyBud".Localized());

                OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.SmoothSkinTongueCaresses".Localized());

                if (Player.Wetness() < 3)
                    OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.CoatingHerTongue".Localized());
                else
                    if (Player.Wetness() < 4)
                        OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.SloppilyLeakingEverywhere".Localized());
                    else
                        OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.SquirtingSensitiveSpot".Localized());

                OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.CurlingTubeShapePistoning".Localized());
            }
            else
                if (Player.CockThatFits(38) != -1)
                {
                    OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.WrapsLipsAround".Localized());

                    if (Player.Cocks.Exists(c => c.SheathOn))
                        OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.SheathWord".Localized());
                    else
                        OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.BaseWord".Localized());

                    OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.SucklingCheeksHollowVacuum".Localized(
                        Player.CockDescript(x)
                    ));

                    if (Player.CockTotal() > 1)
                    {
                        OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.IgnoresOtherDick".Localized());

                        if (Player.CockTotal() > 2)
                            OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.SmearsHerCheeks".Localized());
                        else
                            OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.SmearsFocusingSpecimen".Localized());
                    }
                }
                else
                {
                    OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.WrapsTongueAround".Localized());

                    if (Player.Cocks.Exists(c => c.SheathOn))
                        OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.SheathWordAgain".Localized());
                    else
                        OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.BaseWordAgain".Localized());

                    OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.NumerousTimesBobbingHead".Localized(
                        Player.CockDescript(x)
                    ));

                    if (Player.HasKnot(x))
                        OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.StrugglingWithTheKnot".Localized());
                    else
                        if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                            OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.StuckAtMedialRing".Localized());
                        else
                            if (Player.Cocks[x].Id == BodyPartTypes.Demon)
                                OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.SlowedByAllNubs".Localized());
                            else
                                if (Player.Cocks[x].Id == BodyPartTypes.Cat)
                                    OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.SlowedByAllSpines".Localized());
                                else
                                    if (Player.Cocks[x].Id == new Identifier("coc", "tentacle"))
                                        OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.StoppingUnderMushroomHead".Localized());
                                    else
                                        if (Player.Cocks[x].Id == BodyPartTypes.Anemone)
                                            OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.GigglingStingsAnemonePrick".Localized());
                                        else
                                            OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.RubbingMouthUrethralBulge".Localized());

                    OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.KissOnCumSlit".Localized());
                }

            OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.ParagraphBreak".Localized());

            OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.TalentWorkingGroin".Localized());

            if (Player.HasMuzzle())
                OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.IdeaCloseMouth".Localized());
            else
                OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.ClitBumpsNoseSubdue".Localized());

            OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.ThickViscousCreamFountains".Localized());

            if (Player.Gender == Gender.Female)
            {
                OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.ExultVictorySquealsVibrate".Localized());

                if (Player.Wetness() < 5)
                    OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.SquirtWord".Localized());
                else
                    OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.DripWord".Localized());

                OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.ScreamingEqualIntensity".Localized());
            }
            else
                if (Player.CockThatFits(38) != -1)
                {
                    OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.ExultVictorySqueals".Localized(
                        Player.CockDescript(x)
                    ));

                    if (Player.CumQ() >= 250)
                    {
                        OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.NextRopeOfCream".Localized());

                        if (Player.CumQ() >= 500)
                            OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.KeepPumpingSpooge".Localized());

                        if (Player.CumQ() >= 1000)
                            OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.BellyFillsPullsBack".Localized());

                        if (Player.CumQ() >= 1500)
                            OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.TanVisageCoated".Localized());
                    }
                    else
                        OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.YourEntireLoad".Localized());

                    OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.SentencePeriod".Localized());
                }
                else
                {
                    OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.ExultVictoryAgain".Localized(
                        Player.CockDescript(x)
                    ));

                    if (Player.CumQ() > 250)
                    {
                        OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.DumpChestAndHead".Localized());

                        if (Player.CumQ() > 500)
                            if (Player.CumQ() > 1000)
                                OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.OrgasmDragsOn".Localized());
                            else
                                OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.TopClingsRevealingCurve".Localized());

                        OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.SentencePeriodAgain".Localized());
                    }
                }

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.OneManBukkakeShow".Localized());

            OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.FinallySatedRollOff".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.TwitchingMemberDisentangles".Localized());
            else
                OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.TwitchingClitCatchesTongue".Localized());

            OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.CatchBreathVigor".Localized());

            OutputText("Scenes.Isabellascene.VictoryAgainstIzzzzzySixtyNine.LeaveWithSmile".Localized());

            Player.Orgasm("Generic");

            if (!isabellaFollower())
                SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(9);

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaAngryAtPcCounter, 0);

            Player.SlimeFeed();

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void TooBigVictoryTittyFuckingFuntimesWithMilk()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.TossArmorReveal".Localized(
                Player.CockDescript(x)
            ));

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.Lusty".Localized());
            else
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.Weakened".Localized());

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.EyesWideBeholdsLength".Localized());

            if (Player.LustPercent > 70)
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.PulsateRagingLust".Localized());
            else
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.FillsBloodGrowingLust".Localized());

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.StrokeStandingOver".Localized());

            if (Player.CockThatFits(CombatController.Instance!.Context.Enemy.Body.Get<VaginaPart>().Capacity()) != -1)
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.NoWayInCunt".Localized());
            else
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.SentencePeriodParagraphBreak".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.DontVantMilkersAccent".Localized());
            else
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.DontWantThatPlain".Localized());

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.SayStart".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.TooHotToResist".Localized());
            else
                if (Player.Corruption < 66)
                {
                    OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.IsntUpForDiscussion".Localized());

                    if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                        OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.SoDrippyWantThis".Localized());
                    else
                        OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.LostDealWithConsequences".Localized());
                }
                else
                    OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.ToughShit".Localized());

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.AnswerEmphasizePopLaces".Localized());

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.OffOnBeingDisrobed".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.RemoveWithoutDamage".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.RemoveWithoutCare".Localized());
                else
                    OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.TearOffOffendingGarment".Localized());

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.TearyBrownEyes".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.Ja".Localized());
            else
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.SentencePeriod".Localized());

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.SayEndParagraphBreak".Localized());

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.GazeOnQuadTipped".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.SighingBlissfullyGrabNipples".Localized(
                Player.CockHead(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.PleasuredMoo".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.GrindsFrenzyThrashingLegs".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Cocks[x].Length < 50)
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.BuryFewInches".Localized());
            else
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.CockAcrossHerLips".Localized());

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.MilkSplattersPalms".Localized());

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.LaidOverMilkSpewing".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Cocks[x].Length < 50)
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.ToHerMouth".Localized());
            else
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.SmotherInDickFlesh".Localized());

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.UnthinkingPleasure".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.BallsPullTight".Localized());
            else
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.BubbleWarmthGrowsLoins".Localized());

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.KnowNoStoppingClimax".Localized());

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.StartingAtThe".Localized());

            if (Player.Cocks.Exists(c => c.SheathOn))
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.SheathWord".Localized());
            else
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.BaseWord".Localized());

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.DistortedBulgeUrethraValve".Localized());

            if (Player.Cocks[x].Length < 50)
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.LeansForwardSwallowMore".Localized());
            else
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.LeansBackSwallowPlaceholder".Localized(
                    Player.CockHead(x)
                ));

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.ExplodesInHerMouth".Localized());

            if (Player.CumQ() < 25)
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.SingleGulpDemolishesMeager".Localized());
            else
                if (Player.CumQ() < 150)
                    OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.SlightChangeShapeCheeks".Localized());
                else
                    if (Player.CumQ() < 250)
                        OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.CumEscapesHerCheeks".Localized());
                    else
                        OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.SeedDownHerChin".Localized());

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.NextPulseLargerFirst".Localized());

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.AllGoodThingsEnd".Localized());

            if (Player.CumQ() < 500)
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.SpurtOn".Localized());
            else
                OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.Soak".Localized());

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.BreastsMooingTuggingTeats".Localized());

            if (!isabellaFollower())
                SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(3);

            OutputText("Scenes.Isabellascene.TooBigVictoryTittyFuckingFuntimesWithMilk.WipeOffAndDress".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void VaginalProdNPokeIsabella()
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            if (x < 0)
                x = 0;

            OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.LeastOfConcerns".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.Lusty".Localized());
            else
                OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.Defeated".Localized());

            OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.PalesAndAsks".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.VhyNotMilkAccent".Localized());
            else
                OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.WhyNotMilkPlain".Localized());

            OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.EmphasizesMoo".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.SlowlyEngorging".Localized());
            else
                OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.JuiceSlicked".Localized());

            OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.CommentOnHerVagina".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.NeverVantedFightAccent".Localized());
            else
                OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.NeverWantedFightPlain".Localized());

            OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.TouchAroundHerLoins".Localized((noFur ? "" : "softly-furred ")));

            OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.AskBitesLipPetulantly".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.VantInPussyAccent".Localized());
            else
                OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.WantInPussyPlain".Localized());

            OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.HalfCriesShudder".Localized());

            OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.ReachForSoddenSnatch".Localized(
                Player.Mf("chuckle", "giggle")
            ));

            OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.SensualExaggeration".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
            {
                if (Player.CockArea(x) > CombatController.Instance!.Context.Enemy.VaginalCapacity())
                    OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.CockTooBigAccent".Localized());
                else
                    if (Player.CockArea(x) > 13)
                        OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.RubbingPussyAccent".Localized());
                    else
                        OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.SurpriseNiceComfortableAccent".Localized());

                OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.ScreamsHeavilyAccentedAwkward".Localized());
            }
            else
            {
                if (Player.CockArea(x) > CombatController.Instance!.Context.Enemy.VaginalCapacity())
                    OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.TooBigStretchingPlain".Localized());
                else
                    if (Player.CockArea(x) > 13)
                        OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.RubbingPussyWellPlain".Localized());
                    else
                        OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.SurpriseNiceComfortablePlain".Localized());

                OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.ScreamsLightlyAccentedAwkward".Localized());
            }

            OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.SmirkSlapBreastGeyser".Localized());

            OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.DistractingMilkFocusCunt".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Cocks.Count > 1)
            {
                OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.WishHadRoomFor".Localized());

                if (Player.Cocks.Count > 2)
                    OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.AnotherEachCock".Localized());
                else
                    OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.YourOtherPenis".Localized());

                OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.SlickedLipsTooTight".Localized());
            }

            OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.CuntSquelchesWetly".Localized());

            OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.CreamInYourEye".Localized());

            OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.LegsWrapSqueezeTightly".Localized());

            if (Player.Toughness > 80)
                OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.DamageALesserPerson".Localized());
            else
                if (Player.Toughness > 50)
                    OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.ActuallyHurts".Localized());
                else
                    OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.WorryShellCrushPelvis".Localized());

            OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.PlaceholderWrungEqualTightness".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.CumBubblesUp".Localized());

            if (Player.CumQ() >= 1000)
            {
                if (Player.CumQ() < 1500)
                    OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.BellyPudgesSemenWomb".Localized());
                else
                    if (Player.CumQ() < 2500)
                        OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.BellyDistendsMassivelyProof".Localized());
                    else
                        OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.MassivelyDistendedBellyProof".Localized());

                if (Player.CumQ() >= 1500)
                    OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.HugePuddleBonusThough".Localized());
            }

            OutputText("Scenes.Isabellascene.VaginalProdNPokeIsabella.PullOutHappySigh".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaAngryAtPcCounter, 0);

            if (!isabellaFollower())
                SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(7);

            CombatScene.CleanupAfterCombat();

            Player.Orgasm("Dick");
        }

        public void TinyVictoryTittyFuckingFuntimesWithMilk()
        {
            ClearOutput();

            var x = Player.SmallestCockIndex();

            OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.TossArmorReveal".Localized(
                Player.CockDescript(x)
            ));

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.Lusty".Localized());
            else
                OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.Weakened".Localized());

            OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.SquealsAtYourCocks".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.SinglesOutSmallest".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.ZatVantVantedAccent".Localized());
            else
                OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.WhatYouWantedPlain".Localized());

            OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.GrabsHoldPlaceholderStrokes".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Corruption < 33)
                OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.ShamefulBlushToCheeks".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.TinyBlushToCheeks".Localized());
                else
                    OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.ArentReallyBotheredSlightest".Localized());

            OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.RegainControlDemanding".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.HerOwnNeeds".Localized());
            else
                OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.HerWounds".Localized());

            OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.ParagraphBreak".Localized());

            OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.LeanAgainstGroin".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.IgnoresTheOtherOne".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.SuffixS".Localized());
            }

            OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.SmirksUpJiggleBoobs".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.CuteLittleCockAccent".Localized());
            else
                OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.CuteLittleCockPlain".Localized());

            OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.ArrestTheNod".Localized());

            OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.UnawareOfConflict".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.BodyBlushTeasing".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.CumForMeAccent".Localized());
            else
                OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.CumForMePlain".Localized());

            OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.SultryTeasesTippingPoint".Localized());

            if (Player.CumQ() < 50)
                OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.PatheticLittleLoad".Localized());
            else
                if (Player.CumQ() < 250)
                    OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.NiceGooeyLoad".Localized());
                else
                    if (Player.CumQ() < 500)
                        OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.HeftyTitDrenchingLoad".Localized());
                    else
                        OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.MassiveBodySoakingLoad".Localized());

            OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.IntoCleavage".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.BlottingChestFace".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.CoverMeAccent".Localized());
            else
                OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.CoverMePlain".Localized());

            OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.ShakeSquirmWeak".Localized());

            OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.CleansThenDelvesBack".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.ComeBackSoonAccent".Localized());
            else
                OutputText("Scenes.Isabellascene.TinyVictoryTittyFuckingFuntimesWithMilk.ComeBackSoonPlain".Localized());

            if (!isabellaFollower())
                SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(8);

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaAngryAtPcCounter, 0);

            CombatScene.CleanupAfterCombat();

            Player.Orgasm("Dick");
        }

        public void TalkWithIsabella()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaTimesTalked, CoC.Instance.Flags.Get(GameFlagIds.IsabellaTimesTalked) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaTimesTalked) == 1)
            {
                OutputText("Scenes.Isabellascene.TalkWithIsabella.TellTaleAskWhy".Localized());

                OutputText("Scenes.Isabellascene.TalkWithIsabella.ClearsThroatNervouslyBegins".Localized(((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0) ? "I vas an adventurer in mein homeland. I vould roam the vilds vith nothing but mein shield, mein skills, and mein viits to back me up. It vas vunderbar. I had amassed a small fortune between doing mercenary vork and dungeoneering, but one day I delved too deep. Zere vas a fluttering pink portal, and vith ze thought of riches filling mein noggin, I stepped through" : "I was an adventurer in my homeland. I would roam the wilds with nothing but my shield, my skills, and my wits to back me up. It was wonderful. I had amassed a small fortune between doing mercenary work and dungeoneering, but one day I delved too deep. There was a fluttering pink portal, and with the thought of riches filling my head, I stepped through")));

                OutputText("Scenes.Isabellascene.TalkWithIsabella.TearsAboutPortal".Localized(((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0) ? "vouldn't let me back through. I got stuck here, away from mein friends and family, vith nothing but sex-perverts for company. Thankfully, most of zem couldn't handle mein" : "wouldn't let me back through. I got stuck here, away from my friends and family, with nothing but sex-perverts for company. Thankfully, most of them couldn't handle my")));

                OutputText("Scenes.Isabellascene.TalkWithIsabella.LookCuriouslyAlwaysBovine".Localized(((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0) ? "zat one coming. Still, I vas not always as you see. Some of ze foes I defeated dropped some potions. And... ze bova ones vere sooo delicious. I couldn't help meinself. I even mixed a few different kinds together to see vhat vould happen. It made mein" : "that one coming. Still, I was not always as you see. Some of the foes I defeated dropped some potions. And... the bova ones were sooo delicious. I couldn't help myself. I even mixed a few different kinds together to see what would happen. It made my")));

                OutputText("Scenes.Isabellascene.TalkWithIsabella.MadeANiceHome".Localized());

                OutputText("Scenes.Isabellascene.TalkWithIsabella.CrushingTightHug".Localized());

                if (!isabellaFollower())
                    SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(10);
            }
            else
                OutputText("Scenes.Isabellascene.TalkWithIsabella.ShareTalesAdventures".Localized());

            DoNext(() => ReturnToCamp(1));
        }

    }
}