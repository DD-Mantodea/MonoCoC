using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class PhysicalSpecialHandlers
    {

        public static float KickCalc(CombatContext context, Player player)
        {
            var damage = player.Strength;

            var lowerBody = player.Body.Get<LowerBodyPart>().Id.Path;

            if (lowerBody is "hoofed" or "pony" or "cloven_hoofed")
                damage += 30;
            else if (lowerBody == "bunny")
                damage += 20;
            else if (lowerBody == "kangaroo")
                damage += 35;

            if (player.IsTaur())
                damage += 10;

            damage *= player.PhysMod();

            damage = CombatCalculator.BodyMod(player, damage);

            damage = CombatCalculator.GlobalMod(player, damage);

            return CombatCalculator.ReduceDamage(context, context.Enemy, damage);
        }

        [Register("coc:special_kick")]
        public static void Kick(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var lowerBody = player.Body.Get<LowerBodyPart>().Id.Path;

            var intro = lowerBody switch
            {
                "kangaroo" => player.Body.Get<TailPart>().Id.Path == "kangaroo"

                    ? "Combat.Special.Kick.KangarooTail"

                    : "Combat.Special.Kick.Kangaroo",

                "bunny" => "Combat.Special.Kick.Bunny",

                "hoofed" or "pony" or "cloven_hoofed" => player.IsTaur()

                    ? "Combat.Special.Kick.Taur"

                    : "Combat.Special.Kick.Hoof",

                _ => "Combat.Special.Kick.Generic"
            };

            context.WriteAction(intro.Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = player,

                Defender = context.Enemy,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.Special.Kick.Dodge".Localized());

                return;
            }

            var damage = KickCalc(context, player);

            if (damage <= 0)
            {
                context.Append("Combat.Special.Kick.Blocked".Localized());

                return;
            }

            CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Melee);

            context.Append("Combat.Special.Kick.Hit".Localized(damage));
        }

        public static float GoreCalc(CombatContext context, Player player)
        {
            var horns = Math.Min(player.Body.Get<HornsPart>().Value, 40);

            var sizeBonus = 1 + (player.Tallness - 77) / (120 - 77);

            var thickBonus = 0.5f + player.Thickness / 100f;

            var damage = (float)Math.Floor((player.Strength + horns * 2) * sizeBonus * thickBonus);

            damage += player.Level * 2;

            if (damage < 0)
                damage = 5;

            var cap = player.Level * 10 + 100;

            if (damage > cap)
                damage = cap;

            damage *= player.PhysMod();

            damage = CombatCalculator.BodyMod(player, damage);

            damage = CombatCalculator.GlobalMod(player, damage);

            return CombatCalculator.ReduceDamage(context, context.Enemy, damage);
        }

        public static float GoreChance(CombatContext context)
        {
            var player = context.Player;

            var modifier = -50f;

            var horns = player.Body.Get<HornsPart>().Value;

            if (horns >= 12)
                modifier += 15;

            if (horns >= 20)
                modifier += 15;

            var enemySpeed = Math.Max(1, context.Enemy.Speed);

            var speedPart = (int)(40 * player.Speed / enemySpeed - 40);

            modifier += Math.Clamp(speedPart, -20, 20);

            if (player.HasPerk(Datas.Vanilla.PerkIds.NaturalWeapons))
                modifier += 20;

            return Math.Clamp(context.Enemy.StandardDodgeFunc(player, modifier), 0, 100);
        }

        [Register("coc:special_gore")]
        public static void Gore(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            if (GoreChance(context) > context.Random.Next(100))
            {
                var damage = GoreCalc(context, player);

                if (context.Random.Next(4) > 0)
                    context.WriteAction("Combat.Special.Gore.Hit".Localized());
                else
                {
                    damage *= 2;

                    context.WriteAction("Combat.Special.Gore.Crit".Localized());
                }

                CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Melee);

                if (damage < 20)
                    context.Append("Combat.Special.Gore.PullLight".Localized());
                else if (damage < 40)
                    context.Append("Combat.Special.Gore.PullHeavy".Localized());
                else
                    context.Append("Combat.Special.Gore.PullBrutal".Localized());

                context.Append("Combat.Special.Gore.Damage".Localized(damage));
            }
            else
                context.WriteAction("Combat.Special.Gore.Miss".Localized());
        }

        public static float RamCalc(CombatContext context, Player player)
        {
            var damage = (float)Math.Floor(player.Strength + (player.Speed * 0.2f + player.Level * 2) * 0.7f);

            if (damage < 0)
                damage = 5;

            var cap = player.Level * 10 + 100;

            if (damage > cap)
                damage = cap;

            damage *= player.PhysMod();

            damage = CombatCalculator.BodyMod(player, damage);

            damage = CombatCalculator.GlobalMod(player, damage);

            return CombatCalculator.ReduceDamage(context, context.Enemy, damage);
        }

        [Register("coc:special_horn_stun")]
        public static void HornStun(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            if (GoreChance(context) > context.Random.Next(100))
            {
                var damage = RamCalc(context, player);

                context.WriteAction("Combat.Special.HornStun.Hit".Localized());

                if (context.Random.NextDouble() < CombatCalculator.CriticalChance(player))
                {
                    context.Append("Combat.Special.HornStun.Crit".Localized());

                    damage *= 1.75f;
                }

                CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Melee);

                context.Append("Combat.Special.HornStun.Damage".Localized(damage));

                if (context.Enemy.Stun(2, (int)GoreChance(context)))
                    context.Append("Combat.Special.HornStun.Stun".Localized());
            }
            else
                context.WriteAction("Combat.Special.HornStun.Miss".Localized());
        }
    }
}