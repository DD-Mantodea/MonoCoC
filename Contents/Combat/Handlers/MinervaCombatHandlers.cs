using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class MinervaCombatHandlers
    {

        [Register("coc:minerva_bite")]
        public static void Bite(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Minerva.Bite.Lead".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + 85);

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = true
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
            {
                context.Append("Combat.Minerva.Bite.Dodged".Localized());

                return;
            }

            if (avoid.Outcome == AvoidOutcome.Block)
            {
                context.Append("Combat.Minerva.Bite.Blocked".Localized());

                return;
            }

            if (damage <= 0)
            {
                context.Append("Combat.Minerva.Bite.Deflected".Localized());

                return;
            }

            context.Append("Combat.Minerva.Bite.Hit".Localized());

            player.HealthChange(-damage);
        }

        [Register("coc:minerva_knows_kungfu")]
        public static void KnowsKungfu(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Minerva.Kungfu.Lead".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack + 100);

            enemy.Speed -= 70;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
                context.Append("Combat.Minerva.Kungfu.Dodged".Localized());
            else if (damage <= 0)
                context.Append("Combat.Minerva.Kungfu.Deflected".Localized());
            else
            {
                context.Append("Combat.Minerva.Kungfu.Hit".Localized());

                player.HealthChange(-damage);
            }

            enemy.Speed += 70;
        }

        [Register("coc:minerva_tail_whip")]
        public static void TailWhip(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + 35);

            context.Append("Combat.Minerva.TailWhip.Lead".Localized());

            if (damage <= 0)
            {
                context.Append("Combat.Minerva.TailWhip.Deflected".Localized());

                return;
            }

            context.Append(player.ArmorDefense > 0

                ? "Combat.Minerva.TailWhip.HitArmor".Localized()

                : "Combat.Minerva.TailWhip.Hit".Localized());

            player.HealthChange(-damage);

            CombatCalculator.AddArmorBonus(player, -10);

            player.CreateStatusEffect(StatusEffectIds.TailWhip, 10);
        }

        [Register("coc:minerva_uses_halberd_stab")]
        public static void HalberdStab(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Minerva.HalberdStab.Lead".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack) - 20;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
                context.Append("Combat.Minerva.HalberdStab.Dodged".Localized());
            else if (damage < 0)
                context.Append("Combat.Minerva.HalberdStab.Deflected".Localized());
            else
            {
                context.Append("Combat.Minerva.HalberdStab.Hit".Localized());

                player.HealthChange(-damage);
            }
        }

        [Register("coc:minerva_uses_halberd_chop")]
        public static void HalberdChop(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Minerva.HalberdChop.Lead".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + 100);

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
                context.Append("Combat.Minerva.HalberdChop.Dodged".Localized());
            else if (damage < 0)
                context.Append("Combat.Minerva.HalberdChop.Deflected".Localized());
            else
            {
                context.Append("Combat.Minerva.HalberdChop.Hit".Localized());

                player.HealthChange(-damage);
            }
        }

        [Register("coc:minerva_kite_fire")]
        public static void KiteFire(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Minerva.KiteFire.Lead".Localized());

            var damage = (int)(10 + (enemy.Intelligence / 3f + context.Random.Next(Math.Max(1, (int)(enemy.Intelligence / 2)))) * 1.5f);

            player.HealthChange(-damage);
        }

        [Register("coc:minerva_booty_short_in_yo_face_son")]
        public static void BootyShort(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Minerva.BootyShort.Lead".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + 20 + player.Libido / 10 + context.Random.Next(5));
        }

        [Register("coc:minerva_lick_dat_pole")]
        public static void LickDatPole(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Minerva.LickDatPole.Lead".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + 20 + player.Libido / 10 + context.Random.Next(5));
        }

        [Register("coc:minerva_sirens_song")]
        public static void SirensSong(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (!enemy.HasStatusEffect(StatusEffectIds.SirenSong))
            {
                context.Append("Combat.Minerva.SirensSong.Part1".Localized());

                enemy.CreateStatusEffect(StatusEffectIds.SirenSong);

                return;
            }

            context.Append("Combat.Minerva.SirensSong.Part2".Localized());

            if (context.PlayerWaited)
                context.Append("Combat.Minerva.SirensSong.Waited".Localized());
            else
            {
                context.Append("Combat.Minerva.SirensSong.Lost".Localized());

                player.Lust = player.MaxLust;
            }

            enemy.RemoveStatusEffect(StatusEffectIds.SirenSong);
        }
    }
}