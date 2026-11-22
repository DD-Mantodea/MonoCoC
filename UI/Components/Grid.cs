using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;

using FontStashSharp;
using FontStashSharp.RichText;
using Hjson;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Structures;
using MonoCoC.UI.Styles;
using System.Globalization;
using System.Runtime.CompilerServices;
namespace MonoCoC.UI.Components
{
    public enum GridTrackKind
    {
        Pixel,

        Auto,

        Star
    }

    public readonly struct GridTrack(GridTrackKind kind, float value)
    {
        public GridTrackKind Kind { get; } = kind;

        public float Value { get; } = value;

        public static GridTrack Auto => new(GridTrackKind.Auto, 0);

        public static GridTrack Star(float weight = 1) => new(GridTrackKind.Star, weight <= 0 ? 1 : weight);

        public static GridTrack Pixels(float value) => new(GridTrackKind.Pixel, Math.Max(0, value));
    }

    public class Grid(int rows, int columns) : Container, IFromHjson<Grid> {
        public int Rows = rows;

        public int Columns = columns;

        public int Spacing = 0;

        public bool AutoSize;

        public bool AutoIndex;

        public List<GridTrack> RowDefinitions = [];

        public List<GridTrack> ColumnDefinitions = [];

        private float[] _rowOffsets = [];

        private float[] _columnOffsets = [];

        private float[] _rowSizes = [];

        private float[] _columnSizes = [];

        private Vector2 _measuredSize;

        private bool _dirty = true;

        public override void PerformLayout(GameTime gameTime)
        {
            // 子组件先完成尺寸测量，再根据它们计算轨道与自身大小。
            base.PerformLayout(gameTime);

            if (AutoSize)
            {
                var desired = MeasureDesired();

                if (LayoutData.Size != desired)
                    LayoutData.Size = desired;
            }

            _dirty = true;

            EnsureTracks();
        }

        public void ResizeToFit()
        {
            if (!AutoSize)
                return;

            LayoutData.Size = MeasureDesired();

            _dirty = true;
        }

        public override Vector2 SlotOf(int index)
        {
            EnsureTracks();

            if (index < 0 || index >= LayoutData.Children.Count)
                return Vector2.Zero;

            var child = LayoutData.Children[index];

            var row = RowOf(child, index);

            var column = ColumnOf(child, index);

            var rowOffset = 0f;

            var columnOffset = 0f;

            for (var i = 0; i < row && i < _rowOffsets.Length; i++)
                rowOffset += _rowSizes[i] + Spacing;

            for (var i = 0; i < column && i < _columnOffsets.Length; i++)
                columnOffset += _columnSizes[i] + Spacing;

            return new Vector2(columnOffset, rowOffset);
        }

        public override Vector2 SlotSize(int index)
        {
            EnsureTracks();

            if (index < 0 || index >= LayoutData.Children.Count)
                return Vector2.Zero;

            var child = LayoutData.Children[index];

            var row = RowOf(child, index);

            var column = ColumnOf(child, index);

            var rowSpan = Math.Max(1, child.LayoutData.GridRowSpan);

            var columnSpan = Math.Max(1, child.LayoutData.GridColumnSpan);

            var width = 0f;

            var height = 0f;

            for (var i = column; i < column + columnSpan && i < _columnSizes.Length; i++)
                width += _columnSizes[i] + (i > column ? Spacing : 0);

            for (var i = row; i < row + rowSpan && i < _rowSizes.Length; i++)
                height += _rowSizes[i] + (i > row ? Spacing : 0);

            return new Vector2(width, height);
        }

        private int RowOf(Component child, int index) =>
            child.LayoutData.GridRow >= 0
                ? child.LayoutData.GridRow
                : index / Math.Max(1, Columns);

        private int ColumnOf(Component child, int index) =>
            child.LayoutData.GridColumn >= 0
                ? child.LayoutData.GridColumn
                : index % Math.Max(1, Columns);

        private void EnsureTracks()
        {
            EnsureDefinitions();

            if (!_dirty && _measuredSize == LayoutData.Size)
                return;

            _measuredSize = LayoutData.Size;

            _dirty = false;

            var width = LayoutData.ComputeWidth() - LayoutData.Padding.Left - LayoutData.Padding.Right;

            var height = LayoutData.ComputeHeight() - LayoutData.Padding.Top - LayoutData.Padding.Bottom;

            _columnSizes = Resolve(ColumnDefinitions, width, horizontal: true);

            _rowSizes = Resolve(RowDefinitions, height, horizontal: false);

            _columnOffsets = BuildOffsets(_columnSizes);

            _rowOffsets = BuildOffsets(_rowSizes);
        }

        private void EnsureDefinitions()
        {
            while (RowDefinitions.Count < Rows)
                RowDefinitions.Add(GridTrack.Star());

            while (ColumnDefinitions.Count < Columns)
                ColumnDefinitions.Add(GridTrack.Star());

            if (RowDefinitions.Count > Rows)
            {
                RowDefinitions.RemoveRange(Rows, RowDefinitions.Count - Rows);
            }

            if (ColumnDefinitions.Count > Columns)
            {
                ColumnDefinitions.RemoveRange(Columns, ColumnDefinitions.Count - Columns);
            }

            _rowOffsets = new float[Rows];

            _columnOffsets = new float[Columns];
        }

