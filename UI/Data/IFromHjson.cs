using Hjson;
using MonoCoC.UI.Framework;

namespace MonoCoC.UI.Data
{
    public interface IFromHjson<TSelf> where TSelf : Component, IFromHjson<TSelf>
    {
        static abstract string TypeId { get; }

        static abstract Component FromHjson(JsonObject node, UIBuildContext context);
    }
}
