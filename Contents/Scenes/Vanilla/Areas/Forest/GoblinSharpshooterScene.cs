using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Encounters;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{

    [Register<GoblinSharpshooterScene>("coc:goblin_sharpshooter")]
    public class GoblinSharpshooterScene : Scene
    {
        public static SaveContent Save => SaveContent(SceneIds.Forest.GoblinSharpshooter);

        public bool SawSmokeGrenade => Save.GetBool("sawSmokeGrenade");

        public override void Entry() => MeetGoblinSharpshooter();

        public void Reset()
        {
            Save.SetInt("timesEncountered", 0);

            Save.SetBool("sawSmokeGrenade", false);

            Save.SetFloat("cooldownUntil", 0f);
        }

        public static float EncounterChance() => 0.1f + CoC.Instance.Player.HoursSinceCum / 48f;

        public static bool EncounterWhen() =>

            CoC.Instance.Player.HasCock()

            && View.Time.TotalTime >= Save.GetFloat("cooldownUntil", 0f)

            && CoC.Instance.Player.Level >= 14;

        public static IEncounter Encounter => IEncounter.Create(
            "goblinSharpshooter",

            _ => EncounterChance(),

            _ => EncounterWhen(),

            _ => Go<GoblinSharpshooterScene>());

        public void MeetGoblinSharpshooter()
        {
            ClearOutput();

            Save.SetFloat("cooldownUntil", (float)(View.Time.TotalTime + 48));

            var times = Save.GetInt("timesEncountered", 0);

            if (times == 0)
            {
                OutputText("Scenes.Goblinsharpshooterscene.MeetGoblinSharpshooter.AmbushInstinct".Localized());

                if (Player.Location is "forest" or "deep woods" or "swamp")
                {
                    OutputText("Scenes.Goblinsharpshooterscene.MeetGoblinSharpshooter.RustlingBush".Localized());

                    OutputText("Scenes.Goblinsharpshooterscene.MeetGoblinSharpshooter.NoMovement".Localized());

                    OutputText("Scenes.Goblinsharpshooterscene.MeetGoblinSharpshooter.BranchCrack".Localized());

                    OutputText("Scenes.Goblinsharpshooterscene.MeetGoblinSharpshooter.MusketHit".Localized());

                    OutputText("Scenes.Goblinsharpshooterscene.MeetGoblinSharpshooter.FemaleVoiceBush".Localized());
                }
                else if (Player.Location == "desert")
                {
                    OutputText("Scenes.Goblinsharpshooterscene.MeetGoblinSharpshooter.SandDuneShift".Localized());

                    OutputText("Scenes.Goblinsharpshooterscene.MeetGoblinSharpshooter.NoMovementDune".Localized());

                    OutputText("Scenes.Goblinsharpshooterscene.MeetGoblinSharpshooter.OddBranchCrack".Localized());

                    OutputText("Scenes.Goblinsharpshooterscene.MeetGoblinSharpshooter.MusketHitDune".Localized());

                    OutputText("Scenes.Goblinsharpshooterscene.MeetGoblinSharpshooter.FemaleVoiceDune".Localized());
                }

                OutputText("Scenes.Goblinsharpshooterscene.MeetGoblinSharpshooter.MilkThreat".Localized());

                OutputText("Scenes.Goblinsharpshooterscene.MeetGoblinSharpshooter.ChestWoundCheck".Localized());

                Player.HealthChange(-(50 + Random.Shared.Next(25)));

                OutputText("Scenes.Goblinsharpshooterscene.MeetGoblinSharpshooter.ReloadsFirearm".Localized());
            }
            else
                OutputText("Scenes.Goblinsharpshooterscene.MeetGoblinSharpshooter.MusketCockedBehind".Localized());

            Save.SetInt("timesEncountered", times + 1);

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.GoblinSharpshooter), () => Go<CampScene>());
        }

        public void DefeatSharpshooter()
        {
            ClearOutput();

            OutputText("Scenes.Goblinsharpshooterscene.DefeatSharpshooter.GoblinFallsDefeated".Localized());

            OutputText("Scenes.Goblinsharpshooterscene.DefeatSharpshooter.CaptivePrompt".Localized());

            GoblinScene.GenerateGobboSexMenu();
        }

        public void GoblinEscapes()
        {
            ClearOutput();

            OutputText("Scenes.Goblinsharpshooterscene.GoblinEscapes.SmokeEscape".Localized());

            Save.SetBool("sawSmokeGrenade", true);

            CombatScene.CleanupAfterCombat();
        }
    }
}