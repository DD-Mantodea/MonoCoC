using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:2575-2661 dispellingSpell
    public static class PlayerArianDispelHandlers
    {
        [Register("coc:arian_dispel_available")]
        public static bool DispelAvailable(CombatContext context, CombatState state) =>

            context.Player.KeyItemV1("Arian's Charged Talisman") == 1;

        private static void Remove(CombatContext context, Creature host, Identifier status, string key)
        {
            if (!host.HasStatusEffect(status))
                return;

            context.Append(key.Localized());

            host.RemoveStatusEffect(status);
        }

        [Register("coc:arian_dispel")]
        public static void Dispel(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Arian.Talisman.Dispel.Intro".Localized());

            Remove(context, player, StatusEffectIds.ChargeWeapon, "Combat.Arian.Talisman.Dispel.ChargeWeapon");

            Remove(context, player, StatusEffectIds.Leeching, "Combat.Arian.Talisman.Dispel.Leeching");

            Remove(context, player, StatusEffectIds.TFInflame, "Combat.Arian.Talisman.Dispel.TFInflame");

            Remove(context, player, StatusEffectIds.TFPlate, "Combat.Arian.Talisman.Dispel.TFPlate");

            Remove(context, player, StatusEffectIds.TFMoltenPlate, "Combat.Arian.Talisman.Dispel.TFMoltenPlate");

            Remove(context, player, StatusEffectIds.ParasiteQueen, "Combat.Arian.Talisman.Dispel.ParasiteQueen");

            Remove(context, player, StatusEffectIds.NephilaQueen, "Combat.Arian.Talisman.Dispel.NephilaQueen");

            Remove(context, player, StatusEffectIds.TrueWhispered, "Combat.Arian.Talisman.Dispel.TrueWhispered");

            Remove(context, player, StatusEffectIds.Marked, "Combat.Arian.Talisman.Dispel.Marked");

            Remove(context, player, StatusEffectIds.Nothingness, "Combat.Arian.Talisman.Dispel.Nothingness");

            if (enemy.HasStatusEffect(StatusEffectIds.ChargeWeapon))
            {
                context.Append("Combat.Arian.Talisman.Dispel.EnemyChargeWeapon".Localized());

                enemy.RemoveStatusEffect(StatusEffectIds.ChargeWeapon);
            }

            Remove(context, enemy, StatusEffectIds.VolcanicUberHeal, "Combat.Arian.Talisman.Dispel.GolemShield");

            if (enemy.HasStatusEffect(StatusEffectIds.Fear))
            {
                context.Append("Combat.Arian.Talisman.Dispel.Fear".Localized());

                enemy.Speed += enemy.StatusEffectValue(StatusEffectIds.Fear, 0);

                enemy.RemoveStatusEffect(StatusEffectIds.Fear);
            }

            if (enemy.HasStatusEffect(StatusEffectIds.Illusion))
            {
                context.Append("Combat.Arian.Talisman.Dispel.Illusion".Localized());

                enemy.Speed += enemy.StatusEffectValue(StatusEffectIds.Illusion, 0);

                enemy.RemoveStatusEffect(StatusEffectIds.Illusion);
            }

            Remove(context, player, StatusEffectIds.Might, "Combat.Arian.Talisman.Dispel.Might");

            Remove(context, enemy, StatusEffectIds.Shell, "Combat.Arian.Talisman.Dispel.Shell");

            context.Append("\n");

            Scenes.SceneInstance<Scenes.Vanilla.Npcs.ArianScene>.Get().ClearTalisman();
        }
    }
}