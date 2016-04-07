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
            this.Tools = new Shyu.UI.Controls.ToolStripEx();
            this.Output = new Shyu.UI.Controls.OutputBox();
            this.SuspendLayout();
            // 
            // Tools
            // 
            this.Tools.Location = new System.Drawing.Point(0, 0);
            this.Tools.Name = "Tools";
            this.Tools.Size = new System.Drawing.Size(1034, 25);
            this.Tools.TabIndex = 0;
            this.Tools.Text = "toolStripEx1";
            // 
            // Output
            // 
            this.Output.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Output.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Output.Location = new System.Drawing.Point(0, 25);
            this.Output.Name = "Output";
            this.Output.Size = new System.Drawing.Size(1034, 285);
            this.Output.TabIndex = 1;
            this.Output.Text = "";
            // 
            // OutputMessagePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 310);
            this.Controls.Add(this.Output);
            this.Controls.Add(this.Tools);
            this.DockAreas = ((DigitalRune.Windows.Docking.DockAreas)((DigitalRune.Windows.Docking.DockAreas.Float | DigitalRune.Windows.Docking.DockAreas.Bottom)));
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "OutputMessagePanel";
            this.TabText = "MessagePanel";
            this.Text = "MessagePanel";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OutputPanel_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.ToolStripEx Tools;
        private Controls.OutputBox Output;
    }
}