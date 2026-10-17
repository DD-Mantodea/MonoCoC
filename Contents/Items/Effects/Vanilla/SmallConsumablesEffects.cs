using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 AkbalSaliva.hx ──────────────────────────────────────────────
    public class AkbalSalivaEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            context.Write("Items.AkbalSaliva.Chug".Localized());

            player.HealthChange(player.MaxHealth / 4);

            player.RefillHunger(5);
        }
    }

    // ── 原版 IsabellaMilk.hx ────────────────────────────────────────────
    public class IsabellaMilkEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            player.SlimeFeed();

            context.Write("Items.IsabellaMilk.Swallow".Localized());

            if (player.Fatigue > 0)
                context.Write("Items.IsabellaMilk.LessTired".Localized());

            player.ChangeFatigue(-33);

            player.RefillHunger(20);
        }
    }

    // ── 原版 PeppermintWhite.hx ─────────────────────────────────────────
    public class PeppermintWhiteEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            context.Write("Items.PeppermintWhite.Drink".Localized());

            player.HealthChange(player.MaxHealth);

            player.ChangeFatigue(-100);
        }
    }

    // ── 原版 PurityPeach.hx ─────────────────────────────────────────────
    public class PurityPeachEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            context.Write("Items.PurityPeach.Bite".Localized());

            player.ChangeFatigue(-15);

            player.HealthChange(player.MaxHealth * 0.25f);

            player.RefillHunger(25);
        }
    }

    // ── 原版 SheepMilk.hx ───────────────────────────────────────────────
    public class SheepMilkEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            player.SlimeFeed();

            context.Write("Items.SheepMilk.Gulp".Localized());

            player.ChangeFatigue(-30);

            player.ApplyStats(
                new DynStat(GameStats.Libido, -0.25f),

                new DynStat(GameStats.Lust, -10),

                new DynStat(GameStats.Corruption, -0.5f));

            player.RefillHunger(20);
        }
    }

    // ── 原版 PurePearl.hx ───────────────────────────────────────────────
    public class PurePearlEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            if (player.HasPerk(PerkIds.PurityBlessing))
            {
                context.Write("Items.PurePearl.AlreadyBlessed".Localized());

                // 原版把珍珠塞回背包（物品在使用前已被扣掉，这里退回）
                if (context.Item != null)
                    player.Inventory.Add(context.Item, 1);

                return;
            }

            context.Write("Items.PurePearl.Swallow".Localized());

            context.Write("Items.PurePearl.PerkGained".Localized());

            player.ApplyStats(
                new DynStat(GameStats.Libido, -5),

                new DynStat(GameStats.Lust, -25),

                new DynStat(GameStats.Corruption, -10));

            player.CreatePerk(PerkIds.PurityBlessing, 0, 0, 0, 0);
        }
    }

    // ── 原版 LustStick.hx ───────────────────────────────────────────────
    public class LustStickEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var random = context.Random;

            if (player.HasStatusEffect(StatusEffectIds.LustStickApplied))
            {
                player.ChangeStatusValue(StatusEffectIds.LustStickApplied, 0, random.Next(12) + 12);

                context.Write("Items.LustStick.Reapply".Localized());

                context.Write("Items.LustStick.ReapplyFinish".Localized());
            }
            else
            {
                player.CreateStatusEffect(StatusEffectIds.LustStickApplied, 24, 0, 0, 0);

                context.Write("Items.LustStick.Apply".Localized());

                context.Write(player.HasCock()

                    ? "Items.LustStick.TinglesLittle".Localized()

                    : "Items.LustStick.MakeManPutty".Localized());

                context.Write("Items.LustStick.ApplyFinish".Localized());
            }

            player.ApplyStats(new DynStat(GameStats.Lust, 1));
        }
    }

    // ── 原版 EmberEgg.hx ────────────────────────────────────────────────
    public class EmberEggEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            context.Write("Items.EmberEgg.Crack".Localized());

            if (player.HasPerk(PerkIds.Dragonfire))
            {
                if (player.HasStatusEffect(StatusEffectIds.DragonBreathCooldown))
                    player.RemoveStatusEffect(StatusEffectIds.DragonBreathCooldown);
                else if (!player.HasStatusEffect(StatusEffectIds.DragonBreathBoost))
                    player.CreateStatusEffect(StatusEffectIds.DragonBreathBoost, 0, 0, 0, 0);

                context.Write("Items.EmberEgg.BreathSurge".Localized());
            }

            player.ChangeFatigue(-20);

            player.RefillHunger(50);
        }
    }

    // ── 原版 OculumArachnae.hx ──────────────────────────────────────────
    public class OculumArachnaeEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var mutations = Mutations.Mutations.Instance;

            var eyes = player.Body.Get<EyesPart>();

            mutations.InitTransformation([3]);

            player.SlimeFeed();

            context.Write("Items.OculumArachnae.Eat".Localized());

            if (mutations.ChangeLimit > 0 && context.Random.Next(3) == 0 && eyes.Count < 4)
            {
                context.Write("Items.OculumArachnae.ForeheadEyes".Localized());

                eyes.Count = 4;

                var flags = CoC.Instance.Flags;

                flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + 1);

                return;
            }

            context.Write("Items.OculumArachnae.Refreshed".Localized());

            player.ChangeFatigue(-33);
        }
    }

    // ── 原版 IceShard.hx ────────────────────────────────────────────────
    public class IceShardEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var random = context.Random;

            context.Write("Items.IceShard.Lick".Localized());

            var buffs = new System.Collections.Generic.List<DynStat>();

            if (random.Next(2) == 0 && (player.StrengthPercent < 75 || (100 * player.Toughness / player.GetMaxStats("tou")) < 75))
            {
                context.Write("Items.IceShard.Hardier".Localized());

                if (player.StrengthPercent < 75)
                    buffs.Add(new DynStat(GameStats.Strength, (1 + random.Next(5)) / 5f));

                if ((100 * player.Toughness / player.GetMaxStats("tou")) < 75)
                    buffs.Add(new DynStat(GameStats.Toughness, (1 + random.Next(5)) / 5f));
            }

            if (random.Next(2) == 0 && (100 * player.Speed / player.GetMaxStats("spe")) > 25)
            {
                context.Write("Items.IceShard.Sluggish".Localized());

                buffs.Add(new DynStat(GameStats.Speed, -((1 + random.Next(5)) / 5f)));
            }

            if (random.Next(2) == 0)
            {
                context.Write("Items.IceShard.Numb".Localized());

                buffs.Add(new DynStat(GameStats.Libido, -((1 + random.Next(2)) / 2f)));

                buffs.Add(new DynStat(GameStats.Sensitivity, -((1 + random.Next(2)) / 2f)));
            }

            if (buffs.Count > 0)
                player.ApplyStats([.. buffs]);

            player.RefillHunger(5);
        }
    }
}