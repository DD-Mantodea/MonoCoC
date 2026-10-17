using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Swim
{
    [Register<SharkManScene>("coc:shark_man_scene")]
    public class SharkManScene : Scene
    {

        public static SaveContent Save => SaveContent(SceneIds.Swim.SharkMan);

        public string debugName => "SharkMan";

        public string debugHint => "";

        public override void Entry()
        {
            SharkManEncounter();
        }

        public void Reset()
        {

            Save.SetBool("encountered", false);

            Save.SetBool("hasLost", false);
        }

        public void MarkPlayerEscaped() => Save.SetBool("hasLost", true);

        public bool Encountered => Save.GetBool("encountered");

        public void SharkManEncounter()
        {
            ClearOutput();

            if (!Save.GetBool("hasLost"))
            {
                OutputText("Scenes.Sharkmanscene.SharkManEncounter.ClangEchoes".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Investigate".Localized(), InvestigateTheNoise);

                AddButton(1, "Common.Button.Leave".Localized(), LeaveTheNoiseAlone);
            }
            else
            {
                OutputText("Scenes.Sharkmanscene.SharkManEncounter.InjuredShark".Localized());

                OutputText("Scenes.Sharkmanscene.SharkManEncounter.SharkCharges".Localized());

                ClearButtons();

                CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "shark_man")), () => Go<CampScene>());
            }
        }

        public void LeaveTheNoiseAlone()
        {

            OutputText("Scenes.Sharkmanscene.LeaveTheNoiseAlone.MaybeAnotherTime".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void InvestigateTheNoise()
        {
            ClearOutput();

            OutputText("Scenes.Sharkmanscene.InvestigateTheNoise.ChasesClanging".Localized());

            OutputText("Scenes.Sharkmanscene.InvestigateTheNoise.LouderClang".Localized());

            OutputText("Scenes.Sharkmanscene.InvestigateTheNoise.AttemptsSpeak".Localized());

            OutputText("Scenes.Sharkmanscene.InvestigateTheNoise.DustSettles".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "shark_man")), () => Go<CampScene>());
        }

        public void SharkManWonCombat(bool hpVictory)
        {
            ClearOutput();

            RegisterMacro("lustwin", !hpVictory);

            RegisterMacro("vagCould", CombatController.Instance!.Context.Enemy.CockArea(0) < Player.VaginalCapacity());

            RegisterMacro("assCould", CombatController.Instance!.Context.Enemy.CockArea(0) < Player.AnalCapacity());

            OutputText("Scenes.Sharkmanscene.SharkManWonCombat.ThereSAnExplosion".Localized());

            OutputText("Scenes.Sharkmanscene.SharkManWonCombat.GutturalGrowl".Localized());

            Player.ButtChange(CombatController.Instance!.Context.Enemy.Cocks[0].Length, true);

            if (Player.HasVagina())
                Player.CuntChange(CombatController.Instance!.Context.Enemy.Cocks[0].Length, true);

            OutputText("Scenes.Sharkmanscene.SharkManWonCombat.HipsSnap".Localized());

            OutputText("Scenes.Sharkmanscene.SharkManWonCombat.ColdWater".Localized());

            OutputText("Scenes.Sharkmanscene.SharkManWonCombat.NoSlowing".Localized());

            OutputText("Scenes.Sharkmanscene.SharkManWonCombat.WarmthFloods".Localized());

            Player.SlimeFeed();

            Player.Orgasm("VaginalAnal");

            Save.SetBool("hasLost", true);

            CombatScene.CleanupAfterCombat();
        }

        public void SharkManLostCombat(bool hpVictory)
        {
            ClearOutput();

            RegisterMacro("lustwin", !hpVictory);

            OutputText("Scenes.Sharkmanscene.SharkManLostCombat.AssailantCrumples".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.HelpSatisfy".Localized(), DoubleTeamSharkMan);

            AddButton(1, "Common.Button.Watch".Localized(), GetCuckedByAShark);

            AddButton(2, "Common.Button.CalmHim".Localized(), CalmTheSharkMan)

                ?.DisableIf(Player.LowerBodyType().GetString("body") != "mermaid",

                    "There may be an aquatic melody that calms the mightiest tempest.");

            Save.SetBool("encountered", true);

            AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
        }

        public void CalmTheSharkMan()
        {
            ClearOutput();

            OutputText("Scenes.Sharkmanscene.CalmTheSharkMan.TenderFlame".Localized());

            OutputText("Scenes.Sharkmanscene.CalmTheSharkMan.WhispersRuliah".Localized());

            OutputText("Scenes.Sharkmanscene.CalmTheSharkMan.HesitantLook".Localized());

            SharkManConclusion(false);
        }

        public void GetCuckedByAShark()
        {
            ClearOutput();

            OutputText("Scenes.Sharkmanscene.GetCuckedByAShark.SeizesOlderShark".Localized());

            OutputText("Scenes.Sharkmanscene.GetCuckedByAShark.LowersBetween".Localized());

            OutputText("Scenes.Sharkmanscene.GetCuckedByAShark.CockPresses".Localized());

            OutputText("Scenes.Sharkmanscene.GetCuckedByAShark.NotEnough".Localized());

            OutputText("Scenes.Sharkmanscene.GetCuckedByAShark.GrowlThroat".Localized());

            OutputText("Scenes.Sharkmanscene.GetCuckedByAShark.ClutchesPanting".Localized());

            OutputText("Scenes.Sharkmanscene.GetCuckedByAShark.CriesRise".Localized());

            OutputText("Scenes.Sharkmanscene.GetCuckedByAShark.FollowsSuit".Localized());

            OutputText("Scenes.Sharkmanscene.GetCuckedByAShark.SlumpsAfter".Localized());

            if (!Player.IsTaur() && !Player.IsGenderless() && Player.Lust >= 31)
                Player.Orgasm("VaginalAndDick");
            else
                Player.ApplyStats(new DynStat(GameStats.Lust, 50f));

            DoNext(() => SharkManConclusion());
        }

        public void DoubleTeamSharkMan()
        {
            ClearOutput();

            OutputText("Scenes.Sharkmanscene.DoubleTeamSharkMan.PutsTogether".Localized());

            OutputText("Scenes.Sharkmanscene.DoubleTeamSharkMan.SeizesOpportunity".Localized());

            OutputText("Scenes.Sharkmanscene.DoubleTeamSharkMan.GruntsShiver".Localized());

            OutputText("Scenes.Sharkmanscene.DoubleTeamSharkMan.FingersCurl".Localized());

            OutputText("Scenes.Sharkmanscene.DoubleTeamSharkMan.AlmostDocile".Localized());

            if (Player.HasVagina())
                Player.CuntChange(CombatController.Instance!.Context.Enemy.Cocks[0].Length, true, false);
            else
                Player.ButtChange(CombatController.Instance!.Context.Enemy.Cocks[0].Length, true, false);

            OutputText("Scenes.Sharkmanscene.DoubleTeamSharkMan.SuddenFullness".Localized());

            OutputText("Scenes.Sharkmanscene.DoubleTeamSharkMan.DespiteRoughness".Localized());

            Player.SlimeFeed();

            Player.Orgasm("VaginalAnal");

            DoNext(() => SharkManConclusion());
        }

        public void SharkManConclusion(bool dontClear = true)
        {

            if (dontClear)
                ClearOutput();

            RegisterMacro("notcalmed", dontClear);

            OutputText("Scenes.Sharkmanscene.SharkManConclusion.ShakeLoose".Localized());

            OutputText("Scenes.Sharkmanscene.SharkManConclusion.ExplainsLeaving".Localized());

            OutputText("Scenes.Sharkmanscene.SharkManConclusion.SharkGirlSits".Localized());

            OutputText("Scenes.Sharkmanscene.SharkManConclusion.CasualTone".Localized());

            InventoryScene.TakeItem(ItemIds.AquamarineRing, () => CombatScene.CleanupAfterCombat());
        }

    }
}