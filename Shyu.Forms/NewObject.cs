using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shyu.UI.Forms
{
    public partial class NewObject : Form
    {
        public NewObject()
        {
            InitializeComponent();
        }
        private void PropertyGrid_ValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            PropertyGrid.Refresh();
        }
    }
}
