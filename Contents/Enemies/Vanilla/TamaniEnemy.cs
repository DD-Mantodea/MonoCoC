using MonoCoC.Contents.BodyParts;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<TamaniEnemy>("coc:tamani")]
    public class TamaniEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 4, 1);

            CreateStatusEffect(StatusEffectIds.BonusVCapacity, 55);

            CreateBreastRow(6f);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 0;

            anal.Wetness = 0;

            CreateStatusEffect(StatusEffectIds.BonusACapacity, 40);

            Tallness = 40;

            Body.SetHipsRating("ample", 2);

            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "greenish gray";

            var hair = Body.Get<HairPart>();

            hair.Color = "pink and black";

            hair.Length = 16;

            Gems = Random.Shared.Next(25) + 5;

            ArmorName = "leather straps";

            WeaponName = "fists";

            WeaponVerb = "tiny punch";
        }

        public string TamaniChest()
        {
            var impregnated = CoC.Instance?.Flags.Get(GameFlagIds.TamaniTimesImpregnated) ?? 0;

            var adjective = impregnated switch
            {
                <= 0 => Random.Shared.Next(2) == 0

                    ? "Npcs.Tamani.Chest.Ample".Localized()

                    : "Npcs.Tamani.Chest.Handful".Localized(),

                <= 3 => Random.Shared.Next(2) == 0

                    ? "Npcs.Tamani.Chest.Large".Localized()

                    : "Npcs.Tamani.Chest.Jiggling".Localized(),

                <= 6 => Random.Shared.Next(2) == 0

                    ? "Npcs.Tamani.Chest.Wobbling".Localized()

                    : "Npcs.Tamani.Chest.Impressive".Localized(),

                <= 10 => Random.Shared.Next(2) == 0

                    ? "Npcs.Tamani.Chest.OverProportioned".Localized()

                    : "Npcs.Tamani.Chest.SuperSized".Localized(),

                _ => Random.Shared.Next(2) == 0

                    ? "Npcs.Tamani.Chest.Massive".Localized()

                    : "Npcs.Tamani.Chest.Gigantic".Localized()
            };

            var noun = Random.Shared.Next(11) switch
            {
                <= 2 => "Npcs.Tamani.Chest.Tits".Localized(),

                3 => "Npcs.Tamani.Chest.Melons".Localized(),

                4 => "Npcs.Tamani.Chest.Mounds".Localized(),

                5 => "Npcs.Tamani.Chest.Muffins".Localized(),

                6 => "Npcs.Tamani.Chest.Knockers".Localized(),

                7 => "Npcs.Tamani.Chest.LovePillows".Localized(),

                _ => "Npcs.Tamani.Chest.Breasts".Localized()
            };

            return adjective + noun;
        }

        public override string Description =>

            Definition == null || Definition.DescriptionKey == ""

                ? ""

                : Definition.DescriptionKey.Localized(TamaniChest());
    }
}