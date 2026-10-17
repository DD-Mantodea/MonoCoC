using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{

    public class OviElixirEffect(bool ovimax) : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            if (player.Vaginas.Count == 0)
            {
                context.Write("Items.OviElixir.NeedVagina".Localized());

                return;
            }

            player.SlimeFeed();

            context.Write("Items.OviElixir.Drink".Localized());

            if (player.Pregnancy.Type == PregnancyIds.GooStuffed)
            {
                context.Write("Items.OviElixir.GooBlocked".Localized());

                return;
            }

            if (player.Pregnancy.Type == PregnancyIds.WormStuffed)
            {
                context.Write("Items.OviElixir.WormBlocked".Localized());

                return;
            }

            var incubation = player.Pregnancy.Incubation;

            if (incubation == 0)
            {
                context.Write("Items.OviElixir.EggsStarted".Localized());

                CreatePregnancy(player, Random.Shared.Next(6), RandBigEgg(), RandEggCount());

                return;
            }

            var changed = false;

            if (player.Pregnancy.Type == PregnancyIds.OvielixirEggs)
            {
                if (ovimax && !player.HasPerk(PerkIds.Oviposition) && !CanSpeedUp(player))
                {
                    context.Write("Items.OviElixir.OvimaxRumble".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.OvimaxOverdose,

                        CoC.Instance.Flags.Get(GameFlagIds.OvimaxOverdose) + 1);
                }

                if (player.HasStatusEffect(StatusEffectIds.Eggs))
                {

                    if (player.StatusEffectValue(StatusEffectIds.Eggs, 1) == 0 && RandDoIncEggSize())
                    {
                        player.ChangeStatusValue(StatusEffectIds.Eggs, 1, 1);

                        context.Write("Items.OviElixir.BellyHeavier".Localized());

                        changed = true;
                    }

                    var bonus = BonusEggQty();

                    if (bonus > 0)
                    {
                        context.Write("Items.OviElixir.BellyBigger".Localized());

                        player.ChangeStatusValue(StatusEffectIds.Eggs, 2, bonus);

                        changed = true;
                    }
                }
            }

            if (!changed && CanSpeedUp(player))
            {
                context.Write("Items.OviElixir.SpeedUp".Localized());

                var next = DoSpeedUp(incubation);

                if (next < 2)
                    next = 2;

                player.Pregnancy.Incubation = next;
            }
        }

        public int DoSpeedUp(int incubation) =>

            incubation - (ovimax

                ? (int)(incubation * 0.5f + 15)

                : (int)(incubation * 0.3f + 10));

        public bool CanSpeedUp(Creatures.Player player) =>

            ovimax

                ? player.Pregnancy.Incubation > 20

                : player.Pregnancy.Incubation > 20 && player.Pregnancy.Type != PregnancyIds.Bunny;

        public static int BonusEggQty() => Random.Shared.Next(2) == 0 ? Random.Shared.Next(4) + 1 : 0;

        public static bool RandDoIncEggSize() => Random.Shared.Next(3) == 0;

        public int RandEggCount() => ovimax ? Random.Shared.Next(4) + 6 : Random.Shared.Next(3) + 5;

        public bool RandBigEgg() => ovimax && Random.Shared.Next(2) == 0;

        public static void CreatePregnancy(Creatures.Player player, int type, bool big, int quantity)
        {
            player.KnockUp(PregnancyIds.OvielixirEggs, PregnancyIds.OvielixirEggsIncubation, 1, 1);

            player.CreateStatusEffect(StatusEffectIds.Eggs, type, big ? 1 : 0, quantity, 0);
        }
    }
}