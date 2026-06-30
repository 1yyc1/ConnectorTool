# ConnectorTool

ConnectorTool 是一个 Windows 固定坐标连点器，适合用于游戏剧情推进、重复确认弹窗等需要低频重复点击的场景。

它基于 .NET Framework 4.8 和 WinForms 开发，支持全局热键、固定坐标选择、随机点击间隔扰动、配置保存和日志记录。

## 功能特点

- 固定坐标连续点击
- 一键选择屏幕点击位置
- 全局热键开始和停止，默认 `F1` 开始、`F2` 停止
- 点击间隔可调，默认 `1000ms`
- 随机扰动点击间隔，减少机械感
- 最小化后仍可通过热键控制
- 自动保存上次配置
- 可选日志记录
- 提供 Windows 安装包

## 下载安装

前往 Releases 页面下载最新版安装包：

[下载 ConnectorTool-Setup-1.0.1.exe](https://github.com/1yyc1/ConnectorTool/releases/download/v1.0.1/ConnectorTool-Setup-1.0.1.exe)

下载后双击安装即可。

如果系统提示缺少运行环境，请安装 Microsoft .NET Framework 4.8。

## 使用方法

1. 打开 ConnectorTool。
2. 设置点击间隔，单位为毫秒。
3. 点击 `选择位置`。
4. 在屏幕上点击一次目标位置，程序会记录该坐标。
5. 点击 `开始`，或按默认热键 `F1` 开始连点。
6. 点击 `停止`，或按默认热键 `F2` 停止连点。

窗口最小化后，热键仍然有效。

## 选项说明

### 点击间隔

控制两次点击之间的等待时间。默认是 `1000ms`，也就是约每秒点击一次。

### 随机扰动

开启后，程序会在设定间隔附近做轻微随机浮动，只改变点击间隔，不改变点击坐标。

### 热键

默认热键：

- `F1`：开始连点
- `F2`：停止连点

如果热键被其他程序占用，可以在界面中更换。

### 日志

开启日志后，程序会记录启动、关闭、开始、停止、坐标更新、热键注册失败和异常信息。

程序不会记录每一次点击。

## 注意事项

- 请只在允许使用自动点击工具的场景中使用。
- 启动前必须先选择目标坐标。
- 如果开始或停止热键无效，可能是热键被其他程序占用，请换一个热键。
- 当前版本只支持左键点击。

## 开发和构建

项目结构：

```text
ConnectorTool/        主程序
ConnectorTool.Tests/  单元测试
installer/            Inno Setup 安装包脚本
.github/workflows/    GitHub Actions 发布流程
```

本项目使用：

- C#
- WinForms
- .NET Framework 4.8
- MSTest
- Inno Setup

运行测试：

```powershell
dotnet test ConnectorTool.Tests\ConnectorTool.Tests.csproj
```

发布新版本时，推送 `v*` 格式的标签会触发 GitHub Actions 自动构建安装包并创建 Release。

例如：

```powershell
git tag v1.0.2
git push origin v1.0.2
```

## 版本

当前最新版本：`v1.0.1`

Release 页面：

https://github.com/1yyc1/ConnectorTool/releases
