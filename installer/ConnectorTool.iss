; ConnectorTool 安装包脚本。
; 这个文件给 Inno Setup 使用，用来把 Release 编译输出打成一个可双击安装的 exe。
; GitHub Actions 会在云端安装 Inno Setup，然后调用 ISCC.exe 编译本脚本。

#define MyAppName "ConnectorTool"
#define MyAppVersion GetEnv("APP_VERSION")
#if MyAppVersion == ""
#define MyAppVersion "1.0.0"
#endif
#define MyAppPublisher "ConnectorTool"
#define MyAppExeName "ConnectorTool.exe"
#define SourceDir "..\ConnectorTool\bin\Release"

[Setup]
; AppId 用来让 Windows 识别这是同一个软件，后续升级安装时会覆盖旧版本。
AppId={{DC8E02B5-7C54-4D03-983B-22DAC59712D8}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir=..\artifacts
OutputBaseFilename=ConnectorTool-Setup-{#MyAppVersion}
SetupIconFile=..\image\秒表.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=lowest
ArchitecturesInstallIn64BitMode=x64
UninstallDisplayIcon={app}\{#MyAppExeName}

[Languages]
Name: "chinesesimp"; MessagesFile: "compiler:Languages\ChineseSimplified.isl"

[Tasks]
Name: "desktopicon"; Description: "创建桌面快捷方式"; GroupDescription: "附加快捷方式："; Flags: unchecked

[Files]
; 主程序 exe、config、图标目录都会被打进安装包。
Source: "{#SourceDir}\ConnectorTool.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourceDir}\ConnectorTool.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourceDir}\image\*"; DestDir: "{app}\image"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\image\秒表.ico"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\image\秒表.ico"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "启动 {#MyAppName}"; Flags: nowait postinstall skipifsilent
