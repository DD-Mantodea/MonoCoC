using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Places;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    [Register<FarmersEnemy>("coc:farmers")]
    public class FarmersEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(9, 2);

            Balls = 2;

            BallSize = 1;

            CumMultiplier = 3;

            CreateVagina(false, 2, 2);

            CreateBreastRow(1);

            Tallness = 73;

            WeaponName = "pitchforks";

            WeaponVerb = "stab";

            WeaponAttack = 15;

            ArmorName = "chitin";

            ArmorDefense = 5;
        }

        public override void PerformCombatAction()
        {
            for (var i = 0; i < 4; i++)
                CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<OwcaScene>.Get().BeatUpOwca();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<OwcaScene>.Get().LoseToOwca();

            return true;
        }
    }

    [Register<VapulaFightEnemy>("coc:vapula_fight")]
    public class VapulaFightEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 3, 3);

            CreateBreastRow(0);

            Tallness = 74;

            Body.Get<BodyParts.Vanilla.SkinPart>().Tone = "red";

            Body.Get<BodyParts.Vanilla.HairPart>().Color = "black";

            Body.Get<BodyParts.Vanilla.HairPart>().Length = 15;

            WeaponName = "claws";

            WeaponVerb = "claw";

            WeaponAttack = 15;

            ArmorName = "demonic skin";

            ArmorDefense = 5;
        }

        public override void PerformCombatAction()
        {
            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<OwcaScene>.Get().DefeetVapulasHorde();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<OwcaScene>.Get().LoseOrSubmitToVapula(false);

            return true;
        }
    }

    [Register<LustyDemonsEnemy>("coc:lusty_demons")]
    public class LustyDemonsEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(18, 2);

            CreateCock(18, 2);

            Balls = 2;

            BallSize = 1;

            CumMultiplier = 3;

            CreateVagina(false, 2, 2);

            CreateBreastRow(0);

            Tallness = 74;

            Body.Get<BodyParts.Vanilla.SkinPart>().Tone = "red";

            Body.Get<BodyParts.Vanilla.HairPart>().Color = "black";

            Body.Get<BodyParts.Vanilla.HairPart>().Length = 15;

            WeaponName = "claws";

            WeaponVerb = "claw";

            WeaponAttack = 10;

            ArmorName = "demonic skin";

            ArmorDefense = 5;
        }

        public override void PerformCombatAction()
        {
            var controller = CombatController.Instance!;

            var unitAmount = Math.Max(1, (int)Math.Ceiling(Health / 70f));

            for (var i = 0; i < unitAmount; i++)
                if (Random.Shared.Next(4) == 0)
                    controller.Resolver.EnemyAction(new CombatAbilityDefinition
                    {
                        NameKey = "Combat.Ability.Tease.Name",

                        Type = CombatAbilityType.Tease,

                        Power = 1
                    });
                else
                    controller.Resolver.EnemyBasicAttack();
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<OwcaScene>.Get().DefeetVapulasHorde();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<OwcaScene>.Get().LoseOrSubmitToVapula(false);

            return true;
        }
    }
}