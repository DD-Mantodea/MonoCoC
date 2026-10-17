using MonoCoC.Attributes;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Bog;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Pregnancy
{

    [Register<FrogGirlPregnancyDelivery>("coc:delivery_frog_girl")]
    public class FrogGirlPregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context) =>

            context.Anal

                ? SceneInstance<FrogGirlScene>.Get().UpdateAnalPregnancy()

                : SceneInstance<FrogGirlScene>.Get().UpdateVaginalPregnancy();

        public void Deliver(PregnancyContext context)
        {
            if (context.Anal)
                SceneInstance<FrogGirlScene>.Get().AnalBirth();
            else
                SceneInstance<FrogGirlScene>.Get().VaginalBirth();
        }
    }

    [Register<FaeriePregnancyDelivery>("coc:delivery_faerie")]
    public class FaeriePregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context) =>

            SceneInstance<PhoukaScene>.Get().UpdateVaginalPregnancy();

        public void Deliver(PregnancyContext context) =>

            SceneInstance<PhoukaScene>.Get().VaginalBirth();
    }

    [Register<EmberPregnancyDelivery>("coc:delivery_ember")]
    public class EmberPregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context) =>

            SceneInstance<EmberScene>.Get().UpdateVaginalPregnancy();

        public void Deliver(PregnancyContext context) =>

            SceneInstance<EmberScene>.Get().VaginalBirth();
    }
}