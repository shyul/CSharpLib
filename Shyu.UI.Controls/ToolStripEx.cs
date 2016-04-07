using Shyu.Core;
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
    public class ToolStripEx : ToolStrip
    {
        public ToolStripEx()
        {
            DoubleBuffered = true;
            Renderer = new ToolStripRenderer(Color.White);
        }
    }
    public class ToolStripRenderer : ToolStripProfessionalRenderer
    {
        private Color BackColor = Color.White;
        public ToolStripRenderer(Color BackColor)
        {
            this.BackColor = BackColor;
            RoundedEdges = false;
        }
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(BackColor), e.AffectedBounds);
        }
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
        }
    }
}
