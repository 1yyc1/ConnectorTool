using System;
using System.IO;
using System.Windows.Forms;
using ConnectorTool.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConnectorTool.Tests.Services
{
    /// <summary>
    /// 配置服务测试。
    /// 重点验证两件事：
    /// 1. 默认值是不是符合产品约定
    /// 2. 保存后再读取，内容会不会原样回来
    /// </summary>
    [TestClass]
    public class SettingsServiceTests
    {
        /// <summary>
        /// 验证 AppSettings 的默认值。
        /// 这个测试的意义是：如果以后有人改默认值，我们能第一时间发现。
        /// </summary>
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
            Assert.AreEqual(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"),
                settings.LogDirectory);
        }

        /// <summary>
        /// 验证配置能正确保存和读取。
        /// 测试思路很简单：先写一份，再读回来，比对每一个字段。
        /// </summary>
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
    }
}
