using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Items
{
    /// <summary>
    /// 原版护甲的动态加成（BonusStats.boostsTeaseChance / boostsTeaseDamage / boostsDodge / boostsSpellMod / boostsMinLust）。
    /// 只收录数值会随玩家状态变化的几件；固定数值的护甲加成在下面也一并给出。
    /// </summary>
    public static class ArmorItemBonuses
    {
        // 原版 AdventurersLewdChain.hx:8 / CheerleaderOutfit.hx:7 /
        // LustyMaidensArmor.hx:16 / MothSilkDress.hx:8 / NaughtyNunsHabit.hx:17
        public static (float Chance, float Damage) Tease(Player player)
        {
            switch (player.Armor.Id.Path)
            {
                case "kokiri_lewd_chain":
                {
                    var bonus = player.HasCock()

                        && player.LowerGarment.Id.Path is "ebon_jock" or "runed_jock"

                        ? 8f

                        : 6f;

                    return (bonus, bonus);
                }

                case "cheer_outfit":
                {
                    var bonus = player.IsNakedLower() ? 6f : 4f;

                    return (bonus, bonus);
                }

                case "lusty_maidens_armor":
                {
                    var seduction = (HasVirginVagina(player) ? 10f : 6f)

                        + CoC.Instance.Flags.Get(GameFlagIds.BikiniArmorBonus);

                    return (seduction, seduction);
                }

                case "maid_silk_dress":
                    return (7f, 7f);

                case "nuns_habit":
                    return (4f, 4f);

                default:
                    return (0f, 0f);
            }
        }

        // 原版 MothSilkDress.hx:14 getDodgeBonus()：战斗中随敌人淫欲提高闪避
        public static float Dodge(Player player)
        {
            if (player.Armor.Id.Path != "maid_silk_dress")
                return 0f;

            if (!CombatController.InCombat || CombatController.Instance?.Context.Enemy is not { } enemy)
                return 0f;

            return enemy.Lust / 5f;
        }

        // 原版 NephilaQueensGown.hx:16 getSpellBonus()：百分比点
        public static float Spell(Player player)
        {
            if (player.Armor.Id.Path != "nephila_gown")
                return 0f;

            var bonus = 10f;

            if (player.HasStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum))
                bonus += player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) * 3f;

            return bonus;
        }

        // 原版 NephilaQueensGown.hx:16 getLustBonus()：最低淫欲的减量（负值）
        public static float MinLust(Player player)
        {
            if (player.Armor.Id.Path != "nephila_gown")
                return 0f;

            var bonus = -10f;

            if (player.HasStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum))
                bonus -= player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0);

            return bonus;
        }

        // 原版 Player.hasVirginVagina()
        private static bool HasVirginVagina(Player player) =>

            player.HasVagina() && player.Body.Get<VaginaPart>().Virgin;
    }
}