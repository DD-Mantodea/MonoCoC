using MonoCoC.Contents.Creatures;
using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Items.Behaviors.Vanilla
{
    [Register<TentacleBarkBehavior>("coc:tentacle_bark")]
    public class TentacleBarkBehavior : ItemBehavior, IArmorDefenseBehavior
    {
        public float GetDefense(Armor armor, Player player) =>

            15 + (float)Math.Floor(player.Corruption / 5);
    }
}