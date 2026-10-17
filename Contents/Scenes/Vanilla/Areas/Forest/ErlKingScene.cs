using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Achievements;
using System.Collections.Generic;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{
    [Register<ErlKingScene>("coc:erl_king")]
    public class ErlKingScene : Scene
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

            if (CoC.Instance.Flags.Get(GameFlagIds.WildHuntEncounters) == 0)
                FirstWildHuntEncounter();
            else
                if (!Player.HasKeyItem("Golden Antlers"))
                    RepeatWildHuntEncounter();
                else
                    if (Player.HasKeyItem("Golden Antlers"))
                        EncounterPrincessGwynn();

            CoC.Instance.Flags.Set(GameFlagIds.WildHuntEncounters, CoC.Instance.Flags.Get(GameFlagIds.WildHuntEncounters) + 1);
        }

        public int PlayerHuntScore()
        {

            var baseVal= (int)((Player.Intelligence + Player.Speed) - (Player.Fatigue - Player.MaxFatigue() + 100) * 2);

            if (Player.HasPerk(PerkIds.Evade))
                baseVal += 20;

            if (Player.HasPerk(PerkIds.Runner))
                baseVal += 20;

            if (Player.HasPerk(PerkIds.Unhindered) && (Player.Armor == Armor.Nothing || Player.Armor.Perk == "Adornment"))
                baseVal += 20;

            if (Player.IsDrider())
                baseVal += 20;

            if (Player.HasPerk(PerkIds.CorruptedNinetails))
                baseVal += 30;

            if (Player.HasPerk(PerkIds.EnlightenedNinetails))
                baseVal += 30;

            if (Player.HasPerk(PerkIds.FireLord))
                baseVal += 10;

            if (Player.HasPerk(PerkIds.Whispered))
                baseVal += 10;

            if (Player.HasPerk(PerkIds.Fast))
                baseVal += 10;

            if (Player.HasPerk(PerkIds.Incorporeality))
                baseVal += 10;

            if (Player.CanFly())
                baseVal += 10;

            if (Player.KitsuneScore() > 0)
                baseVal -= (int)(Player.KitsuneScore() * 20);

            if (Player.BunnyScore() > 0)
                baseVal -= (int)(Player.BunnyScore() * 20);

            if (Player.HarpyScore() > 0)
                baseVal -= (int)(Player.HarpyScore() * 20);

            if (Player.GooScore() > 0)
                baseVal -= (int)(Player.GooScore() * 10);

            if (Player.IsTaur())
                baseVal -= 10;

            if (baseVal < 0)
                baseVal = 0;

            return baseVal;
        }

        public void FirstWildHuntEncounter()
        {
            ClearOutput();

            ShowImage("wildhunt-encounter");

            OutputText("Scenes.ErlKing.FirstWildHunt.Fog".Localized());

            OutputText("Scenes.ErlKing.FirstWildHunt.HornAndHowls".Localized());

            if (View.Time.Hours >= 0 && View.Time.Hours <= 10)
                OutputText("Scenes.ErlKing.FirstWildHunt.TimeMorning".Localized());
            else
                if (View.Time.Hours >= 11 && View.Time.Hours <= 13)
                    OutputText("Scenes.ErlKing.FirstWildHunt.TimeDaytime".Localized());
                else
                    if (View.Time.Hours >= 14 && View.Time.Hours <= 17)
                        OutputText("Scenes.ErlKing.FirstWildHunt.TimeAfternoon".Localized());
                    else
                        if (View.Time.Hours >= 18 && View.Time.Hours <= 24)
                            OutputText("Scenes.ErlKing.FirstWildHunt.TimeNight".Localized());

            OutputText("Scenes.ErlKing.FirstWildHunt.Hooves".Localized());

            OutputText("Scenes.ErlKing.FirstWildHunt.HuntApproaches".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Wait".Localized(), () => FirstWildHuntChase(true));

            AddButton(1, "Common.Button.Run".Localized(), () => FirstWildHuntChase(false));
        }

        public void FirstWildHuntChase(bool waited = false)
        {
            ClearOutput();

            if (waited == false)
                OutputText("Scenes.ErlKing.FirstWildHuntChase.FogSurrounds".Localized());
            else
            {
                OutputText("Scenes.ErlKing.FirstWildHuntChase.HoundAppears".Localized());

                OutputText("Scenes.ErlKing.FirstWildHuntChase.SecondHoundEnd".Localized());

                OutputText("Scenes.ErlKing.FirstWildHuntChase.HoundHowls".Localized());

                OutputText("Scenes.ErlKing.FirstWildHuntChase.SecondHoundEnd".Localized());

                OutputText("Scenes.ErlKing.FirstWildHuntChase.HoundsEager".Localized());

                if (Player.Corruption >= 40)
                    if (Player.HasCock() && !Player.HasVagina())
                        OutputText("Scenes.ErlKing.FirstWildHuntChase.YouStiffen".Localized());
                    else
                        if (Player.HasVagina() && !Player.HasCock())
                            OutputText("Scenes.ErlKing.FirstWildHuntChase.PussyWets".Localized());
                        else
                            if (Player.HasVagina() && Player.HasCock())
                            {
                                OutputText("Scenes.ErlKing.FirstWildHuntChase.CockTwitch".Localized());

                                if (Player.Cocks.Count > 1)
                                    OutputText("Scenes.ErlKing.FirstWildHuntChase.CockPluralS".Localized());

                                OutputText("Scenes.ErlKing.FirstWildHuntChase.PussyShiver".Localized());
                            }
                            else
                                OutputText("Scenes.ErlKing.FirstWildHuntChase.AssholeTwitch".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => FirstWildHuntChaseII(waited));
        }

        public void FirstWildHuntChaseII(bool waited = false)
        {
            ClearOutput();

            OutputText("Scenes.ErlKing.FirstWildHuntChaseII.ErlkingArrives".Localized());

            OutputText("Scenes.ErlKing.FirstWildHuntChaseII.ErlkingTall".Localized());

            ShowImage("wildhunt-huntsman");

            OutputText("Scenes.ErlKing.FirstWildHuntChaseII.ErlkingClothed".Localized());

            OutputText("Scenes.ErlKing.FirstWildHuntChaseII.IntroducesHimself".Localized());

            OutputText("Scenes.ErlKing.FirstWildHuntChaseII.GazesAtYou".Localized());

            if (waited)
                OutputText("Scenes.ErlKing.FirstWildHuntChaseII.WarnsBrave".Localized());
            else
                OutputText("Scenes.ErlKing.FirstWildHuntChaseII.WarnsRunFaster".Localized());

            OutputText("Scenes.ErlKing.FirstWildHuntChaseII.FogCloses".Localized());

            OutputText("Scenes.ErlKing.FirstWildHuntChaseII.Package".Localized());

            Player.ChangeFatigue(10);

            if (waited)
                DoNext(() => TakeCaninePepper());
            else
                DoNext(() => TakeFoxBerry());
        }

        public void RepeatWildHuntEncounter()
        {
            ClearOutput();

            ShowImage("wildhunt-encounter");

            OutputText("Scenes.ErlKing.RepeatWildHuntEncounter.FogReturns".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntEncounter.ErlkingComing".Localized());

            if (Player.Body.Get<WingsPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.ErlKing.RepeatWildHuntEncounter.TreesTooClose".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntEncounter.ChoicePrompt".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Run".Localized(), RepeatWildHuntChase);

            AddButton(1, "Common.Button.Wait".Localized(), RepeatWildHuntWait);

            if (PlayerHuntScore() >= 200)
            {
                OutputText("Scenes.ErlKing.RepeatWildHuntEncounter.CanHoldBack".Localized());

                AddButton(5, "Scenes.ErlKing.RepeatWildHuntEncounter.ButtonRunGetCaught".Localized(), () => RepeatWildHuntCaught(110));

                AddButton(10, "Scenes.ErlKing.RepeatWildHuntEncounter.ButtonRunLose".Localized(), () => RepeatWildHuntCaught(90));
            }
        }

        public void RepeatWildHuntWait()
        {
            ClearOutput();

            ShowImage("wildhunt-encounter");

            OutputText("Scenes.ErlKing.RepeatWildHuntWait.FogArrives".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntWait.StandFirm".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntWait.HuntLouder".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntWait.HuntFades".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntWait.ErlkingLosesInterest".Localized());

            if (Player.Intelligence < 80)
                Player.Intelligence += 1;

            ClearButtons();

            DoNext(() => ReturnToCamp(1));
        }

        public void RepeatWildHuntChase()
        {

            var pScore= PlayerHuntScore();

            if (pScore > 150)
                RepeatWildHuntEscaped();
            else
                RepeatWildHuntCaught(pScore);
        }

        public void RepeatWildHuntEscaped()
        {
            ClearOutput();

            ShowImage("wildhunt-escaped");

            OutputText("Scenes.ErlKing.RepeatWildHuntEscaped.RunLikeTheWind".Localized());

            if (Player.IsGoo())
                OutputText("Scenes.ErlKing.RepeatWildHuntEscaped.SlimeBody".Localized());
            else
                if (Player.IsBiped())
                    OutputText("Scenes.ErlKing.RepeatWildHuntEscaped.LegsPound".Localized());
                else
                    if (Player.HasTailInsteadOfLegs())
                        OutputText("Scenes.ErlKing.RepeatWildHuntEscaped.Coils".Localized());
                    else
                        if (Player.IsDrider())
                            OutputText("Scenes.ErlKing.RepeatWildHuntEscaped.ManyLegs".Localized());
                        else
                            if (Player.IsTaur())
                                OutputText("Scenes.ErlKing.RepeatWildHuntEscaped.Hooves".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntEscaped.FogCannotCatch".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntEscaped.InTheClear".Localized());

            Player.ChangeFatigue(10);

            if (Random.Shared.Next(5) == 0)
                if (Random.Shared.Next(2) == 0)
                    Player.ApplyStats(new DynStat(GameStats.Toughness, 1f));
                else
                    Player.ApplyStats(new DynStat(GameStats.Speed, 1f));

            ClearButtons();

            DoNext(() => ReturnToCamp(1));
        }

        public void RepeatWildHuntCaught(int pScore)
        {
            ClearOutput();

            ShowImage("wildhunt-chase");

            OutputText("Scenes.ErlKing.RepeatWildHuntCaught.RunThroughWoods".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntCaught.HoundsDriveYou".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntCaught.FogConfuses".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntCaught.NetTrap".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntCaught.CaughtInNet".Localized());

            if (Player.BunnyScore() >= 4 || Player.KitsuneScore() >= 4 || Player.HarpyScore() >= 4 || pScore > 100)
                DoNext(() => RepeatWildHuntAWinnerIsYou());
            else
                DoNext(() => RepeatWildHuntGivenToTheHounds());
        }

        public void RepeatWildHuntGivenToTheHounds()
        {
            ClearOutput();

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHounds.Disappointed".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHounds.FewMinutesDiversion".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHounds.PerhapsNextTime".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHounds.MomentOfRelief".Localized());

            if (Player.HasVagina())
                ShowImage("wildhunt-chased-female");
            else
                ShowImage("wildhunt-chased-male");

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHounds.CaneFlashes".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHounds.CutRope".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHounds.HoundsAttack".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHounds.HoundTearsArmor".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHounds.TongueVagina".Localized());
            else
                if (Player.Balls > 0)
                    OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHounds.TongueBalls".Localized());
                else
                    if (Player.HasCock())
                        OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHounds.TongueCock".Localized());
                    else
                        OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHounds.TongueTaint".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHounds.TongueAsshole".Localized());

            DoNext(() => RepeatWildHuntGivenToTheHoundsII());
        }

        public void RepeatWildHuntGivenToTheHoundsII()
        {
            ClearOutput();

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHoundsII.FogGratitude".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHoundsII.GlanceBack".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHoundsII.FoldLegs".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHoundsII.OtherHoundPushes".Localized());

            if (Player.Femininity < 30)
                OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHoundsII.StrongJaw".Localized());
            else
                if (Player.Femininity > 70)
                    OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHoundsII.DelicateChin".Localized());
                else
                    OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHoundsII.Chin".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHoundsII.TurnsToCock".Localized());

            if (Player.HasVagina())
            {
                if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Wolf || Player.Body.Get<TailPart>().Id == BodyPartTypes.Dog || Player.Body.Get<TailPart>().Id == BodyPartTypes.Fox)
                    ShowImage("wildhunt-catched-female-canine");
                else
                    if (Player.IsGoo())
                        ShowImage("wildhunt-catched-female-goo");
                    else
                        if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Draconic)
                            ShowImage("wildhunt-catched-female-dragon");
                        else
                            if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Shark)
                                ShowImage("wildhunt-catched-female-shark");
                            else
                                if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Horse)
                                    ShowImage("wildhunt-catched-female-horse");
                                else
                                    if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Cat)
                                        ShowImage("wildhunt-catched-female-feline");
                                    else
                                        ShowImage("wildhunt-catched-female");
            }
            else
                if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Wolf || Player.Body.Get<TailPart>().Id == BodyPartTypes.Dog || Player.Body.Get<TailPart>().Id == BodyPartTypes.Fox)
                    ShowImage("wildhunt-catched-male-canine");
                else
                    if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Draconic)
                        ShowImage("wildhunt-catched-male-dragon");
                    else
                        if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Shark)
                            ShowImage("wildhunt-catched-male-shark");
                        else
                            if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Horse)
                                ShowImage("wildhunt-catched-male-horse");
                            else
                                if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Cat)
                                    ShowImage("wildhunt-catched-male-feline");
                                else
                                    ShowImage("wildhunt-catched-male");

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHoundsII.Facefucked".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHoundsII.GroanAroundDick".Localized());

            Player.ButtChange(12 * 3, true, false, false);

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHoundsII.YelpNoEscape".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHoundsII.TheyEarnedIt".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHoundsII.TwoDogsFuck".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHoundsII.HoundBehindClaws".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHoundsII.TrembleDominated".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHoundsII.TheyCum".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHoundsII.AftercareLicks".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHoundsII.FogRecedes".Localized());

            var gemLoss= 10 + Random.Shared.Next(15);

            if (Player.Gems < gemLoss)
                gemLoss = Player.Gems;

            Player.Gems -= gemLoss;

            OutputText("Scenes.ErlKing.RepeatWildHuntGivenToTheHoundsII.LostGems".Localized(
                 gemLoss
            ));

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Corruption, 1f), new DynStat(GameStats.Lust, 0f));

            Player.ChangeFatigue(10);

            Player.Orgasm("Generic");

            Player.SlimeFeed();

            DoNext(() => TakeCaninePepper());
        }

        public void RepeatWildHuntAWinnerIsYou()
        {
            ClearOutput();

            ShowImage("wildhunt-huntsman");

            OutputText("Scenes.ErlKing.RepeatWildHuntAWinnerIsYou.Applause".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntAWinnerIsYou.ErlkingCongratulates".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntAWinnerIsYou.ChoiceOffered".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntAWinnerIsYou.LoweredDown".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntAWinnerIsYou.NetFallsAway".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntAWinnerIsYou.SeesErlkingCock".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntAWinnerIsYou.RewardThought".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntAWinnerIsYou.HiddenOptions".Localized());

            OutputText("Scenes.ErlKing.RepeatWildHuntAWinnerIsYou.ResentmentSpark".Localized());

            Player.ChangeFatigue(10);

            ClearButtons();

            AddButton(0, "Common.Button.Sex".Localized(), PredatoryPrey);

            AddButton(1, "Scenes.ErlKing.RepeatWildHuntAWinnerIsYou.ButtonPrize".Localized(), WhatsMyPrize);

            AddButton(2, "Common.Button.Stop".Localized(), StopTheMadness);

            AddButton(3, "Scenes.ErlKing.RepeatWildHuntAWinnerIsYou.ButtonSurrender".Localized(), SurrenderToTheHounds);

            AddButton(4, "Common.Button.Revenge".Localized(), HowDareYou);
        }

        public void WhatsMyPrize()
        {
            ClearOutput();

            ShowImage("item-gems");

            OutputText("Scenes.ErlKing.WhatsMyPrize.DemandCompensation".Localized());

            OutputText("Scenes.ErlKing.WhatsMyPrize.ErlkingPays".Localized());

            OutputText("Scenes.ErlKing.WhatsMyPrize.ErlkingDeparts".Localized());

            var gemFind= 10 + Random.Shared.Next(15);

            OutputText("Scenes.ErlKing.WhatsMyPrize.GemsFound".Localized(
                 gemFind
            ));

            Player.Gems += gemFind;

            var selector= Random.Shared.Next(4);

            if (selector == 0)
                DoNext(() => TakeCaninePepper());

            if (selector == 1)
                DoNext(() => TakeFoxBerry());

            if (selector == 2)
                DoNext(() => TakeNeonPinkEgg());

            if (selector == 3)
                DoNext(() => TakeGoldenRing());
        }

        public void TakeCaninePepper()
        {
            ClearOutput();

            ShowImage("item-cPepper");

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.CaninePepper), 1);

