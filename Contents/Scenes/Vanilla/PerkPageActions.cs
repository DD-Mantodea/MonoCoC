using System;
using System.Collections.Generic;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    /// <summary>
    /// 原版 PlayerInfo.hx:601 displayPerks 的条件文本/按钮。
    /// mod 可注册自己的 Perk 页面附加项，PerksScene 不再逐个 if 特判。
    /// </summary>
    public static class PerkPageActions
    {
        public sealed class Entry
        {
            public Identifier Id = default!;

            public Func<bool> Visible = () => false;

            public Func<string> Text = () => "";

            public string LabelKey = "";

            public string HintKey = "";

            public Action Open = () => { };
        }

        private static readonly List<Entry> Entries = [];

        public static IReadOnlyList<Entry> All => Entries;

        static PerkPageActions()
        {
            Register("coc:perk_up",

                () => MonoCoC.CoC.Instance.Player.PerkPoints > 0,

                () => "Scenes.Perks.PerkPoints".Localized((int)MonoCoC.CoC.Instance.Player.PerkPoints),

                "Scenes.Perks.PerkUp", "Scenes.Perks.PerkUpHint",

                () => MonoCoC.Contents.Scenes.Scene.Go<LevelUpScene>());

            Register("coc:double_attack",

                () => MonoCoC.CoC.Instance.Player.HasPerk(MonoCoC.Contents.Datas.Vanilla.PerkIds.DoubleAttack),

                () => "Scenes.Perks.DoubleAttackOptions".Localized(),

                "Scenes.Perks.DblOptions", "Scenes.Perks.DblOptionsHint",

                DoubleAttackOptions.Show);

            Register("coc:ascension_tolerance",

                () => MonoCoC.CoC.Instance.Player.HasPerk(MonoCoC.Contents.Datas.Vanilla.PerkIds.AscensionTolerance),

                () => "Scenes.Perks.ToleranceOptions".Localized(),

                "Scenes.Perks.TolOptions", "Scenes.Perks.TolOptionsHint",

                () => PerkOptionsScene.ShowTolerance(PerksScene.BackToPerks));

            Register("coc:battlemage",

                () => MonoCoC.CoC.Instance.Player.HasPerk(MonoCoC.Contents.Datas.Vanilla.PerkIds.Battlemage),

                () => "Scenes.Perks.MightOptions".Localized(),

                "Scenes.Perks.MightOptions", "Scenes.Perks.MightOptionsHint",

                () => PerkOptionsScene.ShowMight(PerksScene.BackToPerks));

            Register("coc:spellsword",

                () => MonoCoC.CoC.Instance.Player.HasPerk(MonoCoC.Contents.Datas.Vanilla.PerkIds.Spellsword),

                () => "Scenes.Perks.ChargeOptions".Localized(),

                "Scenes.Perks.ChargeOptions", "Scenes.Perks.ChargeOptionsHint",

                () => PerkOptionsScene.ShowCharge(PerksScene.BackToPerks));

            Register("coc:transformation_resistance",

                () => MonoCoC.CoC.Instance.Player.HasPerk(MonoCoC.Contents.Datas.Vanilla.PerkIds.TransformationResistance),

                () => "Scenes.Perks.TransformationResistanceOptions".Localized(),

                "Scenes.Perks.TfResist", "Scenes.Perks.TfResistHint",

                () => PerkOptionsScene.ShowTfResist(PerksScene.BackToPerks));
        }

        public static void Register(string id, Func<bool> visible, Func<string> text,

            string labelKey, string hintKey, Action open)
        {
            if (!Identifier.TryParse(id, out var identifier))
                throw new FormatException($"Invalid perk page action id '{id}'");

            Entries.RemoveAll(entry => entry.Id == identifier);

            Entries.Add(new Entry
            {
                Id = identifier,

                Visible = visible,

                Text = text,

                LabelKey = labelKey,

                HintKey = hintKey,

                Open = open,
            });
        }
    }
}