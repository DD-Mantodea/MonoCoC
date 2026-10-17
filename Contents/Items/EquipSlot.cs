using System;

namespace MonoCoC.Contents.Items
{
    [Flags]
    public enum EquipSlot
    {
        None = 0,

        MainHand = 1 << 0,

        OffHand = 1 << 1,

        Body = 1 << 2,

        LowerBody = 1 << 3,

        Undergarment = 1 << 4,

        Accessory = 1 << 5,

        Inventory = 1 << 6
    }
}