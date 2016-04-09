using Shyu.Core;
using Shyu.UI.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shyu.UI.Controls
{
    [System.ComponentModel.DesignerCategory("code")]
    public partial class Chart : UserControl
    {
        public Queue<double[]> Input = new Queue<double[]>();

        private double[] Data;

        public Chart()
        {
            components = new System.ComponentModel.Container();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Paint += new PaintEventHandler(PaintTrace);
        }

        private void PaintTrace(Object sender, PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            if (Input.Count > 0) Data = Input.Dequeue();

            Rectangle r = new Rectangle(new Point(0, 0), new Size(Size.Width - 1, Size.Height - 1));
            g.DrawRectangle(new Pen(Color.DimGray, 1), r);

            if (Data != null && Data.Length > 0)
            {

                List<PointF> DrawPts = new List<PointF>();
                DataRange dr = new DataRange(Data);
                float PointWidth = Size.Width / (Data.Length - 1);
                float Value_X = 0;
                for (int i = 0; i < Data.Length; i++)
                {
                    DrawPts.Add(new PointF(Value_X, uDraw.GetPixFromYvalueF(false, Size.Height - 1, 0, dr.Minimum, dr.Maximum, Data[i])));
                    Value_X += PointWidth;
                }
                if (DrawPts.Count > 1)
                {
                    g.DrawLines(new Pen(Color.Red, 1), DrawPts.ToArray());
                }
            }
        }

        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
