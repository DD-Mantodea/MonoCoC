using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;

namespace MonoCoC.Contents.Pregnancy
{
    // 原版 PregnancyProgression.hx 的 C# 等价物。
    //
    // 与 Haxe 的差异（有意的，为保持当前 C# 行为零变化）：
    //   Haxe 在 incubation 减到 1 时分娩（PregnancyProgression.hx:77、81-100），
    //   当前 C# 的 Player.TickPregnancy 是减到 <= 0 才清空；
    //   这里沿用 <= 0 的分娩阈值，接入分娩场景后再单独评估是否改到 1。
    public static class PregnancyProgression
    {
        // 对应 Player.pregnancyAdvance()（Character.hx:420-434）+ PregnancyProgression.updatePregnancy（:69-100）。
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

        private static bool Advance(Player player, PregnancyState state, bool anal)
        {
            if (!state.IsActive)
                return false;

            if (state.Incubation > 0)
                state.Incubation--;

            if (state.Incubation < 0)
                state.Incubation = 0;

            // 原版 amilyPregnancyFailsafe（PregnancyProgression.hx:132-146）：在分娩前把纯 Amily 胎改成 Mouse。
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

            var delivery = PregnancyDeliveryRegistry.Resolve(definition?.DeliveryFor(anal));

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
            {
                displayed = delivery.Update(context);
            }

            return displayed;
        }

        // 对应 PregnancyProgression.hx:102-119。
        private static bool CancelHeat(Player player)
        {
            if (!player.HasStatusEffect(VanillaStatusEffects.Heat))
                return false;

            Scene.OutputText("Scenes.PregnancyProgression.CancelHeat.HeatEndsCalmDown".Localized());

            player.ApplyStats(new DynStat(VanillaStats.Libido,
                -player.StatusEffectValue(VanillaStatusEffects.Heat, 1)));

            if (player.Libido < 10)
                player.Libido = 10;

            player.RemoveStatusEffect(VanillaStatusEffects.Heat);

            return true;
        }

        // 对应 PregnancyProgression.hx:132-146。
        private static void AmilyFailsafe(Player player)
        {
            var pregnancy = player.Pregnancy;

            if (!PregnancyLegacyTypes.IsNone(pregnancy.Type) && pregnancy.Type != VanillaPregnancy.Amily)
                return;

            var flags = CoC.Instance.Flags;

            var corrupt = flags.Get(VanillaFlags.AmilyFollower) == 2 || flags.Get(VanillaFlags.AmilyCorruption) > 0;

            var visitingUrta = flags.Get(VanillaFlags.AmilyVisitingUrta) is 1 or 2;

            if (!corrupt && !visitingUrta)
                return;

            pregnancy.Type = VanillaPregnancy.Mouse;
        }

        // 对应 PregnancyProgression.hx:174-198。
        public static void GiveBirth(Player player)
        {
            if (player.Fertility < 15)
                player.Fertility += 1;

            if (player.Fertility < 25)
                player.Fertility += 1;

            if (player.Fertility < 40)
                player.Fertility += 1;

            if (!player.HasStatusEffect(VanillaStatusEffects.Birthed))
            {
                player.CreateStatusEffect(VanillaStatusEffects.Birthed, 1, 0, 0, 0);

                return;
            }

            player.ChangeStatusValue(VanillaStatusEffects.Birthed, 0, 1);

            if (!player.HasPerk(VanillaPerks.BroodMother) &&
                player.StatusEffectValue(VanillaStatusEffects.Birthed, 0) >= 10)
            {
                Scene.OutputText("Scenes.PregnancyProgression.GiveBirth.BroodMotherGained".Localized());

                player.CreatePerk(VanillaPerks.BroodMother, 0, 0, 0, 0);
            }
        }
    }
}
