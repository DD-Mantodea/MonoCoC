using MonoCoC.Contents.Creatures;

namespace MonoCoC.Contents.Items.Behaviors
{
    public interface IWeaponAttackBehavior : IItemBehavior
    {
        float GetAttack(Weapon weapon, Player player);

        void OnAttack(Player player);
    }
}