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
using MonoCoC.UI.Components;
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
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Swamp
{
    [Register("coc:plague_rat")]
    public class PlagueRatScene : Scene
    {



        private static bool noFur => !Player.HasFur();

        private static bool silly => OptionStore.Instance.Get("silly", 0) > 0;

        private static bool metric => OptionStore.Instance.Get("metric", 0) > 0;

        private static bool realistic => OptionStore.Instance.Get("realistic", 0) > 0;

        private static bool survival => OptionStore.Instance.Get("survival", 0) > 0;

        private static bool hasKnot => Player.Cocks.Count > 0 && Player.Cocks.Any(c => c.KnotMultiplier > 1);

        private static void ReturnToCamp(int hours) => Camp.ReturnToCamp(hours);

        // 原版 addNextButton(...).hint(...).disableIf(...) 的等价形式
        private static Button? Next(string text, Action? onClick, string hint = "", bool enabled = true, string disabledReason = "")
        {
            var button = AddNextButton(text, onClick);

            if (button == null)
                return null;

            button.Hint = hint;
            button.Enabled = enabled && onClick != null;
            button.DisabledReason = enabled ? "" : disabledReason;
            button.RefreshVisual();

            return button;
        }

        //  saveContent
        private static SaveContent Save => SaveContent(new Identifier("coc", "PlagueRatScene"));

        //  get_xxx
        private string debugName => "Plague Rat";
        private string debugHint => "";

        public override void Entry()
        {
            PlagueEncounter();
        }

        // New, Reset, Load, OnAscend, SaveToObject, Get_debugName, Get_debugHint, DebugMenu

        // Haxe Reset()   outputText 0 
        public void Reset()
        {

            Save.SetBool("metRat", false);
            Save.SetInt("ratsKilled", 0);
        }

        // Haxe PlagueEncounter()   outputText 6 
        public void PlagueEncounter()
        {
            ClearOutput();

            if (Save.GetBool("metRat"))
            {
                OutputText("Scenes.Plaguerat.PlagueEncounter.TheFamiliarStenchOf".Localized());
                OutputText("Scenes.Plaguerat.PlagueEncounter.APlagueRatWas".Localized());
            }
            else
            {
                OutputText("Scenes.Plaguerat.PlagueEncounter.YouStopPerkedSuddenly".Localized());
                OutputText("Scenes.Plaguerat.PlagueEncounter.StumblingOffFromYou".Localized());
                OutputText("Scenes.Plaguerat.PlagueEncounter.TheVileCreatureLeaps".Localized());
                Player.TakeDamage(10 + Random.Shared.Next(10));
                OutputText("Scenes.Plaguerat.PlagueEncounter.CacklingTheBeastGnashes".Localized());
                Save.SetBool("metRat", true);
            }
            Player.UnlockCodexEntry(VanillaFlags.CodexEntryRatMice);
            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "plague_rat")), () => Go(SceneRegistry.CreateOrDefault("coc:camp")));
        }

        // Haxe PlagueVictory()   outputText 14 
        public void PlagueVictory()
        {
            ClearOutput();

            OutputText("Scenes.Plaguerat.PlagueVictory.BeforeYouVeEven".Localized());
            OutputText("Scenes.Plaguerat.PlagueVictory.HeCacklesLickingYou".Localized());
            OutputText("Scenes.Plaguerat.PlagueVictory.TheRatScurriesUp".Localized());
            OutputText("Scenes.Plaguerat.PlagueVictory.TheMeatyAppendageSlides".Localized());
            OutputText("Scenes.Plaguerat.PlagueVictory.SpurtsOfHotSemen".Localized());
            OutputText("Scenes.Plaguerat.PlagueVictory.ThePlagueRatSlurps".Localized());
            if (Player.IsGenderless())
            {
                OutputText("Scenes.Plaguerat.PlagueVictory.YourAssSplitsWith".Localized());
                if (Player.Body.Get<AnalPart>().Virgin && silly)
                {
                    OutputText("Scenes.Plaguerat.PlagueVictory.Text".Localized());
                }
                Player.ButtChange(3, Player.Body.Get<AnalPart>().Virgin && silly, false, true);
                OutputText("Scenes.Plaguerat.PlagueVictory.TheMixOfSlipping".Localized());
            }
            else
            {
                if (Player.HasCock())
                {
                    OutputText("Scenes.Plaguerat.PlagueVictory.JaggedTeethBeginTo".Localized());
                }
                else
                {
                    OutputText("Scenes.Plaguerat.PlagueVictory.AHorrificTearingSensation".Localized());
                    var virgin = Player.Vaginas[0].Virgin;
                    Player.CuntChange(3, true, true, true);
                    if (virgin)
                    {
                        OutputText("Scenes.Plaguerat.PlagueVictory.TheFirstPenetrationYour".Localized());
                    }
                    OutputText("Scenes.Plaguerat.PlagueVictory.ItBurnsAndProvokes".Localized());
                }
            }
            OutputText("Scenes.Plaguerat.PlagueVictory.ToYourDismayYou".Localized());
            if (Player.HasCock())
            {
                Player.Orgasm("Dick");
            }
            Player.SlimeFeed();
            Player.ApplyStats(new DynStat(VanillaStats.Corruption, 1f));
            CombatScene.CleanupAfterCombat();
        }

        // Haxe PlagueDefeat()   outputText 2 
        public void PlagueDefeat(bool hpVictory)
        {
            ClearOutput();

            if (hpVictory)
            {
                OutputText("Scenes.Plaguerat.PlagueDefeat.ThePlagueRatStumbles".Localized());
            }
            else
            {
                OutputText("Scenes.Plaguerat.PlagueDefeat.CompletelyOvercomeByHis".Localized());
            }
            ClearButtons();
            // 原版 .sexButton(BaseContent.MALE)：需要屌 + 足够欲望
            SexButton(Next("Penetrate", () => PlaguePenetrate(hpVictory), "Give him a good reaming."), Gender.Male);
            Next("Mount", PlagueMount, "Go for a ride on a rodent.",
                Player.Lust >= 33, "You aren't aroused enough to have sex.");
            Next("Kill", PlagueKill);
            if (Player.IsInLake)
            {
                Next("Wash", PlagueWash, "Forceful cleansing is in order.");
            }
            if (silly && Player.HasItem(VanillaUseables.TeddyBear))
            {
                Next("Gift Bear", PlagueBear, "Gift him a bear, show him compassion.");
            }
            if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.Liddellium)))
            {
                Next("Liddellium", ForceLiddellium, "Maybe some alchemy could turn this disgusting creature cute?");
            }
            if (Player.HasMultiTails())
            {
                // 原版 .sexButton(BaseContent.ANYGENDER)：只要不是无性别
                SexButton(Next("Force Fluff", () => SceneInstance<KitsuneScene>.Get().KitsuneGenericFluff(),
                    "Have [themonster] fluff your tails."), AnyGender);
            }
            SetExitButton("Leave", CombatScene.CleanupAfterCombat);
        }

        // Haxe PlaguePenetrate()   outputText 10 
        public void PlaguePenetrate(bool hpVictory = true)
        {
            ClearOutput();

            OutputText("Scenes.Plaguerat.PlaguePenetrate.StaringAtTheFallen".Localized());
            OutputText("Scenes.Plaguerat.PlaguePenetrate.TheRatStillWrithes".Localized(hpVictory ? "injured" : "aroused"));
            OutputText("Scenes.Plaguerat.PlaguePenetrate.Text".Localized());
            OutputText("Scenes.Plaguerat.PlaguePenetrate.WhoIsHeCalling".Localized());
            OutputText("Scenes.Plaguerat.PlaguePenetrate.YouPressYourCrown".Localized());
            OutputText("Scenes.Plaguerat.PlaguePenetrate.IgnoringTheExactReasons".Localized());
            OutputText("Scenes.Plaguerat.PlaguePenetrate.YouCanTQuite".Localized());
            OutputText("Scenes.Plaguerat.PlaguePenetrate.YouCanFeelYourself".Localized());
            OutputText("Scenes.Plaguerat.PlaguePenetrate.WhenYouFinallySlump".Localized());
            OutputText("Scenes.Plaguerat.PlaguePenetrate.YouSwiftlySetOut".Localized());
            Player.Orgasm("Dick");
            Player.ApplyStats(new DynStat(VanillaStats.Corruption, 1f));
            CombatScene.CleanupAfterCombat();
        }

        // Haxe PlagueMount()   outputText 9 
        public void PlagueMount()
        {
            ClearOutput();

            OutputText("Scenes.Plaguerat.PlagueMount.FilthAsideYourNeeds".Localized());
            OutputText("Scenes.Plaguerat.PlagueMount.TheRodentSCock".Localized());
            OutputText("Scenes.Plaguerat.PlagueMount.MuttersTheDemonHis".Localized());
            OutputText("Scenes.Plaguerat.PlagueMount.YouLeanOverAnd".Localized());
            bool virgin;
            if (Player.HasVagina())
            {
                virgin = Player.Vaginas[0].Virgin;
                Player.CuntChange(8, true, true, true);
            }
            else
            {
                virgin = Player.Body.Get<AnalPart>().Virgin;
                Player.ButtChange(8, true, true, true);
            }
            if (virgin)
            {
                OutputText("Scenes.Plaguerat.PlagueMount.ItStingsBothFrom".Localized());
            }
            OutputText("Scenes.Plaguerat.PlagueMount.SinkingDownInvoluntarilyTwitches".Localized());
            OutputText("Scenes.Plaguerat.PlagueMount.TheRatGroansAnd".Localized());
            OutputText("Scenes.Plaguerat.PlagueMount.YourInnardsWritheA".Localized());
            OutputText("Scenes.Plaguerat.PlagueMount.ItTakesAGreat".Localized());
            Player.Orgasm("VaginalAnal");
            Player.SlimeFeed();
            Player.ApplyStats(new DynStat(VanillaStats.Corruption, 1f));
            if (Player.HasVagina())
            {
                Player.KnockUp(VanillaPregnancy.Imp, VanillaPregnancy.ImpIncubation);
            }
            CombatScene.CleanupAfterCombat();
        }

        // Haxe PlagueKill()   outputText 10 
        public void PlagueKill()
        {
            ClearOutput();

            if (Player.Weapon.IsHolySword())
            {
                OutputText("Scenes.Plaguerat.PlagueKill.WithNoDelayYou".Localized());
            }
            else
            {
                if (Player.Weapon.IsScythe())
                {
                    OutputText("Scenes.Plaguerat.PlagueKill.NotEvenPestilenceItself".Localized());
                }
                else
                {
                    if (Player.Weapon.IsSpear())
                    {
                        OutputText("Scenes.Plaguerat.PlagueKill.YouFinishTheFiend".Localized());
                    }
                    else
                    {
                        if (Player.Weapon.IsAxe())
                        {
                            OutputText("Scenes.Plaguerat.PlagueKill.ThisRatIsNext".Localized());
                        }
                        else
                        {
                            if (Player.Weapon.IsBladed())
                            {
                                OutputText("Scenes.Plaguerat.PlagueKill.YouTakeABreath".Localized());
                            }
                            else
                            {
                                if (Player.Weapon.IsWhip())
                                {
                                    OutputText("Scenes.Plaguerat.PlagueKill.YouSlingYourAround".Localized());
                                }
                                else
                                {
                                    if (Player.Weapon.IsStaff() && Player.Weapon.IsChanneling(Player))
                                    {
                                        OutputText("Scenes.Plaguerat.PlagueKill.ReadyToEndThis".Localized());
                                    }
                                    else
                                    {
                                        if (Player.Weapon.IsFirearm())
                                        {
                                            OutputText("Scenes.Plaguerat.PlagueKill.YouSmackTheRat".Localized());
                                        }
                                        else
                                        {
                                            if (Player.Weapon.IsBlunt())
                                            {
                                                OutputText("Scenes.Plaguerat.PlagueKill.SwingingYourAtThe".Localized());
                                            }
                                            else
                                            {
                                                OutputText("Scenes.Plaguerat.PlagueKill.ThroughWithHimYou".Localized());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Save.SetInt("ratsKilled", Save.GetInt("ratsKilled", 0) + 1);
            Player.UpgradeDeusVult();
            CombatScene.CleanupAfterCombat();
        }

        // Haxe PlagueWash()   outputText 5 
        public void PlagueWash()
        {
            ClearOutput();

            OutputText("Scenes.Plaguerat.PlagueWash.SomethingSoRiddledWith".Localized());
            OutputText("Scenes.Plaguerat.PlagueWash.HeScreechesItS".Localized());
            OutputText("Scenes.Plaguerat.PlagueWash.TakingAFirmerStance".Localized());
            OutputText("Scenes.Plaguerat.PlagueWash.GaspingHeScreamsBefore".Localized());
            OutputText("Scenes.Plaguerat.PlagueWash.WhenAtLastYou".Localized());
            ClearButtons();
            Next("Kill", PlagueWashKill, "How dare he ruin his own bath.");
            Next("Throw", PlagueWashThrow, "Send him out to sea.");
        }

        // Haxe PlagueWashKill()   outputText 2 
        public void PlagueWashKill()
        {
            ClearOutput();

            OutputText("Scenes.Plaguerat.PlagueWashKill.SuchInsolenceFromHim".Localized());
            OutputText("Scenes.Plaguerat.PlagueWashKill.YouLeaveTheRat".Localized());
            Save.SetInt("ratsKilled", Save.GetInt("ratsKilled", 0) + 1);
            CombatScene.CleanupAfterCombat();
        }

        // Haxe PlagueWashThrow()   outputText 2 
        public void PlagueWashThrow()
        {
            ClearOutput();

            OutputText("Scenes.Plaguerat.PlagueWashThrow.ToVomitInThe".Localized());
            OutputText("Scenes.Plaguerat.PlagueWashThrow.HeYellsInA".Localized());
            CombatScene.CleanupAfterCombat();
        }

        // Haxe PlagueBear()   outputText 4 
        public void PlagueBear()
        {
            ClearOutput();

            OutputText("Scenes.Plaguerat.PlagueBear.ThePitifulRatIs".Localized());
            OutputText("Scenes.Plaguerat.PlagueBear.SearchingYourThingsYou".Localized());
            OutputText("Scenes.Plaguerat.PlagueBear.HeSaysFurtherBewildered".Localized());
            OutputText("Scenes.Plaguerat.PlagueBear.RelievedThatYouWon".Localized());
            Player.DestroyItems(VanillaUseables.TeddyBear);
            CombatScene.CleanupAfterCombat();
        }

        // Haxe ForceLiddellium()   outputText 6 
        public void ForceLiddellium()
        {
            ClearOutput();

            OutputText("Scenes.Plaguerat.ForceLiddellium.YouMightNotKnow".Localized());
            OutputText("Scenes.Plaguerat.ForceLiddellium.TakingAWhiffOf".Localized());
            OutputText("Scenes.Plaguerat.ForceLiddellium.TheLiquidFinishesDraining".Localized());
            OutputText("Scenes.Plaguerat.ForceLiddellium.HisWorriedMuttersAre".Localized());
            OutputText("Scenes.Plaguerat.ForceLiddellium.ThePlagueRatNow".Localized());
            OutputText("Scenes.Plaguerat.ForceLiddellium.YouReNotEntirely".Localized());
            Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.Liddellium));
            ClearButtons();
            SexButton(Next("Fuck", LiddFuck, "Have a taste, now that she's ready."), Gender.Male);
            Next("Kill", LiddKill, "Be properly rid of this creature.");
            SetExitButton("Leave", LiddLeave);
        }

        // Haxe LiddFuck()   outputText 13 
        public void LiddFuck()
        {
            ClearOutput();

            OutputText("Scenes.Plaguerat.LiddFuck.SheSStillA".Localized());
            OutputText((Player.IsInBog || Player.IsInSwamp
                ? "Scenes.Plaguerat.LiddFuck.ButAlthoughHerAppearanceMud"
                : "Scenes.Plaguerat.LiddFuck.ButAlthoughHerAppearanceGround").Localized());
            OutputText("Scenes.Plaguerat.LiddFuck.SheRepeatsOverAnd".Localized());
            OutputText("Scenes.Plaguerat.LiddFuck.ThereAreAFew".Localized());
            OutputText("Scenes.Plaguerat.LiddFuck.HerNewlyFormedSnatch".Localized());
            OutputText("Scenes.Plaguerat.LiddFuck.YourIsBroughtTo".Localized());
            OutputText("Scenes.Plaguerat.LiddFuck.SheRealizesWhatYou".Localized());
            OutputText("Scenes.Plaguerat.LiddFuck.WhenYouPressIn".Localized());
            OutputText("Scenes.Plaguerat.LiddFuck.YourOtherHandMoves".Localized());
            OutputText("Scenes.Plaguerat.LiddFuck.YouDeliverAResounding".Localized());
            OutputText("Scenes.Plaguerat.LiddFuck.ASingleTwitchAnd".Localized());
            OutputText("Scenes.Plaguerat.LiddFuck.AndJustToDrive".Localized());
            OutputText("Scenes.Plaguerat.LiddFuck.YouPromptlyStandAnd".Localized());
            Player.Orgasm("Dick");
            Player.ApplyStats(new DynStat(VanillaStats.Corruption, 2f));
            ReturnToCamp(1);
        }

        // Haxe LiddKill()   outputText 8 
        public void LiddKill()
        {
            ClearOutput();

            OutputText("Scenes.Plaguerat.LiddKill.ThatSEnoughYou".Localized());
            OutputText("Scenes.Plaguerat.LiddKill.TheRodentMustSense".Localized());
            OutputText("Scenes.Plaguerat.LiddKill.ThoughTheLiddelliumMay".Localized());
            OutputText("Scenes.Plaguerat.LiddKill.WithYourEnvelopingIt".Localized());
            OutputText("Scenes.Plaguerat.LiddKill.ItCriesYouWon".Localized());
            OutputText("Scenes.Plaguerat.LiddKill.YouRaiseTheRock".Localized());
            OutputText("Scenes.Plaguerat.LiddKill.ButDespiteTheNew".Localized());
            OutputText("Scenes.Plaguerat.LiddKill.TheDeedDoneYou".Localized());
            Save.SetInt("ratsKilled", Save.GetInt("ratsKilled", 0) + 1);
            if (!Player.IsReligious())
            {
                Player.ApplyStats(new DynStat(VanillaStats.Corruption, 1f));
            }
            ReturnToCamp(1);
        }

        // Haxe LiddLeave()   outputText 3 
        public void LiddLeave()
        {
            ClearOutput();

            OutputText("Scenes.Plaguerat.LiddLeave.YouDonTNeed".Localized());
            OutputText("Scenes.Plaguerat.LiddLeave.ForHerPartThe".Localized());
            OutputText("Scenes.Plaguerat.LiddLeave.SuchASqueakyVoice".Localized());
            Player.ApplyStats(new DynStat(VanillaStats.Corruption, .5f));
            ReturnToCamp(1);
        }

    }
}
