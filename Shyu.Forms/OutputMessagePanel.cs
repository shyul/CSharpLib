using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DigitalRune.Windows.Docking;

namespace Shyu.UI.Forms
{
    public partial class OutputMessagePanel : DockableForm
    {
        public OutputMessagePanel()
        {
            InitializeComponent();
            OutputMessageWorker.RunWorkerAsync();
        }

        public bool StopOutputMessageWorker = false;

        public Queue<string> OutputMessage = new Queue<string>();
        public void PrintInfo(string Text)
        {
            if (OutputMessage.Count < 100) OutputMessage.Enqueue(" " + Text + "\n");
        }
        public void Clear()
        {
            OutputText.Clear();
        }
        public void SaveText(FileInfo TextFile)
        {
            File.WriteAllText(TextFile.FullName, OutputText.Text);
        }

        private void OutputText_SizeChanged(object sender, EventArgs e)
        {
            OutputText.SelectionStart = OutputText.Text.Length;
            OutputText.ScrollToCaret();
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
                    OutputText.AppendText(OutputMessage.Dequeue());
                }
        }

        private void OutputPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopOutputMessageWorker = true;
        }
    }
}
