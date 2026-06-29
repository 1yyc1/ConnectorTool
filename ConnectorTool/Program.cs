using System;
using System.Windows.Forms;
using ConnectorTool.Forms;

namespace ConnectorTool
{
    /// <summary>
    /// WinForms 程序入口。
    /// 一个桌面程序启动时，会先进入 <see cref="Main"/> 方法，然后由 WinForms 的消息循环接管界面事件。
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// 应用程序主入口。
        /// </summary>
        /// <remarks>
        /// <para>
        /// <see cref="STAThreadAttribute"/> 表示主线程使用 STA（Single Thread Apartment，单线程单元）模型。
        /// WinForms、剪贴板、部分 COM 组件和系统对话框都依赖这个线程模型，所以桌面程序入口通常都要加它。
        /// </para>
        /// <para>
        /// <see cref="Application.EnableVisualStyles"/> 会启用 Windows 当前主题样式，让按钮、输入框等控件看起来不像旧版系统控件。
        /// </para>
        /// <para>
        /// <see cref="Application.SetCompatibleTextRenderingDefault"/> 设置默认文字渲染方式。
        /// 这里传 false，表示优先使用 GDI+ 以外的新式 WinForms 文本渲染方式，和 Visual Studio 新建 WinForms 项目的默认写法一致。
        /// </para>
        /// <para>
        /// <see cref="Application.Run(Form)"/> 会创建消息循环，并显示主窗体。
        /// 只有消息循环存在，按钮点击、热键消息、重绘消息这些事件才会持续被处理。
        /// </para>
        /// </remarks>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
