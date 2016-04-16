namespace Shyu.UI.Forms
{
    partial class ActionPanel
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
            this.toolStripEx1 = new Shyu.UI.Controls.ToolStripEx();
            this.actionBox1 = new Shyu.UI.Controls.ItemsBox();
            this.TypeIcon = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Code = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Source = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // toolStripEx1
            // 
            this.toolStripEx1.Location = new System.Drawing.Point(0, 0);
            this.toolStripEx1.Name = "toolStripEx1";
            this.toolStripEx1.Size = new System.Drawing.Size(1107, 25);
            this.toolStripEx1.TabIndex = 0;
            this.toolStripEx1.Text = "toolStripEx1";
            // 
            // actionBox1
            // 
            this.actionBox1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TypeIcon,
            this.Code,
            this.Description,
            this.Source});
            this.actionBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionBox1.Location = new System.Drawing.Point(0, 25);
            this.actionBox1.Name = "actionBox1";
            this.actionBox1.Size = new System.Drawing.Size(1107, 304);
            this.actionBox1.TabIndex = 1;
            this.actionBox1.UseCompatibleStateImageBehavior = false;
            this.actionBox1.View = System.Windows.Forms.View.Details;
            // 
            // TypeIcon
            // 
            this.TypeIcon.Text = "";
            this.TypeIcon.Width = 30;
            // 
            // Code
            // 
            this.Code.Text = "Code";
            this.Code.Width = 80;
            // 
            // Description
            // 
            this.Description.Text = "Description";
            this.Description.Width = 688;
            // 
            // Source
            // 
            this.Source.DisplayIndex = 1;
            this.Source.Text = "Source";
            this.Source.Width = 72;
            // 
            // ActionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1107, 329);
            this.Controls.Add(this.actionBox1);
            this.Controls.Add(this.toolStripEx1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ActionPanel";
            this.Text = "ActionPanel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.ToolStripEx toolStripEx1;
        private Controls.ItemsBox actionBox1;
        private System.Windows.Forms.ColumnHeader TypeIcon;
        private System.Windows.Forms.ColumnHeader Source;
        private System.Windows.Forms.ColumnHeader Description;
        private System.Windows.Forms.ColumnHeader Code;
    }
}