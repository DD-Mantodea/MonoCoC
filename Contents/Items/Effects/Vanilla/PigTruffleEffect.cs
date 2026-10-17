using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System.Linq;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    /// <summary>
    /// 原版 items/consumables/PigTruffle.hx:13-150 —— 小猪块菌 / 野猪块菌（猪化）。
    /// 两个物品共用同一套逻辑，boar 版本概率更高、皮肤色表不同、且能把猪脸变野猪脸。
    /// </summary>
    public class PigTruffleEffect(bool boar) : ConsumableEffect
    {
        private const string TfSource = "pigTruffle";

        private static readonly string[] BoarTones = ["dark brown", "brown", "brown"];

        private static readonly string[] PigTones = ["pink", "tan", "sable"];

        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var random = context.Random;

            var mutations = Mutations.Mutations.Instance;

            var source = boar ? TfSource + "-boar" : TfSource;

            // 原版 :24 initTransformation([2, 2, 3], boar ? 2 : 1)
            mutations.InitTransformation([2, 2, 3], boar ? 2 : 1);

            context.Write("Items.PigtailTruffle.Use".Localized());

            player.RefillHunger(20);

            var changes = mutations.Changes;

            // 原版 :31 体型变胖
            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && player.Thickness < 75)
            {
                context.Write(player.ModThickness(75, 3));

                mutations.Changes += 1;
            }

            // 原版 :36 肌肉减少（女性或雌雄同体）
            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                && (player.Gender == Gender.Female || player.Gender == Gender.Hermaphrodite)

                && player.Tone > 20)
            {
                context.Write(player.ModTone(20, 4));

                mutations.Changes += 1;
            }

            // 原版 :41 髋部变宽
            var hips = player.Body.Get<HipsPart>();

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                && (player.Gender == Gender.Female || player.Gender == Gender.Hermaphrodite)

                && hips.Rating < 15)
            {
                context.Write("Items.PigtailTruffle.Hips".Localized());

                hips.Rating += 1;

                mutations.Changes += 1;
            }

            // 原版 :47 臀部变大
            var butt = player.Body.Get<ButtPart>();

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && butt.Rating < 12)
            {
                context.Write("Items.PigtailTruffle.Butt".Localized());

                butt.Rating += 1;

                mutations.Changes += 1;
            }

            // 原版 :53 蛋蛋变大
            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                && player.Balls > 0 && player.BallSize < 4)
            {
                context.Write(player.BallSize < 3

                    ? "Items.PigtailTruffle.BallsGrow".Localized()

                    : "Items.PigtailTruffle.BallsGrow2".Localized());

                player.BallSize += 1;

                mutations.Changes += 1;
            }

            var neck = player.Body.Get<NeckPart>();

            var rearBody = player.Body.Get<RearBodyPart>();

            // 原版 :66 颈部还原
            if (neck.Id != BodyPartTypes.Normal && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
                mutations.RestoreNeck(source);

            // 原版 :70 后体还原（非鲨鱼鳍）
            if (rearBody.Id != BodyPartTypes.None
                && rearBody.Id != BodyPartTypes.SharkFin

                && mutations.Changes < mutations.ChangeLimit

                && random.Next(5) == 0)

                mutations.RestoreRearBody(source);

            // 原版 :74 产卵 perk 流失
            if (random.Next(5) == 0)
                mutations.UpdateOvipositionPerk(source);

            // ── 原版 :78 猪屌 ──────────────────────────────────────────────
            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit
                && player.Cocks.Any(cock => cock.Id != BodyPartTypes.Pig))
            {
                if (player.Cocks.Count == 1)
                {
                    context.Write("Items.PigtailTruffle.CockSingle".Localized(
                        player.ClothedOrNakedLower("You pull open your [armor]", "You look down at your exposed groin")));

                    player.Cocks[0].Id = BodyPartTypes.Pig;
                }
                else
                {
                    context.Write("Items.PigtailTruffle.CockMulti".Localized());

                    for (var index = 0; index < player.Cocks.Count; index++)
                        if (player.Cocks[index].Id != BodyPartTypes.Pig)
                        {
                            player.Cocks[index].Id = BodyPartTypes.Pig;

                            break;
                        }
                }

                mutations.Changes += 1;
            }

            var ears = player.Body.Get<EarsPart>();

            // 原版 :95 猪耳
            if (random.Next(boar ? 3 : 4) == 0 && mutations.Changes < mutations.ChangeLimit
                && ears.Id != BodyPartTypes.Pig)
            {
                context.Write("Items.PigtailTruffle.Ears".Localized());

                ears.Id = BodyPartTypes.Pig;

                mutations.Changes += 1;
            }

            var tail = player.Body.Get<TailPart>();

            // 原版 :100 猪尾（需要猪耳）
            if (random.Next(boar ? 2 : 3) == 0 && mutations.Changes < mutations.ChangeLimit
                && ears.Id == BodyPartTypes.Pig

                && tail.Id != BodyPartTypes.Pig)
            {
                context.Write(tail.Id != BodyPartTypes.None

                    ? "Items.PigtailTruffle.TailMorph".Localized()

                    : "Items.PigtailTruffle.TailNew".Localized());

                tail.Id = BodyPartTypes.Pig;

                mutations.Changes += 1;
            }

            var lowerBody = player.Body.Get<LowerBodyPart>();

            // 原版 :113 半人马也能长猪尾（蹄类/矮种马下体）
            if (random.Next(boar ? 2 : 3) == 0 && mutations.Changes < mutations.ChangeLimit
                && ears.Id == BodyPartTypes.Pig

                && tail.Id != BodyPartTypes.Pig

                && player.IsTaur()

                && (lowerBody.Id == BodyPartTypes.Hoofed || lowerBody.Id == BodyPartTypes.Pony))
            {
                context.Write("Items.PigtailTruffle.TailTaur".Localized());

                tail.Id = BodyPartTypes.Pig;

                mutations.Changes += 1;
            }

            // 原版 :119 猪蹄（需要猪耳+猪尾）
            if (random.Next(boar ? 3 : 4) == 0 && mutations.Changes < mutations.ChangeLimit
                && ears.Id == BodyPartTypes.Pig

                && tail.Id == BodyPartTypes.Pig

                && lowerBody.Id != BodyPartTypes.ClovenHoofed)
            {
                if (player.IsTaur())
                    context.Write("Items.PigtailTruffle.LegsTaur".Localized());
                else if (player.HasTailInsteadOfLegs())
                    context.Write("Items.PigtailTruffle.LegsNaga".Localized());
                else
                    context.Write("Items.PigtailTruffle.Legs".Localized());

                lowerBody.Id = BodyPartTypes.ClovenHoofed;

                lowerBody.LegCount = 2;

                mutations.Changes += 1;
            }

            var face = player.Body.Get<FacePart>();

            // 原版 :137 猪脸（需要前三个猪化特征）
            if (mutations.TfNoFur() && random.Next(boar ? 2 : 3) == 0 && mutations.Changes < mutations.ChangeLimit
                && ears.Id == BodyPartTypes.Pig

                && tail.Id == BodyPartTypes.Pig

                && lowerBody.Id == BodyPartTypes.ClovenHoofed

                && face.Id != BodyPartTypes.Pig

                && face.Id != BodyPartTypes.Boar)
            {
                context.Write("Items.PigtailTruffle.Face".Localized());

                face.Id = BodyPartTypes.Pig;

                mutations.Changes += 1;
            }

            // 原版 :143 猪脸 → 野猪脸
            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                && ears.Id == BodyPartTypes.Pig

                && tail.Id == BodyPartTypes.Pig

                && lowerBody.Id == BodyPartTypes.ClovenHoofed

                && face.Id == BodyPartTypes.Pig)
            {
                context.Write("Items.PigtailTruffle.BoarFace".Localized());

                face.Id = BodyPartTypes.Boar;

                mutations.Changes += 1;
            }

            // 原版 :149 肤色
            if (random.Next(boar ? 3 : 4) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                var tones = boar ? BoarTones : PigTones;

                var tone = tones[random.Next(tones.Length)];

                context.Write("Items.PigtailTruffle.SkinColor".Localized(tone));

                player.Body.Get<SkinPart>().Tone = tone;

                player.Body.Get<ArmsPart>().UpdateClaws(player.Body.Get<ArmsPart>().Claws.Id);

                mutations.Changes += 1;
            }

            // 原版 :157 什么都没变
            if (mutations.Changes == changes)
                context.Write("Items.PigtailTruffle.Failsafe".Localized());

            // 原版 :161 flags[KFLAGS.TIMES_TRANSFORMED] += changes
            var flags = CoC.Instance.Flags;

            flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }
    }
}