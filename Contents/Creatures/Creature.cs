using Hjson;
using MonoCoC.Contents.BodyParts;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Perks;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Contents.Stats;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using MonoCoC.Contents.StatusEffects;
using MonoCoC.Contents.Masteries;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Internals;
using MonoCoC.Configuration;
using System.Linq;

namespace MonoCoC.Contents.Creatures
{
    public class Creature
    {
        public Creature()
        {
            Stats.SetProvider(GameStats.MaxHealth, () =>
        {
            var max = Toughness * 2 + 50;

            if (HasPerk(MonoCoC.Contents.Datas.Vanilla.PerkIds.Tank)) max += 50;
            if (HasPerk(MonoCoC.Contents.Datas.Vanilla.PerkIds.Tank2)) max += Toughness;
            if (HasPerk(MonoCoC.Contents.Datas.Vanilla.PerkIds.Tank3)) max += Level * 5;

            max += Level * 15;

            return Math.Min(9999f, max);
        });
        }

        public string ShortName = "You";

        public string Quantifier = "a ";

        public string Race = "";

        public Age _age = Age.Adult;

        public Age Age
        {
            get => _age;

            set
            {
                _age = value;

                ApplyAgeModifiers();
            }
        }

        public virtual void ApplyAgeModifiers()
        {
            Stats.RemoveAll<AgeSource>();

            if (Age == Age.Adult)
                return;

            var source = new AgeSource();

            var statGain = Age switch
            {
                Age.Child => 1.15f,

                Age.Teen => 1.1f,

                Age.Elder => 0.7f,

                _ => 1f
            };

            var statLoss = statGain;

            var corruptionGain = Age switch
            {
                Age.Child => 1.15f,

                Age.Teen => 1.15f,

                _ => 1f
            };

            var corruptionLoss = corruptionGain;

            if (statGain != 1f)
            {
                Stats.AddModifier(GameStats.StatGain, ModifierOperation.Factor, () => statGain, source);

                Stats.AddModifier(GameStats.StatLoss, ModifierOperation.Factor, () => statLoss, source);
            }

            if (corruptionGain != 1f)
            {
                Stats.AddModifier(GameStats.CorruptionGain, ModifierOperation.Factor, () => corruptionGain, source);

                Stats.AddModifier(GameStats.CorruptionLoss, ModifierOperation.Factor, () => corruptionLoss, source);
            }

            if (Age == Age.Child)
            {
                Stats.AddModifier(GameStats.MinLibido, ModifierOperation.Addition, () => -14f, source);

                Stats.AddModifier(GameStats.MinSensitivity, ModifierOperation.Addition, () => -5f, source);

                // 原版 Creature.hx:455 CHILD_MODIFIERS
                Stats.AddModifier(GameStats.LustResistance, ModifierOperation.Addition, () => 10f, source);

                Stats.AddModifier(GameStats.LustResistance, ModifierOperation.Factor, () => 1.4f, source);
            }
            else if (Age == Age.Teen)
            {
                Stats.AddModifier(GameStats.MinLibido, ModifierOperation.Addition, () => 5f, source);

                // 原版 Creature.hx:465 teenLustMod(): -10 - (100 - baseLustPercent) * 0.25
                Stats.AddModifier(GameStats.LustResistance, ModifierOperation.Addition,

                    () => MathF.Truncate(-10f - (100f - GetLustPercentBase()) * 0.25f), source);
            }
            else if (Age == Age.Elder)
            {
                Stats.AddModifier(GameStats.MinLibido, ModifierOperation.Addition, () => -5f, source);

                Stats.AddModifier(GameStats.LustResistance, ModifierOperation.Factor, () => 1.1f, source);
            }
        }

        public Age StartingAge = Age.Adult;

        public Gender Gender
        {
            get
            {
                var hasCock = HasCock();

                var hasVagina = HasVagina();

                if (hasCock && hasVagina)
                    return Gender.Hermaphrodite;

                if (hasCock)
                    return Gender.Male;

                if (hasVagina)
                    return Gender.Female;

                return Gender.Genderless;
            }
        }

        public StatManager Stats = new();

        public float Strength
        {
            get => Stats.Get(GameStats.Strength);

            set => Stats.SetBase(GameStats.Strength, value);
        }

        public float Toughness
        {
            get => Stats.Get(GameStats.Toughness);

            set => Stats.SetBase(GameStats.Toughness, value);
        }

        public float Speed
        {
            get => Stats.Get(GameStats.Speed);

            set => Stats.SetBase(GameStats.Speed, value);
        }

        public float Intelligence
        {
            get => Stats.Get(GameStats.Intelligence);

            set => Stats.SetBase(GameStats.Intelligence, value);
        }

        public float Libido
        {
            get => Stats.Get(GameStats.Libido);

            set => Stats.SetBase(GameStats.Libido, value);
        }

        public float MinLibido
        {
            get => Stats.Get(GameStats.MinLibido);

            set => Stats.SetBase(GameStats.MinLibido, value);
        }

        public float Sensitivity
        {
            get => Stats.Get(GameStats.Sensitivity);

            set => Stats.SetBase(GameStats.Sensitivity, value);
        }

        public float Corruption
        {
            get => Stats.Get(GameStats.Corruption);

            set => Stats.SetBase(GameStats.Corruption, value);
        }

        public float MaxHealth
        {
            get
            {
                var max = Stats.Get(GameStats.MaxHealth);

                // 原版 Creature.hx:4794：Soulburst 每层让最大生命减半
                var soulburst = StatusEffectByType(Combat.Handlers.SoulburstHandlers.Soulburst);

                if (soulburst != null) max /= MathF.Pow(2f, soulburst.Values[0]);

                return max;
            }
        }

        public float FireResistance = 1;

        public float LustVulnerability = 1;

        public float Health;

        public float Lust
        {
            get => Stats.Get(GameStats.Lust);

            set => Stats.SetBase(GameStats.Lust, Math.Clamp(value, MinLust(), MaxLust));
        }

        // 原版 dynStats 每次变化都把 lust 限制在 [minLust, maxLust]
        public void ClampLust() => Lust = Math.Clamp(Lust, MinLust(), MaxLust);

        // 原版 Creature.hx:4860 minLust()：普通角色下限为 0
        public virtual float MinLust() => 0;

        public virtual float MaxLust
        {
            get
            {
                var max = 100f;

                if (this is Player && ((Player)this).DemonScore() >= 4) max += 20;
                if (HasPerk(MonoCoC.Contents.Datas.Vanilla.PerkIds.ImprovedSelfControl)) max += 20;
                if (HasPerk(MonoCoC.Contents.Datas.Vanilla.PerkIds.ImprovedSelfControl2)) max += 20;
                if (HasPerk(MonoCoC.Contents.Datas.Vanilla.PerkIds.BroBody) || HasPerk(MonoCoC.Contents.Datas.Vanilla.PerkIds.BimboBody)
                    || HasPerk(MonoCoC.Contents.Datas.Vanilla.PerkIds.FutaForm)) max += 20;

                if (HasPerk(MonoCoC.Contents.Datas.Vanilla.PerkIds.OmnibusGift)) max += 15;
                if (HasPerk(MonoCoC.Contents.Datas.Vanilla.PerkIds.AscensionDesires)) max += PerkValue(MonoCoC.Contents.Datas.Vanilla.PerkIds.AscensionDesires, 0) * 5;
                if (HasPerk(MonoCoC.Contents.Datas.Vanilla.PerkIds.NephilaArchQueen)) max += 40;

                return max > 999 ? 999 : max;
            }
        }

        public bool IgnoreLust;

        public DynResult ApplyStats(params DynStat[] changes) => ApplyStats(new DynStatsOptions(Scale: true, EnforceMax: true), changes);

        public virtual DynResult ApplyStats(DynStatsOptions options, params DynStat[] changes)
        {
            var result = new DynResult();

            var oldMaxHealth = MaxHealth;

            var oldHealth = Health;

            foreach (var change in changes)
            {
                var current = Stats.Get(change.Stat);

                var delta = ApplyDynOp(current, change.Amount, change.Op);

                if (options.Scale)
                    delta *= DeltaMultiplier(change.Stat, delta);

                var target = current + delta;

                target = Math.Max(MinValue(change.Stat), target);

                if (options.EnforceMax)
                    target = Math.Min(MaxValue(change.Stat), target);

                var applied = target - current;

                if (applied != 0)
                    if (change.Permanent)
                        Stats.AddBase(change.Stat, applied);
                    else
                    {
                        var modifier = Stats.AddTemporaryAddition(change.Stat, applied, new TemporarySource(Guid.NewGuid()));

                        result.Tickets.Add(new StatTicket(Stats, modifier));
                    }

                result.Applied[change.Stat] = result.Applied.GetValueOrDefault(change.Stat) + applied;
            }

            var newMaxHealth = MaxHealth;

            if (newMaxHealth != oldMaxHealth && oldMaxHealth > 0)
                Health = Math.Clamp(oldHealth / oldMaxHealth * newMaxHealth, 0, newMaxHealth);

            return result;
        }

        public static float ApplyDynOp(float current, float amount, DynStatOp op) => op switch
        {
            DynStatOp.Mul => current * amount - current,

            DynStatOp.Div => current / amount - current,

            DynStatOp.Eq => amount - current,

            DynStatOp.Sub => -amount,

            _ => amount
        };

        public virtual float DeltaMultiplier(Stat stat, float delta)
        {
            if (delta == 0)
                return 1;

            var gain = delta > 0;

            if (stat == GameStats.Strength)
                return GainFactor(GameStats.StatGain, gain) * GainFactor(GameStats.StrengthGain, gain);

            if (stat == GameStats.Toughness)
                return GainFactor(GameStats.StatGain, gain) * GainFactor(GameStats.ToughnessGain, gain);

            if (stat == GameStats.Speed)
                return GainFactor(GameStats.StatGain, gain) * GainFactor(GameStats.SpeedGain, gain);

            if (stat == GameStats.Intelligence)
                return GainFactor(GameStats.StatGain, gain) * GainFactor(GameStats.IntelligenceGain, gain);

            if (stat == GameStats.Libido)
                return GainFactor(GameStats.LibidoGain, gain);

            if (stat == GameStats.Sensitivity)
                return GainFactor(GameStats.SensitivityGain, gain);

            if (stat == GameStats.Corruption)
                return GainFactor(GameStats.CorruptionGain, gain);

            return 1;
        }

        public virtual float GainFactor(Stat gainStat, bool gain) => Stats.Factor(gain ? gainStat : LossStat(gainStat));

        public static Stat LossStat(Stat gainStat)
        {
            if (gainStat == GameStats.StrengthGain)
                return GameStats.StrengthLoss;

            if (gainStat == GameStats.ToughnessGain)
                return GameStats.ToughnessLoss;

            if (gainStat == GameStats.SpeedGain)
                return GameStats.SpeedLoss;

            if (gainStat == GameStats.IntelligenceGain)
                return GameStats.IntelligenceLoss;

            if (gainStat == GameStats.LibidoGain)
                return GameStats.LibidoLoss;

            if (gainStat == GameStats.SensitivityGain)
                return GameStats.SensitivityLoss;

            if (gainStat == GameStats.CorruptionGain)
                return GameStats.CorruptionLoss;

            return GameStats.StatLoss;
        }

