using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace MonoCoC.ModLoader
{
    /// <summary>
    /// Reader for the uncompressed .cocmod container.
    /// </summary>
    public sealed class CocModPackage : IDisposable
    {
        private const string MagicText = "COCMOD\0\0";
        private const uint FormatVersion = 1;

        private readonly FileStream _stream;
        private readonly SafeFileHandle _handle;
        private readonly Dictionary<string, CocModEntry> _entries;
        private readonly object _ioLock = new();
        private bool _disposed;

        public string Path { get; }

        public IReadOnlyDictionary<string, CocModEntry> Entries => _entries;

        private CocModPackage(string path, FileStream stream, Dictionary<string, CocModEntry> entries)
        {
            Path = path;
            _stream = stream;
            _handle = stream.SafeFileHandle;
            _entries = entries;
        }

        public static CocModPackage Open(string path)
        {
            var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.RandomAccess);

            try
            {
                using var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);

                var magic = reader.ReadBytes(MagicText.Length);

                if (Encoding.ASCII.GetString(magic) != MagicText)
                    throw new InvalidDataException($"{path}: not a .cocmod file");

                var format = reader.ReadUInt32();

                if (format != FormatVersion)
                    throw new InvalidDataException($"{path}: unsupported .cocmod format {format}");

                var count = reader.ReadUInt32();

                if (count > 1_000_000)
                    throw new InvalidDataException($"{path}: too many entries");

                reader.ReadUInt32();

                var entries = new Dictionary<string, CocModEntry>(StringComparer.Ordinal);

                for (var index = 0; index < count; index++)
                {
                    var pathLength = reader.ReadUInt16();

                    var entryPath = Encoding.UTF8.GetString(reader.ReadBytes(pathLength)).Replace('\\', '/');

                    var offset = reader.ReadInt64();

                    var length = reader.ReadInt64();

                    var hash = reader.ReadBytes(32);

                    if (offset < 0 || length < 0 || offset + length > stream.Length)
                        throw new InvalidDataException($"{path}: invalid entry bounds for '{entryPath}'");

                    entries[entryPath] = new CocModEntry(entryPath, offset, length, hash);
                }

                return new CocModPackage(path, stream, entries);
            }
            catch
            {
                stream.Dispose();
                throw;
            }
        }

        public bool Contains(string path) => _entries.ContainsKey(Normalize(path));

        public IReadOnlyList<string> GetFileNames() => new List<string>(_entries.Keys);

        public byte[] ReadAllBytes(string path)
        {
            var entry = GetEntry(path);

            if (entry.Length > int.MaxValue)
                throw new InvalidDataException($"{Path}: entry '{path}' is too large");

            var buffer = new byte[(int)entry.Length];

            ReadAt(entry.Offset, buffer);

            return buffer;
        }

        public Stream OpenRead(string path) => new CocModSubStream(this, GetEntry(path));

        public string ReadAllText(string path) => Encoding.UTF8.GetString(ReadAllBytes(path));

        public ModManifest ReadManifest()
        {
            if (!Contains("mod.hjson"))
                throw new InvalidDataException($"{Path}: missing mod.hjson");

            return ModManifest.Parse(ReadAllText("mod.hjson"), Path);
        }

        public bool Verify(string path)
        {
            var entry = GetEntry(path);

            if (entry.Hash.Length != 32)
                return false;

            var actual = SHA256.HashData(ReadAllBytes(path));

            return CryptographicOperations.FixedTimeEquals(entry.Hash, actual);
        }

        private CocModEntry GetEntry(string path)
        {
            var normalized = Normalize(path);

            if (_entries.TryGetValue(normalized, out var entry))
                return entry;

            throw new FileNotFoundException($"{Path}: entry not found: {path}", path);
        }

        private void ReadAt(long offset, Span<byte> buffer)
        {
            lock (_ioLock)
            {
                var total = 0;

                while (total < buffer.Length)
                {
                    var read = RandomAccess.Read(_handle, buffer[total..], offset + total);

                    if (read == 0)
                        throw new EndOfStreamException($"{Path}: unexpected end of entry");

                    total += read;
                }
            }
        }

        internal int ReadAt(long offset, byte[] buffer, int bufferOffset, int count)
        {
            lock (_ioLock)
            {
                var total = 0;

                var rented = ArrayPool<byte>.Shared.Rent(count);

                try
                {
                    while (total < count)
                    {
                        var read = RandomAccess.Read(_handle, rented.AsSpan(total, count - total), offset + total);

                        if (read == 0)
                            break;

                        total += read;
                    }

                    Array.Copy(rented, 0, buffer, bufferOffset, total);

                    return total;
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(rented);
                }
            }
        }

        private static string Normalize(string path) => path.Replace('\\', '/').TrimStart('/');

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _stream.Dispose();
        }
    }
}
