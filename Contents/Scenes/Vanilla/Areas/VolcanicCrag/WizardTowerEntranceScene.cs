using MonoCoC.Attributes;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.VolcanicCrag
{

    [Register<WizardTowerEntranceScene>("coc:wizard_tower_entrance")]
    public class WizardTowerEntranceScene : Scene
    {
        public override void Entry()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.WizardTower.EnterDungeon.ArduousTravelsVolcanic".Localized());

            OutputText("Scenes.WizardTower.EnterDungeon.SixHundredFeetTall".Localized());

            OutputText("Scenes.WizardTower.EnterDungeon.MoveAheadHikingDown".Localized());

            OutputText("Scenes.WizardTower.EnterDungeon.AfterFewDifficultMinutes".Localized());

            OutputText("Scenes.WizardTower.EnterDungeon.CloserFireWind".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FoundWizardTower, 1);

            AddButton(0, "Common.Button.Enter".Localized(), () => DungeonScene.Enter(new Identifier("coc", "wizard_tower")));

            AddButton(14, "Common.Button.Leave".Localized(), () => Go<CampScene>());
        }
    }
}