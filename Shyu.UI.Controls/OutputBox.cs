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
    public class OutputBox : RichTextBox
    {
        public OutputBox()
        {
            this.SizeChanged += new System.EventHandler(this.OutputText_SizeChanged);
            this.TextChanged += new System.EventHandler(this.OutputText_SizeChanged);
        }
        private void OutputText_SizeChanged(object sender, EventArgs e)
        {
            this.SelectionStart = this.Text.Length;
            this.ScrollToCaret();
        }

    }
}
