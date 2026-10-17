using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Encounters;
using MonoCoC.Contents.World.Areas;
using MonoCoC.Extensions;
using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas
{

    [Register<DeepWoodsScene>("coc:deepwoods")]
    public class DeepWoodsScene() : AreaScene(AreaIds.Deepwoods)
    {

        protected override void ExploreArea()
        {
            Player.Location = "deep woods";

            Flags.Set(Area.ExplorationFlag, Flags.Get(Area.ExplorationFlag) + 1);

            ClearOutput();

            ClearButtons();

            var context = new EncounterContext
            {
                Player = Player,

                Flags = Flags,

                Time = View.Time,

                Random = Random.Shared,

                Write = OutputText,

                ReturnToCamp = () => ReturnToCamp(1),

                ReturnToCampAfter = ReturnToCamp
            };

            DeepWoodsEncounters.Table.Execute(context);
        }

        public static void Discover()
        {
            var player = CoC.Instance.Player;

            player.CreateStatusEffect(StatusEffectIds.ExploredDeepwoods);

            var deepwoods = AreaRegistry.Get(AreaIds.Deepwoods);

            if (deepwoods != null)
                AreaRegistry.Discover(deepwoods);

            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Deepwoods.Discover.PushDeeperWoods".Localized());

            OutputText("Scenes.Deepwoods.Discover.DeepwoodsUnlocked".Localized());

            AddButton(0, "Scenes.Explore.Continue".Localized(), () => ReturnToCamp(1));
        }

    }
}