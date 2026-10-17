using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Boat
{
    [Register<SharkGirlScene>("coc:shark_girl_scene")]
    public class SharkGirlScene : Scene
    {

        public override void Entry()
        {

            SharkGirlEncounter(Player.Location is "boat" or "swim" ? 1 : 0);
        }

        public void SharkGirlEncounter(float exploreLoc = 0)
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaEncounterCounter) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.IzmaEncounterCounter, 1);

            if (!Player.HasStatusEffect(StatusEffectIds.SharkGirl))
                Player.CreateStatusEffect(StatusEffectIds.SharkGirl, 0, 0, 0, 0);
            else
                if (Player.StatusEffectValue(StatusEffectIds.SharkGirl, 0) >= 7 && Player.Cocks.Count > 0)
                {
                    SharkBadEnd();

                    return ;
                }

            if (exploreLoc == 1)
            {
                OutputText("Scenes.Sharkgirlscene.SharkGirlEncounter.SpiesSharkGirl".Localized());

                if (Player.Location == "swim")
                    OutputText("Scenes.Sharkgirlscene.SharkGirlEncounter.SwishesTail".Localized());
            }
            else
                OutputText("Scenes.Sharkgirlscene.SharkGirlEncounter.WandersShores".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkGirlEncounter.GrayBlur".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkGirlEncounter.CorruptedWoman".Localized());

            Player.UnlockCodexEntry(GameFlagIds.CodexEntrySharkgirls);

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "shark_girl")), () => Go<CampScene>());
        }

        public void SharkWinChoices()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Sharkgirlscene.SharkWinChoices.FallsDefeated".Localized());
            else
            {
                OutputText("Scenes.Sharkgirlscene.SharkWinChoices.BeginsMasturbating".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 15f));
            }

            ClearButtons();

            SexButton(AddButton(0, "Common.Button.UseDick".Localized(), SharkgirlDickFuck), Gender.Male);

            SexButton(AddButton(1, "Common.Button.TailDick".Localized(), SharkgirlDickTail), Gender.Male)

                ?.DisableIf(!Player.IsNaga(), "You need to be a naga to do this.");

            SexButton(AddButton(2, "Common.Button.Facesitting".Localized(), SharkgirlFacesitting), Gender.Female);

            AddButton(3, "Common.Button.DildoRape".Localized(), SharkGirlGetsDildoed)

                ?.DisableIf(!Player.HasKeyItem("Deluxe Dildo"), "This scene requires you to have the Deluxe Dildo.");

            SexButton(AddButton(4, "Common.Button.Hypnosis".Localized(), SharkgirlHypnosis), AnyGender)

                ?.DisableIf(!Player.IsNaga(), "You need to be a naga to do this.");

            AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
        }

        public void SharkgirlDickFuck()
        {
            ClearOutput();

            Player.ChangeStatusValue(StatusEffectIds.SharkGirl, 0, 1);

            OutputText("Scenes.Sharkgirlscene.SharkgirlDickFuck.MakesUpMind".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Sharkgirlscene.SharkgirlDickFuck.SteadiesHand".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkgirlDickFuck.PullsOut".Localized(
                Appearance.BiggestBreastSizeDescript(Player)
            ));

            OutputText("Scenes.Sharkgirlscene.SharkgirlDickFuck.CriesOrgasm".Localized());

            CombatScene.CleanupAfterCombat();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            if (Player.Corruption < 33)
                Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));
        }

        public void SharkgirlDickTail()
        {
            ClearOutput();

            Player.ChangeStatusValue(StatusEffectIds.SharkGirl, 0, 1);

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Sharkgirlscene.SharkgirlDickTail.SlithersMasturbating".Localized());
            else
                OutputText("Scenes.Sharkgirlscene.SharkgirlDickTail.SlithersDefeated".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkgirlDickTail.GrabsHips".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkgirlDickTail.CryingCeases".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sharkgirlscene.SharkgirlDickTail.KeepsMotion".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            CombatScene.CleanupAfterCombat();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            if (Player.Corruption < 33)
                Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));
        }

        public void SharkgirlHypnosis()
        {
            ClearOutput();

            OutputText("Scenes.Sharkgirlscene.SharkgirlHypnosis.ReelsTrips".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkgirlHypnosis.BeginsStir".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkgirlHypnosis.Expressionless".Localized(
                Appearance.AllBreastsDescript(Player)
            ));

            OutputText("Scenes.Sharkgirlscene.SharkgirlHypnosis.RaisesUp".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkgirlHypnosis.PleasureRobs".Localized());

            CombatScene.CleanupAfterCombat();

            Player.Orgasm();

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            if (Player.Corruption < 33)
                Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));
        }

        public void SharkgirlFacesitting()
        {

            OutputText("Scenes.Sharkgirlscene.SharkgirlFacesitting.MakesUpMindRape".Localized());

            if (Player.Gender == Gender.Female)
                OutputText("Scenes.Sharkgirlscene.SharkgirlFacesitting.PullsOutLick".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Sharkgirlscene.SharkgirlFacesitting.RemindsNeglect".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkgirlFacesitting.ParagraphBreak".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkgirlFacesitting.PinsGirl".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkgirlFacesitting.RewardsSlut".Localized());

            if (Player.Gender == Gender.Female)
                OutputText("Scenes.Sharkgirlscene.SharkgirlFacesitting.JuicesSpraying".Localized());
            else
                OutputText("Scenes.Sharkgirlscene.SharkgirlFacesitting.StandsGripping".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkgirlFacesitting.SlutLicks".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkgirlFacesitting.ThoroughlySatisfied".Localized());

            CombatScene.CleanupAfterCombat();

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            if (Player.Corruption < 33)
                Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));
        }

        public void SharkGirlGetsDildoed()
        {
            ClearOutput();

            OutputText("Scenes.Sharkgirlscene.SharkGirlGetsDildoed.GrinEarToEar".Localized());

            if (!Player.Weapon.IsAttached())
                OutputText("Scenes.Sharkgirlscene.SharkGirlGetsDildoed.PutsAway".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkGirlGetsDildoed.DrawsDildo".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Sharkgirlscene.SharkGirlGetsDildoed.LegsSpread".Localized());
            else
                OutputText("Scenes.Sharkgirlscene.SharkGirlGetsDildoed.HoldsLegs".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkGirlGetsDildoed.ParagraphBreak".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkGirlGetsDildoed.WonderfulToy".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkGirlGetsDildoed.SeaSmell".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkGirlGetsDildoed.SlapsCheek".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkGirlGetsDildoed.MonsterWhimpers".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkGirlGetsDildoed.WigglesIt".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkGirlGetsDildoed.PussyClamps".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkGirlGetsDildoed.PlungesToy".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkGirlGetsDildoed.DamnHot".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)((20 + Player.Libido / 5 + Player.Corruption / 10))));

            CombatScene.CleanupAfterCombat();
        }

        public void SharkBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Sharkgirlscene.SharkBadEnd.WeeksPass".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkBadEnd.AwokenSoon".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkBadEnd.SurprisinglyStrong".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkBadEnd.AsksVoice".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkBadEnd.GirlsGrin".Localized());

            DoNext(() => SharkBadEnd2());
        }

        public void SharkBadEnd2()
        {
            ClearOutput();

            OutputText("Scenes.Sharkgirlscene.SharkBadEnd2.MonthsMutations".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkBadEnd2.PlungesAnother".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkBadEnd2.TigerAsks".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkBadEnd2.TigerSays".Localized());

            OutputText("Scenes.Sharkgirlscene.SharkBadEnd2.FreshMeat".Localized());

            GameOver();
        }

        public void SharkLossRape()
        {
            ClearOutput();

            if (Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Sharkgirlscene.SharkLossRape.SlumpsDefeated".Localized());

                if (Player.Health < 1)
                    OutputText("Scenes.Sharkgirlscene.SharkLossRape.HurtWord".Localized());
                else
                    OutputText("Scenes.Sharkgirlscene.SharkLossRape.HornyWord".Localized());

                OutputText("Scenes.Sharkgirlscene.SharkLossRape.ToFightOnThird".Localized());

                OutputText("Scenes.Sharkgirlscene.SharkLossRape.FlauntsBody".Localized());

                CombatScene.CleanupAfterCombat();

                Player.ApplyStats(new DynStat(GameStats.Toughness, -2f));

                return ;
            }

            if (Player.HasVagina() && (Player.Cocks.Count == 0 || Random.Shared.Next(2) == 0))
            {
                OutputText("Scenes.Sharkgirlscene.SharkLossRape.SlumpsDefeatedAgain".Localized());

                if (Player.Health < 1)
                    OutputText("Scenes.Sharkgirlscene.SharkLossRape.HurtWordAgain".Localized());
                else
                    OutputText("Scenes.Sharkgirlscene.SharkLossRape.HornyWordAgain".Localized());

                OutputText("Scenes.Sharkgirlscene.SharkLossRape.ToFightOnAgain".Localized());

                OutputText("Scenes.Sharkgirlscene.SharkLossRape.GigglesMoves".Localized(
                    Player.AssDescript()
                ));

                OutputText("Scenes.Sharkgirlscene.SharkLossRape.RemovesClothes".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Sharkgirlscene.SharkLossRape.SeesCockSmiles".Localized());

                OutputText("Scenes.Sharkgirlscene.SharkLossRape.GazeDrifts".Localized(
                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Sharkgirlscene.SharkLossRape.GrabsPulls".Localized());

                OutputText("Scenes.Sharkgirlscene.SharkLossRape.SighsHappily".Localized());

                OutputText("Scenes.Sharkgirlscene.SharkLossRape.EventuallyPass".Localized());

                Player.Orgasm("Vaginal");

                if (Player.Corruption < 30)
                    Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

                CombatScene.CleanupAfterCombat();

                return ;
            }
            else
            {
                OutputText("Scenes.Sharkgirlscene.SharkLossRape.SlumpsDefeatedThird".Localized());

                if (Player.Health < 1)
                    OutputText("Scenes.Sharkgirlscene.SharkLossRape.HurtWordThird".Localized());
                else
                    OutputText("Scenes.Sharkgirlscene.SharkLossRape.HornyWordThird".Localized());

                OutputText("Scenes.Sharkgirlscene.SharkLossRape.ToFightOnThird".Localized());

                OutputText("Scenes.Sharkgirlscene.SharkLossRape.FeelsFoot".Localized());

                OutputText("Scenes.Sharkgirlscene.SharkLossRape.BeforeEnjoy".Localized());

                OutputText("Scenes.Sharkgirlscene.SharkLossRape.NoQualms".Localized());

                OutputText("Scenes.Sharkgirlscene.SharkLossRape.VaginaClamps".Localized());

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

                if (Player.Corruption < 30)
                    Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

                CombatScene.CleanupAfterCombat();

                return ;
            }
        }

    }
}