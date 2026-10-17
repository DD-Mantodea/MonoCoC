using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.StatusEffects;

namespace MonoCoC.Contents.Combat.Handlers
{
    public static class ImaginaryMiffixHandlers
    {
        [Register("coc:effect_imaginary_miffix_round")]
        public static void Round(CombatContext context, Creature host, StatusEffect status)
        {
            host.IsSilenced = true;

            host.IsCrippled = true;

            host.IsUnfocused = true;

            host.IsClumsy = true;

            host.IsPrude = true;

            host.IsCornered = true;

            host.IsImmobilized = true;
        }

        [Register("coc:effect_imaginary_miffix_expire")]
        public static void Expire(CombatContext context, Creature host, StatusEffect status)
        {
            host.IsSilenced = false;

            host.IsCrippled = false;

            host.IsUnfocused = false;

            host.IsClumsy = false;

            host.IsPrude = false;

            host.IsCornered = false;

            host.IsImmobilized = false;
        }
    }
}