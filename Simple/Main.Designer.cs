namespace Shyu
{
    partial class SimpleMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.Status = new System.Windows.Forms.RichTextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.DialogFileOpen = new System.Windows.Forms.OpenFileDialog();
            this.LoadDataWorker = new System.ComponentModel.BackgroundWorker();
            this.MessageWorker = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnMainData = new System.Windows.Forms.Button();
            this.btnCalendar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // Status
            // 
            this.Status.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Status.Location = new System.Drawing.Point(12, 52);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(440, 424);
            this.Status.TabIndex = 1;
            this.Status.Text = "";
            this.Status.SizeChanged += new System.EventHandler(this.Status_SizeChanged);
            this.Status.TextChanged += new System.EventHandler(this.Status_TextChanged);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(93, 12);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(359, 23);
            this.progressBar.TabIndex = 2;
            // 
            // DialogFileOpen
            // 
            this.DialogFileOpen.FileName = "openFileDialog1";
            // 
            // LoadDataWorker
            // 
            this.LoadDataWorker.WorkerReportsProgress = true;
            this.LoadDataWorker.WorkerSupportsCancellation = true;
            this.LoadDataWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.LoadDataWorker_DoWork);
            this.LoadDataWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.LoadDataWorker_ProgressChanged);
            this.LoadDataWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.LoadDataWorker_RunWorkerCompleted);
            // 
            // MessageWorker
            // 
            this.MessageWorker.WorkerReportsProgress = true;
            this.MessageWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.MessageWorker_DoWork);
            this.MessageWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.MessageWorker_ProgressChanged);
            // 
            // btnMainData
            // 
            this.btnMainData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMainData.Location = new System.Drawing.Point(13, 488);
            this.btnMainData.Name = "btnMainData";
            this.btnMainData.Size = new System.Drawing.Size(75, 23);
            this.btnMainData.TabIndex = 3;
            this.btnMainData.Text = "Main Data";
            this.btnMainData.UseVisualStyleBackColor = true;
            this.btnMainData.Click += new System.EventHandler(this.btnMainData_Click);
            // 
            // btnCalendar
            // 
            this.btnCalendar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCalendar.Location = new System.Drawing.Point(94, 488);
            this.btnCalendar.Name = "btnCalendar";
            this.btnCalendar.Size = new System.Drawing.Size(75, 23);
            this.btnCalendar.TabIndex = 4;
            this.btnCalendar.Text = "Calendar";
            this.btnCalendar.UseVisualStyleBackColor = true;
            this.btnCalendar.Click += new System.EventHandler(this.btnCalendar_Click);
            // 
            // SimpleMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 523);
            this.Controls.Add(this.btnCalendar);
            this.Controls.Add(this.btnMainData);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnStart);
            this.DoubleBuffered = true;
            this.Name = "SimpleMainForm";
            this.Text = "Simple";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SimpleMainForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.RichTextBox Status;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.OpenFileDialog DialogFileOpen;
        private System.ComponentModel.BackgroundWorker LoadDataWorker;
        private System.ComponentModel.BackgroundWorker MessageWorker;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnMainData;
        private System.Windows.Forms.Button btnCalendar;
    }
}

