using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/FetishZealotCombatHandlers.cs（已实现，90 行）
    [Register<FetishZealotEnemy>("coc:fetish_zealot")]
    public class FetishZealotEnemy : Enemy
    {

        public const string ReligiousClothes = "religious clothes";

        public const string LeatherClothes = "leather clothes";

        public const string StudentsClothes = "student's clothes";

        public bool ShotaZealot;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(4, 0.9f);

            CreateCock(6, 1.5f);

            var hair = Body.Get<HairPart>();

            hair.Length = 4;

            WeaponName = "wavy dagger";
        }

        public override void CombatRoundUpdate(Combat.CombatContext context)
        {
            base.CombatRoundUpdate(context);

            var outfit = Datas.Vanilla.OutfitIds.ZealotOutfits[context.Random.Next(Datas.Vanilla.OutfitIds.ZealotOutfits.Length)];

            if (ArmorName != outfit)
            {
                ArmorName = outfit;

                context.Append("Combat.FetishZealot.OutfitShift".Localized());
            }

            Lust = Math.Min(MaxLust, Lust + LustVulnerability * 5);
        }
    }
}