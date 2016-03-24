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
        public SimpleMainForm()
        {
            InitializeComponent();
            MessageWorker.RunWorkerAsync();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            DialogFileOpen.DefaultExt = "csv";
            DialogFileOpen.Filter = "EOD Data Set|*.csv";
            DialogFileOpen.Title = "Open EOD Data Set";
            DialogFileOpen.ShowDialog();
            if (DialogFileOpen.FileName != string.Empty)
            {
                Status.Clear();
                EOD_File = new FileInfo(DialogFileOpen.FileName);
                PrintInfo("Load new file " + EOD_File.FullName);
                LoadDataWorker.RunWorkerAsync();
            }
        }

        private void btnMainData_Click(object sender, EventArgs e)
        {
            GridForm g = new GridForm();
            g.Grid.DataSource = t;
            g.Show();
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            GridForm g = new GridForm();
            g.Grid.DataSource = Calendar;
            g.Show();
        }

        private void SimpleMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            LoadDataWorker.CancelAsync();
            MessageWorker.CancelAsync();
        }
    }
}
