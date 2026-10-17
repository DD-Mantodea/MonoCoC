using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Bog
{
    [Register<BatgirlScene>("coc:batgirl")]
    public class BatgirlScene : Scene
    {

        public override void Entry()
        {
            BatDown();
        }

        public void BatDown()
        {
            ClearOutput();

            OutputText("Scenes.Batgirlscene.BatDown.FallsTooWeak".Localized());

            OutputText("Scenes.Batgirlscene.BatDown.GazeDarts".Localized());

            OutputText("Scenes.Batgirlscene.BatDown.LetMeGoPlea".Localized());

            OutputText("Scenes.Batgirlscene.BatDown.WhyBlood".Localized());

            OutputText("Scenes.Batgirlscene.BatDown.BloodIsLife".Localized());

            OutputText("Scenes.Batgirlscene.BatDown.LightensHold".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Aiko.FuckYamata.ButtonLetHerGo".Localized(), LetBatGo);

            AddButton(1, "Common.Button.GiveBlood".Localized(), GiveBatBlood);

            AddButton(2, "Common.Button.Trade".Localized(), TradeBatBloodDick);
        }

        public void TradeBatBloodVagina()
        {
            ClearOutput();

            OutputText("Scenes.Batgirlscene.TradeBatBloodVagina.WillingBlood".Localized());

            OutputText("Scenes.Batgirlscene.TradeBatBloodVagina.ShufflesLeave".Localized());

            OutputText("Scenes.Batgirlscene.TradeBatBloodVagina.SilenceBlush".Localized());

            OutputText("Scenes.Batgirlscene.TradeBatBloodVagina.GropedCrotch".Localized());

            OutputText("Scenes.Batgirlscene.TradeBatBloodVagina.NoDickSurprise".Localized());

            OutputText("Scenes.Batgirlscene.TradeBatBloodVagina.Inexperienced".Localized());

            OutputText("Scenes.Batgirlscene.TradeBatBloodVagina.LiftsFinger".Localized());
        }

        public void TradeBatBloodDick()
        {
            ClearOutput();

            OutputText("Scenes.Batgirlscene.TradeBatBloodDick.WillingBlood".Localized());

            OutputText("Scenes.Batgirlscene.TradeBatBloodDick.ShufflesLeave".Localized());

            OutputText("Scenes.Batgirlscene.TradeBatBloodDick.SilenceBlush".Localized());

            OutputText("Scenes.Batgirlscene.TradeBatBloodDick.MeetsEyes".Localized());

            OutputText("Scenes.Batgirlscene.TradeBatBloodDick.HandsOnHips".Localized());

            OutputText("Scenes.Batgirlscene.TradeBatBloodDick.FootProbes".Localized());

            OutputText("Scenes.Batgirlscene.TradeBatBloodDick.HipsPushBack".Localized());

            OutputText("Scenes.Batgirlscene.TradeBatBloodDick.GrindsCock".Localized());

            OutputText("Scenes.Batgirlscene.TradeBatBloodDick.LubeString".Localized());

            OutputText("Scenes.Batgirlscene.TradeBatBloodDick.BothHandsGround".Localized());

            OutputText("Scenes.Batgirlscene.TradeBatBloodDick.PushesDeep".Localized());

            OutputText("Scenes.Batgirlscene.TradeBatBloodDick.RotatesFace".Localized());

            OutputText("Scenes.Batgirlscene.TradeBatBloodDick.FinalThrust".Localized());

            OutputText("Scenes.Batgirlscene.TradeBatBloodDick.HeatHuskyBreath".Localized());

            OutputText("Scenes.Batgirlscene.TradeBatBloodDick.SharpBite".Localized());

            Player.Orgasm("dick");

            ReturnToCamp(1);
        }

        public void GiveBatBlood()
        {

        }

        public void LetBatGo()
        {

        }

    }
}