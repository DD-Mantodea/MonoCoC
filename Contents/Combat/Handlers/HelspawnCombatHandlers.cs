using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Linq;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class HelspawnCombatHandlers
    {

        public const string CalledShotSource = "HelspawnCalledShot";

        [Register("coc:helspawn_twin_strikes")]
        public static void TwinStrikes(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            var isBow = Weapon(enemy) == "bow";

            context.Append((isBow

                    ? "Combat.Helspawn.HelspawnTwinStrikes.LeapsBackOut"

                    : "Combat.Helspawn.HelspawnTwinStrikes.LungesAtYou")
                .Localized(enemy.ShortName));

            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        [Register("coc:helspawn_called_shot")]
        public static void CalledShot(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Helspawn.CalledShot.DrawsBackHer".Localized(enemy.ShortName));

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            if (damage <= 0 || (context.Random.Next(2) == 0 && Avoid(context).AttackFailed))
            {
                context.Append("Combat.Helspawn.CalledShot.YouAvoidThe".Localized());

                return;
            }

            context.Append("Combat.Helspawn.CalledShot.OneOfHer".Localized());

            ApplyCalledShot(context);

            player.TakeDamage(damage);
        }

        [Register("coc:helspawn_berserk")]
        public static void Berserk(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            context.Append("Combat.Helspawn.HelSpawnBerserk.LetsOutA".Localized(enemy.ShortName));

            enemy.WeaponAttack += 30;

            enemy.ArmorDefense = 0;
        }

        [Register("coc:helspawn_shield_bash")]
        public static void ShieldBash(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength);

            context.Append("Combat.Helspawn.HelSpawnShieldBash.LashesOutWith".Localized(enemy.ShortName));

            if (damage <= 0 || Avoid(context).AttackFailed)
            {
                context.Append("Combat.Helspawn.HelSpawnShieldBash.YouEvadeThe".Localized());

                return;
            }

            context.Append("Combat.Helspawn.HelSpawnShieldBash.HerShieldCatches".Localized());

            player.TakeDamage(damage);

            if (player.Stun(0, 33, 100, false))
                context.Append("Combat.Helspawn.HelSpawnShieldBash.TheHitStuns".Localized());
        }

        public static void TailWhip(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var damage = enemy.Strength - context.Random.Next(Math.Max(1, (int)player.Toughness));

            context.Append("Combat.Helspawn.TailWhipShitYo.WhipsAtYou".Localized(enemy.ShortName));

            if (damage <= 0 || Avoid(context).AttackFailed)
            {
                context.Append("Combat.Helspawn.TailWhipShitYo.YouEvadeThe".Localized());

                return;
            }

            context.Append("Combat.Helspawn.TailWhipShitYo.STailCatches".Localized(enemy.ShortName));

            player.TakeDamage(damage);
        }

        [Register("coc:helspawn_slutty_mander")]
        public static void SluttyMander(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Helspawn.SluttyMander.JumpsJustOut".Localized(enemy.ShortName));

            if (context.Random.Next(2) == 0)
            {
                context.Append("Combat.Helspawn.SluttyMander.WhatTheFuck".Localized());

                return;
            }

            context.Append("Combat.Helspawn.SluttyMander.YouLeanBack".Localized());

            var lustDelta = player.LustVulnerability * (10 + player.Libido / 10f);

            player.TakeLustDamage(lustDelta);
        }

        [Register("coc:helspawn_focus")]
        public static void Focus(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            context.Append("Combat.Helspawn.HelSpawnFocus.SeeingAMomentary".Localized(enemy.ShortName));

            enemy.Lust = Math.Max(0, enemy.Lust - 50);

            enemy.ChangeFatigue(-50);

            enemy.HealthChange(enemy.MaxHealth / 2f);

            if (enemy is HelspawnEnemy helspawn)
                helspawn.UsedFocus = true;
        }

        [Register("coc:helspawn_has_bow")]
        public static bool HasBow(CombatContext context, CombatState state) =>

            Weapon(context.Enemy) == "bow";

        [Register("coc:helspawn_can_berserk")]
        public static bool CanBerserk(CombatContext context, CombatState state) =>

            context.Enemy.WeaponAttack < 50 || Weapon(context.Enemy) == "scimitar";

        [Register("coc:helspawn_has_shield")]
        public static bool HasShield(CombatContext context, CombatState state) =>

            Weapon(context.Enemy) == "scimitar and shield";

        [Register("coc:helspawn_is_slutty")]
        public static bool IsSlutty(CombatContext context, CombatState state) =>

            context.EnemyAs<HelspawnEnemy>()?.Personality >= 50;

        [Register("coc:helspawn_can_focus")]
        public static bool CanFocus(CombatContext context, CombatState state) =>

            context.EnemyAs<HelspawnEnemy>() is { } helspawn && helspawn.Personality < 50 && !helspawn.UsedFocus;

        public static string Weapon(Enemy enemy) => enemy is HelspawnEnemy helspawn ? helspawn.Weapon : "";

        public static AvoidResult Avoid(CombatContext context) =>

            CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = context.Player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

        public static void ApplyCalledShot(CombatContext context)
        {
            var player = context.Player;

            var source = CalledShotStatusSource();

            var previous = -player.Stats.From(source)
                .Where(modifier => modifier.Stat == GameStats.Speed)
                .Sum(modifier => modifier.Resolve());

            var total = previous + 20 + context.Random.Next(5);

            player.Stats.RemoveFrom(source);

            player.Stats.AddModifier(GameStats.Speed, ModifierOperation.Addition, () => -total, source);

            CombatCalculator.AddCombatBuff(player, GameStats.Speed, 0, CalledShotSource,

                new Identifier("coc", "called_shot"));
        }

        public static StatusSource CalledShotStatusSource() =>

            new(new Identifier("coc", "combat_buff_" + CalledShotSource.ToLowerInvariant().Replace(' ', '_')), CalledShotSource);
    }
}
