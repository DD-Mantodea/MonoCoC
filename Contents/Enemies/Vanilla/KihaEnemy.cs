using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Configuration;
using System;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    [Register<KihaEnemy>("coc:kiha")]
    public class KihaEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 4, 2);

            CreateBreastRow(4);

            Tallness = 73;

            Body.Get<SkinPart>().Tone = "dark";

            Body.Get<HairPart>().Color = "red";

            Body.Get<HairPart>().Length = 3;

            WeaponName = "double-bladed axe";

            WeaponVerb = "fiery cleave";

            WeaponAttack = 25;

            ArmorName = "thick scales";

            ArmorDefense = 30;
        }

        public override void PerformCombatAction()
        {
            switch (Random.Shared.Next(4))
            {
                case 0:
                    CombatController.Instance!.Resolver.EnemyBasicAttack();

                    break;

                case 1:
                    if (OptionStore.Instance.Get("silly", 0) > 0)
                        SillyAttack(CombatController.Instance!.Context);
                    else
                        FirePunch(CombatController.Instance!.Context);

                    break;

                case 2:
                    FireBreath(CombatController.Instance!.Context);

                    break;

                default:
                    TimeWaster(CombatController.Instance!.Context);

                    break;
            }
        }

        public void TimeWaster(CombatContext context)
        {
            context.Write("Scenes.Kiha.KihaTimeWaster.AxeShoulderStretch".Localized());

            context.Player.TakeLustDamage(5);
        }

        public void FirePunch(CombatContext context)
        {
            var player = context.Player;

            context.Write("Common.Text.TheDraconicGirlThrowsHerTrusty".Localized());

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

            var damage = CombatCalculator.ReduceDamage(context, player, Strength);

            context.Write("Scenes.Kiha.KihaFirePunch.FirePunchHits".Localized());

            player.HealthChange(-damage);
        }

        public void FireBreath(CombatContext context)
        {
            var player = context.Player;

            context.Write("Common.Text.KihaThrowsHerArmsBackAnd".Localized());

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

            var damage = 90 + Random.Shared.Next(10);

            context.Write("Scenes.Kiha.KihaFireBreath.InfernoSlams".Localized());

            player.HealthChange(-damage);
        }

        public void SillyAttack(CombatContext context)
        {
            var player = context.Player;

            context.Write("Common.Text.BeforeYouCanStopToThink".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Write("Common.Text.YouTakeTheOpportunityToWalk".Localized());

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, Strength + WeaponAttack) + 5;

            context.Write("Common.Text.ATorrentOfHeatBurstsFrom".Localized());

            player.HealthChange(-damage);
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<KihaScene>.Get().KihaVictoryIntroduction();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<KihaScene>.Get().KihaLossIntro();

            return true;
        }
    }
}