namespace ConnectorTool.Forms
{
    partial class PositionPickerForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelHint;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelHint = new System.Windows.Forms.Panel();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelHint.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHint
            // 
            this.panelHint.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelHint.BackColor = System.Drawing.Color.White;
            this.panelHint.Controls.Add(this.lblSubtitle);
            this.panelHint.Controls.Add(this.lblTitle);
            this.panelHint.Location = new System.Drawing.Point(406, 228);
            this.panelHint.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panelHint.Name = "panelHint";
            this.panelHint.Size = new System.Drawing.Size(753, 158);
            this.panelHint.TabIndex = 0;
            this.panelHint.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandlePickerMouseDown);
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(92)))), ((int)(((byte)(122)))));
            this.lblSubtitle.Location = new System.Drawing.Point(0, 65);
            this.lblSubtitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Padding = new System.Windows.Forms.Padding(29, 0, 29, 0);
            this.lblSubtitle.Size = new System.Drawing.Size(753, 93);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "请把鼠标移动到游戏目标位置，然后单击一次进行记录。\r\n按 Esc 取消，不会覆盖已有坐标。";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSubtitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandlePickerMouseDown);
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(71)))), ((int)(((byte)(133)))));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(15, 13, 15, 0);
            this.lblTitle.Size = new System.Drawing.Size(753, 65);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "选择连点位置";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandlePickerMouseDown);
            // 
            // PositionPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1564, 756);
            this.Controls.Add(this.panelHint);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "PositionPickerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PositionPickerForm";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandlePickerMouseDown);
            this.panelHint.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
