using MonoCoC.Contents.Creatures;
using System.Text.RegularExpressions;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Lake
{
    [Register<FetishCultistScene>("coc:fetish_cultist")]
    public class FetishCultistScene : Scene
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
            ClearOutput();

            if (!Player.HasStatusEffect(StatusEffectIds.FetishOn))
            {
                Player.CreateStatusEffect(StatusEffectIds.FetishOn);

                OutputText("Scenes.Fetishcultistscene.ExecEncounter.AlongTheLakeShore".Localized());

                OutputText("Scenes.Fetishcultistscene.ExecEncounter.UponReachingShore".Localized());

                OutputText("Scenes.Fetishcultistscene.ExecEncounter.BeginToFeel".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Fetishcultistscene.ExecEncounter.LargelyHumanLooking".Localized());

                OutputText("Scenes.Fetishcultistscene.ExecEncounter.LostHerGrasp".Localized());

                var cultist = EnemyRegistry.Create(EnemyIds.FetishCultist);

                cultist.OnDefeated = hpVictory =>
                {
                    ClearOutput();

                    OutputText(hpVictory

                        ? "Hurt too much to continue controlling her powers, the cultist collapses helplessly."

                        : "Overwhelmed by her lusts, the cultist loses the ability to control herself and collapses.");

                    AddButton(0, "Common.Button.Rape".Localized(), PlayerRapesCultist);

                    if (Player.LactationQ() >= 500 || Player.HasStatusEffect(StatusEffectIds.Feeder))
                        AddButton(1, "Common.Button.Breastfeed".Localized(), FetishCultistHasAMilkFetish);

                    if (Player.HasMultiTails())
                        AddButton(2, "Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());

                    AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
                };

                cultist.OnWon = hpVictory =>
                {
                    if (hpVictory)

                        CombatScene.CleanupAfterCombat();
                    else
                        CultistRapesYou();
                };

                CombatScene.Start(cultist, () => Go<CampScene>());
            }
        }

        public void CultistRapesYou()
        {
            ClearOutput();

            CombatController.Instance!.Context.Enemy.Lust = 1;

            CombatController.Instance!.Context.Enemy.Health = 10;

            var changed = false;

            var changedCock = false;

            var changedBoobs = false;

            var sexed = false;

            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ThoughtsOfFetishes".Localized());

            if (Player.IsTaur() || Player.IsNaga())
            {
                if ((Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Hoofed && Player.Balls > 0 && Player.Cocks.Count > 0 && Random.Shared.Next(4) < 3) && (Player.Cocks[0].Id == BodyPartTypes.Horse))
                    {
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.EntranceDoorToStables".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.AsSheStandsThere".Localized(
                             Player.CockDescript(0)
                        ));

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.AfterMinutesOfKissing".Localized(
                             Player.CockDescript(0)
                        ));

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.JustThinkTeasing".Localized(
                             Player.CockDescript(0)
                        ));

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.CrySoundsMore".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.GirlRegainsSenses".Localized(
                             Player.CockHead()
                        ));

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ClapsAndOvations".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.AttentionQuicklyReturned".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.AnnouncerVoiceResonates".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.AsInstructedMount".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.OnceAgainTheLoud".Localized());

                        if (Player.Armor.Id == ArmorIds.ComfortableClothes)
                            Player.ModArmorName = "bridle bit and saddle set";

                        if (Player.Intelligence <= 10 && Random.Shared.Next(2) == 0)
                            CultistBadEnd();
                        else
                        {
                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FewHoursLaterMind".Localized());

                            if (Player.Armor.Id == ArmorIds.ComfortableClothes)
                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FindYourBackOn".Localized());
                            else
                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.StillWearingThat".Localized());

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.OrdealHasLeftYou".Localized());

                            Player.Orgasm("Dick");

                            Player.ApplyStats(new DynStat(GameStats.Intelligence, -2f));

                            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                            CombatScene.CleanupAfterCombat();
                        }

                        return ;
                    }

                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.LookUpAndFind".Localized());

                if (Player.IsTaur())
                {
                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ShudderAndLookAround".Localized());

                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.SoundOfTheDoor".Localized());

                    if (Player.Tallness > 72)
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.MightyWord".Localized());
                    else
                        if (Player.Tallness < 60)
                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.MiniWord".Localized());

                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.CentaurItsTime".Localized());

                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.SlightWarningTingle".Localized());

                    if (Player.Tallness > 84)
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.EvenHumanPartIs".Localized());
                    else
                        if (Player.Tallness > 54)
                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FeelSoHumiliated".Localized());
                        else
                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ThanksToShortStature".Localized());

                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.AsSheRemovesYour".Localized());

                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.CantDoAnythingBut".Localized());

                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.LayAWhile".Localized());

                    if (Player.HasCock() && !Player.HasVagina())
                    {
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.CatchPiercedGoblin".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FewMinutesPassBy".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.LeftExhaustedNeeding".Localized());
                    }
                    else
                        if (!Player.HasCock() && Player.HasVagina())
                        {
                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.TimePassesNobody".Localized(
                                 Player.Mf("stud.", "mare.")
                            ));

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.LookOverShoulder".Localized());

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FromCornerOfEye".Localized());

                            if (Player.Vaginas.Count > 0 && Player.Vaginas[0].Virgin)
                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.TrailOfBloodDrips".Localized());
                            else
                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.PeriodBreak".Localized());

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.BestialLovemaking".Localized());

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ByEndOfDay".Localized());
                        }
                        else
                            if (Player.HasCock() && Player.HasVagina())
                            {
                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ExoticAppearanceMeans".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FewMinutesLaterHear".Localized());

                                if (Player.Tallness > 54)
                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.PushingReinsDownwards".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.BringsASmallStool".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.AlternatingBetweenSucking".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.EaseYourWayInside".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.SeveralHoursPassBy".Localized());
                            }
                            else
                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.PlaceholderLickMarathon".Localized());

                    if (Player.Armor.Id == ArmorIds.ComfortableClothes)
                        Player.ModArmorName = "bridle bit and saddle set";

                    if (Player.Intelligence <= 10 && Random.Shared.Next(2) == 0)
                        CultistBadEnd();
                    else
                    {
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FewHoursLaterAgain".Localized());

                        if (Player.Armor.Id != ArmorIds.ComfortableClothes)
                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FindBackOnAgain".Localized());
                        else
                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.StillWearingThatAgain".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.OrdealLeftYouAgain".Localized());

                        Player.Orgasm("Generic");

                        Player.ApplyStats(new DynStat(GameStats.Intelligence, -2f));

                        Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                        CombatScene.CleanupAfterCombat();
                    }

                    return ;
                }
                else
                    if (Player.IsNaga())
                    {
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ShudderLookAroundAgain".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.SoundDoorAgain".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.CuteAhYourKeeper".Localized());

                        if (Player.Gender == Gender.Genderless)
                        {
                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.NothingYouCanDo".Localized());

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.TappingHerChin".Localized());

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FeelingOfHerFluids".Localized());

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.BellChimesOpening".Localized());
                        }
                        else
                            if (Player.Gender == Gender.Male)
                            {
                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ToShameYouFeel".Localized());

                                if (Player.Cocks.Count > 1)
                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.EachOfWord".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.PokeOutOfPelvic".Localized());

                                if (Player.Cocks.Count > 1)
                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.EachOfWordAgain".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.AndNothingCanDo".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.TappingHerChinAgain".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FeelingFluidsAgain".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.BellChimesOpeningAgain".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FromBehindTheBars".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.StandsUpAndWaves".Localized());

                                if (Player.Cocks.Count > 1)
                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.EachOfWordThird".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.PokeOutOfCloaca".Localized());

                                if (Player.Cocks.Count > 1)
                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.EachOfWordFourth".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.AndTailBeginPumping".Localized());

                                if (Player.Cocks.Count > 1)
                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.CondomsWord".Localized());
                                else
                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.CondomWord".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.StrappedToYourCocks".Localized());

                                if (Player.Cocks.Count > 1)
                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ThemWord".Localized());
                                else
                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ItWord".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.OutAndReplaces".Localized());

                                if (Player.Cocks.Count > 1)
                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ThemWordPeriod".Localized());
                                else
                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ItWordPeriod".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.MustveComeAtLeast".Localized());
                            }
                            else
                                if (Player.Gender == Gender.Female)
                                {
                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ToShameFeelAgain".Localized(
                                         Player.VaginaDescript(0)
                                    ));

                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.TappingHerChinThird".Localized());

                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FeelingFluidsThird".Localized());
                                }
                                else
                                {
                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.AndWithAShudder".Localized(
                                         Player.VaginaDescript(0)
                                    ));

                                    if (Player.Cocks.Count > 1)
                                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.EachOfWordFifth".Localized());

                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.CocksThrustUpInto".Localized());

                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.TappingHerChinFourth".Localized());

                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FeelingFluidsFourth".Localized());
                                }

                        if (Player.Armor.Id == ArmorIds.ComfortableClothes)
                            Player.ModArmorName = "headdress, necklaces, and many body-chains";

                        if (Player.Intelligence <= 10 && Random.Shared.Next(2) == 0)
                            CultistBadEnd();
                        else
                        {
                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FewHoursLaterThird".Localized());

                            if (Player.Armor.Id != ArmorIds.ComfortableClothes)
                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FindBackOnThird".Localized());
                            else
                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.StillWearingThatThird".Localized());

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.OrdealLeftYouThird".Localized());

                            Player.Orgasm("Generic");

                            Player.ApplyStats(new DynStat(GameStats.Intelligence, -2f));

                            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                            CombatScene.CleanupAfterCombat();
                        }

                        return ;
                    }
                    else
                        if ((Player.Body.Get<FacePart>().Id == BodyPartTypes.Dog) && (Player.Body.Get<SkinPart>().Id == BodyPartTypes.Fur) && (Player.Ears.Id == BodyPartTypes.Dog) && (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Dog) && (Player.Body.Get<TailPart>().Id == BodyPartTypes.Dog))
                        {
                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ShudderLookAroundThird".Localized());

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.SoundDoorThird".Localized());

                            if (Player.HasCock() && !Player.HasVagina())
                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.AndWord".Localized());
                            else
                                if (Player.HasVagina() && !Player.HasCock())
                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.AndWordAgain".Localized());
                                else
                                    if (Player.HasVagina() && Player.HasCock())
                                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.CuntEachCockWord".Localized());

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.WordsFullOf".Localized());

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.MagicalKeeperSays".Localized());

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.HearMistressGive".Localized());

                            if (Player.HasCock() && !Player.HasVagina())
                            {
                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.DontNoticeItImmediately".Localized());

                                if (Player.CockTotal() > 1)
                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.HaveWord".Localized());
                                else
                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.HasWord".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.StartedToBecomeErect".Localized());
                            }

                            if (Player.Armor.Id == ArmorIds.ComfortableClothes)
                                Player.ModArmorName = "black leather collar with a badge on it";

                            if (Player.Intelligence <= 10 && Random.Shared.Next(2) == 0)
                                CultistBadEnd();
                            else
                            {
                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FewHoursLaterFourth".Localized());

                                if (Player.Armor.Id != ArmorIds.ComfortableClothes)
                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FindBackOnFourth".Localized());
                                else
                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.StillWearingThatFourth".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.OrdealLeftYouFourth".Localized());

                                Player.Orgasm();

                                Player.ApplyStats(new DynStat(GameStats.Intelligence, -2f));

                                Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                                CombatScene.CleanupAfterCombat();
                            }

                            return ;
                        }
                        else
                            if ((Player.Body.Get<FacePart>().Id == BodyPartTypes.Cat) && (Player.Body.Get<SkinPart>().Id == BodyPartTypes.Fur) && (Player.Ears.Id == BodyPartTypes.Cat) && (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Cat) && (Player.Body.Get<TailPart>().Id == BodyPartTypes.Cat))
                            {
                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.PlaceholderSluttyKitty".Localized());

                                if (Player.Armor.Id == ArmorIds.ComfortableClothes)
                                    Player.ModArmorName = "black leather collar with a bell";

                                if (Player.Intelligence <= 10 && Random.Shared.Next(2) == 0)
                                    CultistBadEnd();
                                else
                                {
                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FewHoursLaterFifth".Localized());

                                    if (Player.Armor.Id != ArmorIds.ComfortableClothes)
                                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FindBackOnFifth".Localized());
                                    else
                                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.StillWearingThatFifth".Localized());

                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.OrdealLeftYouFifth".Localized());

                                    Player.Orgasm();

                                    Player.ApplyStats(new DynStat(GameStats.Intelligence, -2f));

                                    Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                                    CombatScene.CleanupAfterCombat();
                                }

                                return ;
                            }
            }

            if (Random.Shared.Next(2) == 0 && !sexed)
            {
                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.StandUpLookOver".Localized());

                if (Player.IsTaur())
                {
                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.AlsoDawnsToFogged".Localized());

                    if (Player.CockTotal() > 0)
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.SeeYourIncrediblyLarge".Localized());
                }

                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ParagraphBreak".Localized());

                if (Player.Armor.Id == ArmorIds.ComfortableClothes)
                    Player.ModArmorName = "bondage patient clothes";

                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ArentSupposedToBe".Localized());

                if (Player.HasFuckableNipples())
                {
                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.StillSmilingNurse".Localized(
                         Player.NippleDescript(0),

                         Player.NippleDescript(0),

                         Appearance.AllBreastsDescript(Player)
                    ));

                    if (Player.BiggestLactation() < 1)
                    {
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.StartedToLactate".Localized());

                        Player.BoostLactation(1.5f);
                    }
                    else
                    {
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ProducingMoreMilk".Localized());

                        Player.BoostLactation(.6f);
                    }

                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.SatisfiedNurseRises".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, 3f));

                    changed = true;

                    changedBoobs = true;
                }

                if (Player.Cocks.Count > 0 && !changed)
                {
                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.SmileDisappears".Localized());

                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.BrieflyStepsOut".Localized());

                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.StopsHummingBefore".Localized(
                         Player.CockDescript(0)
                    ));

                    Player.CumMultiplier += 2;

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, 3f));

                    changed = true;

                    changedCock = true;
                }

                if (Player.BiggestBreastSize() > 1 && !changed)
                {
                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.SmileDisappearsAgain".Localized());

                    if (Player.BreastRows.Count == 1)
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.OnceAboutHalf".Localized());

                    if (Player.BreastRows.Count == 2)
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.OnceAboutQuarter".Localized(
                             Appearance.AllBreastsDescript(Player)
                        ));

                    if (Player.BreastRows.Count > 2)
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.OnceAboutSixth".Localized(
                             Appearance.AllBreastsDescript(Player)
                        ));

                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.GlowingPleasureFills".Localized(
                         Appearance.AllBreastsDescript(Player)
                    ));

                    if (Player.BiggestLactation() >= 1)
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.StartLactatingEvenMore".Localized());
                    else
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.StartLactating".Localized());

                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.NoddingApprovingly".Localized());

                    Player.BoostLactation(1.5f);

                    changed = true;

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, 3f));

                    changedBoobs = true;
                }

                if (Player.Vaginas.Count > 0 && !changed)
                {
                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.SmileDisappearsThird".Localized());

                    Player.ClearPregnancy();

                    Player.CreateCock();

                    Player.RemoveVagina(0, 1);

                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FullyFormed".Localized(
                         Player.CockDescript(0),

                         Player.CockDescript(0),

                         Player.CockDescript(0)
                    ));

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, 3f));

                    changed = true;

                    changedCock = true;
                }

                if (Player.Gender == Gender.Genderless && !changed)
                {
                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.SmileDisappearsFourth".Localized());

                    Player.CreateCock();

                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FullyFormedAgain".Localized(
                         Player.CockDescript(0),

                         Player.CockDescript(0),

                         Player.CockDescript(0)
                    ));

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, 3f));

                    changed = true;

                    changedCock = true;
                }

                if (Player.Intelligence <= 10 && Random.Shared.Next(2) == 0)
                    CultistBadEnd();
                else
                {
                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FewHoursPassAgain".Localized());

                    if (Player.Armor.Id != ArmorIds.ComfortableClothes)
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FindBackOnSixth".Localized());
                    else
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.StillWearingThatSixth".Localized());

                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.OrdealLeftYouSixth".Localized());

                    Player.Orgasm("Generic");

                    Player.ApplyStats(new DynStat(GameStats.Intelligence, -2f));

                    Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                    CombatScene.CleanupAfterCombat();

                    if (changedBoobs)
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.NoticeThatChanges".Localized());

                    if (changedCock)
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.NoticeThatChangesAgain".Localized());
                }
            }
            else
                if (!sexed)
                {
                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.OutfitShiftsInto".Localized());

                    if (Player.IsTaur())
                    {
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.DawnsFoggedMind".Localized());

                        if (Player.CockTotal() > 0)
                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.SeeIncrediblyLargeAgain".Localized());
                    }

                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ParagraphBreakAgain".Localized());

                    if (Player.Vaginas.Count > 0 && Player.Cocks.Count > 0)
                    {
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.BecomingANice".Localized(
                             Player.VaginaDescript(0)
                        ));

                        if (Player.Armor.Id == ArmorIds.ComfortableClothes)
                            Player.ModArmorName = "crotch-revealing clothes";

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.SaysToYou".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ReplyWithoutHesitation".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.GladSeeYouRemember".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.EagerToPleaseMistress".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.AfterMinuteOfThis".Localized(
                             Player.VaginaDescript(0)
                        ));

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.HandsSlightlyTrembling".Localized(
                             Player.VaginaDescript(0),

                             Player.CockDescript(0)
                        ));

                        if (Player.Cocks.Count == 2)
                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.RubsAgainstHerSkin".Localized(
                                 Player.CockDescript(1)
                            ));

                        if (Player.Cocks.Count > 2)
                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.RestOfYourRubs".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ExtendedForeplay".Localized());

                        if (Player.Cocks.Count > 1)
                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.TakeSpecialCare".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.MistressRisesOffOf".Localized());
                    }
                    else
                        if (Player.Cocks.Count > 0 && (Player.Vaginas.Count == 0 || Random.Shared.Next(2) == 0))
                        {
                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.SeeCocksHaveBecome".Localized());

                            if (Player.Vaginas.Count != 0)
                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.YourVaginaWord".Localized(
                                     Player.VaginaDescript(0)
                                ));

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.AndYourAssUndergarments".Localized());

                            if (Player.Armor.Id == ArmorIds.ComfortableClothes)
                                Player.ModArmorName = "cute servant's clothes";

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.BoyFeelingAroused".Localized());

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.YesMistress".Localized());

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ImGladSeeYou".Localized());

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.EagerPleaseMistressAgain".Localized());

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.AfterMinuteThisAgain".Localized());

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.CryOfJoy".Localized());

                            if (Player.Vaginas.Count > 0)
                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.DesireStartToWell".Localized(
                                     Player.VaginaDescript(0)
                                ));

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.StartsToRockBack".Localized());

                            if (Player.Cocks.Count == 2)
                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.RubsAgainstSkinAgain".Localized(
                                     Player.CockDescript(1)
                                ));

                            if (Player.Cocks.Count > 2)
                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.RestOfYourRub".Localized());

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ExtendedForeplayAgain".Localized());

                            if (Player.Cocks.Count > 1)
                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.TakeSpecialCareAgain".Localized());

                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.MistressRisesOffAgain".Localized());
                        }
                        else
                            if (Player.Vaginas.Count > 0)
                            {
                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.SeeCocksBecomeAgain".Localized(
                                     Player.VaginaDescript(0)
                                ));

                                if (Player.Cocks.Count > 0)
                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.YourCocksWord".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.AndYourAss".Localized());

                                if (Player.Armor.Id == ArmorIds.ComfortableClothes)
                                    Player.ModArmorName = "maid's clothes";

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.GirlFeelingAroused".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.YesMistressAgain".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ImGladYouAgain".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.EagerPleaseMistressThird".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.AfterMinuteThisThird".Localized(
                                     Player.VaginaDescript(0)
                                ));

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.LayBackOnGround".Localized(
                                     Player.VaginaDescript(0),

                                     Player.VaginaDescript(0)
                                ));

                                if (Player.Cocks.Count > 0)
                                    OutputText("Scenes.Fetishcultistscene.CultistRapesYou.DesireRisingIn".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ExtendedForeplayThen".Localized());

                                OutputText("Scenes.Fetishcultistscene.CultistRapesYou.MistressRisesAndDresses".Localized());
                            }

                    if (Player.Gender == Gender.Genderless)
                    {
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.SeeCocksBecomeThird".Localized());

                        if (Player.Armor.Id == ArmorIds.ComfortableClothes)
                            Player.ModArmorName = "servant's clothes";

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ServantFeelingAroused".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.YesMistressThird".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.ImGladYouThird".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.EagerPleaseMistressFourth".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.AfterMinutesOfKissing".Localized(Player.CockDescript(0)));

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.MistressRisesDressesAgain".Localized());
                    }

                    if (Player.Intelligence <= 10 && Random.Shared.Next(2) == 0)
                        CultistBadEnd();
                    else
                    {
                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FewHoursLaterSixth".Localized());

                        if (Player.Armor.Id != ArmorIds.ComfortableClothes)
                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.FindBackOnSeventh".Localized());
                        else
                            OutputText("Scenes.Fetishcultistscene.CultistRapesYou.StillWearingThatSeventh".Localized());

                        OutputText("Scenes.Fetishcultistscene.CultistRapesYou.OrdealLeftYouSeventh".Localized());

                        Player.Orgasm("Generic");

                        Player.ApplyStats(new DynStat(GameStats.Intelligence, -2f));

                        Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                        CombatScene.CleanupAfterCombat();
                    }
                }
        }

        public void PlayerRapesCultist()
        {
            ClearOutput();

            switch (CombatController.Instance!.Context.Enemy.ArmorName)
            {
                case FetishCultistEnemy.SWIMSUIT:
                {
                RapeSwimsuit();

                return ;
                }

                case FetishCultistEnemy.PERVY_NUNS_CLOTHING:
                {
                RapeNun();
                }

                break;
            }

            if (true )
                RapeOther();

            CombatScene.CleanupAfterCombat();
        }

        public void RapeNun()
        {

            bool cockVersion = Player.Gender switch
            {
                Gender.Male => true,

                Gender.Hermaphrodite => Random.Shared.Next(100) < (50),

                _ => false,
            };

        RegisterMacro("doggy", Regex.IsMatch(Player.Race, "\b(?:dog|puppy|wolf)\b"));

            RegisterMacro("cockVersion", cockVersion);

            OutputText("Scenes.Fetishcultistscene.RapeNun.ApproachLatestToSeek".Localized());

            OutputText("Scenes.Fetishcultistscene.RapeNun.TakesOneOfYour".Localized());

            if (cockVersion)
            {
                int randomCockIndex = Random.Shared.Next(Player.CockTotal());

                RegisterMacro("randcock", () => Player.CockDescript(randomCockIndex));

                RegisterMacro("randcocklength", Player.Cocks[randomCockIndex].Length);

                RegisterMacro("randcockwidth", Player.Cocks[randomCockIndex].Thickness);

                OutputText("Scenes.Fetishcultistscene.RapeNun.MeeklyPresentingHole".Localized());

                OutputText("Scenes.Fetishcultistscene.RapeNun.LouderTheCultistScreams".Localized());

                Player.Orgasm("Dick");
            }
            else
            {
                RegisterMacro("spareEels", () => Player.CanDepositEel() ? "1" : "");

                if (Player.IsTaur())
                {
                    OutputText("Scenes.Fetishcultistscene.RapeNun.MeeklyLooksAtBestial".Localized());

                    if (Player.CanDepositEel() && !silly)
                        OutputText("Scenes.Fetishcultistscene.RapeNun.EelParasiteDeposit".Localized());
                }
                else
                    OutputText("Scenes.Fetishcultistscene.RapeNun.MeeklyLiesBack".Localized());

                Player.Orgasm("VaginalAnal");

                if (Player.CanDepositEel())
                    Player.DepositEel();
            }
        }

        public void RapeSwimsuit()
        {

            OutputText("Scenes.Fetishcultistscene.RapeSwimsuit.AdvanceOnDefeated".Localized());

            OutputText("Scenes.Fetishcultistscene.RapeSwimsuit.SlowlyAskHerWhy".Localized());
            // 原版 FetishCultistScene.hx:693：该分支的触发条件
            if (SceneInstance<Npcs.CeraphScene>.Get().HasExhibition())
                OutputText("Scenes.Fetishcultistscene.RapeSwimsuit.GivenExperienceWithCeraph".Localized());
            else
                OutputText("Scenes.Fetishcultistscene.RapeSwimsuit.DontReallyUnderstand".Localized());

            OutputText("Scenes.Fetishcultistscene.RapeSwimsuit.AtSightYourNude".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Fetishcultistscene.RapeSwimsuit.SheObviouslyWantsThis".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Fetishcultistscene.RapeSwimsuit.ThinkHowAwesome".Localized());
                else
                    OutputText("Scenes.Fetishcultistscene.RapeSwimsuit.AlmostDisappointed".Localized());

            OutputText("Scenes.Fetishcultistscene.RapeSwimsuit.TurnsAroundPresenting".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Fetishcultistscene.RapeSwimsuit.EyesLockOntoYour".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Fetishcultistscene.RapeSwimsuit.ThenSheHesitates".Localized());
                else
                    OutputText("Scenes.Fetishcultistscene.RapeSwimsuit.WantDamnMuch".Localized());
            }
            else
                OutputText("Scenes.Fetishcultistscene.RapeSwimsuit.SaysABitNervously".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 30f));

            ClearButtons();

            AddButton(0, "Scenes.Kitsune.DefeatTheKitsunes.ButtonFuckHerVag".Localized(), PlugSwimsuitVag);

            AddButton(1, "Common.Button.Vibrator".Localized(), SwimsuitVibrators);

            AddButton(14, "Common.Button.Leave".Localized(), RapeSwimsuitLeave);

            return ;
        }

        public void RapeSwimsuitLeave()
        {
            ClearOutput();

            OutputText("Scenes.Fetishcultistscene.RapeSwimsuitLeave.AwayAfterSwiping".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void RapeOther()
        {

            OutputText("Scenes.Fetishcultistscene.RapeOther.SeeThatTheCultists".Localized());

            if (Player.Corruption < 40)
                OutputText("Scenes.Fetishcultistscene.RapeOther.StopThinkingMaybe".Localized());
            else
                OutputText("Scenes.Fetishcultistscene.RapeOther.WickedSmilePush".Localized());

            if (Player.Cocks.Count > 0 && (Player.Gender != Gender.Hermaphrodite || Random.Shared.Next(2) != 0))
            {
                OutputText("Scenes.Fetishcultistscene.RapeOther.TellingIsGoing".Localized());

                if (Player.Corruption < 40)
                    OutputText("Scenes.Fetishcultistscene.RapeOther.UncomfortableYouStart".Localized());

                OutputText("Scenes.Fetishcultistscene.RapeOther.NewDeterminationPush".Localized());

                OutputText("Scenes.Fetishcultistscene.RapeOther.GetDressedAndNotice".Localized());

                Player.Orgasm("Dick");
            }
            else
                if (Player.Vaginas.Count > 0)
                {
                    OutputText("Scenes.Fetishcultistscene.RapeOther.TellingShePleases".Localized(
                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Fetishcultistscene.RapeOther.ScreamsForHelp".Localized());

                    if (Player.Corruption < 40)
                        OutputText("Scenes.Fetishcultistscene.RapeOther.UncomfortableYouStartAgain".Localized());

                    OutputText("Scenes.Fetishcultistscene.RapeOther.NewDeterminationYell".Localized());

                    OutputText("Scenes.Fetishcultistscene.RapeOther.FewMinutesLaterClose".Localized());

                    Player.Orgasm("Vaginal");
                }

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Fetishcultistscene.RapeOther.MakeRubYourBody".Localized());
        }

        public void PlugSwimsuitVag()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            if (Player.HasVagina())
                OutputText("Scenes.Fetishcultistscene.PlugSwimsuitVag.DeclineTheVibrators".Localized());
            else
                OutputText("Scenes.Fetishcultistscene.PlugSwimsuitVag.TellHerYoureGonna".Localized());

            OutputText("Scenes.Fetishcultistscene.PlugSwimsuitVag.NodsAndTurns".Localized());

            OutputText("Scenes.Fetishcultistscene.PlugSwimsuitVag.WhenYouPushInside".Localized(
                 Player.CockDescript(x),

                 Player.CockDescript(x)
            ));

            if (Player.CockArea(x) < 50)
                OutputText("Scenes.Fetishcultistscene.PlugSwimsuitVag.PushFurtherUntilYou".Localized());
            else
                if (Player.CockArea(x) < 200)
                    OutputText("Scenes.Fetishcultistscene.PlugSwimsuitVag.PushFurtherUntilTo".Localized());
                else
                    OutputText("Scenes.Fetishcultistscene.PlugSwimsuitVag.ManageMonsterCock".Localized());

            OutputText("Scenes.Fetishcultistscene.PlugSwimsuitVag.SlickInteriorSeems".Localized());

            OutputText("Scenes.Fetishcultistscene.PlugSwimsuitVag.AfterAllTheForeplay".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Fetishcultistscene.PlugSwimsuitVag.YourRemainingCock".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Fetishcultistscene.PlugSwimsuitVag.LetsLooseItsLoad".Localized());
                else
                    OutputText("Scenes.Fetishcultistscene.PlugSwimsuitVag.PluralLetLooseLoads".Localized());

                OutputText("Scenes.Fetishcultistscene.PlugSwimsuitVag.OntoBackAndLegs".Localized());

                if (Player.CumQ() < 50)
                    OutputText("Scenes.Fetishcultistscene.PlugSwimsuitVag.AddingWhiteDecoration".Localized());
                else
                    if (Player.CumQ() < 200)
                        OutputText("Scenes.Fetishcultistscene.PlugSwimsuitVag.AddingLiberalCoating".Localized());
                    else
                        OutputText("Scenes.Fetishcultistscene.PlugSwimsuitVag.DrenchingHerInCum".Localized());

                OutputText("Scenes.Fetishcultistscene.PlugSwimsuitVag.SentencePeriod".Localized());
            }

            OutputText("Scenes.Fetishcultistscene.PlugSwimsuitVag.ParagraphBreak".Localized());

            OutputText("Scenes.Fetishcultistscene.PlugSwimsuitVag.StandUpAndClean".Localized());

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();
        }

        public void SwimsuitVibrators()
        {
            ClearOutput();

            OutputText("Scenes.Fetishcultistscene.SwimsuitVibrators.SuggestYoureInterested".Localized());

            if (Player.VaginalCapacity() <= 30)
            {
                OutputText("Scenes.Fetishcultistscene.SwimsuitVibrators.VeryIntriguedByThe".Localized());

                OutputText("Scenes.Fetishcultistscene.SwimsuitVibrators.CertainlyArentGoing".Localized());

                OutputText("Scenes.Fetishcultistscene.SwimsuitVibrators.CultistLetsGoOf".Localized());

                OutputText("Scenes.Fetishcultistscene.SwimsuitVibrators.PutsVibratingToyTo".Localized(
                     Player.VaginaDescript(0)
                ));

                if (Player.Vaginas[0].Virgin)
                {
                    OutputText("Scenes.Fetishcultistscene.SwimsuitVibrators.HellOfAWay".Localized());

                    Player.CuntChange(50, false, false, false);
                }
                else
                {
                    Player.CuntChange(50, true, true, false);

                    OutputText("Scenes.Fetishcultistscene.SwimsuitVibrators.ParagraphBreak".Localized());
                }

                OutputText("Scenes.Fetishcultistscene.SwimsuitVibrators.ForHoursLeftOn".Localized());

                Player.Orgasm("Vaginal");

                Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, -3f));

                CombatController.Instance!.Context.Enemy.Lust = 98;

                CombatController.Instance!.Context.Enemy.Health = 2;

                Player.Lust = Player.MaxLust;

                CoC.Instance.Flags.Set(GameFlagIds.CombatBonusXpValue, CombatController.Instance!.Context.Enemy.Experience);

                CombatScene.CleanupAfterCombat();

                Player.Orgasm("Vaginal");

                Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, -3f));
            }
            else
            {
                OutputText("Scenes.Fetishcultistscene.SwimsuitVibrators.QuitePleasedWith".Localized());

                OutputText("Scenes.Fetishcultistscene.SwimsuitVibrators.ShowsYouHowTo".Localized(
                     Player.VaginaDescript(0)
                ));

                if (Player.VaginalCapacity() < 75)
                    OutputText("Scenes.Fetishcultistscene.SwimsuitVibrators.TakesEffortToGet".Localized());
                else
                    OutputText("Scenes.Fetishcultistscene.SwimsuitVibrators.EasilySlipsInside".Localized());

                OutputText("Scenes.Fetishcultistscene.SwimsuitVibrators.GigglesAtYouAnd".Localized(
                     Player.VaginaDescript(0)
                ));

                Player.CuntChange(50, true, true, false);

                OutputText("Scenes.Fetishcultistscene.SwimsuitVibrators.ParagraphBreakAgain".Localized());

                OutputText("Scenes.Fetishcultistscene.SwimsuitVibrators.SquealsWithGlee".Localized());

                OutputText("Scenes.Fetishcultistscene.SwimsuitVibrators.DoesntTakeMuchLonger".Localized(
                     Player.VaginaDescript(0)
                ));

                if (Player.Wetness() < 3)
                    OutputText("Scenes.Fetishcultistscene.SwimsuitVibrators.TrickleWord".Localized());
                else
                    if (Player.Wetness() <= 4)
                        OutputText("Scenes.Fetishcultistscene.SwimsuitVibrators.WaveWord".Localized());
                    else
                        OutputText("Scenes.Fetishcultistscene.SwimsuitVibrators.TorrentWord".Localized());

                OutputText("Scenes.Fetishcultistscene.SwimsuitVibrators.OfYourLadyJuices".Localized());

                Player.Orgasm("Vaginal");

                Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, -3f));

                CombatScene.CleanupAfterCombat();
            }
        }

        public void CultistBadEnd()
        {

            OutputText("Scenes.Fetishcultistscene.CultistBadEnd.MindNotAbleTo".Localized());

            DoNext(() => CultistBadEnd2());
        }

        public void CultistBadEnd2()
        {
            ClearOutput();

            OutputText("Scenes.Fetishcultistscene.CultistBadEnd2.OnAHillSits".Localized());

            OutputText("Scenes.Fetishcultistscene.CultistBadEnd2.WithinSitsConfessional".Localized());

            OutputText("Scenes.Fetishcultistscene.CultistBadEnd2.SheRepliesOtherBrightens".Localized());

            OutputText("Scenes.Fetishcultistscene.CultistBadEnd2.MindFallenPreyTo".Localized());

            GameOver();
        }

        public void FetishCultistHasAMilkFetish()
        {
            ClearOutput();

            OutputText("Scenes.Fetishcultistscene.FetishCultistHasAMilkFetish.RemoveYourStanding".Localized());

            OutputText("Scenes.Fetishcultistscene.FetishCultistHasAMilkFetish.MinutesLaterLandscape".Localized());

            OutputText("Scenes.Fetishcultistscene.FetishCultistHasAMilkFetish.ThankYouMaster".Localized(
                 Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Fetishcultistscene.FetishCultistHasAMilkFetish.WhatIsSheTalking".Localized(
                 Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Fetishcultistscene.FetishCultistHasAMilkFetish.ZeeMerechaingOng".Localized());

            OutputText("Scenes.Fetishcultistscene.FetishCultistHasAMilkFetish.CantUnderstandGibberish".Localized());

            OutputText("Scenes.Fetishcultistscene.FetishCultistHasAMilkFetish.UmmmmmNo".Localized());

            OutputText("Scenes.Fetishcultistscene.FetishCultistHasAMilkFetish.ContinuesToNurse".Localized());

            OutputText("Scenes.Fetishcultistscene.FetishCultistHasAMilkFetish.ThongBecomesWet".Localized(
                 Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Fetishcultistscene.FetishCultistHasAMilkFetish.HerMovingYourHand".Localized());

            if (Player.HasCock())
            {
                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Fetishcultistscene.FetishCultistHasAMilkFetish.EachOfY".Localized());
                else
                    OutputText("Scenes.Fetishcultistscene.FetishCultistHasAMilkFetish.YLetter".Localized());

                OutputText("Scenes.Fetishcultistscene.FetishCultistHasAMilkFetish.OurBecomesFullyErect".Localized());
            }
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Fetishcultistscene.FetishCultistHasAMilkFetish.PussyBecomesAroused".Localized());
                else
                    OutputText("Scenes.Fetishcultistscene.FetishCultistHasAMilkFetish.EntireBodyTenses".Localized());

            OutputText("Scenes.Fetishcultistscene.FetishCultistHasAMilkFetish.BothKeepPlaying".Localized());

            if (Player.Intelligence < 33)
            {
                OutputText("Scenes.Fetishcultistscene.FetishCultistHasAMilkFetish.NothingHappensEvenTry".Localized());

                Player.HealthChange(-5);
            }
            else
                if (Player.Intelligence < 66)
                    OutputText("Scenes.Fetishcultistscene.FetishCultistHasAMilkFetish.HeadTurnsRed".Localized());
                else
                    OutputText("Scenes.Fetishcultistscene.FetishCultistHasAMilkFetish.EverythingBackToNormal".Localized());

            OutputText("Scenes.Fetishcultistscene.FetishCultistHasAMilkFetish.CultistIsSleeping".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, .2f));

            Player.Milked();

            CombatScene.CleanupAfterCombat();
        }

    }
}