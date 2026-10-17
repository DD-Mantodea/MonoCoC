using System;
using Microsoft.Xna.Framework;
using MonoCoC.UI.Layout;

namespace MonoCoC.UI.Components
{
    public class Grid(int rows, int columns) : Container
    {
        public int Rows = rows;

        public int Columns = columns;

        public int Spacing = 0;

        /// <summary>
        /// When true, the grid derives its size from its largest child cell
        /// plus spacing and padding. Call ResizeToFit after children exist.
        /// </summary>
        public bool AutoSize;

        public void ResizeToFit()
        {
            if (!AutoSize || LayoutData.Children.Count == 0)
                return;

            var cellWidth = 0;

            var cellHeight = 0;

            foreach (var child in LayoutData.Children)
            {
                cellWidth = Math.Max(cellWidth, child.Width);

                cellHeight = Math.Max(cellHeight, child.Height);
            }

            var width = cellWidth * Columns + Spacing * (Columns - 1) +
                        LayoutData.Padding.Left + LayoutData.Padding.Right;

            var height = cellHeight * Rows + Spacing * (Rows - 1) +
                         LayoutData.Padding.Top + LayoutData.Padding.Bottom;

            LayoutData.Size = new Vector2(width, height);
        }

        public override Vector2 SlotOf(int index)
        {
            if (index < 0 || Rows <= 0 || Columns <= 0)
                return Vector2.Zero;

            var innerWidth = Width - LayoutData.Padding.Left - LayoutData.Padding.Right;

            var innerHeight = Height - LayoutData.Padding.Top - LayoutData.Padding.Bottom;

            var cellWidth = (innerWidth - Spacing * (Columns - 1)) / (float)Columns;

            var cellHeight = (innerHeight - Spacing * (Rows - 1)) / (float)Rows;

            return new Vector2(
                index % Columns * (Spacing + cellWidth),

                index / Columns * (Spacing + cellHeight));
        }
    }
}