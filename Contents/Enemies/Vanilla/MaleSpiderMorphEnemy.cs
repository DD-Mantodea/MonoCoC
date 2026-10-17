using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 原版该类没有 performCombatAction，使用默认普攻（无需 handler）。
    [Register<MaleSpiderMorphEnemy>("coc:male_spider_morph")]
    public class MaleSpiderMorphEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(6, 2);

            Balls = 2;

            BallSize = 2;

            Tallness = 7 * 12 + 6;

            Body.Get<SkinPart>().Tone = "dusky";

            var hair = Body.Get<HairPart>();

            hair.Color = "red";

            hair.Length = 13;

            WeaponName = "dagger";

            ArmorName = "exoskeleton";
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Swamp.MaleSpiderMorphScene>.Get().DefeatSpiderBoy();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (HasStatusEffect(StatusEffectIds.Infested))
            {
                Scenes.Scene.OutputText("Combat.MaleSpiderMorph.CameWorms".Localized());

                return false;
            }

            Scenes.SceneInstance<Scenes.Vanilla.Areas.Swamp.MaleSpiderMorphScene>.Get().LoseToMaleSpiderMorph();

            return true;
        }
    }
}