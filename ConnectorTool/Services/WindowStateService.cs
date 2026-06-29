using System.Windows.Forms;

namespace ConnectorTool.Services
{
    /// <summary>
    /// 窗口状态切换工具。
    /// 由于主窗体是无边框模式，所以最大化/还原逻辑单独抽出来更清晰。
    /// </summary>
    public static class WindowStateService
    {
        /// <summary>
        /// 根据当前状态切换最大化/还原。
        /// </summary>
        public static FormWindowState ToggleMaximize(FormWindowState currentState)
        {
            return currentState == FormWindowState.Maximized
                ? FormWindowState.Normal
                : FormWindowState.Maximized;
        }
    }
}
