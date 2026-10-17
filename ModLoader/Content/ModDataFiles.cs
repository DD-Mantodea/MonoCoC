using Hjson;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.ModLoader
{
    /// <summary>
    /// Scoped access to Data/**.hjson and other files inside a mod.
    /// </summary>
    public sealed class ModDataFiles : IDataFileSource
    {
        private readonly PackageDataFileSource _source;

        internal ModDataFiles(ModFiles files)
        {
            _source = new PackageDataFileSource(files, "Data");
        }

        public string ModId => _source.Name;

        public string Name => _source.Name;

        public bool Exists(string path) => _source.Exists(path);

        public IEnumerable<string> Enumerate(string folder, string pattern = "*.hjson") =>
            _source.Enumerate(folder, pattern);

        public Stream OpenRead(string path) => _source.OpenRead(path);

        public string ReadAllText(string path) => _source.ReadAllText(path);

        public JsonObject ReadHjson(string path)
        {
            var text = ReadAllText(path);

            return HjsonValue.Parse(text) as JsonObject
                ?? throw new InvalidDataException($"{ModId}: '{path}' is not a Hjson object");
        }
    }
}
