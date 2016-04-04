using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Shyu.UI.Controls
{
    public class GetToolStripButton
    {
        private static ToolStripButton GetButton(string Text, Bitmap image, EventHandler eh)
        {
            ToolStripButton tsBtn = new ToolStripButton();
            tsBtn.Name = "tsBtn" + Text.Replace(" ", "");
            tsBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsBtn.ImageTransparentColor = Color.Magenta;
            tsBtn.Image = image;
            tsBtn.Text = Text;
            tsBtn.Click += eh;
            return tsBtn;
        }
        public static ToolStripButton Delete(EventHandler eh)
        {
            return GetButton("Delete", Properties.Resources.common_delete, eh);
        }
        public static ToolStripButton MoveUp(EventHandler eh)
        {
            return GetButton("Move Up", Properties.Resources.arrow_090, eh);
        }
        public static ToolStripButton MoveDown(EventHandler eh)
        {
            return GetButton("Move Down", Properties.Resources.arrow_270, eh);
        }
    }
}
