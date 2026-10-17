using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 scenes/dungeons/helDungeon/PhoenixPlatoon.hx 的三个招式。
    // 文本 key 来自 Tools/PortKit/out/PhoenixPlatoon.texts.json（落在 Monsters/PhoenixPlatoon.hjson）。
    public static class PhoenixPlatoonCombatHandlers
    {
        // 原版 PhoenixPlatoon.hx:8 phoenixPlatoonRush
        //   createStatusEffect(StatusEffects.Attacks, 2 + rand(3)); eAttack();
        // 原版 eAttack 会按 Attacks 的值连续攻击 n 次（Monster.hx:940，0 视作 1），
        // 这里显式循环，因为 C# 的 EnemyBasicAttack 不读 Attacks 状态。
        [Register("coc:phoenix_platoon_rush")]
        private static void Rush(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.PhoenixPlatoon.PhoenixPlatoonRush.YouFallBack".Localized());

            var attacks = 2 + context.Random.Next(3);

            for (var i = 0; i < attacks; i++)
                CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        // 原版 PhoenixPlatoon.hx:17 phoenixPlatoonFireBreath
        //   var damage = 100 + rand(50); player.takeDamage(damage, true);
        [Register("coc:phoenix_platoon_fire_breath")]
        private static void FireBreath(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.PhoenixPlatoon.PhoenixPlatoonFireBreath.SuddenlyTheShield".Localized());

            var damage = 100 + context.Random.Next(50);

            context.Player.TakeDamage(damage);
        }

        // 原版 PhoenixPlatoon.hx:25 phoenixPlatoonLustbang
        //   player.takeLustDamage(40, true);
        [Register("coc:phoenix_platoon_lustbang")]
        private static void Lustbang(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.PhoenixPlatoon.PhoenixPlatoonLustbang.OneOfThe".Localized());

            context.Player.TakeLustDamage(40, true);
        }
    }
}
