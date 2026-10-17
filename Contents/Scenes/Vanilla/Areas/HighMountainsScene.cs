using MonoCoC.Attributes;
using MonoCoC.Contents.Encounters;
using System;
using MonoCoC.Contents.Datas.Vanilla;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas
{

    [Register<HighMountainsScene>("coc:high_mountains")]
    public class HighMountainsScene() : AreaScene(AreaIds.HighMountains)
    {
        protected override void ExploreArea()
        {
            Player.Location = "high mountains";

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

            HighMountainsEncounters.Table.Execute(context);
        }

    }
}