        public virtual float MinValue(Stat stat)
        {
            if (stat == GameStats.Libido)
                return Math.Max(0, MathF.Floor((1 + Stats.Addition(GameStats.MinLibido)) * Stats.Multiplier(GameStats.MinLibido)));

            if (stat == GameStats.Sensitivity)
                return Math.Max(1, MathF.Floor((10 + Stats.Addition(GameStats.MinSensitivity)) * Stats.Multiplier(GameStats.MinSensitivity)));

            if (stat == GameStats.Corruption)
                return 0;

            if (stat == GameStats.Lust)
                return MinLust();

            return 1;
        }

        public virtual float MaxValue(Stat stat)
        {
            if (stat == GameStats.Libido)
                return MaxOr(GameStats.MaxLibido, 100);

            if (stat == GameStats.Sensitivity)
                return MaxOr(GameStats.MaxSensitivity, 100);

            if (stat == GameStats.Corruption)
                return MaxOr(GameStats.MaxCorruption, 100);

            if (stat == GameStats.Lust)
                return MaxLust;

            return 9999;
        }

        public virtual float MaxOr(Stat stat, float fallback)
        {
            var value = Stats.Get(stat);

            return value > 0 ? value : fallback;
        }

        public float Fatigue;

        public bool Sleeping;

        public float Level;

        public float Experience;

        public float AdditionalExperience;

        public int _gems;

        public int Gems
        {
            get
            {
                if (_gems < 0)
                    _gems = 0;

                return _gems;
            }

            set => _gems = Math.Max(0, value);
        }

        public virtual int SetGems(int amount)
        {
            Gems = amount;

            return Gems;
        }

        public virtual int GainGems(int amount) => SetGems(Gems + amount);

        public virtual int LoseGems(int amount) => SetGems(Gems - amount);

        public Body Body;

        public float Tallness;

        public float Femininity;

        public virtual void ModFem(float amount, float max = 100f) =>

            Femininity = Math.Min(max, Femininity + amount);

        public float Tone;

        public float Thickness;

        public float Fertility = 10;

        public float NippleLength = 0.25f;

        public float Balls;

        public float BallSize;

        public float CumMultiplier = 1;

        public float HoursSinceCum;

        public List<CockPart> Cocks => Body.GetList<CockPart>();

        public virtual int CockTotal() => Cocks.Count;

        public virtual string ModTone(float goal, float strength = 1f)
        {
            if (goal == Tone)
                return "";

            strength = Math.Min(strength, Math.Abs(Tone - goal));

            if (goal >= 50 && Tone < goal)
            {
                Tone += strength;

                return goal == Tone

                    ? "[pg]You've gained some muscle tone, but can't gain any more this way. (+" + strength + " muscle tone)"

                    : "[pg]Your body feels a little more solid as you move, and your muscles look slightly more visible. (+" + strength + " muscle tone)";
            }

            if (goal <= 50 && Tone > goal)
            {
                Tone -= strength;

                return goal == Tone

                    ? "[pg]You've lost some tone, but can't lose any more this way. (-" + strength + " muscle tone)"

                    : "[pg]Moving brings with it a little more jiggle than you're used to. You don't seem to have gained weight, but your muscles look less visible. (-" + strength + " muscle tone)";
            }

            return "";
        }

        public virtual string ModThickness(float goal, float strength = 1f)
        {
            if (goal == Thickness)
                return "";

            strength = Math.Min(strength, Math.Abs(Thickness - goal));

            if (goal >= 50 && Thickness < goal)
            {
                Thickness += strength;

                return "[pg]Your center of balance changes a little bit as your body noticeably widens. (+" + strength + " body thickness)";
            }

            if (goal <= 50 && Thickness > goal)
            {
                Thickness -= strength;

                return "[pg]Each movement feels a tiny bit easier than the last. Did you just lose a little weight!? (+" + strength + " body thinness)";
            }

            return "";
        }

        public virtual int FertilizedEggs()
        {
            var spider = GetPerk(PerkIds.SpiderOvipositor);

            var bee = GetPerk(PerkIds.BeeOvipositor);

            if (spider == null && bee == null)
                return -1;

            if (spider != null)
                return (int)spider.Values[1];

            return (int)bee!.Values[1];
        }

        public virtual float AverageCockThickness()
        {
            if (Cocks.Count == 0)
                return 0f;

            var sum = 0f;

            foreach (var cock in Cocks) sum += cock.Thickness;

            return sum / Cocks.Count;
        }

        public virtual bool CanTitFuck() => BiggestBreastSize() >= 3f;

        public virtual float AverageVaginalLooseness() =>

            Vaginas.Count == 0 ? 0f : Vaginas.Average(v => (float)v.Looseness);

        public virtual float Wetness() => Vaginas.Count == 0 ? 0f : Vaginas[0].Wetness;

        public virtual float CockArea(int index) =>

            index >= 0 && index < Cocks.Count ? Cocks[index].Length * Cocks[index].Thickness : 0f;

        public virtual float BiggestCockLength() => Cocks.Count > 0 ? Cocks[BiggestCockIndex()].Length : 0f;

        public virtual float LongestCockLength() => BiggestCockLength();

        public virtual bool CanAutoFellate() => Cocks.Count > 0 && Cocks[0].Length >= 20f;

        public virtual int ThinnestCock()
        {
            var index = 0;

            for (var i = 1; i < Cocks.Count; i++)
                if (Cocks[i].Thickness < Cocks[index].Thickness)
                    index = i;

            return index;
        }

        public virtual int ShortestCockIndex()
        {
            var index = 0;

            for (var i = 1; i < Cocks.Count; i++)
                if (Cocks[i].Length < Cocks[index].Length)
                    index = i;

            return index;
        }

        public virtual float ShortestCockLength() => Cocks.Count > 0 ? Cocks.Min(c => c.Length) : 0f;

        public virtual bool HasLongTongue() => Body.Get<TonguePart>().Id.Path is "long" or "long_tongue";

        public float FatiguePercent => MaxFatigue() <= 0 ? 0 : 100 * Fatigue / MaxFatigue();

        public float HpPercent => MaxHealth <= 0 ? 0 : 100 * Health / MaxHealth;

        public float LustPercent => MaxLust <= 0 ? 0 : 100 * Lust / MaxLust;

        /// <summary>
        /// 原版 Creature.hx:594 str100 = <c>100 * str / getMaxStats('str')</c>。
        /// 原版 getAllMaxStats()（:656）把 str 上限写成占位值 9999，故普通角色此值约 0.5，
        /// 原版中 <c>str100 &gt;= 50</c> 几乎永不成立——这里保留该语义，不做「修正」。
        /// </summary>
        public float StrengthPercent => 100f * Strength / MaxValue(GameStats.Strength);

        public virtual bool LustPercentBetween(float min, float max = 100) => (int)LustPercent >= min && (int)LustPercent <= max;

        public virtual float GetLustPercentBase()
        {
            float levels = Level - 1;

            float resist = (Math.Clamp(levels, 0, 10) * 3f)

                + (Math.Clamp(levels - 10, 0, 10) * 2f)

                + (Math.Clamp(levels - 20, 0, 10) * 1f)

                + (Math.Max(0, levels - 30) * 0.2f);

            return Math.Max(25, 100 - resist);
        }

        public bool InRut => HasStatusEffect(StatusEffectIds.Rut);

        public bool InHeat => HasStatusEffect(StatusEffectIds.Heat);

        public virtual bool HasGills() => Body.Get<GillsPart>().Id != BodyPartTypes.None;

        public HornsPart Horns => Body.Get<HornsPart>();

        public virtual bool CanFly() => Body.Get<WingsPart>().Type.GetBool("flight");

        public virtual bool HasMaraeBless() =>

            HasPerk(PerkIds.PurityBlessing)

            || HasPerk(PerkIds.MaraesGiftButtslut)

            || HasPerk(PerkIds.MaraesGiftFertility)

            || HasPerk(PerkIds.MaraesGiftProfractory)

            || HasPerk(PerkIds.MaraesGiftStud);

        public virtual bool HasFeraBoon() =>

            HasPerk(PerkIds.FerasBoonAlpha)

            || HasPerk(PerkIds.FerasBoonBreedingBitch)

            || HasPerk(PerkIds.FerasBoonMilkingTwat)

            || HasPerk(PerkIds.FerasBoonSeeder)

            || HasPerk(PerkIds.FerasBoonWideOpen);

        public virtual bool CanUseStare()
        {
            var eyes = Body.Get<EyesPart>().Id;

            return eyes == BodyPartTypes.Basilisk || eyes == BodyPartTypes.Cockatrice;
        }

        public virtual bool HasMuzzle()
        {
            var face = Body.Get<FacePart>();

            return BodyPartSlotRegistry.Get(face.SlotId).Types.Get(face.Id).GetBool("muzzle");
        }

        public virtual bool HasCockThatFits(float fits = 0f) => CockThatFits(fits) >= 0;

        public virtual int CockThatFits2(float fits = 0f)
        {
            if (CockTotal() == 1)
                return -1;

            var index = -1;

            var index2 = -1;

            for (var i = Cocks.Count - 1; i >= 0; i--)
            {
                if (CockArea(i) > fits)
                    continue;

                if (index < 0)
                {
                    index = i;

                    continue;
                }

                if (CockArea(i) > CockArea(index))
                {
                    index2 = index;

                    index = i;
                }
                else if (index2 < 0 || CockArea(index2) < CockArea(i))
                    index2 = i;
            }

            return index2;
        }

        public virtual float BiggestCockArea()
        {
            var max = 0f;

            for (var index = 0; index < Cocks.Count; index++)
                max = Math.Max(max, CockArea(index));

            return max;
        }

        public virtual int CountKnottedCocks()
        {
            var count = 0;

            foreach (var cock in Cocks)
                if (cock.HasKnot)
                    count += 1;

            return count;
        }

        public virtual int CountNotKnottedCocks() => Cocks.Count - CountKnottedCocks();

        public virtual int CountCocksOfType(string type)
        {
            var count = 0;

            foreach (var cock in Cocks)
            {
                var cockType = BodyPartSlotRegistry.Get(cock.SlotId).Types.Get(cock.Id).GetString("type");

                if (string.Equals(cockType, type, StringComparison.OrdinalIgnoreCase))
                    count += 1;
            }

            return count;
        }

        public virtual int CockThatFits(float fits = 0, string type = "area")
        {
            if (Cocks.Count == 0)
                return -1;

            var index = -1;

            for (var current = 0; current < Cocks.Count; current++)
            {
                var size = type == "length" ? Cocks[current].Length : CockArea(current);

                if (size > fits)
                    continue;

                if (index < 0)
                {
                    index = current;

                    continue;
                }

                var saved = type == "length" ? Cocks[index].Length : CockArea(index);

                if (size > saved)
                    index = current;
            }

            return index;
        }

        public virtual bool ChangeCockType(int index, string type) =>

