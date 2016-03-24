using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shyu.UserControl;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace Shyu
{
    public partial class SimpleMainForm
    {
        public Queue<string> Message = new Queue<string>();

        private void PrintInfo(string Text)
        {
            if(Message.Count < 100) Message.Enqueue(" " + Text + "\n");
        }

        private void Status_TextChanged(object sender, EventArgs e)
        {
            Status.SelectionStart = Status.Text.Length;
            Status.ScrollToCaret();
        }

        private void Status_SizeChanged(object sender, EventArgs e)
        {
            Status.SelectionStart = Status.Text.Length;
            Status.ScrollToCaret();
        }

        private void MessageWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (Message.Count > 0) MessageWorker.ReportProgress(1);
                else MessageWorker.ReportProgress(0);
                Thread.Sleep(50);
            }
        }
        private void MessageWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage != 0)
                while (Message.Count > 0)
                {
                    if (Status != null) Status.AppendText(Message.Dequeue());
                }
        }
    }
}
