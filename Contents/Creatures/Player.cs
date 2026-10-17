using MonoCoC.Contents.Storage;
using Hjson;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Items.Behaviors;
using MonoCoC.Registry;
using MonoCoC.Saving.Legacy;
using MonoCoC.Contents.BodyParts;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.World.Cabin;
using MonoCoC.Contents.Codex;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using MonoCoC.Contents.Npcs;
using MonoCoC.Contents.Stats;

namespace MonoCoC.Contents.Creatures
{
    public partial class Player : Creature
    {

        public virtual string LowMedHighCum(string lowTxt, string medTxt, string highTxt, float lowAmount = 100f, float medAmount = 500f)
        {
            var q = CumQ();

            if (q <= lowAmount)
                return lowTxt;

            if (q <= medAmount)
                return medTxt;

            return highTxt;
        }

        public virtual string LowMedHighCor(string lowTxt, string medTxt, string highTxt)
        {
            if (Corruption <= 30)
                return lowTxt;

            if (Corruption <= 60)
                return medTxt;

            return highTxt;
        }

        public virtual string TextByWetnessVagina(string dry, string normal, string wet, string slick, string drooling, string slavering)
        {
            var w = Body.Get<VaginaPart>().Wetness;

            return w switch
            {
                <= 0f => dry,

                <= 1f => normal,

                <= 2f => wet,

                <= 3f => slick,

                <= 4f => drooling,

                _ => slavering
            };
        }

        public virtual bool CanDepositEel() =>

            HasStatusEffect(StatusEffectIds.ParasiteEel) && StatusEffectValue(StatusEffectIds.ParasiteEel, 0) > 1;

        public virtual void DepositEel()
        {
            if (!CanDepositEel())
                return;

            var eelCount = StatusEffectValue(StatusEffectIds.ParasiteEel, 0);

            ApplyStats(
                new DynStat(GameStats.Speed, 1f),

                new DynStat(GameStats.Toughness, 1f),

                new DynStat(GameStats.Strength, 1f));

            ChangeStatusValue(StatusEffectIds.ParasiteEel, 0, -1);

            if (eelCount > 0)

                ChangeStatusValue(
                    StatusEffectIds.ParasiteEelNeedCum,

                    2,

                    -(StatusEffectValue(StatusEffectIds.ParasiteEelNeedCum, 2) / eelCount));
        }

        public static readonly Dictionary<Identifier, int> PregnancyNumericIds = new()
        {
            [PregnancyIds.Imp] = 1,
            [PregnancyIds.Minotaur] = 2,
            [PregnancyIds.Mouse] = 4,
            [PregnancyIds.Anemone] = 10,
            [PregnancyIds.TentacleBeastSeed] = 39
        };

        public virtual bool UpdateInfestations(Identifier type)
        {
            var numeric = PregnancyNumericIds.TryGetValue(type, out var id) ? id : -1;

            if (HasStatusEffect(StatusEffectIds.ParasiteEel))
            {
                SatisfyInfestation(StatusEffectIds.ParasiteEelNeedCum, StatusEffectIds.ParasiteEel, numeric);

                return true;
            }

            if (HasStatusEffect(StatusEffectIds.ParasiteNephila))
            {
                SatisfyInfestation(StatusEffectIds.ParasiteNephilaNeedCum, StatusEffectIds.ParasiteNephila, numeric);

                return true;
            }

            return false;
        }

        public virtual void SatisfyInfestation(Identifier needId, Identifier hostId, int type)
        {
            var need = StatusEffectByType(needId);

            if (need == null || Math.Abs(need.Values[1] - type) > 0.01f)
                return;

            if (type == 2)
                ChangeStatusValue(needId, 2, -1);

            ChangeStatusValue(needId, 2, -1);

            if (StatusEffectValue(needId, 2) <= 0)
            {
                RemoveStatusEffect(needId);

                ChangeStatusValue(hostId, 1, 1);
            }
        }

        public virtual float SmallestCockArea()
        {
            float best = float.MaxValue;

            foreach (var c in Cocks)
                best = Math.Min(best, c.Length * c.Thickness);

            return Cocks.Count == 0 ? 0f : best;
        }

        public virtual float AverageCockLength()
        {
            float total = 0;

            foreach (var cock in Cocks)
                total += cock.Length;

            return Cocks.Count == 0 ? 0f : total / Cocks.Count;
        }

        public override float AverageCockThickness()
        {
            float total = 0;

            foreach (var cock in Cocks)
                total += cock.Thickness;

            return Cocks.Count == 0 ? 0f : total / Cocks.Count;
        }

        public virtual int SmallestCockIndex2()
        {
            var order = new List<int>();

            for (var i = 0; i < Cocks.Count; i++) order.Add(i);

            order.Sort((a, b) => Cocks[a].Length.CompareTo(Cocks[b].Length));

            return order.Count > 1 ? order[1] : (order.Count > 0 ? order[0] : 0);
        }

        public virtual void ClearCombatStatuses()
        {
            foreach (var id in CombatOnlyStatuses)
                RemoveStatusEffect(id);

            foreach (var status in StatusEffects.ToList())
                if (status.Definition?.CombatOnly == true)
                    RemoveStatusEffect(status.Id);

            IsSilenced = false;

        IsCrippled = false;

        IsUnfocused = false;

        IsClumsy = false;

        IsPrude = false;

        IsCornered = false;

        IsImmobilized = false;

            RemoveStatusEffect(StatusEffectIds.IzmaBleed);

            CombatCalculator.ClearLinkedBuffs(this);
        }

        public virtual void ClearStatuses() => ClearCombatStatuses();

        public static readonly Identifier[] CombatOnlyStatuses =

        [
            StatusEffectIds.CorrWitchBind,

            StatusEffectIds.ParasiteSlugMusk,

            StatusEffectIds.ParasiteQueen,

            StatusEffectIds.NephilaQueen,

            StatusEffectIds.CounterAB,

            StatusEffectIds.Marked,

            StatusEffectIds.Nothingness,

            StatusEffectIds.ArmorRent,

            StatusEffectIds.Refashioned,

            StatusEffectIds.Revelation,

            StatusEffectIds.Resolve,

            StatusEffectIds.Leeching,

            StatusEffectIds.SentinelNoTease,

            StatusEffectIds.SentinelOmniSilence,

            StatusEffectIds.SentinelPhysicalDisabled,

            StatusEffectIds.Soulburst,

            StatusEffectIds.Apotheosis,

            StatusEffectIds.WaitReadiness,

            StatusEffectIds.Might,

            StatusEffectIds.MinotaurEntangled,

            StatusEffectIds.TentacleBind,

            StatusEffectIds.TentacleCoolDown,

            StatusEffectIds.GiantGrabbed,

            StatusEffectIds.GiantStrLoss,

            StatusEffectIds.GiantBoulder,

            StatusEffectIds.Frostbite,

            StatusEffectIds.BrutalBlows,

            StatusEffectIds.SeverTendons,

            StatusEffectIds.Shell,

            StatusEffectIds.Stunned,

            StatusEffectIds.Fear,

            StatusEffectIds.Uber,

            StatusEffectIds.ScorpGrabbed,

            StatusEffectIds.ScorpVenom,

            StatusEffectIds.ScorpBlind
        ];

        public virtual int MostBreastsPerRow()
        {
            var most = 0;

            foreach (var row in BreastRows)
                most = Math.Max(most, row.Breasts);

            return most;
        }

        public virtual float AverageVaginalWetness()
        {
            if (Vaginas.Count == 0)
                return 0f;

            float total = 0;

            foreach (var v in Vaginas)
                total += v.Wetness;

            return total / Vaginas.Count;
        }

        public virtual float AverageNipplesPerBreast()
        {
            if (BreastRows.Count == 0)
                return 0f;

            float total = 0;

            foreach (var row in BreastRows)
                total += row.NipplesPerBreast;

            return total / BreastRows.Count;
        }

        public virtual bool HasPrehensileTail() => Body.Get<TailPart>().Id.Path is "prehensile" or "raccoon";

        public virtual void HasGottenWashed()
        {
            RemoveStatusEffect(StatusEffectIds.TellyVised);

            if (Body.Get<GillsPart>().Id == BodyPartTypes.Mermaid)
                ChangeFatigue(-20);
        }

        public virtual string ArmorDescript(string nakedText = "gear")
        {
            var parts = new List<string>();

            if (Armor != null && Armor != Armor.Nothing)
                parts.Add(Armor.Name);

            if (!string.IsNullOrEmpty(UpperGarmentName) && UpperGarmentName != "nothing")
                parts.Add(UpperGarmentName);

            if (!string.IsNullOrEmpty(LowerGarmentName) && LowerGarmentName != "nothing")
                parts.Add(LowerGarmentName);

            if (parts.Count == 0)
                return nakedText;

            return string.Join(", ", parts);
        }

        public virtual bool MinotaurAddicted()

            => !HasPerk(PerkIds.MinotaurCumResistance)

               && (HasPerk(PerkIds.MinotaurCumAddict)

                   || CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) >= 1);

        public virtual bool MinotaurNeed()

            => !HasPerk(PerkIds.MinotaurCumResistance)

               && CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) > 1;

        public virtual void MinoCumAddiction(float amount = 10f)
        {
            var tracker = CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionTracker);

            CoC.Instance.Flags.Set(GameFlagIds.MinotaurCumAddictionTracker, tracker + amount);
        }

        public BeardPart Beard => Body.Get<BeardPart>();

        public virtual bool HasBeard() => Body.Get<BeardPart>().Length > 0;

        public virtual int DogCocks() => CountCocksOfType("Dog") + CountCocksOfType("Fox");

        public virtual int WolfCocks() => CountCocksOfType("Wolf");

        public virtual bool IsPregnant() => Pregnancy.IsActive;

        // 原版 Creature.hx:434 wasElder()：起始年龄为 Elder，与天赋无关
        public virtual bool WasElder() => StartingAge == Age.Elder;

        public virtual bool KnockUpChance(int chance, int maxRoll = 100) =>

            Random.Shared.Next(Math.Max(1, maxRoll)) < chance;

        public string? ModArmorName
        {
            get => GetDisplayName(EquipmentSlot.Armor);

            set
            {
                if (value is null) ClearDisplayName(EquipmentSlot.Armor);
                else SetDisplayName(EquipmentSlot.Armor, value);
            }
        }

        public string Location = "";

        public static Player Instance;

        public Player()
        {
            Instance = this;

            Body = Body.Create();

            ItemStacks = new ItemStack[SlotCount];

            for (var index = 0; index < ItemStacks.Length; index++)
                ItemStacks[index] = new ItemStack
                {
                    Unlocked = index < UnlockedSlots
                };

            Inventory = new Inventory(this);

            SetArmor(Armor.Nothing);

            SetUnarmed();

            SetShield(Shield.Nothing);

            SetJewelry(Jewelry.Nothing);

            RebuildStorage();
        }

        public virtual bool HasItemInStorage(Identifier id)        {
            foreach (var area in Storage.Values)
                foreach (var slot in area.Slots)
                    if (!slot.IsEmpty && slot.Item.Id == id)
                        return true;

            return false;
        }

        public virtual bool ConsumeItemInStorage(Identifier id)
        {
            foreach (var area in Storage.Values)
                foreach (var slot in area.Slots)
                {
                    if (slot.IsEmpty || slot.Item.Id != id)
                        continue;

                    slot.Quantity -= 1;

                    if (slot.Quantity <= 0)
                    {
                        slot.Quantity = 0;

                        slot.Item = Item.Nothing;
                    }

                    return true;
                }

            return false;
        }

        public virtual void RebuildStorage()
        {            foreach (var definition in StorageRegistry.Definitions)

                if (!Storage.TryGetValue(definition.Id, out var area))
                {
                    area = new StorageArea(definition);

                    Storage[definition.Id] = area;
                }
                else
                {
                    area.Definition = definition;

                    if (area.Capacity < definition.SlotCount)
                        area.Resize(definition.SlotCount);
                }
        }

        public bool Loaded;

        public bool CharCreation;

        public bool GameOver;

        public string? SlotName;

        public bool AutoSave = true;

        public bool StorageMoveAll;

        public string StartingRace = "human";

        public bool LostVirginity;

        public float Hunger;

        public PregnancyState Pregnancy = new();

        public PregnancyStore PregnancyStore => new(new PlayerPregnancyBacking(Pregnancy));

        public PregnancyState ButtPregnancy = new();

        public float PerkPoints;

        public float StatPoints;

        public float AscensionPerkPoints;

        // 原版 TerraStarCooldown（TimedStatusEffectReal）：按游戏内小时计的冷却到期时间
        public float TerraStarCooldownUntil;

        public int SexOrientation;

        public int SlotCount = Assets.ContentManagers.StorageManager.DefaultItemStackSlots;

        public int UnlockedSlots = Assets.ContentManagers.StorageManager.DefaultUnlockedItemStacks;

        public ItemStack[] ItemStacks;

        public Inventory Inventory;

        public Dictionary<Identifier, KeyItem> KeyItems = [];

        public Dictionary<Identifier, StorageArea> Storage = [];

        public CabinState Cabin = new();

        public Dictionary<Identifier, NpcState> NpcStates = [];

        public List<string> PreviouslyWornClothes = [];

        public Armor Armor = Armor.Nothing;

        public Undergarment UpperGarment = Undergarment.Nothing;

        public Undergarment LowerGarment = Undergarment.Nothing;

        public Weapon Weapon = Weapon.Fists;

        public Shield Shield = Shield.Nothing;

        public Jewelry Jewelry = Jewelry.Nothing;

        public virtual void NotifyRemove(Item item)
        {
            if (item.Behavior is IItemEquipBehavior behavior)
                behavior.OnRemove(this, item);
        }

        public virtual void NotifyEquip(Item item)
        {
            if (item.Behavior is IItemEquipBehavior behavior)
                behavior.OnEquip(this, item);
        }

        public virtual Armor SetArmor(Armor armor)
        {
            var old = Armor.PlayerRemove();

            NotifyRemove(old);

            armor ??= Armor.Nothing;

            Armor = armor.PlayerEquip();

            NotifyEquip(Armor);

            ArmorName = Armor.Name;

            ArmorDefense = Armor.DefenseFor(this);

            ArmorValue = Armor.Value;

            return old;
        }

        public virtual Weapon SetWeapon(Weapon weapon)
        {
            var old = Weapon.PlayerRemove();

            NotifyRemove(old);

            weapon ??= GetUnarmedWeapon();

            Weapon = weapon.PlayerEquip();

            NotifyEquip(Weapon);

            WeaponName = Weapon.Name;

            WeaponAttack = Weapon.AttackFor(this);

            WeaponAccuracy = Weapon.Accuracy;

            WeaponValue = Weapon.Value;

            return old;
        }

        public virtual Shield SetShield(Shield shield)
        {
            var old = Shield.PlayerRemove();

            NotifyRemove(old);

            shield ??= Shield.Nothing;

            Shield = shield.PlayerEquip();

            NotifyEquip(Shield);

            ShieldName = Shield.Name;

            ShieldBlock = Shield.Block;

            ShieldValue = Shield.Value;

            return old;
        }

        public virtual Jewelry SetJewelry(Jewelry jewelry)
        {
            var old = Jewelry.PlayerRemove();

            NotifyRemove(old);

            jewelry ??= Jewelry.Nothing;

            Jewelry = jewelry.PlayerEquip();

            NotifyEquip(Jewelry);

            JewelryName = Jewelry.Name;

            JewelryEffectId = Jewelry.EffectId;

            JewelryEffectMagnitude = Jewelry.EffectMagnitude;

            JewelryValue = Jewelry.Value;

            return old;
        }

        public virtual Undergarment SetUndergarment(Undergarment undergarment, int typeOverride = -1)
        {
            var type = undergarment?.Type ?? Undergarment.TypeNone;

            if (typeOverride != -1)
                type = typeOverride;

            if (type == Undergarment.TypeUpperwear)
            {
                var previous = UpperGarment.PlayerRemove();

                NotifyRemove(previous);

                UpperGarment = (undergarment ?? Undergarment.Nothing).PlayerEquip();

                NotifyEquip(UpperGarment);

                UpperGarmentName = UpperGarment.Name;

                UpperGarmentValue = UpperGarment.Value;

                return previous;
            }

            if (type == Undergarment.TypeLowerwear)
            {
                var previous = LowerGarment.PlayerRemove();

                NotifyRemove(previous);

                LowerGarment = (undergarment ?? Undergarment.Nothing).PlayerEquip();

                NotifyEquip(LowerGarment);

                LowerGarmentName = LowerGarment.Name;

                LowerGarmentValue = LowerGarment.Value;

                return previous;
            }

            return Undergarment.Nothing;
        }

        public virtual Weapon SetUnarmed() => SetWeapon(GetUnarmedWeapon());

        public virtual Weapon GetUnarmedWeapon() => HasClaws() ? Weapon.Claws : Weapon.Fists;

        // 原版 ClockwordShield.hx:56 boostsDodge(4)
        public override float GetEvasionChance()
        {
            var chance = base.GetEvasionChance();

            if (Shield.Id.Path == "clockwork_shield")
                chance += 4;

            return chance;
        }

        public virtual float SpellMod(bool ignoreSoulburst = false)
        {
            var mod = 1f + Stats.Addition(GameStats.SpellPower) / 100f;

            if (HasPerk(PerkIds.MysticLearnings) && Intelligence > 100)
                mod += 0.25f;

            // 原版 NephilaQueensGown.hx:16 boostsSpellMod(getSpellBonus)：百分比点
            mod += ArmorItemBonuses.Spell(this) / 100f;

            mod *= Stats.Multiplier(GameStats.SpellPower);

            return mod;
        }

        public override float ChanceToHit(Creature? defender = null) =>

            base.ChanceToHit(defender) + WeaponAccuracy - 1

            - (HasStatusEffect(StatusEffectIds.ScorpVenom) ? 15 : 0)
            // 原版 LurkBuff.hx：潜伏（value1 > 0）提供 +15 命中
            + (HasStatusEffect(StatusEffectIds.Lurk)

               && StatusEffectValue(StatusEffectIds.Lurk, 0) > 0 ? 15 : 0);

        protected override bool WeaponCanParry()
        {
            if (!Weapon.IsUnarmed())
                return true;

            var mastery = Masteries.Find(candidate => candidate.Id == Weapon.Id);

            var level = mastery?.Level ?? 0;

            if (Weapon.Id.Path == Weapon.Fists.Id.Path)
                return level >= 4;

            if (Weapon.Id.Path == Weapon.Claws.Id.Path)
                return level >= 3;

            return false;
        }

        public bool IsInForest => Location == "forest";

        public bool IsInDeepwoods => Location == "deep woods";

        public bool IsInLake => Location == "lake";

        public bool IsInDesert => Location == "desert";

        public bool IsInBog => Location == "bog";

        public bool IsInVolcanicCrag => Location == "crag";

        public bool IsInGlacialRift => Location == "rift";

        public bool IsInMountains => Location == "mountain";

        public bool IsInHighMountains => Location == "high mountains";

        public bool IsInSwamp => Location == "swamp";

        public bool IsInPlains => Location == "plains";

        public virtual bool HasItemAnywhere(Item item) =>

            HasItemIncludeEquipped(item) || Storage.Values.Any(area => area.Slots.Any(slot => !slot.IsEmpty && slot.Item.Id == item.Id));

        public virtual bool HasItemIncludeEquipped(Item item) =>

            HasItem(item) || IsEquipped(item);

        public virtual bool HasItemIncludeEquipped(Identifier id)
        {
            var item = ItemRegistry.Create(id);

            return HasItem(item) || IsEquipped(item);
        }

        public virtual bool HasItemArray(Item[] items, int[]? quantities = null)
        {
            for (var index = 0; index < items.Length; index++)
            {
                var required = quantities == null ? 1 : quantities[index];

                if (ItemCount(items[index]) < required)
                    return false;
            }

            return true;
        }

        public virtual bool HasItemArray(Identifier[] items, int[]? quantities = null) => HasItemArray(items.Select(ItemRegistry.Create).ToArray(), quantities);

        public virtual bool HasItemArrayAny(Identifier[] items) => HasItemArrayAny(items.Select(ItemRegistry.Create).ToArray());

        public virtual bool HasItemArrayAny(Item[] items) =>

            items.Any(item => HasItem(item));

        public virtual int RoomInExistingStack(Item item)
        {
            for (var index = 0; index < Inventory.Slots.Length; index++)
            {
                var slot = Inventory.Slots[index];

                if (!slot.IsEmpty && slot.Item.Id == item.Id && slot.Quantity < item.MaxStackSize)
                    return index;
            }

            return -1;
        }

        public virtual int EmptySlot() => Inventory.FirstEmptySlot();

        public virtual bool IsEquipped(Item item) =>

            Armor.Id == item.Id || Weapon.Id == item.Id || Shield.Id == item.Id ||

            Jewelry.Id == item.Id || UpperGarment.Id == item.Id || LowerGarment.Id == item.Id;

        public virtual bool IsNaked() =>

            (Armor == Armor.Nothing || Armor.Id == new Identifier("coc", "vine_armor")) &&

            UpperGarment == Undergarment.Nothing &&

            LowerGarment == Undergarment.Nothing;

        public virtual bool IsNakedUpper() =>

            (Armor == Armor.Nothing || Armor.Id == new Identifier("coc", "vine_armor")) &&

            UpperGarment == Undergarment.Nothing;

        public virtual bool IsNakedLower() =>

            ((Armor == Armor.Nothing ||

              Armor.Id == new Identifier("coc", "vine_armor") ||

              Armor.Id == new Identifier("coc", "ivory_corset") ||

              Armor.Id == new Identifier("coc", "cheer_outfit")) &&

             LowerGarment == Undergarment.Nothing) ||

            IsTaur() ||

            HasTailInsteadOfLegs() ||

            IsDrider();

        public virtual bool HasTailInsteadOfLegs() =>

            LowerBodyType().GetString("body") is "coc:naga" or "mermaid";

        public virtual bool HasFur() => Body.Get<SkinPart>().Type.GetBool("fur");

        public virtual bool HasReptileScales() =>

            Body.Get<SkinPart>().Type.GetBool("scales") &&

            !Body.Get<SkinPart>().Type.GetBool("dragonScales");

        public virtual bool HasDragonScales() => Body.Get<SkinPart>().Type.GetBool("dragonScales");

        public virtual bool IsNaga() => LowerBodyType().GetString("body") == "coc:naga";

        public virtual bool AbilityAvailable(IEnumerable<string> abilities, bool inCombat = false, bool ignoreLust = false, bool ignoreFatigue = false)
        {
            var wanted = new HashSet<string>();

            foreach (var name in abilities)
                wanted.Add(Snake(name));

            foreach (var id in CombatAbilityRegistry.Abilities.Ids)
            {
                if (!wanted.Contains(id.Path))
                    continue;

                var ability = CombatAbilityRegistry.Get(id);

                if (ability == null)
                    continue;

                if (!string.IsNullOrEmpty(ability.KnownWhen.Path) && !HasStatusEffect(ability.KnownWhen))
                    continue;

                if (!ignoreFatigue && ability.Cost > 0 && !HasFatigue(ability.Cost, ability.Fatigue))
                    continue;

                return true;
            }

            return false;
        }

        public static string Snake(string name)
        {
            var builder = new System.Text.StringBuilder();

            foreach (var ch in name)
            {
                if (char.IsUpper(ch) && builder.Length > 0)
                    builder.Append('_');

                builder.Append(char.ToLowerInvariant(ch));
            }

            return builder.ToString().Replace(" ", "_");
        }

        public virtual void UpgradeBeautifulSword()
        {
        }

        public virtual void ChangeClitLength(float amount)
        {
        }

        public virtual bool IsReligious() =>

            HasPerk(PerkIds.HistoryReligious) || HasPerk(PerkIds.HistoryDEUSVULT);

        public virtual bool IsNineTails()
        {
            var tail = Body.Get<TailPart>();

            return tail.Id == BodyPartTypes.Fox && tail.Venom == 9;
        }

        public virtual bool IsButtPregnant() => ButtPregnancy.IsActive;

        public virtual bool HasLongTail()
        {
            if (IsNaga())
                return true;

            var tail = Body.Get<TailPart>().Id;

            return tail == BodyPartTypes.Dog

                || tail == BodyPartTypes.Demonic

                || tail == BodyPartTypes.Cow

                || tail == BodyPartTypes.Shark

                || tail == BodyPartTypes.Cat

                || tail == BodyPartTypes.Lizard

                || tail == BodyPartTypes.Kangaroo

                || tail == BodyPartTypes.Fox

                || tail == BodyPartTypes.Draconic

                || tail == BodyPartTypes.Raccoon

                || tail == BodyPartTypes.Mouse

                || tail == BodyPartTypes.Ferret

                || tail == BodyPartTypes.Scorpion

                || tail == BodyPartTypes.Salamander

                || tail == BodyPartTypes.Wolf

                || tail == BodyPartTypes.Imp

                || tail == BodyPartTypes.Cockatrice

                || tail == BodyPartTypes.RedPanda;
        }

        public virtual bool IsGoo() => LowerBodyType().GetString("body") == "goo";

        public virtual bool HasSpiderEyes() => Body.Get<EyesPart>().Type.GetBool("spider");

        public virtual bool InBondageClothes() =>

            Armor.Id == new Identifier("coc", "bondage_straps") ||

            Armor.Id == new Identifier("coc", "vine_armor");

        public virtual bool HasUndergarments() =>

            UpperGarment != Undergarment.Nothing || LowerGarment != Undergarment.Nothing;

        public virtual void AddToWornClothes(Item armor)
        {
            if (!PreviouslyWornClothes.Contains(armor.Name))
                PreviouslyWornClothes.Add(armor.Name);
        }

        public virtual void RefillHunger(float amount = 0, bool nl = true, bool throughFood = true)
        {
            if (HasPerk(PerkIds.DemonBiology) && throughFood)
                amount = 0;

            Hunger = Math.Clamp(Hunger + amount, 0, 100);
        }

        public float LibidoPercent => 100f * Libido / MaxValue(GameStats.Libido);

        public float SensitivityPercent => 100f * Sensitivity / MaxValue(GameStats.Sensitivity);

        public virtual void UpgradeDeusVult()
        {
            if (!HasPerk(PerkIds.HistoryDEUSVULT) || !IsPureEnough(25))
                return;

            Lust = 0;

            if (Perkv1(PerkIds.HistoryDEUSVULT) < 50)
                AddPerkValue(PerkIds.HistoryDEUSVULT, 1, 1);
        }

        public float nipplesPierced;

        public string nipplesPShort = "";

        public string nipplesPLong = "";

        public float lipPierced;

        public string lipPShort = "";

        public string lipPLong = "";

        public float tonguePierced;

        public string tonguePShort = "";

        public string tonguePLong = "";

        public float eyebrowPierced;

        public string eyebrowPShort = "";

        public string eyebrowPLong = "";

        public float earsPierced;

        public string earsPShort = "";

        public string earsPLong = "";

        public float nosePierced;

        public string nosePShort = "";

        public string nosePLong = "";

        public virtual bool FeatheryHairPinEquipped() =>

            HasKeyItem("Feathery hair-pin");

        public virtual float CorAdjustedDown() => Math.Clamp(Corruption - CorruptionTolerance(), 0f, 100f);

        public virtual bool HasChildren() =>

            StatusEffectValue(StatusEffectIds.Birthed, 0) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.AmilyBirthTotal) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.PcTimesBirthedAmilykids) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.BenoitEggs) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.EdrynNumberOfKids) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.EmberEggs) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.JojoLitters) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.KellyKidsMale) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.KellyKids) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.TimesBirthedSharpies) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.PhyllaDriderBabiesCount) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.SheilaJoeys) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.SheilaImps) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.SophieDaughterMaturityCounter) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.SophieEggsLaid) > 0 ||

            CoC.Instance.Flags.Get(GameFlagIds.TamaniNumberOfDaughters) > 0;

        public virtual bool IsPureEnough(float maxCor, bool falseIf100 = false)
        {
            if (falseIf100 && Corruption >= 99.5f)
                return false;

            return CorAdjustedDown() < maxCor;
        }

        public virtual float CorruptionTolerance()
        {
            var tolerance = (int)(PerkValue(PerkIds.AscensionTolerance, 1) * 5 * (1 - PerkValue(PerkIds.AscensionTolerance, 2)));

            if (CoC.Instance.Flags.Get(GameFlagIds.MeaninglessCorruption) > 0)
                tolerance += 100;

            return tolerance;
        }

        public virtual void UnlockCodexEntry(Flag<int> flag, bool nlBefore = true, bool nlAfter = false)
        {
            if (CoC.Instance.Flags.Get(flag) > 0)
                return;

            var entry = CodexRegistry.Get(flag.Id);

            if (entry == null)
                return;

            CoC.Instance.Flags.Set(flag, 1);

            var text = (nlBefore ? "[pg]" : "") +

                       "Codex.Unlock".Localized(entry.NameKey.Localized()) +

                       (nlAfter ? "[pg]" : "");

            if (Scene.View != null)
                Scene.OutputText(text);
        }

        public virtual float BonusFertility()
        {
            var fertility = 0f;

            if (HasStatusEffect(StatusEffectIds.Heat))
                fertility += StatusEffectValue(StatusEffectIds.Heat, 0);

            if (HasPerk(PerkIds.FertilityPlus))
                fertility += 15;

            if (HasPerk(PerkIds.FertilityMinus) && Libido < 25)
                fertility -= 15;

            if (HasPerk(PerkIds.MaraesGiftFertility))
                fertility += 50;

            if (HasPerk(PerkIds.FerasBoonBreedingBitch))
                fertility += 30;

            if (HasPerk(PerkIds.MagicalFertility))
                fertility += 10 + PerkValue(PerkIds.MagicalFertility, 1) * 5;

            if (HasPerk(PerkIds.MothBedding))
                fertility += 20;

            fertility += PerkValue(PerkIds.ElvenBounty, 2);

            fertility += PerkValue(PerkIds.PiercedFertite, 1);

            if (JewelryEffectId == JewelryIds.FertilityEffect)
                fertility += JewelryEffectMagnitude;

            fertility += PerkValue(PerkIds.AscensionFertility, 1) * 5;

            return fertility;
        }

        public override float TotalFertility() => Fertility + BonusFertility();

        public virtual bool GoIntoHeat(bool output = true, int intensity = 1)
        {
            if (!HasVagina() || Pregnancy.Incubation != 0)
                return false;

            if (InHeat)
            {
                var effect = StatusEffectByType(StatusEffectIds.Heat);

                if (effect != null)
                {
                    effect.Values[0] += 5 * intensity;

                    effect.Values[1] += 5 * intensity;

                    effect.Values[2] += 48 * intensity;
                }

                Lust = Math.Min(MaxLust, Lust + 5 * intensity);
            }
            else
            {
                CreateStatusEffect(StatusEffectIds.Heat, 10 * intensity, 15 * intensity, 48 * intensity, 0);

                Lust = Math.Min(MaxLust, Lust + 15 * intensity);
            }

            return true;
        }

        public virtual bool GoIntoRut(bool output = true, int intensity = 1)
        {
            if (!HasCock())
                return false;

            if (InRut)
            {
                var effect = StatusEffectByType(StatusEffectIds.Rut);

                if (effect != null)
                {
                    effect.Values[0] += 5 * intensity;

                    effect.Values[1] += 5 * intensity;

                    effect.Values[2] += 48 * intensity;
                }

                Lust = Math.Min(MaxLust, Lust + 5 * intensity);
            }
            else
            {
                CreateStatusEffect(StatusEffectIds.Rut, 10 * intensity, 15 * intensity, 48 * intensity, 0);

                Lust = Math.Min(MaxLust, Lust + 15 * intensity);
            }

            return true;
        }

        public virtual void KnockUp(Identifier type, int incubation, int maxRoll = 100, int forcePregnancy = 0)
        {

            if (UpdateInfestations(type))
                return;

            if (HasStatusEffect(StatusEffectIds.Contraceptives) && forcePregnancy < 1)
                return;

            var bonus = forcePregnancy >= 1 ? 9000 : forcePregnancy <= -1 ? -9000 : 0;

            if (!Pregnancy.IsActive && TotalFertility() + bonus > Random.Shared.Next(maxRoll) && HasVagina())
                StartPregnancy(type, incubation);
        }

        public virtual bool IsBiped() => Body.Get<LowerBodyPart>().LegCount == 2;

        public virtual void DamageHunger(float amount = 0)
        {
            Hunger = Math.Clamp(Hunger - amount, 0, 100);
        }

        public virtual int RequiredXP(int level = -1)
        {
            if (level < 0)
                level = (int)Level;

            return Math.Min(9999, level * 100);
        }

        public virtual int PotentialLevel()
        {
            var level = (int)Level;

            var xp = (int)Experience;

            while (xp >= RequiredXP(level))
            {
                xp -= RequiredXP(level);

                level++;
            }

            return level;
        }

        public virtual int XpToLevel(int targetLevel)
        {
            var needed = 0;

            while (targetLevel > Level)
            {
                needed += RequiredXP(targetLevel - 1);

                targetLevel--;
            }

            return needed - (int)Experience;
        }

        public virtual bool CanLevelUp() => Experience >= RequiredXP();

        public virtual bool HasWaited() =>

            CoC.Instance.Flags.Get(GameFlagIds.InCombatUsePlayerWaitedFlag) == 1;

        public virtual bool StatsMaxed() =>

            Strength >= 9999 && Toughness >= 9999 && Speed >= 9999 && Intelligence >= 9999;

        public float BonusStrength => Stats.Addition(GameStats.Strength);

        public float BonusToughness => Stats.Addition(GameStats.Toughness);

        public float BonusSpeed => Stats.Addition(GameStats.Speed);

        public float BonusIntelligence => Stats.Addition(GameStats.Intelligence);

        public virtual bool IsFeminine() => Femininity >= 50;

        public virtual bool HasDragonWings(bool large = false) =>

            large

                ? Body.Get<WingsPart>().Id.Path == "draconic_large"

                : Body.Get<WingsPart>().Id.Path is "draconic_small" or "draconic_large";

        public virtual bool HasDragonfire() => HasPerk(PerkIds.Dragonfire);

        public virtual bool HasDragonWingsAndFire() => HasDragonWings() && HasDragonfire();

        public virtual bool IsBasilisk() => false;

        public virtual string IsLoliShota(string loli, string normal) => IsChild() ? loli : normal;

        public virtual string Mfn(string male, string female, string herm) => Gender switch
        {
            Gender.Male => male,

            Gender.Female => female,

            _ => herm,
        };

        public override string Mf(string male, string female)
        {
            var genderSwitch = CoC.Instance.Flags.Get(GameFlagIds.GenderSwitch);

            if (genderSwitch == 1)
                return male;

            if (genderSwitch == 2)
                return female;

            return base.Mf(male, female);
        }

        public virtual string GetRace()
        {
            string Part<T>() where T : BodyPart => Body.Get<T>().Id.Path;

            bool PartIs<T>(params string[] ids) where T : BodyPart => ids.Contains(Part<T>());

            var race = "human";

            if (this.CatScore() >= 4)
                if (IsTaur() && PartIs<LowerBodyPart>("cat"))
                {
                    race = IsLoliShota("kitten-taur", "cat-taur");

                    if (PartIs<FacePart>("human", "catgirl"))
                        race = "sphinx-morph";
                }
                else
                {
                    race = IsLoliShota("kitten-morph", "cat-morph");

                    if (PartIs<FacePart>("human", "catgirl"))
                        race = IsLoliShota("kitten-", "cat-") + Mf("boy", "girl");
                }

            if (this.LizardScore() >= 4)
            {
                race = HasDragonWingsAndFire()

                    ? IsBasilisk() ? "dracolisk" : "dragonewt"

                    : IsBasilisk() ? "basilisk" : "lizan";

                if (IsTaur())
                    race += "-taur";

                if (this.LizardScore() >= 9)
                    return race;
            }

            if (this.DragonScore() >= 6)
            {
                race = "dragon-morph";

                if (PartIs<FacePart>("human"))
                    race = "dragon-" + Mf("man", "girl");

                if (IsTaur())
                    race = "dragon-taur";
            }

            if (this.CockatriceScore() >= 4)
            {
                race = "cockatrice-morph";

                if (this.CockatriceScore() >= 8)
                    race = "cockatrice";

                if (PartIs<FacePart>("human"))
                    race = "cockatrice-" + Mf("boy", "girl");

                if (IsTaur())
                    race = "cockatrice-taur";
            }

            if (this.RedPandaScore() >= 4)
            {
                race = "red-panda-morph";

                if (PartIs<FacePart>("human"))
                    race = "red-panda-" + Mf("boy", "girl");

                if (IsTaur())
                    race = "red-panda-taur";
            }

            if (this.RaccoonScore() >= 4)
            {
                race = "raccoon-morph";

                if (Balls > 0 && BallSize > 5)
                    race = "tanuki-morph";

                if (IsTaur())
                    race = "raccoon-taur";
            }

            if (this.SheepScore() >= 4)
                if (Body.Get<LowerBodyPart>().LegCount == 4 && PartIs<LowerBodyPart>("cloven_hoofed"))
                    race = "sheep-taur";
                else if (IsGenderless() || IsHerm())
                    race = "sheep-morph";
                else if (IsMale() && PartIs<HornsPart>("ram"))
                    race = "ram-morph";
                else
                    race = "sheep-" + Mf("boy", "girl");

            if (this.WolfScore() >= 4)
                race = HasFur() || IsGenderless() || IsHerm() ? "wolf-morph" : "wolf-" + Mf("boy", "girl");

            if (this.DogScore() >= 4)
                if (IsTaur() && PartIs<LowerBodyPart>("dog"))
                    race = IsLoliShota("puppy", "dog");
                else
                {
                    race = IsLoliShota("puppy-morph", "dog-morph");

                    if (PartIs<FacePart>("human"))
                        race = IsLoliShota("puppy-" + Mf("boy", "girl"), "dog-" + Mf("man", "girl"));
                }

            if (this.FoxScore() >= 4)
                if (IsTaur() && PartIs<LowerBodyPart>("fox"))
                    race = "fox-taur";
                else if (HasFur())
                    race = "fox-morph";
                else
                    race = "fox-" + Mf("morph", "girl");

            if (this.FerretScore() >= 4)
                race = HasFur() ? "ferret-morph" : "ferret-" + Mf("morph", "girl");

            if (this.KitsuneScore() >= 4)
                race = "kitsune";

            if (this.HorseScore() >= 3)
                if (IsTaur())
                    race = "centaur-morph";
                else if (PartIs<HornsPart>("unicorn"))
                    race = PartIs<WingsPart>("feathered_large") ? "alicorn" : "unicorn-morph";
                else
                    race = PartIs<WingsPart>("feathered_large") ? "pegasus" : "equine-morph";

            if (this.MutantScore() >= 5 && race == "human")
                race = "corrupted mutant";

            if (this.MinoScore() >= 4)
                race = "minotaur-morph";

            if (this.CowScore() > 5)
                race = "cow-" + Mf("morph", "girl");

            if (this.BeeScore() >= 5)
                race = "bee-morph";

            if (this.GoblinScore() >= 5)
                race = "coc:goblin";

            if (this.HumanScore() >= 5 && race == "corrupted mutant")
                race = "somewhat human mutant";

            if (this.DemonScore() > 4)
                race = "demon-morph";

            if (this.SharkScore() >= 3)
                race = "shark-morph";

            if (this.BunnyScore() >= 4)
            {
                race = "bunny-" + Mf("boy", "girl");

                if (PartIs<HornsPart>("antlers") && Body.Get<HornsPart>().Value > 0)
                    race = "jackalope-" + Mf("boy", "girl");
            }

            if (this.HarpyScore() >= 4)
                race = IsFemale() || IsHerm() ? "harpy" : "avian";

            if (this.SpiderScore() >= 4)
            {
                race = IsGenderless() || IsHerm() ? "spider-morph" : "spider-" + Mf("boy", "girl");

                if (IsDrider())
                    race = "drider";
            }

            if (this.KangaScore() >= 4)
                race = "kangaroo-morph";

            if (this.MouseScore() >= 3)
                race = PartIs<FacePart>("mouse") ? "mouse-morph" : "mouse-" + Mf("boy", "girl");

            if (this.SalamanderScore() >= 4)
                race = IsTaur() ? "salamander-taur" : "salamander-" + Mf("boy", "girl");

            if (this.PigScore() >= 4)
            {
                race = "pig-morph";

                if (PartIs<FacePart>("human"))
                    race = "pig-" + Mf("boy", "girl");

                if (PartIs<FacePart>("boar"))
                    race = "boar-morph";
            }

            if (this.SatyrScore() >= 4)
                race = "satyr";

            if (this.DryadScore() >= 3)
                race = "dryad";

            if (this.RhinoScore() >= 4)
            {
                race = "rhino-morph";

                if (PartIs<FacePart>("human"))
                    race = "rhino-" + Mf("boy", "girl");
            }

            if (this.EchidnaScore() >= 4)
            {
                race = "echidna-morph";

                if (PartIs<FacePart>("human"))
                    race = "echidna-" + Mf("boy", "girl");
            }

            if (this.DeerScore() >= 4)
                if (IsTaur())
                    race = "deer-taur";
                else
                {
                    race = "deer-morph";

                    if (PartIs<FacePart>("human"))
                        race = "deer-" + Mf("boy", "girl");
                }

            if (this.BatScore() >= 4)
            {
                race = "bat-" + Mf("boy", "girl");

                if (OptionStore.Instance.Get("silly", 0) > 0)
                    race = "bat-" + Mf("man", "woman");
            }

            if (this.SirenScore() >= 4)
                race = "siren";

            if (PartIs<LowerBodyPart>("coc:naga"))
                race = "coc:naga";

            if (PartIs<LowerBodyPart>("mermaid"))
                race = "mer" + (IsGenderless() ? "folk" : Mf("man", "maid"));

            if (PartIs<LowerBodyPart>("hoofed") && IsTaur())
                if (PartIs<WingsPart>("feathered_large"))
                    race = PartIs<HornsPart>("unicorn") ? "alicorn-taur" : "pegataur";
                else if (PartIs<HornsPart>("unicorn"))
                    race = "unicorn-taur";
                else if (this.HorseScore() >= 5)
                    race = "equitaur";
                else if (this.MinoScore() >= 4)
                    race = "mino-centaur";
                else
                    race = "centaur";

            if (PartIs<LowerBodyPart>("pony"))
                race = "pony-kin";

            if (this.GooScore() >= 3)
                race = "goo-" + Mf("boy", "girl");

            if (this.HumanScore() >= 4 && Tallness <= 48 && PartIs<WingsPart>("bee_like_small", "bee_like_large"))
                race = "faerie";

            if (this.ImpScore() >= 4)
                race = "coc:imp";

            if (this.GnollScore() >= 5)
                race = "gnoll-morph";

            return race;
        }

        public virtual string HighestRaceScore() => GetRace();

        public virtual bool IsColdBlooded() => this.LizardScore() >= 9 || IsNaga();

        public virtual bool HasDress() =>

            Armor.Id == new Identifier("coc", "maid_dress") ||

            Armor.Id == new Identifier("coc", "ballroom_dress") ||

            Armor.Id == new Identifier("coc", "ballet_dress") ||

            Armor.Id == new Identifier("coc", "summer_dress") ||

            Armor.Id == new Identifier("coc", "silk_summer_dress") ||

            Armor.Id == new Identifier("coc", "maid_silk_dress") ||

            Armor.Id == new Identifier("coc", "nephila_gown") ||

            Armor.Id == new Identifier("coc", "china_dress");

        public virtual string InventoryName() =>

            HasKeyItem(KeyItemIds.Backpack)

                ? Random.Shared.Next(2) == 0 ? "pack" : "backpack"

                : Random.Shared.Next(4) switch
                {
                    0 => "pouch",

                    1 => "item pouch",

                    2 => "bag",

                    _ => "item bag"
                };

        public virtual bool IsDisarmed() => HasStatusEffect(StatusEffectIds.Disarmed);

        public virtual bool CanDisarm() => !IsDisarmed() && !Weapon.IsUnarmed();

        public virtual Weapon? GetDisarmed()
        {
            var status = StatusEffectByType(StatusEffectIds.Disarmed);

            if (status == null)
                return null;

            var saved = status.Data.TryGetValue("weaponID", out var value) ? value.ToString().Trim('"') : "";

            RemoveStatusEffect(StatusEffectIds.Disarmed);

            return ItemRegistry.Resolve(saved) as Weapon;
        }

        public virtual bool Rearm()
        {
            if (!IsDisarmed() || !IsUnarmed())
                return false;

            var weapon = GetDisarmed();

            if (weapon == null)
                return false;

            SetWeapon(weapon);

            return !IsUnarmed();
        }

        public virtual void Disarm(Weapon weapon)
        {
            var status = CreateStatusEffect(StatusEffectIds.Disarmed);

            status.Data["weaponID"] = weapon.Id.ToString();

            SetUnarmed();
        }

        public virtual int EscapeMod()
        {
            var mod = 20 + (int)((CombatController.Instance?.Context.Enemy.Level ?? 0) * 3);

            if (Body.Get<TailPart>().Id == BodyPartTypes.Raccoon &&
                Body.Get<EarsPart>().Id == BodyPartTypes.Raccoon &&

                HasPerk(PerkIds.Runner))

                mod -= 25;

            if (HasPerk(PerkIds.HistoryThief))
                mod -= 20;

            if (!CanFly())
            {
                if (BiggestBreastSize() >= 35)
                    mod += 5;

                if (BiggestBreastSize() >= 66)
                    mod += 10;

                if (Body.Hips >= 20)
                    mod += 5;

                if (Body.Butt >= 20)
                    mod += 5;

                if (BallSize >= 24 && Balls > 0)
                    mod += 5;

                if (BallSize >= 48 && Balls > 0)
                    mod += 10;

                if (BallSize >= 120 && Balls > 0)
                    mod += 10;
            }

            return mod;
        }

        public virtual float GetAgiSpeedBonus()
        {
            var bonus = 0f;

            if (HasPerk(PerkIds.Agility) &&
                (Armor.Perk == "Light" || Armor.Perk == "Adornment" || Armor == Armor.Nothing))

                bonus = Speed / 8f;
            else if (HasPerk(PerkIds.Agility) && Armor.Perk == "Medium")
                bonus = Speed / 13f;

            return Math.Min(15, bonus);
        }

        public virtual float ArmorMiscBonuses()
        {
            var bonus = 0f;

            bonus += Body.Get<SkinPart>().Type.GetInt("armorBonus");

            bonus += Body.Get<RearBodyPart>().Type.GetInt("armorBonus");

            bonus += Body.Get<ArmsPart>().Type.GetInt("armorBonus");

            bonus += Body.Get<LowerBodyPart>().Type.GetInt("armorBonus");

            if (HasPerk(PerkIds.ThickSkin))
                bonus += 2;

            if (HasStatusEffect(StatusEffectIds.TFPlate))
                bonus += StatusEffectValue(StatusEffectIds.TFPlate, 0);

            if (HasPerk(PerkIds.Juggernaut) && Toughness >= 75 && Armor.Perk == "Heavy")
                bonus += 10;

            bonus += GetAgiSpeedBonus();

            return bonus;
        }

        public float ArmorBaseDef => Armor.Defense;

        public float WeaponBaseAttack => Weapon.Attack;

        public float WeaponAcc => Weapon.Accuracy;

        public Dictionary<string, float> GetAllMaxStats() => new()
        {
            ["str"] = 9999,
            ["tou"] = 9999,
            ["spe"] = 9999,
            ["inte"] = 9999,
            ["lib"] = 100,
            ["sens"] = 100,
            ["cor"] = 100,
            ["lust"] = MaxLust
        };

        public virtual float GetMaxStats(string stat) => GetAllMaxStats().GetValueOrDefault(stat, 0);

        public virtual string ClothedOrNaked(string clothed, string naked = "") => IsNaked() ? naked : clothed;

        public virtual string ClothedOrNakedLower(string clothed, string naked = "") => IsNakedLower() ? naked : clothed;

        public virtual float WeightRating()
        {
            var weight = 100f;

            weight *= 1 + (Tallness - 77) / (120 - 77);

            weight *= 1 + Thickness / 200;

            weight *= 1 + Tone / 400;

            if (IsTaur() || IsDrider())
                weight *= 1.3f;

            if (HasTailInsteadOfLegs())
                weight *= 1.1f;

            return weight;
        }

        public virtual void TakeDamage(float amount) => HealthChange(-amount);

        public virtual void TakeLustDamage(float amount) => Lust = Math.Min(MaxLust, Lust + amount);

        public virtual void TakeLustDamage(float amount, bool ignoreResist) => TakeLustDamage(amount);

        public virtual void TakeLustDamage(float amount, bool ignoreResist, bool noDisplay) => TakeLustDamage(amount);

        public override void UpdateUnarmed()
        {
            if (IsUnarmed())
                SetUnarmed();
        }

        public virtual int FirstEmptySlot() => Inventory.FirstEmptySlot();

        public virtual bool AddItem(Item item, int quantity = 1) => Inventory.Add(item, quantity);

        public virtual bool ConsumeItem(Item item, int amount = 1) => Inventory.Consume(item, amount);

        public virtual bool ConsumeItem(Identifier id, int amount = 1) => ConsumeItem(ItemRegistry.Create(id), amount);

        public virtual bool DestroyItems(Identifier id, float numOfItemToRemove) =>

            DestroyItems(id, (int)numOfItemToRemove);

        public virtual bool DestroyItems(Identifier id, int numOfItemToRemove = 1)
        {
            var item = ItemRegistry.Create(id);

            foreach (var slot in ItemStacks)
            {
                if (slot.IsEmpty || slot.Item != item)
                    continue;

                while (slot.Quantity > 0 && numOfItemToRemove > 0)
                {
                    slot.RemoveOneItem();

                    numOfItemToRemove--;
                }
            }

            return numOfItemToRemove <= 0;
        }

        public virtual int ItemCount(Item item) => Inventory.Count(item);

        public virtual int ItemCount(Identifier id) => ItemCount(ItemRegistry.Create(id));

        public virtual bool HasItem(Item item, int minimum = 1) => Inventory.HasItem(item, minimum);

        public virtual bool HasItem(Identifier id) => HasItem(ItemRegistry.Create(id));

        public virtual KeyItem CreateKeyItem(Identifier id, float value1 = 0, float value2 = 0, float value3 = 0, float value4 = 0)
        {
            if (!KeyItems.TryGetValue(id, out var item))
            {
                item = new KeyItem { Id = id };

                KeyItems[id] = item;
            }

            item[0] = value1;

            item[1] = value2;

            item[2] = value3;

            item[3] = value4;

            UpdateItemStacks();

            return item;
        }

        public virtual KeyItem CreateKeyItem(string keyName, float value1 = 0, float value2 = 0, float value3 = 0, float value4 = 0)
        {
            KeyItemIdMap.Resolve(keyName, out var id);

            return CreateKeyItem(id, value1, value2, value3, value4);
        }

        public virtual void RemoveKeyItem(Identifier id)
        {
            KeyItems.Remove(id);

            UpdateItemStacks();
        }

        public virtual void RemoveKeyItem(string keyName)
        {
            KeyItemIdMap.Resolve(keyName, out var id);

            RemoveKeyItem(id);
        }

        public virtual void RemoveKeyItems()
        {
            KeyItems.Clear();

            UpdateItemStacks();
        }

        public virtual bool HasKeyItem(Identifier id) => KeyItems.ContainsKey(id);

        public virtual bool HasKeyItem(string keyName)
        {
            KeyItemIdMap.Resolve(keyName, out var id);

            return HasKeyItem(id);
        }

        public virtual bool TryGetKeyItem(Identifier id, out KeyItem item) => KeyItems.TryGetValue(id, out item);

        public virtual KeyItem GetKeyItem(string keyName)
        {
            KeyItemIdMap.Resolve(keyName, out var id);

            return KeyItems.TryGetValue(id, out var item) ? item : new KeyItem { Id = id };
        }

        public virtual int FindKeyItem(string keyName)
        {
            KeyItemIdMap.Resolve(keyName, out var id);

            return KeyItems.ContainsKey(id) ? 0 : -1;
        }

        public virtual float GetKeyValue(Identifier id, int index) => TryGetKeyItem(id, out var item) ? item[index] : 0;

        public virtual float KeyItemValue(string keyName, int index)
        {
            KeyItemIdMap.Resolve(keyName, out var id);

            return GetKeyValue(id, index - 1);
        }

        public virtual float KeyItemV1(Identifier id) => GetKeyValue(id, 0);

        public virtual float KeyItemV1(string keyName) => KeyItemValue(keyName, 1);

        public virtual float KeyItemV2(string keyName) => KeyItemValue(keyName, 2);

        public virtual float KeyItemV3(string keyName) => KeyItemValue(keyName, 3);

        public virtual float KeyItemV4(string keyName) => KeyItemValue(keyName, 4);

        public virtual NpcState GetNpcState(NpcDefinition definition)
        {
            if (NpcStates.TryGetValue(definition.Id, out var state))
                return state;

            state = definition.StateFactory();

            NpcStates[definition.Id] = state;

            return state;
        }

        public int InventoryCapacity
        {
            get
            {
                var slots = UnlockedSlots;

                if (TryGetKeyItem(KeyItemIds.Backpack, out var backpack))
                    slots += (int)backpack[0];

                return Math.Clamp(slots, UnlockedSlots, ItemStacks.Length);
            }
        }

        public virtual void UpdateItemStacks()
        {
            var slots = InventoryCapacity;

            for (var index = 0; index < ItemStacks.Length; index++)
                ItemStacks[index].Unlocked = index < slots;
        }

        public virtual void FixStorage()
        {
            RebuildStorage();

            var capacity = 4;

            if (HasKeyItem(KeyItemIds.CampChest))
                capacity += 6;

            if (HasKeyItem(KeyItemIds.CampMurkyChest))
                capacity += 4;

            if (HasKeyItem(KeyItemIds.CampOrnateChest))
                capacity += 4;

            if (Storage.TryGetValue(StorageRegistry.ItemsId, out var items)
                && items.Capacity < capacity)

                items.Resize(capacity);
        }

        public virtual void LoadStorage(JsonObject json)
        {
            Storage.Clear();

            RebuildStorage();

            if (json.TryGetValue("storage", out var storageValue) && storageValue is JsonObject storageObject)
                foreach (var pair in storageObject)
                {
                    if (pair.Value is not JsonObject areaJson || !Identifier.TryParse(pair.Key, out var id))
                        continue;

                    if (Storage.TryGetValue(id, out var area))
                        area.Load(areaJson);
                }
            else
            {
                if (json.TryGetValue("inventory", out var inventoryValue) && inventoryValue is JsonObject inventoryJson &&
                    Storage.TryGetValue(StorageRegistry.ItemsId, out var items))

                    items.Load(inventoryJson);

                if (json.TryGetValue("gearStorage", out var gearValue) && gearValue is JsonObject gearJson &&
                    gearJson.TryGetValue("slots", out var slotsValue) && slotsValue is JsonArray legacySlots)
                {
                    LoadLegacyGear("gear_weapons", legacySlots, 0);

                    LoadLegacyGear("gear_armor", legacySlots, 9);

                    LoadLegacyGear("gear_jewelry", legacySlots, 18);

                    LoadLegacyGear("gear_undergarments", legacySlots, 27);

                    LoadLegacyGear("gear_shields", legacySlots, 36);
                }
            }

            FixStorage();
        }

        public virtual void LoadLegacyGear(string key, JsonArray source, int start)
        {
            if (!Storage.TryGetValue(new Identifier("coc", key), out var area))
                return;

            var slots = new JsonArray();

            for (var index = start; index < Math.Min(start + 9, source.Count); index++)
                slots.Add(source[index]);

            area.LoadSlots(slots);
        }

        public virtual void AddKeyValue(Identifier id, int index, float amount)
        {
            if (TryGetKeyItem(id, out var item))
                item[index] += amount;
        }

        public virtual void AddKeyValue(string keyName, int index, float amount)
        {
            KeyItemIdMap.Resolve(keyName, out var id);

            AddKeyValue(id, index - 1, amount);
        }

        public virtual ItemStack? ItemStackAt(int index) => index >= 0 && index < ItemStacks.Length ? ItemStacks[index] : null;

        public static T ResolveEquipment<T>(string value, T fallback) where T : Item
        {
            if (!ItemIdMap.Resolve(value, out var id))
                return fallback;

            if (id == Weapon.Fists.Id)
                return (T)(object)Weapon.Fists;

            if (id == Weapon.Claws.Id)
                return (T)(object)Weapon.Claws;

            var item = ItemRegistry.Create(id);

            return item as T ?? fallback;
        }

        public override void Save(JsonObject json)
        {
            base.Save(json);

            json["startingRace"] = StartingRace;

            json["lostVirginity"] = LostVirginity;

            json["lustVulnerability"] = LustVulnerability;

            json["hunger"] = Hunger;

            var pregnancy = new JsonObject();

            Pregnancy.Save(pregnancy);

            json["pregnancy"] = pregnancy;

            var udder = new JsonObject();

            Udder.Save(udder);

            json["udder"] = udder;

            var buttPregnancy = new JsonObject();

            ButtPregnancy.Save(buttPregnancy);

            json["buttPregnancy"] = buttPregnancy;

            json["perkPoints"] = PerkPoints;

            json["statPoints"] = StatPoints;

            json["ascensionPerkPoints"] = AscensionPerkPoints;

            json["terraStarCooldownUntil"] = TerraStarCooldownUntil;

            json["sexOrientation"] = SexOrientation;

            json["autoSave"] = AutoSave;

            json["storageMoveAll"] = StorageMoveAll;

            json["unlockedItemStacks"] = UnlockedSlots;

            json["armor"] = Armor == null ? "" : Armor.Id.ToString();

            json["upperGarment"] = UpperGarment == null ? "" : UpperGarment.Id.ToString();

            json["lowerGarment"] = LowerGarment == null ? "" : LowerGarment.Id.ToString();

            json["weapon"] = Weapon == null ? "" : Weapon.Id.ToString();

            json["shield"] = Shield == null ? "" : Shield.Id.ToString();

            json["jewelry"] = Jewelry == null ? "" : Jewelry.Id.ToString();

            var itemSlotsJson = new JsonArray();

            foreach (var slot in ItemStacks)
            {
                var slotJson = new JsonObject();

                slot.Save(slotJson);

                itemSlotsJson.Add(slotJson);
            }

            json["itemSlots"] = itemSlotsJson;

            var storageJson = new JsonObject();

            foreach (var pair in Storage)
            {
                var areaJson = new JsonObject();

                pair.Value.Save(areaJson);

                storageJson[pair.Key.ToString()] = areaJson;
            }

            json["storage"] = storageJson;

            var cabinJson = new JsonObject();

            Cabin.Save(cabinJson);

            json["cabin"] = cabinJson;

            var npcStatesJson = new JsonObject();

            foreach (var pair in NpcStates)
            {
                var stateJson = new JsonObject();

                pair.Value.Save(stateJson);

                npcStatesJson[pair.Key.ToString()] = stateJson;
            }

            json["npcStates"] = npcStatesJson;

            var keyItemsJson = new JsonArray();

            foreach (var keyItem in KeyItems.Values)
            {
                var keyItemJson = new JsonObject
                {
                    ["id"] = keyItem.Id.ToString()
                };

                var values = new JsonArray();

                foreach (var value in keyItem.Values)
                    values.Add(value);

                keyItemJson["values"] = values;

                keyItemsJson.Add(keyItemJson);
            }

            json["keyItems"] = keyItemsJson;
        }

        public override void Load(JsonObject json)
        {
            base.Load(json);

            StartingRace = json.TryGetValue("startingRace", out var startingRace) ? (string)startingRace : "human";

            LostVirginity = json.TryGetValue("lostVirginity", out var lostVirginity) && (bool)lostVirginity;

            LustVulnerability = json.TryGetValue("lustVulnerability", out var lustVulnerability) ? (float)lustVulnerability : 1;

            Hunger = json.TryGetValue("hunger", out var hunger) ? (float)hunger : 0;

            Pregnancy = new PregnancyState();

            if (json.TryGetValue("pregnancy", out var pregnancyValue) && pregnancyValue is JsonObject pregnancyObject)
                Pregnancy.Load(pregnancyObject);

            ButtPregnancy = new PregnancyState();

            if (json.TryGetValue("buttPregnancy", out var buttValue) && buttValue is JsonObject buttObject)
                ButtPregnancy.Load(buttObject);

            if (json.TryGetValue("udder", out var udderValue) && udderValue is JsonObject udderObject)
                Udder.Load(udderObject);

            PerkPoints = json.TryGetValue("perkPoints", out var perkPoints) ? (float)perkPoints : 0;

            StatPoints = json.TryGetValue("statPoints", out var statPoints) ? (float)statPoints : 0;

            AscensionPerkPoints = json.TryGetValue("ascensionPerkPoints", out var ascensionPerkPoints) ? (float)ascensionPerkPoints : 0;

            TerraStarCooldownUntil = json.TryGetValue("terraStarCooldownUntil", out var terraStarCooldownUntil) ? (float)terraStarCooldownUntil : 0f;

            SexOrientation = json.TryGetValue("sexOrientation", out var sexOrientation) ? (int)sexOrientation : 0;

            AutoSave = json.TryGetValue("autoSave", out var autoSave) && (bool)autoSave;

            UnlockedSlots = json.TryGetValue("unlockedItemStacks", out var unlockedStacks)

                ? (int)unlockedStacks

                : Assets.ContentManagers.StorageManager.DefaultUnlockedItemStacks;

            StorageMoveAll = json.TryGetValue("storageMoveAll", out var storageMoveAll) && (bool)storageMoveAll;

            var armorId = json.TryGetValue("armor", out var armorValue) ? (string)armorValue : "";

            Armor = ResolveEquipment(armorId, Armor.Nothing);

            var upperGarmentId = json.TryGetValue("upperGarment", out var upperGarmentValue) ? (string)upperGarmentValue : "";

            UpperGarment = ResolveEquipment(upperGarmentId, Undergarment.Nothing);

            var lowerGarmentId = json.TryGetValue("lowerGarment", out var lowerGarmentValue) ? (string)lowerGarmentValue : "";

            LowerGarment = ResolveEquipment(lowerGarmentId, Undergarment.Nothing);

            var weaponId = json.TryGetValue("weapon", out var weaponValue) ? (string)weaponValue : "";

            Weapon = ResolveEquipment(weaponId, Weapon.Fists);

            var shieldId = json.TryGetValue("shield", out var shieldValue) ? (string)shieldValue : "";

            Shield = ResolveEquipment(shieldId, Shield.Nothing);

            var jewelryId = json.TryGetValue("jewelry", out var jewelryValue) ? (string)jewelryValue : "";

            Jewelry = ResolveEquipment(jewelryId, Jewelry.Nothing);

            SetArmor(Armor);

            SetWeapon(Weapon);

            SetShield(Shield);

            SetJewelry(Jewelry);

            SetUndergarment(UpperGarment, Undergarment.TypeUpperwear);

            SetUndergarment(LowerGarment, Undergarment.TypeLowerwear);

            UpdateUnarmed();

            if (json.TryGetValue("itemSlots", out var itemSlotsValue) && itemSlotsValue is JsonArray itemSlotsArray)
                for (var index = 0; index < ItemStacks.Length; index++)
                    if (index < itemSlotsArray.Count && itemSlotsArray[index] is JsonObject slotJson)
                        ItemStacks[index].Load(slotJson);
                    else
                        ItemStacks[index].Clear();

            LoadStorage(json);

            Cabin = new CabinState();

            if (json.TryGetValue("cabin", out var cabinValue) && cabinValue is JsonObject cabinObject)
                Cabin.Load(cabinObject);

            NpcStates.Clear();

            if (json.TryGetValue("npcStates", out var npcValue) && npcValue is JsonObject npcStates)
                foreach (var pair in npcStates)
                {
                    if (!Identifier.TryParse(pair.Key, out var npcId) || pair.Value is not JsonObject stateJson)
                        continue;

                    var definition = NpcRegistry.Get(npcId);

                    var state = definition?.StateFactory() ?? new NpcState();

                    state.Load(stateJson);

                    NpcStates[npcId] = state;
                }

            KeyItems.Clear();

            if (json.TryGetValue("keyItems", out var keyItemsValue) && keyItemsValue is JsonArray keyItemsArray)
                foreach (var entry in keyItemsArray)
                {
                    if (entry is not JsonObject keyItemJson)
                        continue;

                    var id = default(Identifier);

                    if (keyItemJson.TryGetValue("id", out var idValue) && Identifier.TryParse((string)idValue, out var parsedId))
                        id = parsedId;
                    else if (keyItemJson.TryGetValue("keyName", out var legacyName))
                        KeyItemIdMap.Resolve((string)legacyName, out id);

                    if (id.Path == "")
                        continue;

                    var item = CreateKeyItem(id);

                    if (keyItemJson.TryGetValue("values", out var valuesValue) && valuesValue is JsonArray valuesArray)
                        for (var index = 0; index < Math.Min(item.Values.Length, valuesArray.Count); index++)
                            item[index] = (float)valuesArray[index];
                    else
                    {
                        item[0] = keyItemJson.TryGetValue("value1", out var value1) ? (float)value1 : 0;

                        item[1] = keyItemJson.TryGetValue("value2", out var value2) ? (float)value2 : 0;

                        item[2] = keyItemJson.TryGetValue("value3", out var value3) ? (float)value3 : 0;

                        item[3] = keyItemJson.TryGetValue("value4", out var value4) ? (float)value4 : 0;
                    }
                }

            UpdateItemStacks();

            FixStorage();
        }

        public virtual void StartPregnancy(Identifier type, int incubation, int count = 0, Identifier eggType = default)
        {
            Pregnancy.Type = type;

            Pregnancy.Incubation = incubation;

            Pregnancy.Count = count;

            Pregnancy.EggType = eggType;

            if (Pregnancy.EggType.Path == "")
                Pregnancy.EggType = new Identifier("coc", "none");
        }

        public virtual void StartButtPregnancy(Identifier type, int incubation, int count = 0)
        {
            ButtPregnancy.Type = type;

            ButtPregnancy.Incubation = incubation;

            ButtPregnancy.Count = count;
        }

        public virtual void ClearPregnancy()
        {
            Pregnancy = new PregnancyState();
        }

        public virtual void ClearButtPregnancy()
        {
            ButtPregnancy = new PregnancyState();
        }

        public virtual void TickPregnancy()
        {
            if (Pregnancy.IsActive && --Pregnancy.Incubation <= 0)
                ClearPregnancy();

            if (ButtPregnancy.IsActive && --ButtPregnancy.Incubation <= 0)
                ClearButtPregnancy();
        }

        public virtual bool IsTaur() => LowerBodyType().GetString("body") == "taur";

        public virtual bool IsDrider() => LowerBodyType().GetString("body") == "drider";

        public virtual bool HasMultiTails()
        {
            var tail = Body.Get<TailPart>();

            return tail.Id.Path == "fox" && tail.Venom > 1;
        }

        /// <summary>原版 Player.hx:3873 isLongHaul()：长线模式开关。</summary>
        /// <summary>
        /// 原版 Character.hx:630 viridianChange()：任一阴茎穿了「紫罗兰色袜」且尚未变成 displacer。
        /// </summary>
        public virtual bool ViridianChange()
        {
            var count = CockTotal();

            if (count == 0)
                return false;

            while (count > 0)
            {
                count--;

                if (Cocks[count].sock == "amaranthine" && Cocks[count].Id != BodyPartTypes.Displacer)
                    return true;
            }

            return false;
        }

        public virtual bool IsLongHaul() => Settings.ModeSettings.Instance.LongHaul;

        public virtual int Eggs()
        {
            if (!HasPerk(PerkIds.SpiderOvipositor) && !HasPerk(PerkIds.BeeOvipositor))
                return -1;

            return HasPerk(PerkIds.SpiderOvipositor)

                ? (int)PerkValue(PerkIds.SpiderOvipositor, 1)

                : (int)PerkValue(PerkIds.BeeOvipositor, 1);
        }

        /// <summary>
        /// 原版 Creature.hx:3704 fertilizeEggs()：把当前蛋数写入产卵天赋的 v2（已受精数），返回受精蛋总数。
        /// </summary>
        public virtual int FertilizeEggs()
        {
            if (!HasPerk(PerkIds.SpiderOvipositor) && !HasPerk(PerkIds.BeeOvipositor))
                return -1;

            if (HasPerk(PerkIds.SpiderOvipositor))
                SetPerkValue(PerkIds.SpiderOvipositor, 2, Eggs());
            else
                SetPerkValue(PerkIds.BeeOvipositor, 2, Eggs());

            return FertilizedEggs();
        }

        public virtual bool CanOvipositSpider() =>

            Eggs() >= 10 &&

            HasPerk(PerkIds.SpiderOvipositor) &&

            IsDrider() &&

            Body.Get<TailPart>().Id.Path == "spider_abdomen";

        public virtual bool CanOvipositBee() =>

            Eggs() >= 10 &&

            HasPerk(PerkIds.BeeOvipositor) &&

            Body.Get<TailPart>().Id.Path == "bee_abdomen";

        public virtual bool HasOvipositor() => HasPerk(PerkIds.SpiderOvipositor) || HasPerk(PerkIds.BeeOvipositor);

        public virtual bool CanOviposit() => CanOvipositSpider() || CanOvipositBee();

        public virtual void DumpEggs()
        {
            if (HasPerk(PerkIds.SpiderOvipositor))
                AddPerkValue(PerkIds.SpiderOvipositor, 1, -PerkValue(PerkIds.SpiderOvipositor, 1));

            if (HasPerk(PerkIds.BeeOvipositor))
                AddPerkValue(PerkIds.BeeOvipositor, 1, -PerkValue(PerkIds.BeeOvipositor, 1));
        }

        public virtual bool PlayerResistSex(int targetGender, int chanceBonus = 0)
        {
            // 原版 Player.hx:4552：未开启「诱惑」模式、淫欲不足、或菜单无按钮时不会被诱惑
            if (!Settings.ModeSettings.Instance.Temptation || Lust < 33)
                return true;

            var chance = 10f + (Libido + Corruption) * 0.25f * Lust / MaxLust + chanceBonus;

            if (HasPerk(PerkIds.ImprovedSelfControl))
                chance *= 0.9f;

            if (HasPerk(PerkIds.CorruptedLibido))
                chance *= 0.9f;

            var male = SexOrientation / 100f;

            var female = (100 - SexOrientation) / 100f;

            if (targetGender == 2)
                chance *= female;

            if (targetGender == 1)
                chance *= male;

            return Random.Shared.NextDouble() * 100 >= chance;
        }

        public virtual BodyPartType LowerBodyType()
        {
            var legs = Body.Get<LowerBodyPart>();

            return BodyPartSlotRegistry.Get(legs.SlotId).Types.Get(legs.Id);
        }

        public virtual string BodyType()
        {
            var desc = "";

            if (Thickness < 10f)
            {
                if (Tone > 90f) desc += "a lithe body covered in highly visible muscles";
                else if (Tone > 75f) desc += "an incredibly thin, well-muscled frame";
                else if (Tone > 50f) desc += "a very thin body that has a good bit of muscle definition";
                else if (Tone > 25f) desc += "a lithe body and only a little bit of muscle definition";
                else if (Tone > 10f) desc += "a waif-thin body, and soft, forgiving flesh";
                else desc += "an incredibly thin, frail-looking body";
            }
            else if (Thickness < 25f)
            {
                if (Tone > 90f) desc += "a thin body and incredible muscle definition";
                else if (Tone > 75f) desc += "a narrow frame that shows off your muscles";
                else if (Tone > 50f) desc += "a somewhat lithe body and a fair amount of definition";
                else if (Tone > 25f) desc += "a narrow, soft body that still manages to show off a few muscles";
                else desc += "a thin, soft body";
            }
            else if (Thickness < 40f)
            {
                if (Tone > 90f) desc += "a fit, somewhat thin body and rippling muscles all over";
                else if (Tone > 75f) desc += "a thinner-than-average frame and great muscle definition";
                else if (Tone > 50f) desc += "a somewhat narrow body and a decent amount of visible muscle";
                else if (Tone > 25f) desc += "a moderately thin body, soft curves, and only a little bit of muscle";
                else desc += "a fairly thin form and soft, cuddle-able flesh";
            }
            else if (Thickness < 60f)
            {
                if (Tone > 90f) desc += "average thickness and a bevy of perfectly defined muscles";
                else if (Tone > 75f) desc += "an average-sized frame and great musculature";
                else if (Tone > 50f) desc += "a normal waistline and decently visible muscles";
                else if (Tone > 25f) desc += "an average body and soft, unremarkable flesh";
                else desc += "an average frame and soft, untoned flesh with a tendency for jiggle";
            }
            else if (Thickness < 75f)
            {
                if (Tone > 90f) desc += "a somewhat thick body that's covered in slabs of muscle";
                else if (Tone > 75f) desc += "a body that's a little bit wide and has some highly-visible muscles";
                else if (Tone > 50f) desc += "a solid build that displays a decent amount of muscle";
                else if (Tone > 25f) desc += "a slightly wide frame that displays your curves and has hints of muscle underneath";
                else desc += "a soft, plush body with plenty of jiggle";
            }
            else if (Thickness < 90f)
            {
                if (Tone > 90f) desc += "a thickset frame that gives you the appearance of a wall of muscle";
                else if (Tone > 75f) desc += "a burly form and plenty of muscle definition";
                else if (Tone > 50f) desc += "a solid, thick frame and a decent amount of muscles";
                else if (Tone > 25f) desc += "a wide-set body, some soft, forgiving flesh, and a hint of muscle underneath it";
                else
                {
                    desc += "a wide, cushiony body";

                    if (Gender >= Gender.Female || BiggestBreastSize() > 3f || Body.Hips > 7f || Body.Butt > 7f)
                        desc += " and plenty of jiggle on your curves";
                }
            }
            else
                if (Tone > 90f) desc += "an extremely thickset frame and so much muscle others would find you harder to move than a huge boulder";
                else if (Tone > 75f) desc += "a very wide body and enough muscle to make you look like a tank";
                else if (Tone > 50f) desc += "an extremely substantial frame packing a decent amount of muscle";
                else if (Tone > 25f)
                {
                    desc += "a very wide body";

                    if (Gender >= Gender.Female || BiggestBreastSize() > 4f || Body.Hips > 10f || Body.Butt > 10f)
                        desc += ", lots of curvy jiggles,";

                    desc += " and hints of muscle underneath";
                }
                else
                {
                    desc += "a thick";

                    if (Gender >= Gender.Female || BiggestBreastSize() > 4f || Body.Hips > 10f || Body.Butt > 10f)
                        desc += ", voluptuous";

                    desc += " body and plush, ";

                    desc += Gender >= Gender.Female || BiggestBreastSize() > 4f || Body.Hips > 10f || Body.Butt > 10f ? " jiggly curves" : " soft flesh";
                }

            return desc;
        }

        public virtual bool HasSpells() => SpellCount() > 0;

        public virtual float ModCumMultiplier(float delta)
        {
            if (delta == 0)
                return delta;

            if (delta > 0)
            {
                if (HasPerk(PerkIds.MessyOrgasms))
                    delta *= 1.5f;
            }
            else if ((delta < 0) && (HasPerk(PerkIds.MessyOrgasms)))
                    delta *= 0.5f;

            CumMultiplier += delta;

            return delta;
        }

        public virtual int SpellCount(string magic = "")
        {
            Identifier[] spells = magic switch
            {
                "white" => StatusEffectIds.SpellsWhite,

                "black" => StatusEffectIds.SpellsBlack,

                "gray" => StatusEffectIds.SpellsGray,

                _ =>

                [
                    .. StatusEffectIds.SpellsWhite, .. StatusEffectIds.SpellsBlack,
                    .. StatusEffectIds.SpellsGray, .. StatusEffectIds.SpellsOther,
                ],
            };

            var count = 0;

            foreach (var spell in spells)
                if (HasStatusEffect(spell))
                    count++;

            return count;
        }
    }
}
