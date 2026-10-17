using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;
using System.Linq;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 scenes/dungeons/manor/BoneGeneral.hx 的招式。
    // 文本 key 来自 Tools/PortKit/out/BoneGeneral.texts.json（落在 Monsters/BoneGeneral.hjson）。
    public static class BoneGeneralCombatHandlers
    {
        // 原版 eAttack：普通攻击（Monster.hx:940）。
        [Register("coc:bone_general_attack")]
        private static void Attack(CombatContext context, CombatAbilityDefinition ability) =>
            CombatController.Instance!.Resolver.EnemyBasicAttack();

        // 原版 BoneGeneral.hx:57 crushingBlow：
        //   player.stun(rand(2), 25) 决定两段文案之一；damage = player.reduceDamage(40 + rand(20), this)。
        [Register("coc:bone_general_crushing_blow")]
        private static void CrushingBlow(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.BoneGeneral.CrushingBlow.TheBoneGuardian".Localized());

            context.Append(context.Player.Stun(context.Random.Next(2), 25)
                ? "Combat.BoneGeneral.CrushingBlow.YouLoseYour".Localized()
                : "Combat.BoneGeneral.CrushingBlow.YouManageTo".Localized());

            var damage = CombatCalculator.ReduceDamage(context, context.Player, 40 + context.Random.Next(20));

            context.Player.TakeDamage(damage);
        }

        // 原版 BoneGeneral.hx:68 guardAlly：
        //   优先保护活着的 Necromancer，否则随机挑一个活着的非 BoneGeneral 同伙，给它挂 GuardAB(2)。
        // GuardAB 在共享层已生效：CombatContext.CanTarget 按原版 Combat.hx:606 排除被护卫者；
        // 倒计时（Monster.hx:1825）由被护卫敌人各自的 TickStatuses 调
        // BoneGeneralEnemy.TickGuardAb 实现（Enemy.cs:322 / CombatController.cs:787,813）。
        [Register("coc:bone_general_guard_ally")]
        private static void GuardAlly(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.BoneGeneral.GuardAlly.TheBoneGuardian".Localized());

            var necromancer = context.Enemies.FirstOrDefault(e => e is NecromancerEnemy && e.Health > 0);

            if (necromancer != null)
            {
                context.Append("Combat.BoneGeneral.GuardAlly.TheNecromancerIs".Localized());

                necromancer.CreateStatusEffect(VanillaStatusEffects.GuardAB, 2, 0, 0, 0);

                return;
            }

            var candidates = context.Enemies
                .Where(e => e is not BoneGeneralEnemy && e.Health > 0)
                .ToList();

            if (candidates.Count == 0)
                return;

            var target = candidates[context.Random.Next(candidates.Count)];

            context.Append("Combat.BoneGeneral.GuardAlly.01Is".Localized(
                MonoCoC.Utils.Utils.CapitalizeFirstLetter(target.Article),
                target.ShortName,
                target.Pronoun2));

            target.CreateStatusEffect(VanillaStatusEffects.GuardAB, 2, 0, 0, 0);
        }
    }
}
