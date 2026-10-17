using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Linq;
using System.Collections.Generic;
using MonoCoC.Utils;
using MonoCoC.Contents.World;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Bazaar
{
    public enum TellyComType
    {
        TellyComL,

        TellyComB,

        TellyComK,

        TellyComP,

        TellyComA,

        TellyComH,

        TellyComD,

        TellyComCC
    }

    [Register<TellyScene>("coc:telly")]
    public class TellyScene : Scene
    {
        public const int TELLYPATH = 4;

        public static readonly string[] TellyTubbies = ["red", "blue", "pink", "purple", "yellow", "magenta", "green"];

        public static readonly string[] TellyGraphs = ["butterfly", "heart", "star", "flower", "cluster of hearts", "shooting star", "winged-heart", "snowflake"];

        public static readonly string[] TELLYPHOTOS = ["butterfly", "flower", "heart", "star", "skull"];

        public static SaveContent Save => SaveContent(SceneIds.Bazaar.Telly); public bool TellyGenesis;

        public bool noncommiTelly;

        public bool TellyCommute;

        public string TellyTubby = "purple";

        public string TellyGraph = "butterfly";

        public int TellyCardiogram;

        public float TellyTimer;

        public int TellyGram;

        public int TellyCommand;

        public bool TellyPlasmed;

        public int vesTelly;

        public int tasTelly;

        public int experimenTelly;

        public int immorTelly;

        public bool TellyComL;

        public bool TellyComB;

        public bool TellyComK;

        public bool TellyComP;

        public bool TellyComA;

        public bool TellyComH;

        public bool TellyComD;

        public bool TellyComCC;

        public Dictionary<string, bool> TellyOphile = new()
        {
            ["1"] = false,
            ["2"] = false,
            ["3"] = false,
            ["4"] = false,
            ["5"] = false,
            ["6"] = false,
            ["7"] = false,
            ["8"] = false,
            ["9"] = false,
            ["10"] = false,
            ["11"] = false,
            ["12"] = false,
            ["13"] = false,
            ["14"] = false,
            ["15"] = false,
            ["16"] = false,
            ["17"] = false,
            ["18"] = false,
            ["19"] = false,
            ["20"] = false,
            ["21"] = false,
            ["22"] = false,
            ["23"] = false
        };

        public bool incidenTelly;

        public int uTellyty;

        public override void Entry()
        {
            TellyCopy();
        }

        public void Reset()
        {
            TellyGenesis = false;

            noncommiTelly = false;

            TellyCommute = false;

            TellyTubby = "purple";

            TellyGraph = "butterfly";

            TellyCardiogram = 0;

            TellyTimer = 0;

            TellyGram = 0;

            TellyCommand = 0;

            TellyPlasmed = false;

            vesTelly = 0;

            tasTelly = 0;

            experimenTelly = 0;

            immorTelly = 0;

            TellyComL = false;

            TellyComB = false;

            TellyComK = false;

            TellyComP = false;

            TellyComA = false;

            TellyComH = false;

            TellyComD = false;

            TellyComCC = false;

            TellyOphile.Clear();
        }

        public bool TellyCom(TellyComType TellyGuidance, bool TellySet = false)
        {
            return TellyGuidance switch
            {
                TellyComType.TellyComL => TellyComL = TellyComL || TellySet,

                TellyComType.TellyComB => TellyComB = TellyComB || TellySet,

                TellyComType.TellyComK => TellyComK = TellyComK || TellySet,

                TellyComType.TellyComP => TellyComP = TellyComP || TellySet,

                TellyComType.TellyComA => TellyComA = TellyComA || TellySet,

                TellyComType.TellyComH => TellyComH = TellyComH || TellySet,

                TellyComType.TellyComD => TellyComD = TellyComD || TellySet,

                TellyComType.TellyComCC => TellyComCC = TellyComCC || TellySet,

                _ => false,
            };
        }

        public void TellyGenic()
        {

            if (!TellyGenesis)
            {
                TellyTubby = "purple";

                TellyGraph = "butterfly";
            }
            else
                if (View.Time.Days > TellyTimer)
                {
                    TellyTubby = TellyTubbies[Random.Shared.Next(TellyTubbies.Length)];

                    TellyGraph = TellyGraphs[Random.Shared.Next(TellyGraphs.Length)];
                }

            if ((View.Time.Days + View.Time.Hours / 24) > TellyTimer)
                TellyCardiogram = Random.Shared.Next(3);

            TellyTimer = View.Time.Days + View.Time.Hours / 24;
        }

        public void TellyPresence(bool TellyCall = false)
        {

            var TellyBusiness = View.Time.Hours >= 6 && View.Time.Hours < 18;

            if (TellyCall)
            {
                AddButton(TELLYPATH, "Common.Button.TtT".Localized(), () => TellyMarket());

                if (!TellyBusiness)
                    ButtonAt(TELLYPATH)?.DisableIf(true, TellyGenesis ? "Telly's wagon doesn't appear to be around at the moment." : "There's some strangely decorated signage indicating the availability of some shop with a name initialized here as \"TT&T\".\n(Hours of operation: 6am to 6pm)");

                if (TellyCommand == View.Time.Days)
                    if (TellyGenesis)
                        ButtonAt(TELLYPATH)?.DisableIf(true, "Telly isn't available right now.");
                    else
                        if (ButtonAt(TELLYPATH) is { } hiddenTellyButton) hiddenTellyButton.Visible = false;
            }
            else
                if (TellyBusiness)
                    OutputText("Scenes.Telly.TellyPresence.WagonBears".Localized());
        }

        public void HospiTellyty()
        {

            incidenTelly = true;

            ClearButtons();

            if (TellyCommute)
            {
                OutputText("Scenes.Telly.HospiTellyty.TravelsHappenUpon".Localized());

                OutputText("Scenes.Telly.HospiTellyty.InterestedInMaking".Localized());

                AddButton(0, "Common.Button.Shop".Localized(), () => TellyMarket(false, true));

                AddButton(1, "Common.Button.Leave".Localized(), () => TellyHo("[pg]While the offer is tempting, you have things to do, and no pressing need of her wares. You wave her off and she returns in kind, happily wishing you well as she resumes her journey."));
            }
            else
                if (TellyGenesis)
                {
                    OutputText("Scenes.Telly.HospiTellyty.PinkWagon".Localized());

                    AddButton(0, "Common.Button.Enter".Localized(), () => TellyMarket());

                    AddButton(1, "Common.Button.Leave".Localized(), () => TellyHo("[pg]Whatever it may be that has her out here, you'll leave her to it. If you change your mind about visiting her shop, you know where you're likely to find her later."));
                }
                else
                {
                    if (noncommiTelly)
                        OutputText("Scenes.Telly.HospiTellyty.AlongRoadTelly".Localized());
                    else
                    {
                        noncommiTelly = true;

                        OutputText("Scenes.Telly.HospiTellyty.VagueSemblancesRoad".Localized());
                    }

                    AddButton(0, "Common.Button.Enter".Localized(), () => TellyMarket());

                    AddButton(1, "Common.Button.Leave".Localized(), () => TellyHo("[pg]Your travel continues, as you're uninterested in the pastel wagon or its toys and treats."));
                }
        }

        public void TellyHo(string hasTellyVista = "")
        {

            if (incidenTelly)
            {
                incidenTelly = false;

                if (hasTellyVista == "")
                    ReturnToCamp(1);
                else
                {
                    OutputText(hasTellyVista);

                    ReturnToCamp(1);
                }
            }
            else
            {
                SceneInstance<BazaarScene>.Get().EnterTheBazaarAndMenu();

                SceneInstance<BazaarScene>.Get().ShopMenu();
            }
        }

        public void TellyMarket(bool TellyStasis = false, bool floaTelly = false)
        {
            ClearOutput();

            TellyGenic();

            var TellyOlogy = (int)Math.Floor((double)(View.Time.Hours % 3));

            if (immorTelly < DateTime.Now.Year && (DateTime.Now.Month == 2 && DateTime.Now.Day == 14) && TellyGifted() && Player.Gems >= 25)
            {
                OutputText("Scenes.Telly.TellyMarket.EnterTiny".Localized());

                OutputText("Scenes.Telly.TellyMarket.AsSuspected".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), () => TellySis(true));

                AddButton(1, "Common.Button.No".Localized(), () => TellySis(false));

                AddButton(2, "Common.Button.Telly".Localized(), EnTellychy);
            }
            else
            {
                if (!incidenTelly && !TellyGenesis)
                {
                    TellyGenesis = true;

                    if (TellyCommute)
                        OutputText("Scenes.Telly.TellyMarket.JustAsRemember".Localized());
                    else
                    {
                        OutputText("Scenes.Telly.TellyMarket.InsideToys".Localized((CoC.Instance.Flags.Get(GameFlagIds.CodexEntryAlice) > 0 ? "An Alice?" : "A demon?")));

                        OutputText("Scenes.Telly.TellyMarket.GlancesUp".Localized());

                        OutputText("Scenes.Telly.TellyMarket.RetractsHand".Localized());

                        Player.UnlockCodexEntry(GameFlagIds.CodexEntryAlice);

                        OutputText("Scenes.Telly.TellyMarket.ReturnsToCounter".Localized());
                    }
                }
                else
                    if (incidenTelly && !TellyCommute)
                    {
                        TellyCommute = true;

                        if (TellyGenesis)
                        {
                            OutputText("Scenes.Telly.TellyMarket.DoorUnlocked".Localized());

                            OutputText("Scenes.Telly.TellyMarket.TimeThereExplains".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Telly.TellyMarket.InsideToysAgain".Localized((CoC.Instance.Flags.Get(GameFlagIds.CodexEntryAlice) > 0 ? "An Alice?" : "A demon?")));

                            OutputText("Scenes.Telly.TellyMarket.FinishesNeedle".Localized());

                            Player.UnlockCodexEntry(GameFlagIds.CodexEntryAlice);
                        }
                    }
                    else
                        if (View.Time.Days > experimenTelly && experimenTelly != 0 && !incidenTelly)
                        {
                            OutputText("Scenes.Telly.TellyMarket.EntersShop".Localized());

                            OutputText("Scenes.Telly.TellyMarket.ReturnsGreeting".Localized());

                            OutputText("Scenes.Telly.TellyMarket.HitWithSmell".Localized());

                            OutputText("Scenes.Telly.TellyMarket.TailSwings".Localized());

                            OutputText("Scenes.Telly.TellyMarket.SheSays".Localized());

                            if (Player.HasStatusEffect(StatusEffectIds.DragonBreathCooldown))
                                Player.RemoveStatusEffect(StatusEffectIds.DragonBreathCooldown);

                            Player.ChangeFatigue(-Player.Fatigue);

                            Player.RefillHunger(20);

                            experimenTelly = 0;
                        }
                        else
                            if (floaTelly)
                            {
                                OutputText("Scenes.Telly.TellyMarket.SoundsLikeDeal".Localized());

                                OutputText("Scenes.Telly.TellyMarket.WelcomeToToys".Localized());

                                TellyGram = 0;
                            }
                            else
                                if (Holidays.IsHalloween(true) && !incidenTelly)
                                {
                                    OutputText("Scenes.Telly.TellyMarket.WalkIntoShop".Localized());

                                    OutputText("Scenes.Telly.TellyMarket.SheShouts".Localized());

                                    OutputText("Scenes.Telly.TellyMarket.GreetTellyJumps".Localized());

                                    OutputText("Scenes.Telly.TellyMarket.ThatsWhatSheSells".Localized());
                                }
                                else
                                {
                                    OutputText("Scenes.Telly.TellyMarket.InsideToysThird".Localized());

                                    if (tasTelly == View.Time.Days * 100 + View.Time.Hours)
                                        OutputText("Scenes.Telly.TellyMarket.SitsHappily".Localized());
                                    else
                                        if (experimenTelly == View.Time.Days)
                                            OutputText("Scenes.Telly.TellyMarket.SitsBehindHappily".Localized());
                                        else
                                            switch (TellyCardiogram)
                                            {
                                                case 0:
                                                    {
                                                        OutputText("Scenes.Telly.TellyMarket.DoodleCounter".Localized());
                                                    }

                                                    break;

                                                case 1:
                                                    {
                                                        OutputText("Scenes.Telly.TellyMarket.SittingOnBed".Localized());
                                                    }

                                                    break;

                                                case 2:
                                                    {
                                                        OutputText("Scenes.Telly.TellyMarket.DrawingHearts".Localized());
                                                    }

                                                    break;
                                            }

                                    if (!TellyStasis)
                                    {
                                        OutputText("Scenes.Telly.TellyMarket.GlancesUpAtYou".Localized());

                                        TellyGram = 0;
                                    }
                                }

                if (TellyStasis)
                    TellyShopping();
                else
                    TellyCopy();
            }
        }

        public void TellyCopy()
        {

            ClearButtons();

            AddButton(0, "Common.Button.Buy".Localized(), TellyShopping);

            AddButton(1, "Common.Button.Appearance".Localized(), TellyVision);

            AddButton(2, "Common.Button.Talk".Localized(), () => TellySurvey());

            AddButton(14, "Common.Button.Leave".Localized(), () => TellyHo());
        }

        public void TellyShopping()
        {
            var TellyCommerce = new List<TellyAnalysis>
            {
                new() { TellyProcessing = OneOf<Identifier, Action>.FromLeft(new Identifier("coc", "lolipop")), TellyNym = "Lolipop", TellyPayment = 300, TellyPrompt = "You check out a jar of hard candy-topped sticks labeled [say: Lolipops!] with a warning sign attached under the label. Telly chimes in to explain with almost musical cadence, [say: They're bright and sweet, like me! Take too many and you'll look like me too! 300 gems if you're still interested.]" },

                new() { TellyProcessing = OneOf<Identifier, Action>.FromLeft(new Identifier("coc", "numb_rocks")), TellyNym = "NumbRox", TellyPayment = 60, TellyPrompt = "You check out a shelf lined with packets of tiny candy pieces. [say: Those are Numb Rocks,] Telly explains, [say: they taste like sparkles! 60 gems for a pack.]" },

                new() { TellyProcessing = OneOf<Identifier, Action>.FromRight(TellyPlasm), TellyNym = "Candies", TellyPayment = 3, TellyPrompt = "" },

                new() { TellyProcessing = OneOf<Identifier, Action>.FromRight(TellyBear), TellyNym = "Teddy Bear", TellyPayment = 50, TellyPrompt = "" },

                new() { TellyProcessing = OneOf<Identifier, Action>.FromLeft(new Identifier("coc", "rubber_ball")), TellyNym = "Rubber Ball", TellyPayment = 10, TellyPrompt = "You examine a plastic jar, finding numerous purple balls in it. Telly chimes in, [say: Those are bouncy balls, [mister]!] She hops back and forth as she explains, [say: You throw them and they bounce off!] Playful, though a bit dangerous perhaps. [say: 10 gems each!]" },

                new() { TellyProcessing = OneOf<Identifier, Action>.FromRight(TellyPhoto), TellyNym = "Face Paint", TellyPayment = 5, TellyPrompt = "" }
            };

            if (TellyCom(TellyComType.TellyComH))
                TellyCommerce.Add(new() { TellyProcessing = OneOf<Identifier, Action>.FromRight(ViTellyty), TellyNym = "Hug", TellyPayment = 1, TellyPrompt = "Another hug would brighten your day. Of course, you'll pay her 20 gems for the service." });

            ClearButtons();

            AddButton(14, "Common.Button.Back".Localized(), TellyCopy);

            foreach (var analysis in TellyCommerce)
                TellyLens(analysis);
        }

        public void TellyLens(TellyAnalysis TellyAnalysis)
        {
            var TellyPayment = TellyAnalysis.TellyPayment;

            UI.Components.Button button;

            if (TellyAnalysis.TellyProcessing.IsLeft)
            {
                var item = ItemRegistry.Create(TellyAnalysis.TellyProcessing.Left);

                button = AddNextButton(TellyAnalysis.TellyNym, () => TellySales(TellyAnalysis));

                button.Hint(item.LongName, item.LongName);
            }
            else
            {
                button = AddNextButton(TellyAnalysis.TellyNym, TellyAnalysis.TellyProcessing.Right);

                button.Hint(TellyAnalysis.TellyPrompt);
            }

            button.DisableIf(Player.Gems < TellyPayment,

                TellyPayment > 1 ? $"You need {TellyPayment} gems." : "You don't have anything to pay her.");
        }

        public void TellySales(TellyAnalysis TellyAnalysis)
        {
            ClearOutput();

            OutputText("Scenes.Telly.TellySales.SalesPromptBreak".Localized(
                 TellyAnalysis.TellyPrompt
            ));

            AddButton(0, "Common.Button.Yes".Localized(), () => TellyKinesis(TellyAnalysis));

            AddButton(1, "Common.Button.No".Localized(), () => TellyMarket(true));
        }

        public void TellyKinesis(TellyAnalysis TellyAnalysis)
        {
            if (!TellyAnalysis.TellyProcessing.IsLeft)
                throw new Exception("Managed to hit TellyKinesis with a function in TellyProcessing.");

            var TellyChoice = ItemRegistry.Create(TellyAnalysis.TellyProcessing.Left);

            var TellyPayment = TellyAnalysis.TellyPayment;

            Player.Gems -= TellyPayment;

            Player.Inventory.Add(TellyChoice, 1);

            TellyMarket(true);
        }

        public void TellyDrama(TellyAnalysis TellyAnalysis)
        {

            int TellyPayment = TellyAnalysis.TellyPayment;

            Player.Gems += TellyPayment;

            OutputText("Scenes.Telly.TellyDrama.ReturnsGems".Localized());

            TellyMarket(true);
        }

        public void TellyPlasm()
        {
            ClearOutput();

            OutputText("Scenes.Telly.TellyPlasm.LargestJar".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), TellyPlasmic);

            AddButton(1, "Common.Button.No".Localized(), () => TellyMarket(true));
        }

        public void TellyPlasmic()
        {
            ClearOutput();

            Player.Gems -= 3;

            OutputText("Scenes.Telly.TellyPlasmic.TossGems".Localized());

            Player.RefillHunger(1);

            TellyPlasmed = true;

            TellyMarket(true);
        }

        public void TellyPhoto()
        {
            ClearOutput();

            OutputText("Scenes.Telly.TellyPhoto.ShopkeepTendency".Localized());

            OutputText("Scenes.Telly.TellyPhoto.JumpsAtSuggestion".Localized());

            ClearButtons();

            foreach (var TellyGuidance in Enumerable.Range(0, TELLYPHOTOS.Length))
                AddNextButton(Utils.Utils.CapitalizeFirstLetter(TELLYPHOTOS[TellyGuidance]), () => TellyAlgia(TellyGuidance));

            AddButton(14, "Scenes.CorruptedGlade.NeverMind".Localized(), TellyDramatic);
        }

        public void TellyAlgia(int TellyGuidance)
        {
            ClearOutput();

            Player.Gems -= 5;

            OutputText("Scenes.Telly.TellyAlgia.HandsGems".Localized());

            if (Player.HasFur())
                OutputText("Scenes.Telly.TellyAlgia.PaintingOnFur".Localized());
            else
                if (Player.HasGooSkin())
                    OutputText("Scenes.Telly.TellyAlgia.ExperimentalStrokes".Localized());

            OutputText("Scenes.Telly.TellyAlgia.ChimesHappily".Localized());

            OutputText("Scenes.Telly.TellyAlgia.GleefulHumming".Localized());

            OutputText("Scenes.Telly.TellyAlgia.FinishesGrabs".Localized(
                 TELLYPHOTOS[TellyGuidance]
            ));

            var TellyCall = Player.CreateOrFindStatusEffect(StatusEffectIds.TellyVised);

            TellyCall.Values[0] = TellyGuidance;

            DoNext(() => TellyMarket());
        }

        public void TellyDramatic()
        {
            ClearOutput();

            OutputText("Scenes.Telly.TellyDramatic.DecidesAgainst".Localized());

            OutputText("Scenes.Telly.TellyDramatic.SetsPaints".Localized());

            DoNext(() => TellyMarket());
        }

        public void TellyBear()
        {
            ClearOutput();

            OutputText("Scenes.Telly.TellyBear.LargePile".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Yes".Localized(), () => TellyKinesis(new TellyAnalysis { TellyProcessing = OneOf<Identifier, Action>.FromLeft(UseableIds.TeddyBear), TellyNym = "TeddyBear", TellyPayment = 50, TellyPrompt = "" }));

            AddNextButton("Common.Button.No".Localized(), () => TellyMarket(true));

            AddNextButton("Common.Button.HugTelly".Localized(), () => TellyBears());
        }

        public void TellyBears(bool TellyCall = true, int TellyGuidance = 0)
        {
            ClearOutput();

            if (TellyCall)
            {
                OutputText("Scenes.Telly.TellyBears.WasteToJust".Localized());

                OutputText("Scenes.Telly.TellyBears.LetTellyCharge".Localized());

                OutputText("Scenes.Telly.TellyBears.FollowsSuit".Localized((Player.Tallness > 53 ? "lifting her off the floor as you do, and " : "")));

                OutputText("Scenes.Telly.TellyBears.UponRelease".Localized());

                Player.ApplyStats(new DynStat(GameStats.Corruption, .5f));

                Player.ChangeFatigue(-20);

                vesTelly = (int)Math.Floor((double)(View.Time.Days * 100 + View.Time.Hours));

                ClearButtons();

                AddNextButton("Common.Button.Buy".Localized(), () => TellyBears(false, 0));

                AddNextButton("Common.Button.Donate".Localized(), () => TellyBears(false, 1));

                AddNextButton("Common.Button.NoThanks".Localized(), () => TellyBears(false, 2));
            }
            else
                switch (TellyGuidance)
                {
                    case 0:
                        {
                            OutputText("Scenes.Telly.TellyBears.CuddlyEffects".Localized());

                            OutputText("Scenes.Telly.TellyBears.SkillfulMerchant".Localized());

                            Player.Gems -= 50;

                            TellyCom(TellyComType.TellyComH, true);

                            InventoryScene.TakeItem(ItemRegistry.Create(UseableIds.TeddyBear), () => TellyMarket(true));
                        }

                        break;

                    case 1:
                        {
                            OutputText("Scenes.Telly.TellyBears.NoNeedFor".Localized((Player.Gems < 51 ? "every gem you've got" : "the full 50 gem payment")));

                            OutputText("Scenes.Telly.TellyBears.ShiversGlee".Localized());

                            OutputText("Scenes.Telly.TellyBears.FollowsUp".Localized());

                            Player.Gems -= Player.Gems >= 50 ? 50 : Player.Gems;

                            TellyCom(TellyComType.TellyComH, true);

                            DoNext(() => TellyShopping());
                        }

                        break;

                    case 2:
                        {
                            // 原版 Telly.hx:791：熊堆满 10 只时追加一句
                            OutputText("Scenes.Telly.TellyBears.RatherNot".Localized(
                                CoC.Instance.Flags.Get(GameFlagIds.BedBears) >= 10

                                    ? ", your teddy fort is already at max capacity back home"

                                    : ""));

                            OutputText("Scenes.Telly.TellyBears.MoodDims".Localized());

                            DoNext(() => TellyShopping());
                        }

                        break;
                }
        }

        public void ViTellyty()
        {
            ClearOutput();

            OutputText("Scenes.Telly.ViTellyty.NeedMore".Localized());

            OutputText("Scenes.Telly.ViTellyty.IJustHuggedYou".Localized(
                 vesTelly == View.Time.Days * 100
            ));

            OutputText("Scenes.Telly.ViTellyty.RelinquishEmbrace".Localized((Player.Gems < 20 ? "what meager funds you have" : "20 gems")));

            OutputText("Scenes.Telly.ViTellyty.ReturnsEmbrace".Localized());

            OutputText("Scenes.Telly.ViTellyty.ReleasesChildish".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, .5f));

            Player.ChangeFatigue(-20);

            Player.Gems -= Player.Gems >= 20 ? 20 : Player.Gems;

            vesTelly = (int)Math.Floor((double)(View.Time.Days * 100 + View.Time.Hours));

            DoNext(() => TellyMarket());
        }

        public void TellyVision()
        {
            ClearOutput();

            OutputText("Scenes.Telly.TellyVision.SmallChildLike".Localized(
                 MeasurementsUtils.BriefHeight(46)
            ));

            if (Holidays.IsHalloween(true))
                OutputText("Scenes.Telly.TellyVision.UnusualAttire".Localized());
            else
                OutputText("Scenes.Telly.TellyVision.UsualAttire".Localized());

            DoNext(() => TellyMarket());
        }

        public void TellySurvey(int TellyGuidance = -1)
        {

            ClearButtons();

            AddButton(0, "Common.Button.Chat".Localized(), TellyCommunication);

            AddButton(1, "Common.Button.Merchanting".Localized(), TellyMotor);

            if (TellyGenesis)
                AddButton(2, "Common.Button.Bazaar".Localized(), TellyOperation);

            AddButton(3, "Common.Button.Customers".Localized(), TellyRgy);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.Liddellium)) && CoC.Instance.Flags.Get(GameFlagIds.LiddelliumFlag) >= 0)
                AddButton(4, "Common.Button.Strangepotion".Localized(), TellyStic);
            else
                if (!TellyCom(TellyComType.TellyComL) && CoC.Instance.Flags.Get(GameFlagIds.LiddelliumFlag) < 0)
                    AddButton(4, "Common.Button.Liddellium".Localized(), TellyOtic);

            if (AffectionaTelly(false))
                AddNextButton("Common.Button.Gift".Localized(), () => AffectionaTelly());

            if (TellyGuidance != -1)
                ButtonAt(TellyGuidance)?.DisableIf(true);

            AddButton(14, "Common.Button.Back".Localized(), () => TellyCopy());
        }

        public void TellyMotor()
        {
            ClearOutput();

            OutputText("Scenes.Telly.TellyMotor.HowWhyStart".Localized());

            OutputText("Scenes.Telly.TellyMotor.PunctuatesPoint".Localized());

            OutputText("Scenes.Telly.TellyMotor.HowGetStarted".Localized());

            OutputText("Scenes.Telly.TellyMotor.BlinksBlankly".Localized());

            OutputText("Scenes.Telly.TellyMotor.RuinedOldToys".Localized((CoC.Instance.Flags.Get(GameFlagIds.SheilaDemon) != 1 ? "--but they don't let me inside. I have to offer money up front and make a deal by proxy of the guards" : "")));

            OutputText("Scenes.Telly.TellyMotor.WorkedHard".Localized());

            TellySurvey(1);
        }

        public void TellyOperation()
        {
            ClearOutput();

            OutputText("Scenes.Telly.TellyOperation.LifeAtBazaar".Localized((!incidenTelly ? "" : "t")));

            OutputText("Scenes.Telly.TellyOperation.DeclaresAddsIn".Localized());

            OutputText("Scenes.Telly.TellyOperation.TurnsGaze".Localized());

            TellySurvey(2);
        }

        public void TellyRgy()
        {
            ClearOutput();

            OutputText("Scenes.Telly.TellyRgy.CustomersCanBe".Localized());

            OutputText("Scenes.Telly.TellyRgy.AliceMerchantTurns".Localized());

            TellySurvey(3);
        }

        public void TellyStic()
        {
            ClearOutput();

            OutputText("Scenes.Telly.TellyStic.DespiteDemeanor".Localized());

            OutputText("Scenes.Telly.TellyStic.GazingInHands".Localized());

            OutputText("Scenes.Telly.TellyStic.OhThatWasQuick".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.LiddelliumFlag, -1);

            TellySurvey(4);
        }

        public void TellyOtic()
        {
            ClearOutput();

            OutputText("Scenes.Telly.TellyOtic.WouldSell".Localized());

            OutputText("Scenes.Telly.TellyOtic.ShakesHeadCertainly".Localized());

            OutputText("Scenes.Telly.TellyOtic.CantBeAllBad".Localized());

            OutputText("Scenes.Telly.TellyOtic.JovialShopkeep".Localized());

            TellyCom(TellyComType.TellyComL, true);

            TellySurvey(4);
        }

        public void TellyCommunication()
        {
            ClearOutput();

            TellyGram += 1;

            var TellyComs = new List<int>();

            foreach (var pair in TellyOphile)
            {
                var TellySet = int.Parse(pair.Key);

                if (!pair.Value && TellyCall(TellySet))
                    TellyComs.Add(TellySet);
            }

            if (TellyComs.Count == 0)
                foreach (var digiTelly in TellyOphile.Keys)
                {
                    var TellySet = int.Parse(digiTelly);

                    if (TellyCall(TellySet))
                        TellyComs.Add(TellySet);
                }

            int TellyGuidance = TellyComs[Random.Shared.Next(TellyComs.Count)];

            TellyOphile[TellyGuidance.ToString()] = true;

            uTellyty = TellyGuidance;

            switch (TellyGuidance)
            {
                case 1:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.FavoriteColor".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.DarkerThanNight".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.DemonTwirls".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.GetsBackUp".Localized());
                    }

                    break;

                case 2:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.LeansForward".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.ExclaimsShopkeep".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.FoxesAvoidPeople".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.EyeSquishing".Localized());
                    }

                    break;

                case 3:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.GeneralTalk".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.HandAcross".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.TiltsHead".Localized());
                    }

                    break;

                case 4:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.QuiteTheSense".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.StaresHappily".Localized());
                    }

                    break;

                case 5:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.ConversationSteers".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.ExpoundsTopPicks".Localized());
                    }

                    break;

                case 6:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.TooBadTellyIs".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.AbsorbingExposition".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.TrueMerchant".Localized());
                    }

                    break;

                case 7:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.OtherAlices".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.SheReplies".Localized());
                    }

                    break;

                case 8:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.ChimesDemoness".Localized());
                    }

                    break;

                case 9:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.FlickingSpaded".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.AsItHappens".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.LoveCats".Localized());
                    }

                    break;

                case 10:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.SmilesBrightly".Localized());
                    }

                    break;

                case 11:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.MerchantDetails".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.HappyToElucidate".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.HerParents".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.LittleDemonNods".Localized());
                    }

                    break;

                case 12:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.StrangeCreatures".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.ConcernedTone".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.HorrifyingFate".Localized());
                    }

                    break;

                case 13:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.BeingAnAlice".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.SaysWithGrin".Localized());
                    }

                    break;

                case 14:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.StatesLittleGirl".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.LetsOutChuckle".Localized());
                    }

                    break;

                case 15:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.DiveInto".Localized());
                    }

                    break;

                case 16:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.DoingToday".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.JovialCheer".Localized());
                    }

                    break;

                case 17:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.MagicBroadSubject".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.ExplainsVague".Localized());

                        if (!false)
                        {
                            OutputText("Scenes.Telly.TellyCommunication.BlackAndWhite".Localized());

                            OutputText("Scenes.Telly.TellyCommunication.ClarifiesStruggling".Localized());

                            OutputText("Scenes.Telly.TellyCommunication.WasntTooBad".Localized());
                        }
                    }

                    break;

                case 18:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.NoticesEdge".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.ShesADemon".Localized());

                        ClearButtons();

                        AddNextButton("Common.Button.Share".Localized(), TellyShare);

                        AddNextButton("Common.Button.Explain".Localized(), TellyExplain);

                        return;
                    }

                case 19:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.WandersIntoMission".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.NicePerson".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.OptimisticOutlook".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.RaisingHands".Localized());
                    }

                    break;

                case 20:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.PonderDemoness".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.SeemsHappyTo".Localized());
                    }

                    break;

                case 21:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.DemonShopkeepers".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.TellyExplains".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.NeedlessCurious".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.SheAdmits".Localized());
                    }

                    break;

                case 22:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.SoullessMonsters".Localized((CoC.Instance.Flags.Get(GameFlagIds.CodexEntryImps) > 0 ? "Obviously the imps aren't the same, but w" : "W")));

                        OutputText("Scenes.Telly.TellyCommunication.TellyReplies".Localized());

                        OutputText("Scenes.Telly.TellyCommunication.DulyNoted".Localized());
                    }

                    break;

                case 23:
                    {
                        OutputText("Scenes.Telly.TellyCommunication.FitOfGiggles".Localized());
                    }

                    break;
            }

            TellySurvey();
        }

        public void TellyShare()
        {
            OutputText("Scenes.Telly.TellyCommunication.SatingCuriosity".Localized());

            OutputText("Scenes.Telly.TellyCommunication.BlushSpreads".Localized());

            OutputText("Scenes.Telly.TellyCommunication.QuicklyHandsBack".Localized());

            TellySurvey();
        }

        public void TellyExplain()
        {
            OutputText("Scenes.Telly.TellyCommunication.QuellExcitement".Localized());

            OutputText("Scenes.Telly.TellyCommunication.PlacesHands".Localized());

            OutputText("Scenes.Telly.TellyCommunication.MeanToSay".Localized());

            OutputText("Scenes.Telly.TellyCommunication.SaysSurprised".Localized());

            OutputText("Scenes.Telly.TellyCommunication.NoHarmDone".Localized());

            TellySurvey();
        }

        public bool TellyCall(int digiTelly)
        {

            if (digiTelly == uTellyty)
                return false;

            switch (digiTelly)
            {
                case 6:
                    {
                        return false;
                    }

                case 8:
                    {
                        return CoC.Instance.Flags.Get(GameFlagIds.LiddelliumFlag) > 0 && Player.IsChild();
                    }

                case 9:
                    {
                        return false;
                    }

                case 10:
                    {
                        return TellyGram > 2;
                    }

                case 12:
                    {
                        return CoC.Instance.Flags.Get(GameFlagIds.MetKitsunes) > 0;
                    }

                case 13:
                    {
                        return CoC.Instance.Flags.Get(GameFlagIds.AliceChats) != 0 && false;
                    }

                case 18:
                    {
                        return Player.HasKeyItem("Hentai Comic");
                    }

                case 20:
                    {
                        return TellyPlasmed;
                    }

                case 21:
                    {
                        return TellyGenesis;
                    }

                case 23:
                    {
                        return TellyGram > 22;
                    }
            }

            return true;
        }

        public bool AffectionaTelly(bool TellyCall = true)
        {

            var TellyGuidance = false;

            if (TellyCall)
                ClearButtons();

            if (Player.HasItem(UseableIds.TeddyBear) && !TellyCom(TellyComType.TellyComB))
                if (TellyCall)
                    AddNextButton("Common.Button.GiftBear".Localized(), CompassionaTelly);
                else
                    TellyGuidance = true;

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.KitsuneGift)) && !TellyCom(TellyComType.TellyComK) && false)
                if (TellyCall)
                    AddNextButton("Common.Button.KitGift".Localized(), TellyDoscope);
                else
                    TellyGuidance = true;

            if (Player.HasItem(ItemIds.PurePeach) && !TellyCom(TellyComType.TellyComP))
                if (TellyCall)
                    AddNextButton("Common.Button.Peach".Localized(), SweeTelly);
                else
                    TellyGuidance = true;

            if (Player.HasItem(ItemRegistry.Create(UseableIds.AbyssalShard)) && !TellyCom(TellyComType.TellyComA) && silly)
                if (TellyCall)
                    AddNextButton("Common.Button.AShard".Localized(), TellyStial).Hint("You found a most peculiar artifact in that damnable manor, yet the beauty of this crystal compels you to gift it to the little girl.", "Abyssal Shard");
                else
                    TellyGuidance = true;

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.DrakeHeart)) && !TellyCom(TellyComType.TellyComD))
                if (TellyCall)
                    AddNextButton("Common.Button.Flower".Localized(), DelicaTelly);
                else
                    TellyGuidance = true;

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.Cupcake)) && !TellyCom(TellyComType.TellyComCC))
                if (TellyCall)
                    AddNextButton("Common.Button.Cupcake".Localized(), DirTelly);
                else
                    TellyGuidance = true;

            AddButton(14, "Common.Button.Back".Localized(), () => TellySurvey());

            return TellyGuidance;
        }

        public void CompassionaTelly()
        {
            ClearOutput();

            TellyCom(TellyComType.TellyComB, true);

            if (silly)
            {
            }

            OutputText("Scenes.Telly.CompassionaTelly.NiceGifts".Localized());

            OutputText("Scenes.Telly.CompassionaTelly.OfferRepair".Localized());

            OutputText("Scenes.Telly.CompassionaTelly.NothingWrong".Localized());

            OutputText("Scenes.Telly.CompassionaTelly.WhoSaysNot".Localized());

            OutputText("Scenes.Telly.CompassionaTelly.BlushesLooksDown".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Hug".Localized(), CompassionaTellyHug);

            AddButton(1, "Common.Button.Makehertake".Localized(), CompassionaTellyForce);
        }

        public void CompassionaTellyHug()
        {
            ClearOutput();

            OutputText("Scenes.Telly.CompassionaTellyHug.GoingAlongRequest".Localized());

            OutputText("Scenes.Telly.CompassionaTellyHug.ThankYouMister".Localized());

            Player.ConsumeItem(UseableIds.TeddyBear);

            DoNext(() => TellyMarket());
        }

        public void CompassionaTellyForce()
        {
            ClearOutput();

            OutputText("Scenes.Telly.CompassionaTellyForce.GodsDamnBitch".Localized());

            OutputText("Scenes.Telly.CompassionaTellyForce.GigglesIncessantly".Localized());

            Player.ConsumeItem(UseableIds.TeddyBear);

            DoNext(() => TellyMarket());
        }

        public void TellyDoscope()
        {
            ClearOutput();

            OutputText("Scenes.Telly.TellyDoscope.PresentsGift".Localized());

            OutputText("Scenes.Telly.TellyDoscope.EyesLightUp".Localized());

            OutputText("Scenes.Telly.TellyDoscope.ReturnsWithDrum".Localized());

            TellyCom(TellyComType.TellyComK, true);

            Player.DestroyItems(ConsumableIds.KitsuneGift, 1);

            ClearButtons();

            AddNextButton("Common.Button.Okay".Localized(), () => TellyDoscopes(true));

            AddNextButton("Common.Button.JustOpen".Localized(), () => TellyDoscopes(false));
        }

        public void TellyDoscopes(bool TellyGuidance)
        {
            ClearOutput();

            if (TellyGuidance)
            {
                OutputText("Scenes.Telly.TellyDoscopes.WouldBeRemiss".Localized());

                OutputText("Scenes.Telly.TellyDoscopes.JoyedShopkeeper".Localized());
            }
            else
            {
                OutputText("Scenes.Telly.TellyDoscopes.FailingToContain".Localized());

                OutputText("Scenes.Telly.TellyDoscopes.AlwaysNeedDrum".Localized());
            }

            OutputText("Scenes.Telly.TellyDoscopes.HandsRest".Localized());

            OutputText("Scenes.Telly.TellyDoscopes.DisorientedBlast".Localized());

            OutputText("Scenes.Telly.TellyDoscopes.ToBeTotally".Localized((Player.Corruption > 66 || Player.Libido > 66 ? "The baser side of you hoped it would be a kitsune aphrodisiac." : "")));

            OutputText("Scenes.Telly.TellyDoscopes.BeforeAnAnswer".Localized());

            OutputText("Scenes.Telly.TellyDoscopes.ExcuseYou".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Fluff".Localized(), () => TellyDoscopic(true));

            AddNextButton("Common.Button.What".Localized(), () => TellyDoscopic(false));
        }

        public void TellyDoscopic(bool TellyGuidance)
        {
            ClearOutput();

            if (TellyGuidance)
            {
                OutputText("Scenes.Telly.TellyDoscopic.NovelRequest".Localized());

                OutputText("Scenes.Telly.TellyDoscopic.GesturesSeat".Localized());

                if (Player.IsTaur())
                {
                    OutputText("Scenes.Telly.TellyDoscopic.NonBiped".Localized());

                    OutputText("Scenes.Telly.TellyDoscopic.SheSays".Localized());

                    OutputText("Scenes.Telly.TellyDoscopic.HopAndSkip".Localized((Player.Tallness > 64 ? "leaps onto the counter" : "wanders over")));
                }
                else
                    OutputText("Scenes.Telly.TellyDoscopic.SitsAsInstructed".Localized());

                OutputText("Scenes.Telly.TellyDoscopic.SheCommands".Localized());

                OutputText("Scenes.Telly.TellyDoscopic.MaintainingCare".Localized((Player.Tallness > 64 ? "leaps onto the counter" : "wanders over")));

                OutputText("Scenes.Telly.TellyDoscopic.OurVillage".Localized());

                OutputText("Scenes.Telly.TellyDoscopic.HugsKitsune".Localized());

                OutputText("Scenes.Telly.TellyDoscopic.BestFluffer".Localized());

                OutputText("Scenes.Telly.TellyDoscopic.UshersOut".Localized());

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Telly.TellyDoscopic.NothingToFluff".Localized());

                OutputText("Scenes.Telly.TellyDoscopic.RefutesClaim".Localized());

                OutputText("Scenes.Telly.TellyDoscopic.DamnMindKitsunes".Localized());

                OutputText("Scenes.Telly.TellyDoscopic.StompsHer".Localized());

                OutputText("Scenes.Telly.TellyDoscopic.ClosesShop".Localized());

                TellyCommand = (int)Math.Floor((double)(View.Time.Days));

                if (incidenTelly)
                    DoNext(() => TellyHo());
                else
                    Go<BazaarScene>();
            }
        }

        public void SweeTelly()
        {
            ClearOutput();

            OutputText("Scenes.Telly.SweeTelly.LoveOfSweets".Localized());

            OutputText("Scenes.Telly.SweeTelly.RedStripedFruit".Localized());

            OutputText("Scenes.Telly.SweeTelly.VariousDelights".Localized());

            Player.DestroyItems(ItemIds.PurePeach, 1);

            TellyCom(TellyComType.TellyComP, true);

            tasTelly = (int)Math.Floor((double)(View.Time.Days * 100 + View.Time.Hours));

            DoNext(() => TellyMarket());
        }

        public void TellyStial()
        {
            ClearOutput();

            OutputText("Scenes.Telly.TellyStial.TrekAmongDead".Localized());

            OutputText("Scenes.Telly.TellyStial.TakesBlackGem".Localized());

            OutputText("Scenes.Telly.TellyStial.FeelSenseOf".Localized());

            TellyCom(TellyComType.TellyComA, true);

            ;
        }

        public void TellyStially(int TellyGuidance, List<int> TellyCall)
        {
            ClearOutput();

            switch (TellyGuidance)
            {
                case 0:
                    {
                        OutputText("Scenes.Telly.TellyStially.HoldsUp".Localized());

                        OutputText("Scenes.Telly.TellyStially.HorrorContemplates".Localized());

                        OutputText("Scenes.Telly.TellyStially.GlanceAround".Localized());
                    }

                    break;

                case 1:
                    {
                        OutputText("Scenes.Telly.TellyStially.SipsTea".Localized());
                    }

                    break;

                case 2:
                    {
                        OutputText("Scenes.Telly.TellyStially.AsksEntity".Localized());

                        OutputText("Scenes.Telly.TellyStially.TailFlicking".Localized());
                    }

                    break;

                case 3:
                    {
                        OutputText("Scenes.Telly.TellyStially.NothingToGround".Localized());

                        OutputText("Scenes.Telly.TellyStially.AppalledManners".Localized());

                        ;

                        return;
                    }
            }

            TellyCall.Add(TellyGuidance);

            ClearButtons();

            AddNextButton("Scenes.Lumberjack.Button.Drink".Localized(), () => TellyStially(1, TellyCall));

            AddNextButton("Common.Button.Question".Localized(), () => TellyStially(2, TellyCall));

            AddNextButton("Common.Button.Sex".Localized(), () => TellyStially(3, TellyCall));
        }

        public void DelicaTelly()
        {
            ClearOutput();

            OutputText("Scenes.Telly.DelicaTelly.PresentsDrakeHeart".Localized());

            OutputText("Scenes.Telly.DelicaTelly.GaspsDilating".Localized());

            OutputText("Scenes.Telly.DelicaTelly.TwinkleInEyes".Localized());

            OutputText("Scenes.Telly.DelicaTelly.MovesToEmbrace".Localized());

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.DrakeHeart));

            TellyCom(TellyComType.TellyComD, true);

            experimenTelly = (int)Math.Floor((double)(View.Time.Days));

            Player.ApplyStats(new DynStat(GameStats.Corruption, .5f));

            Player.ChangeFatigue(-20);

            DoNext(() => TellyMarket());
        }

        public void DirTelly()
        {
            ClearOutput();

            OutputText("Scenes.Telly.DirTelly.SitsBackHappily".Localized());

            OutputText("Scenes.Telly.DirTelly.DdCupCake".Localized());

            OutputText("Scenes.Telly.DirTelly.LeansInExamines".Localized());

            OutputText("Scenes.Telly.DirTelly.PointsOutCupcake".Localized());

            OutputText("Scenes.Telly.DirTelly.DespiteReservations".Localized());

            OutputText("Scenes.Telly.DirTelly.NeverFinish".Localized());

            Player.DestroyItems(ConsumableIds.Cupcake, 1);

            TellyCom(TellyComType.TellyComCC, true);

            TellyCommand = (int)Math.Floor((double)(View.Time.Days));

            ClearButtons();

            AddNextButton("Common.Button.BiteIn".Localized(), DirTellyBite);

            AddNextButton("Common.Button.LickHer".Localized(), DirTellyLick);

            AddNextButton("Scenes.Lumberjack.Button.Decline".Localized(), DirTellyDecline);
        }

        public void DirTellyBite()
        {
            ClearOutput();

            OutputText("Scenes.Telly.DirTellyBite.SetsHandsDown".Localized());

            OutputText("Scenes.Telly.DirTellyBite.ReciprocatesAndSwipe".Localized());

            Go<BazaarScene>();
        }

        public void DirTellyLick()
        {
            ClearOutput();

            OutputText("Scenes.Telly.DirTellyLick.EyesUpCupcake".Localized());

            OutputText("Scenes.Telly.DirTellyLick.SucksSweetCream".Localized());

            Go<BazaarScene>();
        }

        public void DirTellyDecline()
        {
            ClearOutput();

            OutputText("Scenes.Telly.DirTellyDecline.OnlyACake".Localized());

            OutputText("Scenes.Telly.DirTellyDecline.DeclaresAnotherBite".Localized());

            Go<BazaarScene>();
        }

        public bool TellyGifted()
        {

            if (TellyCom(TellyComType.TellyComB) || TellyCom(TellyComType.TellyComK) || TellyCom(TellyComType.TellyComP) || TellyCom(TellyComType.TellyComA) || TellyCom(TellyComType.TellyComD) || TellyCom(TellyComType.TellyComCC))
                return true;
            else
                return false;
        }

        public void TellySis(bool TellyGuidance)
        {
            ClearOutput();

            immorTelly = DateTime.Now.Year;

            if (TellyGuidance)
                OutputText("Scenes.Telly.TellySis.HerIdeaOf".Localized());
            else
                OutputText("Scenes.Telly.TellySis.CantEvenSay".Localized());

            OutputText("Scenes.Telly.TellySis.GigglesLittle".Localized());

            OutputText("Scenes.Telly.TellySis.GazingOverImage".Localized());

            OutputText("Scenes.Telly.TellySis.CostTwentyFive".Localized());

            OutputText("Scenes.Telly.TellySis.BlinkingFewTimes".Localized());

            OutputText("Scenes.Telly.TellySis.JoltOfAdulation".Localized());

            Player.Gems -= 25;

            Player.ApplyStats(new DynStat(GameStats.Corruption, 3f));

            Player.ChangeFatigue(-20);

            DoNext(() => TellyHo());
        }

        public void EnTellychy()
        {
            ClearOutput();

            immorTelly = DateTime.Now.Year;

            OutputText("Scenes.Telly.EnTellychy.NowYouKnowWhy".Localized((Player.Race == "alice" ? " other" : "")));

            OutputText("Scenes.Telly.EnTellychy.Floatiness".Localized());

            OutputText("Scenes.Telly.EnTellychy.WithdrawingTreat".Localized());

            var __sw0_0 = Player.HighestRaceScore();

            if (__sw0_0 == "alice")
                OutputText("Scenes.Telly.EnTellychy.AliceCandyName".Localized());

            if (__sw0_0 == "dog" || __sw0_0 == "wolf" || __sw0_0 == "fox")
                OutputText("Scenes.Telly.EnTellychy.CanineCandyName".Localized());

            if (__sw0_0 == "kitsune")
                OutputText("Scenes.Telly.EnTellychy.KitsuneCandyName".Localized());

            if (__sw0_0 == "bee")
                OutputText("Scenes.Telly.EnTellychy.BeeCandyName".Localized());

            if (__sw0_0 == "insect")
                OutputText("Scenes.Telly.EnTellychy.InsectCandyName".Localized());

            if (__sw0_0 == "pig")
                OutputText("Scenes.Telly.EnTellychy.PigCandyName".Localized());

            if (__sw0_0 == "harpy")
                OutputText("Scenes.Telly.EnTellychy.HarpyCandyName".Localized());

            if (__sw0_0 == "dryad")
                OutputText("Scenes.Telly.EnTellychy.DryadCandyName".Localized());

            if (__sw0_0 == "goo")
                OutputText("Scenes.Telly.EnTellychy.GooCandyName".Localized());

            if (__sw0_0 == "cat")
                OutputText("Scenes.Telly.EnTellychy.CatCandyName".Localized());

            if (__sw0_0 == "sheep")
                OutputText("Scenes.Telly.EnTellychy.SheepCandyName".Localized());

            if (__sw0_0 != "alice" && __sw0_0 != "dog" && __sw0_0 != "wolf" && __sw0_0 != "fox" && __sw0_0 != "kitsune" && __sw0_0 != "bee" && __sw0_0 != "insect" && __sw0_0 != "pig" && __sw0_0 != "harpy" && __sw0_0 != "dryad" && __sw0_0 != "goo" && __sw0_0 != "cat" && __sw0_0 != "sheep")
                if (Player.Thickness < 75 && Player.Tone < 25)
                    OutputText("Scenes.Telly.EnTellychy.BearHug".Localized(
                         Player.Mf("bear hug", "pretty piggy")
                    ));
                else
                    if (Player.HasPerk(PerkIds.BimboBrains) || Player.HasPerk(PerkIds.FutaFaculties))
                        OutputText("Scenes.Telly.EnTellychy.DitsyDarlingName".Localized());
                    else
                        if (Player.HasPerk(PerkIds.BroBrains))
                            OutputText("Scenes.Telly.EnTellychy.BromanticName".Localized());
                        else
                            OutputText("Scenes.Telly.EnTellychy.HugMe".Localized(
                                 Utils.Utils.RandChoice("hug me", "XOXO", "U rock", "cutie pie", "sweet love", "sweet heart", "Happy Valentine")
                            ));

            OutputText("Scenes.Telly.EnTellychy.IendToken".Localized());

            OutputText("Scenes.Telly.EnTellychy.InscribedUpon".Localized());

            Player.Gems -= 25;

            Player.ApplyStats(new DynStat(GameStats.Corruption, 3f));

            Player.ChangeFatigue(-20);

            DoNext(() => TellyHo());
        }

    }
}