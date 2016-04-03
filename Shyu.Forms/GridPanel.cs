using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shyu.Core;
using DigitalRune.Windows.Docking;

namespace Shyu.UI.Forms
{
    public partial class GridPanel : DockableForm
    {
        public GridPanel()
        {
            InitializeComponent();
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (Grid.Columns[e.ColumnIndex].Name == "EID" && e.Value != null)
            {
                try
                {
                    e.Value = uConv.EIDToTime(Convert.ToInt64(e.Value.ToString())).ToString("MM-dd-yyyy");
                }
                catch { }
            }
        }
    }
}
