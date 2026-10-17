using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Items.Behaviors.Vanilla
{
    // 原版 BeautifulSword.hx:15-23：攻击力随 BEAUTIFUL_SWORD_LEVEL 和腐化林地进度成长
    [Register<BeautifulSwordBehavior>("coc:beautiful_sword")]
    public class BeautifulSwordBehavior : ItemBehavior, IWeaponAttackBehavior
    {
        public float GetAttack(Weapon weapon, Player player)
        {
            var attack = 7f + CoC.Instance.Flags.Get(GameFlagIds.BeautifulSwordLevel) * 2f;

            if (CoC.Instance.Flags.Get(GameFlagIds.CorruptedGladesDestroyed) >= 50)
                attack += 2;

            if (CoC.Instance.Flags.Get(GameFlagIds.CorruptedGladesDestroyed) >= 100)
                attack += 2;

            return attack;
        }

        public void OnAttack(Player player)
        {
        }
    }
}