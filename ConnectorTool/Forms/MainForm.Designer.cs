namespace ConnectorTool.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelTitleBar;
        private System.Windows.Forms.PictureBox picAppIcon;
        private System.Windows.Forms.Label lblWindowTitle;
        private System.Windows.Forms.Button btnWindowMinimize;
        private System.Windows.Forms.Button btnWindowMaximize;
        private System.Windows.Forms.Button btnWindowClose;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel panelClick;
        private System.Windows.Forms.Panel panelPosition;
        private System.Windows.Forms.Panel panelHotkeys;
        private System.Windows.Forms.Panel panelOptions;
        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.NumericUpDown nudInterval;
        private System.Windows.Forms.Label lblIntervalHint;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.TextBox txtPosition;
        private System.Windows.Forms.Button btnPickPosition;
        private System.Windows.Forms.Label lblPositionHint;
        private System.Windows.Forms.Label lblStartHotkey;
        private System.Windows.Forms.ComboBox cboStartHotkey;
        private System.Windows.Forms.Label lblStopHotkey;
        private System.Windows.Forms.ComboBox cboStopHotkey;
        private System.Windows.Forms.Label lblHotkeyHint;
        private System.Windows.Forms.CheckBox chkRandomPerturbation;
        private System.Windows.Forms.CheckBox chkEnableLogging;
        private System.Windows.Forms.TextBox txtLogDirectory;
        private System.Windows.Forms.Button btnBrowseLogDirectory;
        private System.Windows.Forms.Label lblLogPath;
        private System.Windows.Forms.Label lblOptionsHint;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnMinimize;

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
            this.panelTitleBar = new System.Windows.Forms.Panel();
            this.picAppIcon = new System.Windows.Forms.PictureBox();
            this.btnWindowClose = new System.Windows.Forms.Button();
            this.btnWindowMaximize = new System.Windows.Forms.Button();
            this.btnWindowMinimize = new System.Windows.Forms.Button();
            this.lblWindowTitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panelClick = new System.Windows.Forms.Panel();
            this.lblIntervalHint = new System.Windows.Forms.Label();
            this.nudInterval = new System.Windows.Forms.NumericUpDown();
            this.lblInterval = new System.Windows.Forms.Label();
            this.panelPosition = new System.Windows.Forms.Panel();
            this.lblPositionHint = new System.Windows.Forms.Label();
            this.btnPickPosition = new System.Windows.Forms.Button();
            this.txtPosition = new System.Windows.Forms.TextBox();
            this.lblPosition = new System.Windows.Forms.Label();
            this.panelHotkeys = new System.Windows.Forms.Panel();
            this.lblHotkeyHint = new System.Windows.Forms.Label();
            this.cboStopHotkey = new System.Windows.Forms.ComboBox();
            this.lblStopHotkey = new System.Windows.Forms.Label();
            this.cboStartHotkey = new System.Windows.Forms.ComboBox();
            this.lblStartHotkey = new System.Windows.Forms.Label();
            this.panelOptions = new System.Windows.Forms.Panel();
            this.lblOptionsHint = new System.Windows.Forms.Label();
            this.lblLogPath = new System.Windows.Forms.Label();
            this.btnBrowseLogDirectory = new System.Windows.Forms.Button();
            this.txtLogDirectory = new System.Windows.Forms.TextBox();
            this.chkEnableLogging = new System.Windows.Forms.CheckBox();
            this.chkRandomPerturbation = new System.Windows.Forms.CheckBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.panelTitleBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAppIcon)).BeginInit();
            this.panelClick.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterval)).BeginInit();
            this.panelPosition.SuspendLayout();
            this.panelHotkeys.SuspendLayout();
            this.panelOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTitleBar
            // 
            this.panelTitleBar.Controls.Add(this.picAppIcon);
            this.panelTitleBar.Controls.Add(this.btnWindowClose);
            this.panelTitleBar.Controls.Add(this.btnWindowMaximize);
            this.panelTitleBar.Controls.Add(this.btnWindowMinimize);
            this.panelTitleBar.Controls.Add(this.lblWindowTitle);
            this.panelTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitleBar.Location = new System.Drawing.Point(1, 1);
            this.panelTitleBar.Name = "panelTitleBar";
            this.panelTitleBar.Size = new System.Drawing.Size(585, 44);
            this.panelTitleBar.TabIndex = 0;
            // 
            // picAppIcon
            // 
            this.picAppIcon.Location = new System.Drawing.Point(14, 10);
            this.picAppIcon.Name = "picAppIcon";
            this.picAppIcon.Size = new System.Drawing.Size(22, 22);
            this.picAppIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picAppIcon.TabIndex = 4;
            this.picAppIcon.TabStop = false;
            // 
            // btnWindowClose
            // 
            this.btnWindowClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWindowClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWindowClose.Location = new System.Drawing.Point(533, 0);
            this.btnWindowClose.Name = "btnWindowClose";
            this.btnWindowClose.Size = new System.Drawing.Size(52, 44);
            this.btnWindowClose.TabIndex = 3;
            this.btnWindowClose.Text = "X";
            this.btnWindowClose.UseVisualStyleBackColor = true;
            // 
            // btnWindowMaximize
            // 
            this.btnWindowMaximize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWindowMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWindowMaximize.Location = new System.Drawing.Point(481, 0);
            this.btnWindowMaximize.Name = "btnWindowMaximize";
            this.btnWindowMaximize.Size = new System.Drawing.Size(52, 44);
            this.btnWindowMaximize.TabIndex = 2;
            this.btnWindowMaximize.Text = "□";
            this.btnWindowMaximize.UseVisualStyleBackColor = true;
            // 
            // btnWindowMinimize
            // 
            this.btnWindowMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWindowMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWindowMinimize.Location = new System.Drawing.Point(429, 0);
            this.btnWindowMinimize.Name = "btnWindowMinimize";
            this.btnWindowMinimize.Size = new System.Drawing.Size(52, 44);
            this.btnWindowMinimize.TabIndex = 1;
            this.btnWindowMinimize.Text = "—";
            this.btnWindowMinimize.UseVisualStyleBackColor = true;
            // 
            // lblWindowTitle
            // 
            this.lblWindowTitle.AutoSize = true;
            this.lblWindowTitle.Location = new System.Drawing.Point(42, 12);
            this.lblWindowTitle.Name = "lblWindowTitle";
            this.lblWindowTitle.Size = new System.Drawing.Size(104, 20);
            this.lblWindowTitle.TabIndex = 0;
            this.lblWindowTitle.Text = "ConnectorTool";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 71, 133);
            this.lblTitle.Location = new System.Drawing.Point(33, 68);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(217, 60);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Connector";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(73, 92, 122);
            this.lblSubtitle.Location = new System.Drawing.Point(41, 127);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(311, 30);
            this.lblSubtitle.TabIndex = 2;
            this.lblSubtitle.Text = "固定坐标连点器，用于剧情推进等场景";
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.BackColor = System.Drawing.Color.FromArgb(225, 236, 252);
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(30, 71, 133);
            this.lblStatus.Location = new System.Drawing.Point(407, 83);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(146, 38);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "待机";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelClick
            // 
            this.panelClick.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelClick.Controls.Add(this.lblIntervalHint);
            this.panelClick.Controls.Add(this.nudInterval);
            this.panelClick.Controls.Add(this.lblInterval);
            this.panelClick.Location = new System.Drawing.Point(34, 179);
            this.panelClick.Name = "panelClick";
            this.panelClick.Size = new System.Drawing.Size(519, 92);
            this.panelClick.TabIndex = 4;
            // 
            // lblIntervalHint
            // 
            this.lblIntervalHint.AutoSize = true;
            this.lblIntervalHint.ForeColor = System.Drawing.Color.FromArgb(73, 92, 122);
            this.lblIntervalHint.Location = new System.Drawing.Point(267, 35);
            this.lblIntervalHint.Name = "lblIntervalHint";
            this.lblIntervalHint.Size = new System.Drawing.Size(191, 20);
            this.lblIntervalHint.TabIndex = 2;
            this.lblIntervalHint.Text = "建议 800-1500ms 更接近手动";
            // 
            // nudInterval
            // 
            this.nudInterval.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.nudInterval.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudInterval.Location = new System.Drawing.Point(128, 29);
            this.nudInterval.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudInterval.Name = "nudInterval";
            this.nudInterval.Size = new System.Drawing.Size(120, 35);
            this.nudInterval.TabIndex = 1;
            this.nudInterval.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // lblInterval
            // 
            this.lblInterval.AutoSize = true;
            this.lblInterval.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblInterval.ForeColor = System.Drawing.Color.FromArgb(30, 71, 133);
            this.lblInterval.Location = new System.Drawing.Point(20, 31);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(102, 30);
            this.lblInterval.TabIndex = 0;
            this.lblInterval.Text = "点击间隔";
            // 
            // panelPosition
            // 
            this.panelPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPosition.Controls.Add(this.lblPositionHint);
            this.panelPosition.Controls.Add(this.btnPickPosition);
            this.panelPosition.Controls.Add(this.txtPosition);
            this.panelPosition.Controls.Add(this.lblPosition);
            this.panelPosition.Location = new System.Drawing.Point(34, 286);
            this.panelPosition.Name = "panelPosition";
            this.panelPosition.Size = new System.Drawing.Size(519, 111);
            this.panelPosition.TabIndex = 5;
            // 
            // lblPositionHint
            // 
            this.lblPositionHint.AutoSize = true;
            this.lblPositionHint.ForeColor = System.Drawing.Color.FromArgb(73, 92, 122);
            this.lblPositionHint.Location = new System.Drawing.Point(22, 73);
            this.lblPositionHint.Name = "lblPositionHint";
            this.lblPositionHint.Size = new System.Drawing.Size(275, 20);
            this.lblPositionHint.TabIndex = 3;
            this.lblPositionHint.Text = "选择位置后会进入全屏拾取，按 Esc 可取消";
            // 
            // btnPickPosition
            // 
            this.btnPickPosition.Location = new System.Drawing.Point(383, 26);
            this.btnPickPosition.Name = "btnPickPosition";
            this.btnPickPosition.Size = new System.Drawing.Size(112, 40);
            this.btnPickPosition.TabIndex = 2;
            this.btnPickPosition.Text = "选择位置";
            this.btnPickPosition.UseVisualStyleBackColor = true;
            // 
            // txtPosition
            // 
            this.txtPosition.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPosition.Location = new System.Drawing.Point(128, 28);
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.ReadOnly = true;
            this.txtPosition.Size = new System.Drawing.Size(236, 34);
            this.txtPosition.TabIndex = 1;
            // 
            // lblPosition
            // 
            this.lblPosition.AutoSize = true;
            this.lblPosition.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblPosition.ForeColor = System.Drawing.Color.FromArgb(30, 71, 133);
            this.lblPosition.Location = new System.Drawing.Point(20, 30);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(102, 30);
            this.lblPosition.TabIndex = 0;
            this.lblPosition.Text = "目标坐标";
            // 
            // panelHotkeys
            // 
            this.panelHotkeys.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHotkeys.Controls.Add(this.lblHotkeyHint);
            this.panelHotkeys.Controls.Add(this.cboStopHotkey);
            this.panelHotkeys.Controls.Add(this.lblStopHotkey);
            this.panelHotkeys.Controls.Add(this.cboStartHotkey);
            this.panelHotkeys.Controls.Add(this.lblStartHotkey);
            this.panelHotkeys.Location = new System.Drawing.Point(34, 412);
            this.panelHotkeys.Name = "panelHotkeys";
            this.panelHotkeys.Size = new System.Drawing.Size(519, 119);
            this.panelHotkeys.TabIndex = 6;
            // 
            // lblHotkeyHint
            // 
            this.lblHotkeyHint.AutoSize = true;
            this.lblHotkeyHint.ForeColor = System.Drawing.Color.FromArgb(73, 92, 122);
            this.lblHotkeyHint.Location = new System.Drawing.Point(22, 80);
            this.lblHotkeyHint.Name = "lblHotkeyHint";
            this.lblHotkeyHint.Size = new System.Drawing.Size(359, 20);
            this.lblHotkeyHint.TabIndex = 4;
            this.lblHotkeyHint.Text = "如果注册失败，通常说明该键已被其他程序占用，请更换热键";
            // 
            // cboStopHotkey
            // 
            this.cboStopHotkey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStopHotkey.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboStopHotkey.FormattingEnabled = true;
            this.cboStopHotkey.Location = new System.Drawing.Point(355, 27);
            this.cboStopHotkey.Name = "cboStopHotkey";
            this.cboStopHotkey.Size = new System.Drawing.Size(140, 36);
            this.cboStopHotkey.TabIndex = 3;
            // 
            // lblStopHotkey
            // 
            this.lblStopHotkey.AutoSize = true;
            this.lblStopHotkey.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblStopHotkey.ForeColor = System.Drawing.Color.FromArgb(30, 71, 133);
            this.lblStopHotkey.Location = new System.Drawing.Point(241, 29);
            this.lblStopHotkey.Name = "lblStopHotkey";
            this.lblStopHotkey.Size = new System.Drawing.Size(102, 30);
            this.lblStopHotkey.TabIndex = 2;
            this.lblStopHotkey.Text = "停止热键";
            // 
            // cboStartHotkey
            // 
            this.cboStartHotkey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStartHotkey.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboStartHotkey.FormattingEnabled = true;
            this.cboStartHotkey.Location = new System.Drawing.Point(95, 27);
            this.cboStartHotkey.Name = "cboStartHotkey";
            this.cboStartHotkey.Size = new System.Drawing.Size(129, 36);
            this.cboStartHotkey.TabIndex = 1;
            // 
            // lblStartHotkey
            // 
            this.lblStartHotkey.AutoSize = true;
            this.lblStartHotkey.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblStartHotkey.ForeColor = System.Drawing.Color.FromArgb(30, 71, 133);
            this.lblStartHotkey.Location = new System.Drawing.Point(20, 29);
            this.lblStartHotkey.Name = "lblStartHotkey";
            this.lblStartHotkey.Size = new System.Drawing.Size(69, 30);
            this.lblStartHotkey.TabIndex = 0;
            this.lblStartHotkey.Text = "开始";
            // 
            // panelOptions
            // 
            this.panelOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelOptions.Controls.Add(this.lblOptionsHint);
            this.panelOptions.Controls.Add(this.lblLogPath);
            this.panelOptions.Controls.Add(this.btnBrowseLogDirectory);
            this.panelOptions.Controls.Add(this.txtLogDirectory);
            this.panelOptions.Controls.Add(this.chkEnableLogging);
            this.panelOptions.Controls.Add(this.chkRandomPerturbation);
            this.panelOptions.Location = new System.Drawing.Point(34, 545);
            this.panelOptions.Name = "panelOptions";
            this.panelOptions.Size = new System.Drawing.Size(519, 161);
            this.panelOptions.TabIndex = 7;
            // 
            // lblOptionsHint
            // 
            this.lblOptionsHint.AutoSize = true;
            this.lblOptionsHint.ForeColor = System.Drawing.Color.FromArgb(73, 92, 122);
            this.lblOptionsHint.Location = new System.Drawing.Point(22, 123);
            this.lblOptionsHint.Name = "lblOptionsHint";
            this.lblOptionsHint.Size = new System.Drawing.Size(311, 20);
            this.lblOptionsHint.TabIndex = 5;
            this.lblOptionsHint.Text = "随机扰动只作用于时间间隔，不会改变鼠标点击坐标";
            // 
            // lblLogPath
            // 
            this.lblLogPath.AutoSize = true;
            this.lblLogPath.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblLogPath.ForeColor = System.Drawing.Color.FromArgb(30, 71, 133);
            this.lblLogPath.Location = new System.Drawing.Point(20, 78);
            this.lblLogPath.Name = "lblLogPath";
            this.lblLogPath.Size = new System.Drawing.Size(65, 28);
            this.lblLogPath.TabIndex = 4;
            this.lblLogPath.Text = "日志";
            // 
            // btnBrowseLogDirectory
            // 
            this.btnBrowseLogDirectory.Location = new System.Drawing.Point(411, 75);
            this.btnBrowseLogDirectory.Name = "btnBrowseLogDirectory";
            this.btnBrowseLogDirectory.Size = new System.Drawing.Size(84, 34);
            this.btnBrowseLogDirectory.TabIndex = 3;
            this.btnBrowseLogDirectory.Text = "浏览";
            this.btnBrowseLogDirectory.UseVisualStyleBackColor = true;
            // 
            // txtLogDirectory
            // 
            this.txtLogDirectory.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtLogDirectory.Location = new System.Drawing.Point(95, 75);
            this.txtLogDirectory.Name = "txtLogDirectory";
            this.txtLogDirectory.Size = new System.Drawing.Size(304, 33);
            this.txtLogDirectory.TabIndex = 2;
            // 
            // chkEnableLogging
            // 
            this.chkEnableLogging.AutoSize = true;
            this.chkEnableLogging.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkEnableLogging.Location = new System.Drawing.Point(246, 28);
            this.chkEnableLogging.Name = "chkEnableLogging";
            this.chkEnableLogging.Size = new System.Drawing.Size(111, 32);
            this.chkEnableLogging.TabIndex = 1;
            this.chkEnableLogging.Text = "启用日志";
            this.chkEnableLogging.UseVisualStyleBackColor = true;
            // 
            // chkRandomPerturbation
            // 
            this.chkRandomPerturbation.AutoSize = true;
            this.chkRandomPerturbation.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkRandomPerturbation.Location = new System.Drawing.Point(25, 28);
            this.chkRandomPerturbation.Name = "chkRandomPerturbation";
            this.chkRandomPerturbation.Size = new System.Drawing.Size(151, 32);
            this.chkRandomPerturbation.TabIndex = 0;
            this.chkRandomPerturbation.Text = "随机扰动时间";
            this.chkRandomPerturbation.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStart.Location = new System.Drawing.Point(34, 731);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(156, 46);
            this.btnStart.TabIndex = 8;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStop.Location = new System.Drawing.Point(212, 731);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(156, 46);
            this.btnStop.TabIndex = 9;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.Location = new System.Drawing.Point(397, 731);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(156, 46);
            this.btnMinimize.TabIndex = 10;
            this.btnMinimize.Text = "最小化";
            this.btnMinimize.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 800);
            this.Controls.Add(this.btnMinimize);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.panelOptions);
            this.Controls.Add(this.panelHotkeys);
            this.Controls.Add(this.panelPosition);
            this.Controls.Add(this.panelClick);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.panelTitleBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ConnectorTool";
            this.panelTitleBar.ResumeLayout(false);
            this.panelTitleBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAppIcon)).EndInit();
            this.panelClick.ResumeLayout(false);
            this.panelClick.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterval)).EndInit();
            this.panelPosition.ResumeLayout(false);
            this.panelPosition.PerformLayout();
            this.panelHotkeys.ResumeLayout(false);
            this.panelHotkeys.PerformLayout();
            this.panelOptions.ResumeLayout(false);
            this.panelOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
