using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace MonoCoC.Assets.AssetManagers
{
    public class TextureManager : AssetManager<Texture2D>
    {
        public override string Folder => "Textures";

        public override string Pattern => "*.png";

        public override void FromFile(string path, IDictionary<string, Texture2D> entries)
        {
            // key 统一用 '/'，避免 Windows 下子目录取到反斜杠（DungeonLarge\Room vs DungeonLarge/Room）
            var key = Path.GetRelativePath(Paths.AssetPath(Folder), path.Replace(".png", "")).Replace('\\', '/');

            entries[key] = Texture2D.FromFile(CoC.Instance.GraphicsDevice, path);
        }
    }
}