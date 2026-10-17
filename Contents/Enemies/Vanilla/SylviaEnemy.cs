using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Bog;
using System;
using System.Collections.Generic;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    [Register<SylviaEnemy>("coc:sylvia")]
    public class SylviaEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 4, 0);

            CreateBreastRow(6);

            Tallness = 70;

            Body.Get<SkinPart>().Tone = "pale white";

            Body.Get<HairPart>().Color = "purple";

            Body.Get<HairPart>().Length = 8;

            WeaponName = "chitin-plated fist";

            ArmorName = "chitin";
        }

        public override void PerformCombatAction()
        {
            var context = CombatController.Instance!.Context;

            var player = context.Player;

            var options = new List<int>();

            if (!player.HasStatusEffect(StatusEffectIds.MothDose))
                for (var i = 0; i < 10; i++) options.Add(0);
            if (!HasStatusEffect(StatusEffectIds.StunCooldown))
                for (var i = 0; i < 7; i++) options.Add(1);
            for (var i = 0; i < 5; i++) options.Add(2);
            options.Add(3);

            switch (options[Random.Shared.Next(options.Count)])
            {
                case 0: Pheromones(context, player); break;
                case 1: Hypnotize(context, player); break;
                case 2: Seduce(context, player); break;
                default: CombatController.Instance!.Resolver.EnemyBasicAttack(); break;
            }
        }

        public void Pheromones(CombatContext context, Player player)
        {
            context.Write("Scenes.Sylvia.SylviaPheromones.WingGustPheromones".Localized());

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
                context.Write("Common.Text.YourQuickReactionAllowsYouTo".Localized());

                return;
            }

            context.Write("Common.Text.UnableToGetOutOfThe".Localized());

            player.CreateStatusEffect(StatusEffectIds.MothPheromones, 5, 0, 0, 0);

            CombatCalculator.AddCombatBuff(player, GameStats.Sensitivity, 20, "sylvia_pheromones", StatusEffectIds.MothPheromones);

            CombatCalculator.AddCombatBuff(player, GameStats.Speed, -20, "sylvia_pheromones_speed", StatusEffectIds.MothPheromones);
        }

        public void Hypnotize(CombatContext context, Player player)
        {
            context.Write("Scenes.Sylvia.SylviaHypnotize.GrappleApproach".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = true
            });

            if (avoid.AttackFailed)
            {
                context.Write("Common.Text.YouDeftlyManageToAvoidThe".Localized());

                return;
            }

            context.Write("Common.Text.TheMothWrapsHerArmsAround".Localized());

            if (player.Stun(2, 66))
                context.Write("Common.Text.WhenShePullsAwayYouStumble".Localized());

            CreateStatusEffect(StatusEffectIds.StunCooldown, 4, 0, 0, 0);

            player.TakeLustDamage(Random.Shared.Next(30) + 20);
        }

        public void Seduce(CombatContext context, Player player)
        {
            switch (Random.Shared.Next(3))
            {
                case 0:
                    context.Write("Scenes.Sylvia.SeduceAttack.ChestPuffTease".Localized());

                    break;

                case 1:
                    context.Write("Scenes.Sylvia.SeduceAttack.AerialManeuver".Localized());

                    break;

                default:
                    context.Write("Common.Text.SheSuddenlySwoopsInCloseFrom".Localized());

                    break;
            }

            player.TakeLustDamage(Random.Shared.Next(15) + 10);
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<SylviaScene>.Get().SylviaVictory();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<SylviaScene>.Get().SylviaDefeat();

            return true;
        }
    }
}