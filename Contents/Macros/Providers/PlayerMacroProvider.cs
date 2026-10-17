using MonoCoC.Contents.Creatures;
using MonoCoC.Attributes;
using MonoCoC.Utils;
using MonoCoC.Extensions;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Scripting;
using MonoCoC.UI.Components.Views;

namespace MonoCoC.Macros.Providers
{

    [Register<PlayerMacroProvider>("coc:player")]
    public class PlayerMacroProvider : IMacroProvider
    {

        public int Priority => 0;

        public bool TryResolve(string name, out ScriptValue value)
        {
            value = default;

            var player = CoC.Instance?.Player;

            var time = GameView.Instance?.Time;

            if (player == null)
                return false;

            var indexed = Indexed(name, player);

            if (indexed != null)
            {
                value = indexed.Value;

                return true;
            }

            var core = Core(name, player, time);

            if (core != null)
            {
                value = core.Value;

                return true;
            }

            var extra = Extra(name, player, time);

            if (extra != null)
            {
                value = extra.Value;

                return true;
            }

            return false;
        }

        public static ScriptValue? Indexed(string name, Player player)
        {
            var space = name.IndexOf(' ');

            if (space > 0 && int.TryParse(name[(space + 1)..].Trim(), out var rawIndex))
            {
                var head = name[..space].ToLowerInvariant();

                var index = rawIndex - 1;

                var valid = index >= 0 && index < player.Cocks.Count;

                switch (head)
                {
                    case "cock":
                    case "dick":
                        return valid ? ScriptValue.FromString(player.CockDescript(index)) : Scene.InvalidCockIndex(rawIndex);

                    case "cockhead":
                        return valid ? ScriptValue.FromString(player.CockHead(index)) : Scene.InvalidCockIndex(rawIndex);

                    case "knotword":
                        return valid ? ScriptValue.FromString(player.KnotWord(index)) : Scene.InvalidCockIndex(rawIndex);

                    case "cockanimalityadj":
                        return valid ? ScriptValue.FromString(player.CockAnimalityAdj(index)) : ScriptValue.FromString("");
                }
            }

            return null;
        }

