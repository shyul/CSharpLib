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
        public BackgroundWorker OutputMessageWorker = new BackgroundWorker();
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
            OutputMessageWorker.WorkerReportsProgress = true;
            OutputMessageWorker.WorkerSupportsCancellation = true;
            OutputMessageWorker.DoWork += new DoWorkEventHandler(OutputMessageWorker_DoWork);
            OutputMessageWorker.ProgressChanged += new ProgressChangedEventHandler(OutputMessageWorker_ProgressChanged);
            OutputMessageWorker.RunWorkerAsync();
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
        private void OutputMessageWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!StopOutputMessageWorker)
            {
                if (OutputMessage.Count > 0) OutputMessageWorker.ReportProgress(1);
                Thread.Sleep(50);
            }
        }
        private void OutputMessageWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage != 0)
                while (OutputMessage.Count > 0 && !StopOutputMessageWorker)
                {
                    AppendText(OutputMessage.Dequeue());
                }
        }
    }
}