            index >= 0 && index < Cocks.Count && Cocks[index].ChangeType(type);

        public virtual void ChangeAllCockTypes(string type)
        {
            foreach (var cock in Cocks)
                cock.ChangeType(type);
        }

        public List<VaginaPart> Vaginas => Body.GetList<VaginaPart>();

        public List<BreastRowPart> BreastRows => Body.GetList<BreastRowPart>();

        public AnalPart Anal => Body.Get<AnalPart>();

        public virtual float VaginalCapacity() =>

            Vaginas.Count == 0 ? 0 : Vaginas[0].Capacity(StatusEffectValue(StatusEffectIds.BonusVCapacity, 0));

        public virtual float AnalCapacity() =>

            (StatusEffectValue(StatusEffectIds.BonusACapacity, 0) + 6 * Anal.Looseness * Anal.Looseness) *

            (1 + Anal.Wetness / 10f);

        public EarsPart Ears => Body.Get<EarsPart>();

        public List<Perk> Perks = [];

        public List<Mastery> Masteries = [];

        public List<StatusEffect> StatusEffects = [];

        public CombatDistance Distance = CombatDistance.Melee;

        public int ExtraDistance;

        public bool IsFlying;

        public bool IsSilenced;

        public bool IsCrippled;

        public bool IsUnfocused;

        public bool IsClumsy;

        public bool IsAtrophied;

        public bool IsPrude;

        public bool IsCornered;

        public bool IsImmobilized;

        public enum EquipmentSlot
        {
            Armor,

            Weapon,

            UpperGarment,

            LowerGarment
        }

        public readonly Dictionary<EquipmentSlot, string> displayNames = [];

        public string armorName = "";

        public string weaponName = "";

        public string upperGarmentName = "";

        public string lowerGarmentName = "";

        public virtual void SetDisplayName(EquipmentSlot slot, string name) => displayNames[slot] = name;

        public virtual void ClearDisplayName(EquipmentSlot slot) => displayNames.Remove(slot);

        public virtual string? GetDisplayName(EquipmentSlot slot) => displayNames.TryGetValue(slot, out var n) ? n : null;

        public virtual string ArmorName
        {
            get => GetDisplayName(EquipmentSlot.Armor) ?? armorName;

            set => armorName = value;
        }

        public virtual string WeaponName
        {
            get => GetDisplayName(EquipmentSlot.Weapon) ?? weaponName;

            set => weaponName = value;
        }

        public virtual string UpperGarmentName
        {
            get => GetDisplayName(EquipmentSlot.UpperGarment) ?? upperGarmentName;

            set => upperGarmentName = value;
        }

        public virtual string LowerGarmentName
        {
            get => GetDisplayName(EquipmentSlot.LowerGarment) ?? lowerGarmentName;

            set => lowerGarmentName = value;
        }

        public float ArmorDefense;

        public virtual void AddHp(float amount) =>

            Health = Math.Min(Health + amount, MaxHealth);

        public string ArmorPerk = "";

        public float ArmorValue;

        public string WeaponVerb = "";

        public float WeaponAttack;

        public float WeaponAccuracy;

        public float WeaponValue;

        public List<Identifier> WeaponTags = [];

        public string JewelryName = "";

        public float JewelryEffectId;

        public float JewelryEffectMagnitude;

        public string JewelryPerk = "";

        public float JewelryValue;

        public string ShieldName = "";

        public float ShieldBlock;

        public string ShieldPerk = "";

        public float ShieldValue;

        public string UpperGarmentPerk = "";

        public float UpperGarmentValue;

        public string LowerGarmentPerk = "";

        public float LowerGarmentValue;

        public virtual bool IsMale() => Gender == Gender.Male;

        public virtual bool IsFemale() => Gender == Gender.Female;

        public virtual bool IsHerm() => Gender == Gender.Hermaphrodite;

        public virtual bool IsGenderless() => Gender == Gender.Genderless;

        public virtual bool IsChild() => Age == Age.Child;

        public virtual bool IsShota() => IsChild() && HasCock();

        public virtual bool IsCorruptEnough(float minimum, bool falseIfZero = false) =>

            Corruption >= minimum && !(falseIfZero && Corruption <= 0);

        public virtual bool IsTeen() => Age == Age.Teen;

        public virtual bool IsAdult() => Age == Age.Adult;

        public virtual bool IsElder() => Age == Age.Elder;

        public virtual bool HasCock() => Cocks.Count > 0;

        public virtual bool HasKnot(int arg = 0)
        {
            if (arg > Cocks.Count - 1 || arg < 0)
                return false;

            return Cocks[arg].KnotMultiplier > 1;
        }

        public virtual bool HasVagina() => Vaginas.Count > 0;

        public virtual bool HasBalls() => Balls > 0;

        public virtual bool HasGenitals() => HasCock() || HasVagina();

        public virtual bool HasClaws()
        {
            var arms = Body?.Get<ArmsPart>();

            return arms != null && arms.Claws.Id.Path != "normal";
        }

        public virtual bool IsUnarmed() => WeaponName == "fists";

        public virtual void UpdateUnarmed() { }

        public virtual bool CreateCock(float length = 5.5f, float thickness = 1f, Identifier type = default)
        {
            if (Cocks.Count >= 10)
                return false;

            if (string.IsNullOrEmpty(type.Namespace))
                type = BodyPartTypes.Human;

            var cock = BodyPartSlotRegistry.New<CockPart>(BodyPartSlots.Cock, type);

            cock.Body = Body;

            cock.Length = length;

            cock.Thickness = thickness;

            Cocks.Add(cock);

            return true;
        }

        public virtual bool CreateVagina(bool virgin = true, float wetness = 1f, int looseness = 0)
        {
            if (Vaginas.Count >= 2)
                return false;

            var vagina = BodyPartSlotRegistry.New<VaginaPart>(BodyPartSlots.Vagina);

            vagina.Body = Body;

            vagina.Virgin = virgin;

            vagina.Wetness = wetness;

            vagina.Looseness = looseness;

            Vaginas.Add(vagina);

            return true;
        }

        public virtual bool CreateBreastRow(float size = 0f, float nipplesPerBreast = 1f)
        {
            if (BreastRows.Count >= 10)
                return false;

            var row = BodyPartSlotRegistry.New<BreastRowPart>(BodyPartSlots.BreastRow);

            row.Body = Body;

            row.BreastRating = size;

            row.NipplesPerBreast = nipplesPerBreast;

            BreastRows.Add(row);

            return true;
        }

        public virtual void RemoveCock(int index = 0, int count = 1)
        {
            if (index < 0 || count <= 0 || index >= Cocks.Count)
                return;

            Cocks.RemoveRange(index, Math.Min(count, Cocks.Count - index));
        }

        public virtual void RemoveVagina(int index = 0, int count = 1)
        {
            if (index < 0 || count <= 0 || index >= Vaginas.Count)
                return;

            Vaginas.RemoveRange(index, Math.Min(count, Vaginas.Count - index));
        }

        public virtual void RemoveBreastRow(int index = 0, int count = 1)
        {
            if (index < 0 || count <= 0 || index >= BreastRows.Count)
                return;

            if (BreastRows.Count - count < 1)
                return;

            BreastRows.RemoveRange(index, Math.Min(count, BreastRows.Count - index));
        }

        public virtual void ClearGender()
        {
            Balls = 0;

            Cocks.Clear();

            Vaginas.Clear();

            while (BreastRows.Count > 1)

                BreastRows.RemoveAt(0);
        }

        public virtual string BreastCup(int row = 0)
        {
            if (BreastRows.Count == 0)
                return "Body.BreastCup.Zero".Localized();

            var rating = BreastRows[Math.Clamp(row, 0, BreastRows.Count - 1)].BreastRating;

            return $"Body.BreastCup.{Math.Clamp((int)MathF.Floor(rating), 0, 199)}".Localized();
        }

        public virtual string BreastCupForRating(float rating) =>

            $"Body.BreastCup.{Math.Clamp((int)MathF.Floor(rating), 0, 199)}".Localized();

        public virtual float BiggestBreastSize()
        {
            var biggest = -1f;

            foreach (var row in BreastRows)
                biggest = Math.Max(biggest, row.BreastRating);

            return biggest;
        }

        public virtual float GetClitLength(int index = 0)
        {
            if (!HasVagina() || index < 0 || index >= Vaginas.Count)
                return -1f;

            return Vaginas[index].ClitLength;
        }

        public virtual float SetClitLength(float length, int index = 0)
        {
            if (!HasVagina() || index < 0 || index >= Vaginas.Count)
                return -1f;

            Vaginas[index].ClitLength = length;

            return length;
        }

        public virtual Perk GetPerk(Identifier id)
        {
            foreach (var perk in Perks)
                if (perk.Id == id)
                    return perk;

            return null;
        }

        public virtual bool HasPerk(Identifier id) => GetPerk(id) != null;

        public string NipplesPShort = "";

        public string NipplesPLong = "";

        public string LipPShort = "";

        public string LipPLong = "";

        public string TonguePShort = "";

        public string TonguePLong = "";

        public string EyebrowPShort = "";

        public string EyebrowPLong = "";

        public string EarsPShort = "";

        public string EarsPLong = "";

        public string NosePShort = "";

        public string NosePLong = "";

        public virtual float Perkv4(Identifier id) => GetPerk(id)?.Values[3] ?? 0f;

        public virtual float Perkv1(Identifier id) => GetPerk(id)?.Values[0] ?? 0f;

        public virtual void CreatePerk(Identifier id, float value1 = 0f, float value2 = 0f, float value3 = 0f, float value4 = 0f)
        {
            if (HasPerk(id))
                return;

            var perk = new Perk
            {
                Id = id,

                Values = [value1, value2, value3, value4]
            };

            Perks.Add(perk);

            ApplyPerkBonuses(perk);
        }

        public virtual void ApplyPerkBonuses(Perk perk)
        {
            var definition = PerkRegistry.Get(perk.Id);

            if (definition == null)
                return;

            foreach (var bonus in definition.Bonuses)
                Stats.AddModifier(bonus.Stat, bonus.Operation, () => bonus.Value(this), new PerkSource(perk.Id, definition.Name), bonus.Visible);
        }

        public virtual void CreatePerkIfNotHasPerk(Identifier id, float value1 = 0f, float value2 = 0f, float value3 = 0f, float value4 = 0f)
        {
            if (!HasPerk(id))
                CreatePerk(id, value1, value2, value3, value4);
        }

        public virtual Mastery? GetMastery(Identifier id)
        {
            foreach (var mastery in Masteries)
                if (mastery.Id == id)
                    return mastery;

            return null;
        }

        public virtual bool HasMastery(Identifier id) => GetMastery(id) != null;

        public virtual int MasteryLevel(Identifier id) => GetMastery(id)?.Level ?? -1;

        // 原版 Creature.hx:1535 removeMasteries(ignorePerm)：false 时只保留已永久化的精通
        public virtual void RemoveMasteries(bool ignorePerm = true)
        {
            if (ignorePerm)
            {
                Masteries.Clear();

                return;
            }

            Masteries.RemoveAll(m => !m.Permed);
        }

