using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 Taurinum.hx ────────────────────────────────────────────────
    public class TaurinumEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var mutations = Mutations.Mutations.Instance;

            var random = context.Random;

            var body = player.Body;

            mutations.InitTransformation([3]);

            player.SlimeFeed();

            context.Write("Items.Taurinum.Drink".Localized());

            var lowerBody = body.Get<LowerBodyPart>();

            var tail = body.Get<TailPart>();

            // 原版 isTaur() 等价于 legCount > 2
            var isTaur = lowerBody.LegCount > 2;

            if (mutations.Changes < mutations.ChangeLimit && random.Next(2) == 0
                && 100f * player.Speed / player.MaxValue(GameStats.Speed) < 80)
            {
                context.Write("Items.Taurinum.Faster".Localized());

                player.ApplyStats(new DynStat(GameStats.Speed, 1));
            }

            // 经典马半人马
            if (mutations.Changes < mutations.ChangeLimit && random.Next(2) == 0
                && lowerBody.Id.Path == "hoofed" && !isTaur)
            {
                mutations.Changes += 1;

                context.Write("Items.Taurinum.Centaur".Localized());

                if (player.Gender != Gender.Male)
                    context.Write("Items.Taurinum.CentaurGenitals".Localized());

                player.ApplyStats(new DynStat(GameStats.Speed, 3));

                lowerBody.LegCount = 4;

                if (tail.Id.Path == "none")
                    tail.Id = new Identifier("coc", "horse");
            }

            // 通用版本
            if ((lowerBody.Id.Path != "hoofed" && !isTaur) && (mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0))
                {
                    mutations.Changes += 1;

                    if (player.HasTailInsteadOfLegs())
                    {
                        context.Write("Items.Taurinum.FromTail".Localized());

                        lowerBody.Id = new Identifier("coc", "hoofed");

                        tail.Id = new Identifier("coc", "horse");
                    }
                    else
                    {
                        if (lowerBody.Id.Path == "human")
                        {
                            lowerBody.Id = new Identifier("coc", "hoofed");

                            if (tail.Id.Path == "none")
                                tail.Id = new Identifier("coc", "horse");
                        }

                        context.Write("Items.Taurinum.Generic".Localized());
                    }

                    if (player.Gender != Gender.Male)
                        context.Write("Items.Taurinum.GenericGenitals".Localized());

                    player.ApplyStats(new DynStat(GameStats.Speed, 3));

                    lowerBody.LegCount = 4;

                    // 原版这里多写了一次 changes++，会白吃一次变化额度，已修正

                }

            CoC.Instance.Flags.Set(GameFlagIds.TimesTransformed,

                CoC.Instance.Flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }
    }
}