using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using System.IO;

namespace MonoCoC.UI.Components.Combat
{
    public class CombatPlayerStats : PlayerStats, IFromHjson<CombatPlayerStats>
    {
        public CombatPlayerStats()
            : base("Views\\CombatPlayerStats.hjson")
        {
        }

        public static new string TypeId => "coc:combat_player_stats";

        public static new Component FromHjson(Hjson.JsonObject node, UIBuildContext services) =>
            new CombatPlayerStats();

        [System.Runtime.CompilerServices.ModuleInitializer]
        internal static new void Register() => ComponentFactory.Register<CombatPlayerStats>();
    }
}
