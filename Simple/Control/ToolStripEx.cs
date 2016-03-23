using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Shyu.UserControl
{
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ToolStripEx : System.Windows.Forms.ToolStrip
    {
        public ToolStripEx()
        {
            Renderer = new CustomToolStripRenderer();
            DoubleBuffered = true;
        }
    }
    public class CustomToolStripRenderer : ToolStripProfessionalRenderer
    {
        public CustomToolStripRenderer() { RoundedEdges = false; }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.White), e.AffectedBounds);
        }
    }
}
