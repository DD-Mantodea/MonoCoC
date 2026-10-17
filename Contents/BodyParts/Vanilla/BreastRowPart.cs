using Hjson;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.BodyParts.Vanilla
{
    public class BreastRowPart : BodyPart
    {
        public override Identifier SlotId => BodyPartSlots.BreastRow;

        public int Breasts = 2;

        public float BreastRating;

        public float MilkFullness;

        public float NipplesPerBreast = 1;

        public float Lactation;

        public int PreventLactationIncrease;

        public bool Loli;

        public bool Fuckable;

        // 原版 BreastRow.hx:15 nippleCocks：乳头可插入（乳头屌）
        public bool NippleCocks;

        public int NipplesPierced;

        public bool fuckable { get => Fuckable; set => Fuckable = value; }

        public int breasts { get => Breasts; set => Breasts = value; }

        public float nipplesPerBreast { get => NipplesPerBreast; set => NipplesPerBreast = value; }

        public float lactationMultiplier { get => Lactation; set => Lactation = value; }

        public int nipplesPierced { get => NipplesPierced; set => NipplesPierced = value; }

        public static readonly int[] LactationBoost = [0, 0, 2, 3, 6, 9, 17];

        public bool Lactating() => Lactation >= 2f;

        public bool CanTitFuck() => BreastRating >= 3f;

        public bool MilkIsFull() => Lactation > 1f && MilkFullness >= 50f;

        public int TimesMilked;

        public bool Milked()
        {
            MilkFullness = 0;

            TimesMilked++;

            if (PreventLactationIncrease == (int)Lactation)
                return false;

            var required = (int)Lactation switch
            {
                1 => 12,

                2 => 10,

                3 => 12,

                4 => 15,

                5 => 20,

                _ => 0,
            };

            if (required == 0 || TimesMilked < required)
                return false;

            TimesMilked = 5;

            Lactation += 1;

            return true;
        }

        public bool MilkIsOverflowing()
        {
            var level = (int)Lactation;

            if (level <= 1)
                return false;

            var boost = level < LactationBoost.Length ? LactationBoost[level] : LactationBoost[^1];

            return MilkFullness >= 60f + 5f * boost;
        }

        public float MilkQuantity()
        {
            if (Lactation <= 1f)
                return 0f;

            return 0.01f * Math.Max(100f, 2f * MilkFullness) * 20f * Breasts * BreastRating * (Lactation - 1f);
        }

        public string Cup() => Appearance.BreastCup(BreastRating);

        public string Adj()
        {
            var cup = (int)BreastRating;

            return cup switch
            {
                0 => "non-existent",

                1 => "small",

                2 or 3 => "palmable",

                4 or 5 or 6 => "sizeable",

                >= 7 and <= 14 => "huge",

                >= 15 and <= 27 => "gigantic",

                >= 28 and <= 45 => "mammoth",

                _ => "titanic",
            };
        }

        public override void Restore()
        {
            base.Restore();

            Breasts = 2;

            BreastRating = 0;

            MilkFullness = 0;

            NipplesPerBreast = 1;

            Lactation = 0;

            Loli = false;

            Fuckable = false;
        }

        public override void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            json["breasts"] = Breasts;

            json["breastRating"] = BreastRating;

            json["milkFullness"] = MilkFullness;

            json["nipplesPerBreast"] = NipplesPerBreast;

            json["lactation"] = Lactation;

            json["loli"] = Loli;

            json["fuckable"] = Fuckable;

            json["nippleCocks"] = NippleCocks;

            json["nipplesPierced"] = NipplesPierced;
        }

        public override void Load(JsonObject json)
        {
            var saved = json.TryGetValue("id", out var idValue) ? (string)idValue : "";

            Id = Identifier.TryParse(saved, out var parsed) && BodyPartSlotRegistry.Get(SlotId).Types.Has(parsed) ? parsed : BodyPartSlotRegistry.Get(SlotId).DefaultType;

            Breasts = json.TryGetValue("breasts", out var breasts) ? (int)(double)breasts : 2;

            BreastRating = json.TryGetValue("breastRating", out var rating) ? (float)rating : 0;

            MilkFullness = json.TryGetValue("milkFullness", out var fullness) ? (float)fullness : 0;

            NipplesPerBreast = json.TryGetValue("nipplesPerBreast", out var nipples) ? (float)nipples : 1;

            Lactation = json.TryGetValue("lactation", out var lactation) ? (float)lactation : 0;

            Loli = json.TryGetValue("loli", out var loli) && (bool)loli;

            Fuckable = json.TryGetValue("fuckable", out var fuckable) && (bool)fuckable;

            NippleCocks = json.TryGetValue("nippleCocks", out var nippleCocks) && (bool)nippleCocks;

            NipplesPierced = json.TryGetValue("nipplesPierced", out var nipplesPierced) ? (int)(double)nipplesPierced : 0;
        }

        public override string Descript(bool noAdj = false, bool noTone = false) => Appearance.BreastDescript(BreastRating, Lactation);

    }
}