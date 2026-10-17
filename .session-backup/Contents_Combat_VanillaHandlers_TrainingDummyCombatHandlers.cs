using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 TrainingDummy 的招式（src/classes/scenes/camp/TrainingDummy.hx:56-114）。
    public static class TrainingDummyCombatHandlers
    {
        // 原版 dummyWait（TrainingDummy.hx:56-108）：16 段随机「假人不动」台词，无任何伤害/状态。
        [Register("coc:training_dummy_dummy_wait")]
        private static void DummyWait(CombatContext context, CombatAbilityDefinition ability)
        {
            var key = context.Random.Next(16) switch
            {
                0 => "Combat.TrainingDummy.DummyWait.TheDummyStands",
                1 => "Combat.TrainingDummy.DummyWait.TheTrainingDummy",
                2 => "Combat.TrainingDummy.DummyWait.TheDummyEnjoys",
                3 => "Combat.TrainingDummy.DummyWait.LooksAtYou",
                4 => "Combat.TrainingDummy.DummyWait.DoesNothingIt",
                5 => "Combat.TrainingDummy.DummyWait.TheDummyRemains",
                6 => "Combat.TrainingDummy.DummyWait.TheWoodCreaks",
                7 => "Combat.TrainingDummy.DummyWait.RegardsYouWith",
                8 => "Combat.TrainingDummy.DummyWait.TheTrainingDummy2",
                9 => "Combat.TrainingDummy.DummyWait.ThereIsNo",
                10 => "Combat.TrainingDummy.DummyWait.CanTMove",
                11 => "Combat.TrainingDummy.DummyWait.TheWoodDoes",
                12 => "Combat.TrainingDummy.DummyWait.TheDummyDoes",
                13 => "Combat.TrainingDummy.DummyWait.AppearsInsensate",
                14 => "Combat.TrainingDummy.DummyWait.TheTrainingDummy3",
                _ => "Combat.TrainingDummy.DummyWait.TheDummyStands2"
            };

            context.Append(key.Localized());
        }
    }
}
