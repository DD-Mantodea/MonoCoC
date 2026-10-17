using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/SandWitchMobCombatHandlers.cs（已实现，88 行）
    [Register<SandWitchMobEnemy>("coc:sand_witch_mob")]
    public class SandWitchMobEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 2, 3);

            CreateBreastRow(BreastCups.DD);

            CreateBreastRow(BreastCups.DD);

            LustVulnerability = 0.5f;

            Tallness = Random.Shared.Next(12) + 55;

            Body.Get<SkinPart>().Tone = "bronzed";

            var hair = Body.Get<HairPart>();

            hair.Color = "sandy-blonde";

            hair.Length = 15;

            WeaponName = "fists";

            ArmorName = "robes";
        }

        public override bool Defeated(bool hpVictory)
        {
            CoC.Instance.Flags.Set(GameFlagIds.SandwitchMobDefeated, 1);

            CombatController.Instance?.Context.Append("Combat.Hel.SandWitchMobVictory".Localized());

            return false;
        }

        public override bool Won(bool hpVictory)
        {
            Dungeons.DesertCaveRooms.SandWitchMobLost();

            return true;
        }
    }
}