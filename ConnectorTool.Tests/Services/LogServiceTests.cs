using System;
using System.IO;
using ConnectorTool.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConnectorTool.Tests.Services
{
    /// <summary>
    /// 日志服务测试。
    /// 主要确认两件事：
    /// 1. 构造函数不会一上来就创建目录
    /// 2. 真正写日志时，会生成正确的文件
    /// </summary>
    [TestClass]
    public class LogServiceTests
    {
        /// <summary>
        /// 验证“延迟创建目录”的行为。
        /// 这是为了避免你在界面里输入路径时就生成一堆半成品目录。
        /// </summary>
        [TestMethod]
        public void Constructor_ShouldNotCreateDirectoryImmediately()
        {
            var root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));

            var service = new LogService(root);

            Assert.IsFalse(Directory.Exists(root));
            Assert.IsNotNull(service);
        }

        /// <summary>
        /// 验证真正写日志时会在目录里生成文件。
        /// </summary>
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
    }
}
