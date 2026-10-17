using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Pregnancy.Vanilla
{

    [Register<IzmaPregnancyDelivery>("coc:delivery_izma")]
    public class IzmaPregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context)
        {
            var player = context.Player;

            var follower = CoC.Instance.Flags.Get(GameFlagIds.IzmaFollowerStatus) == 1;

            var displayed = false;

            switch (context.Incubation)
            {
                case 275:
                    if (follower)
                        context.Write("Pregnancy.Izma.FollowerSick".Localized(player.ShortName));
                    else
                        context.Write("Pregnancy.Izma.BloatedCravings".Localized());

                    displayed = true;

                    break;

                case 250:
                    context.Write("Pregnancy.Izma.BellyDistended".Localized());

                    displayed = true;

                    break;

                case 216:
                    context.Write(follower

                        ? "Pregnancy.Izma.FollowerUndeniablySwollen".Localized()

                        : "Pregnancy.Izma.UndeniablyPregnant".Localized());

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 1f),

                        new DynStat(GameStats.Sensitivity, 1f),

                        new DynStat(GameStats.Lust, 2f));

                    displayed = true;

                    break;

                case 180:
                    context.Write(follower

                        ? "Pregnancy.Izma.FollowerHeadOverHeels".Localized()

                        : "Pregnancy.Izma.DenyingPregnancy".Localized());

                    displayed = true;

                    break;

                case 120:
                    context.Write(follower

                        ? "Pregnancy.Izma.FollowerGravid".Localized()

                        : "Pregnancy.Izma.Gravid".Localized());

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 1f),

                        new DynStat(GameStats.Sensitivity, 1f),

                        new DynStat(GameStats.Lust, 4f));

                    displayed = true;

                    break;

                case 72:
                    context.Write(follower

                        ? "Pregnancy.Izma.FollowerDreams".Localized()

                        : "Pregnancy.Izma.Dreams".Localized());

                    displayed = true;

                    break;
            }

            if (context.Incubation is 32 or 64 or 85 or 150)
            {
                var biggestTit = player.BiggestBreastSize();

                var rows = player.MostBreastsPerRow();

                var lactation = player.BiggestLactation();

                if (biggestTit >= 3 && rows > 1 && lactation is >= 1 and < 2)
                {
                    context.Write("Pregnancy.Common.SwollenWithMilk".Localized());

                    player.BoostLactation(0.5f);

                    displayed = true;
                }

                if (biggestTit >= 3 && rows > 1 && lactation > 0 && lactation < 1)
                {
                    context.Write("Pregnancy.Common.DropsOfMilk".Localized());

                    player.BoostLactation(0.5f);

                    displayed = true;
                }

                if (biggestTit >= 3 && rows > 1 && lactation == 0)
                {
                    context.Write("Pregnancy.Common.FullAndLactating".Localized());

                    player.BoostLactation(1f);

                    displayed = true;
                }

                if (biggestTit == 2 && rows > 1)
                {
                    context.Write("Pregnancy.Common.GrewToCCups".Localized());

                    player.GrowTits(1, 1, false, 3);

                    displayed = true;
                }

                if (biggestTit == 1 && rows > 1)
                {
                    context.Write("Pregnancy.Common.GrewToBCups".Localized());

                    player.GrowTits(1, 1, false, 3);

                    displayed = true;
                }
            }

            context.Write("[pg]");

            return displayed;
        }

        public void Deliver(PregnancyContext context)
        {

            Scenes.SceneInstance<Scenes.Vanilla.Npcs.IzmaScene>.Get().PcPopsOutASharkTot();
        }
    }
}