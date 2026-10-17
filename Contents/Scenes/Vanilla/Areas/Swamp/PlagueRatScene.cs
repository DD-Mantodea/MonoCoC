using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.UI.Components;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Swamp
{
    [Register<PlagueRatScene>("coc:plague_rat")]
    public class PlagueRatScene : Scene
    {

        public static Button? Next(string text, Action? onClick, string hint = "", bool enabled = true, string disabledReason = "")
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

        public static SaveContent Save => SaveContent(SceneIds.Swamp.PlagueRat);

        public string debugName => "Plague Rat";

        public string debugHint => "";

        public override void Entry()
        {
            PlagueEncounter();
        }

        public void Reset()
        {

            Save.SetBool("metRat", false);

            Save.SetInt("ratsKilled", 0);
        }

        public void PlagueEncounter()
        {
            ClearOutput();

            if (Save.GetBool("metRat"))
            {
                OutputText("Scenes.PlagueRat.PlagueEncounter.TheFamiliarStenchOf".Localized());

                OutputText("Scenes.PlagueRat.PlagueEncounter.APlagueRatWas".Localized());
            }
            else
            {
                OutputText("Scenes.PlagueRat.PlagueEncounter.YouStopPerkedSuddenly".Localized());

                OutputText("Scenes.PlagueRat.PlagueEncounter.StumblingOffFromYou".Localized());

                OutputText("Scenes.PlagueRat.PlagueEncounter.TheVileCreatureLeaps".Localized());

                Player.TakeDamage(10 + Random.Shared.Next(10));

                OutputText("Scenes.PlagueRat.PlagueEncounter.CacklingTheBeastGnashes".Localized());

                Save.SetBool("metRat", true);
            }

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryRatMice);

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "plague_rat")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));
        }

        public void PlagueVictory()
        {
            ClearOutput();

            OutputText("Scenes.PlagueRat.PlagueVictory.BeforeYouVeEven".Localized());

            OutputText("Scenes.PlagueRat.PlagueVictory.HeCacklesLickingYou".Localized());

            OutputText("Scenes.PlagueRat.PlagueVictory.TheRatScurriesUp".Localized());

            OutputText("Scenes.PlagueRat.PlagueVictory.TheMeatyAppendageSlides".Localized());

            OutputText("Scenes.PlagueRat.PlagueVictory.SpurtsOfHotSemen".Localized());

            OutputText("Scenes.PlagueRat.PlagueVictory.ThePlagueRatSlurps".Localized());

            if (Player.IsGenderless())
            {
                OutputText("Scenes.PlagueRat.PlagueVictory.YourAssSplitsWith".Localized());

                if (Player.Body.Get<AnalPart>().Virgin && silly)
                    OutputText("Scenes.PlagueRat.PlagueVictory.Text".Localized());

                Player.ButtChange(3, Player.Body.Get<AnalPart>().Virgin && silly, false, true);

                OutputText("Scenes.PlagueRat.PlagueVictory.TheMixOfSlipping".Localized());
            }
            else
                if (Player.HasCock())
                    OutputText("Scenes.PlagueRat.PlagueVictory.JaggedTeethBeginTo".Localized());
                else
                {
                    OutputText("Scenes.PlagueRat.PlagueVictory.AHorrificTearingSensation".Localized());

                    var virgin = Player.Vaginas[0].Virgin;

                    Player.CuntChange(3, true, true, true);

                    if (virgin)
                        OutputText("Scenes.PlagueRat.PlagueVictory.TheFirstPenetrationYour".Localized());

                    OutputText("Scenes.PlagueRat.PlagueVictory.ItBurnsAndProvokes".Localized());
                }

            OutputText("Scenes.PlagueRat.PlagueVictory.ToYourDismayYou".Localized());

            if (Player.HasCock())
                Player.Orgasm("Dick");

            Player.SlimeFeed();

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void PlagueDefeat(bool hpVictory)
        {
            ClearOutput();

            if (hpVictory)
                OutputText("Scenes.PlagueRat.PlagueDefeat.ThePlagueRatStumbles".Localized());
            else
                OutputText("Scenes.PlagueRat.PlagueDefeat.CompletelyOvercomeByHis".Localized());

            ClearButtons();

            SexButton(Next("Penetrate", () => PlaguePenetrate(hpVictory), "Give him a good reaming."), Gender.Male);

            Next("Mount", PlagueMount, "Go for a ride on a rodent.",

                Player.Lust >= 33, "You aren't aroused enough to have sex.");

            Next("Kill", PlagueKill);

            if (Player.IsInLake)
                Next("Wash", PlagueWash, "Forceful cleansing is in order.");

            if (silly && Player.HasItem(UseableIds.TeddyBear))
                Next("Gift Bear", PlagueBear, "Gift him a bear, show him compassion.");

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.Liddellium)))
                Next("Liddellium", ForceLiddellium, "Maybe some alchemy could turn this disgusting creature cute?");

            if (Player.HasMultiTails())

                SexButton(Next("Force Fluff", () => SceneInstance<KitsuneScene>.Get().KitsuneGenericFluff(),

                    "Have [themonster] fluff your tails."), AnyGender);

            SetExitButton("Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void PlaguePenetrate(bool hpVictory = true)
        {
            ClearOutput();

            OutputText("Scenes.PlagueRat.PlaguePenetrate.StaringAtTheFallen".Localized());

            OutputText("Scenes.PlagueRat.PlaguePenetrate.TheRatStillWrithes".Localized(hpVictory ? "injured" : "aroused"));

            OutputText("Scenes.PlagueRat.PlaguePenetrate.Text".Localized());

            OutputText("Scenes.PlagueRat.PlaguePenetrate.WhoIsHeCalling".Localized());

            OutputText("Scenes.PlagueRat.PlaguePenetrate.YouPressYourCrown".Localized());

            OutputText("Scenes.PlagueRat.PlaguePenetrate.IgnoringTheExactReasons".Localized());

            OutputText("Scenes.PlagueRat.PlaguePenetrate.YouCanTQuite".Localized());

            OutputText("Scenes.PlagueRat.PlaguePenetrate.YouCanFeelYourself".Localized());

            OutputText("Scenes.PlagueRat.PlaguePenetrate.WhenYouFinallySlump".Localized());

            OutputText("Scenes.PlagueRat.PlaguePenetrate.YouSwiftlySetOut".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void PlagueMount()
        {
            ClearOutput();

            OutputText("Scenes.PlagueRat.PlagueMount.FilthAsideYourNeeds".Localized());

            OutputText("Scenes.PlagueRat.PlagueMount.TheRodentSCock".Localized());

            OutputText("Scenes.PlagueRat.PlagueMount.MuttersTheDemonHis".Localized());

            OutputText("Scenes.PlagueRat.PlagueMount.YouLeanOverAnd".Localized());

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
                OutputText("Scenes.PlagueRat.PlagueMount.ItStingsBothFrom".Localized());

            OutputText("Scenes.PlagueRat.PlagueMount.SinkingDownInvoluntarilyTwitches".Localized());

            OutputText("Scenes.PlagueRat.PlagueMount.TheRatGroansAnd".Localized());

            OutputText("Scenes.PlagueRat.PlagueMount.YourInnardsWritheA".Localized());

            OutputText("Scenes.PlagueRat.PlagueMount.ItTakesAGreat".Localized());

            Player.Orgasm("VaginalAnal");

            Player.SlimeFeed();

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            if (Player.HasVagina())
                Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation);

            CombatScene.CleanupAfterCombat();
        }

        public void PlagueKill()
        {
            ClearOutput();

            if (Player.Weapon.IsHolySword())
                OutputText("Scenes.PlagueRat.PlagueKill.WithNoDelayYou".Localized());
            else
                if (Player.Weapon.IsScythe())
                    OutputText("Scenes.PlagueRat.PlagueKill.NotEvenPestilenceItself".Localized());
                else
                    if (Player.Weapon.IsSpear())
                        OutputText("Scenes.PlagueRat.PlagueKill.YouFinishTheFiend".Localized());
                    else
                        if (Player.Weapon.IsAxe())
                            OutputText("Scenes.PlagueRat.PlagueKill.ThisRatIsNext".Localized());
                        else
                            if (Player.Weapon.IsBladed())
                                OutputText("Scenes.PlagueRat.PlagueKill.YouTakeABreath".Localized());
                            else
                                if (Player.Weapon.IsWhip())
                                    OutputText("Scenes.PlagueRat.PlagueKill.YouSlingYourAround".Localized());
                                else
                                    if (Player.Weapon.IsStaff() && Player.Weapon.IsChanneling(Player))
                                        OutputText("Scenes.PlagueRat.PlagueKill.ReadyToEndThis".Localized());
                                    else
                                        if (Player.Weapon.IsFirearm())
                                            OutputText("Scenes.PlagueRat.PlagueKill.YouSmackTheRat".Localized());
                                        else
                                            if (Player.Weapon.IsBlunt())
                                                OutputText("Scenes.PlagueRat.PlagueKill.SwingingYourAtThe".Localized());
                                            else
                                                OutputText("Scenes.PlagueRat.PlagueKill.ThroughWithHimYou".Localized());

            Save.SetInt("ratsKilled", Save.GetInt("ratsKilled", 0) + 1);

            Player.UpgradeDeusVult();

            CombatScene.CleanupAfterCombat();
        }

        public void PlagueWash()
        {
            ClearOutput();

            OutputText("Scenes.PlagueRat.PlagueWash.SomethingSoRiddledWith".Localized());

            OutputText("Scenes.PlagueRat.PlagueWash.HeScreechesItS".Localized());

            OutputText("Scenes.PlagueRat.PlagueWash.TakingAFirmerStance".Localized());

            OutputText("Scenes.PlagueRat.PlagueWash.GaspingHeScreamsBefore".Localized());

            OutputText("Scenes.PlagueRat.PlagueWash.WhenAtLastYou".Localized());

            ClearButtons();

            Next("Kill", PlagueWashKill, "How dare he ruin his own bath.");

            Next("Throw", PlagueWashThrow, "Send him out to sea.");
        }

        public void PlagueWashKill()
        {
            ClearOutput();

            OutputText("Scenes.PlagueRat.PlagueWashKill.SuchInsolenceFromHim".Localized());

            OutputText("Scenes.PlagueRat.PlagueWashKill.YouLeaveTheRat".Localized());

            Save.SetInt("ratsKilled", Save.GetInt("ratsKilled", 0) + 1);

            CombatScene.CleanupAfterCombat();
        }

        public void PlagueWashThrow()
        {
            ClearOutput();

            OutputText("Scenes.PlagueRat.PlagueWashThrow.ToVomitInThe".Localized());

            OutputText("Scenes.PlagueRat.PlagueWashThrow.HeYellsInA".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void PlagueBear()
        {
            ClearOutput();

            OutputText("Scenes.PlagueRat.PlagueBear.ThePitifulRatIs".Localized());

            OutputText("Scenes.PlagueRat.PlagueBear.SearchingYourThingsYou".Localized());

            OutputText("Scenes.PlagueRat.PlagueBear.HeSaysFurtherBewildered".Localized());

            OutputText("Scenes.PlagueRat.PlagueBear.RelievedThatYouWon".Localized());

            Player.DestroyItems(UseableIds.TeddyBear);

            CombatScene.CleanupAfterCombat();
        }

        public void ForceLiddellium()
        {
            ClearOutput();

            OutputText("Scenes.PlagueRat.ForceLiddellium.YouMightNotKnow".Localized());

            OutputText("Scenes.PlagueRat.ForceLiddellium.TakingAWhiffOf".Localized());

            OutputText("Scenes.PlagueRat.ForceLiddellium.TheLiquidFinishesDraining".Localized());

            OutputText("Scenes.PlagueRat.ForceLiddellium.HisWorriedMuttersAre".Localized());

            OutputText("Scenes.PlagueRat.ForceLiddellium.ThePlagueRatNow".Localized());

            OutputText("Scenes.PlagueRat.ForceLiddellium.YouReNotEntirely".Localized());

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.Liddellium));

            ClearButtons();

            SexButton(Next("Fuck", LiddFuck, "Have a taste, now that she's ready."), Gender.Male);

            Next("Kill", LiddKill, "Be properly rid of this creature.");

            SetExitButton("Common.Button.Leave".Localized(), LiddLeave);
        }

        public void LiddFuck()
        {
            ClearOutput();

            OutputText("Scenes.PlagueRat.LiddFuck.SheSStillA".Localized());

            OutputText((Player.IsInBog || Player.IsInSwamp

                ? "Scenes.PlagueRat.LiddFuck.ButAlthoughHerAppearanceMud"

                : "Scenes.PlagueRat.LiddFuck.ButAlthoughHerAppearanceGround").Localized());

            OutputText("Scenes.PlagueRat.LiddFuck.SheRepeatsOverAnd".Localized());

            OutputText("Scenes.PlagueRat.LiddFuck.ThereAreAFew".Localized());

            OutputText("Scenes.PlagueRat.LiddFuck.HerNewlyFormedSnatch".Localized());

            OutputText("Scenes.PlagueRat.LiddFuck.YourIsBroughtTo".Localized());

            OutputText("Scenes.PlagueRat.LiddFuck.SheRealizesWhatYou".Localized());

            OutputText("Scenes.PlagueRat.LiddFuck.WhenYouPressIn".Localized());

            OutputText("Scenes.PlagueRat.LiddFuck.YourOtherHandMoves".Localized());

            OutputText("Scenes.PlagueRat.LiddFuck.YouDeliverAResounding".Localized());

            OutputText("Scenes.PlagueRat.LiddFuck.ASingleTwitchAnd".Localized());

            OutputText("Scenes.PlagueRat.LiddFuck.AndJustToDrive".Localized());

            OutputText("Scenes.PlagueRat.LiddFuck.YouPromptlyStandAnd".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void LiddKill()
        {
            ClearOutput();

            OutputText("Scenes.PlagueRat.LiddKill.ThatSEnoughYou".Localized());

            OutputText("Scenes.PlagueRat.LiddKill.TheRodentMustSense".Localized());

            OutputText("Scenes.PlagueRat.LiddKill.ThoughTheLiddelliumMay".Localized());

            OutputText("Scenes.PlagueRat.LiddKill.WithYourEnvelopingIt".Localized());

            OutputText("Scenes.PlagueRat.LiddKill.ItCriesYouWon".Localized());

            OutputText("Scenes.PlagueRat.LiddKill.YouRaiseTheRock".Localized());

            OutputText("Scenes.PlagueRat.LiddKill.ButDespiteTheNew".Localized());

            OutputText("Scenes.PlagueRat.LiddKill.TheDeedDoneYou".Localized());

            Save.SetInt("ratsKilled", Save.GetInt("ratsKilled", 0) + 1);

            if (!Player.IsReligious())
                Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void LiddLeave()
        {
            ClearOutput();

            OutputText("Scenes.PlagueRat.LiddLeave.YouDonTNeed".Localized());

            OutputText("Scenes.PlagueRat.LiddLeave.ForHerPartThe".Localized());

            OutputText("Scenes.PlagueRat.LiddLeave.SuchASqueakyVoice".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, .5f));

            DoNext(() => ReturnToCamp(1));
        }
    }
}