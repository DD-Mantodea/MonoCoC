using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Swamp;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 SlimyCloth.hx（史莱姆转化布）──────────────────────────────
    public class SlimyClothEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var mutations = Mutations.Mutations.Instance;

            var random = context.Random;

            var body = player.Body;

            var flags = CoC.Instance.Flags;

            var armor = player.Armor;

            if (armor != null && armor.Id.Path == "vine_armor")
            {
                if (armor.Save.GetBool("clothUsed"))
                {
                    context.Write("Items.SlimyCloth.VineSecondUse".Localized());

                    if (flags.Get(GameFlagIds.HolliSubmissive) == 1
                        && (AlrauneScene.Save.GetInt("questAsked", 0) & AlrauneScene.ASKEDHO) != 0)

                        context.Write("Items.SlimyCloth.VineHolliRealization".Localized());

                    player.SetArmor(Armor.Nothing);

                    armor.Stage = 1;

                    armor.Save.SetBool("clothUsed", false);

                    return;
                }

                context.Write("Items.SlimyCloth.VineFirstUse".Localized());

                armor.Save.SetBool("clothUsed", true);

                return;
            }

            context.Write("Items.SlimyCloth.Apply".Localized());

            if (player.LibidoPercent < 80)
            {
                player.ApplyStats(
                    new DynStat(GameStats.Libido, 0.5f + (90 - player.Libido) / 10f),

                    new DynStat(GameStats.Lust, player.Libido / 2f));

                context.Write("Items.SlimyCloth.Libido".Localized());
            }

            if (player.SensitivityPercent < 50)
            {
                context.Write("Items.SlimyCloth.Sensitive".Localized());

                player.ApplyStats(new DynStat(GameStats.Sensitivity, 1));
            }
            else if (player.SensitivityPercent > 50)
            {
                context.Write("Items.SlimyCloth.Numb".Localized());

                player.ApplyStats(new DynStat(GameStats.Sensitivity, -1));
            }

            if (body.Get<NeckPart>().Id.Path != "normal" && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
                mutations.RestoreNeck("gooGasmic");

            if (body.Get<RearBodyPart>().Id != BodyPartTypes.SharkFin
                && mutations.Changes < mutations.ChangeLimit && random.Next(5) == 0)

                mutations.RestoreRearBody("gooGasmic");

            if (random.Next(5) == 0)
                mutations.UpdateOvipositionPerk("gooGasmic");

            var wings = body.Get<WingsPart>();

            var rearBody = body.Get<RearBodyPart>();

            if (wings.Id != BodyPartTypes.None || rearBody.Id == BodyPartTypes.SharkFin)
            {
                if (rearBody.Id == BodyPartTypes.SharkFin)
                {
                    context.Write("Items.SlimyCloth.FinMelt".Localized());

                    rearBody.Restore();
                }
                else
                    context.Write("Items.SlimyCloth.WingsMelt".Localized());

                wings.Restore();

                return;
            }

            var colorChoice = WeightedColor(random);

            var hair = body.Get<HairPart>();

            if (hair.Id.Path != "goo")
            {
                hair.Id = new Identifier("coc", "goo");

                if (hair.Length <= 0)
                {
                    context.Write("Items.SlimyCloth.HairBald".Localized());

                    hair.Length = 5;
                }
                else if (!hair.Color.Contains("rubbery") && !hair.Color.Contains("latex-textured"))
                    context.Write("Items.SlimyCloth.HairGoo".Localized());
                else
                    context.Write("Items.SlimyCloth.HairLatex".Localized());

                if (Array.IndexOf(ColorLists.GooMorph, hair.Color) == -1)
                {
                    context.Write("Items.SlimyCloth.HairColor1".Localized());

                    hair.Color = colorChoice;

                    context.Write("Items.SlimyCloth.HairColor2".Localized());
                }

                player.ApplyStats(new DynStat(GameStats.Lust, 10));

                return;
            }

            var skin = body.Get<SkinPart>();

            if (mutations.TfNoFur(2) && hair.Id.Path == "goo"
                && (skin.Id != new Identifier("coc", "goo") || skin.DescKey != "Skin.Desc.skin" || skin.Adj != "slimy"))
            {
                if (player.HasPlainSkin())
                    context.Write("Items.SlimyCloth.SkinPlain".Localized());
                else if (player.HasFur())
                    context.Write("Items.SlimyCloth.SkinFur".Localized());
                else if (player.HasScales())
                    context.Write("Items.SlimyCloth.SkinScales".Localized());

                skin.Id = new Identifier("coc", "goo");

                skin.DescKey = "Skin.Desc.skin";

                skin.Adj = "slimy";

                body.Get<UnderBodyPart>().Restore();

                if (Array.IndexOf(ColorLists.GooMorph, skin.Tone) == -1)
                {
                    context.Write("Items.SlimyCloth.SkinTone1".Localized());

                    skin.Tone = colorChoice;

                    context.Write("Items.SlimyCloth.SkinTone2".Localized());

                    var arms = body.Get<ArmsPart>();

                    if (arms.Id.Path != "human" || arms.Claws.Id.Path != "normal")
                        mutations.RestoreArms("gooGasmic");
                }

                return;
            }

            var lowerBody = body.Get<LowerBodyPart>();

            if (skin.Id.Path == "goo" && skin.Adj == "slimy" && skin.DescKey == "Skin.Desc.skin"
                && lowerBody.Id.Path != "goo")
            {
                context.Write("Items.SlimyCloth.Legs".Localized());

                player.Tallness -= 3 + random.Next(2);

                if (player.Tallness < 36)
                {
                    player.Tallness = 36;

                    context.Write("Items.SlimyCloth.LegsMinHeight".Localized());
                }

                lowerBody.Id = new Identifier("coc", "goo");

                lowerBody.LegCount = 1;

                return;
            }

            var vaginas = body.GetList<VaginaPart>();

            if (!player.HasVagina())
            {
                context.Write("Items.SlimyCloth.GrowVagina".Localized());

                player.CreateVagina();

                vaginas = body.GetList<VaginaPart>();

                if (vaginas.Count > 0)
                {
                    vaginas[0].Wetness = 4;

                    vaginas[0].Looseness = 4;

                    vaginas[0].ClitLength = 0.4f;
                }

                return;
            }

            if (player.VaginalCapacity() < 9000)
            {
                if (!player.HasStatusEffect(StatusEffectIds.BonusVCapacity))
                    player.CreateStatusEffect(StatusEffectIds.BonusVCapacity, 9000, 0, 0, 0);
                else
                    player.ChangeStatusValue(StatusEffectIds.BonusVCapacity, 0, 9000);

                context.Write("Items.SlimyCloth.InfiniteVagina".Localized(player.VaginaDescript(0)));

                return;
            }

            if (player.Tallness < 100 && random.Next(3) <= 1)
            {
                context.Write("Items.SlimyCloth.GrowTaller".Localized());

                player.Tallness += 2;

                player.ApplyStats(
                    new DynStat(GameStats.Strength, 1),

                    new DynStat(GameStats.Toughness, 1));
            }
            else
                if (!player.HasStatusEffect(StatusEffectIds.SlimeCraving))
                {
                    context.Write("Items.SlimyCloth.CravingStart".Localized());

                    player.CreateStatusEffect(StatusEffectIds.SlimeCraving, 0, 0, 0, 1);
                }
                else
                {
                    context.Write("Items.SlimyCloth.CravingFed".Localized());

                    player.ChangeStatusValue(StatusEffectIds.SlimeCraving, 0, 0);
                }

            if (random.Next(2) == 0)
                context.Write(player.ModFemGoal(85, 3));

            if (random.Next(2) == 0)
                context.Write(player.ModThickness(20, 3));

            if (random.Next(2) == 0)
                context.Write(player.ModTone(15, 5));

            flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }

        // 原版 WeightedChoice：绿 30 / 紫 20 / 蓝 20 / 天蓝 20 / 翠绿 10
        private static string WeightedColor(Random random)
        {
            var roll = random.Next(100);

            if (roll < 30)
                return "green";

            if (roll < 50)
                return "purple";

            if (roll < 70)
                return "blue";

            if (roll < 90)
                return "cerulean";

            return "emerald";
        }
    }
}