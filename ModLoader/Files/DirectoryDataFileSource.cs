using System.Collections.Generic;
using System.IO;

namespace MonoCoC.ModLoader
{
    public sealed class DirectoryDataFileSource : IDataFileSource
    {
        private readonly string _root;

        public DirectoryDataFileSource(string name, string root)
        {
            Name = name;
            _root = root;
        }

        public string Name { get; }

        public bool Exists(string path) => File.Exists(Combine(path));

        public IEnumerable<string> Enumerate(string folder, string pattern)
        {
            var directory = Combine(folder);

            if (!Directory.Exists(directory))
                yield break;

            foreach (var file in Directory.EnumerateFiles(directory, pattern, SearchOption.AllDirectories))
                yield return Path.GetRelativePath(_root, file).Replace('\\', '/');
        }

        public Stream OpenRead(string path) => File.OpenRead(Combine(path));

        public string ReadAllText(string path) => File.ReadAllText(Combine(path));

        private string Combine(string path) => Path.Combine(_root, path.Replace('/', Path.DirectorySeparatorChar));
    }
}
