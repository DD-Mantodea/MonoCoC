using MonoCoC.Assets;
using System;
using System.IO;

namespace MonoCoC.UI.Data
{
    public static class UIResourceLocator
    {
        public static string ResolveDocument(string relativePath)
        {
            if (Path.IsPathRooted(relativePath))
                return relativePath;

            var directory = new DirectoryInfo(AppContext.BaseDirectory);

            while (directory is not null)
            {
                if (File.Exists(Path.Combine(directory.FullName, "MonoCoC.csproj")))
                {
                    var source = Path.Combine(directory.FullName, "Assets", "UI", relativePath);

                    if (File.Exists(source))
                        return source;
                }

                directory = directory.Parent;
            }

            return Path.Combine(Paths.AssetPath("UI"), relativePath);
        }
    }
}
