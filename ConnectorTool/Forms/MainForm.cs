using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConnectorTool.Interop;
using ConnectorTool.Services;

namespace ConnectorTool.Forms
{
    /// <summary>
    /// 主窗体。
    /// 负责承载 UI、保存和恢复配置、处理热键、启动/停止连点，以及连接所有服务层。
    /// </summary>
    public partial class MainForm : Form
    {
        private static readonly Keys[] HotkeyOptions =
        {
            Keys.F1,
            Keys.F2,
            Keys.F3,
            Keys.F4,
            Keys.F5,
            Keys.F6,
            Keys.F7,
            Keys.F8,
            Keys.F9,
            Keys.F10,
            Keys.F11,
            Keys.F12,
            Keys.Home,
            Keys.End,
            Keys.Insert,
            Keys.Pause
        };

        /// <summary>
        /// 配置服务。
        /// 负责把用户设置读写到磁盘。
        /// </summary>
        private readonly SettingsService _settingsService;
        /// <summary>
        /// 全局热键服务。
        /// </summary>
        private readonly HotkeyService _hotkeyService;
        /// <summary>
        /// 连点服务。
        /// </summary>
        private readonly ClickService _clickService;

        /// <summary>
        /// 当前加载到界面的配置对象。
        /// </summary>
        private AppSettings _settings;
        /// <summary>
        /// 当前启用的日志服务。
        /// </summary>
        private LogService _logService;
        /// <summary>
        /// 用来避免“加载配置时触发控件事件”造成的重复写回。
        /// </summary>
        private bool _isApplyingSettings;

        /// <summary>
        /// 构造主窗体。
        /// 这里会完成所有服务初始化和 UI 样式初始化。
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            DoubleBuffered = true;
            MinimumSize = new Size(587, 800);
            Icon = AppIconService.LoadIconOrNull();

            _settingsService = new SettingsService();
            _hotkeyService = new HotkeyService();
            _clickService = new ClickService(
                new ClickTimingService(),
                settings => NativeMethods.ClickLeftAt(settings.TargetX, settings.TargetY),
                (delay, token) => Task.Delay(delay, token));

            _clickService.Faulted += ClickServiceOnFaulted;

            ConfigureVisualStyle();
            PopulateHotkeys();
            WireEvents();
            LoadSettings();
            ApplySettingsToControls();
            PrepareLogging(false);
            RefreshUiState();
        }

