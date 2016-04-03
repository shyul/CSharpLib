namespace Shyu.UI.Forms
{
    partial class OutputMessagePanel
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
            this.OutputText = new System.Windows.Forms.RichTextBox();
            this.OutputMessageWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // OutputText
            // 
            this.OutputText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputText.Location = new System.Drawing.Point(0, 0);
            this.OutputText.Name = "OutputText";
            this.OutputText.Size = new System.Drawing.Size(1034, 310);
            this.OutputText.TabIndex = 0;
            this.OutputText.Text = "";
            this.OutputText.SizeChanged += new System.EventHandler(this.OutputText_SizeChanged);
            this.OutputText.TextChanged += new System.EventHandler(this.OutputText_SizeChanged);
            // 
            // OutputMessageWorker
            // 
            this.OutputMessageWorker.WorkerReportsProgress = true;
            this.OutputMessageWorker.WorkerSupportsCancellation = true;
            this.OutputMessageWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.OutputMessageWorker_DoWork);
            this.OutputMessageWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.OutputMessageWorker_ProgressChanged);
            // 
            // OutputMessagePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 310);
            this.Controls.Add(this.OutputText);
            this.DockAreas = ((DigitalRune.Windows.Docking.DockAreas)((DigitalRune.Windows.Docking.DockAreas.Float | DigitalRune.Windows.Docking.DockAreas.Bottom)));
            this.Name = "OutputMessagePanel";
            this.TabText = "MessagePanel";
            this.Text = "MessagePanel";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OutputPanel_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox OutputText;
        public System.ComponentModel.BackgroundWorker OutputMessageWorker;
    }
}