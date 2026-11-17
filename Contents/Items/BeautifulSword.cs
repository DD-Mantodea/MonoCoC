using MonoCoC.Attributes;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Items
{
    [ItemCategory("coc:weapon", "Icons/inventory_weapon.svg", "Items.Type.Weapon")]
    [Register<BeautifulSword>("coc:beautiful_sword")]
    public sealed class BeautifulSword : Weapon
    {
        public override string EffectDescription => "Items.BeautifulSword.Effect".Localized();
    }
}
