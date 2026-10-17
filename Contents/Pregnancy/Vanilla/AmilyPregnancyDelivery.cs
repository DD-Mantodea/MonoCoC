using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Pregnancy.Vanilla
{

    [Register<AmilyPregnancyDelivery>("coc:delivery_amily")]
    public class AmilyPregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context)
        {
            var player = context.Player;

            var flags = CoC.Instance.Flags;

            var displayed = false;

            switch (context.Incubation)
            {
                case 336:
                    context.Write("Pregnancy.Mouse.BellySlightlyLarger".Localized());

                    displayed = true;

                    break;

                case 280:
                    context.Write("Pregnancy.Mouse.BellyDistended".Localized());

                    displayed = true;

                    break;

                case 216:
                {
                    context.Write("Pregnancy.Amily.Unmistakable".Localized());

                    if (flags.Get(GameFlagIds.AmilyFollower) == 1)
                        context.Write("Pregnancy.Amily.ReassuringSmile".Localized());

                    player.ApplyStats(
                        new DynStat(GameStats.Speed, -1f),

                        new DynStat(GameStats.Libido, 1f),

                        new DynStat(GameStats.Sensitivity, 1f),

                        new DynStat(GameStats.Lust, 2f));

                    displayed = true;

                    break;
                }

                case 180:
                    context.Write("Pregnancy.Imp.Kick".Localized());

                    displayed = true;

                    break;

                case 120:
                    context.Write("Pregnancy.Amily.HugelyPregnant".Localized());

                    displayed = true;

                    break;

                case 72:
                    context.Write("Pregnancy.Amily.MoreThanOne".Localized());

                    player.ApplyStats(
                        new DynStat(GameStats.Speed, -3f),

                        new DynStat(GameStats.Libido, 1f),

                        new DynStat(GameStats.Sensitivity, 1f),

                        new DynStat(GameStats.Lust, 4f));

                    displayed = true;

                    break;

                case 48:
                    context.Write("Pregnancy.Amily.KickingFrequently".Localized());

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
                }

                if (biggestTit >= 3 && rows > 1 && lactation > 0 && lactation < 1)
                {
                    context.Write("Pregnancy.Common.DropsOfMilk".Localized());

                    player.BoostLactation(0.5f);
                }

                if (biggestTit >= 3 && rows > 1 && lactation == 0)
                {
                    context.Write("Pregnancy.Common.FullAndLactating".Localized());

                    player.BoostLactation(1f);
                }

                if (biggestTit == 2 && rows > 1)
                {
                    context.Write("Pregnancy.Common.GrewToCCups".Localized());

                    player.GrowTits(1, 1, false, 3);
                }

                if (biggestTit == 1 && rows > 1)
                {
                    context.Write("Pregnancy.Common.GrewToBCups".Localized());

                    player.GrowTits(1, 1, false, 3);
                }
            }

            return displayed;
        }

        public void Deliver(PregnancyContext context)
        {
            var player = context.Player;

            var flags = CoC.Instance.Flags;

            player.BoostLactation(0.01f);

            PregnancyUtils.CreateVaginaIfMissing(context);

            flags.Set(GameFlagIds.PcTimesBirthedAmilykids, flags.Get(GameFlagIds.PcTimesBirthedAmilykids) + 1);

            if (flags.Get(GameFlagIds.AmilyFollower) == 1)
            {
                WriteCampVersion(context);

                flags.Set(GameFlagIds.PcTimesBirthedAmilykids, flags.Get(GameFlagIds.PcTimesBirthedAmilykids) + 1);

                return;
            }

            var totalBirths = flags.Get(GameFlagIds.PcTimesBirthedAmilykids) + flags.Get(GameFlagIds.AmilyBirthTotal);

            if (totalBirths >= 5)
            {
                context.Write("Pregnancy.Amily.QuestPart1".Localized());

                context.Write("Pregnancy.Amily.QuestPart2".Localized());

                context.Write("Pregnancy.Amily.QuestPart3".Localized());

                context.Write("Pregnancy.Amily.QuestPart4".Localized());

                context.Write("Pregnancy.Amily.QuestPart5".Localized());

                context.Write("Pregnancy.Amily.QuestPart6".Localized());

                flags.Set(GameFlagIds.PcPendingPreggers, 1);

                Scene.AddNextButton("Common.Button.Accept".Localized(), () => QuestEnding(context, accept: true));

                Scene.AddNextButton("Common.Button.StayFriends".Localized(), () => QuestEnding(context, accept: false, friend: true));

                Scene.AddNextButton("Common.Button.ShootDown".Localized(), () => QuestEnding(context, accept: false, friend: false));

                return;
            }

            context.Write("Pregnancy.Amily.NormalPart1".Localized());

            context.Write("Pregnancy.Amily.NormalPart2".Localized());

            context.Write("Pregnancy.Amily.NormalPart3".Localized());

            context.Write("Pregnancy.Amily.NormalPart4".Localized());

            context.Write("Pregnancy.Amily.NormalPart5".Localized());

            context.Write("Pregnancy.Amily.NormalPart6".Localized());

            player.CuntChange(60, true, true, false);

            if (player.Vaginas.Count > 0 && player.Vaginas[0].Wetness <= 0)
                player.Vaginas[0].Wetness += 1;

            player.Orgasm("Vaginal");

            player.ApplyStats(
                new DynStat(GameStats.Strength, -1f),

                new DynStat(GameStats.Toughness, -2f),

                new DynStat(GameStats.Speed, 3f),

                new DynStat(GameStats.Libido, 1f),

                new DynStat(GameStats.Sensitivity, 0.5f));
        }

        public static void WriteCampVersion(PregnancyContext context)
        {
            var flags = CoC.Instance.Flags;

            context.Write("Pregnancy.Amily.CampPart1".Localized());

            context.Write("Pregnancy.Amily.CampPart2".Localized());

            context.Write("Pregnancy.Amily.CampPart3".Localized(
                flags.Get(GameFlagIds.PcTimesBirthedAmilykids) > 0

                    ? "Pregnancy.Amily.CampRepeat".Localized()

                    : "Pregnancy.Amily.CampFirst".Localized()));

            context.Write("Pregnancy.Amily.CampPart4".Localized());

            context.Write("Pregnancy.Amily.CampPart5".Localized());

            context.Write("Pregnancy.Amily.CampPart6".Localized());

            context.Write("Pregnancy.Amily.CampPart7".Localized());

            context.Write("Pregnancy.Amily.CampPart8".Localized());

            context.Write("Pregnancy.Amily.CampPart9".Localized());
        }

        public static void QuestEnding(PregnancyContext context, bool accept, bool friend = false)
        {
            var player = context.Player;

            var flags = CoC.Instance.Flags;

            Scene.ClearOutput();

            context.Write("Pregnancy.Amily.EndIntro".Localized());

            context.Write("Pregnancy.Amily.EndConfession".Localized(
                player.Gender == Gender.Female

                    ? "Pregnancy.Amily.AnotherWoman".Localized()

                    : "Pregnancy.Amily.AHermaphrodite".Localized()));

            context.Write("Pregnancy.Amily.EndAsk".Localized());

            if (accept)
            {
                context.Write("Pregnancy.Amily.EndAccept1".Localized());

                context.Write("Pregnancy.Amily.EndAccept2".Localized());

                flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 1);

                flags.Set(GameFlagIds.AmilyFollower, 1);

                flags.Set(GameFlagIds.AmilyCupSize, 1);

                flags.Set(GameFlagIds.AmilyNippleLength, 0.3f);

                flags.Set(GameFlagIds.AmilyHipRating, 6);

                flags.Set(GameFlagIds.AmilyAssSize, 6);
            }
            else if (friend)
            {
                context.Write("Pregnancy.Amily.EndFriends1".Localized());

                context.Write("Pregnancy.Amily.EndFriends2".Localized());
            }
            else
            {
                context.Write("Pregnancy.Amily.EndShootDown1".Localized());

                context.Write("Pregnancy.Amily.EndShootDown2".Localized());

                flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 1);
            }

            flags.Set(GameFlagIds.PcTimesBirthedAmilykids, flags.Get(GameFlagIds.PcTimesBirthedAmilykids) + 1);
        }
    }
}