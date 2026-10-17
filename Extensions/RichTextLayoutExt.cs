using FontStashSharp.RichText;
using Microsoft.Xna.Framework;

namespace MonoCoC.Extensions
{
    public static class RichTextLayoutExt
    {
        extension(RichTextLayout layout)
        {
            public RichTextLayout Append(string text)
            {
                layout.Text += text;

                return layout;
            }

            public RichTextLayout AppendLine(string text = "")
            {
                layout.Text += text + '\n';

                return layout;
            }

            public RichTextLayout AppendColor(string text, Color color)
            {
                layout.Text += $"/c[#{color.R:X2}{color.G:X2}{color.B:X2}{color.A:X2}]{text}/cd";

                return layout;
            }

            public RichTextLayout AppendFont(string fontName, int size, string text)
            {
                layout.Text += $"/f[{fontName},{size}]{text}/fd";

                return layout;
            }

            public RichTextLayout AppendUnderline(string text)
            {
                layout.Text += $"/tu{text}/td";

                return layout;
            }

            public RichTextLayout AppendStrike(string text)
            {
                layout.Text += $"/ts{text}/td";

                return layout;
            }

            public RichTextLayout AppendOffset(int offset, string text)
            {
                layout.Text += $"/v[{offset}]{text}/vd";

                return layout;
            }

            public RichTextLayout AppendSpace(int size)
            {
                layout.Text += $"/s[{size}]";

                return layout;
            }

            public RichTextLayout Clear()
            {
                layout.Text = "";

                return layout;
            }
        }
    }
}
