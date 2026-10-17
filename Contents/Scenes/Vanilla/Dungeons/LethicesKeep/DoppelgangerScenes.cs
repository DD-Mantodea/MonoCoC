using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep
{

    [Register<DoppelgangerScenes>("coc:doppelganger_scene")]
    public class DoppelgangerScenes : Scene
    {

        public override void Entry()
        {
        }

        [Register("coc:keep_hook_glasses")]
        public static void KeepHookGlasses() => SceneInstance<DoppelgangerScenes>.Get().GetDemGlasses();

        public void GetDemGlasses()
        {
            ClearOutput();

            OutputText("Scenes.Doppelgangerscenes.GetDemGlasses.StepsIntoRoom".Localized());

            OutputText("Scenes.Doppelgangerscenes.GetDemGlasses.TurnsAway".Localized());

            if (Player.HumanScore() > 4)
                OutputText("Scenes.Doppelgangerscenes.GetDemGlasses.HonestlySurprising".Localized());
            else
                OutputText("Scenes.Doppelgangerscenes.GetDemGlasses.LastTimeLooked".Localized());

            OutputText("Scenes.Doppelgangerscenes.GetDemGlasses.ContinuesStaring".Localized());

            OutputText("Scenes.Doppelgangerscenes.GetDemGlasses.VoiceSoundsLike".Localized());

            OutputText("Scenes.Doppelgangerscenes.GetDemGlasses.RightAtFront".Localized());

            OutputText("Scenes.Doppelgangerscenes.GetDemGlasses.ThereWeGo".Localized());

            OutputText("Scenes.Doppelgangerscenes.GetDemGlasses.TakenYourPlace".Localized());

            OutputText("Scenes.Doppelgangerscenes.GetDemGlasses.MustFightYourself".Localized());

            CombatScene.Start(
                EnemyRegistry.Create(new Identifier("coc", "doppelganger")),

                RoomDungeon.ResumeFromFight);
        }

        public void PunchYourselfInTheBalls()
        {
            Flags.Set(GameFlagIds.D3DoppelgangerDefeated, 1);

            Player.CreateKeyItem("Laybans", 0, 0, 0, 0);

            ClearOutput();

            OutputText("Scenes.Doppelgangerscenes.PunchYourselfInTheBalls.DoppelFalls".Localized());

            OutputText("Scenes.Doppelgangerscenes.PunchYourselfInTheBalls.MirrorSnivels".Localized());

            ClearButtons();

            if (!Player.IsTaur() && (Player.HasCock() || Player.HasVagina()))
                AddButton(0, "Common.Button.OhReally".Localized(), FuckYourself);

            AddButton(1, "Common.Button.EndIt".Localized(), KillYourself);
        }

        public void FuckYourself()
        {
            ClearOutput();

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.ThisIsOpportunity".Localized());

            var hasVirginVagina = Player.HasVagina() && Player.Vaginas[0].Virgin;

            OutputText(hasVirginVagina

                ? "Scenes.Doppelgangerscenes.FuckYourself.SmilesWidely".Localized()

                : "Scenes.Doppelgangerscenes.FuckYourself.SmilesWidelyAgain".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.MoansDisbelief".Localized());

            if (Player.Vaginas.Count == 0 && Player.Cocks.Count == 0)
            {
                OutputText("Scenes.Doppelgangerscenes.FuckYourself.StrangelyMore".Localized());

                Player.CreateVagina();
            }

            if (Player.Cocks.Count >= 1 && !Player.HasVagina())
                FuckYourselfMale();
            else if (Player.HasCock() && Player.HasVagina())
                FuckYourselfHerm();
            else if (Player.HasVagina() && !Player.HasCock())
                FuckYourselfFemale();

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.KeyItemAcquiredBanner".Localized());

            Flags.Set(GameFlagIds.D3MirrorsShattered, 1);

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();

            DoNext(RoomDungeon.ResumeFromFight);
        }

        public void FuckYourselfMale()
        {
            OutputText("Scenes.Doppelgangerscenes.FuckYourself.TurnsMirrorDemon".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Doppelgangerscenes.FuckYourself.SighsSmooth".Localized());
            else if (Player.IsDrider())
                OutputText("Scenes.Doppelgangerscenes.FuckYourself.TangleLegs".Localized());
            else if (Player.HasFur() && Player.Body.Get<TailPart>().Id.Path != "none")
                OutputText("Scenes.Doppelgangerscenes.FuckYourself.ClambersOnTop".Localized());
            else
                OutputText("Scenes.Doppelgangerscenes.FuckYourself.ClambersEnjoying".Localized(FleshWord()));

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.CannotBelieve".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.ShutsUp".Localized());

            OutputText(Player.Body.Get<AnalPart>().Wetness > 3

                ? "Scenes.Doppelgangerscenes.FuckYourself.NoProblemClone".Localized()

                : "Scenes.Doppelgangerscenes.FuckYourself.WellTough".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.GoesSlowly".Localized());

            if (Player.HasPerk(PerkIds.Buttslut) || Player.HasPerk(PerkIds.MaraesGiftButtslut))
                OutputText("Scenes.Doppelgangerscenes.FuckYourself.GloryInDecision".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.LockedInto".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.HooksArm".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.RaspsGlee".Localized(NippleState()));

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.WordlessSound".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.WrapsHand".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.HoldCloseStill".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.CrackingMirror".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.GrowlsTaunt".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.BuriesYourself".Localized());

            OutputText(Player.CumQ() >= 1000

                ? "Scenes.Doppelgangerscenes.FuckYourself.KnowsExpect".Localized()

                : "Scenes.Doppelgangerscenes.FuckYourself.RidesHigh".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.ShatteringSound".Localized());
        }

        public void FuckYourselfHerm()
        {
            OutputText("Scenes.Doppelgangerscenes.FuckYourself.GazeDrops".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Doppelgangerscenes.FuckYourself.SighsSmoothAgain".Localized());

            if (Player.IsDrider())
                OutputText("Scenes.Doppelgangerscenes.FuckYourself.ConsidersMounting".Localized());

            if (Player.HasFur())
                OutputText("Scenes.Doppelgangerscenes.FuckYourself.ClambersOnTopAgain".Localized());
            else
                OutputText("Scenes.Doppelgangerscenes.FuckYourself.ClambersEnjoyingAgain".Localized(FleshWord()));

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.CannotBelieveAgain".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.ShutsHimUp".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Doppelgangerscenes.FuckYourself.SWord".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.BattingBack".Localized());

            OutputText(Player.AverageVaginalWetness() < 3

                ? "Scenes.Doppelgangerscenes.FuckYourself.GleamingInvitingly".Localized()

                : "Scenes.Doppelgangerscenes.FuckYourself.DroolingLubricant".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.AsKnewItWould".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.GoesSlowlyAgain".Localized());

            if (Player.HasVagina() && Player.Vaginas[0].Virgin)
                OutputText("Scenes.Doppelgangerscenes.FuckYourself.GruntGivesIn".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.InitiallyFrozen".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.PlacesPalm".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.RaspsGleeAgain".Localized(NippleState()));

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.WordlessSoundAgain".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.GrinWidening".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.HoldCloseOn".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.CrackingMirrorAgain".Localized(
                Player.HasFuckableNipples() ? "beading nipple" : "needily erect"));

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.GrowlsTauntAgain".Localized());

            OutputText(Player.CumQ() >= 1000

                ? "Scenes.Doppelgangerscenes.FuckYourself.KnowsExpectAgain".Localized()

                : "Scenes.Doppelgangerscenes.FuckYourself.RidesHighFull".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.ShatteringSoundAgain".Localized());
        }

        public void FuckYourselfFemale()
        {
            OutputText("Scenes.Doppelgangerscenes.FuckYourself.GazeDropsAgain".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Doppelgangerscenes.FuckYourself.SighsSmoothThird".Localized());
            else if (Player.IsDrider())
                OutputText("Scenes.Doppelgangerscenes.FuckYourself.MomentarilyThrown".Localized());
            else if (Player.HasFur())
                OutputText("Scenes.Doppelgangerscenes.FuckYourself.ClambersOnTopThird".Localized());
            else
                OutputText("Scenes.Doppelgangerscenes.FuckYourself.ClambersEnjoyingThird".Localized(FleshWord()));

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.CannotBelieveThird".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.CantCriticize".Localized());

            OutputText(Player.AverageVaginalWetness() < 3

                ? "Scenes.Doppelgangerscenes.FuckYourself.GleamingInvitinglyAgain".Localized()

                : "Scenes.Doppelgangerscenes.FuckYourself.DroolingLubricantAgain".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.KnewItWouldAgain".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.GoesSlowlyAbdomen".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.PlacesPalmAgain".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.RaspsGleeThird".Localized(NippleState()));

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.WordlessSoundThird".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.GrinDelicately".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.HoldCloseOnAgain".Localized());

            OutputText(Player.Thickness > 75 || Player.Tone < 25

                ? "Scenes.Doppelgangerscenes.FuckYourself.SharedJigglyFlesh".Localized()

                : "Scenes.Doppelgangerscenes.FuckYourself.SharedMuscledFlesh".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.PushesClitIn".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.CrackingMirrorThird".Localized(
                Player.HasFuckableNipples() ? "beading" : "erect"));

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.GrowlsTauntThird".Localized());

            OutputText(Player.CumQ() > 1000

                ? "Scenes.Doppelgangerscenes.FuckYourself.KnowsExpectThird".Localized()

                : "Scenes.Doppelgangerscenes.FuckYourself.RidesHighFemale".Localized());

            OutputText("Scenes.Doppelgangerscenes.FuckYourself.ShatteringSoundThird".Localized());
        }

        public string NippleState() => Player.HasFuckableNipples() ? "wetting itself needily" : "standing to a needy point";

        public string FleshWord() => Player.IsGoo() ? "gooey" : "smooth";

        public void KillYourself()
        {
            ClearOutput();

            OutputText("Scenes.Doppelgangerscenes.KillYourself.HoldsItsGaze".Localized());

            Flags.Set(GameFlagIds.D3MirrorsShattered, 1);

            CombatScene.CleanupAfterCombat();

            DoNext(RoomDungeon.ResumeFromFight);
        }

        public void InSovietCoCSelfFucksYou()
        {
            ClearOutput();

            OutputText("Scenes.Doppelgangerscenes.InSovietCoCSelfFucksYou.FeelsVague".Localized());

            OutputText("Scenes.Doppelgangerscenes.InSovietCoCSelfFucksYou.BreatheThings".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Doppelgangerscenes.InSovietCoCSelfFucksYou.TurgidCock".Localized());
            else if (Player.HasVagina())
                OutputText("Scenes.Doppelgangerscenes.InSovietCoCSelfFucksYou.VaginaFeels".Localized());
            else
            {
                OutputText("Scenes.Doppelgangerscenes.InSovietCoCSelfFucksYou.TheWayYourPrefix".Localized());

                Player.CreateVagina();

                OutputText("Scenes.Doppelgangerscenes.InSovietCoCSelfFucksYou.FeelsSlideFingers".Localized());
            }

            if (Player.HasCock() && !Player.HasVagina())
            {
                OutputText("Scenes.Doppelgangerscenes.InSovietCoCSelfFucksYou.MasturbateForced".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Doppelgangerscenes.InSovietCoCSelfFucksYou.OtherCocksTighten".Localized());
            }
            else if (Player.HasCock() && Player.HasVagina())
            {
                OutputText("Scenes.Doppelgangerscenes.InSovietCoCSelfFucksYou.MasturbateForcedAgain".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Doppelgangerscenes.InSovietCoCSelfFucksYou.OtherCocksTightenAgain".Localized());
            }
            else if (!Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Doppelgangerscenes.InSovietCoCSelfFucksYou.MasturbateForcedThird".Localized());

            OutputText("Scenes.Doppelgangerscenes.InSovietCoCSelfFucksYou.MouthSaysFinally".Localized());

            OutputText("Scenes.Doppelgangerscenes.InSovietCoCSelfFucksYou.BodyBrainFull".Localized());

            OutputText("Scenes.Doppelgangerscenes.InSovietCoCSelfFucksYou.GoesInsane".Localized());

            GameOver();
        }
    }
}