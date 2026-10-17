using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Configuration;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:628-695 cleansingPalmCalc / spellCleansingPalm
    public static class PlayerCleansingPalmHandlers
    {
        [Register("coc:knows_cleansing_palm")]
        public static bool KnowsCleansingPalm(CombatContext context, CombatState state) =>

            context.Player.HasPerk(PerkIds.CleansingPalm);

        [Register("coc:too_corrupt_for_cleansing_palm")]
        public static bool TooCorrupt(CombatContext context, CombatState state) =>

            !context.Player.IsPureEnough(10);

        [Register("coc:spell_cleansing_palm")]
        public static void CleansingPalm(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.Id.Path == "jojo" && CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) < 2)
            {
                context.Append("Combat.Spell.CleansingPalm.JojoCancel".Localized());

                player.MasteryXP(new Identifier("coc", "Casting"), 2 + context.Random.Next(7));

                return;
            }

            if (enemy is LivingStatueEnemy)
            {
                context.Append("Combat.Spell.CleansingPalm.LivingStatue".Localized());

                return;
            }

            var damage = 0;

            if (enemy.Corruption > 20)
            {
                var baseDamage = player.Intelligence / 4f;

                var range = player.Intelligence / 3f;

                if (player.Weapon.IsUnarmed())
                    range *= 1.5f;

                var multi = (enemy.Corruption - 20) / 25f;

                if (multi > 1.5f)
                {
                    multi = 1.5f;

                    multi += (enemy.Corruption - 57.5f) / 100f;
                }

                multi *= player.SpellMod();

                var min = (int)MathF.Round(CombatCalculator.GlobalMod(player, baseDamage * multi));

                var max = (int)MathF.Round(CombatCalculator.GlobalMod(player, (baseDamage + range) * multi));

                damage = min + context.Random.Next(Math.Max(1, max - min + 1));
            }

            if (damage > 0)
            {
                var pronoun = enemy.Plural

                    ? "them"

                    : enemy.Gender switch
                    {
                        Gender.Male => "him",

                        Gender.Female => "her",

                        _ => "it",
                    };

                context.Append("Combat.Spell.CleansingPalm.Hit".Localized(pronoun));

                if (OptionStore.Instance.Get("silly", 0) > 0 && enemy.Corruption >= 85)
                    context.Append("Combat.Spell.CleansingPalm.SuperEffective".Localized());

                CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Magical);
            }
            else
                context.Append("Combat.Spell.CleansingPalm.Ignored".Localized());

        }
    }
}