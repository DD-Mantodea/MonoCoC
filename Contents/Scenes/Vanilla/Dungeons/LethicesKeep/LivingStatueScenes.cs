using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep
{
    [Register<LivingStatueScenes>("coc:living_statue_scenes")]
    public class LivingStatueScenes : Scene
    {

        public override void Entry()
        {
            Encounter();
        }

        public void Encounter()
        {
            ClearOutput();

            OutputText("Scenes.Livingstatuescenes.Encounter.StatueAwakens".Localized());

            if (silly)
                OutputText("Scenes.Livingstatuescenes.Encounter.KneelessAdvantage".Localized());

            OutputText("Scenes.Livingstatuescenes.Encounter.MossBeardTrespasser".Localized());

            OutputText("Scenes.Livingstatuescenes.Encounter.HammerOverheadStride".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "living_statue")), RoomDungeon.ResumeFromFight);
        }

        public void BeatUpDaStatue(bool hpVictory)
        {
            ClearOutput();

            OutputText("Scenes.Livingstatuescenes.BeatUpDaStatue.CracksSpread".Localized());

            OutputText("Scenes.Livingstatuescenes.BeatUpDaStatue.LethiciteGleam".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.TakeStone".Localized(), TakeTheStone);
        }

        public void TakeTheStone()
        {
            ClearOutput();

            OutputText("Scenes.Livingstatuescenes.TakeTheStone.ClaimPrizeRubble".Localized());

            if (Player.HasKeyItem("Marae's Lethicite"))
                OutputText("Scenes.Livingstatuescenes.TakeTheStone.BigAsMaraes".Localized());
            else
                OutputText("Scenes.Livingstatuescenes.TakeTheStone.FactoryCrystalSize".Localized());

            OutputText("Scenes.Livingstatuescenes.TakeTheStone.LethiciteAcquired".Localized());

            Player.CreateKeyItem("Stone Statue Lethicite", 0, 0, 0, 0);

            CombatScene.CleanupAfterCombat();

            RoomDungeon.ResumeFromFight();
        }

        public void FuckinMarbleOP(bool hpVictory, bool pcCameWorms = false)
        {
            ClearOutput();

            OutputText("Scenes.Livingstatuescenes.FuckinMarbleOP.CrushedByHammer".Localized());

            GameOver();
        }

        [Register("coc:keep_hook_livingstatue")]
        public static void KeepHookLivingStatue() =>

            SceneInstance<LivingStatueScenes>.Get().Encounter();

    }
}