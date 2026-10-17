using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.GlacialRift
{
    [Register<MoraScene>("coc:mora")]
    public class MoraScene : Scene
    {

        public bool encountered = false;

        public static SaveContent Save => SaveContent(SceneIds.GlacialRift.Mora);

        public string debugName => "Mora";

        public string debugHint => "";

        public override void Entry()
        {
            MoraEncounter();
        }

        public void Reset()
        {

            Save.SetBool("encountered", false);
        }

        public void MoraEncounter()
        {
            ClearOutput();

            if (!Save.GetBool("encountered"))
            {
                OutputText("Scenes.Morascene.MoraEncounter.SnowdriftsPickUp".Localized());

                OutputText("Scenes.Morascene.MoraEncounter.FigureAppears".Localized());

                OutputText("Scenes.Morascene.MoraEncounter.NowhereToGo".Localized());

                AddButton(0, "Common.Button.Fight".Localized(), () => CombatScene.Start(EnemyRegistry.Create(EnemyIds.Mora), () => Go<CampScene>(), canFlee: false));

                AddButton(1, "Common.Button.Flee".Localized(), FleeTheMora);
            }
            else
            {
                OutputText("Scenes.Morascene.MoraEncounter.SnowObscures".Localized());

                CombatScene.Start(EnemyRegistry.Create(EnemyIds.Mora), () => Go<CampScene>());
            }

            Save.SetBool("encountered", true);
        }

        public void FleeTheMora()
        {

            OutputText("Scenes.Morascene.FleeTheMora.CampToGo".Localized());

            OutputText("Scenes.Morascene.FleeTheMora.NothingWillChange".Localized());

            OutputText("Scenes.Morascene.FleeTheMora.WillowRaised".Localized());

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.Mora), () => Go<CampScene>());
        }

        public void MoraWinChoices(bool hpVictory)
        {
            ClearOutput();

            RegisterMacro("lustvictory", !hpVictory);

            OutputText("Scenes.Morascene.MoraWinChoices.DropsToSide".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Fuck".Localized(), MoraFuck);

            AddNextButton("Common.Button.Bonegrind".Localized(), MoraBonegrind);

            AddNextButton("Common.Button.Ribjob".Localized(), MoraRibjob);

            AddNextButton("Common.Button.Kill".Localized(), KillTheMora);

            AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
        }

        public void MoraFuck()
        {
            ClearOutput();

            OutputText("Scenes.Morascene.MoraFuck.CocksBearing".Localized());

            OutputText("Scenes.Morascene.MoraFuck.SkinTautBone".Localized());

            OutputText("Scenes.Morascene.MoraFuck.PushesIn".Localized());

            OutputText("Scenes.Morascene.MoraFuck.PressesBones".Localized());

            OutputText("Scenes.Morascene.MoraFuck.RisesGuard".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void MoraBonegrind()
        {
            ClearOutput();

            OutputText("Scenes.Morascene.MoraBonegrind.LimbsSplayed".Localized());

            OutputText("Scenes.Morascene.MoraBonegrind.SlidesHands".Localized());

            OutputText("Scenes.Morascene.MoraBonegrind.HandToCrotch".Localized());

            OutputText("Scenes.Morascene.MoraBonegrind.CupsBreast".Localized());

            OutputText("Scenes.Morascene.MoraBonegrind.EmptinessFelt".Localized());

            OutputText("Scenes.Morascene.MoraBonegrind.CriesClimax".Localized());

            OutputText("Scenes.Morascene.MoraBonegrind.FrigidRift".Localized());

            Player.Orgasm("VaginalAndDick");

            CombatScene.CleanupAfterCombat();
        }

        public void MoraRibjob()
        {
            ClearOutput();

            OutputText("Scenes.Morascene.MoraRibjob.WalksClose".Localized());

            OutputText("Scenes.Morascene.MoraRibjob.BlankStare".Localized());

            OutputText("Scenes.Morascene.MoraRibjob.BaresBody".Localized());

            OutputText("Scenes.Morascene.MoraRibjob.DenseFog".Localized());

            OutputText("Scenes.Morascene.MoraRibjob.ReleasesArm".Localized());

            OutputText("Scenes.Morascene.MoraRibjob.CaressesFace".Localized());

            Player.Orgasm("VaginalAndDick");

            CombatScene.CleanupAfterCombat();
        }

        public void MoraLose()
        {
            ClearOutput();

            OutputText("Scenes.Morascene.MoraLose.PouncesStance".Localized());

            OutputText("Scenes.Morascene.MoraLose.DontFret".Localized());

            OutputText("Scenes.Morascene.MoraLose.SuchAMess".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Morascene.MoraLose.HoofStomps".Localized());
            else
            {
                OutputText("Scenes.Morascene.MoraLose.HoofStompsAgain".Localized());

                if (Player.HasVagina())
                    Player.CuntChange(0, true, false);
                else
                    Player.ButtChange(0, true, false);

                OutputText("Scenes.Morascene.MoraLose.FingeringScrapes".Localized());
            }

            OutputText("Scenes.Morascene.MoraLose.PunchBlindsides".Localized());

            OutputText("Scenes.Morascene.MoraLose.HateSeething".Localized());

            Player.Fatigue = Player.MaxFatigue();

            Player.ApplyStats(new DynStat(GameStats.Toughness, -5f), new DynStat(GameStats.Corruption, 2f));

            CombatScene.CleanupAfterCombat();
        }

        public void KillTheMora()
        {

            OutputText("Scenes.Morascene.KillTheMora.LaysLimp".Localized());

            if (Player.Weapon.IsAxe() || Player.Weapon.IsScythe())
                OutputText("Scenes.Morascene.KillTheMora.SwingsIntoNeck".Localized());
            else
                if (Player.Weapon.IsBlunt())
                    OutputText("Scenes.Morascene.KillTheMora.HeftsSlams".Localized());
                else
                    if (Player.Weapon.IsRanged())
                        OutputText("Scenes.Morascene.KillTheMora.ReleasesBlade".Localized());
                    else
                        if (Player.Weapon.IsStabby())
                            OutputText("Scenes.Morascene.KillTheMora.TwirlsHandle".Localized());
                        else
                            OutputText("Scenes.Morascene.KillTheMora.HoldsDown".Localized());

            Player.UpgradeDeusVult();

            CombatScene.CleanupAfterCombat();
        }

    }
}