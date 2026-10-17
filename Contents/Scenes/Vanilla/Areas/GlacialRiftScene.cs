using MonoCoC.Attributes;
using MonoCoC.Contents.Encounters;
using System;
using MonoCoC.Contents.Datas.Vanilla;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas
{

    [Register<GlacialRiftScene>("coc:glacial_rift")]
    public class GlacialRiftScene() : AreaScene(AreaIds.GlacialRift)
    {
        protected override void ExploreArea()
        {
            Player.Location = "rift";

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

            GlacialRiftEncounters.Table.Execute(context);
        }

    }
}