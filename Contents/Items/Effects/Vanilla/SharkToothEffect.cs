using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Attributes;
using System;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    [Register<SharkToothEffect>("coc:shark_tooth")]
    public class SharkToothEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var random = context.Random;

            var changes = 0;

            const int changeLimit = 4;

            context.Write("Common.Text.YouHaveNoIdeaWhyBut".Localized());

            if (player.Strength < 60 && random.Next(3) == 0)
            {
                player.Strength = Math.Max(0, player.Strength + 1 + random.Next(2));

                context.Write("Common.Text.APainfulRipplePassesThroughThe".Localized());
            }

            if (player.Speed < 100 && random.Next(3) == 0)
            {
                player.Speed = Math.Max(0, player.Speed + 1 + random.Next(3));

                context.Write("Common.Text.ShiveringWithoutWarningYouNearlyTrip".Localized());
            }

            if (player.Sensitivity > 25 && random.Next(2) == 0 && changes < changeLimit)
            {
                player.Sensitivity = Math.Max(0, player.Sensitivity - 1 - random.Next(3));

                context.Write("Common.Text.ItTakesAWhileButYou".Localized());
            }

            if (player.Libido < 100 && random.Next(3) == 0 && changes < changeLimit)
            {
                player.Libido = Math.Min(100, player.Libido + 1 + random.Next(3));

                context.Write("Common.Text.ABlushWorksItsWayAcross".Localized());
            }

            if (player.Intelligence > 40 && random.Next(3) == 0 && changes < changeLimit)
            {
                player.Intelligence = Math.Max(0, player.Intelligence - 1 - random.Next(3));

                context.Write("Common.Text.YouShakeYourHeadAndStruggle".Localized());
            }

            if (!player.HasVagina() && (player.Gender == 0 || (changes < changeLimit && random.Next(3) == 0)))
            {
                if (player.Balls > 0)
                    context.Write("Common.Text.AnItchStartsBehindYourBalls".Localized());
                else if (player.HasCock())
                    context.Write("Common.Text.AnItchStartsOnYourGroin".Localized());
                else
                    context.Write("Common.Text.AnItchStartsOnYourGroin2".Localized());

                player.CreateVagina();

                player.SetClitLength(0.25f);

                player.Sensitivity = Math.Min(100, player.Sensitivity + 10);

                changes++;
            }

            if (!player.HasCock() && changes < changeLimit && random.Next(3) == 0)
            {
                context.Write("Common.Text.AStabbingPainHitsYourFeatureless".Localized());

                if (player.Balls == 0)
                {
                    player.Balls = 2;

                    player.BallSize = 2;

                    context.Write("Common.Text.AndAPairOfBalls".Localized());
                }
                else
                    context.Write("!");

                player.CreateCock(7, 1.4f);

                player.Libido = Math.Min(100, player.Libido + 4);

                player.Sensitivity = Math.Min(100, player.Sensitivity + 5);

                player.Lust = Math.Min(100, player.Lust + 20);

                changes++;
            }

            if ((player.Balls == 0 || player.Balls == 2) && player.HasCock() && changes < changeLimit && random.Next(3) == 0)
            {
                if (player.Balls == 2)
                {
                    player.Balls = 4;

                    context.Write("Common.Text.ASuddenPainRacksYourAbdomen".Localized());
                }
                else
                {
                    player.Balls = 2;

                    player.BallSize = 2;

                    context.Write("Common.Text.ASuddenPainRacksYourAbdomen2".Localized());
                }

                player.Libido = Math.Min(100, player.Libido + 2);

                player.Sensitivity = Math.Min(100, player.Sensitivity + 3);

                player.Lust = Math.Min(100, player.Lust + 10);

                changes++;
            }

            if (changes < changeLimit && player.BreastRows.Count > 1 && random.Next(3) == 0)
            {
                player.RemoveBreastRow(0, 1);

                context.Write("Common.Text.YourExtraBreastRowTinglesAnd".Localized());

                changes++;
            }

            var face = player.Body.Get<FacePart>();

            if (face.Id != BodyPartTypes.SharkTeeth && random.Next(3) == 0 && changes < changeLimit)
            {
                face.Id = BodyPartTypes.SharkTeeth;

                context.Write("Common.Text.YourMouthAchesAsYourGums".Localized());

                changes++;
            }

            var eyes = player.Body.Get<EyesPart>();

            if (changes < changeLimit && random.Next(5) == 0 && eyes.Id != BodyPartTypes.Human)
            {
                eyes.Id = BodyPartTypes.Human;

                eyes.Count = 2;

                context.Write("Common.Text.YouBlinkAndStumbleAsVertigo".Localized());

                changes++;
            }

            var tail = player.Body.Get<TailPart>();

            if (tail.Id != BodyPartTypes.Shark && random.Next(3) == 0 && changes < changeLimit)
            {
                var hadTail = tail.Id != BodyPartTypes.None;

                tail.Id = BodyPartTypes.Shark;

                context.Write(!hadTail

                    ? "Jets of pain shoot down your spine. A fully formed shark tail bursts through your back; you swish it around, surprised by how flexible it is."

                    : "Jets of pain shoot down your spine into your tail. It bulges out until it becomes a large, flexible shark tail.");

                changes++;
            }

            var gills = player.Body.Get<GillsPart>();

            if (gills.Id != BodyPartTypes.Fish && tail.Id == BodyPartTypes.Shark && face.Id == BodyPartTypes.SharkTeeth && changes < changeLimit && random.Next(3) == 0)
            {
                gills.Id = BodyPartTypes.Fish;

                context.Write("Common.Text.YourNeckTinglesAndYouFeel".Localized());

                changes++;
            }

            var hair = player.Body.Get<HairPart>();

            if (hair.Color != "silver" && random.Next(4) == 0 && changes < changeLimit)
            {
                hair.Color = "silver";

                context.Write("Common.Text.ATinglingSpreadsAcrossYourScalp".Localized());

                changes++;
            }

            var skin = player.Body.Get<SkinPart>();

            if (skin.Tone != "orange and black striped" && random.Next(7) == 0 && changes < changeLimit)
            {
                skin.Id = BodyPartTypes.Plain;

                skin.Tone = "orange and black striped";

                player.Body.Get<UnderBodyPart>().Restore();

                context.Write("Common.Text.YourSkinTinglesAndItchesBefore".Localized());

                changes++;
            }

            var wings = player.Body.Get<WingsPart>();

            var rearBody = player.Body.Get<RearBodyPart>();

            if ((wings.Id != BodyPartTypes.None || rearBody.Id != BodyPartTypes.SharkFin) && changes < changeLimit && random.Next(3) == 0)
            {
                wings.Restore();

                rearBody.Id = BodyPartTypes.SharkFin;

                context.Write("Common.Text.YourWingsFoldIntoYourBack".Localized());

                changes++;
            }

            if (changes == 0)
                context.Write("Common.Text.NothingHappenedWeird".Localized());

            player.RefillHunger(5, true, false);

            if (changes > 0)
                CoC.Instance.Flags.Set(GameFlagIds.TimesTransformed, CoC.Instance.Flags.Get(GameFlagIds.TimesTransformed) + changes);
        }
    }
}