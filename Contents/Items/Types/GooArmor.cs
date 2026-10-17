using MonoCoC.Attributes;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Items
{

    [Register<GooArmor>("coc:goo_armor")]
    public class GooArmor : Armor
    {
        public override void UseText()
        {
            Scene.OutputText("Items.GooArmor.UseText".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.MetValeria, 1);

            Scene.AwardAchievement(AchievementIds.GooArmor);
        }

        public override void RemoveText() => Scene.OutputText("Items.GooArmor.RemoveText".Localized());
    }
}