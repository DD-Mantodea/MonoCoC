using MonoCoC.Contents.Encounters;
using System;
using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas
{
    [Register<ForestScene>("coc:forest")]
    public class ForestScene() : AreaScene(AreaIds.Forest)
    {
        protected override void ExploreArea()
        {
            Player.Location = "forest";

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

            ForestEncounters.Table.Execute(context);
        }

    }
}