ReturnToCamp(1);
        }

        public void TakeFoxBerry()
        {
            ClearOutput();

            ShowImage("item-fBerry");

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.FoxBerry), 1);

ReturnToCamp(1);
        }

        public void TakeNeonPinkEgg()
        {
            ClearOutput();

            ShowImage("item-egg-neon");

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.NephilaPinkEgg), 1);

ReturnToCamp(1);
        }

        public void TakeGoldenRing()
        {
            ClearOutput();

            ShowImage("item-gRing");

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.GldRind), 1);

ReturnToCamp(1);
        }

        public void StopTheMadness()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.ErlkingDisabled, 1);

            ShowImage("wildhunt-escaped");

            OutputText("Scenes.ErlKing.StopTheMadness.ConfrontErlking".Localized());

            OutputText("Scenes.ErlKing.StopTheMadness.ErlkingAsksStop".Localized());

            OutputText("Scenes.ErlKing.StopTheMadness.ExplainFrustration".Localized());

            OutputText("Scenes.ErlKing.StopTheMadness.ErlkingDeparts".Localized());

            OutputText("Scenes.ErlKing.StopTheMadness.HuntEnds".Localized());

            ClearButtons();

            DoNext(() => ReturnToCamp(1));
        }

        public void SurrenderToTheHounds()
        {
            ClearOutput();

            OutputText("Scenes.ErlKing.SurrenderToTheHounds.ShiverFear".Localized());

            OutputText("Scenes.ErlKing.SurrenderToTheHounds.SurrenderBeg".Localized());

            OutputText("Scenes.ErlKing.SurrenderToTheHounds.ErlkingComforts".Localized());

            OutputText("Scenes.ErlKing.SurrenderToTheHounds.BrokenShiver".Localized());

            ShowImage("wildhunt-surrender-I");

            OutputText("Scenes.ErlKing.SurrenderToTheHounds.LookAtMe".Localized());

            OutputText("Scenes.ErlKing.SurrenderToTheHounds.TransformationStarts".Localized());

            if (Player.HasFur())
                OutputText("Scenes.ErlKing.SurrenderToTheHounds.FurBlack".Localized());
            else
                OutputText("Scenes.ErlKing.SurrenderToTheHounds.FurTide".Localized());

            OutputText("Scenes.ErlKing.SurrenderToTheHounds.MusclesSwell".Localized());

            if (Player.HasBreasts())
                OutputText("Scenes.ErlKing.SurrenderToTheHounds.ChestFlattens".Localized());
            else
                OutputText("Scenes.ErlKing.SurrenderToTheHounds.TFragment".Localized());

            OutputText("Scenes.ErlKing.SurrenderToTheHounds.TautMuscles".Localized());

            if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Dog)
                OutputText("Scenes.ErlKing.SurrenderToTheHounds.DoggiePaws".Localized());
            else
                OutputText("Scenes.ErlKing.SurrenderToTheHounds.LegsRebuild".Localized());

            OutputText("Scenes.ErlKing.SurrenderToTheHounds.GenitalsRearrange".Localized());

            if (Player.HasVagina() && !Player.HasCock())
                OutputText("Scenes.ErlKing.SurrenderToTheHounds.ClitToBalls".Localized());
            else
                if (Player.HasCock() && !Player.HasVagina())
                {
                    if (Player.Cocks.Count > 1)
                        OutputText("Scenes.ErlKing.SurrenderToTheHounds.CocksMerge".Localized());
                    else
                        if (Player.Cocks[0].Id != BodyPartTypes.Dog)
                            OutputText("Scenes.ErlKing.SurrenderToTheHounds.CockTransformsAgain".Localized());
                        else
                            OutputText("Scenes.ErlKing.SurrenderToTheHounds.CaninePrickThrobsAgain".Localized());
                }
                else
                    if (Player.HasVagina() && Player.HasCock())
                    {
                        if (Player.Cocks.Count > 1)
                            OutputText("Scenes.ErlKing.SurrenderToTheHounds.HermTransition".Localized());
                        else
                        {
                            OutputText("Scenes.ErlKing.SurrenderToTheHounds.FemaleTransition".Localized());

                            if (Player.Cocks[0].Id != BodyPartTypes.Dog)
                                OutputText("Scenes.ErlKing.SurrenderToTheHounds.CockTransformsAgain".Localized());
                            else
                                OutputText("Scenes.ErlKing.SurrenderToTheHounds.CaninePrickThrobsAgain".Localized());
                        }
                    }
                    else
                        if (!Player.HasVagina() && !Player.HasCock())
                            OutputText("Scenes.ErlKing.SurrenderToTheHounds.NewPrick".Localized());

            DoNext(() => SurrenderToTheHoundsII());
        }

        public void SurrenderToTheHoundsII()
        {
            ClearOutput();

            OutputText("Scenes.ErlKing.SurrenderToTheHoundsII.FurCoversYour".Localized());

            if (Player.Balls == 0)
                OutputText("Scenes.ErlKing.SurrenderToTheHoundsII.NewKnot".Localized());

            OutputText("Scenes.ErlKing.SurrenderToTheHoundsII.SheathForms".Localized());

            if (Player.Body.Get<WingsPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.ErlKing.SurrenderToTheHoundsII.WingsFallOff".Localized());

            OutputText("Scenes.ErlKing.SurrenderToTheHoundsII.DoggieSmile".Localized());

            ShowImage("wildhunt-surrender-II");

            OutputText("Scenes.ErlKing.SurrenderToTheHoundsII.CurlTowardCock".Localized());

            if (Player.Body.Get<FacePart>().Id != BodyPartTypes.Dog)
                OutputText("Scenes.ErlKing.SurrenderToTheHoundsII.MuzzleGrows".Localized());

            OutputText("Scenes.ErlKing.SurrenderToTheHoundsII.MasturbationForbidden".Localized());

            OutputText("Scenes.ErlKing.SurrenderToTheHoundsII.MasterCommands".Localized());

            OutputText("Scenes.ErlKing.SurrenderToTheHoundsII.MasterStrokes".Localized());

            OutputText("Scenes.ErlKing.SurrenderToTheHoundsII.EyesRollBack".Localized());

            if (!Player.HasKnot())
                OutputText("Scenes.ErlKing.SurrenderToTheHoundsII.NewKnot".Localized());

            OutputText("Scenes.ErlKing.SurrenderToTheHoundsII.KnotSwells".Localized());

            OutputText("Scenes.ErlKing.SurrenderToTheHoundsII.BuckingBliss".Localized());

            OutputText("Scenes.ErlKing.SurrenderToTheHoundsII.Climax".Localized());

            OutputText("Scenes.ErlKing.SurrenderToTheHoundsII.HoundsCleanCum".Localized());

            DoNext(() => SurrenderToTheHoundsIII());
        }

        public void SurrenderToTheHoundsIII()
        {
            ClearOutput();

            ShowImage("badend-wildhunt");

            OutputText("Scenes.ErlKing.SurrenderToTheHoundsIII.FollowHunt".Localized());

            GameOver();
        }

        public void PredatoryPrey()
        {
            ClearOutput();

            OutputText("Scenes.ErlKing.PredatoryPrey.StandMesmerized".Localized());

            OutputText("Scenes.ErlKing.PredatoryPrey.AskWhatOffering".Localized());

            OutputText("Scenes.ErlKing.PredatoryPrey.ExactlyWhatThinking".Localized());

            OutputText("Scenes.ErlKing.PredatoryPrey.NeckKissUndress".Localized());

            if (!Player.IsTaur())
            {
                if (Player.HasVagina() && !Player.HasCock())
                {
                    OutputText("Scenes.ErlKing.PredatoryPrey.BackAgainstTreeAgain".Localized());

                    if (Player.IsBiped() || Player.IsDrider() || Player.IsGoo())
                        OutputText("Scenes.ErlKing.PredatoryPrey.LegsAroundBackAgain".Localized());
                    else
                        if (Player.HasTailInsteadOfLegs())
                            OutputText("Scenes.ErlKing.PredatoryPrey.TailCoilsAroundAgain".Localized());

                    OutputText("Scenes.ErlKing.PredatoryPrey.HandUnderAssVaginal".Localized());

                    OutputText("Scenes.ErlKing.PredatoryPrey.TakeMeVaginal".Localized());

                    if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Dog || Player.Body.Get<TailPart>().Id == BodyPartTypes.Wolf || Player.Body.Get<TailPart>().Id == BodyPartTypes.Fox)
                        ShowImage("wildhunt-prey-female-canine");
                    else
                        if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Deer)
                            ShowImage("wildhunt-prey-female-cervine");
                        else
                            if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Cat)
                                ShowImage("wildhunt-prey-female-feline");
                            else
                                ShowImage("wildhunt-prey-female");

                    Player.CuntChange(12 * 3, true, true, false);

                    OutputText("Scenes.ErlKing.PredatoryPrey.FuckAgainstTree".Localized());

                    OutputText("Scenes.ErlKing.PredatoryPrey.ClimaxVaginal".Localized());

                    OutputText("Scenes.ErlKing.PredatoryPrey.ClingVaginal".Localized());

                    OutputText("Scenes.ErlKing.PredatoryPrey.FogDrowsyAgain".Localized());

                    OutputText("Scenes.ErlKing.PredatoryPrey.WakeUpLaterAgain".Localized());
                }
                else
                    if (Player.HasCock())
                    {
                        OutputText("Scenes.ErlKing.PredatoryPrey.BackAgainstTreeAgain".Localized());

                        if (Player.IsBiped())
                            OutputText("Scenes.ErlKing.PredatoryPrey.LegsAroundBackAgain".Localized());
                        else
                            if (Player.HasTailInsteadOfLegs())
                                OutputText("Scenes.ErlKing.PredatoryPrey.TailCoilsAroundAgain".Localized());

                        OutputText("Scenes.ErlKing.PredatoryPrey.HandUnderAssAnal".Localized());

                        OutputText("Scenes.ErlKing.PredatoryPrey.TakeMeAnal".Localized());

                        if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Dog || Player.Body.Get<TailPart>().Id == BodyPartTypes.Wolf || Player.Body.Get<TailPart>().Id == BodyPartTypes.Fox)
                            ShowImage("wildhunt-prey-male-canine");
                        else
                            if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Deer)
                                ShowImage("wildhunt-prey-male-cervine");
                            else
                                if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Rabbit)
                                    ShowImage("wildhunt-prey-male-bunny");
                                else
                                    if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Cat)
                                        ShowImage("wildhunt-prey-male-feline");
                                    else
                                        ShowImage("wildhunt-prey-male");

                        Player.ButtChange(12 * 3, true, true, false);

                        OutputText("Scenes.ErlKing.PredatoryPrey.FuckAnal".Localized());

                        OutputText("Scenes.ErlKing.PredatoryPrey.ClimaxAnal".Localized());

                        OutputText("Scenes.ErlKing.PredatoryPrey.ClingAnal".Localized());

                        OutputText("Scenes.ErlKing.PredatoryPrey.FogDrowsyAgain".Localized());

                        OutputText("Scenes.ErlKing.PredatoryPrey.WakeUpLaterAgain".Localized());
                    }
            }
            else
            {
                OutputText("Scenes.ErlKing.PredatoryPrey.StablePride".Localized());

                if (Player.HasVagina() && !Player.HasCock())
                {
                    ShowImage("wildhunt-prey-taur-female");

                    OutputText("Scenes.ErlKing.PredatoryPrey.ChestAgainstTree".Localized());

                    Player.CuntChange(12 * 3, true, true, false);

                    OutputText("Scenes.ErlKing.PredatoryPrey.ArmsAroundTrunk".Localized());

                    if (Player.BiggestLactation() > 0)
                        OutputText("Scenes.ErlKing.PredatoryPrey.MilkOnTrunk".Localized());

                    OutputText("Scenes.ErlKing.PredatoryPrey.AbrasionClimax".Localized());

                    OutputText("Scenes.ErlKing.PredatoryPrey.CumAgainstTree".Localized());

                    OutputText("Scenes.ErlKing.PredatoryPrey.AfterglowLog".Localized());

                    OutputText("Scenes.ErlKing.PredatoryPrey.Salve".Localized());

                    OutputText("Scenes.ErlKing.PredatoryPrey.WakeChest".Localized());
                }
                else
                    if (Player.HasCock())
                    {
                        ShowImage("wildhunt-prey-taur-male");

                        OutputText("Scenes.ErlKing.PredatoryPrey.HandjobPrep".Localized());

                        OutputText("Scenes.ErlKing.PredatoryPrey.HandjobPant".Localized());

                        OutputText("Scenes.ErlKing.PredatoryPrey.HandjobMilk".Localized());

                        OutputText("Scenes.ErlKing.PredatoryPrey.HandjobFaster".Localized());

                        OutputText("Scenes.ErlKing.PredatoryPrey.HandjobClimax".Localized());

                        OutputText("Scenes.ErlKing.PredatoryPrey.HandjobClean".Localized());

                        OutputText("Scenes.ErlKing.PredatoryPrey.SleepyCamp".Localized());
                    }
            }

            if (Player.Toughness < Player.Strength)
                Player.ApplyStats(new DynStat(GameStats.Toughness, 1f), new DynStat(GameStats.Lust, 0f));
            else
                Player.ApplyStats(new DynStat(GameStats.Strength, 1f), new DynStat(GameStats.Lust, 0f));

            Player.Orgasm("Generic");

            Player.SlimeFeed();

            ClearButtons();

            DoNext(() => ReturnToCamp(1));
        }

        public void HowDareYou()
        {
            ClearOutput();

            ShowImage("wildhunt-revenge");

            OutputText("Scenes.ErlKing.HowDareYou.ConfrontErlking".Localized());

            OutputText("Scenes.ErlKing.HowDareYou.StruckPained".Localized());

            OutputText("Scenes.ErlKing.HowDareYou.KnockDown".Localized());

            OutputText("Scenes.ErlKing.HowDareYou.BegsForCane".Localized());

            OutputText("Scenes.ErlKing.HowDareYou.RecountHunts".Localized());

            OutputText("Scenes.ErlKing.HowDareYou.LessonInMind".Localized());

            OutputText("Scenes.ErlKing.HowDareYou.Slap".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Rape".Localized(), HowDareYouII);

            AddNextButton("Scenes.ErlKing.HowDareYou.ButtonDont".Localized(), HowDareYouLeave);
        }

        public void HowDareYouII()
        {
            ClearOutput();

            if (Player.HasVagina() && !Player.HasCock())
            {
                OutputText("Scenes.ErlKing.HowDareYouII.PinHimDown".Localized());

                OutputText("Scenes.ErlKing.HowDareYouII.BuriesFace".Localized());

                OutputText("Scenes.ErlKing.HowDareYouII.PullsAntlers".Localized());

                OutputText("Scenes.ErlKing.HowDareYouII.HeadHeldLicking".Localized());

                ShowImage("wildhunt-revenge-female");

                OutputText("Scenes.ErlKing.HowDareYouII.DignifiedKingFallen".Localized());

                OutputText("Scenes.ErlKing.HowDareYouII.MoansSubmissive".Localized());

                OutputText("Scenes.ErlKing.HowDareYouII.FaceRide".Localized());

                OutputText("Scenes.ErlKing.HowDareYouII.StandsOverLord".Localized());

                OutputText("Scenes.ErlKing.HowDareYouII.FogTakesHim".Localized());
            }
            else
                if (Player.HasCock())
                {
                    OutputText("Scenes.ErlKing.HowDareYouII.RevealsCock".Localized());

                    OutputText("Scenes.ErlKing.HowDareYouII.TakesCockEagerly".Localized());

                    OutputText("Scenes.ErlKing.HowDareYouII.LacksSkillEnthusiasm".Localized());

                    OutputText("Scenes.ErlKing.HowDareYouII.ShovesHimDown".Localized());

                    OutputText("Scenes.ErlKing.HowDareYouII.TurnsOnAllFours".Localized());

                    ShowImage("wildhunt-revenge-male");

                    OutputText("Scenes.ErlKing.HowDareYouII.DignifiedKingBared".Localized());

                    OutputText("Scenes.ErlKing.HowDareYouII.AdmitsPrincess".Localized());

                    OutputText("Scenes.ErlKing.HowDareYouII.RewardPress".Localized());

                    OutputText("Scenes.ErlKing.HowDareYouII.PushIntoHim".Localized());

                    OutputText("Scenes.ErlKing.HowDareYouII.HeShivers".Localized());

                    OutputText("Scenes.ErlKing.HowDareYouII.TotalSubmission".Localized());

                    OutputText("Scenes.ErlKing.HowDareYouII.HeMoansSlump".Localized());

                    OutputText("Scenes.ErlKing.HowDareYouII.DirtySlutLiked".Localized());

                    OutputText("Scenes.ErlKing.HowDareYouII.OnceLordPurrs".Localized());

                    OutputText("Scenes.ErlKing.HowDareYouII.SlidesOutStrokes".Localized());

                    OutputText("Scenes.ErlKing.HowDareYouII.SheCroons".Localized());
                }

            AwardAchievement(AchievementIds.HunterIsHunted, true, true);

            Player.Orgasm("Generic");

            ClearButtons();

            TakeGoldenAntlers();
        }

        public void HowDareYouLeave()
        {
            ClearOutput();

            OutputText("Scenes.ErlKing.HowDareYouLeave.NoNeedForHim".Localized());

            OutputText("Scenes.ErlKing.HowDareYouLeave.AntlersBreak".Localized());

            OutputText("Scenes.ErlKing.HowDareYouLeave.StepsBack".Localized());

            OutputText("Scenes.ErlKing.HowDareYouLeave.FogTakesHim".Localized());

            TakeGoldenAntlers();
        }

        public void TakeGoldenAntlers()
        {

            Player.CreateKeyItem("Golden Antlers", 0, 0, 0, 0);

            if (CoC.Instance.Flags.Get(GameFlagIds.ErlkingCaneObtained) == 0)
            {
                OutputText("Scenes.ErlKing.TakeGoldenAntlers.AntlerStick".Localized());

                ShowImage("item-cane");

                Player.Inventory.Add(ItemRegistry.Create(WeaponIds.HuntsmansCane), 1);

ReturnToCamp(1);

                CoC.Instance.Flags.Set(GameFlagIds.ErlkingCaneObtained, 1);
            }
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void EncounterPrincessGwynn()
        {
            ClearOutput();

            ShowImage("wildhunt-princess");

            OutputText("Scenes.ErlKing.EncounterPrincessGwynn.RustlingBushes".Localized());

            OutputText("Scenes.ErlKing.EncounterPrincessGwynn.GwynnGreets".Localized());

            switch (Random.Shared.Next(6))
            {
                case 0:
                {
                OutputText("Scenes.ErlKing.EncounterPrincessGwynn.PinkSlathered".Localized());
                }

                break;

                case 1:
                {
                if ((CoC.Instance.Flags.Get(GameFlagIds.AkbalQuestStatus) & (int)AkbalEvent.QuestDone) == 0)
                    OutputText("Scenes.ErlKing.EncounterPrincessGwynn.AkbalMarks".Localized());
                else
                    OutputText("Scenes.ErlKing.EncounterPrincessGwynn.CoughsCumAgain".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.ErlKing.EncounterPrincessGwynn.CoughsCumAgain".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.ErlKing.EncounterPrincessGwynn.PinkStained".Localized());
                }

                break;

                case 4:
                {
                OutputText("Scenes.ErlKing.EncounterPrincessGwynn.BoundBruises".Localized());
                }

                break;

                case 5:
                {
                OutputText("Scenes.ErlKing.EncounterPrincessGwynn.HoundClawMarks".Localized());
                }

                break;
            }

            OutputText("Scenes.ErlKing.EncounterPrincessGwynn.MoreFeminine".Localized());

            OutputText("Scenes.ErlKing.EncounterPrincessGwynn.GwynnHugs".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredPrincessGwynn) == 0)
            {
                OutputText("Scenes.ErlKing.EncounterPrincessGwynn.ThanksForCane".Localized());

                OutputText("Scenes.ErlKing.EncounterPrincessGwynn.CallMeGwynn".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesEncounteredPrincessGwynn, CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredPrincessGwynn) + 1);

            OutputText("Scenes.ErlKing.EncounterPrincessGwynn.WhatCanIDo".Localized());

            if (Player.HasCock())
                OutputText("Scenes.ErlKing.EncounterPrincessGwynn.OfferSuck".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.ErlKing.EncounterPrincessGwynn.OfferEat".Localized());

            OutputText("Scenes.ErlKing.EncounterPrincessGwynn.SharesPotion".Localized());

            OutputText("Scenes.ErlKing.EncounterPrincessGwynn.Options".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.SuckMe".Localized(), null);

            AddButton(1, "Common.Button.Assfuck".Localized(), null);

            AddButton(2, "Common.Button.EatMe".Localized(), null);

            if (Player.HasCock())
            {
                AddButton(0, "Common.Button.SuckMe".Localized(), GwynnSucksDicks);

                AddButton(1, "Common.Button.Assfuck".Localized(), GwynnGetsButtfuxed);
            }

            if (Player.HasVagina())
                AddButton(2, "Common.Button.EatMe".Localized(), GwynnNomsDaCunts);

            AddButton(3, "Scenes.ErlKing.EncounterPrincessGwynn.ButtonMilkDick".Localized(), GwynnGetsDickmilked);

            AddButton(4, "Scenes.ErlKing.EncounterPrincessGwynn.ButtonGifts".Localized(), GwynnGibsGifts);

            AddButton(10, "Common.Button.Kill".Localized(), GwynnMurder);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void GwynnMurder()
        {
            ClearOutput();

            OutputText("Scenes.ErlKing.GwynnMurder.DecideToKill".Localized());

            OutputText("Scenes.ErlKing.GwynnMurder.SnapNeck".Localized());

            OutputText("Scenes.ErlKing.GwynnMurder.OneLessBitchBoy".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ErlkingDisabled, 1);

            ClearButtons();

            DoNext(() => ReturnToCamp(1));
        }

        public void GwynnSucksDicks()
        {
            ClearOutput();

            var x= 0;

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                ShowImage("wildhunt-princess-orally-horse");
            else
                if (Player.Cocks[x].Id == BodyPartTypes.Wolf || Player.Cocks[x].Id == BodyPartTypes.Dog || Player.Cocks[x].Id == BodyPartTypes.Fox)
                    ShowImage("wildhunt-princess-orally-canine");
                else
                    if (Player.Cocks[x].Id == BodyPartTypes.Dragon)
                        ShowImage("wildhunt-princess-orally-dragon");
                    else
                        if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.ClovenHoofed)
                            ShowImage("wildhunt-princess-orally-deer");
                        else
                            if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Hoofed)
                                ShowImage("wildhunt-princess-orally-bovine");
                            else
                                ShowImage("wildhunt-princess-orally");

            OutputText("Scenes.ErlKing.GwynnSucksDicks.GwynnKneels".Localized());

            OutputText("Scenes.ErlKing.GwynnSucksDicks.TongueAndFingers".Localized());

            OutputText("Scenes.ErlKing.GwynnSucksDicks.HappySlut".Localized());

            OutputText("Scenes.ErlKing.GwynnSucksDicks.CumsInMouth".Localized());

            OutputText("Scenes.ErlKing.GwynnSucksDicks.LicksCum".Localized());

            OutputText("Scenes.ErlKing.GwynnSucksDicks.PatsHead".Localized());

            OutputText("Scenes.ErlKing.GwynnSucksDicks.ThanksMaster".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Lust, 0f));

            Player.Orgasm("Dick");

            ClearButtons();

            DoNext(() => ReturnToCamp(1));
        }

        public void GwynnGetsButtfuxed()
        {
            ClearOutput();

            var x= 0;

            OutputText("Scenes.ErlKing.GwynnGetsButtfuxed.GwynnLubesYou".Localized());

            OutputText("Scenes.ErlKing.GwynnGetsButtfuxed.AlwaysPrepared".Localized());

            OutputText("Scenes.ErlKing.GwynnGetsButtfuxed.PresentsHerself".Localized());

            OutputText("Scenes.ErlKing.GwynnGetsButtfuxed.AnalEntry".Localized());

            if (Player.Cocks[x].Id == new Identifier("coc", "tentacle"))
                ShowImage("wildhunt-princess-anally-tentacle");
            else
                if (Player.Cocks[x].Id == BodyPartTypes.Lizard)
                    ShowImage("wildhunt-princess-anally-lizard");
                else
                    if (Player.Cocks[x].Id == BodyPartTypes.Dragon)
                        ShowImage("wildhunt-princess-anally-dragon");
                    else
                        if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Deer)
                            ShowImage("wildhunt-princess-anally-cervine");
                        else
                            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                                ShowImage("wildhunt-princess-anally-horse");
                            else
                                if (Player.Cocks[x].Id == BodyPartTypes.Human)
                                    ShowImage("wildhunt-princess-anally-human");
                                else
                                    if (Player.Cocks[x].Id == BodyPartTypes.Rhino)
                                        ShowImage("wildhunt-princess-anally-rhino");
                                    else
                                        if (Player.Cocks[x].Id == BodyPartTypes.Cat)
                                            ShowImage("wildhunt-princess-anally-feline");
                                        else
                                            if (Player.Cocks[x].Id == BodyPartTypes.Dog)
                                                ShowImage("wildhunt-princess-anally-canine");
                                            else
                                                if (Player.Cocks[x].Id == BodyPartTypes.Wolf)
                                                {
                                                    if (Player.HasVagina())
                                                        ShowImage("wildhunt-princess-anally-wolf-herm");
                                                    else
                                                        ShowImage("wildhunt-princess-anally-wolf-male");
                                                }
                                                else
                                                    ShowImage("wildhunt-princess-anally");

            OutputText("Scenes.ErlKing.GwynnGetsButtfuxed.AssMilksYou".Localized());

            OutputText("Scenes.ErlKing.GwynnGetsButtfuxed.GwynnCums".Localized());

            OutputText("Scenes.ErlKing.GwynnGetsButtfuxed.CleansYouUp".Localized());

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Lust, 0f));

            Player.Orgasm("Dick");

            ClearButtons();

            DoNext(() => ReturnToCamp(1));
        }

        public void GwynnNomsDaCunts()
        {
            ClearOutput();

            if (Player.Vaginas[0].Looseness == 5 && Player.Body.Get<TailPart>().Id == BodyPartTypes.Draconic)
                ShowImage("wildhunt-princess-lickgina-dragon");
            else
                if (Player.Vaginas[0].Id == BodyPartTypes.Horse)
                    ShowImage("wildhunt-princess-lickgina-equine");
                else
                    ShowImage("wildhunt-princess-lickgina");

            OutputText("Scenes.ErlKing.GwynnNomsDaCunts.YesMaam".Localized());

            OutputText("Scenes.ErlKing.GwynnNomsDaCunts.TonguePlunges".Localized());

            OutputText("Scenes.ErlKing.GwynnNomsDaCunts.YouSigh".Localized());

            OutputText("Scenes.ErlKing.GwynnNomsDaCunts.FingersAndOrgasm".Localized());

            OutputText("Scenes.ErlKing.GwynnNomsDaCunts.SlumpBack".Localized());

            if (Player.HasBreasts())
                OutputText("Scenes.ErlKing.GwynnNomsDaCunts.BetweenBreasts".Localized());

            OutputText("Scenes.ErlKing.GwynnNomsDaCunts.SeesHerLicking".Localized());

            OutputText("Scenes.ErlKing.GwynnNomsDaCunts.Aftercare".Localized());

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Lust, 0f));

            Player.Orgasm("Vaginal");

            ClearButtons();

            DoNext(() => ReturnToCamp(1));
        }

        public void GwynnGetsDickmilked()
        {
            ClearOutput();

            OutputText("Scenes.ErlKing.GwynnGetsDickmilked.GwynnAsksSure".Localized());

            OutputText("Scenes.ErlKing.GwynnGetsDickmilked.YouOrderHerDown".Localized());

            OutputText("Scenes.ErlKing.GwynnGetsDickmilked.HerDickRigid".Localized());

            OutputText("Scenes.ErlKing.GwynnGetsDickmilked.GwynnOffersLube".Localized());

            OutputText("Scenes.ErlKing.GwynnGetsDickmilked.StrokingHerCock".Localized());

            OutputText("Scenes.ErlKing.GwynnGetsDickmilked.WickedlyRaisesTail".Localized());

            ShowImage("wildhunt-princess-milking");

            OutputText("Scenes.ErlKing.GwynnGetsDickmilked.KneesBesideHead".Localized());

            OutputText("Scenes.ErlKing.GwynnGetsDickmilked.ShovesCockInMouth".Localized());

            OutputText("Scenes.ErlKing.GwynnGetsDickmilked.SelfSuckCum".Localized());

            OutputText("Scenes.ErlKing.GwynnGetsDickmilked.DropsTail".Localized());

            OutputText("Scenes.ErlKing.GwynnGetsDickmilked.LeavesHer".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f), new DynStat(GameStats.Libido, 2f));

            ClearButtons();

            DoNext(() => ReturnToCamp(1));
        }

        public void GwynnGibsGifts()
        {
            ClearOutput();

            ShowImage("item-lube");

            OutputText("Scenes.ErlKing.GwynnGibsGifts.AskForGift".Localized());

            OutputText("Scenes.ErlKing.GwynnGibsGifts.GwynnBounces".Localized());

            OutputText("Scenes.ErlKing.GwynnGibsGifts.LubeExplanation".Localized());

            OutputText("Scenes.ErlKing.GwynnGibsGifts.KissAndOfferMore".Localized());

            OutputText("Scenes.ErlKing.GwynnGibsGifts.RunsToMakeMore".Localized());

            OutputText("Scenes.ErlKing.GwynnGibsGifts.PocketsBottle".Localized());

            if (Random.Shared.Next(4) > 0)
            {
                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.PrincessPoker), 1);

ReturnToCamp(1);
            }
            else
            {
                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.PrincessPoker), 1);

GoldenRindBonus();
            }
        }

        public void GoldenRindBonus()
        {
            ClearOutput();

            ShowImage("wildhunt-princess");

            OutputText("Scenes.ErlKing.GoldenRindBonus.PuckerLeftover".Localized());

            DoNext(() => TakeGoldenRing());
        }

    }
}