        // 原版 Creature.hx:1599 permMastery：返回是否从“未永久”变为“永久”
        public virtual bool PermMastery(Identifier id)
        {
            var mastery = GetMastery(id);

            if (mastery == null || mastery.Permed)
                return false;

            mastery.Permed = true;

            return true;
        }

        public virtual int MasteryXP(Identifier id, int change = 0, bool announce = true)
        {
            var mastery = GetMastery(id);

            if (mastery == null)
            {
                if (change <= 0)
                    return -1;

                mastery = AddMastery(id, 0);
            }

            mastery.XP += change;

            if (mastery.XP < 0)
                mastery.XP = 0;

            return mastery.XP;
        }

        public virtual void RemovePerk(Identifier id)
        {
            var perk = GetPerk(id);

            if (perk == null)
                return;

            Perks.Remove(perk);

            Stats.RemoveFrom(new PerkSource(id, PerkRegistry.GetName(id)));
        }

        public virtual void RemovePerks()
        {
            foreach (var perk in Perks)
                Stats.RemoveFrom(new PerkSource(perk.Id, PerkRegistry.GetName(perk.Id)));

            Perks.Clear();
        }

        public virtual float PerkValue(Identifier id, int index = 1)
        {
            var perk = GetPerk(id);

            if (perk == null || index < 1 || index > 4)
                return 0f;

            return perk.Values[index - 1];
        }

        public virtual void AddPerkValue(Identifier id, int index, float delta)
        {
            var perk = GetPerk(id);

            if (perk == null || index < 1 || index > 4)
                return;

            perk.Values[index - 1] += delta;
        }

        public virtual StatusEffect StatusEffectByType(Identifier id)
        {
            foreach (var effect in StatusEffects)
                if (effect.Id == id)
                    return effect;

            return null;
        }

        public virtual bool HasStatusEffect(Identifier id) => StatusEffectByType(id) != null;

        public virtual float StatusEffectValue(Identifier id, int index)
        {
            var effect = StatusEffectByType(id);

            if (effect == null || index < 0 || index >= effect.Values.Length)
                return 0;

            return effect.Values[index];
        }

        public virtual void AddStatusValue(Identifier id, int index, float delta)
        {
            var effect = StatusEffectByType(id);

            if (effect == null || index < 0 || index >= effect.Values.Length)
                return;

            effect.Values[index] += delta;
        }

        public virtual void ChangeFatigue(float amount) => Fatigue = Math.Clamp(Fatigue + amount, 0, 100);

        public virtual float MaxFatigue()
        {
            var max = 100 + Stats.Addition(GameStats.FatigueMax);

            return max > 999 ? 999 : max;
        }

        public virtual float FatigueLeft() => MaxFatigue() - Fatigue;

        public virtual float PhysicalCost(float mod)
        {
            var costPercent = 100f;

            if (HasPerk(PerkIds.IronMan))
                costPercent -= 50;

            if (IsChild())
                costPercent *= 1.3f;

            if (IsElder())
                costPercent *= 1.2f;

            return mod * costPercent / 100f;
        }

        public virtual float SpellCost(float mod)
        {
            var costPercent = 100 + Stats.Addition(GameStats.SpellCost);

            if (HasPerk(PerkIds.BloodMage) && costPercent < 50)
                costPercent = 50;
            else if (costPercent < 25)
                costPercent = 25;

            mod *= costPercent / 100f;

            if (HasPerk(PerkIds.BloodMage) && mod < 5)
                mod = 5;
            else if (mod < 2)
                mod = 2;

            mod *= Stats.Multiplier(GameStats.SpellCost);

            return MathF.Round(mod * 100) / 100;
        }

        public virtual bool HasFatigue(float mod, FatigueType type = FatigueType.None)
        {
            if (type == FatigueType.None)
                return true;

            if (type is FatigueType.Magical or FatigueType.MagicalHeal)
                mod = SpellCost(mod);

            if (type == FatigueType.Physical)
                mod = PhysicalCost(mod);

            return Fatigue + mod < MaxFatigue();
        }

        public virtual float ChangeFatigue(float mod, FatigueType type)
        {
            if (type is FatigueType.Magical or FatigueType.MagicalHeal)
                mod = SpellCost(mod);

            if (type == FatigueType.Physical)
                mod = PhysicalCost(mod);

            var applied = mod;

            ChangeFatigue(applied);

            return applied;
        }

        public virtual float MovementChance(Creature enemy)
        {
            var baseChance = enemy.Speed <= 0 ? 100f : Speed / (enemy.Speed * 2) * 100f;

            baseChance += Stats.Addition(GameStats.MovementChance);

            return MathF.Round(baseChance * 100f) / 100f;
        }

        public virtual float TotalFertility()
        {
            var total = Fertility + Stats.Addition(GameStats.Fertility);

            if (HasPerk(PerkIds.FertilityPlus))
                total += 15;

            if (HasPerk(PerkIds.FertilityMinus) && Libido < 25)
                total -= 15;

            return Math.Max(0, total);
        }

        public virtual float Regeneration(bool combat = true, bool doHeal = true)
        {
            var maxRegen = combat ? 5f : 10f;

            var healingPercent = Stats.Addition(GameStats.HealthRegenPercent);

            var healBonus = Stats.Addition(GameStats.HealthRegenFlat);

            healingPercent = Math.Clamp(healingPercent, -maxRegen, maxRegen);

            if (doHeal)
                HealthChange(MathF.Round(MaxHealth * healingPercent / 100f) + healBonus);

            return healingPercent;
        }

        public virtual float HealthRatio() => MaxHealth <= 0 ? 1 : Health / MaxHealth;

        public virtual float LustRatio() => MaxLust <= 0 ? 0 : Lust / MaxLust;

        public virtual float SpellbladeBonus() => 1f;

        public virtual float PhysMod() => Stats.Factor(GameStats.PhysDamage);

        public virtual bool Stun(int duration = 1, int chance = 33, int challengeRoll = 100, bool applyPerks = true)
        {
            if ((!HasPerk(PerkIds.StunImmune) || !applyPerks) &&
                Random.Shared.Next(Math.Max(1, challengeRoll)) < chance &&

                !HasStatusEffect(StatusEffectIds.Stunned))
            {
                if (HasPerk(PerkIds.SoftSkull))
                    duration += 1;

                CreateStatusEffect(StatusEffectIds.Stunned, duration, applyPerks ? 1 : 0, 0, 0, duration);

                return true;
            }

            return false;
        }

        public virtual void Teased(float lustDelta) => ApplyTease(lustDelta);

        public virtual void ApplyTease(float lustDelta)
        {
            if (lustDelta <= 0)
                return;

            Lust = Math.Min(MaxLust, Lust + lustDelta * LustVulnerability);
        }

        public virtual bool CanMove() =>

            !IsImmobilized && !HasPerk(PerkIds.ImmovableObject) && !HasStatusEffect(StatusEffectIds.Stunned);

        public virtual float GetBaseCritChance() => 5f + Stats.Addition(GameStats.CriticalChance);

        public virtual float GetMeleeCritBonus() => Stats.Addition(GameStats.CritCWeapon);

        public virtual float ChanceToHit(Creature? defender = null)
        {
            var toHit = 95f;

            if (HasStatusEffect(StatusEffectIds.Blind) && !HasPerk(PerkIds.BlindImmune))
                toHit *= 0.33f;

            toHit += Stats.Get(GameStats.Accuracy);

            if (toHit < 10)
                toHit = 10;

            return toHit;
        }

        public virtual float GetEvasionChance()
        {
            if (HasStatusEffect(StatusEffectIds.TFSupercharging))
                return 0;

            var chance = Speed / 10f;

            chance += Stats.Addition(GameStats.Dodge);

            if (HasPerk(PerkIds.ExtraDodge))
                chance += PerkValue(PerkIds.ExtraDodge, 1);

            if (HasStatusEffect(StatusEffectIds.Backstab))
                chance += 25;

            // 原版 MothSilkDress.hx:14 boostsDodge(getDodgeBonus)：随敌人淫欲提高闪避
            if (this is Player armorPlayer)
                chance += Items.ArmorItemBonuses.Dodge(armorPlayer);

            return chance * Stats.Multiplier(GameStats.Dodge);
        }

        public virtual float StandardDodgeFunc(Creature attacker, float toHitBonus = 0) =>

            Math.Clamp(attacker.ChanceToHit(this) + toHitBonus - GetEvasionChance(), 0, 100);

        public virtual string? GetEvasionReason(Creature attacker, float toHitChance)
        {
            if (toHitChance > Random.Shared.Next(100))
                return null;

            var reasons = new List<string> { "Speed" };

            if (attacker.HasStatusEffect(StatusEffectIds.Blind) && !attacker.HasPerk(PerkIds.BlindImmune))
                reasons.Add("Blind");

            if (HasPerk(PerkIds.Evade))
                reasons.Add("Evade");

            if (HasPerk(PerkIds.Flexibility))
                reasons.Add("Flexibility");

            if (HasPerk(PerkIds.Misdirection) && ArmorName == "red, high-society bodysuit")
                reasons.Add("Misdirection");

            if (HasPerk(PerkIds.Unhindered) && ArmorDefense == 0)
                reasons.Add("Unhindered");

            return reasons[Random.Shared.Next(reasons.Count)];
        }

        public virtual bool CanParry()
        {
            if (HasStatusEffect(StatusEffectIds.Retribution))
                return false;

            if (HasStatusEffect(StatusEffectIds.TFSupercharging))
                return false;

            return HasPerk(PerkIds.Parry) && WeaponCanParry() && Speed >= 50 && Strength >= 50;
        }

        public virtual float ParryChance()
        {
            var chance = (Speed - 50) / 5f;

            chance += Stats.Addition(GameStats.Parry);

            chance *= Stats.Multiplier(GameStats.Parry);

            return chance;
        }

        public virtual bool CombatParry() => CanParry() && Random.Shared.NextDouble() * 100f < ParryChance();

        protected virtual bool WeaponCanParry() => true;

        public virtual bool AttackCountered(Creature defender)
        {
            if (defender.HasStatusEffect(StatusEffectIds.Retribution))
                return false;

            if ((defender.HasStatusEffect(StatusEffectIds.CounterAB)) && (Random.Shared.Next((int)Speed + 30) < Random.Shared.Next((int)defender.Speed)))
                {
                    defender.ChangeStatusValue(StatusEffectIds.CounterAB, 0, 1);

                    return true;
                }

            return false;
        }

        public virtual void ChangeStatusValue(Identifier id, int index, float delta)
        {
            var effect = StatusEffectByType(id);

            if (effect == null || index < 0 || index >= effect.Values.Length)
                return;

            effect.Values[index] += delta;
        }

        public virtual void SetPerkValue(Identifier id, int index, float value)
        {
            var perk = GetPerk(id);

            if (perk == null || index < 1 || index > 4)
                return;

            perk.Values[index - 1] = value;
        }

