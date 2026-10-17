using MonoCoC.Attributes;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Pregnancy.Vanilla
{

    [Register<SatyrPregnancyDelivery>("coc:delivery_satyr")]
    public class SatyrPregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context)
        {
            var displayed = false;

            switch (context.Incubation)
            {
                case 150:
                    context.Write("Pregnancy.Satyr.Sluggish".Localized());

                    displayed = true;

                    break;

                case 125:
                    context.Write("Pregnancy.Satyr.BellyBigger".Localized());

                    displayed = true;

                    break;

                case 100:
                    context.Write("Pregnancy.Satyr.Fluttering".Localized());

                    displayed = true;

                    break;

                case 75:
                    context.Write("Pregnancy.Satyr.Bump".Localized());

                    displayed = true;

                    break;

                case 50:
                    context.Write("Pregnancy.Satyr.HornyAndHungry".Localized());

                    displayed = true;

                    break;

                case 30:
                    context.Write("Pregnancy.Satyr.KicksForFood".Localized());

                    displayed = true;

                    break;
            }

            return displayed;
        }

        public void Deliver(PregnancyContext context)
        {

            Scenes.SceneInstance<Scenes.Vanilla.Areas.Plains.SatyrScene>.Get().SatyrBirth(!context.Anal);
        }
    }
}