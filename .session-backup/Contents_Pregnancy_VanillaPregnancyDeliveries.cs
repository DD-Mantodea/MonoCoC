using MonoCoC.Attributes;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Bog;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Pregnancy
{
    // 原版 5 个 scene 直接把自身注册进 PregnancyProgression（FrogGirl/Phouka/DesertCave/Ember/UrtaPregs/Cotton）。
    // 这些 scene 在 C# 已经有 Update/Birth 方法（此前是死代码），这里用薄 adapter 把它们接进 registry。
    [Register("coc:delivery_frog_girl")]
    public sealed class FrogGirlPregnancyDelivery : IPregnancyDelivery
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

    [Register("coc:delivery_faerie")]
    public sealed class FaeriePregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context) =>
            SceneInstance<PhoukaScene>.Get().UpdateVaginalPregnancy();

        public void Deliver(PregnancyContext context) =>
            SceneInstance<PhoukaScene>.Get().VaginalBirth();
    }

    [Register("coc:delivery_ember")]
    public sealed class EmberPregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context) =>
            SceneInstance<EmberScene>.Get().UpdateVaginalPregnancy();

        public void Deliver(PregnancyContext context) =>
            SceneInstance<EmberScene>.Get().VaginalBirth();
    }
}
