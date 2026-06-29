using System.Windows.Forms;
using ConnectorTool.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConnectorTool.Tests.Services
{
    /// <summary>
    /// 窗口状态切换测试。
    /// 这个类很小，但单测能防止以后改标题栏时把最大化/还原逻辑弄坏。
    /// </summary>
    [TestClass]
    public class WindowStateServiceTests
    {
        /// <summary>
        /// 窗口正常时，点击最大化按钮应该变成最大化。
        /// </summary>
        [TestMethod]
        public void ToggleMaximize_WhenWindowIsNormal_ShouldReturnMaximized()
        {
            Assert.AreEqual(FormWindowState.Maximized, WindowStateService.ToggleMaximize(FormWindowState.Normal));
        }

        /// <summary>
        /// 窗口最大化时，再点一次应该还原。
        /// </summary>
        [TestMethod]
        public void ToggleMaximize_WhenWindowIsMaximized_ShouldReturnNormal()
        {
            Assert.AreEqual(FormWindowState.Normal, WindowStateService.ToggleMaximize(FormWindowState.Maximized));
        }
    }
}
