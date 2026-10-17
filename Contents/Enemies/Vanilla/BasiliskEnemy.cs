using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/BasiliskCombatHandlers.cs（已实现，72 行）
    [Register<BasiliskEnemy>("coc:basilisk")]
    public class BasiliskEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(6, 2);

            Balls = 2;

            BallSize = 2;

            Tallness = 6 * 12 + 2;

            Body.Get<SkinPart>().Tone = "gray-green";

            var hair = Body.Get<HairPart>();

            hair.Color = "gray-green";

            hair.Length = 2;

            WeaponName = "claws";

            ArmorName = "scales";
        }

        public override string Description
        {
            get
            {
                var player = CoC.Instance.Player;

                var canLook = player.HasPerk(PerkIds.BasiliskResistance)

                    || player.CanUseStare()

                    || player.HasKeyItem("Laybans");

                return (canLook ? "Npcs.Basilisk.Desc.Safe" : "Npcs.Basilisk.Desc.Avoiding").Localized();
            }
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.HighMountains.BasiliskScene>.Get().DefeatBasilisk();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (HasStatusEffect(StatusEffectIds.Infested))
            {
                Scenes.Scene.OutputText("Combat.Basilisk.CameWorms".Localized());

                return false;
            }

            Scenes.SceneInstance<Scenes.Vanilla.Areas.HighMountains.BasiliskScene>.Get().LoseToBasilisk();

            return true;
        }
    }
}