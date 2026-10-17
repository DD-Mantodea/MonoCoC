using System.Collections.Generic;

namespace MonoCoC.Contents.Tags
{
    public static class TagSet
    {
        public static string Normalize(string tag) => tag.Replace(" ", "").ToLowerInvariant();

        public static List<string> NormalizeAll(IEnumerable<string> tags)
        {
            var result = new List<string>();

            foreach (var tag in tags)
                result.Add(Normalize(tag));

            return result;
        }

        public static bool Has(ITaggable entity, string tag)
        {
            var normalized = Normalize(tag);

            var tags = entity.Tags;

            for (var index = 0; index < tags.Count; index++)
                if (tags[index] == normalized)
                    return true;

            return false;
        }

        public static bool HasAny(ITaggable entity, params string[] wanted)
        {
            var tags = entity.Tags;

            for (var index = 0; index < tags.Count; index++)
                foreach (var candidate in wanted)
                    if (tags[index] == Normalize(candidate))
                        return true;

            return false;
        }

        public static bool HasAll(ITaggable entity, params string[] wanted)
        {
            foreach (var candidate in wanted)
                if (!Has(entity, candidate))
                    return false;

            return true;
        }

        public static IEnumerable<T> WithTags<T>(IEnumerable<T> entities, params string[] tags) where T : ITaggable
        {
            foreach (var entity in entities)
                if (HasAny(entity, tags))
                    yield return entity;
        }

        public static IEnumerable<T> WithoutTags<T>(IEnumerable<T> entities, params string[] tags) where T : ITaggable
        {
            foreach (var entity in entities)
                if (!HasAny(entity, tags))
                    yield return entity;
        }
    }
}