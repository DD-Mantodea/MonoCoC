using MonoCoC.Attributes;
using MonoCoC.Contents.Encounters;
using System;
using MonoCoC.Contents.Datas.Vanilla;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas
{

    [Register<BogScene>("coc:bog")]
    public class BogScene() : AreaScene(AreaIds.Bog)
    {
        protected override void ExploreArea()
        {
            Player.Location = "bog";

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

            BogEncounters.Table.Execute(context);
        }

    }
}