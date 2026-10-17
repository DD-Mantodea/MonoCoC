using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Bog
{
    [Register<AnneMarieScene>("coc:anne_marie")]
    public class AnneMarieScene : Scene
    {

        public const int MET_ANNE = 1;

        public const int RUDE_TO_ANNE = 2;

        public override void Entry()
        {
            EncounterAnneMarie();
        }

        public void EncounterAnneMarie()
        {
            ClearOutput();

            OutputText("Scenes.Annemariescene.EncounterAnneMarie.ThickMarsh".Localized());

            OutputText("Scenes.Annemariescene.EncounterAnneMarie.CabinRise".Localized());

            OutputText("Scenes.Annemariescene.EncounterAnneMarie.MudEasier".Localized());

            OutputText("Scenes.Annemariescene.EncounterAnneMarie.AvoidsWindows".Localized());

            OutputText("Scenes.Annemariescene.EncounterAnneMarie.DoorBaffles".Localized());

            DoNext(() => ContinueConv());
        }

        public void ContinueConv()
        {
            ClearOutput();

            OutputText("Scenes.Annemariescene.ContinueConv.RifleAtEyes".Localized());

            OutputText("Scenes.Annemariescene.ContinueConv.HandsWhereISee".Localized());

            OutputText("Scenes.Annemariescene.ContinueConv.ConvinceOrTeach".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.NoHarm".Localized(), () => Encounter2(true));

            AddButton(1, "Common.Button.NoMiss".Localized(), () => Encounter2(false));
        }

        public void Encounter2(bool kind = true)
        {
            ClearOutput();

            if (kind)
            {
                OutputText("Scenes.Annemariescene.Encounter2.RaisesHands".Localized());

                OutputText("Scenes.Annemariescene.Encounter2.EyebrowsInquisitive".Localized());

                OutputText("Scenes.Annemariescene.Encounter2.SighOfRelief".Localized());

                OutputText("Scenes.Annemariescene.Encounter2.BetterLook".Localized());

                OutputText("Scenes.Annemariescene.Encounter2.AngryGreenEyes".Localized());

                OutputText("Scenes.Annemariescene.Encounter2.WhyOnEdge".Localized());

                OutputText("Scenes.Annemariescene.Encounter2.Understandable".Localized());

                OutputText("Scenes.Annemariescene.Encounter2.EarsTwitch".Localized());

                OutputText("Scenes.Annemariescene.Encounter2.ForestNorth".Localized());

                OutputText("Scenes.Annemariescene.Encounter2.WingedArmChin".Localized());

                OutputText("Scenes.Annemariescene.Encounter2.YoureWelcome".Localized());

                OutputText("Scenes.Annemariescene.Encounter2.WhyStillHere".Localized());

                OutputText("Scenes.Annemariescene.Encounter2.LooksTense".Localized());

                OutputText("Scenes.Annemariescene.Encounter2.VeryPerceptive".Localized());

                OutputText("Scenes.Annemariescene.Encounter2.WhyPushedOff".Localized());

                OutputText("Scenes.Annemariescene.Encounter2.ShutsDoor".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.AnnemarieStatus, 1);

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Annemariescene.Encounter2.WarningFist".Localized());

                OutputText("Scenes.Annemariescene.Encounter2.ThanksForWarning".Localized());

                OutputText("Scenes.Annemariescene.Encounter2.GrabsBarrel".Localized());

                OutputText("Scenes.Annemariescene.Encounter2.SphereCracks".Localized());

                OutputText("Scenes.Annemariescene.Encounter2.LastWords".Localized());

                DoNext(() => Encounter3wakeup());
            }
        }

        public void Encounter3wakeup()
        {
            ClearOutput();

            OutputText("Scenes.Annemariescene.Encounter3wakeup.InsectWakeup".Localized());

            DoNext(() => ReturnToCamp(4));
        }

        public void MysteriousStranger()
        {

            OutputText("Scenes.Annemariescene.MysteriousStranger.MuzzleFlash".Localized());

            if (Random.Shared.Next(9) < 8)
            {
                OutputText("Scenes.Annemariescene.MysteriousStranger.ProjectileHits".Localized());

                CombatController.Instance!.Context.DealDamageToPlayer(150 + Random.Shared.Next(150));
            }
            else
                OutputText("Scenes.Annemariescene.MysteriousStranger.GroundCraters".Localized());

            OutputText("Scenes.Annemariescene.MysteriousStranger.CannonBoom".Localized());
        }

    }
}