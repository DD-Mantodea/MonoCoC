using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Combat
{

    public static class PlayerCombatConditions
    {

        [Register("coc:can_kick")]
        public static bool CanKick(CombatContext context, CombatState state)
        {
            var player = context.Player;

            if (player.IsTaur())
                return true;

            var lowerBody = player.Body.Get<LowerBodyPart>().Id.Path;

            return lowerBody is "hoofed" or "pony" or "cloven_hoofed" or "bunny" or "kangaroo";
        }

        [Register("coc:can_gore")]
        public static bool CanGore(CombatContext context, CombatState state)
        {
            var horns = context.Player.Body.Get<HornsPart>();

            return horns.Id.Path == "cow_minotaur" && horns.Value >= 6;
        }

        [Register("coc:can_horn_stun")]
        public static bool CanHornStun(CombatContext context, CombatState state)
        {
            var horns = context.Player.Body.Get<HornsPart>();

            return horns.Id.Path == "ram" && horns.Value >= 2;
        }

        [Register("coc:player_has_vagina")]
        public static bool PlayerHasVagina(CombatContext context, CombatState state) =>

            context.Player.HasVagina();

        [Register("coc:corrupted_witch_heals_ready")]
        public static bool CorruptedWitchHealsReady(CombatContext context, CombatState state) =>

            context.Enemy.HealthRatio() < 0.6f &&

            context.Enemy.Fatigue <= 90 &&

            context.Player.HasCock();
    }
}