        public virtual void HealthChange(float amount)
        {
            // 原版 Creature.hx:5009 TFShell：一回合内 HP 变化归零
            if (HasStatusEffect(StatusEffectIds.TFShell))
                amount = 0;

            if (amount > 0 && HasPerk(PerkIds.HistoryHealer))
                amount *= 1.2f;

            Health = Math.Clamp(Health + amount, 0, MaxHealth);
        }

        public virtual bool Bleed(Creature? attacker, int duration = 3, float intensity = 1f, bool alwaysApply = false)
        {
            if (!alwaysApply && HasPerk(PerkIds.BleedImmune))
                return false;

            if (attacker != null && attacker.HasPerk(PerkIds.AntiCoagulant))
                duration += 1;

            CreateStatusEffect(StatusEffectIds.IzmaBleed, duration, intensity, 0, 0, duration);

            return true;
        }

        public virtual bool IsBleeding() => HasStatusEffect(StatusEffectIds.IzmaBleed);

        public virtual float BleedIntensity()
        {
            var total = 0f;

            var max = 0f;

            foreach (var status in StatusEffects)
            {
                if (status.Id != StatusEffectIds.IzmaBleed)
                    continue;

                var intensity = status.Values[1];

                max = MathF.Max(max, intensity);

                total += intensity;
            }

            return max + (total - max) / 10f;
        }

        public virtual int BleedDamage(bool maxOnly = false, bool minOnly = false)
        {
            var percent = Random.Shared.Next(2, 6);

            var intensity = BleedIntensity();

            if (maxOnly)
                percent = 5;

            if (minOnly)
                percent = 2;

            return (int)(MaxHealth * percent * intensity / 100f);
        }

        public virtual StatusEffect CreateOrFindStatusEffect(Identifier id, float value1 = 0f, float value2 = 0f, float value3 = 0f, float value4 = 0f)
        {
            var existing = StatusEffectByType(id);

            return existing ?? CreateStatusEffect(id, value1, value2, value3, value4);
        }

        public virtual void RemoveStatusEffect(Identifier id)
        {
            StatusEffects.RemoveAll(effect => effect.Id == id);

            CombatCalculator.OnStatusRemoved(this, id);
        }

        public virtual StatusEffect CreateStatusEffect(Identifier id, float value1 = 0f, float value2 = 0f, float value3 = 0f, float value4 = 0f, float duration = 0f)
        {
            var effect = StatusEffectByType(id);

            if (effect != null)
                return effect;

            effect = new StatusEffect
            {
                Id = id,

                Values = [value1, value2, value3, value4],

                Duration = duration
            };

            StatusEffects.Add(effect);

            return effect;
        }

        public virtual Mastery AddMastery(Identifier id, int level = 1)
        {
            var mastery = Masteries.Find(candidate => candidate.Id == id);

            if (mastery == null)
            {
                mastery = new Mastery { Id = id, Level = level };

                Masteries.Add(mastery);
            }
            else if (level > mastery.Level)
                mastery.Level = level;

            return mastery;
        }

        public virtual void AddStatusEffect(StatusEffect effect)
        {
            if (effect == null || StatusEffectByType(effect.Id) != null)
                return;

            StatusEffects.Add(effect);
        }

        public virtual void RemoveStatuses() => StatusEffects.Clear();

        public virtual string Gendered(string male, string female)
        {
            var malePoints = 0f;

            var femalePoints = 0f;

            if (HasCock())
                malePoints += 1f;

            if (HasBalls())
                malePoints += 0.5f;

            if (HasVagina())
                femalePoints += 1.5f;

            if (Femininity < (IsFemale() ? 45 : 50))
                malePoints += 1f;

            if (Femininity < (IsFemale() ? 25 : 35))
                malePoints += 1f;

            if (Femininity < (IsFemale() ? 5 : 10))
                malePoints += 1f;

            if (Femininity > (IsMale() ? 55 : 50))
                femalePoints += 1f;

            if (Femininity > (IsMale() ? 75 : 65))
                femalePoints += 1f;

            if (Femininity > (IsMale() ? 95 : 90))
                femalePoints += 1f;

            if (BiggestBreastSize() >= 1f)
                femalePoints += 1f;

            if (IsHerm())
                femalePoints += 0.1f;

            if (IsGenderless())
                malePoints += 0.1f;

            return femalePoints > malePoints ? female : male;
        }

        public virtual string GenderedOrNeuter(string male, string female, string neuter) => IsGenderless() ? neuter : Gendered(male, female);

        public virtual void Save(JsonObject json)
        {
            json["shortName"] = ShortName;

            json["quantifier"] = Quantifier;

            json["race"] = Race;

            json["age"] = Age.ToString();

            json["startingAge"] = StartingAge.ToString();

            json["fireResistance"] = FireResistance;

            json["health"] = Health;

            json["lust"] = Lust;

            json["fatigue"] = Fatigue;

            json["level"] = Level;

            json["experience"] = Experience;

            json["additionalExperience"] = AdditionalExperience;

            json["gems"] = Gems;

            json["tallness"] = Tallness;

            json["femininity"] = Femininity;

            json["tone"] = Tone;

            json["thickness"] = Thickness;

            json["fertility"] = Fertility;

            json["nippleLength"] = NippleLength;

            json["balls"] = Balls;

            json["ballSize"] = BallSize;

            json["cumMultiplier"] = CumMultiplier;

            json["hoursSinceCum"] = HoursSinceCum;

            json["distance"] = Distance.ToString();

            json["extraDistance"] = ExtraDistance;

            json["isFlying"] = IsFlying;

            json["isSilenced"] = IsSilenced;

            json["isCrippled"] = IsCrippled;

            json["isUnfocused"] = IsUnfocused;

            json["isClumsy"] = IsClumsy;

            var cocksJson = new JsonArray();

            foreach (var cock in Cocks)
            {
                var entry = new JsonObject();

                cock.Save(entry);

                cocksJson.Add(entry);
            }

            json["cocks"] = cocksJson;

            var vaginasJson = new JsonArray();

            foreach (var vagina in Vaginas)
            {
                var entry = new JsonObject();

                vagina.Save(entry);

                vaginasJson.Add(entry);
            }

            json["vaginas"] = vaginasJson;

            var breastRowsJson = new JsonArray();

            foreach (var row in BreastRows)
            {
                var entry = new JsonObject();

                row.Save(entry);

                breastRowsJson.Add(entry);
            }

            json["breastRows"] = breastRowsJson;

            var analJson = new JsonObject();

            Anal.Save(analJson);

            json["anal"] = analJson;

            var perksJson = new JsonArray();

            foreach (var perk in Perks)
            {
                var entry = new JsonObject();

                perk.Save(entry);

                perksJson.Add(entry);
            }

            json["perks"] = perksJson;

            var masteriesJson = new JsonArray();

            foreach (var mastery in Masteries)
            {
                var entry = new JsonObject();

                mastery.Save(entry);

                masteriesJson.Add(entry);
            }

            json["masteries"] = masteriesJson;

            var statusEffectsJson = new JsonArray();

            foreach (var effect in StatusEffects)
            {
                var entry = new JsonObject();

                effect.Save(entry);

                statusEffectsJson.Add(entry);
            }

            json["statusEffects"] = statusEffectsJson;

            var statsJson = new JsonObject();

            Stats.Save(statsJson);

            json["stats"] = statsJson;
        }

        public virtual void Load(JsonObject json)
        {
            ShortName = json.TryGetValue("shortName", out var shortName) ? (string)shortName : "You";

            Quantifier = json.TryGetValue("quantifier", out var quantifier) ? (string)quantifier : "a ";

            Race = json.TryGetValue("race", out var race) ? (string)race : "";

            Age = json.TryGetValue("age", out var age) ? Enum.Parse<Age>((string)age, true) : Age.Adult;

            StartingAge = json.TryGetValue("startingAge", out var startingAge) ? Enum.Parse<Age>((string)startingAge, true) : Age.Adult;

            FireResistance = json.TryGetValue("fireResistance", out var fireResistance) ? (float)fireResistance : 1;

            Health = json.TryGetValue("health", out var health) ? (float)health : 0;

            Lust = json.TryGetValue("lust", out var lust) ? (float)lust : 0;

            Fatigue = json.TryGetValue("fatigue", out var fatigue) ? (float)fatigue : 0;

            Level = json.TryGetValue("level", out var level) ? (float)level : 0;

            Experience = json.TryGetValue("experience", out var experience) ? (float)experience : 0;

            AdditionalExperience = json.TryGetValue("additionalExperience", out var additionalExperience) ? (float)additionalExperience : 0;

            Gems = json.TryGetValue("gems", out var gems) ? (int)gems : 0;

            Tallness = json.TryGetValue("tallness", out var tallness) ? (float)tallness : 0;

            Femininity = json.TryGetValue("femininity", out var femininity) ? (float)femininity : 0;

            Tone = json.TryGetValue("tone", out var tone) ? (float)tone : 0;

            Thickness = json.TryGetValue("thickness", out var thickness) ? (float)thickness : 0;

            Fertility = json.TryGetValue("fertility", out var fertility) ? (float)fertility : 10;

            NippleLength = json.TryGetValue("nippleLength", out var nippleLength) ? (float)nippleLength : 0.25f;

            Balls = json.TryGetValue("balls", out var balls) ? (float)balls : 0;

            BallSize = json.TryGetValue("ballSize", out var ballSize) ? (float)ballSize : 0;

            CumMultiplier = json.TryGetValue("cumMultiplier", out var cumMultiplier) ? (float)cumMultiplier : 1;

            HoursSinceCum = json.TryGetValue("hoursSinceCum", out var hoursSinceCum) ? (float)hoursSinceCum : 0;

            Distance = json.TryGetValue("distance", out var distance) ? Enum.Parse<CombatDistance>((string)distance, true) : CombatDistance.Melee;

            ExtraDistance = json.TryGetValue("extraDistance", out var extraDistance) ? (int)extraDistance : 0;

            IsFlying = json.TryGetValue("isFlying", out var isFlying) && (bool)isFlying;

            IsSilenced = json.TryGetValue("isSilenced", out var isSilenced) && (bool)isSilenced;

            IsCrippled = json.TryGetValue("isCrippled", out var isCrippled) && (bool)isCrippled;

            IsUnfocused = json.TryGetValue("isUnfocused", out var isUnfocused) && (bool)isUnfocused;

            IsClumsy = json.TryGetValue("isClumsy", out var isClumsy) && (bool)isClumsy;

            Cocks.Clear();

            if (json.TryGetValue("cocks", out var cocksValue) && cocksValue is JsonArray cocksArray)
                foreach (var item in cocksArray)
                {
                    if (item is not JsonObject cockJson)
                        continue;

                    var cock = BodyPartSlotRegistry.New<CockPart>(BodyPartSlots.Cock);

                    cock.Body = Body;

                    cock.Load(cockJson);

                    Cocks.Add(cock);
                }

            Vaginas.Clear();

            if (json.TryGetValue("vaginas", out var vaginasValue) && vaginasValue is JsonArray vaginasArray)
                foreach (var item in vaginasArray)
                {
                    if (item is not JsonObject vaginaJson)
                        continue;

                    var vagina = BodyPartSlotRegistry.New<VaginaPart>(BodyPartSlots.Vagina);

                    vagina.Body = Body;

                    vagina.Load(vaginaJson);

                    Vaginas.Add(vagina);
                }

            BreastRows.Clear();

            if (json.TryGetValue("breastRows", out var breastRowsValue) && breastRowsValue is JsonArray breastRowsArray)
                foreach (var item in breastRowsArray)
                {
                    if (item is not JsonObject breastRowJson)
                        continue;

                    var row = BodyPartSlotRegistry.New<BreastRowPart>(BodyPartSlots.BreastRow);

                    row.Body = Body;

                    row.Load(breastRowJson);

                    BreastRows.Add(row);
                }

            Anal.Load(json.TryGetValue("anal", out var analValue) && analValue is JsonObject analJson ? analJson : []);

            Perks.Clear();

            Stats.ClearModifiers();

            ApplyAgeModifiers();

            if (json.TryGetValue("perks", out var perksValue) && perksValue is JsonArray perksArray)
                foreach (var item in perksArray)
                {
                    if (item is not JsonObject perkJson)
                        continue;

                    var perk = new Perk();

                    perk.Load(perkJson);

                    Perks.Add(perk);

                    ApplyPerkBonuses(perk);
                }

            Masteries.Clear();

            if (json.TryGetValue("masteries", out var masteriesValue) && masteriesValue is JsonArray masteriesArray)
                foreach (var item in masteriesArray)
                {
                    if (item is not JsonObject masteryJson)
                        continue;

                    var mastery = new Mastery();

                    mastery.Load(masteryJson);

                    Masteries.Add(mastery);
                }

            StatusEffects.Clear();

            if (json.TryGetValue("statusEffects", out var statusEffectsValue) && statusEffectsValue is JsonArray statusEffectsArray)
                foreach (var item in statusEffectsArray)
                {
                    if (item is not JsonObject statusEffectJson)
                        continue;

                    var effect = new StatusEffect();

                    effect.Load(statusEffectJson);

                    StatusEffects.Add(effect);
                }

            Stats.Load(json.TryGetValue("stats", out var statsValue) && statsValue is JsonObject statsJson ? statsJson : []);
        }

