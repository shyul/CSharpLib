using Shyu.Core;
using Shyu.UI.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shyu.UI.Controls
{
    public enum ConsoleMessageType
    {
        Info = 0,
        Start = 1,
        Stop = 2,
        Done = 3,
        Warning = 4,
        Error = 5,
    }
    public class ConsoleMessage
    {
        public string SourceName = string.Empty;
        public TextInfo Info = new TextInfo();
        public ConsoleMessageType MessageType = ConsoleMessageType.Info;
        public int Code = -1;
        public int DebugLevel = 0;
    }
    public class ConsoleMessageArgs : EventArgs
    {
        public ConsoleMessage cm = new ConsoleMessage();
    }
    public delegate void ConsoleMessageHandler(object sender, ConsoleMessageArgs e);
    [System.ComponentModel.DesignerCategory("code")]
    public class OutputBox : RichTextBox
    {
        public Queue<string> OutputMessage = new Queue<string>();
        public bool StopOutputMessageWorker = false;

        public OutputBox()
        {
            BorderStyle = BorderStyle.None;
            Font = new Font("Courier New", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            BackColor = Color.White;
            ReadOnly = true;
            ScrollBars = RichTextBoxScrollBars.ForcedVertical;
            SizeChanged += new EventHandler(OutputText_SizeChanged);
            TextChanged += new EventHandler(OutputText_SizeChanged);
            Task.Factory.StartNew(() => Output());
        }
        public void PrintInfo(string Text)
        {
            if (OutputMessage.Count < 100) OutputMessage.Enqueue(" " + Text + "\n");
        }
        public void SaveText(FileInfo TextFile)
        {
            File.WriteAllText(TextFile.FullName, Text);
        }
        private void OutputText_SizeChanged(object sender, EventArgs e)
        {
            SelectionStart = Text.Length;
            ScrollToCaret();
        }
        private void Output()
        {
            while (!StopOutputMessageWorker)
            {
                if (OutputMessage.Count > 0)
                {
                    while (OutputMessage.Count > 0 && !StopOutputMessageWorker)
                    {
                        Invoke((MethodInvoker)delegate { AppendText(OutputMessage.Dequeue()); });
                    }
                }
                Thread.Sleep(50);
            }
        }
    }
}
