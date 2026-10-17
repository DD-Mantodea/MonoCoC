using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 scenes/dungeons/manor/NamelessHorror.hx 的招式。
    // 文本 key 来自 Tools/PortKit/out/NamelessHorror.texts.json（落在 Monsters/NamelessHorror.hjson）。
    public static class NamelessHorrorCombatHandlers
    {
        // 原版 NamelessHorror.hx:103 overwhelm：
        //   createStatusEffect(Attacks, 4)；玩家血量 <40% 时 spe = 40；eAttack()；之后 spe = 100。
        // C# 的 EnemyBasicAttack 不读 Attacks 状态，所以按 4 次显式循环（原版 Monster.hx:940 会跑 4 次）。
        [Register("coc:nameless_horror_overwhelm")]
        private static void Overwhelm(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.NamelessHorror.Overwhelm.DozensOfTentacles".Localized());

            context.Enemy.CreateStatusEffect(VanillaStatusEffects.Attacks, 4);

            var lowHealth = context.Player.HealthRatio() < 0.4f;

            if (lowHealth)
                context.Enemy.Speed = 40;

            for (var i = 0; i < 4; i++)
                CombatController.Instance!.Resolver.EnemyBasicAttack();

            if (lowHealth)
                context.Enemy.Speed = 100;

            context.Enemy.RemoveStatusEffect(VanillaStatusEffects.Attacks);
        }

        // 原版 NamelessHorror.hx:115 refashionIt：
        //   player.takeDamage(25 + rand(50))；挂 Refashioned(2)；
        //   Utils.rand(3) 挑一项属性额外 +30，然后把四项基础属性全部重掷成 40 + rand(180)。
        // 原版把旧值存进 playerStats 但从不读回（死字段），这里同样只写不读。
        [Register("coc:nameless_horror_refashion_it")]
        private static void RefashionIt(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.NamelessHorror.RefashionIt.TheCreatureLifts".Localized());
            context.Append("Combat.NamelessHorror.RefashionIt.YouReHit".Localized());
            context.Append("Combat.NamelessHorror.RefashionIt.BeforeYouCan".Localized());

            var player = context.Player;

            player.TakeDamage(25 + context.Random.Next(50));

            // 原版 value1=2，由 Combat 每回合递减；C# 用 Duration=2 交给 CombatStatusTicker 过期。
            player.CreateStatusEffect(VanillaStatusEffects.Refashioned, 2, 0, 0, 0, 2);

            var horror = context.EnemyAs<NamelessHorrorEnemy>();

            if (horror != null)
            {
                horror.PlayerStats[0] = player.Strength;
                horror.PlayerStats[1] = player.Toughness;
                horror.PlayerStats[2] = player.Intelligence;
                horror.PlayerStats[3] = player.Speed;
            }

            var lucky = context.Random.Next(3);

            player.Strength = 40 + context.Random.Next(180) + (lucky == 0 ? 30 : 0);
            player.Toughness = 40 + context.Random.Next(180) + (lucky == 1 ? 30 : 0);
            player.Intelligence = 40 + context.Random.Next(180) + (lucky == 2 ? 30 : 0);
            // 原版这个三元用的是 lucky == 3，Utils.rand(3) 永远不给 3，等于恒 +0，保留以对齐原文。
            player.Speed = 40 + context.Random.Next(180) + (lucky == 3 ? 30 : 0);
        }

        // 原版 NamelessHorror.hx:137 reveal：
        //   originalName = player.short；player.short = "Ephraim"；挂 Revelation(2, 50)。
        // 改名走 CombatContext.PendingNameRestore（CombatContext.cs:179）：原版由 Combat.hx:1701
        // 在 Revelation 到期时还原 player.short；C# 由 CombatController.RestorePendingName
        // 在 Finish / ContinueCombat 统一还原（CombatController.cs:973-983），所以这里可以放心改名。
        [Register("coc:nameless_horror_reveal")]
        private static void Reveal(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.NamelessHorror.Reveal.TheCreatureLifts".Localized());

            var horror = context.EnemyAs<NamelessHorrorEnemy>();

            if (horror != null)
                horror.OriginalName = context.Player.ShortName;

            // ??= 保留最早的名字：重复 reveal 时不会把 "Ephraim" 记成待还原值。
            context.PendingNameRestore ??= context.Player.ShortName;
            context.Player.ShortName = "Ephraim";

            context.Player.CreateStatusEffect(VanillaStatusEffects.Revelation, 2, 50, 0, 0, 2);
        }

        // 原版 NamelessHorror.hx:132 becomeNothingness：给玩家挂 Nothingness(2)。
        // 玩家的减伤判定在 NamelessHorrorEnemy.ModifyIncomingDamage（原版 handleDamaged）。
        [Register("coc:nameless_horror_become_nothingness")]
        private static void BecomeNothingness(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.NamelessHorror.BecomeNothingness.TheCreatureShrieks".Localized());

            context.Player.CreateStatusEffect(VanillaStatusEffects.Nothingness, 2, 0, 0, 0, 2);
        }

        // 原版 NamelessHorror.hx:144 conjoin：
        //   player.takeDamage(player.HP * 0.25 * doomLevel)；doomLevel += 1。
        // 原版它不在 MonsterAI 表里（performCombatAction 每 5 回合直接调用），C# 包成一条自我招式。
        [Register("coc:nameless_horror_conjoin")]
        private static void Conjoin(CombatContext context, CombatAbilityDefinition ability)
        {
            var horror = context.EnemyAs<NamelessHorrorEnemy>();
            var doomLevel = horror?.DoomLevel ?? 1f;

            context.Append("Combat.NamelessHorror.Conjoin.TheCreatureShrieks".Localized());
            context.Append("Combat.NamelessHorror.Conjoin.TheVisionShatters".Localized());

            context.Player.TakeDamage(context.Player.Health * 0.25f * doomLevel);

            if (horror != null)
                horror.DoomLevel += 1;
        }
    }
}
