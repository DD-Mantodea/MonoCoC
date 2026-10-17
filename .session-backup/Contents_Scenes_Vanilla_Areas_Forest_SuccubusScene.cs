// 原版 classes/scenes/monsters/SuccubusScene.hx（96 行）的 C# 移植。

using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using MonoCoC.OptionSystem;
using System;
using System.Linq;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Contents.Npcs;
using MonoCoC.Contents.Internals;
using System.Collections.Generic;
using MonoCoC.Utils;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{
    [Register("coc:succubus_scene")]
    public class SuccubusScene : Scene
    {



        private static bool noFur => !Player.HasFur();

        private static bool silly => OptionStore.Instance.Get("silly", 0) > 0;

        private static bool metric => OptionStore.Instance.Get("metric", 0) > 0;

        private static bool realistic => OptionStore.Instance.Get("realistic", 0) > 0;

        private static bool survival => OptionStore.Instance.Get("survival", 0) > 0;

        private static bool hasKnot => Player.Cocks.Count > 0 && Player.Cocks.Any(c => c.KnotMultiplier > 1);

        private static void ReturnToCamp(int hours) => Go(SceneRegistry.CreateOrDefault("coc:camp"));

        public override void Entry()
        {
            // no entry method in the original class (battle-branch scene): entered from area encounters
        }

        // New

        // Haxe EncounterSuccubus()   outputText 3 
        public void EncounterSuccubus()
        {
            ClearOutput();

            OutputText("Scenes.Succubusscene.EncounterSuccubus.IvorySuccubusSpotted".Localized());
            Player.UnlockCodexEntry(VanillaFlags.CodexEntrySuccubus);
            if (Player.HasCock())
            {
                OutputText("Scenes.Succubusscene.EncounterSuccubus.CockOffer".Localized());
                ClearButtons();
                AddButton(0, "Fight", FightAgainstSuccubus);
                AddButton(1, "Accept", () => LoseToSuccubus(false));
                AddButton(4, "Run", TryToFlee);
            }
            else
            {
                OutputText("Scenes.Succubusscene.EncounterSuccubus.ChargesIntoFight".Localized());
                CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "succubus")), () => Go<Camp>());
            }
        }

        // Haxe FightAgainstSuccubus()   outputText 0 
        public void FightAgainstSuccubus()
        {

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "succubus")), () => Go<Camp>(), canFlee: false);
        }

        // Haxe WinAgainstSuccubus()   outputText 2 
        public void WinAgainstSuccubus()
        {
            ClearOutput();

            OutputText("Scenes.Succubusscene.WinAgainstSuccubus.FallsToKnees".Localized()
                (CombatController.Instance?.Context.Enemy.Health ?? 0) <= 0 ? "beaten" : "aroused");
            if (Player.HasCock() && Player.Lust >= 33)
            {
                OutputText("Scenes.Succubusscene.WinAgainstSuccubus.RapePrompt".Localized());
                AddButton(0, "Rape Her", WinAgainstSuccubusRape);
                AddButton(14, "Leave", () => ReturnToCamp(1));
            }
            else
            {
                CombatScene.CleanupAfterCombat();
            }
        }

        // Haxe WinAgainstSuccubusRape()   outputText 7 
        public void WinAgainstSuccubusRape()
        {
            ClearOutput();

            OutputText("Scenes.Succubusscene.WinAgainstSuccubusRape.MovesOverStripping".Localized(
                Player.ClothedOrNakedLower(", stripping out of your " + Player.ArmorDescript() + ", exposing your " + Player.CockDescript() + "", " while your " + Player.CockDescript() + " swings freely")
            ));
            OutputText("Scenes.Succubusscene.WinAgainstSuccubusRape.GrindPelvis".Localized());
            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Succubusscene.WinAgainstSuccubusRape.AllCocksThrust".Localized());
            }
            else
            {
                OutputText("Scenes.Succubusscene.WinAgainstSuccubusRape.SingleCockThrust".Localized());
            }
            OutputText("Scenes.Succubusscene.WinAgainstSuccubusRape.HugeLoad".Localized());
            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Succubusscene.WinAgainstSuccubusRape.SecondCockCram".Localized());
            }
            OutputText("Scenes.Succubusscene.WinAgainstSuccubusRape.SlowRapeGloat".Localized());
            Player.Orgasm("Generic");
            Player.ApplyStats(new DynStat(VanillaStats.Libido, -2f), new DynStat(VanillaStats.Corruption, 3f));
            CombatScene.CleanupAfterCombat();
        }

        // Haxe LoseToSuccubus()   outputText 6 
        public void LoseToSuccubus(bool fromBattle = true)
        {
            ClearOutput();

            if (fromBattle)
            {
                OutputText("Scenes.Succubusscene.LoseToSuccubus.GivesIn".Localized()
                    Player.Health <= 0 ? "beaten" : "aroused");
            }
            if (Player.HasCock())
            {
                OutputText("Scenes.Succubusscene.LoseToSuccubus.KissDeepThroat".Localized());
                OutputText("Scenes.Succubusscene.LoseToSuccubus.RidesYou".Localized());
                OutputText("Scenes.Succubusscene.LoseToSuccubus.AwakensExhausted".Localized());
                Player.Orgasm("Generic");
                Player.ApplyStats(new DynStat(VanillaStats.Libido, -2f), new DynStat(VanillaStats.Corruption, 3f));
                if (fromBattle)
                {
                    CombatScene.CleanupAfterCombat();
                }
                else
                {
                    ReturnToCamp(2);
                }
            }
            else
            {
                OutputText("Scenes.Succubusscene.LoseToSuccubus.CaressesFace".Localized()
                    Player.Libido + Player.Corruption < 80 ? "reluctantly" : "eagerly");
                OutputText("Scenes.Succubusscene.LoseToSuccubus.LeavesBottle".Localized());
                Player.Orgasm("Generic");
                Player.ApplyStats(new DynStat(VanillaStats.Libido, -2f), new DynStat(VanillaStats.Corruption, 3f));
                Player.Inventory.Add(ItemRegistry.Create(VanillaConsumables.IncubiDraft), 1);
CombatScene.CleanupAfterCombat();
            }
        }

        // Haxe TryToFlee()   outputText 0 
        public void TryToFlee()
        {
            ClearOutput();

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "succubus")), () => Go<Camp>());

            // 原版 tryToFlee：startCombat(new Succubus()); combat.runAway()（SuccubusScene.hx:90-94）。
            CombatController.Instance!.Run();
        }

    }
}
