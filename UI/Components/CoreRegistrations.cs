using Hjson;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using System.Runtime.CompilerServices;
using static MonoCoC.UI.Data.UIParseUtils;

namespace MonoCoC.UI.Components
{
    public static class CoreComponentRegistrations
    {
        [ModuleInitializer]
        internal static void Register()
        {
            ComponentFactory.Register("coc:view", (node, services) => new View());

            ComponentFactory.Register("coc:panel", (node, services) =>
                new Container
                {
                    CanHit = node.Bool("canHit", false),
                    BackgroundColor = ColorOf(node.Str("backgroundColor", "#00000000"))
                });
        }
    }
}
