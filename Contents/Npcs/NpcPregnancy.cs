using Hjson;
using MonoCoC.Assets;
using MonoCoC.Attributes;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MonoCoC.Contents.Npcs
{

    public class NpcPregnancyDefinition : IIdentifiable
    {
        public Identifier Id { get; set; }

        public float Incubation = 216;

        public int[] Events = [];

        public Identifier? OnBirth;
    }

    public class NpcPregnancyManager : ContentManager<NpcPregnancyDefinition>
    {
        public override string Folder => "Data/Npcs/Pregnancy";

        public override string Pattern => "*.hjson";

        public static NpcPregnancyDefinition? Get(Identifier id) =>

            ManagerInstance<NpcPregnancyManager>.Instance.Dict.TryGetValue(id.ToString(), out var definition)

                ? definition

                : null;

        public override void FromFile(string path, IDictionary<string, NpcPregnancyDefinition> entries)
        {
            var root = (JsonObject)HjsonValue.Parse(File.ReadAllText(path));

            var id = Str(root, "id");

            if (!Identifier.TryParse(id, out var identifier))
                throw new FormatException($"Invalid NPC pregnancy id '{id}' in '{path}'");

            var definition = new NpcPregnancyDefinition
            {
                Id = identifier,

                Incubation = Float(root, "incubation", 216)
            };

            if (root.TryGetValue("events", out var eventsValue) && eventsValue is JsonArray events)
            {
                var list = new List<int>();

                foreach (var entry in events)
                    list.Add(int.Parse(entry.ToString()));

                definition.Events = [.. list];
            }

            if (Identifier.TryParse(Str(root, "onBirth"), out var onBirth))
                definition.OnBirth = onBirth;

            entries[identifier.ToString()] = definition;
        }

        public static string Str(JsonObject root, string key, string fallback = "")
        {
            if (!root.TryGetValue(key, out var value) || value == null)
                return fallback;

            var text = value.ToString();

            if (text.Length > 1 && text[0] == '"' && text[^1] == '"')
                text = text[1..^1];

            return text;
        }

        public static int Int(JsonObject root, string key, int fallback = 0) =>

            int.TryParse(Str(root, key), out var value) ? value : fallback;

        public static float Float(JsonObject root, string key, float fallback = 0) =>

            float.TryParse(Str(root, key), out var value) ? value : fallback;
    }

    public static class NpcBirthRegistry
    {
        static NpcBirthRegistry() => RegistrationScanner.OnMethods(RegisterMethod);

        public static readonly Dictionary<Identifier, Action<NpcState>> Handlers = [];

        public static readonly Type Signature = typeof(Action<NpcState>);

        public static void Invoke(Identifier id, NpcState state)
        {
            if (Handlers.TryGetValue(id, out var handler))
                handler(state);
        }

        internal static bool RegisterMethod(MethodInfo method, object[] attributes)
        {
            if (method.ReturnType != typeof(void))
                return false;

            var parameters = method.GetParameters();

            if (parameters.Length != 1 || parameters[0].ParameterType != typeof(NpcState))
                return false;

            var handler = (Action<NpcState>)method.CreateDelegate(Signature);

            foreach (RegisterAttribute attribute in attributes)
                Handlers[Identifier.Parse(attribute.Id)] = handler;

            return true;
        }
    }

    public static class NpcPregnancy
    {
        public static NpcPregnancyDefinition? DefinitionOf(NpcDefinition npc) => NpcPregnancyManager.Get(npc.Id);

        public static bool IsPregnant(NpcDefinition npc, NpcState state) => state.IsPregnant;

        public static PregnancyStore StoreFor(NpcDefinition npc, NpcState state)
        {
            var store = new PregnancyStore(new NpcStatePregnancyBacking(state));

            var definition = DefinitionOf(npc);

            if (definition != null && definition.Events.Length > 0)
                store.AddEventSet(PregnancyIds.PlayerFather, definition.Events);

            return store;
        }

        public static PregnancyStore StoreForButt(NpcDefinition npc, NpcState state)
        {
            var store = new PregnancyStore(new NpcStateButtPregnancyBacking(state));

            var definition = DefinitionOf(npc);

            if (definition != null && definition.Events.Length > 0)
                store.AddEventSet(PregnancyIds.PlayerFather, definition.Events);

            return store;
        }

        public static void KnockUp(NpcDefinition npc, NpcState state, Identifier type, float? incubation = null)
        {
            if (state.IsPregnant)
                return;

            var definition = DefinitionOf(npc);

            state.PregnancyType = type;

            state.PregnancyIncubation = incubation ?? definition?.Incubation ?? 216;
        }

        public static void Clear(NpcState state)
        {
            state.PregnancyType = default;

            state.PregnancyIncubation = 0;
        }

        public static int EventStage(NpcDefinition npc, NpcState state)
        {
            var definition = DefinitionOf(npc);

            if (definition == null || !state.IsPregnant)
                return 0;

            var stage = 0;

            foreach (var threshold in definition.Events)
            {
                if (state.PregnancyIncubation > threshold)
                    break;

                stage++;
            }

            return stage;
        }

        public static void Tick()
        {
            var player = CoC.Instance?.Player;

            if (player == null)
                return;

            foreach (var pair in player.NpcStates)
            {
                var state = pair.Value;

                if (!state.IsPregnant)
                    continue;

                state.PregnancyIncubation = Math.Max(0, state.PregnancyIncubation - 1);

                if (state.PregnancyIncubation > 0)
                    continue;

                var definition = NpcPregnancyManager.Get(pair.Key);

                if (definition?.OnBirth is { } onBirth)
                    NpcBirthRegistry.Invoke(onBirth, state);
            }
        }
    }
}