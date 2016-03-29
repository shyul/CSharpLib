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
            this.MessageWorker = new System.ComponentModel.BackgroundWorker();
            this.btnEOD = new System.Windows.Forms.Button();
            this.btnRatios = new System.Windows.Forms.Button();
            this.tbSymbolName = new System.Windows.Forms.TextBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
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
            this.progressBar.Location = new System.Drawing.Point(94, 12);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(358, 23);
            this.progressBar.TabIndex = 2;
            // 
            // DialogFileOpen
            // 
            this.DialogFileOpen.FileName = "openFileDialog1";
            // 
            // MessageWorker
            // 
            this.MessageWorker.WorkerReportsProgress = true;
            this.MessageWorker.WorkerSupportsCancellation = true;
            this.MessageWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.MessageWorker_DoWork);
            this.MessageWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.MessageWorker_ProgressChanged);
            // 
            // btnEOD
            // 
            this.btnEOD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEOD.Location = new System.Drawing.Point(94, 488);
            this.btnEOD.Name = "btnEOD";
            this.btnEOD.Size = new System.Drawing.Size(75, 23);
            this.btnEOD.TabIndex = 3;
            this.btnEOD.Text = "EOD";
            this.btnEOD.UseVisualStyleBackColor = true;
            this.btnEOD.Click += new System.EventHandler(this.btnEOD_Click);
            // 
            // btnRatios
            // 
            this.btnRatios.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRatios.Location = new System.Drawing.Point(175, 488);
            this.btnRatios.Name = "btnRatios";
            this.btnRatios.Size = new System.Drawing.Size(75, 23);
            this.btnRatios.TabIndex = 4;
            this.btnRatios.Text = "Ratios";
            this.btnRatios.UseVisualStyleBackColor = true;
            this.btnRatios.Click += new System.EventHandler(this.btnRatios_Click);
            // 
            // tbSymbolName
            // 
            this.tbSymbolName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSymbolName.Location = new System.Drawing.Point(13, 489);
            this.tbSymbolName.Name = "tbSymbolName";
            this.tbSymbolName.Size = new System.Drawing.Size(75, 21);
            this.tbSymbolName.TabIndex = 5;
            this.tbSymbolName.Text = "AAPL";
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.Location = new System.Drawing.Point(377, 487);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 6;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(296, 487);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 7;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // SimpleMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 523);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.tbSymbolName);
            this.Controls.Add(this.btnRatios);
            this.Controls.Add(this.btnEOD);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnStart);
            this.DoubleBuffered = true;
            this.Name = "SimpleMainForm";
            this.Text = "Simple";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SimpleMainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.RichTextBox Status;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.OpenFileDialog DialogFileOpen;
        private System.ComponentModel.BackgroundWorker MessageWorker;
        private System.Windows.Forms.Button btnEOD;
        private System.Windows.Forms.Button btnRatios;
        private System.Windows.Forms.TextBox tbSymbolName;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnLoad;
    }
}

