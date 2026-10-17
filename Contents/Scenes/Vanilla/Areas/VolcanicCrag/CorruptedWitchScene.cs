using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Pregnancy;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.VolcanicCrag
{
    [Register<CorruptedWitchScene>("coc:corrupted_witch")]
    public class CorruptedWitchScene : Scene
    {

        public override void Entry()
        {
            CorrWitchIntro();
        }

        public void CorrWitchMenu()
        {

            ClearButtons();

            AddButton(0, "Scenes.Akbal.RepeatAkbalPostSubmission.ButtonDeny".Localized(), StartFight);

            AddButton(1, "Common.Button.Accept".Localized(), CorrWitchSexytime);
        }

        public void CorrWitchIntro()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.Volcwitchnumberofchildren) >= 5 || CoC.Instance.Flags.Get(GameFlagIds.Volcwitchnumberofbirths) >= 5)
            {
                OutputText("Scenes.Corruptedwitchscene.CorrWitchIntro.BarrenLandscape".Localized());

                OutputText("Scenes.Corruptedwitchscene.CorrWitchIntro.StillSuspicious".Localized((CoC.Instance.Flags.Get(GameFlagIds.Volcwitchnumberofchildren) > 0 ? "inseminate me" : "be inseminated")));

                ClearButtons();

                if (Player.Lust >= 33)
                    AddNextButton("Common.Button.Sex".Localized(), CorrWitchSexytime);
                else
                    AddNextButtonDisabled("Sex", "You're not in the mood for sex.", "");

                AddNextButton("Common.Button.Knowledge".Localized(), CorrWitchTrade);

                if ((CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & (int)CorruptedCovenFlags.ToldName) != 0)
                    AddNextButton("Scenes.Dullahan.DullTalkMenu.ButtonCirce".Localized(), CorrWitchCirce);

                if (Player.Balls == 0 && CoC.Instance.Flags.Get(GameFlagIds.Volcwitchnumberofchildren) >= 5 && !Player.HasPerk(PerkIds.PotentProstate) && Player.HasCock())
                {
                    OutputText("Scenes.Corruptedwitchscene.CorrWitchIntro.VirilityRemark".Localized());

                    AddNextButton("Common.Button.Boost".Localized(), CorrWitchBlessing);
                }

                if (Player.HasVagina() && CoC.Instance.Flags.Get(GameFlagIds.Volcwitchnumberofbirths) >= 5 && !Player.IsPregnant() && !Player.HasPerk(PerkIds.PotentPregnancy))
                {
                    OutputText("Scenes.Corruptedwitchscene.CorrWitchIntro.WombFertility".Localized());

                    AddNextButton("Common.Button.Vigor".Localized(), CorrWitchBlessingWomb);
                }

                SetExitButton("Common.Button.Back".Localized());

                return ;
            }

            OutputText("Scenes.Corruptedwitchscene.CorrWitchIntro.WalksBarren".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchIntro.VoluptuousWoman".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchIntro.DidntSoundLike".Localized());

            CorrWitchMenu();
        }

        public void CorrWitchCirce()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedwitchscene.CorrWitchCirce.AsksAboutCirce".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchCirce.HandToHer".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchCirce.MomentsLaterGives".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchCirce.SighsFrustration".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchCirce.StubbornWitches".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void CorrWitchTrade()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedwitchscene.CorrWitchTrade.AsksForSpells".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchTrade.CombatSpells".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchTrade.CantBeatPrice".Localized());

            float minoCum = Player.ItemCount(ConsumableIds.MinotaurCum);

            if (minoCum > 3)
                minoCum = 3;

            ClearButtons();

            if (minoCum >= 3)
                AddButton(0, "Common.Button.BuyBook".Localized(), () => BuyWitchBook(minoCum));
            else
                AddButton(0, "Common.Button.BuyBook".Localized(), null);

            AddButton(14, "Common.Button.Back".Localized(), CorrWitchIntro);
        }

        public void BuyWitchBook(float minocum)
        {
            ClearOutput();

            Player.DestroyItems(ConsumableIds.MinotaurCum, minocum);

            OutputText("Scenes.Corruptedwitchscene.BuyWitchBook.HandsBottles".Localized());

            OutputText("Scenes.Corruptedwitchscene.BuyWitchBook.StoresBottles".Localized());

            OutputText("Scenes.Corruptedwitchscene.BuyWitchBook.AppearanceChanges".Localized());

            OutputText("Scenes.Corruptedwitchscene.BuyWitchBook.BookTransforms".Localized());

            OutputText("Scenes.Corruptedwitchscene.BuyWitchBook.WishYouLuck".Localized());

            OutputText("Scenes.Corruptedwitchscene.BuyWitchBook.TakesBookThanks".Localized());

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.GrayBook), 1);

