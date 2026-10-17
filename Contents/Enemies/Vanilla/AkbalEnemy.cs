using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<AkbalEnemy>("coc:akbal")]
    public class AkbalEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            var fierce = definition.Id.Path.Contains("fierce", System.StringComparison.OrdinalIgnoreCase);

            CreateCock(15, 2.5f, BodyPartTypes.Cat);

            Balls = 2;

            BallSize = 4;

            CumMultiplier = 6;

            CreateBreastRow(4);

            Tallness = 48;

            Body.Get<SkinPart>().Tone = "spotted";

            Body.Get<SkinPart>().Id = BodyPartTypes.Fur;

            Body.Get<TailPart>().Id = BodyPartTypes.Cat;

            WeaponName = "claws";

            ArmorName = "shimmering pelt";

            WeaponAttack = fierce ? 20 : 5;

            ArmorDefense = fierce ? 14 : 5;
        }

        public override bool Defeated(bool hpVictory)
        {
            var scene = SceneInstance<AkbalScene>.Get();

            if (Definition.Id.Path.Contains("fierce", System.StringComparison.OrdinalIgnoreCase))
                switch (CoC.Instance.Flags.Get(GameFlagIds.AkbalDayDone))
                {
                    case 0: scene.AkbalEventGoblinWin(); break;
                    case 1: scene.AkbalEventAliceWin(); break;
                    case 2: scene.AkbalEventKitsuneWin(); break;
                    default: CombatScene.CleanupAfterCombat(); break;
                }
            else
                scene.AkbalDefeated(hpVictory);

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            var scene = SceneInstance<AkbalScene>.Get();

            if (Definition.Id.Path.Contains("fierce", System.StringComparison.OrdinalIgnoreCase))
                switch (CoC.Instance.Flags.Get(GameFlagIds.AkbalDayDone))
                {
                    case 0: scene.AkbalEventGoblinLose(); break;
                    case 1: scene.AkbalEventAliceLose(); break;
                    case 2: scene.AkbalEventKitsuneLose(); break;
                    default: CombatScene.CleanupAfterCombat(); break;
                }
            else
                scene.AkbalWon(hpVictory);

            return true;
        }
    }
}