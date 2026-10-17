using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.World;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{

    [Register<MarbleScene>("coc:marble_scene")]
    public partial class MarbleScene : Scene, ITimeAware
    {

        public override void Entry()
        {
        }

        public void MarbleFightWin()
        {

            ClearOutput();

            if (CurrentMonster?.Health < 1)
                OutputText("Scenes.Marblescene.MarbleFightWin.DefeatedLookingUp".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleFightWin.DesperateSkirtLift".Localized());

            OutputText("Scenes.Marblescene.MarbleFightWin.CrowdOfOnlookers".Localized());

            ClearButtons();

            if (Player.HasPerk(PerkIds.Feeder) || Player.LactationQ() > 200)
                AddButton(0, "Common.Button.FeedHer".Localized(), ForceFeedMarble);
            else
                AddButtonDisabled(0, "Feed Her", "Scenes.Marblescene.MarbleFightWin.FeedHerHint".Localized());

            AddButton(1, "Common.Button.Rapeinroom".Localized(), RapeMarbleInHerRoom);

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void MarbleFightLose()
        {

            ClearOutput();

            if (Player.Health < 1)
                OutputText("Scenes.Marblescene.MarbleFightLose.KnockedToGround".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleFightLose.OvercomeByDesire".Localized());

            OutputText("Scenes.Marblescene.MarbleFightLose.WhisperedThreat".Localized());

            CombatScene.CleanupAfterCombat();
        }
    }
}