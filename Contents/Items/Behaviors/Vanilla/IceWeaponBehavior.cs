using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Items.Behaviors.Vanilla
{
    /// <summary>
    /// 原版 items/weapons/IceWeapon.hx:11-146 —— 冰制武器。
    /// 攻击值随融化计时器下降；每次攻击前先融化结算，计时器到 5 会警告、到 0 直接碎掉；
    /// 裂谷（rift）里不融化，火山岩区（crag）一次攻击就直接烧化。
    /// </summary>
    [Register<IceWeaponBehavior>("coc:ice_axe")]
    [Register<IceWeaponBehavior>("coc:ice_mace")]
    [Register<IceWeaponBehavior>("coc:ice_spear")]
    [Register<IceWeaponBehavior>("coc:ice_sword")]
    [Register<IceWeaponBehavior>("coc:ice_dagger")]
    [Register<IceWeaponBehavior>("coc:ice_scythe")]
    [Register<IceWeaponBehavior>("coc:ice_staff")]
    public class IceWeaponBehavior : ItemBehavior, IWeaponAttackBehavior
    {
        private static readonly Identifier[] IceWeaponIds =

        [
            new("coc", "ice_axe"),

            new("coc", "ice_mace"),

            new("coc", "ice_spear"),

            new("coc", "ice_sword"),

            new("coc", "ice_dagger"),

            new("coc", "ice_scythe"),

            new("coc", "ice_staff"),
        ];

        public IceWeaponBehavior()
        {
            if (CoC.Instance.Flags.Get(GameFlagIds.IceWeaponTimer) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.IceWeaponTimer, 10);
        }

        public float GetAttack(Weapon weapon, Player player) =>

            weapon.Attack + CoC.Instance.Flags.Get(GameFlagIds.IceWeaponTimer);

        // 原版计时器由 preAttack 里的 melt(true) 结算，OnAttack 不再重复扣减
        public void OnAttack(Player player)
        {
        }

        // 原版 :46 melt(attacking)
        public static bool Melt(Player player, bool attacking, Action<string> write)
        {
            var flags = CoC.Instance.Flags;

            switch (player.Location)
            {
                case "rift":
                    // 裂谷里冰武器不融化
                    return true;

                case "crag":
                    // 火山岩区一次直接烧化
                    flags.Set(GameFlagIds.IceWeaponTimer, -50);

                    break;

                default:
                    flags.Set(GameFlagIds.IceWeaponTimer, flags.Get(GameFlagIds.IceWeaponTimer) - 1);

                    break;
            }

            var timer = flags.Get(GameFlagIds.IceWeaponTimer);

            if (timer <= 0)
            {
                if (attacking)
                    write(timer == -50

                        ? "Scenes.IceWeapon.VolcanicAttack".Localized()

                        : "Scenes.IceWeapon.AttackMelt".Localized());
                else
                    write("Scenes.IceWeapon.MeltAway".Localized());

                RemoveAllIceWeapons();

                return false;
            }

            if (timer == 5)
            {
                if (attacking)
                {
                    write("Scenes.IceWeapon.AttackWeak".Localized());

                    return true;
                }

                write("Scenes.IceWeapon.Weak".Localized());

                return false;
            }

            return true;
        }

        // 原版 :80 removeAllIceWeapons()
        public static void RemoveAllIceWeapons()
        {
            var player = CoC.Instance.Player;

            foreach (var id in IceWeaponIds)
            {
                if (player.Weapon.HasTag("melting"))
                    player.SetUnarmed();

                while (player.HasItem(id))

                    player.DestroyItems(id, 1);
            }

            CoC.Instance.Flags.Set(GameFlagIds.IceWeaponTimer, 0);
        }

        // 原版 :39 playerHasIceWeapon()
        public static string PlayerHasIceWeapon()
        {
            var player = CoC.Instance.Player;

            if (player.Weapon.HasTag("melting"))
                return player.Weapon.Name;

            foreach (var id in IceWeaponIds)
                if (player.HasItem(id))
                    return ItemRegistry.Create(id).Name;

            return "";
        }
    }
}