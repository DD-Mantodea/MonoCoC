using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    public static class TrainingDummyCamp
    {

        public static SaveContent Save => Scene.SaveContent(new Identifier("coc", "camp"));

        public static bool DummyBuilt
        {
            get => Save.GetBool("dummyBuilt");

            set => Save.SetBool("dummyBuilt", value);
        }

        public static int DummyGender
        {
            get => Save.GetInt("dummyGender");

            set => Save.SetInt("dummyGender", value);
        }

        public static string DummyName
        {
            get => Save.GetString("dummyName");

            set => Save.SetString("dummyName", value);
        }

        public static void AddCampActions()
        {
            if (DummyBuilt)
            {
                Scene.AddButton(5, DummyName,

                    () => SceneInstance<TrainingDummyScene>.Get().DummyMenu(true),

                    "Scenes.Camp.Actions.Hint.Dummy".Localized(DummyName));

                return;
            }

            if (Scene.Player.HasKeyItem("Carpenter's Toolbox"))
                Scene.AddButton(5, "Common.Button.BuildDummy".Localized(), BuildDummyPrompt,

                    "Scenes.Camp.Actions.Hint.BuildDummy".Localized());
        }

        public static void BuildDummyPrompt()
        {
            Scene.ClearOutput();

            Scene.OutputText("Scenes.Camp.DummyBuild.Prompt".Localized());

            var hasResources = Scene.Flags.Get(GameFlagIds.CampCabinWoodResources) >= 10

                && Scene.Player.KeyItemV1("Carpenter's Toolbox") >= 50

                && Scene.Flags.Get(GameFlagIds.CampCabinStoneResources) >= 5;

            if (hasResources)
            {
                Scene.OutputText("Scenes.Camp.DummyBuild.DoItNow".Localized());

                Scene.ClearButtons();

                Scene.AddButton(0, "Common.Yes".Localized(), BuildDummy);

                Scene.AddButton(1, "Common.No".Localized(), () => Scene.Go<CampScene>());

                return;
            }

            Scene.OutputText("Scenes.Camp.DummyBuild.LackResources".Localized());

            Scene.OutputText("Scenes.Camp.DummyBuild.NoResources".Localized());

            Scene.ClearButtons();

            Scene.AddButton(0, "Common.OK".Localized(), () => Scene.Go<CampScene>());
        }

        public static void BuildDummy()
        {
            Scene.ClearOutput();

            Scene.OutputText("Scenes.Camp.DummyBuild.SawInHand".Localized());

            Scene.OutputText("Scenes.Camp.DummyBuild.WorksOnDummy".Localized());

            Scene.OutputText("Scenes.Camp.DummyBuild.LooksLikeScarecrow".Localized());

            Scene.Player.ChangeFatigue(20);

            Scene.Flags.Set(GameFlagIds.CampCabinWoodResources, Scene.Flags.Get(GameFlagIds.CampCabinWoodResources) - 7);

            Scene.Player.AddKeyValue("Carpenter's Toolbox", 1, -38);

            Scene.Flags.Set(GameFlagIds.CampCabinStoneResources, Scene.Flags.Get(GameFlagIds.CampCabinStoneResources) - 5);

            DummyBuilt = true;

            EngenderDummy();
        }

        public static void EngenderDummy(int option = 0)
        {

            const int male = 1;

            const int female = 2;

            switch (option)
            {
                case male:
                    Scene.ClearOutput();

                    Scene.OutputText("Scenes.Camp.DummyBuild.AddDick".Localized());

                    Scene.OutputText("Scenes.Camp.DummyBuild.AddDickAfter".Localized());

                    DummyGender += male;

                    Scene.Player.ChangeFatigue(5);

                    Scene.Flags.Set(GameFlagIds.CampCabinWoodResources, Scene.Flags.Get(GameFlagIds.CampCabinWoodResources) - 1);

                    Scene.Player.AddKeyValue("Carpenter's Toolbox", 1, -4);

                    break;

                case female:
                    Scene.ClearOutput();

                    Scene.OutputText("Scenes.Camp.DummyBuild.AddTits".Localized());

                    Scene.OutputText("Scenes.Camp.DummyBuild.AddTitsAfter".Localized());

                    DummyGender += female;

                    Scene.Player.ChangeFatigue(5);

                    Scene.Flags.Set(GameFlagIds.CampCabinWoodResources, Scene.Flags.Get(GameFlagIds.CampCabinWoodResources) - 2);

                    Scene.Player.AddKeyValue("Carpenter's Toolbox", 1, -8);

                    break;

                case 3:
                    Scene.ClearOutput();

                    Scene.OutputText("Scenes.Camp.DummyBuild.Finish".Localized());

                    Scene.OutputText("Scenes.Camp.DummyBuild.NamePrompt".Localized());

                    Scene.ClearButtons();

                    Scene.GenericNamePrompt();

                    Scene.AddButton(0, "Common.Button.Confirm".Localized(), () => NameDummy(false));

                    return;
            }

            Scene.ClearButtons();

            Scene.AddNextButton("CharCreation.GenericStyleCustomizeMenu.Button8".Localized(), () => EngenderDummy(3));

            if (DummyGender >= female)
                Scene.AddNextButtonDisabled("Tits", "You've already added that.");
            else
                Scene.AddNextButton("Common.Button.Tits".Localized(), () => EngenderDummy(female));

            if (DummyGender % 2 == 1)
                Scene.AddNextButtonDisabled("Dick", "You've already added that.");
            else
                Scene.AddNextButton("Common.Button.Dick".Localized(), () => EngenderDummy(male));
        }

        public static void NameDummy(bool rename = false)
        {
            Scene.ClearOutput();

            var name = Scene.FinishInput().Trim();

            DummyName = name;

            if (name.Length == 0)
            {
                Scene.OutputText("Scenes.Camp.NameDummy.Empty".Localized());

                Scene.ClearButtons();

                Scene.GenericNamePrompt();

                Scene.AddButton(0, "Common.Button.Confirm".Localized(), () => NameDummy(rename));

                return;
            }

            if (name == Scene.Player.ShortName)
                Scene.OutputText("Scenes.Camp.NameDummy.SelfHarm".Localized());
            else if (name is "Dummy" or "Training" or "Training Dummy" or "Punchbag" or "Punching Bag"
                or "Wood" or "Log" or "Mannequin")

                Scene.OutputText("Scenes.Camp.NameDummy.Imaginative".Localized());
            else if (name is "Fenoxo" or "Fen" or "Fagnoxo" or "Jewnoxo")
                Scene.OutputText("Scenes.Camp.NameDummy.Fenoxo".Localized());
            else if (name is "Savin" or "Savcuck")
                Scene.OutputText("Scenes.Camp.NameDummy.Savin".Localized());
            else if (name == "Satan")
                Scene.OutputText("Scenes.Camp.NameDummy.Satan".Localized());
            else if (name == "Rathazul")
                Scene.OutputText("Scenes.Camp.NameDummy.Rathazul".Localized());
            else if (name == "Lethice")
                Scene.OutputText("Scenes.Camp.NameDummy.Lethice".Localized());
            else if (name is "Urta" or "Behemoth" or "Anzu")
                Scene.OutputText("Scenes.Camp.NameDummy.DonutSteel".Localized());

            if (rename)
            {
                Scene.OutputText("Scenes.Camp.NameDummy.Renamed".Localized());

                SceneInstance<TrainingDummyScene>.Get().DummyMenu();

                return;
            }

            Scene.OutputText("Scenes.Camp.NameDummy.Built".Localized());

            Scene.OutputText("Scenes.Camp.NameDummy.AddedToMenu".Localized());

            Scene.ReturnToCamp(2);
        }
    }
}