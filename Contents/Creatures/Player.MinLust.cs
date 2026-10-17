using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using System;

namespace MonoCoC.Contents.Creatures
{
    public partial class Player
    {
        /// <summary>
        /// 原版 Player.hx:3383 minLust() —— 玩家淫欲的下限。
        /// 由 perk / 首饰 / 状态 / 蛋数 / 营地 NPC 进度叠加，再经软上限与 Cold Blooded 调整。
        /// 结果通过覆盖 MinValue(GameStats.Lust) 在所有 ApplyStats 里生效。
        /// </summary>
        public override float MinLust()
        {
            var min = 0f;

            var minCap = MaxLust;

            var minMin = 0f;

            var count = 0;

            // 原版 getBonusStat(BonusStat.minLust)：perk 部分在这里逐个展开
            if (HasPerk(PerkIds.PentUp))
            {
                min += PerkValue(PerkIds.PentUp, 0);

                count += 1;
            }

            if (HasPerk(PerkIds.PiercedCrimstone))
            {
                min += PerkValue(PerkIds.PiercedCrimstone, 0);

                count += 1;
            }

            if (HasPerk(PerkIds.PiercedIcestone))
            {
                min -= PerkValue(PerkIds.PiercedIcestone, 0);

                count += 1;
            }

            // 原版 JewelryLib.hx:131-136：冰石戒指降下限，绯红石戒指抬下限
            var jewelryBonus = (Jewelry.Definition?.Id.Path ?? "") switch
            {
                "lesser_icestone_ring" => -10f,

                "icestone_ring" => -15f,

                "greater_icestone_ring" => -20f,

                "lesser_crimstone_ring" => 10f,

                "crimstone_ring" => 15f,

                "greater_crimstone_ring" => 20f,

                _ => 0f,
            };

            if (jewelryBonus != 0)
            {
                min += jewelryBonus;

                count += 1;
            }

            // 原版 NephilaQueensGown.hx:16 boostsMinLust(getLustBonus)
            var gownBonus = ArmorItemBonuses.MinLust(this);

            if (gownBonus != 0)
            {
                min += gownBonus;

                count += 1;
            }

            if (Eggs() >= 20)
            {
                min += 10;

                count += 1;
            }

            if (Eggs() >= 40)
            {
                min += 10;

                count += 1;
            }

            if (HasStatusEffect(StatusEffectIds.AnemoneArousal))
            {
                min += 30;

                count += 1;
            }

            if (HasStatusEffect(StatusEffectIds.ParasiteSlug))
            {
                min += 10;

                count += 1;
            }

            if (HasStatusEffect(StatusEffectIds.ParasiteEelNeedCum))
            {
                min += 5 * StatusEffectValue(StatusEffectIds.ParasiteEelNeedCum, 2);

                count += 1;
            }

            if (HasStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum))
            {
                min += 5 * StatusEffectValue(StatusEffectIds.ParasiteNephilaNeedCum, 2);

                count += 1;
            }

            if (HasStatusEffect(StatusEffectIds.BimboChampagne)
                || HasPerk(PerkIds.BimboBody)

                || HasPerk(PerkIds.BroBody)

                || HasPerk(PerkIds.FutaForm))
            {
                min += 40;

                count += 1;
            }

            var shouldraTimer = CoC.Instance.Flags.Get(GameFlagIds.ShouldraSleepTimer);

            if (shouldraTimer <= -168 && CoC.Instance.Flags.Get(GameFlagIds.UrtaQuestStatus) != 0.75f)
            {
                min += 20;

                count += 1;

                if (shouldraTimer <= -216)
                {
                    min += 30;

                    count += 1;
                }
            }

            // 原版「下限的下限」：只取最大值，不叠加
            if (Armor.Definition?.Id.Path == "lusty_maidens_armor")
                minMin = Math.Max(minMin, 30);

            if (Armor.Definition?.Id.Path == "tentacle_bark")
                minMin = Math.Max(minMin, 20);

            if (HasStatusEffect(StatusEffectIds.Luststick))
                minMin = Math.Max(minMin, 50);

            if (HasStatusEffect(StatusEffectIds.Infested))
                minMin = Math.Max(minMin, 50);

            if (HasStatusEffect(StatusEffectIds.ParasiteSlugReproduction))
                minMin = Math.Max(minMin, 80);

            if (HasPerk(PerkIds.ParasiteMusk))
                minMin = Math.Max(minMin, 50);

            // 超过一个修正来源时才套软上限
            if (count > 1)
                min = MinLustSoftCap(min);

            // 冷血 perk：单独在软上限之后结算
            if (HasPerk(PerkIds.ColdBlooded))
            {
                if (min > 20)
                    min -= Math.Min(20, min - 20);

                minCap -= 20;
            }

            return Math.Clamp(MathF.Round(min), minMin, minCap);
        }

        // 原版 Player.hx:3468 minLustSoftCap()
        private static float MinLustSoftCap(float raw)
        {
            var result = 0f;

            while (raw > 20)
            {
                raw -= 20;

                result += 20;

                raw *= 0.9f;
            }

            return result + raw;
        }

        // 让所有走 ApplyStats 的淫欲变化都受下限约束（原版 Creature.hx:767 boundFloat(minLust(), …)）
        public override float MinValue(Stat stat) =>

            stat == GameStats.Lust ? MinLust() : base.MinValue(stat);
    }
}