using MonoCoC.Attributes;
using MonoCoC.Extensions;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class HarpyMobCombatHandlers
    {
        [Register("coc:harpy_mob_harpy_horde_claw_flurry")]
        public static void ClawFlurry(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = (HarpyMobEnemy)context.Enemy;

            var player = context.Player;

            context.Append("Combat.Ability.HarpyMobClawFlurry.Text".Localized());

            var hits = 3 + Random.Shared.Next(3);

            for (var index = 0; index < hits; index++)
            {
                var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
                {
                    Attacker = enemy,

                    Defender = player,

                    DoDodge = true,

                    DoParry = false,

                    DoBlock = false,

                    DoFatigue = false
                });

                if (avoid.Outcome == AvoidOutcome.Dodge)
                    continue;

                player.HealthChange(-(8 + Random.Shared.Next(8)));
            }
        }

        // 原版 HarpyMob.hx:41 harpyHordeGangBangAttack：擒住后立刻判定一次挣扎
        [Register("coc:harpy_mob_harpy_horde_gang_bang_attack")]
        public static void GangBang(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Ability.HarpyMobGangBang.Text".Localized());

            player.CreateStatusEffect(StatusEffectIds.HarpyBind, 0, 0, 0, 0);

            Struggle(context, false);
        }

        // 原版 HarpyMob.hx:48 harpyHordeGangBangStruggle
        public static void Struggle(CombatContext context, bool clearDisplay = true)
        {
            var player = context.Player;

            if (clearDisplay)
                context.Clear();

            // 失败条件：rand(10) > 0 && Str/5 + rand(20) < 23
            if (context.Random.Next(10) > 0 && player.Strength / 5f + context.Random.Next(20) < 23)
            {
                context.Append("Combat.HarpyMob.Struggle.Failed".Localized());

                player.HealthChange(-(80 + context.Random.Next(40)));
            }
            else
            {
                player.RemoveStatusEffect(StatusEffectIds.HarpyBind);

                context.Append("Combat.HarpyMob.Struggle.Escaped".Localized());
            }
        }

        [Register("coc:harpy_mob_harpy_horde_lust_attack")]
        public static void LustAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Ability.HarpyMobLustAttack.Text".Localized());

            var lust = 15;

            if (player.HasCock())
                lust += 15;

            player.TakeLustDamage(lust);
        }
    }
}