using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Pregnancy
{

    public static class PregnancyProgression
    {

        public static bool UpdatePlayer(Player player)
        {
            if (player == null)
                return false;

            var vaginal = player.Pregnancy;

            var anal = player.ButtPregnancy;

            if (!vaginal.IsActive && !anal.IsActive)
                return false;

            var displayed = CancelHeat(player);

            displayed |= Advance(player, vaginal, false);

            displayed |= Advance(player, anal, true);

            return displayed;
        }

        public static bool Advance(Player player, PregnancyState state, bool anal)
        {
            if (!state.IsActive)
                return false;

            if (state.Incubation > 0)
                state.Incubation--;

            if (state.Incubation < 0)
                state.Incubation = 0;

            if (!anal && state.Incubation <= 1)
                AmilyFailsafe(player);

            var definition = PregnancyTypeRegistry.Get(state.Type);

            var context = new PregnancyContext
            {
                Player = player,

                State = state,

                Definition = definition,

                Anal = anal,

                Write = Scene.OutputText
            };

            var delivery = PregnancyDeliveryRegistry.Resolve(definition?.DeliveryFor(anal ? "anal" : "vaginal"));

            var displayed = false;

            if (state.Incubation <= 0)
            {
                delivery.Deliver(context);

                if (!anal)
                    GiveBirth(player);

                if (anal)
                    player.ClearButtPregnancy();
                else
                    player.ClearPregnancy();

                displayed = true;
            }
            else
                displayed = delivery.Update(context);

            return displayed;
        }

        public static bool CancelHeat(Player player)
        {
            if (!player.HasStatusEffect(StatusEffectIds.Heat))
                return false;

            Scene.OutputText("Scenes.PregnancyProgression.CancelHeat.HeatEndsCalmDown".Localized());

            player.ApplyStats(new DynStat(GameStats.Libido,

                -player.StatusEffectValue(StatusEffectIds.Heat, 1)));

            if (player.Libido < 10)
                player.Libido = 10;

            player.RemoveStatusEffect(StatusEffectIds.Heat);

            return true;
        }

        public static void AmilyFailsafe(Player player)
        {
            var pregnancy = player.Pregnancy;

            if (!PregnancyLegacyTypes.IsNone(pregnancy.Type) && pregnancy.Type != PregnancyIds.Amily)
                return;

            var flags = CoC.Instance.Flags;

            var corrupt = flags.Get(GameFlagIds.AmilyFollower) == 2 || flags.Get(GameFlagIds.AmilyCorruption) > 0;

            var visitingUrta = flags.Get(GameFlagIds.AmilyVisitingUrta) is 1 or 2;

            if (!corrupt && !visitingUrta)
                return;

            pregnancy.Type = PregnancyIds.Mouse;
        }

        public static void GiveBirth(Player player)
        {
            if (player.Fertility < 15)
                player.Fertility += 1;

            if (player.Fertility < 25)
                player.Fertility += 1;

            if (player.Fertility < 40)
                player.Fertility += 1;

            if (!player.HasStatusEffect(StatusEffectIds.Birthed))
            {
                player.CreateStatusEffect(StatusEffectIds.Birthed, 1, 0, 0, 0);

                return;
            }

            player.ChangeStatusValue(StatusEffectIds.Birthed, 0, 1);

            if (!player.HasPerk(PerkIds.BroodMother) &&
                player.StatusEffectValue(StatusEffectIds.Birthed, 0) >= 10)
            {
                Scene.OutputText("Scenes.PregnancyProgression.GiveBirth.BroodMotherGained".Localized());

                player.CreatePerk(PerkIds.BroodMother, 0, 0, 0, 0);
            }
        }
    }
}