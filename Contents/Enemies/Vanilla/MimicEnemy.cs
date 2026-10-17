using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<MimicEnemy>("coc:mimic")]
    public class MimicEnemy : Enemy
    {

        public const int AppearanceRock = 0;

        public const int AppearanceBoob = 1;

        public const int AppearanceDick = 2;

        public const int AppearanceChest = 3;

        public const float Jacques00 = 199f;

        public int AppearanceType = AppearanceChest;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            if (AppearanceType == AppearanceDick)
                CreateCock(80, 16, BodyPartTypes.Human);

            Balls = 0;

            BallSize = 0;

            CreateBreastRow(AppearanceType == AppearanceBoob ? Jacques00 : 0);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 0;

            Tallness = Random.Shared.Next(24) + 25;

            Body.SetHipsRating("boyish");

            Body.SetButtRating("tight");

            if (AppearanceType == AppearanceRock)
                Body.Get<SkinPart>().Tone = "gray";

            if (AppearanceType == AppearanceBoob || AppearanceType == AppearanceDick)
                Body.Get<SkinPart>().Tone = "light";
            else
                Body.Get<SkinPart>().Tone = "brown";

            var hair = Body.Get<HairPart>();

            hair.Color = "none";

            hair.Length = 0;

            WeaponName = "mouth";

            WeaponVerb = "bite";

            WeaponAttack = 20 + (int)(CoC.Instance?.Player?.Level ?? 0);

            if (AppearanceType == AppearanceRock)
            {
                ArmorName = "stone surface";

                ArmorDefense = 70;
            }

            if (AppearanceType == AppearanceDick || AppearanceType == AppearanceBoob)
            {
                ArmorName = "flesh";

                ArmorDefense = 0;
            }
            else
            {
                ArmorName = "wooden surface";

                ArmorDefense = 20;
            }

            LustVulnerability = 0;

            Gems = Random.Shared.Next(30) + 11;

            CreatePerk(PerkIds.Immovable);

        }

        public override string Description => AppearanceType switch
        {
            AppearanceRock => "Npcs.Mimic.DescRock".Localized(),

            AppearanceBoob => "Npcs.Mimic.DescBoob".Localized(),

            AppearanceDick => "Npcs.Mimic.DescDick".Localized(),

            _ => base.Description
        };

        public override bool BeforeCombatAction(CombatContext context)
        {
            if (Distance == CombatDistance.Distant)
            {
                context.WriteAction("Combat.Mimic.PerformCombatAction.TheCreatureSits".Localized());

                return true;
            }

            return false;
        }

        public static MimicEnemy Create(EnemyDefinition definition, int appearanceType)
        {
            var enemy = new MimicEnemy { AppearanceType = appearanceType };

            enemy.Id = definition.Id;

            enemy.Configure(definition);

            enemy.ApplyDefinitionEquipment(definition);

            return enemy;
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Forest.MimicScene>
                .Get().KillTheMimic();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Forest.MimicScene>
                .Get().MimicTentacle2();

            return true;
        }
    }
}
