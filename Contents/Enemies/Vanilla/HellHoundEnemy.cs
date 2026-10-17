using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/areas/mountain/HellHound.hx（level 5 双头地狱犬）。
    // 招式与 AI 见 Assets/Data/Combat/Ai/HellHound.hjson 与 HellHoundCombatHandlers.cs。
    [Register<HellHoundEnemy>("coc:hell_hound")]
    public class HellHoundEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            WeaponName = "claws";

            ArmorName = "flaming hide";
        }

        // 原版 HellHound.hx:71 defeated()：战败文本 + 「Fuck it / Lick」菜单
        public override bool Defeated(bool hpVictory)
        {
            var scene = Scenes.SceneInstance<Scenes.Vanilla.Areas.Mountain.HellHoundScene>.Get();

            var player = CoC.Instance.Player;

            Scenes.Scene.ClearOutput();

            Scenes.Scene.OutputText(hpVictory

                ? "Scenes.Hellhound.Defeated.FlamesDimDefeated".Localized()

                : "Scenes.Hellhound.Defeated.RefusesToHurt".Localized());

            Scenes.Scene.ClearButtons();

            // 原版：lust >= 33 且非无性别才给性爱选项
            if (player.Lust >= 33 && !player.IsGenderless())
            {
                if (player.HasVagina() && !player.HasTailInsteadOfLegs())
                    Scenes.Scene.AddButton(0, "Common.Button.FuckIt".Localized(), () => scene.HellHoundPropahRape());

                if (!hpVictory)
                    Scenes.Scene.AddButton(1, "Common.Button.Lick".Localized(), () => scene.HellHoundGetsRaped());
            }

            Scenes.Scene.AddButton(14, "Common.Button.Leave".Localized(), () => Scenes.Scene.ReturnToCamp(1));

            return true;
        }

        // 原版 HellHound.hx:94 won()
        public override bool Won(bool hpVictory)
        {
            if (HasStatusEffect(StatusEffectIds.Infested))
            {
                Scenes.Scene.OutputText("Scenes.Hellhound.Won.SnortsLeaves".Localized());

                return false;
            }

            Scenes.SceneInstance<Scenes.Vanilla.Areas.Mountain.HellHoundScene>.Get().HellhoundRapesPlayer();

            return true;
        }
    }
}