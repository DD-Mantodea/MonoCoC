using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{
    [Register<GoblinScene>("coc:goblin_scene")]
    [Register<GoblinScene>("coc:goblin_victory")]
    [Register<GoblinScene>("coc:goblin_defeat")]
    public class GoblinScene : Scene
    {
        public override void Entry()
        {
            if (CombatScene.LastOutcome is CombatOutcome.HpVictory or CombatOutcome.LustVictory)
                Victory();
            else
                Defeat();
        }

        public static void Victory()
        {
            ClearOutput();

            if (CombatScene.LastOutcome == CombatOutcome.HpVictory)
                OutputText("Scenes.Goblin.VictoryHp".Localized());
            else
            {
                OutputText("Scenes.Goblin.VictoryLust".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 20));
            }

            if (Player.Lust < 33)
                OutputText("Scenes.Goblin.NotHorny".Localized());

            GenerateGobboSexMenu();
        }

        public static void GenerateGobboSexMenu()
        {
            ClearButtons();

            AddButtonDisabled(0, "Scenes.Goblin.Button.DickFuck".Localized(), "Scenes.Goblin.Hint.FittingCock".Localized(), "Scenes.Goblin.Button.DickFuck".Localized());

            AddButtonDisabled(1, "Scenes.Goblin.Button.DickTooBig".Localized(), "Scenes.Goblin.Hint.LargeCock".Localized(), "Scenes.Goblin.Button.DickTooBig".Localized());

            AddButtonDisabled(2, "Scenes.Goblin.Button.CorruptDick".Localized(), "Scenes.Goblin.Hint.LargeCockCorrupt".Localized(), "Scenes.Goblin.Button.CorruptDick".Localized());

            AddButtonDisabled(3, "Scenes.Goblin.Button.DickInAss".Localized(), "Scenes.Goblin.Hint.CockCorrupt".Localized(), "Scenes.Goblin.Button.DickInAss".Localized());

            AddButtonDisabled(4, "Scenes.Goblin.Button.JogFuck".Localized(), "Scenes.Goblin.Hint.FittingCock".Localized(), "Scenes.Goblin.Button.JogFuck".Localized());

            AddButtonDisabled(5, "Scenes.Goblin.Button.Breastfeed".Localized(), "Scenes.Goblin.Hint.Milk".Localized(), "Scenes.Goblin.Button.Breastfeed".Localized());

            AddButtonDisabled(6, "Scenes.Goblin.Button.UseCondom".Localized(), "Scenes.Goblin.Hint.Condom".Localized(), "Scenes.Goblin.Button.UseCondom".Localized());

            AddButtonDisabled(7, "Scenes.Goblin.Button.Pussies".Localized(), "Scenes.Goblin.Hint.Vagina".Localized(), "Scenes.Goblin.Button.Pussies".Localized());

            AddButtonDisabled(8, "Scenes.Goblin.Button.LayEggs".Localized(), "Scenes.Goblin.Hint.Eggs".Localized(), "Scenes.Goblin.Button.LayEggs".Localized());

            AddButtonDisabled(9, "Scenes.Goblin.Button.FacefuckPiss".Localized(), "Scenes.Goblin.Hint.Watersports".Localized());

            AddButtonDisabled(10, "Scenes.Goblin.Button.WombFuck".Localized(), "Scenes.Goblin.Hint.Gore".Localized());

            if (Player.BreastRows.Count > 0 &&
                (Player.LactationQ() >= 500 || Player.HasStatusEffect(StatusEffectIds.Feeder)))

                AddButton(5, "Scenes.Goblin.Button.Breastfeed".Localized(), GiveGoblinAMilkMustache);

            var capacity = CombatScene.LastEnemy?.VaginalCapacity() ?? 0;

            if (Player.Lust >= 33 &&
                Player.Cocks.Count > 0 &&

                Player.CockThatFits(capacity) >= 0)

                AddButton(0, "Scenes.Goblin.Button.DickFuck".Localized(), GatsGoblinBoners);

            if (Player.Lust >= 33 &&
                Player.Cocks.Count > 0 &&

                Player.CockThatFits(capacity) >= 0 &&

                Player.Inventory.HasItem(ItemRegistry.Create(new Identifier("coc", "condom"))))

                AddButton(6, "Scenes.Goblin.Button.UseCondom".Localized(), GoblinCondomed);

            if (Player.Lust >= 33 && Player.HasVagina())
                if (Player.IsTaur() || Player.IsNaga())
                    AddButtonDisabled(7, "Scenes.Goblin.Button.Pussies".Localized(), "Scenes.Goblin.Hint.BodyType".Localized(), "Scenes.Goblin.Button.Pussies".Localized());
                else if (Player.IsChild())
                    AddButtonDisabled(7, "Scenes.Goblin.Button.Pussies".Localized(), "Scenes.Goblin.Hint.Loli".Localized(), "Scenes.Goblin.Button.Pussies".Localized());
                else
                    AddButton(7, "Scenes.Goblin.Button.Pussies".Localized(), GobboGetsRapedFem);

            if (Player.Lust >= 33 && Player.CanOvipositSpider())
                AddButton(8, "Scenes.Goblin.Button.LayEggs".Localized(), LaySomeDriderEggsInGobboTwat);

            if (Player.Lust >= 33 && Player.Cocks.Count > 0)
            {
                var biggest = Player.BiggestCockIndex();

                if (Player.CockArea(biggest) > capacity)
                    AddButton(1, "Scenes.Goblin.Button.DickTooBig".Localized(), ManRapesGoblinTooBig);

                if (Player.CockThatFits(capacity) >= 0)
                    AddButton(4, "Scenes.Goblin.Button.JogFuck".Localized(), JogFuck);

                if (Player.CockArea(biggest) > capacity &&
                    (Player.Corruption >= 80 || Player.HasPerk(PerkIds.Sadist)))

                    AddButton(2, "Scenes.Goblin.Button.CorruptDick".Localized(), RapeAGoblinCorruptTooBig);

                if (CombatScene.LastEnemy?.AnalCapacity() is { } analCapacity &&
                    Player.CockThatFits(analCapacity) >= 0 &&

                    (Player.Corruption >= 70 || Player.HasPerk(PerkIds.Sadist)))

                    AddButton(3, "Scenes.Goblin.Button.DickInAss".Localized(), GobboButtSecks);
            }

            var enemyGender = CombatScene.LastEnemy?.Gender switch
            {
                Gender.Male => 1,

                Gender.Female => 2,

                _ => 0
            };

            if (Player.PlayerResistSex(enemyGender))
                AddButton(14, "Common.Leave".Localized(), Leave);
            else
                AddButtonDisabled(14, "Common.Leave".Localized(), "You can't resist the allure of sex!");
        }

        public static void GiveGoblinAMilkMustache()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Goblin.Milk".Localized(
                Appearance.BiggestBreastSizeDescript(Player),

                Player.Mf("stud", "slut")));

            OutputText("Scenes.Goblin.MilkAgain".Localized(
                Player.NippleDescript(0),

                Player.Mf("stud", "slut")));

            OutputText("Scenes.Goblin.MilkThird".Localized());

            Player.ApplyStats(
                new DynStat(GameStats.Libido, 0.2f),

                new DynStat(GameStats.Lust, -50));

            var feeder = Player.CreateStatusEffect(StatusEffectIds.Feeder);

            feeder.Values[0] += 1;

            feeder.Values[1] = 0;

            CombatScene.CleanupAfterCombat();
        }

        public static void ManRapesGoblinTooBig()
        {
            var index = Player.BiggestCockIndex();

            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Goblin.TooBig".Localized());

            if (Player.CockArea(index) < 80)
                OutputText("Scenes.Goblin.TooBigNearly".Localized());

            OutputText("Scenes.Goblin.TooBigAgain".Localized());

            OutputText((Player.Corruption < 50

                ? "Scenes.Goblin.TooBigCorLow"

                : "Scenes.Goblin.TooBigCorHigh").Localized());

            OutputText((Player.Cocks[0].Id.Path == "horse"

                ? "Scenes.Goblin.TooBigFlare"

                : "Scenes.Goblin.TooBigHead").Localized(Player.CockDescript(index)));

            OutputText((Player.Cocks[0].Id.Path == "horse"

                ? "Scenes.Goblin.TooBigCrownFlare"

                : "Scenes.Goblin.TooBigCrown").Localized());

            OutputText("Scenes.Goblin.TooBigThird".Localized(Player.CockDescript(index)));

            if (Player.Cocks[0].KnotMultiplier > 1)
                OutputText("Scenes.Goblin.TooBigKnot".Localized());

            OutputText("Scenes.Goblin.TooBigFourth".Localized());

            OutputText((Player.Gender == Gender.Hermaphrodite

                ? "Scenes.Goblin.TooBigHerm"

                : "Scenes.Goblin.TooBigMan").Localized());

            OutputText("Scenes.Goblin.TooBigFifth".Localized(Player.CockDescript(index)));

            OutputText((Player.CumQ() < 75

                ? "Scenes.Goblin.TooBigCumLow"

                : Player.CumQ() < 250

                    ? "Scenes.Goblin.TooBigCumMid"

                    : "Scenes.Goblin.TooBigCumHigh").Localized());

            OutputText("Scenes.Goblin.TooBigSixth".Localized());

            AddButton(0, "Common.Continue".Localized(), () =>
            {
                Player.Orgasm("Dick");

                CombatScene.CleanupAfterCombat();
            });
        }

        public static void RapeAGoblinCorruptTooBig()
        {
            var index = Player.BiggestCockIndex();

            var cock = Player.CockDescript(index);

            var cocks = Player.CockTotal() > 1 ? Player.MultiCockDescriptLight() : cock;

            var head = Player.CockHead(index);

            var vagina = CombatScene.LastEnemy?.VaginaDescript(0) ?? "pussy";

            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Goblin.Corrupt".Localized(
                cocks,

                Player.CockTotal() == 1 ? "Common.Text.Is".Localized() : "Common.Text.AreAll".Localized(),

                Player.CockTotal() > 1 ? "Scenes.Lottie.AnalLottieMilkshake.OneOfYourCocks".Localized() : "Common.Text.Your".Localized() + cock,

                Player.Cocks[0].Length < 20 ? "Common.Text.AboutAsLongAsHerEntire".Localized() : "Common.Text.BiggerThanSheIs".Localized()));

            OutputText("Scenes.Goblin.CorruptAgain".Localized(cock, head, vagina));

            OutputText("Scenes.Goblin.CorruptThird".Localized(vagina));

            OutputText("Scenes.Goblin.CorruptFourth".Localized(vagina));

            OutputText("Scenes.Goblin.CorruptFifth".Localized(cock, vagina));

            OutputText("Scenes.Goblin.CorruptSixth".Localized());

            OutputText("Scenes.Goblin.CorruptSeventh".Localized(cock,

                Player.Cocks[0].Length < 20

                    ? "can feel the massive bulge in her midsection"

                    : "stretch her so much that you can see the bulge even from this angle"));

            if (Player.BiggestBreastSize() >= 1)
            {
                OutputText("Scenes.Goblin.Corrupt7Breast".Localized());

                if (Player.BiggestLactation() >= 1)
                    OutputText((Player.BiggestLactation() < 2

                        ? "Scenes.Goblin.Corrupt7MilkLight"

                        : Player.BiggestLactation() < 3

                            ? "Scenes.Goblin.Corrupt7MilkMedium"

                            : "Scenes.Goblin.Corrupt7MilkHeavy").Localized());
            }

            OutputText("Scenes.Goblin.Corrupt8Start".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Goblin.Corrupt8Vagina".Localized(Player.VaginaDescript(0)));

                OutputText((Player.Vaginas[0].Wetness <= 3

                    ? "Scenes.Goblin.Corrupt8Juices"

                    : Player.Vaginas[0].Wetness <= 4

                        ? "Scenes.Goblin.Corrupt8Floods"

                        : "Scenes.Goblin.Corrupt8Explodes").Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Goblin.Corrupt8OtherCocks".Localized());
            }
            else if (Player.CockTotal() > 1)
                OutputText((Player.CockTotal() > 2

                    ? "Scenes.Goblin.Corrupt8OtherPlural"

                    : "Scenes.Goblin.Corrupt8OtherSingle").Localized());

            OutputText("Scenes.Goblin.Corrupt8End".Localized());

            OutputText("Scenes.Goblin.CorruptNinth".Localized(vagina, cock,

                Player.Balls >= 2 ? "Your [balls] are trying but, h" : "H"));

            OutputText("Scenes.Goblin.Corrupt10Start".Localized(cock));

            if (Player.CockTotal() > 2)
                OutputText("Scenes.Goblin.Corrupt10MultiPlural".Localized());
            else if (Player.CockTotal() == 2)
                OutputText("Scenes.Goblin.Corrupt10MultiSingle".Localized());

            OutputText("Scenes.Goblin.Corrupt10End".Localized(cock));

            AddButton(0, "Common.Continue".Localized(), () =>
            {
                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Corruption, 1));

                CombatScene.CleanupAfterCombat();
            });
        }

        public static void GobboButtSecks()
        {
            var index = Player.CockThatFits(CombatScene.LastEnemy?.AnalCapacity() ?? 0);

            if (index < 0)
                index = 0;

            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Goblin.Butt".Localized());

            OutputText("Scenes.Goblin.ButtAgain".Localized());

            OutputText("Scenes.Goblin.ButtThird".Localized(Player.CockDescript(index)));

            OutputText("Scenes.Goblin.ButtFourth".Localized(Player.CockDescript(index)));

            OutputText("Scenes.Goblin.ButtFifth".Localized());

            OutputText("Scenes.Goblin.ButtSixth".Localized(Player.CockDescript(index)));

            OutputText((Player.Balls > 0

                ? "Scenes.Goblin.ButtSack"

                : "Scenes.Goblin.ButtHips").Localized(
                    Player.CockDescript(index),

                    Player.Balls > 0 ? Player.SackDescript() : Player.HipDescript()));

            OutputText("Scenes.Goblin.ButtSeventh".Localized());

            OutputText("Scenes.Goblin.ButtEighth".Localized());

            AddButton(0, "Common.Continue".Localized(), () =>
            {
                Player.Orgasm("Dick");

                CombatScene.CleanupAfterCombat();
            });
        }

        public static void GobboGetsRapedFem()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Goblin.Pussies".Localized());

            OutputText((Player.Corruption < 33

                ? "Scenes.Goblin.PussiesCorLow"

                : Player.Corruption < 66

                    ? "Scenes.Goblin.PussiesCorMid"

                    : "Scenes.Goblin.PussiesCorHigh").Localized());

            OutputText("Scenes.Goblin.PussiesAgain".Localized());

            Player.CuntChange(Player.VaginalCapacity(), true);

            OutputText("Scenes.Goblin.PussiesThird".Localized(Player.VaginaDescript(0)));

            if (Player.GetClitLength() >= 7)
                OutputText("Scenes.Goblin.PussiesClit".Localized());

            OutputText("Scenes.Goblin.PussiesFourth".Localized(Player.VaginaDescript(0)));

            OutputText("Scenes.Goblin.PussiesFifth".Localized(Player.VaginaDescript(0)));

            OutputText((Player.Vaginas[0].Looseness <= 3

                ? "Scenes.Goblin.PussiesGapeSmall"

                : "Scenes.Goblin.PussiesGapeLarge").Localized());

            OutputText("Scenes.Goblin.PussiesSixth".Localized());

            AddButton(0, "Common.Continue".Localized(), () =>
            {
                Player.Orgasm("Vaginal");

                CombatScene.CleanupAfterCombat();
            });
        }

        public static void GoblinCondomed()
        {
            var capacity = CombatScene.LastEnemy?.VaginalCapacity() ?? 0;

            var index = Player.CockThatFits(capacity);

            if (index < 0)
                return;

            var cock = Player.CockDescript(index);

            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Goblin.Condom".Localized());

            OutputText("Scenes.Goblin.CondomAgain".Localized(cock));

            OutputText((Player.Balls > 0

                ? "Scenes.Goblin.CondomBalls"

                : "Scenes.Goblin.CondomBase").Localized());

            OutputText("Scenes.Goblin.CondomThird".Localized());

            OutputText("Scenes.Goblin.CondomFourth".Localized(cock));

            OutputText("Scenes.Goblin.Condom4End".Localized());

            OutputText("Scenes.Goblin.CondomFifth".Localized(cock));

            OutputText("Scenes.Goblin.CondomSixth".Localized(cock));

            OutputText("Scenes.Goblin.CondomSeventh".Localized(cock));

            OutputText("Scenes.Goblin.Condom7Latex".Localized());

            OutputText("Scenes.Goblin.Condom7End".Localized());

            OutputText("Scenes.Goblin.CondomEighth".Localized());

            OutputText("Scenes.Goblin.CondomNinth".Localized());

            AddButton(0, "Common.Continue".Localized(), () =>
            {
                RemoveCondom();

                Player.Orgasm("Dick");

                CombatScene.CleanupAfterCombat();
            });
        }

        public static void RemoveCondom()
        {
            var condom = ItemRegistry.Create(new Identifier("coc", "condom"));

            foreach (var slot in Player.Inventory.Slots)
            {
                if (slot.IsEmpty || slot.Item.Id != condom.Id)
                    continue;

                slot.RemoveOneItem();

                return;
            }
        }

        public static void GatsGoblinBoners()
        {
            var enemy = CombatScene.LastEnemy;

            var index = Player.CockThatFits(enemy?.AnalCapacity() ?? 0);

            if (index < 0)
                index = Player.SmallestCockIndex();

            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Goblin.Gats".Localized());

            OutputText("Scenes.Goblin.GatsAgain".Localized());

            OutputText("Scenes.Goblin.GatsThird".Localized(
                Player.CockDescript(index),

                Player.CockTotal() > 1 ? "s" : ""));

            OutputText("Scenes.Goblin.GatsFourth".Localized(
                Player.CockDescript(index),

                Player.CockTotal() > 1 ? "s" : "",

                Player.CockTotal() > 1 ? "Common.Text.Them".Localized() : "Common.Text.It".Localized()));

            OutputText((Player.CockTotal() == 1

                ? "Scenes.Goblin.GatsPlungeSingle"

                : Player.CockTotal() == 2

                    ? "Scenes.Goblin.GatsPlungeDouble"

                    : "Scenes.Goblin.GatsPlungeMulti").Localized());

            OutputText("Scenes.Goblin.GatsFifth".Localized());

            OutputText("Scenes.Goblin.GatsSixth".Localized(
                Player.CockDescript(index),

                Player.CockTotal() == 2 ? "s" : ""));

            OutputText("Scenes.Goblin.GatsSeventh".Localized(Player.CockDescript(index)));

            AddButton(0, "Common.Continue".Localized(), () =>
            {
                Player.Orgasm("Dick");

                CombatScene.CleanupAfterCombat();
            });
        }

        public static void LaySomeDriderEggsInGobboTwat()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Goblin.Eggs".Localized());

            OutputText((Player.Cocks.Count > 0

                ? "Scenes.Goblin.EggsHarden"

                : Player.HasVagina()

                    ? "Scenes.Goblin.EggsMoisten"

                    : "Scenes.Goblin.EggsTwitch").Localized());

            OutputText("Scenes.Goblin.EggsAgain".Localized());

            OutputText((Player.Femininity > 50

                ? "Scenes.Goblin.EggsFemHigh"

                : "Scenes.Goblin.EggsFemLow").Localized());

            OutputText("Scenes.Goblin.EggsThird".Localized());

            OutputText("Scenes.Goblin.EggsFourth".Localized());

            OutputText("Scenes.Goblin.EggsFifth".Localized());

            OutputText("Scenes.Goblin.EggsSixth".Localized());

            OutputText("Scenes.Goblin.EggsSeventh".Localized());

            OutputText("Scenes.Goblin.EggsEighth".Localized());

            if (Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Goblin.EggsOrgasmCock".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Goblin.EggsOrgasmBoth".Localized());
            }
            else if (Player.HasVagina())
                OutputText("Scenes.Goblin.EggsOrgasmVagina".Localized());

            OutputText("Scenes.Goblin.EggsTenth".Localized());

            OutputText("Scenes.Goblin.Eggs12".Localized());

            OutputText("Scenes.Goblin.Eggs13".Localized());

            AddButton(0, "Common.Continue".Localized(), () =>
            {
                Player.DumpEggs();

                Player.Orgasm("Ovi");

                CombatScene.CleanupAfterCombat();
            });
        }

        public static void JogFuck()
        {
            var capacity = CombatScene.LastEnemy?.VaginalCapacity() ?? 0;

            var index = Player.CockThatFits(capacity);

            if (index < 0)
                index = Player.BiggestCockIndex();

            var cock = Player.CockDescript(index);

            var chest = Player.BiggestBreastSize() >= 1 ? Player.AllBreastsDescript() : "torso";

            var chestThing = Player.BiggestBreastSize() >= 1 ? "Common.Text.Tits".Localized() : "Common.Text.Skin".Localized();

            var flavor = Player.Cocks[0].Id.Path == "demon"

                ? "rubbing painfully against your demonic nubs"

                : Player.Cocks[0].KnotMultiplier > 1

                    ? "stretching painfully around your knot"

                    : "feeling painfully tight around you";

            var fitFlavor = Player.Cocks[0].Id.Path == "demon"

                ? "rubbing perfectly against your demonic nubs"

                : Player.Cocks[0].KnotMultiplier > 1

                    ? "stretching tightly around your knot"

                    : "feeling absolutely perfect around you";

            var tight = Player.CockArea(index) > capacity * 0.8f;

            ClearOutput();

            ClearButtons();

            if (tight)
            {
                OutputText("Scenes.Goblin.JogTight".Localized(cock));

                OutputText("Scenes.Goblin.JogTightAgain".Localized(cock, flavor, chest));

                OutputText("Scenes.Goblin.JogTightThird".Localized());

                OutputText("Scenes.Goblin.JogTightFourth".Localized(cock, chestThing));

                OutputText("Scenes.Goblin.JogTightFifth".Localized());

                OutputText("Scenes.Goblin.JogTightSixth".Localized(cock));

                if (Player.Balls > 0)
                    OutputText("Scenes.Goblin.JogBalls".Localized());

                OutputText("Scenes.Goblin.JogTightSeventh".Localized());

                OutputText((Player.CumQ() >= 500

                    ? "Scenes.Goblin.JogCumThird"

                    : Player.CumQ() >= 250

                        ? "Scenes.Goblin.JogCumAgain"

                        : Player.CumQ() >= 100

                            ? "Scenes.Goblin.JogCum"

                            : "").Localized(cock));

                OutputText("Scenes.Goblin.JogTightEighth".Localized());
            }
            else
            {
                OutputText("Scenes.Goblin.JogFit".Localized(cock));

                OutputText("Scenes.Goblin.JogFitAgain".Localized(cock, fitFlavor, chest));

                OutputText("Scenes.Goblin.JogFitThird".Localized());

                OutputText("Scenes.Goblin.JogFitFourth".Localized(cock, chestThing));

                OutputText("Scenes.Goblin.JogFitFifth".Localized());

                OutputText("Scenes.Goblin.JogFitSixth".Localized(cock));

                if (Player.Balls > 0)
                    OutputText("Scenes.Goblin.JogBalls".Localized());

                OutputText("Scenes.Goblin.JogFitSeventh".Localized());

                OutputText((Player.CumQ() >= 500

                    ? "Scenes.Goblin.JogCumThird"

                    : Player.CumQ() >= 250

                        ? "Scenes.Goblin.JogCumAgain"

                        : "").Localized(cock));

                OutputText("Scenes.Goblin.JogFitEighth".Localized());
            }

            AddButton(0, "Common.Continue".Localized(), () =>
            {
                Player.Orgasm("Dick");

                CombatScene.CleanupAfterCombat();
            });
        }

        public static void Defeat()
        {
            ClearOutput();

            ClearButtons();

            OutputText((CombatScene.LastOutcome == CombatOutcome.HpDefeat

                ? "Scenes.Goblin.Defeat1Hp"

                : "Scenes.Goblin.Defeat1Lust").Localized());

            if (Player.Cocks.Count > 0)
            {
                var length = Player.Cocks[0].Length;

                OutputText((length <= 7

                    ? "Scenes.Goblin.DefeatCockSmall"

                    : length <= 14

                        ? "Scenes.Goblin.DefeatCockMedium"

                        : "Scenes.Goblin.DefeatCockLarge").Localized());
            }

            if (Player.Vaginas.Count > 0)
            {
                var wetness = Player.Vaginas[0].Wetness;

                OutputText((wetness <= 2

                    ? "Scenes.Goblin.DefeatVaginaDry"

                    : wetness < 4

                        ? "Scenes.Goblin.DefeatVaginaWet"

                        : "Scenes.Goblin.DefeatVaginaDrooling").Localized(Player.VaginaDescript(0)));
            }

            OutputText((Player.HasFuckableNipples()

                ? "Scenes.Goblin.DefeatNippleCunts"

                : "Scenes.Goblin.DefeatNipples").Localized());

            if (Player.IsShota())
            {
                var shotaCapacity = CombatScene.LastEnemy?.VaginalCapacity() ?? 0;

                if (Player.Cocks.Count > 0 && Player.CockThatFits(shotaCapacity) >= 0)
                {
                    DefeatShota();

                    return;
                }

                DefeatPlaceholder("Scenes.Goblin.Hint.Shota".Localized());

                return;
            }

            if (Player.HasPerk(PerkIds.BimboBrains) || Player.HasPerk(PerkIds.FutaFaculties))
            {
                DefeatPlaceholder("Scenes.Goblin.Hint.Bimbo".Localized());

                return;
            }

            var capacity = CombatScene.LastEnemy?.VaginalCapacity() ?? 0;

            if (Player.Cocks.Count > 0 && (!Player.HasVagina() || Random.Shared.Next(2) == 0))
            {
                if (Player.CockArea(0) > capacity)
                {
                    DefeatTooBig();

                    return;
                }

                DefeatMaleFits();

                return;
            }

            if (Player.HasVagina())
            {
                DefeatFemale();

                return;
            }

            DefeatPlaceholder("Scenes.Goblin.Hint.FemaleNoVagina".Localized());
        }

        public static void DefeatFemale()
        {
            var vagina = Player.VaginaDescript(0);

            OutputText("Scenes.Goblin.DefeatFemale".Localized());

            OutputText((Player.Corruption < 33

                ? "Scenes.Goblin.DefeatFemaleCorLow"

                : Player.Corruption < 66

                    ? "Scenes.Goblin.DefeatFemaleCorMid"

                    : "Scenes.Goblin.DefeatFemaleCorHigh").Localized());

            OutputText("Scenes.Goblin.DefeatFemaleAgain".Localized());

            Player.CuntChange(Player.VaginalCapacity(), true);

            OutputText("Scenes.Goblin.DefeatFemaleThird".Localized(vagina));

            if (Player.GetClitLength() >= 7)
                OutputText("Scenes.Goblin.DefeatFemaleClit".Localized());

            OutputText("Scenes.Goblin.DefeatFemaleFourth".Localized(vagina));

            OutputText("Scenes.Goblin.DefeatFemaleFifth".Localized(vagina));

            OutputText((Player.Vaginas[0].Looseness <= 3

                ? "Scenes.Goblin.DefeatFemaleGapeSmall"

                : "Scenes.Goblin.DefeatFemaleGapeLarge").Localized());

            OutputText("Scenes.Goblin.DefeatFemaleSixth".Localized());

            AddButton(0, "Common.Continue".Localized(), () =>
            {
                Player.Orgasm("Vaginal");

                CombatScene.CleanupAfterCombat();
            });
        }

        public static void DefeatPlaceholder(string reason)
        {
            OutputText("[pg]<b>" + reason + "Common.Text.B".Localized());

            AddButton(14, "Common.Leave".Localized(), Leave);
        }

        public static void DefeatShota()
        {
            var index = Player.CockThatFits(CombatScene.LastEnemy?.VaginalCapacity() ?? 0);

            if (index < 0)
                index = 0;

            var cock = Player.CockDescript(index);

            OutputText("Scenes.Goblin.DefeatShota".Localized(
                Player.Health <= 1 ? "Common.Text.YourInjuries".Localized() : "Common.Text.TheBurningSensationMakingYouFuzzy".Localized()));

            OutputText("Scenes.Goblin.DefeatShotaAgain".Localized());

            OutputText("Scenes.Goblin.DefeatShotaThird".Localized());

            OutputText((Player.Cocks[index].Length < 5

                ? "Scenes.Goblin.DefeatShotaSmall"

                : "Scenes.Goblin.DefeatShotaBig").Localized());

            OutputText("Scenes.Goblin.DefeatShotaFourth".Localized());

            OutputText("Scenes.Goblin.DefeatShotaFifth".Localized(
                Player.Balls > 0

                    ? "cup your [balls] in one hand, squeezing gently as the drug begins to affect them."

                    : "squeeze your taint in her hand, giggling as your prostate suddenly begins to swell from the drug."));

            OutputText("Scenes.Goblin.DefeatShotaSixth".Localized(cock));

            AddButton(0, "Common.Continue".Localized(), () =>
            {
                Player.Orgasm("Dick");

                CombatScene.CleanupAfterCombat();
            });
        }

        public static void DefeatTooBig()
        {
            var cock = Player.CockDescript(0);

            OutputText("Scenes.Goblin.DefeatTooBig".Localized(
                cock,

                Player.Balls > 0 ? "Common.Text.YourBalls".Localized() : "Scenes.Goblinscene.GoblinRapesPlayer.BaseOfYourCock".Localized()));

            OutputText("Scenes.Goblin.DefeatTooBigAgain".Localized(
                Player.Balls > 0 ? "Common.Text.YourBalls".Localized() : "Scenes.Goblinscene.GoblinRapesPlayer.BaseOfYourCock".Localized()));

            OutputText((Player.Corruption < 50

                ? "Scenes.Goblin.DefeatTooBig3Low"

                : "Scenes.Goblin.DefeatTooBig3High").Localized());

            OutputText("Scenes.Goblin.DefeatTooBigThird".Localized(
                Player.HasVagina() ? "Scenes.Goblinscene.GoblinRapesPlayer.CaressingYourClit".Localized() : "Scenes.Goblinscene.GoblinRapesPlayer.PressingAgainstProstate".Localized()));

            OutputText("Scenes.Goblin.DefeatTooBigFourth".Localized(cock));

            OutputText("");

            AddButton(0, "Common.Continue".Localized(), () =>
            {
                Player.Orgasm("Dick");

                CombatScene.CleanupAfterCombat();
            });
        }

        public static void DefeatMaleFits()
        {
            var cock = Player.CockDescript(0);

            OutputText("Scenes.Goblin.DefeatMale".Localized(
                cock,

                Player.Cocks[0].Id.Path == "horse" ? "Common.Text.Flare".Localized() : "Common.Text.CockHead".Localized()));

            OutputText("Scenes.Goblin.DefeatMaleAgain".Localized());

            OutputText("Scenes.Goblin.DefeatMaleThird".Localized());

            OutputText((Player.Corruption < 50

                ? "Scenes.Goblin.DefeatMale4Low"

                : "Scenes.Goblin.DefeatMale4High").Localized());

            OutputText("Scenes.Goblin.DefeatMaleFourth".Localized(
                Player.Balls > 0 ? "Scenes.Goblinscene.GoblinRapesPlayer.CupBallsOneHand".Localized() : "Scenes.Goblinscene.GoblinRapesPlayer.SqueezeTaintInHand".Localized()));

            OutputText("Scenes.Goblin.DefeatMaleFifth".Localized());

            AddButton(0, "Common.Continue".Localized(), () =>
            {
                Player.Orgasm("Dick");

                CombatScene.CleanupAfterCombat();
            });
        }

        public static void GoblinLoliScene()
        {
            ClearOutput();

            OutputText("Sauntering over, you seek relief in the poor little goblin." + Player.ClothedOrNakedLower(" You strip away your [armor]", " You prepare yourself") + "Common.Text.AsTheGreenShortstackStaresOn".Localized());

            OutputText("Scenes.Goblinscene.GoblinLoliScene.PushHerAroundOnto".Localized());

            OutputText("Scenes.Goblinscene.GoblinLoliScene.DisgruntledBossedAround".Localized());

            OutputText("Scenes.Goblinscene.GoblinLoliScene.ContinueRockingHips".Localized());

            OutputText("Scenes.Goblinscene.GoblinLoliScene.LeanOnHerBody".Localized());

            OutputText("Scenes.Goblinscene.GoblinLoliScene.SuccumbingToOral".Localized());

            CombatScene.CleanupAfterCombat();

            Player.Orgasm("Vaginal");

            Go<CampScene>();
        }

        public static void FacefuckPiss()
        {
            ClearOutput();

            OutputText("Scenes.Goblinscene.FacefuckPiss.GoblinDepleted".Localized());

            OutputText("Scenes.Goblinscene.FacefuckPiss.AfterFewMoments".Localized());

            OutputText("Scenes.Goblinscene.FacefuckPiss.SmirkToYourselfGrab".Localized());

            OutputText("Scenes.Goblinscene.FacefuckPiss.UrgeToFuckLittle".Localized());

            OutputText("Scenes.Goblinscene.FacefuckPiss.RightHandStillGrasps".Localized());

            OutputText("Scenes.Goblinscene.FacefuckPiss.SpittleDownBallsack".Localized());

            if (Player.Tallness >= 80)
                OutputText("Scenes.Goblinscene.FacefuckPiss.KeepingVoraciousPace".Localized());

            OutputText("Scenes.Goblinscene.FacefuckPiss.SoonReachYourPeak".Localized());

            OutputText("Scenes.Goblinscene.FacefuckPiss.SexualUrgeDimsDown".Localized());

            OutputText("Scenes.Goblinscene.FacefuckPiss.LastOfBladder".Localized());

            OutputText("Scenes.Goblinscene.FacefuckPiss.CatchBreath".Localized());

            Player.Orgasm("Dick");

            AwardAchievement(AchievementIds.UrineTrouble);

            CombatScene.CleanupAfterCombat();
        }

        public static void GobboWombFuck()
        {
            ClearOutput();

            OutputText("Scenes.Goblinscene.GobboWombFuck.SighAsYouApproach".Localized());

            OutputText("Scenes.Goblinscene.GobboWombFuck.GroansFromDizziness".Localized());

            OutputText("Scenes.Goblinscene.GobboWombFuck.ThenFillHerWomb".Localized());

            OutputText("Scenes.Goblinscene.GobboWombFuck.StopsShortWhenSees".Localized());

            OutputText("[pg]Using your " + (Player.IsNaga() ? "Common.Text.SnakeCoils".Localized() : "Common.Text.FreeHand".Localized()) + ", you grip the [monster.Short]'s neck and hold her still. You assure her you just want to fill her womb, just as she wished. Her worried eyes staring at the knife in your hand suggest she doesn't believe you. Actions will have to speak louder than words, then. You lunge the knife inside her stomach, forcing her to gag again. Slowly, you pull the blade down, opening her flesh around where you surmise her womb to be. True to your word, you pull out your [cock], pushing it against the gaping wound you've made. Her attempts to scream are mostly squelched by constricting her neck, however she does manage to make a rather large amount of noise sobbing.");

            OutputText("[pg]The warm blood pouring over your " + Appearance.CockNoun(Player) + "Common.Text.IsSoothingYetExcitingEmboldeningYour".Localized());

            OutputText("Scenes.Goblinscene.GobboWombFuck.HaltsHerBegging".Localized());

            OutputText("Scenes.Goblinscene.GobboWombFuck.SpasmingMusclesCause".Localized());

            OutputText("Scenes.Goblinscene.GobboWombFuck.SobbingQuietsDown".Localized());

            OutputText("Scenes.Goblinscene.GobboWombFuck.CatchBreathAndWipe".Localized());

            Player.Orgasm("Dick");

            if (!Player.HasPerk(PerkIds.HistoryDEUSVULT))
                Player.ApplyStats(new DynStat(GameStats.Corruption, 6f));

            CoC.Instance.Flags.Set(GameFlagIds.GoblinsKilled, CoC.Instance.Flags.Get(GameFlagIds.GoblinsKilled) + 1);

            CombatScene.CleanupAfterCombat();
        }

        public static void Leave() => Go<CampScene>();
    }
}