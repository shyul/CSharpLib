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
    public static class uDraw
    {
        public static void Candlestick(Graphics g, Color EdgeColor, Color FillColor, 
            int EdgeWidth, float Width, float X, 
            float Close_PixF, float Open_PixF, float Low_PixF, float High_PixF)
        {
            int IntX = uConv.Round(X);
            Pen pen = new Pen(EdgeColor, EdgeWidth);
            g.DrawLine(pen, new Point(IntX, uConv.Round(High_PixF)), new Point(IntX, uConv.Round(Low_PixF)));

            float Y = 0, Height = 0;
            if (Close_PixF >= Open_PixF) Y = Open_PixF; else Y = Close_PixF;
            Height = Math.Abs(Close_PixF - Open_PixF);
            if (Height < 1) Height = 1;
            PointF pointF = new PointF(X - (Width / 2.0f), Y);
            SizeF sizeF = new SizeF(Width, Height);
            g.FillRectangle(new SolidBrush(FillColor), new RectangleF(pointF, sizeF));
            g.DrawRectangle(new Pen(EdgeColor, EdgeWidth), new Rectangle(new Point(uConv.Round(pointF.X), uConv.Round(pointF.Y)), new Size(uConv.Round(sizeF.Width), uConv.Round(sizeF.Height))));
        }

        public static void Column(Graphics g, Color EdgeColor, Color FillColor, 
            int EdgeWidth, float Width, float X, float Y1, float Y2)
        {
            float Y = 0, Height = 1;
            if (Y1 >= Y2) Y = Y2; else Y = Y1;
            Height = Math.Abs(Y1 - Y2);
            if (Height < 1) Height = 1;
            Column(g, EdgeColor, FillColor, EdgeWidth, new PointF(X - (Width / 2.0f), Y), new SizeF(Width, Height));
        }

        public static void Column(Graphics g, Color EdgeColor, Color FillColor, int EdgeWidth, PointF Pt, SizeF Size)
        {
            g.FillRectangle(new SolidBrush(FillColor), new RectangleF(Pt, Size));
            //g.FillRectangle(new SolidBrush(FillColor), new Rectangle(new Point(uConv.Round(Pt.X), uConv.Round(Pt.Y)), new Size(uConv.Round(Size.Width), uConv.Round(Size.Height))));
            g.DrawRectangle(new Pen(EdgeColor, EdgeWidth), new Rectangle(new Point(uConv.Round(Pt.X), uConv.Round(Pt.Y)), new Size(uConv.Round(Size.Width), uConv.Round(Size.Height))));
            //g.DrawRectangle(new Pen(EdgeColor, EdgeWidth), new Rectangle(new Point((int)(Pt.X), (int)(Pt.Y)), new Size((int)(Size.Width), (int)(Size.Height))));
        }
    }

    public static class LineStyle
    {
        public static float[] Solid = new float[] { 1 };
        public static float[] Dot = new float[] { 1, 2 };
        public static float[] Dash = new float[] { 4, 2 };
        public static float[] DotDash = new float[] { 1, 2, 4, 2 };
        public static float[] DashDash = new float[] { 3, 3 };
    }


}
