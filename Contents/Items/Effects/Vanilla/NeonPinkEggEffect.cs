using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 NeonPinkEgg.hx（兔类转化）──────────────────────────────────
    public class NeonPinkEggEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context) =>

            ApplyEffect(context.Player, context.Write, context.Random, false);

        public static void ApplyEffect(Player player, Action<string> write, Random random, bool pregnantChange)
        {
            var mutations = Mutations.Mutations.Instance;

            var body = player.Body;

            var flags = CoC.Instance.Flags;

            mutations.InitTransformation([2, 2]);

            if (!pregnantChange)
                write("Items.NeonPinkEgg.Eat".Localized());
            else
            {
                mutations.ChangeLimit = 1;

                var womb = player.Pregnancy.Type == PregnancyIds.Bunny;

                var backdoor = player.ButtPregnancy.Type == PregnancyIds.Bunny;

                write("Items.NeonPinkEgg.PregnancyWarning1".Localized());

                if (womb)
                    write("Items.NeonPinkEgg.PregnancyWomb".Localized());

                if (womb && backdoor)
                    write("Items.NeonPinkEgg.PregnancyAnd".Localized());

                if (backdoor)
                    write("Items.NeonPinkEgg.PregnancyBackdoor".Localized());

                write(womb && backdoor

                    ? "Items.NeonPinkEgg.PregnancyRumblePlural".Localized()

                    : "Items.NeonPinkEgg.PregnancyRumble".Localized());

                write("Items.NeonPinkEgg.PregnancyWarning2".Localized());
            }

            var spePercent = 100f * player.Speed / player.MaxValue(GameStats.Speed);

            if (mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0 && spePercent < 80)
            {
                if (spePercent < 30)
                    write("Items.NeonPinkEgg.SpeedLow".Localized());
                else if (spePercent < 50)
                    write("Items.NeonPinkEgg.SpeedMid".Localized());
                else if (spePercent < 65)
                    write("Items.NeonPinkEgg.SpeedHigh".Localized());
                else
                    write("Items.NeonPinkEgg.SpeedMax".Localized());

                player.ApplyStats(new DynStat(GameStats.Speed, spePercent < 35 ? 2 : 1));
            }

            if (mutations.Changes < mutations.ChangeLimit && random.Next(5) == 0)
            {
                player.ApplyStats(
                    new DynStat(GameStats.Libido, 1),

                    new DynStat(GameStats.Lust, 5 + player.Libido / 7));

                if (player.LibidoPercent < 30)
                    player.ApplyStats(new DynStat(GameStats.Libido, 1));

                if (player.LibidoPercent < 40)
                    player.ApplyStats(new DynStat(GameStats.Libido, 1));

                if (player.LibidoPercent < 60)
                    player.ApplyStats(new DynStat(GameStats.Libido, 1));

                WriteLibidoText(player, write, random);
            }

            var sensPercent = player.SensitivityPercent;

            if (sensPercent < 60 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
                if (random.Next(3) != 2)
                {
                    write("Items.NeonPinkEgg.SensitivityLow".Localized());

                    player.ApplyStats(new DynStat(GameStats.Sensitivity, 5));
                }
                else
                {
                    player.ApplyStats(new DynStat(GameStats.Sensitivity, 15));

                    write("Items.NeonPinkEgg.SensitivityHigh1".Localized());

                    switch (player.Gender)
                    {
                        case Gender.Genderless:
                            write("Items.NeonPinkEgg.SensitivityGenderless".Localized());

                            break;

                        case Gender.Male:
                            write("Items.NeonPinkEgg.SensitivityMale".Localized());

                            break;

                        case Gender.Female:
                            write("Items.NeonPinkEgg.SensitivityFemale".Localized());

                            break;

                        case Gender.Hermaphrodite:
                            write("Items.NeonPinkEgg.SensitivityHerm".Localized());

                            break;
                    }

                    write("Items.NeonPinkEgg.SensitivityHigh2".Localized());
                }

            if (mutations.Changes < mutations.ChangeLimit && random.Next(2) == 0)
            {
                var text = player.ModFemGoal(player.Gender is Gender.Female or Gender.Hermaphrodite ? 90 : 61, 4);

                if (text != "")
                {
                    write(text);

                    mutations.Changes += 1;
                }
            }

            var vaginas = body.GetList<VaginaPart>();

            if (player.Wetness() > 3 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0
                && player.HasVagina())
            {
                write("Items.NeonPinkEgg.Wetness".Localized());

                vaginas = body.GetList<VaginaPart>();

                vaginas[0].Wetness -= 1;

                mutations.Changes += 1;
            }

            if (mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0
                && player.Fertility < 50 && player.HasVagina())
            {
                player.Fertility += 2 + random.Next(5);

                mutations.Changes += 1;

                write("Items.NeonPinkEgg.Fertility".Localized());
            }

            if (body.Get<NeckPart>().Id.Path != "normal" && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
                mutations.RestoreNeck("neonPinkEgg");

            if (body.Get<RearBodyPart>().Id != BodyPartTypes.SharkFin
                && mutations.Changes < mutations.ChangeLimit && random.Next(5) == 0)

                mutations.RestoreRearBody("neonPinkEgg");

            if (random.Next(4) == 0)
                mutations.UpdateOvipositionPerk("neonPinkEgg");

            if (player.HasVagina() && !player.HasPerk(PerkIds.BunnyEggs)
                && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0 && player.BunnyScore() > 3)
            {
                write("Items.NeonPinkEgg.BunnyEggs1".Localized());

                write("Items.NeonPinkEgg.BunnyEggs2".Localized());

                player.CreatePerk(PerkIds.BunnyEggs, 0, 0, 0, 0);

                mutations.Changes += 1;
            }

            if (player.Balls > 0 && player.BallSize > 5 && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                if (player.BallSize < 10)
                {
                    write("Items.NeonPinkEgg.BallsShrinkSmall".Localized());

                    player.BallSize -= 1;
                }
                else if (player.BallSize < 25)
                {
                    write("Items.NeonPinkEgg.BallsShrinkMid".Localized());

                    player.BallSize -= 2 + random.Next(3);
                }
                else
                {
                    write("Items.NeonPinkEgg.BallsShrinkBig".Localized());

                    player.BallSize -= 6 + random.Next(3);
                }

                mutations.Changes += 1;
            }

            if (player.Balls > 2 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                mutations.Changes += 1;

                write("Items.NeonPinkEgg.ExtraBalls".Localized());

                player.Balls -= 2;
            }

            if ((player.Balls > 0 || player.HasCock()) && player.CumQ() < 3000
                && random.Next(3) == 0 && mutations.ChangeLimit > 1)
            {
                mutations.Changes += 1;

                player.CumMultiplier += 3 + random.Next(7);

                if (player.CumQ() >= 250)
                    player.ApplyStats(new DynStat(GameStats.Lust, 3));

                if (player.CumQ() >= 750)
                    player.ApplyStats(new DynStat(GameStats.Lust, 4));

                if (player.CumQ() >= 2000)
                    player.ApplyStats(new DynStat(GameStats.Lust, 5));

                if (player.Balls > 0)
                {
                    if (player.CumQ() < 50)
                        write("Items.NeonPinkEgg.CumBallsSmall".Localized());
                    else if (player.CumQ() < 250)
                    {
                        write("Items.NeonPinkEgg.CumBallsMid1".Localized());

                        if (player.HasCock())
                            write("Items.NeonPinkEgg.CumBallsMid2".Localized());
                    }
                    else if (player.CumQ() < 750)
                    {
                        write("Items.NeonPinkEgg.CumBallsLarge1".Localized());

                        if (player.HasCock())
                            write("Items.NeonPinkEgg.CumBallsLargeCock".Localized());
                    }
                    else if (player.CumQ() < 2000)
                    {
                        write("Items.NeonPinkEgg.CumBallsXL1".Localized());

                        if (player.HasCock())
                            write("Items.NeonPinkEgg.CumBallsXLCock".Localized());

                        write("Items.NeonPinkEgg.CumBallsXL2".Localized());
                    }
                    else
                    {
                        write("Items.NeonPinkEgg.CumBallsXXL1".Localized());

                        if (player.HasCock())
                            write("Items.NeonPinkEgg.CumBallsXXLCock".Localized());

                        write("Items.NeonPinkEgg.CumBallsXXL2".Localized());
                    }
                }
                else
                    if (player.CumQ() < 50)
                        write("Items.NeonPinkEgg.CumNoBallsSmall".Localized());
                    else if (player.CumQ() < 250)
                        write("Items.NeonPinkEgg.CumNoBallsMid".Localized());
                    else if (player.CumQ() < 750)
                        write("Items.NeonPinkEgg.CumNoBallsLarge".Localized());
                    else if (player.CumQ() < 2000)
                        write("Items.NeonPinkEgg.CumNoBallsXL".Localized());
                    else
                        write("Items.NeonPinkEgg.CumNoBallsXXL".Localized());
            }

            var ears = body.Get<EarsPart>();

            var lowerBody = body.Get<LowerBodyPart>();

            var tail = body.Get<TailPart>();

            var face = body.Get<FacePart>();

            if (lowerBody.Id.Path != "bunny" && mutations.Changes < mutations.ChangeLimit
                && random.Next(5) == 0 && ears.Id.Path == "bunny")
            {
                write(player.IsTaur()

                    ? "Items.NeonPinkEgg.BunnyFeetTaur".Localized()

                    : "Items.NeonPinkEgg.BunnyFeet".Localized());

                mutations.Changes += 1;

                lowerBody.Id = new Identifier("coc", "bunny");

                lowerBody.LegCount = 2;
            }

            if (mutations.TfNoFur() && ears.Id.Path == "bunny" && face.Id.Path != "bunny"
                && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                write("Items.NeonPinkEgg.BunnyFace1".Localized());

                mutations.Changes += 1;

                if (HumanishFaces.Contains(face.Id.Path))
                    write("Items.NeonPinkEgg.BunnyFaceHumanish".Localized());
                else
                    write("Items.NeonPinkEgg.BunnyFaceFurry".Localized());

                face.Id = new Identifier("coc", "bunny");
            }

            if (ears.Id.Path != "bunny" && mutations.Changes < mutations.ChangeLimit
                && random.Next(3) == 0 && tail.Id.Path == "rabbit")
            {
                write("Items.NeonPinkEgg.BunnyEars".Localized());

                ears.Id = new Identifier("coc", "bunny");

                mutations.Changes += 1;
            }

            if (tail.Id.Path != "rabbit" && random.Next(2) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                write(tail.Id.Path != "none"

                    ? "Items.NeonPinkEgg.BunnyTailExisting".Localized()

                    : "Items.NeonPinkEgg.BunnyTailNone".Localized());

                tail.Id = new Identifier("coc", "rabbit");

                mutations.Changes += 1;
            }

            if (random.Next(4) == 0 && player.HasGills() && mutations.Changes < mutations.ChangeLimit)
                mutations.UpdateGills();

            if (body.Get<AntennaePart>().Id.Path != "none" && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
                mutations.RemoveAntennae();

            var wings = body.Get<WingsPart>();

            var rearBody = body.Get<RearBodyPart>();

            if ((wings.Id != BodyPartTypes.None || rearBody.Id == BodyPartTypes.SharkFin)
                && random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit)

                mutations.RemoveWings("neonPinkEgg");

            if (mutations.Changes == 0)
            {
                if (player.LibidoPercent < 100)
                    mutations.Changes += 1;

                player.ApplyStats(
                    new DynStat(GameStats.Libido, 1),

                    new DynStat(GameStats.Lust, 5 + player.Libido / 7));

                if (player.LibidoPercent < 30)
                    player.ApplyStats(new DynStat(GameStats.Libido, 1));

                if (player.LibidoPercent < 40)
                    player.ApplyStats(new DynStat(GameStats.Libido, 1));

                if (player.LibidoPercent < 60)
                    player.ApplyStats(new DynStat(GameStats.Libido, 1));

                WriteLibidoText(player, write, random);
            }

            player.RefillHunger(20);

            flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }

        private static readonly string[] HumanishFaces =

        [
            "human", "horse", "dog", "cow_minotaur", "shark_teeth", "snake_fangs",

            "catgirl", "bunny", "kangaroo", "spider_fangs", "fox", "raccoon_mask",
        ];

        private static void WriteLibidoText(Player player, Action<string> write, Random random)
        {
            var libPercent = player.LibidoPercent;

            if (libPercent < 60)
            {
                if (!player.HasCock() || (player.Gender == Gender.Hermaphrodite && random.Next(2) == 0))
                {
                    if (libPercent >= 30)
                        write("Items.NeonPinkEgg.LibidoCuntMid".Localized());
                    else
                    {
                        write("Items.NeonPinkEgg.LibidoCuntLow1".Localized());

                        if (player.Corruption < 25)
                            write("Items.NeonPinkEgg.LibidoCor25".Localized());
                        else if (player.Corruption < 60)
                            write("Items.NeonPinkEgg.LibidoCor60".Localized());
                        else if (player.Corruption < 90)
                            write("Items.NeonPinkEgg.LibidoCor90".Localized());
                        else
                            write("Items.NeonPinkEgg.LibidoCorMax".Localized());
                    }
                }
                else
                    if (libPercent >= 30)
                        write("Items.NeonPinkEgg.LibidoCockMid".Localized());
                    else
                    {
                        write("Items.NeonPinkEgg.LibidoCockLow1".Localized());

                        write(random.Next(2) == 0

                            ? "Items.NeonPinkEgg.LibidoFuckFemaleHare".Localized()

                            : "Items.NeonPinkEgg.LibidoFuckHermRabbit".Localized());

                        if (player.Corruption < 25)
                            write("Items.NeonPinkEgg.LibidoCor25".Localized());
                        else if (player.Corruption < 50)
                            write("Items.NeonPinkEgg.LibidoCor50".Localized());
                        else if (player.Corruption < 75)
                            write("Items.NeonPinkEgg.LibidoCor75".Localized());
                        else
                            write("Items.NeonPinkEgg.LibidoCorMaxCock".Localized());
                    }
            }
            else if (libPercent < 80)
            {
                write("Items.NeonPinkEgg.LibidoHigh1".Localized());

                switch (player.Gender)
                {
                    case Gender.Genderless:
                        write("Items.NeonPinkEgg.LibidoHighGenderless".Localized());

                        break;

                    case Gender.Male:
                        write("Items.NeonPinkEgg.LibidoHighMale".Localized());

                        break;

                    case Gender.Female:
                        write("Items.NeonPinkEgg.LibidoHighFemale".Localized());

                        break;

                    case Gender.Hermaphrodite:
                        write("Items.NeonPinkEgg.LibidoHighHerm".Localized());

                        break;
                }
            }
            else
            {
                write("Items.NeonPinkEgg.LibidoMega1".Localized());

                if (player.Corruption < 33)
                    write("Items.NeonPinkEgg.LibidoMegaLow".Localized());
                else if (player.Corruption < 66)
                    write("Items.NeonPinkEgg.LibidoMegaMid".Localized());
                else
                {
                    write("Items.NeonPinkEgg.LibidoMegaHigh".Localized());

                    write(libPercent < 100

                        ? "Items.NeonPinkEgg.LibidoMegaHigher".Localized()

                        : "Items.NeonPinkEgg.LibidoMegaFuck".Localized());
                }
            }
        }
    }
}