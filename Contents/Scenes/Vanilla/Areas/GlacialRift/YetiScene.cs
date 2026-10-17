using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.GlacialRift
{
    [Register<YetiScene>("coc:yeti")]
    public class YetiScene : Scene
    {

        public override void Entry()
        {

        }

        public void LoseToYeti()
        {
            ClearOutput();

            if (Player.Health <= 0)
                OutputText("Scenes.Yetiscene.LoseToYeti.StrengthGivesWay".Localized());
            else
                OutputText("Scenes.Yetiscene.LoseToYeti.ReachesLimit".Localized());

            GetFuckedByYetiDoggyStyle();
        }

        public void GetFuckedByYetiDoggyStyle()
        {

            OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.BodyPinned".Localized());

            if (!Player.HasCock() && !Player.HasVagina())
                OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.TinglingArousal".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.BloodSwelling".Localized());

            if (Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.AsWellAs".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.LipsBeginTremble".Localized(
                     Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.SpunAround".Localized());

            OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.HotSlick".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.FullyErect".Localized());

            OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.NeedyGrunt".Localized());

            ShowImage("yeti-loss");

            OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.FleshSmacks".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.CumOnCock".Localized());

                if (Player.Cocks.Count == 1)
                    OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.ItWord".Localized());
                else
                    OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.ThemWord".Localized());

                OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.AcheSuffix".Localized());
            }

            if (Player.Balls > 0)
                OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.BallsackSlimy".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.PoolingAlong".Localized());

            OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.BallsSlap".Localized());

            if (Player.Mf("m", "f") == "m")
                OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.TipRams".Localized());

            OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.MoansLouder".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.CumRopes".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.DrenchedSex".Localized(
                     Player.VaginaDescript(0)
                ));

            Player.ButtChange(18, true);

            OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.LowSnarl".Localized());

            OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.StrugglesIce".Localized());

            OutputText("Scenes.Yetiscene.GetFuckedByYetiDoggyStyle.CollectsThings".Localized());

            Player.Orgasm("Anal");

            CombatScene.CleanupAfterCombat();
        }

        public void WinAgainstYeti()
        {
            ClearOutput();

            OutputText("Scenes.Yetiscene.WinAgainstYeti.HungryLook".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.ButtFuck".Localized(), null);

            AddButton(1, "Common.Button.Ridehiscock".Localized(), null);

            if (Player.Armor?.Id == ArmorIds.NaughtyNunsHabit)
                AddButton(2, "Scenes.Akbal.AkbalDefeated.ButtonWorshipCock".Localized(), () => Contents.Items.Behaviors.Vanilla.NaughtyNunsHabitBehavior.CockWorship());

            if (Player.Lust >= 33)
            {
                if (Player.HasCock())
                    AddButton(0, "Common.Button.ButtFuck".Localized(), FuckYetiInTheAss);

                if (Player.HasVagina())
                    AddButton(1, "Common.Button.Ridehiscock".Localized(), RideYetisCock);
            }

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void FuckYetiInTheAss()
        {
            ClearOutput();

            OutputText("Scenes.Yetiscene.FuckYetiInTheAss.WalksCloser".Localized());

            OutputText("Scenes.Yetiscene.FuckYetiInTheAss.CuriositySlide".Localized());

            OutputText("Scenes.Yetiscene.FuckYetiInTheAss.FullyHard".Localized());

            if (Player.Cocks[0].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Yetiscene.FuckYetiInTheAss.FlaredHead".Localized());

            if (Player.Cocks[0].Id == BodyPartTypes.Cat)
                OutputText("Scenes.Yetiscene.FuckYetiInTheAss.GruntSlipsPast".Localized());

            if (Player.HasKnot())
                OutputText("Scenes.Yetiscene.FuckYetiInTheAss.TipSlipsIn".Localized());
            else
                OutputText("Scenes.Yetiscene.FuckYetiInTheAss.GroansLoud".Localized());

            OutputText("Scenes.Yetiscene.FuckYetiInTheAss.StrokesPrick".Localized());

            if (Player.BiggestCockArea() >= 20)
                OutputText("Scenes.Yetiscene.FuckYetiInTheAss.ShovesDeep".Localized());

            if (Player.BiggestCockArea() <= 20 && Player.Cocks[Player.BiggestCockIndex()].HasKnot)
                OutputText("Scenes.Yetiscene.FuckYetiInTheAss.PopsKnot".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Yetiscene.FuckYetiInTheAss.ClenchReadies".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Yetiscene.FuckYetiInTheAss.OthersUnload".Localized());

            OutputText("Scenes.Yetiscene.FuckYetiInTheAss.TucksAway".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void RideYetisCock()
        {
            ClearOutput();

            ShowImage("yeti-vag-ride");

            OutputText("Scenes.Yetiscene.RideYetisCock.WetYourself".Localized());

            if ((Player.Vaginas.Count == 0 ? 0 : Player.Vaginas[0].Looseness) <= 1)
                OutputText("Scenes.Yetiscene.RideYetisCock.WhimpersSlightly".Localized());

            Player.CuntChange(18, true);

            OutputText("Scenes.Yetiscene.RideYetisCock.ParagraphBreak".Localized());

            OutputText("Scenes.Yetiscene.RideYetisCock.SettlesAtop".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Yetiscene.RideYetisCock.HisBelly".Localized());

            if (Player.Tallness >= 60 && Player.Tallness < 72)
                OutputText("Scenes.Yetiscene.RideYetisCock.ChestFur".Localized());

            if (Player.Tallness >= 72)
                OutputText("Scenes.Yetiscene.RideYetisCock.HisNape".Localized());

            OutputText("Scenes.Yetiscene.RideYetisCock.ThickMusk".Localized());

            OutputText("Scenes.Yetiscene.RideYetisCock.WrappedFur".Localized());

            OutputText("Scenes.Yetiscene.RideYetisCock.BouncesOn".Localized());

            OutputText("Scenes.Yetiscene.RideYetisCock.FinalMoan".Localized());

            OutputText("Scenes.Yetiscene.RideYetisCock.WetPop".Localized());

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();
        }

    }
}