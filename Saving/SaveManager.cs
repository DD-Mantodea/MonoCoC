using Hjson;
using MonoCoC.Assets;
using System;
using System.IO;
using System.Linq;

namespace MonoCoC.Saving
{
    public static class SaveManager
    {
        private static readonly string[] ReservedNames =

        [
            "CON", "PRN", "AUX", "NUL",

            "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",

            "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
        ];

        public static int Revision { get; private set; }

        public static string Folder => Path.Combine(Paths.Root, "Saves");

        public static string PathOf(string slot)
        {
            var name = Sanitize(slot);

            return Path.Combine(Folder, name + ".json");
        }

        public static bool Exists(string slot) => File.Exists(PathOf(slot));

        public static string[] Slots()
        {
            if (!Directory.Exists(Folder))
                return [];

            var files = Directory.GetFiles(Folder, "*.json");

            var slots = new string[files.Length];

            for (var index = 0; index < files.Length; index++)
                slots[index] = Path.GetFileNameWithoutExtension(files[index]);

            return slots;
        }

        public static JsonObject Load(string slot)
        {
            var path = PathOf(slot);

            if (!File.Exists(path))
                return null;

            return (JsonObject)HjsonValue.Parse(File.ReadAllText(path));
        }

        public static void Save(string slot, JsonObject data)
        {
            Directory.CreateDirectory(Folder);

            var path = PathOf(slot);

            var temp = path + ".tmp";

            File.WriteAllText(temp, data.ToString());

            File.Move(temp, path, true);

            Revision++;
        }

        public static void Delete(string slot)
        {
            var path = PathOf(slot);

            if (!File.Exists(path))
                return;

            File.Delete(path);

            Revision++;
        }

        public static void Rename(string from, string to)
        {
            var source = PathOf(from);

            if (!File.Exists(source))
                return;

            Directory.CreateDirectory(Folder);

            File.Move(source, PathOf(to), true);

            Revision++;
        }

        public static string Sanitize(string slot)
        {
            if (string.IsNullOrWhiteSpace(slot))
                throw new ArgumentException("Slot name is empty", nameof(slot));

            var invalid = Path.GetInvalidFileNameChars();

            var name = new string(slot.Where(character => !invalid.Contains(character)).ToArray()).Trim().TrimEnd('.');

            if (name.Length == 0)
                throw new ArgumentException("Slot name is invalid", nameof(slot));

            if (ReservedNames.Contains(name.ToUpperInvariant()))
                name = "_" + name;

            return name;
        }
    }
}