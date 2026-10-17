using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 SnakeOil.hx ────────────────────────────────────────────────
    public class SnakeOilEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var mutations = Mutations.Mutations.Instance;

            var random = context.Random;

            var flags = CoC.Instance.Flags;

            player.SlimeFeed();

            context.Write("Items.SnakeOil.Drink".Localized());

            mutations.InitTransformation([2, 2]);

            var inCombat = CombatController.Instance?.Context != null;

            if (!inCombat)
                context.Write("Items.SnakeOil.Aftertaste".Localized());

            if (100f * player.Speed / player.MaxValue(GameStats.Speed) < 70 && random.Next(2) == 0)
            {
                player.ApplyStats(new DynStat(GameStats.Speed, 2 - player.Speed / 10 / 5));

                context.Write("Items.SnakeOil.Faster".Localized());

                if (100f * player.Speed / player.MaxValue(GameStats.Speed) < 40)
                    context.Write("Items.SnakeOil.NotThatFast".Localized());
            }

            if (player.Body.Get<NeckPart>().Id.Path != "normal" && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
                mutations.RestoreNeck("snakeOil");

            if (player.Body.Get<RearBodyPart>().Id != BodyPartTypes.SharkFin
                && mutations.Changes < mutations.ChangeLimit && random.Next(5) == 0)

                mutations.RestoreRearBody("snakeOil");

            if (random.Next(5) == 0)
                mutations.UpdateOvipositionPerk("snakeOil");

            if (mutations.Changes < mutations.ChangeLimit && player.Body.GetList<BreastRowPart>().Count > 1 && random.Next(3) == 0)
                mutations.RemoveExtraBreastRow("snakeOil");

            var wings = player.Body.Get<WingsPart>();

            var rearBody = player.Body.Get<RearBodyPart>();

            if ((wings.Id != BodyPartTypes.None || rearBody.Id == BodyPartTypes.SharkFin)
                && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                if (rearBody.Id == BodyPartTypes.SharkFin)
                {
                    context.Write("Items.SnakeOil.FinGone".Localized());

                    rearBody.Restore();
                }
                else
                    context.Write("Items.SnakeOil.WingsGone".Localized());

                wings.Id = BodyPartTypes.None;

                mutations.Changes += 1;
            }

            if (player.Body.Get<AntennaePart>().Id.Path != "none" && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
                mutations.RemoveAntennae();

            if (mutations.Changes == 0 && random.Next(3) == 0)
                mutations.GainSnakeTongue();

            var tongue = player.Body.Get<TonguePart>();

            var face = player.Body.Get<FacePart>();

            if (mutations.Changes == 0 && tongue.Id.Path == "snake" && face.Id.Path != "snake_fangs"
                && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.SnakeOil.Fangs".Localized());

                if (face.Id.Path != "human" && face.Id.Path != "shark_teeth" && face.Id.Path != "bunny"
                    && face.Id.Path != "spider_fangs")

                    context.Write("Items.SnakeOil.FaceHuman".Localized());

                face.Id = new Identifier("coc", "snake_fangs");

                mutations.Changes += 1;
            }

            var lowerBody = player.Body.Get<LowerBodyPart>();

            if (mutations.Changes == 0 && face.Id.Path == "snake_fangs" && lowerBody.Id.Path != "naga"
                && random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.SnakeOil.LegsMerge1".Localized());

                context.Write("Items.SnakeOil.LegsMerge2".Localized());

                context.Write("Items.SnakeOil.LegsMerge3".Localized());

                if (player.Balls > 0 && player.BallSize > 10)
                    context.Write("Items.SnakeOil.NoBalls".Localized());

                context.Write("Items.SnakeOil.LegsMerge4".Localized());

                lowerBody.Id = new Identifier("coc", "naga");

                lowerBody.LegCount = 1;

                var tail = player.Body.Get<TailPart>();

                tail.Id = BodyPartTypes.None;

                tail.Venom = 0;

                tail.Recharge = 0;

                if (player.Body.Get<UnderBodyPart>().Skin.HasScales)
                {
                    player.Body.Get<SkinPart>().Tone = "green";

                    player.Body.Get<UnderBodyPart>().Skin.Tone = "light green";
                }
                else
                {
                    var underBody = player.Body.Get<UnderBodyPart>();

                    underBody.Id = new Identifier("coc", "naga");

                    underBody.Skin.Id = new Identifier("coc", "lizard_scales");

                    underBody.Skin.Tone = "green";

                    underBody.Skin.Adj = "";

                    underBody.Skin.DescKey = "Skin.Desc.Scales";
                }

                var arms = player.Body.Get<ArmsPart>();

                arms.UpdateClaws(arms.Claws.Id);

                mutations.Changes += 1;
            }

            if (random.Next(4) == 0 && player.HasGills() && mutations.Changes < mutations.ChangeLimit)
                mutations.UpdateGills();

            if (mutations.Changes == 0)
                context.Write("Items.SnakeOil.NoEffect".Localized());

            player.RefillHunger(5);

            flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }
    }
}