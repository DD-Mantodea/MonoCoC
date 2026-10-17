using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Linq;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 classes.scenes.npcs.Helspawn 的招式（Helspawn.hx:29 / :42 / :62 / :69 / :90 / :108 / :123）。
    // 原版文案里的 [helspawn] 标签依赖原版 scenes/npcs/HelSpawnScene.hx 提供替换值（C# 无该场景），
    // 所以本地化里统一改成 {0}，由这里传 context.Enemy.ShortName。
    public static class HelspawnCombatHandlers
    {
        // 原版 CalledShotDebuff：每次 -20-rand(5) 速度，可叠加，战斗结束还原。
        private const string CalledShotSource = "HelspawnCalledShot";

        // 原版 helspawnTwinStrikes()（Helspawn.hx:29）：
        // createStatusEffect(StatusEffects.Attacks, 0) 后 eAttack()。
        // 原版 eAttack 里 attacks == 0 会按 1 次处理（Monster.hx:941），所以这里就是一次普攻。
        [Register("coc:helspawn_twin_strikes")]
        private static void TwinStrikes(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;
            var isBow = Weapon(enemy) == "bow";

            context.Append((isBow
                    ? "Combat.Helspawn.HelspawnTwinStrikes.LeapsBackOut"
                    : "Combat.Helspawn.HelspawnTwinStrikes.LungesAtYou")
                .Localized(enemy.ShortName));

            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        // 原版 calledShot()（Helspawn.hx:42）：命中后挂 CalledShot 降速，
        // 判定写成「伤害 <= 0 或（50% 概率且闪避成功）」，所以有一半情况直接判定命中。
        [Register("coc:helspawn_called_shot")]
        private static void CalledShot(CombatContext context, CombatAbilityDefinition ability)
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

        // 原版 helSpawnBerserk()（Helspawn.hx:62）：weaponAttack += 30、armorDef = 0
        [Register("coc:helspawn_berserk")]
        private static void Berserk(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            context.Append("Combat.Helspawn.HelSpawnBerserk.LetsOutA".Localized(enemy.ShortName));

            enemy.WeaponAttack += 30;
            enemy.ArmorDefense = 0;
        }

        // 原版 helSpawnShieldBash()（Helspawn.hx:69）：str 伤害 + 33% 眩晕
        [Register("coc:helspawn_shield_bash")]
        private static void ShieldBash(CombatContext context, CombatAbilityDefinition ability)
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

        // 原版 tailWhipShitYo()（Helspawn.hx:90）：护甲穿透的火焰尾鞭；
        // performCombatAction 末尾有 25% 概率额外追加一次（见 HelspawnEnemy.BeforeCombatAction）。
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

        // 原版 sluttyMander()（Helspawn.hx:108）：50% 被玩家踢回去，否则 lustVuln * (10 + lib / 10)
        [Register("coc:helspawn_slutty_mander")]
        private static void SluttyMander(CombatContext context, CombatAbilityDefinition ability)
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

        // 原版 helSpawnFocus()（Helspawn.hx:123）：欲望 -50、疲劳 -50、回半血，一次性
        [Register("coc:helspawn_focus")]
        private static void Focus(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            context.Append("Combat.Helspawn.HelSpawnFocus.SeeingAMomentary".Localized(enemy.ShortName));

            enemy.Lust = Math.Max(0, enemy.Lust - 50);

            enemy.ChangeFatigue(-50);

            enemy.HealthChange(enemy.MaxHealth / 2f);

            if (enemy is HelspawnEnemy helspawn)
                helspawn.UsedFocus = true;
        }

        // ── AI 条件（原版 MonsterAI.add 的 when）──

        [Register("coc:helspawn_has_bow")]
        private static bool HasBow(CombatContext context, CombatState state) =>
            Weapon(context.Enemy) == "bow";

        [Register("coc:helspawn_can_berserk")]
        private static bool CanBerserk(CombatContext context, CombatState state) =>
            context.Enemy.WeaponAttack < 50 || Weapon(context.Enemy) == "scimitar";

        [Register("coc:helspawn_has_shield")]
        private static bool HasShield(CombatContext context, CombatState state) =>
            Weapon(context.Enemy) == "scimitar and shield";

        [Register("coc:helspawn_is_slutty")]
        private static bool IsSlutty(CombatContext context, CombatState state) =>
            context.EnemyAs<HelspawnEnemy>()?.Personality >= 50;

        [Register("coc:helspawn_can_focus")]
        private static bool CanFocus(CombatContext context, CombatState state) =>
            context.EnemyAs<HelspawnEnemy>() is { } helspawn && helspawn.Personality < 50 && !helspawn.UsedFocus;

        private static string Weapon(Enemy enemy) => enemy is HelspawnEnemy helspawn ? helspawn.Weapon : "";

        private static AvoidResult Avoid(CombatContext context) =>
            CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,
                Defender = context.Player,
                DoDodge = true,
                DoParry = false,
                DoBlock = false
            });

        // 原版 CalledShotDebuff.apply(firstTime)：buffHost(Spe(-20 - rand(5)))；
        // C# 没有该状态定义，用「来源 + 累计值」修饰层实现，并登记脱战清理。
        private static void ApplyCalledShot(CombatContext context)
        {
            var player = context.Player;
            var source = CalledShotStatusSource();

            var previous = -player.Stats.From(source)
                .Where(modifier => modifier.Stat == VanillaStats.Speed)
                .Sum(modifier => modifier.Resolve());

            var total = previous + 20 + context.Random.Next(5);

            player.Stats.RemoveFrom(source);

            player.Stats.AddModifier(VanillaStats.Speed, ModifierOperation.Addition, () => -total, source);

            CombatCalculator.AddCombatBuff(player, VanillaStats.Speed, 0, CalledShotSource,
                new Identifier("coc", "called_shot"));
        }

        private static StatusSource CalledShotStatusSource() =>
            new(new Identifier("coc", "combat_buff_" + CalledShotSource.ToLowerInvariant().Replace(' ', '_')), CalledShotSource);
    }
}
