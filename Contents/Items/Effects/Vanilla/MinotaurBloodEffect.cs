using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Configuration;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 MinotaurBlood.hx（牛头人转化）──────────────────────────────
    public class MinotaurBloodEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var mutations = Mutations.Mutations.Instance;

            var random = context.Random;

            var body = player.Body;

            var flags = CoC.Instance.Flags;

            player.SlimeFeed();

            mutations.InitTransformation([2, 3, 3], 1, 2);

            context.Write("Items.MinotaurBlood.Drink".Localized());

            var strPercent = 100f * player.Strength / player.MaxValue(GameStats.Strength);

            var touPercent = 100f * player.Toughness / player.MaxValue(GameStats.Toughness);

            var sensPercent = 100f * player.Sensitivity / player.MaxValue(GameStats.Sensitivity);

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                if (strPercent <= 50)
                {
                    context.Write("Items.MinotaurBlood.StrengthWeak".Localized());

                    player.ApplyStats(new DynStat(GameStats.Strength, strPercent <= 20 ? 3 : 2));
                }
                else
                {
                    player.ApplyStats(new DynStat(GameStats.Strength, strPercent >= 75 ? 0.5f : 1));

                    context.Write("Items.MinotaurBlood.StrengthStrong".Localized());
                }

                if (random.Next(2) == 0 && strPercent > 50)
                {
                    context.Write("Items.MinotaurBlood.Slower".Localized());

                    player.ApplyStats(new DynStat(GameStats.Speed, -1));
                }
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                if (touPercent <= 50)
                {
                    context.Write("Items.MinotaurBlood.ToughnessWeak".Localized());

                    player.ApplyStats(new DynStat(GameStats.Toughness, touPercent <= 20 ? 3 : 2));
                }
                else
                {
                    player.ApplyStats(new DynStat(GameStats.Toughness, touPercent >= 75 ? 0.5f : 1));

                    context.Write("Items.MinotaurBlood.ToughnessStrong".Localized());
                }

                if (random.Next(2) == 0 && sensPercent > 10)
                    if (touPercent > 75)
                    {
                        context.Write("Items.MinotaurBlood.Sens75".Localized());

                        player.ApplyStats(new DynStat(GameStats.Sensitivity, -3));
                    }
                    else if (touPercent > 50)
                    {
                        context.Write("Items.MinotaurBlood.Sens50".Localized());

                        player.ApplyStats(new DynStat(GameStats.Sensitivity, -2));
                    }
                    else
                    {
                        context.Write("Items.MinotaurBlood.SensLow".Localized());

                        player.ApplyStats(new DynStat(GameStats.Sensitivity, -3));
                    }
            }

            var cocks = body.GetList<CockPart>();

            var vaginas = body.GetList<VaginaPart>();

            if (mutations.Changes < mutations.ChangeLimit && random.Next(2) == 0
                && player.BallSize <= 5 && player.CountCocksOfType("Horse") > 0)
            {
                if (player.Balls == 0)
                {
                    player.Balls = 2;

                    player.BallSize = 1;

                    context.Write("Items.MinotaurBlood.GrowBalls".Localized());

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 2),

                        new DynStat(GameStats.Lust, 5));
                }
                else
                {
                    player.BallSize += 1;

                    context.Write(player.BallSize <= 2

                        ? "Items.MinotaurBlood.BallsGrowSmall".Localized(player.SimpleBallsDescript())

                        : "Items.MinotaurBlood.BallsGrowBig".Localized(player.SimpleBallsDescript()));

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 1),

                        new DynStat(GameStats.Lust, 3));
                }

                mutations.Changes += 1;
            }

            if (body.Get<NeckPart>().Id.Path != "normal" && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
                mutations.RestoreNeck("minotaurBlood");

            if (body.Get<RearBodyPart>().Id != BodyPartTypes.SharkFin
                && mutations.Changes < mutations.ChangeLimit && random.Next(5) == 0)

                mutations.RestoreRearBody("minotaurBlood");

            if (random.Next(5) == 0)
                mutations.UpdateOvipositionPerk("minotaurBlood");

            if (random.Next(4) == 0)
                mutations.RestoreArms("minotaurBlood");

            var lowerBody = body.Get<LowerBodyPart>();

            if (lowerBody.Id.Path != "hoofed" && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                if (lowerBody.Id.Path == "human")
                    context.Write("Items.MinotaurBlood.HoovesHuman".Localized());
                else if (lowerBody.Id.Path == "dog")
                    context.Write("Items.MinotaurBlood.HoovesDog".Localized());
                else if (player.HasTailInsteadOfLegs())
                    context.Write("Items.MinotaurBlood.HoovesTail".Localized());
                else if (lowerBody.Id.Path != "naga")
                    context.Write("Items.MinotaurBlood.HoovesOther".Localized());

                context.Write("Items.MinotaurBlood.HoovesEnd".Localized());

                lowerBody.Id = new Identifier("coc", "hoofed");

                lowerBody.LegCount = 2;

                player.ApplyStats(new DynStat(GameStats.Speed, 1));

                mutations.Changes += 1;
            }

            if (!(OptionStore.Instance.Get("hyper", 0) > 0))
            {
                if (vaginas.Count > 0)
                {
                    if (vaginas[0].Looseness > 0)
                    {
                        context.Write("Items.MinotaurBlood.VaginaTighten".Localized(player.VaginaDescript(0)));

                        vaginas[0].Looseness -= 1;
                    }
                    else
                    {
                        context.Write("Items.MinotaurBlood.VaginaGone".Localized(player.VaginaDescript(0)));

                        if (cocks.Count == 0)
                        {
                            context.Write("Items.MinotaurBlood.ClitToCock".Localized());

                            player.CreateCock();

                            cocks = body.GetList<CockPart>();

                            cocks[0].Length = vaginas[0].ClitLength + 2;

                            cocks[0].Thickness = 1;

                            cocks[0].Id = new Identifier("coc", "horse");

                            cocks[0].SheathOn = true;

                            vaginas[0].ClitLength = 0.25f;
                        }

                        player.RemoveVagina(0, 1);

                        vaginas = body.GetList<VaginaPart>();
                    }

                    mutations.Changes += 1;
                }

                var rows = body.GetList<BreastRowPart>();

                if (mutations.Changes < mutations.ChangeLimit && rows.Count > 1 && random.Next(3) == 0)
                    mutations.RemoveExtraBreastRow("minotaurBlood");
                else if (random.Next(2) == 0 && mutations.Changes < mutations.ChangeLimit && rows.Count > 0)
                    if (rows.Count == 1)
                    {
                        if (rows[0].BreastRating >= 1)
                        {
                            var temp = 1;

                            rows[0].BreastRating -= 1;

                            if (rows[0].BreastRating > 8)
                            {
                                temp += 1;

                                rows[0].BreastRating -= 1;
                            }

                            context.Write(temp == 1

                                ? "Items.MinotaurBlood.BreastsShrink".Localized(rows[0].Cup())

                                : "Items.MinotaurBlood.BreastsShrinkMuch".Localized(rows[0].Cup()));

                            mutations.Changes += 1;
                        }
                    }
                    else
                    {
                        var changed = 0;

                        if (player.BiggestBreastSize() >= 1)
                            context.Write("Items.MinotaurBlood.NewLine".Localized());

                        for (var index = 0; index < rows.Count; index++)
                        {
                            if (rows[index].BreastRating < 1)
                                continue;

                            rows[index].BreastRating -= 1;

                            changed += 1;

                            context.Write("Items.MinotaurBlood.NewLine".Localized());

                            context.Write(changed > 1

                                ? "Items.MinotaurBlood.AndYour".Localized()

                                : "Items.MinotaurBlood.Your".Localized());

                            context.Write("Items.MinotaurBlood.BreastRowShrink".Localized(player.BreastDescript(index), rows[index].Cup()));
                        }

                        if (changed == 2)
                            context.Write("Items.MinotaurBlood.Lighter2".Localized());

                        if (changed == 3)
                            context.Write("Items.MinotaurBlood.Lighter3".Localized());

                        if (changed >= 4)
                            context.Write("Items.MinotaurBlood.Lighter4".Localized());

                        if (changed > 0)
                            mutations.Changes += 1;
                    }
            }

            if (mutations.Changes < mutations.ChangeLimit && random.Next(2) == 0 && cocks.Count > 0)
            {
                var selected = -1;

                for (var index = 0; index < cocks.Count; index++)
                    if (cocks[index].Id.Path == "horse" && (cocks[index].Length < 36 || cocks[index].Thickness < 5))
                    {
                        selected = index;

                        break;
                    }

                if (selected != -1)
                {
                    if (cocks[selected].Thickness < 5)
                    {
                        var temp = player.IncreaseCock(selected, 2 + random.Next(8));

                        cocks[selected].Thicken(1);

                        temp += 1;

                        if (temp > 6)
                            context.Write("Items.MinotaurBlood.CockSurgeBig".Localized(player.CockDescript(selected)));
                        else if (temp >= 3)
                            context.Write("Items.MinotaurBlood.CockSurgeMid".Localized(player.CockDescript(selected)));
                        else
                            context.Write("Items.MinotaurBlood.CockSurgeSmall".Localized());

                        context.Write("Items.MinotaurBlood.CockThicker".Localized());
                    }
                    else
                    {
                        var temp = player.IncreaseCock(selected, 2 + random.Next(8));

                        if (temp > 6)
                            context.Write("Items.MinotaurBlood.CockSurgeBig".Localized(player.CockDescript(selected)));
                        else if (temp >= 3)
                            context.Write("Items.MinotaurBlood.CockSurgeMid".Localized(player.CockDescript(selected)));
                        else
                            context.Write("Items.MinotaurBlood.CockSurgeSmall".Localized());
                    }

                    mutations.Changes += 1;
                }
            }

            if (cocks.Count > 0 && random.Next(2) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                var selected = -1;

                for (var index = 0; index < cocks.Count; index++)
                    if (cocks[index].Id.Path != "horse")
                    {
                        selected = index;

                        break;
                    }

                if (selected != -1)
                {
                    if (cocks[selected].Id.Path == "human" || cocks[selected].TypeIndex > 2)
                        context.Write("Items.MinotaurBlood.HorseCockHuman".Localized(player.CockDescript(selected)));

                    if (cocks[selected].Id.Path == "dog")
                        context.Write("Items.MinotaurBlood.HorseCockDog".Localized());

                    cocks[selected].Id = new Identifier("coc", "horse");

                    cocks[selected].SheathOn = true;

                    player.IncreaseCock(selected, 4);

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 5),

                        new DynStat(GameStats.Sensitivity, 4),

                        new DynStat(GameStats.Lust, 35));

                    context.Write(player.CountCocksOfType("Horse") > 1

                        ? "Items.MinotaurBlood.HorseCockAnother".Localized()

                        : "Items.MinotaurBlood.HorseCockEnd".Localized());

                    mutations.Changes += 1;
                }
            }

            if (random.Next(4) == 0)
                player.GoIntoRut(true);

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit
                && !player.HasStatusEffect(StatusEffectIds.Dysfunction))
            {
                if (cocks.Count > 0)
                    context.Write("Items.MinotaurBlood.DysfunctionCock".Localized());
                else if (player.HasVagina())
                    context.Write("Items.MinotaurBlood.DysfunctionVagina".Localized(player.VaginaDescript(0)));

                if (cocks.Count > 0 || player.HasVagina())
                {
                    player.CreateStatusEffect(StatusEffectIds.Dysfunction, 96, 0, 0, 0);

                    mutations.Changes += 1;
                }
            }

            if (mutations.Changes < mutations.ChangeLimit && random.NextDouble() * 1.7 < 1 && player.Tallness < 108)
            {
                var temp = random.Next(5) + 3;

                if (player.Tallness > 90)
                    temp /= 2;

                if (temp == 0)
                    temp = 1;

                if (temp < 5)
                    context.Write("Items.MinotaurBlood.HeightSlight".Localized());
                else if (temp < 7)
                    context.Write("Items.MinotaurBlood.HeightMid".Localized());
                else
                    context.Write("Items.MinotaurBlood.HeightBig".Localized());

                player.Tallness += temp;

                mutations.Changes += 1;
            }

            var ears = body.Get<EarsPart>();

            var tail = body.Get<TailPart>();

            var face = body.Get<FacePart>();

            var horns = body.Get<HornsPart>();

            if ((mutations.TfNoFur() && ears.Id.Path == "cow" && lowerBody.Id.Path == "hoofed"
                && player.Tallness >= 90 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0) && (face.Id.Path != "cow_minotaur"))
                {
                    context.Write("Items.MinotaurBlood.MinoFace".Localized());

                    mutations.Changes += 1;

                    face.Id = new Identifier("coc", "cow_minotaur");
                }

            if (mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0
                && ears.Id.Path == "cow" && tail.Id.Path == "cow")
            {
                if (horns.Id.Path is "cow_minotaur" or "none")
                    if (horns.Id.Path == "cow_minotaur")
                    {
                        if (vaginas.Count > 0)
                        {
                            if (horns.Value > 4)
                            {
                                context.Write("Items.MinotaurBlood.HornsFemaleNoGrow".Localized());

                                context.Write("Items.MinotaurBlood.HeatRefill".Localized());

                                player.HoursSinceCum += 200;

                                player.ApplyStats(new DynStat(GameStats.Lust, 20));
                            }
                            else
                            {
                                context.Write("Items.MinotaurBlood.HornsFemaleGrow".Localized());

                                horns.Value += 3;
                            }

                            mutations.Changes += 1;
                        }
                        else
                        {
                            var temp = 1 + random.Next(3);

                            horns.Value += temp;

                            if (temp == 1)
                                context.Write("Items.MinotaurBlood.HornsGrow1".Localized());
                            else if (temp == 2)
                                context.Write("Items.MinotaurBlood.HornsGrow2".Localized());
                            else
                                context.Write("Items.MinotaurBlood.HornsGrow3".Localized());

                            if (horns.Value < 3)
                                context.Write("Items.MinotaurBlood.HornsTiny".Localized());
                            else if (horns.Value < 6)
                                context.Write("Items.MinotaurBlood.HornsYoung".Localized());
                            else if (horns.Value < 12)
                                context.Write("Items.MinotaurBlood.HornsGrown".Localized());
                            else if (horns.Value < 20)
                                context.Write("Items.MinotaurBlood.HornsLarge".Localized());
                            else
                                context.Write("Items.MinotaurBlood.HornsHuge".Localized());

                            if (random.Next(2) == 0 && mutations.Changes < mutations.ChangeLimit)
                            {
                                context.Write("Items.MinotaurBlood.HeatRefill".Localized());

                                player.HoursSinceCum += 200;

                                player.ApplyStats(new DynStat(GameStats.Lust, 20));
                            }

                            mutations.Changes += 1;
                        }
                    }
                    else
                    {
                        context.Write("Items.MinotaurBlood.HornsNew".Localized());

                        horns.Id = new Identifier("coc", "cow_minotaur");

                        horns.Value = 2;

                        mutations.Changes += 1;
                    }

                if (horns.Id.Path == "demon" || horns.Id.Path != "cow_minotaur")
                {
                    context.Write("Items.MinotaurBlood.HornsReshape".Localized());

                    horns.Id = new Identifier("coc", "cow_minotaur");

                    mutations.Changes += 1;
                }
            }

            if (ears.Id.Path != "cow" && mutations.Changes < mutations.ChangeLimit
                && tail.Id.Path == "cow" && random.Next(2) == 0)
            {
                context.Write("Items.MinotaurBlood.CowEars".Localized());

                ears.Id = new Identifier("coc", "cow");

                mutations.Changes += 1;
            }

            if (mutations.Changes < mutations.ChangeLimit && random.Next(2) == 0 && tail.Id.Path != "cow")
            {
                if (tail.Id.Path == "none")
                    context.Write("Items.MinotaurBlood.TailNone".Localized());
                else if (tail.Id.Path is "spider_abdomen" or "bee_abdomen")
                    context.Write("Items.MinotaurBlood.TailInsect".Localized());
                else
                    context.Write("Items.MinotaurBlood.TailOther".Localized());

                tail.Id = new Identifier("coc", "cow");

                mutations.Changes += 1;
            }

            if (random.Next(4) == 0 && player.HasGills() && mutations.Changes < mutations.ChangeLimit)
                mutations.UpdateGills();

            var anal = body.Get<AnalPart>();

            if (mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0
                && ((anal.Wetness > 0 && !player.HasPerk(PerkIds.MaraesGiftButtslut)) || anal.Wetness > 1))
            {
                context.Write("Items.MinotaurBlood.AnalTighten".Localized());

                anal.Wetness -= 1;

                if (anal.Looseness > 1)
                    anal.Looseness -= 1;

                mutations.Changes += 1;
            }

            if (random.Next(4) == 0)
                context.Write(player.ModFemGoal(5, 10));

            if (random.Next(4) == 0)
                context.Write(player.ModTone(85, 3));

            if (random.Next(4) == 0)
                context.Write(player.ModThickness(70, 4));

            if (mutations.Changes == 0)
            {
                context.Write("Items.MinotaurBlood.FailSafe1".Localized());

                if (player.Balls > 0)
                {
                    context.Write("Items.MinotaurBlood.FailSafeBalls".Localized());

                    player.HoursSinceCum += 200;
                }

                player.HealthChange(50);

                player.ApplyStats(new DynStat(GameStats.Lust, 50));
            }

            player.RefillHunger(25);

            flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }
    }
}