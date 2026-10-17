using MonoCoC.Attributes;

using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Pregnancy;
using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Lake
{
    [Register<GooGirlScene>("coc:goo_girl")]
    public class GooGirlScene : Scene
    {

        public GooGirlEnemy gooGirl = null!;

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
            GooGirlChance();
        }

        public float GooGirlChance()
        {

            switch (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown))
            {
                case 1:
                {
                return 0.75f;
                }

                case 2:
                {
                return 0.25f;
                }
            }

            if (true )
                return 0.5f;
        }

        public float SlimeChance()
        {

            return 1.0f - GooGirlChance();
        }

        public void ExecEncounter()
        {

            EncounterSlime();
        }

        public void EncounterSlime()
        {

            CoC.Instance.Flags.Set(GameFlagIds.TimesMetOoze, CoC.Instance.Flags.Get(GameFlagIds.TimesMetOoze) + 1);

            var slime = new GreenSlimeEnemy();

            if (Player.Intelligence >= 25)
            {
                OutputText("Scenes.Googirlscene.EncounterSlime.ShufflingSound".Localized());

                CombatScene.Start(slime, () => Go<CampScene>());

                if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 1)
                    OutputText("Scenes.Googirlscene.EncounterSlime.AmazedSlimeBanner".Localized());

                return ;
            }

            if (Player.Speed >= 30)
            {
                OutputText("Scenes.Googirlscene.EncounterSlime.MoistAnkle".Localized());

                CombatScene.Start(slime, () => Go<CampScene>());

                if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 1)
                    OutputText("Scenes.Googirlscene.EncounterSlime.AmazedSlimeBannerAgain".Localized());

                return ;
            }

            if (Player.Strength >= 40)
            {
                OutputText("Scenes.Googirlscene.EncounterSlime.MoistSpongy".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 1)
                    OutputText("Scenes.Googirlscene.EncounterSlime.AmazedSlimeBannerThird".Localized());

                OutputText("Scenes.Googirlscene.EncounterSlime.ParagraphBreak".Localized());

                CombatScene.Start(slime, () => Go<CampScene>());

                return ;
            }

            OutputText("Scenes.Googirlscene.EncounterSlime.MoistSpongyAgain".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 1)
                OutputText("Scenes.Googirlscene.EncounterSlime.AmazedSlimeBannerFourth".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Lust, 10f));

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.GreenSlime), () => Go<CampScene>());
        }

        public void EncounterGooGirl()
        {
            ClearOutput();

            gooGirl = new GooGirlEnemy();

            OutputText("Scenes.Googirlscene.EncounterGooGirl.PaleRedLight".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) != 2)
                OutputText("Scenes.Googirlscene.EncounterGooGirl.SapphireWord".Localized());
            else
                OutputText("Scenes.Googirlscene.EncounterGooGirl.MurkyWord".Localized());

            OutputText("Scenes.Googirlscene.EncounterGooGirl.PauseAtWaters".Localized());

            OutputText("Scenes.Googirlscene.EncounterGooGirl.SlimeSculpting".Localized(
                 GooColor()
            ));

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryGoogirls);

            gooGirl.OnDefeated = hpVictory => BeatUpGoo();

            gooGirl.OnWon = hpVictory => GetBeatByGooGirl();

            CombatScene.Start(gooGirl, () => Go<CampScene>());
        }

        public void CoreDropChance()
        {

            if (Random.Shared.Next(4) == 0 && Player.HasStatusEffect(StatusEffectIds.SlimeCraving) && !Player.HasPerk(PerkIds.SlimeCore) && Player.IsGoo() && Player.GooScore() >= 4)
            {
                OutputText("Scenes.Googirlscene.CoreDropChance.SlithersAway".Localized(
                     GooColor8()
                ));

                OutputText("Scenes.Googirlscene.CoreDropChance.MomentaryPressure".Localized());

                OutputText("Scenes.Googirlscene.CoreDropChance.GainedSlimeCore".Localized());

                Player.CreatePerk(PerkIds.SlimeCore, 0, 0, 0, 0);
            }
        }

        public string GooColor()
        {

            return gooGirl.GooColor();
        }

        public string GooColor2()
        {

            return gooGirl.GooColor2();
        }

        public string GooColor3()
        {

            return gooGirl.GooColor3();
        }

        public string GooColor4()
        {

            return gooGirl.GooColor4();
        }

        public string GooColor5()
        {

            return gooGirl.GooColor5();
        }

        public string GooColor6()
        {

            return gooGirl.GooColor6();
        }

        public string GooColor7()
        {

            return gooGirl.GooColor7();
        }

        public string GooColor8()
        {

            return gooGirl.GooColor8();
        }

        public string GooColor9()
        {

            return gooGirl.GooColor9();
        }

        public string GooColor10()
        {

            return gooGirl.GooColor10();
        }

        public string GooColor11()
        {

            return gooGirl.GooColor11();
        }

        public void GooGirlBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Googirlscene.GooGirlBadEnd.CollapseStrength".Localized(
                 GooColor()
            ));

            OutputText("Scenes.Googirlscene.GooGirlBadEnd.NucleusSwells".Localized());

            OutputText("Scenes.Googirlscene.GooGirlBadEnd.PantingTerms".Localized(
                 GooColor2()
            ));

            OutputText("Scenes.Googirlscene.GooGirlBadEnd.BodyLeaden".Localized());

            DoNext(() => GooGirlBadEnd2());
        }

        public void GooGirlBadEnd2()
        {
            ClearOutput();

            OutputText("Scenes.Googirlscene.GooGirlBadEnd2.OneYearLater".Localized());

            OutputText("Scenes.Googirlscene.GooGirlBadEnd2.TwoSistersSwim".Localized());

            OutputText("Scenes.Googirlscene.GooGirlBadEnd2.FullPlateMail".Localized());

            OutputText("Scenes.Googirlscene.GooGirlBadEnd2.GripsGentle".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Googirlscene.GooGirlBadEnd2.PseudopodExtends".Localized());
            else
                OutputText("Scenes.Googirlscene.GooGirlBadEnd2.HeadFirstInsert".Localized());

            OutputText("Scenes.Googirlscene.GooGirlBadEnd2.ShiversJubilation".Localized());

            OutputText("Scenes.Googirlscene.GooGirlBadEnd2.SlidesNucleus".Localized(
                 GooColor3(),

                 GooColor2()
            ));

            OutputText("Scenes.Googirlscene.GooGirlBadEnd2.RadiatingHeat".Localized());

            GameOver();
        }

        public void SlimeBadEnd()
        {

            OutputText("Scenes.Googirlscene.SlimeBadEnd.BodyWobbles".Localized());

            DoNext(() => SlimeBadEnd2());
        }

        public void SlimeBadEnd2()
        {
            ClearOutput();

            OutputText("Scenes.Googirlscene.SlimeBadEnd2.OneYearLater".Localized());

            OutputText("Scenes.Googirlscene.SlimeBadEnd2.NewChampion".Localized());

            OutputText("Scenes.Googirlscene.SlimeBadEnd2.ReprieveInterrupted".Localized());

            OutputText("Scenes.Googirlscene.SlimeBadEnd2.IncrediblyPleasurable".Localized());

            OutputText("Scenes.Googirlscene.SlimeBadEnd2.NewFoundVigor".Localized());

            OutputText("Scenes.Googirlscene.SlimeBadEnd2.FewHours".Localized());

            OutputText("Scenes.Googirlscene.SlimeBadEnd2.EveryYearAfter".Localized());

            GameOver();
        }

        public void GetBeatByGooGirl()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.BonusItemAfterCombatId) == ConsumableIds.RubberBall)
                CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, "");

            CoC.Instance.Flags.Set(GameFlagIds.GoogirlConsecutiveLosses, CoC.Instance.Flags.Get(GameFlagIds.GoogirlConsecutiveLosses) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.GoogirlConsecutiveLosses) >= 5 && Player.GooScore() >= 4)
                GooGirlBadEnd();
            else
                if (Player.Gender == Gender.Genderless)
                    GenderlessLoseToGooGal();
                else
                    if (Player.Gender == Gender.Male)
                        DudeLoseToGooGal();
                    else
                        if (Player.Gender == Gender.Female)
                            FemaleLoseToGooGal();
                        else
                            HermLoseToGooGal();

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedNormalGoos, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedNormalGoos) + 1);
        }

        public void GenderlessLoseToGooGal()
        {
            ClearOutput();

            OutputText("Scenes.Googirlscene.GenderlessLoseToGooGal.StumbleKnees".Localized());

            OutputText("Scenes.Googirlscene.GenderlessLoseToGooGal.CarefulSurvey".Localized(
                 GooColor()
            ));

            OutputText("Scenes.Googirlscene.GenderlessLoseToGooGal.RedCorePulses".Localized());

            OutputText("Scenes.Googirlscene.GenderlessLoseToGooGal.StruggleVibrations".Localized(
                 GooColor3(),

                 GooColor4()
            ));

            OutputText("Scenes.Googirlscene.GenderlessLoseToGooGal.DigestiveSphincter".Localized(
                 GooColor5(),

                 GooColor6()
            ));

            OutputText("Scenes.Googirlscene.GenderlessLoseToGooGal.AwakensHack".Localized(
                 GooColor()
            ));

            Player.SlimeFeed();

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 4f));

            CombatScene.CleanupAfterCombat();
        }

        public void DudeLoseToGooGal()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Googirlscene.DudeLoseToGooGal.ReelingBackwards".Localized(
                 GooColor(),

                 GooColor5(),

                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Googirlscene.DudeLoseToGooGal.HeatSecondSkin".Localized(
                 GooColor2(),

                 Player.CockDescript(x),

                 GooColor4()
            ));

            ShowImage("googirl-loss-male");

            OutputText("Scenes.Googirlscene.DudeLoseToGooGal.JerksTentacle".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Googirlscene.DudeLoseToGooGal.LitheOoze".Localized(
                     Player.BallsDescriptLight()
                ));

            OutputText("Scenes.Googirlscene.DudeLoseToGooGal.SemiTransparentHue".Localized(
                 GooColor2(),

                 GooColor5()
            ));

            OutputText("Scenes.Googirlscene.DudeLoseToGooGal.ClenchesMouth".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Googirlscene.DudeLoseToGooGal.ScrotumLurches".Localized());

            OutputText("Scenes.Googirlscene.DudeLoseToGooGal.ThinTongue".Localized(
                 GooColor5()
            ));

            if (Player.CumQ() <= 500)
                OutputText("Scenes.Googirlscene.DudeLoseToGooGal.PearlFluid".Localized());
            else
                OutputText("Scenes.Googirlscene.DudeLoseToGooGal.ExpressionPleased".Localized(
                     Player.CockDescript(x),

                     GooColor4(),

                     GooColor()
                ));

            OutputText("Scenes.Googirlscene.DudeLoseToGooGal.ArmSlidesOut".Localized());

            Player.SlimeFeed();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 4f));

            Player.SlimeFeed();

            CombatScene.CleanupAfterCombat();
        }

        public void HermLoseToGooGal()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Googirlscene.HermLoseToGooGal.CantTakeAnymore".Localized(
                 GooColor2(),

                 GooColor7()
            ));

            OutputText("Scenes.Googirlscene.HermLoseToGooGal.BreaksKiss".Localized(
                 GooColor3()
            ));

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Googirlscene.HermLoseToGooGal.SWord".Localized());

            OutputText("Scenes.Googirlscene.HermLoseToGooGal.RipplingWalls".Localized(
                 Player.CockDescript(x),

                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Googirlscene.HermLoseToGooGal.DozensMouths".Localized());

            if (Player.BiggestLactation() >= 2)
                OutputText("Scenes.Googirlscene.HermLoseToGooGal.MouthsSucking".Localized(
                     Appearance.AllBreastsDescript(Player)
                ));

            OutputText("Scenes.Googirlscene.HermLoseToGooGal.SpurtingEcstasy".Localized(
                 GooColor3()
            ));

            OutputText("Scenes.Googirlscene.HermLoseToGooGal.RunDry".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 4f));

            Player.SlimeFeed();

            CombatScene.CleanupAfterCombat();
        }

        public void FemaleLoseToGooGal()
        {
            ClearOutput();

            OutputText("Scenes.Googirlscene.FemaleLoseToGooGal.SwayBalance".Localized());

            var titsOrgasm = false;

            if (Player.Pregnancy.Incubation <= 280 && Player.Pregnancy.Incubation >= 1)
                OutputText("Scenes.Googirlscene.FemaleLoseToGooGal.SquishesBody".Localized(
                     GooColor()
                ));
            else
            {
                OutputText("Scenes.Googirlscene.FemaleLoseToGooGal.ShrugsGestures".Localized(
                     GooColor8()
                ));

                OutputText("Scenes.Googirlscene.FemaleLoseToGooGal.PokesBreasts".Localized(
                     Appearance.AllBreastsDescript(Player)
                ));

                ShowImage("googirl-loss-female");

                OutputText("Scenes.Googirlscene.FemaleLoseToGooGal.ExperimentalSwallow".Localized(
                     GooColor7()
                ));

                Player.BreastRows[0].BreastRating += 1;

                Player.BoostLactation(1);

                titsOrgasm = true;
            }

            if (Player.Pregnancy.Incubation <= 280 && Player.Pregnancy.Incubation >= 1)
            {
                OutputText("Scenes.Googirlscene.FemaleLoseToGooGal.SqueezesBreasts".Localized(
                     GooColor(),

                     GooColor(),

                     Appearance.AllBreastsDescript(Player)
                ));

                OutputText("Scenes.Googirlscene.FemaleLoseToGooGal.MilkSquirts".Localized(
                     GooColor()
                ));

                if (Player.LactationQ() <= 1000)
                    OutputText("Scenes.Googirlscene.FemaleLoseToGooGal.DrainsQuickly".Localized());
                else
                    if (Player.LactationQ() <= 2000)
                        OutputText("Scenes.Googirlscene.FemaleLoseToGooGal.DrinksGratefully".Localized());
                    else
                        OutputText("Scenes.Googirlscene.FemaleLoseToGooGal.BitesLip".Localized(
                             GooColor5()
                        ));

                OutputText("Scenes.Googirlscene.FemaleLoseToGooGal.FondBop".Localized());

                titsOrgasm = true;
            }
            else
            {
                OutputText("Scenes.Googirlscene.FemaleLoseToGooGal.FlashesSmile".Localized(
                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Googirlscene.FemaleLoseToGooGal.FaceFindsPussy".Localized(
                     Player.VaginaDescript(0)
                ));

                ShowImage("googirl-loss-female");

                OutputText("Scenes.Googirlscene.FemaleLoseToGooGal.TriesToStop".Localized(
                     GooColor()
                ));

                if (Player.VaginalCapacity() <= 30)
                    OutputText("Scenes.Googirlscene.FemaleLoseToGooGal.BottomsOut".Localized());
                else
                    OutputText("Scenes.Googirlscene.FemaleLoseToGooGal.MoreOoze".Localized());

                OutputText("Scenes.Googirlscene.FemaleLoseToGooGal.BodyDistorted".Localized());

                OutputText("Scenes.Googirlscene.FemaleLoseToGooGal.SlidesBackOut".Localized());

                if (Player.Pregnancy.Incubation >= 1)
                    OutputText("Scenes.Googirlscene.FemaleLoseToGooGal.EverythingRunsOut".Localized());
                else
                    Player.KnockUp(PregnancyIds.GooGirl, PregnancyIds.GooGirlIncubation, 1, 1);
            }

            Player.Orgasm(titsOrgasm ? "Tits" : "Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 4f));

            Player.SlimeFeed();

            CombatScene.CleanupAfterCombat();
        }

        public void GooPregVagBirth()
        {

            gooGirl = new GooGirlEnemy();

            CoC.Instance.Flags.Set(GameFlagIds.GoogirlBirths, CoC.Instance.Flags.Get(GameFlagIds.GoogirlBirths) + 1);

            OutputText("Scenes.Googirlscene.GooPregVagBirth.NewlineBreak".Localized());

            if (Player.Vaginas.Count == 0)
            {
                OutputText("Scenes.Googirlscene.GooPregVagBirth.TerriblePressure".Localized());

                Player.CreateVagina();
            }

            OutputText("Scenes.Googirlscene.GooPregVagBirth.LurchingBelly".Localized());

            OutputText("Scenes.Googirlscene.GooPregVagBirth.SludgeQuivers".Localized(
                 GooColor4()
            ));

            Player.ApplyStats(new DynStat(GameStats.Lust, 50f));
        }

        public void BeatUpGoo()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.GoogirlConsecutiveLosses, 0);

            OutputText("Scenes.Googirlscene.BeatUpGoo.ScuffleExcitement".Localized());

            OutputText("Scenes.Googirlscene.BeatUpGoo.DefeatedAtMercy".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Dicksex".Localized(), null);

            AddButton(1, "Common.Button.Bigdicksex".Localized(), null);

            AddButton(2, "Common.Button.HermSex".Localized(), null);

            AddButton(3, "Common.Button.Femfuck".Localized(), null);

            AddButton(4, "Scenes.Googirlscene.ExhibitionismFuck".Localized(), null);

            AddButton(5, "Common.Button.Breastfeed".Localized(), null);

            AddButton(6, "Common.Button.LayEggs".Localized(), null);

            AddButton(7, "Common.Button.Valeria".Localized(), null);

            if ((CoC.Instance.Flags.Get(GameFlagIds.GooTfedMean) == 0 && CoC.Instance.Flags.Get(GameFlagIds.GooTfedNice) == 0) && CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedNormalGoos) >= 2)
                AddButton(8, "Common.Button.MakeSlave".Localized(), null);

            AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));

            if (Player.Lust >= 33)
            {
                if (Player.HasCock())
                {
                    if (Player.Cocks[Player.SmallestCockIndex()].Length < 24)
                        AddButton(0, "Common.Button.Dicksex".Localized(), () => GooMaleRape(2));

                    if (Player.BiggestCockLength() >= 24)
                        AddButton(1, "Common.Button.Bigdicksex".Localized(), () => GooMaleRape(1));
                }

                if (Player.IsHerm())
                    AddButton(2, "Common.Button.HermSex".Localized(), VictoryHermSex);

                if (Player.IsFemale())
                {
                    AddButton(3, "Common.Button.Femfuck".Localized(), NormalFemaleRapesAGooGirl);

                    if (Player.IsCorruptEnough(66) || Player.HasPerk(PerkIds.Pervert))
                        AddButton(4, "Scenes.Googirlscene.ExhibitionismFuck".Localized(), ExhibitionismGooGirlVictoryRape);
                }

                if (Player.ArmorName == "goo armor" && !Player.IsButtPregnant() && !Player.IsPregnant())
                {
                    OutputText("Scenes.Googirlscene.BeatUpGoo.ValeriaEbb".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.TimesValeriaGooThreesomed) == 0)
                        OutputText("Scenes.Googirlscene.BeatUpGoo.OfferThreesome".Localized());
                    else
                        OutputText("Scenes.Googirlscene.BeatUpGoo.OfferThreesomeAgain".Localized());

                    AddButton(7, "Common.Button.Valeria".Localized(), null );
                }
            }

            if (Player.HasPerk(PerkIds.Feeder) || Player.LactationQ() >= 500)
                AddButton(5, "Common.Button.Breastfeed".Localized(), VictoryRapeAGooGalAsFeeder);

            if (Player.CanOvipositBee())
                AddButton(6, "Common.Button.LayEggs".Localized(), LayBeeEggsInGoo);

            if ((CoC.Instance.Flags.Get(GameFlagIds.GooTfedMean) == 0 && CoC.Instance.Flags.Get(GameFlagIds.GooTfedNice) == 0) && CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedNormalGoos) >= 2)
                if (Player.IsPureEnough(50) && (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk))) && (Player.HasItem(ItemRegistry.Create(ConsumableIds.EggBlack)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.EggLargeBlack))))

                    return ;
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.PcKnowsAboutBlackEggs) > 0)
                    {
                        if (CoC.Instance.Flags.Get(GameFlagIds.TimesThoughtAboutGooRecruitment) == 0)
                        {
                            OutputText("Scenes.Googirlscene.BeatUpGoo.QuiveringLump".Localized());

                            CoC.Instance.Flags.Set(GameFlagIds.TimesThoughtAboutGooRecruitment, CoC.Instance.Flags.Get(GameFlagIds.TimesThoughtAboutGooRecruitment) + 1);
                        }
                        else
                            OutputText("Scenes.Googirlscene.BeatUpGoo.SurveyVictory".Localized());

                        if ((Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk))) && (Player.HasItem(ItemRegistry.Create(ConsumableIds.EggBlack)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.EggLargeBlack))))
                        {
                            OutputText("Scenes.Googirlscene.BeatUpGoo.HandyGood".Localized());

                            AddButton(8, "Common.Button.MakeSlave".Localized(), null );
                        }
                    }

            if (Player.HasMultiTails())
                AddNextButton("Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());
        }

        public void GooMaleRape(float type = 1)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedNormalGoos, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedNormalGoos) + 1);

            var x = Player.BiggestCockIndex();

            if (Player.BiggestCockLength() >= 24 && Player.ShortestCockLength() < 24)
                if (type == 1)
                    x = Player.BiggestCockIndex();
                else
                    x = Player.ShortestCockIndex();

            OutputText("Scenes.Googirlscene.GooMaleRape.WrigglesHopefully".Localized());

            if (Player.Cocks[x].Length < 24)
            {
                OutputText("Scenes.Googirlscene.GooMaleRape.DroppingLevel".Localized(
                     GooColor(),

                     GooColor2(),

                     GooColor9()
                ));

                OutputText("Scenes.Googirlscene.GooMaleRape.ClapsHands".Localized(
                     Player.CockDescript(x),

                     Player.CockDescript(x)
                ));

                ShowImage("googirl-win-male");

                OutputText("Scenes.Googirlscene.GooMaleRape.OilyCrevasse".Localized());

                OutputText("Scenes.Googirlscene.GooMaleRape.IcyBliss".Localized(
                     Player.CockDescript(x),

                     GooColor()
                ));

                OutputText("Scenes.Googirlscene.GooMaleRape.StimulationAssault".Localized());

                OutputText("Scenes.Googirlscene.GooMaleRape.DumpsRest".Localized(
                     GooColor()
                ));

                if (Player.IsCorruptEnough(85) && Random.Shared.Next(2) == 0)
                    OutputText("Scenes.Googirlscene.GooMaleRape.GlanceBack".Localized(
                         GooColor11()
                    ));
            }
            else
            {
                OutputText("Scenes.Googirlscene.GooMaleRape.HooksArms".Localized(
                     GooColor4(),

                     GooColor()
                ));

                OutputText("Scenes.Googirlscene.GooMaleRape.RubsBetween".Localized(
                     Player.CockDescript(x),

                     GooColor2()
                ));

                ShowImage("googirl-win-male");

                OutputText("Scenes.Googirlscene.GooMaleRape.ForcefulThrust".Localized(
                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Googirlscene.GooMaleRape.TipNearsCore".Localized(
                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Googirlscene.GooMaleRape.FuckedHowWanted".Localized());

                OutputText("Scenes.Googirlscene.GooMaleRape.OrgasmBursts".Localized(
                     GooColor5(),

                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Googirlscene.GooMaleRape.AllDenizens".Localized());

                CoreDropChance();
            }

            Player.Orgasm("Dick");

            Player.SlimeFeed();

            CombatScene.CleanupAfterCombat();
        }

        public void VictoryHermSex()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedNormalGoos, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedNormalGoos) + 1);

            OutputText("Scenes.Googirlscene.VictoryHermSex.RelentsBody".Localized(
                 GooColor8(),

                 GooColor4()
            ));

            OutputText("Scenes.Googirlscene.VictoryHermSex.TakesBreasts".Localized(
                 GooColor(),

                 GooColor5()
            ));

            OutputText("Scenes.Googirlscene.VictoryHermSex.QuicklyStrips".Localized(
                 Player.MultiCockDescriptLight(),

                 GooColor2(),

                 GooColor5()
            ));

            ShowImage("googirl-win-male");

            OutputText("Scenes.Googirlscene.VictoryHermSex.SlowingEnjoy".Localized(
                 Player.CockDescript(x)
            ));

            Player.CuntChange(Player.CockArea(0), true, false, true);

            OutputText("Scenes.Googirlscene.VictoryHermSex.EveryWithdraw".Localized(
                 GooColor5(),

                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Googirlscene.VictoryHermSex.MimickingWhore".Localized());

            if (Player.IsCorruptEnough(85) && Random.Shared.Next(2) == 0)
                OutputText("Scenes.Googirlscene.VictoryHermSex.GlanceBack".Localized(
                     GooColor11()
                ));

            CoreDropChance();

            Player.Orgasm("Dick");

            Player.SlimeFeed();

            CombatScene.CleanupAfterCombat();
        }

        public void VictoryRapeAGooGalAsFeeder()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedNormalGoos, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedNormalGoos) + 1);

            OutputText("Scenes.Googirlscene.VictoryRapeAGooGalAsFeeder.WobblesCuriously".Localized(
                 GooColor4()
            ));

            OutputText("Scenes.Googirlscene.VictoryRapeAGooGalAsFeeder.StrippingWaist".Localized(
                 GooColor4(),

                 Appearance.AllBreastsDescript(Player)
            ));

            ShowImage("googirl-win-female");

            OutputText("Scenes.Googirlscene.VictoryRapeAGooGalAsFeeder.SwallowsMilk".Localized(
                 Appearance.AllBreastsDescript(Player),

                 GooColor5()
            ));

            OutputText("Scenes.Googirlscene.VictoryRapeAGooGalAsFeeder.FillsCheeks".Localized(
                 GooColor2(),

                 GooColor8()
            ));

            if (Player.HasVagina())
                OutputText("Scenes.Googirlscene.VictoryRapeAGooGalAsFeeder.ClitVibrates".Localized());
            else
                if (Player.HasCock())
                    if (Player.HasVagina())
                        OutputText("Scenes.Googirlscene.VictoryRapeAGooGalAsFeeder.CockVibrates".Localized());
                    else
                        if (Player.HasVagina())
                            OutputText("Scenes.Googirlscene.VictoryRapeAGooGalAsFeeder.AnusVibrates".Localized());

            OutputText("Scenes.Googirlscene.VictoryRapeAGooGalAsFeeder.RegainConsciousness".Localized(
                 GooColor(),

                 GooColor5()
            ));

            CoreDropChance();

            Player.Orgasm("Tits");

            Player.ChangeStatusValue(StatusEffectIds.Feeder, 0, 1);

            Player.ChangeStatusValue(StatusEffectIds.Feeder, 2, 0);

            CombatScene.CleanupAfterCombat();
        }

        public void ExhibitionismGooGirlVictoryRape()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedNormalGoos, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedNormalGoos) + 1);

            OutputText("Scenes.Googirlscene.ExhibitionismGooGirlVictoryRape.WobblesCuriously".Localized());

            OutputText("Scenes.Googirlscene.ExhibitionismGooGirlVictoryRape.ClosesDistance".Localized(
                 GooColor8(),

                 Appearance.AllBreastsDescript(Player),

                 GooColor2(),

                 GooColor4(),

                 GooColor6()
            ));

            OutputText("Scenes.Googirlscene.ExhibitionismGooGirlVictoryRape.HandsEntwine".Localized(
                 GooColor10(),

                 GooColor5()
            ));

            OutputText("Scenes.Googirlscene.ExhibitionismGooGirlVictoryRape.FreeingHand".Localized(
                 GooColor4(),

                 Player.VaginaDescript(0),

                 GooColor5()
            ));

            OutputText("Scenes.Googirlscene.ExhibitionismGooGirlVictoryRape.CriesSwooning".Localized());

            OutputText("Scenes.Googirlscene.ExhibitionismGooGirlVictoryRape.LoudOrgasms".Localized(
                 GooColor7()
            ));

            CoreDropChance();

            Player.Orgasm("Generic");

            Player.SlimeFeed();

            CombatScene.CleanupAfterCombat();
        }

        public void NormalFemaleRapesAGooGirl()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedNormalGoos, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedNormalGoos) + 1);

            OutputText("Scenes.Googirlscene.NormalFemaleRapesAGooGirl.WobblesCuriously".Localized(
                 GooColor2()
            ));

            OutputText("Scenes.Googirlscene.NormalFemaleRapesAGooGirl.WithYourHandsWrapped".Localized(
                 GooColor4(),

                 Player.VaginaDescript(0),

                 GooColor8(),

                 Appearance.AllBreastsDescript(Player),

                 GooColor5()
            ));

            OutputText("Scenes.Googirlscene.NormalFemaleRapesAGooGirl.StrangeSensations".Localized(
                 GooColor2(),

                 GooColor2()
            ));

            ShowImage("googirl-win-female");

            OutputText("Scenes.Googirlscene.NormalFemaleRapesAGooGirl.ShallowGulps".Localized());

            OutputText("Scenes.Googirlscene.NormalFemaleRapesAGooGirl.ThirdFinger".Localized(
                 GooColor4()
            ));

            OutputText("Scenes.Googirlscene.NormalFemaleRapesAGooGirl.UnsureHowMuch".Localized(
                 GooColor3()
            ));

            CoreDropChance();

            Player.Orgasm("Vaginal");

            Player.SlimeFeed();

            CombatScene.CleanupAfterCombat();
        }

        public void SpyOnGooAndOozeSex()
        {
            ClearOutput();

            gooGirl = new GooGirlEnemy();

            OutputText("Scenes.Googirlscene.SpyOnGooAndOozeSex.HearsSounds".Localized(
                 GooColor7()
            ));

            OutputText("Scenes.Googirlscene.SpyOnGooAndOozeSex.BlinkingRealize".Localized(
                 GooColor8(),

                 GooColor3()
            ));

            OutputText("Scenes.Googirlscene.SpyOnGooAndOozeSex.CrimsonCoreSwells".Localized(
                 GooColor8(),

                 GooColor()
            ));

            OutputText("Scenes.Googirlscene.SpyOnGooAndOozeSex.HurriesAway".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(4 + Player.Corruption / 10)));

            DoNext(() => ReturnToCamp(1));
        }

        public void LayBeeEggsInGoo()
        {
            ClearOutput();

            OutputText("Scenes.Googirlscene.LayBeeEggsInGoo.StandsTriumphantly".Localized());

            OutputText("Scenes.Googirlscene.LayBeeEggsInGoo.RaisesAbdomen".Localized());

            OutputText("Scenes.Googirlscene.LayBeeEggsInGoo.PlungesOvipositor".Localized());

            OutputText("Scenes.Googirlscene.LayBeeEggsInGoo.HoneyBulge".Localized());

            OutputText("Scenes.Googirlscene.LayBeeEggsInGoo.ContinuesTime".Localized());

            Player.DumpEggs();

            Player.Orgasm("Ovi");

            CombatScene.CleanupAfterCombat();
        }

    }
}