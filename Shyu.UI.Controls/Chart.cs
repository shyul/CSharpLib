using Shyu.Core;
using Shyu.UI.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shyu.UI.Controls
{
    [System.ComponentModel.DesignerCategory("code")]
    public partial class Chart : UserControl
    {
        public Queue<double[]> Input = new Queue<double[]>();
        public BackgroundWorker UpdateWorker = new BackgroundWorker();

        private double[] Data;

        public Chart()
        {
            components = new System.ComponentModel.Container();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            UpdateWorker.WorkerReportsProgress = true;
            UpdateWorker.WorkerSupportsCancellation = true;
            UpdateWorker.DoWork += new DoWorkEventHandler(UpdateWorker_DoWork);
            UpdateWorker.ProgressChanged += new ProgressChangedEventHandler(UpdateWorker_ProgressChanged);
            UpdateWorker.RunWorkerAsync();
            Paint += new PaintEventHandler(PaintTrace);
            SizeChanged += new EventHandler(UpdateGraphics);
        }
        private void UpdateWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (Input.Count > 0) UpdateWorker.ReportProgress(1);
                Thread.Sleep(50);
            }
        }
        private void UpdateWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage != 0)
                while (Input.Count > 0)
                {
                    Data = Input.Dequeue();
                    Invalidate();
                }
        }
        private void UpdateGraphics(object sender, EventArgs e)
        {
            this.Invalidate();      // Call PaintEventHandler for controls
        }
        private void PaintTrace(Object sender, PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Rectangle r = new Rectangle(new Point(0, 0), new Size(Size.Width - 1, Size.Height - 1));
            g.DrawRectangle(new Pen(Color.DimGray, 1), r);

            if (Data != null && Data.Length > 0)
            {

                List<PointF> DrawPts = new List<PointF>();
                DataRange dr = new DataRange(Data);
                float PointWidth = (float)Size.Width / ((float)Data.Length - 1.0f);
                float Value_X = 0;
                for (int i = 0; i < Data.Length; i++)
                {
                    DrawPts.Add(new PointF(Value_X, uDraw.GetPixFromYvalueF(false, Size.Height - 1, 0, dr.Maximum - 100, dr.Maximum, Data[i])));
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
