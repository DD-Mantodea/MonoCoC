using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using System;
using System.Collections.Generic;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    [Register<GooArmorEnemy>("coc:goo_armor")]
    public class GooArmorEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 4, 0);

            CreateBreastRow(3);

            Tallness = 72;

            Body.Get<SkinPart>().Tone = "blue";

            Body.Get<LowerBodyPart>().Id = BodyPartTypes.Goo;

            WeaponName = "goo sword";

            WeaponVerb = "slash";

            WeaponAttack = 60;

            ArmorName = "armor";

            ArmorDefense = 50;
        }

        public override void PerformCombatAction()
        {
            var context = CombatController.Instance!.Context;

            var player = context.Player;

            var options = new List<int> { 1, 1, 2, 2, 2 };

            if (!player.HasStatusEffect(StatusEffectIds.GooArmorSilence))
            {
                options.Add(0);

                options.Add(0);
            }

            switch (options[Random.Shared.Next(options.Count)])
            {
                case 0: Silence(context, player); break;
                case 1: Physical(context, player); break;
                default: GooConsume(context, player); break;
            }
        }

        public void Physical(CombatContext context, Player player)
        {
            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = true,

                DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Write("Scenes.GooArmor.GooArmorAttackPhysical.SwordArcEvaded".Localized());

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, Strength + WeaponAttack);

            if (damage <= 0) damage = 1;

            context.Write("Common.Text.TheGooArmorRushesForwardAnd".Localized());

            player.HealthChange(-damage);
        }

        public void GooConsume(CombatContext context, Player player)
        {
            context.Write("Common.Text.SuddenlyTheGooGirlLeaksHalf".Localized());

            player.CreateStatusEffect(StatusEffectIds.GooArmorBind, 0, 0, 0, 0);
        }

        public void Silence(CombatContext context, Player player)
        {
            context.Write("Scenes.GooArmor.GooSilenceAttack.GooMouthSilence".Localized());

            player.CreateStatusEffect(StatusEffectIds.GooArmorSilence, 0, 0, 0, 0);
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<ValeriaScene>.Get().PcWinsValeriaSpar();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<ValeriaScene>.Get().PcWinsValeriaSparDefeat();

            return true;
        }
    }
}