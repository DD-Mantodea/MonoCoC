using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Pregnancy;
using System.Collections.Generic;
using MonoCoC.Contents.World;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Plains
{
    [Register<BunnyGirlScene>("coc:bunny")]
    public class BunnyGirlScene : Scene
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
            BunnbunbunMeet();
        }

        public void BunnbunbunMeet()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.MetBunbun) == 0)
            {
                OutputText("Scenes.Bunnygirl.BunnbunbunMeet.AWord".Localized());

                if (Player.Tallness > 64)
                    OutputText("Scenes.Bunnygirl.BunnbunbunMeet.ShortWord".Localized());

                OutputText("Scenes.Bunnygirl.BunnbunbunMeet.BunnyAppearsFromBushes".Localized((noFur ? "smooth, muscular" : "muscular, fur-covered")));

                OutputText("Scenes.Bunnygirl.BunnbunbunMeet.StartingAboveTheBunny".Localized((noFur ? "knees" : "hips")));

                if (Player.Libido > 50)
                    OutputText("Scenes.Bunnygirl.BunnbunbunMeet.WonderWhatSheHides".Localized());
                else
                    if (Player.Corruption < 40)
                        OutputText("Scenes.Bunnygirl.BunnbunbunMeet.ThankfulForModesty".Localized());
                    else
                        OutputText("Scenes.Bunnygirl.BunnbunbunMeet.ConsideringTearingAway".Localized());

                OutputText("Scenes.Bunnygirl.BunnbunbunMeet.ParagraphBreak".Localized());

                OutputText("Scenes.Bunnygirl.BunnbunbunMeet.StillFrozenStaring".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Talk".Localized(), TalkToBunnyBunBun);

                AddButton(1, "Common.Button.RapeHer".Localized(), RapeBunBun);

                SetExitButton("Common.Button.Back".Localized());
            }
            else
            {
                if (Random.Shared.Next(6) == 0 || (Holidays.IsEaster() && Random.Shared.Next(3) == 0))
                {
                    AdjathaEggsplosions();

                    return ;
                }

                OutputText("Scenes.Bunnygirl.BunnbunbunMeet.SpyFamiliarEars".Localized());

                OutputText("Scenes.Bunnygirl.BunnbunbunMeet.LeanForBetterLook".Localized());

                OutputText("Scenes.Bunnygirl.BunnbunbunMeet.ClapsHandsOverMouth".Localized());

                OutputText("Scenes.Bunnygirl.BunnbunbunMeet.WhichBodyPartsQuestion".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 20)));

                ClearButtons();

                AddButton(0, "Common.Button.YourVagina".Localized(), null);

                AddButton(1, "Common.Button.HerVagina".Localized(), null);

                AddButton(2, "69", null);

                AddButton(3, "Common.Button.Layyoureggs".Localized(), null);

                if (Player.HasVagina())
                    AddButton(0, "Common.Button.YourVagina".Localized(), BunbunFucksYourVag);

                if (Player.HasCockThatFits(40))
                    AddButton(1, "Common.Button.HerVagina".Localized(), BunbunGetsFucked);

                if (!Player.IsGenderless())
                    AddButton(2, "69", Bunbun69);

                if (Player.CanOviposit())
                    if (!Player.CanOvipositBee() || Random.Shared.Next(2) == 0)
                        AddButton(3, "Common.Button.Layyoureggs".Localized(), OvipositBunnyEaster);
                    else
                        AddButton(3, "Common.Button.Layyoureggs".Localized(), LayEggsInBunbuns);

                AddButton(4, "Common.Button.YourAss".Localized(), BunbunFucksPCInAss);

                SetExitButton("Common.Button.Back".Localized());
            }
        }

        public void TalkToBunnyBunBun()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.MetBunbun, CoC.Instance.Flags.Get(GameFlagIds.MetBunbun) + 1);

            OutputText("Scenes.Bunnygirl.TalkToBunnyBunBun.IntroduceYourself".Localized());

            OutputText("Scenes.Bunnygirl.TalkToBunnyBunBun.ShrugOnlyAnswer".Localized());

            if (Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.TalkToBunnyBunBun.SighsWipesSweat".Localized());
            else
                OutputText("Scenes.Bunnygirl.TalkToBunnyBunBun.SighsWipesSweatAgain".Localized());

            OutputText("Scenes.Bunnygirl.TalkToBunnyBunBun.RoseColorErupts".Localized());

            OutputText("Scenes.Bunnygirl.TalkToBunnyBunBun.OnHerBackPlaying".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 20)));

            ClearButtons();

            AddButton(0, "Common.Button.YourVagina".Localized(), null);

            AddButton(1, "Common.Button.HerVagina".Localized(), null);

            AddButton(2, "69", null);

            if (Player.HasVagina())
                AddButton(0, "Common.Button.YourVagina".Localized(), BunbunFucksYourVag);

            if (Player.HasCockThatFits(40))
                AddButton(1, "Common.Button.HerVagina".Localized(), BunbunGetsFucked);

            if (!Player.IsGenderless())
                AddButton(2, "69", Bunbun69);

            AddButton(3, "Common.Button.YourAss".Localized(), BunbunFucksPCInAss);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void RapeBunBun()
        {
            ClearOutput();

            if (Player.Speed < 60)
            {
                OutputText("Scenes.Bunnygirl.RapeBunBun.LungeTackle".Localized());

                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.NephilaPinkEgg), 1);

