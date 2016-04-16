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
    public class ItemsBox : ListView
    {
        public ItemsBox()
        {
            this.View = View.Details;
            /*
            this.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] 
            {
                this.TypeIcon,
                this.Code,
                this.Description,
                this.Source
            });*/
        }
        /*
        private System.Windows.Forms.ColumnHeader TypeIcon;
        private System.Windows.Forms.ColumnHeader Source;
        private System.Windows.Forms.ColumnHeader Description;
        private System.Windows.Forms.ColumnHeader Code;
        */
    }
}
