using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 RhinoSteak.hx（犀牛转化）───────────────────────────────────
    public class RhinoSteakEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var mutations = Mutations.Mutations.Instance;

            var random = context.Random;

            var body = player.Body;

            var flags = CoC.Instance.Flags;

            mutations.InitTransformation(null, 3);

            player.RefillHunger(40);

            var strPercent = 100f * player.Strength / player.MaxValue(GameStats.Strength);

            var touPercent = 100f * player.Toughness / player.MaxValue(GameStats.Toughness);

            var spePercent = 100f * player.Speed / player.MaxValue(GameStats.Speed);

            if (random.Next(3) == 0 && strPercent < 100)
            {
                if (strPercent < 50)
                {
                    context.Write("Items.RhinoSteak.StrengthLow".Localized());

                    player.ApplyStats(new DynStat(GameStats.Strength, 0.5f));
                }
                else
                    context.Write("Items.RhinoSteak.StrengthHigh".Localized());

                player.ApplyStats(new DynStat(GameStats.Strength, 0.5f));
            }

            if (random.Next(3) == 0 && touPercent < 100)
            {
                context.Write("Items.RhinoSteak.Toughness".Localized());

                player.ApplyStats(new DynStat(GameStats.Toughness, 1));
            }

            if (random.Next(2) == 0 && spePercent > 80 && strPercent >= 50)
            {
                context.Write("Items.RhinoSteak.Slower".Localized());

                player.ApplyStats(new DynStat(GameStats.Speed, -1));
            }

            if (random.Next(3) == 0 && touPercent < 50 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.RhinoSteak.NumbSkin".Localized());

                player.ApplyStats(new DynStat(GameStats.Sensitivity, -1));
            }

            var horns = body.Get<HornsPart>();

            var ears = body.Get<EarsPart>();

            var lowerBody = body.Get<LowerBodyPart>();

            var face = body.Get<FacePart>();

            if (random.Next(3) == 0 && player.Intelligence > 15 && face.Id.Path == "rhino" && horns.Value == 2)
            {
                context.Write("Items.RhinoSteak.Dumber".Localized());

                player.ApplyStats(new DynStat(GameStats.Intelligence, -1));
            }

            if (random.Next(3) == 0 && player.RhinoScore() >= 2
                && (random.Next(2) == 0 || !player.InRut) && player.HasCock())

                player.GoIntoRut(true);

            if (body.Get<NeckPart>().Id.Path != "normal" && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
                mutations.RestoreNeck("rhinoTFs");

            if (body.Get<RearBodyPart>().Id != BodyPartTypes.SharkFin
                && mutations.Changes < mutations.ChangeLimit && random.Next(5) == 0)

                mutations.RestoreRearBody("rhinoTFs");

            if (random.Next(5) == 0)
                mutations.UpdateOvipositionPerk("rhinoTFs");

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit
                && horns.Id.Path != "unicorn" && ears.Id.Path == "horse"

                && (lowerBody.Id.Path is "hoofed" or "cloven_hoofed" || player.HorseScore() >= 3))
            {
                context.Write("Items.RhinoSteak.UnicornHorn".Localized());

                horns.Id = new Identifier("coc", "unicorn");

                horns.Value = 6;

                mutations.Changes += 1;
            }

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit
                && horns.Id.Path == "unicorn" && horns.Value is > 0 and < 12)
            {
                context.Write("Items.RhinoSteak.UnicornHornGrow".Localized());

                horns.Value = 12;

                mutations.Changes += 1;
            }

            var wings = body.Get<WingsPart>();

            var rearBody = body.Get<RearBodyPart>();

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit
                && (wings.Id != BodyPartTypes.None || rearBody.Id == BodyPartTypes.SharkFin))
            {
                if (rearBody.Id == BodyPartTypes.SharkFin)
                {
                    context.Write("Items.RhinoSteak.FinGone".Localized());

                    rearBody.Restore();
                }
                else
                    context.Write("Items.RhinoSteak.WingsGone".Localized());

                wings.Restore();

                mutations.Changes += 1;
            }

            var skin = body.Get<SkinPart>();

            var arms = body.Get<ArmsPart>();

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit
                && (skin.Id.Path != "plain" || skin.Tone != "gray" || skin.Adj != "tough"))
            {
                context.Write("Items.RhinoSteak.Skin1".Localized());

                context.Write(skin.Id.Path switch
                {
                    "plain" => "Items.RhinoSteak.SkinPlain".Localized(),

                    "fur" => "Items.RhinoSteak.SkinFur".Localized(),

                    "lizard_scales" or "dragon_scales" or "fish_scales" => "Items.RhinoSteak.SkinScales".Localized(),

                    "goo" => "Items.RhinoSteak.SkinGoo".Localized(),

                    _ => "Items.RhinoSteak.SkinDefault".Localized(),
                });

                skin.Tone = "gray";

                skin.Adj = "tough";

                skin.Id = new Identifier("coc", "plain");

                skin.DescKey = "Skin.Desc.skin";

                body.Get<UnderBodyPart>().Restore();

                arms.UpdateClaws(arms.Claws.Id);

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && arms.Id.Path != "human")
            {
                if (arms.Id.Path == "harpy")
                    context.Write("Items.RhinoSteak.ArmsHarpy".Localized());
                else if (arms.Id.Path == "spider")
                    context.Write("Items.RhinoSteak.ArmsSpider".Localized());

                arms.Restore();

                mutations.Changes += 1;
            }

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit && lowerBody.Id.Path != "human")
            {
                if (player.IsBiped())
                    context.Write("Items.RhinoSteak.LegsHuman".Localized());

                lowerBody.Id = new Identifier("coc", "human");

                lowerBody.LegCount = 2;

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                && body.Get<AntennaePart>().Id.Path != "none")

                mutations.RemoveAntennae();

            var hair = body.Get<HairPart>();

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit && hair.Id.Path != "normal")
            {
                switch (hair.Id.Path)
                {
                    case "feather":
                        context.Write(hair.Length >= 6

                            ? "Items.RhinoSteak.HairFeatherLong".Localized()

                            : "Items.RhinoSteak.HairFeatherShort".Localized());

                        break;

                    case "goo":
                        context.Write("Items.RhinoSteak.HairGoo".Localized());

                        break;

                    case "ghost" or "anemone":
                        context.Write("Items.RhinoSteak.HairTentacle".Localized());

                        break;
                }

                mutations.Changes += 1;
            }

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit && hair.Id.Path != "normal")
            {
                hair.Id = new Identifier("coc", "normal");

                flags.Set(GameFlagIds.HairGrowthStoppedBecauseLizard, 0);

                if (mutations.Changes < mutations.ChangeLimit)
                    mutations.Changes += 0;
            }

            // 上面的分支在原版里是同一个 if 内的收尾；这里保持“头发变正常 + 解除停长”的语义
            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                && flags.Get(GameFlagIds.HairGrowthStoppedBecauseLizard) > 0)
            {
                context.Write("Items.RhinoSteak.HairGrowingAgain".Localized());

                flags.Set(GameFlagIds.HairGrowthStoppedBecauseLizard, 0);

                mutations.Changes += 1;
            }

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit && player.HasGills())
                mutations.UpdateGills();

            var cocks = body.GetList<CockPart>();

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit
                && player.HasCock() && player.CountCocksOfType("Rhino") < cocks.Count)
            {
                context.Write(cocks.Count == 1

                    ? "Items.RhinoSteak.RhinoCockOne".Localized()

                    : "Items.RhinoSteak.RhinoCockMany".Localized());

                context.Write("Items.RhinoSteak.RhinoCockBody".Localized(
                    player.ClothedOrNakedLower("pull it out from your [armor]", "lean over")));

                foreach (var cock in cocks)
                    if (cock.Id.Path != "rhino")
                    {
                        cock.Id = new Identifier("coc", "rhino");

                        cock.SheathOn = true;

                        break;
                    }

                player.ApplyStats(new DynStat(GameStats.Lust, 20));

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && ears.Id.Path != "rhino")
            {
                context.Write("Items.RhinoSteak.Ears".Localized());

                ears.Id = new Identifier("coc", "rhino");

                mutations.Changes += 1;
            }

            if (mutations.TfNoFur() && random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit
                && ears.Id.Path == "rhino" && skin.Tone == "gray" && face.Id.Path != "rhino")
            {
                context.Write("Items.RhinoSteak.Face".Localized());

                face.Id = new Identifier("coc", "rhino");

                mutations.Changes += 1;
            }

            var tail = body.Get<TailPart>();

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                && player.IsBiped() && tail.Id.Path != "rhino")
            {
                context.Write(tail.Id.Path != "none"

                    ? "Items.RhinoSteak.TailOther".Localized()

                    : "Items.RhinoSteak.TailNone".Localized());

                tail.Id = new Identifier("coc", "rhino");

                mutations.Changes += 1;
            }

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit
                && face.Id.Path == "rhino" && horns.Id.Path != "rhino")
            {
                context.Write("Items.RhinoSteak.Horns1".Localized());

                horns.Value = 1;

                horns.Id = new Identifier("coc", "rhino");

                mutations.Changes += 1;
            }

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit
                && face.Id.Path == "rhino" && horns.Id.Path == "rhino" && horns.Value == 1)
            {
                context.Write("Items.RhinoSteak.Horns2".Localized());

                context.Write("Items.RhinoSteak.Horns2End".Localized());

                horns.Value = 2;

                horns.Id = new Identifier("coc", "rhino");

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                && player.HasCock() && player.ShortestCockLength() < 10

                && cocks.Count - player.CountCocksOfType("Rhino") > 0)
            {
                var affected = 0;

                var temp = 0f;

                foreach (var cock in cocks)
                {
                    if (cock.Id.Path == "rhino" && cock.Length >= 10)
                        continue;

                    temp = player.IncreaseCock(player.SmallestCockIndex(), random.Next(2) + 1);

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 0.5f),

                        new DynStat(GameStats.Lust, 3));

                    affected += 1;
                }

                context.Write("Items.RhinoSteak.LengthChange".Localized());

                player.LengthChange(temp, affected);

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                && player.HasCock() && player.CountCocksOfType("Rhino") > 0)
            {
                foreach (var cock in cocks)
                    if (cock.Id.Path == "rhino" && cock.Thickness < 3)
                    {
                        cock.Thicken(0.5f);

                        player.ApplyStats(
                            new DynStat(GameStats.Libido, 0.5f),

                            new DynStat(GameStats.Lust, 3));

                        break;
                    }

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                && player.HasCock() && player.CountCocksOfType("Rhino") > 0)
            {
                for (var index = 0; index < cocks.Count; index++)
                    if (cocks[index].Id.Path == "rhino" && cocks[index].Length < 18)
                    {
                        var temp = player.IncreaseCock(index, 1 + random.Next(2));

                        context.Write("Items.RhinoSteak.LengthChange".Localized());

                        player.LengthChange(temp, 1);

                        player.ApplyStats(
                            new DynStat(GameStats.Libido, 0.5f),

                            new DynStat(GameStats.Lust, 3));

                        break;
                    }

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                && player.Balls > 0 && player.BallSize < 4)
            {
                context.Write(player.BallSize <= 2

                    ? "Items.RhinoSteak.BallsGrowSmall".Localized(player.SimpleBallsDescript())

                    : "Items.RhinoSteak.BallsGrowBig".Localized(player.SimpleBallsDescript()));

                player.ApplyStats(
                    new DynStat(GameStats.Libido, 1),

                    new DynStat(GameStats.Lust, 3));

                player.BallSize += 1;

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                && player.HasVagina() && player.StatusEffectValue(StatusEffectIds.BonusVCapacity, 0) < 40)
            {
                if (!player.HasStatusEffect(StatusEffectIds.BonusVCapacity))
                    player.CreateStatusEffect(StatusEffectIds.BonusVCapacity, 0, 0, 0, 0);

                player.ChangeStatusValue(StatusEffectIds.BonusVCapacity, 0, 5);

                context.Write("Items.RhinoSteak.Vagina".Localized(player.VaginaDescript(0)));

                mutations.Changes += 1;
            }

            // 原版这里误用了 BonusVCapacity 判断后庭容量，按意图改成 BonusACapacity
            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                && player.HasVagina() && player.StatusEffectValue(StatusEffectIds.BonusACapacity, 0) < 60)
            {
                if (!player.HasStatusEffect(StatusEffectIds.BonusACapacity))
                    player.CreateStatusEffect(StatusEffectIds.BonusACapacity, 0, 0, 0, 0);

                player.ChangeStatusValue(StatusEffectIds.BonusACapacity, 0, 5);

                context.Write("Items.RhinoSteak.Anal".Localized());

                mutations.Changes += 1;
            }

            if (random.Next(2) == 0 && mutations.Changes < mutations.ChangeLimit && player.Tallness < 102)
            {
                var temp = random.Next(5) + 3;

                if (player.Tallness > 90)
                    temp /= 2;

                if (temp == 0)
                    temp = 1;

                if (temp > 6)
                    temp = 6;

                if (temp < 3)
                    context.Write("Items.RhinoSteak.HeightSlight".Localized());
                else if (temp < 6)
                    context.Write("Items.RhinoSteak.HeightMid".Localized());
                else
                    context.Write("Items.RhinoSteak.HeightBig".Localized());

                player.Tallness += temp;

                mutations.Changes += 1;
            }

            if (random.Next(2) == 0 && player.Tone < 80)
                if (player.Tone < 50)
                    player.ModTone(80, 2 + random.Next(2));
                else
                    player.ModTone(80, 1 + random.Next(2));

            if (random.Next(2) == 0 && player.Thickness < 80)
                if (player.Thickness < 50)
                    player.ModThickness(80, 2 + random.Next(2));
                else
                    player.ModThickness(80, 1 + random.Next(2));

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                && flags.Get(GameFlagIds.IncreasedHairGrowthSerumTimesApplied) > 0)
            {
                context.Write("Items.RhinoSteak.HairSerumReset".Localized());

                flags.Set(GameFlagIds.IncreasedHairGrowthSerumTimesApplied, 0);

                mutations.Changes += 1;
            }

            flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }
    }
}