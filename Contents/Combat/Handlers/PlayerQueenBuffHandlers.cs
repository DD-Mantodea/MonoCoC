using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:4227-4289 parasiteQueen / nephilaQueen
    public static class PlayerQueenBuffHandlers
    {
        private static int ParasiteCalc(CombatContext context) =>

            Math.Min((int)context.Player.StatusEffectValue(StatusEffectIds.ParasiteEel, 0) * 2, 20);

        private static int NephilaCalc(CombatContext context) =>

            Math.Min((int)context.Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) * 5, 50);

        [Register("coc:can_parasite_queen")]
        public static bool CanParasiteQueen(CombatContext context, CombatState state) =>

            context.Player.HasPerk(PerkIds.ParasiteQueen);

        [Register("coc:parasite_queen_disabled")]
        public static bool ParasiteQueenDisabled(CombatContext context, CombatState state) =>

            context.Player.StatusEffectValue(StatusEffectIds.ParasiteEel, 0) <= 5

            || context.Player.HasStatusEffect(StatusEffectIds.ParasiteQueen);

        [Register("coc:can_nephila_queen")]
        public static bool CanNephilaQueen(CombatContext context, CombatState state) =>

            context.Player.HasPerk(PerkIds.NephilaQueen);

        [Register("coc:nephila_queen_disabled")]
        public static bool NephilaQueenDisabled(CombatContext context, CombatState state) =>

            context.Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) <= 2

            || context.Player.HasStatusEffect(StatusEffectIds.NephilaQueen);

        [Register("coc:special_parasite_queen")]
        public static void ParasiteQueen(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var parasites = ParasiteCalc(context);

            player.CreateStatusEffect(StatusEffectIds.ParasiteQueen, parasites, 0, 0, 0, 99);

            CombatCalculator.AddCombatBuff(player, GameStats.Strength, parasites, "ParasiteQueen", StatusEffectIds.ParasiteQueen);

            CombatCalculator.AddCombatBuff(player, GameStats.Toughness, parasites, "ParasiteQueen", StatusEffectIds.ParasiteQueen);

            CombatCalculator.AddCombatBuff(player, GameStats.Speed, parasites, "ParasiteQueen", StatusEffectIds.ParasiteQueen);

            context.Append("Combat.Special.ParasiteQueen.Text1".Localized());

            context.Append("Combat.Special.ParasiteQueen.Text2".Localized());

            context.Append("[pg]");

            player.AddStatusValue(StatusEffectIds.ParasiteEel, 0, -1);
        }

        [Register("coc:special_nephila_queen")]
        public static void NephilaQueen(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var parasites = NephilaCalc(context);

            player.CreateStatusEffect(StatusEffectIds.NephilaQueen, parasites, 0, 0, 0, 99);

            CombatCalculator.AddCombatBuff(player, GameStats.Intelligence, parasites, "NephilaQueen", StatusEffectIds.NephilaQueen);

            context.Append("Combat.Special.ParasiteQueen.Text1".Localized());

            context.Append("[pg]");

            // 原版这里误扣 ParasiteEel；C# 修正为扣减 Nephila 寄生虫层数
            player.AddStatusValue(StatusEffectIds.ParasiteNephila, 0, -1);
        }
    }
}