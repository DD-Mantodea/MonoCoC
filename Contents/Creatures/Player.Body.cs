using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Creatures
{
    public partial class Player
    {
        public float LactationQ()
        {
            if (BiggestLactation() < 1)
                return 0;

            if (!HasStatusEffect(StatusEffectIds.LactationEndurance))
                CreateStatusEffect(StatusEffectIds.LactationEndurance, 1);

            var endurance = StatusEffectValue(StatusEffectIds.LactationEndurance, 0);

            var total = 0f;

            foreach (var row in BreastRows)
                total += 10 * Math.Max(1, row.BreastRating) * row.Lactation * row.Breasts * endurance;

            if (HasPerk(PerkIds.MilkMaid))
                total += 200 + PerkValue(PerkIds.MilkMaid, 1) * 100;

            if (StatusEffectValue(StatusEffectIds.LactationReduction, 0) >= 48)
                total *= 1.5f;

            return Math.Min(total, int.MaxValue);
        }

        public bool IsLactating() => LactationQ() > 0;

        public float IncreaseCock(int cockNum, float lengthDelta)
        {
            if (cockNum < 0 || cockNum >= Cocks.Count)
                return 0;

            return Cocks[cockNum].GrowCock(lengthDelta, HasPerk(PerkIds.BigCock));
        }

        public float IncreaseEachCock(float lengthDelta)
        {
            var totalGrowth = 0f;

            for (var index = 0; index < Cocks.Count; index++)
                totalGrowth += IncreaseCock(index, lengthDelta);

            return totalGrowth;
        }

        public float VaginalCapacityBonus()
        {
            if (Vaginas.Count == 0)
                return 0;

            var bonus = 0f;

            if (IsTaur())
                bonus += 50;
            else if (IsPart<LowerBodyPart>("coc:naga", "mermaid"))
                bonus += 20;

            if (HasPerk(PerkIds.WetPussy))
                bonus += 20;

            if (HasPerk(PerkIds.HistorySlut))
                bonus += 20;

            if (HasPerk(PerkIds.OneTrackMind))
                bonus += 10;

            if (HasPerk(PerkIds.Cornucopia))
                bonus += 30;

            if (HasPerk(PerkIds.FerasBoonWideOpen))
                bonus += 25;

            if (HasPerk(PerkIds.FerasBoonMilkingTwat))
                bonus += 40;

            if (HasStatusEffect(StatusEffectIds.ParasiteEel))
                bonus = 10 * StatusEffectValue(StatusEffectIds.ParasiteEel, 0);

            bonus += StatusEffectValue(StatusEffectIds.BonusVCapacity, 0);

            return bonus;
        }

        public override float VaginalCapacity() =>

            Vaginas.Count == 0 ? 0 : Vaginas[0].Capacity(VaginalCapacityBonus());

        public override float AnalCapacity()
        {
            var bonus = 0f;

            if (IsTaur())
                bonus = 30;

            if (HasPerk(PerkIds.HistorySlut))
                bonus += 20;

            if (HasPerk(PerkIds.Cornucopia))
                bonus += 30;

            if (HasPerk(PerkIds.OneTrackMind))
                bonus += 10;

            if (Anal.Wetness > 0)
                bonus += 15;

            return (bonus + StatusEffectValue(StatusEffectIds.BonusACapacity, 0) + 6 * Anal.Looseness * Anal.Looseness) * (1 + Anal.Wetness / 10f);
        }

        public static void Write(string key, params object[] args)
        {
            if (Scene.View == null)
                return;

            Scene.OutputText(args.Length == 0 ? key.Localized() : key.Localized(args));
        }

        public void ShrinkTits(bool ignoreHyperHappy = false)
        {
            if (GameSettings.Hyper && !ignoreHyperHappy)
                return;

            if (BreastRows.Count == 1)
            {
                var row = BreastRows[0];

                if (row.BreastRating < 1)
                    return;

                var shrunk = 1;

                row.BreastRating -= 1;

                if (row.BreastRating >= 1 && Random.Shared.Next(2) == 0 && !HasPerk(PerkIds.BigTits))
                {
                    shrunk += 1;

                    row.BreastRating -= 1;
                }

                if (row.BreastRating < 0)
                    row.BreastRating = 0;

                var plural = row.BreastRating >= 1 ? "s" : "";

                Write((shrunk == 1 ? "Body.ShrinkTits.Single1" : "Body.ShrinkTits.Single2").Localized(BreastCup(0), plural));

                return;
            }

            if (BreastRows.Count <= 1)
                return;

            if (Scene.View != null)
                Scene.OutputText("\n");

            var changeCount = 0;

            for (var index = BreastRows.Count - 1; index >= 0; index--)
            {
                var row = BreastRows[index];

                if (row.BreastRating >= 1)
                {
                    row.BreastRating -= 1;

                    changeCount += 1;

                    var plural = row.BreastRating >= 1 ? "s" : "";

                    Write(
                        index < BreastRows.Count - 1 ? "Body.ShrinkTits.RowNext" : "Body.ShrinkTits.RowFirst",

                        BreastDescript(index),

                        BreastCup(index),

                        plural);
                }

                row.BreastRating = Math.Max(row.BreastRating, 0);
            }

            switch (changeCount)
            {
                case 2:
                    Write("Body.ShrinkTits.Count2".Localized());

                    break;

                case 3:
                    Write("Body.ShrinkTits.Count3".Localized());

                    break;

                case 0:
                case 1:
                    break;

                default:
                    Write("Body.ShrinkTits.CountMany".Localized());

                    break;
            }
        }

        public void GrowTits(float amount, float rowsGrown, bool display, float growthType)
        {
            if (BreastRows.Count == 0)
                return;

            if (HasPerk(PerkIds.BigTits) && Random.Shared.Next(3) == 0 && amount < 1)
                amount = 1;

            if (growthType == 1)
                while (rowsGrown > 0)
                {
                    var index = 0;

                    for (var i = 0; i < BreastRows.Count; i++)
                        if (BreastRows[i].BreastRating < BreastRows[index].BreastRating)
                            index = i;

                    var growth = amount;

                    if (!GameSettings.Hyper)
                    {
                        var rating = BreastRows[index].BreastRating;

                        var big = HasPerk(PerkIds.BigTits);

                        if (rating > 3)
                            growth /= big ? 1.3f : 1.5f;

                        if (rating > 7)
                            growth /= big ? 1.5f : 2f;

                        if (rating > 9)
                            growth /= big ? 1.5f : 2f;

                        if (rating > 12)
                            growth /= big ? 1.5f : 2f;
                    }

                    BreastRows[index].BreastRating += growth;

                    rowsGrown--;
                }

            if (!GameSettings.Hyper)
            {
                var rating = BreastRows[0].BreastRating;

                var big = HasPerk(PerkIds.BigTits);

                if (rating > 3)
                    amount /= big ? 1.3f : 1.5f;

                if (rating > 7)
                    amount /= big ? 1.5f : 2f;

                if (rating > 12)
                    amount /= big ? 1.5f : 2f;
            }

            if (growthType == 2)
            {
                var index = 0;

                while (rowsGrown > 0)
                {
                    if (index + 1 > BreastRows.Count)
                        index = 0;

                    BreastRows[index].BreastRating += amount;

                    index += 1;

                    rowsGrown--;
                }
            }

            if (growthType == 3)
                while (rowsGrown > 0)
                {
                    rowsGrown--;

                    BreastRows[0].BreastRating += amount;
                }

            if (display)
            {
                var multiple = BreastRows.Count > 1;

                var description = BreastDescript(0);

                if (growthType < 3)
                {
                    if (amount <= 2)
                        Write((multiple ? "Body.GrowTits.RowsSmall" : "Body.GrowTits.SingleSmall").Localized(description));
                    else if (amount <= 4)
                        Write((multiple ? "Body.GrowTits.RowsMedium" : "Body.GrowTits.SingleMedium").Localized(description));
                    else
                        Write((multiple ? "Body.GrowTits.RowsLarge" : "Body.GrowTits.SingleLarge").Localized(description));
                }
                else
                {
                    if (amount <= 2)
                        Write((multiple ? "Body.GrowTits.TopRowsSmall" : "Body.GrowTits.TopSingleSmall").Localized(description));

                    if (amount > 2 && amount <= 4)
                        Write((multiple ? "Body.GrowTits.TopRowsMedium" : "Body.GrowTits.SingleMedium").Localized(description));

                    if (amount > 4)
                        Write((multiple ? "Body.GrowTits.TopRowsLarge" : "Body.GrowTits.SingleLarge").Localized(description));
                }
            }

            if (BiggestBreastSize() >= 8.5f && NippleLength < 2)
            {
                if (display)
                    Write("Body.GrowTits.Nipple".Localized());

                NippleLength = 2;
            }

            if (BiggestBreastSize() >= 7 && NippleLength < 1)
            {
                if (display)
                    Write("Body.GrowTits.Nipple".Localized());

                NippleLength = 1;
            }

            if (BiggestBreastSize() >= 5 && NippleLength < 0.75f)
            {
                if (display)
                    Write("Body.GrowTits.Nipple".Localized());

                NippleLength = 0.75f;
            }

            if (BiggestBreastSize() >= 3 && NippleLength < 0.5f)
            {
                if (display)
                    Write("Body.GrowTits.Nipple".Localized());

                NippleLength = 0.5f;
            }
        }
    }
}