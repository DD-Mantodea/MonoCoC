using Hjson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MonoCoC.ModLoader
{
    /// <summary>
    /// tML-style access to files inside a .cocmod package.
    /// </summary>
    public sealed class ModFiles
    {
        private readonly CocModPackage _package;

        public string ModId { get; }

        internal ModFiles(string modId, CocModPackage package)
        {
            ModId = modId;
            _package = package;
        }

        public IReadOnlyList<string> GetFileNames() => _package.GetFileNames();

        public bool Exists(string path) => _package.Contains(path);

        public byte[] ReadAllBytes(string path) => _package.ReadAllBytes(path);

        public byte[] GetFileBytes(string path) => ReadAllBytes(path);

        public Stream OpenRead(string path) => _package.OpenRead(path);

        public Stream GetFileStream(string path) => OpenRead(path);

        public string ReadAllText(string path) => _package.ReadAllText(path);

        public string GetFileText(string path) => ReadAllText(path);

        public bool FileExists(string path) => Exists(path);

        public JsonObject ReadHjson(string path) =>
            HjsonValue.Parse(ReadAllText(path)) as JsonObject
            ?? throw new InvalidDataException($"{ModId}: '{path}' is not a Hjson object");

        public override string ToString() => $"{ModId} files";
    }
}
