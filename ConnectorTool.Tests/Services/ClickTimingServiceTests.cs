using System;
using ConnectorTool.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConnectorTool.Tests.Services
{
    /// <summary>
    /// 点击间隔计算服务测试。
    /// 这个类只测“时间怎么算”，不碰真实鼠标点击，所以测试运行时不会真的点屏幕。
    /// </summary>
    [TestClass]
    public class ClickTimingServiceTests
    {
        /// <summary>
        /// 启用随机扰动时，实际延迟应该落在基础间隔上下 10% 的范围内。
        /// </summary>
        /// <remarks>
        /// 这里传入 <c>new Random(1234)</c> 是为了让随机数序列固定。
        /// 固定随机种子后，每次跑测试得到的随机结果都一样，测试就不会“一会儿过、一会儿不过”。
        /// </remarks>
        [TestMethod]
        public void GetDelay_WhenPerturbationEnabled_ShouldStayWithinTenPercentRange()
        {
            var service = new ClickTimingService(new Random(1234));

            // 多跑几次是为了覆盖不同随机值，但范围判断仍然稳定。
            for (var index = 0; index < 200; index++)
            {
                var delay = service.GetDelay(1000, true);

                // Assert.IsTrue 表示“条件必须成立”，否则测试失败。
                // 第二个参数是失败时的提示信息，方便快速看到异常值。
                Assert.IsTrue(delay >= 900 && delay <= 1100, $"delay={delay}");
            }
        }

        /// <summary>
        /// 关闭随机扰动时，实际延迟应该完全等于用户输入的基础间隔。
        /// </summary>
        [TestMethod]
        public void GetDelay_WhenPerturbationDisabled_ShouldReturnBaseInterval()
        {
            var service = new ClickTimingService(new Random(1234));

            // Assert.AreEqual 表示“期望值”和“实际值”必须相等。
            // 这里 1000 是期望值，service.GetDelay(...) 是实际值。
            Assert.AreEqual(1000, service.GetDelay(1000, false));
        }
    }
}
