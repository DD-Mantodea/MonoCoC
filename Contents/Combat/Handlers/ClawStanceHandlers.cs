using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.StatusEffects;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Combat.Handlers
{
    /// <summary>
    /// 原版 CombatAbilities.hx:2273-2295 / ClawStanceBuff.hx / LurkBuff.hx —— 爪姿态与潜伏。
    /// 徒手爪 + 自然武器、且没拿盾时可用；装备变化会在回合结束时自动解除。
    /// </summary>
    public static class ClawStanceHandlers
    {
        private static readonly Identifier ClawMastery = new("coc", "Claw");

        // 原版 availableWhen：NaturalWeapons && canClawFist()（爪精通至少 1 级）
        [Register("coc:claw_stance_ready")]
        public static bool ClawStanceReady(CombatContext context, CombatState state) =>

            context.Player.HasPerk(PerkIds.NaturalWeapons)

            && context.Player.Weapon.IsClaw()

            && context.Player.Weapon.IsUnarmed()

            && context.Player.Shield.Id.Path == "nothing"

            && context.Player.MasteryLevel(ClawMastery) >= 1;

        // 原版 :6941 clawStance()
        [Register("coc:claw_stance")]
        public static void ClawStance(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.WriteAction("Combat.Ability.ClawStance.Text".Localized());

            // 原版 stanceChange(Claw)：先卸掉其它姿态；C# 目前只有爪姿态
            player.RemoveStatusEffect(StatusEffectIds.ClawStance);

            player.CreateStatusEffect(StatusEffectIds.ClawStance);

            // 原版 canFist(Activation)：爪精通 4 级时额外获得潜伏
            if (player.MasteryLevel(ClawMastery) >= 4)
            {
                context.Write("Combat.Ability.ClawStance.Activation".Localized());

                player.CreateStatusEffect(StatusEffectIds.Lurk);
            }
        }

        // 原版 ClawStanceBuff.onCombatRound()：武器/盾变化时解除
        [Register("coc:effect_claw_stance_round")]
        public static void ClawStanceRound(CombatContext context, Creature host, StatusEffect status)
        {
            var player = (Player)host;

            if (player.Weapon.IsBareClaw() && player.Shield.Id.Path == "nothing")
                return;

            context.WriteAction("Combat.Effect.ClawStance.Broken".Localized());

            player.RemoveStatusEffect(StatusEffectIds.ClawStance);
        }
    }
}