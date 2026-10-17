using MonoCoC.Attributes;
using MonoCoC.Contents.Scenes.Vanilla.Quests;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Npcs;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Configuration;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<EdrynScene>("coc:edryn")]
    public class EdrynScene : Scene, ITimeAware
    {

        public static SaveContent Save => SaveContent(SceneIds.TelAdre.Edryn);

        public NpcDefinition Definition => NpcRegistry.Get(NpcIds.Edryn)!;

        public NpcState State => Player.GetNpcState(Definition);

        private PregnancyStore? _pregnancy;

        public PregnancyStore Pregnancy => _pregnancy ??= NpcPregnancy.StoreFor(Definition, State);

        public bool TimeChange()
        {
            Pregnancy.PregnancyAdvance();

            if (Pregnancy.IsPregnant
                && CoC.Instance.Flags.Get(GameFlagIds.EdrynPregnantAndNotToldPcYet) == 0

                && Pregnancy.Type != PregnancyIds.Taoth)

                CoC.Instance.Flags.Set(GameFlagIds.EdrynPregnancyIncubation, CoC.Instance.Flags.Get(GameFlagIds.EdrynPregnancyIncubation) + 1);

            return false;
        }

        public bool TimeChangeLarge()
        {
            if (Pregnancy.IsPregnant && Pregnancy.Incubation == 0)
                if (Pregnancy.Type == PregnancyIds.Taoth)
                {
                    SceneInstance<UrtaQuestScene>.Get().UrtaAndEdrynGodChildEpilogue();

                    Pregnancy.KnockUpForce(PregnancyLegacyTypes.FromNumeric(CoC.Instance.Flags.Get(GameFlagIds.UrtaFertile)), CoC.Instance.Flags.Get(GameFlagIds.UrtaPregEverybody));

                    CoC.Instance.Flags.Set(GameFlagIds.UrtaFertile, 0);

                    CoC.Instance.Flags.Set(GameFlagIds.UrtaPregEverybody, 0);

                    return true;
                }
                else if (Pregnancy.Type == PregnancyIds.PlayerFather)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.EdrynNumberOfKids, CoC.Instance.Flags.Get(GameFlagIds.EdrynNumberOfKids) + 1);

                    CoC.Instance.Flags.Set(GameFlagIds.EdrynNeedsToTalkAboutKid, 1);

                    Pregnancy.KnockUpForce();
                }

            return false;
        }

        public int edrynHeliaLastThreesomeCheck = 0;

        public string debugName => "Edryn";

        public string debugHint => "";

        public override void Entry()
        {
            EdrynKidEncounter();
        }

        public void Reset()
        {

            Save.SetInt("boys", 0);

            Save.SetInt("girls", 0);

            Save.SetInt("herms", 0);

            Save.SetBool("kidMet", false);
        }

        public int EdrynGetKid()
        {

            int kidGender = 0;

            if (Random.Shared.Next(CoC.Instance.Flags.Get(GameFlagIds.EdrynNumberOfKids)) <= Save.GetInt("boys", 0))
                kidGender = 1;
            else
                kidGender = Random.Shared.Next(Save.GetInt("girls", 0) + Save.GetInt("herms", 0)) <= Save.GetInt("girls", 0) ? 2 : 3;

            RegisterMacro("kid.he", kidGender == 1 ? "he" : "she");

            RegisterMacro("kid.him", kidGender == 1 ? "him" : "her");

            RegisterMacro("kid.his", kidGender == 1 ? "his" : "her");

            RegisterMacro("kid.son", kidGender == 1 ? "son" : "daughter");

            return kidGender;
        }

        public void EdrynSprite(bool nude = false)
        {

        }

        public void EdrynBarTalk()
        {
            ClearOutput();

            EdrynSprite();

            if (!Player.HasStatusEffect(StatusEffectIds.Edryn))
                Player.CreateStatusEffect(StatusEffectIds.Edryn, 0, 0, 0, 0);

            ShowImage("edryn-bar-chat");

            int x = Player.CockThatFits(300);

            if (x < 0)
                x = 0;

            if (SceneInstance<KatherineEmploymentScene>.Get().CanTalkToEdryn())
            {
                SceneInstance<KatherineEmploymentScene>.Get().TalkToEdryn();

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.EdrynNeedsToTalkAboutKid) == 1)
            {
                float kidGender = Random.Shared.Next(2) + 1;

                if (Random.Shared.Next(10) < 2)
                    kidGender = 3;

                OutputText("Scenes.Edryn.EdrynBarTalk.CracksIntoBeautifulSmile".Localized());

                if (kidGender == 1)
                    OutputText("Scenes.Edryn.EdrynBarTalk.SonWord".Localized());
                else
                    if (kidGender == 2)
                        OutputText("Scenes.Edryn.EdrynBarTalk.DaughterWord".Localized());
                    else
                        OutputText("Scenes.Edryn.EdrynBarTalk.HermWord".Localized());

                OutputText("Scenes.Edryn.EdrynBarTalk.WerentInTownBut".Localized());

                if (kidGender == 1)
                    OutputText("Scenes.Edryn.EdrynBarTalk.SonWordAgain".Localized());
                else
                    OutputText("Scenes.Edryn.EdrynBarTalk.DaughterWordAgain".Localized());

                OutputText("Scenes.Edryn.EdrynBarTalk.TryingToStandUp".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EdrynNumberOfKids) == 1)
                {
                    OutputText("Scenes.Edryn.EdrynBarTalk.GoingToHaveTo".Localized());

                    if (kidGender == 1)
                        OutputText("Scenes.Edryn.EdrynBarTalk.HimWord".Localized());
                    else
                        OutputText("Scenes.Edryn.EdrynBarTalk.HerWord".Localized());

                    OutputText("Scenes.Edryn.EdrynBarTalk.RightButHelpYou".Localized());
                }
                else
                {
                    OutputText("Scenes.Edryn.EdrynBarTalk.CantBelieveWeve".Localized(
                        NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.EdrynNumberOfKids))
                    ));

                    if (CoC.Instance.Flags.Get(GameFlagIds.EdrynNumberOfKids) < 3)
                        OutputText("Scenes.Edryn.EdrynBarTalk.MissYouFilling".Localized());
                    else
                        OutputText("Scenes.Edryn.EdrynBarTalk.MakeMePregnantAgain".Localized());
                }

                OutputText("Scenes.Edryn.EdrynBarTalk.SentencePeriod".Localized());

                OutputText("Scenes.Edryn.EdrynBarTalk.StayAndChat".Localized());

                View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));

                CoC.Instance.Flags.Set(GameFlagIds.EdrynNeedsToTalkAboutKid, 0);

                Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());

                return ;
            }
            else
                if (Player.IsPregnant())
                {
                    OutputText("Scenes.Edryn.EdrynBarTalk.SmilesPleasantlyApproach".Localized());

                    OutputText("Scenes.Edryn.EdrynBarTalk.SpotAtHerTable".Localized());

                    PregdrynOffer();

                    return ;
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.EdrynNumberOfKids) > 0)
                    {
                        OutputText("Scenes.Edryn.EdrynBarTalk.GesturesTakeSeat".Localized());

                        if (Player.Cocks.Count == 0)
                        {
                            OutputText("Scenes.Edryn.EdrynBarTalk.LooksDownEyesFixing".Localized());

                            OutputText("Scenes.Edryn.EdrynBarTalk.LooksLikeYouWont".Localized());

                            View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));

                            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());

                            return ;
                        }

                        if (Player.CockArea(x) < 300 && Player.CockArea(x) > 24)
                        {
                            OutputText("Scenes.Edryn.EdrynBarTalk.WinksAsSheLeaves".Localized());

                            OutputText("Scenes.Edryn.EdrynBarTalk.LookDownAndCurse".Localized());

                            PregdrynOffer();

                            return ;
                        }
                        else
                            if (Player.CockArea(x) <= 24)
                            {
                                OutputText("Scenes.Edryn.EdrynBarTalk.LooksDownEyesFixating".Localized());

                                Identifier itype = default;

                                if (CoC.Instance.Flags.Get(GameFlagIds.EdrynGiftCounter) < 2)
                                {
                                    OutputText("Scenes.Edryn.EdrynBarTalk.TakeThisStuff".Localized());

                                    itype = ItemIds.GroPlus;

                                    CoC.Instance.Flags.Set(GameFlagIds.EdrynGiftCounter, CoC.Instance.Flags.Get(GameFlagIds.EdrynGiftCounter) + 1);
                                }
                                else
                                    if (CoC.Instance.Flags.Get(GameFlagIds.EdrynGiftCounter) < 5)
                                    {
                                        OutputText("Scenes.Edryn.EdrynBarTalk.TakeStuffAgain".Localized());

                                        CoC.Instance.Flags.Set(GameFlagIds.EdrynGiftCounter, CoC.Instance.Flags.Get(GameFlagIds.EdrynGiftCounter) + 1);

                                        if (Random.Shared.Next(2) == 0)
                                            itype = ConsumableIds.MinotaurBlood;
                                        else
                                            itype = ConsumableIds.IncubiDraft;
                                    }
                                    else
                                    {
                                        OutputText("Scenes.Edryn.EdrynBarTalk.LoveToHelpYou".Localized());

                                        View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));

                                        Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());

                                        return ;
                                    }

                                Player.Inventory.Add(ItemRegistry.Create(itype), 1);

                                ReturnToCamp(1);

                                return ;
                            }
                            else
                            {
                                OutputText("Scenes.Edryn.EdrynBarTalk.LooksDownAndGasps".Localized());

                                if (CoC.Instance.Flags.Get(GameFlagIds.EdrynGiftCounter) < 2)
                                {
                                    OutputText("Scenes.Edryn.EdrynBarTalk.EyesLightUp".Localized());

                                    CoC.Instance.Flags.Set(GameFlagIds.EdrynGiftCounter, CoC.Instance.Flags.Get(GameFlagIds.EdrynGiftCounter) + 1);

                                    Player.Inventory.Add(ItemRegistry.Create(ItemIds.Reducto), 1);

                                    ReturnToCamp(1);

                                    return ;
                                }
                                else
                                {
                                    OutputText("Scenes.Edryn.EdrynBarTalk.SheSays".Localized());

                                    View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));

                                    Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());

                                    return ;
                                }
                            }
                    }

            OutputText("Scenes.Edryn.EdrynBarTalk.SmilesPleasantlyAgain".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Edryn.EdrynBarTalk.OfferingASpotAgain".Localized());
            else
                OutputText("Scenes.Edryn.EdrynBarTalk.GesturingToNearbyStool".Localized());

            OutputText("Scenes.Edryn.EdrynBarTalk.DespiteTheOddity".Localized());

            OutputText("Scenes.Edryn.EdrynBarTalk.CloseLookAtHer".Localized());

            if (Player.IsTaur() && Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Edryn.EdrynBarTalk.PotentFemaleScentWorks".Localized());

                if (Player.Cocks[0].Id == BodyPartTypes.Horse)
                    OutputText("Scenes.Edryn.EdrynBarTalk.PoursOutOfSheath".Localized());
                else
                    OutputText("Scenes.Edryn.EdrynBarTalk.FillsNearInstantaneously".Localized());

                OutputText("Scenes.Edryn.EdrynBarTalk.NowRockHardBreathe".Localized());

                OutputText("Scenes.Edryn.EdrynBarTalk.SwayingSnorting".Localized(
                    Player.CockHead(),

                    Player.Mf("studs", "hotties")
                ));

                OutputText("Scenes.Edryn.EdrynBarTalk.SoftHandSlidesUnder".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 70f));

                EdrynOffer();

                return ;
            }
            else
                if (Player.Cocks.Count > 0)
                {
                    if (Player.CountCocksOfType("Horse") > 0)
                    {
                        OutputText("Scenes.Edryn.EdrynBarTalk.PotentFemaleScentMakes".Localized());

                        if (Player.Cocks[0].Length > 16)
                            OutputText("Scenes.Edryn.EdrynBarTalk.BarelyRegisterThump".Localized(
                                Appearance.CockNoun(BodyPartTypes.Horse)
                            ));
                        else
                            OutputText("Scenes.Edryn.EdrynBarTalk.SquirmUncomfortably".Localized());

                        OutputText("Scenes.Edryn.EdrynBarTalk.SwayingSnorting".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, 60f));

                        EdrynOffer();

                        return ;
                    }
                    else
                    {
                        OutputText("Scenes.Edryn.EdrynBarTalk.PotentMuskyScentOnly".Localized());

                        if (Player.IsCorruptEnough(50) || Player.StatusEffectValue(StatusEffectIds.Edryn, 0) > 0 || (OptionStore.Instance.Get("lowStandards", 0) > 0))
                        {
                            OutputText("Scenes.Edryn.EdrynBarTalk.MindWondersWhatHer".Localized());

                            OutputText("Scenes.Edryn.EdrynBarTalk.RapidlyDissipatingScent".Localized());

                            if (Player.Tallness < 60)
                                OutputText("Scenes.Edryn.EdrynBarTalk.OnAStool".Localized());

                            OutputText("Scenes.Edryn.EdrynBarTalk.BehindHerFuckingHer".Localized());

                            if (Player.Cocks[0].Id == new Identifier("coc", "tentacle"))
                                OutputText("Scenes.Edryn.EdrynBarTalk.SmilesCoylyWhenYour".Localized());

                            OutputText("Scenes.Edryn.EdrynBarTalk.ParagraphBreak".Localized());

                            Player.ApplyStats(new DynStat(GameStats.Lust, 40f));

                            EdrynOffer();

                            return ;
                        }
                        else
                        {
                            OutputText("Scenes.Edryn.EdrynBarTalk.WaitUntilSheReturns".Localized());

                            View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));

                            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
                        }
                    }
                }
                else
                    if (Player.HasVagina() && Player.LustPercent > 70)
                    {
                        OutputText("Scenes.Edryn.EdrynBarTalk.WetAsYouAre".Localized());

                        View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));

                        Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
                    }
                    else
                    {
                        OutputText("Scenes.Edryn.EdrynBarTalk.PotentMuskyScentAgain".Localized());

                        OutputText("Scenes.Edryn.EdrynBarTalk.WaitHerReturnAgain".Localized());

                        View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));

                        Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
                    }
        }

        public void EdrynOffer()
        {

            float cost = 0;

            switch (Player.StatusEffectValue(StatusEffectIds.Edryn, 0))
            {
                case 0:
                {
                cost = 200;
                }

                break;

                case 1:
                {
                cost = 100;
                }

                break;

                case 2:
                {
                cost = 50;
                }

                break;

                case 3:
                {
                cost = 25;
                }

                break;

                case 4:
                {
                cost = 0;
                }

                break;
            }

            if (true )
                cost = 0;

            int x = Player.CockThatFits(300);

            if (x < 0)
                x = 0;

            if (Player.IsTaur() && Player.Cocks.Count > 0)
            {
                if (Player.CockArea(x) < 24)
                {
                    OutputText("Scenes.Edryn.EdrynOffer.BitSmallForMe".Localized());

                    OutputText("Scenes.Edryn.EdrynOffer.BitDisappointedWith".Localized());

                    View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));

                    Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());

                    return ;
                }

                if (Player.CockArea(x) > 300)
                {
                    OutputText("Scenes.Edryn.EdrynOffer.TooBigForMe".Localized());

                    OutputText("Scenes.Edryn.EdrynOffer.BitDisappointedWithAgain".Localized());

                    View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));

                    Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());

                    return ;
                }

                OutputText("Scenes.Edryn.EdrynOffer.HotToTrot".Localized(
                    Player.Mf("stud", "Miss Hot-And-Bulgy"),

                    Player.Mf("studs", "breeders")
                ));

                OutputText("Scenes.Edryn.EdrynOffer.GroanMoreTurnedOn".Localized());

                if (cost > 0)
                    OutputText("Scenes.Edryn.EdrynOffer.GemsQuestion".Localized(
                        NumberUtils.Num2Text(cost)
                    ));
                else
                    OutputText("Scenes.Edryn.EdrynOffer.HourOfYourTime".Localized());

                OutputText("Scenes.Edryn.EdrynOffer.SayEnd".Localized());

                OutputText("Scenes.Edryn.EdrynOffer.ReleasesYouLooksExpectantly".Localized(
                    Player.CockDescript(x)
                ));

                if (cost > 0)
                    OutputText("Scenes.Edryn.EdrynOffer.PayGemsToFuck".Localized(
                        NumberUtils.Num2Text(cost)
                    ));
                else
                    OutputText("Scenes.Edryn.EdrynOffer.FuckHerAgain".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), EdrynSexSelecter);

                AddButton(1, "Common.Button.No".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
            }
            else
                if (Player.CockTotal() > 0)
                    if (Player.CountCocksOfType("Horse") > 0)
                    {
                        if (Player.CockArea(x) > 300)
                        {
                            OutputText("Scenes.Edryn.EdrynOffer.NeedToBeSmaller".Localized());

                            OutputText("Scenes.Edryn.EdrynOffer.BitDisappointedWithThird".Localized());

                            View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));

                            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());

                            return ;
                        }

                        if (Player.CockArea(x) >= 24)
                        {
                            OutputText("Scenes.Edryn.EdrynOffer.AsksInstinctively".Localized());

                            OutputText("Scenes.Edryn.EdrynOffer.DoesntSeemIrritated".Localized(
                                Player.Mf("Mr.", "Miss")
                            ));

                            if (cost > 0)
                                OutputText("Scenes.Edryn.EdrynOffer.GemsComma".Localized(
                                    NumberUtils.Num2Text(cost)
                                ));
                            else
                                OutputText("Scenes.Edryn.EdrynOffer.HourOfTimeComma".Localized());

                            OutputText("Scenes.Edryn.EdrynOffer.LetYouGetPersonal".Localized());

                            OutputText("Scenes.Edryn.EdrynOffer.StandsUpTheScent".Localized());

                            if (Player.Balls > 0)
                                OutputText("Scenes.Edryn.EdrynOffer.BallsTightAndYour".Localized());

                            OutputText("Scenes.Edryn.EdrynOffer.WillYouAccept".Localized(
                                Player.CockDescript(x)
                            ));

                            if (cost > 0)
                                OutputText("Scenes.Edryn.EdrynOffer.PayGemsAgain".Localized(
                                    NumberUtils.Num2Text(cost)
                                ));
                            else
                                OutputText("Scenes.Edryn.EdrynOffer.FuckHerAgain".Localized());

                            AddButton(0, "Common.Button.Yes".Localized(), EdrynSexSelecter);

                            AddButton(1, "Common.Button.No".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
                        }
                        else
                        {
                            OutputText("Scenes.Edryn.EdrynOffer.OuchIsThatAll".Localized());

                            OutputText("Scenes.Edryn.EdrynOffer.BitDisappointedWithFourth".Localized());

                            View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));

                            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());

                            return ;
                        }
                    }
                    else
                    {
                        if (Player.CockArea(x) > 300)
                        {
                            OutputText("Scenes.Edryn.EdrynOffer.NeedSmallerAgain".Localized());

                            OutputText("Scenes.Edryn.EdrynOffer.BitDisappointedWithFifth".Localized());

                            View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));

                            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());

                            return ;
                        }

                        if (Player.CockArea(x) >= 24)
                        {
                            OutputText("Scenes.Edryn.EdrynOffer.SheTeases".Localized());

                            OutputText("Scenes.Edryn.EdrynOffer.WordsRingTrue".Localized());

                            OutputText("Scenes.Edryn.EdrynOffer.KeepsTalkingMerc".Localized());

                            if (cost > 0)
                                OutputText("Scenes.Edryn.EdrynOffer.GemsPlain".Localized(
                                    NumberUtils.Num2Text(cost)
                                ));
                            else
                                OutputText("Scenes.Edryn.EdrynOffer.HourOfTimePlain".Localized());

                            OutputText("Scenes.Edryn.EdrynOffer.RideOfALifetime".Localized());

                            OutputText("Scenes.Edryn.EdrynOffer.StandsUpAndTurns".Localized());

                            if (cost > 0)
                                OutputText("Scenes.Edryn.EdrynOffer.PayGemsThird".Localized(
                                    NumberUtils.Num2Text(cost)
                                ));

                            AddButton(0, "Common.Button.Yes".Localized(), EdrynSexSelecter);

                            AddButton(1, "Common.Button.No".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
                        }
                        else
                        {
                            OutputText("Scenes.Edryn.EdrynOffer.WowThatsItSorry".Localized());

                            OutputText("Scenes.Edryn.EdrynOffer.BitDisappointedWithSixth".Localized());

                            View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));

                            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
                        }
                    }
        }

        public void EdrynSexSelecter()
        {
            ClearOutput();

            float cost = 0;

            switch (Player.StatusEffectValue(StatusEffectIds.Edryn, 0))
            {
                case 0:
                {
                cost = 200;
                }

                break;

                case 1:
                {
                cost = 100;
                }

                break;

                case 2:
                {
                cost = 50;
                }

                break;

                case 3:
                {
                cost = 25;
                }

                break;

                case 4:
                {
                cost = 0;
                }

                break;
            }

            if (true )
                cost = 0;

            if (cost > Player.Gems)
            {
                OutputText("Scenes.Edryn.EdrynSexSelecter.RealizeCantAfford".Localized());

                View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));

                Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());

                return ;
            }

            Player.Gems -= (int)(cost);

            EdrynFucktroduction();

            Player.ChangeStatusValue(StatusEffectIds.Edryn, 0, 1);
        }

        public void FuckEdrynTaur()
        {
            ClearOutput();

            EdrynSprite(true);

            ShowImage("edryn-fuck-as-taur");

            int x = Player.CockThatFits(300);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Edryn.FuckEdrynTaur.EquineBodyLurches".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Edryn.FuckEdrynTaur.LooksOverShoulderAt".Localized());

            OutputText("Scenes.Edryn.FuckEdrynTaur.RingsArentTooHigh".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Edryn.FuckEdrynTaur.MoansLewdlyRocking".Localized());

            if (Player.Corruption < 75)
                OutputText("Scenes.Edryn.FuckEdrynTaur.RecoilForASecond".Localized());
            else
                OutputText("Scenes.Edryn.FuckEdrynTaur.SmileRealizing".Localized());

            OutputText("Scenes.Edryn.FuckEdrynTaur.TwistsAroundToLook".Localized());

            if (Player.BiggestBreastSize() <= 1)
                OutputText("Scenes.Edryn.FuckEdrynTaur.StrokesYourChest".Localized());
            else
                OutputText("Scenes.Edryn.FuckEdrynTaur.CaressesABreast".Localized());

            OutputText("Scenes.Edryn.FuckEdrynTaur.WhileCaressesSideOf".Localized());

            OutputText("Scenes.Edryn.FuckEdrynTaur.CuntLocksTightly".Localized(
                Player.CockHead(x),

                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText(Player.BallsDescriptLight());
            else
                OutputText("Scenes.Edryn.FuckEdrynTaur.LoinsWord".Localized());

            OutputText("Scenes.Edryn.FuckEdrynTaur.SentencePeriod".Localized());

            OutputText("Scenes.Edryn.FuckEdrynTaur.RipplingAndSqueezing".Localized());

            if (Player.CumQ() < 100)
                OutputText("Scenes.Edryn.FuckEdrynTaur.EvenAfterYouveFinished".Localized());
            else
            {
                OutputText("Scenes.Edryn.FuckEdrynTaur.SqueezingMoreAndMore".Localized());

                if (Player.CumQ() > 250)
                    OutputText("Scenes.Edryn.FuckEdrynTaur.BodyGurglesNoisily".Localized());

                if (Player.CumQ() > 500)
                    OutputText("Scenes.Edryn.FuckEdrynTaur.KeepsGoingBloating".Localized());

                OutputText("Scenes.Edryn.FuckEdrynTaur.CuntKeepsMilking".Localized());
            }

            if (Player.StatusEffectValue(StatusEffectIds.Edryn, 0) <= 1)
                OutputText("Scenes.Edryn.FuckEdrynTaur.PullOutWetPop".Localized());
            else
                if (Player.StatusEffectValue(StatusEffectIds.Edryn, 0) <= 3)
                    OutputText("Scenes.Edryn.FuckEdrynTaur.SighsHappilyLooksDown".Localized());
                else
                    if (Player.StatusEffectValue(StatusEffectIds.Edryn, 0) == 4)
                        OutputText("Scenes.Edryn.FuckEdrynTaur.SighsHappilyAndSays".Localized());
                    else
                        OutputText("Scenes.Edryn.FuckEdrynTaur.SighsContentedly".Localized());

            OutputText("Scenes.Edryn.FuckEdrynTaur.WipeUpRedress".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.Edryn, 0) >= 4)
                EdrynPregChance();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -3f));

            if (Player.Lust < 30)
                Player.Lust = 30;

            ReturnToCamp(1);
        }

        public void FuckEdrynNonTaur()
        {
            ClearOutput();

            EdrynSprite(true);

            ShowImage("edryn-fuck-as-non-taur");

            int x = Player.CockThatFits(300);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Edryn.FuckEdrynNonTaur.WigglesHerAmple".Localized());

            if (Player.Tallness < 48)
                OutputText("Scenes.Edryn.FuckEdrynNonTaur.FaceWord".Localized());
            else
                OutputText("Scenes.Edryn.FuckEdrynNonTaur.BodyWord".Localized());

            OutputText("Scenes.Edryn.FuckEdrynNonTaur.LooksOverShoulderAnd".Localized());

            if (Player.Tallness > 72)
                OutputText("Scenes.Edryn.FuckEdrynNonTaur.DropDownToKnees".Localized());

            OutputText("Scenes.Edryn.FuckEdrynNonTaur.PressingIntoFolds".Localized());

            OutputText("Scenes.Edryn.FuckEdrynNonTaur.GaspYouPullBack".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Edryn.FuckEdrynNonTaur.GrabAStool".Localized());

            OutputText("Scenes.Edryn.FuckEdrynNonTaur.SupportingYour".Localized());

            if (Player.CockArea(x) > 200)
                OutputText("Scenes.Edryn.FuckEdrynNonTaur.HeftyPackageBothHands".Localized());
            else
                OutputText("Scenes.Edryn.FuckEdrynNonTaur.SelfWithYourHand".Localized());

            OutputText("Scenes.Edryn.FuckEdrynNonTaur.GuideTowardsShining".Localized(
                Player.CockDescript(x),

                Appearance.CockNoun(BodyPartTypes.Human)
            ));

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Edryn.FuckEdrynNonTaur.RingOfPrepuceSlipping".Localized());
            else
                OutputText("Scenes.Edryn.FuckEdrynNonTaur.SquirtingBitOfFluid".Localized());

            OutputText("Scenes.Edryn.FuckEdrynNonTaur.FurtherYouPushInside".Localized());

            OutputText("Scenes.Edryn.FuckEdrynNonTaur.WhinniesAndPushesBack".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Edryn.FuckEdrynNonTaur.RelaxesEnoughToAllow".Localized());

            OutputText("Scenes.Edryn.FuckEdrynNonTaur.MoansAndWhinnies".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Edryn.FuckEdrynNonTaur.FeelYouShouldDo".Localized());

            OutputText("Scenes.Edryn.FuckEdrynNonTaur.WhenSheFinallyCums".Localized());

            OutputText("Scenes.Edryn.FuckEdrynNonTaur.EyesRollBack".Localized());

            if (Player.CumQ() < 100)
                OutputText("Scenes.Edryn.FuckEdrynNonTaur.EvenAfterYouveFinished".Localized());
            else
            {
                OutputText("Scenes.Edryn.FuckEdrynNonTaur.SqueezingMoreAndMore".Localized());

                if (Player.CumQ() >= 250)
                    OutputText("Scenes.Edryn.FuckEdrynNonTaur.BodyGurglesNoisily".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Edryn.FuckEdrynNonTaur.KeepsGoingBloating".Localized());

                OutputText("Scenes.Edryn.FuckEdrynNonTaur.CuntKeepsMilking".Localized());
            }

            OutputText("Scenes.Edryn.FuckEdrynNonTaur.EdgesOfYourVision".Localized());

            if (Player.HasKnot(x))
                OutputText("Scenes.Edryn.FuckEdrynNonTaur.TriesToRiseLifting".Localized());
            else
                OutputText("Scenes.Edryn.FuckEdrynNonTaur.SlowlyRisesUpOff".Localized());

            OutputText("Scenes.Edryn.FuckEdrynNonTaur.DamBeingBroken".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.Edryn, 0) <= 1)
                OutputText("Scenes.Edryn.FuckEdrynNonTaur.SighsHappilyLooksDown".Localized());
            else
                if (Player.StatusEffectValue(StatusEffectIds.Edryn, 0) <= 3)
                    OutputText("Scenes.Edryn.FuckEdrynNonTaur.SighsLooksDownAgain".Localized());
                else
                    if (Player.StatusEffectValue(StatusEffectIds.Edryn, 0) == 4)
                        OutputText("Scenes.Edryn.FuckEdrynNonTaur.SighsHappilyAndSays".Localized());
                    else
                        OutputText("Scenes.Edryn.FuckEdrynNonTaur.SighsContentedly".Localized());

            OutputText("Scenes.Edryn.FuckEdrynNonTaur.CleanUpRedress".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.Edryn, 0) >= 4)
                EdrynPregChance();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -3f));

            if (Player.Lust < 30)
                Player.Lust = 30;

            ReturnToCamp(1);
        }

        public bool EdrynBar()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.EdrynNeverSeeAgain) == 0 && View.Time.Hours >= 14 && View.Time.Hours <= 19 && (View.Time.Hours < 17 || CoC.Instance.Flags.Get(GameFlagIds.EdrynNumberOfKids) == 0))
                return true;

            return false;
        }

        public bool EdrynHeliaThreesomePossible()
        {

            if (View.Time.TotalTime == edrynHeliaLastThreesomeCheck || View.Time.TotalTime == -edrynHeliaLastThreesomeCheck)
                return edrynHeliaLastThreesomeCheck > 0;

            edrynHeliaLastThreesomeCheck = (int)(View.Time.TotalTime);

            if (Player.Gender == Gender.Genderless || View.Time.Hours < 14 || View.Time.Hours >= 20 || Random.Shared.Next(2) == 0 || (CoC.Instance.Flags.Get(GameFlagIds.PcPromisedHelMonogamyFucks) == 1 || CoC.Instance.Flags.Get(GameFlagIds.HelFuckbuddy) == 0 && SceneInstance<HelScene>.Get().FollowerHel()))
            {
                edrynHeliaLastThreesomeCheck = -edrynHeliaLastThreesomeCheck;

                return false;
            }

            return true;
        }

        public void HelAppearance()
        {

            OutputText("Scenes.Edryn.HelAppearance.SurpriseYouSeeHel".Localized());
        }

        public void ApproachHelAtZeBitch()
        {
            ClearOutput();

            SceneInstance<HelScene>.Get().SpriteChooser();

            ShowImage("hel-chat-at-bar");

            ClearButtons();

            int x = Player.CockThatFits(300);

            if (x < 0)
                x = 0;

            if (EdrynBar() && Player.StatusEffectValue(StatusEffectIds.Edryn, 0) >= 4 && Player.CockArea(x) < 300 && Player.CockArea(x) > 24 && CoC.Instance.Flags.Get(GameFlagIds.HelEdrynOffer) == 0)
            {
                OutputText("Scenes.Edryn.ApproachHelAtZeBitch.HeliaSaysCoyGrin".Localized());

                if (Player.Femininity < 49)
                    OutputText("Scenes.Edryn.ApproachHelAtZeBitch.HandsomeWord".Localized());
                else
                    OutputText("Scenes.Edryn.ApproachHelAtZeBitch.BeautifulWord".Localized());

                OutputText("Scenes.Edryn.ApproachHelAtZeBitch.BusinessOrPleasure".Localized());

                OutputText("Scenes.Edryn.ApproachHelAtZeBitch.SpendFewMinutesTalking".Localized());

                OutputText("Scenes.Edryn.ApproachHelAtZeBitch.CentauressYouMeanEdryn".Localized());

                OutputText("Scenes.Edryn.ApproachHelAtZeBitch.LaughsSnakingTailUnder".Localized());

                OutputText("Scenes.Edryn.ApproachHelAtZeBitch.SittingWithSalamander".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.HelEdrynOffer, 1);

                AddButton(1, "Common.Button.Edryn3some".Localized(), HelEdrynThreeSomeStartYerEngines);
            }
            else
            {
                OutputText("Scenes.Edryn.ApproachHelAtZeBitch.HeliaCoyGrinAgain".Localized());

                if (Player.Femininity < 49)
                    OutputText("Scenes.Edryn.ApproachHelAtZeBitch.HandsomeWordAgain".Localized());
                else
                    OutputText("Scenes.Edryn.ApproachHelAtZeBitch.BeautifulWordAgain".Localized());

                OutputText("Scenes.Edryn.ApproachHelAtZeBitch.BusinessOrPleasure".Localized());

                OutputText("Scenes.Edryn.ApproachHelAtZeBitch.FewMinutesTalking".Localized());

                if (EdrynBar() && Player.CockArea(x) < 300 && Player.CockArea(x) > 24 && CoC.Instance.Flags.Get(GameFlagIds.HelEdrynOffer) == 1)
                {
                    OutputText("Scenes.Edryn.ApproachHelAtZeBitch.EventuallyHelGivesNod".Localized());

                    AddButton(1, "Common.Button.Edryn3some".Localized(), HelEdrynThreeSomeStartYerEngines);
                }
                else
                {
                    OutputText("Scenes.Edryn.ApproachHelAtZeBitch.EventuallyHelGivesYou".Localized());

                    if (Player.StatusEffectValue(StatusEffectIds.Edryn, 0) < 4)
                        AddButton(1, "Common.Button.Edryn3some".Localized(), null);
                    else
                        if (Player.CockArea(x) > 300)
                            AddButton(1, "Common.Button.Edryn3some".Localized(), null);
                        else
                            if (Player.CockArea(x) < 24)
                                AddButton(1, "Common.Button.Edryn3some".Localized(), null);
                            else
                                AddButton(1, "Common.Button.Edryn3some".Localized(), null);
                }
            }

            AddButton(0, "Common.Button.FoxGirls".Localized(), null );

            AddButton(14, "Common.Button.Leave".Localized(), LeaveHelInZeBitch);
        }

        public void LeaveHelInZeBitch()
        {
            ClearOutput();

            if (EdrynBar() && CoC.Instance.Flags.Get(GameFlagIds.HelEdrynOffer) == 1)
                OutputText("Scenes.Edryn.LeaveHelInZeBitch.DecideAgainstTrying".Localized());
            else
            {
                OutputText("Scenes.Edryn.LeaveHelInZeBitch.BrushHeliasRequest".Localized());

                OutputText("Scenes.Edryn.LeaveHelInZeBitch.SpendRestOfThe".Localized());
            }

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

        public void HelEdrynThreeSomeStartYerEngines()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.EdrynTimesHelThreesomed) == 0)
            {
                OutputText("Scenes.Edryn.HelEdrynThreeSomeStartYerEngines.IdeaFormsInYour".Localized());

                OutputText("Scenes.Edryn.HelEdrynThreeSomeStartYerEngines.GaspsEyebrowsArching".Localized());

                OutputText("Scenes.Edryn.HelEdrynThreeSomeStartYerEngines.SaysHalfSmile".Localized());

                OutputText("Scenes.Edryn.HelEdrynThreeSomeStartYerEngines.IntroduceTheGirls".Localized());

                OutputText("Scenes.Edryn.HelEdrynThreeSomeStartYerEngines.SurprisinglyHelBlushes".Localized());

                OutputText("Scenes.Edryn.HelEdrynThreeSomeStartYerEngines.JustAboutSqueals".Localized());
            }
            else
            {
                OutputText("Scenes.Edryn.HelEdrynThreeSomeStartYerEngines.TakeHelHandAnd".Localized());

                OutputText("Scenes.Edryn.HelEdrynThreeSomeStartYerEngines.NormallyATwosomeGirl".Localized());

                OutputText("Scenes.Edryn.HelEdrynThreeSomeStartYerEngines.ThreeHeadUpstairs".Localized());
            }

            ThreesomeEdrynAndHel();
        }

        public void ThreesomeEdrynAndHel()
        {
            ClearOutput();

            ShowImage("edryn-hel-threesome");

            int x = Player.CockThatFits(300);

            CoC.Instance.Flags.Set(GameFlagIds.EdrynTimesHelThreesomed, CoC.Instance.Flags.Get(GameFlagIds.EdrynTimesHelThreesomed) + 1);

            OutputText("Scenes.Edryn.ThreesomeEdrynAndHel.ArriveInPrivateRoom".Localized());

            OutputText("Scenes.Edryn.ThreesomeEdrynAndHel.HelGetsToWork".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Edryn.ThreesomeEdrynAndHel.AlrightBreakTheGirls".Localized());

            OutputText("Scenes.Edryn.ThreesomeEdrynAndHel.HelRollsHerBack".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Edryn.ThreesomeEdrynAndHel.ClopsBackForcing".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Edryn.ThreesomeEdrynAndHel.SackSlappingWetly".Localized());

            OutputText("Scenes.Edryn.ThreesomeEdrynAndHel.SentencePeriod".Localized());

            OutputText("Scenes.Edryn.ThreesomeEdrynAndHel.AtopCentaurHelScrambles".Localized());

            OutputText("Scenes.Edryn.ThreesomeEdrynAndHel.PullDoubleDuty".Localized());

            OutputText("Scenes.Edryn.ThreesomeEdrynAndHel.DoubleAttackIs".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Edryn.ThreesomeEdrynAndHel.CumAMomentLater".Localized());

            OutputText("Scenes.Edryn.ThreesomeEdrynAndHel.ThreeLayInHeap".Localized());

            OutputText("Scenes.Edryn.ThreesomeEdrynAndHel.WhenSatisfiedYouStumble".Localized());

            OutputText("Scenes.Edryn.ThreesomeEdrynAndHel.AgreesNearlyFalling".Localized());

            Player.Orgasm("Dick");

            SceneInstance<HelFollowerScene>.Get().HelAffection(5);

            ReturnToCamp(1);
        }

        public void FindOutEdrynIsPregnant()
        {
            ClearOutput();

            EdrynSprite();

            OutputText("Scenes.Edryn.FindOutEdrynIsPregnant.LyingDownAtHer".Localized());

            OutputText("Scenes.Edryn.FindOutEdrynIsPregnant.FinallyLooksUp".Localized());

            OutputText("Scenes.Edryn.FindOutEdrynIsPregnant.BlushesFiercelyBlurting".Localized());

            OutputText("Scenes.Edryn.FindOutEdrynIsPregnant.HowDoYouReact".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.ShockedExpression".Localized(), ShockedByEdrynsPregnancy);

            AddButton(1, "Common.Button.Pleased".Localized(), PleasedbyPregdryn);

            if (Player.HasCock())
                AddButton(2, "Common.Button.Aroused".Localized(), ArousedByPregdryn);
            else
                AddButton(2, "Common.Button.Aroused".Localized(), null);
        }

        public void ShockedByEdrynsPregnancy()
        {
            ClearOutput();

            OutputText("Scenes.Edryn.ShockedByEdrynsPregnancy.StammerForAnAnswer".Localized());

            OutputText("Scenes.Edryn.ShockedByEdrynsPregnancy.WhatDoYouDo".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.AcceptIt".Localized(), ShockedByPregdrynThenAccept);

            AddButton(1, "Common.Button.RejectIt".Localized(), BeAnAssholeToPregdryn);
        }

        public void ShockedByPregdrynThenAccept()
        {
            ClearOutput();

            OutputText("Scenes.Edryn.ShockedByPregdrynThenAccept.LeaningForwardGrabHold".Localized());

            OutputText("Scenes.Edryn.ShockedByPregdrynThenAccept.ExhalesSheClimbsUp".Localized());

            PregdrynOffer();
        }

        public void BeAnAssholeToPregdryn()
        {
            ClearOutput();

            OutputText("Scenes.Edryn.BeAnAssholeToPregdryn.LookPanickedCentauress".Localized());

            OutputText("Scenes.Edryn.BeAnAssholeToPregdryn.EveryoneInBarTurns".Localized());

            OutputText("Scenes.Edryn.BeAnAssholeToPregdryn.WillNoLongerSpeak".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.EdrynNeverSeeAgain, 1);

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

        public void PleasedbyPregdryn()
        {
            ClearOutput();

            OutputText("Scenes.Edryn.PleasedbyPregdryn.CrackIntoASmile".Localized());

            OutputText("Scenes.Edryn.PleasedbyPregdryn.PullsBackFromTable".Localized());

            PregdrynOffer();
        }

        public void ArousedByPregdryn()
        {
            ClearOutput();

            OutputText("Scenes.Edryn.ArousedByPregdryn.BreakIntoAGrin".Localized());

            PregdrynOffer();
        }

        public void PregdrynOffer(bool cs = true)
        {
            ClearOutput();

            if (!Player.IsPregnant())
            {
                EdrynSexSelecter();

                return ;
            }

            if (Player.Pregnancy.Incubation < 250)
            {
                OutputText("Scenes.Edryn.PregdrynOffer.StrugglesToMoveWaddling".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Edryn.PregdrynOffer.AllFillInSeconds".Localized());
                else
                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Edryn.PregdrynOffer.FillsSecondsAgain".Localized());

                OutputText("Scenes.Edryn.PregdrynOffer.TotallyDazedByThe".Localized());

                if (Player.Corruption + Player.Libido < 100 || Player.CockTotal() < 1)
                    OutputText("Scenes.Edryn.PregdrynOffer.StruggleNotToReach".Localized());
                else
                {
                    OutputText("Scenes.Edryn.PregdrynOffer.CantStopYourselfGrabbing".Localized());

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Edryn.PregdrynOffer.YourWord".Localized());
                    else
                        OutputText("Scenes.Edryn.PregdrynOffer.AWord".Localized());

                    OutputText("Scenes.Edryn.PregdrynOffer.AndStrokingUnderTable".Localized());
                }

                OutputText("Scenes.Edryn.PregdrynOffer.ParagraphBreak".Localized());

                OutputText("Scenes.Edryn.PregdrynOffer.WouldntNoticeHerReturn".Localized());

                if (Player.Balls > 0)
                    OutputText(Player.BallsDescriptLight());
                else
                    if (Player.Cocks.Exists(c => c.SheathOn))
                        OutputText("Scenes.Edryn.PregdrynOffer.SheathWord".Localized());
                    else
                        if (Player.CockTotal() > 0)
                            OutputText(Player.CockDescript(0));
                        else
                            OutputText("Scenes.Edryn.PregdrynOffer.CrotchWord".Localized());

                OutputText("Scenes.Edryn.PregdrynOffer.ToRouseYouFrom".Localized());
            }
            else
            {
                OutputText("Scenes.Edryn.PregdrynOffer.AsUsualWhenPivots".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Edryn.PregdrynOffer.FillSecondsAgain".Localized());
                else
                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Edryn.PregdrynOffer.FillsSecondsAgain".Localized());

                OutputText("Scenes.Edryn.PregdrynOffer.LittleBitDazed".Localized());

                if (Player.Corruption + Player.Libido < 100 || Player.CockTotal() < 1)
                    OutputText("Scenes.Edryn.PregdrynOffer.StruggleNotToTouch".Localized());
                else
                {
                    OutputText("Scenes.Edryn.PregdrynOffer.CantStopYourselfAgain".Localized());

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Edryn.PregdrynOffer.YourWordAgain".Localized());

                    OutputText("Scenes.Edryn.PregdrynOffer.AWordAgain".Localized());

                    OutputText("Scenes.Edryn.PregdrynOffer.StrokingUnderTable".Localized());
                }

                OutputText("Scenes.Edryn.PregdrynOffer.ParagraphBreakAgain".Localized());

                OutputText("Scenes.Edryn.PregdrynOffer.AsUsualBarelyCognizant".Localized());

                if (Player.Balls > 0)
                    OutputText(Player.BallsDescriptLight());
                else
                    if (Player.Cocks.Exists(c => c.SheathOn))
                        OutputText("Scenes.Edryn.PregdrynOffer.SheathWordAgain".Localized());
                    else
                        if (Player.CockTotal() > 0)
                            OutputText(Player.CockDescript(0));
                        else
                            OutputText("Scenes.Edryn.PregdrynOffer.CrotchWordAgain".Localized());

                OutputText("Scenes.Edryn.PregdrynOffer.OutOfYourDaze".Localized());
            }

            if (Player.CockTotal() == 0)
            {
                OutputText("Scenes.Edryn.PregdrynOffer.LooksDownEyesFixing".Localized());

                OutputText("Scenes.Edryn.PregdrynOffer.LeavesLittleDepressed".Localized());

                View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));

                Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());

                return ;
            }

            int x = Player.CockThatFits(300);

            if (x < 0)
                x = 0;

            if (Player.CockArea(x) >= 24 && Player.CockArea(x) < 300)
            {
                OutputText("Scenes.Edryn.PregdrynOffer.SmilingRadiantlyAsShe".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EdrynNumberOfKids) == 0)
                    OutputText("Scenes.Edryn.PregdrynOffer.StrongerEffectOnYou".Localized());
                else
                    OutputText("Scenes.Edryn.PregdrynOffer.CumJustFromSniffing".Localized());

                OutputText("Scenes.Edryn.PregdrynOffer.TryToDenyIt".Localized());

                OutputText("Scenes.Edryn.PregdrynOffer.CentaurDoesntWaitFor".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 50f));

                OutputText("Scenes.Edryn.PregdrynOffer.HowToHandleThis".Localized());

                ClearButtons();

                OutputText("Scenes.Edryn.PregdrynOffer.CouldHaveGreatPregnant".Localized());

                AddButton(0, "Common.Button.PregFuck".Localized(), FuckPregEdryn);

                if (Player.BiggestCockArea() >= 300)
                {
                    OutputText("Scenes.Edryn.PregdrynOffer.PartOfYouRefuses".Localized());

                    AddButton(1, "Common.Button.Nofiteating".Localized(), JizzFromEatingPregdrynOut);
                }

                OutputText("Scenes.Edryn.PregdrynOffer.CouldGoDownOn".Localized());

                AddButton(2, "Common.Button.EatRutFuck".Localized(), EatEdrynPussyLikeABawss);

                return ;
            }
            else
                if (Player.CockArea(x) > 300)
                {
                    OutputText("Scenes.Edryn.PregdrynOffer.BrushesHandOverAll".Localized());

                    OutputText("Scenes.Edryn.PregdrynOffer.LooksAtYouPleadingly".Localized());

                    OutputText("Scenes.Edryn.PregdrynOffer.ThoughtfulLookCrosses".Localized());

                    OutputText("Scenes.Edryn.PregdrynOffer.ReleasesYouAndClimbs".Localized());

                    JizzFromEatingPregdrynOut();

                    Player.ApplyStats(new DynStat(GameStats.Lust, 50f));

                    return ;
                }
                else
                {
                    OutputText("Scenes.Edryn.PregdrynOffer.GigglesButHandContinues".Localized());

                    OutputText("Scenes.Edryn.PregdrynOffer.LaughsAtTheExpression".Localized());

                    OutputText("Scenes.Edryn.PregdrynOffer.CentaurDoesntWait".Localized());

                    JizzFromEatingPregdrynOut();

                    Player.ApplyStats(new DynStat(GameStats.Lust, 50f));

                    return ;
                }
        }

        public void FuckPregEdryn()
        {
            ClearOutput();

            EdrynSprite(true);

            ShowImage("edryn-preggo-fuck");

            int x = Player.CockThatFits(300);

            if (x < 0)
                x = 0;

            if (!Player.IsTaur())
            {
                OutputText("Scenes.Edryn.FuckPregEdryn.LurchesForwardsAsSoon".Localized());

                if (Player.Tallness < 60)
                    OutputText("Scenes.Edryn.FuckPregEdryn.BouncingAgainstTop".Localized());
                else
                    if (Player.Tallness < 84)
                        OutputText("Scenes.Edryn.FuckPregEdryn.BatteringYourFace".Localized());
                    else
                        OutputText("Scenes.Edryn.FuckPregEdryn.CrushedAgainstChest".Localized());

                OutputText("Scenes.Edryn.FuckPregEdryn.WorthTheBruise".Localized());

                OutputText("Scenes.Edryn.FuckPregEdryn.TearOffArmor".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Edryn.FuckPregEdryn.ChosenWord".Localized());

                OutputText("Scenes.Edryn.FuckPregEdryn.DripsWithAnticipation".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Edryn.FuckPregEdryn.CentaursGashDevours".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Edryn.FuckPregEdryn.HeavyThudsEcho".Localized());

                OutputText("Scenes.Edryn.FuckPregEdryn.GrabHerTail".Localized());

                OutputText("Scenes.Edryn.FuckPregEdryn.SinksDownWholeBody".Localized());

                if (Player.CumQ() < 100)
                    OutputText("Scenes.Edryn.FuckPregEdryn.YourDumpsTheLast".Localized(
                        Player.CockDescript(x)
                    ));
                else
                    if (Player.CumQ() < 500)
                        OutputText("Scenes.Edryn.FuckPregEdryn.YourSpurtsThickFlows".Localized(
                            Player.CockDescript(x)
                        ));
                    else
                        OutputText("Scenes.Edryn.FuckPregEdryn.YourEruptsPours".Localized(
                            Player.CockDescript(x),

                            Player.CockDescript(x)
                        ));

                OutputText("Scenes.Edryn.FuckPregEdryn.ParagraphBreak".Localized());

                OutputText("Scenes.Edryn.FuckPregEdryn.SqueezeOverSizedAss".Localized());

                OutputText("Scenes.Edryn.FuckPregEdryn.RaisesHerBackHalf".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Edryn.FuckPregEdryn.LeansAgainstWall".Localized());

                OutputText("Scenes.Edryn.FuckPregEdryn.AfterGatheringPouches".Localized());
            }
            else
            {
                OutputText("Scenes.Edryn.FuckPregEdryn.SmilesAtYou".Localized());

                OutputText("Scenes.Edryn.FuckPregEdryn.BreakKissAndSuck".Localized(
                    Player.CockDescript(x),

                    Player.CockHead(x)
                ));

                OutputText("Scenes.Edryn.FuckPregEdryn.BitesDownHarder".Localized());

                if (!Player.Cocks.Exists(c => c.SheathOn))
                    OutputText("Scenes.Edryn.FuckPregEdryn.ByTheBase".Localized());
                else
                    OutputText("Scenes.Edryn.FuckPregEdryn.JustAboveYourSheath".Localized());

                OutputText("Scenes.Edryn.FuckPregEdryn.LowerHalfOfBody".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Edryn.FuckPregEdryn.CentauressPullsBack".Localized(
                    Player.Mf("stud", "dear")
                ));

                OutputText("Scenes.Edryn.FuckPregEdryn.GruntFromTheForce".Localized());

                OutputText("Scenes.Edryn.FuckPregEdryn.SlipperySilkenTunnel".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Edryn.FuckPregEdryn.AndBalls".Localized());

                OutputText("Scenes.Edryn.FuckPregEdryn.TwatClutchesSoTightly".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Edryn.FuckPregEdryn.YourIsMilkedFrom".Localized(
                    Player.CockDescript(x)
                ));

                if (!Player.Cocks.Exists(c => c.SheathOn))
                    OutputText("Scenes.Edryn.FuckPregEdryn.BaseWord".Localized());
                else
                    OutputText("Scenes.Edryn.FuckPregEdryn.SheathWord".Localized());

                OutputText("Scenes.Edryn.FuckPregEdryn.CulminatingInSqueeze".Localized(
                    Player.CockHead(x)
                ));

                if (!(Player.CumQ() < 300))
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Edryn.FuckPregEdryn.ProdigiousJismOutput".Localized());
                    else
                    {
                        OutputText("Scenes.Edryn.FuckPregEdryn.BodyCumProduction".Localized());

                        if (Player.CumQ() >= 4000)
                            OutputText("Scenes.Edryn.FuckPregEdryn.TimeYouCalmDown".Localized());
                    }

                OutputText("Scenes.Edryn.FuckPregEdryn.ParagraphBreakAgain".Localized());

                OutputText("Scenes.Edryn.FuckPregEdryn.BreaksTheKiss".Localized());

                if (Player.Tallness > 74)
                    OutputText("Scenes.Edryn.FuckPregEdryn.BigWord".Localized());

                OutputText("Scenes.Edryn.FuckPregEdryn.PlayfullyPushesYouBack".Localized(
                    Appearance.CockNoun(BodyPartTypes.Human)
                ));

                OutputText("Scenes.Edryn.FuckPregEdryn.AfterBothStandUp".Localized());
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -.5f));

            ReturnToCamp(1);
        }

        public void JizzFromEatingPregdrynOut()
        {
            ClearOutput();

            EdrynSprite(true);

            int x = Player.CockThatFits(300);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.ShutTheDoorBehind".Localized());

            OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.SmellIsPotentSexual".Localized());

            if (Player.CockArea(x) < 24)
                OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.SmallDickTwitchLeak".Localized());
            else
                OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.MammothCockDragsGround".Localized());

            OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.UrgeToFindJuicy".Localized());

            OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.SoLostInDesperate".Localized());

            OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.WordsDeafEars".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.EachOfYFragment".Localized());
            else
                OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.YFragment".Localized());

            OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.OurCocksWord".Localized());

            if (Player.CumQ() < 100)
                OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.DripsPrecumOnPillows".Localized());
            else
                if (Player.CumQ() < 500)
                    OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.LeaksPrecumSteady".Localized());
                else
                    OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.PoursEnoughPrecum".Localized());

            OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.PowerfulSmellThe".Localized());

            OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.AfterDarkRoom".Localized());

            OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.SquirmingWrithingEdryn".Localized());

            OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.TremblePullAway".Localized());

            if (Player.IsTaur())
            {
                OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.LegsTwitchWeekly".Localized(
                    Appearance.CockNoun(BodyPartTypes.Human)
                ));

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.PluralS".Localized());

                OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.SentencePeriod".Localized());
            }

            OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.ButtonLikePussyIs".Localized());

            OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.StartledFromSexualFog".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.ContractsAndSpasms".Localized(
                    Player.VaginaDescript(0)
                ));

                if (Player.Vaginas[0].Wetness == 5)
                    OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.SquirtingWord".Localized());
                else
                    OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.LeakingWord".Localized());

                OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.PaleImitationOfEdryn".Localized());
            }

            OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.CumOozesOut".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.EachOfWord".Localized());

            OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.InASteadyStream".Localized());

            if (Player.CumQ() < 1000)
            {
                OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.FlowGetsThicker".Localized());

                if (Player.CumQ() < 3000)
                    OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.LastOfLiquidPleasure".Localized());
                else
                    OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.GreaterAmountsOfLiquid".Localized());
            }

            OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.ParagraphBreak".Localized());

            OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.LaughsHaulsYouUp".Localized());

            OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.NodAndGiveMilk".Localized());

            OutputText("Scenes.Edryn.JizzFromEatingPregdrynOut.ExclaimsYouLeave".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 2f));

            ReturnToCamp(1);
        }

        public void EdrynPregChance()
        {

            if (Player.KnockUpChance(0, 25) && Player.CumQ() >= 250)
            {
                Player.KnockUp(PregnancyIds.PlayerFather, PregnancyIds.CentaurIncubation + 80);

                CoC.Instance.Flags.Set(GameFlagIds.EdrynPregnantAndNotToldPcYet, 0);
            }
        }

        public void EdrynFucktroduction()
        {
            ClearOutput();

            int x = Player.CockThatFits(300);

            if (x < 0)
                x = 0;

            float cost = 0;

            switch (Player.StatusEffectValue(StatusEffectIds.Edryn, 0))
            {
                case 0:
                {
                cost = 200;
                }

                break;

                case 1:
                {
                cost = 100;
                }

                break;

                case 2:
                {
                cost = 50;
                }

                break;

                case 3:
                {
                cost = 25;
                }

                break;

                case 4:
                {
                cost = 0;
                }

                break;
            }

            if (true )
                cost = 0;

            if (!Player.IsTaur())
            {
                if (cost > 0)
                    OutputText("Scenes.Edryn.EdrynFucktroduction.GemsExchangeHands".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.EdrynNumberOfKids) == 0)
                        OutputText("Scenes.Edryn.EdrynFucktroduction.ArmFindsWayBack".Localized());
                    else
                        OutputText("Scenes.Edryn.EdrynFucktroduction.RoundOfChuckles".Localized());

                if (Player.Cocks[0].Id == BodyPartTypes.Horse)
                    OutputText("Scenes.Edryn.EdrynFucktroduction.MakesYouTwitchAche".Localized(
                        Player.CockDescript(x)
                    ));
                else
                    OutputText("Scenes.Edryn.EdrynFucktroduction.OverpoweringStrangeLust".Localized());
            }
            else
            {
                if (cost > 0)
                    OutputText("Scenes.Edryn.EdrynFucktroduction.CoinsExchangeHands".Localized());
                else
                    OutputText("Scenes.Edryn.EdrynFucktroduction.GrabsYouPullingBack".Localized());

                if (Player.Cocks[0].Id == BodyPartTypes.Horse)
                    OutputText("Scenes.Edryn.EdrynFucktroduction.MakingYourTwitchAgain".Localized(
                        Player.CockDescript(x)
                    ));
                else
                    OutputText("Scenes.Edryn.EdrynFucktroduction.OverpoweringStrangeLustAgain".Localized());
            }

            OutputText("Scenes.Edryn.EdrynFucktroduction.HowHaveHer".Localized());

            ClearButtons();

            AddButton(1, "Common.Button.EatHerOut".Localized(), EatEdrynPussyLikeABawss);

            if (Player.IsTaur())
                AddButton(0, "Common.Button.Fuck".Localized(), FuckEdrynTaur);
            else
                AddButton(0, "Common.Button.Fuck".Localized(), FuckEdrynNonTaur);
        }

        public void EatEdrynPussyLikeABawss()
        {
            ClearOutput();

            EdrynSprite(true);

            ShowImage("edryn-eat-her-out");

            var x = Player.CockThatFits(300);

            if (x < 0)
                x = Player.SmallestCockIndex();

            var y = Player.CockThatFits2(300);

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.StartsToSaySomething".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.ItsUnderside".Localized());
            else
                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.TheirUnderside".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.SmileMirthfully".Localized());

            if (Player.IsPregnant())
                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.YouCanTellSince".Localized());
            else
                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.NoteThatSmellSeems".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.BlushingCentaurTeases".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.SmirkingTellHer".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesEatenEdrynPussyRut) == 0)
                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.SmileTuggingCorners".Localized());
            else
                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.LecherousGrinSpreading".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.SentencePeriod".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.TrotsCloserAndSays".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.AtTheFirstLick".Localized());

            if (Player.Body.Get<TonguePart>().Id.Path != "human")
                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.NotEvenBegunTo".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.BustyWomanMoans".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.SwayDizzilyOnYour".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.ItsWord".Localized());
            else
                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.TheyreWord".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.SoFullHardnessBecomes".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.SmashingRightBack".Localized());

            if (Player.Gender != Gender.Male)
                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.CloseEnoughToIt".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.RedLustyHazeStarted".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.AbruptlyWhinnies".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.TrotsAwayWobblyLegs".Localized(
                Player.CockDescript(x)
            ));

            if (y >= 0)
                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.AndYourIntoAsshole".Localized(
                    Player.CockDescript(y)
                ));

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.HerWord".Localized());

            if (Player.IsPregnant())
                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.MilkyTitsSwollen".Localized());
            else
                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.BigTits".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.BounceAroundLudicrously".Localized());

            if (Player.IsPregnant())
                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.AsTheyDripMilk".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.GrabHoldOfGigantic".Localized());

            if (y >= 0)
                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.ClenchingTightness".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.GrowlSavagely".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.WhinePiteously".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.LikeMagicCrushing".Localized());

            if (Player.IsPregnant())
                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.BabyFilledWord".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.MiddleForSupport".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.SlurpsAndSquishes".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.CuntAbruptlySqueezes".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.ArchingBackAllow".Localized(
                Player.CockDescript(x)
            ));

            if (y >= 0)
                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.AndCock".Localized(
                    Player.CockDescript(y)
                ));

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.ToTheWord".Localized());

            if (y >= 0)
                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.IrFragment".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.HiltWord".Localized());

            if (y >= 0)
                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.PluralS".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.BlissfulReliefWashes".Localized());

            if (!Player.IsPregnant())
            {
                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.EmptyWombTakesIn".Localized());

                if (y >= 0)
                    OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.AssInASimilar".Localized());
            }
            else
            {
                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.WombPluggedWithYour".Localized());

                if (y >= 0)
                    OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.ButtholeSimilarTreatment".Localized());

                OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.AfterAllSheMilks".Localized());
            }

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.SlumpDownUponFuzzy".Localized());

            OutputText("Scenes.Edryn.EatEdrynPussyLikeABawss.SquishSquishSquish".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), PostEdrynEatOutRut);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, .25f), new DynStat(GameStats.Sensitivity, -3f));
        }

        public void PostEdrynEatOutRut()
        {
            ClearOutput();

            OutputText("Scenes.Edryn.PostEdrynEatOutRut.WhenYouWakeYour".Localized());

            HideUpDown();

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(20 + Player.Libido / 5)));

            if (Player.StatusEffectValue(StatusEffectIds.Edryn, 0) >= 4)
            {
                EdrynPregChance();

                EdrynPregChance();

                EdrynPregChance();
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesEatenEdrynPussyRut, CoC.Instance.Flags.Get(GameFlagIds.TimesEatenEdrynPussyRut) + 1);

            ReturnToCamp(4);
        }

        public void EdrynKidEncounter()
        {
            ClearOutput();

            var kidGender = EdrynGetKid();

            OutputText("Scenes.Edryn.EdrynKidEncounter.ThroughABusyTel".Localized());

            OutputText("Scenes.Edryn.EdrynKidEncounter.FeelASuddenImpact".Localized());

            OutputText("Scenes.Edryn.EdrynKidEncounter.SidlesUpToYou".Localized());

            OutputText("Scenes.Edryn.EdrynKidEncounter.IsntDisagreeableTo".Localized());

            OutputText("Scenes.Edryn.EdrynKidEncounter.CaughtCompletelyOff".Localized());

            OutputText("Scenes.Edryn.EdrynKidEncounter.SaysOnTheCheek".Localized());

            OutputText("Scenes.Edryn.EdrynKidEncounter.ThatsHowMommyKisses".Localized(
                Player.Mf("[i: Mommy] kisses Daddy", "[i: you] kiss Mommy")
            ));

            OutputText("Scenes.Edryn.EdrynKidEncounter.ChucklesAndTakesHer".Localized());

            OutputText("Scenes.Edryn.EdrynKidEncounter.ChildChimesInLittle".Localized());

            OutputText("Scenes.Edryn.EdrynKidEncounter.WaveToTheCouple".Localized());

            Save.SetBool("kidMet", true);

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

    }
}