using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    [Register<CeruleanPotionEffect>("coc:cerulean_potion")]
    public class CeruleanPotionEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            context.Player.Lust = Math.Min(100, context.Player.Lust + 5);

            context.Write("Items.CeruleanPotion.Use".Localized());
        }
    }

    [Register<NumbingEffect>("coc:numbing")]
    public class NumbingEffect(bool oil) : ConsumableEffect
    {
        // 注册表要求 public 无参构造；实际由 ConsumableEffectRegistry 按数据里的 oil 字段构造。
        public NumbingEffect() : this(false) { }

        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var loss = 5 + (float)Math.Floor(player.Sensitivity / 10);

            player.Sensitivity = Math.Max(0, player.Sensitivity - loss);

            context.Write(oil

                ? "Items.NumbOil.Use".Localized()

                : "Items.NumbRocks.Use".Localized());
        }
    }

    [Register<PurityPhilterEffect>("coc:purity_philter")]
    public class PurityPhilterEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var loss = 5 + (float)Math.Floor(player.Corruption / 10);

            player.Corruption = Math.Max(0, player.Corruption - loss);

            context.Write("Items.PurityPhilter.Use".Localized());
        }
    }

    [Register<SensitivityDraftEffect>("coc:sensitivity_draft")]
    public class SensitivityDraftEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            player.Sensitivity = Math.Min(100, player.Sensitivity + 10);

            player.Lust = Math.Min(100, player.Lust + 5);

            context.Write("Items.SensitivityDraft.Use".Localized());
        }
    }

    [Register<ScholarTeaEffect>("coc:scholar_tea")]
    public class ScholarTeaEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            context.Player.Intelligence = Math.Min(100, context.Player.Intelligence + 2);

            context.Write("Items.ScholarTea.Use".Localized());
        }
    }

    [Register<VitalityTinctureEffect>("coc:vitality_tincture")]
    public class VitalityTinctureEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            player.Strength += 1;

            player.Toughness += 1;

            context.Write("Items.VitalityTincture.Use".Localized());
        }
    }

    [Register<DeBimboEffect>("coc:de_bimbo")]
    public class DeBimboEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            player.RemovePerk(PerkIds.BimboBrains);

            player.RemovePerk(PerkIds.FutaFaculties);

            player.RemovePerk(PerkIds.BroBrains);

            context.Write("Items.DeBimbo.Use".Localized());
        }
    }

}