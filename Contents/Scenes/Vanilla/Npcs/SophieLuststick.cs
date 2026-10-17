using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{

    public static class SophieLuststick
    {
        public static void Apply(float hours = 4)
        {
            var player = CoC.Instance.Player;

            if (player.HasPerk(PerkIds.LuststickAdapted))
                return;

            CoC.Instance.Flags.Set(GameFlagIds.LuststickResistanceProgress,

                CoC.Instance.Flags.Get(GameFlagIds.LuststickResistanceProgress) + (int)Math.Floor(hours / 2));

            if (!player.HasCock())
                return;

            hours = Math.Min(hours, 20);

            if (player.HasStatusEffect(StatusEffectIds.Luststick))
            {
                var current = player.StatusEffectValue(StatusEffectIds.Luststick, 0);

                if (current < 20)
                {
                    if (current + hours > 20)
                        hours = 20 - current;

                    player.ChangeStatusValue(StatusEffectIds.Luststick, 0, hours);
                }
            }
            else
            {
                CoC.Instance.Flags.Set(GameFlagIds.LuststickLibidoInitial, player.Libido);

                player.CreateStatusEffect(StatusEffectIds.Luststick, hours, 0, 0, 0);
            }
        }
    }
}