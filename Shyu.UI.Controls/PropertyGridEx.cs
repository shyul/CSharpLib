using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shyu.UI.Controls
{
    public partial class PropertyGridEx : UserControl
    {
        public override Color BackColor
        {
            get
            {
                return _BackColor;
            }
            set
            {
                _BackColor = value;
                ToolStrip.BackColor = value;
                Grid.BackColor = value;
            }
        }

        public Color _BackColor;

        public PropertyGridEx()
        {
            InitializeComponent();
            ToolStrip.Renderer = new ToolStripRenderer(this.BackColor);
        }
        public void AddStripItems(ToolStripItem[] tsItem)
        {
            for (int i = 0; i < tsItem.Length; i++)
            {
                tsItem[i].AutoSize = false;
                if (tsItem[i].GetType() == typeof(ToolStripButton))
                    tsItem[i].Size = new Size(24, 24);
                else if (tsItem[i].GetType() == typeof(ToolStripSeparator))
                    tsItem[i].Size = new Size(6, 26);
                else
                    tsItem[i].Size = new Size(tsItem[i].Size.Width, 24);
            }
            ToolStrip.Items.AddRange(tsItem);
        }
        private void tsBtnGridCategorized_Click(object sender, EventArgs e)
        {
            if (!tsBtnGridCategorized.Checked)
            {
                tsBtnGridAlphabetical.Checked = false;
                tsBtnGridCategorized.Checked = true;
                Grid.PropertySort = PropertySort.Categorized;
                Grid.Refresh();
            }
        }
        private void tsBtnGridAlphabetical_Click(object sender, EventArgs e)
        {
            if (!tsBtnGridAlphabetical.Checked)
            {
                tsBtnGridAlphabetical.Checked = true;
                tsBtnGridCategorized.Checked = false;
                Grid.PropertySort = PropertySort.Alphabetical;
                Grid.Refresh();
            }
        }
    }
}
