using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<BazaarMobEnemy>("coc:mob")]
    public class BazaarMobEnemy : Enemy
    {

        public float UnitHp = 50f;

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

            LustVulnerability = 0.5f;

            Gems = Random.Shared.Next(5) + 5;

            Stats.AddModifier(GameStats.MaxHealth, ModifierOperation.Addition,

                () => UnitAmount * 100f - Math.Min(9999f, Toughness * 2 + 50 + Level * 15),

                new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;
        }

        public override bool BeforeCombatAction(CombatContext context)
        {
            UnitAmount = (int)MathF.Ceiling(Health / 100f);

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

                    WeaponName = "spear";

                    WeaponAttack = 25;

                    break;

                case 1:
                    WeaponVerb = "slash";

                    WeaponName = "scimitar";

                    WeaponAttack = 25;

                    break;

                case 2:
                    WeaponVerb = "crush";

                    WeaponName = "mace";

                    WeaponAttack = 15;

                    break;

                case 3:
                    WeaponVerb = "bash";

                    WeaponName = "club";

                    WeaponAttack = 30;

                    break;

                default:

                    WeaponVerb = "pick";

                    WeaponName = "pickaxe";

                    WeaponAttack = 20;

                    break;
            }
        }

    }
}