using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<SkeletonHordeEnemy>("coc:skeleton_horde")]
    public class SkeletonHordeEnemy : Enemy
    {

        public float UnitHp = 100f;

        public int UnitAmount = 4;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateBreastRow(BreastCups.E);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 0;

            CreateStatusEffect(StatusEffectIds.BonusACapacity, 30);

            Tallness = 35 + Random.Shared.Next(4);

            Body.SetHipsRating("ample", 2);

            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "dark green";

            var hair = Body.Get<HairPart>();

            hair.Color = "purple";

            hair.Length = 4;

            WeaponName = "fists";

            WeaponVerb = "tiny punch";

            ArmorName = "leather straps";

            Gems = Random.Shared.Next(5) + 5;

            LustVulnerability = 0;

            CreatePerk(PerkIds.BleedImmune);

            CreatePerk(PerkIds.PoisonImmune);

            Stats.AddModifier(GameStats.MaxHealth, ModifierOperation.Addition,

                () => UnitAmount * UnitHp - Math.Min(9999f, Toughness * 2 + 50 + Level * 15),

                new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;
        }

        public override bool BeforeCombatAction(CombatContext context)
        {
            UnitAmount = (int)Math.Ceiling(Health / UnitHp);

            context.Append("\n");

            for (var i = 0; i < UnitAmount; i++)
            {
                RecombobulateWeapon();

                CombatController.Instance!.Resolver.EnemyBasicAttack();
            }

            return true;
        }

        public void RecombobulateWeapon()
        {
            switch (Random.Shared.Next(4))
            {
                case 0:
                    WeaponVerb = "stab";

                    WeaponName = "pitchfork";

                    WeaponAttack = 50;

                    break;

                case 1:
                    WeaponVerb = "slash";

                    WeaponName = "rusty sword";

                    WeaponAttack = 70;

                    break;

                case 2:
                    WeaponVerb = "crush";

                    WeaponName = "rusty mace";

                    WeaponAttack = 60;

                    break;

                case 3:
                    WeaponVerb = "slap";

                    WeaponName = "femur";

                    WeaponAttack = 30;

                    break;

                default:

                    WeaponVerb = "pick";

                    WeaponName = "pickaxe";

                    WeaponAttack = 40;

                    break;
            }
        }

        public override bool Plural => UnitAmount != 1;

        public override void TickStatuses(CombatContext context) =>

            BoneGeneralEnemy.TickGuardAb(context, this);

        public override bool HandleFear(CombatContext context)
        {
            context.WriteAction("Combat.SkeletonHorde.HandleFear.TheSkeletonsAren".Localized());

            RemoveStatusEffect(StatusEffectIds.Fear);

            return true;
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<ManorScene>.Get().DefeatNecro();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<ManorScene>.Get().LoseToNecro();

            return true;
        }

    }
}