ReturnToCamp(1);
        }

        public void CorrWitchSexytime()
        {

            if (Player.Lust < 33)
                Player.Lust = 33;

            ClearButtons();

            AddButton(0, "Common.Button.Vagina".Localized(), CorrWitchFuckHerVagina);

            AddButton(1, "Common.Button.TooBigtruegrowAPenis".Localized(), CorrWitchGrowaPenis);

            if (Player.IsGenderless())
                AddButton(3, "Common.Button.Genderless".Localized(), CorrWitchDisappointed);
        }

        public void CorrWitchGrowaPenis()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.AsksHowCould".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.FingersSpread".Localized());

            if (Player.IsTaur())
            {
                OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.NotBadCock".Localized());

                if (Player.Libido > 70 || Player.Corruption > 70)
                    OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.TinglesDrooling".Localized());
                else
                    OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.EroticSpellcasting".Localized());

                OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.SorceressUnfit".Localized(Player.ClothedOrNakedLower(" open up your [armor],"), (Player.Wetness() > 3 ? "the copious " : "the"), (Player.HasBreasts() ? " and being to grope your breasts and tweak your nipples" : "")));

                OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.MuskHeadSwim".Localized());

                OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.MassiveShaftSlides".Localized());

                OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.LongInsertion".Localized());

                OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.EnjoyingSensations".Localized());

                OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.VoicesConcern".Localized());

                OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.PassiveRole".Localized());

                OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.BlissfulSeconds".Localized());

                OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.AfterFuck".Localized());
            }
            else
            {
                if (Player.Libido > 70 || Player.Corruption > 70)
                    OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.MouthWaters".Localized());
                else
                    OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.InterestingSpell".Localized());

                OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.StripsArmor".Localized());

                OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.ProdsVulva".Localized());

                OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.GrabsHips".Localized());

                Player.CuntChange(8, true, true, false);

                OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.AngerFades".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.SwingsHeavily".Localized());

                OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.ThrustsUneven".Localized((Player.HasCock() ? " [cock] and hips " : " hips ")));

                if (Player.HasCock())
                    OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.CockExplodes".Localized());

                OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.LockedTogether".Localized());

                if (Player.VaginalCapacity() >= 100)
                    OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.WombHappilyReceives".Localized());

                if (Player.VaginalCapacity() < 100 && Player.VaginalCapacity() > 50)
                    OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.WombManagesReceive".Localized());

                if (Player.VaginalCapacity() <= 50)
                    OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.WombBarelyReceives".Localized());

                OutputText("Scenes.Corruptedwitchscene.CorrWitchGrowaPenis.CaressesHead".Localized());
            }

            if (!Player.IsPregnant())
                Player.KnockUp(PregnancyIds.CorruptedWitch, PregnancyIds.CorruptedWitchIncubation, 120);

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();
        }

        public void GiveBirthToWitches()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedwitchscene.GiveBirthToWitches.HeatGrows".Localized());

            OutputText("Scenes.Corruptedwitchscene.GiveBirthToWitches.RushesToEdge".Localized());

            OutputText("Scenes.Corruptedwitchscene.GiveBirthToWitches.TriesToPush".Localized());

            OutputText("Scenes.Corruptedwitchscene.GiveBirthToWitches.LiesWorried".Localized());

            OutputText("Scenes.Corruptedwitchscene.GiveBirthToWitches.GatherAround".Localized());

            OutputText("Scenes.Corruptedwitchscene.GiveBirthToWitches.NotMuchTime".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.Volcwitchnumberofbirths, CoC.Instance.Flags.Get(GameFlagIds.Volcwitchnumberofbirths) + 1);

            switch (CoC.Instance.Flags.Get(GameFlagIds.Volcwitchnumberofbirths))
            {
                case 1:
                {
                OutputText("Scenes.Corruptedwitchscene.GiveBirthToWitches.FertileOutsider".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Corruptedwitchscene.GiveBirthToWitches.AidedUs".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Corruptedwitchscene.GiveBirthToWitches.UniqueFertility".Localized());
                }

                break;

                case 5:
                {
                OutputText("Scenes.Corruptedwitchscene.GiveBirthToWitches.ManySisters".Localized());
                }

                break;
            }

            if (true )
                OutputText("Scenes.Corruptedwitchscene.GiveBirthToWitches.ThankYouAgain".Localized());

            OutputText("Scenes.Corruptedwitchscene.GiveBirthToWitches.DisappearMysteriously".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            DoNext(() => ReturnToCamp(1));
        }

        public void CorrWitchDisappointed()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedwitchscene.CorrWitchDisappointed.TellsAndShows".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchDisappointed.AssRidiculous".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchDisappointed.StillWitches".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void CorrWitchBlessingWomb()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb.TellsInterested".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb.NumbersLimited".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb.OrdersStrip".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb.ThrustsInOneMotion".Localized((Player.VaginalCapacity() > 50 ? "easily reaching your womb" : "reaching your womb with some difficulty")));

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb.SearingPain".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb.PainSubsides".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb.LowersDown".Localized());

            if (Player.Wetness() < 3)
                OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb.DroolingWord".Localized());

            if (Player.Wetness() < 5)
                OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb.GushingWord".Localized());
            else
                OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb.SplatteringWord".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb.FemcumOver".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb.ErectOrDrooling".Localized((Player.CockTotal() > 1 ? "aren't" : "isn't"), (Player.CockTotal() > 1 ? "they don't" : "it doesn't")));

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb.PicksUpPace".Localized());

            DoNext(() => CorrWitchBlessingWomb2());
        }

        public void CorrWitchBlessingWomb2()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb2.WakesToSound".Localized((Player.HasBreasts() ? "and you feel your breasts jiggle backwards and forwards" : "")));

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb2.ThrustingBecoming".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb2.CoilsAround".Localized());

            if (Player.IsGoo())
                OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb2.EnvelopsAround".Localized());
            else
                OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb2.LocksWitch".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb2.PullsHerClose".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb2.PleasureHighEnds".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb2.SomehowDepresses".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessingWomb2.PotentPregnancy".Localized());

            Player.CreatePerk(PerkIds.PotentPregnancy, 0, 0, 0, 0);

            if (!Player.IsPregnant())
                Player.KnockUp(PregnancyIds.CorruptedWitch, PregnancyIds.CorruptedWitchIncubation, 170);

            DoNext(() => ReturnToCamp(2));
        }

        public void CorrWitchBlessing()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessing.TellsInterested".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessing.BlackFlame".Localized());

            if (!Player.HasVagina())
            {
                OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessing.TakesFinger".Localized());

                OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessing.PleasantHeat".Localized());
            }
            else
            {
                OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessing.TakesFingerAgain".Localized());

                OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessing.PleasantHeatAgain".Localized());

                if (Player.Wetness() < 3)
                    OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessing.DroolingWord".Localized());

                if (Player.Wetness() < 5)
                    OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessing.GushingWord".Localized());
                else
                    OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessing.SplatteringWord".Localized());

                OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessing.FemcumOver".Localized());

                OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessing.ClenchLaunches".Localized());

                OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessing.ProstateGoing".Localized());
            }

            Player.Orgasm("Vaginal");

            if (Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()))
                DoNext(() => CorrWitchBlessing2());
            else
                DoNext(() => CorrWitchBlessing2TOOBIG());
        }

        public void CorrWitchBlessing2()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessing2.WakesDistinctive".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessing2.EquipsArmor".Localized());

            Player.CreatePerk(PerkIds.PotentProstate, 0, 0, 0, 0);

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessing2.PotentProstate".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void CorrWitchBlessing2TOOBIG()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessing2TOOBIG.WakesDistinctive".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessing2TOOBIG.EquipsArmor".Localized());

            Player.CreatePerk(PerkIds.PotentProstate, 0, 0, 0, 0);

            OutputText("Scenes.Corruptedwitchscene.CorrWitchBlessing2TOOBIG.PotentProstate".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void StartFight()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedwitchscene.StartFight.TellsNotInterested".Localized());

            if (Player.Libido > 70)
                OutputText("Scenes.Corruptedwitchscene.StartFight.BitOfStandoff".Localized());

            OutputText("Scenes.Corruptedwitchscene.StartFight.ReadiesWeapon".Localized());

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.CorruptedWitch), () => Go<CampScene>());
        }

        public void DefeatWitch()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Health <= 0)
                OutputText("Scenes.Corruptedwitchscene.DefeatWitch.FallsToKnees".Localized());
            else
                OutputText("Scenes.Corruptedwitchscene.DefeatWitch.FallsToKneesAgain".Localized());

            OutputText("Scenes.Corruptedwitchscene.DefeatWitch.ClutchesGround".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Vagina".Localized(), CorrWitchFuckHerVagina);

            AddButton(1, "Common.Button.TooBigtruegrowAPenis".Localized(), CorrWitchGrowaPenis);

            if (Player.IsGenderless())
                AddButton(3, "Common.Button.Genderless".Localized(), CombatScene.CleanupAfterCombat);

            if (Player.HasMultiTails())
                AddNextButton("Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());

            AddButton(14, "Common.Button.Leave".Localized(), () => LeaveWitch());
        }

        public void LeaveWitch()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedwitchscene.LeaveWitch.LeavesWitch".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void CorrWitchFuckHerVagina()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedwitchscene.CorrWitchFuckHerVagina.StripsArmor".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchFuckHerVagina.MakingMost".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchFuckHerVagina.LaysOnMagic".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Corruptedwitchscene.CorrWitchFuckHerVagina.ThrustFeel".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchFuckHerVagina.VaginaClenches".Localized());

            if (Player.CumQ() < 100)
                OutputText("Scenes.Corruptedwitchscene.CorrWitchFuckHerVagina.ReleasesInside".Localized());

            if (Player.CumQ() >= 100 && Player.CumQ() < 1000)
                OutputText("Scenes.Corruptedwitchscene.CorrWitchFuckHerVagina.ReleasesInsideAgain".Localized());

            if (Player.CumQ() >= 1000 && Player.CumQ() < 5000)
                OutputText("Scenes.Corruptedwitchscene.CorrWitchFuckHerVagina.ReleasesInsideThird".Localized());

            if (Player.CumQ() >= 5000)
                OutputText("Scenes.Corruptedwitchscene.CorrWitchFuckHerVagina.ReleasesInsideFourth".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            CorrWitchPregChance();

            Player.Orgasm("Dick");

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void CorrWitchTOOBIGFuck()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuck.StripsArmor".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuck.PantiesDampening".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuck.MovesUnder".Localized());
            else
                OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuck.KneelsNext".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuck.DesiresPeak".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuck.BestToFit".Localized());

            if (Player.Balls == 0)
            {
                if (Player.HasVagina())
                    OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuck.ThrustingInto".Localized());
                else
                    OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuck.MassagingTaint".Localized());
            }
            else
                OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuck.SqueezingBalls".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuck.WithTheOther".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuck.PicksUpPaceSlurping".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuck.LoudGroan".Localized());

            if (Player.CumQ() < 100)
                OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuck.ReleasesInside".Localized((Player.IsTaur() ? "holds onto your hind legs," : "holds onto your hips,")));

            if (Player.CumQ() >= 100 && Player.CumQ() < 1000)
                OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuck.ReleasesInsideAgain".Localized((Player.IsTaur() ? "holds onto your hind legs," : "holds onto your hips,")));

            if (Player.CumQ() >= 1000 && Player.CumQ() < 5000)
                OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuck.ReleasesInsideThird".Localized((Player.IsTaur() ? "holds onto your hind legs," : "holds onto your hips,")));

            if (Player.CumQ() >= 5000)
                OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuck.ReleasesInsideFourth".Localized((Player.IsTaur() ? "holds onto your hind legs," : "holds onto your hips,")));

            CorrWitchPregChance();

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void CorrWitchTOOBIGFuckNoFight()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuckNoFight.StripsArmor".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuckNoFight.PantiesDampening".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuckNoFight.MovesUnder".Localized());
            else
                OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuckNoFight.KneelsNext".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuckNoFight.DesiresPeak".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuckNoFight.BestToFit".Localized());

            if (Player.Balls == 0)
            {
                if (Player.HasVagina())
                    OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuckNoFight.ThrustingInto".Localized());
                else
                    OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuckNoFight.MassagingTaint".Localized());
            }
            else
                OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuckNoFight.SqueezingBalls".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuckNoFight.WithTheOther".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuckNoFight.PicksUpPaceSlurping".Localized());

            OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuckNoFight.LoudGroan".Localized());

            if (Player.CumQ() < 100)
                OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuckNoFight.ReleasesInside".Localized((Player.IsTaur() ? "holds onto your hind legs," : "holds onto your hips,")));

            if (Player.CumQ() >= 100 && Player.CumQ() < 1000)
                OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuckNoFight.ReleasesInsideAgain".Localized((Player.IsTaur() ? "holds onto your hind legs," : "holds onto your hips,")));

            if (Player.CumQ() >= 1000 && Player.CumQ() < 5000)
                OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuckNoFight.ReleasesInsideThird".Localized((Player.IsTaur() ? "holds onto your hind legs," : "holds onto your hips,")));

            if (Player.CumQ() >= 5000)
                OutputText("Scenes.Corruptedwitchscene.CorrWitchTOOBIGFuckNoFight.ReleasesInsideFourth".Localized((Player.IsTaur() ? "holds onto your hind legs," : "holds onto your hips,")));

            CorrWitchPregChance();

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void CorrWitchPregChance()
        {

            var preg = false;

            var temp = (int)(Player.CumQ() / 500);

            if (temp > 5)
                temp = 5;

            temp += Player.VirilityQ() * 2;

            if (Player.CumQ() > 250 && temp >= Random.Shared.Next(100))
                preg = true;

            if (preg)
                CoC.Instance.Flags.Set(GameFlagIds.Volcwitchnumberofchildren, CoC.Instance.Flags.Get(GameFlagIds.Volcwitchnumberofchildren) + 1);
        }

        public void MindControlBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEnd.WrenchedOut".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEnd.DesperationArrests".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEnd.SaysOutLoud".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEnd.SmilesExtends".Localized());

            if (Player.HasCock())
            {
                DoNext(() => MindControlBadEndCock());

                return ;
            }

            if (Player.HasVagina())
            {
                DoNext(() => MindControlBadEndVagina());

                return ;
            }
        }

        public void MindControlBadEndCock()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndCock.VisionFogs".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndCock.WakesUp".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndCock.Whimpers".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndCock.ManaclesUnlocked".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndCock.ReachesToGrab".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndCock.NodsCannotDeny".Localized());

            DoNext(() => MindControlBadEndCock2());
        }

        public void MindControlBadEndCock2()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndCock2.OneMonthLater".Localized((Player.CockTotal() > 1 ? "[cocks] are" : "[cock] is")));

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndCock2.BreedingChamberDoors".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndCock2.EyesWidenDesperation".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndCock2.OrgasmUnleashes".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndCock2.GloriousAs".Localized());

            GameOver();
        }

        public void MindControlBadEndVagina()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndVagina.WakesUp".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndVagina.Whimpers".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndVagina.ManaclesUnlocked".Localized());

            if (Player.IsPregnant())
                OutputText("Scenes.Corruptedwitchscene.MindControlBadEndVagina.LubricantMention".Localized());
            else
                OutputText("Scenes.Corruptedwitchscene.MindControlBadEndVagina.DepressingEmptiness".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndVagina.DesiresPeak".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndVagina.NodsCannotDeny".Localized());

            DoNext(() => MindControlBadEndVagina2());
        }

        public void MindControlBadEndVagina2()
        {
            ClearOutput();

            Player.Vaginas[0].Wetness = 5;

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndVagina2.OneMonthLater".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndVagina2.BreedingChamberDoors".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndVagina2.NearlyCries".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndVagina2.DeliciousMoments".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndVagina2.ChoosesYou".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndVagina2.HiltsAgain".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Corruptedwitchscene.MindControlBadEndVagina2.CoilsAround".Localized());

            if (Player.IsGoo())
                OutputText("Scenes.Corruptedwitchscene.MindControlBadEndVagina2.EnvelopsAround".Localized());
            else
                OutputText("Scenes.Corruptedwitchscene.MindControlBadEndVagina2.LocksWitch".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndVagina2.EjaculationComing".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndVagina2.WombIsntReady".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndVagina2.VaginaTingling".Localized());

            OutputText("Scenes.Corruptedwitchscene.MindControlBadEndVagina2.GloriousAs".Localized());

            GameOver();
        }

        public void LoseToWitch()
        {
            ClearOutput();

            if (Player.HasCock())
                Player.Orgasm("Dick");
            else
                Player.Orgasm("Vaginal");

            if (Player.Health <= 0 && Player.Gender != Gender.Genderless)
            {
                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.CollapsesInjured".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.UncorksFlask".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.PlansChange".Localized());

                if (Player.HasCock())
                {
                    OutputText("Scenes.Corruptedwitchscene.LoseToWitch.MuskySmell".Localized());

                    Player.GoIntoRut(false, 2);
                }

                if (Player.HasVagina())
                {
                    OutputText("Scenes.Corruptedwitchscene.LoseToWitch.MoistensFeel".Localized());

                    Player.GoIntoHeat(false, 2);
                }

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.WhatWasThat".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.LustDraft".Localized());
            }

            OutputText("Scenes.Corruptedwitchscene.LoseToWitch.CollapsesDesires".Localized(
                 Player.ClothedOrNakedLower(" ripping off your [armor] and")
            ));

            OutputText("Scenes.Corruptedwitchscene.LoseToWitch.SitsNearby".Localized());

            OutputText("Scenes.Corruptedwitchscene.LoseToWitch.NotSureDefying".Localized());

            OutputText("Scenes.Corruptedwitchscene.LoseToWitch.RemovesRobes".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.HardeningBegging".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.LeakingLubricant".Localized());

            OutputText("Scenes.Corruptedwitchscene.LoseToWitch.ProveYourself".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
            {
                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.KneelsDesperate".Localized(
                     Player.ClothedOrNakedLower(", stripping off your [armor], ")
                ));

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.RigidParalyzed".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.WhispersPlea".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.BeforeWordsFinish".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.ReleasedFromSpell".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.LeansAgainstWall".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.BodyTenses".Localized());

                if (Player.CumQ() < 100)
                    OutputText("Scenes.Corruptedwitchscene.LoseToWitch.UnloadsInside".Localized());

                if (Player.CumQ() >= 100 && Player.CumQ() < 1000)
                    OutputText("Scenes.Corruptedwitchscene.LoseToWitch.UnloadsInsideAgain".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Corruptedwitchscene.LoseToWitch.UnloadsInsideThird".Localized());

                if (Player.VaginalCapacity() >= 100)
                    OutputText("Scenes.Corruptedwitchscene.LoseToWitch.WombHappilyReceives".Localized());

                if (Player.VaginalCapacity() < 100 && Player.VaginalCapacity() > 50)
                    OutputText("Scenes.Corruptedwitchscene.LoseToWitch.WombManagesReceive".Localized());

                if (Player.VaginalCapacity() <= 50)
                    OutputText("Scenes.Corruptedwitchscene.LoseToWitch.WombBarelyReceives".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.SatisfiedFalls".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.LeavesYouSleep".Localized());

                CorrWitchPregChance();

                if (!Player.IsPregnant())
                    Player.KnockUp(PregnancyIds.CorruptedWitch, PregnancyIds.CorruptedWitchIncubation, 120);
            }

            if (Player.Gender == Gender.Female)
            {
                Player.CuntChange(8, true, true, false);

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.KneelsDesperateAgain".Localized(
                     Player.ClothedOrNakedLower(", stripping off your [armor], ")
                ));

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.RigidParalyzedAgain".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.WhispersPleaAgain".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.BeforeWordsFinishAgain".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.ReleasedFromSpellAgain".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.LeansAgainstWallAgain".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.BodyTensesAgain".Localized());

                if (Player.VaginalCapacity() >= 100)
                    OutputText("Scenes.Corruptedwitchscene.LoseToWitch.WombHappilyReceivesAgain".Localized());

                if (Player.VaginalCapacity() < 100 && Player.VaginalCapacity() > 50)
                    OutputText("Scenes.Corruptedwitchscene.LoseToWitch.WombManagesReceiveAgain".Localized());

                if (Player.VaginalCapacity() <= 50)
                    OutputText("Scenes.Corruptedwitchscene.LoseToWitch.WombBarelyReceivesAgain".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.SatisfiedFallsAgain".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.LeavesYouSleepAgain".Localized());

                if (!Player.IsPregnant())
                    Player.KnockUp(PregnancyIds.CorruptedWitch, PregnancyIds.CorruptedWitchIncubation, 120);
            }

            if (Player.Gender == Gender.Male)
            {
                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.KneelsDesperateThird".Localized(
                     Player.ClothedOrNakedLower(", stripping off your [armor], ")
                ));

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.RigidParalyzedThird".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.WhispersPleaThird".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.BeforeWordsFinishThird".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.SuddenlyReleased".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.SitsDownNearby".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.MoansSoftly".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.BodyTensesThird".Localized());

                if (Player.CumQ() < 100)
                    OutputText("Scenes.Corruptedwitchscene.LoseToWitch.FewJets".Localized());

                if (Player.CumQ() >= 100 && Player.CumQ() < 1000)
                    OutputText("Scenes.Corruptedwitchscene.LoseToWitch.SeveralJets".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Corruptedwitchscene.LoseToWitch.Torrent".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.SatisfiedFallsThird".Localized());

                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.LeavesYouSleepThird".Localized());

                CorrWitchPregChance();

                CombatScene.CleanupAfterCombat();
            }

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Corruptedwitchscene.LoseToWitch.LacksGenitals".Localized());

            CombatScene.CleanupAfterCombat();
        }

    }
}