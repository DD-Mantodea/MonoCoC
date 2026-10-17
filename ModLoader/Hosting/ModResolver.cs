using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.ModLoader
{
    public static class ModResolver
    {
        public static ModResolveResult Resolve(
            IReadOnlyList<ModCandidate> installed,
            IEnumerable<string> enabledIds)
        {
            var result = new ModResolveResult();

            var byId = new Dictionary<string, ModCandidate>(StringComparer.OrdinalIgnoreCase);

            foreach (var candidate in installed)
            {
                if (!candidate.IsValid || candidate.Manifest is null)
                {
                    result.Warnings.Add($"{candidate.PackagePath}: {candidate.Error ?? "invalid manifest"}");
                    continue;
                }

                if (!byId.TryAdd(candidate.Manifest.Id, candidate))
                    result.Errors.Add($"duplicate mod id: {candidate.Manifest.Id}");
            }

            var enabled = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var id in enabledIds)
                if (byId.ContainsKey(id))
                    enabled.Add(id);
                else
                    result.Warnings.Add($"enabled mod is not installed: {id}");

            foreach (var id in enabled.ToArray())
                EnableWithDependencies(id, byId, enabled, result);

            if (result.Errors.Count > 0)
                return result;

            foreach (var id in enabled)
                CheckDependencies(byId[id], byId, result);

            if (result.Errors.Count > 0)
                return result;

            var ordered = Sort(enabled, byId, result);

            result.EnabledIds.AddRange(ordered.Select(candidate => candidate.Manifest!.Id));

            result.Mods.AddRange(ordered);

            return result;
        }

        private static void EnableWithDependencies(
            string id,
            Dictionary<string, ModCandidate> byId,
            HashSet<string> enabled,
            ModResolveResult result)
        {
            if (!byId.TryGetValue(id, out var candidate) || candidate.Manifest is null)
                return;

            foreach (var dependency in candidate.Manifest.Dependencies)
            {
                if (dependency.Weak)
                    continue;

                if (!byId.ContainsKey(dependency.Id))
                {
                    result.Errors.Add($"{candidate.Manifest.Id}: missing required dependency {dependency.Id}");
                    continue;
                }

                if (enabled.Add(dependency.Id))
                    EnableWithDependencies(dependency.Id, byId, enabled, result);
            }
        }

        private static void CheckDependencies(
            ModCandidate candidate,
            Dictionary<string, ModCandidate> byId,
            ModResolveResult result)
        {
            var manifest = candidate.Manifest!;

            foreach (var dependency in manifest.Dependencies)
            {
                if (!byId.TryGetValue(dependency.Id, out var dependencyMod) || dependencyMod.Manifest is null)
                {
                    if (!dependency.Weak)
                        result.Errors.Add($"{manifest.Id}: missing required dependency {dependency.Id}");

                    continue;
                }

                if (!dependency.Range.IsSatisfiedBy(dependencyMod.Manifest.Version))
                {
                    var message = $"{manifest.Id}: requires {dependency.Id} {dependency.Range}, installed {dependencyMod.Manifest.Version}";

                    if (dependency.Weak)
                        result.Warnings.Add(message);
                    else
                        result.Errors.Add(message);
                }
            }
        }

        private static List<ModCandidate> Sort(
            HashSet<string> enabled,
            Dictionary<string, ModCandidate> byId,
            ModResolveResult result)
        {
            var nodes = enabled.ToDictionary(id => id, id => byId[id], StringComparer.OrdinalIgnoreCase);

            var outgoing = nodes.Keys.ToDictionary(id => id, _ => new HashSet<string>(StringComparer.OrdinalIgnoreCase), StringComparer.OrdinalIgnoreCase);

            var incoming = nodes.Keys.ToDictionary(id => id, _ => 0, StringComparer.OrdinalIgnoreCase);

            void AddEdge(string from, string to)
            {
                if (from == to || !nodes.ContainsKey(from) || !nodes.ContainsKey(to))
                    return;

                if (outgoing[from].Add(to))
                    incoming[to]++;
            }

            foreach (var (id, candidate) in nodes)
            {
                var manifest = candidate.Manifest!;

                foreach (var dependency in manifest.Dependencies)
                    if (nodes.ContainsKey(dependency.Id))
                        AddEdge(dependency.Id, id);

                foreach (var after in manifest.LoadAfter)
                    AddEdge(after, id);

                foreach (var before in manifest.LoadBefore)
                    AddEdge(id, before);
            }

            var ready = new SortedSet<string>(nodes.Keys.Where(id => incoming[id] == 0), StringComparer.OrdinalIgnoreCase);

            var ordered = new List<ModCandidate>();

            while (ready.Count > 0)
            {
                var id = ready.Min!;

                ready.Remove(id);

                ordered.Add(nodes[id]);

                foreach (var next in outgoing[id])
                    if (--incoming[next] == 0)
                        ready.Add(next);
            }

            if (ordered.Count != nodes.Count)
            {
                var cyclic = nodes.Keys.Where(id => incoming[id] > 0).OrderBy(id => id, StringComparer.OrdinalIgnoreCase);

                result.Errors.Add("dependency cycle: " + string.Join(" -> ", cyclic));
            }

            return ordered;
        }
    }
}