        private Vector2 MeasureDesired()
        {
            EnsureDefinitions();

            var width = Resolve(ColumnDefinitions, 0, horizontal: true).Sum() + Spacing * Math.Max(0, Columns - 1);
            var height = Resolve(RowDefinitions, 0, horizontal: false).Sum() + Spacing * Math.Max(0, Rows - 1);

            return new Vector2(
                width + LayoutData.Padding.Left + LayoutData.Padding.Right,
                height + LayoutData.Padding.Top + LayoutData.Padding.Bottom);
        }

        private float[] Resolve(List<GridTrack> definitions, float available, bool horizontal)
        {
            var result = new float[definitions.Count];

            var consumed = 0f;

            var starTotal = 0f;

            for (var i = 0; i < definitions.Count; i++)
            {
                var track = definitions[i];

                if (track.Kind == GridTrackKind.Pixel)
                {
                    result[i] = track.Value;

                    consumed += track.Value;
                }
                else if (track.Kind == GridTrackKind.Auto)
                {
                    var desired = ChildDesired(i, horizontal);

                    result[i] = desired;

                    consumed += desired;
                }
                else
                {
                    starTotal += track.Value;
                }
            }

            var remaining = Math.Max(0, available - consumed - Spacing * Math.Max(0, definitions.Count - 1));

            for (var i = 0; i < definitions.Count; i++)
            {
                if (definitions[i].Kind != GridTrackKind.Star)
                    continue;

                result[i] = available <= 0 || starTotal <= 0
                    ? ChildDesired(i, horizontal)
                    : remaining * definitions[i].Value / starTotal;
            }

            return result;
        }

        private float ChildDesired(int track, bool horizontal)
        {
            var result = 0f;

            for (var index = 0; index < LayoutData.Children.Count; index++)
            {
                var child = LayoutData.Children[index];

                var position = horizontal ? ColumnOf(child, index) : RowOf(child, index);

                if (position != track)
                    continue;

                var size = child.LayoutData.ComputeSize();

                result = Math.Max(result, horizontal ? size.X : size.Y);
            }

            return result;
        }

        private static float[] BuildOffsets(float[] sizes)
        {
            var offsets = new float[sizes.Length];

            var offset = 0f;

            for (var i = 0; i < sizes.Length; i++)
            {
                offsets[i] = offset;

                offset += sizes[i];
            }

            return offsets;
        }
    
        public static string TypeId => "coc:grid";

        public static Component FromHjson(JsonObject node, UIBuildContext services)
        {
            var rowDefinitions = ReadGridTracks(node, "rows", "rowDefinitions");

            var columnDefinitions = ReadGridTracks(node, "columns", "columnDefinitions");

            var grid = new Grid(
                rowDefinitions.Count > 0 ? rowDefinitions.Count : Math.Max(1, node.Int("rows", 1)),
                columnDefinitions.Count > 0 ? columnDefinitions.Count : Math.Max(1, node.Int("columns", 1)));

            if (rowDefinitions.Count > 0)
                grid.RowDefinitions = rowDefinitions;

            if (columnDefinitions.Count > 0)
                grid.ColumnDefinitions = columnDefinitions;

            grid.AutoSize = node.Bool("autoSize", false);

            grid.AutoIndex = node.Bool("autoIndex", false);

            if (node.TryGetValue("spacing", out var spacing))
                grid.Spacing = UIParseUtils.Int(spacing);

            return grid;
        }


        private static List<GridTrack> ReadGridTracks(JsonObject node, string countKey, string definitionsKey)
        {
            var result = new List<GridTrack>();

            var value = node.TryGetValue(definitionsKey, out var definitions)
                ? definitions
                : node.TryGetValue(countKey, out var count) ? count : null;

            if (value is not JsonArray array)
            {
                if (value is not null)
                {
                    var text = value.ToString().Trim('"').Trim();

                    if (text.StartsWith("auto", StringComparison.OrdinalIgnoreCase))
                    {
                        var repeat = text.Length == 4
                            ? 1
                            : int.TryParse(text[4..], NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed)
                                ? Math.Max(0, parsed)
                                : 0;

                        for (var i = 0; i < repeat; i++)
                            result.Add(GridTrack.Auto);
                    }
                }

                return result;
            }

            foreach (var entry in array)
                result.Add(ParseGridTrack(entry));

            return result;
        }


        private static GridTrack ParseGridTrack(JsonValue? value)
        {
            if (value is null)
                return GridTrack.Star();

            if (value is JsonObject obj)
            {
                var size = obj.Str("size", obj.Str("value", obj.Str("height", obj.Str("width", "1*"))));

                return ParseGridTrackString(size);
            }

            return ParseGridTrackString(value.ToString().Trim('"'));
        }


        private static GridTrack ParseGridTrackString(string value)
        {
            value = value.Trim();

            if (value.Equals("auto", StringComparison.OrdinalIgnoreCase))
                return GridTrack.Auto;

            if (value.EndsWith('*'))
            {
                var weight = value[..^1];

                return float.TryParse(weight, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed)
                    ? GridTrack.Star(parsed)
                    : GridTrack.Star();
            }

            return float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var pixels)
                ? GridTrack.Pixels(pixels)
                : GridTrack.Star();
        }


        [ModuleInitializer]
        internal static void Register() => ComponentFactory.Register<Grid>();
    
}

    internal static class GridArrayExtensions
    {
        public static float Sum(this float[] values)
        {
            var result = 0f;

            foreach (var value in values)
                result += value;

            return result;
        }
    }
}
