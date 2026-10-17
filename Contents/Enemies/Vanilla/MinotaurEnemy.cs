using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<MinotaurEnemy>("coc:minotaur")]
    public class MinotaurEnemy : Enemy
    {

        public bool HasAxe { get; set; }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            HasAxe = Random.Shared.Next(3) == 0;

            var furColor = new[] { "black", "brown" }[Random.Shared.Next(2)];

            CreateCock(Random.Shared.Next(13) + 24, 2 + Random.Shared.Next(3), BodyPartTypes.Horse);

            Balls = 2;

            BallSize = 2 + Random.Shared.Next(13);

            CreateBreastRow(0);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 4;

            anal.Wetness = 1;

            if (!HasStatusEffect(StatusEffectIds.BonusACapacity))
                CreateStatusEffect(StatusEffectIds.BonusACapacity, 30);

            Tallness = Random.Shared.Next(37) + 84;

            Body.SetHipsRating("average");

            Body.SetButtRating("average");

            Body.Get<LowerBodyPart>().Id = BodyPartTypes.Hoofed;

            var skin = Body.Get<SkinPart>();

            skin.FurColor = furColor;

            skin.Tone = furColor;

            skin.Id = BodyPartTypes.Fur;

            var hair = Body.Get<HairPart>();

            hair.Color = furColor;

            hair.Length = 3;

            Body.Get<FacePart>().Id = BodyPartTypes.CowMinotaur;

            Strength = HasAxe ? 75 : 50;

            Toughness = 60;

            Speed = 30;

            Intelligence = 20;

            Libido = 40 + BallSize * 2;

            Sensitivity = 15 + BallSize * 2;

            Corruption = 35;

            WeaponName = HasAxe ? "axe" : "fist";

            WeaponVerb = HasAxe ? "cleave" : "punch";

            ArmorName = "thick fur";

            Lust = BallSize * 3;

            LustVulnerability = HasAxe ? 0.84f : 0.87f;

            Level = HasAxe ? 6 : 5;

            Gems = Random.Shared.Next(5) + 5;

            // 原版 Minotaur.hx:94-99：掉落由 Loot 覆盖提供（依 hasAxe 走两条 ChainedDrop）
        }

        /// <summary>
        /// 原版 Minotaur.hx:94：有斧时 MINOBLO(1/2) → (矮小且力量低 ? SDELITE : L__AXE)(1/2) → 无；
        /// 无斧时 MINOCUM(1/5) → MINOBLO(1/2) → 无。
        /// </summary>
        public override System.Collections.Generic.IReadOnlyList<EnemyLoot> Loot
        {
            get
            {
                if (HasAxe)
                {
                    var player = CoC.Instance.Player;

                    var second = player.Tallness < 78 && player.Strength < 90

                        ? ConsumableIds.SuccubusDelight

                        : WeaponIds.LargeAxe;

                    return

                    [
                        new EnemyLoot { Item = ConsumableIds.MinotaurBlood, Chance = 0.5f },

                        new EnemyLoot { Item = second, Chance = 0.5f },
                    ];
                }

                return

                [
                    new EnemyLoot { Item = ConsumableIds.MinotaurCum, Chance = 0.2f },

                    new EnemyLoot { Item = ConsumableIds.MinotaurBlood, Chance = 0.5f },
                ];
            }
        }

        public override string Description => BuildDescription();

        public string BuildDescription()
        {
            var text = "Npcs.Minotaur.Desc".Localized(Body.Get<HairPart>().Color);

            if (BallSize > 4)
                text += "Npcs.Minotaur.Desc.BigBalls".Localized(BallsDescript(true, true));

            if (HasAxe)
                text += "Npcs.Minotaur.Desc.Axe".Localized();

            return text;
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Mountain.MinotaurScene>.Get().MinoVictoryRapeChoices();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (HasStatusEffect(StatusEffectIds.Infested))
            {
                Scenes.Scene.OutputText("Combat.Minotaur.CameWorms".Localized());

                Combat.CombatScene.CleanupAfterCombat();

                return true;
            }

            Scenes.SceneInstance<Scenes.Vanilla.Areas.Mountain.MinotaurScene>.Get().GetRapedByMinotaur();

            return true;
        }
    }
}