        public virtual string SkinDescript(bool noAdj = false, bool noTone = false) => Body.Get<SkinPart>().Descript(noAdj, noTone);

        public virtual string SkinKind() => Body.Get<SkinPart>().Kind;

        public virtual string SkinFurScales() => Body.Get<SkinPart>().SkinFurScales();

        public virtual string HairDescript() => Body.Get<HairPart>().Descript();

        public virtual string BeardDescript() => Body.Get<BeardPart>().Descript();

        public virtual string TongueDescript() => Body.Get<TonguePart>().Descript();

        public virtual string FaceDescript() => Body.Get<FacePart>().Descript();

        public virtual string EyesDescript() => Body.Get<EyesPart>().Descript();

        public virtual string TailDescript() => Body.Get<TailPart>().Descript();

        public virtual string CockMultiNoun(int index = 0) =>

            Appearance.CockMultiNoun(Cocks[index].Id, this);

        public virtual bool HasFurryTail() => Body.Get<TailPart>().Id.Path is

            "horse" or "dog" or "cow" or "cat" or "rabbit" or "kangaroo" or "fox" or "raccoon"

            or "ferret" or "goat" or "deer" or "wolf" or "sheep" or "red_panda" or "coc:gnoll";

        public virtual bool HasStinger() => Body.Get<TailPart>().Id.Path is "scorpion" or "bee_abdomen";

        public virtual string WingsDescript() => Body.Get<WingsPart>().Descript();

        public virtual string HornsDescript() => Body.Get<HornsPart>().Descript();

        public virtual string NeckDescript() => Body.Get<NeckPart>().Descript();

        public virtual string RearBodyDescript() => Body.Get<RearBodyPart>().Descript();

        public virtual string ClawsDescript(bool plural = true) => Body.Get<ArmsPart>().Claws.ClawsDescript(plural);

        public virtual string LowerBodyDescript() => Body.Get<LowerBodyPart>().Descript();

        public virtual string Legs() => Body.Get<LowerBodyPart>().Legs();

        public virtual string UnderBodyDescript() => Body.Get<UnderBodyPart>().Descript();

        public virtual int BiggestTitRow()
        {
            var index = 0;

            for (var i = 1; i < BreastRows.Count; i++)
                if (BreastRows[i].BreastRating > BreastRows[index].BreastRating)
                    index = i;

            return index;
        }

        public virtual string BreastDescript(int row = 0)
        {
            if (BreastRows.Count == 0)
                return "flat breasts";

            return BreastRows[Math.Clamp(row, 0, BreastRows.Count - 1)].Descript();
        }

        public virtual string AllBreastsDescript() => Appearance.AllBreastsDescript(this);

        public virtual float AscensionFactor(float multiplier = 25) => NewGamePlusMod() * multiplier;

        public virtual int NewGamePlusMod() =>

            Math.Clamp(CoC.Instance.Flags.Get(GameFlagIds.NewGamePlusLevel), 0, 4);

        public virtual string ChestDesc() => BiggestBreastSize() < 1f ? "chest" : Appearance.BiggestBreastSizeDescript(this);

        public virtual string AllChestDesc() => BiggestBreastSize() < 1f ? "chest" : AllBreastsDescript();

        public virtual string ButtDescript() => Appearance.ButtDescript(this);

        public virtual string AssDescript() => ButtDescript();

        public virtual string HipDescript() => Appearance.HipDescript(this);

        public virtual string ClitDescript() => Appearance.ClitDescript(this);

        public virtual string VaginaDescript(int index = 0) => Appearance.VaginaDescript(this, index);

        public virtual string WetnessDescript(int index = 0) => Appearance.WetnessDescript(Vaginas[index]);

        public virtual string BallsDescript(bool plural = true, bool withArticle = false) => Appearance.BallsDescript(this, plural, withArticle);

        public virtual string BallsDescriptLight(bool forcedSize = true, bool withArticle = false) => Appearance.BallsDescriptLight(this, forcedSize, withArticle);

        public virtual string SimpleBallsDescript(bool withArticle = false) => Appearance.SimpleBallsDescript(this, withArticle);

        public virtual string SackDescript() => Appearance.SackDescript(this);

        public virtual string EarsDescript() => Ears.Descript();

        public virtual string SheathDescript() => Cocks.Count == 0 ? "" : Appearance.SheathDescript(Cocks[0]);

        public virtual float BiggestLactation()
        {
            var value = 0f;

            foreach (var row in BreastRows)
                value = Math.Max(value, row.Lactation);

            return value;
        }

        public virtual float AverageLactation()
        {
            if (BreastRows.Count == 0)
                return 0;

            var sum = 0f;

            foreach (var row in BreastRows)
                sum += row.Lactation;

            return MathF.Floor(sum / BreastRows.Count);
        }

        public virtual float TotalNipples()
        {
            var count = 0f;

            foreach (var row in BreastRows)
                count += row.Breasts * row.NipplesPerBreast;

            return count;
        }

        public virtual bool HasFuckableNipples()
        {
            foreach (var row in BreastRows)
                if (row.Fuckable)
                    return true;

            return false;
        }

        public virtual bool HasBreasts() => BreastRows.Count > 0 && BiggestBreastSize() >= 1;

        public virtual float BoostLactation(float todo)
        {
            if (BreastRows.Count == 0)
                return 0;

            var index = 0;

            var changes = 0f;

            if (todo >= 0)
            {
                var reduction = StatusEffectByType(StatusEffectIds.LactationReduction);

                reduction?.Values[0] = 0;

                RemoveStatusEffect(StatusEffectIds.LactationReduc0);

                RemoveStatusEffect(StatusEffectIds.LactationReduc1);

                RemoveStatusEffect(StatusEffectIds.LactationReduc2);

                RemoveStatusEffect(StatusEffectIds.LactationReduc3);
            }

            if (todo > 0)
                while (todo > 0)
                {
                    var counter = BreastRows.Count;

                    todo -= 0.1f;

                    while (counter > 0)
                    {
                        counter--;

                        if (BreastRows[index].Lactation > BreastRows[counter].Lactation)
                            index = counter;
                    }

                    var temp2 = 0.1f;

                    if (BreastRows[index].Lactation > 1.5f)
                        temp2 /= 2;

                    if (BreastRows[index].Lactation > 2.5f)
                        temp2 /= 2;

                    if (BreastRows[index].Lactation > 3)
                        temp2 /= 2;

                    changes += temp2;

                    BreastRows[index].Lactation += temp2;
                }
            else
                while (todo < 0)
                {
                    var counter = BreastRows.Count;

                    index = 0;

                    if (todo > -0.1f)
                    {
                        while (counter > 0)
                        {
                            counter--;

                            if (BreastRows[index].Lactation < BreastRows[counter].Lactation)
                                index = counter;
                        }

                        BreastRows[index].Lactation += todo;

                        if (BreastRows[index].Lactation < 0)
                            BreastRows[index].Lactation = 0;

                        todo = 0;
                    }
                    else
                    {
                        todo += 0.1f;

                        while (counter > 0)
                        {
                            counter--;

                            if (BreastRows[index].Lactation < BreastRows[counter].Lactation)
                                index = counter;
                        }

                        changes += todo;

                        BreastRows[index].Lactation += todo;

                        if (BreastRows[index].Lactation < 0)
                            BreastRows[index].Lactation = 0;
                    }
                }

            return changes;
        }

        public virtual void Milked()
        {
            var reduction = StatusEffectByType(StatusEffectIds.LactationReduction);

            reduction?.Values[0] = 0;

            RemoveStatusEffect(StatusEffectIds.LactationReduc0);

            RemoveStatusEffect(StatusEffectIds.LactationReduc1);

            RemoveStatusEffect(StatusEffectIds.LactationReduc2);

            RemoveStatusEffect(StatusEffectIds.LactationReduc3);

            if (HasPerk(PerkIds.Feeder))
            {
                var feeder = StatusEffectByType(StatusEffectIds.Feeder);

                if (feeder == null)
                    feeder = CreateStatusEffect(StatusEffectIds.Feeder);

                feeder.Values[0] += 1;

                feeder.Values[1] = 0;
            }
        }

