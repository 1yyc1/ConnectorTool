# ConnectorTool Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 为 `.NET Framework 4.8` WinForms 项目实现一个可用、可配置、支持全局热键和固定坐标点击的连点器，并完成基础自动化验证与手工验收。

**Architecture:** 保留 WinForms 作为界面宿主，但把配置、日志、热键、点击调度、坐标拾取拆成独立文件。自动化测试主要覆盖可纯逻辑验证的部分，例如配置序列化、随机扰动区间、日志文件生成与点击状态约束；Win32 热键和 UI 交互通过手工验证补齐。

**Tech Stack:** C#、WinForms、.NET Framework 4.8、Win32 P/Invoke、MSTest

---

## 文件结构

### 现有文件将修改

- `ConnectorTool/ConnectorTool.csproj`
  - 加入新增源文件与必要引用，并把 `Form1` 切换为新的主窗体实现
- `ConnectorTool/Program.cs`
  - 启动新的主窗体
- `ConnectorTool/App.config`
  - 保持轻量，必要时补充运行设置

### 新建业务与基础设施文件

- `ConnectorTool/AppSettings.cs`
  - 应用配置模型
- `ConnectorTool/AppRuntimeState.cs`
  - UI 和点击运行状态枚举
- `ConnectorTool/Interop/NativeMethods.cs`
  - Win32 调用封装
- `ConnectorTool/Services/SettingsService.cs`
  - 读取和保存 `%AppData%\ConnectorTool\settings.json`
- `ConnectorTool/Services/LogService.cs`
  - 记录高价值事件
- `ConnectorTool/Services/ClickTimingService.cs`
  - 负责计算带随机扰动的实际间隔
- `ConnectorTool/Services/ClickService.cs`
  - 负责点击循环与取消控制
- `ConnectorTool/Services/HotkeyService.cs`
  - 负责全局热键注册、更新、释放
- `ConnectorTool/Forms/MainForm.cs`
  - 主窗体逻辑
- `ConnectorTool/Forms/MainForm.Designer.cs`
  - 主窗体控件布局
- `ConnectorTool/Forms/PositionPickerForm.cs`
  - 全屏坐标拾取层
- `ConnectorTool/Forms/PositionPickerForm.Designer.cs`
  - 拾取层控件布局

### 新建测试文件

- `ConnectorTool.Tests/ConnectorTool.Tests.csproj`
  - MSTest 测试项目
- `ConnectorTool.Tests/Services/ClickTimingServiceTests.cs`
  - 验证随机扰动逻辑
- `ConnectorTool.Tests/Services/SettingsServiceTests.cs`
  - 验证配置持久化
- `ConnectorTool.Tests/Services/LogServiceTests.cs`
  - 验证日志路径和写入行为
- `ConnectorTool.Tests/Services/ClickServiceTests.cs`
  - 验证点击服务的开始/停止约束和参数校验

### 方案说明

- UI 相关行为放在 `Forms/`
- 可复用逻辑放在 `Services/`
- Win32 调用集中在 `Interop/`
- 测试只覆盖稳定的纯逻辑和可注入依赖部分，不直接测试真实热键注册和真实鼠标点击

## 任务拆分

### Task 1: 搭建测试项目与配置模型骨架

**Files:**
- Create: `ConnectorTool.Tests/ConnectorTool.Tests.csproj`
- Create: `ConnectorTool/AppSettings.cs`
- Create: `ConnectorTool/AppRuntimeState.cs`
- Modify: `ConnectorTool/ConnectorTool.csproj`
- Modify: `ConnectorTool.slnx`
- Test: `ConnectorTool.Tests/Services/SettingsServiceTests.cs`

- [ ] **Step 1: 先写配置默认值的失败测试**

```csharp
using ConnectorTool;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConnectorTool.Tests.Services
{
    [TestClass]
    public class SettingsServiceTests
    {
        [TestMethod]
        public void AppSettings_Defaults_ShouldMatchProductDecision()
        {
            var settings = new AppSettings();

            Assert.AreEqual(1000, settings.ClickIntervalMs);
            Assert.AreEqual(System.Windows.Forms.Keys.F1, settings.StartHotkey);
            Assert.AreEqual(System.Windows.Forms.Keys.F2, settings.StopHotkey);
            Assert.IsTrue(settings.RandomPerturbationEnabled);
            Assert.IsFalse(settings.LoggingEnabled);
            Assert.IsFalse(settings.HasTargetPosition);
        }
    }
}
```

