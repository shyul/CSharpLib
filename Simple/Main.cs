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
    public partial class SimpleMainForm : Form
    {
        TechDataBase tdb = new TechDataBase();

        public SimpleMainForm()
        {
            InitializeComponent();
            tdb = new TechDataBase();
            tdb.LoadDataWorker = new BackgroundWorker();
            tdb.Message = Message;
            tdb.StockCountry = StockCountry.US;
            tdb.LoadDataWorker.WorkerReportsProgress = true;
            tdb.LoadDataWorker.WorkerSupportsCancellation = true;
            tdb.LoadDataWorker.DoWork += new DoWorkEventHandler(tdb.LoadDataWorker_DoWork);
            tdb.LoadDataWorker.ProgressChanged += new ProgressChangedEventHandler(this.LoadDataWorker_ProgressChanged);
            tdb.LoadDataWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.LoadDataWorker_RunWorkerCompleted);
            MessageWorker.RunWorkerAsync();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == "Start")
            {
                DialogFileOpen.DefaultExt = "csv";
                DialogFileOpen.Filter = "EOD Data Set|*.csv";
                DialogFileOpen.Title = "Open EOD Data Set";
                if (DialogFileOpen.ShowDialog() == DialogResult.OK && DialogFileOpen.FileName != string.Empty)// && DialogFileOpen.DialogResult == DialogResult.OK)
                {
                    Status.Clear();
                    tdb.EODInputFile = new FileInfo(DialogFileOpen.FileName);
                    PrintInfo("Load new file " + tdb.EODInputFile.FullName);
                    tdb.CancelPending = false;
                    tdb.LoadDataWorker.RunWorkerAsync();
                    btnStart.Text = "Stop";
                }
            }
            else if (btnStart.Text == "Stop")
            {
                tdb.CancelPending = true;
                PrintInfo("Cancelled");
                File.WriteAllText(@"d:\temp.txt", Status.Text);
                btnStart.Text = "Start";
            }
            else
            {
                tdb.CancelPending = true;
                btnStart.Text = "Start";
            }
        }

        private void btnMainData_Click(object sender, EventArgs e)
        {
            GridForm g = new GridForm();
            g.Grid.DataSource = tdb.Table_EOD;
            g.Show();
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            GridForm g = new GridForm();
            g.Grid.DataSource = tdb.Table_Ratios;
            g.Show();
        }

        private void SimpleMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            tdb.LoadDataWorker.CancelAsync();
            MessageWorker.CancelAsync();
            
        }

        private void LoadDataWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void LoadDataWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = 0;
            tdb.CancelPending = true;
            btnStart.Text = "Start";
            PrintInfo("Done.");
        }
    }
}
