using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Extensions;
using MonoCoC.Contents.Achievements;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.GlacialRift
{
    [Register<FrostGiantScene>("coc:frost_giant")]
    public class FrostGiantScene : Scene
    {

        public override void Entry()
        {

        }

        public void LoseToGiant()
        {

            CombatScene.CleanupAfterCombat();
        }

        public void WinAgainstGiant()
        {
            ClearOutput();

            OutputText("Scenes.Frostgiantscene.WinAgainstGiant.GiantFalls".Localized());

            if (Player.Weapon.IsUnarmed() && Player.Armor.LongName == "nothing" && Player.ShieldName == "nothing")
                AwardAchievement(AchievementIds.ChuckNorris);

            ClearButtons();

            AddButton(0, "Common.Button.Nosefuck".Localized(), null);

            AddButton(1, "Common.Button.Ridevaginally".Localized(), null);

            AddButton(2, "Common.Button.RideAnally".Localized(), null);

            if (Player.Lust >= 33)
            {
                if (Player.HasCock())
                    AddButton(0, "Common.Button.Nosefuck".Localized(), NoseJob);

                if (Player.HasVagina())
                    AddButton(1, "Common.Button.Ridevaginally".Localized(), RideVaginally);

                AddButton(2, "Common.Button.RideAnally".Localized(), RideAnally);
            }

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void NoseJob()
        {
            ClearOutput();

            OutputText("Scenes.Frostgiantscene.NoseJob.OnlyOneOption".Localized(
                 Player.MultiCockDescript()
            ));

            OutputText("Scenes.Frostgiantscene.NoseJob.TestsNostril".Localized());

            OutputText("Scenes.Frostgiantscene.NoseJob.WhipsOut".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Frostgiantscene.NoseJob.PluralS".Localized());

            OutputText("Scenes.Frostgiantscene.NoseJob.ColdAirShrinks".Localized());

            if (Player.Cocks.Count == 1)
                OutputText("Scenes.Frostgiantscene.NoseJob.NostrilMassages".Localized());
            else
                OutputText("Scenes.Frostgiantscene.NoseJob.NostrilsMassage".Localized());

            OutputText("Scenes.Frostgiantscene.NoseJob.AwkwardFighting".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Frostgiantscene.NoseJob.PullsBalls".Localized());

            OutputText("Scenes.Frostgiantscene.NoseJob.PreDribblesSeptum".Localized());

            OutputText("Scenes.Frostgiantscene.NoseJob.FirstSpurt".Localized());

            OutputText("Scenes.Frostgiantscene.NoseJob.SpurtsContinue".Localized());

            OutputText("Scenes.Frostgiantscene.NoseJob.BellowsScreams".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void RideVaginally()
        {
            ClearOutput();

            OutputText("Scenes.Frostgiantscene.RideVaginally.RubsVagina".Localized());

            if (Player.Tallness < 36)
                OutputText("Scenes.Frostgiantscene.RideVaginally.DwarfsHeight".Localized());
            else
                if (Player.Tallness >= 36 && Player.Tallness < 48)
                    OutputText("Scenes.Frostgiantscene.RideVaginally.MatchesHeight".Localized());
                else
                    OutputText("Scenes.Frostgiantscene.RideVaginally.SlightlyShorter".Localized());

            OutputText("Scenes.Frostgiantscene.RideVaginally.ParagraphBreak".Localized());

            if (Player.Cocks.Count <= 0)
                OutputText("Scenes.Frostgiantscene.RideVaginally.NoOtherOption".Localized());
            else
                OutputText("Scenes.Frostgiantscene.RideVaginally.GivesVaginaAttention".Localized());

            OutputText("Scenes.Frostgiantscene.RideVaginally.ReplacesArmor".Localized());

            OutputText("Scenes.Frostgiantscene.RideVaginally.ClimbsCrotch".Localized());

            OutputText("Scenes.Frostgiantscene.RideVaginally.BracingBreath".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Frostgiantscene.RideVaginally.ShootsJism".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Frostgiantscene.RideVaginally.JuiceSplatters".Localized());

            OutputText("Scenes.Frostgiantscene.RideVaginally.GiantNeedsRelease".Localized());

            Player.RefillHunger(50);

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();
        }

        public void RideAnally()
        {
            ClearOutput();

            OutputText("Scenes.Frostgiantscene.RideAnally.RemovesArmorBottom".Localized());

            if (Player.Tallness < 36)
                OutputText("Scenes.Frostgiantscene.RideAnally.DwarfsHeight".Localized());
            else
                if (Player.Tallness >= 36 && Player.Tallness < 48)
                    OutputText("Scenes.Frostgiantscene.RideAnally.MatchesHeight".Localized());
                else
                    OutputText("Scenes.Frostgiantscene.RideAnally.SlightlyShorter".Localized());

            OutputText("Scenes.Frostgiantscene.RideAnally.ParagraphBreak".Localized());

            OutputText("Scenes.Frostgiantscene.RideAnally.ReplacesArmor".Localized());

            OutputText("Scenes.Frostgiantscene.RideAnally.BracingBreath".Localized());

            OutputText("Scenes.Frostgiantscene.RideAnally.ReadiesDismount".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Frostgiantscene.RideAnally.ShootsJism".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Frostgiantscene.RideAnally.JuiceSplatters".Localized());

            OutputText("Scenes.Frostgiantscene.RideAnally.GiantNeedsRelease".Localized());

            Player.ButtChange(144, true, true, false);

            OutputText("Scenes.Frostgiantscene.RideAnally.YelpsSurprise".Localized());

            Player.RefillHunger(30);

            Player.Orgasm("Anal");

            CombatScene.CleanupAfterCombat();
        }

        public void SuckHimOff()
        {
            ClearOutput();

            OutputText("Scenes.Frostgiantscene.SuckHimOff.PlaceholderOnly".Localized());

            Player.RefillHunger(100);

            CombatScene.CleanupAfterCombat();
        }

        public void ExitGiant()
        {

            ClearButtons();

            CombatScene.CleanupAfterCombat();
        }

    }
}