using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Desert;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Pregnancy.Vanilla
{

    [Register<SandTrapPregnancyDelivery>("coc:delivery_sand_trap")]
    public class SandTrapPregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context)
        {
            if (context.Incubation != 36)
                return false;

            context.Write("Pregnancy.SandTrap.BowelsGurgle".Localized());

            context.Player.ClearButtPregnancy();

            return true;
        }

        public void Deliver(PregnancyContext context)
        {
        }
    }

    [Register<SandTrapFertilePregnancyDelivery>("coc:delivery_sand_trap_fertile")]
    public class SandTrapFertilePregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context)
        {
            var displayed = false;

            if (context.Incubation == 36)
            {
                context.Write("Pregnancy.SandTrapFertile.Bloated".Localized());

                displayed = true;
            }

            if (context.Incubation == 20)
            {
                context.Write("Pregnancy.SandTrapFertile.OilyDrips".Localized(context.Player.AssholeDescript()));

                context.Player.ApplyStats(
                    new DynStat(GameStats.Lust, 500f),

                    new DynStat(GameStats.Intelligence, -0.5f));

                displayed = true;
            }

            return displayed;
        }

        public void Deliver(PregnancyContext context)
        {
            var player = context.Player;

            Scenes.SceneInstance<SandTrapScene>.Get().BirfSandTarps();

            var butt = player.Body.Get<ButtPart>();

            if ((butt.Rating < 17) && (butt.Rating < 13 || Random.Shared.Next(2) == 0))
                {
                    butt.Rating += 1;

                    context.Write("Pregnancy.SandTrapFertile.ButtGrew".Localized(player.ButtDescript()));
                }
        }
    }
}