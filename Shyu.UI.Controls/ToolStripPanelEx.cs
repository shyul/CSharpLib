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
    public class ToolStripPanelEx : ToolStripPanel
    {
        public ToolStripPanelEx()
        {
            DoubleBuffered = true;
            BackColor = Color.White;
            Renderer = new ToolStripRenderer(BackColor);
        }
    }
}
