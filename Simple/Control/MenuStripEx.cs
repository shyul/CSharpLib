using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shyu.UserControl
{
    public partial class MenuStripEx : System.Windows.Forms.MenuStrip
    {
        public MenuStripEx()
        {
            Renderer = new CustomToolStripRenderer();
            DoubleBuffered = true;
        }
    }
}
