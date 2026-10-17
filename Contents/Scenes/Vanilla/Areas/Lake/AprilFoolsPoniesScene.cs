using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Lake
{

    [Register<AprilFoolsPoniesScene>("coc:april_fools_ponies")]
    public class AprilFoolsPoniesScene : Scene
    {
        public override void Entry() => PoniesEncounter();

        public static bool IsAprilFools()
        {
            var now = DateTime.Now;

            return now.Month == 4 && now.Day == 1;
        }

        public void PoniesEncounter()
        {
            ClearOutput();

            OutputText("Scenes.Aprilfools.Ponies.Intro".Localized());

            AddButton(0, "Combat.Button.Approach".Localized(), ApproachPonies);

            AddButton(14, "Common.Button.Leave".Localized(), LeavePonies);

            CoC.Instance.Flags.Set(
                GameFlagIds.SillyModePonies,

                CoC.Instance.Flags.Get(GameFlagIds.SillyModePonies) + 1);
        }

        public void LeavePonies()
        {
            ClearOutput();

            OutputText("Scenes.Aprilfools.Ponies.Leave".Localized());

            Go<CampScene>();
        }

        public void ApproachPonies()
        {
            ClearOutput();

            OutputText("Scenes.Aprilfools.Ponies.Approach".Localized());

            AddButton(0, "Common.Button.TooCreepy".Localized(), DerpCreepy);

            AddButton(1, "Common.Button.YayParty".Localized(), DerpyParty);

            AddButton(14, "Common.Button.Leave".Localized(), DerpPolitely);
        }

        public void DerpPolitely()
        {
            ClearOutput();

            OutputText("Scenes.Aprilfools.Ponies.Politely".Localized());

            Go<CampScene>();
        }

        public void DerpCreepy()
        {
            ClearOutput();

            OutputText("Scenes.Aprilfools.Ponies.Creepy".Localized());

            OutputText("Scenes.Aprilfools.Ponies.CreepyReward".Localized());

            Player.Experience += 10;

            Player.Gems += 5;

            Go<CampScene>();
        }

        public void DerpyParty()
        {
            ClearOutput();

            OutputText("Scenes.Aprilfools.Ponies.Party".Localized());

            var lower = Player.Body.Get<LowerBodyPart>();

            lower.Id = BodyPartTypes.Pony;

            lower.LegCount = 4;

            Go<CampScene>();
        }
    }
}