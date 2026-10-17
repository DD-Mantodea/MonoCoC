using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Encounters;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;
using MonoCoC.Contents.Scenes.Vanilla.Areas.DeepWoods;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas
{

    public static class DeepWoodsEncounters
    {

        public const int AkbalEventsDone =

            (int)AkbalEvent.Goblin | (int)AkbalEvent.Alice | (int)AkbalEvent.Kitsune1 | (int)AkbalEvent.Kitsune2;

        public static readonly IEncounter Table = IEncounter.Group(
            "deepwoods",

            CommonEncounters.Table,

            IEncounter.Create("kitsune", _ => 1f, _ => true,

                _ => Scene.Go<KitsuneScene>()),

            IEncounter.Create("coc:dullahan", _ => ForestEncounters.DullahanChance(), _ => ForestEncounters.DullahanAvailable(),

                _ => Scene.Go<DullahanScene>()),

            IEncounter.Create("alice", _ => 1f, _ => true, _ => Scene.Go<AliceScene>()),

            IEncounter.Create("akbal", _ => AkbalChance(), _ => AkbalChance() > 0f,

                _ => Scene.Go<AkbalScene>()),

            IEncounter.Create("coc:tamani", _ => 1f, _ => true,

                _ => Scene.Go<TamaniScene>()),

            IEncounter.Create("faerie", _ => 1f,

                _ => CoC.Instance.Flags.Get(GameFlagIds.FaerieEncounterDisabled) <= 0,

                _ => Scene.Go<FaerieScene>()),

            IEncounter.Create("erlking",

                _ => CoC.Instance.Flags.Get(GameFlagIds.ErlkingDisabled) == 0 ? 2f : 0f,

                _ => CoC.Instance.Flags.Get(GameFlagIds.ErlkingDisabled) == 0,

                _ => Scene.Go<ErlKingScene>()),

            IEncounter.Create("fera", _ => 1f, _ => true, _ => Scene.Go<FeraScene>()),

            IEncounter.Create("lumber", _ => 1f, _ => ForestEncounters.CanGatherWood(), ForestEncounters.GatherWoods),

            IEncounter.Create("corrupted_glade", _ => ForestEncounters.CorruptedGladeChance(), _ => true,

                _ => Scene.Go<CorruptedGladeScene>()),

            GoblinSharpshooterScene.Encounter,

            IEncounter.Create("tentabeast", _ => 1f, player => player.Level >= 2,

                _ => Scene.Go<TentacleBeastScene>()),

            IEncounter.Create("dungeon", _ => 1f, _ => true, _ => Contents.Dungeons.DungeonScene.Enter(Datas.Vanilla.SceneIds.Dungeon.DeepCave)),

            IEncounter.Create("walk", _ => 0.01f, _ => true, Walk),

            IEncounter.Create("aiko", _ => 0.35f, AikoAvailable,

                _ => Scene.Go<AikoScene>()),

            IEncounter.Create("corrupted_glade_faerie", _ => 0.1f, _ => FaerieGladesEnabled,

                _ => FaerieScene.GladesEncounter()));

        public static float AkbalChance()
        {
            var flags = CoC.Instance.Flags;

            var status = flags.Get(GameFlagIds.AkbalQuestStatus);

            if ((status & (int)AkbalEvent.QuestDone) > 0)
                return 0f;

            if (status > 0)
            {
                var dayDone = flags.Get(GameFlagIds.AkbalDayDone);

                var days = Scene.View.Time.Days;

                if ((status & AkbalEventsDone) == 0 && days > 1 + dayDone)
                    return 1f;

                if ((status & AkbalEventsDone) > 0 && days > 3 + dayDone)
                    return 1f;

                return 0f;
            }

            return 1f;
        }

        public static bool AikoAvailable(Player player)
        {
            var flags = CoC.Instance.Flags;

            return player.Level >= 16

                && flags.Get(GameFlagIds.AikoTimesMet) < 4

                && flags.Get(GameFlagIds.AikoBallReturned) != 2;
        }

        public static bool FaerieGladesEnabled =>

            CoC.Instance.Flags.Get(GameFlagIds.CorruptedGladesDestroyed) < 50 &&

            CoC.Instance.Flags.Get(GameFlagIds.FaerieEncounterDisabled) <= 0;

        public static void Walk(EncounterContext context)
        {
            Scene.ClearOutput();

            Scene.ClearButtons();

            context.Write("Scenes.Deepwoods.DeepwoodsWalkFn.PeacefulWalkReflection".Localized());

            context.Player.Toughness += 0.5f;

            context.Player.Intelligence += 1;

            Scene.AddButton(0, "Scenes.Explore.Continue".Localized(), context.ReturnToCamp);
        }
    }
}