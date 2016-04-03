using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using Shyu.Core;

namespace Shyu.UI.Drawing
{
    public class QuickTag
    {
        #region Variables
        public double Value = 0;
        public Dictionary<string, TextLine> Line = new Dictionary<string, TextLine>();

        // Box Size Settings
        public bool FixBoxSize = false;
        public int BoxWidth = 0;
        public int BoxHeight = 0;

        // Location Settings
        public Point Location = Point.Empty;

        // Shape and Color Settings
        public TagShape Shape = TagShape.None;
        public int CornerSize = 1;
        public int EdgeSize = 1;
        public int Margin = 1;
        public int ArrowLength = 4;
        public int ArrowWidth = 8;
        public Color EdgeColor = Color.Navy;
        public Color FillColor = Color.SkyBlue;

        // Calculations Out
        public int FinalWidth { get; private set; }
        public int FinalHeight { get; private set; }

        private float CtX = 0; //{ get; private set; }
        private float CtY = 0; // { get; private set; }
        private PointF[] boxPoints = new PointF[4];
        #endregion
        public void Draw(Graphics g)
        {
            if (boxPoints != null)
            {
                int X = Location.X;
                int Y = Location.Y;

                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                StringFormat strFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                PointF point = new PointF(CtX + X, CtY + Y);

                PointF[] DrawPoints = new PointF[boxPoints.Length];
                Array.Copy(boxPoints, DrawPoints, boxPoints.Length);

                for (int i = 0; i < DrawPoints.Length; i++)
                {
                    DrawPoints[i].X += X;
                    DrawPoints[i].Y += Y;
                }
                g.FillPolygon(new SolidBrush(FillColor), DrawPoints);
                g.DrawPolygon(new Pen(EdgeColor, EdgeSize), DrawPoints);

                bool IsFirstLine = true;
                float NextBase = 0;
                for (int i = 0; i < Line.Count; i++)
                {
                    TextLine ln = Line.ElementAt(i).Value;
                    string n = Line.ElementAt(i).Key;
                    if (ln.Visible)
                    {
                        point.X -= ln.Location.X;

                        if (IsFirstLine)
                        {
                            point.Y = uConv.Round(point.Y - BoxHeight / 2 + ln.OffsetY / 2 - ln.Location.Y);
                            NextBase = CtY + Y - BoxHeight / 2 + ln.OffsetY + ln.LineSpace;
                            IsFirstLine = false;
                        }
                        else
                        {
                            point.Y = uConv.Round(NextBase + ln.OffsetY / 2 - ln.Location.Y);
                            NextBase += ln.OffsetY + ln.LineSpace;
                        }

                        g.DrawString(ln.Text, ln.Font, new SolidBrush(ln.Color), point, strFormat);

                    }
                    Line[n] = ln;
                }
            }
        }
        public void GetShape()
        {
            Bitmap bmp = new Bitmap((EdgeSize + Margin) * 2, (EdgeSize + Margin) * 2);
            Graphics g = Graphics.FromImage(bmp);
            GetShape(g);
        }
        public void GetShape(Graphics g)
        {
            float TmpBoxWidth = (EdgeSize + Margin) * 2;
            float TmpBoxHeight = (EdgeSize + Margin) * 2;

            for (int i = 0; i < Line.Count; i++)
            {
                TextLine ln = Line.ElementAt(i).Value;
                string n = Line.ElementAt(i).Key;
                if (ln.Visible)
                {
                    SizeF sf = g.MeasureString(ln.Text, ln.Font);
                    float w = sf.Width - ln.Size.Width + (EdgeSize + Margin) * 2;
                    float h = sf.Height - ln.Size.Height + ln.LineSpace;
                    if (TmpBoxWidth < w) TmpBoxWidth = w;
                    ln.OffsetY = h;
                    TmpBoxHeight += h;
                }
                Line[n] = ln;
            }

            if (!FixBoxSize)
            {
                BoxWidth = uConv.Round(TmpBoxWidth);
                BoxHeight = uConv.Round(TmpBoxHeight);
            }

            switch (Shape)
            {
                case (TagShape.None):
                    CtX = 0;
                    CtY = 0;
                    FinalWidth = BoxWidth + 1;
                    FinalHeight = BoxHeight + 1;
                    boxPoints = new PointF[8];
                    boxPoints[0].X = CtX - BoxWidth / 2;
                    boxPoints[0].Y = CtY - BoxHeight / 2 + CornerSize;
                    boxPoints[1].X = CtX - BoxWidth / 2 + CornerSize;
                    boxPoints[1].Y = CtY - BoxHeight / 2;
                    boxPoints[2].X = CtX + BoxWidth / 2 - CornerSize;
                    boxPoints[2].Y = CtY - BoxHeight / 2;
                    boxPoints[3].X = CtX + BoxWidth / 2;
                    boxPoints[3].Y = CtY - BoxHeight / 2 + CornerSize;
                    boxPoints[4].X = CtX + BoxWidth / 2;
                    boxPoints[4].Y = CtY + BoxHeight / 2 - CornerSize;
                    boxPoints[5].X = CtX + BoxWidth / 2 - CornerSize;
                    boxPoints[5].Y = CtY + BoxHeight / 2;
                    boxPoints[6].X = CtX - BoxWidth / 2 + CornerSize;
                    boxPoints[6].Y = CtY + BoxHeight / 2;
                    boxPoints[7].X = CtX - BoxWidth / 2;
                    boxPoints[7].Y = CtY + BoxHeight / 2 - CornerSize;
                    break;

                case (TagShape.Left):
                    CtX = ArrowLength + (BoxWidth / 2);
                    CtY = 0;
                    FinalWidth = BoxWidth + ArrowLength + 1;
                    FinalHeight = BoxHeight + 1;
                    boxPoints = new PointF[11];
                    boxPoints[0].X = CtX - BoxWidth / 2;
                    boxPoints[0].Y = CtY - BoxHeight / 2 + CornerSize;
                    boxPoints[1].X = CtX - BoxWidth / 2 + CornerSize;
                    boxPoints[1].Y = CtY - BoxHeight / 2;
                    boxPoints[2].X = CtX + BoxWidth / 2 - CornerSize;
                    boxPoints[2].Y = CtY - BoxHeight / 2;
                    boxPoints[3].X = CtX + BoxWidth / 2;
                    boxPoints[3].Y = CtY - BoxHeight / 2 + CornerSize;
                    boxPoints[4].X = CtX + BoxWidth / 2;
                    boxPoints[4].Y = CtY + BoxHeight / 2 - CornerSize;
                    boxPoints[5].X = CtX + BoxWidth / 2 - CornerSize;
                    boxPoints[5].Y = CtY + BoxHeight / 2;
                    boxPoints[6].X = CtX - BoxWidth / 2 + CornerSize;
                    boxPoints[6].Y = CtY + BoxHeight / 2;
                    boxPoints[7].X = CtX - BoxWidth / 2;
                    boxPoints[7].Y = CtY + BoxHeight / 2 - CornerSize;
                    boxPoints[8].X = ArrowLength;
                    boxPoints[8].Y = CtY + ArrowWidth / 2;
                    boxPoints[9].X = 0;
                    boxPoints[9].Y = 0;
                    boxPoints[10].X = ArrowLength;
                    boxPoints[10].Y = CtY - ArrowWidth / 2;
                    break;

                case (TagShape.LeftNarrow):
                    CtX = ArrowLength + (BoxWidth / 2) - 1;
                    CtY = 0;
                    FinalWidth = BoxWidth + ArrowLength - 1;
                    FinalHeight = BoxHeight + 1;
                    boxPoints = new PointF[7];
                    boxPoints[0].X = CtX - BoxWidth / 2 + CornerSize;
                    boxPoints[0].Y = CtY - BoxHeight / 2;
                    boxPoints[1].X = CtX + BoxWidth / 2 - CornerSize;
                    boxPoints[1].Y = CtY - BoxHeight / 2;
                    boxPoints[2].X = CtX + BoxWidth / 2;
                    boxPoints[2].Y = CtY - BoxHeight / 2 + CornerSize;
                    boxPoints[3].X = CtX + BoxWidth / 2;
                    boxPoints[3].Y = CtY + BoxHeight / 2 - CornerSize;
                    boxPoints[4].X = CtX + BoxWidth / 2 - CornerSize;
                    boxPoints[4].Y = CtY + BoxHeight / 2;
                    boxPoints[5].X = CtX - BoxWidth / 2 + CornerSize;
                    boxPoints[5].Y = CtY + BoxHeight / 2;
                    boxPoints[6].X = 0;
                    boxPoints[6].Y = 0;
                    break;

                case (TagShape.Right):
                    CtX = -(BoxWidth / 2) - ArrowLength;
                    CtY = 0;
                    FinalWidth = BoxWidth + ArrowLength + 1;
                    FinalHeight = BoxHeight + 1;
                    boxPoints = new PointF[11];
                    boxPoints[0].X = CtX - BoxWidth / 2;
                    boxPoints[0].Y = CtY - BoxHeight / 2 + CornerSize;
                    boxPoints[1].X = CtX - BoxWidth / 2 + CornerSize;
                    boxPoints[1].Y = CtY - BoxHeight / 2;
                    boxPoints[2].X = CtX + BoxWidth / 2 - CornerSize;
                    boxPoints[2].Y = CtY - BoxHeight / 2;
                    boxPoints[3].X = CtX + BoxWidth / 2;
                    boxPoints[3].Y = CtY - BoxHeight / 2 + CornerSize;
                    boxPoints[4].X = CtX + BoxWidth / 2;
                    boxPoints[4].Y = CtY - ArrowWidth / 2;
                    boxPoints[5].X = 0;
                    boxPoints[5].Y = 0;
                    boxPoints[6].X = CtX + BoxWidth / 2;
                    boxPoints[6].Y = CtY + ArrowWidth / 2;
                    boxPoints[7].X = CtX + BoxWidth / 2;
                    boxPoints[7].Y = CtY + BoxHeight / 2 - CornerSize;
                    boxPoints[8].X = CtX + BoxWidth / 2 - CornerSize;
                    boxPoints[8].Y = CtY + BoxHeight / 2;
                    boxPoints[9].X = CtX - BoxWidth / 2 + CornerSize;
                    boxPoints[9].Y = CtY + BoxHeight / 2;
                    boxPoints[10].X = CtX - BoxWidth / 2;
                    boxPoints[10].Y = CtY + BoxHeight / 2 - CornerSize;
                    break;

                case (TagShape.RightNarrow):
                    CtX = -(BoxWidth / 2) - ArrowLength;
                    CtY = 0;
                    FinalWidth = BoxWidth + ArrowLength + 1;
                    FinalHeight = BoxHeight + 1;
                    boxPoints = new PointF[7];
                    boxPoints[0].X = CtX - BoxWidth / 2;
                    boxPoints[0].Y = CtY - BoxHeight / 2 + CornerSize;
                    boxPoints[1].X = CtX - BoxWidth / 2 + CornerSize;
                    boxPoints[1].Y = CtY - BoxHeight / 2;
                    boxPoints[2].X = CtX + BoxWidth / 2 - CornerSize;
                    boxPoints[2].Y = CtY - BoxHeight / 2;
                    boxPoints[3].X = 0;
                    boxPoints[3].Y = 0;
                    boxPoints[4].X = CtX + BoxWidth / 2 - CornerSize;
                    boxPoints[4].Y = CtY + BoxHeight / 2;
                    boxPoints[5].X = CtX - BoxWidth / 2 + CornerSize;
                    boxPoints[5].Y = CtY + BoxHeight / 2;
                    boxPoints[6].X = CtX - BoxWidth / 2;
                    boxPoints[6].Y = CtY + BoxHeight / 2 - CornerSize;
                    break;

                case (TagShape.Up):
                    CtX = 0;
                    CtY = (BoxHeight / 2) + ArrowLength;
                    FinalWidth = BoxWidth + 1;
                    FinalHeight = BoxHeight + ArrowLength + 1;
                    boxPoints = new PointF[11];
                    boxPoints[0].X = CtX - BoxWidth / 2;
                    boxPoints[0].Y = CtY - BoxHeight / 2 + CornerSize;
                    boxPoints[1].X = CtX - BoxWidth / 2 + CornerSize;
                    boxPoints[1].Y = CtY - BoxHeight / 2;
                    boxPoints[2].X = -ArrowWidth / 2;
                    boxPoints[2].Y = ArrowLength;
                    boxPoints[3].X = 0;
                    boxPoints[3].Y = 0;
                    boxPoints[4].X = ArrowWidth / 2;
                    boxPoints[4].Y = ArrowLength;
                    boxPoints[5].X = CtX + BoxWidth / 2 - CornerSize;
                    boxPoints[5].Y = CtY - BoxHeight / 2;
                    boxPoints[6].X = CtX + BoxWidth / 2;
                    boxPoints[6].Y = CtY - BoxHeight / 2 + CornerSize;
                    boxPoints[7].X = CtX + BoxWidth / 2;
                    boxPoints[7].Y = CtY + BoxHeight / 2 - CornerSize;
                    boxPoints[8].X = CtX + BoxWidth / 2 - CornerSize;
                    boxPoints[8].Y = CtY + BoxHeight / 2;
                    boxPoints[9].X = CtX - BoxWidth / 2 + CornerSize;
                    boxPoints[9].Y = CtY + BoxHeight / 2;
                    boxPoints[10].X = CtX - BoxWidth / 2;
                    boxPoints[10].Y = CtY + BoxHeight / 2 - CornerSize;
                    break;

                case (TagShape.Down):
                    CtX = 0;
                    CtY = -(BoxHeight / 2) - ArrowLength;
                    FinalWidth = BoxWidth + 1;
                    FinalHeight = BoxHeight + ArrowLength + 1;
                    boxPoints = new PointF[11];
                    boxPoints[0].X = CtX - BoxWidth / 2;
                    boxPoints[0].Y = CtY - BoxHeight / 2 + CornerSize;
                    boxPoints[1].X = CtX - BoxWidth / 2 + CornerSize;
                    boxPoints[1].Y = CtY - BoxHeight / 2;
                    boxPoints[2].X = CtX + BoxWidth / 2 - CornerSize;
                    boxPoints[2].Y = CtY - BoxHeight / 2;
                    boxPoints[3].X = CtX + BoxWidth / 2;
                    boxPoints[3].Y = CtY - BoxHeight / 2 + CornerSize;
                    boxPoints[4].X = CtX + BoxWidth / 2;
                    boxPoints[4].Y = CtY + BoxHeight / 2 - CornerSize;
                    boxPoints[5].X = CtX + BoxWidth / 2 - CornerSize;
                    boxPoints[5].Y = CtY + BoxHeight / 2;
                    boxPoints[6].X = ArrowWidth / 2;
                    boxPoints[6].Y = -ArrowLength;
                    boxPoints[7].X = 0;
                    boxPoints[7].Y = 0;
                    boxPoints[8].X = -ArrowWidth / 2;
                    boxPoints[8].Y = -ArrowLength;
                    boxPoints[9].X = CtX - BoxWidth / 2 + CornerSize;
                    boxPoints[9].Y = CtY + BoxHeight / 2;
                    boxPoints[10].X = CtX - BoxWidth / 2;
                    boxPoints[10].Y = CtY + BoxHeight / 2 - CornerSize;
                    break;

                case (TagShape.UpDown):
                    CtX = 0;
                    CtY = 0;
                    FinalWidth = BoxWidth + 1;
                    FinalHeight = BoxHeight + ArrowLength + ArrowLength + 1;
                    boxPoints = new PointF[14];
                    boxPoints[0].X = CtX - BoxWidth / 2;
                    boxPoints[0].Y = CtY - BoxHeight / 2 + CornerSize;
                    boxPoints[1].X = CtX - BoxWidth / 2 + CornerSize;
                    boxPoints[1].Y = CtY - BoxHeight / 2;
                    boxPoints[2].X = CtX - ArrowWidth / 2;
                    boxPoints[2].Y = CtY - BoxHeight / 2;
                    boxPoints[3].X = CtX;
                    boxPoints[3].Y = CtY - BoxHeight / 2 - ArrowLength;
                    boxPoints[4].X = ArrowWidth / 2;
                    boxPoints[4].Y = CtY - BoxHeight / 2;
                    boxPoints[5].X = CtX + BoxWidth / 2 - CornerSize;
                    boxPoints[5].Y = CtY - BoxHeight / 2;
                    boxPoints[6].X = CtX + BoxWidth / 2;
                    boxPoints[6].Y = CtY - BoxHeight / 2 + CornerSize;
                    boxPoints[7].X = CtX + BoxWidth / 2;
                    boxPoints[7].Y = CtY + BoxHeight / 2 - CornerSize;
                    boxPoints[8].X = CtX + BoxWidth / 2 - CornerSize;
                    boxPoints[8].Y = CtY + BoxHeight / 2;
                    boxPoints[9].X = CtX + ArrowWidth / 2;
                    boxPoints[9].Y = CtY + BoxHeight / 2;
                    boxPoints[10].X = CtX;
                    boxPoints[10].Y = CtY + BoxHeight / 2 + ArrowLength;
                    boxPoints[11].X = CtX - ArrowWidth / 2;
                    boxPoints[11].Y = CtY + BoxHeight / 2;
                    boxPoints[12].X = CtX - BoxWidth / 2 + CornerSize;
                    boxPoints[12].Y = CtY + BoxHeight / 2;
                    boxPoints[13].X = CtX - BoxWidth / 2;
                    boxPoints[13].Y = CtY + BoxHeight / 2 - CornerSize;
                    break;

                default:
                    break;
            }
        }
        public enum TagShape
        {
            None,
            Left,
            Right,
            LeftNarrow,
            RightNarrow,
            Up,
            Down,
            UpDown
        }

    }
}
