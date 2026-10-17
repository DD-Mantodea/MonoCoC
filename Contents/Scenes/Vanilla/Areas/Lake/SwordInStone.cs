using MonoCoC.Attributes;

using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Lake
{
    [Register<SwordInStone>("coc:sword_in_stone")]
    public class SwordInStone : Scene
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
            ExecEncounter();
        }

        public void ExecEncounter()
        {

            ShowImage("event-lake-bSword");

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) < 2)
            {
                OutputText("Scenes.Swordinstone.ExecEncounter.GlintCombatStance".Localized());

                OutputText("Scenes.Swordinstone.ExecEncounter.BladeEncasedTree".Localized());

                OutputText("Scenes.Swordinstone.ExecEncounter.PullFreePrompt".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), () => TryToTakeSwordInStone()); AddButton(1, "No", () => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Swordinstone.ExecEncounter.MassiveTreeSight".Localized());

                OutputText("Scenes.Swordinstone.ExecEncounter.GatherPiecesPrompt".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), () => TakeBrokenSword()); AddButton(1, "No", () => ReturnToCamp(1));

                Player.CreateStatusEffect(StatusEffectIds.BSwordBroken, 0, 0, 0, 0);
            }
        }

        public void TryToTakeSwordInStone()
        {
            ClearOutput();

            ShowImage("item-bSword");

            if (!Player.IsPureEnough(25))
            {
                OutputText("Scenes.Swordinstone.TryToTakeSwordInStone.GripHandle".Localized());

                if (Player.Strength > 70)
                    OutputText("Scenes.Swordinstone.TryToTakeSwordInStone.PullMightily".Localized());

                if (Player.Strength <= 70 && Player.Strength >= 40)
                    OutputText("Scenes.Swordinstone.TryToTakeSwordInStone.PullHard".Localized());

                if (Player.Strength < 40)
                    OutputText("Scenes.Swordinstone.TryToTakeSwordInStone.PullDesperate".Localized());

                OutputText("Scenes.Swordinstone.TryToTakeSwordInStone.SwordStaysLodged".Localized());

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Swordinstone.TryToTakeSwordInStone.GripHandlePure".Localized());

                if (Player.Strength > 70)
                    OutputText("Scenes.Swordinstone.TryToTakeSwordInStone.FallsOnAss".Localized());

                if (Player.Strength <= 70 && Player.Strength >= 40)
                    OutputText("Scenes.Swordinstone.TryToTakeSwordInStone.SwordSlidesFree".Localized());

                if (Player.Strength < 40)
                    OutputText("Scenes.Swordinstone.TryToTakeSwordInStone.SwordEasilyFree".Localized());

                OutputText("Scenes.Swordinstone.TryToTakeSwordInStone.TrunkIntactLeaf".Localized());

                OutputText("Scenes.Swordinstone.TryToTakeSwordInStone.BladeDescription".Localized());

                Player.ApplyStats(new DynStat(GameStats.Libido, (float)(-(Player.Libido / 3))), new DynStat(GameStats.Lust, -15f));

                InventoryScene.TakeItem(ItemRegistry.Create(WeaponIds.BeautifulSword), () => ReturnToCamp(1));

                Player.CreateStatusEffect(StatusEffectIds.TookBlessedSword, 0, 0, 0, 0);
            }
        }

        public void TakeBrokenSword()
        {
            ClearOutput();

            ShowImage("item-brokenSword");

            OutputText("Scenes.Swordinstone.TakeBrokenSword.BrokenSwordInspected".Localized());

            OutputText("Scenes.Swordinstone.TakeBrokenSword.RestoreIdea".Localized());

            InventoryScene.TakeItem(ItemRegistry.Create(WeaponIds.BeautifulSword), () => ReturnToCamp(1));
        }

    }
}