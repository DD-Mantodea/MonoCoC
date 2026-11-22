using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;

namespace MonoCoC.UI.Components.Combat
{
    public class SimpleStats : PlayerStats, IFromHjson<SimpleStats>
    {
        public SimpleStats()
            : base("Views\\SimpleStats.hjson")
        {
        }

        public static new string TypeId => "coc:simple_stats";

        public static new Component FromHjson(Hjson.JsonObject node, UIBuildContext services) =>
            new SimpleStats();

        [System.Runtime.CompilerServices.ModuleInitializer]
        internal static new void Register() => ComponentFactory.Register<SimpleStats>();
    }
}