        /// <summary>
        /// 窗体显示后注册热键并记录启动日志。
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            RegisterHotkeysFromCurrentSelection(true);
            PrepareLogging(false);
            TryWriteLogInfo("Application started.");
        }

        /// <summary>
        /// 窗体大小变化时同步更新标题栏上的最大化/还原按钮。
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateWindowButtons();
            Invalidate();
        }

        /// <summary>
        /// 绘制窗体边框线。
        /// 无边框模式下，我们自己补一个浅色边线，防止窗体边缘过于“飘”。
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var pen = new Pen(Color.FromArgb(176, 202, 237)))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            }
        }

        /// <summary>
        /// 绘制蓝白渐变背景。
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (var brush = new LinearGradientBrush(ClientRectangle, Color.FromArgb(227, 242, 255), Color.White, 90f))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }
        }

        /// <summary>
        /// 关闭前保存配置并释放资源。
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveSettings();
            _clickService.Stop();
            TryWriteLogInfo("Application shutting down.");
            _hotkeyService.Unregister(Handle);
            base.OnFormClosing(e);
        }

        /// <summary>
        /// 句柄销毁时注销热键，避免热键残留。
        /// </summary>
        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (!RecreatingHandle)
            {
                _hotkeyService.Unregister(Handle);
            }

            base.OnHandleDestroyed(e);
        }

        /// <summary>
        /// 消息循环入口。
        /// 这里接收 WM_HOTKEY，然后映射到开始/停止操作。
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WmHotKey)
            {
                var hotkeyId = m.WParam.ToInt32();
                if (hotkeyId == HotkeyService.StartHotkeyId)
                {
                    StartClicking();
                }
                else if (hotkeyId == HotkeyService.StopHotkeyId)
                {
                    StopClicking();
                }
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// 初始化界面视觉风格。
        /// </summary>
        private void ConfigureVisualStyle()
        {
            BackColor = Color.White;

            StyleCard(panelClick);
            StyleCard(panelPosition);
            StyleCard(panelHotkeys);
            StyleCard(panelOptions);

            panelTitleBar.BackColor = Color.FromArgb(240, 247, 255);
            picAppIcon.Image = AppIconService.LoadIconOrNull()?.ToBitmap();
            lblWindowTitle.ForeColor = Color.FromArgb(30, 71, 133);
            lblWindowTitle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);

            StyleWindowButton(btnWindowMinimize);
            StyleWindowButton(btnWindowMaximize);
            StyleWindowCloseButton(btnWindowClose);

            StylePrimaryButton(btnStart);
            StylePrimaryButton(btnPickPosition);
            StyleSecondaryButton(btnStop);
            StyleSecondaryButton(btnMinimize);
            StyleSecondaryButton(btnBrowseLogDirectory);

            txtPosition.BackColor = Color.FromArgb(245, 249, 255);
            txtLogDirectory.BackColor = Color.White;

            UpdateWindowButtons();
        }

        /// <summary>
        /// 给内容卡片统一设置白底和边框。
        /// </summary>
        private void StyleCard(Panel panel)
        {
            panel.BackColor = Color.FromArgb(252, 253, 255);
            panel.BorderStyle = BorderStyle.FixedSingle;
        }

        /// <summary>
        /// 主按钮样式：蓝底白字。
        /// </summary>
        private void StylePrimaryButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = Color.FromArgb(58, 120, 246);
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        }

        /// <summary>
        /// 次按钮样式：白底蓝边。
        /// </summary>
        private void StyleSecondaryButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(176, 202, 237);
            button.FlatAppearance.BorderSize = 1;
            button.BackColor = Color.White;
            button.ForeColor = Color.FromArgb(30, 71, 133);
            button.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
        }

        /// <summary>
        /// 标题栏按钮的基础样式。
        /// </summary>
        private void StyleWindowButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = Color.Transparent;
            button.ForeColor = Color.FromArgb(30, 71, 133);
            button.Font = new Font("Segoe UI Symbol", 10.5F, FontStyle.Regular);
            button.Margin = Padding.Empty;
        }

        /// <summary>
        /// 关闭按钮样式。
        /// </summary>
        private void StyleWindowCloseButton(Button button)
        {
            StyleWindowButton(button);
            button.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
        }

        /// <summary>
        /// 填充热键下拉框。
        /// 这里只放我们允许用户选的组合，避免乱选导致不可用。
        /// </summary>
        private void PopulateHotkeys()
        {
            cboStartHotkey.Items.AddRange(HotkeyOptions.Cast<object>().ToArray());
            cboStopHotkey.Items.AddRange(HotkeyOptions.Cast<object>().ToArray());
        }

        /// <summary>
        /// 给控件绑定事件。
        /// </summary>
        private void WireEvents()
        {
            panelTitleBar.MouseDown += HandleTitleBarMouseDown;
            lblWindowTitle.MouseDown += HandleTitleBarMouseDown;
            panelTitleBar.DoubleClick += (_, __) => ToggleWindowState();
            lblWindowTitle.DoubleClick += (_, __) => ToggleWindowState();

            btnWindowMinimize.Click += (_, __) => WindowState = FormWindowState.Minimized;
            btnWindowMaximize.Click += (_, __) => ToggleWindowState();
            btnWindowClose.Click += (_, __) => Close();

            btnPickPosition.Click += (_, __) => PickPosition();
            btnBrowseLogDirectory.Click += (_, __) => BrowseLogDirectory();
            btnStart.Click += (_, __) => StartClicking();
            btnStop.Click += (_, __) => StopClicking();
            btnMinimize.Click += (_, __) => WindowState = FormWindowState.Minimized;

            nudInterval.ValueChanged += (_, __) =>
            {
                if (_isApplyingSettings)
                {
                    return;
                }

                _settings.ClickIntervalMs = (int)nudInterval.Value;
                RefreshUiState();
            };

            chkRandomPerturbation.CheckedChanged += (_, __) =>
            {
                if (_isApplyingSettings)
                {
                    return;
                }

                _settings.RandomPerturbationEnabled = chkRandomPerturbation.Checked;
            };

            chkEnableLogging.CheckedChanged += (_, __) =>
            {
                if (_isApplyingSettings)
                {
                    return;
                }

                _settings.LoggingEnabled = chkEnableLogging.Checked;
                PrepareLogging(true);
                RefreshLogPathUi();
            };

            txtLogDirectory.TextChanged += (_, __) =>
            {
                if (_isApplyingSettings)
                {
                    return;
                }

                _settings.LogDirectory = NormalizeLogDirectory(txtLogDirectory.Text.Trim());
            };

            cboStartHotkey.SelectedIndexChanged += (_, __) => HandleHotkeySelectionChanged();
            cboStopHotkey.SelectedIndexChanged += (_, __) => HandleHotkeySelectionChanged();
        }

        /// <summary>
        /// 让无边框窗体顶部栏可以像系统标题栏一样拖动。
        /// </summary>
        private void HandleTitleBarMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            NativeMethods.ReleaseCapture();
            NativeMethods.SendMessage(Handle, NativeMethods.WmNclButtonDown, NativeMethods.HtCaption, 0);
        }

        /// <summary>
        /// 切换最大化和还原状态。
        /// </summary>
        private void ToggleWindowState()
        {
            WindowState = WindowStateService.ToggleMaximize(WindowState);
            UpdateWindowButtons();
        }

        /// <summary>
        /// 根据当前窗体状态更新标题栏按钮文案。
        /// </summary>
        private void UpdateWindowButtons()
        {
            btnWindowMaximize.Text = WindowState == FormWindowState.Maximized ? "❐" : "□";
        }

        /// <summary>
        /// 从磁盘加载设置。
        /// 出错时会回退到默认配置。
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                _settings = _settingsService.Load();
                _settings.LogDirectory = NormalizeLogDirectory(_settings.LogDirectory);
            }
            catch (Exception exception)
            {
                _settings = new AppSettings();
                MessageBox.Show(
                    "配置加载失败，已恢复默认设置。\r\n\r\n" + exception.Message,
                    "ConnectorTool",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 把配置写回到控件上。
        /// </summary>
        private void ApplySettingsToControls()
        {
            _isApplyingSettings = true;
            try
            {
                nudInterval.Value = Math.Max(nudInterval.Minimum, Math.Min(nudInterval.Maximum, _settings.ClickIntervalMs));
                chkRandomPerturbation.Checked = _settings.RandomPerturbationEnabled;
                chkEnableLogging.Checked = _settings.LoggingEnabled;
                txtLogDirectory.Text = NormalizeLogDirectory(_settings.LogDirectory);

                cboStartHotkey.SelectedItem = HotkeyOptions.Contains(_settings.StartHotkey) ? (object)_settings.StartHotkey : Keys.F1;
                cboStopHotkey.SelectedItem = HotkeyOptions.Contains(_settings.StopHotkey) ? (object)_settings.StopHotkey : Keys.F2;
                txtPosition.Text = FormatPosition();
            }
            finally
            {
                _isApplyingSettings = false;
            }

            RefreshLogPathUi();
        }

        /// <summary>
        /// 把日志目录归一化为一个可用路径。
        /// 为空时使用默认 Logs 目录。
        /// </summary>
        private string NormalizeLogDirectory(string directory)
        {
            return string.IsNullOrWhiteSpace(directory)
                ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs")
                : directory.Trim();
        }

        /// <summary>
        /// 把当前坐标格式化成界面可读文本。
        /// </summary>
        private string FormatPosition()
        {
            return _settings.HasTargetPosition
                ? string.Format("X: {0}, Y: {1}", _settings.TargetX, _settings.TargetY)
                : "尚未选择";
        }

        /// <summary>
        /// 热键下拉框变更后的处理逻辑。
        /// </summary>
        private void HandleHotkeySelectionChanged()
        {
            if (_isApplyingSettings)
            {
                return;
            }

            if (!(cboStartHotkey.SelectedItem is Keys selectedStart) || !(cboStopHotkey.SelectedItem is Keys selectedStop))
            {
                return;
            }

            if (selectedStart == selectedStop)
            {
                MessageBox.Show("开始热键和停止热键不能相同。", "ConnectorTool", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ApplySettingsToControls();
                return;
            }

            var previousStart = _settings.StartHotkey;
            var previousStop = _settings.StopHotkey;

            _settings.StartHotkey = selectedStart;
            _settings.StopHotkey = selectedStop;

            if (!RegisterHotkeysFromCurrentSelection(true))
            {
                _settings.StartHotkey = previousStart;
                _settings.StopHotkey = previousStop;
                ApplySettingsToControls();
                return;
            }

            TryWriteLogInfo("Hotkeys updated: Start=" + _settings.StartHotkey + ", Stop=" + _settings.StopHotkey);
        }

        /// <summary>
        /// 按当前设置重新注册热键。
        /// </summary>
        private bool RegisterHotkeysFromCurrentSelection(bool showErrors)
        {
            try
            {
                _hotkeyService.Register(Handle, _settings.StartHotkey, _settings.StopHotkey);
                return true;
            }
            catch (Win32Exception exception)
            {
                TryWriteLogWarning(exception.Message);
                if (showErrors)
                {
                    MessageBox.Show(exception.Message, "热键注册失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                return false;
            }
        }

        /// <summary>
        /// 打开坐标拾取层。
        /// </summary>
        private void PickPosition()
        {
            if (_clickService.IsRunning)
            {
                MessageBox.Show("请先停止连点，再重新选择坐标。", "ConnectorTool", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            lblStatus.Text = "拾取中";
            lblStatus.BackColor = Color.FromArgb(255, 241, 212);
            lblStatus.ForeColor = Color.FromArgb(144, 93, 18);
            Hide();

            try
            {
                using (var picker = new PositionPickerForm())
                {
                    if (picker.ShowDialog(this) == DialogResult.OK && picker.SelectedPosition.HasValue)
                    {
                        var point = picker.SelectedPosition.Value;
                        _settings.TargetX = point.X;
                        _settings.TargetY = point.Y;
                        _settings.HasTargetPosition = true;
                        txtPosition.Text = FormatPosition();
                        TryWriteLogInfo("Position updated: X=" + point.X + ", Y=" + point.Y);
                    }
                }
            }
            finally
            {
                Show();
                Activate();
                RefreshUiState();
            }
        }

        /// <summary>
        /// 打开文件夹选择对话框。
        /// </summary>
        private void BrowseLogDirectory()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择日志保存目录";
                dialog.SelectedPath = NormalizeLogDirectory(txtLogDirectory.Text);

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtLogDirectory.Text = dialog.SelectedPath;
                    _settings.LogDirectory = dialog.SelectedPath;
                    if (_settings.LoggingEnabled)
                    {
                        PrepareLogging(true);
                    }
                }
            }
        }

        /// <summary>
        /// 开始连点。
        /// </summary>
        private void StartClicking()
        {
            try
            {
                SyncSettingsFromControls();
                PrepareLogging(true);
                _clickService.Start(_settings);
                TryWriteLogInfo("Clicking started.");
                RefreshUiState();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "无法开始连点", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 停止连点。
        /// </summary>
        private void StopClicking()
        {
            if (!_clickService.IsRunning)
            {
                RefreshUiState();
                return;
            }

            _clickService.Stop();
            TryWriteLogInfo("Clicking stopped.");
            RefreshUiState();
        }

        /// <summary>
        /// 根据当前状态刷新界面。
        /// </summary>
        private void RefreshUiState()
        {
            txtPosition.Text = FormatPosition();

            if (_clickService.IsRunning)
            {
                lblStatus.Text = "运行中";
                lblStatus.BackColor = Color.FromArgb(215, 245, 228);
                lblStatus.ForeColor = Color.FromArgb(19, 121, 72);
            }
            else if (_settings.HasTargetPosition)
            {
                lblStatus.Text = "就绪";
                lblStatus.BackColor = Color.FromArgb(225, 236, 252);
                lblStatus.ForeColor = Color.FromArgb(30, 71, 133);
            }
            else
            {
                lblStatus.Text = "待机";
                lblStatus.BackColor = Color.FromArgb(238, 242, 248);
                lblStatus.ForeColor = Color.FromArgb(95, 108, 126);
            }

            btnStart.Enabled = _settings.HasTargetPosition && !_clickService.IsRunning;
            btnStop.Enabled = _clickService.IsRunning;
            btnPickPosition.Enabled = !_clickService.IsRunning;
            nudInterval.Enabled = !_clickService.IsRunning;
            cboStartHotkey.Enabled = !_clickService.IsRunning;
            cboStopHotkey.Enabled = !_clickService.IsRunning;
        }

        /// <summary>
        /// 控制日志相关输入框是否可编辑。
        /// </summary>
        private void RefreshLogPathUi()
        {
            txtLogDirectory.Enabled = chkEnableLogging.Checked;
            btnBrowseLogDirectory.Enabled = chkEnableLogging.Checked;
            lblLogPath.Enabled = chkEnableLogging.Checked;
        }

        /// <summary>
        /// 从界面控件同步配置值到内存对象。
        /// </summary>
        private void SyncSettingsFromControls()
        {
            _settings.ClickIntervalMs = (int)nudInterval.Value;
            _settings.StartHotkey = (Keys)cboStartHotkey.SelectedItem;
            _settings.StopHotkey = (Keys)cboStopHotkey.SelectedItem;
            _settings.RandomPerturbationEnabled = chkRandomPerturbation.Checked;
            _settings.LoggingEnabled = chkEnableLogging.Checked;
            _settings.LogDirectory = NormalizeLogDirectory(txtLogDirectory.Text);
        }

        /// <summary>
        /// 把当前设置保存到磁盘。
        /// </summary>
        private void SaveSettings()
        {
            try
            {
                SyncSettingsFromControls();
                _settingsService.Save(_settings);
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "配置保存失败。\r\n\r\n" + exception.Message,
                    "ConnectorTool",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 根据当前日志开关和目录准备日志服务。
        /// </summary>
        private void PrepareLogging(bool showErrors)
        {
            _logService = null;

            if (!_settings.LoggingEnabled)
            {
                return;
            }

            try
            {
                _logService = new LogService(NormalizeLogDirectory(_settings.LogDirectory));
            }
            catch (Exception exception)
            {
                _settings.LoggingEnabled = false;
                chkEnableLogging.Checked = false;
                if (showErrors)
                {
                    MessageBox.Show(
                        "日志目录不可用，已自动关闭日志功能。\r\n\r\n" + exception.Message,
                        "ConnectorTool",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// 写 Info 日志的安全包装。
        /// </summary>
        private void TryWriteLogInfo(string message)
        {
            TryWriteLog(service => service.WriteInfo(message));
        }

        /// <summary>
        /// 写 Warning 日志的安全包装。
        /// </summary>
        private void TryWriteLogWarning(string message)
        {
            TryWriteLog(service => service.WriteWarning(message));
        }

        /// <summary>
        /// 写 Error 日志的安全包装。
        /// </summary>
        private void TryWriteLogError(string message)
        {
            TryWriteLog(service => service.WriteError(message));
        }

        /// <summary>
        /// 统一的日志执行入口，避免日志失败影响主流程。
        /// </summary>
        private void TryWriteLog(Action<LogService> writeAction)
        {
            if (_logService == null)
            {
                return;
            }

            try
            {
                writeAction(_logService);
            }
            catch
            {
                _logService = null;
                _settings.LoggingEnabled = false;
                if (!IsDisposed)
                {
                    BeginInvoke((Action)(() =>
                    {
                        chkEnableLogging.Checked = false;
                        RefreshLogPathUi();
                    }));
                }
            }
        }

        /// <summary>
        /// 连点后台异常时的回调。
        /// </summary>
        private void ClickServiceOnFaulted(object sender, Exception exception)
        {
            TryWriteLogError(exception.ToString());

            if (IsDisposed)
            {
                return;
            }

            BeginInvoke((Action)(() =>
            {
                RefreshUiState();
                MessageBox.Show(
                    "连点过程中出现异常，已自动停止。\r\n\r\n" + exception.Message,
                    "ConnectorTool",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }));
        }
    }
}
