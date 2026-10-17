using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Pregnancy.Vanilla
{

    [Register<TentacleBeastPregnancyDelivery>("coc:delivery_tentacle_beast")]
    public class TentacleBeastPregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context)
        {
            var displayed = false;

            switch (context.Incubation)
            {
                case 120:
                    context.Write("Pregnancy.TentacleBeast.Stage1".Localized());

                    displayed = true;

                    break;

                case 90:
                    context.Write("Pregnancy.TentacleBeast.Stage2".Localized());

                    displayed = true;

                    break;

                case 60:
                    context.Write("Pregnancy.TentacleBeast.Stage3".Localized());

                    displayed = true;

                    break;

                case 30:
                    context.Write("Pregnancy.TentacleBeast.Stage4".Localized());

                    displayed = true;

                    break;
            }

            return displayed;
        }

        public void Deliver(PregnancyContext context)
        {
            var player = context.Player;

            PregnancyUtils.CreateVaginaIfMissing(context);

            Scenes.SceneInstance<TentacleBeastScene>.Get().PumpkinBirth();

            var hips = player.Body.Get<HipsPart>();

            if (hips.Rating < 10)
            {
                hips.Rating += 1;

                context.Write("Pregnancy.TentacleBeast.HipsWider".Localized(player.ArmorName, player.HipDescript()));
            }

            context.Write("[pg]");
        }
    }
}