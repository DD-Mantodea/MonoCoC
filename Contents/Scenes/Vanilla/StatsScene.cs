using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    // 原版 PlayerInfo.hx:146 displayStats 的统计页入口
    [Register<StatsScene>("coc:stats")]
    public class StatsScene : Scene
    {
        private static System.Action? _returnTo;

        public static void Show(System.Action returnTo)
        {
            _returnTo = returnTo;

            Go<StatsScene>();
        }

        internal sealed class Section
        {
            private static string KeyOf(string title) =>

                new(title.Where(char.IsLetterOrDigit).ToArray());

            public static string LocalizeTitle(string title) =>

                ("Stats." + KeyOf(title) + ".Name").LocalizedOr(title);

            public static string LocalizeSection(string title) =>

                ("Scenes.Stats.Section." + KeyOf(title)).LocalizedOr(title);

            private readonly StringBuilder _builder = new();

            public Section(string title)
            {
                _builder.Append("\n<font size=\"24\"><b>")
                    .Append(LocalizeSection(title))
                    .Append("</b></font>\n");
            }

            public void Add(string title, object? value, bool show = true)
            {
                if (show)
                    _builder.Append("<b>").Append(LocalizeTitle(title)).Append(":</b> ")
                        .Append(value).Append('\n');
            }

            public void AddNonZero(string title, object? value)
            {
                if (value is not string && value is IConvertible convertible)
                {
                    var number = convertible.ToDouble(System.Globalization.CultureInfo.InvariantCulture);

                    if (Math.Abs(number) < 0.0001)
                        return;
                }

                Add(title, value);
            }

            public void AddPositive(string title, object? value)
            {
                if (value is not string && value is IConvertible convertible)
                {
                    var number = convertible.ToDouble(System.Globalization.CultureInfo.InvariantCulture);

                    if (number <= 0)
                        return;
                }

                Add(title, value);
            }

            public void Append() => OutputText(_builder.ToString());
        }

        public override void Entry()
        {
            ClearOutput();

            ClearButtons();

            DisplayHeader("Scenes.Stats.Title".Localized());

            var combat = new Section("Combat Stats");

            var attackMultiplier = Player.PhysMod() * Player.Stats.Factor(GameStats.AttackDamage);

            var weaponDamageBonus = Player.Stats.Addition(GameStats.WeaponDamage)

                * Player.Stats.Multiplier(GameStats.WeaponDamage);

            var weaponAttack = MathF.Round(Player.WeaponAttack

                + 2f * Player.Weapon.MasteryLevel(Player)

                + weaponDamageBonus);

            var attackDamage = (int)MathF.Round((weaponAttack + Player.Strength) * attackMultiplier);

            var doubleAttackDamage = (int)MathF.Round((weaponAttack + MathF.Min(Player.Strength, 60)) * attackMultiplier);

            combat.Add("Attack Damage",

                Player.HasPerk(PerkIds.DoubleAttack)

                    ? $"{attackDamage} (Double: {doubleAttackDamage})"

                    : $"{attackDamage}");

            combat.Add("Physical Damage", $"{MathF.Round(100 * Player.PhysMod())}%");

            combat.Add("Critical Chance", $"{MathF.Round(Player.GetBaseCritChance())}%");

            combat.Add("Weapon Critical Chance", $"{MathF.Round(Player.GetMeleeCritBonus())}%");

            combat.Add("Dodge Chance", $"{MathF.Round(Player.GetEvasionChance())}%");

            combat.Add("Parry Chance", $"{MathF.Round(Player.CanParry() ? Player.ParryChance() : 0)}%");

            var armorValue = Player.ArmorDefense;

            var armorPerk = Player.Armor.Perk;

            if (Player.HasPerk(PerkIds.Agility))
            {
                var speedBonus = armorPerk is "Light" or "Adornment" || Player.Armor.Name == "nothing"

                    ? Player.Speed / 8f

                    : armorPerk == "Medium" ? Player.Speed / 13f : 0f;

                armorValue += MathF.Min(15f, MathF.Round(speedBonus));
            }

            if (Player.HasPerk(PerkIds.Juggernaut) && Player.Toughness >= 75 && armorPerk == "Heavy")
                armorValue += 10;

            combat.Add("Armor", armorValue);

            float DamageTaken(bool maxDamage, bool minDamage)
            {
                var mult = 100f;

                var toughness = maxDamage ? 0f : minDamage ? 25f : 0f;

                mult -= toughness;

                if (mult < 75f)
                    mult = 75f;

                mult -= armorValue;

                if (Player.HasPerk(PerkIds.ImmovableObject) && Player.Toughness >= 75)
                    mult *= 0.9f;

                if (Player.StatusEffectValue(StatusEffectIds.BlackCatBeer, 0) > 0)
                    mult *= 0.75f;

                if (mult < 20f)
                    mult = 20f;

                return mult;
            }

            combat.Add("Damage Resistance",

                $"{100 - MathF.Round(DamageTaken(true, false))}-{100 - MathF.Round(DamageTaken(false, true))}% (Higher is better)");

            var teaseBonus = Contents.Items.ArmorItemBonuses.Tease(Player);

            combat.Add("Tease Chance", Player.UpperGarment.Sexiness + Player.LowerGarment.Sexiness + teaseBonus.Chance);

            combat.Add("Tease Damage", Player.Stats.Addition(GameStats.TeaseDamage) + teaseBonus.Damage);

            var valeriaRegen = Player.Armor.Id.Path == "goo_armor"

                ? (CoC.Instance.Flags.Get(GameFlagIds.ValeriaFluids) < 50

                    ? CoC.Instance.Flags.Get(GameFlagIds.ValeriaFluids) / 25f

                    : 2f)

                : 0f;

            var regenPercentCombat = Math.Clamp(Player.Stats.Addition(GameStats.HealthRegenPercent) + valeriaRegen, -5f, 5f);

            var regenPercentNonCombat = Math.Clamp(Player.Stats.Addition(GameStats.HealthRegenPercent) + valeriaRegen * 2f, -10f, 10f);

            var regenFlat = Player.Stats.Addition(GameStats.HealthRegenFlat);

            combat.Add("Health Regen (Combat)",

                $"{MathF.Round(Player.MaxHealth * regenPercentCombat / 100f) + regenFlat} ({regenPercentCombat}% + {regenFlat})");

            combat.Add("Health Regen (Non-combat)",

                $"{MathF.Round(Player.MaxHealth * regenPercentNonCombat / 100f) + regenFlat} ({regenPercentNonCombat}% + {regenFlat})");

            var levels = Player.Level - 1;

            var levelResist = Math.Clamp(levels, 0, 10) * 3f

                + Math.Clamp(levels - 10, 0, 10) * 2f

                + Math.Clamp(levels - 20, 0, 10) * 1f

                + Math.Max(0, levels - 30) * 0.2f;

            var lustPercent = Math.Max(25f, 100f - levelResist);

            lustPercent = Math.Max(25f, lustPercent - Player.Stats.Addition(GameStats.LustResistance));

            lustPercent /= Player.Stats.Factor(GameStats.LustResistance);

            combat.Add("Lust Resistance", $"{100 - MathF.Round(lustPercent)}% (Higher is better)");

            combat.Add("Spell Mod", $"{MathF.Round(100 * Player.SpellMod())}%");

            combat.Add("Spell Cost", $"{Player.SpellCost(100)}%");

            combat.Append();

            var races = new Section("Racial Scores");

            races.AddPositive("Human", (int)Player.HumanScore());

            races.AddPositive("Mutant", (int)Player.MutantScore());

            races.AddPositive("Demon", (int)Player.DemonScore());

            races.AddPositive("Goblin", (int)Player.GoblinScore());

            races.AddPositive("Goo", (int)Player.GooScore());

            races.AddPositive("Cow", (int)Player.CowScore());

            races.AddPositive("Minotaur", (int)Player.MinoScore());

            races.AddPositive("Cat", (int)Player.CatScore());

            races.AddPositive("Lizard", (int)Player.LizardScore());

            races.AddPositive("Salamander", (int)Player.SalamanderScore());

            races.AddPositive("Dragon", (int)Player.DragonScore());

            races.AddPositive("Naga", (int)Player.NagaScore());

            races.AddPositive("Sand Trap", (int)Player.SandTrapScore());

            races.AddPositive("Avian", (int)Player.HarpyScore());

            races.AddPositive("Shark", (int)Player.SharkScore());

            races.AddPositive("Siren", (int)Player.SirenScore());

            races.AddPositive("Dog", (int)Player.DogScore());

            races.AddPositive("Wolf", (int)Player.WolfScore());

            races.AddPositive("Fox", (int)Player.FoxScore());

            races.AddPositive("Kitsune", (int)Player.KitsuneScore());

            races.AddPositive("Echidna", (int)Player.EchidnaScore());

            races.AddPositive("Mouse", (int)Player.MouseScore());

            races.AddPositive("Ferret", (int)Player.FerretScore());

            races.AddPositive("Raccoon", (int)Player.RaccoonScore());

            races.AddPositive("Bunny", (int)Player.BunnyScore());

            races.AddPositive("Kangaroo", (int)Player.KangaScore());

            races.AddPositive("Horse", (int)Player.HorseScore());

            races.AddPositive("Deer", (int)Player.DeerScore());

            races.AddPositive("Satyr", (int)Player.SatyrScore());

            races.AddPositive("Rhino", (int)Player.RhinoScore());

            races.AddPositive("Spider", (int)Player.SpiderScore());

            races.AddPositive("Pig", (int)Player.PigScore());

            races.AddPositive("Bee", (int)Player.BeeScore());

            races.AddPositive("Cockatrice", (int)Player.CockatriceScore());

            races.AddPositive("Red-Panda", (int)Player.RedPandaScore());

            races.AddPositive("Dryad", (int)Player.DryadScore());

            races.AddPositive("Mermaid", (int)Player.MermaidScore());

            races.Append();

            var flags = CoC.Instance.Flags;

            var misc = new Section("Miscellaneous Stats");

            var campPopulation = CampScene.GetCampPopulation();

            var glades = flags.Get(GameFlagIds.CorruptedGladesDestroyed);

            misc.Add("Camp Population", campPopulation, campPopulation > 0);

            misc.Add("Corrupted Glades Status", glades < 100 ? $"{100 - glades}% remaining" : "Extinct", glades > 0);

            misc.Add("Eggs Traded For", flags.Get(GameFlagIds.EggsBought), flags.Get(GameFlagIds.EggsBought) > 0);

            misc.Add("Times Had Fun with Feline Flexibility",

                flags.Get(GameFlagIds.TimesAutofellatioDueToCatFlexability),

                flags.Get(GameFlagIds.TimesAutofellatioDueToCatFlexability) > 0);

            misc.Add("Times Circle Jerked in the Arena", flags.Get(GameFlagIds.FapArenaSessions),

                flags.Get(GameFlagIds.FapArenaSessions) > 0);

            misc.Add("Victories in the Arena", flags.Get(GameFlagIds.FapArenaVictories),

                flags.Get(GameFlagIds.FapArenaSessions) > 0);

            misc.Add("Spells Cast", flags.Get(GameFlagIds.SpellsCast), flags.Get(GameFlagIds.SpellsCast) > 0);

            misc.Add("Times Bad-Ended", flags.Get(GameFlagIds.TimesBadEnded), flags.Get(GameFlagIds.TimesBadEnded) > 0);

            misc.Add("Times Orgasmed", flags.Get(GameFlagIds.TimesOrgasmed), flags.Get(GameFlagIds.TimesOrgasmed) > 0);

            misc.Append();

            var children = new Section("Children");

            children.AddNonZero("Times Given Birth",

                Player.StatusEffectValue(StatusEffectIds.Birthed, 0));

            children.AddNonZero("Litters With Amily",

                flags.Get(GameFlagIds.AmilyBirthTotal) + flags.Get(GameFlagIds.PcTimesBirthedAmilykids));

            children.AddNonZero("Children With Marble", flags.Get(GameFlagIds.MarbleKids));

            children.AddNonZero("Ant Children With Phylla", flags.Get(GameFlagIds.AntKids));

            var isabella = SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Npcs.IsabellaScene>.Get();

            children.AddNonZero("Children With Isabella (Male)", isabella.GetIsabellaChildType(1));

            children.AddNonZero("Children With Isabella (Female)", isabella.GetIsabellaChildType(2));

            children.AddNonZero("Children With Isabella (Herm)", isabella.GetIsabellaChildType(3));

            children.AddNonZero("Total Children With Isabella", isabella.TotalIsabellaChildren());

            var sophieChildren = SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Npcs.SophieBimboScene>.Get().SophieChildren();

            children.AddNonZero("Children With Sophie", sophieChildren);

            children.AddNonZero("Eggs Fertilized For Sophie", flags.Get(GameFlagIds.SophieEggsLaid) + sophieChildren);

            children.AddNonZero("Children With Urta",

                SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Npcs.UrtaPregsScene>.Get().UrtaKids());

            children.AddNonZero("Benoit Eggs Laid", flags.Get(GameFlagIds.BenoitEggs));

            children.AddNonZero("Benoite Eggs Produced", flags.Get(GameFlagIds.FemoitEggsLaid));

            children.AddNonZero("Children With Corrupted Witches",

                flags.Get(GameFlagIds.Volcwitchnumberofbirths) + flags.Get(GameFlagIds.Volcwitchnumberofchildren));

            children.AddNonZero("Children With Cotton", flags.Get(GameFlagIds.CottonKidCount));

            children.AddNonZero("Children With Edryn", flags.Get(GameFlagIds.EdrynNumberOfKids));

            children.AddNonZero("Ember Offspring (Male)", flags.Get(GameFlagIds.EmberChildrenMales));

            children.AddNonZero("Ember Offspring (Female)", flags.Get(GameFlagIds.EmberChildrenFemales));

            children.AddNonZero("Ember Offspring (Herm)", flags.Get(GameFlagIds.EmberChildrenHerms));

            children.AddNonZero("Total Children With Ember",

                flags.Get(GameFlagIds.EmberChildrenMales) + flags.Get(GameFlagIds.EmberChildrenFemales)

                + flags.Get(GameFlagIds.EmberChildrenHerms));

            children.AddNonZero("Ember Eggs Produced", flags.Get(GameFlagIds.EmberEggs));

            children.AddNonZero("Children With Izma (Sharkgirls)", flags.Get(GameFlagIds.IzmaChildrenSharkgirls));

            children.AddNonZero("Children With Izma (Tigersharks)", flags.Get(GameFlagIds.IzmaChildrenTigersharks));

            children.AddNonZero("Total Children with Izma",

                flags.Get(GameFlagIds.IzmaChildrenSharkgirls) + flags.Get(GameFlagIds.IzmaChildrenTigersharks));

            children.AddNonZero("Children With Kelly (Male)", flags.Get(GameFlagIds.KellyKidsMale));

            children.AddNonZero("Children With Kelly (Female)",

                flags.Get(GameFlagIds.KellyKids) - flags.Get(GameFlagIds.KellyKidsMale));

            children.AddNonZero("Total Children With Kelly", flags.Get(GameFlagIds.KellyKids));

            children.AddNonZero("Kiha Offspring (Male)", flags.Get(GameFlagIds.KihaChildrenBoys));

            children.AddNonZero("Kiha Offspring (Female)", flags.Get(GameFlagIds.KihaChildrenGirls));

            children.AddNonZero("Kiha Offspring (Herm)", flags.Get(GameFlagIds.KihaChildrenHerms));

            children.AddNonZero("Total Children With Kiha",

                flags.Get(GameFlagIds.KihaChildrenBoys) + flags.Get(GameFlagIds.KihaChildrenGirls)

                + flags.Get(GameFlagIds.KihaChildrenHerms));

            children.AddNonZero("Lynnette Children", flags.Get(GameFlagIds.LynnetteBabyCount));

            children.AddNonZero("Children With Minerva", flags.Get(GameFlagIds.MinervaChildren));

            children.AddNonZero("Drider Children With Phylla", flags.Get(GameFlagIds.PhyllaDriderBabiesCount));

            children.AddNonZero("Total Children With Phylla",

                flags.Get(GameFlagIds.AntKids) + flags.Get(GameFlagIds.PhyllaDriderBabiesCount));

            children.AddNonZero("Children With Sheila (Joeys)", flags.Get(GameFlagIds.SheilaJoeys));

            children.AddNonZero("Children With Sheila (Imps)", flags.Get(GameFlagIds.SheilaImps));

            children.AddNonZero("Total Children With Sheila",

                flags.Get(GameFlagIds.SheilaJoeys) + flags.Get(GameFlagIds.SheilaImps));

            if (flags.Get(GameFlagIds.TamaniNumberOfDaughters) > 0)
                children.AddNonZero("Children With Tamani",

                    $"{flags.Get(GameFlagIds.TamaniNumberOfDaughters)} (after all forms of natural selection)");

            children.AddNonZero("Adult Minotaur Offspring", flags.Get(GameFlagIds.AdultMinotaurOffsprings));

            children.Append();

            var body = new Section("Body Stats");

            var survival = Configuration.OptionStore.Instance.Get("survival", 0) > 0;

            body.Add("Satiety", $"{Player.Hunger:0} / 100", survival);

            body.Add("Anal Capacity", $"{MathF.Round(Player.AnalCapacity()):0}");

            body.Add("Anal Looseness", $"{MathF.Round(Player.Body.Get<MonoCoC.Contents.BodyParts.Vanilla.AnalPart>().Looseness):0}");

            body.Add("Fertility Rating (Base)", $"{MathF.Round(Player.Fertility):0}");

            body.Add("Fertility Rating (With Bonuses)", $"{MathF.Round(Player.TotalFertility()):0}");

            body.Add("Virility Rating", Player.VirilityQ(), Player.CumQ() > 0);

            body.Add("Cum Production", $"{Player.CumQ():N0}mL", Player.CumQ() > 0);

            body.Add("Milk Production", $"{Player.LactationQ():N0}mL", Player.LactationQ() > 0);

            var orientation = Player.SexOrientation switch
            {
                >= 90 => "Strictly attracted to males",

                >= 70 => "Attracted to males",

                > 60 => "Prefer males, bicurious",

                >= 40 => "Bisexual",

                > 30 => "Prefer females, bicurious",

                > 10 => "Attracted to females",

                _ => "Strictly attracted to females",
            };

            body.Add("Sexual Orientation", $"{orientation} ({Player.SexOrientation})");

            body.Add("Pregnancy Speed Multiplier", 1);

            body.Add("Total Cocks", Player.CockTotal(), Player.HasCock());

            if (Player.HasCock())
            {
                var totalLength = Player.Cocks.Sum(cock => cock.Length);

                var totalGirth = Player.Cocks.Sum(cock => cock.Thickness);

                body.Add("Total Cock Length", MeasurementsUtils.NumInchesOrCentimetres(totalLength));

                body.Add("Total Cock Girth", MeasurementsUtils.InchesOrCentimetres(totalGirth));
            }

            body.Add("Vaginal Capacity", $"{MathF.Round(Player.VaginalCapacity()):0}", Player.HasVagina());

            body.Append();

            var addict = new Section("Addictions");

            var marble = SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Npcs.MarbleScene>.Get();

            var marbleMilk = Player.HasPerk(PerkIds.MarbleResistant)

                ? 0

                : Player.HasPerk(PerkIds.MarblesMilk) ? 100 : (int)MathF.Round(marble.MarbleAddiction);

            addict.Add("Marble Milk", $"{marbleMilk}%", marble.KnowAddiction > 0);

            var minoIntake = flags.Get(GameFlagIds.MinotaurCumIntakeCount);

            var minoAddiction = flags.Get(GameFlagIds.MinotaurCumAddictionTracker);

            var minoText = !Player.HasPerk(PerkIds.MinotaurCumAddict)

                ? $"{MathF.Round(minoAddiction / 10) * 10}%"

                : Player.HasPerk(PerkIds.MinotaurCumResistance) ? "0% (Immune)" : "100+%";

            addict.Add("Minotaur Cum", minoText,

                minoIntake > 0 || minoAddiction > 0

                || Player.HasPerk(PerkIds.MinotaurCumAddict)

                || Player.HasPerk(PerkIds.MinotaurCumResistance));

            addict.Append();

            var effects = new Section("Ongoing Status Effects");

            effects.Add("Heat", $"{MathF.Round(Player.StatusEffectValue(StatusEffectIds.Heat, 2))} hours remaining",

                Player.HasStatusEffect(StatusEffectIds.Heat));

            effects.Add("Rut", $"{MathF.Round(Player.StatusEffectValue(StatusEffectIds.Rut, 2))} hours remaining",

                Player.HasStatusEffect(StatusEffectIds.Rut));

            effects.Add("Luststick", $"{MathF.Round(Player.StatusEffectValue(StatusEffectIds.Luststick, 0))} hours remaining",

                Player.StatusEffectValue(StatusEffectIds.Luststick, 0) > 0);

            effects.Add("Luststick Application",

                $"{MathF.Round(Player.StatusEffectValue(StatusEffectIds.LustStickApplied, 0))} hours remaining",

                Player.StatusEffectValue(StatusEffectIds.LustStickApplied, 0) > 0);

            effects.Add("Lusty Tongue",

                $"{MathF.Round(Player.StatusEffectValue(StatusEffectIds.LustyTongue, 0))} hours remaining",

                Player.StatusEffectValue(StatusEffectIds.LustyTongue, 0) > 0);

            effects.Add("Black Cat Beer",

                $"{Player.StatusEffectValue(StatusEffectIds.BlackCatBeer, 0)} hours remaining (Lust resistance 20% lower, physical resistance 25% higher)",

                Player.StatusEffectValue(StatusEffectIds.BlackCatBeer, 0) > 0);

            effects.Add("Uma's Massage",

                $"{Player.StatusEffectValue(StatusEffectIds.UmasMassage, 2)} hours remaining.",

                Player.StatusEffectValue(StatusEffectIds.UmasMassage, 0) > 0);

            effects.Add("Dysfunction",

                $"{Player.StatusEffectValue(StatusEffectIds.Dysfunction, 0)} hours remaining. (Disables masturbation)",

                Player.StatusEffectValue(StatusEffectIds.Dysfunction, 0) > 0);

            effects.Add("Inebriation",

                $"{Player.StatusEffectValue(StatusEffectIds.PhoukaWhiskeyAffect, 0)} hours remaining.",

                Player.StatusEffectValue(StatusEffectIds.PhoukaWhiskeyAffect, 0) > 0);

            effects.Append();

            var people = new Section("Interpersonal Stats");

            people.Add("Amily's Affection", flags.Get(GameFlagIds.AmilyAffection),

                flags.Get(GameFlagIds.AmilyMet) > 0);

            var isabellaFollower = SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Npcs.IsabellaFollowerScene>.Get();

            people.Add("Isabella's Affection",

                $"{(isabellaFollower.IsabellaFollower() ? 100 : flags.Get(GameFlagIds.IsabellaAffection))}%",

                flags.Get(GameFlagIds.IsabellaAffection) > 0 || isabellaFollower.IsabellaFollower());

            people.Add("Kiha's Affection", $"{flags.Get(GameFlagIds.KihaAffection)}%",

                flags.Get(GameFlagIds.KihaAffectionLevel) >= 2);

            people.Add("Owca's Attitude", flags.Get(GameFlagIds.OwcasAttitude),

                flags.Get(GameFlagIds.OwcasAttitude) > 0);

            people.Add("Arian's Health", flags.Get(GameFlagIds.ArianHealth),

                flags.Get(GameFlagIds.ArianPark) > 0);

            people.Add("Arian Sex Counter", flags.Get(GameFlagIds.ArianVirgin),

                flags.Get(GameFlagIds.ArianVirgin) > 0);

            people.Add("Benoit Affection", $"{flags.Get(GameFlagIds.BenoitAffection)}%",

                flags.Get(GameFlagIds.BenoitAffection) > 0);

            var ceraphParts = flags.Get(GameFlagIds.CeraphDicksOwned)

                + flags.Get(GameFlagIds.CeraphPussiesOwned)

                + flags.Get(GameFlagIds.CeraphTitsOwned);

            people.Add("Body Parts Taken By Ceraph", ceraphParts, ceraphParts > 0);

            var ember = SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Npcs.EmberScene>.Get();

            people.Add("Ember's Affection", $"{ember.EmberAffection()}%", ember.EmberAffection() > 0);

            people.Add("Ember Spar Intensity", ember.EmberSparIntensity(), ember.EmberSparIntensity() > 0);

            var helia = SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Npcs.HelFollowerScene>.Get();

            people.Add("Helia's Affection", $"{helia.HelAffection()}%", helia.HelAffection() > 0);

            people.Add("Helia Bonus Points", flags.Get(GameFlagIds.HelBonusPoints),

                helia.HelAffection() >= 100);

            people.Add("Sheila's Corruption", flags.Get(GameFlagIds.SheilaCorruption),

                flags.Get(GameFlagIds.SheilaXp) != 0);

            var urta = SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Npcs.UrtaScene>.Get();

            var urtaStatus = "";

            if (urta.UrtaLove())
                urtaStatus = flags.Get(GameFlagIds.UrtaQuestStatus) switch
                {
                    -1 => "Gone",

                    0 => "Lover",

                    1 => "Lover+",

                    _ => urtaStatus,
                };
            else if (flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) == -1)
                urtaStatus = "Ashamed";
            else if (flags.Get(GameFlagIds.UrtaPcAffectionCounter) < 30)
                urtaStatus = $"{MathF.Round(flags.Get(GameFlagIds.UrtaPcAffectionCounter) * 3.3333f)}%";
            else
                urtaStatus = "Ready To Confess Love";

            var urtaLabel = flags.Get(GameFlagIds.UrtaPcAffectionCounter) < 30 ? "Urta's Affection" : "Urta's Status";

            people.Add(urtaLabel, urtaStatus, flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) != 0);

            people.Add("Brooke's Affection", flags.Get(GameFlagIds.BrookeAffection),

                flags.Get(GameFlagIds.BrookeAffection) > 0);

            var sylvia = SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.Bog.SylviaScene>.Get();

            people.Add("Sylvia's Affection", $"{sylvia.sylviaGetAff}%", sylvia.SylviaProg > 0);

            people.Add("Sylvia's Dominance", $"{sylvia.sylviaGetDom}%", sylvia.SylviaProg > 0);

            people.Add("Kid A's Confidence", $"{flags.Get(GameFlagIds.KidAXp)}%",

                flags.Get(GameFlagIds.KidAXp) > 0);

            people.Add("Helia's Spar Intensity",

                SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Npcs.HelScene>.Get().HeliaSparIntensity());

            people.Add("Pablo's Affection", $"{flags.Get(GameFlagIds.PabloAffection)}%",

                flags.Get(GameFlagIds.PabloAffection) > 0);

            people.Add("Valeria's Fluid", $"{CoC.Instance.Flags.Get(GameFlagIds.ValeriaFluids)}%",

                CoC.Instance.Flags.Get(GameFlagIds.ValeriaFluids) > 0);

            people.Add("Lottie's Encouragement", $"{flags.Get(GameFlagIds.LottieConfidence)} (higher is better)",

                flags.Get(GameFlagIds.LottieEncounterCounter) > 0);

            people.Add("Lottie's Figure", $"{flags.Get(GameFlagIds.LottieFigure)} (higher is better)",

                flags.Get(GameFlagIds.LottieEncounterCounter) > 0);

            people.Add("Lynnette's Approval", flags.Get(GameFlagIds.LynnetteApproval),

                flags.Get(GameFlagIds.LynnetteApproval) != 0);

            people.Add("Rubi's Affection", $"{flags.Get(GameFlagIds.RubiAffection)}%",

                flags.Get(GameFlagIds.RubiAffection) > 0);

            people.Add("Rubi's Orifice Capacity",

                $"{SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.RubiScene>.Get().RubiCapacity()}%",

                flags.Get(GameFlagIds.RubiAffection) > 0);

            people.Add("Katherine's Submissiveness",

                SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.KatherineScene>.Get().Submissiveness(),

                flags.Get(GameFlagIds.KatherineUnlocked) >= 4);

            var marbleAffection = (int)Player.StatusEffectValue(StatusEffectIds.Marble, 0);

            people.Add("Marble's Affection", $"{marbleAffection}%",

                marbleAffection > 0);

            people.Add("Submissiveness To Kelt",

                $"{Math.Min(100, (int)MathF.Round(Player.StatusEffectValue(StatusEffectIds.Kelt, 1) / 130f * 100))}%",

                Player.HasStatusEffect(StatusEffectIds.Kelt)

                && flags.Get(GameFlagIds.KeltBreakLevel) == 0

                && flags.Get(GameFlagIds.KeltKilled) == 0);

            people.Add("Aiko affection", $"{Math.Clamp(flags.Get(GameFlagIds.AikoAffection), 0, 100)}%",

                flags.Get(GameFlagIds.AikoTimesMet) > 0);

            people.Add("Aiko corruption", flags.Get(GameFlagIds.AikoCorruption),

                flags.Get(GameFlagIds.AikoTimesMet) > 0);

            people.Append();

            var advancement = new Section("Advancement");

            advancement.Add("Level", Player.Level);

            advancement.Add("XP", $"{Player.Experience:0}/{Player.RequiredXP():0}");

            advancement.Add("Gems", Player.Gems);

            advancement.Add("Day", View.Time.Days);

            advancement.Append();

            AddButtonDisabled(0, "Stats");

            if (Player.CanBuyStats())
                AddButton(0, "Stat Up", () => Go<LevelUpScene>());

            AddButton(1, "Mastery", () => Go<MasteryScene>());

            var detailRows = BuildDetailRows(attackDamage, doubleAttackDamage, armorValue,

                regenPercentCombat, regenPercentNonCombat, regenFlat);

            StatsDetailScene.SetRows(detailRows);

            for (var index = 0; index < detailRows.Count; index++)
            {
                var detailIndex = index;

                Output.AddLink(detailRows[index].Label, () => StatsDetailScene.OpenDetail(detailIndex));
            }

            AddButton(14, "Common.Back".Localized(), Exit);
        }

        private static List<StatsDetailScene.StatDetail> BuildDetailRows(
            int attackDamage, int doubleAttackDamage, float armorValue,

            float regenPercentCombat, float regenPercentNonCombat, float regenFlat)
        {
            static string Summary(Stat stat, string indent = "    ")
            {
                var modifiers = Player.Stats.Modifiers
                    .Where(modifier => modifier.Stat == stat && modifier.Visible)
                    .ToList();

                if (modifiers.Count == 0)
                    return "";

                var builder = new StringBuilder();

                foreach (var modifier in modifiers)
                {
                    var value = modifier.Resolve();

                    if (MathF.Abs(value) < 0.001f)
                        continue;

                    builder.Append('\n').Append(indent).Append(modifier.Source.DisplayName).Append(": ");

                    builder.Append(modifier.Operation switch
                    {
                        ModifierOperation.Addition => $"{MathF.Round(value):+0;-0;0}",

                        ModifierOperation.Factor => $"<b>x{value:0.##}</b>",

                        _ => $"<b>x{1f + value:0.##}</b>",
                    });
                }

                return builder.ToString();
            }

            return

            [
                new("Attack Damage", () =>
                {
                    var weapon = Player.Weapon;

                    var weaponDamage = Player.Stats.Addition(GameStats.WeaponDamage)

                        * Player.Stats.Multiplier(GameStats.WeaponDamage);

                    var weaponSummary = Summary(GameStats.WeaponDamage, "        ");

                    var attackSummary = Summary(GameStats.AttackDamage, "    ");

                    var physSummary = Summary(GameStats.PhysDamage, "    ");

                    var globalSummary = Summary(GameStats.GlobalDamage, "    ");

                    return $"<b>Stat Base:</b> {Player.Strength:0} ({MathF.Min(Player.Strength, 60):0} for double attack)"

                        + $"\n\n<b>Calculated Weapon Damage:</b> {MathF.Round(Player.WeaponAttack + 2f * weapon.MasteryLevel(Player) + weaponDamage):0}"

                        + $"\n    <b>Weapon Damage (Base):</b> {weapon.AttackFor(Player):0}"

                        + $"\n    <b>Mastery Bonus:</b> {2 * weapon.MasteryLevel(Player)}"

                        + (weaponSummary == "" ? "" : "\n    <b>Weapon Damage Bonuses:</b>" + weaponSummary)

                        + (attackSummary == "" ? "" : "\n<b>Attack Damage Bonuses:</b>" + attackSummary)

                        + (physSummary == "" ? "" : "\n<b>Physical Damage Bonuses:</b>" + physSummary)

                        + (globalSummary == "" ? "" : "\n<b>Global Damage Bonuses:</b>" + globalSummary)

                        + $"\n\n<b>Attack:</b> {attackDamage} (<b>Double:</b> {doubleAttackDamage})";
                }),

                new("Physical Damage", () =>

                    $"Base: 100%{Summary(GameStats.PhysDamage)}\n\nResult: {MathF.Round(100 * Player.PhysMod())}%"),

                new("Critical Chance", () =>

                    $"Base: 5%{Summary(GameStats.CriticalChance)}\n\nResult: {MathF.Round(Player.GetBaseCritChance())}%"),

                new("Weapon Critical Chance", () =>

                    $"Base: 0%{Summary(GameStats.CritCWeapon)}\n\nResult: {MathF.Round(Player.GetMeleeCritBonus())}%"),

                new("Dodge Chance", () =>

                    $"Base: {MathF.Round(Player.Speed / 10f)}%{Summary(GameStats.Dodge)}\n\nResult: {MathF.Round(Player.GetEvasionChance())}%"),

                new("Parry Chance", () =>

                    $"Base: {MathF.Round((Player.Speed - 50f) / 5f)}%{Summary(GameStats.Parry)}\n\nResult: {MathF.Round(Player.CanParry() ? Player.ParryChance() : 0)}%"),

                new("Armor", () =>

                    $"{Player.Armor.Name}: {Player.Armor.Defense:0}"

                    + $"\n{Player.UpperGarment.Name}: {Player.UpperGarment.ArmorDefense:0}"

                    + $"\n{Player.LowerGarment.Name}: {Player.LowerGarment.ArmorDefense:0}"

                    + $"\nMisc: {Player.ArmorMiscBonuses():0}"

                    + Summary(GameStats.Armor)

                    + $"\n\nResult: {armorValue:0}"),

                new("Damage Resistance", () =>
                {
                    float Taken(bool maxDamage, bool minDamage)
                    {
                        var mult = 100f;

                        mult -= maxDamage ? 0f : minDamage ? 25f : 0f;

                        if (mult < 75f)
                            mult = 75f;

                        mult -= armorValue;

                        if (Player.HasPerk(PerkIds.ImmovableObject) && Player.Toughness >= 75)
                            mult *= 0.9f;

                        if (Player.StatusEffectValue(StatusEffectIds.BlackCatBeer, 0) > 0)
                            mult *= 0.75f;

                        return Math.Max(20f, mult);
                    }

                    return $"Toughness: {Player.Toughness:0}\nArmor: {armorValue:0}"

                        + $"\n\nResult: {100 - MathF.Round(Taken(true, false))}-{100 - MathF.Round(Taken(false, true))}% (Higher is better)";
                }),

                new("Tease Chance", () =>

                    $"Sexiness: {Player.UpperGarment.Sexiness + Player.LowerGarment.Sexiness:0}"

                    + $"\n\nResult: {Player.UpperGarment.Sexiness + Player.LowerGarment.Sexiness + Contents.Items.ArmorItemBonuses.Tease(Player).Chance:0}"),

                new("Tease Damage", () =>

                    $"Base: 0{Summary(GameStats.TeaseDamage)}"

                    + $"\n\nResult: {Player.Stats.Addition(GameStats.TeaseDamage) + Contents.Items.ArmorItemBonuses.Tease(Player).Damage:0}"),

                new("Health Regen", () =>

                    $"Combat: {regenPercentCombat}% + {regenFlat:0}"

                    + $"\nNon-combat: {regenPercentNonCombat}% + {regenFlat:0}"

                    + Summary(GameStats.HealthRegenPercent, "    ")

                    + Summary(GameStats.HealthRegenFlat, "    ")),

                new("Lust Resistance", () =>

                    $"Base: {100 - Player.GetLustPercentBase():0.##}%"

                    + Summary(GameStats.LustResistance)

                    + $"\n\nResult: {100 - MathF.Round(Player.LustPercent)}% (Higher is better)"),

                new("Spell Mod", () =>

                    $"Base: 100%\n\nResult: {MathF.Round(100 * Player.SpellMod())}%"),

                new("Spell Cost", () =>

                    $"Base: 100%{Summary(GameStats.SpellCost)}\n\nResult: {Player.SpellCost(100)}%"),
            ];
        }

        private static void Exit()
        {
            UI.Components.Combat.PlayerStats.HiddenByMenu = false;

            if (_returnTo != null)
            {
                var returnTo = _returnTo;

                _returnTo = null;

                returnTo();

                return;
            }

            Go<CampScene>();
        }
    }

    // 原版 PlayerInfo.hx displayStatSummary 的点击 Stat 详细页
    [Register<StatsDetailScene>("coc:stats_details")]
    public class StatsDetailScene : Scene
    {
        public sealed record StatDetail(string Label, Func<string> Text);

        private static List<StatDetail> _rows = [];

        private static int _jumpIndex = -1;

        private readonly PagedMenu<StatDetail> _pager = new();

        public static void SetRows(List<StatDetail> rows) => _rows = rows;

        public static void OpenDetail(int index)
        {
            _jumpIndex = index;

            Go<StatsDetailScene>();
        }

        public override void Entry()
        {
            if (_jumpIndex >= 0 && _jumpIndex < _rows.Count)
            {
                var detail = _rows[_jumpIndex];

                _jumpIndex = -1;

                Show(detail);

                return;
            }

            Render();
        }

        private void Render()
        {
            ClearOutput();

            ClearButtons();

            DisplayHeader("Stat Details");

            _pager.Items = _rows;

            _pager.PageSize = 12;

            _pager.Render(
                (slot, label, action) =>
                {
                    if (action == null)
                        AddButtonDisabled(slot, label);
                    else
                        AddButton(slot, label, action);
                },

                detail => detail.Label,

                detail => () => Show(detail),

                () => Go<StatsScene>(),

                Render,

                "Prev",

                "Next",

                "Back",

                0);
        }

        private void Show(StatDetail detail)
        {
            ClearOutput();

            ClearButtons();

            DisplayHeader(StatsScene.Section.LocalizeTitle(detail.Label));

            OutputText(detail.Text());

            AddButton(14, "Common.Back".Localized(), () => Go<StatsScene>());
        }
    }

    [Register<MasteryScene>("coc:mastery")]
    public class MasteryScene : Scene
    {
        public override void Entry()
        {
            ClearOutput();

            ClearButtons();

            DisplayHeader("Scenes.Stats.Mastery".Localized());

            if (Player.Masteries.Count == 0)
                OutputText("You have not learned any masteries yet.");
            else
            {
                var builder = new StringBuilder();

                var descriptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["Tease"] = "Tease mastery",
                    ["Shield"] = "Shield mastery",
                    ["Casting"] = "Spellcasting mastery",
                    ["Terrestrial Fire"] = "Terrestrial fire magic mastery",
                };

                foreach (var mastery in Player.Masteries)
                {
                    static string KeyOf(string value) =>

                        new(value.Where(char.IsLetterOrDigit).ToArray());

                    var rawName = mastery.Id.Path.Replace('_', ' ');

                    var name = ("Masteries." + KeyOf(rawName) + ".Name").LocalizedOr(rawName);

                    var maxXp = (int)(100 * Math.Pow(1.5, mastery.Level));

                    var description = ("Masteries." + KeyOf(rawName) + ".Desc").LocalizedOr(
                        descriptions.TryGetValue(rawName, out var known) ? known : $"{rawName} mastery");

                    builder.Append("[pg-]<b>").Append(name).Append("</b>")
                        .Append(" - Level ").Append(mastery.Level)
                        .Append("\n    XP: ").Append(mastery.XP).Append('/').Append(maxXp)
                        .Append("\n    ").Append(description);

                    if (mastery.Permed)
                        builder.Append("\n    <b>Permanent</b>");

                    builder.Append("\n\n");
                }

                OutputText(builder.ToString());
            }

            if (Player.CanBuyStats())
                AddButton(0, "Stat Up", () => Go<LevelUpScene>());

            else
                AddButton(0, "Stats", () => Go<StatsScene>());

            AddButtonDisabled(1, "Mastery");

            AddButton(14, "Common.Back".Localized(), () => Go<CampScene>());
        }
    }

    internal static class SectionExtensions
    {
        public static T Also<T>(this T value, System.Action<T> action)
        {
            action(value);

            return value;
        }
    }
}
