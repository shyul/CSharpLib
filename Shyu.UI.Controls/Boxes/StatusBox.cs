using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Shyu.UI.Drawing;

namespace Shyu.UI.Controls
{
    public enum ConsoleStatusType
    {
        Info = 0,
        Start = 1,
        Stop = 2,
        Done = 3,
        Warning = 4,
        Error = 5,
    }
    public class ConsoleStatus
    {
        public string SourceName = string.Empty;
        public TextInfo Info  = new TextInfo();
        public ConsoleStatusType MessageType = ConsoleStatusType.Info;
        public int Code = -1;
        public int DebugLevel = 0;
        public TextInfo[] Details;

    }
    public class ConsoleStatusArgs : EventArgs
    {
        public ConsoleStatus cs = new ConsoleStatus();
    }
    public delegate void ConsoleStatusHandler(object sender, ConsoleStatusArgs e);


    [System.ComponentModel.DesignerCategory("code")]
    public class StatusBox : TreeView
    {
        public int DebugLevel = 0;

        public StatusBox()
        {
            Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            Indent = 18;
            ItemHeight = 18;
            RightToLeft = RightToLeft.No;
            ImageList = new ImageList();
            ImageList.TransparentColor = Color.Transparent;
            ImageList.ColorDepth = ColorDepth.Depth32Bit;
            ImageList.ImageSize = new Size(16, 16);
            ImageList.Images.Add("Info", Properties.Resources.Message_Info);
            ImageList.Images.Add("Start", Properties.Resources.Message_Start);
            ImageList.Images.Add("Stop", Properties.Resources.Message_Stop);
            ImageList.Images.Add("Done", Properties.Resources.Message_Done);
            ImageList.Images.Add("Warning", Properties.Resources.Message_Warning);
            ImageList.Images.Add("Error", Properties.Resources.Message_Error);
            
        }

        public void ShowStatus(object sender, ConsoleStatusArgs e)
        {
            ConsoleStatus m = e.cs;
            if (m.DebugLevel <= DebugLevel)
            {
                TreeNode tr = new TreeNode();
                tr.Text = string.Empty;
                if (m.SourceName != string.Empty)
                    tr.Text = m.SourceName;
                else
                    tr.Text = sender.GetType().Name;

                switch (m.MessageType)
                {
                    case (ConsoleStatusType.Info):
                        tr.Text += " [" + m.Code + "]: ";
                        tr.ImageIndex = 0;
                        break;
                    case (ConsoleStatusType.Start):
                        tr.Text += " [" + m.Code + "]: ";
                        tr.ImageIndex = 1;
                        break;
                    case (ConsoleStatusType.Stop):
                        tr.Text += " [" + m.Code + "]: ";
                        tr.ImageIndex = 2;
                        break;
                    case (ConsoleStatusType.Done):
                        tr.Text += " [" + m.Code + "]: ";
                        tr.ImageIndex = 3;
                        break;
                    case (ConsoleStatusType.Warning):
                        tr.Text += " WARNING [" + m.Code + "]: ";
                        tr.ImageIndex = 4;
                        break;
                    case (ConsoleStatusType.Error):
                        tr.Text += " ERROR [" + m.Code + "]: ";
                        tr.ImageIndex = 5;
                        break;
                    default:
                        throw new NotImplementedException();
                }

                tr.Text += m.Info.Text;
                tr.ForeColor = m.Info.TextColor;
                tr.BackColor = m.Info.BackColor;
                tr.NodeFont = m.Info.Font;

                for (int i = 0; i < m.Details.Length; i++)
                {
                    tr.Nodes.Add(new TreeNode()
                    {
                        Text = m.Details[i].Text,
                        ForeColor = m.Details[i].TextColor,
                        BackColor = m.Details[i].BackColor,
                        NodeFont = m.Details[i].Font
                    });

                }
                Nodes.Add(tr);
            }
        }
    }
}
