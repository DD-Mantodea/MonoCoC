using System;
using System.IO;

namespace MonoCoC.Assets
{
    public static class Paths
    {
        public static string Root = Path.GetDirectoryName(Environment.ProcessPath);

        public static string Assets = Path.Combine(Root, "Assets");

        public static string AssetPath(string folderName) => Path.Combine(Assets, folderName);

        public static string[] GetAssets(string folderName, string pattern) => Directory.GetFiles(Path.Combine(Assets, folderName), pattern, SearchOption.AllDirectories);
    }
}