//  Tools/PortKit/emit.mjs  plan.json 
//  Haxe TODO 

using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
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
    [Register("coc:mimic_scene")]
    public class MimicScene : Scene
    {
        // 原版多处读 player.breastRows[0].nippleCocks（MimicScene.hx:206/296/339/351/357/364/368/372/542/581/602/627/634/657）。
        // C# 的 BreastRowPart 没有 nippleCocks 字段（数据缺失：原版 classes/bodyParts/BreastRow.hx），
        // 该状态在本移植版里不可达，因此这些分支统一按 false 处理，不伪造一个可被设置的假字段。
        //  APPEARANCE_ROCK
        private const int APPEARANCE_ROCK = 0;
        //  APPEARANCE_BOOB
        private const int APPEARANCE_BOOB = 1;
        //  APPEARANCE_DICK
        private const int APPEARANCE_DICK = 2;
        //  APPEARANCE_CHEST
        private const int APPEARANCE_CHEST = 3;

        // 原版 public var mimicAppearance:Int = 0（MimicScene.hx:9）：由遭遇入口随机决定，
        // 再作为构造参数传给 new Mimic(mimicAppearance)（MimicScene.hx:179/186）。
        public int MimicAppearance;

        private int mimicAppearance
        {
            get => MimicAppearance;
            set => MimicAppearance = value;
        }




        private static bool noFur => !Player.HasFur();

        private static bool silly => OptionStore.Instance.Get("silly", 0) > 0;

        private static bool metric => OptionStore.Instance.Get("metric", 0) > 0;

        private static bool realistic => OptionStore.Instance.Get("realistic", 0) > 0;

        private static bool survival => OptionStore.Instance.Get("survival", 0) > 0;

        private static bool hasKnot => Player.Cocks.Count > 0 && Player.Cocks.Any(c => c.KnotMultiplier > 1);

        private static void ReturnToCamp(int hours) => Go(SceneRegistry.CreateOrDefault("coc:camp"));

        public override void Entry()
        {
            // 原版没有 Entry 概念，场景由 mimicTentacleStart() 进入（MimicScene.hx:35）。
            MimicTentacleStart();
        }

        // New

        // Haxe IsSexy()   outputText 0 
        public bool IsSexy()
        {

            var __sw0_0 = mimicAppearance;
            if (__sw0_0 == APPEARANCE_ROCK)
            {
                return false;
            }
            if (__sw0_0 == APPEARANCE_BOOB)
            {
                return true;
            }
            if (__sw0_0 == APPEARANCE_DICK)
            {
                return true;
            }
            if (__sw0_0 == APPEARANCE_CHEST)
            {
                return false;
            }
            if (true /* default */)
            {
                return false;
            }
        }

        // Haxe MimicTentacleStart()   outputText 20 
        public void MimicTentacleStart()
        {
            ClearOutput();

            var choices = new[] { APPEARANCE_ROCK, APPEARANCE_BOOB, APPEARANCE_DICK, APPEARANCE_CHEST, APPEARANCE_CHEST, APPEARANCE_CHEST };
            mimicAppearance = choices[Random.Shared.Next(choices.Length)];
            if (Player.IsInDesert)
            {
                OutputText("Scenes.Mimicscene.MimicTentacleStart.WindHowls".Localized());
            }
            else
            {
                if (Player.IsInMountains)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacleStart.ThunderRumbles".Localized());
                }
                else
                {
                    OutputText("Scenes.Mimicscene.MimicTentacleStart.PeacefulNature".Localized());
                }
            }
            OutputText("Scenes.Mimicscene.MimicTentacleStart.ExploresDeeper".Localized());
            if (mimicAppearance == APPEARANCE_ROCK)
            {
                OutputText("Scenes.Mimicscene.MimicTentacleStart.HugeBlockStone".Localized());
            }
            else
            {
                if (mimicAppearance == APPEARANCE_BOOB)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacleStart.CantBeDescribed".Localized());
                }
                else
                {
                    if (mimicAppearance == APPEARANCE_DICK)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacleStart.EnormousFlaccid".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacleStart.LargeWoodenBox".Localized());
                    }
                }
            }
            OutputText("Scenes.Mimicscene.MimicTentacleStart.ShadyAlcove".Localized());
            if (Random.Shared.Next((int)(18)) + (Player.Intelligence / 2) < 15)
            {
                OutputText("Scenes.Mimicscene.MimicTentacleStart.EntrancedSight".Localized());
                if (mimicAppearance == APPEARANCE_ROCK)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacleStart.MagicRockWord".Localized());
                }
                else
                {
                    if (mimicAppearance == APPEARANCE_BOOB)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacleStart.GiantBoobWord".Localized());
                    }
                    else
                    {
                        if (mimicAppearance == APPEARANCE_DICK)
                        {
                            OutputText("Scenes.Mimicscene.MimicTentacleStart.HugePenisWord".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Mimicscene.MimicTentacleStart.TreasureChestWord".Localized());
                        }
                    }
                }
                OutputText("Scenes.Mimicscene.MimicTentacleStart.IgnoringRemnants".Localized());
                MimicTentacle1();
            }
            else
            {
                if (mimicAppearance == APPEARANCE_ROCK)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacleStart.WhileMagicRock".Localized());
                }
                else
                {
                    if (mimicAppearance == APPEARANCE_BOOB)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacleStart.WhileGiantBoob".Localized());
                    }
                    else
                    {
                        if (mimicAppearance == APPEARANCE_DICK)
                        {
                            OutputText("Scenes.Mimicscene.MimicTentacleStart.WhileHugePenis".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Mimicscene.MimicTentacleStart.WhileTreasureChest".Localized());
                        }
                    }
                }
                OutputText("Scenes.Mimicscene.MimicTentacleStart.WarrantsInvestigation".Localized());
                AddButton(0, "Yes", MimicTentacle1);
                AddButton(1, "No", () => ReturnToCamp(1));
            }
        }

        // Haxe MimicTentacle1()   outputText 29 
        public void MimicTentacle1()
        {
            ClearOutput();

            OutputText("Scenes.Mimicscene.MimicTentacle1.ApproachesFewFeet".Localized());
            if (mimicAppearance == APPEARANCE_ROCK)
            {
                OutputText("Scenes.Mimicscene.MimicTentacle1.StoneWord".Localized());
            }
            else
            {
                if (mimicAppearance == APPEARANCE_BOOB)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle1.HugeTitWord".Localized());
                }
                else
                {
                    if (mimicAppearance == APPEARANCE_DICK)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle1.GiantDickWord".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle1.ChestWord".Localized());
                    }
                }
            }
            if (Random.Shared.Next((int)(100)) + (float)(int)Math.Floor(Player.Intelligence / 1.5f) >= 80)
            {
                if (Random.Shared.Next((int)(Player.Intelligence)) + (float)(int)Math.Floor(Player.Intelligence / 3) >= 50 + Player.NewGamePlusMod() * 10)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle1.BeginsToReach".Localized());
                    var itemRoll = Random.Shared.Next((int)(4));
                    if (itemRoll == 0)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle1.SilverVial".Localized());
                    }
                    if (itemRoll == 1)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle1.SmallSack".Localized());
                    }
                    if (itemRoll == 2)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle1.GlassBottle".Localized());
                    }
                    if (itemRoll == 3)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle1.ClayJar".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle1.PileOfRubbish".Localized());
                    if (itemRoll == 0)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle1.VialWord".Localized());
                    }
                    if (itemRoll == 1)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle1.SackWord".Localized());
                    }
                    if (itemRoll == 2)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle1.BottleWord".Localized());
                    }
                    if (itemRoll == 3)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle1.JarWord".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle1.LeavesMonster".Localized());
                    if (itemRoll == 0)
                    {
                        Player.Inventory.Add(ItemRegistry.Create(VanillaConsumables.PurityPhilter), 1);
ReturnToCamp(1);
                    }
                    if (itemRoll == 1)
                    {
                        FindSomeGems();
                    }
                    if (itemRoll == 2)
                    {
                        Player.Inventory.Add(ItemRegistry.Create(VanillaConsumables.NumbingOil), 1);
ReturnToCamp(1);
                    }
                    if (itemRoll == 3)
                    {
                        Player.Inventory.Add(ItemRegistry.Create(VanillaConsumables.HUMMUS_), 1);
                        ReturnToCamp(1);
                    }
                    return ;
                }
                else
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle1.ReachesTowards".Localized());
                    ClearButtons();
                    AddButton(0, "Fight", Fight);
                    SetExitButton("Back");
                    return ;
                }
            }
            else
            {
                OutputText("Scenes.Mimicscene.MimicTentacle1.ReachOutToTouch".Localized());
                if (mimicAppearance == APPEARANCE_ROCK)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle1.StoneSurface".Localized());
                }
                else
                {
                    if (mimicAppearance == APPEARANCE_BOOB)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle1.BoobSkin".Localized());
                    }
                    else
                    {
                        if (mimicAppearance == APPEARANCE_DICK)
                        {
                            OutputText("Scenes.Mimicscene.MimicTentacle1.CockSkin".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Mimicscene.MimicTentacle1.BoxSurface".Localized());
                        }
                    }
                }
                OutputText("Scenes.Mimicscene.MimicTentacle1.StruggleToPull".Localized());
                OutputText("Scenes.Mimicscene.MimicTentacle1.ParagraphBreak".Localized());
                if (mimicAppearance == APPEARANCE_ROCK)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle1.EnormousMaw".Localized());
                }
                else
                {
                    if (mimicAppearance == APPEARANCE_BOOB)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle1.NippleOpens".Localized());
                    }
                    else
                    {
                        if (mimicAppearance == APPEARANCE_DICK)
                        {
                            OutputText("Scenes.Mimicscene.MimicTentacle1.CockRearsUp".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Mimicscene.MimicTentacle1.LidOpens".Localized());
                        }
                    }
                }
                OutputText("Scenes.Mimicscene.MimicTentacle1.BreaksFree".Localized());
                StartMimicCombat(false);
                return ;
            }
        }

        // Haxe Fight()   outputText 0 
        public void Fight()
        {
            StartMimicCombat(true);
        }

        // 原版两处 startCombat(new Mimic(mimicAppearance)) / startCombatImmediate(...)（MimicScene.hx:179/186）：
        // 外观由场景的 mimicAppearance 传入，敌人侧用 MimicEnemy.Create(definition, appearance) 构造。
        // fight() 里的 `monster.distance = Distant`（MimicScene.hx:185）在开战前没有 monster 可写，
        // 这里等价地设置到新建的敌人身上。
        private void StartMimicCombat(bool distant)
        {
            var definition = EnemyRegistry.Definitions.Get(new Identifier("coc", "mimic"));
            var monster = MimicEnemy.Create(definition, MimicAppearance);

            if (distant)
                monster.Distance = CombatDistance.Distant;

            CombatScene.Start(monster, () => Go<Camp>(), canFlee: !distant);
        }

        // Haxe MimicTentacle2()   outputText 185 
        public void MimicTentacle2()
        {
            ClearOutput();

            var tempSize = float.NaN;
            if (Player.HasCock())
            {
                tempSize = (float)((Player.NippleLength + Player.Cocks[0].Length / 2) * 100) / 100;
            }
            var nippleCockDescript = Player.NippleDescript(0);
            if (!IsSexy())
            {
                OutputText("Scenes.Mimicscene.MimicTentacle2.CollapsingUnable".Localized());
            }
            OutputText("Scenes.Mimicscene.MimicTentacle2.HundredsTendrils".Localized());
            if (Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Mimicscene.MimicTentacle2.CrotchSmooth".Localized());
                if (false)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.BeforeSomethingRash".Localized(
                        nippleCockDescript,
                        tempSize
                    ));
                    if (IsSexy())
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.MovesTorso".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.TentacleCollect".Localized());
                        if (Player.BreastRows.Count() * Player.BreastRows[0].NipplesPerBreast == 2)
                        {
                            OutputText("Scenes.Mimicscene.MimicTentacle2.BothDicknipplesEngulf".Localized()
                                nippleCockDescript
                            );
                        }
                        else
                        {
                            OutputText("Scenes.Mimicscene.MimicTentacle2.EveryDicknipple".Localized(
                                (int)(Player.BreastRows.Count() * Player.BreastRows[0].NipplesPerBreast),
                                nippleCockDescript
                            ));
                        }
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.TheWord".Localized());
                    if (IsSexy())
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.MouthBegins".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.MouthTentacleBeginsThird".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.SuckCockteats".Localized());
                    if (Player.BreastRows[0].Lactation > 0)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.MilkyWord".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.CumDeepHungry".Localized());
                    if (IsSexy())
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.BeforeBlackOut".Localized());
                    }
                }
                if (Player.HasFuckableNipples())
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.BeforeSomethingRashAgain".Localized());
                    if (Player.Vaginas[0].Looseness < 2)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.SmallOpeningsWord".Localized());
                    }
                    if (Player.Vaginas[0].Looseness >= 2 && Player.Vaginas[0].Looseness < 4)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.PuckeredOpeningsWord".Localized());
                    }
                    if (Player.Vaginas[0].Looseness >= 4)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.EngorgedLipsWord".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.NipplesEaseOpen".Localized());
                    if (Player.Vaginas[0].Wetness < 2)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.DrippingWord".Localized());
                    }
                    if (Player.Vaginas[0].Wetness >= 2 && Player.Vaginas[0].Wetness < 4)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.DroolingWord".Localized());
                    }
                    if (Player.Vaginas[0].Wetness >= 4)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.SpurtingWord".Localized());
                    }
                    if (Player.BreastRows[0].Lactation > 0)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.MilkyWordAgain".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.FluidsSurprised".Localized());
                    if (IsSexy())
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.LongWhippyTongues".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.GiganticTongue".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.SlurpNewBounty".Localized());
                    OutputText("Scenes.Mimicscene.MimicTentacle2.MindReels".Localized());
                    if (IsSexy())
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.UncountableTongues".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.HugeTongueCovers".Localized());
                        OutputText("Scenes.Mimicscene.MimicTentacle2.BreastsLongStrokes".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.OrgasmRepeatedly".Localized());
                    if (IsSexy())
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.TwoDozenTongues".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.TongueIntoEach".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.WeakMoan".Localized());
                    OutputText("Scenes.Mimicscene.MimicTentacle2.ContinuesToAlternate".Localized());
                    if (IsSexy())
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.BeforeBlackOutAgain".Localized());
                    }
                }
                if (!false && !Player.HasFuckableNipples())
                {
                    if (IsSexy())
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.EyesNarrowFurious".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.TentaclesQuiver".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.HundredsWhipping".Localized());
                    if (IsSexy())
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.BeforeBlackOutThird".Localized());
                    }
                }
            }
            if (Player.Gender == Gender.Male)
            {
                OutputText("Scenes.Mimicscene.MimicTentacle2.AndYourWord".Localized());
                if (Player.CockTotal() == 1)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.CockIsWord".Localized(
                        Player.CockDescript()
                    ));
                }
                else
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.CocksAreWord".Localized(
                        Player.MultiCockDescript()
                    ));
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.SoonRockHard".Localized());
                if (IsSexy())
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.MovesCrotch".Localized());
                    if (Player.CockTotal() > 1)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.PluralS".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.NightmareHole".Localized());
                    if (Player.CockTotal() == 1)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.AttemptToPull".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.PluralSAttemptPull".Localized());
                    }
                }
                else
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.OrificeEnveloping".Localized());
                    if (Player.CockTotal() == 1)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.YourCockWord".Localized(
                            Player.CockDescript()
                        ));
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.AllCocksAtOnce".Localized());
                    }
                }
                if (false)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.MeanwhileTendrils".Localized(
                        nippleCockDescript,
                        tempSize
                    ));
                    if (IsSexy())
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.EyesNarrowAnger".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.SeemsAnnoyed".Localized());
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.TheWordAgain".Localized());
                if (IsSexy())
                {
                    if (false)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.MouthsBegin".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.MouthBeginsAgain".Localized());
                    }
                }
                else
                {
                    if (false)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.MouthTentaclesBeginAgain".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.MouthTentacleBeginsAgain".Localized());
                    }
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.SuckAndMassageCock".Localized());
                if (Player.CockTotal() > 1 || false)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.PluralSAgain".Localized());
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.SpinningMotion".Localized());
                if (false)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.PluralSThird".Localized());
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.BeastNotSatisfied".Localized());
                if (Player.CockTotal() > 1 || false)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.PluralSFourth".Localized());
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.SearchMoreCum".Localized());
                if (IsSexy())
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.BeforeBlackOutFourth".Localized());
                }
            }
            if (Player.Gender == Gender.Female)
            {
                OutputText("Scenes.Mimicscene.MimicTentacle2.AndYourVagina".Localized());
                if (Player.Vaginas.Count > 1)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.PluralSAre".Localized());
                }
                else
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.IsWord".Localized());
                }
                if (Player.Vaginas[0].Wetness < 2)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.DrippingWordAgain".Localized());
                }
                if (Player.Vaginas[0].Wetness >= 2 && Player.Vaginas[0].Wetness < 4)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.DroolingWordAgain".Localized());
                }
                if (Player.Vaginas[0].Wetness >= 4)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.SpurtingWordAgain".Localized());
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.JuicesEverywhere".Localized());
                if (IsSexy())
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.ThinWhippyTongues".Localized());
                }
                else
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.HugeTongueRunsAgain".Localized());
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.AcrossYourVagina".Localized());
                if (Player.Vaginas.Count > 1)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.PluralSFifth".Localized());
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.CreatureBurble".Localized());
                if (Player.HasFuckableNipples())
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.MeanwhileTendrilsAgain".Localized());
                    if (Player.Vaginas[0].Looseness < 2)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.SmallOpeningsWordAgain".Localized());
                    }
                    if (Player.Vaginas[0].Looseness >= 2 && Player.Vaginas[0].Looseness < 4)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.PuckeredOpeningsWordAgain".Localized());
                    }
                    if (Player.Vaginas[0].Looseness >= 4)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.EngorgedLipsWordAgain".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.NipplesEaseOpenAgain".Localized());
                    if (Player.Vaginas[0].Wetness < 2)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.DrippingWordThird".Localized());
                    }
                    if (Player.Vaginas[0].Wetness >= 2 && Player.Vaginas[0].Wetness < 4)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.DroolingWordThird".Localized());
                    }
                    if (Player.Vaginas[0].Wetness >= 4)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.SpurtingWordThird".Localized());
                    }
                    if (Player.AverageLactation() > 0)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.MilkyWordThird".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.FluidsSurprisedAgain".Localized());
                    if (IsSexy())
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.SproutedTongues".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.EvenLargerTongue".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.SlurpNewBountyAgain".Localized());
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.MindReelsAgain".Localized());
                if (IsSexy())
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.UncountableTonguesAgain".Localized());
                    if (Player.Vaginas.Count > 1)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.PluralSSixth".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.AndWord".Localized());
                    OutputText("Scenes.Mimicscene.MimicTentacle2.BreastsWord".Localized());
                }
                else
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.HugeTongueCoversAgain".Localized());
                    if (Player.Vaginas.Count == 1)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.YourVaginaWord".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.AllYourVaginas".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.AndWordAgain".Localized());
                    OutputText("Scenes.Mimicscene.MimicTentacle2.BreastsLongStrokesAgain".Localized());
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.OrgasmRepeatedlyAgain".Localized());
                if (IsSexy())
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.TwoDozenTonguesAgain".Localized());
                    if (Player.Vaginas.Count == 1)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.YourVaginaWordAgain".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.EachYourVaginas".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.ExploringNook".Localized());
                }
                else
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.TongueDeepInto".Localized());
                    if (Player.Vaginas.Count == 1)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.YourVaginaWordThird".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.VaginaOneByOne".Localized());
                    }
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.WeakMoanAgain".Localized());
                if (Player.HasFuckableNipples())
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.MoanDragsOut".Localized());
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.ContinuesToAlternateAgain".Localized());
                if (IsSexy())
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.BeforeBlackOutFifth".Localized());
                }
            }
            if (Player.Gender == Gender.Hermaphrodite)
            {
                OutputText("Scenes.Mimicscene.MimicTentacle2.YourVaginaSpace".Localized());
                if (Player.Vaginas.Count > 1)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.PluralSSeventh".Localized());
                }
                if (Player.Vaginas[0].Wetness < 2)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.DrippingSpace".Localized());
                }
                if (Player.Vaginas[0].Wetness >= 2 && Player.Vaginas[0].Wetness < 4)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.DroolingSpace".Localized());
                }
                if (Player.Vaginas[0].Wetness >= 4)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.SpurtingSpace".Localized());
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.JuicesWhileYour".Localized());
                if (Player.CockTotal() == 1)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.RapidlyBecomes".Localized(
                        Player.CockDescript()
                    ));
                }
                else
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.RapidlyBecome".Localized(
                        Player.MultiCockDescript()
                    ));
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.RockHardEyesRoll".Localized());
                if (IsSexy())
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.ThinWhippyTonguesAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.HugeTongueRunsAgain".Localized());
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.AcrossYourVaginaAgain".Localized());
                if (Player.Vaginas.Count > 1)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.PluralSEighth".Localized());
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.SentencePeriod".Localized());
                OutputText("Scenes.Mimicscene.MimicTentacle2.OrificeEnvelopingAgain".Localized());
                if (Player.CockTotal() == 1)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.YourCockWordAgain".Localized(
                        Player.CockDescript()
                    ));
                }
                else
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.EveryCockAtOnce".Localized());
                }
                if (false || Player.HasFuckableNipples())
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.MeanwhileTendrilsThird".Localized());
                }
                if (Player.HasFuckableNipples())
                {
                    if (Player.Vaginas[0].Looseness < 2)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.TheSmallOpenings".Localized());
                    }
                    if (Player.Vaginas[0].Looseness >= 2 && Player.Vaginas[0].Looseness < 4)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.ThePuckeredOpenings".Localized());
                    }
                    if (Player.Vaginas[0].Looseness >= 4)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.TheEngorgedLips".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.NipplesEaseOpenThird".Localized());
                    if (Player.Vaginas[0].Wetness < 2)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.DrippingWordFifth".Localized());
                    }
                    if (Player.Vaginas[0].Wetness >= 2 && Player.Vaginas[0].Wetness < 4)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.DroolingWordFifth".Localized());
                    }
                    if (Player.Vaginas[0].Wetness >= 4)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.SpurtingWordFifth".Localized());
                    }
                    if (Player.BreastRows[0].Lactation > 0)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.MilkyWordFourth".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.FluidsSurprisedThird".Localized());
                    if (IsSexy())
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.SproutedTonguesAgain".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.EvenLargerTongueAgain".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.SlurpNewBountyThird".Localized());
                }
                if (false)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.NipplecocksGrow".Localized(
                        nippleCockDescript,
                        tempSize
                    ));
                    if (IsSexy())
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.EyesNarrowAngerAgain".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.SeemsAnnoyedAgain".Localized());
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.MindReelsThird".Localized());
                if (IsSexy())
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.UncountableTonguesThird".Localized());
                    if (Player.Vaginas.Count > 1)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.PluralSNinth".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.AndWordThird".Localized());
                    OutputText("Scenes.Mimicscene.MimicTentacle2.BreastsWhileThe".Localized());
                    if (false)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.MouthsBeginAgain".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.MouthBeginsThird".Localized());
                    }
                }
                else
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.HugeTongueSlithers".Localized());
                    if (Player.CockTotal() == 1 && !Player.Cocks.Exists(c => c.SheathOn))
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.CockWord".Localized());
                    }
                    else
                    {
                        if (!Player.Cocks.Exists(c => c.SheathOn))
                        {
                            OutputText("Scenes.Mimicscene.MimicTentacle2.BundleOfCocks".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Mimicscene.MimicTentacle2.SheathWord".Localized());
                        }
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.UpEntireFront".Localized());
                    if (Player.Vaginas.Count == 1)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.YourVaginaWordFourth".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.AllYourVaginasAgain".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.AndWordFourth".Localized());
                    OutputText("Scenes.Mimicscene.MimicTentacle2.BreastsLongStrokesWhile".Localized());
                    if (false)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.MouthTentaclesBeginAgain".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.MouthTentacleBeginsThird".Localized());
                    }
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.SuckMassageCock".Localized());
                if (Player.CockTotal() > 1 || false)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.PluralSTenth".Localized());
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.SpinningMotionAgain".Localized());
                OutputText("Scenes.Mimicscene.MimicTentacle2.OrgasmRepeatedlyThird".Localized());
                if (IsSexy())
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.TwoDozenTonguesThird".Localized());
                    if (Player.Vaginas.Count == 1)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.YourVaginaWordFifth".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.EachYourVaginasAgain".Localized());
                    }
                    OutputText("Scenes.Mimicscene.MimicTentacle2.ExploringNookAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.TongueDeepIntoAgain".Localized());
                    if (Player.Vaginas.Count == 1)
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.YourVaginaWordSixth".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Mimicscene.MimicTentacle2.EachVaginaAgain".Localized());
                    }
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.DozensDownUrethra".Localized());
                if (Player.CockTotal() > 1 || false)
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.PluralSEleventh".Localized());
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.SearchMoreCumAgain".Localized());
                if (Player.HasVagina())
                {
                    Player.CuntChange(60, true);
                }
                if (Player.HasFuckableNipples())
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.MoanDragsOutAgain".Localized());
                }
                OutputText("Scenes.Mimicscene.MimicTentacle2.ContinuesToAlternateThird".Localized());
                if (IsSexy())
                {
                    OutputText("Scenes.Mimicscene.MimicTentacle2.BeforeBlackOutSixth".Localized());
                }
            }
            Player.Orgasm("Generic");
            MimicTentacleEnd();
        }

        // Haxe MimicTentacleEnd()   outputText 3 
        public void MimicTentacleEnd()
        {
            ClearOutput();

            OutputText("Scenes.Mimicscene.MimicTentacleEnd.WakesSurprised".Localized());
            if (Player.HasVagina())
            {
                if (IsSexy())
                {
                    if (Random.Shared.Next((int)(5)) > Player.Vaginas[0].Wetness)
                    {
                        Player.Vaginas[0].Wetness += 1;
                        OutputText("Scenes.Mimicscene.MimicTentacleEnd.StrangelyAfterOrdeal".Localized());
                    }
                }
                else
                {
                    if (Random.Shared.Next((int)(5)) > Player.Vaginas[0].Looseness)
                    {
                        Player.Vaginas[0].Looseness += 1;
                        OutputText("Scenes.Mimicscene.MimicTentacleEnd.CuntStretched".Localized());
                    }
                }
            }
            Player.ApplyStats(new DynStat(VanillaStats.Intelligence, -2f), new DynStat(VanillaStats.Sensitivity, 5f), new DynStat(VanillaStats.Corruption, 2f));
            CombatScene.CleanupAfterCombat();
        }

        // Haxe KillTheMimic()   outputText 1 
        public void KillTheMimic()
        {
            ClearOutput();

            OutputText("Scenes.Mimicscene.KillTheMimic.MonsterCollapses".Localized());
            CombatScene.CleanupAfterCombat();
        }

        // Haxe FindSomeGems()   outputText 1 
        public void FindSomeGems()
        {

            var quantity = (int)((Random.Shared.Next((int)(30)) + 10 + Player.Level) * (1 + (Player.Perkv1(VanillaPerks.AscensionFortune) * 0.1f)));
            Player.Gems += quantity;
            OutputText("Scenes.Mimicscene.FindSomeGems.OpeningSmallBag".Localized()
                quantity
            );
            ReturnToCamp(1);
        }

    }
}