        public virtual float CumCapacity()
        {
            if (!HasCock())
                return 0;

            var capacity = Balls > 0

                ? MathF.Pow(4f / 3f * MathF.PI * (BallSize / 2f), 3) * Balls

                : MathF.Pow(4f / 3f * MathF.PI, 3) * 2;

            if (HasPerk(PerkIds.BroBody))
                capacity *= 1.3f;

            if (HasPerk(PerkIds.ParasiteMusk))
                capacity *= 1.2f;

            if (HasPerk(PerkIds.FertilityPlus))
                capacity *= 1.5f;

            if (HasPerk(PerkIds.FertilityMinus) && Libido < 25)
                capacity *= 0.7f;

            if (HasPerk(PerkIds.MessyOrgasms))
                capacity *= 1.5f;

            if (HasPerk(PerkIds.OneTrackMind))
                capacity *= 1.1f;

            if (HasPerk(PerkIds.MaraesGiftStud))
                capacity += 350;

            if (HasPerk(PerkIds.FerasBoonAlpha))
                capacity += 200;

            if (HasPerk(PerkIds.MagicalVirility))
                capacity += 200;

            if (HasPerk(PerkIds.FerasBoonSeeder))
                capacity += 1000;

            capacity += PerkValue(PerkIds.ElvenBounty, 1);

            if (HasPerk(PerkIds.BroBody))
                capacity += 200;

            capacity += StatusEffectValue(StatusEffectIds.Rut, 0);

            capacity *= 1 + 2 * PerkValue(PerkIds.PiercedFertite, 1) / 100f;

            if (JewelryEffectId == JewelryIds.FertilityEffect)
                capacity *= 1 + JewelryEffectMagnitude / 100f;

            capacity *= CumMultiplier;

            return Math.Min(capacity, int.MaxValue);
        }

        public virtual int VirilityQ()
        {
            if (!HasCock())
                return 0;

            var percent = (int)Math.Floor(CumQ() / 100);

            var simpleBonuses = new (Identifier Perk, int Value)[]
            {
                (PerkIds.BroBody, 5),

                (PerkIds.MaraesGiftStud, 15),

                (PerkIds.FerasBoonAlpha, 10),

                (PerkIds.FertilityPlus, 3),

                (PerkIds.PiercedFertite, 3),

                (PerkIds.OneTrackMind, 3),

                (PerkIds.MothBedding, 10),

                (PerkIds.MessyOrgasms, 3),

                (PerkIds.SatyrSexuality, 10)
            };

            foreach (var (perk, value) in simpleBonuses)
                if (HasPerk(perk))
                    percent += value;

            if (PerkValue(PerkIds.ElvenBounty) > 0)
                percent += 5;

            if (HasPerk(PerkIds.FertilityMinus) && LustPercent < 25)
                percent -= 3;

            if (HasPerk(PerkIds.MagicalVirility))
                percent += (int)(5 + PerkValue(PerkIds.MagicalVirility));

            if ((int)JewelryEffectId == 2)
                percent += (int)JewelryEffectMagnitude;

            if (HasPerk(PerkIds.AscensionVirility))
                percent += (int)(PerkValue(PerkIds.AscensionVirility) * 5);

            if (InRut)
                percent += 10;

            return Math.Clamp(percent, 0, 100);
        }

        public virtual float CumQ()
        {
            if (!HasCock())
                return 0;

            var lustCoefficient = (Lust + 50) / 10f;

            if (OptionStore.Instance.Get("realistic", 0) > 0)
            {
                lustCoefficient = (Lust + 50) / 5f;

                if (HasPerk(PerkIds.PilgrimsBounty))
                    lustCoefficient = 30;

                var percent = lustCoefficient + (HoursSinceCum + 10);

                if (percent > 100)
                    percent = 100;

                return percent / 100f * CumCapacity();
            }

            if (HasPerk(PerkIds.PilgrimsBounty))
                lustCoefficient = 150 / 10f;

            float quantity;

            if ((Balls == 0 || HasStatusEffect(StatusEffectIds.Uniball)) && HasPerk(PerkIds.PotentProstate))
                quantity = (int)(4 * 2 * CumMultiplier * 2 * lustCoefficient * (HoursSinceCum + 10) / 24f) / 10f;
            else if (Balls == 0)
                quantity = (int)(1.25f * 2 * CumMultiplier * 2 * lustCoefficient * (HoursSinceCum + 10) / 24f) / 10f;
            else
                quantity = (int)(BallSize * Balls * CumMultiplier * 2 * lustCoefficient * (HoursSinceCum + 10) / 24f) / 10f;

            if (HasPerk(PerkIds.BroBody))
                quantity *= 1.3f;

            if (HasPerk(PerkIds.FertilityPlus))
                quantity *= 1.5f;

            if (HasPerk(PerkIds.FertilityMinus) && Libido < 25)
                quantity *= 0.7f;

            if (HasPerk(PerkIds.MessyOrgasms))
                quantity *= 1.5f;

            if (HasPerk(PerkIds.OneTrackMind))
                quantity *= 1.1f;

            if (HasPerk(PerkIds.ParasiteMusk))
                quantity *= 1.2f;

            if (HasPerk(PerkIds.MaraesGiftStud))
                quantity += 350;

            if (HasPerk(PerkIds.FerasBoonAlpha))
                quantity += 200;

            if (HasPerk(PerkIds.MagicalVirility))
                quantity += 200 + PerkValue(PerkIds.MagicalVirility, 1) * 100;

            if (HasPerk(PerkIds.FerasBoonSeeder))
                quantity += 1000;

            quantity += PerkValue(PerkIds.ElvenBounty, 1);

            if (HasPerk(PerkIds.BroBody))
                quantity += 200;

            if (HasPerk(PerkIds.SatyrSexuality))
                quantity += 50;

            quantity += StatusEffectValue(StatusEffectIds.Rut, 0);

            quantity *= 1 + 2 * PerkValue(PerkIds.PiercedFertite, 1) / 100f;

            if (JewelryEffectId == JewelryIds.FertilityEffect)
                quantity *= 1 + JewelryEffectMagnitude / 100f;

            if (quantity < 2)
                quantity = 2;

            return Math.Min(quantity, int.MaxValue);
        }

        public virtual void Orgasm(string type = "Default", bool real = true, int times = 1)
        {
            var lustDrain = 1f;

            if (HasStatusEffect(StatusEffectIds.ParasiteSlugReproduction) && type != "Anal")
                lustDrain = 0;

            switch (type)
            {
                case "Anal":
                    if (HasStatusEffect(StatusEffectIds.ParasiteSlugReproduction))
                        StatusEffectByType(StatusEffectIds.ParasiteSlugReproduction).Values[0] = 1;

                    break;

                case "Vaginal":
                case "Dick":
                case "Lips":
                case "Tits":
                case "Nipples":
                case "Ovi":
                    break;

                case "All":
                    if (HasCock())
                        Orgasm("Dick");

                    if (HasVagina())
                        Orgasm("Vaginal");

                    Orgasm("Anal");

                    return;

                case "DickAndAnal":
                    Orgasm("Anal");

                    if (HasCock())
                        Orgasm("Dick");

                    return;

                case "VaginalAndAnal":
                    Orgasm("Anal");

                    if (HasVagina())
                        Orgasm("Vaginal");

                    return;

                case "VaginalAndDick":
                    if (HasCock())
                        Orgasm("Dick");

                    if (HasVagina())
                        Orgasm("Vaginal");

                    return;

                case "VaginalAnal":
                    Orgasm(HasVagina() ? "Vaginal" : "Anal", real);

                    return;

                case "DickAnal":
                    Orgasm(Random.Shared.Next(2) == 0 ? "Dick" : "Anal", real);

                    return;

                case "DickVaginal":
                    Orgasm(HasCock() ? "Dick" : "Vaginal");

                    return;

                default:
                    if (!HasVagina() && !HasCock())
                    {
                        Orgasm("Anal");

                        return;
                    }

                    if (HasVagina() && HasCock())
                    {
                        Orgasm(Random.Shared.Next(2) == 0 ? "Vaginal" : "Dick", real);

                        return;
                    }

                    Orgasm(HasVagina() ? "Vaginal" : "Dick", real);

                    return;
            }

            if (real)
                OrgasmReal(times, lustDrain);
        }

        public virtual void OrgasmReal(int times = 1, float amount = 1)
        {
            ApplyStats(
                new DynStatsOptions(Scale: false, EnforceMax: true),

                new DynStat(GameStats.Lust, 1 - amount, DynStatOp.Mul));

            HoursSinceCum = 0;

            var flags = CoC.Instance.Flags;

            flags.Set(GameFlagIds.TimesOrgasmed, flags.Get(GameFlagIds.TimesOrgasmed) + times);

            var controller = CombatController.Instance;

            if (this is Player player &&
                CombatController.InCombat &&

                controller != null &&

                controller.Context.Enemy != null &&

                controller.Context.Player == player)
            {
                var enemy = controller.Context.Enemy;

                if (player.HasPerk(PerkIds.DemonBiology))
                {
                    player.ChangeFatigue(-enemy.Lust / 2);

                    player.RefillHunger(enemy.Lust, true, false);
                }

                if (enemy.Gender == Gender.Male)
                    player.SexOrientation += 1;

                if (enemy.Gender == Gender.Female)
                    player.SexOrientation -= 1;

                player.SexOrientation = Math.Clamp(player.SexOrientation, 0, 100);
            }

            var gilded = 0;

            foreach (var cock in Cocks)
                if (cock.Sock == "gilded")
                    gilded += 1;

            if (gilded > 0)
            {
                var index = Random.Shared.Next(Cocks.Count);

                var bonusGems = Random.Shared.Next((int)Cocks[index].Thickness) + gilded;

                if (Scene.View != null)
                    Scene.OutputText("Sex.Orgasm.GildedSocks".Localized(CockDescript(index), bonusGems));

                Gems += bonusGems;
            }
        }

        public virtual string CockDescript(int index = 0) => Appearance.CockDescript(this, index);

        public virtual string CockClit(int number = 0) =>

            HasCock() && number >= 0 && number < Cocks.Count ? CockDescript(number) : ClitDescript();

        public virtual string GenderText(string male = "man", string female = "woman", string futa = "herm", string eunuch = "eunuch") =>

            HasVagina() ? (HasCock() ? futa : female) : HasCock() ? male : eunuch;

        public virtual string AssholeOrPussy() => Appearance.AssholeOrPussy(this);

        public virtual int ThickestCock()
        {
            var best = -1;

            var thickness = -1f;

            for (var i = 0; i < Cocks.Count; i++)
            {
                if (Cocks[i].Thickness <= thickness)
                    continue;

                thickness = Cocks[i].Thickness;

                best = i;
            }

            return best < 0 ? 0 : best;
        }

        public virtual float ThickestCockThickness() => Cocks.Count == 0 ? 0f : Cocks[ThickestCock()].Thickness;

        public virtual string CockDescriptShort(int index = 0) => Appearance.CockDescriptShort(this, index);

        public virtual string KnotWord(int index = 0)
        {
            if (index < 0 || index >= Cocks.Count)
                return "base";

            var cock = Cocks[index];

            if (!cock.HasKnot)
                return "base";

            if (OptionStore.Instance.Get("silly", 0) > 0)
                return RandomUtils.Choice(
                [
                    "pussy-plug", "cunt-stretcher", "spunk-seal", "breeding-bulb", "snatch-stretcher",

                    "twat-tie", "cock-lock", "hole-hitch", "fuck-fastener", "slut-stuffer",

                    "bitch-breaker", "cum-cork"
                ]);

            return cock.Id == BodyPartTypes.Dog || cock.Id == BodyPartTypes.Fox

                || cock.Id == new Identifier("coc", "wolf")

                ? "knot"

                : cock.Id == BodyPartTypes.Lizard || cock.Id == BodyPartTypes.Dragon

                    ? "bulb"

                    : "knot";
        }

