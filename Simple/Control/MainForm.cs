using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shyu.UserControl
{
    [DesignerCategory("code")]
    public partial class MainForm : Form
    {
        private Panel TitlePanel;
        private Label TitleLabel;

        public MainForm()
        {
            TitlePanel = new Panel();
            TitleLabel = new Label();

            TitleLabel.AutoSize = true;
            TitleLabel.Font = new Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            TitleLabel.ForeColor = Color.White;
            TitleLabel.Location = new Point(5, 7);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.TabIndex = 0;
            TitleLabel.Text = "TechChart";

            TitlePanel.BackColor = System.Drawing.Color.Firebrick;
            TitlePanel.Controls.Add(this.TitleLabel);
            TitlePanel.Height = 30;
            TitlePanel.Width = this.Width;
            TitlePanel.Location = this.Location;
            TitlePanel.Dock = DockStyle.Top;
            TitlePanel.Name = "TitlePanel";
            TitlePanel.TabIndex = 0;

            Controls.Add(TitlePanel);
            FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 450);
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            Name = "MainForm";
            Text = "Form1";
        }
    }
}