ReturnToCamp(1);
            }
            else
            {
                CoC.Instance.Flags.Set(GameFlagIds.MetBunbun, CoC.Instance.Flags.Get(GameFlagIds.MetBunbun) + 1);

                OutputText("Scenes.Bunnygirl.RapeBunBun.LungeTackleAgain".Localized());

                OutputText("Scenes.Bunnygirl.RapeBunBun.BellyWarmAndWet".Localized());

                OutputText("Scenes.Bunnygirl.RapeBunBun.BunHermFollowsGaze".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 10f), new DynStat(GameStats.Corruption, 3f));

                ClearButtons();

                AddButton(0, "Common.Button.YourVagina".Localized(), null);

                AddButton(1, "Common.Button.HerVagina".Localized(), null);

                AddButton(2, "69", null);

                if (Player.HasVagina())
                    AddButton(0, "Common.Button.YourVagina".Localized(), BunbunFucksYourVag);

                if (Player.HasCockThatFits(40))
                    AddButton(1, "Common.Button.HerVagina".Localized(), BunbunGetsFucked);

                if (!Player.IsGenderless())
                    AddButton(2, "69", Bunbun69);

                AddButton(3, "Common.Button.YourAss".Localized(), BunbunFucksPCInAss);

                SetExitButton("Common.Button.Back".Localized());
            }
        }

        public void BunbunFucksYourVag()
        {
            ClearOutput();

            OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.DisrobeGrinning".Localized());

            if (!Player.Weapon.IsUnarmed())
                OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.AndWeaponWord".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.BunnyLooksOnStroking".Localized(
                 Player.VaginaDescript(0)
            ));

            if (Player.Vaginas[0].Wetness >= 4)
                OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.MoistureLeaksDrizzle".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.ParagraphBreak".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.SwallowHerTip".Localized());

            if (Player.Vaginas[0].Wetness >= 3)
                OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.NotThatNeeded".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.NoMoreEncouragement".Localized());

            if (Player.VaginalCapacity() < 20)
                OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.CloseToBottom".Localized());
            else
                if (Player.VaginalCapacity() < 50)
                    OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.GetToBottom".Localized());
                else
                    OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.DoesntTakeLong".Localized(
                         Player.VaginaDescript(0)
                    ));

            Player.CuntChange(35, true);

            OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.FireInHerEyes".Localized(
                 Player.AssDescript()
            ));

            if (Player.Tallness >= 72)
            {
                OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.SheerWeightGrunt".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.SlowYourDescent".Localized());
                else
                    OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.HandleYourWeight".Localized());
            }
            else
                OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.BodiesClapSquish".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.HowMuchPreCum".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.GrabBunnyHips".Localized(
                 Player.VaginaDescript(0)
            ));

            if (Player.GetClitLength() >= 4)
                OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.ClitBouncesBelly".Localized());

            if (Player.BiggestLactation() >= 1)
                OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.MilkBeads".Localized(
                     Appearance.AllBreastsDescript(Player)
                ));

            OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.EagerGirlMoans".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.EggsKnockYouUp".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.EyesCrossThickness".Localized(
                 Player.VaginaDescript(0)
            ));

            Player.CuntChange(70, true);

            OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.ParagraphBreakThird".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.ThrashingChanting".Localized(
                 Player.VaginaDescript(0)
            ));

            if (Player.GetClitLength() >= 4)
                OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.FondleClitStrokeCock".Localized());
            else
                OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.CaressClitCircle".Localized());

            if (Player.CockTotal() > 0)
            {
                OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.TwitchesAndDrools".Localized(
                     Player.SMultiCockDesc()
                ));

                if (Player.Cocks[0].Length < 10)
                    OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.OnTheBunnyBelly".Localized());
                else
                    if (Player.Cocks[0].Length < 18)
                        OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.BetweenTinyTits".Localized());
                    else
                        if (Player.Cocks[0].Length < 28)
                            OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.OnTheBunnyFace".Localized());
                        else
                            OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.PastTheBunnyHead".Localized());

                OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.FreeHandTendsMale".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.MaleHalfSlatherIt".Localized());
                else
                    OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.MaleHalfSlatherThem".Localized());
            }

            OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.FutaRabbitDickHead".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.GetOffHard".Localized(
                 Player.VaginaDescript(0)
            ));

            if (Player.CockTotal() > 0)
            {
                OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.EachCockBlastsRopes".Localized());

                if (Player.CumQ() > 500)
                    OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.KeepsPumpingCoated".Localized());

                if (Player.CumQ() > 2000)
                    OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.ExcessRiverDownHill".Localized());

                if (Player.CumQ() > 500)
                    OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.SentencePeriod".Localized());
            }

            OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.ParagraphBreakFourth".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.SprawlOutLeaking".Localized());

            if (Player.BunnyScore() < 4)
                OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.GoodToThinkStraight".Localized());
            else
                if (Player.Pregnancy.Incubation > 0)
                    OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.ThinkStraightAgain".Localized());
                else
                    OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.ThinkStraightThird".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksYourVag.WalksAwayPuffyTail".Localized());

            if (Player.BunnyScore() < 4)
                Player.KnockUp(PregnancyIds.Bunny, PregnancyIds.BunnyEggsIncubation);
            else
                Player.KnockUp(PregnancyIds.Bunny, PregnancyIds.BunnyBabyIncubation, 60);

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, -3f));

            Player.Sleeping = true;

            ReturnToCamp(1);
        }

        public void BunbunFucksPCInAss()
        {
            ClearOutput();

            OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.NaughtyIdeaDiscardArmor".Localized(
                 Player.AssDescript()
            ));

            OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.LookOverShoulder".Localized());

            if (Player.AnalCapacity() < 20)
                OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.StretchesWide".Localized());
            else
                if (Player.AnalCapacity() < 40)
                    OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.KeepPausing".Localized());
                else
                {
                    OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.WishItWasBigger".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.GirlWord".Localized());
                    else
                        OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.BitchWord".Localized());

                    OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.HerLapGiggling".Localized());
                }

            Player.ButtChange(35, true);

            OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.ParagraphBreak".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.PartnerGasps".Localized());

            if (Player.Cocks.Count > 0)
                OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.BumpsPastProstate".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.LongEaredSlutNipples".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.PickUpSpeed".Localized());

            if ((Player.HasVagina()) && (Player.Vaginas[0].Wetness >= 3))
                    OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.JuicesRunFrom".Localized(
                         Player.VaginaDescript(0)
                    ));

            OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.FingersFindNipples".Localized());

            if (!Player.HasFuckableNipples())
            {
                OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.PullingAndTugging".Localized());

                if (Player.BiggestLactation() >= 1)
                {
                    OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.SpurtingOut".Localized());

                    if (Player.BiggestLactation() >= 4)
                        OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.SpraysWord".Localized());
                    else
                        OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.DropsWord".Localized());

                    OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.OfMilkWord".Localized());
                }

                OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.SentencePeriod".Localized());
            }
            else
                OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.SlidingInsideThem".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.BunnyCaressSqueeze".Localized(
                 Player.AssDescript()
            ));

            OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.StrokeHerThighs".Localized((noFur ? "smooth skin" : "downy fur")));

            if (Player.CockTotal() > 0)
                OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.StickyPreCumPouring".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.ClenchingDripping".Localized(
                         Player.VaginaDescript(0)
                    ));

            OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.SentencePeriodAgain".Localized());

            if (Player.BiggestLactation() >= 1)
            {
                OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.MilkWord".Localized());

                if (Player.BiggestLactation() < 2)
                    OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.LeaksWord".Localized());
                else
                    if (Player.BiggestLactation() < 4)
                        OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.DripsWord".Localized());
                    else
                        OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.PoursWord".Localized());

                OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.FromYourNipples".Localized());
            }

            OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.WaitEggsConfusion".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.GaspPainKnot".Localized());

            if (Player.CockTotal() > 0)
                OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.SqueezesPastProstate".Localized());
            else
                OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.ArousalReturns".Localized());

            Player.ButtChange(70, true);

            OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.ParagraphBreakThird".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.BulgeReachesTip".Localized());

            if (Player.CockTotal() > 0)
            {
                OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.EruptsInTruth".Localized(
                     Player.SMultiCockDesc()
                ));

                if (Player.CumQ() > 500)
                    OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.SplatteringSeedVolume".Localized());

                if (Player.CumQ() > 2000)
                    OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.RiverOfCum".Localized());

                if (Player.CumQ() > 500)
                    OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.SentencePeriodThird".Localized());
            }

            if (Player.HasVagina())
                OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.ClenchesEmptyNeglected".Localized(
                     Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.ParagraphBreakFourth".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.SlideOffDripping".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.IndeterminateTimeLater".Localized());

            OutputText("Scenes.Bunnygirl.BunbunFucksPCInAss.WalksAwayPuffyTail".Localized());

            Player.StartButtPregnancy(PregnancyIds.Bunny, PregnancyIds.BunnyEggsIncubation, 1);

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            Player.Sleeping = true;

            ReturnToCamp(1);
        }

        public void BunbunGetsFucked()
        {
            ClearOutput();

            var x = Player.CockThatFits(40);

            var y= Player.CockThatFits2(40);

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.DisrobeTossArmor".Localized(
                 Player.CockDescript(x),

                 Player.CockDescript(x)
            ));

            if (y != -1)
                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.AlignCockBackdoor".Localized(
                     Player.CockDescript(y)
                ));
            else
                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.BarelyRestrain".Localized());

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.ParagraphBreak".Localized());

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.NubileEyesCross".Localized(
                 Player.CockHead(x)
            ));

            if (y != -1)
                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.SqueaksPressure".Localized());

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.BreedingHoleRelaxes".Localized());

            if (y != -1)
                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.AlongWithAnus".Localized());

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.AndYourWord".Localized(
                 Player.CockDescript(x)
            ));

            if (y != -1)
                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.AndSinkWord".Localized(
                     Player.CockDescript(y)
                ));
            else
                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.SinksWord".Localized());

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.IntoSlipperyDepths".Localized(
                 Player.Mf("stud", "sexy")
            ));

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.DontMakeSense".Localized());

            if (Player.CockArea(x) >= 25)
                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.BellyBulgesCockShape".Localized());

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.BottomOutSquirt".Localized());

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.NoooooSheCries".Localized());

            if (y != -1)
                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.AssholeLoosens".Localized(
                     Player.CockDescript(y)
                ));
            else
            {
                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.LubricantSplatters".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Bunnygirl.BunbunGetsFucked.BellyAndBalls".Localized());
                else
                    OutputText("Scenes.Bunnygirl.BunbunGetsFucked.AndBelly".Localized());

                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.SoonSticky".Localized());
            }

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.NeverLetsUp".Localized());

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.BallsDisappear".Localized());

            if (y != -1)
                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.AssholeStaysSame".Localized());

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.ParagraphBreakThird".Localized());

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.ConvulsionsManic".Localized());

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.BottomOutBrutal".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.SlappingBalls".Localized());

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.AsYourWord".Localized());

            if (Player.Cocks.Exists(c => c.SheathOn))
                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.SheathWord".Localized());
            else
                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.BaseWord".Localized());

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.RubsInflamedLips".Localized(
                 Player.SMultiCockDesc()
            ));

            if ((Player.CockTotal() == 2 && y == -1) || Player.CockTotal() > 2)
                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.AndOntoWord".Localized());

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.InsensateBreederMilks".Localized());

            if (y != -1)
            {
                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.AssGurgles".Localized());

                if (Player.CumQ() > 500)
                    OutputText("Scenes.Bunnygirl.BunbunGetsFucked.StomachGainsPudge".Localized());

                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.SentencePeriod".Localized());
            }

            if (Player.CumQ() > 500)
                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.KeepCummingBlasts".Localized());

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.WombNeverFills".Localized());

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.ParagraphBreakFourth".Localized());

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.SwollenCockBulge".Localized());

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.FeminineGruntEgg".Localized());

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.AskIfNormal".Localized());

            if (Player.CumQ() > 100)
                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.AnotherRollsIn".Localized());

            if (Player.CumQ() > 250)
                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.OneMoreQuads".Localized());

            var z = 8 + Random.Shared.Next((int)(Player.CumQ() / 100));

            if (z > 50)
                z = 50;

            if (Player.CumQ() > 700)
                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.EggAfterEgg".Localized(
                     z
                ));
            else
                OutputText("Scenes.Bunnygirl.BunbunGetsFucked.SentencePeriodAgain".Localized());

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.MoansContentedly".Localized());

            OutputText("Scenes.Bunnygirl.BunbunGetsFucked.ShrugPickUpEgg".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.NephilaPinkEgg), 1);

