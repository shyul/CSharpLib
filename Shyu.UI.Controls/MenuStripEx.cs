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
    public partial class MenuStripEx : MenuStrip
    {
        public MenuStripEx()
        {
            DoubleBuffered = true;
            Renderer = new OsianoMenuStripRenderer();
        }
    }
    public class OsianoMenuStripRenderer : ToolStripProfessionalRenderer
    {
        public OsianoMenuStripRenderer() { RoundedEdges = false; }
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.White), e.AffectedBounds);
        }
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
        }
    }
}
