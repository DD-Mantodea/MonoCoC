using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/BazaarGatekeeperCombatHandlers.cs（已实现，137 行）
    [Register<BazaarGatekeeperEnemy>("coc:gatekeeper")]
    public class BazaarGatekeeperEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(8, 1.5f, BodyPartTypes.Demon);

            Tallness = 10 * 12;

            Body.Get<SkinPart>().Tone = "crimson";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 8;

            WeaponName = "dual scimitars";

            ArmorName = "tunic and pants";
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Places.Bazaar.BazaarScene>.Get().WinAgainstGuard();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.Scene.ClearOutput();

            if (hpVictory)
                Scenes.Scene.OutputText("Scenes.BazaarGatekeeper.Won.CutsCollapse".Localized());
            else
                Scenes.Scene.OutputText("Scenes.BazaarGatekeeper.Won.LustCollapse".Localized());

            Combat.CombatScene.CleanupAfterCombat();

            return true;
        }
    }
}