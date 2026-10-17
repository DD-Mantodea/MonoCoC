using MonoCoC.Attributes;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/areas/mountain/InfestedHellhound.hx（level 5 被寄生的双头地狱犬）。
    // 招式与 AI 见 Assets/Data/Combat/Ai/InfestedHellhound.hjson。
    [Register<InfestedHellhoundEnemy>("coc:infested_hellhound")]
    public class InfestedHellhoundEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            WeaponName = "claws";

            ArmorName = "flaming hide";
        }

        // 原版 InfestedHellhound.hx:71 defeated()：战败文本 + 「Lick」菜单
        public override bool Defeated(bool hpVictory)
        {
            var scene = Scenes.SceneInstance<Scenes.Vanilla.Areas.Mountain.HellHoundScene>.Get();

            var player = CoC.Instance.Player;

            Scenes.Scene.ClearOutput();

            Scenes.Scene.OutputText(hpVictory

                ? "Scenes.Infestedhellhound.Defeated.FlamesDimWhine".Localized()

                : "Scenes.Infestedhellhound.Defeated.ArousalDefeat".Localized());

            Scenes.Scene.ClearButtons();

            // 原版：仅欲望胜利且 lust >= 33 时给「Lick」选项
            if (player.Lust >= 33 && !player.IsGenderless() && !hpVictory)
                Scenes.Scene.AddButton(0, "Common.Button.Lick".Localized(), () => scene.HellHoundGetsRaped());

            Scenes.Scene.AddButton(14, "Common.Button.Leave".Localized(), () => Scenes.Scene.ReturnToCamp(1));

            return true;
        }

        // 原版 InfestedHellhound.hx:92 won()：无论 HP/欲望胜利都走 loss rape 场景
        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Mountain.InfestedHellhoundScene>.Get().InfestedHellhoundLossRape();

            return true;
        }
    }
}