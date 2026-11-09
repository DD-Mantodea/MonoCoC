using MonoCoC.Contents.BodyParts;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Registry;
using MonoCoC.Extensions;
using MonoCoC.Utils;
using System;
using System.Linq;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    [Register<PlayerAppearance>("coc:player_appearance")]
    public class PlayerAppearance : Scene
    {
        public override void Entry()
        {
            ClearOutput();

            ClearButtons();

            DisplayHeader("Scenes.PlayerAppearance.Name".Localized());

            var race = Player.GetRace();

            if (race != Player.StartingRace || Player.Age != Player.StartingAge)
            {
                var started = "";

                if (race != Player.StartingRace)
                    started += Player.StartingRace;

                if (Player.Age != Player.StartingAge)
                    started += " " + ("Scenes.PlayerAppearance.Age." + Player.StartingAge).Localized();

                started = started.Trim();

                WriteLine("Scenes.PlayerAppearance.RaceChange".Localized(AOrAn(started) + " " + started));
            }

            WriteLine("Scenes.PlayerAppearance.Opening".Localized(MeasurementsUtils.FootInchOrMetre(Player.Tallness), Player.MaleFemaleHerm(), race, Player.AgeDesc(), Player.BodyType()));

            var armorDescription = Player.ArmorDescript();

            var worn = armorDescription == "gear"

                ? "naked"

                : "wearing your " + armorDescription;

            WriteLine("Scenes.PlayerAppearance.Wearing".Localized(worn, Player.WeaponName));

            if (Player.FeatheryHairPinEquipped())
                WriteLine(Player.Body.Get<HairPart>().Length > 0

                    ? "Scenes.PlayerAppearance.HairPinInHair".Localized()

                    : "Scenes.PlayerAppearance.HairPinOnHead".Localized());

            if (Player.JewelryName != "nothing")
                WriteLine("Scenes.PlayerAppearance.Jewelry".Localized(Player.Jewelry.LongName));

            WriteFace();

            WriteEyes();

            WriteHairAndEars();

            WriteTongue();

            WriteHorns();

            WriteNeck();

            WriteBody();

            WriteWings();

            WriteAss();

            WriteTail();

            WritePregnancy();

            WriteChest();

            WriteCock();

            var earsPart = Player.Body.Get<EarsPart>();

            if (earsPart.EarsPierced > 0)
                WriteLine("Scenes.PlayerAppearance.EarsPierced".Localized(
                    Player.EarsPShort != "" ? Player.EarsPShort : "Scenes.PlayerAppearance.Piercing".Localized()));

            if (Player.BreastRows.Count > 0 && Player.BreastRows[0].NipplesPierced > 0)
                WriteLine("Scenes.PlayerAppearance.NipplesPierced".Localized(
                    "Scenes.PlayerAppearance.Nipples".Localized(),

                    Player.NipplesPShort != "" ? Player.NipplesPShort : "Scenes.PlayerAppearance.Piercing".Localized()));

            SetExitButton("Common.Back".Localized(), () => Go<CampScene>());
        }

        public static void WriteHorns()
        {
            var horns = Player.Body.Get<HornsPart>();

            var type = BodyPartSlotRegistry.Get(horns.SlotId).Types.Get(horns.Id).GetString("appearance");

            if (type == "")
                return;

            var value = horns.Value;

            switch (type)
            {
                case "HornsDemon":
                    if (value == 2)
                        Key("HornsDemon2");
                    else if (value == 4)
                        Key("HornsDemon4");
                    else if (value == 6)
                        Key("HornsDemon6", MeasurementsUtils.NumInchesOrCentimetres(8));
                    else if (value >= 8)
                        Key("HornsDemon8", MeasurementsUtils.NumInchesOrCentimetres(10), NumberUtils.Num2Text((int)value));

                    break;

                case "HornsMinotaur":
                    if (value >= 20)
                        Key("HornsMinotaur20");
                    else if (value >= 12)
                        Key("HornsMinotaur12");
                    else if (value >= 6)
                        Key("HornsMinotaur6");
                    else if (value >= 3)
                        Key("HornsMinotaur3");
                    else
                        Key("HornsMinotaur0");

                    break;

                case "HornsDraconicX2":
                    if (value > 0)
                        Key("HornsDraconicX2", MeasurementsUtils.InchOrCentimetre(value));

                    break;

                case "HornsAntlers":
                    if (value > 0)
                        Key("HornsAntlers", NumberUtils.Num2Text((int)value));

                    break;

                case "HornsSheep":
                    Key(value == 1 ? "HornsSheepSmall" : "HornsSheepLarge");

                    break;

                case "HornsRam":
                    Key(value == 1 ? "HornsRamSmall" : "HornsRamLarge", (int)value);

                    break;

                case "HornsGoat":
                    Key(value == 1 ? "HornsGoatSmall" : "HornsGoatLarge");

                    break;

                case "HornsRhino":
                    if (value >= 2)
                    {
                        var face = Player.Body.Get<FacePart>();

                        var hornStyle = BodyPartSlotRegistry.Get(face.SlotId).Types.Get(face.Id).GetString("hornStyle");

                        if (hornStyle == "rhino")
                            Key("HornsRhinoSecond", MeasurementsUtils.NumInchesOrCentimetres(7));
                        else
                            Key("HornsRhino", MeasurementsUtils.NumInchesOrCentimetres(7));

            Key("FaceLong", Player.FaceDesc());
                    }
                    else
                        Key("HornsRhino", MeasurementsUtils.NumInchesOrCentimetres(6));

                    break;

                case "HornsUnicorn":
                    if (value < 12)
                        Key("HornsUnicornShort", MeasurementsUtils.NumInchesOrCentimetres(6));
                    else
                        Key("HornsUnicornLong", MeasurementsUtils.NumInchesOrCentimetres(12), MeasurementsUtils.NumInchesOrCentimetres(2));

                    break;

                case "HornsWooden":
                    Key(value == 0 ? "HornsWoodenOld" : "HornsWooden");

                    break;

                default:
                    if (("Scenes.PlayerAppearance." + type).LocalizedOr("") == "")
                        Key("HornsUnknown");
                    else
                        Key(type);

                    break;
            }
        }

        public static void WriteNeck()
        {
            var neck = Player.Body.Get<NeckPart>();

            var type = BodyPartSlotRegistry.Get(neck.SlotId).Types.Get(neck.Id).GetString("appearance");

            if (type == "")
                return;

            if (type == "NeckDraconic")
            {
                var lengthKey = neck.Len < 8 ? "NeckDraconicFew" : neck.Len < 13 ? "NeckDraconicSomewhat" : neck.Len < 18 ? "NeckDraconicVery" : "NeckDraconicExtremely";

                Key("NeckDraconic", ("Scenes.PlayerAppearance." + lengthKey).Localized(), MathF.Round(neck.Len));

                Key(neck.Len < 10 ? "NeckBendShort" : neck.Len < 16 ? "NeckBendMedium" : "NeckBendLong");

                return;
            }

            if (type == "NeckCockatrice")
            {
                var color = neck.Color == "" ? "" : ("BodyParts.Color." + neck.Color.PascalKey()).LocalizedOr(neck.Color);

                Key("NeckCockatrice", color);
            }
        }

        public static void WriteHairAndEars()
        {
            var hair = Player.Body.Get<HairPart>();

            var ears = Player.Body.Get<EarsPart>();

            var earType = BodyPartSlotRegistry.Get(ears.SlotId).Types.Get(ears.Id).GetString("ear");

            if (hair.Length == 0)
            {
                var skin = Player.Body.Get<SkinPart>();

                if (skin.Id.Path == "fur")
                    Key("HairBaldFur");
                else if (skin.Id.Path == "wool")
                    Key("HairBaldWool");
                else
                    Key("HairBald");

                if (earType != "" && earType != "Human")
                    Key("EarsBald" + earType);
            }
            else if (earType != "")
                Key("EarsHair" + earType);

            if (ears.Id.Path == "cockatrice")
                Key("EarsCockatrice");

            var gills = Player.Body.Get<GillsPart>();

            if (hair.Length > 0 && gills.Id.Path is "fish" or "mermaid")
                Key("GillsNeck");

            var antennae = Player.Body.Get<AntennaePart>();

            if (antennae.Id.Path == "bee")
            {
                if (hair.Length == 0)
                    Key("AntennaeBeeBald");
                else if (ears.Id.Path == "bunny")
                    Key("AntennaeBeeBunny");
                else
                    Key("AntennaeBeeHair");
            }
            else if (antennae.Id.Path == "cockatrice")
                Key("AntennaeCockatrice");

            var beard = Player.Body.Get<BeardPart>();

            if (beard.Length > 0)
                Key("BeardCover", Player.BeardDescript());
        }

        public static void WriteBody()
        {
            Key("BodyHumanoid");

            var underBody = Player.Body.Get<UnderBodyPart>();

            var skin = Player.Body.Get<SkinPart>();

            if (underBody.Id.Path != "none" && underBody.Skin.Id != skin.Id)
                Key("BodyUnderBody");

            var arms = Player.Body.Get<ArmsPart>();

            var armsAppearance = BodyPartSlotRegistry.Get(arms.SlotId).Types.Get(arms.Id).GetString("appearance");

            if (armsAppearance != "")
                Key(armsAppearance);

            var rear = Player.Body.Get<RearBodyPart>();

            var rearAppearance = BodyPartSlotRegistry.Get(rear.SlotId).Types.Get(rear.Id).GetString("appearance");

            if (rearAppearance != "")
                Key(rearAppearance);

            if (!Player.IsTaur())
            {
                var hips = Player.Thickness > 70 ? "HipsFat" : Player.Thickness < 30 ? "HipsThin" : "HipsStd";

                Key(hips, Player.HipDescript());
            }

            var legs = Player.Body.Get<LowerBodyPart>();

            var legType = BodyPartSlotRegistry.Get(legs.SlotId).Types.Get(legs.Id);

            if (legType.GetString("body") == "taur")
            {
                var taurLegs = legType.GetString("taurLegs");

                Key(taurLegs != "" ? taurLegs : "TaurLegsHoofed");
            }
            else
            {
                var legsAppearance = legType.GetString("appearance");

                if (legsAppearance != "")
                    Key(legsAppearance);
            }
        }

        public static void WriteWings()
        {
            var wings = Player.Body.Get<WingsPart>();

            var appearance = BodyPartSlotRegistry.Get(wings.SlotId).Types.Get(wings.Id).GetString("appearance");

            if (appearance != "")
                Key(appearance);
        }

        public static void WriteAss()
        {
            var prefix = Player.Tone < 60 ? "AssFat" : "AssFit";

            var rating = Player.Body.Butt;

            var bracket = rating < 4 ? "0" : rating < 6 ? "4" : rating < 10 ? "6" : rating < 15 ? "10" : rating < 20 ? "15" : "20";

            Key(prefix + bracket);
        }

        public static void WriteTail()
        {
            var tail = Player.Body.Get<TailPart>();

            var appearance = BodyPartSlotRegistry.Get(tail.SlotId).Types.Get(tail.Id).GetString("appearance");

            if (appearance != "")
                if (appearance == "TailFox" && tail.Venom > 1)
                    Key("TailFoxMulti", NumberUtils.Num2Text((int)tail.Venom));
                else
                    Key(appearance);

            if (tail.Id.Path == "spider_abdomen")
                if (tail.Venom > 50 && tail.Venom < 80)
                    Key("TailSpiderFull");
                else if (tail.Venom >= 80 && tail.Venom < 100)
                    Key("TailSpiderVeryFull");
                else if (tail.Venom >= 100)
                    Key("TailSpiderBursting");

            if (tail.Id.Path == "bee_abdomen")
                if (tail.Venom > 50 && tail.Venom < 80)
                    Key("TailBeeDrop");
                else if (tail.Venom >= 80 && tail.Venom < 100)
                    Key("TailBeeCoated");
                else if (tail.Venom >= 100)
                    Key("TailBeeDrips");

            var legs = Player.Body.Get<LowerBodyPart>();

            var legType = BodyPartSlotRegistry.Get(legs.SlotId).Types.Get(legs.Id);

            var bodyText = legType.GetString("bodyText");

            if (bodyText != "")
                Key(bodyText);

            var gills = Player.Body.Get<GillsPart>();

            if (gills.Id.Path == "anemone")
                Key("GillsAnemone");

            if (legs.Id.Path is "coc:naga" or "mermaid" && Player.Gender > 0)
                Key("TailConceal");
        }

        public static void WriteCock()
        {
            if (Player.Cocks.Count == 0)
                return;

            if (Player.Cocks.Count > 1)
            {
                var legType = BodyPartSlotRegistry.Get(Player.Body.Get<LowerBodyPart>().SlotId).Types.Get(Player.Body.Get<LowerBodyPart>().Id);

                var multiKey = Player.IsTaur() && legType.GetString("taurCockMulti") != "" ? legType.GetString("taurCockMulti") : "CockIntroMulti";

                Key(multiKey, Player.MultiCockDescript());
            }
            else if (Player.IsTaur())
            {
                var legType = BodyPartSlotRegistry.Get(Player.Body.Get<LowerBodyPart>().SlotId).Types.Get(Player.Body.Get<LowerBodyPart>().Id);

                Key(legType.GetString("taurCock") != "" ? legType.GetString("taurCock") : "CockTaurSingle");
            }

            for (var index = 0; index < Player.Cocks.Count; index++)
            {
                var cock = Player.Cocks[index];

                var type = BodyPartSlotRegistry.Get(cock.SlotId).Types.Get(cock.Id).GetString("type");

                Key("CockLine", Player.CockDescript(index), MeasurementsUtils.InchesOrCentimetres(cock.Length), MeasurementsUtils.InchesOrCentimetres(cock.Thickness));

                if (("Scenes.PlayerAppearance.CockType" + type).LocalizedOr("") != "")
                    Key("CockType" + type);

                if (cock.KnotMultiplier > 1f)
                    Key("CockKnot", MeasurementsUtils.InchesOrCentimetres(cock.Thickness * cock.KnotMultiplier));

                if (cock.Sock != "" && ("Scenes.PlayerAppearance.CockSock" + cock.Sock).LocalizedOr("") != "")
                    Key("CockSock" + cock.Sock);
            }

            if (Player.Balls > 0)
                Key("CockBalls", Player.BallsDescript());

            if (Player.Vaginas.Count > 0)
                Key("CockVagina", Player.VaginaDescript(), Player.ClitDescript());

            Key("CockAsshole", Player.AssholeDescript());
        }

        public static void WriteChest()
        {
            var rows = Player.BreastRows;

            if (rows.Count == 0)
                return;

            if (rows.Count == 1)
            {
                var row = rows[0];

                var nipples = row.NipplesPerBreast;

                Key("ChestSingle", NumberUtils.Num2Text(row.Breasts), Player.BreastDescript(0),

                    nipples == 1 ? "a" : NumberUtils.Num2Text((int)nipples),

                    MeasurementsUtils.ShortSuffix(Player.NippleLength), Player.NippleDescript(0), nipples == 1 ? "" : "s");

                if (row.BreastRating >= 1)
                    Key("ChestBra", Player.BreastCup(0));

                if (row.MilkFullness > 75)
                    Key("ChestMilk", Player.BreastDescript(0));

                return;
            }

            Key("ChestRowsIntro", NumberUtils.Num2Text(rows.Count));

            for (var index = 0; index < rows.Count; index++)
            {
                var row = rows[index];

                var nipples = row.NipplesPerBreast;

                var prefix = index < 5 ? KeyText("ChestRow" + (index + 1)) : KeyText("ChestRowGeneric");

                Key("ChestRowFormat", prefix, NumberUtils.Num2Text(row.Breasts), Player.BreastDescript(index),

                    NumberUtils.Num2Text((int)nipples), MeasurementsUtils.ShortSuffix(Player.NippleLength),

                    Player.NippleDescript(index), nipples == 1 ? "" : "s");

                if (row.BreastRating >= 1)
                    Key("ChestBra", Player.BreastCup(index));

                if (row.MilkFullness > 75)
                    Key("ChestMilk", Player.BreastDescript(index));
            }
        }

        public static void WritePregnancy()
        {
            if (Player.HasStatusEffect(new Identifier("coc", "goo_stuffed")))
                Key("Pregnancy.GooStuffed");

            if (Player.ButtPregnancy.IsActive)
                WritePregnancyState(Player.ButtPregnancy, true);

            if (Player.Pregnancy.IsActive)
                WritePregnancyState(Player.Pregnancy, false);

            var eel = Player.StatusEffectByType(new Identifier("coc", "parasite_eel"));

            if (eel != null)
            {
                var count = (int)eel.Values[0];

                Key(count <= 1 ? "Parasites.Eel.Single" : count == 2 ? "Parasites.Eel.Plural" : "Parasites.Eel.Several");

                Key("Parasites.Eel.Leak");
            }

            var nephila = Player.StatusEffectByType(new Identifier("coc", "parasite_nephila"));

            if (nephila != null)
            {
                var value = (int)nephila.Values[0];

                var tier = value <= 1 ? 1 : value == 2 ? 2 : value <= 3 ? 3 : value <= 5 ? 4 : value <= 9 ? 5 : value <= 14 ? 6 : value <= 19 ? 7 : 8;

                Key("Parasites.Nephila." + tier);

                Key("Parasites.Nephila.Leak");
            }

            if (Player.HasPerk(PerkIds.NephilaArchQueen))
                Key("Pregnancy.Nephila.ArchQueen");
        }

        public static void WritePregnancyState(PregnancyState state, bool butt)
        {
            var type = PregnancyTypeRegistry.Get(state.Type);

            var special = type?.Special ?? "";

            switch (special)
            {
                case "eggs":
                    WriteEggPregnancy(state);

                    return;

                case "faerie":
                    WriteFaeriePregnancy(state);

                    return;

                case "urta":
                    WriteUrtaPregnancy(state);

                    return;

                case "satyr":
                    WriteSatyrPregnancy(state);

                    return;

                case "frog":
                    WriteFrogPregnancy(state);

                    return;
            }

            if (butt)

                return; // （上面已按 type 分派），其余类型原版不输出任何描述，因此这里不产生文本。 // 原版 PlayerAppearance.hx:1286-1345 对臀部孕期只处理 FROG_GIRL 与 SATYR 两种

            var incubation = state.Incubation;

            if (incubation > 336)
                Key("Pregnancy.Early");
            else if (incubation > 280)
                Key("Pregnancy.Bulge");
            else if (incubation > 216)
                Key("Pregnancy.Noticeable");
            else if (incubation > 180)
                Key("Pregnancy.Obvious");
            else if (incubation > 120)
                Key("Pregnancy.Large");
            else if (incubation > 72)
                Key("Pregnancy.Distended");
            else if (incubation > 48)
                Key("Pregnancy.Distended");
            else
                Key(type != null && type.FullTermKey != "" ? type.FullTermKey : "Pregnancy.Native.FullTerm");
        }

        public static void WriteEggPregnancy(PregnancyState state)
        {
            var count = state.Count;

            if ((state.Incubation > 50) && (state.Incubation > 50))
                    return;

                if (state.Incubation > 20)
                Key("Pregnancy.Eggs.SymbolSize", KeyText(count < 10 ? "Pregnancy.Eggs.Basketball" : count < 20 ? "Pregnancy.Eggs.Watermelon" : "Pregnancy.Eggs.BeachBall"));
            else
                Key("Pregnancy.Eggs.SymbolSize", KeyText(count < 10 ? "Pregnancy.Eggs.Watermelon" : count < 20 ? "Pregnancy.Eggs.BeachBall" : "Pregnancy.Eggs.LargeMedicineBall"));
        }

        public static void WriteFaeriePregnancy(PregnancyState state)
        {
            var suffix = state.Incubation <= 100 ? "Pregnancy.Faerie.Liquid" : state.Incubation <= 140 ? "Pregnancy.Faerie.Syrup" : "Pregnancy.Faerie.Solid";

            Key("Pregnancy.Faerie.Belly", KeyText(suffix));
        }

        public static void WriteUrtaPregnancy(PregnancyState state)
        {
            var incubation = state.Incubation;

            if (incubation > 432)
                Key("Pregnancy.Urta.Larger");
            else if (incubation > 360)
                Key("Pregnancy.Urta.Bulge");
            else if (incubation > 288)
                Key("Pregnancy.Urta.Kicking");
            else if (incubation > 216)
                Key("Pregnancy.Urta.Heavy");
            else if (incubation > 144)
                Key("Pregnancy.Urta.Round");
            else if (incubation > 72)
                Key("Pregnancy.Urta.Distended");
            else
                Key("Pregnancy.Urta.FullTerm");
        }

        public static void WriteSatyrPregnancy(PregnancyState state)
        {
            var incubation = state.Incubation;

            if (incubation >= 75 && incubation < 125)
                Key("Pregnancy.Satyr.PotBelly");
            else if (incubation >= 50)
                Key("Pregnancy.Satyr.Bulge");
            else if (incubation >= 30)
                Key("Pregnancy.Satyr.Painful");
            else
                Key("Pregnancy.Native.FullTerm");
        }

        public static void WriteFrogPregnancy(PregnancyState state)
        {
            if (state.Incubation >= 8)
                Key("Pregnancy.Frog.Waddle");
            else
                Key("Pregnancy.Frog.Full");
        }

        public static string KeyText(string key) => ("Scenes.PlayerAppearance." + key).Localized();

        public static void Key(string key, params object[] args)
        {
            WriteLine(("Scenes.PlayerAppearance." + key).Localized(args));
        }

        public static void WriteEyes()
        {
            var eyes = Player.Body.Get<EyesPart>();

            var type = BodyPartSlotRegistry.Get(eyes.SlotId).Types.Get(eyes.Id);

            WriteAppearance(type.GetString("appearance"));

            var prefix = type.GetString("eyePrefix");

            if (prefix != "")
                WriteLine("Scenes.PlayerAppearance.EyesReptile".Localized(("Scenes.PlayerAppearance." + prefix).Localized(), "Scenes.PlayerAppearance.EyesReptileSuffix".Localized()));

            WriteAppearance(type.GetString("eyeExtra"));

            if (eyes.Count > 2)
            {
                var count = eyes.Count - 2;

                var number = ("Scenes.PlayerAppearance.Number." + count).LocalizedOr(count.ToString());

                var noun = (count == 1 ? "Scenes.PlayerAppearance.Eye" : "Scenes.PlayerAppearance.Eyes").Localized();

                WriteLine("Scenes.PlayerAppearance.EyesExtra".Localized(number, noun));
            }
        }

        public static void WriteTongue()
        {
            var tongue = Player.Body.Get<TonguePart>();

            WriteAppearance(BodyPartSlotRegistry.Get(tongue.SlotId).Types.Get(tongue.Id).GetString("appearance"));
        }

        public static void WriteFace()
        {
            var face = Player.Body.Get<FacePart>();

            var skin = Player.Body.Get<SkinPart>();

            var faceType = BodyPartSlotRegistry.Get(face.SlotId).Types.Get(face.Id);

            var appearance = faceType.Appearance(Player.SkinKind());

            if (appearance == "")
                WriteAppearance(BodyPartSlotRegistry.Get(skin.SlotId).Types.Get(skin.Id).GetString("appearance"));

            WriteAppearance(faceType.GetString("extra"));

            WriteAppearance(appearance);

            Key("FaceLong", Player.FaceDesc());
        }

        public static void WriteAppearance(string suffix)
        {
            if (string.IsNullOrEmpty(suffix))
                return;

            WriteLine(("Scenes.PlayerAppearance." + suffix).Localized());
        }

        public static string AOrAn(string word)
        {
            var prefixes = new[] { "alicorn", "equi", "avian", "echidna", "coc:imp", "adult", "elder" };

            return prefixes.Any(prefix => word.StartsWith(prefix)) ? "an" : "a";
        }

        public static void Line(string label, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            WriteLine("Scenes.PlayerAppearance.Line".Localized(("Scenes.PlayerAppearance.Label." + label).Localized(), value));
        }

        public static void WriteLine(string text)
        {
            OutputText(text);

            Output.Append("\n");
        }
    }
}