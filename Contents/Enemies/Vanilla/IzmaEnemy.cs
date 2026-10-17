using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using System;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    [Register<IzmaEnemy>("coc:izma")]
    public class IzmaEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(15, 2.2f);

            Balls = 4;

            BallSize = 3;

            CreateVagina(false, 2, 2);

            CreateBreastRow(5);

            Tallness = 65;

            Body.Get<SkinPart>().Tone = "striped red-orange";

            Body.Get<HairPart>().Color = "silver";

            Body.Get<HairPart>().Length = 20;

            WeaponName = "clawed gauntlets";

            WeaponVerb = "clawed punches";

            WeaponAttack = 45;

            ArmorName = "bikini and grass skirt";

            ArmorDefense = 8;
        }

        public override void PerformCombatAction()
        {
            switch (Random.Shared.Next(4))
            {
                case 0:
                    CombatController.Instance!.Resolver.EnemyBasicAttack();

                    break;

                case 1:
                    PressurePoints(CombatController.Instance!.Context);

                    break;

                case 2:
                    Clinch(CombatController.Instance!.Context);

                    break;

                default:
                    Tease(CombatController.Instance!.Context);

                    break;
            }
        }

        public void PressurePoints(CombatContext context)
        {
            var player = context.Player;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = true
            });

            if (avoid.AttackFailed)
                return;

            context.Write("Common.Text.IzmaRushesYouWithImpressiveSpeed".Localized());

            player.ChangeFatigue(20 + Random.Shared.Next(20));
        }

        public void Clinch(CombatContext context)
        {
            var player = context.Player;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = true
            });

            if (avoid.AttackFailed)
                return;

            var damage = Math.Max(0, 130 - Random.Shared.Next((int)(player.Toughness + player.ArmorDefense)));

            context.Write("Scenes.Izma.IzmaSpecials2.ClinchLock".Localized());

            if (player.ArmorDefense <= 10 || damage > 0)
            {
                context.Write("Scenes.Izma.IzmaSpecials2.BladeDragWrithe".Localized());

                player.Bleed(this);
            }
            else
                context.Write("Common.Text.LaughAsHerBladesScrapeUselessly".Localized());

            context.Write("Scenes.Izma.IzmaSpecials2.EmbraceBreakLeap".Localized());

            player.HealthChange(-damage);
        }

        public void Tease(CombatContext context)
        {
            context.Write("Scenes.Izma.IzmaSpecials3.SelfFondleTaunt".Localized());

            context.Player.TakeLustDamage(20 + context.Player.Libido / 5);
        }

        public override bool Defeated(bool hpVictory)
        {
            var scene = SceneInstance<IzmaScene>.Get();

            if (scene.IzmaFollower())
                scene.WinSpar();
            else
                scene.DefeatIzma();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            var scene = SceneInstance<IzmaScene>.Get();

            if (scene.IzmaFollower())
                scene.LoseSpar(hpVictory);
            else
                scene.IzmaWins();

            return true;
        }
    }
}