using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.StatusEffects;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class WebSilenceHandlers
    {

        public static void Apply(Creature host)
        {
            EffectApplication.Apply(host, StatusEffectIds.WebSilence);

            host.IsSilenced = true;
        }

        [Register("coc:web_silence_round")]
        public static void Round(CombatContext context, Creature host, StatusEffect status)
        {
            if (status.Values[0] >= 2 || context.Random.Next(20) + 1 + host.Strength / 10 >= 15)
            {
                context.Write("Combat.Effect.WebSilence.BreakFree".Localized());

                host.RemoveStatusEffect(status.Id);

                host.IsSilenced = false;

                return;
            }

            context.Write("Combat.Effect.WebSilence.Struggle".Localized());

            status.Values[0] += 1;
        }

        [Register("coc:web_silence_expire")]
        public static void Expire(CombatContext context, Creature host, StatusEffect status) => host.IsSilenced = false;
    }
}