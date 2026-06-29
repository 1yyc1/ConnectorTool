using System;
using System.IO;
using ConnectorTool.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConnectorTool.Tests.Services
{
    /// <summary>
    /// 应用图标服务测试。
    /// 这里只验证默认图标路径算得对不对，避免以后改目录结构时图标找不到。
    /// </summary>
    [TestClass]
    public class AppIconServiceTests
    {
        /// <summary>
        /// 默认图标应该来自 image/秒表.ico。
        /// </summary>
        [TestMethod]
        public void GetDefaultIconPath_ShouldPointToImageFolderIco()
        {
            var expected = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image", "秒表.ico");

            Assert.AreEqual(expected, AppIconService.GetDefaultIconPath());
        }
    }
}