- [ ] **Step 2: 运行测试，确认因类型不存在而失败**

Run: `dotnet test ConnectorTool.Tests/ConnectorTool.Tests.csproj --filter AppSettings_Defaults_ShouldMatchProductDecision`

Expected: FAIL，提示 `AppSettings` 或测试项目引用尚未建立。

- [ ] **Step 3: 新建测试项目和配置模型最小实现**

```xml
<!-- ConnectorTool.Tests/ConnectorTool.Tests.csproj -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net48</TargetFramework>
    <IsPackable>false</IsPackable>
    <LangVersion>latest</LangVersion>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="MSTest.TestAdapter" Version="3.5.2" />
    <PackageReference Include="MSTest.TestFramework" Version="3.5.2" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\ConnectorTool\ConnectorTool.csproj" />
  </ItemGroup>
</Project>
```

```csharp
// ConnectorTool/AppSettings.cs
using System;
using System.IO;
using System.Windows.Forms;

namespace ConnectorTool
{
    public class AppSettings
    {
        public int ClickIntervalMs { get; set; } = 1000;
        public Keys StartHotkey { get; set; } = Keys.F1;
        public Keys StopHotkey { get; set; } = Keys.F2;
        public int TargetX { get; set; }
        public int TargetY { get; set; }
        public bool HasTargetPosition { get; set; }
        public bool RandomPerturbationEnabled { get; set; } = true;
        public bool LoggingEnabled { get; set; }
        public string LogDirectory { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ConnectorToolLogs");
    }
}
```

```csharp
// ConnectorTool/AppRuntimeState.cs
namespace ConnectorTool
{
    public enum AppRuntimeState
    {
        Idle,
        Ready,
        Running,
        PickingPosition
    }
}
```

- [ ] **Step 4: 再跑一次测试，确认转绿**

Run: `dotnet test ConnectorTool.Tests/ConnectorTool.Tests.csproj --filter AppSettings_Defaults_ShouldMatchProductDecision`

Expected: PASS

### Task 2: 用 TDD 实现随机扰动与点击服务基础约束

**Files:**
- Create: `ConnectorTool/Services/ClickTimingService.cs`
- Create: `ConnectorTool/Services/ClickService.cs`
- Create: `ConnectorTool.Tests/Services/ClickTimingServiceTests.cs`
- Create: `ConnectorTool.Tests/Services/ClickServiceTests.cs`

- [ ] **Step 1: 先写随机扰动区间测试**

```csharp
[TestMethod]
public void GetDelay_WhenPerturbationEnabled_ShouldStayWithinTenPercentRange()
{
    var service = new ClickTimingService(new Random(1234));

    for (var i = 0; i < 200; i++)
    {
        var delay = service.GetDelay(1000, true);
        Assert.IsTrue(delay >= 900 && delay <= 1100, $"delay={delay}");
    }
}

[TestMethod]
public void GetDelay_WhenPerturbationDisabled_ShouldReturnBaseInterval()
{
    var service = new ClickTimingService(new Random(1234));

    Assert.AreEqual(1000, service.GetDelay(1000, false));
}
```

- [ ] **Step 2: 运行测试，确认失败**

Run: `dotnet test ConnectorTool.Tests/ConnectorTool.Tests.csproj --filter ClickTimingServiceTests`

Expected: FAIL，提示 `ClickTimingService` 不存在。

- [ ] **Step 3: 写最小实现让测试通过**

```csharp
using System;

namespace ConnectorTool.Services
{
    public class ClickTimingService
    {
        private readonly Random _random;

        public ClickTimingService(Random random = null)
        {
            _random = random ?? new Random();
        }

        public int GetDelay(int baseIntervalMs, bool perturbationEnabled)
        {
            if (baseIntervalMs < 1)
            {
                return 1;
            }

            if (!perturbationEnabled)
            {
                return baseIntervalMs;
            }

            var ratio = 0.9 + (_random.NextDouble() * 0.2);
            return Math.Max(1, (int)Math.Round(baseIntervalMs * ratio));
        }
    }
}
```

- [ ] **Step 4: 再写点击服务约束的失败测试**

