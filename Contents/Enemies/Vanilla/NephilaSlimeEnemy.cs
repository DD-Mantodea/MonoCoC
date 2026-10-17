using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/areas/mountain/NephilaSlime.hx（level 14 大型史莱姆娘）。
    // 招式与 AI 见 Assets/Data/Combat/Ai/NephilaSlime.hjson 与 NephilaSlimeCombatHandlers.cs。
    [Register<NephilaSlimeEnemy>("coc:nephila_slime")]
    public class NephilaSlimeEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            WeaponName = "tentacles";

            ArmorName = "slimy hide";
        }

        // 原版 NephilaSlime.hx:99 defeated() -> defeatNephilaSlime()
        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Mountain.NephilaSlimeScene>.Get().DefeatNephilaSlime();

            return true;
        }

        // 原版 NephilaSlime.hx:103 won()
        public override bool Won(bool hpVictory)
        {
            if (HasStatusEffect(StatusEffectIds.Infested))
            {
                Scenes.Scene.OutputText("Scenes.Nephilaslimescene.Won.RecoilDisgust".Localized());

                return false;
            }

            Scenes.SceneInstance<Scenes.Vanilla.Areas.Mountain.NephilaSlimeScene>.Get().LoseToNephilaSlime();

            return true;
        }
    }
}