using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Combat.Handlers;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<HelspawnEnemy>("coc:helspawn")]
    public class HelspawnEnemy : Enemy
    {

        public bool UsedFocus;

        public string Weapon => CoC.Instance.Flags.Get(GameFlagIds.HelspawnWeapon);

        public int Personality => CoC.Instance.Flags.Get(GameFlagIds.HelspawnPersonality);

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            var weapon = Weapon;

            var name = CoC.Instance.Flags.Get(GameFlagIds.HelspawnName);

            if (!string.IsNullOrEmpty(name))
                ShortName = name;

            CreateVagina(false, 1, 2);

            CreateStatusEffect(StatusEffectIds.BonusVCapacity, 85);

            CreateBreastRow(BreastCups.EBig);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 0;

            anal.Wetness = 0;

            CreateStatusEffect(StatusEffectIds.BonusACapacity, 85);

            Tallness = 78;

            Body.Hips = 12;

            Body.Butt = 9;

            Body.Get<SkinPart>().Tone = "dusky";

            var hair = Body.Get<HairPart>();

            hair.Color = "red";

            hair.Length = 13;

            Strength = 50;

            Toughness = 50;

            Speed = 65;

            Intelligence = 40;

            Libido = 35;

            Sensitivity = 55;

            Corruption = 20;

            WeaponName = string.IsNullOrEmpty(weapon) ? "scimitar" : weapon;

            WeaponVerb = weapon switch
            {
                "bow" => "blunted arrow",

                _ => "slash"
            };

            WeaponAttack = 20;

            if (weapon == "bow")
            {
                NormalAttackAvoid.DoParry = false;

                NormalAttackAvoid.DoCounter = false;
            }

            ArmorName = "scales";

            ArmorDefense = 12;

            ArmorPerk = "";

            ArmorValue = 50;

            Lust = 30;

            LustVulnerability = 0.55f;

            Level = 12;

            Gems = 10 + Random.Shared.Next(5);

            Body.Get<TailPart>().Id = BodyPartTypes.Salamander;

        }

        public override string Description => "Npcs.Helspawn.Desc".Localized(
            ShortName,

            Personality >= 50

                ? "Npcs.Helspawn.Outfit.Slutty".Localized()

                : "Npcs.Helspawn.Outfit.Modest".Localized(),

            Weapon switch
            {
                "bow" => "Npcs.Helspawn.Weapon.Bow".Localized(),

                "scimitar and shield" => "Npcs.Helspawn.Weapon.Shield".Localized(),

                _ => "Npcs.Helspawn.Weapon.Scimitar".Localized()
            });

        public override bool BeforeCombatAction(CombatContext context)
        {
            if (context.Random.Next(4) == 0)
                HelspawnCombatHandlers.TailWhip(context);

            return false;
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Npcs.HelSpawnScene>.Get().BeatUpYourDaughter();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Npcs.HelSpawnScene>.Get().LoseSparringToDaughter();

            return true;
        }
    }
}