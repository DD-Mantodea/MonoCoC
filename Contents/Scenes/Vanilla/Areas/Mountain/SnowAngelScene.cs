using MonoCoC.Attributes;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Contents.World;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Mountain
{
    [Register<SnowAngelScene>("coc:snow_angel")]
    public class SnowAngelScene : Scene
    {

        public override void Entry()
        {
            GatsSpectacularRouter();
        }

        public void GatsSpectacularRouter()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.GatsAngelQuestBegan) < 2)
                ChristmasEncounterAngelJunk();
            else
                if (!Player.HasKeyItem("North Star Key"))
                    FindTheWindUpKey();
                else
                    GiveThatBitchAKeyYo();
        }

        public void ChristmasEncounterAngelJunk()
        {
            ClearOutput();

            ClearButtons();

            if (!Holidays.IsSaturnalia() && CoC.Instance.Flags.Get(GameFlagIds.GatsAngelQuestBegan) < 1)
            {
                OutputText("Scenes.Xmassnowangel.ChristmasEncounterAngelJunk.TraipseMountainside".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), EncounterQuoteUnquoteAngel);

                AddButton(1, "Common.Button.No".Localized(), DontExploreGatsChristmasTale);
            }
            else
            {
                OutputText("Scenes.Xmassnowangel.ChristmasEncounterAngelJunk.TraipseMountainsideAgain".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), EncounterQuoteUnquoteAngel);

                AddButton(1, "Common.Button.No".Localized(), DontExploreGatsChristmasTale);
            }
        }

        public void DontExploreGatsChristmasTale()
        {
            ClearOutput();

            OutputText("Scenes.Xmassnowangel.DontExploreGatsChristmasTale.NoWarning".Localized());

            OutputText("Scenes.Xmassnowangel.DontExploreGatsChristmasTale.WarmerThere".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.GatsAngelDisabled, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void EncounterQuoteUnquoteAngel()
        {
            ClearOutput();

            if (!Holidays.IsSaturnalia() && CoC.Instance.Flags.Get(GameFlagIds.GatsAngelQuestBegan) < 1)
            {
                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.NothingYet".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.SourceOfSong".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.TaleToTell".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.NothingElse".Localized((silly ? " (you didn't)" : "")));

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.OnceUponTime".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.WistfulTune".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.NoOneKnew".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.GummedSmile".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.HushesAway".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.GatsAngelQuestBegan, 1);
            }
            else
            {
                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.NothingYetAgain".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.TrueGiant".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.InvisibleWings".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.GoddessWord".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.StepForward".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.CloserGet".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.WhoIsThere".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.NoTimeHide".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.LostChild".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.BacksAgainstWall".Localized());

                if (silly)
                    OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.WithoutPenis".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.LovingAura".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.QuitePretty".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.CocksEyebrow".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.BraveSoldier".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.AsksIfGod".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.NotGodNorGiant".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.PatShoulder".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.FallInLove".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.SilenceFalls".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.BringHimBack".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.MagicWeakened".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.TryYourBest".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.BelieveInYou".Localized());

                OutputText("Scenes.Xmassnowangel.EncounterQuoteUnquoteAngel.RestsHead".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.GatsAngelTimeToFindKey, 1);

                CoC.Instance.Flags.Set(GameFlagIds.GatsAngelQuestBegan, 2);
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void FindTheWindUpKey()
        {
            ClearOutput();

            OutputText("Scenes.Xmassnowangel.FindTheWindUpKey.ReachesBase".Localized());

            OutputText("Scenes.Xmassnowangel.FindTheWindUpKey.StripsBold".Localized());

            OutputText("Scenes.Xmassnowangel.FindTheWindUpKey.GreenGlow".Localized());

            OutputText("Scenes.Xmassnowangel.FindTheWindUpKey.RegretOnly".Localized());

            OutputText("Scenes.Xmassnowangel.FindTheWindUpKey.NotMuchGround".Localized());

            OutputText("Scenes.Xmassnowangel.FindTheWindUpKey.SturdyGrip".Localized());

            if (Player.Tallness > 60)
                OutputText("Scenes.Xmassnowangel.FindTheWindUpKey.CantStandStraight".Localized());

            OutputText("Scenes.Xmassnowangel.FindTheWindUpKey.GreenGrotto".Localized());

            OutputText("Scenes.Xmassnowangel.FindTheWindUpKey.PluckPrize".Localized());

            OutputText("Scenes.Xmassnowangel.FindTheWindUpKey.WarmGlow".Localized());

            OutputText("Scenes.Xmassnowangel.FindTheWindUpKey.ReentersTunnel".Localized());

            Player.CreateKeyItem("North Star Key", 0, 0, 0, 0);

            DoNext(() => ReturnToCamp(1));
        }

        public void GiveThatBitchAKeyYo()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.GatsAngelTimeToFindKey) < 876700)
            {
                OutputText("Scenes.Xmassnowangel.GiveThatBitchAKeyYo.PerilousClimb".Localized());

                OutputText("Scenes.Xmassnowangel.GiveThatBitchAKeyYo.TrekHalts".Localized());

                OutputText("Scenes.Xmassnowangel.GiveThatBitchAKeyYo.FuckAround".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.GiveTheKey".Localized(), GoodEndForChristmasMiracleICant);

                AddButton(1, "Common.Button.FuckSoldier".Localized(), TheWorstEndingEverDotJPG);

                AddButton(2, "Common.Button.FuckSpirit".Localized(), FuckTheAngelBadEnd);
            }
            else
            {
                OutputText("Scenes.Xmassnowangel.GiveThatBitchAKeyYo.PerilousClimbAgain".Localized());

                OutputText("Scenes.Xmassnowangel.GiveThatBitchAKeyYo.ReachesPeak".Localized());

                OutputText("Scenes.Xmassnowangel.GiveThatBitchAKeyYo.WalksToSoldier".Localized());

                OutputText("Scenes.Xmassnowangel.GiveThatBitchAKeyYo.SnapsPerfectly".Localized());

                OutputText("Scenes.Xmassnowangel.GiveThatBitchAKeyYo.WhirrClick".Localized());

                OutputText("Scenes.Xmassnowangel.GiveThatBitchAKeyYo.CarolLine".Localized());

                OutputText("Scenes.Xmassnowangel.GiveThatBitchAKeyYo.LittleWave".Localized());

                OutputText("Scenes.Xmassnowangel.GiveThatBitchAKeyYo.NorthShakes".Localized());

                OutputText("Scenes.Xmassnowangel.GiveThatBitchAKeyYo.AlreadyDead".Localized());

                OutputText("Scenes.Xmassnowangel.GiveThatBitchAKeyYo.TakesFists".Localized());

                OutputText("Scenes.Xmassnowangel.GiveThatBitchAKeyYo.UnsureRespond".Localized());

                OutputText("Scenes.Xmassnowangel.GiveThatBitchAKeyYo.SheDidntKnow".Localized());

                OutputText("Scenes.Xmassnowangel.GiveThatBitchAKeyYo.OnlyWaiting".Localized());

                OutputText("Scenes.Xmassnowangel.GiveThatBitchAKeyYo.ColdMerciless".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.GatsAngelDisabled, 1);

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void TheWorstEndingEverDotJPG()
        {
            ClearOutput();

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.SpinningKey".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.AtLeastTry".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.MindMadeUp".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.ReachesBase".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.ProppedShoulder".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.WhyDragged".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.InnocuousTone".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.MayIAsk".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.ImTheSpirit".Localized());

            if (Player.Corruption > 49)
                OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.BestResponse".Localized());
            else
                OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.WaitWhat".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.CantBeRight".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.HardToRead".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.EachCockSlot".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.AndWord".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.YourVagina".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.ConvinceSoldier".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.KeyholeWord".Localized());
            else
                OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.WindupKeyWord".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.WorthyCandidate".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.PlayfulSmile".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.WhatDoing".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.SlidesKey".Localized());
            else
                OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.MischievousTongue".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.ThisIsntRight".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.BacksAway".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.YouArentCarol".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.BeginsToFalter".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.LastGasp".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.ForlornArm".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.MoreToIt".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.SlipsBack".Localized());

            if (Player.Corruption > 49)
                OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.WasteOfTime".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.GlowingInnards".Localized());

            OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.DonsArmor".Localized());

            if (Player.Corruption > 49)
                OutputText("Scenes.Xmassnowangel.TheWorstEndingEverDotJPG.UnsatisfiedLust".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(2 + Player.Libido / 10 + Player.Corruption / 10)), new DynStat(GameStats.Corruption, 10f));

            DoNext(() => ReturnToCamp(1));

            CoC.Instance.Flags.Set(GameFlagIds.GatsAngelDisabled, 1);

            Player.RemoveKeyItem("North Star Key");
        }

        public void FuckTheAngelBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.SpiritLies".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.MochaTitan".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.HnnNorth".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.WoahRude".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.DoesntKnow".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.WaitWhatsGoingOn".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.AsIfSheDoesnt".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.CompensationProvided".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.DidntExpect".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.NoChoice".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.TakingOutJunk".Localized());
            else
                OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.CaressingCunt".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.MouthMuscle".Localized());

            if (Player.HasCock())
                OutputText(Player.MultiCockDescriptLight());
            else
                OutputText(Player.VaginaDescript());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.EachBump".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.MouthLubricated".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.MemberWord".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.SLetter".Localized());
            }
            else
                OutputText(Player.VaginaDescript());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.SoftnessMaw".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.GiantessHand".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.CockWord".Localized());
            else
                OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.PussyWord".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.AwakenedSomething".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.IncreasingEcstasy".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.SyrupOrgan".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.ButtDrilling".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.DickWord".Localized());
            else
                OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.VaginaWord".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.FuckTongue".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.PervadingThoughts".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.JerkHips".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.HotWhiteSeed".Localized());
            else
                OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.FeminineJuices".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.StarvingTongue".Localized());

            if (Player.HasCock())
            {
                OutputText(Player.MultiCockDescriptLight());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.ThrobsWord".Localized());
                else
                    OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.ThrobWord".Localized());
            }
            else
                OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.ThrobsWordAgain".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEnd.PaintingGiantess".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), FuckTheAngelBadEndPartII);
        }

        public void FuckTheAngelBadEndPartII()
        {
            ClearOutput();

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEndPartII.ItShakes".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEndPartII.KickItOver".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEndPartII.ShesSobbing".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEndPartII.WatchingEntireTime".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEndPartII.CouldVeJoined".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEndPartII.OneReason".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEndPartII.StillAngry".Localized());

            OutputText("Scenes.Xmassnowangel.FuckTheAngelBadEndPartII.OutOfMess".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.GatsAngelDisabled, 1);

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));

            DoNext(() => ReturnToCamp(1));
        }

        public void GoodEndForChristmasMiracleICant()
        {
            ClearOutput();

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.DecisionClear".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.SmilesBack".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.SmilingWryly".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.ClickWord".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.SoundResounds".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.SaysSoftly".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.OffGuard".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.SaysInteresting".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.StandSilently".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.EnormousHand".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.BeginsRetort".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.CastsAround".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.ChangesEyes".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.DoubtsHelp".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.PlantsKiss".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.ImpossiblyMoans".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.StillHere".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.LikeToKnow".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.NotASpirit".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.NorthWheezes".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.SheExplains".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.MeantToGoOn".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICant.ThreeWords".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GoodEndForChristmasMiracleICantII);
        }

        public void GoodEndForChristmasMiracleICantII()
        {
            ClearOutput();

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICantII.ThrowsArmsWide".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndForChristmasMiracleICantII.CrooksEye".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Dullahan.DefeatedDullahan.ButtonStay".Localized(), StayForPretendNTRGoodWinjobSex);

            AddButton(14, "Common.Button.Leave".Localized(), GoodEndLeaveBeforeSexings);
        }

        public void GoodEndLeaveBeforeSexings()
        {
            ClearOutput();

            OutputText("Scenes.Xmassnowangel.GoodEndLeaveBeforeSexings.GetsMessage".Localized());

            OutputText("Scenes.Xmassnowangel.GoodEndLeaveBeforeSexings.GentleHumming".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), ConclusionOfGiantessGoodEnd);
        }

        public void StayForPretendNTRGoodWinjobSex()
        {
            ClearOutput();

            OutputText("Scenes.Xmassnowangel.StayForPretendNTRGoodWinjobSex.PreferToStay".Localized());

            OutputText("Scenes.Xmassnowangel.StayForPretendNTRGoodWinjobSex.DrawsAway".Localized());

            OutputText("Scenes.Xmassnowangel.StayForPretendNTRGoodWinjobSex.ShouldersShift".Localized());

            OutputText("Scenes.Xmassnowangel.StayForPretendNTRGoodWinjobSex.HoistsUp".Localized());

            OutputText("Scenes.Xmassnowangel.StayForPretendNTRGoodWinjobSex.NoddingCalmly".Localized());

            OutputText("Scenes.Xmassnowangel.StayForPretendNTRGoodWinjobSex.KnightKicks".Localized());

            OutputText("Scenes.Xmassnowangel.StayForPretendNTRGoodWinjobSex.SimplePleasure".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

            OutputText("Scenes.Xmassnowangel.StayForPretendNTRGoodWinjobSex.ParagraphBreak".Localized());

            if (Player.Lust >= Player.MaxLust)
                OutputText("Scenes.Xmassnowangel.StayForPretendNTRGoodWinjobSex.HandsCrawl".Localized());

            OutputText("Scenes.Xmassnowangel.StayForPretendNTRGoodWinjobSex.KneadingDeeply".Localized());

            OutputText("Scenes.Xmassnowangel.StayForPretendNTRGoodWinjobSex.SexuallyBelabored".Localized());

            OutputText("Scenes.Xmassnowangel.StayForPretendNTRGoodWinjobSex.GuessComing".Localized());

            if (Player.Lust >= Player.MaxLust)
            {
                OutputText("Scenes.Xmassnowangel.StayForPretendNTRGoodWinjobSex.ClimaxShakes".Localized());

                if (Player.Wetness() >= 4 || Player.CumQ() >= 1000)
                    OutputText("Scenes.Xmassnowangel.StayForPretendNTRGoodWinjobSex.FigurativelyToo".Localized());

                OutputText("Scenes.Xmassnowangel.StayForPretendNTRGoodWinjobSex.SentencePeriod".Localized());
            }

            OutputText("Scenes.Xmassnowangel.StayForPretendNTRGoodWinjobSex.LaysPanting".Localized());

            OutputText("Scenes.Xmassnowangel.StayForPretendNTRGoodWinjobSex.WithNorthInTow".Localized());

            if (Player.Lust >= Player.MaxLust)
            {
                OutputText("Scenes.Xmassnowangel.StayForPretendNTRGoodWinjobSex.DoesUpArmor".Localized());

                Player.Orgasm("Generic");
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), ConclusionOfGiantessGoodEnd);
        }

        public void ConclusionOfGiantessGoodEnd()
        {
            ClearOutput();

            OutputText("Scenes.Xmassnowangel.ConclusionOfGiantessGoodEnd.AirCrisp".Localized());

            OutputText("Scenes.Xmassnowangel.ConclusionOfGiantessGoodEnd.CantBelieve".Localized());

            OutputText("Scenes.Xmassnowangel.ConclusionOfGiantessGoodEnd.BowsObligation".Localized());

            OutputText("Scenes.Xmassnowangel.ConclusionOfGiantessGoodEnd.BidFarewell".Localized());

            OutputText("Scenes.Xmassnowangel.ConclusionOfGiantessGoodEnd.ItsCarol".Localized());

            OutputText("Scenes.Xmassnowangel.ConclusionOfGiantessGoodEnd.PocketsKey".Localized());

            OutputText("Scenes.Xmassnowangel.ConclusionOfGiantessGoodEnd.MerryChristmas".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PcEncounteredChristmasElfBefore) == 0)
                OutputText("Scenes.Xmassnowangel.ConclusionOfGiantessGoodEnd.NoIdea".Localized());

            AwardAchievement(AchievementIds.HolidayChristmasII);

            if (!Player.HasPerk(PerkIds.AChristmasCarol))
            {
                OutputText("Scenes.Xmassnowangel.ConclusionOfGiantessGoodEnd.PerkGained".Localized());

                Player.CreatePerk(PerkIds.AChristmasCarol, 0, 0, 0, 0);
            }

            CoC.Instance.Flags.Set(GameFlagIds.GatsAngelGoodEnded, 1);

            DoNext(() => ReturnToCamp(1));
        }

    }
}