```csharp
[TestMethod]
public void Start_ShouldThrow_WhenTargetPositionIsMissing()
{
    var service = new ClickService(new ClickTimingService(new Random(1)));

    var settings = new AppSettings
    {
        ClickIntervalMs = 1000,
        HasTargetPosition = false
    };

    Assert.ThrowsException<InvalidOperationException>(() => service.Start(settings));
}

[TestMethod]
public void StartThenStop_ShouldFlipRunningState()
{
    var service = new ClickService(new ClickTimingService(new Random(1)));
    var settings = new AppSettings
    {
        ClickIntervalMs = 1000,
        HasTargetPosition = true,
        TargetX = 100,
        TargetY = 200
    };

    service.Start(settings);
    Assert.IsTrue(service.IsRunning);

    service.Stop();
    Assert.IsFalse(service.IsRunning);
}
```

- [ ] **Step 5: 运行测试，确认失败**

Run: `dotnet test ConnectorTool.Tests/ConnectorTool.Tests.csproj --filter ClickServiceTests`

Expected: FAIL，提示 `ClickService` 不存在。

- [ ] **Step 6: 写最小点击服务骨架**

```csharp
using System;
using System.Threading;

namespace ConnectorTool.Services
{
    public class ClickService
    {
        private readonly ClickTimingService _clickTimingService;
        private CancellationTokenSource _cts;

        public ClickService(ClickTimingService clickTimingService)
        {
            _clickTimingService = clickTimingService;
        }

        public bool IsRunning { get; private set; }

        public void Start(AppSettings settings)
        {
            if (!settings.HasTargetPosition)
            {
                throw new InvalidOperationException("Target position is required.");
            }

            if (settings.ClickIntervalMs < 1)
            {
                throw new InvalidOperationException("Click interval must be at least 1ms.");
            }

            if (IsRunning)
            {
                return;
            }

            _cts = new CancellationTokenSource();
            IsRunning = true;
        }

        public void Stop()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            IsRunning = false;
        }
    }
}
```

- [ ] **Step 7: 运行两组测试，确认全部通过**

Run: `dotnet test ConnectorTool.Tests/ConnectorTool.Tests.csproj --filter "ClickTimingServiceTests|ClickServiceTests"`

Expected: PASS

### Task 3: 用 TDD 实现配置与日志服务

**Files:**
- Create: `ConnectorTool/Services/SettingsService.cs`
- Create: `ConnectorTool/Services/LogService.cs`
- Create: `ConnectorTool.Tests/Services/SettingsServiceTests.cs`
- Create: `ConnectorTool.Tests/Services/LogServiceTests.cs`
- Modify: `ConnectorTool/ConnectorTool.csproj`

- [ ] **Step 1: 为配置读写先写失败测试**

```csharp
[TestMethod]
public void SaveThenLoad_ShouldRoundTripAllSettings()
{
    var root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
    var service = new SettingsService(root);
    var original = new AppSettings
    {
        ClickIntervalMs = 1500,
        StartHotkey = Keys.F6,
        StopHotkey = Keys.F7,
        TargetX = 88,
        TargetY = 99,
        HasTargetPosition = true,
        RandomPerturbationEnabled = false,
        LoggingEnabled = true,
        LogDirectory = Path.Combine(root, "logs")
    };

    service.Save(original);
    var loaded = service.Load();

    Assert.AreEqual(original.ClickIntervalMs, loaded.ClickIntervalMs);
    Assert.AreEqual(original.StartHotkey, loaded.StartHotkey);
    Assert.AreEqual(original.StopHotkey, loaded.StopHotkey);
    Assert.AreEqual(original.TargetX, loaded.TargetX);
    Assert.AreEqual(original.TargetY, loaded.TargetY);
    Assert.AreEqual(original.HasTargetPosition, loaded.HasTargetPosition);
    Assert.AreEqual(original.RandomPerturbationEnabled, loaded.RandomPerturbationEnabled);
    Assert.AreEqual(original.LoggingEnabled, loaded.LoggingEnabled);
    Assert.AreEqual(original.LogDirectory, loaded.LogDirectory);
}
```

- [ ] **Step 2: 跑测试，确认失败**

Run: `dotnet test ConnectorTool.Tests/ConnectorTool.Tests.csproj --filter SaveThenLoad_ShouldRoundTripAllSettings`

Expected: FAIL，提示 `SettingsService` 不存在。

- [ ] **Step 3: 写最小配置服务**

