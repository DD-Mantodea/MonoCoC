using MonoCoC.Contents.Encounters;
using System;
using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas
{

    [Register<PlainsScene>("coc:plains")]
    public class PlainsScene() : AreaScene(AreaIds.Plains)
    {
        protected override void ExploreArea()
        {
            Player.Location = "plains";

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

            PlainsEncounters.Table.Execute(context);
        }

    }
}