using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/BrigidCombatHandlers.cs（已实现，48 行）
    [Register<BrigidEnemy>("coc:brigid")]
    public class BrigidEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 5, 3);

            Tallness = Random.Shared.Next(8) + 70;

            Body.Get<SkinPart>().Tone = "red";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 15;

            WeaponName = "poker";

            ArmorName = "armor";
        }

        public override bool Defeated(bool hpVictory)
        {
            CoC.Instance.Flags.Set(Flags.GameFlagIds.HelBrigidDefeated, 1);

            CombatController.Instance?.Context.Append("Combat.Hel.BrigidVictory".Localized());

            Scenes.SceneInstance<Scenes.Vanilla.Dungeons.HelDungeon.BrigidScene>
                .Get().PcDefeatsBrigid(hpVictory);

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            CombatController.Instance?.Context.Append("Combat.Hel.BrigidDefeat".Localized());

            Scenes.SceneInstance<Scenes.Vanilla.Dungeons.HelDungeon.BrigidScene>
                .Get().PcDefeatedByBrigid();

            return true;
        }
    }
}
