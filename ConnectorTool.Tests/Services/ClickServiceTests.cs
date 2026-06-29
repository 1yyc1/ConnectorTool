using System;
using System.Threading;
using System.Threading.Tasks;
using ConnectorTool.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConnectorTool.Tests.Services
{
    /// <summary>
    /// 连点服务测试。
    /// 这里主要测试“启动/停止规则”和“后台异常时的状态行为”。
    /// </summary>
    [TestClass]
    public class ClickServiceTests
    {
        /// <summary>
        /// 如果没有选择目标坐标，连点不应该开始。
        /// 这个规则可以防止程序错误地点击自己的窗口。
        /// </summary>
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

        /// <summary>
        /// 验证启动后 IsRunning 会变成 true，停止后会变成 false。
        /// 这就是最基本的状态机行为。
        /// </summary>
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

        /// <summary>
        /// 用一个会抛异常的点击动作，验证后台错误会触发 Faulted 事件。
        /// 这里使用 TaskCompletionSource，是为了让测试能等到后台任务真正执行。
        /// </summary>
        [TestMethod]
        public async Task Start_WhenClickLoopFaults_ShouldRaiseFaultedAndResetState()
        {
            var exception = default(Exception);
            var signal = new TaskCompletionSource<bool>();
            var service = new ClickService(
                new ClickTimingService(new Random(1)),
                _ => throw new InvalidOperationException("boom"),
                (_, __) => Task.CompletedTask);

            service.Faulted += (_, ex) =>
            {
                exception = ex;
                signal.TrySetResult(true);
            };

            var settings = new AppSettings
            {
                ClickIntervalMs = 1000,
                HasTargetPosition = true,
                TargetX = 100,
                TargetY = 200
            };

            service.Start(settings);

            var completed = await Task.WhenAny(signal.Task, Task.Delay(2000, CancellationToken.None));
            Assert.AreSame(signal.Task, completed);
            Assert.IsNotNull(exception);
            Assert.IsFalse(service.IsRunning);
        }
    }
}
