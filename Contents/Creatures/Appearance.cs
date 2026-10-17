using MonoCoC.Contents.BodyParts;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Creatures
{
    public static class Appearance
    {
        public static string Pick(params string[] keys) => keys[Random.Shared.Next(keys.Length)].Localized();

        public static string BreastSizeDescript(float value)
        {
            if (value >= 20f)
                return Pick("Appearance.BreastSize.LudicrouslySized", "Appearance.BreastSize.HideouslyLarge", "Appearance.BreastSize.AbsurdlyLarge", "Appearance.BreastSize.BackBreaking", "Appearance.BreastSize.Colossal", "Appearance.BreastSize.Immense");

            if (value >= 15f)
                return Pick("Appearance.BreastSize.Mountainous", "Appearance.BreastSize.Monumental", "Appearance.BreastSize.BackBreaking", "Appearance.BreastSize.ExerciseBallSized", "Appearance.BreastSize.Immense");

            if (value >= 10f)
                return Pick("Appearance.BreastSize.MassiveMotherly", "Appearance.BreastSize.Luscious", "Appearance.BreastSize.Smothering", "Appearance.BreastSize.Prodigious");

            if (value >= 8f)
                return Pick("Appearance.BreastSize.BasketballSized", "Appearance.BreastSize.Whorish", "Appearance.BreastSize.Cushiony", "Appearance.BreastSize.Wobbling");

            if (value >= 7f)
                return Pick("Appearance.BreastSize.SoccerballSized", "Appearance.BreastSize.HandOverflowing", "Appearance.BreastSize.Generous", "Appearance.BreastSize.Jiggling");

            if (value >= 5f)
                return Pick("Appearance.BreastSize.Big", "Appearance.BreastSize.Large", "Appearance.BreastSize.Pillowy", "Appearance.BreastSize.Jiggly", "Appearance.BreastSize.VolleyballSized");

            if (value >= 3f)
                return Pick("Appearance.BreastSize.Nice", "Appearance.BreastSize.HandFilling", "Appearance.BreastSize.WellRounded", "Appearance.BreastSize.Supple", "Appearance.BreastSize.SoftballSized");

            if (value >= 1f)
                return Pick("Appearance.BreastSize.Palmable", "Appearance.BreastSize.Tight", "Appearance.BreastSize.Perky", "Appearance.BreastSize.BaseballSized");

            return "Appearance.Breast.Manly".Localized();
        }

        public static string BreastDescript(float size, float lactation)
        {
            if (size < 1f)
                return "Appearance.Breast.Flat".Localized();

            var descript = Random.Shared.Next(2) == 0 ? BreastSizeDescript(size) : "";

            switch (Random.Shared.Next(10))
            {
                case 1:
                    if (lactation > 2f)
                        return descript + "Appearance.Breast.Noun.MilkUdders".Localized();

                    break;

                case 2:
                    if (lactation > 1.5f)
                        descript += "Appearance.Breast.Milky".Localized();

                    if (size > 4f)
                        return descript + "Appearance.Breast.Noun.Tits".Localized();

                    break;

                case 4:
                case 5:
                case 6:
                    return descript + "Appearance.Breast.Noun.Tits".Localized();

                case 7:
                    if (lactation >= 2.5f)
                        return descript + "Appearance.Breast.Noun.Udders".Localized();

                    if (lactation >= 1f)
                        descript += "Appearance.Breast.Milk".Localized();

                    return descript + "Appearance.Breast.Noun.Jugs".Localized();

                case 8:
                    return size > 6f ? descript + "Appearance.Breast.Noun.LovePillows".Localized() : descript + "Appearance.Breast.Noun.Boobs".Localized();

                case 9:
                    if (size > 6f)
                        return descript + "Appearance.Breast.Noun.Tits".Localized();

                    break;
            }

            return descript + "Appearance.Breast.Noun.Breasts".Localized();
        }

        public static string BiggestBreastSizeDescript(Creature creature)
        {
            var row = creature.BreastRows[creature.BiggestTitRow()];

            if (row.BreastRating < 1f)
                return "Appearance.Breast.Flat".Localized();

            var descript = Random.Shared.Next(2) == 0 ? BreastSizeDescript(row.BreastRating) : "";

            descript += Random.Shared.Next(10) switch
            {
                0 => "Appearance.Breast.Noun.Breasts".Localized(),

                1 => row.Lactation > 2f ? "Appearance.Breast.Noun.MilkUdders".Localized() : "Appearance.Breast.Noun.Breasts".Localized(),

                2 => (row.Lactation > 1.5f ? "milky" : "") + (row.BreastRating > 4f ? "Appearance.Breast.Noun.Tits".Localized() : "Appearance.Breast.Noun.Breasts".Localized()),

                3 => "Appearance.Breast.Noun.Breasts".Localized(),

                4 or 5 or 6 => "Appearance.Breast.Noun.Tits".Localized(),

                7 => row.Lactation switch
                {
                    < 1f => "Appearance.Breast.Noun.Jugs".Localized(),

                    < 2.5f => "Appearance.Breast.Noun.MilkJugs".Localized(),

                    _ => "Appearance.Breast.Noun.Udders".Localized()
                },

                8 => row.BreastRating > 6f ? "Appearance.Breast.Noun.LovePillows".Localized() : "Appearance.Breast.Noun.Boobs".Localized(),

                9 => row.BreastRating > 6f ? "Appearance.Breast.Noun.Tits".Localized() : "Appearance.Breast.Noun.Breasts".Localized(),

                _ => ""
            };

            return descript;
        }

        public static string AllBreastsDescript(Creature creature)
        {
            var storage = creature.BreastRows.Count switch
            {
                0 => "Appearance.Breast.Row.None".Localized(),

                2 => "Appearance.Breast.Row.Two".Localized(),

                3 => Pick("Appearance.BreastAll.ThreeRowsOf", "Appearance.BreastAll.MultiLayered"),

                4 => Pick("Appearance.BreastAll.FourRowsOf", "Appearance.BreastAll.FourTiered"),

                5 => Pick("Appearance.BreastAll.FiveRowsOf", "Appearance.BreastAll.FiveTiered"),

                _ => ""
            };

            if (creature.BreastRows.Count == 0)
                return storage;

            return storage + BiggestBreastSizeDescript(creature);
        }

        public static string ButtDescript(Creature creature) =>

            creature.Body.Get<ButtPart>().Type.DescribeRating(creature.Body.Butt, creature.Tone);

        public static string ButtDescriptShort(Creature creature)
        {
            var rating = creature.Body.Butt;

            var description = rating switch
            {
                < 2 => Utils.Utils.RandChoice("insignificant ", "very small "),

                < 4 => Utils.Utils.RandChoice("tight ", "firm ", "compact "),

                < 6 => Utils.Utils.RandChoice("regular ", "unremarkable "),

                < 8 => Utils.Utils.RandChoice("full ", "shapely ", "handful of ass"),

                < 10 => Utils.Utils.RandChoice("squeezable ", "large ", "substantial "),

                < 13 => Utils.Utils.RandChoice("jiggling ", "spacious ", "heavy "),

                < 16 => Utils.Utils.RandChoice("expansive ", "voluminous ", "generous amount of ass"),

                < 20 => Utils.Utils.RandChoice("huge ", "vast ", "jiggling expanse of ass"),

                _ => Utils.Utils.RandChoice("ginormous ", "colossal ", "tremendous "),
            };

            if (description.EndsWith(' '))
            {
                description += Utils.Utils.RandChoice("butt ", "ass ");

                if (Utils.Utils.randomChance(50))
                    description += "cheeks";
            }

            return description;
        }

        public static string HipDescript(Creature creature)
        {
            var smallFlared = creature.Thickness < 30f && !creature.IsChild() && (creature.Gender != Gender.Male || creature.Femininity > 55f);

            return creature.Body.Get<HipsPart>().Type.DescribeHips(creature.Body.Hips, creature.Thickness, smallFlared);
        }

        public static string ClitDescript(Creature creature)
        {
            var length = creature.Vaginas[0].ClitLength;

            var description = "";

            if (Random.Shared.Next(2) == 0)
                description += length switch
                {
                    >= 4f => Pick("Appearance.Clit.Monster", "Appearance.Clit.Tremendous", "Appearance.Clit.Colossal", "Appearance.Clit.Enormous", "Appearance.Clit.Bulky"),

                    >= 1.5f => Pick("Appearance.Clit.Large", "Appearance.Clit.Large", "Appearance.Clit.Substantial", "Appearance.Clit.Substantial", "Appearance.Clit.Considerable"),

                    >= 0.3f => "",

                    _ => Pick("Appearance.Clit.Tiny", "Appearance.Clit.Little", "Appearance.Clit.Petite", "Appearance.Clit.Diminutive", "Appearance.Clit.Miniature")
                };

            if (creature.Vaginas.Count > 0 && creature.Vaginas[0].ClitPierced > 0 && Random.Shared.Next(2) == 0)
                description += "Appearance.Clit.Pierced".Localized();
            else if (creature.Body.Get<SkinPart>().HasFur && Random.Shared.Next(2) == 0)
                description += "Appearance.Clit.Bitch".Localized();
            else if (creature.Lust > 70f && Random.Shared.Next(4) != 0)
                description += Pick("Appearance.Clit.Throbbing", "Appearance.Clit.Pulsating", "Appearance.Clit.Hard");

            description += Pick("Appearance.Clit.Clit", "Appearance.Clit.Clitty", "Appearance.Clit.Button", "Appearance.Clit.PleasureBuzzer", "Appearance.Clit.Clit", "Appearance.Clit.Clitty", "Appearance.Clit.Button", "Appearance.Clit.Clit", "Appearance.Clit.Clit", "Appearance.Clit.Button");

            return description;
        }

        public static string WetnessDescript(VaginaPart vagina) => vagina.Wetness switch
        {
            <= 0f => "Appearance.Wetness.Dry".Localized(),

            <= 1f => "Appearance.Wetness.Moist".Localized(),

            <= 2f => "Appearance.Wetness.Wet".Localized(),

            <= 3f => "Appearance.Wetness.Slick".Localized(),

            <= 4f => "Appearance.Wetness.Drooling".Localized(),

            _ => "Appearance.Wetness.Slavering".Localized()
        };

        public static string AssholeOrPussy(Creature creature) =>

            creature.HasVagina() ? VaginaDescript(creature, 0) : creature.AssholeDescript();

        public static string VaginaDescript(Creature creature, int index = 0)
        {
            var vagina = creature.Vaginas[index];

            var weighting = vagina.Looseness switch
            {
                0 => 61,

                4 or 5 => 10,

                _ => 0
            };

            var description = "";

            if (Random.Shared.Next(100) < 40 + weighting)
                description += vagina.Looseness switch
                {
                    0 => vagina.Virgin ? "Appearance.Vagina.Looseness.Virgin".Localized() : "Appearance.Vagina.Looseness.Tight".Localized(),

                    1 => "",

                    2 => "Appearance.Vagina.Looseness.Loose".Localized(),

                    3 => "Appearance.Vagina.Looseness.VeryLoose".Localized(),

                    4 => "Appearance.Vagina.Looseness.Gaping".Localized(),

                    _ => "Appearance.Vagina.Looseness.GapingWide".Localized()
                };

            if (Random.Shared.Next(100) < 30 + weighting)
            {
                if (description != "")
                    description += ", ";

                description += WetnessDescript(vagina);
            }

            if (BodyPartSlotRegistry.Get(vagina.SlotId).Types.Get(vagina.Id).GetString("descript") == "BlackSandTrap" && Random.Shared.Next(2) == 0)
            {
                if (description != "")
                    description += ", ";

                description += Pick("Appearance.Vagina.Color.BlackSandTrap.Black", "Appearance.Vagina.Color.BlackSandTrap.Onyx", "Appearance.Vagina.Color.BlackSandTrap.Ebony", "Appearance.Vagina.Color.BlackSandTrap.Dusky", "Appearance.Vagina.Color.BlackSandTrap.Sable", "Appearance.Vagina.Color.BlackSandTrap.Obsidian", "Appearance.Vagina.Color.BlackSandTrap.MidnightHued", "Appearance.Vagina.Color.BlackSandTrap.JetBlack");
            }

            if (vagina.LabiaPierced > 0 && Random.Shared.Next(3) == 0)
            {
                if (description != "")
                    description += ", ";

                description += "Appearance.Vagina.Pierced".Localized();
            }

            if (description != "")
                description += " ";

            return description + Pick("Appearance.Vagina.Vagina", "Appearance.Vagina.Pussy", "Appearance.Vagina.Cooter", "Appearance.Vagina.Twat", "Appearance.Vagina.Cunt", "Appearance.Vagina.Snatch", "Appearance.Vagina.FuckHole", "Appearance.Vagina.Muff");
        }

        public static string BallsDescription(bool forcedSize, bool plural, Creature creature, bool withArticle = false)
        {
            if (creature.Balls == 0)
                return "Appearance.Sack.Prostate".Localized();

            var description = "";

            void AddLeadingSpace()
            {
                if (description.Length > 0)
                    description += " ";
            }

            if (plural && !creature.HasStatusEffect(StatusEffectIds.Uniball))
            {
                var suffix = withArticle ? "Article" : "Bare";

                description += creature.Balls switch
                {
                    1 => Pick("Appearance.Balls.Count.One." + suffix, "Appearance.Balls.Count.One." + suffix + "2", "Appearance.Balls.Count.One." + suffix + "3", "Appearance.Balls.Count.One." + suffix + "4"),

                    2 => Pick("Appearance.Balls.Count.Two." + suffix, "Appearance.Balls.Count.Two." + suffix + "2", "Appearance.Balls.Count.Two." + suffix + "3"),

                    3 => Pick("Appearance.Balls.Count.Three." + suffix, "Appearance.Balls.Count.Three." + suffix + "2", "Appearance.Balls.Count.Three." + suffix + "3"),

                    4 => Pick("Appearance.Balls.Count.Four." + suffix, "Appearance.Balls.Count.Four." + suffix + "2", "Appearance.Balls.Count.Four." + suffix + "3"),

                    _ => Pick("Appearance.Balls.Count.Many." + suffix, "Appearance.Balls.Count.Many." + suffix + "2", "Appearance.Balls.Count.Many." + suffix + "3")
                };
            }

            if (forcedSize || Random.Shared.Next(100) < 66)
            {
                AddLeadingSpace();

                description += creature.BallSize switch
                {
                    >= 18f => "Appearance.Balls.Size.HideouslySwollen".Localized(),

                    >= 15f => "Appearance.Balls.Size.BeachBall".Localized(),

                    >= 12f => "Appearance.Balls.Size.Watermelon".Localized(),

                    >= 9f => "Appearance.Balls.Size.Basketball".Localized(),

                    >= 7f => "Appearance.Balls.Size.Soccerball".Localized(),

                    >= 5f => "Appearance.Balls.Size.Cantaloupe".Localized(),

                    >= 4f => "Appearance.Balls.Size.Grapefruit".Localized(),

                    >= 3f => "Appearance.Balls.Size.Apple".Localized(),

                    >= 2f => "Appearance.Balls.Size.Baseball".Localized(),

                    > 1f => "Appearance.Balls.Size.Large".Localized(),

                    >= 1f => "",

                    >= 0.5f => "Appearance.Balls.Size.Small".Localized(),

                    _ => "Appearance.Balls.Size.Tiny".Localized()
                };
            }

            if (creature.HasStatusEffect(StatusEffectIds.Uniball))
            {
                AddLeadingSpace();

                description += Pick("Appearance.Balls.Uniball.TightlyCompressed", "Appearance.Balls.Uniball.Snug", "Appearance.Balls.Uniball.Cute", "Appearance.Balls.Uniball.PleasantlySqueezed", "Appearance.Balls.Uniball.CompressedTogether");
            }

            if (creature.HoursSinceCum >= 48 && Random.Shared.Next(100) < 50 && !forcedSize)
            {
                AddLeadingSpace();

                description += Pick("Appearance.Balls.Overflow.Overflowing", "Appearance.Balls.Overflow.Swollen", "Appearance.Balls.Overflow.CumEngorged");
            }

            if (description.Length == 0 && creature.LustPercent > 90 && Random.Shared.Next(100) < 50 && !forcedSize)
                description += Pick("Appearance.Balls.Lusty.Eager", "Appearance.Balls.Lusty.Full", "Appearance.Balls.Lusty.Needy", "Appearance.Balls.Lusty.Desperate", "Appearance.Balls.Lusty.Throbbing", "Appearance.Balls.Lusty.Heated", "Appearance.Balls.Lusty.Trembling", "Appearance.Balls.Lusty.Quivering", "Appearance.Balls.Lusty.Quaking");

            if (creature.Body.Get<SkinPart>().Id.Path == "goo")
            {
                AddLeadingSpace();

                description += Pick("Appearance.Balls.Goo.Goopy", "Appearance.Balls.Goo.Gooey", "Appearance.Balls.Goo.Slimy");
            }

            AddLeadingSpace();

            description += Pick("Appearance.Balls.Nut", "Appearance.Balls.Gonad", "Appearance.Balls.Teste", "Appearance.Balls.Testicle", "Appearance.Balls.Testicle", "Appearance.Balls.Ball", "Appearance.Balls.Ball", "Appearance.Balls.Ball");

            if (plural)
                description += "Appearance.Plural.S".Localized();

            return description;
        }

        public static string BallsDescript(Creature creature, bool plural = true, bool withArticle = false) =>

            BallsDescription(true, plural, creature, withArticle);

        public static string BallsDescriptLight(Creature creature, bool forcedSize = true, bool withArticle = false) =>

            BallsDescription(forcedSize, true, creature, withArticle);

        public static string SimpleBallsDescript(Creature creature, bool withArticle = false) =>

            BallsDescription(false, true, creature, withArticle);

        public static string SackDescript(Creature creature) => creature.Balls == 0 ? "Appearance.Sack.Prostate" : Pick("Appearance.Sack.Scrotum", "Appearance.Sack.Sack", "Appearance.Sack.Nutsack", "Appearance.Sack.Ballsack", "Appearance.Sack.Beanbag", "Appearance.Sack.Pouch");

        public static string NippleDescript(Creature creature, int row = 0, bool plural = false)
        {
            var breastRow = creature.BreastRows[row];

            var description = "";

            var haveDescription = false;

            if (Random.Shared.Next(4) == 0)
            {
                var size = creature.NippleLength switch
                {
                    < 0.25f => Pick("Appearance.Nipple.Adj.Tiny", "Appearance.Nipple.Adj.IttyBitty", "Appearance.Nipple.Adj.TeenyTiny", "Appearance.Nipple.Adj.Dainty"),

                    < 0.4f => "",

                    < 1f => Pick("Appearance.Nipple.Adj.Prominent", "Appearance.Nipple.Adj.PencilEraserSized", "Appearance.Nipple.Adj.EyeCatching", "Appearance.Nipple.Adj.Pronounced", "Appearance.Nipple.Adj.Striking"),

                    < 2f => Pick("Appearance.Nipple.Adj.ForwardsJutting", "Appearance.Nipple.Adj.OverSized", "Appearance.Nipple.Adj.Fleshy", "Appearance.Nipple.Adj.LargeProtruding"),

                    < 3.2f => Pick("Appearance.Nipple.Adj.Elongated", "Appearance.Nipple.Adj.Massive", "Appearance.Nipple.Adj.Awkward", "Appearance.Nipple.Adj.Lavish", "Appearance.Nipple.Adj.Hefty"),

                    _ => Pick("Appearance.Nipple.Adj.Bulky", "Appearance.Nipple.Adj.Ponderous", "Appearance.Nipple.Adj.ThumbSized", "Appearance.Nipple.Adj.CockSized", "Appearance.Nipple.Adj.CowLike")
                };

                description += size;

                haveDescription = true;
            }

            if (!haveDescription && Random.Shared.Next(3) == 0)
            {
                var lactation = creature.BiggestLactation();

                if (creature.HasFuckableNipples())
                {
                    description += lactation > 1f

                        ? Pick("Appearance.Nipple.Fuckable.MilkLubricated", "Appearance.Nipple.Fuckable.Lactating", "Appearance.Nipple.Fuckable.MilkSlicked", "Appearance.Nipple.Fuckable.Milky")

                        : Pick("Appearance.Nipple.Fuckable.Wet", "Appearance.Nipple.Fuckable.Mutated", "Appearance.Nipple.Fuckable.Slimy", "Appearance.Nipple.Fuckable.Slippery", "Appearance.Nipple.Fuckable.Oozing", "Appearance.Nipple.Fuckable.Dewy");

                    haveDescription = true;
                }
                else if (lactation > 0f)
                {
                    description += lactation switch
                    {
                        <= 1f => Pick("Appearance.Nipple.Milk.LightOne", "Appearance.Nipple.Milk.LightTwo", "Appearance.Nipple.Milk.LightThree"),

                        <= 2f => Pick("Appearance.Nipple.Milk.ModerateOne", "Appearance.Nipple.Milk.ModerateTwo", "Appearance.Nipple.Milk.ModerateThree"),

                        _ => Pick("Appearance.Nipple.Milk.HeavyOne", "Appearance.Nipple.Milk.HeavyTwo", "Appearance.Nipple.Milk.HeavyThree", "Appearance.Nipple.Milk.HeavyFour")
                    };

                    haveDescription = true;
                }
            }
            else if (!haveDescription && Random.Shared.Next(3) == 0)
                if (creature.Lust > 75f)
                {
                    description += Pick("Appearance.Nipple.Arousal.Throbbing", "Appearance.Nipple.Arousal.Trembling", "Appearance.Nipple.Arousal.Needy");

                    haveDescription = true;
                }
                else if (creature.Lust > 50f)
                {
                    description += Pick("Appearance.Nipple.Arousal.Erect", "Appearance.Nipple.Arousal.Perky", "Appearance.Nipple.Arousal.Firm", "Appearance.Nipple.Arousal.Tender");

                    haveDescription = true;
                }

            if (!haveDescription && Random.Shared.Next(2) == 0 && breastRow.NipplesPierced > 0)
            {
                description += breastRow.NipplesPierced == 5 ? "Appearance.Nipple.Pierced.Chained".Localized() : "Appearance.Nipple.Pierced.Pierced".Localized();

                haveDescription = true;
            }

            if (!haveDescription && creature.Body.Get<SkinPart>().Id.Path == "goo")
                description += Pick("Appearance.Nipple.Goo.SlimeSlick", "Appearance.Nipple.Goo.Goopy", "Appearance.Nipple.Goo.Slippery");

            var teatOrNipple = creature.BiggestLactation() >= 1f && creature.NippleLength >= 1f ? "Appearance.Nipple.Noun.Teat" : "Appearance.Nipple.Noun.Nipple";

            var noun = Pick(
                "Appearance.Nipple.Noun.Nipple",

                creature.NippleLength < 0.5f ? "Appearance.Nipple.Noun.PerkyNipple" : "Appearance.Nipple.Noun.CherryLikeNub",

                teatOrNipple,

                teatOrNipple,

                "Appearance.Nipple.Noun.Nipple");

            description += noun;

            return plural ? description + "Appearance.Plural.S".Localized() : description;
        }

        public static string NippleNouns(Creature creature, int row = 0) => NippleDescript(creature, row, true);

        public static string CockNoun(Creature creature, int index = 0)
        {
            var cock = creature.Cocks[index];

            var type = BodyPartSlotRegistry.Get(cock.SlotId).Types.Get(cock.Id).GetString("type");

            if (type == "")
                type = "Default";

            return ("Appearance.Cock.Noun.".Localized() + type).Localized();
        }

        public static string CockNouns(Creature creature, int index = 0) => CockNoun(creature, index) + "Appearance.Plural.S".Localized();

        public static string CockNoun(Identifier type)
        {
            var name = type.Path;

            if (string.IsNullOrEmpty(name))
                name = "human";

            return ("Appearance.Cock.Noun." + char.ToUpperInvariant(name[0]) + name[1..]).Localized();
        }

        public static string CockMultiNoun(Identifier type, Creature charRef = null)
        {
            var path = type.Path ?? "";

            var adjectives = path switch
            {
                "bee" => ["bee", "insect", "insectoid"],

                "dog" => charRef != null && charRef.IsChild()

                    ? ["dog", "doggy", "canine", "puppy"]

                    : ["dog", "doggy", "canine"],

                "horse" => charRef != null

                    ? (charRef.IsChild()

                        ? new[] { "horse", "equine", charRef.Mf("stallion", "mare"), "foal" }

                        : ["horse", "equine", charRef.Mf("stallion", "mare")])

                    : ["horse", "equine"],

                "demon" => ["demon", "demonic", "corrupted"],

                "tentacle" => ["tentacle", "plant", "vine"],

                "cat" => charRef != null && charRef.IsChild()

                    ? ["feline", "cat", "kitty", "kitten"]

                    : ["feline", "cat", "kitty"],

                "lizard" => ["reptile", "reptilian", "lizard", "snake", "serpent"],

                "rhino" => ["rhino"],

                "wolf" => charRef != null && charRef.IsChild()

                    ? ["wolf", "canine", "puppy"]

                    : ["wolf", "canine"],

                "anemone" => ["anemone"],

                "kangaroo" => ["kangaroo", "roo"],

                "dragon" => ["dragon", "draconic"],

                "displacer" => ["displacer", "coeurl", "alien"],

                "fox" => ["fox", "vulpine"],

                "pig" => ["pig", "piggy"],

                "avian" => ["bird", "avian"],

                "echidna" => ["echidna"],

                "red_panda" => ["red panda"],

                "ferret" => ["ferret"],

                "coc:gnoll" => ["coc:gnoll", "hyena"],

                _ => [],
            };

            var description = adjectives.Length > 0

                ? adjectives[Random.Shared.Next(adjectives.Length)] + " "

                : "";

            var nouns = new[]
            {
                "cock", "cock", "cock", "cock", "cock",

                "prick", "prick",

                "pecker",

                "shaft", "shaft", "shaft",
            };

            var noun = nouns[Random.Shared.Next(nouns.Length)];

            description += ("Appearance.Cock.Noun." + char.ToUpperInvariant(noun[0]) + noun[1..]);

            return description;
        }

        public static string CockHead(Creature creature, int index = 0)        {
            var cock = creature.Cocks[index];

            var type = BodyPartSlotRegistry.Get(cock.SlotId).Types.Get(cock.Id).GetString("type");

            if (type == "")
                type = "Default";

            return ("Appearance.Cock.Head.".Localized() + type).Localized();
        }

        public static string CockAdjective(Creature creature, int index = 0)
        {
            var cock = creature.Cocks[index];

            var isPierced = creature.Cocks.Count == 1 && cock.Pierced > 0;

            var hasSock = creature.Cocks.Count == 1 && cock.Sock != "";

            var isGooey = creature.Body.Get<SkinPart>().Id.Path == "goo";

            if (isPierced && Random.Shared.Next(5) == 0)
                return "Appearance.Cock.Pierced".Localized();

            if (hasSock && Random.Shared.Next(5) == 0)
                return Pick("Appearance.Cock.Sock.SockSheathed", "Appearance.Cock.Sock.GarmentWrapped", "Appearance.Cock.Sock.SmartlyDressed", "Appearance.Cock.Sock.ClothShrouded", "Appearance.Cock.Sock.FabricSwaddled", "Appearance.Cock.Sock.Covered");

            if (isGooey && Random.Shared.Next(4) == 0)
                return Pick("Appearance.Cock.Goo.Goopy", "Appearance.Cock.Goo.Gooey", "Appearance.Cock.Goo.Slimy");

            return CockAdjective(cock.Length, cock.Thickness);
        }

        public static string CockAdjective(float length, float thickness)
        {
            if (Random.Shared.Next(3) == 0)
                return length switch
                {
                    < 3f => Pick("Appearance.Cock.Adj.Little", "Appearance.Cock.Adj.ToySized", "Appearance.Cock.Adj.Mini", "Appearance.Cock.Adj.Budding", "Appearance.Cock.Adj.Tiny"),

                    < 5f => Pick("Appearance.Cock.Adj.Short", "Appearance.Cock.Adj.Small"),

                    < 7f => Pick("Appearance.Cock.Adj.FairSized", "Appearance.Cock.Adj.Nice"),

                    < 9f => Pick("Appearance.Cock.Adj.Sizable", "Appearance.Cock.Adj.Long", "Appearance.Cock.Adj.Lengthy"),

                    < 13f => Pick("Appearance.Cock.Adj.Huge", "Appearance.Cock.Adj.FootLong", "Appearance.Cock.Adj.CucumberLength"),

                    < 18f => Pick("Appearance.Cock.Adj.Massive", "Appearance.Cock.Adj.KneeLength", "Appearance.Cock.Adj.ForearmLength"),

                    < 30f => Pick("Appearance.Cock.Adj.Enormous", "Appearance.Cock.Adj.Giant", "Appearance.Cock.Adj.ArmLike"),

                    _ => "Appearance.Cock.Adj.Towering".Localized()
                };

            return thickness switch
            {
                <= 0.75f => Pick("Appearance.Cock.Adj.Thin", "Appearance.Cock.Adj.Slender", "Appearance.Cock.Adj.Narrow"),

                <= 1.2f => "Appearance.Cock.Adj.Ample".Localized(),

                <= 1.4f => Pick("Appearance.Cock.Adj.Ample", "Appearance.Cock.Adj.Big"),

                <= 2f => Pick("Appearance.Cock.Adj.Broad", "Appearance.Cock.Adj.Meaty", "Appearance.Cock.Adj.Girthy"),

                <= 3.5f => Pick("Appearance.Cock.Adj.Fat", "Appearance.Cock.Adj.Distended", "Appearance.Cock.Adj.Wide"),

                _ => Pick("Appearance.Cock.Adj.InhumanlyDistended", "Appearance.Cock.Adj.MonstrouslyThick", "Appearance.Cock.Adj.Bloated")
            };
        }

        public static string CockDescription(Identifier type, float length, float girth, int lust = 50, float cumQ = 10)
        {
            var noun = CockNoun(type);

            if (!Utils.Utils.randomChance(50))
                return noun;

            var adjective = CockAdjective(length, girth);

            return type == BodyPartTypes.Human ? adjective + " " + noun : adjective + ", " + noun;
        }

        public static string CockDescriptShort(Creature creature, int index = 0)
        {
            if (creature.Cocks.Count == 0)
                return "<b>ERROR. INVALID CREATURE SPECIFIED to cockDescriptShort</b>";

            var cock = creature.Cocks[index];

            var description = "";

            if (Random.Shared.Next(3) == 0)
            {
                var adj = cock.Length switch
                {
                    >= 30f => "Appearance.Cock.Adj.Towering",

                    >= 18f => "Appearance.Cock.Adj.Enormous",

                    >= 13f => "Appearance.Cock.Adj.Massive",

                    >= 10f => "Appearance.Cock.Adj.Huge",

                    >= 7f => "Appearance.Cock.Adj.Long",

                    >= 5f => "Appearance.Cock.Adj.Average",

                    _ => "Appearance.Cock.Adj.Short"
                };

                description = adj.Localized() + " ";
            }
            else if (Random.Shared.Next(2) == 0)

                if (cock.Thickness <= 0.75f)
                    description = "Appearance.Cock.Adj.Narrow".Localized() + " ";
                else if (cock.Thickness > 1f && cock.Thickness <= 1.4f)
                    description = "Appearance.Cock.Adj.Ample".Localized() + " ";
                else if (cock.Thickness > 1.4f && cock.Thickness <= 2f)
                    description = "Appearance.Cock.Adj.Broad".Localized() + " ";
                else if (cock.Thickness > 2f && cock.Thickness <= 3.5f)
                    description = "Appearance.Cock.Adj.Fat".Localized() + " ";
                else if (cock.Thickness > 3.5f)
                    description = "Appearance.Cock.Adj.Distended".Localized() + " ";

            return description + CockNoun(creature, index);
        }

        public static string CockDescript(Creature creature, int index = 0) => CockAdjective(creature, index) + " " + CockNoun(creature, index);

        public static string CockMultiLDescriptionShort(Creature creature)
        {
            var cocks = creature.Cocks;

            if (cocks.Count < 1)
                return "<b>ERROR: NO WANGS DETECTED for cockMultiLightDesc()</b>";

            if (cocks.Count == 1)
                return CockDescript(creature, 0);

            var type = CockTypeName(cocks[0]);

            var sameType = cocks.All(cock => CockTypeName(cock) == type);

            var desc = sameType ? CockNoun(creature, 0) : "Appearance.Cock.Noun.Human".Localized();

            if (desc.Contains("penis"))
                desc += "e";

            return desc + "s";
        }

        public static string CockTypeName(CockPart cock)
        {
            var type = BodyPartSlotRegistry.Get(cock.SlotId).Types.Get(cock.Id).GetString("type");

            return type == "" ? "Default" : type;
        }

        public static string SheathDescript(CockPart cock) => cock.SheathOn ? "Appearance.Cock.Sheath".Localized() : "";

        public static string MultiCockDescript(Creature creature) => Pick("Appearance.Cock.Multi.Two", "Appearance.Cock.Multi.PairOf", "Appearance.Cock.Multi.Two2", "Appearance.Cock.Multi.BraceOf") + CockNouns(creature, 0);

        public static string MultiCockDescriptLight(Creature creature, int excludeIndex = -1)
        {
            var total = creature.Cocks.Count;

            if (total < 1)
                return "<B>Error: multiCockDescriptLight() called with no penises present.</B>";

            if (total == 1 || (total == 2 && excludeIndex >= 0 && excludeIndex < total))
                return creature.CockDescript(0);

            var types = new List<string>();

            var typeIndex = 0;

            for (var index = 0; index < total; index++)
            {
                if (index == excludeIndex)
                    continue;

                var type = BodyPartSlotRegistry.Get(creature.Cocks[index].SlotId).Types.Get(creature.Cocks[index].Id).GetString("type");

                if (type == "")
                    type = "Default";

                if (!types.Contains(type))
                {
                    types.Add(type);

                    typeIndex = index;
                }
            }

            var same = types.Count == 1;

            var adjectiveIndex = creature.BiggestCockIndex();

            var separator = same && creature.Cocks[typeIndex].Id.Path == "human" ? " " : ", ";

            if (same)
            {
                var description = PickLightPrefix(total, true) + CockAdjective(creature, adjectiveIndex) + separator + CockNoun(creature, typeIndex);

                if (description.Contains("penis"))
                    description += "e";

                return description + "s";
            }

            return PickLightPrefix(total, false) + CockAdjective(creature, adjectiveIndex) + ", " + Pick(
                "Appearance.Cock.MultiLight.MutatedCocks",

                "Appearance.Cock.MultiLight.MutatedDicks",

                "Appearance.Cock.MultiLight.MixedCocks",

                "Appearance.Cock.MultiLight.MismatchedDicks");
        }

        public static string PickLightPrefix(int total, bool same)
        {
            if (total == 2)
                return same

                    ? Pick(
                        "Appearance.Cock.MultiLight.PairOf",

                        "Appearance.Cock.MultiLight.Two",

                        "Appearance.Cock.MultiLight.BraceOf",

                        "Appearance.Cock.MultiLight.Matching",

                        "Appearance.Cock.MultiLight.Twin")

                    : Pick(
                        "Appearance.Cock.MultiLight.PairOf",

                        "Appearance.Cock.MultiLight.Two",

                        "Appearance.Cock.MultiLight.BraceOf");

            if (total == 3)
                return same

                    ? Pick(
                        "Appearance.Cock.MultiLight.Three",

                        "Appearance.Cock.MultiLight.GroupOf",

                        "Appearance.Cock.MultiLight.Menage",

                        "Appearance.Cock.MultiLight.TriadOf",

                        "Appearance.Cock.MultiLight.TriumvirateOf")

                    : Pick(
                        "Appearance.Cock.MultiLight.Three",

                        "Appearance.Cock.MultiLight.GroupOf");

            return Pick(
                "Appearance.Cock.MultiLight.BundleOf",

                "Appearance.Cock.MultiLight.ObsceneGroupOf",

                "Appearance.Cock.MultiLight.ClusterOf",

                "Appearance.Cock.MultiLight.WrigglingBunchOf");
        }

        public static readonly string[] BreastCupNames =

        [
            "flat", "A-cup", "B-cup", "C-cup", "D-cup", "DD-cup", "big DD-cup", "E-cup", "big E-cup", "EE-cup",

            "big EE-cup", "F-cup", "big F-cup", "FF-cup", "big FF-cup", "G-cup", "big G-cup", "GG-cup", "big GG-cup", "H-cup",

            "big H-cup", "HH-cup", "big HH-cup", "HHH-cup", "I-cup", "big I-cup", "II-cup", "big II-cup", "J-cup", "big J-cup",

            "JJ-cup", "big JJ-cup", "K-cup", "big K-cup", "KK-cup", "big KK-cup", "L-cup", "big L-cup", "LL-cup", "big LL-cup",

            "M-cup", "big M-cup", "MM-cup", "big MM-cup", "MMM-cup", "large MMM-cup", "N-cup", "large N-cup", "NN-cup", "large NN-cup",

            "O-cup", "large O-cup", "OO-cup", "large OO-cup", "P-cup", "large P-cup", "PP-cup", "large PP-cup", "Q-cup", "large Q-cup",

            "QQ-cup", "large QQ-cup", "R-cup", "large R-cup", "RR-cup", "large RR-cup", "S-cup", "large S-cup", "SS-cup", "large SS-cup",

            "T-cup", "large T-cup", "TT-cup", "large TT-cup", "U-cup", "large U-cup", "UU-cup", "large UU-cup", "V-cup", "large V-cup",

            "VV-cup", "large VV-cup", "W-cup", "large W-cup", "WW-cup", "large WW-cup", "X-cup", "large X-cup", "XX-cup", "large XX-cup",

            "Y-cup", "large Y-cup", "YY-cup", "large YY-cup", "Z-cup", "large Z-cup", "ZZ-cup", "large ZZ-cup", "ZZZ-cup", "large ZZZ-cup",

            "hyper A-cup", "hyper B-cup", "hyper C-cup", "hyper D-cup", "hyper DD-cup", "hyper big DD-cup", "hyper E-cup", "hyper big E-cup", "hyper EE-cup", "hyper big EE-cup",

            "hyper F-cup", "hyper big F-cup", "hyper FF-cup", "hyper big FF-cup", "hyper G-cup", "hyper big G-cup", "hyper GG-cup", "hyper big GG-cup", "hyper H-cup", "hyper big H-cup",

            "hyper HH-cup", "hyper big HH-cup", "hyper HHH-cup", "hyper I-cup", "hyper big I-cup", "hyper II-cup", "hyper big II-cup", "hyper J-cup", "hyper big J-cup", "hyper JJ-cup",

            "hyper big JJ-cup", "hyper K-cup", "hyper big K-cup", "hyper KK-cup", "hyper big KK-cup", "hyper L-cup", "hyper big L-cup", "hyper LL-cup", "hyper big LL-cup", "hyper M-cup",

            "hyper big M-cup", "hyper MM-cup", "hyper big MM-cup", "hyper MMM-cup", "hyper large MMM-cup", "hyper N-cup", "hyper large N-cup", "hyper NN-cup", "hyper large NN-cup", "hyper O-cup",

            "hyper large O-cup", "hyper OO-cup", "hyper large OO-cup", "hyper P-cup", "hyper large P-cup", "hyper PP-cup", "hyper large PP-cup", "hyper Q-cup", "hyper large Q-cup", "hyper QQ-cup",

            "hyper large QQ-cup", "hyper R-cup", "hyper large R-cup", "hyper RR-cup", "hyper large RR-cup", "hyper S-cup", "hyper large S-cup", "hyper SS-cup", "hyper large SS-cup", "hyper T-cup",

            "hyper large T-cup", "hyper TT-cup", "hyper large TT-cup", "hyper U-cup", "hyper large U-cup", "hyper UU-cup", "hyper large UU-cup", "hyper V-cup", "hyper large V-cup", "hyper VV-cup",

            "hyper large VV-cup", "hyper W-cup", "hyper large W-cup", "hyper WW-cup", "hyper large WW-cup", "hyper X-cup", "hyper large X-cup", "hyper XX-cup", "hyper large XX-cup", "hyper Y-cup",

            "hyper large Y-cup", "hyper YY-cup", "hyper large YY-cup", "hyper Z-cup", "hyper large Z-cup", "hyper ZZ-cup", "hyper large ZZ-cup", "hyper ZZZ-cup", "hyper large ZZZ-cup", "jacques00-cup",
        ];

        public static string BreastCup(float size)
        {
            var index = (int)MathF.Floor(size);

            if (index < 0)
                index = 0;

            if (index > BreastCupNames.Length - 1)
                index = BreastCupNames.Length - 1;

            return BreastCupNames[index];
        }

    }
}