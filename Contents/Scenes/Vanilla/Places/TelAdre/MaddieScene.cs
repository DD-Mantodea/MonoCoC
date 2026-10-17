using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using System;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<MaddieScene>("coc:maddie")]
    public class MaddieScene : Scene
    {

        public override void Entry()
        {
            ProcMaddieOneIntro();
        }

        public void ProcMaddieOneIntro()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.MinoChefIntro) == 0)
            {
                OutputText("Scenes.Maddie.ProcMaddieOneIntro.EntersBakery".Localized());

                if (Player.Tallness < 72)
                    OutputText("Scenes.Maddie.ProcMaddieOneIntro.LookingDown".Localized());
                else
                    if (Player.Tallness < 100)
                        OutputText("Scenes.Maddie.ProcMaddieOneIntro.StaringLevelly".Localized());
                    else
                        OutputText("Scenes.Maddie.ProcMaddieOneIntro.GlaringUp".Localized());

                OutputText("Scenes.Maddie.ProcMaddieOneIntro.ReleasesShoulder".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MinoChefIntro, 1);
            }
            else
                OutputText("Scenes.Maddie.ProcMaddieOneIntro.WalksIntoBakery".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), FollowMinotaurIntoBackroom);

            AddButton(1, "Common.Button.No".Localized(), SceneInstance<BakeryScene>.Get().Bakeryuuuuuu);
        }

        public void FollowMinotaurIntoBackroom()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.MinoChefExplainedIngredients) == 0)
            {
                OutputText("Scenes.Maddie.FollowMinotaurIntoBackroom.FollowsBeast".Localized());

                OutputText("Scenes.Maddie.FollowMinotaurIntoBackroom.LooksOver".Localized());

                if (Player.HasPerk(PerkIds.MinotaurCumAddict))
                    OutputText("Scenes.Maddie.FollowMinotaurIntoBackroom.LicksLips".Localized());
                else
                    OutputText("Scenes.Maddie.FollowMinotaurIntoBackroom.BlushesRealizing".Localized());

                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.BeeHoney)) && Player.HasItem(ItemRegistry.Create(ConsumableIds.LustDraft)))
                {
                    AddButton(0, "Common.Button.GiveThem".Localized(), HandOverIngredientsItBeBakingTimeYo);

                    AddButton(14, "Common.Button.Leave".Localized(), NopeAintGotNoneODemSpeculIngredimathings);
                }
                else
                    SceneInstance<BakeryScene>.Get().Bakeryuuuuuu();

                CoC.Instance.Flags.Set(GameFlagIds.MinoChefExplainedIngredients, 1);
            }
            else
            {
                OutputText("Scenes.Maddie.FollowMinotaurIntoBackroom.FollowsChef".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Yes".Localized(), null);

                AddButton(1, "Common.Button.No".Localized(), NopeAintGotNoneODemSpeculIngredimathings);

                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.BeeHoney)) && Player.HasItem(ItemRegistry.Create(ConsumableIds.LustDraft)))
                {
                    AddButton(0, "Common.Button.Yes".Localized(), HandOverIngredientsItBeBakingTimeYo);

                    AddButton(1, "Common.Button.LieNo".Localized(), NopeAintGotNoneODemSpeculIngredimathings);
                }
            }
        }

        public void NopeAintGotNoneODemSpeculIngredimathings()
        {
            ClearOutput();

            OutputText("Scenes.Maddie.NopeAintGotNoneODemSpeculIngredimathings.ChefSighs".Localized());

            OutputText("Scenes.Maddie.NopeAintGotNoneODemSpeculIngredimathings.NoPoint".Localized());

            if (Player.Corruption > 50)
                OutputText("Scenes.Maddie.NopeAintGotNoneODemSpeculIngredimathings.StartingFight".Localized());
            else
                OutputText("Scenes.Maddie.NopeAintGotNoneODemSpeculIngredimathings.OverstayingWelcome".Localized());

            OutputText("Scenes.Maddie.NopeAintGotNoneODemSpeculIngredimathings.YouDepart".Localized());

            SceneInstance<BakeryScene>.Get().Bakeryuuuuuu();
        }

        public void HandOverIngredientsItBeBakingTimeYo()
        {
            ClearOutput();

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.BeeHoney));

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.LustDraft));

            OutputText("Scenes.Maddie.HandOverIngredientsItBeBakingTimeYo.HandsIngredients".Localized());

            OutputText("Scenes.Maddie.HandOverIngredientsItBeBakingTimeYo.ToyDrips".Localized());

            if (Player.HasPerk(PerkIds.MinotaurCumAddict))
                OutputText("Scenes.Maddie.HandOverIngredientsItBeBakingTimeYo.LurchesForward".Localized());

            OutputText("Scenes.Maddie.HandOverIngredientsItBeBakingTimeYo.ParagraphBreak".Localized());

            OutputText("Scenes.Maddie.HandOverIngredientsItBeBakingTimeYo.GrabsWhisk".Localized());

            if (Player.HasPerk(PerkIds.MinotaurCumAddict))
                OutputText("Scenes.Maddie.HandOverIngredientsItBeBakingTimeYo.MouthSalivates".Localized());
            else
                OutputText("Scenes.Maddie.HandOverIngredientsItBeBakingTimeYo.NotSureWant".Localized());

            OutputText("Scenes.Maddie.HandOverIngredientsItBeBakingTimeYo.ParagraphBreakTwo".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Wait".Localized(), WaitForSlutCake);

            if (!Player.HasPerk(PerkIds.MinotaurCumAddict))
                AddButton(1, "Common.Button.SneakOut".Localized(), SneakAwayFromMaddie);
            else
                AddButton(1, "Common.Button.SneakOut".Localized(), null);
        }

        public void SneakAwayFromMaddie()
        {
            ClearOutput();

            OutputText("Scenes.Maddie.SneakAwayFromMaddie.GetsOut".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.MaddieStatus, -2);

            ReturnToCamp(1);
        }

        public void WaitForSlutCake()
        {
            ClearOutput();

            OutputText("Scenes.Maddie.WaitForSlutCake.WalksBackBakery".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Maddie.WaitForSlutCake.AntsyWord".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Maddie.WaitForSlutCake.NervousThing".Localized());
                else
                    OutputText("Scenes.Maddie.WaitForSlutCake.IntriguedThing".Localized());

            OutputText("Scenes.Maddie.WaitForSlutCake.WaitressBrings".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Maddie.WaitForSlutCake.SmellsNormal".Localized());
            else
                OutputText("Scenes.Maddie.WaitForSlutCake.SipsWhileWaits".Localized());

            OutputText("Scenes.Maddie.WaitForSlutCake.FeelsLikeEternity".Localized());

            OutputText("Scenes.Maddie.WaitForSlutCake.WhereCouldHeGo".Localized());

            OutputText("Scenes.Maddie.WaitForSlutCake.TallerThanImprisoned".Localized());

            OutputText("Scenes.Maddie.WaitForSlutCake.ChefStillWearing".Localized());

            OutputText("Scenes.Maddie.WaitForSlutCake.UtterlyShocked".Localized());

            OutputText("Scenes.Maddie.WaitForSlutCake.RunningGoodIdea".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.RunAway".Localized(), RunAwayFromMaddiiiieee);

            AddButton(1, "Common.Button.TryToTalk".Localized(), TalkToMaddie);
        }

        public void RunAwayFromMaddiiiieee()
        {
            ClearOutput();

            OutputText("Scenes.Maddie.RunAwayFromMaddiiiieee.TurnsTail".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.MaddieStatus, -1);

            ReturnToCamp(1);
        }

        public void RunAwayMaddieFollowup()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.MaddieStatus, -2);

            OutputText("Scenes.Maddie.RunAwayMaddieFollowup.ReturnsStrange".Localized());

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

        public void TalkToMaddie()
        {
            ClearOutput();

            OutputText("Scenes.Maddie.TalkToMaddie.SpeaksCalmly".Localized());

            OutputText("Scenes.Maddie.TalkToMaddie.ConfectionLooksDown".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Maddie.TalkToMaddie.OutOfIcing".Localized());
            else
                OutputText("Scenes.Maddie.TalkToMaddie.SoMuchIcing".Localized());

            OutputText("Scenes.Maddie.TalkToMaddie.ShesWhat".Localized());

            if (Player.HasCock())
            {
                int x = Player.CockThatFits(60);

                if (x < 0)
                    x = 0;

                OutputText("Scenes.Maddie.TalkToMaddie.SaysMaddie".Localized());

                if (Player.LustPercent >= 75)
                    OutputText("Scenes.Maddie.TalkToMaddie.RockHardWord".Localized());
                else
                    OutputText("Scenes.Maddie.TalkToMaddie.HardeningWord".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.ShaftWord".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Maddie.TalkToMaddie.SWord".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.SpongeSoftHand".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.CuteyAsks".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Maddie.TalkToMaddie.MustersAuthority".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Maddie.TalkToMaddie.OffhandedlyMention".Localized());
                    else
                        OutputText("Scenes.Maddie.TalkToMaddie.SmirksMention".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.CupcakeRetorts".Localized(
                    NumberUtils.Num2Text((float)(int)Math.Floor(Player.Cocks[x].Length)),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Maddie.TalkToMaddie.HalfMeltedTongue".Localized());

                if (!Player.Cocks.Exists(c => c.SheathOn))
                    OutputText("Scenes.Maddie.TalkToMaddie.EncirclesBase".Localized());
                else
                    OutputText("Scenes.Maddie.TalkToMaddie.PokesProds".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.SyrupyResidue".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Maddie.TalkToMaddie.SuctionHollowing".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Maddie.TalkToMaddie.PushesFurther".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Maddie.TalkToMaddie.CumHeavySpheres".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.SqueezesSuckers".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.MilksYou".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Maddie.TalkToMaddie.EmptyingEveryDrop".Localized(
                        Player.CockDescript(x)
                    ));
                else
                {
                    OutputText("Scenes.Maddie.TalkToMaddie.SkinAbsorbs".Localized());

                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Maddie.TalkToMaddie.SWordAgain".Localized());

                    OutputText("Scenes.Maddie.TalkToMaddie.SentencePeriod".Localized());
                }

                OutputText("Scenes.Maddie.TalkToMaddie.JizzGuzzlingGirl".Localized());

                if (Player.CockTotal() > 1)
                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Maddie.TalkToMaddie.OtherPenis".Localized());
                    else
                        OutputText("Scenes.Maddie.TalkToMaddie.OtherDicks".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.BodyGoesLimp".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.CreationLeansBack".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.OvenWarmedTiles".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.LaterBanner".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.WokenByFurry".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.BeastManFinished".Localized());
            }
            else
            {
                OutputText("Scenes.Maddie.TalkToMaddie.AirheadedPastry".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.NippleForced".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.DeliciousGooey".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.IsntMyIcing".Localized());

                if (Player.Thickness < 60)
                    OutputText("Scenes.Maddie.TalkToMaddie.LookingAwfulThin".Localized());
                else
                    if (Player.Tone >= 70)
                        OutputText("Scenes.Maddie.TalkToMaddie.CarvedLike".Localized());
                    else
                        OutputText("Scenes.Maddie.TalkToMaddie.YoudBetter".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.MmmDontLetIt".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.WeightyBreast".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.RocksBack".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.ExclaimsMaddie".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.ExcitedCupcake".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.PudgyFlounces".Localized());

                if (Player.Thickness < 60 || Player.Tone >= 50)
                    OutputText("Scenes.Maddie.TalkToMaddie.ForwardWord".Localized());
                else
                    OutputText("Scenes.Maddie.TalkToMaddie.MoreForward".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.OverwhelmsYou".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.LaterBannerAgain".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.WokenByFurryAgain".Localized());

                OutputText("Scenes.Maddie.TalkToMaddie.BeastManFinishedAgain".Localized());

                OutputText(Player.ModThickness(100, 10));

            }

            CoC.Instance.Flags.Set(GameFlagIds.MaddieStatus, 3);

            ReturnToCamp(1);
        }

        public void BakeryEpilogue()
        {
            ClearOutput();

            OutputText("Scenes.Maddie.BakeryEpilogue.EntersBakery".Localized());

            OutputText("Scenes.Maddie.BakeryEpilogue.ThanksFigured".Localized());

            OutputText("Scenes.Maddie.BakeryEpilogue.XDashSuffix".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.MaddieStatus, 4);

            SceneInstance<BakeryScene>.Get().Bakeryuuuuuu();
        }

    }
}