```csharp
using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace ConnectorTool.Services
{
    public class SettingsService
    {
        private readonly string _settingsFilePath;

        public SettingsService(string appDataRoot = null)
        {
            var root = appDataRoot ??
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ConnectorTool");
            Directory.CreateDirectory(root);
            _settingsFilePath = Path.Combine(root, "settings.json");
        }

        public AppSettings Load()
        {
            if (!File.Exists(_settingsFilePath))
            {
                return new AppSettings();
            }

            using (var stream = File.OpenRead(_settingsFilePath))
            {
                var serializer = new DataContractJsonSerializer(typeof(AppSettings));
                return (AppSettings)serializer.ReadObject(stream);
            }
        }

        public void Save(AppSettings settings)
        {
            using (var stream = File.Create(_settingsFilePath))
            {
                var serializer = new DataContractJsonSerializer(typeof(AppSettings));
                serializer.WriteObject(stream, settings);
            }
        }
    }
}
```

- [ ] **Step 4: 为日志写入先写失败测试**

```csharp
[TestMethod]
public void WriteInfo_ShouldAppendLineToDailyLogFile()
{
    var root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
    var service = new LogService(root);

    service.WriteInfo("click started");

    var files = Directory.GetFiles(root, "ConnectorTool-*.log");
    Assert.AreEqual(1, files.Length);

    var content = File.ReadAllText(files[0]);
    StringAssert.Contains(content, "Info");
    StringAssert.Contains(content, "click started");
}
```

- [ ] **Step 5: 跑测试，确认失败**

Run: `dotnet test ConnectorTool.Tests/ConnectorTool.Tests.csproj --filter WriteInfo_ShouldAppendLineToDailyLogFile`

Expected: FAIL，提示 `LogService` 不存在。

- [ ] **Step 6: 写最小日志服务**

```csharp
using System;
using System.IO;
using System.Text;

namespace ConnectorTool.Services
{
    public class LogService
    {
        private readonly string _directory;

        public LogService(string directory)
        {
            _directory = directory;
            Directory.CreateDirectory(_directory);
        }

        public void WriteInfo(string message)
        {
            Write("Info", message);
        }

        public void WriteWarning(string message)
        {
            Write("Warning", message);
        }

        public void WriteError(string message)
        {
            Write("Error", message);
        }

        private void Write(string level, string message)
        {
            var filePath = Path.Combine(_directory, $"ConnectorTool-{DateTime.Now:yyyy-MM-dd}.log");
            var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}{Environment.NewLine}";
            File.AppendAllText(filePath, line, Encoding.UTF8);
        }
    }
}
```

- [ ] **Step 7: 运行配置与日志测试，确认全部通过**

Run: `dotnet test ConnectorTool.Tests/ConnectorTool.Tests.csproj --filter "SettingsServiceTests|LogServiceTests"`

Expected: PASS

### Task 4: 实现 Win32 互操作、热键服务和坐标拾取层

**Files:**
- Create: `ConnectorTool/Interop/NativeMethods.cs`
- Create: `ConnectorTool/Services/HotkeyService.cs`
- Create: `ConnectorTool/Forms/PositionPickerForm.cs`
- Create: `ConnectorTool/Forms/PositionPickerForm.Designer.cs`
- Modify: `ConnectorTool/ConnectorTool.csproj`

- [ ] **Step 1: 先写热键变更约束的失败测试**

```csharp
[TestMethod]
public void AppSettings_ShouldAllowDifferentStartAndStopHotkeys()
{
    var settings = new AppSettings
    {
        StartHotkey = Keys.F3,
        StopHotkey = Keys.F4
    };

    Assert.AreNotEqual(settings.StartHotkey, settings.StopHotkey);
}
```

- [ ] **Step 2: 运行测试，确认现有模型已支持，作为保护测试保留**

Run: `dotnet test ConnectorTool.Tests/ConnectorTool.Tests.csproj --filter AppSettings_ShouldAllowDifferentStartAndStopHotkeys`

Expected: PASS

- [ ] **Step 3: 实现 Win32 封装与热键服务**

```csharp
// ConnectorTool/Interop/NativeMethods.cs
using System;
using System.Runtime.InteropServices;

namespace ConnectorTool.Interop
{
    internal static class NativeMethods
    {
        public const int WmHotKey = 0x0312;
        public const uint InputMouse = 0;
        public const uint MouseEventfLeftDown = 0x0002;
        public const uint MouseEventfLeftUp = 0x0004;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, int vk);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
    }
}
```

