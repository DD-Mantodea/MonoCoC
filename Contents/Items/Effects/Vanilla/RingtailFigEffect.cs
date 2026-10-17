using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 RingtailFig.hx ─────────────────────────────────────────────
    public class RingtailFigEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var mutations = Mutations.Mutations.Instance;

            var random = context.Random;

            var body = player.Body;

            mutations.InitTransformation([2, 3, 3]);

            context.Write("Items.RingtailFig.Eat".Localized());

            var spe100 = 100f * player.Speed / player.MaxValue(GameStats.Speed);

            var sens100 = 100f * player.Sensitivity / player.MaxValue(GameStats.Sensitivity);

            var tou100 = 100f * player.Toughness / player.MaxValue(GameStats.Toughness);

            if (spe100 < 80 && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.RingtailFig.Speed".Localized());

                if (spe100 < 40)
                    player.ApplyStats(new DynStat(GameStats.Speed, 1));

                player.ApplyStats(new DynStat(GameStats.Speed, 1));
            }

            if (sens100 < 80 && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.RingtailFig.Sensitivity".Localized());

                if (sens100 < 60)
                    player.ApplyStats(new DynStat(GameStats.Sensitivity, 2));

                player.ApplyStats(new DynStat(GameStats.Sensitivity, 2));
            }

            if (random.Next(4) == 0 && tou100 > 50 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.RingtailFig.ToughnessLoss".Localized());

                if (tou100 > 75)
                    player.ApplyStats(new DynStat(GameStats.Toughness, -1));

                player.ApplyStats(new DynStat(GameStats.Toughness, -1));
            }

            if (player.HasCock() && player.Balls > 0 && player.BallSize < 15
                && random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.RingtailFig.BallSize".Localized());

                player.BallSize += 2 + random.Next(3);

                player.ApplyStats(new DynStat(GameStats.Libido, 1));
            }

            if (player.Balls == 0 && player.HasFur() && body.Get<FacePart>().Id.Path == "raccoon"
                && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.RingtailFig.GrowBalls".Localized());

                player.Balls = 2;

                player.BallSize = 1;

                mutations.Changes += 1;
            }

            if (random.Next(4) == 0 && player.Thickness < 80 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write(player.ModThickness(80, 2));

                mutations.Changes += 1;
            }

            if (body.Get<NeckPart>().Id.Path != "normal" && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
                mutations.RestoreNeck("ringtailFig");

            if (body.Get<RearBodyPart>().Id != BodyPartTypes.SharkFin
                && mutations.Changes < mutations.ChangeLimit && random.Next(5) == 0)

                mutations.RestoreRearBody("ringtailFig");

            if (random.Next(5) == 0)
                mutations.UpdateOvipositionPerk("ringtailFig");

            var tail = body.Get<TailPart>();

            if (tail.Id.Path != "raccoon" && random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write(tail.Id.Path == "none"

                    ? "Items.RingtailFig.TailFromNone".Localized()

                    : "Items.RingtailFig.TailFromOther".Localized());

                tail.Id = new Identifier("coc", "raccoon");

                mutations.Changes += 1;
            }

            var ears = body.Get<EarsPart>();

            var lowerBody = body.Get<LowerBodyPart>();

            var skin = body.Get<SkinPart>();

            if (mutations.TfNoFur() && lowerBody.Id.Path == "raccoon" && ears.Id.Path == "raccoon"
                && !player.HasFur() && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                context.Write("Items.RingtailFig.GrowFur".Localized());

                skin.Id = new Identifier("coc", "fur");

                skin.Adj = "";

                skin.DescKey = "Skin.Desc.fur";

                skin.FurColor = "gray";

                body.Get<UnderBodyPart>().Restore();

                context.Write("Items.RingtailFig.FurCoat".Localized());

                mutations.Changes += 1;
            }

            if (tail.Id.Path == "raccoon" && ears.Id.Path != "raccoon" && random.Next(4) == 0
                && mutations.Changes < mutations.ChangeLimit)
            {
                if (ears.Id.Path is "dog" or "bunny" or "kangaroo")
                    context.Write("Items.RingtailFig.EarsFromLong".Localized());
                else if (ears.Id.Path is "horse" or "cow" or "cat")
                    context.Write("Items.RingtailFig.EarsFromShort".Localized());
                else
                    context.Write("Items.RingtailFig.EarsFromHuman".Localized());

                ears.Id = new Identifier("coc", "raccoon");

                mutations.Changes += 1;
            }

            if (ears.Id.Path == "raccoon" && lowerBody.Id.Path != "raccoon" && mutations.Changes < mutations.ChangeLimit
                && random.Next(4) == 0)
            {
                if (player.HasTailInsteadOfLegs())
                {
                    context.Write("Items.RingtailFig.LegsFromTail".Localized());

                    player.ApplyStats(new DynStat(GameStats.Lust, -30));

                    player.ChangeFatigue(5);
                }
                else if (player.IsGoo())
                    context.Write("Items.RingtailFig.LegsFromGoo".Localized());
                else if (lowerBody.Id.Path is "hoofed" or "bee" or "pony" or "chitinous_spider_legs" || player.IsTaur())
                {
                    context.Write("Items.RingtailFig.LegsFromHooves1".Localized());

                    if (lowerBody.Id.Path is "hoofed" or "pony")
                        context.Write("Items.RingtailFig.LegsFromHoovesHoof".Localized());
                    else
                        context.Write("Items.RingtailFig.LegsFromHoovesBoot".Localized());

                    context.Write("Items.RingtailFig.LegsFromHooves2".Localized());

                    if (player.IsTaur())
                        context.Write("Items.RingtailFig.LegsFromHoovesTaur".Localized());

                    context.Write("Items.RingtailFig.LegsFromHoovesEnd".Localized());
                }
                else
                {
                    context.Write("Items.RingtailFig.LegsFromHuman".Localized());

                    if (lowerBody.Id.Path is "human" or "demonic_high_heels" or "demonic_claws" && !player.HasFur())
                        context.Write("Items.RingtailFig.LegsFromHumanFur".Localized());

                    context.Write("Items.RingtailFig.LegsFromHumanEnd".Localized());
                }

                lowerBody.Id = new Identifier("coc", "raccoon");

                lowerBody.LegCount = 2;

                mutations.Changes += 1;
            }

            var face = body.Get<FacePart>();

            if (face.Id.Path != "raccoon_mask" && face.Id.Path != "raccoon"
                && random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                if (face.Id.Path is "human" or "shark_teeth" or "snake_fangs" or "bunny")
                {
                    context.Write("Items.RingtailFig.MaskFromHumanoid1".Localized());

                    if (face.Id.Path is "shark_teeth" or "snake_fangs" or "bunny")
                        context.Write("Items.RingtailFig.MaskFromHumanoidTeeth".Localized());

                    context.Write("Items.RingtailFig.MaskFromHumanoid2".Localized());

                    var inkyBody = ((skin.Tone is "ebony" or "black") && (player.HasPlainSkin() || player.IsGoo()))

                        || ((body.Get<HairPart>().Color is "black" or "midnight") && (player.HasFur() || player.HasScales()));

                    context.Write(inkyBody

                        ? "Items.RingtailFig.MaskBarelyVisible".Localized()

                        : "Items.RingtailFig.Mask".Localized());
                }
                else
                {
                    context.Write("Items.RingtailFig.MaskFromMuzzle1".Localized());

                    var inkyBody = (body.Get<HairPart>().Color is "black" or "midnight") && (player.HasFur() || player.HasScales());

                    if (inkyBody)
                        context.Write("Items.RingtailFig.MaskFromMuzzleBlackFur".Localized());
                    else if ((skin.Tone is "ebony" or "black") && (player.HasPlainSkin() || player.IsGoo()))
                        context.Write("Items.RingtailFig.MaskFromMuzzleBlackSkin".Localized());
                    else
                        context.Write("Items.RingtailFig.MaskFromMuzzle".Localized());
                }

                face.Id = new Identifier("coc", "raccoon_mask");

                mutations.Changes += 1;
            }
            else if (mutations.TfNoFur() && face.Id.Path == "raccoon_mask" && lowerBody.Id.Path == "raccoon"
                && player.HasFur() && random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.RingtailFig.FullFace".Localized());

                mutations.Changes += 1;

                face.Id = new Identifier("coc", "raccoon");
            }
            else if (random.Next(2) == 0 && mutations.Changes < mutations.ChangeLimit
                && face.Id.Path != "raccoon_mask" && face.Id.Path != "raccoon")
            {
                context.Write("Items.RingtailFig.Rings".Localized());

                player.ChangeFatigue(10);

                mutations.Changes += 1;
            }

            if (mutations.Changes == 0)
            {
                context.Write("Items.RingtailFig.NoEffect".Localized());

                player.ChangeFatigue(5);
            }

            player.RefillHunger(30);

            CoC.Instance.Flags.Set(GameFlagIds.TimesTransformed,

                CoC.Instance.Flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }
    }
}