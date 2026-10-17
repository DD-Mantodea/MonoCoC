using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 SuccubisDream.hx ───────────────────────────────────────────
    public class SuccubisDreamEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var mutations = Mutations.Mutations.Instance;

            var random = context.Random;

            player.SlimeFeed();

            var crit = 1 + (random.Next(2) + 1);

            mutations.InitTransformation([2, 2]);

            context.Write("Items.SuccubisDream.Drink".Localized());

            if (player.Corruption < 33)
                context.Write("Items.SuccubisDream.Gross".Localized());

            if (player.Corruption >= 66)
                context.Write("Items.SuccubisDream.LickLips".Localized());

            if (player.Corruption < 50 || random.Next(2) != 0)
            {
                context.Write("Items.SuccubisDream.FeelDirty".Localized());

                var temp = 1;

                if (player.Corruption < 50)
                    temp += 1;

                if (player.Corruption < 40)
                    temp += 1;

                if (player.Corruption < 30)
                    temp += 1;

                if (player.Corruption >= 90)
                    temp = 0;

                player.ApplyStats(new DynStat(GameStats.Corruption, temp + 2));

                mutations.Changes += 1;
            }

            if (player.Balls < 4)
            {
                if (player.Balls > 0)
                {
                    player.Balls = 4;

                    context.Write("Items.SuccubisDream.FourBalls".Localized());
                }

                if (player.Balls == 0)
                {
                    player.Balls = 2;

                    player.BallSize = 1;

                    context.Write("Items.SuccubisDream.GrowBalls".Localized());
                }

                mutations.Changes += 1;
            }

            var cocks = player.Body.GetList<CockPart>();

            if (random.NextDouble() * 1.5 < 1 && mutations.Changes < mutations.ChangeLimit
                && player.Balls > 0 && cocks.Count > 0)
            {
                player.BallSize += 1;

                if (player.BallSize > 10)
                    player.BallSize -= 0.5f;

                context.Write(player.BallSize <= 2

                    ? "Items.SuccubisDream.BallsGrowSmall".Localized(player.SimpleBallsDescript())

                    : "Items.SuccubisDream.BallsGrowBig".Localized(player.SimpleBallsDescript()));

                player.ApplyStats(
                    new DynStat(GameStats.Libido, 1),

                    new DynStat(GameStats.Lust, 3));
            }

            if ((mutations.Changes < mutations.ChangeLimit && random.Next(2) == 0 && cocks.Count > 0) && (player.CumMultiplier < 6 && random.Next(2) == 0 && mutations.Changes < mutations.ChangeLimit))
                {
                    var temp = player.HasPerk(PerkIds.MessyOrgasms) ? 6 : 3;

                    if (temp < player.CumMultiplier + 0.4f * crit)
                        mutations.Changes -= 1;
                    else
                    {
                        player.CumMultiplier += 0.4f * crit;

                        if (player.Balls == 0)
                            context.Write("Items.SuccubisDream.ChurningBody".Localized());
                        else
                            context.Write("Items.SuccubisDream.ChurningBalls".Localized());

                        if (crit > 1)
                            context.Write("Items.SuccubisDream.PreDribble".Localized());

                        player.ApplyStats(new DynStat(GameStats.Libido, 1));
                    }

                    mutations.Changes += 1;
                }

            if (mutations.Changes == 0)
            {
                context.Write("Items.SuccubisDream.FailSafe".Localized());

                player.HoursSinceCum += 100;

                mutations.Changes += 1;
            }

            if (player.Balls > 0 && random.Next(3) == 0)
                context.Write(player.ModFemGoal(12, 5));

            player.RefillHunger(15);
        }
    }
}