```csharp
// ConnectorTool/Services/HotkeyService.cs
using System;
using System.ComponentModel;
using System.Windows.Forms;
using ConnectorTool.Interop;

namespace ConnectorTool.Services
{
    public class HotkeyService : IDisposable
    {
        public const int StartHotkeyId = 0x1001;
        public const int StopHotkeyId = 0x1002;

        public void Register(IntPtr handle, Keys startHotkey, Keys stopHotkey)
        {
            Unregister(handle);

            if (!NativeMethods.RegisterHotKey(handle, StartHotkeyId, 0, (int)startHotkey))
            {
                throw new Win32Exception("开始热键注册失败。");
            }

            if (!NativeMethods.RegisterHotKey(handle, StopHotkeyId, 0, (int)stopHotkey))
            {
                NativeMethods.UnregisterHotKey(handle, StartHotkeyId);
                throw new Win32Exception("停止热键注册失败。");
            }
        }

        public void Unregister(IntPtr handle)
        {
            NativeMethods.UnregisterHotKey(handle, StartHotkeyId);
            NativeMethods.UnregisterHotKey(handle, StopHotkeyId);
        }

        public void Dispose()
        {
        }
    }
}
```

- [ ] **Step 4: 实现坐标拾取层**

```csharp
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ConnectorTool.Forms
{
    public partial class PositionPickerForm : Form
    {
        public event EventHandler<Point> PositionPicked;

        public PositionPickerForm()
        {
            InitializeComponent();
            KeyPreview = true;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            TopMost = true;
            BackColor = Color.Black;
            Opacity = 0.35;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                PositionPicked?.Invoke(this, Cursor.Position);
                Close();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
```

- [ ] **Step 5: 编译主项目，确认新增基础设施无编译错误**

Run: `msbuild ConnectorTool/ConnectorTool.csproj /t:Build /p:Configuration=Debug`

Expected: Build succeeded

### Task 5: 重建主窗体 UI 并接入真实点击流程

**Files:**
- Create: `ConnectorTool/Forms/MainForm.cs`
- Create: `ConnectorTool/Forms/MainForm.Designer.cs`
- Modify: `ConnectorTool/Program.cs`
- Modify: `ConnectorTool/ConnectorTool.csproj`
- Modify: `ConnectorTool/Services/ClickService.cs`

- [ ] **Step 1: 先写主窗体核心状态约束测试**

```csharp
[TestMethod]
public void Start_ShouldKeepRunning_WhenCalledTwice()
{
    var service = new ClickService(new ClickTimingService(new Random(1)));
    var settings = new AppSettings
    {
        ClickIntervalMs = 1000,
        HasTargetPosition = true,
        TargetX = 100,
        TargetY = 200
    };

    service.Start(settings);
    service.Start(settings);

    Assert.IsTrue(service.IsRunning);

    service.Stop();
}
```

- [ ] **Step 2: 运行测试，确认当前实现已满足，作为回归保护保留**

Run: `dotnet test ConnectorTool.Tests/ConnectorTool.Tests.csproj --filter Start_ShouldKeepRunning_WhenCalledTwice`

Expected: PASS

- [ ] **Step 3: 扩展点击服务为真实点击循环**

```csharp
public void Start(AppSettings settings)
{
    if (!settings.HasTargetPosition)
    {
        throw new InvalidOperationException("Target position is required.");
    }

    if (settings.ClickIntervalMs < 1)
    {
        throw new InvalidOperationException("Click interval must be at least 1ms.");
    }

    if (IsRunning)
    {
        return;
    }

    _cts = new CancellationTokenSource();
    IsRunning = true;
    var token = _cts.Token;

    Task.Run(async () =>
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                NativeMethods.SetCursorPos(settings.TargetX, settings.TargetY);
                SendLeftClick();

                var delay = _clickTimingService.GetDelay(
                    settings.ClickIntervalMs,
                    settings.RandomPerturbationEnabled);

                await Task.Delay(delay, token);
            }
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            IsRunning = false;
        }
    }, token);
}
```

- [ ] **Step 4: 实现新的主窗体**

```csharp
// Program.cs
Application.Run(new MainForm());
```

```csharp
// MainForm 核心职责
- 加载 SettingsService 中的配置
- 根据配置刷新控件值
- 在“选择位置”按钮中打开 PositionPickerForm
- 在 “开始/停止” 按钮中调用 ClickService
- 在 WndProc 中响应开始热键和停止热键
- 在配置变更时重新注册热键
- 在关闭时保存配置
```

