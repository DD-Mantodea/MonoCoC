using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using FontStashSharp.RichText;
using MonoCoC.Assets;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using MonoCoC.UI.Styles;

using Hjson;

namespace MonoCoC.UI.Components.Combat
{
    public class DetailStats : Container, IFromHjson<DetailStats>
    {
        private readonly List<(StatRow Row, Func<string> Value)> _entries = [];

        private readonly string _section;

        private readonly UIStateStyle _style;

        public Action<StatsDetailScene.StatDetail>? DetailSelected;

        public DetailStats(string section, UIStateStyle style)
        {
            _section = section;

            _style = style;

            CanHit = true;

            BuildRows();
        }

        private void BuildRows()
        {
            var y = 0;

            var column = 0;

            if (string.Equals(_section, "combat", StringComparison.OrdinalIgnoreCase))
            {
                var details = StatsScene.BuildCombatDetails();

                var index = 0;

                foreach (var entry in CombatEntries())
                {
                    var detailIndex = index;

                    Action? click = detailIndex < details.Count
                        ? () => DetailSelected?.Invoke(details[detailIndex])
                        : null;

                    AddRow(entry.LabelKey.Localized(), entry.Value, ref y, ref column, click);

                    index++;
                }
            }
            else
            {
                var types = ManagerInstance<StatTypeManager>.Instance.Registry.Ids
                    .Select(id => ManagerInstance<StatTypeManager>.Instance.TryGet(new Stat(id), out var type) ? type : null)
                    .Where(type => type is { Visible: true } && type.Group == "core")
                    .OrderBy(type => type!.Order)
                    .ThenBy(type => type!.Name)
                    .ToList();

                foreach (var type in types)
                    AddRow(type!.Name, () => Format(type, CoC.Instance.Player.Stats.Get(type.Stat)), ref y, ref column);
            }
        }

        private void AddRow(string label, Func<string> value, ref int y, ref int column, Action? onClick = null)
        {
            var available = Width > 0 ? Width : 596;

            var rowWidth = MathF.Max(1, (available - _style.ColumnGap * (_style.Columns - 1)) / _style.Columns);

            var x = column * (rowWidth + _style.ColumnGap);

            var height = _style.Height > 0 ? _style.Height : 32;

            var font = UIFonts.GetAt(UIFonts.Family(_style.FontRole), _style.FontSize);

            var row = new StatRow(font)
                .Position((int)x, y)
                .Size((int)rowWidth, height)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            row.ApplyStyle(_style);

            row.CanHit = onClick is not null;

            row.Label.Text(label);

            row.Value.Text(value());

            if (onClick is not null)
                row.OnClick = () =>
                {
                    SelectRow(row);

                    onClick();
                };

            _entries.Add((row, value));

            column++;

            if (column < _style.Columns)
                return;

            column = 0;

            y += height + _style.RowGap;
        }

        private void SelectRow(StatRow selected)
        {
            foreach (var (row, _) in _entries)
                row.Selected = ReferenceEquals(row, selected);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var (row, value) in _entries)
                row.Value.Text(value());

            base.Update(gameTime);
        }

        private static IEnumerable<(string LabelKey, Func<string> Value)> CombatEntries()
        {
            yield return ("Views.PlayerStats.AttackDamage", () =>
            {
                var player = CoC.Instance.Player;

                return MathF.Round((player.WeaponAttack + player.Strength) * player.PhysMod()).ToString("0");
            });

            yield return ("Views.PlayerStats.PhysicalDamage", () => MathF.Round(100 * CoC.Instance.Player.PhysMod()) + "%");

            yield return ("Views.PlayerStats.CriticalChance", () => MathF.Round(CoC.Instance.Player.GetBaseCritChance()) + "%");

            yield return ("Views.PlayerStats.WeaponCrit", () => MathF.Round(CoC.Instance.Player.GetMeleeCritBonus()) + "%");

            yield return ("Views.PlayerStats.DodgeChance", () => MathF.Round(CoC.Instance.Player.GetEvasionChance()) + "%");

            yield return ("Views.PlayerStats.ParryChance", () => MathF.Round(CoC.Instance.Player.CanParry() ? CoC.Instance.Player.ParryChance() : 0) + "%");

            yield return ("Views.PlayerStats.Armor", () => MathF.Round(CoC.Instance.Player.ArmorDefense).ToString("0"));

            yield return ("Views.PlayerStats.DamageResistance", () => MathF.Round(100 - Math.Clamp(CoC.Instance.Player.ArmorDefense, 0, 80)) + "%");

            yield return ("Views.PlayerStats.TeaseChance", () => MathF.Round((CoC.Instance.Player.UpperGarment?.Sexiness + CoC.Instance.Player.LowerGarment?.Sexiness) ?? 0).ToString("0"));

            yield return ("Views.PlayerStats.TeaseDamage", () => MathF.Round(CoC.Instance.Player.Stats.Addition(GameStats.TeaseDamage)).ToString("0"));

            yield return ("Views.PlayerStats.HealthRegen", () => MathF.Round(CoC.Instance.Player.Stats.Addition(GameStats.HealthRegenPercent)) + "%");

            yield return ("Views.PlayerStats.LustResistance", () => MathF.Round(100 - CoC.Instance.Player.LustPercent) + "%");

            yield return ("Views.PlayerStats.SpellMod", () => MathF.Round(100 * CoC.Instance.Player.SpellMod()) + "%");

            yield return ("Views.PlayerStats.SpellCost", () => MathF.Round(CoC.Instance.Player.SpellCost(100)) + "%");
        }

        private static string Format(StatType type, float value) => type.Format switch
        {
            StatFormat.Int => MathF.Round(value).ToString("0"),
            StatFormat.Percent => value.ToString("0.#") + "%",
            _ => value.ToString("0.##")
        };

        public static string TypeId => "coc:detail_stats";

        public static Component FromHjson(JsonObject node, UIBuildContext services) =>
            new DetailStats(
                node.Str("section", "core"),
                UIStateStyle.Parse(services.ResolveStyle(node, "rowStyle")));

        [System.Runtime.CompilerServices.ModuleInitializer]
        internal static void Register() => ComponentFactory.Register<DetailStats>();
    }
}
