using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Configuration;
using System;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Factory
{
    [Register<OmnibusOverseerScene>("coc:omnibus_overseer")]
    public class OmnibusOverseerScene : Scene
    {

        public override void Entry()
        {

        }

        public void EncounterOmnibus()
        {

            ClearButtons();

            OutputText("Scenes.Omnibusoverseerscene.EncounterOmnibus.NudeDemonicWoman".Localized());

            OutputText("Scenes.Omnibusoverseerscene.EncounterOmnibus.PacesDesk".Localized());

            OutputText("Scenes.Omnibusoverseerscene.EncounterOmnibus.WantsToDeny".Localized());

            OutputText("Scenes.Omnibusoverseerscene.EncounterOmnibus.SheCoos".Localized());

            AddButton(0, "Common.Button.Fight".Localized(), DoFightOmnibus);

            AddButton(1, "Common.Button.Accept".Localized(), AcceptOmnibus);
        }

        public void DoFightOmnibus()
        {
            ClearOutput();

            OutputText("Scenes.Omnibusoverseerscene.DoFightOmnibus.CombatPose".Localized());

            OutputText("Scenes.Omnibusoverseerscene.DoFightOmnibus.SheTaunts".Localized());

            OutputText("Scenes.Omnibusoverseerscene.DoFightOmnibus.GlassDoorCloses".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FactoryOmnibusDefeated, 1);

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "omnibus_overseer")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));

            return;
        }

        public void AcceptOmnibus()
        {
            ClearOutput();

            OutputText("Scenes.Omnibusoverseerscene.AcceptOmnibus.SauntersCloser".Localized());

        }

        public void DoLossOmnibus()
        {

            DoBadEndOmnibusPart1();
        }

        public void WinAgainstOmnibus()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
            {
                OutputText("Scenes.Omnibusoverseerscene.WinAgainstOmnibus.OmnibusTrembles".Localized());

                OutputText("Scenes.Omnibusoverseerscene.WinAgainstOmnibus.SheCries".Localized());
            }
            else
            {
                OutputText("Scenes.Omnibusoverseerscene.WinAgainstOmnibus.DropsToKnees".Localized());

                OutputText("Scenes.Omnibusoverseerscene.WinAgainstOmnibus.SheBegs".Localized());
            }

            OutputText("Scenes.Omnibusoverseerscene.WinAgainstOmnibus.UseHerBoon".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.GrowBreasts".Localized(), ChooseBreasts);

            AddButton(1, "Scenes.Kitsune.DefeatTheKitsunes.ButtonGrowDick".Localized(), ChooseDick);

            AddButton(2, "Common.Button.NormalFace".Localized(), NormalFace);

            AddButton(3, "Common.Button.NormalChest".Localized(), NormalChest);

            AddButton(4, "Common.Button.NormalGroin".Localized(), NormalGroin);

            AddButton(5, "Common.Button.NormalLegs".Localized(), NormalLegs);

            if (OptionStore.Instance.Get("taint", 0) > 0 && !Player.HasPerk(PerkIds.PurityBlessing))
                AddButton(6, "Common.Button.RemoveCurse".Localized(), RemoveCurse);

            AddButton(7, "Common.Button.MoreLust".Localized(), () => PostOmnibusBoon(true));

            AddButton(9, "Common.Button.NoLetGo".Localized(), LetGoOmnibus);

            AddButton(10, "Common.Button.NoKillHer".Localized(), KillOmnibus);
        }

        public void ChooseBreasts()
        {
            ClearOutput();

            if (Player.BreastRows.Count == 0)
            {
                OutputText("Scenes.Omnibusoverseerscene.ChooseBreasts.ChestSwells".Localized());

                Player.CreateBreastRow();

                Player.BreastRows[0].BreastRating = 3;

                Player.NippleLength = .25f;
            }
            else
                if (Player.BreastRows.Count <= 1)
                {
                    OutputText("Scenes.Omnibusoverseerscene.ChooseBreasts.BreastsTingle".Localized(
                        Appearance.AllBreastsDescript(Player)
                    ));

                    Player.BreastRows[0].BreastRating += 3;

                    if (Player.NippleLength < .75f)
                    {
                        Player.NippleLength += .5f;

                        OutputText("Scenes.Omnibusoverseerscene.ChooseBreasts.NipplesHard".Localized());
                    }
                }
                else
                {
                    OutputText("Scenes.Omnibusoverseerscene.ChooseBreasts.TopTingle".Localized());

                    Player.BreastRows[0].BreastRating += 4;

                    OutputText("Scenes.Omnibusoverseerscene.ChooseBreasts.NextRowJiggle".Localized(
                        Player.BreastDescript(1)
                    ));

                    Player.BreastRows[1].BreastRating = Player.BreastRows[0].BreastRating - 1;

                    if (Player.BreastRows.Count >= 3)
                    {
                        OutputText("Scenes.Omnibusoverseerscene.ChooseBreasts.ThirdGroup".Localized(
                            Player.BreastDescript(2)
                        ));

                        Player.BreastRows[2].BreastRating = Player.BreastRows[1].BreastRating - 1;
                    }

                    if (Player.BreastRows.Count >= 4)
                    {
                        OutputText("Scenes.Omnibusoverseerscene.ChooseBreasts.RemainingWonderful".Localized(
                            Player.BreastDescript(3)
                        ));

                        Player.BreastRows[3].BreastRating = Player.BreastRows[2].BreastRating - 1;

                        if (Player.BreastRows.Count == 5)
                            Player.BreastRows[4].BreastRating = Player.BreastRows[3].BreastRating - 1;
                    }

                    if (Player.NippleLength < .75f)
                    {
                        Player.NippleLength += .5f;

                        OutputText("Scenes.Omnibusoverseerscene.ChooseBreasts.NipplesHardAgain".Localized());
                    }
                }

            OutputText("Scenes.Omnibusoverseerscene.ChooseBreasts.ParagraphBreak".Localized());

            PostOmnibusBoon();
        }

        public void RemoveCurse()
        {
            ClearOutput();

            OutputText("Scenes.Omnibusoverseerscene.RemoveCurse.KindOfCurse".Localized());

            OutputText("Scenes.Omnibusoverseerscene.RemoveCurse.WeavesMagic".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ZetazPotionCured, 1);

            OutputText("Scenes.Omnibusoverseerscene.RemoveCurse.ColdnessRecedes".Localized());

            PostOmnibusBoon();
        }

        public void ChooseDick()
        {
            ClearOutput();

            int temp = 0;

            if (Player.Cocks.Count == 0)
            {
                OutputText("Scenes.Omnibusoverseerscene.ChooseDick.PressureBuilds".Localized());

                if (Player.Corruption < 80)
                    OutputText("Scenes.Omnibusoverseerscene.ChooseDick.HorrorSkin".Localized());
                else
                    OutputText("Scenes.Omnibusoverseerscene.ChooseDick.CuriousSkin".Localized());

                Player.CreateCock();

                Player.Cocks[0].Id = BodyPartTypes.Demon;

                Player.Cocks[0].Length = 10;

                Player.Cocks[0].Thickness = 2;
            }

            if (Player.Cocks.Count == 1)
                if (Player.CountCocksOfType("DEMON") < 1)
                {
                    OutputText("Scenes.Omnibusoverseerscene.ChooseDick.SmilesLonger".Localized());

                    if (Player.Corruption < 80)
                        OutputText("Scenes.Omnibusoverseerscene.ChooseDick.HorrorCockSkin".Localized());
                    else
                        OutputText("Scenes.Omnibusoverseerscene.ChooseDick.CuriousCockSkin".Localized());

                    Player.IncreaseCock(0, 3 + Random.Shared.Next(5));

                    Player.Cocks[0].Thicken(2);

                    Player.Cocks[0].Id = BodyPartTypes.Demon;

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Omnibusoverseerscene.ChooseDick.CorruptNodules".Localized(
                            Player.CockDescript(0)
                        ));
                    else
                        OutputText("Scenes.Omnibusoverseerscene.ChooseDick.WrigglingNodules".Localized(
                            Player.CockDescript(0),

                            Player.CockDescript(0)
                        ));
                }
                else
                {
                    OutputText("Scenes.Omnibusoverseerscene.ChooseDick.LeapsForwards".Localized(
                        Player.CockDescript(0)
                    ));

                    Player.Cocks[0].Length += 6 + Random.Shared.Next(10);

                    Player.Cocks[0].Thicken(3);
                }

            if (Player.Cocks.Count > 1)
                if (Player.CountCocksOfType("DEMON") == Player.CockTotal())
                {
                    OutputText("Scenes.Omnibusoverseerscene.ChooseDick.LeapForwards".Localized(
                        Player.MultiCockDescriptLight()
                    ));

                    temp = Player.Cocks.Count;

                    while (temp > 0)
                    {
                        temp--;

                        Player.IncreaseCock(temp, 6 + Random.Shared.Next(10));

                        Player.Cocks[temp].Thicken(3);
                    }
                }
                else
                {
                    OutputText("Scenes.Omnibusoverseerscene.ChooseDick.SmilesLongerAgain".Localized());

                    if (Player.Corruption < 80)
                        OutputText("Scenes.Omnibusoverseerscene.ChooseDick.InHorrorWord".Localized());
                    else
                        OutputText("Scenes.Omnibusoverseerscene.ChooseDick.CuriouslyWord".Localized());

                    OutputText("Scenes.Omnibusoverseerscene.ChooseDick.AsSkinTurns".Localized());

                    temp = Player.Cocks.Count;

                    while (temp > 0)
                    {
                        temp--;

                        Player.Cocks[temp].Length += 3 + Random.Shared.Next(5);

                        Player.Cocks[temp].Thicken(2);

                        Player.Cocks[temp].Id = BodyPartTypes.Demon;
                    }

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Omnibusoverseerscene.ChooseDick.DicksTransforming".Localized(
                            Player.MultiCockDescriptLight()
                        ));
                    else
                        OutputText("Scenes.Omnibusoverseerscene.ChooseDick.DicksTransformingAgain".Localized(
                            Player.MultiCockDescriptLight(),

                            Player.MultiCockDescriptLight()
                        ));
                }

            PostOmnibusBoon();

            return;
        }

        public void NormalFace()
        {
            ClearOutput();

            if (Player.Body.Get<HornsPart>().Id != BodyPartTypes.None || Player.Body.Get<AntennaePart>().Id != BodyPartTypes.None)
            {
                OutputText("Scenes.Omnibusoverseerscene.NormalFace.ForeheadItches".Localized());

                if (Player.Horns.Id.Path != "none")
                {
                    OutputText("Scenes.Omnibusoverseerscene.NormalFace.HornsFallOff".Localized());

                    Player.Body.Get<HornsPart>().Id = BodyPartTypes.None;
                }

                Player.Body.Get<AntennaePart>().Restore();
            }

            if (Player.Ears.Id != BodyPartTypes.Human)
            {
                OutputText("Scenes.Omnibusoverseerscene.NormalFace.EarsReform".Localized());

                Player.Ears.Id = BodyPartTypes.Human;
            }

            if (Player.Body.Get<FacePart>().Id != BodyPartTypes.Human)
            {
                OutputText("Scenes.Omnibusoverseerscene.NormalFace.FaceRearranges".Localized());

                Player.Body.Get<FacePart>().Id = BodyPartTypes.Human;
            }

            PostOmnibusBoon();
        }

        public void NormalChest()
        {
            ClearOutput();

            var temp = 0;

            if (Player.BreastRows.Count > 1)
            {
                Player.RemoveBreastRow(1, Player.BreastRows.Count - 1);

                OutputText("Scenes.Omnibusoverseerscene.NormalChest.ChestLighter".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

                if (Player.NippleLength > 1)
                {
                    OutputText("Scenes.Omnibusoverseerscene.NormalChest.NipplesShrink".Localized());

                    Player.NippleLength = .75f;
                }

                temp += 1;
            }

            if (Player.BreastRows[0].BreastRating > 7)
            {
                OutputText("Scenes.Omnibusoverseerscene.NormalChest.WeightyFlesh".Localized());

                Player.BreastRows[0].BreastRating = 3 + Random.Shared.Next(5);

                OutputText("Scenes.Omnibusoverseerscene.NormalChest.YouNowHave".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

                temp += 1;
            }

            if (Player.NippleLength > 1)
            {
                OutputText("Scenes.Omnibusoverseerscene.NormalChest.NipplesShrinkAgain".Localized());

                Player.NippleLength = .75f;

                temp += 1;
            }

            if (Player.HasFuckableNipples())
            {
                OutputText("Scenes.Omnibusoverseerscene.NormalChest.NippleOpeningsClose".Localized());

                Player.BreastRows[0].Fuckable = false;

                temp += 1;
            }

            if (!Player.HasPlainSkin())
            {
                OutputText("Scenes.Omnibusoverseerscene.NormalChest.SkinSheds".Localized());

                Player.Body.Get<SkinPart>().DescKey = "Skin.Desc.skin";

                Player.Body.Get<SkinPart>().Id = BodyPartTypes.Plain;

                Player.Body.Get<UnderBodyPart>().Restore();

                temp += 1;
            }

            if (temp == 0)
                OutputText("Scenes.Omnibusoverseerscene.NormalChest.NoObviousChange".Localized());

            PostOmnibusBoon();
        }

        public void NormalGroin()
        {
            ClearOutput();

            var temp = 0;

            OutputText("Scenes.Omnibusoverseerscene.NormalGroin.ShiveringSensation".Localized());

            if (Player.Cocks.Count > 1)
            {
                OutputText("Scenes.Omnibusoverseerscene.NormalGroin.CocksRetract".Localized());

                Player.RemoveCock(1, Player.Cocks.Count - 1);

                temp += 1;
            }

            if (Player.HasCock())
            {
                if (Player.Cocks[0].Length > 12)
                {
                    OutputText("Scenes.Omnibusoverseerscene.NormalGroin.TinglingWorms".Localized());

                    Player.Cocks[0].Length = 11;

                    temp += 1;
                }

                if (Player.Cocks[0].Thickness > 2)
                {
                    OutputText("Scenes.Omnibusoverseerscene.NormalGroin.ThicknessWithers".Localized());

                    Player.Cocks[0].Thickness = 2;

                    temp += 1;
                }

                if (Player.Cocks[0].Id != BodyPartTypes.Human && Player.Cocks[0].Id != BodyPartTypes.Demon)
                {
                    OutputText("Scenes.Omnibusoverseerscene.NormalGroin.InhumanShifts".Localized());

                    Player.Cocks[0].Id = BodyPartTypes.Human;

                    temp += 1;
                }

                if (Player.Cocks[0].Id == BodyPartTypes.Demon)
                {
                    OutputText("Scenes.Omnibusoverseerscene.NormalGroin.BumpsFade".Localized());

                    temp += 1;
                }
            }

            if (Player.BallSize > 5)
            {
                temp += 1;

                OutputText("Scenes.Omnibusoverseerscene.NormalGroin.BallsPull".Localized());

                Player.BallSize = 2 + Random.Shared.Next(3);
            }

            if (temp > 0)
                OutputText("Scenes.Omnibusoverseerscene.NormalGroin.ParagraphBreak".Localized());

            if ((Player.Vaginas.Count > 0) && (Player.Vaginas[0].Wetness >= 3))
                {
                    OutputText("Scenes.Omnibusoverseerscene.NormalGroin.VaginaFluids".Localized(
                        Player.VaginaDescript(0)
                    ));

                    Player.Vaginas[0].Wetness = 2;

                    temp += 1;
                }

            if (Player.Gender == Gender.Genderless)
            {
                if (Player.Balls > 0 || Player.Femininity < 35 || Random.Shared.Next(3) == 0)
                {
                    Player.CreateCock();

                    Player.Cocks[0].Id = BodyPartTypes.Demon;

                    Player.Cocks[0].Length = 10;

                    Player.Cocks[0].Thickness = 2;

                    OutputText("Scenes.Omnibusoverseerscene.NormalGroin.PressureBuilds".Localized());

                    if (Player.Corruption < 80)
                        OutputText("Scenes.Omnibusoverseerscene.NormalGroin.HorrorSkin".Localized());
                    else
                        OutputText("Scenes.Omnibusoverseerscene.NormalGroin.CuriousSkin".Localized());

                    if (Player.Balls == 0)
                    {
                        Player.Balls = 2;

                        OutputText("Scenes.Omnibusoverseerscene.NormalGroin.IncrediblePain".Localized());

                        Player.BallSize = 1;
                    }
                }
                else
                {
                    Player.CreateVagina();

                    OutputText("Scenes.Omnibusoverseerscene.NormalGroin.ItchingStarts".Localized(
                        Player.VaginaDescript(0)
                    ));
                }

                temp += 1;

                OutputText("Scenes.Omnibusoverseerscene.NormalGroin.ParagraphBreakAgain".Localized());
            }

            if (Player.Anal.Wetness >= 3)
            {
                OutputText("Scenes.Omnibusoverseerscene.NormalGroin.AssFluids".Localized(
                    Player.AssDescript()
                ));

                Player.Anal.Wetness = 2;

                temp += 1;
            }

            if (temp == 0)
                OutputText("Scenes.Omnibusoverseerscene.NormalGroin.NoObviousChange".Localized());

            PostOmnibusBoon();
        }

        public void NormalLegs()
        {
            ClearOutput();

            if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Human)
                OutputText("Scenes.Omnibusoverseerscene.NormalLegs.ShouldSlapSelf".Localized());
            else
                OutputText("Scenes.Omnibusoverseerscene.NormalLegs.CollapseLegs".Localized());

            Player.Body.Get<LowerBodyPart>().Id = BodyPartTypes.Human;

            Player.Body.Get<LowerBodyPart>().LegCount = 2;

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
            {
                OutputText("Scenes.Omnibusoverseerscene.NormalLegs.SomethingDetaches".Localized());

                Player.Body.Get<TailPart>().Id = BodyPartTypes.None;

                Player.Body.Get<TailPart>().Venom = 0;

                Player.Body.Get<TailPart>().Recharge = 5;
            }

            PostOmnibusBoon();
        }

        public void PostOmnibusBoon(bool willing = false)
        {
            ClearOutput();

            if (willing)
            {
                OutputText("Scenes.Omnibusoverseerscene.PostOmnibusBoon.OmnibusDisappeared".Localized());

                OutputText("Scenes.Omnibusoverseerscene.PostOmnibusBoon.MasturbatesQuickly".Localized());

                OutputText("Scenes.Omnibusoverseerscene.PostOmnibusBoon.ExactlyWhatAsked".Localized());

                OutputText("Scenes.Omnibusoverseerscene.PostOmnibusBoon.PerkGained".Localized());
            }
            else
                OutputText("Scenes.Omnibusoverseerscene.PostOmnibusBoon.OmnibusDisappearedAgain".Localized());

            Player.CreatePerk(PerkIds.OmnibusGift, 0, 0, 0, 0);

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

            CombatScene.CleanupAfterCombat();
        }

        public void LetGoOmnibus()
        {

            OutputText("Scenes.Omnibusoverseerscene.LetGoOmnibus.RefusesPloy".Localized());

            OutputText("Scenes.Omnibusoverseerscene.LetGoOmnibus.SprintsOut".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void KillOmnibus()
        {
            ClearOutput();

            if (Player.Weapon.IsHolySword())
                OutputText("Scenes.Omnibusoverseerscene.KillOmnibus.RighteousPower".Localized());
            else
                if (Player.Weapon.IsScythe())
                    OutputText("Scenes.Omnibusoverseerscene.KillOmnibus.OverseerBroken".Localized());
                else
                    if (Player.Weapon.Id == WeaponIds.Blunderbuss)
                        OutputText("Scenes.Omnibusoverseerscene.KillOmnibus.NoMercy".Localized());
                    else
                        if (Player.Weapon.IsFirearm())
                            OutputText("Scenes.Omnibusoverseerscene.KillOmnibus.SwingsTemple".Localized());
                        else
                            if (Player.Weapon.IsBlunt())
                                OutputText("Scenes.Omnibusoverseerscene.KillOmnibus.GripAndSwing".Localized());
                            else
                                if (Player.Weapon.IsAxe())
                                    OutputText("Scenes.Omnibusoverseerscene.KillOmnibus.HeavyPunch".Localized());
                                else
                                    if (Player.Weapon.IsSharp())
                                        OutputText("Scenes.Omnibusoverseerscene.KillOmnibus.AdvancesOverseer".Localized());
                                    else
                                        OutputText("Scenes.Omnibusoverseerscene.KillOmnibus.GrabsHead".Localized());

            Player.UpgradeBeautifulSword();

            CoC.Instance.Flags.Set(GameFlagIds.D1OmnibusKilled, 1);

            CombatScene.CleanupAfterCombat();
        }

        public void DoBadEndOmnibusPart1()
        {
            ClearOutput();

            if (Player.Health < 1)
                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart1.StaggersDesk".Localized());
            else
                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart1.LeansOnDesk".Localized());

            OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart1.SidlesUp".Localized());

            OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart1.JustTheThing".Localized());

            if ((int)Player.Gender <= 1)
                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart1.ManWord".Localized());
            else
                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart1.WomanWord".Localized());

            OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart1.CrossbreedingParasites".Localized());

            OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart1.LookOfFear".Localized());

            OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart1.OpensDrawer".Localized());

            DoNext(() => DoBadEndOmnibusPart2());
        }

        public void DoBadEndOmnibusPart2()
        {
            ClearOutput();

            if (Player.Cocks.Count > 1)
            {
                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.GreenMass".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.ForSomeoneDifferent".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.DesperateEscape".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.AndBallsFull".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.PleasureWashes".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.DetachedFascination".Localized(
                    Player.MultiCockDescript()
                ));

                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.BallsWord".Localized());

                    if (Player.Vaginas.Count > 0)
                        OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.AndVagina".Localized(
                            Player.VaginaDescript(0)
                        ));
                }
                else
                    if (Player.Vaginas.Count > 0)
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.TaintWord".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.SlidingAbdomen".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.SoothingVoice".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.NodsUnaware".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.VoiceLaughs".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.OverloadedPleasure".Localized());

                if (Player.CumQ() < 50)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.ApplesWord".Localized());

                if (Player.CumQ() >= 50 && Player.CumQ() < 300)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.CantaloupesWord".Localized());

                if (Player.CumQ() >= 300)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.WatermelonsWord".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.EndOfEachDick".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.BeingUsedForCum".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.GreatestFetish".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.FoodSource".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.FlashWantToCum".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.TendrilsCrawl".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.FlashNeedToCum".Localized());

                if (Player.BreastRows.Count == 1)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.LowerCurve".Localized());

                if (Player.BreastRows.Count > 1)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.LowestPair".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.FlashLoveCumming".Localized());

                if (Player.BreastRows.Count == 3)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.MiddleBreasts".Localized());

                if (Player.BreastRows.Count == 2)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.PlantSummit".Localized());

                if (Player.BreastRows.Count == 1)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.HardAsSteel".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.FlashToldToOrgasm".Localized());

                if (Player.BreastRows.Count == 1)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.WrigglingSlidesUp".Localized());

                if (Player.BreastRows.Count >= 2)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.ClimbsTopPair".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.FlashToObey".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.OtherPet".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.FlashingSubsides".Localized(
                    NumberUtils.Num2Text(Player.Cocks.Count)
                ));
                // 原版 OmnibusOverseerScene.hx:534 — AikoScene.hx:1889 / FactoryScene.hx 等：Marble 是跟随者
                if (SceneInstance<Npcs.MarbleScene>.Get().MarbleFollower())
                {
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.LoudScream".Localized());

                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.MistressTurnsBack".Localized());

                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.FineReplacement".Localized());
                }
                else
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.LooksApproval".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.StayHere".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.HappyTask".Localized());

                GameOver();

                return;
            }

            if (Player.Cocks.Count == 1)
            {
                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.SquirmingMass".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.PredatorySmile".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.SweetWord".Localized());

                if (Player.Corruption >= 66)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.TaintedWord".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.SexualEnergies".Localized());

                if (Player.AverageCockLength() < 15)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.BeastEngulfs".Localized());
                else
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.DistendingObscenely".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.SlimyTentacles".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.SmothersBalls".Localized());
                else
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.OverTaint".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.ThreeTendrils".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.TryToEndure".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.EvilGlint".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.CocksHead".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.BurrowDeeper".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.MottledSurface".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.CoosPetting".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.GoodReward".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.TeasingBalls".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.NotEvenTouching".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.PantsGroans".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.ObediaSlave".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.GoodToy".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.PatsPanties".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.AndBalls".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.OversizedProstate".Localized());
                // 原版 OmnibusOverseerScene.hx:534 — AikoScene.hx:1889 / FactoryScene.hx 等：Marble 是跟随者
                if (SceneInstance<Npcs.MarbleScene>.Get().MarbleFollower())
                {
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.LoudScreamAgain".Localized());

                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.MistressTurnsBackAgain".Localized());

                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.FineReplacementAgain".Localized());
                }

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.PatsHead".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.DaysToCome".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.AfterMonth".Localized());

                GameOver();

                return;
            }

            if (Player.Vaginas.Count == 1 || Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.SeamlessPanties".Localized());

                if (Player.Gender == Gender.Genderless)
                {
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.YanksBottom".Localized());

                    Player.CreateVagina();
                }
                else
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.YanksArmor".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.LivingCreature".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.PullsBack".Localized());

                if (Player.Vaginas[0].Wetness <= 2)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.WetWord".Localized());
                else
                    if (Player.Vaginas[0].Wetness <= 4)
                        OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.DripOnChair".Localized());
                    else
                        OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.SoakChair".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.HeadyTaste".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.ShakesHead".Localized(
                    Player.VaginaDescript(0)
                ));

                Player.CuntChange(6, true, false);

                Player.ButtChange(6, true, false);

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.BrightRedEyes".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.BlinksTears".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.AttunedKind".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.WontBeRapist".Localized());
                // 原版 OmnibusOverseerScene.hx:534 — AikoScene.hx:1889 / FactoryScene.hx 等：Marble 是跟随者
                if (SceneInstance<Npcs.MarbleScene>.Get().MarbleFollower())
                {
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.LoudScreamThird".Localized());

                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.MistressTurnsBackThird".Localized());

                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.FineReplacementThird".Localized());
                }

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.TakesSecond".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.LiftsUp".Localized(
                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.GigglesReaches".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

                if (Player.NippleLength < .5f)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.ErasersWord".Localized());
                else
                    if (Player.NippleLength < 3)
                        OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.BulletsWord".Localized());
                    else
                        OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.TinyCocksWord".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.InMoments".Localized());

                if (Player.BiggestLactation() > 2)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.DripMilk".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.SighDisappointment".Localized());

                if (Player.BreastRows.Count <= 1)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.YourNipple".Localized());
                else
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.YourTopNipple".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.StarShaped".Localized());

                if (Player.BiggestLactation() > 1)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.MilkErupts".Localized());

                if (Player.BreastRows.Count > 1)
                    OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.MeatPole".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.DemonicFlood".Localized(
                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.SitsUp".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.SetsOff".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.OneMonthLater".Localized());

                OutputText("Scenes.Omnibusoverseerscene.DoBadEndOmnibusPart2.LicksJism".Localized());

                GameOver();

                return;
            }
        }

    }
}