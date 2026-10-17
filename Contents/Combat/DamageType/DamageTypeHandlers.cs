using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class DamageTypeHandlers
    {
        [Register("coc:damage_fire")]
        public static void Fire(CombatContext context, float damage)
        {
            var player = context.Player;

            var vineArmor = player.Armor != null && player.Armor.Id.Path == "vine_armor";

            if (!vineArmor && player.DryadScore() < 4)
                return;

            if (player.MaxHealth > 0)
                player.ChangeFatigue(damage / player.MaxHealth * 100f);
        }
    }
}