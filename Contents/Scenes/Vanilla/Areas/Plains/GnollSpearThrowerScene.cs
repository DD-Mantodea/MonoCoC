using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Contents.Enemies;
using System.Collections.Generic;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Plains
{
    [Register<GnollSpearThrowerScene>("coc:gnoll_spear_thrower")]
    public class GnollSpearThrowerScene : Scene
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
            Gnoll2Encounter();
        }

        public void Gnoll2Encounter()
        {
            ClearOutput();

            ShowImage("monster-gnollspearthrower");

            if (CoC.Instance.Flags.Get(GameFlagIds.HaveEncounteredGnollPlains) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.HaveEncounteredGnollPlains, CoC.Instance.Flags.Get(GameFlagIds.HaveEncounteredGnollPlains) + 1);

                OutputText("Scenes.Gnollspearthrowerscene.Gnoll2Encounter.WavesDryGrass".Localized());

                OutputText("Scenes.Gnollspearthrowerscene.Gnoll2Encounter.OnlyWarning".Localized());

                if (Player.Tallness < 77)
                    OutputText("Scenes.Gnollspearthrowerscene.Gnoll2Encounter.TowersOver".Localized());
                else
                    OutputText("Scenes.Gnollspearthrowerscene.Gnoll2Encounter.StandsProudly".Localized());

                OutputText("Scenes.Gnollspearthrowerscene.Gnoll2Encounter.SpottedPelt".Localized());

                OutputText("Scenes.Gnollspearthrowerscene.Gnoll2Encounter.GrowlSnaps".Localized((noFur ? "skin" : "fur")));
            }
            else
            {
                OutputText("Scenes.Gnollspearthrowerscene.Gnoll2Encounter.BarelyRegisterWaves".Localized());

                OutputText("Scenes.Gnollspearthrowerscene.Gnoll2Encounter.StrainsEars".Localized());

                if (Player.Tallness < 77)
                    OutputText("Scenes.Gnollspearthrowerscene.Gnoll2Encounter.TowersOverAgain".Localized());
                else
                    OutputText("Scenes.Gnollspearthrowerscene.Gnoll2Encounter.StandsProudlyAgain".Localized());

                OutputText("Scenes.Gnollspearthrowerscene.Gnoll2Encounter.SpottedPeltAgain".Localized());

                OutputText("Scenes.Gnollspearthrowerscene.Gnoll2Encounter.GrowlSnapsAgain".Localized((noFur ? "skin" : "fur")));
            }

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.GnollSpearThrower), () => Go<CampScene>());
        }

        public void HyenaSpearLossAnal()
        {
            ClearOutput();

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubusDream)))

                return ;

            OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.GnollLaughs".Localized());

            if (Player.Health < 1)
                OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.BruisedBattered".Localized());
            else
                OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.ArousalCoursing".Localized());

            OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.JoltOfFear".Localized());

            if (!Player.Weapon.IsUnarmed())
                OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.BeforeRecover".Localized());
            else
                OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.PullsArmor".Localized());

            OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.ParagraphBreak".Localized());

            OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.SpottedPaws".Localized());

            OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.MockingLaughter".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.PantingFeel".Localized());
            else
                if (Player.IsGoo())
                    OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.GnollPullsAway".Localized());
                else
                    if (!Player.IsTaur())
                    {
                        OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.PantingLook".Localized());

                        if (Player.Balls > 0)
                            OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.ClampsSack".Localized());
                        else
                            if (Player.HasVagina())
                                OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.GrabsClit".Localized());
                            else
                                OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.PlungesAsshole".Localized());

                        OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.CriesOutTied".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.PantingLookAgain".Localized());

                        if (Player.Balls > 0)
                            OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.ClampsSackAgain".Localized());
                        else
                            if (Player.HasVagina())
                                OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.GrabsClitAgain".Localized());
                            else
                                OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.PlungesAssholeAgain".Localized());

                        OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.CriesOutLift".Localized());
                    }

            OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.SharpTalons".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.PawWraps".Localized());
            else
                OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.PawDisappears".Localized());

            OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.SoftWhimper".Localized());

            ShowImage("gnoll-fuck-butt");

            OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.ClawedHands".Localized());

            Player.ButtChange(30, true, true, false);

            OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.AirFillsSound".Localized());

            OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.MuscledForm".Localized((noFur ? "Tough skin" : "Coarse fur")));

            OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.BeginsPiston".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.ShaftSlides".Localized());
            else
                OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.EachLunge".Localized());

            OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.TimeLosesMeaning".Localized());

            OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.PowerfulBody".Localized());

            OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.WarmWeight".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.CumSpurts".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.OwnJuices".Localized());

            OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.MusclesRipple".Localized());

            OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.PullsOutGrunt".Localized());

            if ((int)Player.Gender > 0)
                OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.FallsHard".Localized());
            else
                OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.FallsHardAgain".Localized());

            OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.SlowlyFades".Localized());

            if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Goo)
                OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.EyesSlideClosed".Localized());

            OutputText("Scenes.Gnollspearthrowerscene.HyenaSpearLossAnal.LastThingHear".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

            CombatScene.CleanupAfterCombat();
        }

        public void HyenaVictory()
        {
            ClearOutput();

            OutputText("Scenes.Gnollspearthrowerscene.HyenaVictory.DrawsJavelin".Localized());

            if (CombatController.Instance!.Context.Enemy.Health <= 0)
                OutputText("Scenes.Gnollspearthrowerscene.HyenaVictory.DazedWord".Localized());
            else
                OutputText("Scenes.Gnollspearthrowerscene.HyenaVictory.ArousedWord".Localized());

            OutputText("Scenes.Gnollspearthrowerscene.HyenaVictory.ToFightFurther".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Kitsune.DefeatTheKitsunes.ButtonGetBJ".Localized(), null);

            AddButton(1, "Common.Button.GetLicked".Localized(), null);

            if (Player.Lust >= 33 && !Player.IsGenderless())
            {
                OutputText("Scenes.Gnollspearthrowerscene.HyenaVictory.UsingHyena".Localized());

                if (Player.HasCock())
                    AddButton(0, "Scenes.Kitsune.DefeatTheKitsunes.ButtonGetBJ".Localized(), HyenaVictoryRapeFellatio);

                if (Player.HasVagina())
                    AddButton(1, "Common.Button.GetLicked".Localized(), VictoryRapeHyenaCunnilingus);
            }

            AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
        }

        public void HyenaVictoryRapeFellatio()
        {
            ClearOutput();

            var x = Player.CockThatFits(40);

            if (x < 0)
                x = 0;

            var y= Player.CockThatFits2(40);

            if (Player.CockTotal() > 1 && (y < 0 || y == x))
            {
                y = 0;

                if (y == x)
                    y = 1;
            }

            ShowImage("gnoll-fuck-oral");

            OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.SensingWill".Localized((noFur ? "its" : "a furred")));

            OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.DarkEyesWatch".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.WithAStart".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.HeadPresses".Localized());

            if (Player.Cocks[x].Length > 7)
                OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.TipPresses".Localized(
                     Player.CockDescript(x)
                ));

            OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.WarmNose".Localized());

            if (Player.Cocks[x].Length > 7)
                OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.BackOfMouth".Localized());

            OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.StartsPulled".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.IncredibleSensations".Localized());
            else
                if (Player.CockTotal() == 2)
                {
                    OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.IncredibleSensationsAgain".Localized(
                         Player.CockDescript(y)
                    ));

                    if (Player.Balls > 0)
                        OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.SecondPawCups".Localized());

                    OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.ParagraphBreak".Localized());
                }

            if (Player.Cocks.Count > 2)
            {
                OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.IncredibleSensationsThird".Localized(
                     Player.CockDescript(y)
                ));

                if (Player.CockTotal() == 3)
                    OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.YourOtherPrefix".Localized());
                else
                    OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.AnotherPrefix".Localized());

                OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.FreeMember".Localized());
            }

            OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.WavesPleasure".Localized());

            if (Player.Cocks[x].Length >= 7)
                OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.LipsPullAway".Localized(
                     Player.CockDescript(x)
                ));

            OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.StartsGrowl".Localized());

            if (Player.Cocks[x].Length >= 7)
            {
                OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.OrgasmCrashes".Localized(
                     Player.CockDescript(x)
                ));

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.SecondThrobs".Localized(
                         Player.CockDescript(y)
                    ));

                if (Player.CockTotal() >= 3)
                    OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.ThrobInPaws".Localized());

                OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.KneesWeaken".Localized());
            }
            else
            {
                OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.OrgasmCrashesAgain".Localized(
                     Player.CockDescript(x)
                ));

                if (Player.Cocks.Count == 2)
                    OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.ThrobsInHand".Localized(
                         Player.CockDescript(y)
                    ));

                if (Player.CockTotal() >= 3)
                    OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.ThrobInPawsAgain".Localized());

                OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.KneesWeakenAgain".Localized());
            }

            OutputText("Scenes.Gnollspearthrowerscene.HyenaVictoryRapeFellatio.WorldGoesGray".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void VictoryRapeHyenaCunnilingus()
        {
            ClearOutput();

            OutputText("Scenes.Gnollspearthrowerscene.VictoryRapeHyenaCunnilingus.DarkEyesApproach".Localized());

            OutputText("Scenes.Gnollspearthrowerscene.VictoryRapeHyenaCunnilingus.SlowlyDawns".Localized((noFur ? "each topped with a hard, black nipple" : "covered in soft fur with a black nipple poking through the cream colored pelt")));

            OutputText("Scenes.Gnollspearthrowerscene.VictoryRapeHyenaCunnilingus.PullingAway".Localized((noFur ? "its" : "a furred")));

            OutputText("Scenes.Gnollspearthrowerscene.VictoryRapeHyenaCunnilingus.HeadyMusk".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Gnollspearthrowerscene.VictoryRapeHyenaCunnilingus.OnImpulse".Localized());

            ShowImage("gnoll-fuck-cunny");

            OutputText("Scenes.Gnollspearthrowerscene.VictoryRapeHyenaCunnilingus.WetPinkTongue".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Gnollspearthrowerscene.VictoryRapeHyenaCunnilingus.LostInBuilding".Localized((noFur ? "of her mouth" : "the front of her muzzle")));

            OutputText("Scenes.Gnollspearthrowerscene.VictoryRapeHyenaCunnilingus.PressureIncreases".Localized());

            Player.CuntChange(8, true, true, false);

            OutputText("Scenes.Gnollspearthrowerscene.VictoryRapeHyenaCunnilingus.ParagraphBreak".Localized());

            OutputText("Scenes.Gnollspearthrowerscene.VictoryRapeHyenaCunnilingus.LiftsYouOff".Localized());

            OutputText("Scenes.Gnollspearthrowerscene.VictoryRapeHyenaCunnilingus.FinalAction".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Gnollspearthrowerscene.VictoryRapeHyenaCunnilingus.AwakeLater".Localized());

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();
        }

    }
}