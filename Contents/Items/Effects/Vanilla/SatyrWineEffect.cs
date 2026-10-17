using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 SatyrWine.hx ───────────────────────────────────────────────
    public class SatyrWineEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var mutations = Mutations.Mutations.Instance;

            var random = context.Random;

            var body = player.Body;

            context.Write("Items.SatyrWine.Drink".Localized());

            player.RefillHunger(10);

            mutations.InitTransformation(null, 3);

            if (random.Next(2) == 0)
            {
                context.Write("Items.SatyrWine.HeatLoins".Localized());

                player.ApplyStats(new DynStat(GameStats.Lust, 25));

                if (player.LibidoPercent < 100)
                {
                    if (player.LibidoPercent < 50)
                        player.ApplyStats(new DynStat(GameStats.Libido, 1));

                    player.ApplyStats(new DynStat(GameStats.Libido, 1));
                }
            }

            var cocks = body.GetList<CockPart>();

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && player.HasCock())
            {
                var index = player.SmallestCockIndex();

                if (cocks[index].Length < 12)
                {
                    context.Write("Items.SatyrWine.CockLonger".Localized());

                    cocks[index].Length += 1;

                    mutations.Changes += 1;
                }
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && player.HasCock())
            {
                var index = player.SmallestCockIndex();

                if (cocks[index].Thickness < 4)
                {
                    context.Write("Items.SatyrWine.CockThicker".Localized());

                    cocks[index].Thicken(0.5f);

                    mutations.Changes += 1;
                }
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && player.Balls > 0)
            {
                context.Write("Items.SatyrWine.MasculineFace".Localized());

                player.ApplyStats(new DynStat(GameStats.Lust, 20));

                if (player.CumMultiplier < 10)
                    player.CumMultiplier += 1;

                if (player.CumMultiplier < 50)
                    player.CumMultiplier += 0.5f;

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && player.HasVagina()
                && player.HasStatusEffect(StatusEffectIds.BonusVCapacity))
            {
                context.Write("Items.SatyrWine.VaginaTingle".Localized());

                var capacity = player.StatusEffectValue(StatusEffectIds.BonusVCapacity, 0);

                if (capacity >= 0)
                {
                    player.ChangeStatusValue(StatusEffectIds.BonusVCapacity, 0, -(random.Next(5) + 5));

                    if (player.StatusEffectValue(StatusEffectIds.BonusVCapacity, 0) <= 0)
                        player.RemoveStatusEffect(StatusEffectIds.BonusVCapacity);
                }

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && player.HasVagina() && !player.HasCock())
            {
                context.Write("Items.SatyrWine.VaginaToCock".Localized());

                player.RemoveVagina();

                player.CreateCock(6, 1, new Identifier("coc", "human"));

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && player.HasCock() && player.Balls <= 0)
            {
                context.Write("Items.SatyrWine.GrowBalls".Localized(
                    player.ClothedOrNaked("into your [armor]", "all over the ground"),

                    player.ClothedOrNakedLower("remove your [armor] to ")));

                player.Balls = 2;

                player.BallSize = 1;

                player.Orgasm("Generic");

                mutations.Changes += 1;
            }

            if (body.Get<NeckPart>().Id.Path != "normal" && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
                mutations.RestoreNeck("satyrTFs");

            if (body.Get<RearBodyPart>().Id != BodyPartTypes.SharkFin
                && mutations.Changes < mutations.ChangeLimit && random.Next(5) == 0)

                mutations.RestoreRearBody("satyrTFs");

            if (random.Next(5) == 0)
                mutations.UpdateOvipositionPerk("satyrTFs");

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && player.HasScales())
            {
                context.Write("Items.SatyrWine.HumanSkin".Localized());

                body.Get<SkinPart>().Id = new Identifier("coc", "plain");

                body.Get<UnderBodyPart>().Restore();

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && body.Get<ArmsPart>().Id.Path != "human")
            {
                context.Write("Items.SatyrWine.HumanArms".Localized());

                body.Get<ArmsPart>().Id = new Identifier("coc", "human");

                mutations.Changes += 1;
            }

            var lowerBody = body.Get<LowerBodyPart>();

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit && lowerBody.Id.Path != "cloven_hoofed")
            {
                context.Write("Items.SatyrWine.GoatLegs".Localized());

                lowerBody.Id = new Identifier("coc", "cloven_hoofed");

                lowerBody.LegCount = 2;

                mutations.Changes += 1;
            }

            var horns = body.Get<HornsPart>();

            var face = body.Get<FacePart>();

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                && lowerBody.Id.Path == "cloven_hoofed" && horns.Id.Path == "goat" && face.Id.Path != "human")
            {
                context.Write("Items.SatyrWine.HumanFace".Localized());

                face.Id = new Identifier("coc", "human");

                mutations.Changes += 1;
            }

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit && !player.HasScales()
                && body.Get<EarsPart>().Id.Path != "elfin")
            {
                context.Write("Items.SatyrWine.ElfinEars".Localized());

                body.Get<EarsPart>().Id = new Identifier("coc", "elfin");

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && horns.Id.Path == "none")
            {
                context.Write("Items.SatyrWine.GoatHorns".Localized());

                horns.Id = new Identifier("coc", "goat");

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && horns.Id.Path != "goat")
            {
                context.Write("Items.SatyrWine.GoatHornsReshape".Localized());

                horns.Value = 1;

                horns.Id = new Identifier("coc", "goat");

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && horns.Id.Path == "goat" && horns.Value == 1)
            {
                context.Write("Items.SatyrWine.GoatHornsGrow".Localized());

                horns.Value = 2;

                mutations.Changes += 1;
            }

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit && body.Get<AntennaePart>().Id.Path != "none")
                mutations.RemoveAntennae();

            var humanCocks = player.CountCocksOfType("Human");

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && cocks.Count == 1 && humanCocks == 0)
            {
                context.Write("Items.SatyrWine.HumanCockOne".Localized(
                    player.ClothedOrNakedLower("pull it out from your [armor], to ")));

                cocks[0].Id = new Identifier("coc", "human");

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && cocks.Count > 1 && cocks.Count - humanCocks > 0)
            {
                context.Write("Items.SatyrWine.HumanCockMany".Localized(
                    player.ClothedOrNakedLower("pull it out from your [armor], releasing", "notice")));

                foreach (var cock in cocks)
                    if (cock.Id.Path != "human")
                    {
                        cock.Id = new Identifier("coc", "human");

                        break;
                    }

                mutations.Changes += 1;
            }

            var tail = body.Get<TailPart>();

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && tail.Id.Path == "none")
            {
                context.Write("Items.SatyrWine.GoatTail".Localized());

                tail.Id = new Identifier("coc", "goat");

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && tail.Id.Path != "none" && tail.Id.Path != "goat")
            {
                context.Write("Items.SatyrWine.GoatTailReshape".Localized());

                tail.Id = new Identifier("coc", "goat");

                mutations.Changes += 1;
            }

            if (mutations.Changes == 0)
                context.Write("Items.SatyrWine.NoEffect".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TimesTransformed,

                CoC.Instance.Flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }
    }
}