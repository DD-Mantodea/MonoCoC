using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Configuration;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 EchidnaCake.hx（针鼹转化）──────────────────────────────────
    public class EchidnaCakeEffect : ConsumableEffect
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

            var hair = body.Get<HairPart>();

            var arms = body.Get<ArmsPart>();

            var eyes = body.Get<EyesPart>();

            var ears = body.Get<EarsPart>();

            var tail = body.Get<TailPart>();

            var lowerBody = body.Get<LowerBodyPart>();

            var tongue = body.Get<TonguePart>();

            var face = body.Get<FacePart>();

            var skin = body.Get<SkinPart>();

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit
                && hair.Id.Path != "normal" && hair.Id.Path != "quill")
            {
                context.Write("Items.EchidnaCake.HairNormal".Localized());

                hair.Id = new Identifier("coc", "normal");

                mutations.Changes += 1;
            }

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit && arms.Id.Path == "harpy")
            {
                context.Write("Items.EchidnaCake.ArmsFeathers".Localized());

                arms.Id = new Identifier("coc", "human");

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && player.HasGills())
                mutations.UpdateGills();

            if ((random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                 && eyes.Id.Path == "four_spider_eyes") || eyes.Id.Path == "spider")
            {
                context.Write("Items.EchidnaCake.Eyes".Localized());

                eyes.Id = new Identifier("coc", "human");

                eyes.Count = 2;

                mutations.Changes += 1;
            }

            var rows = body.GetList<BreastRowPart>();

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && player.AverageNipplesPerBreast() > 4)
            {
                context.Write("Items.EchidnaCake.Nipples".Localized());

                foreach (var row in rows)
                    row.NipplesPerBreast = 1;

                mutations.Changes += 1;
            }

            if (mutations.TfNoFur() && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && !player.HasFur())
            {
                context.Write("Items.EchidnaCake.GrowFur".Localized());

                skin.Adj = "";

                skin.DescKey = "Skin.Desc.fur";

                skin.Id = new Identifier("coc", "fur");

                skin.FurColor = "brown";

                body.Get<UnderBodyPart>().Restore();

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit && ears.Id.Path != "echidna")
            {
                context.Write("Items.EchidnaCake.Ears1".Localized());

                context.Write(ears.Id.Path == "lizard"

                    ? "Items.EchidnaCake.EarsLizard".Localized()

                    : "Items.EchidnaCake.EarsDefault".Localized());

                context.Write("Items.EchidnaCake.Ears2".Localized());

                ears.Id = new Identifier("coc", "echidna");

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                && ears.Id.Path == "echidna" && tail.Id.Path != "echidna")
            {
                context.Write("Items.EchidnaCake.Tail1".Localized());

                var tailKey = tail.Id.Path switch
                {
                    "none" => "Items.EchidnaCake.TailNone",

                    "harpy" => "Items.EchidnaCake.TailHarpy",

                    "rabbit" => "Items.EchidnaCake.TailRabbit",

                    "cow" => "Items.EchidnaCake.TailCow",

                    "cat" => "Items.EchidnaCake.TailCat",

                    "dog" => "Items.EchidnaCake.TailDog",

                    "kangaroo" => "Items.EchidnaCake.TailKangaroo",

                    "lizard" or "draconic" => "Items.EchidnaCake.TailLizard",

                    "fox" => "Items.EchidnaCake.TailFox",

                    "raccoon" => "Items.EchidnaCake.TailRaccoon",

                    "horse" => player.IsTaur()

                        ? "Items.EchidnaCake.TailHorseTaur"

                        : "Items.EchidnaCake.TailHorse",

                    "mouse" => "Items.EchidnaCake.TailMouse",

                    "ferret" => "Items.EchidnaCake.TailFerret",

                    "rhino" => "Items.EchidnaCake.TailRhino",

                    _ => "Items.EchidnaCake.TailDefault",
                };

                context.Write(tailKey.Localized());

                context.Write("Items.EchidnaCake.Tail2".Localized());

                tail.Id = new Identifier("coc", "echidna");

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                && ears.Id.Path == "echidna" && tail.Id.Path == "echidna"

                && lowerBody.Id.Path != "echidna")
            {
                context.Write("Items.EchidnaCake.Legs1".Localized());

                var legsKey = lowerBody.Id.Path switch
                {
                    "naga" or "mermaid" => "Items.EchidnaCake.LegsNaga",

                    "goo" => "Items.EchidnaCake.LegsGoo",

                    "human" => "Items.EchidnaCake.LegsHuman",

                    "hoofed" or "cloven_hoofed" => "Items.EchidnaCake.LegsHoofed",

                    "harpy" or "demonic_claws" or "demonic_high_heels" or "lizard" or "dragon" => "Items.EchidnaCake.LegsClaws",

                    "cat" or "dog" or "fox" or "kangaroo" or "bunny" or "raccoon" or "ferret" => "Items.EchidnaCake.LegsPaws",

                    _ => "Items.EchidnaCake.LegsDefault",
                };

                context.Write(legsKey.Localized());

                context.Write("Items.EchidnaCake.Legs2".Localized());

                lowerBody.Id = new Identifier("coc", "echidna");

                lowerBody.LegCount = 2;

                mutations.Changes += 1;
            }

            var cocks = body.GetList<CockPart>();

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                && player.HasCock() && player.CountCocksOfType("Echidna") < cocks.Count)
            {
                context.Write("Items.EchidnaCake.Cock1".Localized());

                context.Write(cocks.Count == 1

                    ? "Items.EchidnaCake.CockSingle".Localized(player.ClothedOrNakedLower("pull it out from your [armor], right in the middle of the food tent, watching", "watch"))

                    : "Items.EchidnaCake.CockMany".Localized(player.ClothedOrNakedLower("pull the offending cock out from your [armor], right in the middle of the food tent, watching", "watch")));

                var hasSheath = false;

                foreach (var cock in cocks)
                    if (cock.SheathOn)
                    {
                        hasSheath = true;

                        break;
                    }

                if (!hasSheath)
                    context.Write("Items.EchidnaCake.CockSheath".Localized());

                context.Write("Items.EchidnaCake.CockEnd".Localized());

                foreach (var cock in cocks)
                    if (cock.Id.Path != "echidna")
                    {
                        cock.Id = new Identifier("coc", "echidna");

                        cock.SheathOn = true;

                        break;
                    }

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                && player.EchidnaScore() >= 2 && tongue.Id.Path != "echidna")
            {
                context.Write("Items.EchidnaCake.Tongue".Localized());

                tongue.Id = new Identifier("coc", "echidna");

                mutations.Changes += 1;
            }

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit
                && flags.Get(GameFlagIds.HairGrowthStoppedBecauseLizard) == 1 && hair.Id.Path == "normal")
            {
                context.Write("Items.EchidnaCake.Quills".Localized());

                hair.Id = new Identifier("coc", "quill");

                mutations.Changes += 1;
            }

            if (mutations.TfNoFur() && random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit
                && player.HasFur() && ears.Id.Path == "echidna" && tail.Id.Path == "echidna"

                && tongue.Id.Path == "echidna")
            {
                context.Write("Items.EchidnaCake.Face".Localized());

                face.Id = new Identifier("coc", "echidna");

                mutations.Changes += 1;
            }

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit
                && player.EchidnaScore() >= 2 && flags.Get(GameFlagIds.HairGrowthStoppedBecauseLizard) == 0)
            {
                context.Write("Items.EchidnaCake.HairStops1".Localized());

                context.Write("Items.EchidnaCake.HairStops2".Localized());

                mutations.Changes += 1;

                flags.Set(GameFlagIds.HairGrowthStoppedBecauseLizard,

                    flags.Get(GameFlagIds.HairGrowthStoppedBecauseLizard) + 1);
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                && player.HasCock() && player.CumMultiplier < 25)
            {
                var temp = 1 + random.Next(4);

                if (player.HasPerk(PerkIds.MessyOrgasms))
                    temp += random.Next(10);

                temp /= 10;

                player.CumMultiplier += temp;

                if (player.Balls == 0)
                    context.Write("Items.EchidnaCake.CumBody".Localized());
                else
                    context.Write("Items.EchidnaCake.CumBalls".Localized());

                context.Write("Items.EchidnaCake.CumPre".Localized());

                mutations.Changes += 1;
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit
                && player.Gender == Gender.Male && AverageBreastSize(player) > 2

                && !(OptionStore.Instance.Get("hyper", 0) > 0))
            {
                context.Write("Items.EchidnaCake.ShrinkTits".Localized());

                player.ShrinkTits();

                mutations.Changes += 1;
            }

            if (body.Get<NeckPart>().Id.Path != "normal" && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
                mutations.RestoreNeck("echidnaTFs");

            if (body.Get<RearBodyPart>().Id != BodyPartTypes.SharkFin
                && mutations.Changes < mutations.ChangeLimit && random.Next(5) == 0)

                mutations.RestoreRearBody("echidnaTFs");

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit
                && player.EchidnaScore() >= 3 && player.HasVagina() && !player.HasPerk(PerkIds.Oviposition))

                mutations.UpdateOvipositionPerk("echidnaTFs");

            if (random.Next(3) == 0 && (random.Next(2) == 0 || !player.InHeat)
                && player.HasVagina() && player.StatusEffectValue(StatusEffectIds.Heat, 1) < 30)

                player.GoIntoHeat(true);

            if (random.Next(2) == 0 && player.Thickness < 90)
                player.ModThickness(90, 2);

            if (random.NextDouble() * 2.4 < 1 && mutations.Changes < mutations.ChangeLimit
                && player.HasVagina() && player.Mf("m", "f") == "f"

                && body.Get<HipsPart>().Rating < 14)
            {
                context.Write("Items.EchidnaCake.WiderHips".Localized());

                body.Get<HipsPart>().Rating += 1;

                mutations.Changes += 1;
            }

            flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }

        private static float AverageBreastSize(Player player)
        {
            var rows = player.Body.GetList<BreastRowPart>();

            if (rows.Count == 0)
                return 0;

            var total = 0f;

            foreach (var row in rows)
                total += row.BreastRating;

            return total / rows.Count;
        }
    }
}