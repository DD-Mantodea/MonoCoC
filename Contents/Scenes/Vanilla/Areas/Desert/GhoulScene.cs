using MonoCoC.Attributes;

using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Desert
{
    [Register<GhoulScene>("coc:ghoul")]
    public class GhoulScene : Scene
    {

        public static string FaerieHairColor = "";

        public static string FaerieOutfit = "";

        public static readonly Dictionary<string, string> outfits = new()
{
    ["nude"] = "bare, exposed body",
    ["straps"] = "solid black stockings and leather chest-wraps",
    ["leaf"] = "leafy wrappings",
    ["petal"] = "chest-wrap and tiny skirt of vibrant petals",
};

        public override void Entry()
        {
            GhoulEncounter();
        }

        public void GhoulEncounter()
        {
            ClearOutput();

            ShowImage("event-hyena");

            OutputText("Scenes.Ghoulscene.GhoulEncounter.HyenaSpottingIntro".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesExploredPlains) > 0)
                OutputText("Scenes.Ghoulscene.GhoulEncounter.LocatedAtPlains".Localized());
            else
                OutputText("Scenes.Ghoulscene.GhoulEncounter.LocatedElsewhere".Localized());

            OutputText("Scenes.Ghoulscene.GhoulEncounter.HyenaChargeBegins".Localized());

            var enemy = EnemyRegistry.Create(EnemyIds.Ghoul);

            enemy.OnDefeated = hpVictory =>
            {
                ClearOutput();

                OutputText("Scenes.Ghoul.Defeated.ScreechVanishes".Localized());

                CombatScene.CleanupAfterCombat();
            };

            enemy.OnWon = hpVictory => GhoulWon();

            CombatScene.Start(enemy, () => Go<CampScene>());
        }

        public void GhoulWon()
        {
            ClearOutput();

            CombatScene.CleanupAfterCombat();

            ShowImage("monster-ghoul");

            if (Player.Health <= 0)
                OutputText("Scenes.Ghoulscene.GhoulWon.DefeatFromWounds".Localized());
            else
                OutputText("Scenes.Ghoulscene.GhoulWon.DefeatFromLust".Localized());

            OutputText("Scenes.Ghoulscene.GhoulWon.GhoulLunges".Localized());

            OutputText("Scenes.Ghoulscene.GhoulWon.WakingAfterAttack".Localized());

            Player.ApplyStats(new DynStat(GameStats.Strength, -2f));

            Player.ApplyStats(new DynStat(GameStats.Toughness, -3f));

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 3f));

            DoNext(() => ReturnToCamp(4));
        }

    }
}