using System.Collections.Generic;
using System.IO;

namespace MonoCoC.ModLoader
{
    /// <summary>
    /// Read-only source for data files, independent of physical directories.
    /// </summary>
    public interface IDataFileSource
    {
        string Name { get; }

        bool Exists(string path);

        IEnumerable<string> Enumerate(string folder, string pattern);

        Stream OpenRead(string path);

        string ReadAllText(string path);
    }
}