        public virtual string CockAnimalityAdj(int index = 0)
        {
            if (index < 0 || index >= Cocks.Count)
                return "";

            var type = Cocks[index].Id;

            string[] choices = type == BodyPartTypes.Dog ? ["canine ", "canid ", "bestial ", "beastly ", "animalistic "]

                : type == BodyPartTypes.Fox ? ["vulpine ", "bestial ", "animalistic ", "foxy "]

                : type == BodyPartTypes.Demon ? ["demonic ", "corrupted ", "unholy ", "vile ", "blighted ", "perverse "]

                : type == BodyPartTypes.Dragon ? ["draconic ", "mythic ", "savage ", "reptilian "]

                : type == BodyPartTypes.Lizard ? ["reptilian ", "serpentine ", "saurian "]

                : type == BodyPartTypes.Cat ? ["felid ", "feline ", "animalistic "]

                : type == BodyPartTypes.Horse ? ["equine ", "stallion ", "bestial ", "animalistic "]

                : type == BodyPartTypes.Bee ? ["bee ", "insectoid "]

                : type == BodyPartTypes.Kangaroo ? ["marsupial ", "mammalian ", "roo "]

                : type == new Identifier("coc", "anemone") ? ["aquatic ", "venomous ", "blue "]

                : type == new Identifier("coc", "displacer") ? ["alien ", "beastly ", "bizarre "]

                : type == new Identifier("coc", "tentacle") ? ["plantlike ", "corrupted ", "unusual "]

                : type == new Identifier("coc", "gnoll") ? ["animalistic ", "bestial ", "savage "]

                : [];

            return choices.Length == 0 ? "" : RandomUtils.Choice(choices);
        }

        public virtual string HandsDescriptShort(bool plural = true)
        {
            var arms = Body?.Get<ArmsPart>();

            var word = "hand";

            if (arms != null)
            {
                var armsId = arms.Id;

                var clawsId = arms.Claws.Id;

                if (armsId.Path == "predator")
                    word = clawsId == new Identifier("coc", "cockatrice") ? "talon" : "claw";
                else if (armsId == BodyPartTypes.Dragon || armsId == BodyPartTypes.Lizard
                    || armsId == new Identifier("coc", "salamander"))

                    word = "claw";
                else if (armsId == new Identifier("coc", "wolf") || armsId == new Identifier("coc", "red_panda")
                    || armsId == BodyPartTypes.Cat || armsId == BodyPartTypes.Dog

                    || armsId == BodyPartTypes.Fox || armsId == BodyPartTypes.Ferret

                    || armsId == new Identifier("coc", "gnoll"))

                    word = "paw";
                else if (armsId == new Identifier("coc", "cockatrice"))
                    word = "talon";
            }

            return word + (plural ? "s" : "");
        }

        public virtual string CockHead(int index = 0) => Appearance.CockHead(this, index);

        public virtual string MultiCockDescript() => Appearance.MultiCockDescript(this);

        public virtual string MultiCockDescriptLight(int excludeIndex = -1) => Appearance.MultiCockDescriptLight(this, excludeIndex);

        public virtual string SMultiCockDesc() =>

            (Cocks.Count > 1 ? "Appearance.Cock.MultiPrefix.OneOfYour" : "Appearance.Cock.MultiPrefix.Your").Localized() + Appearance.CockMultiLDescriptionShort(this);

        public virtual int CountCockSocks(string type)
        {
            var count = 0;

            foreach (var cock in Cocks)
                if (cock.Sock == type)
                    count++;

            return count;
        }

        public virtual IReadOnlyList<int> CockIndicesBySize() =>

            Enumerable.Range(0, Cocks.Count)
                .OrderByDescending(index => Cocks[index].Length)
                .ToList();

        public virtual int BiggestCockIndex()
        {
            if (Cocks.Count == 0)
                return 0;

            var index = 0;

            for (var current = 1; current < Cocks.Count; current++)
                if (Cocks[current].Length > Cocks[index].Length)
                    index = current;

            return index;
        }

        public virtual int[] CocksBySize()
        {
            var order = new int[Cocks.Count];

            for (var index = 0; index < order.Length; index++)
                order[index] = index;

            Array.Sort(order, (a, b) => CockArea(b).CompareTo(CockArea(a)));

            return order;
        }

        public virtual int BiggestCockIndex2() => Cocks.Count <= 1 ? 0 : CocksBySize()[1];

        public virtual int BiggestCockIndex3() => Cocks.Count <= 2 ? 0 : CocksBySize()[2];

        public virtual int RandomCockThatFits(float fits = 0, string type = "area") =>

            RandomCockByComparison(fits, type, true);

        public virtual int RandomCockTooBig(float noFits = 0, string type = "area") =>

            RandomCockByComparison(noFits, type, false);

        private int RandomCockByComparison(float limit, string type, bool wantFits)
        {
            if (Cocks.Count <= 0)
                return -1;

            var cockArray = new List<int>();

            for (var index = Cocks.Count - 1; index >= 0; index--)
            {
                var value = type switch
                {
                    "area" => CockArea(index),

                    "length" => Cocks[index].Length,

                    "width" => Cocks[index].Thickness,

                    _ => -1f,
                };

                if (value < 0)
                    continue;

                if (wantFits ? value <= limit : value > limit)
                    cockArray.Add(index);
            }

            return cockArray.Count > 0 ? cockArray[Utils.Utils.Rand(cockArray.Count)] : -1;
        }

        public virtual int SmallestCockIndex()
        {
            if (Cocks.Count == 0)
                return 0;

            var index = 0;

            for (var current = 1; current < Cocks.Count; current++)
                if (Cocks[current].Length < Cocks[index].Length)
                    index = current;

            return index;
        }

        public virtual string NippleDescript(int row = 0, bool plural = false) => Appearance.NippleDescript(this, row, plural);

        public virtual string Nipples(int row = 0) => Appearance.NippleNouns(this, row);

        public virtual string AssholeDescript(bool forceDesc = false) => Anal.AssholeDescript(forceDesc);

        public virtual string HairShort() => Body.Get<HairPart>().HairShort();

        public virtual string HairOrFur() => Body.Get<SkinPart>().IsFluffy ? "BodyParts.Descript.HairOrFur.Fur".Localized() : "BodyParts.Descript.HairOrFur.Hair".Localized();

        public virtual string HairOrFurColor()
        {
            var hair = Body.Get<HairPart>();

            if (!Body.Get<SkinPart>().IsFluffy)
                return hair.Color;

            var fur = Body.Get<SkinPart>().FurColor;

            return fur == "no" ? hair.Color : fur;
        }

        public virtual string Leg() => Body.Get<LowerBodyPart>().Leg();

        public virtual string Feet() => Body.Get<LowerBodyPart>().Feet();

        public virtual string Foot() => Body.Get<LowerBodyPart>().Foot();

        public virtual string AgeDesc(string child = " child", string teen = " teenager", string adult = "", string elder = " elder") => Age switch
        {
            Age.Child => child,

            Age.Teen => teen,

            Age.Elder => elder,

            _ => adult
        };

        public virtual string Mf(string male, string female)
        {
            var malePoints = 0f;

            var femalePoints = 0f;

            if (HasCock())
                malePoints += 1;

            if (Balls > 0)
                malePoints += 0.5f;

            if (HasVagina())
                femalePoints += 1.5f;

            if (Femininity < (IsFemale() ? 45 : 50))
                malePoints += 1;

            if (Femininity < (IsFemale() ? 25 : 35))
                malePoints += 1;

            if (Femininity < (IsFemale() ? 5 : 10))
                malePoints += 1;

            if (Femininity > (IsMale() ? 55 : 50))
                femalePoints += 1;

            if (Femininity > (IsMale() ? 75 : 65))
                femalePoints += 1;

            if (Femininity > (IsMale() ? 95 : 90))
                femalePoints += 1;

            if (BiggestBreastSize() >= 1f)
                femalePoints += 1;

            if (IsHerm())
                femalePoints += 0.1f;

            if (IsGenderless())
                malePoints += 0.1f;

            return femalePoints > malePoints ? female : male;
        }

        public virtual string MaleFemaleHerm(bool caps = false)
        {
            var word = Gender switch
            {
                Gender.Genderless => Mf("genderless", "fem-genderless"),

                Gender.Male => Mf("male", BiggestBreastSize() > 1f ? "shemale" : "femboy"),

                Gender.Female => Mf("cuntboy", "female"),

                Gender.Hermaphrodite => Mf("maleherm", "hermaphrodite"),

                _ => "genderless"
            };

            return caps && word.Length > 0 ? char.ToUpperInvariant(word[0]) + word[1..] : word;
        }

        // ===== 原版 Character.hx:655 growHair() / :675 growBeard()：毛发增长与阈值提示 =====

        /// <summary>原版 Character.hx:655 growHair()：蛇怪棘刺不长发，蛇怪羽冠上限 8。</summary>
        public virtual bool GrowHair(float amount = 0.1f)
        {
            var hair = Body.Get<HairPart>();

            var tempHair = hair.Length;

            if (hair.Id == BodyPartTypes.BasiliskSpines)
                return false;

            hair.Length += amount;

            if (hair.Id == BodyPartTypes.BasiliskPlume && hair.Length > 8)
                hair.Length = 8;

            if (hair.Length > 0 && tempHair == 0)
            {
                Scene.OutputText("PlayerEvents.Hair.NoLongerBald".Localized(HairDescript()));

                return true;
            }

            // 原版 :664：跨越 1 / 3 / 6 / 10 英寸阈值时提示
            if ((hair.Length >= 1 && tempHair < 1) || (hair.Length >= 3 && tempHair < 3)
                || (hair.Length >= 6 && tempHair < 6) || (hair.Length >= 10 && tempHair < 10))
            {
                Scene.OutputText("PlayerEvents.Hair.NewThreshold".Localized(HairDescript()));

                return true;
            }

            return false;
        }

        /// <summary>原版 Character.hx:675 growBeard()：跨越 0.2 / 0.5 / 1.5 阈值时提示。</summary>
        public virtual bool GrowBeard(float amount = 0.1f)
        {
            var beard = Body.Get<BeardPart>();

            var tempBeard = beard.Length;

            beard.Length += amount;

            if (beard.Length > 0 && tempBeard == 0)
            {
                Scene.OutputText("PlayerEvents.Beard.FirstGrowth".Localized(BeardDescript()));

                return true;
            }

            if ((beard.Length >= 0.2f && tempBeard < 0.2f) || (beard.Length >= 0.5f && tempBeard < 0.5f)
                || (beard.Length >= 1.5f && tempBeard < 1.5f))
            {
                Scene.OutputText("PlayerEvents.Beard.NewThreshold".Localized(BeardDescript()));

                return true;
            }

            return false;
        }
    }

    public enum Age
    {
        Adult,

        Child,

        Teen,

        Elder
    }

    public enum Gender
    {
        Genderless,

        Male,

        Female,

        Hermaphrodite,

        HERM = Hermaphrodite
    }

    public enum Build
    {
        Small,

        Medium,

        Large,

        Andro

    }
}
