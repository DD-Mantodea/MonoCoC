using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Items
{
    [Register<Useable>("coc:useable")]
    public class Useable : Item
    {
        public Action<Item>? UseAction;

        public override bool CanUse() => UseAction != null;

        public override bool UseItem()
        {
            if (UseAction == null)
                return false;

            UseAction(this);

            return true;
        }
    }
}