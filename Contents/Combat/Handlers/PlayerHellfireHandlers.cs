using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:3042-3105 hellFire
    public static class PlayerHellfireHandlers
    {
        [Register("coc:has_hellfire")]
        public static bool HasHellfire(CombatContext context, CombatState state) =>

            context.Player.HasPerk(PerkIds.Hellfire);

        [Register("coc:spell_hellfire")]
        public static void Hellfire(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var damage = player.Level * 8f + context.Random.Next(10) + player.Intelligence / 2f + player.Corruption / 5f;

            damage = InfernoModifiers.CalcInfernoMod(context, damage);

            if (player.HasStatusEffect(StatusEffectIds.GooArmorSilence))
                damage += 25;

            damage = CombatCalculator.GlobalMod(player, damage);

            if (enemy is LivingStatueEnemy)
            {
                context.AppendLog("Combat.Spell.Hellfire.LivingStatue".Localized());

                return;
            }

            if (enemy is AkbalUnsealedEnemy)
            {
                context.AppendLog("Combat.Spell.Hellfire.Akbal".Localized());

                context.AppendLog("Combat.Spell.Hellfire.AkbalChuckle".Localized());

                return;
            }

            if (!player.HasStatusEffect(StatusEffectIds.GooArmorSilence))
                context.AppendLog("Combat.Spell.Hellfire.Intro".Localized());

            if (player.HasStatusEffect(StatusEffectIds.WebSilence))
            {
                context.AppendLog("Combat.Spell.Hellfire.WebBurns".Localized());

                player.RemoveStatusEffect(StatusEffectIds.WebSilence);
            }

            if (player.HasStatusEffect(StatusEffectIds.GooArmorSilence))
            {
                context.AppendLog("Combat.Spell.Hellfire.GooBurns".Localized());

                player.RemoveStatusEffect(StatusEffectIds.GooArmorSilence);
            }

            if (enemy is IsabellaEnemy && !enemy.HasStatusEffect(StatusEffectIds.Stunned))
            {
                context.AppendLog("Combat.Spell.Hellfire.IsabellaShield".Localized());

                context.AppendLog("Combat.Spell.Hellfire.IsabellaTaunt".Localized());

                return;
            }

            if (enemy is ValaEnemy && !enemy.HasStatusEffect(StatusEffectIds.Stunned))
            {
                context.AppendLog("Combat.Spell.Hellfire.ValaBlow".Localized());

                if (player.GetEvasionChance() > context.Random.Next(100))
                    context.AppendLog("Combat.Spell.Hellfire.ValaDodge".Localized());
                else
                {
                    context.AppendLog("Combat.Spell.Hellfire.ValaHit".Localized());

                    player.Lust = Math.Min(player.MaxLust, player.Lust + damage / 6f);
                }

                return;
            }

            if (enemy.Intelligence < 10)
            {
                context.AppendLog("Combat.Spell.Hellfire.SimpleFoe".Localized());

                CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Magical);
            }
            else if (enemy.LustVulnerability > 0)
            {
                context.AppendLog("Combat.Spell.Hellfire.LustFoe".Localized());

                CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Magical);

                enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + enemy.LustVulnerability * damage / 6f);
            }
            else
                context.AppendLog("Combat.Spell.Hellfire.NoEffect".Localized());
        }
    }
}