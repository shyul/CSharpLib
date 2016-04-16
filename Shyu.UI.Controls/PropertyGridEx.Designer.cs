namespace Shyu.UI.Controls
{
    partial class PropertyGridEx
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ToolStrip = new System.Windows.Forms.ToolStrip();
            this.tsSept1 = new System.Windows.Forms.ToolStripSeparator();
            this.Grid = new System.Windows.Forms.PropertyGrid();
            this.tsBtnGridCategorized = new System.Windows.Forms.ToolStripButton();
            this.tsBtnGridAlphabetical = new System.Windows.Forms.ToolStripButton();
            this.ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolStrip
            // 
            this.ToolStrip.AutoSize = false;
            this.ToolStrip.BackColor = System.Drawing.Color.White;
            this.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsBtnGridCategorized,
            this.tsBtnGridAlphabetical,
            this.tsSept1});
            this.ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip.Margin = new System.Windows.Forms.Padding(2);
            this.ToolStrip.Name = "ToolStrip";
            this.ToolStrip.Padding = new System.Windows.Forms.Padding(2, 0, 1, 1);
            this.ToolStrip.Size = new System.Drawing.Size(372, 26);
            this.ToolStrip.TabIndex = 0;
            this.ToolStrip.Text = "toolStrip1";
            // 
            // tsSept1
            // 
            this.tsSept1.Name = "tsSept";
            this.tsSept1.Size = new System.Drawing.Size(6, 26);
            // 
            // Grid
            // 
            this.Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Grid.Location = new System.Drawing.Point(0, 26);
            this.Grid.Name = "Grid";
            this.Grid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.Grid.Size = new System.Drawing.Size(372, 458);
            this.Grid.TabIndex = 1;
            this.Grid.ToolbarVisible = false;
            // 
            // tsBtnGridCategorized
            // 
            this.tsBtnGridCategorized.AutoSize = false;
            this.tsBtnGridCategorized.Checked = true;
            this.tsBtnGridCategorized.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsBtnGridCategorized.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnGridCategorized.Image = global::Shyu.UI.Controls.Properties.Resources.Order_ByCategory;
            this.tsBtnGridCategorized.Name = "tsBtnGridCategorized";
            this.tsBtnGridCategorized.Size = new System.Drawing.Size(24, 24);
            this.tsBtnGridCategorized.Text = "Sort Categorized";
            this.tsBtnGridCategorized.Click += new System.EventHandler(this.tsBtnGridCategorized_Click);
            // 
            // tsBtnGridAlphabetical
            // 
            this.tsBtnGridAlphabetical.AutoSize = false;
            this.tsBtnGridAlphabetical.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnGridAlphabetical.Image = global::Shyu.UI.Controls.Properties.Resources.Order_ByAplha;
            this.tsBtnGridAlphabetical.Name = "tsBtnGridAlphabetical";
            this.tsBtnGridAlphabetical.Size = new System.Drawing.Size(24, 24);
            this.tsBtnGridAlphabetical.Text = "Sort Alphabetical";
            this.tsBtnGridAlphabetical.Click += new System.EventHandler(this.tsBtnGridAlphabetical_Click);
            // 
            // PropertyGridEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Grid);
            this.Controls.Add(this.ToolStrip);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PropertyGridEx";
            this.Size = new System.Drawing.Size(372, 484);
            this.ToolStrip.ResumeLayout(false);
            this.ToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolStripButton tsBtnGridCategorized;
        private System.Windows.Forms.ToolStripButton tsBtnGridAlphabetical;
        private System.Windows.Forms.ToolStripSeparator tsSept1;
        public System.Windows.Forms.PropertyGrid Grid;
        public System.Windows.Forms.ToolStrip ToolStrip;
    }
}
