using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:3149-3218 superWhisperAttack
    public static class PlayerWhisperHandlers
    {
        [Register("coc:knows_whisper")]
        public static bool KnowsWhisper(CombatContext context, CombatState state) =>

            context.Player.HasPerk(PerkIds.Whispered);

        [Register("coc:enemy_feared")]
        public static bool EnemyFeared(CombatContext context, CombatState state) =>

            context.Enemy.HasStatusEffect(StatusEffectIds.Fear);

        [Register("coc:spell_whisper")]
        public static void Whisper(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (player.HasStatusEffect(StatusEffectIds.ThroatPunch)
                || player.HasStatusEffect(StatusEffectIds.WebSilence))
            {
                context.Append("Combat.Spell.Whisper.Silence".Localized());

                return;
            }

            if (enemy.Id.Path == "pod" || enemy.Intelligence == 0)
            {
                context.Append("Combat.Spell.Whisper.NoMind".Localized());

                player.ChangeFatigue(1);

                return;
            }

            if (enemy is LivingStatueEnemy || enemy is VolcanicGolemEnemy)
            {
                context.Append("Combat.Spell.Whisper.NoSoul".Localized());

                player.ChangeFatigue(1);

                return;
            }

            if (enemy.HasPerk(PerkIds.Focused))
            {
                context.Append("Combat.Spell.Whisper.Focused".Localized());

                return;
            }

            if (player.Intelligence < enemy.Intelligence || enemy.Plural)
            {
                context.Append("Combat.Spell.Whisper.TooStrong".Localized());

                player.ChangeFatigue(10);

                return;
            }

            if (context.Random.Next(10) == 0)
            {
                context.Append("Combat.Spell.Whisper.Distracted".Localized());

                player.ChangeFatigue(10);

                return;
            }

            context.Append("Combat.Spell.Whisper.Success".Localized());

            enemy.CreateStatusEffect(StatusEffectIds.Fear, 1, 0, 0, 0);
        }
    }
}