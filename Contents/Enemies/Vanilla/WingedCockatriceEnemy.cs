using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    /// <summary>
    /// 原版 WingedCockatrice.hx:9：Cockatrice 的带翼变体——大型羽翼 + 速度 +10，
    /// 立绘换成 cockatricewithwings，其余数值完全沿用父类。
    /// </summary>
    [Register<WingedCockatriceEnemy>("coc:winged_cockatrice")]
    public class WingedCockatriceEnemy : CockatriceEnemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 :12 wings.type = Wings.FEATHERED_LARGE
            Body.Get<WingsPart>().Id = BodyPartTypes.FeatheredLarge;

            // 原版 :13 spe += 10
            Speed += 10;
        }
    }
}