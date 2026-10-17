//  Tools/PortKit/emit.mjs  plan.json 
//  Haxe TODO 

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
    [Register("coc:goblin_assassin_scene")]
    public class GoblinAssassinScene : Scene
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
            GoblinAssassinEncounter();
        }

        // New

        // Haxe GoblinAssassinEncounter()   outputText 3 
        public void GoblinAssassinEncounter()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(VanillaFlags.TimesEncounteredGoblinAssassin) == 0)
            {
                OutputText("Scenes.Goblinassassinscene.GoblinAssassinEncounter.NeedleWhizzesThroughAir".Localized());
                OutputText("Scenes.Goblinassassinscene.GoblinAssassinEncounter.ReadyYourWeapon".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinassassinscene.GoblinAssassinEncounter.FamiliarSightCatches".Localized());
            }
            CoC.Instance.Flags.Set(VanillaFlags.TimesEncounteredGoblinAssassin, CoC.Instance.Flags.Get(VanillaFlags.TimesEncounteredGoblinAssassin) + 1);
            Player.UnlockCodexEntry(VanillaFlags.CodexEntryGoblins);
            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "goblin_assassin")), () => Go(SceneRegistry.CreateOrDefault("coc:forest")));
        }

        // Haxe GobboAssassinBeatYaUp()   outputText 85 
        public void GobboAssassinBeatYaUp()
        {
            ClearOutput();

            if (Player.IsShota() && Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()))
            {
                // 未移植：原版 GoblinAssassinScene.hx:68 调 game.goblinScene.gobboShotaRape()（GoblinScene.hx:243），
                // C# GoblinScene 还没有这个方法，所以 shota 分支只清屏返回（文本/按钮都在被调场景里）。
                return ;
            }
            int x = 0;
            if (Player.Lust100 <= 99)
            {
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.CollapseUtterlyBeaten".Localized());
            }
            if (Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()) && (!Player.HasVagina() || Random.Shared.Next((int)(2)) == 0))
            {
                x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());
                if (x < 0)
                {
                    x = Player.SmallestCockIndex();
                }
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.TearThroughYourArmor".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.WatchesImpassivelyFor".Localized());
                if (Player.CockTotal() == 1)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.ThatThing".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.ThoseThings".Localized());
                }
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.LookMightyTasty".Localized(
                    Player.Mf("stud", "sexy")
                ));
                if (Player.Tallness >= 60)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.BigWord".Localized());
                }
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.EnoughJizzToPay".Localized(
                    Player.Mf("strapping young man", "sexy breeder")
                ));
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.DancingFingersPirouette".Localized());
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.GentleCaressBefore".Localized());
                }
                else
                {
                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.TipsGivingSoakedMound".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.TaintTipsGiving".Localized());
                    }
                }
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.SheExplains".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.TryToAnswerNo".Localized());
                if (Player.Femininity < 40)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.AlmostWord".Localized());
                }
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.GirlishMoanNeedThrumming".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.ViridianSkinCaresses".Localized());
                if (Player.Cocks.Exists(c => c.SheathOn))
                {
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.TheSheathWord".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.YourBaseWord".Localized());
                }
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.BriefMomentVanishes".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.ResponseAsImmediateAs".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.MusesEmeraldSlut".Localized());
                if (Player.Cocks.Count > 1)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.PluralS".Localized());
                }
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.WildWithNeed".Localized(
                    Player.CockDescript(x)
                ));
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.ToesSmearPrecum".Localized(
                    Player.CockHead(x),
                    NumberUtils.Num2Text((float)(Player.Cocks[x].Length)),
                    Player.CockDescript(x)
                ));
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.ShortStackedWomanMoves".Localized(
                    Player.CockDescript(x)
                ));
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.SayStart".Localized());
                if (silly)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.StopRightThereScum".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.HoldYourHorses".Localized());
                }
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.EmeraldAssassinGlances".Localized());
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.InYourBalls".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.InsideYou".Localized());
                }
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.PayYourFineMight".Localized());
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.HandSqueezesEncouragingly".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.HandsSqueezeEncouragingly".Localized());
                }
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.GoblinImpalesHerself".Localized());
                if (Player.CumQ() >= 750)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.SupernaturalFertilityCan".Localized());
                    if (Player.CumQ() >= 2000)
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.SwellingRoundedPregnant".Localized());
                    }
                }
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.PassionSpendsItself".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.StandingUpWithGoo".Localized());
            }
            else
            {
                if (Player.BiggestCockArea() > CombatController.Instance!.Context.Enemy.VaginalCapacity() && Player.HasCock() && (!Player.HasVagina() || Random.Shared.Next((int)(2)) == 0))
                {
                    x = Player.BiggestCockIndex();
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.TearThroughArmorAgain".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.AssassinSighsGrumbles".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.WatchTiradeConfused".Localized(
                        Player.CockDescript(x)
                    ));
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.EmeraldCurtainsDrape".Localized(
                        Player.CockDescript(x)
                    ));
                    if (Player.BiggestCockLength() > Player.Tallness / 2)
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.LongPhallusBlocks".Localized());
                    }
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.NoneTooTenderSwat".Localized());
                    if (Player.Balls > 0)
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.BallsWord".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.CockWord".Localized());
                    }
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.GoblinGrunts".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.HesitantlyObey".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.SaysViridianTart".Localized());
                    if (Player.BiggestCockLength() < Player.Tallness / 2)
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.SittingOnYourFace".Localized(
                            Player.CockHead(x)
                        ));
                    }
                    else
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.SittingOnGroundAbove".Localized(
                            Player.CockHead(x)
                        ));
                    }
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.SoftFeetClaspDick".Localized(
                        Player.CockDescript(x)
                    ));
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.LubricatedByThe".Localized());
                    if (Player.CumQ() >= 1000)
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.SpurtHugeDollops".Localized());
                    }
                    else
                    {
                        if (Player.CumQ() >= 250)
                        {
                            OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.DroolSteadyFlow".Localized());
                        }
                    }
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.WontBeLongNow".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.TryToGruntProtest".Localized());
                    if (Player.BiggestCockLength() >= Player.Tallness / 2)
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.PulsatingIronHardCock".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.CaptorsPillowyAsscheeks".Localized());
                    }
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.GoblinPullsHard".Localized(
                        Player.CockHead(x),
                        Player.CockDescript(x)
                    ));
                    if (Player.CumQ() <= 10)
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.DribblesWord".Localized());
                    }
                    else
                    {
                        if (Player.CumQ() <= 100)
                        {
                            OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.SquirtsWord".Localized());
                        }
                        else
                        {
                            if (Player.CumQ() <= 250)
                            {
                                OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.ThickWadsWord".Localized());
                            }
                            else
                            {
                                if (Player.CumQ() <= 1750)
                                {
                                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.WavesOfGooeySpooge".Localized());
                                }
                                else
                                {
                                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.RiversOfCum".Localized());
                                }
                            }
                        }
                    }
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.IntoGoblinsBox".Localized());
                    if (Player.CumQ() >= 2500)
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.BellyRoundsUpNicely".Localized());
                    }
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.GreenGirlGivesYour".Localized(
                        Player.CockDescript(x)
                    ));
                }
                else
                {
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.DefeatedByOwnNeeds".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.MasturbatingUnabashedly".Localized());
                    if (Player.HasFuckableNipples())
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.PistonInAndOut".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.TautNippleTwisting".Localized());
                    }
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.ReverieInterrupted".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.LookingOnInConfusion".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.BeforeMutterProtest".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.TauntsFaceFuckingBox".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.IrritatedSlapCracks".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.MomentaryPainIn".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.SuckOnHerClit".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.PantingIntoGreen".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.PinprickOnTender".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.EyesRollBack".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboAssassinBeatYaUp.PassingOutInPuddle".Localized());
                }
            }
            CombatScene.CleanupAfterCombat();
            Player.ApplyStats(new DynStat(VanillaStats.Lust, -100f));
        }

        // Haxe GobboAssassinRapeIntro()   outputText 2 
        public void GobboAssassinRapeIntro()
        {
            ClearOutput();

            OutputText("Scenes.Goblinassassinscene.GobboAssassinRapeIntro.AssassinFallsToFeet".Localized());
            Player.ApplyStats(new DynStat(VanillaStats.Lust, 20f));
            if (Player.Lust < 33)
            {
                OutputText("Scenes.Goblinassassinscene.GobboAssassinRapeIntro.ArentHornyEnough".Localized());
            }
            // 原版 GoblinAssassinScene.hx:243-254 是一排 addButtonDisabled（带 tooltip/header），emit 丢成了 AddButton(...,null)。
            AddButtonDisabled(0, "Dick Fuck", "This scene requires you to have a fitting cock.", "Dick Fuck");
            AddButtonDisabled(1, "DickTooBig", "This scene requires you to have an overly large cock.", "Dick Too Big");
            AddButtonDisabled(2, "CorruptDick", "This scene requires you to have an overly large cock and high corruption.", "Corrupt Big");
            AddButtonDisabled(3, "Dick In Ass", "This scene requires you to have a cock and high corruption.", "Dick In Ass");
            AddButtonDisabled(4, "Jog Fuck", "This scene requires you to have a fitting cock.", "Jog Fuck");
            AddButtonDisabled(5, "Breastfeed", "This scene requires you to have enough milk.", "Breastfeed");
            AddButtonDisabled(6, "Use Condom", "This scene requires you to have either a condom or a spider abdomen and fitting cock.", "Use Condom");
            AddButtonDisabled(7, "Pussies", "This scene requires you to have a vagina.", "Pussies");
            AddButtonDisabled(8, "Lay Eggs", "This scene requires you to have a drider ovipositor and some eggs.", "Lay Eggs");
            if (OptionStore.Instance.Get("gore", 0) > 0)
            {
                AddButtonDisabled(10, "Womb Fuck", "This scene requires you to have a fitting cock.", "Womb Fuck");
            }
            if (Player.HasVagina() && Player.Lust >= 33)
            {
                if (Player.IsChild() && Player.HasVagina())
                {
                    // 原版 GoblinAssassinScene.hx:259 → game.goblinScene.goblinLoliScene（GoblinScene.cs:907 有同名静态方法）
                    AddButton(7, "Pussies", GoblinScene.GoblinLoliScene);
                }
                else
                {
                    AddButton(7, "Pussies", GobboGetsRapedFem);
                }
            }
            if (Player.HasCock() && Player.Lust >= 33)
            {
                if (Player.CockArea(Player.BiggestCockIndex()) > CombatController.Instance!.Context.Enemy.VaginalCapacity() && (Player.IsCorruptEnough(80) || Player.HasPerk(VanillaPerks.Sadist)))
                {
                    AddButton(2, "CorruptDick", RapeAGoblinCorruptTooBig);
                }
                if (Player.CockArea(Player.BiggestCockIndex()) > CombatController.Instance!.Context.Enemy.VaginalCapacity())
                {
                    AddButton(1, "DickTooBig", ManRapesGoblinTooBig);
                }
                if (Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()))
                {
                    AddButton(0, "Dick Fuck", GatsGoblinBoners);
                    AddButton(4, "Jog Fuck", GobboGetsRapedMaleFits);
                    if (OptionStore.Instance.Get("gore", 0) > 0)
                    {
                        // 原版 GoblinAssassinScene.hx:279 → game.goblinScene.gobboWombFuck（GoblinScene.cs:947）
                        AddButton(10, "Womb Fuck", GoblinScene.GobboWombFuck).Hint("Slice out the goblin's womb and give it a pounding.");
                    }
                }
                if (Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity()) && (Player.IsCorruptEnough(70) || Player.HasPerk(VanillaPerks.Sadist)))
                {
                    AddButton(3, "Dick In Ass", GobboButtSecks);
                }
                if (Player.Body.Get<TailPart>().Id == VanillaBodyPartTypes.SpiderAbdomen && Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()))
                {
                    AddButton(6, "Web Condom", () => GoblinCondomed(0));
                }
                else
                {
                    if (Player.HasItem(ItemRegistry.Create(Identifier.Parse("coc:condom"))) && Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()))
                    {
                        AddButton(6, "Use Condom", () => GoblinCondomed(1));
                    }
                }
                if (OptionStore.Instance.Get("watersports", 0) > 0 && Player.HasCockThatFits(60))
                {
                    // 原版 GoblinAssassinScene.hx:293 → game.goblinScene.facefuckPiss（GoblinScene.cs:923）
                    AddButton(9, "Facefuck & Piss", GoblinScene.FacefuckPiss).Hint("Facefuck the goblin. And relieve yourself");
                }
            }
            if (Player.CanOvipositSpider())
            {
                AddButton(8, "Lay Eggs", LaySomeDriderEggsInGobboTwat);
            }
            if (Player.LactationQ() >= 500 || Player.HasStatusEffect(VanillaStatusEffects.Feeder))
            {
                AddButton(5, "Breastfeed", GiveGoblinAMilkMustache);
            }
            // 原版 GoblinAssassinScene.hx:304 `setSexLeaveButton()`（CoC.hx:677-689）：
            //   默认 func = combat.cleanupAfterCombat()；只有 playerResistSex(敌人性别) 时可按，否则按钮置灰。
            var enemyGender = CombatScene.LastEnemy?.Gender switch
            {
                Gender.Male => 1,
                Gender.Female => 2,
                _ => 0
            };

            if (Player.PlayerResistSex(enemyGender))
                AddButton(14, "Common.Leave".Localized(), CombatScene.CleanupAfterCombat);
            else
                AddButtonDisabled(14, "Common.Leave".Localized(), "You can't resist the allure of sex!");
        }

        // Haxe GiveGoblinAMilkMustache()   outputText 3 
        public void GiveGoblinAMilkMustache()
        {
            ClearOutput();

            OutputText("Scenes.Goblinassassinscene.GiveGoblinAMilkMustache.SlowlyWalkUpTo".Localized(
                Appearance.BiggestBreastSizeDescript(Player)
            ));
            OutputText("Scenes.Goblinassassinscene.GiveGoblinAMilkMustache.StrugglesAgainstGrip".Localized(
                Player.Mf("slut", "stud"),
                Player.NippleDescript(0)
            ));
            OutputText("Scenes.Goblinassassinscene.GiveGoblinAMilkMustache.AfterAWhileFeel".Localized());
            Player.ApplyStats(new DynStat(VanillaStats.Libido, .2f), new DynStat(VanillaStats.Lust, -50f));
            Player.ChangeStatusValue(VanillaStatusEffects.Feeder, 0, 1);
            Player.ChangeStatusValue(VanillaStatusEffects.Feeder, 2, 0);
            CombatScene.CleanupAfterCombat();
        }

        // Haxe GobboButtSecks()   outputText 15 
        public void GobboButtSecks()
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());
            if (x < 0)
            {
                x = 0;
            }
            ShowImage("goblin-win-male-buttsex");
            OutputText("Scenes.Goblinassassinscene.GobboButtSecks.EasilyDefeatAnother".Localized());
            OutputText("Scenes.Goblinassassinscene.GobboButtSecks.BegsThoughDefeated".Localized());
            OutputText("Scenes.Goblinassassinscene.GobboButtSecks.InsultedWhoEmerged".Localized());
            OutputText("Scenes.Goblinassassinscene.GobboButtSecks.QuicklyRemovingYour".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Goblinassassinscene.GobboButtSecks.WithASneer".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Goblinassassinscene.GobboButtSecks.BegsLipsTrembling".Localized());
            OutputText("Scenes.Goblinassassinscene.GobboButtSecks.SayNothingToTrembling".Localized());
            OutputText("Scenes.Goblinassassinscene.GobboButtSecks.WithoutReservationsSlam".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Goblinassassinscene.GobboButtSecks.ProtestingSquirmsMore".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Goblinassassinscene.GobboButtSecks.CriesIfExpandingHer".Localized(
                Player.CockDescript(x)
            ));
            if (Player.Balls > 0)
            {
                OutputText(Player.SackDescript());
            }
            else
            {
                OutputText(Player.HipDescript());
            }
            OutputText("Scenes.Goblinassassinscene.GobboButtSecks.SmackingPlumpRump".Localized());
            OutputText("Scenes.Goblinassassinscene.GobboButtSecks.TightConfinesOfGoblin".Localized());
            OutputText("Scenes.Goblinassassinscene.GobboButtSecks.PickUpYourAnd".Localized());
            CombatScene.CleanupAfterCombat();
            Player.Orgasm("Dick");
        }

        // Haxe GobboGetsRapedFem()   outputText 57 
        public void GobboGetsRapedFem()
        {
            ClearOutput();

            if (Player.IsTaur())
            {
                ShowImage("goblin-win-female-taur-rapedfem");
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.PickUpGoblinHolding".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.BlushesRedFlow".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.ByThirdBottleShes".Localized(
                    Player.VaginaDescript(0)
                ));
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.FlipCreatureBack".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.SquealsThoughDelight".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.DropHerForReal".Localized(
                    Player.VaginaDescript(0)
                ));
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.CantSeeWhatShe".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.EffortsRewarded".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.CasuallyDressIgnoring".Localized());
                CombatScene.CleanupAfterCombat();
                Player.Orgasm("Vaginal");
            }
            else
            {
                if (Player.IsNaga())
                {
                    ShowImage("goblin-win-female-naga-rapedfem");
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.SlitherOverToHelpless".Localized());
                    if (Player.Body.Get<FacePart>().Id == VanillaBodyPartTypes.SnakeFangs)
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.BareYourFangs".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.SmileAtHerAnd".Localized());
                    }
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.ParagraphBreak".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.WrapYourselfTightly".Localized());
                    if (Player.Corruption > 60)
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.HissSomethingIncoherent".Localized());
                        OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.FingersSpreadOpen".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.AskHerIfShes".Localized());
                        OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.OpeningFingersOpenHer".Localized());
                        OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.PityForCreature".Localized());
                    }
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.EffectsDontTakeLong".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.SlideYourTailUp".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.SmoothMotionTail".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.NoWarningPullOut".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.PullItOutExamine".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.ShruggingDumpContents".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.ProvenVeryFalse".Localized(
                        Player.VaginaDescript(0)
                    ));
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.HearMuffledYell".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.OrgasmRepeatedly".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.ThoroughlyConfused".Localized());
                    CombatScene.CleanupAfterCombat();
                    Player.Orgasm("Vaginal");
                }
                else
                {
                    ShowImage("goblin-win-female-rapedfem");
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.PickUpGoblinSitting".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.BlushesRedFlowAgain".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.ByThirdBottleAgain".Localized(
                        Player.VaginaDescript(0)
                    ));
                    if (Player.Vaginas[0].Wetness < 2)
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.GrowsPuffyAndMoist".Localized());
                    }
                    else
                    {
                        if (Player.Vaginas[0].Wetness < 4)
                        {
                            OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.DripsWithMoisture".Localized());
                        }
                        else
                        {
                            if (Player.Vaginas[0].Wetness < 5)
                            {
                                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.SlowlySoakThighs".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.DroolsWithNeed".Localized());
                            }
                        }
                    }
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.ArousalCanWait".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.ThwackHandSmacks".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.ScoldSlappingHarder".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.SquealsThoughDelightAgain".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.PullHerOffLeg".Localized(
                        Player.VaginaDescript(0)
                    ));
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.CumOnDruggedGreen".Localized());
                    if (Player.Vaginas[0].Wetness > 4)
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.SplatteringWithFluids".Localized());
                    }
                    else
                    {
                        if (Player.Vaginas[0].Wetness > 3)
                        {
                            OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.CoatingFaceWithProof".Localized());
                        }
                        else
                        {
                            if (Player.Vaginas[0].Wetness > 2)
                            {
                                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.LeavingStickyWith".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.LeavingTasteOfPussy".Localized());
                            }
                        }
                    }
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.SentencePeriodSpace".Localized());
                    if (Player.BiggestLactation() >= 3.5f)
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.MilkExplodesFromNipples".Localized());
                    }
                    else
                    {
                        if (Player.BiggestLactation() > 2)
                        {
                            OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.TwinStreamsOfMilk".Localized());
                        }
                        else
                        {
                            if (Player.BiggestLactation() >= 1)
                            {
                                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.MilkDribblesFalling".Localized());
                            }
                        }
                    }
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.StrangeDopeyFace".Localized());
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.CasuallyDressIgnoringAgain".Localized());
                    if (Player.Corruption > 50)
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.SmirkWord".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.SighWord".Localized());
                    }
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.AndSaunterOffFeeling".Localized());
                    if (Player.Corruption < 50)
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.BitGuiltyOverdoing".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboGetsRapedFem.ThoroughlySatisfied".Localized());
                    }
                    CombatScene.CleanupAfterCombat();
                    Player.Orgasm("Vaginal");
                }
            }
        }

        // Haxe RapeAGoblinCorruptTooBig()   outputText 56 
        public void RapeAGoblinCorruptTooBig()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();
            ShowImage("goblin-win-male-corruptedfuck");
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.BeginToRemoveYour".Localized());
            if (Player.CockTotal() == 1)
            {
                OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.IsWord".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.AreAllWord".Localized());
            }
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.FarMoreAware".Localized());
            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.CockComesToRest".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.YourComesToRest".Localized(
                    Player.CockDescript(x)
                ));
            }
            if (Player.Cocks[0].Length < 20)
            {
                OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.LongAsHerTorso".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.BiggerThanSheIs".Localized());
            }
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.GivesWickedIdea".Localized());
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.SeatLegsWide".Localized(
                Player.CockDescript(x),
                Player.CockHead(x),
                Player.CockDescript(x),
                CombatController.Instance!.Context.Enemy.VaginaDescript(),
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.BothDrippingWithSweat".Localized(
                CombatController.Instance!.Context.Enemy.VaginaDescript()
            ));
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.GoblinRelaxesALittle".Localized(
                Player.CockDescript(x),
                CombatController.Instance!.Context.Enemy.VaginaDescript()
            ));
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.RepositionYourFucktoy".Localized(
                Player.CockDescript(x),
                CombatController.Instance!.Context.Enemy.VaginaDescript()
            ));
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.SaysWeaklyTipOf".Localized());
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.PleadsRestOfThe".Localized());
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.BegsShaftStartsTo".Localized());
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.MoreEntersSmallBody".Localized());
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.SheFillsToCapacity".Localized());
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.AndBeyond".Localized());
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.FuckMeSay".Localized());
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.RamHomeTheRest".Localized(
                Player.CockDescript(x)
            ));
            if (Player.Cocks[0].Length < 20)
            {
                OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.FeelMassiveBulge".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.StretchHerSoMuch".Localized());
            }
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.DrivesYourLustEven".Localized(
                CombatController.Instance!.Context.Enemy.VaginaDescript()
            ));
            if (Player.BiggestBreastSize() >= 1)
            {
                OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.HeadRestingBetween".Localized());
                if (Player.BiggestLactation() >= 1)
                {
                    OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.AsYourNippleWord".Localized());
                    if (Player.BiggestLactation() < 2)
                    {
                        OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.DripMilkyTears".Localized());
                    }
                    if (Player.BiggestLactation() < 3)
                    {
                        OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.WeepStreamsOfMilk".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.PlasterWithGouts".Localized());
                    }
                }
            }
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.HairIsMattedWith".Localized());
            if (Player.BiggestLactation() >= 1)
            {
                OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.MilkAndWord".Localized());
            }
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.SweatOfBoth".Localized());
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.YouYellHowever".Localized());
            if (Player.HasVagina())
            {
                OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.WhileYourVaginaWord".Localized(
                    Player.VaginaDescript(0)
                ));
                if (Player.Vaginas[0].Wetness <= 3)
                {
                    OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.JuicesWord".Localized());
                }
                else
                {
                    if (Player.Vaginas[0].Wetness <= 4)
                    {
                        OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.FloodsWord".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.ExplodesWord".Localized());
                    }
                }
                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.RestOfYourDrench".Localized());
                }
            }
            else
            {
                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.WhileYourOtherCock".Localized(
                        Appearance.CockNoun(VanillaBodyPartTypes.Human)
                    ));
                    if (Player.CockTotal() > 2)
                    {
                        OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.DrenchHerWord".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.DrenchesHerWord".Localized());
                    }
                }
            }
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.BitchTooTight".Localized());
            if (Player.Balls >= 2)
            {
                OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.AreTryingButH".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.HLetter".Localized());
            }
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.ErSpasmingClamping".Localized(
                CombatController.Instance!.Context.Enemy.VaginaDescript(),
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.MomentPasses".Localized());
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.SeemsLikeHours".Localized());
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.ShoutPullingTwo".Localized());
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.PressedSoFirmly".Localized(
                Player.CockDescript(x)
            ));
            if (Player.CockTotal() > 2)
            {
                OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.RemainingBlastGeysers".Localized());
            }
            if (Player.CockTotal() == 2)
            {
                OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.RemainingPenisBlasts".Localized());
            }
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.SameTightnessPrevented".Localized());
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.FinallyBlowLastWad".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.StandPrepareLeave".Localized());
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.YouSay".Localized());
            OutputText("Scenes.Goblinassassinscene.RapeAGoblinCorruptTooBig.SheAbsolutelyWill".Localized());
            Player.Orgasm("Dick");
            Player.ApplyStats(new DynStat(VanillaStats.Corruption, 1f));
            CombatScene.CleanupAfterCombat();
        }

        // Haxe ManRapesGoblinTooBig()   outputText 26 
        public void ManRapesGoblinTooBig()
        {
            ClearOutput();

            ShowImage("goblin-win-male-corruptedbj");
            var x = Player.BiggestCockIndex();
            OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.WhipOutStiffening".Localized());
            if (Player.CockArea(x) < 80)
            {
                OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.NearlyWord".Localized());
            }
            OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.AbsurdSizeToVictim".Localized());
            OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.YouWord".Localized());
            if (Player.Corruption < 50)
            {
                OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.ShrugAndGuess".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.GrinHappyToTry".Localized());
            }
            OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.TheWord".Localized());
            if (Player.Cocks[0].Id == VanillaBodyPartTypes.Horse)
            {
                OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.FlareWord".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.HeadWord".Localized());
            }
            OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.VisiblyPulsesIn".Localized(
                Player.CockDescript(x)
            ));
            if (Player.Cocks[0].Id == VanillaBodyPartTypes.Horse)
            {
                OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.FlareWordAgain".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.CrownWord".Localized());
            }
            OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.AfterMomentOrTwo".Localized());
            OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.DisgruntledTeasePull".Localized(
                Player.CockDescript(x)
            ));
            if (Player.HasKnot(0))
            {
                OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.HookingTheHeels".Localized());
            }
            OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.PeriodParagraphBreak".Localized());
            OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.MuseAsSheGrinds".Localized());
            if (Player.Gender == Gender.Hermaphrodite)
            {
                OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.HermWord".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.ManWord".Localized());
            }
            OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.ThatsForSureStart".Localized());
            OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.YoudNeverThink".Localized(
                Player.CockDescript(x)
            ));
            if (Player.CumQ() < 75)
            {
                OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.SplatteringFewMore".Localized());
            }
            else
            {
                if (Player.CumQ() < 250)
                {
                    OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.PaintingThickLayer".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.SoakingAndLeaving".Localized());
                }
            }
            OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.GreenSlutSeemsHandle".Localized());
            OutputText("Scenes.Goblinassassinscene.ManRapesGoblinTooBig.ShakeHeadAndLeave".Localized());
            Player.Orgasm("Dick");
            CombatScene.CleanupAfterCombat();
        }

        // Haxe GobboGetsRapedMaleFits()   outputText 60 
        public void GobboGetsRapedMaleFits()
        {
            ClearOutput();

            var x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());
            if (x < 0)
            {
                x = Player.BiggestCockIndex();
            }
            ShowImage("goblin-win-male-getridden");
            if (Player.CockArea(x) > CombatController.Instance!.Context.Enemy.VaginalCapacity() * .8f)
            {
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.PickUpDefeatedGoblin".Localized(
                    Player.CockDescript(x),
                    Player.CockDescript(x)
                ));
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.StartWalkingMovements".Localized());
                if (Player.Cocks[0].Id == VanillaBodyPartTypes.Demon)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.RubbingPainfully".Localized());
                }
                else
                {
                    if (Player.HasKnot(0))
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.StretchingPainfully".Localized());
                    }
                    else
                    {
                        if (Player.Cocks[0].Id == VanillaBodyPartTypes.Horse || Player.Cocks[0].TypeIndex > 3)
                        {
                            OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.FeelingPainfullyTight".Localized());
                        }
                    }
                }
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.EveryStepShe".Localized());
                if (Player.BiggestBreastSize() >= 1)
                {
                    OutputText(Appearance.AllBreastsDescript(Player));
                }
                else
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.TorsoWord".Localized());
                }
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.SupportHerselfSteady".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.PantsHappilyTongue".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.AllTheEncouragement".Localized(
                    Player.CockDescript(x)
                ));
                if (Player.BiggestBreastSize() >= 1)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.TitsWord".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.SkinWord".Localized());
                }
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.HangsOnClenching".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.SheMoans".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.FamiliarTightness".Localized(
                    Player.CockDescript(x)
                ));
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.YourTightenReleasing".Localized());
                }
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.HowlOfPowerfulOrgasm".Localized());
                if (Player.CumQ() >= 100)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.EnhancedBodyStuffs".Localized(
                        Player.CockDescript(x)
                    ));
                }
                if (Player.CumQ() >= 500)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.OrgasmSoPotent".Localized(
                        Player.CockDescript(x)
                    ));
                }
                else
                {
                    if (Player.CumQ() >= 250)
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.OrgasmSoPotentAgain".Localized(
                            Player.CockDescript(x)
                        ));
                    }
                }
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.ParagraphBreak".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.PantAndStopPulling".Localized());
                if (Player.CumQ() >= 250)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.PoursOutInRiver".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.LeaksWord".Localized());
                }
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.FromGapingTwatShe".Localized());
                if (Player.Corruption > 50)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.ChucklingWord".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.ShudderingWord".Localized());
                }
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.MakeYourWayBack".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.PickUpDefeatedAgain".Localized(
                    Player.CockDescript(x)
                ));
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.StartWalkingMovementsAgain".Localized());
                if (Player.Cocks[0].Id == VanillaBodyPartTypes.Demon)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.RubbingPerfectly".Localized());
                }
                else
                {
                    if (Player.HasKnot(0))
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.StretchingTightly".Localized());
                    }
                    else
                    {
                        if (Player.Cocks[0].Id == VanillaBodyPartTypes.Horse || Player.Cocks[0].TypeIndex > 3)
                        {
                            OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.FeelingAbsolutelyPerfect".Localized());
                        }
                    }
                }
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.EveryStepYou".Localized());
                if (Player.BiggestBreastSize() >= 1)
                {
                    OutputText(Appearance.AllBreastsDescript(Player));
                }
                else
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.TorsoWordAgain".Localized());
                }
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.SupportHerselfSteadyAgain".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.PantsHappilyTongueAgain".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.AllTheEncouragementAgain".Localized(
                    Player.CockDescript(x)
                ));
                if (Player.BiggestBreastSize() >= 1)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.TitsWordAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.SkinWordAgain".Localized());
                }
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.HangsOnClenchingAgain".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.SheMoansAgain".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.FamiliarTightnessAgain".Localized(
                    Player.CockDescript(x)
                ));
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.YourTightenReleasingAgain".Localized());
                }
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.HowlOfOrgasmAgain".Localized());
                if (Player.CumQ() >= 250)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.EnhancedBodyStuffsAgain".Localized());
                    if (Player.Balls > 0)
                    {
                        OutputText(Player.BallsDescriptLight());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.LegsWord".Localized());
                    }
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.SentencePeriod".Localized());
                }
                if (Player.CumQ() >= 500)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.OrgasmSoPotentThird".Localized(
                        Player.CockDescript(x)
                    ));
                }
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.ParagraphBreakAgain".Localized());
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.PantStopPullingAgain".Localized());
                if (Player.CumQ() >= 250)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.PoursOutRiverAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.LeaksWordAgain".Localized());
                }
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.FromGapingTwatAgain".Localized());
                if (Player.Corruption > 50)
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.ChucklingWordAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.ShudderingWordAgain".Localized());
                }
                OutputText("Scenes.Goblinassassinscene.GobboGetsRapedMaleFits.MakeWayBackAgain".Localized());
            }
            CombatScene.CleanupAfterCombat();
            Player.Orgasm("Dick");
        }

        // Haxe GoblinCondomed()   outputText 14 
        public void GoblinCondomed(float mode = 0)
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());
            ShowImage("goblin-win-male-goblincondomed");
            OutputText("Scenes.Goblinassassinscene.GoblinCondomed.DefeatedGoblinGirlsKnees".Localized());
            OutputText("Scenes.Goblinassassinscene.GoblinCondomed.ImpatientlyThrustsLegs".Localized(
                Player.CockDescript(x)
            ));
            if (Player.Balls > 0)
            {
                OutputText("Scenes.Goblinassassinscene.GoblinCondomed.YourThrobbingSack".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinassassinscene.GoblinCondomed.BaseOfYourShaft".Localized());
            }
            OutputText("Scenes.Goblinassassinscene.GoblinCondomed.DigitsCurlAround".Localized());
            OutputText("Scenes.Goblinassassinscene.GoblinCondomed.WideningLakeOf".Localized());
            if (mode == 0)
            {
                OutputText("Scenes.Goblinassassinscene.GoblinCondomed.WorkYourSpinnerets".Localized(
                    Player.CockDescript(x)
                ));
            }
            else
            {
                Player.DestroyItems(Identifier.Parse("coc:condom"), 1);
                OutputText("Scenes.Goblinassassinscene.GoblinCondomed.TearOpenPacket".Localized(
                    Player.CockDescript(x)
                ));
            }
            OutputText("Scenes.Goblinassassinscene.GoblinCondomed.TimeToGiveGoblin".Localized());
            OutputText("Scenes.Goblinassassinscene.GoblinCondomed.HoldingWrigglingFeet".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Goblinassassinscene.GoblinCondomed.MouthyBitchForgot".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Goblinassassinscene.GoblinCondomed.SquirmingRightSideUp".Localized());
            if (mode == 0)
            {
                OutputText("Scenes.Goblinassassinscene.GoblinCondomed.InformHerSpider".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinassassinscene.GoblinCondomed.InformHerLatex".Localized());
            }
            Player.Orgasm("Dick");
            CombatScene.CleanupAfterCombat();
        }

        // Haxe GatsGoblinBoners()   outputText 24 
        public void GatsGoblinBoners()
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());
            if (x < 0)
            {
                x = Player.SmallestCockIndex();
            }
            ShowImage("goblin-win-male-goblinboners");
            OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.GoblinLiesStrewn".Localized());
            OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.LuckilyHaveEvery".Localized());
            OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.BeginToFondleCock".Localized());
            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.PluralS".Localized());
            }
            OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.WalkTowardsUnsuspecting".Localized());
            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.PluralSAgain".Localized());
            }
            OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.PeriodParagraphBreak".Localized());
            OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.YelpsMoreSurprised".Localized());
            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.PluralSThird".Localized());
            }
            OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.AndSlapWord".Localized());
            if (Player.CockTotal() == 1)
            {
                OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.ItWord".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.ThemWord".Localized());
            }
            OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.AgainstBareFlesh".Localized(
                Player.CockDescript(x)
            ));
            if (Player.CockTotal() == 1)
            {
                OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.DickInsidePanting".Localized());
            }
            else
            {
                if (Player.CockTotal() == 2)
                {
                    OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.BothDicksInside".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.TwoDicksInside".Localized());
                    if (Player.CockTotal() >= 4)
                    {
                        OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.PluralSFourth".Localized());
                    }
                    OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.RemainingSandwiched".Localized());
                }
            }
            OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.PeriodParagraphBreakAgain".Localized());
            OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.RoughlyPound".Localized());
            OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.EventuallyBeginToFeel".Localized(
                Player.CockDescript(x)
            ));
            if (Player.CockTotal() == 2)
            {
                OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.PluralSFifth".Localized());
            }
            OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.SlowlyReleaseYourself".Localized());
            OutputText("Scenes.Goblinassassinscene.GatsGoblinBoners.PickYourselfBackUp".Localized(
                Player.CockDescript(x)
            ));
            Player.Orgasm("Dick");
            CombatScene.CleanupAfterCombat();
        }

        // Haxe LaySomeDriderEggsInGobboTwat()   outputText 22 
        public void LaySomeDriderEggsInGobboTwat()
        {
            ClearOutput();

            ShowImage("goblin-win-drider-egged");
            OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.PitifulStruggling".Localized());
            if (Player.HasCock())
            {
                OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.HardenWord".Localized());
            }
            else
            {
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.MoistenWord".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.TwitchWord".Localized());
                }
            }
            OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.SlightlyCantHelp".Localized());
            OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.UndressAndSkitterOver".Localized());
            if (Player.Femininity > 50)
            {
                OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.HaventGotAnyBitch".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.HaventGotAnyBastard".Localized());
            }
            OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.LashesOutWithFeet".Localized());
            OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.SquirmsAndYouSlip".Localized());
            OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.PlaythingFallenSilent".Localized());
            OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.NervousToneNothing".Localized());
            OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.StifledGaspsEscape".Localized());
            OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.FinallyBottomOut".Localized());
            OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.FeelItsSlowPath".Localized());
            if (Player.HasCock())
            {
                OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.OwnOrgasmJustAs".Localized());
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.AndYFragment".Localized());
                }
            }
            else
            {
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.YFragment".Localized());
                }
            }
            if (Player.HasVagina())
            {
                OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.OurClenchingInTime".Localized());
            }
            OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.NextEggRollsForward".Localized());
            OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.KeepHerPinned".Localized());
            OutputText("Scenes.Goblinassassinscene.LaySomeDriderEggsInGobboTwat.LayingHerInShade".Localized());
            Player.DumpEggs();
            Player.Orgasm("Ovi");
            CombatScene.CleanupAfterCombat();
        }

    }
}
