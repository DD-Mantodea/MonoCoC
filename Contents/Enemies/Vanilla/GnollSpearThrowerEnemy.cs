using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Quests;
using MonoCoC.Extensions;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/GnollSpearThrowerCombatHandlers.cs（已实现，230 行）
    [Register<GnollSpearThrowerEnemy>("coc:gnoll_spear_thrower")]
    public class GnollSpearThrowerEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 4, 3);

            Tallness = 72;

            Body.Get<SkinPart>().Tone = "tawny";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 22;

            WeaponName = "teeth";

            ArmorName = "skin";
        }

        public override bool Defeated(bool hpVictory)
        {
            if (ShortName == "alpha gnoll")
            {
                Scene.ClearOutput();

                Scene.OutputText("Scenes.GnollSpearThrower.Defeated.AlphaDefeated".Localized());

                Scene.ClearButtons();

                Scene.AddButton(0, "Common.Button.Fuck".Localized(), () => SceneInstance<UrtaQuestScene>.Get().WinRapeHyenaPrincess());

                Scene.AddButton(1, "Common.Button.SuccMilk".Localized(), () => SceneInstance<UrtaQuestScene>.Get().UseSuccubiMilkOnGnollPrincesses());

                Scene.AddButton(14, "Common.Button.Leave".Localized(), () => SceneInstance<UrtaQuestScene>.Get().UrtaNightSleep());

                return true;
            }

            SceneInstance<Scenes.Vanilla.Areas.Plains.GnollSpearThrowerScene>.Get().HyenaVictory();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (ShortName == "alpha gnoll")
            {
                SceneInstance<UrtaQuestScene>.Get().LoseToGnollPrincessAndGetGangBanged();

                return true;
            }

            if (HasStatusEffect(StatusEffectIds.Infested))
            {
                Scene.OutputText("Combat.GnollSpearThrower.CameWorms".Localized());

                return false;
            }

            SceneInstance<Scenes.Vanilla.Areas.Plains.GnollSpearThrowerScene>.Get().HyenaSpearLossAnal();

            return true;
        }
    }
}