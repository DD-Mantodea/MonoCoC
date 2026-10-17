using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{
    [Register<CorruptedGladeScene>("coc:corrupted_glade")]
    public class CorruptedGladeScene : Scene
    {
        public override void Entry()
        {
            if (Random.Shared.Next(4) == 0)
                TrappedSatyr();
            else
                Intro();
        }

        public static void Intro()
        {
            ClearOutput();

            ClearButtons();

            var effectiveCorruption = Player.Corruption + Player.CorruptionTolerance();

            OutputText("Scenes.CorruptedGlade.Intro".Localized());

            if (effectiveCorruption <= 33)
            {
                if (Player.Speed < 60 && Random.Shared.Next((int)Player.Speed + 50) < 50)
                {
                    OutputText("Scenes.CorruptedGlade.LowDripped".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 20 + Player.Libido / 5));
                }
                else
                {
                    OutputText("Scenes.CorruptedGlade.LowAvoid".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 2));
                }

                OutputText("Scenes.CorruptedGlade.DestroyHint".Localized());

                AddButton(1, "Scenes.CorruptedGlade.Destroy".Localized(), DestroyChoice);

                AddButton(0, "Common.Continue".Localized(), () => ReturnToCamp(1));

                return;
            }

            if (effectiveCorruption <= 66)
            {
                OutputText("Scenes.CorruptedGlade.MidIntro".Localized());

                OutputText(Random.Shared.Next(3) switch
                {
                    0 => "Scenes.CorruptedGlade.MidFlowers".Localized(),

                    1 => "Scenes.CorruptedGlade.MidVines".Localized(),

                    _ => "Scenes.CorruptedGlade.MidTrees".Localized()
                });

                Player.ApplyStats(
                    new DynStat(GameStats.Lust, 20 + Player.Libido / 5),

                    new DynStat(GameStats.Corruption, 0.5f));

                OutputText("Scenes.CorruptedGlade.DestroyHint".Localized());

                AddButton(1, "Scenes.CorruptedGlade.Destroy".Localized(), DestroyChoice);

                AddButton(0, "Common.Continue".Localized(), () => ReturnToCamp(1));

                return;
            }

            OutputText("Scenes.CorruptedGlade.HighIntro".Localized());

            AddButton(0, "Scenes.CorruptedGlade.Flowers".Localized(), FlowerFun);

            AddButton(1, "Scenes.CorruptedGlade.Vines".Localized(), TentacleFun);

            AddButton(2, "Scenes.CorruptedGlade.Trees".Localized(), TreeBoobFun);

            AddButton(3, "Scenes.CorruptedGlade.Destroy".Localized(), DestroyChoice);

            AddButton(14, "Common.Back".Localized(), () => ReturnToCamp(1));
        }

        public static void DestroyChoice()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.CorruptedGlade.DestroyChoice".Localized());

            var slot = 0;

            if (Player.HasPerk(PerkIds.Dragonfire) ||
                Player.HasPerk(PerkIds.FireLord) ||

                Player.HasPerk(PerkIds.Hellfire))

                AddButton(slot++, "Scenes.CorruptedGlade.FireBreath".Localized(), () => Destroy(0));

            if (Player.HasPerk(PerkIds.EnlightenedNinetails) ||
                Player.HasPerk(PerkIds.CorruptedNinetails))

                AddButton(slot++, "Scenes.CorruptedGlade.FoxFire".Localized(), () => Destroy(1));

            if (Player.HasStatusEffect(StatusEffectIds.KnowsWhitefire))
                AddButton(slot++, "Scenes.CorruptedGlade.Whitefire".Localized(), () => Destroy(2));

            if (Player.HasKeyItem(KeyItemIds.CarpentersToolbox) || Player.Weapon.Id.Path == "large_axe")
                AddButton(slot++, "Scenes.CorruptedGlade.Axe".Localized(), () => Destroy(3));

            if (Player.WeaponVerb is "stab" or "slash" or "cleave" or "keen cut")
                AddButton(slot++, "Scenes.CorruptedGlade.Weapon".Localized(), () => Destroy(4));

            AddButton(slot, "Scenes.CorruptedGlade.Hands".Localized(), () => Destroy(5));

            AddButton(14, "Scenes.CorruptedGlade.NeverMind".Localized(), () => ReturnToCamp(1));
        }

        public static void Destroy(int choice)
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.CorruptedGlade.DestroyStart".Localized());

            var amount = 0;

            switch (choice)
            {
                case 0:
                    OutputText("Scenes.CorruptedGlade.DestroyFireBreath".Localized());

                    amount += 1;

                    Player.ChangeFatigue(20);

                    break;

                case 1:
                    OutputText("Scenes.CorruptedGlade.DestroyFoxFire".Localized());

                    amount += 1;

                    Player.ChangeFatigue(20);

                    break;

                case 2:
                    OutputText("Scenes.CorruptedGlade.DestroyWhitefire".Localized());

                    amount += 1;

                    Player.ChangeFatigue(25);

                    break;

                case 3:
                    OutputText("Scenes.CorruptedGlade.DestroyAxe".Localized());

                    OutputText("Scenes.CorruptedGlade.DestroyAxeWood".Localized());

                    var wood = CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources);

                    CoC.Instance.Flags.Set(GameFlagIds.CampCabinWoodResources, Math.Min(999, wood + 10));

                    Player.ChangeFatigue(30 - Player.Strength / 10);

                    break;

                case 4:
                    OutputText("Scenes.CorruptedGlade.DestroyWeapon".Localized());

                    Player.ChangeFatigue(40 - Player.Strength / 5);

                    break;

                case 5:
                    if (Player.Strength < 30)
                    {
                        OutputText("Scenes.CorruptedGlade.DestroyHandsLow".Localized());

                        Player.ChangeFatigue(50);

                        ReturnToCamp(1);

                        return;
                    }

                    if (Player.Strength < 70)
                    {
                        OutputText("Scenes.CorruptedGlade.DestroyHandsMid".Localized());

                        Player.ChangeFatigue(40);
                    }
                    else
                    {
                        OutputText("Scenes.CorruptedGlade.DestroyHandsHigh".Localized());

                        Player.ChangeFatigue(30);
                    }

                    break;
            }

            var destroyed = CoC.Instance.Flags.Get(GameFlagIds.CorruptedGladesDestroyed);

            OutputText(destroyed == 0

                ? "Scenes.CorruptedGlade.DestroyFirst".Localized()

                : "Scenes.CorruptedGlade.DestroyAgain".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, -1));

            destroyed += amount + 1;

            CoC.Instance.Flags.Set(GameFlagIds.CorruptedGladesDestroyed, destroyed);

            if (destroyed == 25)
                OutputText("Scenes.CorruptedGlade.DestroyThird".Localized());

            if (destroyed == 50)
                OutputText("Scenes.CorruptedGlade.DestroyFourth".Localized());

            if (destroyed == 75)
                OutputText("Scenes.CorruptedGlade.DestroyFifth".Localized());

            if (destroyed >= 100)
                OutputText("Scenes.CorruptedGlade.DestroySixth".Localized());

            AddButton(0, "Common.Continue".Localized(), () => ReturnToCamp(1));
        }

        public static void FlowerFun()
        {
            ClearOutput();

            ClearButtons();

            if (Player.HasCock())
            {
                if (Player.Cocks.Count == 1)
                {
                    OutputText("Scenes.CorruptedGlade.FlowerSingle".Localized());

                    OutputText("Scenes.CorruptedGlade.FlowerSingleAgain".Localized(Player.CockDescript(0)));

                    OutputText((Player.Balls > 0

                        ? "Scenes.CorruptedGlade.FlowerSingleBalls"

                        : "Scenes.CorruptedGlade.FlowerSingleTaint").Localized());

                    OutputText("Scenes.CorruptedGlade.FlowerSingleThird".Localized());
                }
                else
                {
                    OutputText("Scenes.CorruptedGlade.FlowerMulti".Localized(Player.MultiCockDescriptLight()));

                    OutputText("Scenes.CorruptedGlade.FlowerMultiAgain".Localized(Player.MultiCockDescriptLight()));

                    OutputText("Scenes.CorruptedGlade.FlowerMultiThird".Localized());
                }

                OutputText("Scenes.CorruptedGlade.FlowerCum".Localized());

                OutputText((Player.Cocks.Count > 1

                    ? "Scenes.CorruptedGlade.FlowerTendrilsMulti"

                    : "Scenes.CorruptedGlade.FlowerTendrilsSingle").Localized());

                var balls = Player.Balls >= 2 ? "Scenes.Mimicscene.MimicTentacle2.AndYourWord".Localized() + Player.BallsDescript() : "and";

                OutputText((Player.Cocks.Count > 1

                    ? "Scenes.CorruptedGlade.FlowerAfterMulti"

                    : "Scenes.CorruptedGlade.FlowerAfterSingle").Localized(balls));

                var cum = Player.CumQ();

                if (cum < 25)
                    OutputText("Scenes.CorruptedGlade.FlowerCumLow".Localized());
                else if (cum < 250)
                    OutputText("Scenes.CorruptedGlade.FlowerCumMid".Localized());
                else if (cum < 500)
                    OutputText("Scenes.CorruptedGlade.FlowerCumHigh".Localized());
                else
                    OutputText("Scenes.CorruptedGlade.FlowerCumHuge".Localized());

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2));

                var booster = 1;

                if (Player.Balls == 0)
                    booster += 3;
                else if (Player.BallSize < 6)
                    booster += 2;

                if (Player.HasPerk(PerkIds.MessyOrgasms) && Player.CumMultiplier < 3)
                    booster += 1;

                Player.CumMultiplier += booster;

                if (Random.Shared.Next((int)(Player.CumQ() * 0.5f)) >= 100)
                {
                    OutputText("Scenes.CorruptedGlade.FlowerSeed".Localized());

                    Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.PumpkinSlice));
                }
            }
            else
            {
                OutputText("Scenes.CorruptedGlade.FlowerOral".Localized());

                OutputText("Scenes.CorruptedGlade.FlowerOralAgain".Localized());

                OutputText("Scenes.CorruptedGlade.FlowerOralThird".Localized());

                OutputText("Scenes.CorruptedGlade.FlowerOralFourth".Localized());

                Player.SlimeFeed();

                Player.Orgasm("Lips");

                Player.ApplyStats(
                    new DynStat(GameStats.Sensitivity, 4),

                    new DynStat(GameStats.Corruption, 1));
            }

            AddButton(0, "Common.Continue".Localized(), () => ReturnToCamp(1));
        }

        public static void TentacleFun()
        {
            ClearOutput();

            ClearButtons();

            if (Player.Vaginas.Count > 0 && Random.Shared.Next(2) == 0)
            {
                OutputText("Scenes.CorruptedGlade.TentacleVaginal".Localized());

                var tentacle = Random.Shared.Next(3);

                var looseness = Player.Vaginas[0].Looseness;

                var size = tentacle switch
                {
                    0 => looseness switch
                    {
                        0 => 0,

                        1 => 4,

                        2 => 16,

                        3 => 40,

                        4 => 65,

                        _ => 100
                    },

                    1 => looseness switch
                    {
                        0 => 3,

                        1 => 7,

                        2 => 26,

                        3 => 60,

                        4 => 115,

                        _ => 175
                    },

                    _ => looseness switch
                    {
                        0 => 6,

                        1 => 9,

                        2 => 34,

                        3 => 78,

                        4 => 135,

                        _ => 210
                    }
                };

                OutputText((tentacle switch
                {
                    0 => "Scenes.CorruptedGlade.TentacleSmall",

                    1 => "Scenes.CorruptedGlade.TentacleMedium",

                    _ => "Scenes.CorruptedGlade.TentacleLarge"
                }).Localized());

                OutputText("Scenes.CorruptedGlade.TentacleVaginalAgain".Localized(Player.VaginaDescript(0)));

                OutputText((tentacle switch
                {
                    0 => "Scenes.CorruptedGlade.TentacleVaginalSmall",

                    1 => "Scenes.CorruptedGlade.TentacleVaginalMedium",

                    _ => "Scenes.CorruptedGlade.TentacleVaginalLarge"
                }).Localized(Player.VaginaDescript(0)));

                if (Player.CuntChange(size, true))
                    OutputText("Common.Text.Pg".Localized());

                OutputText("Scenes.CorruptedGlade.TentacleClimax".Localized(Player.VaginaDescript(0)));

                OutputText("Scenes.CorruptedGlade.TentacleWake".Localized());

                Player.Orgasm("Vaginal");

                Player.ApplyStats(
                    new DynStat(GameStats.Sensitivity, 5),

                    new DynStat(GameStats.Corruption, 2));

                GreenHair();

                if (Random.Shared.Next(4) == 0 && Player.Body.Hips <= 10)
                {
                    OutputText("Scenes.CorruptedGlade.TentacleHips".Localized());

                    Player.Body.Hips += Random.Shared.Next(3) + 1;

                    Player.Fertility += 1;
                }

                Player.KnockUp(PregnancyIds.TentacleBeastSeed, PregnancyIds.TentacleBeastSeedIncubation);
            }
            else
            {
                OutputText("Scenes.CorruptedGlade.TentacleOral".Localized());

                OutputText("Scenes.CorruptedGlade.TentacleOralAgain".Localized());

                OutputText("Scenes.CorruptedGlade.TentacleOralThird".Localized());

                OutputText("Scenes.CorruptedGlade.TentacleOralFourth".Localized());

                OutputText("Scenes.CorruptedGlade.TentacleOralTingle".Localized());

                if (Player.Cocks.Count > 0)
                {
                    OutputText(Player.Cocks.Count == 1

                        ? Player.CockDescript(0)

                        : Player.MultiCockDescriptLight());

                    if (Player.Vaginas.Count > 0)
                        OutputText("Common.Text.And".Localized() + Player.VaginaDescript(0));
                }
                else if (Player.Vaginas.Count > 0)
                    OutputText(Player.VaginaDescript(0));
                else
                    OutputText("Common.Text.Nipples".Localized());

                OutputText("Scenes.CorruptedGlade.TentacleOralTingleEnd".Localized());

                Player.ApplyStats(
                    new DynStat(GameStats.Lust, 25 + Player.Libido / 10),

                    new DynStat(GameStats.Corruption, 2));

                GreenHair();

                if (Random.Shared.Next(4) == 0 && Player.Body.Butt <= 10)
                {
                    OutputText("Scenes.CorruptedGlade.TentacleButt".Localized());

                    Player.Body.Butt += Random.Shared.Next(3) + 1;
                }

                if (Random.Shared.Next(3) == 0 &&
                    Player.Cocks.Count > 0 &&

                    Player.Body.Get<HairPart>().Color == "green" &&

                    Player.CountCocksOfType("Tentacle") < Player.CockTotal())

                    if (Player.Cocks.Count == 1)
                    {
                        OutputText("Scenes.CorruptedGlade.TentacleCockSingle".Localized());

                        Player.ChangeCockType(0, "Tentacle");
                    }
                    else
                    {
                        OutputText("Scenes.CorruptedGlade.TentacleCockMulti".Localized());

                        Player.ChangeAllCockTypes("Tentacle");
                    }
            }

            Player.SlimeFeed();

            AddButton(0, "Common.Continue".Localized(), () => ReturnToCamp(1));
        }

        public static void GreenHair()
        {
            if (Random.Shared.Next(3) != 0 || Player.Body.Get<HairPart>().Color == "green")
                return;

            OutputText("Scenes.CorruptedGlade.TentacleHair".Localized());

            Player.Body.Get<HairPart>().Color = "green";
        }

        public static void TreeBoobFun()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.CorruptedGlade.Tree".Localized());

            OutputText("Scenes.CorruptedGlade.TreeAgain".Localized());

            OutputText("Scenes.CorruptedGlade.TreeThird".Localized());

            Player.ApplyStats(
                new DynStat(GameStats.Sensitivity, 1),

                new DynStat(GameStats.Lust, 15),

                new DynStat(GameStats.Corruption, 0.5f));

            Player.SlimeFeed();

            if (!Player.HasStatusEffect(StatusEffectIds.LustyTongue))
                if (Random.Shared.Next(4) == 0)
                {
                    OutputText("Scenes.CorruptedGlade.TreeTongue".Localized());

                    Player.CreateStatusEffect(StatusEffectIds.LustyTongue, 24);
                }
                else
                    OutputText("Scenes.CorruptedGlade.TreeNoTongue".Localized());

            if (Random.Shared.Next(2) == 0)
                if (Player.BiggestBreastSize() > 5)
                {
                    OutputText("Scenes.CorruptedGlade.TreeGrowLarge".Localized());

                    Player.GrowTits(1, 1 + Random.Shared.Next(3), false, 1);
                }
                else if (Player.BiggestBreastSize() == 0)
                {
                    OutputText("Scenes.CorruptedGlade.TreeGrowNew".Localized());

                    Player.GrowTits(1, 2 + Random.Shared.Next(3), false, 1);
                }
                else
                {
                    OutputText("Scenes.CorruptedGlade.TreeGrowSmall".Localized());

                    Player.GrowTits(1, 1 + Random.Shared.Next(3), false, 1);
                }

            AddButton(0, "Common.Continue".Localized(), () => ReturnToCamp(1));
        }

        public static void TrappedSatyr()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.CorruptedGlade.SatyrIntro".Localized());

            OutputText("Scenes.CorruptedGlade.SatyrIntroAgain".Localized());

            Player.UnlockCodexEntry(GameFlagIds.CodexEntrySatyrs, false, true);

            if (!Player.HasCock())
            {
                OutputText("Scenes.CorruptedGlade.SatyrNoCock".Localized());

                AddButton(0, "Common.Continue".Localized(), () => ReturnToCamp(1));

                return;
            }

            OutputText("Scenes.CorruptedGlade.SatyrPrompt".Localized());

            AddButton(0, "Scenes.CorruptedGlade.Ravage".Localized(), RapeSatyr);

            AddButton(14, "Scenes.CorruptedGlade.Leave".Localized(), IgnoreSatyr);
        }

        public static void IgnoreSatyr()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.CorruptedGlade.SatyrIgnore".Localized());

            OutputText((Player.Corruption < 50

                ? "Scenes.CorruptedGlade.SatyrIgnoreLow"

                : "Scenes.CorruptedGlade.SatyrIgnoreHigh").Localized());

            OutputText("Scenes.CorruptedGlade.SatyrIgnoreEnd".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 5 + Player.Libido / 20));

            AddButton(0, "Common.Continue".Localized(), () => ReturnToCamp(1));
        }

        public static void RapeSatyr()
        {
            ClearOutput();

            ClearButtons();

            var index = Player.BiggestCockIndex();

            OutputText((Player.Corruption < 33

                ? "Scenes.CorruptedGlade.SatyrRapeLow"

                : Player.Corruption < 66

                    ? "Scenes.CorruptedGlade.SatyrRapeMid"

                    : "Scenes.CorruptedGlade.SatyrRapeHigh").Localized());

            OutputText("Scenes.CorruptedGlade.SatyrStrip".Localized());

            OutputText((Player.HasTailInsteadOfLegs()

                ? "Scenes.CorruptedGlade.SatyrSlither"

                : "Scenes.CorruptedGlade.SatyrSneak").Localized());

            OutputText("Scenes.CorruptedGlade.SatyrStroke".Localized(Player.CockDescript(index), Player.CockHead(index)));

            OutputText("Scenes.CorruptedGlade.SatyrYelp".Localized());

            OutputText("Scenes.CorruptedGlade.SatyrSlap".Localized(Appearance.CockNoun(Player, index)));

            OutputText("Scenes.CorruptedGlade.SatyrHorns".Localized());

            OutputText("Scenes.CorruptedGlade.SatyrPound".Localized());

            OutputText((Player.CockArea(index) >= 100

                ? "Scenes.CorruptedGlade.SatyrStretchBig"

                : "Scenes.CorruptedGlade.SatyrStretchFit").Localized(Player.CockDescript(index)));

            OutputText("Scenes.CorruptedGlade.SatyrGroin".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.CorruptedGlade.SatyrBalls".Localized());

            OutputText("Scenes.CorruptedGlade.SatyrCumShot".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.CorruptedGlade.SatyrCumMulti".Localized());

            OutputText("Scenes.CorruptedGlade.SatyrCumEnd".Localized());

            OutputText("Scenes.CorruptedGlade.SatyrSuckle".Localized());

            var cum = Player.CumQ();

            if (cum < 250)
                OutputText("Scenes.CorruptedGlade.SatyrCumLow".Localized());
            else if (cum < 1000)
                OutputText("Scenes.CorruptedGlade.SatyrCumMid".Localized());
            else
                OutputText("Scenes.CorruptedGlade.SatyrCumHigh".Localized());

            OutputText("Scenes.CorruptedGlade.SatyrAbsorbed".Localized());

            if (cum >= 1000)
                OutputText("Scenes.CorruptedGlade.SatyrAbsorbedWide".Localized());

            OutputText("Scenes.CorruptedGlade.SatyrEndPunct".Localized());

            OutputText("Scenes.CorruptedGlade.SatyrWatch".Localized());

            OutputText("Scenes.CorruptedGlade.SatyrAsk".Localized(Player.CockDescript(index)));

            Player.Orgasm("Dick");

            AddButton(0, "Scenes.CorruptedGlade.Again".Localized(), SecondSatyrFuck);

            AddButton(1, "Scenes.CorruptedGlade.Leave".Localized(), DontRepeatFuckSatyr);
        }

        public static void DontRepeatFuckSatyr()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.CorruptedGlade.SatyrLeave".Localized());

            AddButton(0, "Common.Continue".Localized(), () => ReturnToCamp(1));
        }

        public static void SecondSatyrFuck()
        {
            ClearOutput();

            ClearButtons();

            var index = Player.CockThatFits(0f);

            if (index < 0)
                index = Player.SmallestCockIndex();

            OutputText("Scenes.CorruptedGlade.SatyrAgain".Localized(Player.CockDescript(index)));

            OutputText("Scenes.CorruptedGlade.SatyrAgainAgain".Localized());

            OutputText("Scenes.CorruptedGlade.SatyrAgainThird".Localized());

            OutputText("Scenes.CorruptedGlade.SatyrAgainFourth".Localized());

            OutputText("Scenes.CorruptedGlade.SatyrAgainFifth".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(
                new DynStat(GameStats.Libido, 1),

                new DynStat(GameStats.Sensitivity, -5));

            AddButton(0, "Common.Continue".Localized(), () => ReturnToCamp(1));
        }

    }
}