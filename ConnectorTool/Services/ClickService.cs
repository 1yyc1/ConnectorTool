using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectorTool.Services
{
    /// <summary>
    /// 连点执行服务。
    /// 职责只有一个：根据配置，在后台循环触发鼠标点击，并允许外部随时停止。
    /// </summary>
    public class ClickService
    {
        /// <summary>
        /// 负责计算每一轮点击之间要等待多久。
        /// </summary>
        private readonly ClickTimingService _clickTimingService;
        /// <summary>
        /// 实际执行一次点击的动作。
        /// 这里使用委托注入，方便测试替换成假的点击动作。
        /// </summary>
        private readonly Action<AppSettings> _clickAction;
        /// <summary>
        /// 延迟函数。
        /// 默认会走 Task.Delay，测试时可以替换成立即完成的版本。
        /// </summary>
        private readonly Func<int, CancellationToken, Task> _delayAsync;
        /// <summary>
        /// 当前运行中的取消令牌源。
        /// 用来停止后台点击循环。
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// 创建连点服务。
        /// </summary>
        /// <param name="clickTimingService">间隔计算器。</param>
        public ClickService(ClickTimingService clickTimingService)
            : this(clickTimingService, _ => { }, (delay, token) => Task.Delay(delay, token))
        {
        }

        /// <summary>
        /// 创建连点服务的可注入版本。
        /// 主要给主程序和测试都留一个明确入口。
        /// </summary>
        public ClickService(
            ClickTimingService clickTimingService,
            Action<AppSettings> clickAction,
            Func<int, CancellationToken, Task> delayAsync)
        {
            _clickTimingService = clickTimingService ?? throw new ArgumentNullException(nameof(clickTimingService));
            _clickAction = clickAction ?? throw new ArgumentNullException(nameof(clickAction));
            _delayAsync = delayAsync ?? throw new ArgumentNullException(nameof(delayAsync));
        }

        /// <summary>
        /// 当前是否处于运行状态。
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// 当后台点击循环出现非取消异常时触发。
        /// UI 层可以订阅这个事件来显示错误提示。
        /// </summary>
        public event EventHandler<Exception> Faulted;

        /// <summary>
        /// 启动连点。
        /// </summary>
        /// <param name="settings">当前配置。</param>
        public void Start(AppSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (!settings.HasTargetPosition)
            {
                throw new InvalidOperationException("Target position is required.");
            }

            if (settings.ClickIntervalMs < 1)
            {
                throw new InvalidOperationException("Click interval must be at least 1ms.");
            }

            // 只允许启动一次，重复点击开始按钮不会创建多个后台循环。
            if (IsRunning)
            {
                return;
            }

            // 创建取消令牌，后台任务会定期检查它。
            _cancellationTokenSource = new CancellationTokenSource();
            IsRunning = true;
            var token = _cancellationTokenSource.Token;

            // 后台循环：点击 -> 等待 -> 再点击。
            Task.Run(async () =>
            {
                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        // 先执行一次实际点击。
                        _clickAction(settings);

                        // 再根据当前设置算出下一次等待时间。
                        var delay = _clickTimingService.GetDelay(
                            settings.ClickIntervalMs,
                            settings.RandomPerturbationEnabled);

                        // 注意：这里的等待支持取消，停止时能立刻打断。
                        await _delayAsync(delay, token).ConfigureAwait(false);
                    }
                }
                catch (OperationCanceledException)
                {
                    // 正常停止时会走到这里，不算异常。
                }
                catch (Exception exception)
                {
                    // 真正的业务异常交给 UI 层处理。
                    Faulted?.Invoke(this, exception);
                }
                finally
                {
                    // 无论是正常停止还是异常退出，都要把运行状态清掉。
                    IsRunning = false;
                }
            }, token);
        }

        /// <summary>
        /// 停止连点。
        /// 会取消后台任务并释放令牌源。
        /// </summary>
        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            IsRunning = false;
        }
    }
}
