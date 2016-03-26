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

namespace Shyu
{
    public partial class SimpleMainForm
    {
        TechDataBase tdb = new TechDataBase();

        private void LoadDataWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void LoadDataWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = 0;
            PrintInfo("Done.");
        }

    }
}
