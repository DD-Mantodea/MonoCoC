using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System.Collections.Generic;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/SandMotherCombatHandlers.cs（已实现，76 行）
    [Register<SandMotherEnemy>("coc:sand_mother")]
    public class SandMotherEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 2, 3);

            CreateBreastRow(BreastCups.DD);

            Tallness = 8 * 12 + 6;

            Body.Get<SkinPart>().Tone = "bronzed";

            var hair = Body.Get<HairPart>();

            hair.Color = "platinum-blonde";

            hair.Length = 15;

            WeaponName = "fists";

            ArmorName = "robes";
        }

        public bool PreparingGigaFire;

        public float GigaHealth;

        public static readonly Identifier WhisperId = new("coc", "sand_mother_get_whispered");

        public static readonly Identifier LightningId = new("coc", "sand_mother_eat_a_lightning_bolt");

        public static readonly Identifier GigaFireId = new("coc", "sand_mother_giga_fire");

        public static readonly Identifier GigaFire2Id = new("coc", "sand_mother_giga_fire2");

        public static readonly Identifier ShieldId = new("coc", "sand_mother_earth_shield");

        public static readonly Identifier TelekinesisId = new("coc", "sand_mother_telekinesis");

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            if (PreparingGigaFire)
                return new CombatAiAction { Ability = GigaFire2Id };

            var options = new List<(Identifier Id, int Weight)>();

            if (!context.Player.HasStatusEffect(StatusEffectIds.Whispered))
                options.Add((WhisperId, 1));

            options.Add((LightningId, 1));

            options.Add((GigaFireId, 1));

            if (!HasStatusEffect(StatusEffectIds.Earthshield))
                options.Add((ShieldId, 1));

            options.Add((TelekinesisId, 1));

            var total = 0;

            foreach (var option in options) total += option.Weight;

            var pick = Random.Shared.Next(total);

            foreach (var option in options)
            {
                if (pick < option.Weight) return new CombatAiAction { Ability = option.Id };
                pick -= option.Weight;
            }

            return new CombatAiAction { Ability = LightningId };
        }

        public override bool Defeated(bool hpVictory)
        {
            CoC.Instance.Flags.Set(GameFlagIds.SandMotherDefeated, 1);

            CoC.Instance.Flags.Set(GameFlagIds.SandMotherVictoryPending, 1);

            CoC.Instance.Flags.Set(GameFlagIds.SandWitchesFriendly, 1);

            CoC.Instance.Flags.Set(GameFlagIds.SandwitchThroneUnlocked, 1);

            CombatController.Instance?.Context.Append("Scenes.DesertCave.DefeatTheSandMother.PantingWord".Localized());

            CombatController.Instance?.Context.Append("Scenes.DesertCave.DefeatTheSandMother.InExhaustion".Localized());

            CombatController.Instance?.Context.Append("Scenes.DesertCave.DefeatTheSandMother.WithLust".Localized());

            CombatController.Instance?.Context.Append("Scenes.DesertCave.DefeatTheSandMother.SinksBackIntoThrone".Localized());

            return false;
        }

        public override bool Won(bool hpVictory)
        {
            Dungeons.DesertCaveRooms.LoseToTheSandMother();

            return true;
        }

    }
}