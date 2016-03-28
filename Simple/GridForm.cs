using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shyu
{
    public partial class GridForm : Form
    {
        public GridForm()
        {
            InitializeComponent();
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //MessageBox.Show(e.Value.ToString());
            if (Grid.Columns[e.ColumnIndex].Name == "EID")
            {
                if (e != null)
                {
                    if (e.Value != null)
                    {
                        try
                        {
                            // Map what the user typed into UTC.
                            e.Value = uConv.EIDToTime(Convert.ToInt64(e.Value.ToString())).ToString("MM-dd-yyyy");
                            // Set the ParsingApplied property to 
                            // Show the event is handled.
                            //e.ParsingApplied = true;

                        }
                        catch (FormatException)
                        {
                            // Set to false in case another CellParsing handler
                            // wants to try to parse this DataGridViewCellParsingEventArgs instance.
                            //e.ParsingApplied = false;
                        }
                    }
                }
            }
        }
    }
}
