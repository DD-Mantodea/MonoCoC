using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MonoCoC.ModLoader
{
    /// <summary>
    /// Builds an uncompressed .cocmod package from a directory.
    /// </summary>
    public static class CocModPackageBuilder
    {
        private const string MagicText = "COCMOD\0\0";
        private const uint FormatVersion = 1;

        public static void Build(string sourceDirectory, string outputPath)
        {
            if (!Directory.Exists(sourceDirectory))
                throw new DirectoryNotFoundException(sourceDirectory);

            var files = Directory
                .GetFiles(sourceDirectory, "*", SearchOption.AllDirectories)
                .Select(path => new
                {
                    FullPath = path,
                    Name = Path.GetRelativePath(sourceDirectory, path).Replace('\\', '/')
                })
                .OrderBy(file => file.Name, StringComparer.Ordinal)
                .ToList();

            if (files.All(file => file.Name != "mod.hjson"))
                throw new InvalidDataException("source directory does not contain mod.hjson");

            var outputDirectory = Path.GetDirectoryName(Path.GetFullPath(outputPath));

            if (!string.IsNullOrEmpty(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            using var stream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None);
            using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);

            writer.Write(Encoding.ASCII.GetBytes(MagicText));
            writer.Write(FormatVersion);
            writer.Write((uint)files.Count);
            writer.Write(0u);

            var payloadOffset = stream.Position + files.Sum(file => 2 + Encoding.UTF8.GetByteCount(file.Name) + 8 + 8 + 32);

            var metadata = new List<(string Name, long Offset, long Length, byte[] Hash)>();

            foreach (var file in files)
            {
                var bytes = File.ReadAllBytes(file.FullPath);

                var hash = SHA256.HashData(bytes);

                metadata.Add((file.Name, payloadOffset, bytes.Length, hash));

                payloadOffset += bytes.Length;
            }

            foreach (var entry in metadata)
            {
                var name = Encoding.UTF8.GetBytes(entry.Name);

                writer.Write((ushort)name.Length);
                writer.Write(name);
                writer.Write(entry.Offset);
                writer.Write(entry.Length);
                writer.Write(entry.Hash);
            }

            foreach (var file in files)
                writer.Write(File.ReadAllBytes(file.FullPath));
        }
    }
}
