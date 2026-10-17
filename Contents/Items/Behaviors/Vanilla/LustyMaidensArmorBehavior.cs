using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Items.Behaviors.Vanilla
{

    [Register<LustyMaidensArmorBehavior>("coc:lusty_maidens_armor")]
    public class LustyMaidensArmorBehavior : ItemBehavior
    {
        public static void Paizuri(Enemy monster)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.ClearButtons();

            var cock = monster.CockDescriptShort(0);

            var head = monster.CockHead();

            var virgin = player.HasVagina() && player.Body.Get<VaginaPart>().Virgin;

            Scene.OutputText("Scenes.LustyMaiden.OpenerCockExposed".Localized(cock));

            if (monster.Lust < 50)
                Scene.OutputText("Scenes.LustyMaiden.TierNotReady".Localized(head));
            else if (monster.Lust < 100)
                Scene.OutputText("Scenes.LustyMaiden.TierTurgid".Localized(head));
            else
                Scene.OutputText("Scenes.LustyMaiden.TierRaging".Localized());

            Scene.OutputText("Scenes.LustyMaiden.Moistness".Localized());

            Scene.OutputText("Scenes.LustyMaiden.Chastise".Localized(cock));

            Scene.OutputText("Scenes.LustyMaiden.Smile".Localized());

            Scene.OutputText((virgin ? "Scenes.LustyMaiden.Virgin" : "Scenes.LustyMaiden.NotVirgin").Localized());

            Scene.OutputText("Scenes.LustyMaiden.SqueezeThemOut".Localized());

            Scene.OutputText("Scenes.LustyMaiden.Nods".Localized());

            if (monster.Balls > 0)
                Scene.OutputText("Scenes.LustyMaiden.BallsFondle".Localized());
            else if (monster.ShortName == "anemone")
                Scene.OutputText("Scenes.LustyMaiden.AnemoneTaint".Localized());
            else
                Scene.OutputText("Scenes.LustyMaiden.Taint".Localized());

            Scene.OutputText("Scenes.LustyMaiden.Dive".Localized(cock, head));

            if (monster.CockArea(0) >= 80)
                Scene.OutputText("Scenes.LustyMaiden.ThroatStretch".Localized());

            Scene.OutputText("Scenes.LustyMaiden.MouthFeelsGood".Localized());

            Scene.OutputText("Scenes.LustyMaiden.UpAndDown".Localized());

            if (player.HasFuckableNipples() || player.LactationQ() >= 100)
                Scene.OutputText("Scenes.LustyMaiden.NipplesDripping".Localized());

            Scene.OutputText("Scenes.LustyMaiden.NipplesSensitive".Localized());

            Scene.OutputText((virgin ? "Scenes.LustyMaiden.MaidenheadWord" : "Scenes.LustyMaiden.SloppyGashWord").Localized());

            Scene.OutputText("Scenes.LustyMaiden.Hotter".Localized());

            Scene.OutputText("Scenes.LustyMaiden.BelowYou".Localized());

            if (monster.CockArea(0) >= 80)
                Scene.OutputText("Scenes.LustyMaiden.MessyCough".Localized());

            Scene.OutputText("Scenes.LustyMaiden.TeaseCommand".Localized(monster.Mf("boy", "girl")));

            Scene.OutputText("Scenes.LustyMaiden.GroansAndShakes".Localized());

            if (monster.Balls > 0)
                Scene.OutputText("Scenes.LustyMaiden.BallsPumping".Localized());

            Scene.OutputText("Scenes.LustyMaiden.Urethra".Localized());

            Scene.OutputText("Scenes.LustyMaiden.HoldOn".Localized());

            Scene.OutputText("Scenes.LustyMaiden.Seize".Localized(monster.Mf("His", "Her"), cock));

            Scene.OutputText("Scenes.LustyMaiden.FinalGlow".Localized());

            player.SlimeFeed();

            var bonus = CoC.Instance.Flags.Get(GameFlagIds.BikiniArmorBonus) + 2;

            CoC.Instance.Flags.Set(GameFlagIds.BikiniArmorBonus, Math.Min(bonus, 8));

            player.Orgasm("Vaginal");

            player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

            if (virgin)
                player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

            Scene.AddButton(0, "Scenes.Explore.Continue".Localized(), () => Scene.Go<CampScene>());
        }
    }
}