        public static ScriptValue? Core(string name, Player player, Contents.World.Time? time) => name.ToLowerInvariant() switch
        {
            "ischild" => (ScriptValue?)ScriptValue.FromBool(player.IsChild()),

            "isteen" => (ScriptValue?)ScriptValue.FromBool(player.IsTeen()),

            "isadult" => (ScriptValue?)ScriptValue.FromBool(player.IsAdult()),

            "iselder" => (ScriptValue?)ScriptValue.FromBool(player.IsElder()),

            "ismale" => (ScriptValue?)ScriptValue.FromBool(player.IsMale()),

            "isfemale" => (ScriptValue?)ScriptValue.FromBool(player.IsFemale()),

            "isherm" => (ScriptValue?)ScriptValue.FromBool(player.IsHerm()),

            "isgenderless" => (ScriptValue?)ScriptValue.FromBool(player.IsGenderless()),

            "name" => (ScriptValue?)ScriptValue.FromString(player.ShortName),

            "hair" => (ScriptValue?)ScriptValue.FromString(player.HairDescript()),

            "haircolor" => (ScriptValue?)ScriptValue.FromString(player.Body.Get<HairPart>().Color),

            "skin" => (ScriptValue?)ScriptValue.FromString(player.SkinDescript()),

            "skin.noadj" => (ScriptValue?)ScriptValue.FromString(player.SkinDescript(true)),

            "skinshort" => (ScriptValue?)ScriptValue.FromString(player.SkinDescript(true, true)),

            "skindesc" => (ScriptValue?)ScriptValue.FromString(player.Body.Get<SkinPart>().Desc),

            "skintone" => (ScriptValue?)ScriptValue.FromString(player.Body.Get<SkinPart>().Tone),

            "furcolor" => (ScriptValue?)ScriptValue.FromString(player.Body.Get<SkinPart>().FurColor),

            "skinfurscales" => (ScriptValue?)ScriptValue.FromString(player.SkinFurScales()),

            "underbody.skinfurscales" => (ScriptValue?)ScriptValue.FromString(player.Body.Get<UnderBodyPart>().Skin.SkinFurScales()),

            "underbody.skintone" => (ScriptValue?)ScriptValue.FromString(player.Body.Get<UnderBodyPart>().Skin.Tone),

            "underbody.furcolor" => (ScriptValue?)ScriptValue.FromString(player.Body.Get<UnderBodyPart>().Skin.FurColor),
            // 原版 Wings.hx:getColorDesc()：龙翼主色叫 membranes、第二色叫 bones
            "wingcolordesc" => (ScriptValue?)ScriptValue.FromString(WingColorDesc(player, false)),

            "wingcolor2desc" => (ScriptValue?)ScriptValue.FromString(WingColorDesc(player, true)),

            "face" => (ScriptValue?)ScriptValue.FromString(player.FaceDescript()),

            "eyes" => (ScriptValue?)ScriptValue.FromString(player.EyesDescript()),

            "eyecount" => (ScriptValue?)ScriptValue.FromString(player.Body.Get<EyesPart>().Count.ToString()),

            "tongue" => (ScriptValue?)ScriptValue.FromString(player.TongueDescript()),

            "tail" => (ScriptValue?)ScriptValue.FromString(player.TailDescript()),

            "wings" => (ScriptValue?)ScriptValue.FromString(player.WingsDescript()),

            "wingcolor" => (ScriptValue?)ScriptValue.FromString(player.Body.Get<WingsPart>().Color),

            "wingcolor2" => (ScriptValue?)ScriptValue.FromString(player.Body.Get<WingsPart>().Color2),

            "horns" => (ScriptValue?)ScriptValue.FromString(player.HornsDescript()),

            "neck" => (ScriptValue?)ScriptValue.FromString(player.NeckDescript()),

            "neckcolor" => (ScriptValue?)ScriptValue.FromString(player.Body.Get<NeckPart>().Color),

            "rearbody" => (ScriptValue?)ScriptValue.FromString(player.RearBodyDescript()),

            "rearbodycolor" => (ScriptValue?)ScriptValue.FromString(player.Body.Get<RearBodyPart>().Color),

            "claws" => (ScriptValue?)ScriptValue.FromString(player.ClawsDescript()),

            "claw" => (ScriptValue?)ScriptValue.FromString(player.ClawsDescript(false)),

            "legs" => (ScriptValue?)ScriptValue.FromString(player.Legs()),

            "beard" => (ScriptValue?)ScriptValue.FromString(player.BeardDescript()),

            "breasts" => (ScriptValue?)ScriptValue.FromString(player.BreastDescript(0)),

            "tits" => (ScriptValue?)ScriptValue.FromString(player.BreastDescript(0)),

            "lasttits" => (ScriptValue?)ScriptValue.FromString(player.BreastDescript(player.BreastRows.Count - 1)),

            "breastcup" => (ScriptValue?)ScriptValue.FromString(player.BreastCup(0)),

            "lastbreastcup" => (ScriptValue?)ScriptValue.FromString(player.BreastCup(player.BreastRows.Count - 1)),

            "chest" => (ScriptValue?)ScriptValue.FromString(player.ChestDesc()),

            "fullchest" => (ScriptValue?)ScriptValue.FromString(player.AllChestDesc()),

            "ass" => (ScriptValue?)ScriptValue.FromString(player.ButtDescript()),

            "butt" => (ScriptValue?)ScriptValue.FromString(player.ButtDescript()),

            "hips" => (ScriptValue?)ScriptValue.FromString(player.HipDescript()),

            "clit" => (ScriptValue?)ScriptValue.FromString(player.Vaginas.Count > 0 ? player.ClitDescript() : ""),

            "vagina" => (ScriptValue?)ScriptValue.FromString(player.Vaginas.Count > 0 ? player.VaginaDescript() : ""),

            "vagorass" => (ScriptValue?)ScriptValue.FromString(player.Vaginas.Count > 0 ? player.VaginaDescript() : player.AssholeDescript()),

            "genitals" => (ScriptValue?)ScriptValue.FromString(player.Gender switch
            {
                Gender.Genderless => "anus",

                Gender.Male => player.Cocks.Count > 1 ? "cocks" : "cock",

                Gender.Female => "pussy",

                Gender.Hermaphrodite => "cock and pussy",

                _ => ""
            }),

            "genitalsdetail" => (ScriptValue?)ScriptValue.FromString(player.Gender switch
            {
                Gender.Genderless => player.AssholeDescript(),

                Gender.Male => player.Cocks.Count > 1 ? player.MultiCockDescript() : player.CockDescript(),

                Gender.Female => player.VaginaDescript(),

                Gender.Hermaphrodite => player.CockDescript() + " and " + player.VaginaDescript(),

                _ => ""
            }),

            "cumnormal" => (ScriptValue?)ScriptValue.FromBool(player.CumQ() > 50 && player.CumQ() <= 150),

            "haslegs" => (ScriptValue?)ScriptValue.FromBool(player.Body.Get<LowerBodyPart>().LegCount > 1),

            "corruption" => (ScriptValue?)ScriptValue.FromFloat(player.Corruption),

            "vaginalwetness" => (ScriptValue?)ScriptValue.FromFloat(player.Vaginas.Count > 0 ? player.Vaginas[0].Wetness : 0),                "pussy" => (ScriptValue?)ScriptValue.FromString(player.Vaginas.Count > 0 ? player.VaginaDescript() : ""),

            "cunt" => (ScriptValue?)ScriptValue.FromString(player.Vaginas.Count > 0 ? player.VaginaDescript() : ""),

            "vag" => (ScriptValue?)ScriptValue.FromString(player.Vaginas.Count > 0 ? player.VaginaDescript() : ""),

            "wet" => (ScriptValue?)ScriptValue.FromString(player.Vaginas.Count > 0 ? player.WetnessDescript() : ""),

            "balls" => (ScriptValue?)ScriptValue.FromString(player.BallsDescript()),

            "sack" => (ScriptValue?)ScriptValue.FromString(player.SackDescript()),

            "cock" => (ScriptValue?)ScriptValue.FromString(player.Cocks.Count > 0 ? player.CockDescript() : ""),

            "cocks" => (ScriptValue?)ScriptValue.FromString(player.Cocks.Count > 0 ? Appearance.CockNouns(player, 0) : ""),

            "cockhead" => (ScriptValue?)ScriptValue.FromString(player.Cocks.Count > 0 ? player.CockHead() : ""),

            "multicock" => (ScriptValue?)ScriptValue.FromString(player.Cocks.Count > 0 ? player.MultiCockDescript() : ""),

            "nipple" => (ScriptValue?)ScriptValue.FromString(player.BreastRows.Count > 0 ? player.NippleDescript() : ""),

            "nipples" => (ScriptValue?)ScriptValue.FromString(player.BreastRows.Count > 0 ? player.Nipples() : ""),

            "asshole" => (ScriptValue?)ScriptValue.FromString(player.AssholeDescript()),

            "hairshort" => (ScriptValue?)ScriptValue.FromString(player.HairShort()),

            "hairorfur" => (ScriptValue?)ScriptValue.FromString(player.HairOrFur()),

            "hairorfurcolor" => (ScriptValue?)ScriptValue.FromString(player.HairOrFurColor()),

            "leg" => (ScriptValue?)ScriptValue.FromString(player.Leg()),

            "feet" => (ScriptValue?)ScriptValue.FromString(player.Feet()),

            "foot" => (ScriptValue?)ScriptValue.FromString(player.Foot()),

            "sheath" => (ScriptValue?)ScriptValue.FromString(player.SheathDescript()),

            "ears" => (ScriptValue?)ScriptValue.FromString(player.EarsDescript()),

            "ear" => (ScriptValue?)ScriptValue.FromString(player.EarsDescript()),

            "armor" => (ScriptValue?)ScriptValue.FromString(player.Armor.Name),

            "hasweapon" => (ScriptValue?)ScriptValue.FromBool(!player.IsUnarmed()),

            "istaur" => (ScriptValue?)ScriptValue.FromBool(player.IsTaur()),

            "hand" => (ScriptValue?)ScriptValue.FromString("hand"),

            "walk" => (ScriptValue?)ScriptValue.FromString(player.IsNaga() ? "slither" : "walk"),

            "clitdesc" => (ScriptValue?)ScriptValue.FromString(player.Vaginas.Count > 0 ? player.ClitDescript() : ""),

            "cock smallest" => (ScriptValue?)ScriptValue.FromString(player.Cocks.Count > 0 ? player.CockDescript(player.SmallestCockIndex()) : ""),

            "cock biggest" => (ScriptValue?)ScriptValue.FromString(player.Cocks.Count > 0 ? player.CockDescript(player.BiggestCockIndex()) : ""),

            "cockhead smallest" => (ScriptValue?)ScriptValue.FromString(player.Cocks.Count > 0 ? player.CockHead(player.SmallestCockIndex()) : ""),

            "cockhead biggest" => (ScriptValue?)ScriptValue.FromString(player.Cocks.Count > 0 ? player.CockHead(player.BiggestCockIndex()) : ""),

            "cocktype smallest" => (ScriptValue?)ScriptValue.FromString(player.Cocks.Count > 0 ? Appearance.CockNoun(player, player.SmallestCockIndex()) : ""),

            "cocktype biggest" => (ScriptValue?)ScriptValue.FromString(player.Cocks.Count > 0 ? Appearance.CockNoun(player, player.BiggestCockIndex()) : ""),

            "hasccock" => (ScriptValue?)ScriptValue.FromBool(player.HasCock()),

            "libido" => (ScriptValue?)ScriptValue.FromFloat(player.Libido),

            "intelligence" => (ScriptValue?)ScriptValue.FromFloat(player.Intelligence),

            "sensitivity" => (ScriptValue?)ScriptValue.FromFloat(player.Sensitivity),

            "strength" => (ScriptValue?)ScriptValue.FromFloat(player.Strength),

            "toughness" => (ScriptValue?)ScriptValue.FromFloat(player.Toughness),

            "speed" => (ScriptValue?)ScriptValue.FromFloat(player.Speed),

            "tone" => (ScriptValue?)ScriptValue.FromString(player.Body.Get<SkinPart>().Tone),

            "isunarmed" => ScriptValue.FromBool(!CoC.Instance.Player.IsUnarmed()),

            "isswimming" => (ScriptValue?)ScriptValue.FromBool(player.Location == "swim"),

            "hasscales" => (ScriptValue?)ScriptValue.FromBool(player.HasScales()),

            "isday" => (ScriptValue?)ScriptValue.FromBool(time?.IsDay ?? false),

            "builtcabin" => (ScriptValue?)ScriptValue.FromBool(CoC.Instance.Flags.Get(GameFlagIds.CampCabinProgress) >= 4),

            "thickness" => (ScriptValue?)ScriptValue.FromFloat(player.Thickness),

            "ballsize" => (ScriptValue?)ScriptValue.FromFloat(player.BallSize),

            "hairlength" => (ScriptValue?)ScriptValue.FromFloat(player.Body.Get<HairPart>().Length),

            "vaginallooseness" => (ScriptValue?)ScriptValue.FromFloat(player.Vaginas.Count > 0 ? player.Vaginas[0].Looseness : 0),

            "analwetness" => (ScriptValue?)ScriptValue.FromFloat(player.Body.Get<AnalPart>().Wetness),

            "anallooseness" => (ScriptValue?)ScriptValue.FromFloat(player.Body.Get<AnalPart>().Looseness),

            "hascock" => (ScriptValue?)ScriptValue.FromBool(player.HasCock()),

            "height" => (ScriptValue?)ScriptValue.FromFloat(player.Tallness),

            "tallness" => (ScriptValue?)ScriptValue.FromFloat(player.Tallness),

            "femininity" => (ScriptValue?)ScriptValue.FromFloat(player.Femininity),

            "isnaga" => (ScriptValue?)ScriptValue.FromBool(player.IsNaga()),

            "tailleg" => (ScriptValue?)ScriptValue.FromBool(player.HasTailInsteadOfLegs()),

            "isbiped" => (ScriptValue?)ScriptValue.FromBool(player.IsBiped()),

            "isdrider" => (ScriptValue?)ScriptValue.FromBool(player.IsDrider()),

            "hasknot" => (ScriptValue?)ScriptValue.FromBool(player.HasKnot()),

            "hashair" => (ScriptValue?)ScriptValue.FromBool(player.Body.Get<HairPart>().Length > 0),

            "hasarmor" => (ScriptValue?)ScriptValue.FromBool(player.Armor.Id.Path != "nothing"),

            "cumhighleast" => (ScriptValue?)ScriptValue.FromBool(player.CumQ() > 350),

            "biggestcocklength" => (ScriptValue?)ScriptValue.FromFloat(
                player.Cocks.Count > 0 ? player.Cocks[player.BiggestCockIndex()].Length : 0),

            "hasvagina" => (ScriptValue?)ScriptValue.FromBool(player.HasVagina()),

            "hasballs" => (ScriptValue?)ScriptValue.FromBool(player.Balls > 0),

            "hasbreasts" => (ScriptValue?)ScriptValue.FromBool(player.BreastRows.Count > 0),

            "islactating" => (ScriptValue?)ScriptValue.FromBool(player.IsLactating()),

            "isnaked" => (ScriptValue?)ScriptValue.FromBool(player.IsNaked()),

            "isnakedlower" => (ScriptValue?)ScriptValue.FromBool(player.IsNakedLower()),

            "isnakedupper" => (ScriptValue?)ScriptValue.FromBool(player.IsNakedUpper()),

            "isunderage" => (ScriptValue?)ScriptValue.FromBool(player.IsChild() || player.IsTeen()),

            "isfeminine" => (ScriptValue?)ScriptValue.FromBool(player.IsFemale() || player.IsHerm()),

            "singleleg" => (ScriptValue?)ScriptValue.FromBool(player.IsNaga()),

            "isgoo" => (ScriptValue?)ScriptValue.FromBool(player.Body.Get<SkinPart>().Id.Path == "goo"),

            "cockcount" => (ScriptValue?)ScriptValue.FromInt(player.Cocks.Count),

            "cocklength" => (ScriptValue?)ScriptValue.FromFloat(player.Cocks.Count > 0 ? player.Cocks[player.BiggestCockIndex()].Length : 0),

            "cockthickness" => (ScriptValue?)ScriptValue.FromFloat(player.Cocks.Count > 0 ? player.Cocks[player.BiggestCockIndex()].Thickness : 0),

            "biggesttitsize" => (ScriptValue?)ScriptValue.FromFloat(player.BiggestBreastSize()),

            "cumquantity" => (ScriptValue?)ScriptValue.FromFloat(player.CumQ()),

            "lib" => (ScriptValue?)ScriptValue.FromFloat(player.Libido),

            "lib100" => (ScriptValue?)ScriptValue.FromFloat(player.Libido),

            "cor" => (ScriptValue?)ScriptValue.FromFloat(player.Corruption),

            "cor100" => (ScriptValue?)ScriptValue.FromFloat(player.Corruption),

            "sens" => (ScriptValue?)ScriptValue.FromFloat(player.Sensitivity),

            "sens100" => (ScriptValue?)ScriptValue.FromFloat(player.Sensitivity),

            "lust" => (ScriptValue?)ScriptValue.FromFloat(player.Lust),

            "LustPercent" => (ScriptValue?)ScriptValue.FromFloat(player.Lust),

            "hp" => (ScriptValue?)ScriptValue.FromFloat(player.Health),

            "hp100" => (ScriptValue?)ScriptValue.FromFloat(player.Health),

            "str" => (ScriptValue?)ScriptValue.FromFloat(player.Strength),

            "tou" => (ScriptValue?)ScriptValue.FromFloat(player.Toughness),

            "spe" => (ScriptValue?)ScriptValue.FromFloat(player.Speed),

            "inte" => (ScriptValue?)ScriptValue.FromFloat(player.Intelligence),

            "fatigue" => (ScriptValue?)ScriptValue.FromFloat(player.Fatigue),

            _ => null
        };

