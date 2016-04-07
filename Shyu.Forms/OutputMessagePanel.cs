using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DigitalRune.Windows.Docking;

namespace Shyu.UI.Forms
{
    public partial class OutputMessagePanel : DockableForm
    {
        public OutputMessagePanel()
        {
            InitializeComponent();
        }

        private void OutputPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Output.StopOutputMessageWorker = true;
        }
    }
}
