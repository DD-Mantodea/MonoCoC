using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System.Linq;

namespace MonoCoC.Contents.Scenes.Vanilla
{

    [Register<TrainingDummyScene>("coc:training_dummy_scene")]
    public class TrainingDummyScene : Scene
    {
        public override void Entry() => DummyMenu();

        public void DummyMenu(bool output = false)
        {
            ClearOutput();

            if (output)
                OutputText("Scenes.Trainingdummyscene.DummyMenu.WalksToDummy".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Train".Localized(), DummyFight, "Test your [weapon] against this piece of wood.");

            AddButton(1, "CharCreation.AscensionMenu.Button5".Localized(), RenameDummy, "Change its name.");

            AddButton(2, "Common.Button.Destroy".Localized(), DestroyDummy, "Destroy the thing. You can always build a new one, if you want.");

            AddButton(14, "Common.Back".Localized(), CampScene.CampActions);
        }

        public void DummyFight()
        {
            var monster = EnemyRegistry.Create(new Identifier("coc", "training_dummy"));

            CombatScene.Start(monster, () => Go<CampScene>());
        }

        public void RenameDummy()
        {
            ClearOutput();

            OutputText("Scenes.Trainingdummyscene.RenameDummy.NewNamePrompt".Localized());

            ClearButtons();

            GenericNamePrompt();

            AddButton(0, "Common.Button.Confirm".Localized(), () => TrainingDummyCamp.NameDummy(true));
        }

        public void DestroyDummy()
        {
            ClearOutput();

            OutputText("Scenes.Trainingdummyscene.DestroyDummy.DecidesToDiscard".Localized());

            OutputText("Scenes.Trainingdummyscene.DestroyDummy.PriesFree".Localized());

            TrainingDummyCamp.DummyBuilt = false;

            TrainingDummyCamp.DummyGender = 0;

            TrainingDummyCamp.DummyName = "";

            Go<CampScene>();
        }

        public void DummyLost()
        {
            ClearOutput();

            OutputText("Scenes.Trainingdummyscene.DummyLost.DummyFallsOver".Localized());

            ClearButtons();

            AddNextButton("Common.Button.SetUp".Localized(), DummySetUp);

            if (TrainingDummyCamp.DummyGender >= 2)
                SexButton(AddNextButton("Common.Button.Titfuck".Localized(), DummyTitfuck), Gender.Male);
            else
                AddNextButtonDisabled("Titfuck", "The dummy doesn't have tits.");

            if (TrainingDummyCamp.DummyGender % 2 == 1)
                SexButton(AddNextButton("Common.Button.Ride".Localized(), DummyRide), Gender.Female);
            else
                AddNextButtonDisabled("Ride", "The dummy doesn't have a dick.");

            if (TrainingDummyCamp.DummyGender % 2 == 1)
                AddNextButton("Common.Button.AnalRide".Localized(), DummyAnalRide);
            else
                AddNextButtonDisabled("Anal Ride", "The dummy doesn't have a dick.");
        }

        public void DummyWon(bool hpVictory)
        {
            ClearOutput();

            if (hpVictory)
            {
                OutputText("Scenes.Trainingdummyscene.DummyWon.DefeatedByObject".Localized());

                OutputText("Scenes.Trainingdummyscene.DummyWon.PassesOut".Localized());

                CombatScene.CleanupAfterCombat();

                return;
            }

            OutputText("Scenes.Trainingdummyscene.DummyWon.DropsWeaponPanting".Localized());

            ClearButtons();

            if (TrainingDummyCamp.DummyGender >= 2)
                SexButton(AddNextButton("Common.Button.Titfuck".Localized(), DummyTitfuck), Gender.Male);
            else
                AddNextButtonDisabled("Titfuck", "The dummy doesn't have tits.");

            if (TrainingDummyCamp.DummyGender % 2 == 1)
                SexButton(AddNextButton("Common.Button.Ride".Localized(), DummyRide), Gender.Female);
            else
                AddNextButtonDisabled("Ride", "The dummy doesn't have a dick.");

            if (TrainingDummyCamp.DummyGender % 2 == 1)
                AddNextButton("Common.Button.AnalRide".Localized(), DummyAnalRide);
            else
                AddNextButtonDisabled("Anal Ride", "The dummy doesn't have a dick.");

            if (TrainingDummyCamp.DummyGender % 2 == 0 && (!Player.HasCock() || TrainingDummyCamp.DummyGender == 0))
                AddNextButton("Common.Button.Faint".Localized(), DummyFaint);
        }

        public void DummyTitfuck()
        {
            ClearOutput();

            OutputText("Scenes.Trainingdummyscene.DummyTitfuck.FlopsDick".Localized());

            OutputText("Scenes.Trainingdummyscene.DummyTitfuck.SkipsForeplay".Localized());

            OutputText("Scenes.Trainingdummyscene.DummyTitfuck.TimberChafes".Localized());

            OutputText("Scenes.Trainingdummyscene.DummyTitfuck.ClimaxApproaches".Localized());

            OutputText("Scenes.Trainingdummyscene.DummyTitfuck.StainsDummy".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void DummyRide()
        {
            ClearOutput();

            OutputText("Scenes.Trainingdummyscene.DummyRide.MasturbationRationale".Localized());

            if (Player.Vaginas.Any(vagina => vagina.Virgin))
            {
                OutputText("Scenes.Trainingdummyscene.DummyRide.FirstPenetration".Localized());

                OutputText("Scenes.Trainingdummyscene.DummyRide.StrangestFeeling".Localized());
            }
            else
                OutputText("Scenes.Trainingdummyscene.DummyRide.AlignsEntrance".Localized());

            Player.CuntChange(5.5f, true, true);

            OutputText("Scenes.Trainingdummyscene.DummyRide.SlowDeliberate".Localized());

            OutputText("Scenes.Trainingdummyscene.DummyRide.RidesHard".Localized(
                TrainingDummyCamp.DummyGender >= 2

                    ? "Scenes.Trainingdummyscene.DummyRide.GropeBreasts".Localized()

                    : "Scenes.Trainingdummyscene.DummyRide.SteadyChest".Localized()));

            OutputText("Scenes.Trainingdummyscene.DummyRide.BuriesDeep".Localized());

            OutputText("Scenes.Trainingdummyscene.DummyRide.ExtractsShaft".Localized());

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();
        }

        public void DummyAnalRide()
        {
            ClearOutput();

            OutputText("Scenes.Trainingdummyscene.DummyAnalRide.MasturbationRationale".Localized());

            OutputText("Scenes.Trainingdummyscene.DummyAnalRide.AlignsButthole".Localized());

            OutputText("Scenes.Trainingdummyscene.DummyAnalRide.SphincterDrag".Localized());

            OutputText("Scenes.Trainingdummyscene.DummyAnalRide.RidesHard".Localized(
                TrainingDummyCamp.DummyGender >= 2

                    ? "Scenes.Trainingdummyscene.DummyRide.GropeBreasts".Localized()

                    : "Scenes.Trainingdummyscene.DummyRide.SteadyChest".Localized()));

            OutputText("Scenes.Trainingdummyscene.DummyAnalRide.BuriesDeep".Localized());

            OutputText("Scenes.Trainingdummyscene.DummyAnalRide.ExtractsShaft".Localized());

            Player.Orgasm("Anal");

            CombatScene.CleanupAfterCombat();
        }

        public void DummyFaint()
        {
            ClearOutput();

            OutputText("Scenes.Trainingdummyscene.DummyFaint.NothingToSate".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void DummySetUp()
        {
            ClearOutput();

            OutputText("Scenes.Trainingdummyscene.DummySetUp.PushedLimits".Localized());

            CombatScene.CleanupAfterCombat();
        }
    }
}