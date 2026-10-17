using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Npcs;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{
    [Register<TamanisDaughtersScene>("coc:tamanis_daughters")]
    public class TamanisDaughtersScene : Scene
    {

        public static bool TamaniPresent;

        public const int PlayerFatherPregnancyType = 25;

        public const int DaughterPregnancyIncubation = 216;

        public static bool DaughtersPregnant =>

            CoC.Instance.Flags.Get(GameFlagIds.TamaniDaughtersPregnancyType) != 0;

        public static bool TamaniIsPregnant()
        {
            var scene = SceneInstance<TamaniScene>.Get();

            return NpcPregnancy.IsPregnant(scene.Definition, scene.State);
        }

        public static void KnockUpTamani() => SceneInstance<TamaniScene>.Get().TamaniKnockUp();

        public TamanisDaughtersScene()
        {
        }

        public override void Entry()
        {

        }

        public bool TimeChange()
        {
            var flags = CoC.Instance?.Flags;

            if (flags == null)
                return false;

            if (DaughtersPregnant)
            {
                var incubation = flags.Get(GameFlagIds.TamaniDaughterPreggoCountdown);

                if (incubation > 0)
                {
                    incubation -= 1;

                    flags.Set(GameFlagIds.TamaniDaughterPreggoCountdown, incubation);
                }

                if (incubation == 0)
                {
                    flags.Set(GameFlagIds.TamaniNumberOfDaughters,

                        flags.Get(GameFlagIds.TamaniNumberOfDaughters) +

                        flags.Get(GameFlagIds.TamaniDaughtersPregnancyCount));

                    flags.Set(GameFlagIds.TamaniDaughtersPregnancyCount, 0);

                    flags.Set(GameFlagIds.TamaniDaughtersPregnancyType, 0);
                }
            }

            if (flags.Get(GameFlagIds.TimesEncountedTamanisDaughters) == 0 &&
                flags.Get(GameFlagIds.TamaniNumberOfDaughters) > 30)

                flags.Set(GameFlagIds.TamaniNumberOfDaughters, 30);

            if (flags.Get(GameFlagIds.TamaniNumberOfDaughters) > 40 &&
                View.Time.Hours > 23 && View.Time.Days % 4 == 0)

                flags.Set(GameFlagIds.TamaniNumberOfDaughters,

                    flags.Get(GameFlagIds.TamaniNumberOfDaughters) - 1);

            return false;
        }

        public bool TimeChangeLarge() => false;

        public string TdCup()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.TamaniNumberOfDaughters) < 20)
                return "C";
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.TamaniNumberOfDaughters) < 30)
                    return "D";
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.TamaniNumberOfDaughters) < 40)
                        return "DD";
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.TamaniNumberOfDaughters) < 50)
                            return "E";
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.TamaniNumberOfDaughters) < 60)
                                return "EE";

            return "F";
        }

        public void EncounterTamanisDaughters()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.TimesEncountedTamanisDaughters, CoC.Instance.Flags.Get(GameFlagIds.TimesEncountedTamanisDaughters) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesEncountedTamanisDaughters) > 0 && Random.Shared.Next(10) == 0)
            {
                TamaniPresent = true;

                OutputText("Scenes.TamanisDaughters.EncounterTamanisDaughters.BlockedByGoblins".Localized(
                     NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.TamaniNumberOfDaughters) / 4)
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.TamaniTimesHypnotised) >= 10)
                    OutputText("Scenes.TamanisDaughters.EncounterTamanisDaughters.YourWife".Localized());

                OutputText("Scenes.TamanisDaughters.EncounterTamanisDaughters.RealizeDaughters".Localized(
                     TdCup()
                ));

                OutputText("Scenes.TamanisDaughters.EncounterTamanisDaughters.WhatDoYouDo".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Fight".Localized(), FightTamanisDaughters);

                AddButton(1, "Common.Button.FuckThem".Localized(), FuckYoDaughtersHomie);

                AddButton(2, "Common.Button.LetThem".Localized(), LegTamanisDaughtersRAEPYou);

                return ;
            }

            TamaniPresent = false;

            OutputText("Scenes.TamanisDaughters.EncounterTamanisDaughters.BlockedByGoblinsAgain".Localized());

            OutputText("Scenes.TamanisDaughters.EncounterTamanisDaughters.GoblinsYou".Localized(
                 NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.TamaniNumberOfDaughters) / 4)
            ));

            if (Player.Weapon.IsUnarmed())
                OutputText("Scenes.TamanisDaughters.EncounterTamanisDaughters.ReadyYourWeapon".Localized());
            else
                OutputText("Scenes.TamanisDaughters.EncounterTamanisDaughters.DrawYourWeapon".Localized());

            OutputText("Scenes.TamanisDaughters.EncounterTamanisDaughters.GlanceAround".Localized(
                 TdCup()
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedTamanisDaughters) == 0)
            {
                OutputText("Scenes.TamanisDaughters.EncounterTamanisDaughters.TiredOfLeftovers".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Fight".Localized(), FightTamanisDaughters);

                AddButton(1, "Common.Button.FuckThem".Localized(), FuckYoDaughtersHomie);

                AddButton(2, "Common.Button.LetThem".Localized(), LegTamanisDaughtersRAEPYou);

                AddButton(3, "Scenes.TamanisDaughters.EncounterTamanisDaughters.ButtonPlayDumb".Localized(), PlayDumbToTamanisDaughters);
            }
            else
            {
                OutputText("Scenes.TamanisDaughters.EncounterTamanisDaughters.CameBackForCream".Localized());

                OutputText("Scenes.TamanisDaughters.EncounterTamanisDaughters.NoIsNotAWord".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Fight".Localized(), FightTamanisDaughters);

                AddButton(1, "Common.Button.FuckThem".Localized(), FuckYoDaughtersHomie);

                AddButton(2, "Common.Button.LetThem".Localized(), LegTamanisDaughtersRAEPYou);
            }
        }

        public void PlayDumbToTamanisDaughters()
        {
            ClearOutput();

            OutputText("Scenes.TamanisDaughters.PlayDumbToTamanisDaughters.ShrugAndAsk".Localized(
                 Player.Mf("guy", "person")
            ));

            if (Player.Intelligence / 2 + 25 > Random.Shared.Next(75))
            {
                OutputText("Scenes.TamanisDaughters.PlayDumbToTamanisDaughters.LeaderLooksYouUp".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.TimesEncountedTamanisDaughters) > 1)
                    OutputText("Scenes.TamanisDaughters.PlayDumbToTamanisDaughters.NotPayingAttention".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            OutputText("Scenes.TamanisDaughters.PlayDumbToTamanisDaughters.StampsFoot".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesEncountedTamanisDaughters) == 1)
                OutputText("Scenes.TamanisDaughters.PlayDumbToTamanisDaughters.First".Localized());

            OutputText("Scenes.TamanisDaughters.PlayDumbToTamanisDaughters.LittersOneWay".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.FuckThem".Localized(), FuckYoDaughtersHomie);

            AddButton(1, "Common.Button.Fight".Localized(), FightTamanisDaughters);

            AddButton(2, "Common.Button.LetThem".Localized(), LegTamanisDaughtersRAEPYou);
        }

        public void FightTamanisDaughters()
        {
            ClearOutput();

            OutputText("Scenes.TamanisDaughters.FightTamanisDaughters.WhirlAround".Localized());

            var enemy = EnemyRegistry.Create(EnemyIds.TamanisDaughters);

            if (TamaniPresent)
            {

                enemy.Strength += 5;

                enemy.Toughness += 5;

                enemy.Health += 10;

                enemy.Lust -= 20;
            }

            CombatScene.Start(enemy, () => Go<CampScene>());

            return ;
        }

        public void FuckYoDaughtersHomie()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedTamanisDaughters, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedTamanisDaughters) + 1);

            var cocks = Player.Cocks.Count;

            float daughters = CoC.Instance.Flags.Get(GameFlagIds.TamaniNumberOfDaughters) / 2;

            OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.ShrugOutArmor".Localized());

            if (cocks == 1)
                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.Your".Localized());
            else
                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.OneOfYour".Localized());

            OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.SwingingIt".Localized());

            OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.ComeAndGetIt".Localized());

            if (daughters < 12)
                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.GirlsSurge".Localized());
            else
                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.CrowdSurges".Localized());

            OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.MobbingFromAllSides".Localized());

            int primary = Player.CockThatFits(50);

            if (primary >= 0)
            {
                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.ImpaleRandom".Localized(
                     Player.CockDescript(primary),

                     Player.CockDescript(primary)
                ));

                if (TamaniPresent)
                {
                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.PushesAsideSluts".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.CupsYourBalls".Localized());
                    else
                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.LicksTaint".Localized());
                }

                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.ParagraphBreak".Localized());

                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.ViseGrip".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.BloatingBelly".Localized());

                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.FallsOffEyesCrossed".Localized());

                if (TamaniPresent)
                {
                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.ShovesRestMounts".Localized());

                    if (Player.Tallness >= 60)
                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.ScrabblingUp".Localized());

                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.KissRoughly".Localized());
                }
                else
                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.AnotherMounts".Localized());

                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.SwarmedInMud".Localized());

                if (TamaniPresent)
                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.JugsRestAgainst".Localized());
                else
                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.DaughterLooksUp".Localized());

                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.PerverseThoughts".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.SupposedToSave".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.CameToMakeBetter".Localized());
                    else
                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.CameToMakeBetterAgain".Localized());

                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.MoanAsCuntPressed".Localized());

                if (!TamaniPresent)
                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.Daughter".Localized());
                else
                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.FavoriteGoblinMilf".Localized());

                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.PeriodBreak".Localized());

                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.PressureSpikes".Localized());

                if (!TamaniPresent)
                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.YourGoblinDaughter".Localized());
                else
                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.Tamani".Localized());

                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.InHerWomb".Localized(
                     Player.CockDescript(primary)
                ));

                if (daughters > 10)
                {
                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.OrgasmTrailsOff".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.BallsPuffUp".Localized());
                    else
                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.GutClenches".Localized());

                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.ParagraphBreakAgain".Localized());

                    if (TamaniPresent)
                    {
                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.PushesSlutOff".Localized());

                        if (TamaniIsPregnant())
                            OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.RubsPregnantBelly".Localized());
                        else
                            OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.FondlesNipples".Localized());

                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.MoansGrinds".Localized());
                    }
                    else
                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.FirstDaughterFace".Localized());

                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.PerverseThoughtsWorm".Localized(
                         Player.CockDescript(primary)
                    ));

                    if (daughters < 20)
                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.TheMajority".Localized());
                    else
                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.Half".Localized());

                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.SpoogeSlickedCunts".Localized());

                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.AsYour".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.BallsEmpty".Localized());
                    else
                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.ProstateEmpties".Localized());

                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.GirlWandersAsks".Localized());

                    if (daughters > 20)
                    {
                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.LieEmptyHusk".Localized());

                        if (TamaniPresent)
                            OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.WhileTamaniWalks".Localized());

                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.FewNotSatisfied".Localized(
                             Player.CockDescript(primary)
                        ));

                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.SighingHappily".Localized());

                        if (daughters < 50)
                            OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.GigglesSootheSleep".Localized());
                        else
                            OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.GigglesSootheSleepAgain".Localized());
                    }
                }

                if (daughters <= 20)
                {
                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.ExhaustedShaken".Localized());

                    if (TamaniPresent)
                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.TamaniFrenches".Localized());
                    else
                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.Period".Localized());

                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.LicksSoreMember".Localized());
                }
            }
            else
            {
                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.PickRandomGirl".Localized());

                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.IdeaSoundsGreat".Localized(
                     Player.CockHead()
                ));

                if (TamaniPresent)
                {
                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.ReachesIntoPouch".Localized());

                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.DisappearsBehind".Localized());

                    if (Player.Tallness > 48)
                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.HearHerClimbing".Localized());

                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.DisdainRuined".Localized(
                         Player.CockHead(),

                         Player.AssDescript()
                    ));

                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.HeadSlipsInside".Localized());

                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.CumBubblesNose".Localized());

                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.WiltsDrooping".Localized(
                         Player.CockDescript(0)
                    ));

                    if (Player.Balls > 0)
                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.BallsAcheStrain".Localized());

                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.PreCumBeads".Localized());
                }

                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.ThirdAttacksTaint".Localized());

                if (Player.Balls > 0)
                    OutputText(Player.BallsDescriptLight());
                else
                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.Shaft".Localized());

                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.PressesNipples".Localized());

                if (!TamaniPresent)
                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.PressesBackdoor".Localized());
                else
                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.RidgesFlare".Localized());

                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.ExplodeHarder".Localized(
                     Player.CockDescript(0)
                ));

                if (TamaniPresent)
                {
                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.PushesScreams".Localized());

                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.VoiceTrailsGibberish".Localized());
                }
                else
                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.AnotherBottle".Localized());

                if (TamaniPresent)
                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.LetterT".Localized());

                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.GirlsShiftPosition".Localized());

                if (!TamaniPresent)
                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.ShrugDrink".Localized());
                else
                    OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.YouBegin".Localized());

                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.HornierAndHornier".Localized());

                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.GirlGetsIdea".Localized());

                OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.FinishCollapse".Localized());

                if (!(daughters < 20))
                    if (daughters < 30)
                    {
                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.VialSlipped".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Toughness, -.5f), new DynStat(GameStats.Intelligence, -.5f));
                    }
                    else
                    {
                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.VialAfterVial".Localized());

                        OutputText("Scenes.TamanisDaughters.FuckYoDaughtersHomie.PassedOutBeforeAnswer".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Toughness, -.75f), new DynStat(GameStats.Intelligence, -1f), new DynStat(GameStats.Libido, .5f));
                    }
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Corruption, 1f));

            if (TamaniPresent)
                KnockUpTamani();

            KnockUpDaughters();

            Player.CumMultiplier += .3f;

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(4));
        }

        public void LegTamanisDaughtersRAEPYou()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedTamanisDaughters, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedTamanisDaughters) + 1);

            var cocks = Player.Cocks.Count;

            float daughters = CoC.Instance.Flags.Get(GameFlagIds.TamaniNumberOfDaughters) / 2;

            int primary = Player.CockThatFits(50);

            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.KnowingFullWell".Localized());

            if (daughters > 20)
                OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.Large".Localized());

            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.CrowdWontLetLeave".Localized());

            if (cocks > 1)
                OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.OneOfAgain".Localized());

            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.TeasingRises".Localized(
                 Player.Mf("man-", "")
            ));

            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.GashMountsFace".Localized());

            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.OthersStayBusy".Localized());

            if (primary >= 0)
            {
                OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.StraddlesGuides".Localized());

                if (cocks == 2)
                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.HandsLatchFreeDick".Localized());
                else
                    if (cocks > 2)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.MoreHandsLatch".Localized());
                    else
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.TongueAsshole".Localized());

                OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ParagraphBreak".Localized());

                OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.FluidsSoakFace".Localized());

                if (cocks > 2)
                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ManyGirlsImpaled".Localized());
                else
                    if (cocks == 2)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.WhorishExpressions".Localized());
                    else
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ElderDaughterGaze".Localized(
                             Player.CockDescript(primary)
                        ));

                OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.NothingButCum".Localized());

                if (cocks > 1)
                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.TwitchingUnloads".Localized());
                else
                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.TwitchingUnloadsAgain".Localized());

                if (Player.CumQ() >= 1000)
                {
                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.SupernaturalSpooge".Localized());

                    if (cocks == 1)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.BellyLooksPregnant".Localized());
                    else
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.UterusesPregnant".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.Period".Localized());
                }

                if (cocks == 1)
                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.RisesBlowsKiss".Localized());
                else
                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.TheyRiseOff".Localized());

                if (TamaniPresent)
                {
                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ShovesCumFilled".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.SheGives".Localized());

                    if (cocks > 1)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.OneOfAgain".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.PlayfulSlapDeflating".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.PantomimeSighDrags".Localized());

                    if (!Player.Cocks.Exists(c => c.SheathOn))
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.Base".Localized());
                    else
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.SheathAgain".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ToTheTipGiggling".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.NiceWifeFlask".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.MassagesYour".Localized());

                    if (Player.BiggestBreastSize() < 1)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ChestFifth".Localized());
                    else
                        OutputText(Appearance.AllBreastsDescript(Player));

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.AsSheMounts".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.SqueezingWetCunt".Localized(
                         Player.CockDescript(primary),

                         Player.CockDescript(primary)
                    ));

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.DroolAcceptingTruth".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.WordsStopMattering".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.BallsThird".Localized());
                    else
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.GroinAgain".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ChurningLust".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.TamaniTimesHypnotised) < 10)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.WaitWifeMistress".Localized());
                    else
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.YesSoundsRight".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.UntilOrgasmWracks".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.WrigglesMoans".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.MistressStepsAway".Localized());

                    if (daughters < 20)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.SmallPackUponYou".Localized());
                    else
                        if (daughters < 30)
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.PackForcingAphrodisiacs".Localized());
                        else
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.MassiveGroupDrugging".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.LieDruggedDrained".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.TamaniTimesHypnotised, CoC.Instance.Flags.Get(GameFlagIds.TamaniTimesHypnotised) + 1);

                    KnockUpTamani();
                }
                else
                    if (daughters < 20)
                    {
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.SmallerGirlsTakeTurns".Localized(
                             Player.CockDescript(primary)
                        ));

                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.AmazinglyOrgasmAgain".Localized(
                             Player.CockDescript(primary)
                        ));

                        if (Player.CumQ() < 100)
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.TwitchEmptyingLast".Localized());
                        else
                            if (Player.CumQ() < 500)
                                OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.TwitchPumpingFull".Localized());
                            else
                                OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.TwitchPowerfulSpasms".Localized());

                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.DisappointedMoans".Localized(
                             Player.CockDescript(primary)
                        ));

                        if (Player.CumQ() >= 500)
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.EnterprisingGoblinsGather".Localized());

                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ParagraphBreakAgain".Localized());

                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.TiredFromAcrobatics".Localized(
                             Player.CockDescript(primary)
                        ));
                    }
                    else
                    {
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.SmallerGirlsTakeTurnsAgain".Localized(
                             Player.CockDescript(primary)
                        ));

                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.AmazinglyOrgasmAgainAgain".Localized());

                        if (Player.CumQ() < 100)
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.TwitchEmptyingLastAgain".Localized());
                        else
                            if (Player.CumQ() < 500)
                                OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.TwitchPumpingFullAgain".Localized());
                            else
                                OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.TwitchPowerfulSpasmsAgain".Localized());

                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.DisappointedMoansAgain".Localized(
                             Player.CockDescript(primary)
                        ));

                        if (Player.CumQ() >= 500)
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.EnterprisingGoblinsGatherAgain".Localized());

                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ParagraphBreakThird".Localized());

                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ExhaustedFromBattering".Localized());

                        if (Player.Balls > 0)
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.BallsSwelling".Localized());
                        else
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.GroinShifts".Localized());

                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.CocksSpasmUnearned".Localized());

                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.GruntLongRope".Localized());

                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.FilledGoblinPulled".Localized());

                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.RidingReverseCowgirl".Localized());

                        if (daughters < 40)
                        {
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.CeaselessOrgasm".Localized());

                            if (Player.Balls > 0)
                                OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.BallsHurt".Localized());
                            else
                                OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.CrotchHurts".Localized());

                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.FromGeneratingSpooge".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.CeaselessOrgasmAgain".Localized());

                            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Corruption, .5f));

                            Player.CumMultiplier += .3f;
                        }
                    }
            }
            else
                if (!TamaniPresent)
                {
                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.DaughterLaunchesOnto".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.FrowningPresses".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.FluidsSoakFaceAgain".Localized());

                    if (daughters < 20)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.Pert".Localized());
                    else
                        if (daughters > 40)
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.Heavy".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.BreastsOverPart".Localized());

                    if (cocks == 2)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.AnotherPetiteClimbs".Localized(
                             Player.CockDescript(1)
                        ));

                    if (cocks > 2)
                    {
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ThirdSluttyMakeup".Localized());

                        if (cocks > 3)
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.RemainingCocks".Localized());
                        else
                            OutputText(Player.CockDescript(2));

                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.PeriodAgain".Localized());
                    }

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ParagraphBreakFourth".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.PivotsGashAgainst".Localized(
                         Player.CockHead()
                    ));

                    if (Player.BiggestBreastSize() < 1)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ChestAgain".Localized());
                    else
                        OutputText(Appearance.AllBreastsDescript(Player));

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.SendingFreshSurge".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.BallsAgain".Localized());
                    else
                        if (Player.HasVagina())
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.Pussy".Localized());
                        else
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.Taint".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.MassagingYour".Localized());

                    if (Player.Cocks.Exists(c => c.SheathOn))
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.SheathAgain".Localized());
                    else
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.Crotch".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.SensitiveAroundCrotch".Localized(
                         Player.Mf("stud", "sexy")
                    ));

                    if (cocks == 2)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.HerSisterAgrees".Localized());
                    else
                        if (cocks > 2)
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.HerSistersAgree".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.BodyCavesDemands".Localized());

                    if (cocks == 1)
                    {
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.DaughterGigglesSqueals".Localized());

                        if (Player.CumQ() < 100)
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.OrgasmEndsPulsing".Localized());
                        else
                            if (Player.CumQ() < 500)
                                OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.OrgasmDragsPumping".Localized());
                            else
                            {
                                OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.OrgasmDragsSplattering".Localized());

                                if (Player.CumQ() > 1000)
                                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.SeedGushesHer".Localized());

                                if (Player.CumQ() > 5000)
                                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.OthersFrolic".Localized());
                            }
                    }
                    else
                    {
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.DaughtersGiggleSqueal".Localized());

                        if (Player.CumQ() < 100)
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.OrgasmEndsPulsingAgain".Localized());
                        else
                            if (Player.CumQ() < 500)
                                OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.OrgasmDragsPumpingAgain".Localized());
                            else
                            {
                                OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.OrgasmDragsSplatteringAgain".Localized());

                                if (Player.CumQ() > 1000)
                                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.SeedGushesTheirs".Localized());

                                if (Player.CumQ() > 5000)
                                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.OthersFrolicAgain".Localized());
                            }
                    }

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ParagraphBreakFifth".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.SpentLieInDirt".Localized());

                    if (cocks == 1)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ParagraphBreakSixth".Localized());
                    else
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.EachOfLowerAgain".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.CocksRockSolid".Localized());

                    if (cocks == 1)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.PurpleLippedSlut".Localized(
                             Player.CockHead()
                        ));
                    else
                    {
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.PurpleLippedSlutAgain".Localized(
                             Player.CockDescript(1)
                        ));

                        if (cocks > 2)
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.SpreadEveryCock".Localized());

                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.FeelsHeavenly".Localized());
                    }

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.LetterY".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.GoblinServedBetween".Localized());

                    if (Player.BiggestBreastSize() < 1)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ChestThird".Localized());
                    else
                        OutputText(Appearance.AllBreastsDescript(Player));

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.CirclingNipplesMassaging".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ConvulsiveRipples".Localized());

                    if (cocks == 1)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.BlastsWaveSeed".Localized());
                    else
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.BlastWavesSeed".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.DoneInByEffort".Localized());
                }
                else
                {
                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ScuffleBreaksOut".Localized(
                         Player.Mf("male", "herm")
                    ));

                    if (cocks > 1)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.EachOfLower".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.YourCocks".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.HighPitchedVoices".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.SighIntoFragrant".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.MotherAlwaysSaid".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.TamaniTimesHypnotised) >= 10)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.Wife".Localized());
                    else
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.WifeQuoted".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.MockIndignationTaps".Localized());

                    if (cocks > 1)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.EachOfLowerThird".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.YourCocksEyesRoll".Localized(
                         Appearance.CockNoun(BodyPartTypes.Human)
                    ));

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.PullsRingGag".Localized());

                    if (Player.Corruption > 66)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.SighAnticipating".Localized());
                    else
                        if (Player.Corruption > 33)
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.DontResist".Localized());
                        else
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ShudderStruggling".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.PullsVialsPills".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.OkGirlsLineUp".Localized());

                    if (daughters < 20)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.OtherGirlsWaiting".Localized());
                    else
                        if (daughters < 35)
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.LotOfGirls".Localized());
                        else
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.TonOfYou".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.CommandsMother".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.WordsProphetic".Localized(
                         Player.CockHead()
                    ));

                    if (cocks > 2)
                    {
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.OtherCocksWasting".Localized(
                             Appearance.CockNoun(BodyPartTypes.Human)
                        ));

                        if (Player.BiggestBreastSize() < 1)
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ChestFourth".Localized());
                        else
                            OutputText(Appearance.AllBreastsDescript(Player));

                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.CirclingSensitiveNipples".Localized());

                        if (Player.BiggestLactation() >= 1)
                        {
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.AsTheyStart".Localized());

                            if (Player.BiggestLactation() < 2)
                                OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.LeakAgain".Localized());
                            else
                                if (Player.BiggestLactation() < 3)
                                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.DripAgain".Localized());
                                else
                                    if (Player.BiggestLactation() < 5)
                                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.SpewAgain".Localized());
                                    else
                                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.FountainAgain".Localized());

                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.MilkAgain".Localized());
                        }

                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.PerverseSceneFeeds".Localized());
                    }
                    else
                        if (cocks == 2)
                        {
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.OtherCockWasting".Localized(
                                 Appearance.CockNoun(BodyPartTypes.Human)
                            ));

                            if (Player.BiggestBreastSize() < 1)
                                OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ChestFifth".Localized());
                            else
                                OutputText(Appearance.AllBreastsDescript(Player));

                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.CirclingSensitiveNipplesAgain".Localized());

                            if (Player.BiggestLactation() >= 1)
                            {
                                OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.AsTheyStartAgain".Localized());

                                if (Player.BiggestLactation() < 2)
                                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.LeakAgain".Localized());
                                else
                                    if (Player.BiggestLactation() < 3)
                                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.DripAgain".Localized());
                                    else
                                        if (Player.BiggestLactation() < 5)
                                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.SpewAgain".Localized());
                                        else
                                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.FountainAgain".Localized());

                                OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.MilkAgain".Localized());
                            }

                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.PerverseSceneFeedsAgain".Localized());
                        }

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.NextSlutGrabs".Localized(
                         Player.CockHead()
                    ));

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ContinueFertilize".Localized());

                    if (daughters < 20)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.CrowdTakesTime".Localized());
                    else
                        if (daughters < 35)
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.CrowdTakesForever".Localized());
                        else
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.CrowdRationsSemen".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.SoonPregnantStaggerOff".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.TamaniTimesHypnotised) > 10)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.BestWifeEver".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.TamaniTimesHypnotised) > 5)
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.GreatWifeWait".Localized());
                        else
                            OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.IsntThatBad".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.PullsBackKisses".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.OffersCanteen".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.TamaniTimesHypnotised) > 10)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.YourWife".Localized());
                    else
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.Tamani".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.HeadNumb".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.PinkHaze".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.TamaniTimesHypnotised) > 0)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.RememberLastTime".Localized());
                    else
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.SpecialPotion".Localized(
                             Appearance.CockNoun(BodyPartTypes.Human)
                        ));

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.SlidesJigglingBody".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.LetsPinkLippedTouch".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ShesCompletelyRight".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.WordsStopMatteringAgain".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.BallsThird".Localized());
                    else
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.GroinAgain".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.ChurningLustAgain".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.TamaniTimesHypnotised) < 10)
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.WaitWifeMistressAgain".Localized());
                    else
                        OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.YesSoundsRightAgain".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.UntilOrgasmWracksAgain".Localized());

                    OutputText("Scenes.TamanisDaughters.LegTamanisDaughtersRAEPYou.MistressStepsAwayAgain".Localized());

                    KnockUpTamani();

                    CoC.Instance.Flags.Set(GameFlagIds.TamaniTimesHypnotised, CoC.Instance.Flags.Get(GameFlagIds.TamaniTimesHypnotised) + 1);
                }

            Player.Orgasm("DickAndAnal");

            Player.ApplyStats(new DynStat(GameStats.Strength, -.5f), new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Corruption, 1f));

            if (TamaniPresent)
                KnockUpTamani();

            KnockUpDaughters();

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(4));
        }

        public void TamaniDaughtersCombatLossDrain()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedTamanisDaughters, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedTamanisDaughters) + 1);

            var cocks = Player.Cocks.Count;

            float daughters = CoC.Instance.Flags.Get(GameFlagIds.TamaniNumberOfDaughters) / 2;

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.ResistInVain".Localized());

            if (Player.Health < 1)
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.Defeated".Localized());
            else
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.LustWeakened".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.PulledOffFeet".Localized());

            if (Player.Lust >= Player.MaxLust)
            {
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.DoesntBotherTooMuch".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.FondlingBallsAnd".Localized());

                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.StrokingShaft".Localized());
            }
            else
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.BodySlipsConsciousness".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.ParagraphBreak".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.HourLaterCave".Localized());

            if (cocks > 1)
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.OneOf".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.CocksKeepTurnedOn".Localized());

            if (!Player.IsTaur())
            {
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.HandsLowerYou".Localized());

                if (cocks > 1)
                    OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.ThingsNotRestrained".Localized());
                else
                    OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.ThingNotRestrained".Localized());
            }
            else
            {
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.HandsLowerYouAgain".Localized());

                if (cocks > 1)
                    OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.ThingsNotRestrainedAgain".Localized());
                else
                    OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.ThingNotRestrainedAgain".Localized());
            }

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.BlueHairPullsLever".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TamaniDaughterChairCounter) == 0)
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.StopWorryingInvented".Localized());
            else
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.StopWorryingLoveSeat".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.HearCommotion".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.GoblinDeviceSexual".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.ShiverManhood".Localized());

            if (cocks > 1)
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.AreS".Localized());
            else
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.Is".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.TotallyEncapsulated".Localized());

            if (cocks == 1)
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.LetterA".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.WetOrifice".Localized());

            if (cocks > 1)
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.SThird".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.SoCold".Localized());

            if (cocks > 1)
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.SAgain".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.Wilt".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.MachineryLouder".Localized());

            if (cocks > 1)
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.EachOfLower".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.CocksHarderThicker".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.Chest".Localized());
            else
                OutputText(Appearance.AllBreastsDescript(Player));

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.ShovesNippleIn".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.SucklesUnable".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.FamiliarVoiceWhispers".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TamaniDaughterChairCounter) == 0)
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.TimeForMedicine".Localized());
            else
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.RelaxDad".Localized(
                     Player.Mf("stud", "slut")
                ));

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.YouBlush".Localized());

            if (SceneInstance<
                    Npcs.CeraphScene>.Get().HasExhibition())

                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.TurnedOnBeingUsed".Localized());
            else
                if (Player.Corruption > 60)
                    OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.TurnedOnMilked".Localized());
                else
                    OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.HorrifiedButAroused".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.ParagraphBreakAgain".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.WarmFluidFlows".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.BallsTightenSwell".Localized());
            else
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.BodyTightNeedy".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.SucklingPleasure".Localized());

            if (cocks > 1)
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.EachOfLowerAgain".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.CocksIncrease".Localized());

            if (cocks == 1)
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.That".Localized());
            else
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.Those".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.MechanicalHole".Localized());

            if (cocks > 1)
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.SThird".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.UntilRelease".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.GoblinsSmiling".Localized(
                 Player.Mf("laughing", "giggling")
            ));

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.SecondsFromOrgasm".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.BallsClench".Localized());
            else
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.BodyStruggles".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.MachinesWhir".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.StopsBreastFeeding".Localized());

            if (cocks > 1)
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.EachOfLowerThird".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.BrainFlooded".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.AnotherVoiceMoans".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.TrappedConstantOrgasm".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TamaniDaughterChairCounter) < 4)
            {
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.LoseConsciousness".Localized());

                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.WakeAloneClearing".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.TamaniDaughterChairCounter) == 0)
                {
                    OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.HusbandBaby".Localized(
                         Player.Mf("Husband", "Baby")
                    ));

                    OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.AnyIdeaHowHard".Localized());

                    OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.HugsCums".Localized());

                    OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.TamaniI".Localized());
                }
                else
                {
                    OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.SeriouslyNotFunny".Localized(
                         Player.Mf("Man", "Toughen")
                    ));

                    OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.FucksLove".Localized());

                    OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.TamaniIAgain".Localized());
                }

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersCombatLossDrain.LoseConsciousnessAgain".Localized());

                DoNext(() => TamaniDaughtersBadEndChoice());
            }

            CoC.Instance.Flags.Set(GameFlagIds.TamaniDaughterChairCounter, CoC.Instance.Flags.Get(GameFlagIds.TamaniDaughterChairCounter) + 1);

            KnockUpDaughters();

            Player.CumMultiplier += .3f;

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Strength, -.5f), new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Corruption, 1f));
        }

        public void TamaniDaughtersBadEndChoice()
        {

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersBadEndChoice.NextMorningBlink".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersBadEndChoice.GoblinLeansHugs".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 1)
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersBadEndChoice.ExgartuanBarks".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersBadEndChoice.OptionsChoice".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), TamaniDaughtersYesBadEndMePlease);

            AddButton(1, "Common.Button.No".Localized(), TamaniDaughtersDeclineBadEnd);

            AddButton(2, "Scenes.TamanisDaughters.TamaniDaughtersBadEndChoice.ButtonIndividual".Localized(), TamanisDaughtersFillIndividuallyBADEND);
        }

        public void TamaniDaughtersYesBadEndMePlease()
        {
            ClearOutput();

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersYesBadEndMePlease.WonderfulCries".Localized());

            if (Player.Cocks.Count == 1)
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersYesBadEndMePlease.S".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersYesBadEndMePlease.CumIntoTubes".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 1)
                OutputText("Scenes.TamanisDaughters.TamaniDaughtersYesBadEndMePlease.ExgartuanMoans".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersYesBadEndMePlease.LifeTrappedOrgasm".Localized());

            GameOver();
        }

        public void TamaniDaughtersDeclineBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersDeclineBadEnd.SeriouslyExclaims".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersDeclineBadEnd.SheSighs".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersDeclineBadEnd.RestraintsPopOff".Localized());

            OutputText("Scenes.TamanisDaughters.TamaniDaughtersDeclineBadEnd.SheepishlyLeave".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void TamanisDaughtersFillIndividuallyBADEND()
        {
            ClearOutput();

            OutputText("Scenes.TamanisDaughters.TamanisDaughtersFillIndividuallyBADEND.ReallyAsks".Localized());

            OutputText("Scenes.TamanisDaughters.TamanisDaughtersFillIndividuallyBADEND.AgreeToDoThat".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.TamanisDaughters.TamanisDaughtersFillIndividuallyBADEND.EachOfLower".Localized());

            OutputText("Scenes.TamanisDaughters.TamanisDaughtersFillIndividuallyBADEND.RisingFullHardness".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.TamanisDaughters.TamanisDaughtersFillIndividuallyBADEND.OneOfThem".Localized());
            else
                OutputText("Scenes.TamanisDaughters.TamanisDaughtersFillIndividuallyBADEND.It".Localized());

            OutputText("Scenes.TamanisDaughters.TamanisDaughtersFillIndividuallyBADEND.AsIfPerson".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.TamanisDaughters.TamanisDaughtersFillIndividuallyBADEND.LetterA".Localized());
            else
                OutputText("Scenes.TamanisDaughters.TamanisDaughtersFillIndividuallyBADEND.Your".Localized());

            OutputText("Scenes.TamanisDaughters.TamanisDaughtersFillIndividuallyBADEND.DoesntMakeItEasy".Localized());

            OutputText("Scenes.TamanisDaughters.TamanisDaughtersFillIndividuallyBADEND.WellEndowedAnnounces".Localized());

            OutputText("Scenes.TamanisDaughters.TamanisDaughtersFillIndividuallyBADEND.CheerReverberates".Localized());

            OutputText("Scenes.TamanisDaughters.TamanisDaughtersFillIndividuallyBADEND.RestOfYourLife".Localized());

            GameOver();
        }

        public void LoseToDaughtersWithTamaniThere()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedTamanisDaughters, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedTamanisDaughters) + 1);

            int primary = Player.CockThatFits(50);

            OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.AttemptsInVain".Localized());

            if (Player.Health < 1)
                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.LyingInDirtTrembling".Localized());
            else
            {
                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.LyingBackTooHard".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.OneOfAgain".Localized());

                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.TremblingLeak".Localized());
            }

            OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.TamaniPushesFront".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.Chest".Localized());
            else
                OutputText(Appearance.AllBreastsDescript(Player));

            OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.NowHusband".Localized());

            OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.BottlePlug".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TamaniTimesHypnotised) < 2)
                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.SyrupySweet".Localized());
            else
                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.FamiliarTaste".Localized());

            OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.NearlyGag".Localized());

            OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.RubsTemples".Localized());

            OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.ReachesIntoCocks".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.OneOfAgain".Localized());

            OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.CasuallyStroking".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TamaniTimesHypnotised) > 10)
                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.BodyRemembers".Localized());
            else
                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.BodyKnows".Localized());

            OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.SayEnd".Localized());

            OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.OfCourseShesRight".Localized());

            OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.HandStrokesFaster".Localized());

            if (Player.HasFur())
                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.MattingFur".Localized());

            OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.AsSheContinues".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TamaniTimesHypnotised) < 10)
                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.FillingTruths".Localized());
            else
                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.ObedientHusband".Localized());

            OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.ServiceWifesPussy".Localized());

            OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.NoddingWhileSheTalks".Localized());

            if (Player.CumQ() < 50)
                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.CumDripsPuddle".Localized());
            else
                if (Player.CumQ() < 250)
                    OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.CumOnHand".Localized());
                else
                    if (Player.CumQ() < 600)
                        OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.ThickWaves".Localized());
                    else
                    {
                        OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.MassiveWave".Localized());

                        if (Player.CumQ() >= 2000)
                            OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.StopsPoolHuge".Localized());
                    }

            OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.PrideGoodHusband".Localized());

            OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.WelcomeIntoArms".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.EachOfLower".Localized());

            OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.GrowsBackErect".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.GrabAndStroke".Localized());
            else
                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.EachGrabsOne".Localized());

            if (primary < 0)
            {
                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.TooBigToImpregnate".Localized());

                if (Player.CockTotal() == 1)
                {
                    OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.DaughterDoesntMind".Localized());

                    if (Player.CumQ() > 700)
                        OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.GetsEverywhereHer".Localized());

                    if (Player.CumQ() > 2000)
                        OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.PuddleDeeper".Localized());
                }
                else
                {
                    OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.DaughtersDontMind".Localized());

                    if (Player.CumQ() > 700)
                        OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.GetsEverywhereTheir".Localized());

                    if (Player.CumQ() > 2000)
                        OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.PuddleDeeperAgain".Localized());
                }

                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.ParagraphBreak".Localized());

                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.DrainedTwoOrgasms".Localized());
            }
            else
            {
                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.GoblinTwatDevours".Localized());

                if (Player.BiggestCockArea() < 30)
                    OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.InSpiteOfLarge".Localized());
                else
                    OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.LargeSize".Localized());

                if (Player.Cocks.Count > 1)
                {
                    OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.GruntHappily".Localized());

                    if (Player.CumQ() > 700)
                        OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.GetsEverywhereTheirAgain".Localized());

                    if (Player.CumQ() > 2000)
                        OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.PuddleDeeperThird".Localized());
                }
                else
                {
                    OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.GruntHappilyAgain".Localized());

                    if (Player.CumQ() > 700)
                        OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.GetsEverywhereHerAgain".Localized());

                    if (Player.CumQ() > 2000)
                        OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.PuddleDeeperFourth".Localized());
                }

                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.ParagraphBreakAgain".Localized());

                OutputText("Scenes.TamanisDaughters.LoseToDaughtersWithTamaniThere.DrainedTwoOrgasmsAgain".Localized());
            }

            KnockUpDaughters();

            KnockUpTamani();

            CoC.Instance.Flags.Set(GameFlagIds.TamaniTimesHypnotised, CoC.Instance.Flags.Get(GameFlagIds.TamaniTimesHypnotised) + 1);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Strength, -.5f), new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, 1f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void KnockUpDaughters()
        {

            if (DaughtersPregnant)
                return ;

            CoC.Instance.Flags.Set(GameFlagIds.TamaniDaughtersPregnancyType, PlayerFatherPregnancyType);

            CoC.Instance.Flags.Set(GameFlagIds.TamaniDaughterPreggoCountdown, DaughterPregnancyIncubation);

            CoC.Instance.Flags.Set(GameFlagIds.TamaniDaughtersPregnancyCount, 2);

            var cum = Player.CumQ();

            if (Player.HasPerk(PerkIds.MaraesGiftStud))
                CoC.Instance.Flags.Set(GameFlagIds.TamaniDaughtersPregnancyCount, CoC.Instance.Flags.Get(GameFlagIds.TamaniDaughtersPregnancyCount) + 3);

            if (cum >= 50 && Random.Shared.Next(2) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.TamaniDaughtersPregnancyCount, CoC.Instance.Flags.Get(GameFlagIds.TamaniDaughtersPregnancyCount) + 1);

            if (cum >= 100 && Random.Shared.Next(2) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.TamaniDaughtersPregnancyCount, CoC.Instance.Flags.Get(GameFlagIds.TamaniDaughtersPregnancyCount) + 1);

            if (cum >= 200 && Random.Shared.Next(2) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.TamaniDaughtersPregnancyCount, CoC.Instance.Flags.Get(GameFlagIds.TamaniDaughtersPregnancyCount) + 1);

            if (cum >= 300 && Random.Shared.Next(2) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.TamaniDaughtersPregnancyCount, CoC.Instance.Flags.Get(GameFlagIds.TamaniDaughtersPregnancyCount) + 1);

            if (cum >= 400 && Random.Shared.Next(2) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.TamaniDaughtersPregnancyCount, CoC.Instance.Flags.Get(GameFlagIds.TamaniDaughtersPregnancyCount) + 1);

            if (cum >= 500 && Random.Shared.Next(2) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.TamaniDaughtersPregnancyCount, CoC.Instance.Flags.Get(GameFlagIds.TamaniDaughtersPregnancyCount) + 1);

            if (cum >= 600 && Random.Shared.Next(2) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.TamaniDaughtersPregnancyCount, CoC.Instance.Flags.Get(GameFlagIds.TamaniDaughtersPregnancyCount) + 1);
        }

        public void CombatWinAgainstDaughters()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Health < 1)
            {
                OutputText("Scenes.TamanisDaughters.CombatWinAgainstDaughters.SmileSatisfaction".Localized());

                if (Player.Lust >= 33 && Player.CockTotal() > 0)
                {
                    OutputText("Scenes.TamanisDaughters.CombatWinAgainstDaughters.InSpiteInjuries".Localized());

                    OutputText("Scenes.TamanisDaughters.CombatWinAgainstDaughters.DoYouFuckThemAgain".Localized());

                    AddButton(0, "Common.Button.FuckThem".Localized(), FuckYoDaughtersHomie);

                    AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
                }
                else
                    CombatScene.CleanupAfterCombat();

                return ;
            }
            else
            {
                OutputText("Scenes.TamanisDaughters.CombatWinAgainstDaughters.SmileSatisfactionAgain".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

                if (Player.Lust >= 33 && Player.CockTotal() > 0)
                {
                    OutputText("Scenes.TamanisDaughters.CombatWinAgainstDaughters.CouldStillFuck".Localized());

                    OutputText("Scenes.TamanisDaughters.CombatWinAgainstDaughters.DoYouFuckThemAgain".Localized());

                    AddButton(0, "Common.Button.Yes".Localized(), FuckYoDaughtersHomie);

                    AddButton(1, "Common.Button.No".Localized(), CombatScene.CleanupAfterCombat);
                }
                else
                    CombatScene.CleanupAfterCombat();

                return ;
            }
        }

        public void LoseToDaughters()
        {

            if (Player.Lust >= Player.MaxLust)
            {
                if (Player.HasStatusEffect(StatusEffectIds.Infested))
                {

                    ClearOutput();

                    OutputText("Scenes.Wormsscene.InfestOrgasm.CeaselessSquirming".Localized());

                    Player.Orgasm("Dick");

                    if (Player.HasStatusEffect(StatusEffectIds.Infested) &&
                        Player.StatusEffectValue(StatusEffectIds.Infested, 0) < 5)
                    {
                        Player.ChangeStatusValue(StatusEffectIds.Infested, 0, 1);

                        Player.CumMultiplier += 0.5f;

                        if (Player.StatusEffectValue(StatusEffectIds.Infested, 0) == 5)
                            if (Player.Balls == 0)
                                OutputText("Scenes.Wormsscene.InfestOrgasm.AfterEmptying".Localized());
                            else
                            {
                                OutputText("Scenes.Wormsscene.InfestOrgasm.AwkwardSensation".Localized());

                                Player.BallSize += 2;

                                Player.CumMultiplier += 1;
                            }
                    }

                    OutputText("Scenes.TamanisDaughters.LoseToDaughters.GoblinsSighGross".Localized());

                    Player.Orgasm("Dick");
                }

                OutputText("Scenes.TamanisDaughters.LoseToDaughters.GiveUpTooTurnedOn".Localized());

                if (Player.CockTotal() == 0)
                {
                    OutputText("Scenes.TamanisDaughters.LoseToDaughters.SexySlutsPout".Localized());

                    CombatScene.CleanupAfterCombat();

                    return ;
                }

                if (TamaniPresent)
                {
                    if (Random.Shared.Next(2) == 0)
                        LoseToDaughtersWithTamaniThere();
                    else
                        LegTamanisDaughtersRAEPYou();

                    return ;
                }
                else
                {
                    if (Random.Shared.Next(2) == 0)
                        TamaniDaughtersCombatLossDrain();
                    else
                        LegTamanisDaughtersRAEPYou();

                    return ;
                }
            }
            else
            {
                OutputText("Scenes.TamanisDaughters.LoseToDaughters.OverwhelmedWounds".Localized());

                if (Player.CockTotal() == 0)
                {
                    OutputText("Scenes.TamanisDaughters.LoseToDaughters.SexySlutsPoutAgain".Localized());

                    CombatScene.CleanupAfterCombat();

                    return ;
                }

                if (TamaniPresent)
                {
                    LoseToDaughtersWithTamaniThere();

                    return ;
                }
                else
                {
                    if (Random.Shared.Next(2) == 0)
                        TamaniDaughtersCombatLossDrain();
                    else
                        LegTamanisDaughtersRAEPYou();

                    return ;
                }
            }
        }

    }
}