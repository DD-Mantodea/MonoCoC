using MonoCoC.Contents.Items.Behaviors;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Attributes;
using MonoCoC.Contents.Tags;
using System.Collections.Generic;

namespace MonoCoC.Contents.Items
{
    [Register<Item>("coc:default")]
    public class Item : IIdentifiable, ITaggable
    {
        public static readonly Item Nothing = new() { Id = new Identifier("coc", "nothing"), Name = "nothing", LongName = "nothing" };

        public Identifier Id { get; set; }

        public ItemSave Save { get; } = new();

        public virtual bool knownBound() => Save.GetBool("bound") && Save.GetBool("unequipTried");

        public virtual bool KnownBound() => knownBound();

        public virtual bool IsNormalShirt() => !Save.GetBool("bound") && !Save.GetBool("enchanted");

        public string Name = "";

        public string LongName = "";

        public string ShortNameKey = "";

        public string LongNameKey = "";

        public string DescriptionKey = "";

        public string UseTextKey = "";

        public string HeaderKey = "";

        public float Value;

        public int MaxStackSize = 5;

        public int Durability;

        public bool Degradable;

        public string DegradesInto = "";

        public int Stage = 1;

        public ItemDefinition? Definition;

        public List<string> Tags = [];

        IReadOnlyList<string> ITaggable.Tags => Tags;

        public virtual bool HasTag(string tag) => TagSet.Has(this, tag);

        public virtual bool Matches(string ruleId) => TagQuery.Matches(this, ruleId);

        public IItemBehavior? Behavior;

        public virtual Item PlayerEquip() => this;

        public virtual Item PlayerRemove() => this;

        public bool InvUseOnly;

        public virtual bool CanUse() => false;

        public virtual bool UseItem() => false;

        public virtual void OnUse() { }

        public virtual void UseText()
        {
            Scenes.Scene.OutputText(UseTextKey.Length > 0

                ? StringExt.Localized(UseTextKey)

                : StringExt.Localized("Items.UseText.Default", LongName));
        }

        public virtual void RemoveText()
        {
            Scenes.Scene.OutputText(StringExt.Localized("Items.RemoveText.Default", LongName));
        }

        public virtual void Configure(ItemDefinition definition)
        {
            Definition = definition;

            Id = definition.Id;

            ShortNameKey = definition.ShortNameKey;

            LongNameKey = definition.LongNameKey;

            DescriptionKey = definition.DescriptionKey;

            UseTextKey = definition.UseTextKey;

            HeaderKey = definition.HeaderKey;

            Name = definition.ShortNameKey == "" ? definition.Id.Path : definition.ShortNameKey.Localized();

            LongName = definition.LongNameKey == "" ? Name : definition.LongNameKey.Localized();

            Value = definition.Value;

            MaxStackSize = definition.MaxStackSize;

            InvUseOnly = definition.InvUseOnly;

            Tags = TagSet.NormalizeAll(definition.Tags);

            Durability = definition.Durability;

            Degradable = definition.Durability > 0;

            DegradesInto = definition.DegradesInto;

            Behavior = null;

            if (definition.Behavior != "" && Identifier.TryParse(definition.Behavior, out var behaviorId))
                Behavior = ItemBehaviorRegistry.Resolve(behaviorId);
        }

        public string TooltipText
        {
            get
            {
                var text = DescriptionKey == "" ? LongName : DescriptionKey.Localized();

                if (Behavior is IItemDescriptionBehavior description && CoC.Instance.Player != null)
                    return description.GetDescription(this, CoC.Instance.Player, text);

                return text;
            }
        }

        public string TooltipHeader => HeaderKey == "" ? Name : HeaderKey.Localized();

        public string InvLabel => Name;

        public static bool operator ==(Item? item, Identifier id) =>

            item is null ? id.Equals(default) : item.Id.Equals(id);

        public static bool operator !=(Item? item, Identifier id) => !(item == id);

        public override bool Equals(object? obj) => obj switch
        {
            Item other => Id.Equals(other.Id),

            Identifier id => Id.Equals(id),

            _ => false,
        };

        public override int GetHashCode() => Id.GetHashCode();
    }
}