ReturnToCamp(1);
        }

        public void Bunbun69()
        {
            ClearOutput();

            if (Player.IsTaur())
            {
                if (Random.Shared.Next(2) == 0)
                {
                    OutputText("Scenes.Bunnygirl.Bunbun69.DeclareIntention".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.WhatQuestion".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.RealizedAbsurdity".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.YoureWeird".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.HopsOff".Localized());

                    Player.Orgasm("Generic");

                    Player.ApplyStats(new DynStat(GameStats.Intelligence, -2f));

                    DoNext(() => ReturnToCamp(1));
                }
                else
                {
                    OutputText("Scenes.Bunnygirl.Bunbun69.DeclareIntentionAgain".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.SheSplutters".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.RealizeAbsurdity".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.LaughingHardShakes".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.EggOnYourFace".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.WipeOffTrotAway".Localized());

                    Player.Orgasm("Generic");

                    Player.ApplyStats(new DynStat(GameStats.Intelligence, -2f));

                    Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.NephilaPinkEgg), 1);

ReturnToCamp(1);
                }

                return ;
            }

            if (Player.CockTotal() > 0)
            {
                if (Holidays.IsEaster())
                {
                    OutputText("Scenes.Bunnygirl.Bunbun69.ShameNotBothFun".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.BunnyAdmiresYourLength".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.SuckAllTheHarder".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.TeasingIllAdvised".Localized((noFur ? "" : " fuzzy")));

                    if (Player.Cocks[0].Length >= 8)
                        OutputText("Scenes.Bunnygirl.Bunbun69.PassesSixInches".Localized());

                    if (Player.Cocks[0].Length >= 12)
                        OutputText("Scenes.Bunnygirl.Bunbun69.CantConcentrate".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.ParagraphBreak".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.ChestRisesRapidly".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.SpryBunnyDizzy".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.BreedingHysteriaJams".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Bunnygirl.Bunbun69.BallsWord".Localized());
                    else
                        OutputText("Scenes.Bunnygirl.Bunbun69.ClitWord".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.WhiskersVibrating".Localized());

                    if (!Player.HasVagina())
                        OutputText("Scenes.Bunnygirl.Bunbun69.ProstateWord".Localized());
                    else
                        OutputText("Scenes.Bunnygirl.Bunbun69.InnerWallsWord".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.OrgasmOverpowersDaze".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.SecondEggDownThroat".Localized());

                    Player.RefillHunger(60);

                    if (Player.Body.Get<TailPart>().Id != BodyPartTypes.Rabbit)
                        if (Player.Body.Get<TailPart>().Id == BodyPartTypes.None)
                            OutputText("Scenes.Bunnygirl.Bunbun69.HeatLowerBack".Localized());
                        else
                            OutputText("Scenes.Bunnygirl.Bunbun69.TailJigglesShrinks".Localized());

                    if (Player.Ears.Id != BodyPartTypes.Bunny)
                    {
                        OutputText("Scenes.Bunnygirl.Bunbun69.TopOfHeadNext".Localized());

                        OutputText("Scenes.Bunnygirl.Bunbun69.EyebrowsDrawnUp".Localized());
                    }

                    OutputText("Scenes.Bunnygirl.Bunbun69.ParagraphBreakAgain".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.HappySighSlidesOff".Localized());

                    if (Player.CumQ() < 250)
                        OutputText("Scenes.Bunnygirl.Bunbun69.SlightSwell".Localized());
                    else
                        if (Player.CumQ() < 1000)
                            OutputText("Scenes.Bunnygirl.Bunbun69.NoticeableGirth".Localized());
                        else
                            OutputText("Scenes.Bunnygirl.Bunbun69.BloatedObscenely".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.SpunkCalmsEggLaying".Localized());

                    Player.Body.Get<TailPart>().Id = BodyPartTypes.Rabbit;

                    Player.Ears.Id = BodyPartTypes.Bunny;

                    DoNext(() => ReturnToCamp(1));

                    Player.Orgasm("Dick");

                    Player.Orgasm("Lips", false);

                    Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));
                }
                else
                {
                    OutputText("Scenes.Bunnygirl.Bunbun69.NotBothFunAgain".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.AdmiresYourLengthAgain".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.SuckAllHarderAgain".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.TeasingIllAdvisedAgain".Localized((noFur ? "" : " fuzzy")));

                    if (Player.Cocks[0].Length >= 8)
                        OutputText("Scenes.Bunnygirl.Bunbun69.PassesSixInchesAgain".Localized());

                    if (Player.Cocks[0].Length >= 12)
                        OutputText("Scenes.Bunnygirl.Bunbun69.CantConcentrateAgain".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.ParagraphBreakThird".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.ChestRisesRapidlyAgain".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.SpryBunnyDizzyAgain".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.EggHysteriaJams".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Bunnygirl.Bunbun69.BallsWordAgain".Localized());
                    else
                        if (Player.HasVagina())
                            OutputText("Scenes.Bunnygirl.Bunbun69.ClitWordAgain".Localized());
                        else
                            OutputText("Scenes.Bunnygirl.Bunbun69.GroinWord".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.WhiskersVibratingAgain".Localized());

                    if (!Player.HasVagina())
                        OutputText("Scenes.Bunnygirl.Bunbun69.ProstateWordAgain".Localized());
                    else
                        OutputText("Scenes.Bunnygirl.Bunbun69.InnerWallsWordAgain".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.OrgasmOverpowersBunnyDaze".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.SecondEggThroatAgain".Localized());

                    Player.RefillHunger(60);

                    if (Player.Body.Get<TailPart>().Id != BodyPartTypes.Rabbit)
                        if (Player.Body.Get<TailPart>().Id == BodyPartTypes.None)
                            OutputText("Scenes.Bunnygirl.Bunbun69.HeatLowerBackAgain".Localized());
                        else
                            OutputText("Scenes.Bunnygirl.Bunbun69.TailJigglesShrinksAgain".Localized());

                    if (Player.Ears.Id != BodyPartTypes.Bunny)
                    {
                        OutputText("Scenes.Bunnygirl.Bunbun69.HeadNextAgain".Localized());

                        OutputText("Scenes.Bunnygirl.Bunbun69.EyebrowsDrawnUpAgain".Localized());
                    }

                    OutputText("Scenes.Bunnygirl.Bunbun69.ParagraphBreakFourth".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.SlidesOffAgain".Localized());

                    if (Player.CumQ() < 250)
                        OutputText("Scenes.Bunnygirl.Bunbun69.SlightSwellAgain".Localized());
                    else
                        if (Player.CumQ() < 1000)
                            OutputText("Scenes.Bunnygirl.Bunbun69.NoticeableGirthAgain".Localized());
                        else
                            OutputText("Scenes.Bunnygirl.Bunbun69.BloatedObscenelyAgain".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.CalmsEggLayingAgain".Localized());

                    if (Player.Ears.Id != BodyPartTypes.Bunny)
                        OutputText("Scenes.Bunnygirl.Bunbun69.NewWord".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.EarsPlayfulTweak".Localized());

                    Player.Body.Get<TailPart>().Id = BodyPartTypes.Rabbit;

                    Player.Ears.Id = BodyPartTypes.Bunny;

                    DoNext(() => ReturnToCamp(1));

                    Player.Orgasm("Lips", false);

                    Player.Orgasm("Dick");

                    Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));
                }
            }
            else
            {
                OutputText("Scenes.Bunnygirl.Bunbun69.PussyShame".Localized());

                OutputText("Scenes.Bunnygirl.Bunbun69.BunnyAdmiresPussy".Localized());

                OutputText("Scenes.Bunnygirl.Bunbun69.SuckHarderPussy".Localized());

                OutputText("Scenes.Bunnygirl.Bunbun69.TeasingIllAdvisedPussy".Localized((noFur ? "" : " fuzzy")));

                OutputText("Scenes.Bunnygirl.Bunbun69.ChestRisesPussy".Localized());

                OutputText("Scenes.Bunnygirl.Bunbun69.SpryBunnyDizzyPussy".Localized());

                OutputText("Scenes.Bunnygirl.Bunbun69.EggHysteriaJamsPussy".Localized());

                OutputText("Scenes.Bunnygirl.Bunbun69.SecondEggPussy".Localized());

                if (Player.Body.Get<TailPart>().Id != BodyPartTypes.Rabbit)
                    if (Player.Body.Get<TailPart>().Id == BodyPartTypes.None)
                        OutputText("Scenes.Bunnygirl.Bunbun69.HeatLowerBackPussy".Localized());
                    else
                        OutputText("Scenes.Bunnygirl.Bunbun69.TailJigglesPussy".Localized());

                if (Player.Ears.Id != BodyPartTypes.Bunny)
                {
                    OutputText("Scenes.Bunnygirl.Bunbun69.HeadNextPussy".Localized());

                    OutputText("Scenes.Bunnygirl.Bunbun69.EyebrowsDrawnPussy".Localized());
                }

                OutputText("Scenes.Bunnygirl.Bunbun69.ParagraphBreakFifth".Localized());

                OutputText("Scenes.Bunnygirl.Bunbun69.HappySighPussy".Localized());

                if (Player.Ears.Id != BodyPartTypes.Bunny)
                    OutputText("Scenes.Bunnygirl.Bunbun69.NewWordAgain".Localized());

                OutputText("Scenes.Bunnygirl.Bunbun69.EarsPlayfulTweakAgain".Localized());

                Player.Body.Get<TailPart>().Id = BodyPartTypes.Rabbit;

                Player.Ears.Id = BodyPartTypes.Bunny;

                DoNext(() => ReturnToCamp(1));

                Player.Orgasm("Vaginal");

                Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));
            }
        }

        public void LayEggsInBunbuns()
        {
            ClearOutput();

            OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.DisplayedLapin".Localized());

            if (Player.Eggs() < 20)
                OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.EagerEggTube".Localized());
            else
                if (Player.Eggs() < 40)
                    OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.DistendedBugPart".Localized());
                else
                    OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.BeeAbdomenDrools".Localized());

            OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.BunnyGaspsNoticing".Localized());

            OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.PullingHerPresses".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.AgainstEmptyCrotch".Localized());
            else
            {
                if (Player.HasCock())
                    OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.AlongsideWord".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.HeftyOvalBalls".Localized());

                OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.SentencePeriod".Localized());
            }

            OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.BracesWithArms".Localized());

            OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.ShudderAdjusting".Localized());

            OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.ForcesAlongShaft".Localized());

            OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.ContinuesWrithing".Localized());

            if ((int)Player.Gender > 0)
            {
                OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.CommaSpace".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.EachCockWord".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.AndWord".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.YourVaginaWord".Localized());

                OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.LatheringBunnyIn".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.SpunkWord".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.AndWordAgain".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.FemcumWord".Localized());
            }

            OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.StuffingWarmHole".Localized());

            OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.AnotherEggAnother".Localized());

            OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.FrontCoatedSpooge".Localized());

            if ((int)Player.Gender > 0)
            {
                OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.AddToTheMess".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.SprayingLongLoad".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.AndWordThird".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.SplashingBallsGirlcum".Localized());

                OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.SentencePeriodAgain".Localized());
            }

            OutputText("Scenes.Bunnygirl.LayEggsInBunbuns.NoMoreToGive".Localized());

            Player.DumpEggs();

            Player.Orgasm("Ovi");

            DoNext(() => ReturnToCamp(1));
        }

        public void OvipositBunnyEaster()
        {
            ClearOutput();

            OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.SuggestEggs".Localized());

            OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.WheresThat".Localized());

            OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.ItsASurprise".Localized());

            OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.ConfusedLookObeys".Localized((noFur ? "" : " and her down-soft fur")));

            OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.GrabHerHips".Localized((noFur ? "bunny" : "furry")));

            if (Player.CanOvipositBee())
                OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.InsectWord".Localized());
            else
                OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.ArachnidWord".Localized());

            OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.ClaimingIncubator".Localized());

            OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.SnickerPatEars".Localized());

            OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.OvipositorSlidesFree".Localized((noFur ? "" : " fuzzy")));

            OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.ChitterExcitedly".Localized((noFur ? "" : " furry")));

            OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.IfShedSettleDown".Localized());

            OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.BackstrokePresses".Localized());

            OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.ProngRipplesDistends".Localized());

            OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.ArmsGiveOut".Localized());

            OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.MusesFade".Localized());

            if (Player.Eggs() < 20)
                OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.EggsDeposited".Localized());
            else
                if (Player.Eggs() < 40)
                {
                    OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.EggsLurchStream".Localized());

                    if (!Player.CanOvipositBee())
                        OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.ArachnidWordAgain".Localized());
                    else
                        OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.InsectileWord".Localized());

                    OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.NotEmptyForceMore".Localized());
                }
                else
                {
                    OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.EggsCrammedForcefully".Localized());

                    if (silly)
                        OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.EnergizerBunny".Localized());
                    else
                        OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.EndlessPregnancyBeast".Localized());

                    OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.ForcingWombStretch".Localized());
                }

            OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.ClimbOffRetracts".Localized());

            OutputText("Scenes.Bunnygirl.OvipositBunnyEaster.HareGoesLimp".Localized());

            Player.DumpEggs();

            Player.Orgasm("Ovi");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void AdjathaEggsplosions()
        {
            ClearOutput();

            OutputText("Scenes.Bunnygirl.AdjathaEggsplosions.TrekInterruptedCry".Localized());

            OutputText("Scenes.Bunnygirl.AdjathaEggsplosions.LyingInValley".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.AdjathaEggsplosions.DuskyWord".Localized());
            else
                OutputText("Scenes.Bunnygirl.AdjathaEggsplosions.ChocolateWord".Localized());

            OutputText("Scenes.Bunnygirl.AdjathaEggsplosions.SkinnedWomanAtopHer".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.AdjathaEggsplosions.TannedWord".Localized());
            else
                OutputText("Scenes.Bunnygirl.AdjathaEggsplosions.CaramelWord".Localized());

            OutputText("Scenes.Bunnygirl.AdjathaEggsplosions.TwoPairsBunnyEars".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.AdjathaEggsplosions.ThinGushingPreCum".Localized());
            else
                OutputText("Scenes.Bunnygirl.AdjathaEggsplosions.ThickSyrupyCream".Localized());

            OutputText("Scenes.Bunnygirl.AdjathaEggsplosions.BunnyGirlTiedUp".Localized());

            OutputText("Scenes.Bunnygirl.AdjathaEggsplosions.TakesNotice".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.AdjathaEggsplosions.OliveWord".Localized());
            else
                OutputText("Scenes.Bunnygirl.AdjathaEggsplosions.CinnamonWord".Localized());

            OutputText("Scenes.Bunnygirl.AdjathaEggsplosions.ShePantsTongueLolling".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.AdjathaEggsplosions.CoffeeWord".Localized());
            else
                OutputText("Scenes.Bunnygirl.AdjathaEggsplosions.ToffeeWord".Localized());

            OutputText("Scenes.Bunnygirl.AdjathaEggsplosions.ImploresGnollLady".Localized());

            OutputText("Scenes.Bunnygirl.AdjathaEggsplosions.SqueezingKnees".Localized());

            OutputText("Scenes.Bunnygirl.AdjathaEggsplosions.WhatWillYouDo".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.FreeHer".Localized(), FreeHerOhGodWhyDidYouDoThis);

            if (Player.HasVagina())
                AddButton(1, "Common.Button.GetEgged".Localized(), GetEggflated);

            if (Player.HasCock())
                if (Player.HasCockThatFits(40))
                    AddButton(2, "Common.Button.FuckHer".Localized(), FuckTheEggBoundBun);
                else
                    OutputText("Scenes.Bunnygirl.AdjathaEggsplosions.TooBigToFuck".Localized());

            SetExitButton("Common.Button.Back".Localized());
        }

        public void FreeHerOhGodWhyDidYouDoThis()
        {
            ClearOutput();

            OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.StepUpInspectBonds".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.SheenOfSweat".Localized());
            else
                OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.ShimmeringGlaze".Localized());

            OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.UrgentLustPulsing".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.FreckledBrown".Localized());
            else
                OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.PuddingRich".Localized());

            OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.AbdomenRounded".Localized());

            if (Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.ConyWord".Localized());

            OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.CockJerksAir".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.BubblingPearls".Localized());
            else
                OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.GooeyMarshmallows".Localized());

            OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.PreDribblingCrest".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.CarnalFruit".Localized());
            else
                OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.CandiedPlenty".Localized());

            OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.BitesLowerLip".Localized());

            OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.BendingForwardRopes".Localized());

            OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.BeforeRingLands".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.SplatteringSpoo".Localized());
            else
                OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.ClimaxingCream".Localized());

            OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.ShoweringSpray".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.BrightPinkEggs".Localized());
            else
                OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.CandyCoatedConfections".Localized());

            OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.IssueForthAmid".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.CumWord".Localized());
            else
                OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.IcingWord".Localized());

            OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.GeyserPlopping".Localized());

            OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.GaspingSquealing".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.PertWord".Localized());
            else
                OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.GumDropWord".Localized());

            OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.ToesDigFurrows".Localized());

            OutputText("Scenes.Bunnygirl.FreeHerOhGodWhyDidYouDoThis.FranticPaceEbbs".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f));

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.NephilaPinkEgg), 1);

