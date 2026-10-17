using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;
using System.Linq;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class BoneCourtierCombatHandlers
    {

        [Register("coc:bone_courtier_tempting_goblet")]
        public static void TemptingGoblet(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.BoneCourtier.TemptingGoblet.TheBoneSorcerer".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = context.Player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
            {

                context.Append(avoid.DodgeReason switch
                {
                    "Evade" => "Combat.BoneCourtier.TemptingGoblet.YouRollAway".Localized(),

                    "Misdirection" => "Combat.BoneCourtier.TemptingGoblet.YouManageTo".Localized(),

                    "Flexibility" => "Combat.BoneCourtier.TemptingGoblet.YouTwistOut".Localized(),

                    _ => "Combat.BoneCourtier.TemptingGoblet.YouReFast".Localized()
                });

                return;
            }

            context.Append("Combat.BoneCourtier.TemptingGoblet.TheLiquidHits".Localized());

            var damage = CombatCalculator.ReduceDamage(context, context.Player, 20 + context.Random.Next(50));

            context.Player.TakeDamage(damage);

            context.Player.TakeLustDamage(20 + context.Player.Libido / 10f);
        }

        [Register("coc:bone_courtier_mark")]
        public static void Mark(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.BoneCourtier.Mark.TheBoneSorcerer".Localized());

            context.Player.CreateStatusEffect(StatusEffectIds.Marked, 2, 0, 0, 0);
        }

        [Register("coc:bone_courtier_heal")]
        public static void Heal(CombatContext context, CombatAbilityDefinition ability)
        {
            const float healPercent = 0.25f;

            context.Append("Combat.BoneCourtier.Heal.TheBoneSorcerer".Localized());

            if (context.Enemies.Count == 1)
            {
                var selfHeal = healPercent * context.Enemy.Health;

                context.Enemy.AddHp(selfHeal);

                context.Append("Combat.BoneCourtier.Heal.TheBoneSorcerer2".Localized("#00ff00", (int)MathF.Round(selfHeal)));

                return;
            }

            var necromancer = context.Enemies.FirstOrDefault(e => e is NecromancerEnemy && e.Health > 0);

            if (necromancer != null)
            {
                var heal = healPercent * necromancer.Health;

                necromancer.AddHp(heal);

                context.Append("Combat.BoneCourtier.Heal.OneIs".Localized(
                    Utils.Utils.CapitalizeFirstLetter(necromancer.Article),

                    necromancer.ShortName,

                    "#00ff00",

                    (int)MathF.Round(heal)));

                return;
            }

            var candidates = context.Enemies.Where(e => e.Health > 0).ToList();

            if (candidates.Count == 0)
                return;

            var target = candidates[context.Random.Next(candidates.Count)];

            var healAmount = healPercent * target.Health;

            target.AddHp(healAmount);

            context.Append("Combat.BoneCourtier.Heal.OneIs".Localized(
                Utils.Utils.CapitalizeFirstLetter(target.Article),

                target.ShortName,

                "#00ff00",

                (int)MathF.Round(healAmount)));
        }
    }
}