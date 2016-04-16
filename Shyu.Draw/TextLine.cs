using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Shyu.Core;

namespace Shyu.UI.Drawing
{
    public class TextInfo
    {
        public string Text;
        public Font Font = new Font("Tahoma", 7.5F, FontStyle.Bold);
        public Color TextColor = SystemColors.InfoText;
        public Color BackColor = SystemColors.Window;
        public StringFormat Format = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
    }

    public class TextLine
    {
        public string Text = "QuickTag";
        public Font Font = new Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold);
        public Color Color = Color.Navy;
        public Color BackColor = Color.Transparent;
        public bool Visible = true;
        public PointF Location = new PointF(0, 0);
        public SizeF Size = new SizeF(0, 0);
        public float LineSpace = 0;
        public float OffsetY = 0;
        public StringFormat Format = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

        public void GetShape(Graphics g)
        {
            Size = g.MeasureString(Text, Font, Location, Format);
        }

        public void Draw(Graphics g)
        {
            g.DrawString(Text, Font, new SolidBrush(Color), Location, Format);
        }

        public void DrawWithBg(Graphics g)
        {
            g.FillRectangle(new SolidBrush(BackColor), new RectangleF(new PointF(Location.X, Location.Y - Size.Height / 2 - 1), Size));
            g.DrawString(Text, Font, new SolidBrush(Color), Location, Format);
        }

    }
}
