using Hjson;
using MonoCoC.Contents.BodyParts;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.ModLoader;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Assets.ContentManagers
{
    public class BodyPartManager : ContentManager<BodyPartSlot>
    {
        private static readonly Dictionary<Identifier, Func<BodyPart>> Factories = [];

        private readonly List<Registry<BodyPartType>> _typeRegistries = [];

        public override string Folder => "Data/BodyParts";

        public override string Pattern => "*.hjson";

        public override Registry<BodyPartSlot> Registry => BodyPartSlotRegistry.Registry;

        public override void Load()
        {
            foreach (var file in Paths.GetAssets(Folder, Pattern))
            {
                var entries = new Dictionary<string, BodyPartSlot>();

                FromText(file, File.ReadAllText(file), entries);

                foreach (var pair in entries)
                    Store(pair.Key, pair.Value, null);
            }
        }

        public override void FromText(string path, string text, IDictionary<string, BodyPartSlot> entries)
        {
            var config = (JsonObject)HjsonValue.Parse(text);

            if (!config.TryGetValue("kind", out var kindValue))
                throw new FormatException($"Body part config '{path}' has no 'kind' field");

            var slotId = Identifier.Parse((string)kindValue);

            if (!Factories.TryGetValue(slotId, out var factory))
                throw new FormatException($"Unknown body part kind '{slotId}' in '{path}'");

            if (!config.TryGetValue("types", out var types) || types is not JsonArray array)
                throw new FormatException($"Body part config '{path}' has no 'types' array");

            var owner = ModContext.Current?.Manifest.Id;

            var definition = new BodyPartSlot
            {
                Id = slotId,

                DefaultType = config.TryGetValue("default", out var defaultId) ? Identifier.Parse((string)defaultId) : default,

                Collection = config.TryGetValue("collection", out var collectionValue) && (bool)collectionValue,

                Factory = factory,

                Config = config
            };

            _typeRegistries.Add(definition.Types);

            foreach (var item in array)
            {
                if (item is not JsonObject entry || !entry.TryGetValue("id", out var idValue))
                    throw new FormatException($"Body part config '{path}' has an invalid type entry");

                var type = new BodyPartType
                {
                    Id = Identifier.Parse((string)idValue),

                    Slot = slotId,

                    DescKey = entry.TryGetValue("desc", out var desc) ? (string)desc : "",

                    Config = entry
                };

                definition.Types.Register(type.Id, () => type, owner);
            }

            definition.Types.SetDefault(definition.DefaultType);

            entries[definition.Id.ToString()] = definition;
        }

        public override void RemoveModData(string owner)
        {
            foreach (var registry in _typeRegistries)
                registry.RemoveAll(owner);

            _typeRegistries.RemoveAll(registry => registry.Ids.Count == 0);

            base.RemoveModData(owner);
        }


        static BodyPartManager() => RegisterVanillaFactories();

        private static void RegisterVanillaFactories()
        {
            var factories = new Dictionary<Identifier, Func<BodyPart>>()
            {
                [BodyPartSlots.Tail] = () => new TailPart(),
                [BodyPartSlots.Skin] = () => new SkinPart(),
                [BodyPartSlots.Hair] = () => new HairPart(),
                [BodyPartSlots.Face] = () => new FacePart(),
                [BodyPartSlots.Claws] = () => new ClawsPart(),
                [BodyPartSlots.Beard] = () => new BeardPart(),
                [BodyPartSlots.Eyes] = () => new EyesPart(),
                [BodyPartSlots.Arms] = () => new ArmsPart(),
                [BodyPartSlots.LowerBody] = () => new LowerBodyPart(),
                [BodyPartSlots.Tongue] = () => new TonguePart(),
                [BodyPartSlots.RearBody] = () => new RearBodyPart(),
                [BodyPartSlots.Neck] = () => new NeckPart(),
                [BodyPartSlots.Wings] = () => new WingsPart(),
                [BodyPartSlots.UnderBody] = () => new UnderBodyPart(),
                [BodyPartSlots.Horns] = () => new HornsPart(),
                [BodyPartSlots.Ears] = () => new EarsPart(),
                [BodyPartSlots.Cock] = () => new CockPart(),
                [BodyPartSlots.Vagina] = () => new VaginaPart(),
                [BodyPartSlots.BreastRow] = () => new BreastRowPart(),
                [BodyPartSlots.Anal] = () => new AnalPart(),

                [BodyPartSlots.Butt] = () => new ButtPart(),

                [BodyPartSlots.Hips] = () => new HipsPart(),

                [BodyPartSlots.Gills] = () => new GillsPart(),

                [BodyPartSlots.Antennae] = () => new AntennaePart(),
            };

            foreach (var pair in factories)
                Factories[pair.Key] = pair.Value;
        }
    }
}