ReturnToCamp(1);
        }

        public void FuckTheEggBoundBun()
        {
            ClearOutput();

            OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.GiftWrappedTreat".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.SheenOfSweat".Localized());
            else
                OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.ShimmeringGlaze".Localized());

            OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.UrgentLustPulsing".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.FreckledBrown".Localized());
            else
                OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.PuddingRich".Localized());

            OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.AbdomenRounded".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.BubblingPearls".Localized());
            else
                OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.GooeyMarshmallows".Localized());

            OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.PreDribblingCrest".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.CarnalFruit".Localized());
            else
                OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.CandiedPlenty".Localized());

            OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.BitesLowerLip".Localized());

            OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.PlantYourselfFront".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.DuskyWord".Localized());
            else
                OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.CaramelSkinnedWord".Localized());

            OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.BunnyGirlEasySmile".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.PluralSCocks".Localized());

            OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.FreeHandsOnHips".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.SuppleMounds".Localized());
            else
                OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.TautDough".Localized());

            OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.WigglingAss".Localized());

            OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.BegsNotMoreEggs".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.YieldingFlesh".Localized());
            else
                OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.SemiSpongyHotcakes".Localized());

            OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.LightProddingTight".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.GrimaceVulgarGoo".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.SwallowGulpSpoo".Localized());
                else
                    OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.RevelProfaneStimulation".Localized());

            OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.SentencePeriod".Localized());

            OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.GruntForceYourself".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.DarklyFreckledThighs".Localized());
            else
                OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.SavoryChocolateThighs".Localized());

            OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.ReachingThreshold".Localized());

            OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.OrgasmOneCockErupts".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.FloodingSemenGorgedCunt".Localized());
            else
                OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.GlazingCreamFilledCenter".Localized());

            OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.BucksHelplesslyGut".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.SteadyStreamsSeeping".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.BellyButtonOutward".Localized());
                else
                    OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.GrowingLargerCumSplatters".Localized());

            OutputText("Scenes.Bunnygirl.FuckTheEggBoundBun.NoSoonerSpunk".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void GetEggflated()
        {
            ClearOutput();

            OutputText("Scenes.Bunnygirl.GetEggflated.TooCruelToLeave".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.SheenOfSweat".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.ShimmeringGlaze".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.UrgentLustPulsing".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.FreckledBrown".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.PuddingRich".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.AbdomenRounded".Localized());

            if (Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.ConyWord".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.CockJerksAir".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.BubblingPearls".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.GooeyMarshmallows".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.PreDribblingCrest".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.CarnalFruit".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.CandiedPlenty".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.BitesLowerLip".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.SettleInFront".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.TwitchingLength".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.SucculentDelicacy".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.ThoughtOfLetting".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.RingWord".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.DonutWord".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.AroundConicalCock".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.PreCumWord".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.MilkyCream".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.LoopPassageSimple".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.StepForwardPress".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.FrothyJuices".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.SlickHoney".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.SpearHeadSlipsIn".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.ThickPole".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.SweetShaft".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.RipplesHotCold".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.SwollenMast".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.BloatedBaster".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.PressingBackwards".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.SuppleWord".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.MarshmallowWord".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.TestesPliantSurface".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.WellPaddedBedding".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.SpongeCakesGiant".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.SentencePeriod".Localized());

            Player.CuntChange(28, true, true, false);

            OutputText("Scenes.Bunnygirl.GetEggflated.AtopYourPerch".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.GurglingSeed".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.SaccharineSeed".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.OnlyOneThingLeft".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.TwitchingTuft".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.CottonCandyPoof".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.FluffyTailJunction".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.SwollenDick".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.CandiedCock".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.DistortsUnderHeft".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.JizzBurbling".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.JellyBubbling".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.CrestPressesCervix".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.TinglingLiquidPassion".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.FatteningCreamFilling".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.AnotherEggRushes".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.EggAfterEggFills".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.LoadWord".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.TreatWord".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.PantDifficultBreath".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Bunnygirl.GetEggflated.SpasmsMessyOrgasm".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.VisionSwimsBloats".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.AdvancedPregnancy".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.GluttonousExcess".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.CushioningBallsEmptied".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.CumWord".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.CreamWord".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.LeakingStuffedSnatch".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.EggLoaded".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.CandyFattened".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.WeightOnConicalCock".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Bunnygirl.GetEggflated.SteadyOrbsMovement".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.TimeFlowsFading".Localized());

            if (!Holidays.IsEaster())
                OutputText("Scenes.Bunnygirl.GetEggflated.BunnySeed".Localized());
            else
                OutputText("Scenes.Bunnygirl.GetEggflated.MarshmallowAndChocolate".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.LoadsUpBelly".Localized());

            OutputText("Scenes.Bunnygirl.GetEggflated.ParagraphBreak".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Bunnygirl.GetEggflated.ExhaustedFindStrength".Localized((Player.Cocks.Count > 1 ? "" : "s"), (Player.Cocks.Count > 1 ? "their" : "its")));

            OutputText("Scenes.Bunnygirl.GetEggflated.EnoughEggsMelt".Localized());

            if (Player.BunnyScore() < 4)
                Player.KnockUp(PregnancyIds.Bunny, PregnancyIds.BunnyEggsIncubation);
            else
            {
                Player.KnockUp(PregnancyIds.Bunny, PregnancyIds.BunnyBabyIncubation, 60);

                Player.Fertility += 1;
            }

            OutputText(Player.ModThickness(100, 3));

            OutputText(Player.ModTone(0, 3));

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, -3f));

            ReturnToCamp(1);
        }

    }
}