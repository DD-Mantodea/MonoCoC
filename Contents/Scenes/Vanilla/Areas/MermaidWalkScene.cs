using MonoCoC.Contents.Flags;
using MonoCoC.Registry;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas
{

    public static class MermaidWalkScene
    {
        public static bool IsMermaid()
        {
            return CoC.Instance.Player.LowerBodyType().GetString("body") == "mermaid";
        }

        public static bool Available()
        {
            return IsMermaid() && CoC.Instance.Flags.Get(GameFlagIds.DoneMermaidWalk) == 0;
        }

        public static void Run(string area)
        {
            var flags = CoC.Instance.Flags;

            var firstTime = area == "desert"

                ? flags.Get(GameFlagIds.DesertWaterNeeded) == 0

                : flags.Get(GameFlagIds.CragWaterNeeded) == 0;

            if (area == "desert")
                flags.Set(GameFlagIds.DesertWaterNeeded, 1);
            else
                flags.Set(GameFlagIds.CragWaterNeeded, 1);

            Scene.ClearOutput();

            if (firstTime)
                Scene.OutputText(area == "desert"

                    ? "Crossing into the desert, the sun-baked sand immediately irritates your scales, sapping moisture from your fish-like coil. You reach for water, desperate for relief."

                    : "The heat increases dramatically as you crest the rocky terrain. Your throat squeezes shut and leaves you coughing yourself hoarse. You need some moisture.");

            string? fluid = null;

            foreach (var candidate in new[]
                     {
                         "coc:spring_water",

                         "coc:succubi_milk",

                         "coc:marble_milk"
                     })

                if (CoC.Instance.Player.HasItem(Identifier.Parse(candidate)))
                {
                    fluid = candidate;

                    break;
                }

            if (fluid == null)
            {
                Scene.OutputText("Common.Text.PgNothingSuitableAppearsInYour".Localized());

                Scene.Go<CampScene>();

                return;
            }

            CoC.Instance.Player.ConsumeItem(Identifier.Parse(fluid));

            Scene.OutputText("Common.Text.PgYouQuaffTheFluidYou".Localized());

            flags.Set(GameFlagIds.DoneMermaidWalk, 1);

            Scene.Go<CampScene>();
        }
    }
}