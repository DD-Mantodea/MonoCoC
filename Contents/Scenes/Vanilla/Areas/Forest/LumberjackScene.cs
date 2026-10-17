using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Configuration;
using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{
    [Register<LumberjackScene>("coc:lumberjack")]
    public class LumberjackScene : Scene
    {
        public static bool _encountered;

        internal static bool Encountered => _encountered;

        public static bool _aggressive;

        public static bool _questioned;

        public static bool _freedSuccubus;

        public static bool _executed;

        public static bool _combatPending;

        public override void Entry()
        {
            if (_combatPending)
            {
                _combatPending = false;

                if (CombatScene.LastOutcome is CombatOutcome.HpVictory or CombatOutcome.LustVictory)
                    Defeated();
                else
                    Won();

                return;
            }

            Meet();
        }

        public static void Meet()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumberjack.MeetFirst".Localized());

            OutputText("Scenes.Lumberjack.MeetCautious".Localized());

            AddButton(0, "Scenes.Lumberjack.Button.Talk".Localized(), Talk);

            AddButton(1, "Scenes.Lumberjack.Button.Attack".Localized(), Attack);

            AddButton(2, "Scenes.Lumberjack.Button.Leave".Localized(), Leave);
        }

        public static void Leave()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumberjack.Leave".Localized());

            AddButton(0, "Common.Leave".Localized(), () => Go<CampScene>());
        }

        public static void Attack()
        {
            _encountered = true;

            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumberjack.AttackFirst".Localized());

            OutputText("Scenes.Lumberjack.AttackObserve".Localized());

            AddButton(0, "Scenes.Lumberjack.Button.Talk".Localized(), Talk);

            AddButton(1, "Scenes.Lumberjack.Button.Fight".Localized(), Fight);
        }

        public static void Fight()
        {
            _aggressive = true;

            StartFight();
        }

        public static void StartFight()
        {
            _combatPending = true;

            var lumberjack = EnemyRegistry.Create(EnemyIds.Lumberjack);

            lumberjack.OnDefeated = hpVictory =>
            {
                _combatPending = false;

                Defeated();
            };

            lumberjack.OnWon = _ =>
            {
                _combatPending = false;

                Won();
            };

            CombatScene.Start(
                lumberjack,

                () => Go<CampScene>());
        }

        public static void Talk()
        {
            _encountered = true;

            ClearOutput();

            ClearButtons();

            OutputText((_aggressive

                ? "Scenes.Lumberjack.Talk1Aggressive"

                : "Scenes.Lumberjack.Talk1Friendly").Localized());

            OutputText("Scenes.Lumberjack.TalkGreeting".Localized());

            OutputText("Scenes.Lumberjack.TalkReassure".Localized());

            OutputText("Scenes.Lumberjack.TalkSeatOffer".Localized());

            DoNext(Talk2);
        }

        public static void Talk2()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumberjack.TalkSecondFirst".Localized());

            OutputText("Scenes.Lumberjack.TalkSecondSecond".Localized());

            OutputText((Player.IsChild()

                ? "Scenes.Lumberjack.TalkSecondThirdChild"

                : "Scenes.Lumberjack.TalkSecondThird").Localized());

            AddButton(0, "Scenes.Lumberjack.Button.Drink".Localized(), () => Talk3(true));

            AddButton(1, "Scenes.Lumberjack.Button.Question".Localized(), Question);
        }

        public static void Question()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumberjack.QuestionFirst".Localized());

            OutputText("Scenes.Lumberjack.QuestionSecond".Localized());

            OutputText("Scenes.Lumberjack.QuestionThird".Localized());

            _questioned = true;

            AddButton(0, "Scenes.Lumberjack.Button.Drink".Localized(), () => Talk3(true));

            AddButton(1, "Scenes.Lumberjack.Button.Decline".Localized(), () => Talk3(false));
        }

        public static void Talk3(bool drank)
        {
            ClearOutput();

            ClearButtons();

            if (drank)
            {
                OutputText("Scenes.Lumberjack.DrinkFirst".Localized());

                OutputText("Scenes.Lumberjack.DrinkSecond".Localized());

                if (!_questioned)
                    OutputText("Scenes.Lumberjack.DrinkQuestion".Localized());

                OutputText("Scenes.Lumberjack.DrinkThird".Localized());

                Player.ChangeFatigue(-10);

                Player.ApplyStats(new DynStat(GameStats.Toughness, 1), new DynStat(GameStats.Libido, 1));
            }
            else
                OutputText("Scenes.Lumberjack.Decline".Localized());

            AddButton(0, "Scenes.Lumberjack.Button.Lifestyle".Localized(), Lifestyle);

            AddButton(1, "Scenes.Lumberjack.Button.Woodworking".Localized(), Woodwork);

            AddButton(2, "Scenes.Lumberjack.Button.Sex".Localized(), AskSex);
        }

        public static void Lifestyle()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumberjack.LifestyleFirst".Localized());

            OutputText("Scenes.Lumberjack.LifestyleSecond".Localized());

            OutputText("Scenes.Lumberjack.LifestyleThird".Localized());

            OutputText("Scenes.Lumberjack.LifestyleFourth".Localized());

            OutputText((Player.Intelligence > 50

                ? "Scenes.Lumberjack.Lifestyle5Smart"

                : "Scenes.Lumberjack.LifestyleFifth").Localized());

            OutputText("Scenes.Lumberjack.LifestyleSixth".Localized());

            AddButton(0, "Scenes.Lumberjack.Button.Woodworking".Localized(), Woodwork);

            AddButton(1, "Scenes.Lumberjack.Button.Sex".Localized(), AskSex);
        }

        public static void Woodwork()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumberjack.WoodworkFirst".Localized());

            OutputText("Scenes.Lumberjack.WoodworkSecond".Localized());

            OutputText("Scenes.Lumberjack.WoodworkThird".Localized());

            AddButton(0, "Scenes.Lumberjack.Button.BorrowTools".Localized(), BorrowTools);

            AddButton(1, "Common.Leave".Localized(), () => Go<CampScene>());
        }

        public static void AskSex()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumberjack.SexFirst".Localized());

            OutputText("Scenes.Lumberjack.SexSecond".Localized());

            OutputText("Scenes.Lumberjack.SexThird".Localized());

            DoNext(() => Reveal(false));
        }

        public static void BorrowTools()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumberjack.ToolsFirst".Localized());

            OutputText("Scenes.Lumberjack.ToolsSecond".Localized());

            OutputText("Scenes.Lumberjack.ToolsThird".Localized());

            DoNext(() => Reveal(true));
        }

        public static void Reveal(bool borrow)
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumberjack.RevealFirst".Localized());

            OutputText((borrow

                ? "Scenes.Lumberjack.Reveal2Borrow"

                : "Scenes.Lumberjack.Reveal2Sex").Localized());

            OutputText((borrow

                ? "Scenes.Lumberjack.Reveal3Borrow"

                : "Scenes.Lumberjack.Reveal3Sex").Localized());

            OutputText("Scenes.Lumberjack.RevealFourth".Localized());

            OutputText("");

            AddButton(0, "Scenes.Lumberjack.Button.Fight".Localized(), StartFight);
        }

        public static void Defeated()
        {
            ClearOutput();

            ClearButtons();

            var condition = CombatScene.LastOutcome == CombatOutcome.HpVictory

                ? "battered beyond threatening"

                : "hopelessly focused on himself";

            OutputText((_aggressive

                ? "Scenes.Lumberjack.Defeat1Aggressive"

                : "Scenes.Lumberjack.DefeatFirst").Localized(condition));

            AddButton(0, "Common.Continue".Localized(), Defeated2);
        }

        public static void Defeated2()
        {
            ClearOutput();

            ClearButtons();

            if (_aggressive)
            {
                OutputText("Scenes.Lumberjack.DefeatAggressiveFirst".Localized());

                OutputText("Scenes.Lumberjack.DefeatAggressiveSecond".Localized());
            }
            else
                OutputText("Scenes.Lumberjack.DefeatSecond".Localized());

            OutputText("Scenes.Lumberjack.SuccubusThanks".Localized());

            AddButton(0, "Scenes.Lumberjack.Button.Free".Localized(), Free);

            AddButton(1, "Scenes.Lumberjack.Button.Fuck".Localized(), Fuck);

            AddButton(2, "Scenes.Lumberjack.Button.Execute".Localized(), ExecuteSucc);

            AddButton(3, "Scenes.Lumberjack.Button.Leave".Localized(), LeaveSucc);
        }

        public static void Free()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumberjack.Free.WonUndoesBindings".Localized());

            OutputText("Scenes.Lumberjack.Free.SheApologizesShaking".Localized());

            OutputText("Scenes.Lumberjack.Free.SuccubusLeavesPlace".Localized());

            _freedSuccubus = true;

            Decision();
        }

        public static void Fuck()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumberjack.Fuck.FreeAfterServing".Localized());

            OutputText("Scenes.Lumberjack.Fuck.EagerlyOffersBody".Localized(
                Player.Corruption > 50 || Player.HasPerk(PerkIds.HistoryReligious)

                    ? "treat her however you like"

                    : "be considerate of her condition"));

            if (Player.HasCock())
            {
                OutputText("Scenes.Lumberjack.Fuck.SuccubusSquirms".Localized());

                OutputText("Scenes.Lumberjack.Fuck.SoftThighsRoughBlood".Localized());

                OutputText("Scenes.Lumberjack.Fuck.DoubtsHerEnjoyment".Localized());

                OutputText("Scenes.Lumberjack.Fuck.MullsThenSheClenches".Localized());

                OutputText("Scenes.Lumberjack.Fuck.PrefersYouToBrother".Localized());

                OutputText("Scenes.Lumberjack.Fuck.VaginalMusclesClench".Localized());

                Fuck2();

                return;
            }

            AddButton(0, "Scenes.Lumberjack.Button.PenTailing".Localized(), () => LesbFuck(true));

            AddButton(1, "Scenes.Lumberjack.Button.Tailgrind".Localized(), () => LesbFuck(false));
        }

        public static void LesbFuck(bool penetrate)
        {
            ClearOutput();

            ClearButtons();

            RegisterMacro("pen", penetrate);

            OutputText("Scenes.Lumberjack.LesbFuck.HeatBuildsSatisfied".Localized());

            OutputText("Scenes.Lumberjack.LesbFuck.TailPleasesEagerly".Localized());

            OutputText("Scenes.Lumberjack.LesbFuck.DoubtsHerEnjoyment".Localized());

            OutputText("Scenes.Lumberjack.LesbFuck.MullsThenSheClenches".Localized());

            OutputText("Scenes.Lumberjack.LesbFuck.PrefersYouToBrother".Localized());

            OutputText("Scenes.Lumberjack.LesbFuck.MusclesClenchTight".Localized());

            Fuck2();
        }

        public static void Fuck2()
        {
            OutputText("Scenes.Lumberjack.Fuck2.CatchesBreathAfter".Localized());

            OutputText("Scenes.Lumberjack.Fuck2.AsksToBeFreed".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 2));

            ClearButtons();

            AddButton(0, "Scenes.Lumberjack.Button.Free".Localized(), Free);

            AddButton(1, "Scenes.Lumberjack.Button.Execute".Localized(), ExecuteSucc);

            AddButton(2, "Scenes.Lumberjack.Button.Leave".Localized(), LeaveSucc);
        }

        public static void ExecuteSucc()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumberjack.ExecuteSucc.NoIntentionToSpare".Localized());

            Decision();
        }

        public static void LeaveSucc()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumberjack.LeaveSucc.RegagsTheSuccubus".Localized());

            Decision();
        }

        public static bool FoeExcited => CombatScene.LastEnemy is { } foe && foe.Lust >= foe.MaxLust;

        public static int HaxeGender => Player.Gender switch
        {
            Gender.Male => 0,

            Gender.Female => 1,

            Gender.Hermaphrodite => 2,

            _ => 3
        };

        public static void Decision()
        {
            OutputText("Scenes.Lumberjack.Decision.TurnsToCarpentry".Localized(
                FoeExcited ? "erection still apparent" : "too battered to stand"));

            OutputText("Scenes.Lumberjack.Decision.LooksAtYourAxe".Localized());

            OutputText("Scenes.Lumberjack.Decision.WordsFromTheLoser".Localized());

            AddButton(0, "Scenes.Lumberjack.Button.Rape".Localized(), () => Rape(HaxeGender));

            if (Player.HasVagina())
                AddButton(1, "Scenes.Lumberjack.Button.TakeVaginally".Localized(), () => Rape(2));
            else if (!Player.IsGenderless())
                AddButton(1, "Scenes.Lumberjack.Button.TakeAnally".Localized(), () => Rape(0));

            AddButton(2, "Scenes.Lumberjack.Button.Execute".Localized(), () => Execute(false));

            AddButton(3, "Scenes.Lumberjack.Button.Spare".Localized(), Spare);
        }

        public static void Rape(int genitals)
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumberjack.Rape.AxeBesideHisFace".Localized());

            OutputText("Scenes.Lumberjack.Rape.DemonContemplates".Localized(
                Player.DemonScore() >= 4 && Player.IsFeminine()

                    ? "[say: Fuck you,] he spits. [say: Not a surprise a whore like you wants to try a real man for once, but I'd rather die.]"

                    : "[say: We're all the same these days, aren't we? You're just another self-serving rape-fanatic.] He chuckles and smirks as he says this."));

            if (genitals is 0 or 2)
            {

                OutputText("Scenes.Lumberjack.Rape.FlauntsForIncubus".Localized());

                OutputText("Scenes.Lumberjack.Rape.PushesHimOntoBack".Localized(
                    FoeExcited ? "already rigid " : ""));

                OutputText("Scenes.Lumberjack.Rape.MockingSmirkDenial".Localized(
                    Player.DemonScore() >= 4 && Player.IsFeminine()

                        ? "[say: Cheap whores like you aren't worth it, I know I'm better than that,]"

                        : "[say: You're the one jumping on my cock like a bitch in heat,]"));

                var virgin = Player.HasVagina()

                    ? Player.Vaginas[0].Virgin

                    : Player.Anal.Virgin;

                if (Player.HasVagina())
                    Player.CuntChange(18f, true, true);
                else
                    Player.ButtChange(18f, true, true);

                var loose = genitals == 2

                    ? Player.Vaginas[0].Looseness >= 3

                    : Player.Anal.Looseness >= 3;

                OutputText("Scenes.Lumberjack.Rape.BumpyMeatStimulates".Localized(
                    virgin

                        ? " of your untainted depths. This dream-like pleasure is incredible, and your body reacts intensely at the joy of [if (hasvagina) {being deflowered by|giving your virginity to}] a lust demon"

                        : "",

                    loose

                        ? "He may not be packing something massive, but his tool is spectacularly suited to the job. You gyrate your hips, pressing his cock against the walls of your pliable and loose insides"

                        : "You take a few moments to adjust. The size he's packing would be enough on its own, but this tool of his is finely tuned to spectacularly fill its role"));

                OutputText("Scenes.Lumberjack.Rape.UnashamedMoaning".Localized());

                OutputText("Scenes.Lumberjack.Rape.MeatyPhallusDeep".Localized(
                    Player.Thickness >= 75 || Player.Tallness > 84 ? " with his marvelous strength" : ""));

                OutputText("Scenes.Lumberjack.Rape.RidesBuildingTempo".Localized());

                OutputText("Scenes.Lumberjack.Rape.WoodsmanMeetsHalfway".Localized());

                OutputText("Scenes.Lumberjack.Rape.BriefBlackoutPause".Localized());

                Player.SlimeFeed();

                Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation);
            }
            else
            {

                OutputText("Scenes.Lumberjack.Rape.SlapsCockLightly".Localized());

                OutputText("Scenes.Lumberjack.Rape.YanksDownHisPants".Localized(
                    FoeExcited

                        ? "His rigid, demonic cock betrays his attitude"

                        : "Not that his battered form has many options to choose from"));

                OutputText("Scenes.Lumberjack.Rape.CursesAsItTwitches".Localized());

                OutputText("Scenes.Lumberjack.Rape.GraspsBumpyCock".Localized());

                OutputText("Scenes.Lumberjack.Rape.OrgasmsWhileTaken".Localized());

                OutputText("Scenes.Lumberjack.Rape.SemenFillsDepths".Localized());
            }

            OutputText("Scenes.Lumberjack.Rape.UrgesQuenchedChoice".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 3));

            AddButton(0, "Scenes.Lumberjack.Button.Execute".Localized(), () => Execute(true));

            AddButton(1, "Scenes.Lumberjack.Button.Spare".Localized(), Spare);
        }

        public static void Execute(bool raped)
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumberjack.Execute.RaisesAxeToKill".Localized(
                raped ? "Now properly sated," : "He has made his choice, so"));

            _executed = true;

            Flags.Set(GameFlagIds.BeautifulSwordLevel, Flags.Get(GameFlagIds.BeautifulSwordLevel) + 1);

            End();
        }

        public static void Spare()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumberjack.Spare.DecidesNotToKill".Localized());

            End();
        }

        public static void End()
        {
            OutputText("Scenes.Lumberjack.End.SalvageTheRemains".Localized());

            if (!Player.HasKeyItem(KeyItemIds.CarpentersToolbox))
            {
                Player.CreateKeyItem(KeyItemIds.CarpentersToolbox);

                OutputText("Scenes.Lumberjack.End.Text".Localized());
            }

            Flags.Set(
                GameFlagIds.CampCabinWoodResources,

                Math.Min(999, Flags.Get(GameFlagIds.CampCabinWoodResources) + 50));

            AddButton(0, "Common.Continue".Localized(), () =>
            {
                View.Time.Minutes = 0;

                View.Time.PassTime(3);

                Go<CampScene>();
            });
        }

        public static bool GoreEnabled => OptionStore.Instance.Get("gore", 0) > 0;

        public static void Won()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumberjack.Won.FightNotLost".Localized());

            OutputText("Scenes.Lumberjack.Won.GroundConfusion".Localized());

            OutputText("Scenes.Lumberjack.Won.Text".Localized());

            OutputText("Scenes.Lumberjack.Won.DesperateFinalSurge".Localized());

            OutputText("Scenes.Lumberjack.Won.RefreshedAndSmug".Localized());

            OutputText("Scenes.Lumberjack.Won.ScreamsWhileDragged".Localized());

            OutputText("Scenes.Lumberjack.Won.NoNeedForStruggling".Localized());

            if (GoreEnabled)
            {
                OutputText("Scenes.Lumberjack.Won.GentlePatInstead".Localized());

                OutputText("Scenes.Lumberjack.Won.SardonicBigBoyDemand".Localized());

                OutputText("Scenes.Lumberjack.Won.DreadAxeApproaches".Localized());

                OutputText("Scenes.Lumberjack.Won.FaceLoomsClose".Localized());

                OutputText("Scenes.Lumberjack.Won.KeepsYouBreathing".Localized());

                OutputText("Scenes.Lumberjack.Won.RepeatedPassOuts".Localized());

                OutputText("Scenes.Lumberjack.Won.StepsBackWipesBrow".Localized());

                OutputText("Scenes.Lumberjack.Won.HeadSwimsShock".Localized());

                if (Player.HasCock())
                {
                    OutputText("Scenes.Lumberjack.Won.WontBeNeedingThis".Localized());

                    OutputText("Scenes.Lumberjack.Won.BearsDownOnYou".Localized());

                    OutputText("Scenes.Lumberjack.Won.ChuckleAtYourSize".Localized());

                    OutputText("Scenes.Lumberjack.Won.BlurryShapeRises".Localized());
                }
                else
                    OutputText("Scenes.Lumberjack.Won.SorryForTheFuss".Localized());
            }
            else
                OutputText("Scenes.Lumberjack.Won.HauledToWallSpace".Localized());

            AddButton(0, "Next".Localized(), Won2);
        }

        public static void Won2()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumberjack.Won2.WakesInDarkRoom".Localized());

            OutputText("Scenes.Lumberjack.Won2.Text".Localized());

            OutputText("Scenes.Lumberjack.Won2.PowerlessToStopHim".Localized());

            OutputText("Scenes.Lumberjack.Won2.WoodsmanExposesYou".Localized());

            OutputText("Scenes.Lumberjack.Won2.WantsToBreakFree".Localized());

            OutputText("Scenes.Lumberjack.Won2.DemonPaceQuickens".Localized());

            OutputText("Scenes.Lumberjack.Won2.HeldTightAsPulses".Localized());

            OutputText("Scenes.Lumberjack.Won2.RefreshedAndSmug".Localized());

            OutputText("Scenes.Lumberjack.Won2.SilentAsHeLeaves".Localized());

            AddButton(0, "Next".Localized(), Won3);
        }

        public static void Won3()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumberjack.Won3.PainFadesNewLife".Localized());

            OutputText("Scenes.Lumberjack.Won3.UsesYouFrequently".Localized());

            OutputText("Scenes.Lumberjack.Won3.DaysSpentWaiting".Localized());

            OutputText("Scenes.Lumberjack.Won3.YearsPassSeasons".Localized());

            Player.GameOver = true;

            GameOver();
        }
    }
}