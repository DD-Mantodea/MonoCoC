using MonoCoC.Assets;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Tags;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Npcs
{
    public interface INpc
    {
        NpcDefinition Definition { get; }

        NpcState State { get; }

        void Talk();
    }

    public interface ICombatNpc : INpc
    {
        Enemy CreateEnemy();
    }

    public class NpcDefinition : IIdentifiable, ITaggable
    {
        public Identifier Id { get; set; }

        public string NameKey = "";

        public Identifier SceneId;

        public Identifier? CombatId;

        public bool DefaultHostile;

    public List<string> Tags = [];

    IReadOnlyList<string> ITaggable.Tags => Tags;

        public Func<NpcState> StateFactory = () => new NpcState();
    }

    public static class NpcRegistry
    {
        public static Registry<NpcDefinition> Npcs => ManagerInstance<NpcManager>.Instance.Registry;

        public static NpcDefinition? Get(Identifier id) =>

            Npcs.TryGet(id, out var npc) ? npc : null;

        public static NpcDefinition? Get(string id) => Get(Identifier.Parse(id));

        public static IReadOnlyList<NpcDefinition> All() =>

            Npcs.Ids.Select(Get).OfType<NpcDefinition>().ToList();

        public static void Register(NpcDefinition npc) =>

            Npcs.Register(npc.Id, () => npc);
    }
}