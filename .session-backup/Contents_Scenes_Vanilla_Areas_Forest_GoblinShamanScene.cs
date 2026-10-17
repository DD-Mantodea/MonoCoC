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
    [Register("coc:goblin_shaman_scene")]
    public class GoblinShamanScene : Scene
    {
        // 原版 Creature.isLoli()：isChild() && hasVagina()（Creature.hx:3004-3007）。
        // C# 的 Creature 只有 IsShota()/IsChild()，没有 IsLoli()（Contents/Creatures 不在本任务可改范围），
        // 所以在场景内按同一判据实现。
        private static bool IsLoliPlayer() => Player.IsChild() && Player.HasVagina();



        private static bool noFur => !Player.HasFur();

        private static bool silly => OptionStore.Instance.Get("silly", 0) > 0;

        private static bool metric => OptionStore.Instance.Get("metric", 0) > 0;

        private static bool realistic => OptionStore.Instance.Get("realistic", 0) > 0;

        private static bool survival => OptionStore.Instance.Get("survival", 0) > 0;

        private static bool hasKnot => Player.Cocks.Count > 0 && Player.Cocks.Any(c => c.KnotMultiplier > 1);

        private static void ReturnToCamp(int hours) => Go(SceneRegistry.CreateOrDefault("coc:camp"));

        public override void Entry()
        {
            GoblinShamanEncounter();
        }

        // New

        // Haxe GoblinShamanEncounter()   outputText 4 
        public void GoblinShamanEncounter()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(VanillaFlags.TimesEncounteredGoblinShaman) == 0)
            {
                OutputText("Scenes.Goblinshamanscene.GoblinShamanEncounter.GoblinJumpsOut".Localized());
                OutputText("Scenes.Goblinshamanscene.GoblinShamanEncounter.CastYouIntoSubmission".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinshamanscene.GoblinShamanEncounter.MysticalGoblinCatches".Localized());
                OutputText("Scenes.Goblinshamanscene.GoblinShamanEncounter.CastSubmissionAgain".Localized());
            }
            CoC.Instance.Flags.Set(VanillaFlags.TimesEncounteredGoblinShaman, CoC.Instance.Flags.Get(VanillaFlags.TimesEncounteredGoblinShaman) + 1);
            Player.UnlockCodexEntry(VanillaFlags.CodexEntryGoblins);
            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "goblin_shaman")), () => Go(SceneRegistry.CreateOrDefault("coc:forest")));
        }

        // Haxe GoblinShamanBeatYaUp()   outputText 85 
        public void GoblinShamanBeatYaUp()
        {
            ClearOutput();

            if (Player.IsShota() && Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()))
            {
                // 
                return ;
            }
            int x = 0;
            if (Player.Lust100 <= 99)
            {
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.CollapseUtterlyBeaten".Localized());
            }
            if (Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()) && (!Player.HasVagina() || Random.Shared.Next((int)(2)) == 0))
            {
                x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());
                if (x < 0)
                {
                    x = Player.SmallestCockIndex();
                }
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.SoArousedSoHorny".Localized());
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.WatchesImpassivelyFor".Localized());
                if (Player.CockTotal() == 1)
                {
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.ThatThing".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.ThoseThings".Localized());
                }
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.LookMightyTasty".Localized(
                    Player.Mf("stud", "sexy")
                ));
                if (Player.Tallness >= 60)
                {
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.BigWord".Localized());
                }
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.EnoughJizzToPay".Localized(
                    Player.Mf("strapping young man", "sexy breeder")
                ));
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.DancingFingersPirouette".Localized());
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.GentleCaressBefore".Localized());
                }
                else
                {
                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.TipsGivingSoakedMound".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.TaintTipsGiving".Localized());
                    }
                }
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.SheExplains".Localized());
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.TryToAnswerNo".Localized());
                if (Player.Femininity < 40)
                {
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.AlmostWord".Localized());
                }
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.GirlishMoanNeedThrumming".Localized());
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.ViridianSkinCaresses".Localized());
                if (Player.Cocks.Exists(c => c.SheathOn))
                {
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.TheSheathWord".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.YourBaseWord".Localized());
                }
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.BriefMomentVanishes".Localized());
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.ResponseAsImmediateAs".Localized());
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.MusesEmeraldSlut".Localized());
                if (Player.Cocks.Count > 1)
                {
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.PluralS".Localized());
                }
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.WildWithNeed".Localized(
                    Player.CockDescript(x)
                ));
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.ToesSmearPrecum".Localized(
                    Player.CockHead(x),
                    NumberUtils.Num2Text((float)(Player.Cocks[x].Length)),
                    Player.CockDescript(x)
                ));
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.ShortStackedWomanMoves".Localized(
                    Player.CockDescript(x)
                ));
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.SayStart".Localized());
                if (silly)
                {
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.StopRightThereScum".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.HoldYourHorses".Localized());
                }
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.EmeraldShamanGlances".Localized());
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.InYourBalls".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.InsideYou".Localized());
                }
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.PayYourFineMight".Localized());
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.HandSqueezesEncouragingly".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.HandsSqueezeEncouragingly".Localized());
                }
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.GoblinImpalesHerself".Localized());
                if (Player.CumQ() >= 750)
                {
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.SupernaturalFertilityCan".Localized());
                    if (Player.CumQ() >= 2000)
                    {
                        OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.SwellingRoundedPregnant".Localized());
                    }
                }
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.PassionSpendsItself".Localized());
                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.StandingUpWithGoo".Localized());
            }
            else
            {
                if (Player.BiggestCockArea() > CombatController.Instance!.Context.Enemy.VaginalCapacity() && Player.HasCock() && (!Player.HasVagina() || Random.Shared.Next((int)(2)) == 0))
                {
                    x = Player.BiggestCockIndex();
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.TearThroughYourArmor".Localized());
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.ShamanSighsGrumbles".Localized());
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.WatchTiradeConfused".Localized(
                        Player.CockDescript(x)
                    ));
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.EmeraldCurtainsDrape".Localized(
                        Player.CockDescript(x)
                    ));
                    if (Player.BiggestCockLength() > Player.Tallness / 2)
                    {
                        OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.LongPhallusBlocks".Localized());
                    }
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.NoneTooTenderSwat".Localized());
                    if (Player.Balls > 0)
                    {
                        OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.BallsWord".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.CockWord".Localized());
                    }
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.GoblinGrunts".Localized());
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.HesitantlyObey".Localized());
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.SaysViridianTart".Localized());
                    if (Player.BiggestCockLength() < Player.Tallness / 2)
                    {
                        OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.SittingOnYourFace".Localized(
                            Player.CockHead(x)
                        ));
                    }
                    else
                    {
                        OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.SittingOnGroundAbove".Localized(
                            Player.CockHead(x)
                        ));
                    }
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.SoftFeetClaspDick".Localized(
                        Player.CockDescript(x)
                    ));
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.LubricatedByThe".Localized());
                    if (Player.CumQ() >= 1000)
                    {
                        OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.SpurtHugeDollops".Localized());
                    }
                    else
                    {
                        if (Player.CumQ() >= 250)
                        {
                            OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.DroolSteadyFlow".Localized());
                        }
                    }
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.WontBeLongNow".Localized());
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.TryToGruntProtest".Localized());
                    if (Player.BiggestCockLength() >= Player.Tallness / 2)
                    {
                        OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.PulsatingIronHardCock".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.CaptorsPillowyAsscheeks".Localized());
                    }
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.GoblinPullsHard".Localized(
                        Player.CockHead(x),
                        Player.CockDescript(x)
                    ));
                    if (Player.CumQ() <= 10)
                    {
                        OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.DribblesWord".Localized());
                    }
                    else
                    {
                        if (Player.CumQ() <= 100)
                        {
                            OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.SquirtsWord".Localized());
                        }
                        else
                        {
                            if (Player.CumQ() <= 250)
                            {
                                OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.ThickWadsWord".Localized());
                            }
                            else
                            {
                                if (Player.CumQ() <= 1750)
                                {
                                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.WavesOfGooeySpooge".Localized());
                                }
                                else
                                {
                                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.RiversOfCum".Localized());
                                }
                            }
                        }
                    }
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.IntoGoblinsBox".Localized());
                    if (Player.CumQ() >= 2500)
                    {
                        OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.BellyRoundsUpNicely".Localized());
                    }
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.GreenGirlGivesYour".Localized(
                        Player.CockDescript(x)
                    ));
                }
                else
                {
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.DefeatedByOwnNeeds".Localized());
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.MasturbatingUnabashedly".Localized());
                    if (Player.HasFuckableNipples())
                    {
                        OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.PistonInAndOut".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.TautNippleTwisting".Localized());
                    }
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.ReverieInterrupted".Localized());
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.LookingOnInConfusion".Localized());
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.BeforeMutterProtest".Localized());
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.TauntsFaceFuckingBox".Localized());
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.IrritatedSlapCracks".Localized());
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.MomentaryPainIn".Localized());
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.SuckOnHerClit".Localized());
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.PantingIntoGreen".Localized());
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.PinprickOnTender".Localized());
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.EyesRollBack".Localized());
                    OutputText("Scenes.Goblinshamanscene.GoblinShamanBeatYaUp.PassingOutInPuddle".Localized());
                }
            }
            Player.Orgasm("Generic");
            CombatScene.CleanupAfterCombat();
        }

        // Haxe GoblinShamanRapeIntro()   outputText 2 
        public void GoblinShamanRapeIntro()
        {
            ClearOutput();

            OutputText("Scenes.Goblinshamanscene.GoblinShamanRapeIntro.ShamanFallsToFeet".Localized());
            if (Player.Lust < 33)
            {
                OutputText("Scenes.Goblinshamanscene.GoblinShamanRapeIntro.ArentHornyEnough".Localized());
            }
            // 原版 addButtonDisabled 的默认禁用占位（GoblinShamanScene.hx:231-241），后续条件满足时
            // 会用 addButton 覆盖同一个槽位。
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
                if (IsLoliPlayer())
                {
                    // 原版 game.goblinScene.goblinLoliScene（GoblinShamanScene.hx:248）
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
                        // 原版 game.goblinScene.gobboWombFuck（GoblinShamanScene.hx:268）
                        AddButton(10, "Womb Fuck", GoblinScene.GobboWombFuck);
                    }
                }
                if (Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity()) >= 0 && (Player.IsCorruptEnough(70) || Player.HasPerk(VanillaPerks.Sadist)))
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
                if (OptionStore.Instance.Get("watersports", 0) > 0 && Player.CockThatFits(60) >= 0)
                {
                    // 原版 game.goblinScene.facefuckPiss（GoblinShamanScene.hx:282）
                    AddButton(9, "Facefuck & Piss", GoblinScene.FacefuckPiss);
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
            AddButton(14, "Leave", () => ReturnToCamp(1));
        }

        // Haxe GiveGoblinAMilkMustache()   outputText 3 
        public void GiveGoblinAMilkMustache()
        {
            ClearOutput();

            OutputText("Scenes.Goblinshamanscene.GiveGoblinAMilkMustache.SlowlyWalkUpTo".Localized(
                Appearance.BiggestBreastSizeDescript(Player)
            ));
            OutputText("Scenes.Goblinshamanscene.GiveGoblinAMilkMustache.StrugglesAgainstGrip".Localized(
                Player.Mf("slut", "stud"),
                Player.NippleDescript(0)
            ));
            OutputText("Scenes.Goblinshamanscene.GiveGoblinAMilkMustache.AfterAWhileFeel".Localized());
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
            OutputText("Scenes.Goblinshamanscene.GobboButtSecks.EasilyDefeatAnother".Localized());
            OutputText("Scenes.Goblinshamanscene.GobboButtSecks.BegsThoughDefeated".Localized());
            OutputText("Scenes.Goblinshamanscene.GobboButtSecks.InsultedWhoEmerged".Localized());
            OutputText("Scenes.Goblinshamanscene.GobboButtSecks.QuicklyRemovingYour".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Goblinshamanscene.GobboButtSecks.WithASneer".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Goblinshamanscene.GobboButtSecks.BegsLipsTrembling".Localized());
            OutputText("Scenes.Goblinshamanscene.GobboButtSecks.SayNothingToTrembling".Localized());
            OutputText("Scenes.Goblinshamanscene.GobboButtSecks.WithoutReservationsSlam".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Goblinshamanscene.GobboButtSecks.ProtestingSquirmsMore".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Goblinshamanscene.GobboButtSecks.CriesIfExpandingHer".Localized(
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
            OutputText("Scenes.Goblinshamanscene.GobboButtSecks.SmackingPlumpRump".Localized());
            OutputText("Scenes.Goblinshamanscene.GobboButtSecks.TightConfinesOfGoblin".Localized());
            OutputText("Scenes.Goblinshamanscene.GobboButtSecks.PickUpYourAnd".Localized());
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
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.PickUpGoblinHolding".Localized());
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.BlushesRedFlow".Localized());
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.ByThirdBottleShes".Localized(
                    Player.VaginaDescript(0)
                ));
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.FlipCreatureBack".Localized());
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.SquealsThoughDelight".Localized());
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.DropHerForReal".Localized(
                    Player.VaginaDescript(0)
                ));
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.CantSeeWhatShe".Localized());
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.EffortsRewarded".Localized());
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.CasuallyDressIgnoring".Localized());
                CombatScene.CleanupAfterCombat();
                Player.Orgasm("Vaginal");
            }
            else
            {
                if (Player.IsNaga())
                {
                    ShowImage("goblin-win-female-naga-rapedfem");
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.SlitherOverToHelpless".Localized());
                    if (Player.Body.Get<FacePart>().Id == VanillaBodyPartTypes.SnakeFangs)
                    {
                        OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.BareYourFangs".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.SmileAtHerAnd".Localized());
                    }
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.ParagraphBreak".Localized());
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.WrapYourselfTightly".Localized());
                    if (Player.Corruption > 60)
                    {
                        OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.HissSomethingIncoherent".Localized());
                        OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.FingersSpreadOpen".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.AskHerIfShes".Localized());
                        OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.OpeningFingersOpenHer".Localized());
                        OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.PityForCreature".Localized());
                    }
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.EffectsDontTakeLong".Localized());
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.SlideYourTailUp".Localized());
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.SmoothMotionTail".Localized());
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.NoWarningPullOut".Localized());
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.PullItOutExamine".Localized());
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.ShruggingDumpContents".Localized());
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.ProvenVeryFalse".Localized(
                        Player.VaginaDescript(0)
                    ));
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.HearMuffledYell".Localized());
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.OrgasmRepeatedly".Localized());
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.ThoroughlyConfused".Localized());
                    CombatScene.CleanupAfterCombat();
                    Player.Orgasm("Vaginal");
                }
                else
                {
                    ShowImage("goblin-win-female-rapedfem");
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.PickUpGoblinSitting".Localized());
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.BlushesRedFlowAgain".Localized());
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.ByThirdBottleAgain".Localized(
                        Player.VaginaDescript(0)
                    ));
                    if (Player.Vaginas[0].Wetness < 2)
                    {
                        OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.GrowsPuffyAndMoist".Localized());
                    }
                    else
                    {
                        if (Player.Vaginas[0].Wetness < 4)
                        {
                            OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.DripsWithMoisture".Localized());
                        }
                        else
                        {
                            if (Player.Vaginas[0].Wetness < 5)
                            {
                                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.SlowlySoakThighs".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.DroolsWithNeed".Localized());
                            }
                        }
                    }
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.ArousalCanWait".Localized());
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.ThwackHandSmacks".Localized());
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.ScoldSlappingHarder".Localized());
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.SquealsThoughDelightAgain".Localized());
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.PullHerOffLeg".Localized(
                        Player.VaginaDescript(0)
                    ));
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.CumOnDruggedGreen".Localized());
                    if (Player.Vaginas[0].Wetness > 4)
                    {
                        OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.SplatteringWithFluids".Localized());
                    }
                    else
                    {
                        if (Player.Vaginas[0].Wetness > 3)
                        {
                            OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.CoatingFaceWithProof".Localized());
                        }
                        else
                        {
                            if (Player.Vaginas[0].Wetness > 2)
                            {
                                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.LeavingStickyWith".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.LeavingTasteOfPussy".Localized());
                            }
                        }
                    }
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.SentencePeriodSpace".Localized());
                    if (Player.BiggestLactation() >= 3.5f)
                    {
                        OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.MilkExplodesFromNipples".Localized());
                    }
                    else
                    {
                        if (Player.BiggestLactation() > 2)
                        {
                            OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.TwinStreamsOfMilk".Localized());
                        }
                        else
                        {
                            if (Player.BiggestLactation() >= 1)
                            {
                                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.MilkDribblesFalling".Localized());
                            }
                        }
                    }
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.StrangeDopeyFace".Localized());
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.CasuallyDressIgnoringAgain".Localized());
                    if (Player.Corruption > 50)
                    {
                        OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.SmirkWord".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.SighWord".Localized());
                    }
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.AndSaunterOffFeeling".Localized());
                    if (Player.Corruption < 50)
                    {
                        OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.BitGuiltyOverdoing".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinshamanscene.GobboGetsRapedFem.ThoroughlySatisfied".Localized());
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
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.BeginToRemoveYour".Localized());
            if (Player.CockTotal() == 1)
            {
                OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.IsWord".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.AreAllWord".Localized());
            }
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.FarMoreAware".Localized());
            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.CockComesToRest".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.YourComesToRest".Localized(
                    Player.CockDescript(x)
                ));
            }
            if (Player.Cocks[0].Length < 20)
            {
                OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.LongAsHerTorso".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.BiggerThanSheIs".Localized());
            }
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.GivesWickedIdea".Localized());
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.SeatLegsWide".Localized(
                Player.CockDescript(x),
                Player.CockHead(x),
                Player.CockDescript(x),
                CombatController.Instance!.Context.Enemy.VaginaDescript(),
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.BothDrippingWithSweat".Localized(
                CombatController.Instance!.Context.Enemy.VaginaDescript()
            ));
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.GoblinRelaxesALittle".Localized(
                Player.CockDescript(x),
                CombatController.Instance!.Context.Enemy.VaginaDescript()
            ));
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.RepositionYourFucktoy".Localized(
                Player.CockDescript(x),
                CombatController.Instance!.Context.Enemy.VaginaDescript()
            ));
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.SaysWeaklyTipOf".Localized());
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.PleadsRestOfThe".Localized());
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.BegsShaftStartsTo".Localized());
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.MoreEntersSmallBody".Localized());
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.SheFillsToCapacity".Localized());
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.AndBeyond".Localized());
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.FuckMeSay".Localized());
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.RamHomeTheRest".Localized(
                Player.CockDescript(x)
            ));
            if (Player.Cocks[0].Length < 20)
            {
                OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.FeelMassiveBulge".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.StretchHerSoMuch".Localized());
            }
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.DrivesYourLustEven".Localized(
                CombatController.Instance!.Context.Enemy.VaginaDescript()
            ));
            if (Player.BiggestBreastSize() >= 1)
            {
                OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.HeadRestingBetween".Localized());
                if (Player.BiggestLactation() >= 1)
                {
                    OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.AsYourNippleWord".Localized());
                    if (Player.BiggestLactation() < 2)
                    {
                        OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.DripMilkyTears".Localized());
                    }
                    if (Player.BiggestLactation() < 3)
                    {
                        OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.WeepStreamsOfMilk".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.PlasterWithGouts".Localized());
                    }
                }
            }
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.HairIsMattedWith".Localized());
            if (Player.BiggestLactation() >= 1)
            {
                OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.MilkAndWord".Localized());
            }
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.SweatOfBoth".Localized());
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.YouYellHowever".Localized());
            if (Player.HasVagina())
            {
                OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.WhileYourVaginaWord".Localized(
                    Player.VaginaDescript(0)
                ));
                if (Player.Vaginas[0].Wetness <= 3)
                {
                    OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.JuicesWord".Localized());
                }
                else
                {
                    if (Player.Vaginas[0].Wetness <= 4)
                    {
                        OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.FloodsWord".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.ExplodesWord".Localized());
                    }
                }
                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.RestOfYourDrench".Localized());
                }
            }
            else
            {
                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.WhileYourOtherCock".Localized(
                        Appearance.CockNoun(VanillaBodyPartTypes.Human)
                    ));
                    if (Player.CockTotal() > 2)
                    {
                        OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.DrenchHerWord".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.DrenchesHerWord".Localized());
                    }
                }
            }
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.BitchTooTight".Localized());
            if (Player.Balls >= 2)
            {
                OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.AreTryingButH".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.HLetter".Localized());
            }
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.ErSpasmingClamping".Localized(
                CombatController.Instance!.Context.Enemy.VaginaDescript(),
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.MomentPasses".Localized());
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.SeemsLikeHours".Localized());
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.ShoutPullingTwo".Localized());
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.PressedSoFirmly".Localized(
                Player.CockDescript(x)
            ));
            if (Player.CockTotal() > 2)
            {
                OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.RemainingBlastGeysers".Localized());
            }
            if (Player.CockTotal() == 2)
            {
                OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.RemainingPenisBlasts".Localized());
            }
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.SameTightnessPrevented".Localized());
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.FinallyBlowLastWad".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.StandPrepareLeave".Localized());
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.YouSay".Localized());
            OutputText("Scenes.Goblinshamanscene.RapeAGoblinCorruptTooBig.SheAbsolutelyWill".Localized());
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
            OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.WhipOutStiffening".Localized());
            if (Player.CockArea(x) < 80)
            {
                OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.NearlyWord".Localized());
            }
            OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.AbsurdSizeToVictim".Localized());
            OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.YouWord".Localized());
            if (Player.Corruption < 50)
            {
                OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.ShrugAndGuess".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.GrinHappyToTry".Localized());
            }
            OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.TheWord".Localized());
            if (Player.Cocks[0].Id == VanillaBodyPartTypes.Horse)
            {
                OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.FlareWord".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.HeadWord".Localized());
            }
            OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.VisiblyPulsesIn".Localized(
                Player.CockDescript(x)
            ));
            if (Player.Cocks[0].Id == VanillaBodyPartTypes.Horse)
            {
                OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.FlareWordAgain".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.CrownWord".Localized());
            }
            OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.AfterMomentOrTwo".Localized());
            OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.DisgruntledTeasePull".Localized(
                Player.CockDescript(x)
            ));
            if (Player.HasKnot(0))
            {
                OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.HookingTheHeels".Localized());
            }
            OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.PeriodParagraphBreak".Localized());
            OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.MuseAsSheGrinds".Localized());
            if (Player.Gender == Gender.Hermaphrodite)
            {
                OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.HermWord".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.ManWord".Localized());
            }
            OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.ThatsForSureStart".Localized());
            OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.YoudNeverThink".Localized(
                Player.CockDescript(x)
            ));
            if (Player.CumQ() < 75)
            {
                OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.SplatteringFewMore".Localized());
            }
            else
            {
                if (Player.CumQ() < 250)
                {
                    OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.PaintingThickLayer".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.SoakingAndLeaving".Localized());
                }
            }
            OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.GreenSlutSeemsHandle".Localized());
            OutputText("Scenes.Goblinshamanscene.ManRapesGoblinTooBig.ShakeHeadAndLeave".Localized());
            Player.Orgasm("Dick");
            CombatScene.CleanupAfterCombat();
        }

        // Haxe GobboGetsRapedMaleFits()   outputText 60 
        public void GobboGetsRapedMaleFits()
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());
            if (x < 0)
            {
                x = Player.BiggestCockIndex();
            }
            ShowImage("goblin-win-male-getridden");
            if (Player.CockArea(x) > CombatController.Instance!.Context.Enemy.VaginalCapacity() * .8f)
            {
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.PickUpDefeatedGoblin".Localized(
                    Player.CockDescript(x),
                    Player.CockDescript(x)
                ));
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.StartWalkingMovements".Localized());
                if (Player.Cocks[0].Id == VanillaBodyPartTypes.Demon)
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.RubbingPainfully".Localized());
                }
                else
                {
                    if (Player.HasKnot(0))
                    {
                        OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.StretchingPainfully".Localized());
                    }
                    else
                    {
                        if (Player.Cocks[0].Id == VanillaBodyPartTypes.Horse || Player.Cocks[0].TypeIndex > 3)
                        {
                            OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.FeelingPainfullyTight".Localized());
                        }
                    }
                }
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.EveryStepShe".Localized());
                if (Player.BiggestBreastSize() >= 1)
                {
                    OutputText(Appearance.AllBreastsDescript(Player));
                }
                else
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.TorsoWord".Localized());
                }
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.SupportHerselfSteady".Localized());
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.PantsHappilyTongue".Localized());
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.AllTheEncouragement".Localized(
                    Player.CockDescript(x)
                ));
                if (Player.BiggestBreastSize() >= 1)
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.TitsWord".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.SkinWord".Localized());
                }
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.HangsOnClenching".Localized());
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.SheMoans".Localized());
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.FamiliarTightness".Localized(
                    Player.CockDescript(x)
                ));
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.YourTightenReleasing".Localized());
                }
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.HowlOfPowerfulOrgasm".Localized());
                if (Player.CumQ() >= 100)
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.EnhancedBodyStuffs".Localized(
                        Player.CockDescript(x)
                    ));
                }
                if (Player.CumQ() >= 500)
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.OrgasmSoPotent".Localized(
                        Player.CockDescript(x)
                    ));
                }
                else
                {
                    if (Player.CumQ() >= 250)
                    {
                        OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.OrgasmSoPotentAgain".Localized(
                            Player.CockDescript(x)
                        ));
                    }
                }
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.ParagraphBreak".Localized());
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.PantAndStopPulling".Localized());
                if (Player.CumQ() >= 250)
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.PoursOutInRiver".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.LeaksWord".Localized());
                }
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.FromGapingTwatShe".Localized());
                if (Player.Corruption > 50)
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.ChucklingWord".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.ShudderingWord".Localized());
                }
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.MakeYourWayBack".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.PickUpDefeatedAgain".Localized(
                    Player.CockDescript(x)
                ));
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.StartWalkingMovementsAgain".Localized());
                if (Player.Cocks[0].Id == VanillaBodyPartTypes.Demon)
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.RubbingPerfectly".Localized());
                }
                else
                {
                    if (Player.HasKnot(0))
                    {
                        OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.StretchingTightly".Localized());
                    }
                    else
                    {
                        if (Player.Cocks[0].Id == VanillaBodyPartTypes.Horse || Player.Cocks[0].TypeIndex > 3)
                        {
                            OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.FeelingAbsolutelyPerfect".Localized());
                        }
                    }
                }
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.EveryStepYou".Localized());
                if (Player.BiggestBreastSize() >= 1)
                {
                    OutputText(Appearance.AllBreastsDescript(Player));
                }
                else
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.TorsoWordAgain".Localized());
                }
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.SupportHerselfSteadyAgain".Localized());
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.PantsHappilyTongueAgain".Localized());
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.AllTheEncouragementAgain".Localized(
                    Player.CockDescript(x)
                ));
                if (Player.BiggestBreastSize() >= 1)
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.TitsWordAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.SkinWordAgain".Localized());
                }
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.HangsOnClenchingAgain".Localized());
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.SheMoansAgain".Localized());
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.FamiliarTightnessAgain".Localized(
                    Player.CockDescript(x)
                ));
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.YourTightenReleasingAgain".Localized());
                }
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.HowlOfOrgasmAgain".Localized());
                if (Player.CumQ() >= 250)
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.EnhancedBodyStuffsAgain".Localized());
                    if (Player.Balls > 0)
                    {
                        OutputText(Player.BallsDescriptLight());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.LegsWord".Localized());
                    }
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.SentencePeriod".Localized());
                }
                if (Player.CumQ() >= 500)
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.OrgasmSoPotentThird".Localized(
                        Player.CockDescript(x)
                    ));
                }
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.ParagraphBreakAgain".Localized());
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.PantStopPullingAgain".Localized());
                if (Player.CumQ() >= 250)
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.PoursOutRiverAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.LeaksWordAgain".Localized());
                }
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.FromGapingTwatAgain".Localized());
                if (Player.Corruption > 50)
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.ChucklingWordAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.ShudderingWordAgain".Localized());
                }
                OutputText("Scenes.Goblinshamanscene.GobboGetsRapedMaleFits.MakeWayBackAgain".Localized());
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
            OutputText("Scenes.Goblinshamanscene.GoblinCondomed.DefeatedGoblinGirlsKnees".Localized());
            OutputText("Scenes.Goblinshamanscene.GoblinCondomed.ImpatientlyThrustsLegs".Localized(
                Player.CockDescript(x)
            ));
            if (Player.Balls > 0)
            {
                OutputText("Scenes.Goblinshamanscene.GoblinCondomed.YourThrobbingSack".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinshamanscene.GoblinCondomed.BaseOfYourShaft".Localized());
            }
            OutputText("Scenes.Goblinshamanscene.GoblinCondomed.DigitsCurlAround".Localized());
            OutputText("Scenes.Goblinshamanscene.GoblinCondomed.WideningLakeOf".Localized());
            if (mode == 0)
            {
                OutputText("Scenes.Goblinshamanscene.GoblinCondomed.WorkYourSpinnerets".Localized(
                    Player.CockDescript(x)
                ));
            }
            else
            {
                Player.DestroyItems(Identifier.Parse("coc:condom"), 1);
                OutputText("Scenes.Goblinshamanscene.GoblinCondomed.TearOpenPacket".Localized(
                    Player.CockDescript(x)
                ));
            }
            OutputText("Scenes.Goblinshamanscene.GoblinCondomed.TimeToGiveGoblin".Localized());
            OutputText("Scenes.Goblinshamanscene.GoblinCondomed.HoldingWrigglingFeet".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Goblinshamanscene.GoblinCondomed.MouthyBitchForgot".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Goblinshamanscene.GoblinCondomed.SquirmingRightSideUp".Localized());
            if (mode == 0)
            {
                OutputText("Scenes.Goblinshamanscene.GoblinCondomed.InformHerSpider".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinshamanscene.GoblinCondomed.InformHerLatex".Localized());
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
            OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.GoblinLiesStrewn".Localized());
            OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.LuckilyHaveEvery".Localized());
            OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.BeginToFondleCock".Localized());
            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.PluralS".Localized());
            }
            OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.WalkTowardsUnsuspecting".Localized());
            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.PluralSAgain".Localized());
            }
            OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.PeriodParagraphBreak".Localized());
            OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.YelpsMoreSurprised".Localized());
            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.PluralSThird".Localized());
            }
            OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.AndSlapWord".Localized());
            if (Player.CockTotal() == 1)
            {
                OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.ItWord".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.ThemWord".Localized());
            }
            OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.AgainstBareFlesh".Localized(
                Player.CockDescript(x)
            ));
            if (Player.CockTotal() == 1)
            {
                OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.DickInsidePanting".Localized());
            }
            else
            {
                if (Player.CockTotal() == 2)
                {
                    OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.BothDicksInside".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.TwoDicksInside".Localized());
                    if (Player.CockTotal() >= 4)
                    {
                        OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.PluralSFourth".Localized());
                    }
                    OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.RemainingSandwiched".Localized());
                }
            }
            OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.PeriodParagraphBreakAgain".Localized());
            OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.RoughlyPound".Localized());
            OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.EventuallyBeginToFeel".Localized(
                Player.CockDescript(x)
            ));
            if (Player.CockTotal() == 2)
            {
                OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.PluralSFifth".Localized());
            }
            OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.SlowlyReleaseYourself".Localized());
            OutputText("Scenes.Goblinshamanscene.GatsGoblinBoners.PickYourselfBackUp".Localized(
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
            OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.PitifulStruggling".Localized());
            if (Player.HasCock())
            {
                OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.HardenWord".Localized());
            }
            else
            {
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.MoistenWord".Localized());
                }
                else
                {
                    OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.TwitchWord".Localized());
                }
            }
            OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.SlightlyCantHelp".Localized());
            OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.UndressAndSkitterOver".Localized());
            if (Player.Femininity > 50)
            {
                OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.HaventGotAnyBitch".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.HaventGotAnyBastard".Localized());
            }
            OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.LashesOutWithFeet".Localized());
            OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.SquirmsAndYouSlip".Localized());
            OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.PlaythingFallenSilent".Localized());
            OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.NervousToneNothing".Localized());
            OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.StifledGaspsEscape".Localized());
            OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.FinallyBottomOut".Localized());
            OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.FeelItsSlowPath".Localized());
            if (Player.HasCock())
            {
                OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.OwnOrgasmJustAs".Localized());
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.AndYFragment".Localized());
                }
            }
            else
            {
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.YFragment".Localized());
                }
            }
            if (Player.HasVagina())
            {
                OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.OurClenchingInTime".Localized());
            }
            OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.NextEggRollsForward".Localized());
            OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.KeepHerPinned".Localized());
            OutputText("Scenes.Goblinshamanscene.LaySomeDriderEggsInGobboTwat.LayingHerInShade".Localized());
            Player.DumpEggs();
            Player.Orgasm("Ovi");
            CombatScene.CleanupAfterCombat();
        }

    }
}
