using System;
using System.Collections.Generic;
using FontStashSharp.RichText;
using LibRimeSharp.Data;
using Microsoft.Xna.Framework;
using MonoCoC.Extensions;
using MonoCoC.UI;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;

namespace MonoCoC.UI.Components
{
    public class ImeCandidateBar : HjsonView
    {
        private const int CandidateCount = 9;

        private Container Panel { get; set; } = null!;

        private Button[] Candidates { get; } = new Button[CandidateCount];

        private string _lastSignature = "";

        public ImeCandidateBar()
        {
            Initialize();

            CanHit = false;

            Visible = false;
        }

        protected override string ScreenFile => "Views\\ImeCandidateBar.hjson";

        [UIRequired("ime.panel")]
        private void BindPanel(Container part) => Panel = part;

        [UIRequired("ime.candidate.0")]
        private void BindCandidate0(Button part) => Candidates[0] = part;

        [UIRequired("ime.candidate.1")]
        private void BindCandidate1(Button part) => Candidates[1] = part;

        [UIRequired("ime.candidate.2")]
        private void BindCandidate2(Button part) => Candidates[2] = part;

        [UIRequired("ime.candidate.3")]
        private void BindCandidate3(Button part) => Candidates[3] = part;

        [UIRequired("ime.candidate.4")]
        private void BindCandidate4(Button part) => Candidates[4] = part;

        [UIRequired("ime.candidate.5")]
        private void BindCandidate5(Button part) => Candidates[5] = part;

        [UIRequired("ime.candidate.6")]
        private void BindCandidate6(Button part) => Candidates[6] = part;

        [UIRequired("ime.candidate.7")]
        private void BindCandidate7(Button part) => Candidates[7] = part;

        [UIRequired("ime.candidate.8")]
        private void BindCandidate8(Button part) => Candidates[8] = part;

        public void ShowAt(Rectangle anchor, IReadOnlyList<RimeCandidate> candidates, int highlight)
        {
            if (Content is null)
                return;

            var signature = anchor.X + "|" + anchor.Y + "|" + highlight + "|";

            foreach (var candidate in candidates)
                signature += candidate.Text + "\u0001";

            if (signature == _lastSignature)
            {
                Visible = true;

                return;
            }

            _lastSignature = signature;

            Content.CanHit = false;

            var font = UIFonts.GetAt(UIFonts.Family("coc:menu"), 26);

            var buttonHeight = Math.Max(36, (int)Math.Ceiling((double)font.LineHeight) + 6);

            var totalWidth = 0;

            for (var index = 0; index < CandidateCount; index++)
            {
                var button = Candidates[index];

                if (index >= candidates.Count)
                {
                    button.Visible = false;

                    continue;
                }

                var text = index + 1 + "." + candidates[index].Text;

                button.SetLabel(text);

                button.Visible = true;

                button.Selected = index == highlight;

                var width = (int)Math.Ceiling(font.MeasureString(text).X) + 24;

                button.LayoutData.Position = new Vector2(10 + totalWidth, 6);

                button.LayoutData.Size = new Vector2(width, buttonHeight);

                button.RefreshVisual();

                var lineHeight = (int)Math.Ceiling((double)font.MeasureString(text).Y);

                button.Label.RichTextLayout.Font = font;

                button.Label.Alignment(TextHorizontalAlignment.Left);

                button.Label.WrapWidth(width - 4);

                button.Label.LayoutData.Size = new Vector2(width - 4, buttonHeight);

                button.Label.Position(4, Math.Max(0, (buttonHeight - lineHeight) / 2));

                totalWidth += width + 10;
            }

            var panelWidth = Math.Max(360, totalWidth + 12);

            var panelHeight = buttonHeight + 12;

            Panel.LayoutData.Size = new Vector2(panelWidth, panelHeight);

            var x = Math.Clamp(anchor.X, 0, Math.Max(0, LayoutConstants.ScreenWidth - panelWidth));

            var y = anchor.Y - panelHeight - 2;

            if (y < 0)
                y = anchor.Bottom + 2;

            if (y + panelHeight > LayoutConstants.ScreenHeight)
                y = Math.Max(0, LayoutConstants.ScreenHeight - panelHeight);

            Content.LayoutData.LayoutType = LayoutType.Absolute;

            Content.LayoutData.Position = new Vector2(x, y);

            Visible = true;
        }

        public void Hide() => Visible = false;
    }
}
