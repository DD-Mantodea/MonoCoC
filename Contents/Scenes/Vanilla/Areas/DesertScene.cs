using MonoCoC.Attributes;
using MonoCoC.Contents.Encounters;
using System;
using MonoCoC.Contents.Datas.Vanilla;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas
{

    [Register<DesertScene>("coc:desert")]
    public class DesertScene() : AreaScene(AreaIds.Desert)
    {
        protected override void ExploreArea()
        {
            Player.Location = "desert";

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

            DesertEncounters.Table.Execute(context);
        }

    }
}