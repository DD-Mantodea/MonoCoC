using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 MouseCocoa.hx（鼠类转化）───────────────────────────────────
    public class MouseCocoaEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var mutations = Mutations.Mutations.Instance;

            var random = context.Random;

            var body = player.Body;

            var flags = CoC.Instance.Flags;

            mutations.InitTransformation([2, 3, 3]);

            context.Write("Items.MouseCocoa.Intro1".Localized());

            if (!player.IsTaur())
                context.Write("Items.MouseCocoa.IntroSit".Localized());

            context.Write("Items.MouseCocoa.Intro2".Localized());

            if (player.Tallness >= 45 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                context.Write(player.IsTaur()

                    ? "Items.MouseCocoa.HeightTaur".Localized()

                    : "Items.MouseCocoa.Height".Localized());

                player.ApplyStats(new DynStat(GameStats.Speed, 1));

                player.Tallness -= 1;

                if (player.Tallness > 60)
                    player.Tallness -= 1;

                if (player.Tallness > 70)
                    player.Tallness -= 1;

                if (player.Tallness > 80)
                    player.Tallness -= 1;

                if (player.Tallness > 90)
                    player.Tallness -= 2;

                if (player.Tallness > 100)
                    player.Tallness -= 2;

                mutations.Changes += 1;
            }

            if (100f * player.Toughness / player.MaxValue(GameStats.Toughness) > 50
                && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                context.Write("Items.MouseCocoa.Toughness".Localized());

                player.ApplyStats(new DynStat(GameStats.Toughness, -1));

                if (100f * player.Toughness / player.MaxValue(GameStats.Toughness) >= 75)
                    player.ApplyStats(new DynStat(GameStats.Toughness, -1));

                if (100f * player.Toughness / player.MaxValue(GameStats.Toughness) >= 90)
                    player.ApplyStats(new DynStat(GameStats.Toughness, -1));
            }

            var vaginal = player.VaginalCapacity();

            var anal = player.AnalCapacity();

            if (player.Tallness < 60
                && (anal < 100 || (vaginal < 100 && player.HasVagina()))

                && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                context.Write("Items.MouseCocoa.Capacity1".Localized());

                context.Write(vaginal < 100 && player.HasVagina()

                    ? "Items.MouseCocoa.CapacityVagina".Localized()

                    : "Items.MouseCocoa.CapacityAnal".Localized());

                context.Write("Items.MouseCocoa.Capacity2".Localized());

                if (player.IsTaur())
                    context.Write("Items.MouseCocoa.CapacityTaurScratch".Localized());
                else
                    context.Write("Items.MouseCocoa.CapacityScratch".Localized());

                context.Write("Items.MouseCocoa.Capacity3".Localized());

                if (player.IsTaur())
                    context.Write("Items.MouseCocoa.CapacityTaurEnd".Localized());
                else
                    context.Write("Items.MouseCocoa.CapacityEnd".Localized());

                context.Write("Items.MouseCocoa.CapacityEndPeriod".Localized());

                player.ApplyStats(new DynStat(GameStats.Lust, 10 + player.Sensitivity / 5));

                if (vaginal < 100 && player.HasVagina())
                {
                    if (!player.HasStatusEffect(StatusEffectIds.BonusVCapacity))
                        player.CreateStatusEffect(StatusEffectIds.BonusVCapacity, 0, 0, 0, 0);

                    player.ChangeStatusValue(StatusEffectIds.BonusVCapacity, 0, 5);
                }
                else
                {
                    if (!player.HasStatusEffect(StatusEffectIds.BonusACapacity))
                        player.CreateStatusEffect(StatusEffectIds.BonusACapacity, 0, 0, 0, 0);

                    player.ChangeStatusValue(StatusEffectIds.BonusACapacity, 0, 5);
                }

                mutations.Changes += 1;
            }

            if (player.StatusEffectValue(StatusEffectIds.Heat, 1) < 30
                && random.Next(2) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                var intensified = player.InHeat;

                if (player.GoIntoHeat(false))
                {
                    if (intensified)
                    {
                        context.Write("Items.MouseCocoa.HeatIntensified".Localized());

                        var minoCum = new Identifier("coc", "minotaur_cum");

                        if (!player.Inventory.HasItem(minoCum))
                            context.Write("Items.MouseCocoa.HeatNoMinoCum".Localized());
                        else if (player.Lust < player.MaxLust || player.IsTaur())
                            context.Write("Items.MouseCocoa.HeatMinoCumConsider".Localized());
                        else
                        {
                            context.Write("Items.MouseCocoa.HeatMinoCumUse".Localized());

                            player.MinoCumAddiction(5);

                            player.KnockUp(PregnancyIds.Minotaur, 175, 175);

                            player.ConsumeItem(minoCum);
                        }
                    }
                    else
                    {
                        context.Write("Items.MouseCocoa.HeatStart".Localized());

                        player.Fertility += 1;
                    }

                    mutations.Changes += 1;
                }
            }

            if (body.Get<NeckPart>().Id.Path != "normal" && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
                mutations.RestoreNeck("mouseCocoa");

            if (body.Get<RearBodyPart>().Id != BodyPartTypes.SharkFin
                && mutations.Changes < mutations.ChangeLimit && random.Next(5) == 0)

                mutations.RestoreRearBody("mouseCocoa");

            if (random.Next(5) == 0)
                mutations.UpdateOvipositionPerk("mouseCocoa");

            var ears = body.Get<EarsPart>();

            var tail = body.Get<TailPart>();

            var face = body.Get<FacePart>();

            if (ears.Id.Path != "mouse" && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                context.Write("Items.MouseCocoa.Ears1".Localized());

                if (ears.Id.Path is "horse" or "cow" or "dog" or "bunny" or "kangaroo")
                    context.Write("Items.MouseCocoa.EarsShrink".Localized());
                else
                    context.Write("Items.MouseCocoa.EarsPull".Localized());

                context.Write("Items.MouseCocoa.Ears2".Localized());

                ears.Id = new Identifier("coc", "mouse");

                mutations.Changes += 1;
            }

            if (ears.Id.Path == "mouse" && tail.Id.Path != "mouse"
                && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                if (tail.Id.Path != "none")
                {
                    context.Write("Items.MouseCocoa.TailOther1".Localized());

                    context.Write(tail.Id.Path == "horse"

                        ? "Items.MouseCocoa.TailElongate".Localized()

                        : "Items.MouseCocoa.TailCompress".Localized());

                    context.Write("Items.MouseCocoa.TailOther2".Localized());
                }
                else
                    context.Write("Items.MouseCocoa.TailNone".Localized());

                tail.Id = new Identifier("coc", "mouse");

                mutations.Changes += 1;
            }

            if (mutations.TfNoFur() && ears.Id.Path == "mouse"
                && face.Id.Path is "human" or "shark_teeth" or "bunny" or "spider_fangs" or "raccoon_mask"

                && random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.MouseCocoa.Teeth1".Localized());

                if (face.Id.Path != "human")
                    context.Write("Items.MouseCocoa.TeethRecede".Localized());

                context.Write("Items.MouseCocoa.Teeth2".Localized());

                face.Id = new Identifier("coc", "buckteeth");

                mutations.Changes += 1;
            }

            // 原版这里用一串 || 判断“不是这些人脸”，恒为真；改成真正的“还不是鼠脸”判断
            if (mutations.TfNoFur() && player.HasFur() && face.Id.Path != "mouse"
                && random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.MouseCocoa.MouseFace".Localized());

                face.Id = new Identifier("coc", "mouse");

                mutations.Changes += 1;
            }

            var skin = body.Get<SkinPart>();

            if (mutations.TfNoFur()
                && (!player.HasFur() || (player.HasFur() && skin.FurColor != "brown" && skin.FurColor != "white"))

                && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                if (!player.HasFur())
                {
                    context.Write("Items.MouseCocoa.FurItch1".Localized());

                    if (!player.HasPlainSkin())
                        context.Write("Items.MouseCocoa.FurItchPlural".Localized());

                    context.Write("Items.MouseCocoa.FurItch2".Localized());

                    if (tail.Id.Path != "none")
                        context.Write("Items.MouseCocoa.FurNotTail".Localized());

                    context.Write("Items.MouseCocoa.FurItch3".Localized());

                    if (random.Next(10) < 8)
                    {
                        context.Write("Items.MouseCocoa.FurBrown".Localized());

                        skin.FurColor = "brown";
                    }
                    else
                    {
                        context.Write("Items.MouseCocoa.FurWhite".Localized());

                        skin.FurColor = "white";
                    }

                    context.Write("Items.MouseCocoa.FurItch4".Localized());

                    if (player.HasScales())
                        context.Write("Items.MouseCocoa.FurScalesPop".Localized());

                    context.Write("Items.MouseCocoa.FurItch5".Localized());
                }
                else
                {
                    context.Write("Items.MouseCocoa.FurReplace1".Localized());

                    if (random.Next(10) < 8)
                    {
                        context.Write("Items.MouseCocoa.FurBrown".Localized());

                        skin.FurColor = "brown";
                    }
                    else
                    {
                        context.Write("Items.MouseCocoa.FurWhite".Localized());

                        skin.FurColor = "white";
                    }

                    context.Write("Items.MouseCocoa.FurReplace2".Localized());
                }

                skin.Adj = "";

                skin.DescKey = "Skin.Desc.fur";

                skin.Id = new Identifier("coc", "fur");

                body.Get<UnderBodyPart>().Restore();

                mutations.Changes += 1;
            }

            player.RefillHunger(10);

            flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }
    }
}