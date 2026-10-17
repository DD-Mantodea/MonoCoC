using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Encounters
{
    public static class ItemEncounters
    {
        public static IEncounter Find(
            string name,

            string textKey,

            Func<Item> item,

            float chance,

            Func<Player, bool>? when = null) =>

            IEncounter.Create(
                name,

                _ => chance,

                when ?? (_ => true),

                context =>
                {
                    context.Write(textKey.Localized());

                    InventoryScene.TakeItem(item(), context.ReturnToCamp);
                });
    }
}