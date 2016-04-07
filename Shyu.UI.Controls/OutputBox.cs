using Shyu.Core;
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
    [System.ComponentModel.DesignerCategory("code")]
    public class OutputBox : RichTextBox
    {
        public Queue<string> OutputMessage = new Queue<string>();
        public BackgroundWorker OutputMessageWorker = new BackgroundWorker();
        public bool StopOutputMessageWorker = false;

        public OutputBox()
        {
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
