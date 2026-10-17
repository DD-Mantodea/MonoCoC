using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;

namespace MonoCoC.ModLoader
{
    public sealed class PackageDataFileSource : IDataFileSource
    {
        private readonly ModFiles _files;
        private readonly string _root;

        public PackageDataFileSource(ModFiles files, string root)
        {
            _files = files;
            _root = root.Trim('/');
        }

        public string Name => _files.ModId;

        public bool Exists(string path) => _files.Exists(Combine(path));

        public IEnumerable<string> Enumerate(string folder, string pattern)
        {
            var prefix = Combine(folder);

            foreach (var path in _files.GetFileNames())
            {
                if (!path.StartsWith(prefix + "/", StringComparison.Ordinal) &&
                    !string.Equals(path, prefix, StringComparison.Ordinal))
                    continue;

                if (!FileSystemName.MatchesSimpleExpression(pattern, Path.GetFileName(path), ignoreCase: true))
                    continue;

                var relative = _root.Length > 0 && path.StartsWith(_root + "/", StringComparison.Ordinal)
                    ? path[(_root.Length + 1)..]
                    : path;

                yield return relative;
            }
        }

        public Stream OpenRead(string path) => _files.OpenRead(Combine(path));

        public string ReadAllText(string path) => _files.ReadAllText(Combine(path));

        private string Combine(string path)
        {
            var normalized = path.Replace('\\', '/').TrimStart('/');

            return _root.Length == 0 ? normalized : _root + "/" + normalized;
        }
    }
}