        public static ScriptValue? Extra(string name, Player player, Contents.World.Time? time) => name switch
        {

            "he" => PlayerTextMacros.Str(player.Mf("he", "she")),

            "him" => PlayerTextMacros.Str(player.Mf("him", "her")),

            "his" => PlayerTextMacros.Str(player.Mf("his", "her")),

            "hers" => PlayerTextMacros.Str(player.Mf("his", "hers")),

            "himself" => PlayerTextMacros.Str(player.Mf("himself", "herself")),

            "herself" => PlayerTextMacros.Str(player.Mf("himself", "herself")),

            "he2" => PlayerTextMacros.Str("they"),

            "him2" => PlayerTextMacros.Str("them"),

            "his2" => PlayerTextMacros.Str("their"),

            "boy" => PlayerTextMacros.Str(player.Mf("boy", "girl")),

            "guy" => PlayerTextMacros.Str(player.Mf("guy", "girl")),

            "man" => PlayerTextMacros.Str(player.Mf("man", "woman")),

            "men" => PlayerTextMacros.Str(player.Mf("men", "women")),

            "manboy" => PlayerTextMacros.Str(player.IsChild() || player.IsTeen() ? player.Mf("boy", "girl") : player.Mf("man", "woman")),

            "menboys" => PlayerTextMacros.Str(player.IsChild() || player.IsTeen() ? player.Mf("boys", "girls") : player.Mf("men", "women")),

            "master" => PlayerTextMacros.Str(player.Mf("master", "mistress")),

            "sir" => PlayerTextMacros.Str(player.Mf("sir", "ma'am")),

            "maam" => PlayerTextMacros.Str(player.Mf("sir", "ma'am")),

            "ma'am" => PlayerTextMacros.Str(player.Mf("sir", "ma'am")),

            "madam" => PlayerTextMacros.Str(player.Mf("sir", "madam")),

            "mister" => PlayerTextMacros.Str(player.Mf("mister", "miss")),

            "daddy" => PlayerTextMacros.Str(player.Mf("daddy", "mommy")),

            "dad" => PlayerTextMacros.Str(player.Mf("dad", "mom")),

            "father" => PlayerTextMacros.Str(player.Mf("father", "mother")),

            "son" => PlayerTextMacros.Str(player.Mf("son", "daughter")),

            "boyfriend" => PlayerTextMacros.Str(player.Mf("boyfriend", "girlfriend")),

            "god" => PlayerTextMacros.Str(player.Mf("god", "goddess")),

            "king" => PlayerTextMacros.Str(player.Mf("king", "queen")),

            "lord" => PlayerTextMacros.Str(player.Mf("lord", "lady")),

            "prince" => PlayerTextMacros.Str(player.Mf("prince", "princess")),

            "baron" => PlayerTextMacros.Str(player.Mf("baron", "baroness")),

            "masculine" => PlayerTextMacros.Str(player.Mf("masculine", "feminine")),

            "paternal" => PlayerTextMacros.Str(player.Mf("paternal", "maternal")),

            "malespersons" => PlayerTextMacros.Str(player.Mf("males", "persons")),

            "malefemaleherm" => PlayerTextMacros.Str(player.MaleFemaleHerm()),

            "merfolk" => PlayerTextMacros.Str("mer" + (player.IsGenderless() ? "folk" : player.Mf("man", "maid"))),

            "merfolks" => PlayerTextMacros.Str("mer" + (player.IsGenderless() ? "folk" : player.Mf("men", "maids"))),

            "eachcock" => PlayerTextMacros.Str(player.SMultiCockDesc()),

            "onecock" => PlayerTextMacros.Str(player.SMultiCockDesc()),

            "cocks" => PlayerTextMacros.Str(player.MultiCockDescriptLight()),

            "multicock" => PlayerTextMacros.Str(player.MultiCockDescriptLight()),

            "multicockdescriptlight" => PlayerTextMacros.Str(player.MultiCockDescriptLight()),

            "cock" => PlayerTextMacros.Str(player.CockDescript(0)),

            "dick" => PlayerTextMacros.Str(player.CockDescript(0)),

            "cockshort" => PlayerTextMacros.Str(player.CockDescriptShort(0)),

            "cockhead" => PlayerTextMacros.Str(player.CockHead(0)),

            "cockclit" => PlayerTextMacros.Str(PlayerTextMacros.CockOrClit(player)),

            "lowerbodyskin" => PlayerTextMacros.Str(player.Body.Get<LowerBodyPart>().Skin()),

            "arms" => PlayerTextMacros.Str(player.Body.Get<ArmsPart>().Phrase()),

            "tailnumber" => PlayerTextMacros.Str(NumberUtils.Num2Text(player.Body.Get<TailPart>().Venom)),

            "areaname" => PlayerTextMacros.Str(player.Location),

            "skinis" => PlayerTextMacros.Str(player.HasScales() ? "are" : "is"),

            "weaponsingular" => PlayerTextMacros.Str(player.WeaponName),

            "hands" => PlayerTextMacros.Str(player.HandsDescriptShort()),

            "snowman" => PlayerTextMacros.Str("snowman"),

            "cocktype" => PlayerTextMacros.Str(Appearance.CockNoun(player)),

            "pussy" => PlayerTextMacros.Str(player.VaginaDescript()),

            "cunt" => PlayerTextMacros.Str(player.VaginaDescript()),

            "vag" => PlayerTextMacros.Str(player.VaginaDescript()),

            "asshole" => PlayerTextMacros.Str(player.AssholeDescript()),

            "butthole" => PlayerTextMacros.Str(player.AssholeDescript()),

            "assholeorpussy" => PlayerTextMacros.Str(player.HasVagina() ? player.VaginaDescript() : player.AssholeDescript()),

            "cockorvag" => PlayerTextMacros.Str(player.HasCock() ? player.CockDescript(0) : player.VaginaDescript()),

            "balls" => PlayerTextMacros.Str(player.BallsDescriptLight()),

            "aballs" => PlayerTextMacros.Str(player.BallsDescriptLight()),

            "ballsfull" => PlayerTextMacros.Str(player.BallsDescript()),

            "aballsfull" => PlayerTextMacros.Str(player.BallsDescript()),

            "ballsack" => PlayerTextMacros.Str(player.SackDescript()),

            "sack" => PlayerTextMacros.Str(player.SackDescript()),

            "ballcount" => PlayerTextMacros.Str(NumberUtils.Num2Text(player.Balls)),

            "nipple" => PlayerTextMacros.Str(Appearance.NippleDescript(player, 0)),

            "nipples" => PlayerTextMacros.Str(Appearance.NippleDescript(player, 0) + "s"),

            "sheath" => PlayerTextMacros.Str(player.Cocks.Count > 0 ? Appearance.SheathDescript(player.Cocks[0]) : ""),

            "wet" => PlayerTextMacros.Str(player.WetnessDescript(0)),

            "cockplural" => PlayerTextMacros.Str(player.CockTotal() == 1 ? "cock" : "cocks"),

            "dickplural" => PlayerTextMacros.Str(player.CockTotal() == 1 ? "dick" : "dicks"),

            "headplural" => PlayerTextMacros.Str(player.CockTotal() == 1 ? "head" : "heads"),

            "prickplural" => PlayerTextMacros.Str(player.CockTotal() == 1 ? "prick" : "pricks"),

            "cockhas" => PlayerTextMacros.Str(player.CockTotal() > 1 ? "cocks have" : "cock has"),

            "cockey" => PlayerTextMacros.Str(player.CockTotal() > 1 ? "they" : "it"),

            "cockem" => PlayerTextMacros.Str(player.CockTotal() > 1 ? "them" : "it"),

            "cockeir" => PlayerTextMacros.Str(player.CockTotal() > 1 ? "their" : "its"),

            "genitalis" => PlayerTextMacros.Str(player.Gender switch
            {
                Gender.Genderless => "anus is",

                Gender.Male => player.CockTotal() > 1 ? "cocks are" : "cock is",

                Gender.Female => "pussy is",

                Gender.Hermaphrodite => "cock and pussy are",

                _ => ""
            }),

            "genitaley" => PlayerTextMacros.Str(player.Gender switch
            {
                Gender.Genderless => "it",

                Gender.Male => player.CockTotal() > 1 ? "they" : "it",

                Gender.Female => "it",

                Gender.Hermaphrodite => "they",

                _ => ""
            }),

            "genitalem" => PlayerTextMacros.Str(player.Gender switch
            {
                Gender.Genderless => "it",

                Gender.Male => player.CockTotal() > 1 ? "them" : "it",

                Gender.Female => "it",

                Gender.Hermaphrodite => "them",

                _ => ""
            }),

            "allbreasts" => PlayerTextMacros.Str(player.AllBreastsDescript()),

            "alltits" => PlayerTextMacros.Str(player.AllBreastsDescript()),

            "lastbreasts" => PlayerTextMacros.Str(player.BreastDescript(player.BreastRows.Count - 1)),

            "lasttits" => PlayerTextMacros.Str(player.BreastDescript(player.BreastRows.Count - 1)),

            "lastbreastcup" => PlayerTextMacros.Str(player.BreastCup(player.BreastRows.Count - 1)),

            "breast" => PlayerTextMacros.Str("breast"),

            "facelong" => PlayerTextMacros.Str(player.FaceDescript()),

            "bodytype" => PlayerTextMacros.Str(player.BodyType()),

            "age" => PlayerTextMacros.Str(player.AgeDesc()),

            "race" => PlayerTextMacros.Str(player.Race),

            "gems" => PlayerTextMacros.Str(player.Gems.ToString()),

            "tail" => PlayerTextMacros.Str(player.TailDescript()),

            "walk" => PlayerTextMacros.Str(PlayerTextMacros.WalkWord(player, false)),

            "walking" => PlayerTextMacros.Str(PlayerTextMacros.WalkWord(player, true)),

            "player" => PlayerTextMacros.Str(player.ShortName),

            "weapon" => PlayerTextMacros.Str(player.WeaponName),

            "weaponname" => PlayerTextMacros.Str(player.WeaponName),

            "armorname" => PlayerTextMacros.Str(player.ArmorName),

            "shield" => PlayerTextMacros.Str(player.ShieldName),

            "lowergarment" => PlayerTextMacros.Str(player.LowerGarmentName),

            "uppergarment" => PlayerTextMacros.Str(player.UpperGarmentName),

            "inv" => PlayerTextMacros.Str(player.InventoryName()),

            "inventory" => PlayerTextMacros.Str(player.InventoryName()),

            "pouch" => PlayerTextMacros.Str(player.InventoryName()),

            "pack" => PlayerTextMacros.Str(player.InventoryName()),

            "magic" => PlayerTextMacros.Str(CoC.Instance.Flags.Get(GameFlagIds.MagicSwitch) switch
            {
                0 => "Black & White",

                1 => "Terrestrial Fire",

                _ => "Black & White"
            }),

            "agility" => PlayerTextMacros.Str("[Agility]"),

            "evade" => PlayerTextMacros.Str("[Evade]"),

            "misdirection" => PlayerTextMacros.Str("[Misdirection]"),

            "garg" => PlayerTextMacros.Str(CoC.Instance.Flags.Get(GameFlagIds.GarName)),

            "akky" => PlayerTextMacros.Str(CoC.Instance.Flags.Get(GameFlagIds.AkkyName)),

            "dullhorse" => PlayerTextMacros.Str(CoC.Instance.Flags.Get(GameFlagIds.DullahanHorseName) == 1 ? "Lenore" : "her horse"),

            "amilyclothing" => PlayerTextMacros.Str(string.IsNullOrEmpty(CoC.Instance.Flags.Get(GameFlagIds.AmilyClothing)) ? "rags" : CoC.Instance.Flags.Get(GameFlagIds.AmilyClothing)),

            "aikotailnumber" => PlayerTextMacros.Str(CoC.Instance.Flags.Get(GameFlagIds.AikoBossComplete) > 0 ? "eight" : "seven"),

            "helmspawn" => PlayerTextMacros.Str("Helspawn"),

            "helspawn" => PlayerTextMacros.Str("Helspawn"),

            "helspawneyes" => PlayerTextMacros.Str("emerald"),

            "helspawnscales" => PlayerTextMacros.Str("red"),

            "latexyname" => PlayerTextMacros.Str("latex girl"),

            "bathgirlname" => PlayerTextMacros.Str("milk girl"),

            "cabin" => PlayerTextMacros.Str(CoC.Instance.Flags.Get(GameFlagIds.CampCabinProgress) >= 4 ? "cabin" : "camp"),

            "bed" => PlayerTextMacros.Str("bed"),

            "dummyname" => PlayerTextMacros.Str("training dummy"),

            _ => null
        };

        public static string Cap(string text) =>

            string.IsNullOrEmpty(text) ? text : char.ToUpperInvariant(text[0]) + text[1..];

        /// <summary>原版 Wings.hx:getColorDesc()：龙翼主色=membranes，第二色=bones，其它翅膀没有颜色部位名。</summary>
        public static string WingColorDesc(Player player, bool second)
        {
            var wings = player.Body.Get<WingsPart>();

            if (wings.Id != Contents.Datas.Vanilla.BodyPartTypes.DraconicSmall
                && wings.Id != Contents.Datas.Vanilla.BodyPartTypes.DraconicLarge)

                return "";

            return (second ? "BodyParts.Wings.ColorDesc.Bones" : "BodyParts.Wings.ColorDesc.Membranes").Localized();
        }
    }
}