- [ ] **Step 5: 完成 WinForms 蓝白渐变界面布局**

```csharp
// MainForm.Designer.cs 至少包含这些控件
private System.Windows.Forms.Label lblTitle;
private System.Windows.Forms.Label lblStatus;
private System.Windows.Forms.NumericUpDown nudInterval;
private System.Windows.Forms.ComboBox cboStartHotkey;
private System.Windows.Forms.ComboBox cboStopHotkey;
private System.Windows.Forms.TextBox txtPosition;
private System.Windows.Forms.Button btnPickPosition;
private System.Windows.Forms.CheckBox chkRandomPerturbation;
private System.Windows.Forms.CheckBox chkEnableLogging;
private System.Windows.Forms.TextBox txtLogDirectory;
private System.Windows.Forms.Button btnBrowseLogDirectory;
private System.Windows.Forms.Button btnStart;
private System.Windows.Forms.Button btnStop;
private System.Windows.Forms.Button btnMinimize;
```

- [ ] **Step 6: 编译主项目，确认主窗体与点击逻辑接通**

Run: `msbuild ConnectorTool/ConnectorTool.csproj /t:Build /p:Configuration=Debug`

Expected: Build succeeded

### Task 6: 接入日志、配置保存并完成完整验证

**Files:**
- Modify: `ConnectorTool/Forms/MainForm.cs`
- Modify: `ConnectorTool/Services/ClickService.cs`
- Modify: `ConnectorTool/Services/SettingsService.cs`
- Modify: `ConnectorTool/Services/LogService.cs`

- [ ] **Step 1: 在主窗体接入日志事件**

```csharp
// 记录这些事件
_logService.WriteInfo("Application started.");
_logService.WriteInfo("Position updated: X=123, Y=456");
_logService.WriteInfo("Clicking started.");
_logService.WriteInfo("Clicking stopped.");
_logService.WriteWarning("Start hotkey registration failed.");
_logService.WriteError(ex.ToString());
```

- [ ] **Step 2: 在主窗体关闭时保存配置**

```csharp
private void SaveSettings()
{
    _settings.ClickIntervalMs = (int)nudInterval.Value;
    _settings.StartHotkey = (Keys)cboStartHotkey.SelectedItem;
    _settings.StopHotkey = (Keys)cboStopHotkey.SelectedItem;
    _settings.RandomPerturbationEnabled = chkRandomPerturbation.Checked;
    _settings.LoggingEnabled = chkEnableLogging.Checked;
    _settings.LogDirectory = txtLogDirectory.Text.Trim();

    _settingsService.Save(_settings);
}
```

- [ ] **Step 3: 跑全部自动化测试**

Run: `dotnet test ConnectorTool.Tests/ConnectorTool.Tests.csproj`

Expected: 全部 PASS

- [ ] **Step 4: 执行手工验收**

Run:
- `msbuild ConnectorTool/ConnectorTool.csproj /t:Build /p:Configuration=Debug`
- 启动 `ConnectorTool/bin/Debug/ConnectorTool.exe`

Expected:
- 能选择屏幕坐标并回填到界面
- 最小化后按开始热键可以持续点击
- 最小化后按停止热键可以立即停止
- 关闭后重开，配置仍然存在
- 启用日志时，日志写入自定义目录

## 自检结论

### 1. Spec coverage

- 固定坐标点击：Task 4、Task 5
- 自定义开始/停止热键：Task 4、Task 5
- 最小化后热键控制：Task 5、Task 6
- 随机扰动：Task 2
- 日志与自定义路径：Task 3、Task 6
- 配置持久化：Task 1、Task 3、Task 6
- 蓝白渐变 UI：Task 5

### 2. Placeholder scan

- 未保留 `TODO`、`TBD`、`implement later` 等占位语
- 所有任务都给出了明确文件和运行命令

### 3. Type consistency

- `AppSettings`、`ClickTimingService`、`ClickService`、`SettingsService`、`LogService`、`HotkeyService`、`PositionPickerForm`、`MainForm` 在各任务中保持一致

## 执行交接

计划文件已保存后，执行方式有两种：

1. `Subagent-Driven（推荐）`
   - 每个任务单独派发子代理执行并复核

2. `Inline Execution`
   - 在当前会话内按任务顺序直接执行

当前工作区不是 Git 仓库，因此本计划不包含提交步骤；如果后续接入 Git，再补充分段提交策略。
