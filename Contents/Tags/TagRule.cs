using MonoCoC.Assets;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Registry;
using System.Collections.Generic;

namespace MonoCoC.Contents.Tags
{
    public class TagRule : IIdentifiable
    {
        public Identifier Id { get; set; }

        public List<string> Any = [];

        public List<string> Always = [];

        public List<string> Never = [];

        public List<string> All = [];
    }

    public static class TagRuleRegistry
    {
        public static Registry<TagRule> Rules =>

            ManagerInstance<TagRuleManager>.Instance.Registry;

        public static TagRule? Get(string id) =>

            Rules.TryGet(new Identifier("coc", id), out var rule) ? rule : null;

        public static TagRule? Get(Identifier id) =>

            Rules.TryGet(id, out var rule) ? rule : null;
    }

    public static class TagQuery
    {
        public static bool Matches(ITaggable entity, string ruleId)
        {
            var rule = TagRuleRegistry.Get(ruleId);

            if (rule == null)
                return false;

            var tags = entity.Tags;

            foreach (var tag in tags)
            {
                if (rule.Never.Contains(tag)) return false;
                if (rule.Always.Contains(tag)) return true;
            }

            if (rule.All.Count > 0)
            {
                foreach (var required in rule.All)
                    if (!TagSet.Has(entity, required))
                        return false;

                return true;
            }

            if (rule.Any.Count == 0)
                return false;

            foreach (var tag in tags)
                if (rule.Any.Contains(tag))
                    return true;

            return false;
        }

        public static bool Matches(ITaggable entity, Identifier ruleId) =>

            Matches(entity, ruleId.Path);
    }
}