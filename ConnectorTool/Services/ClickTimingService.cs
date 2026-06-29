using System;

namespace ConnectorTool.Services
{
    /// <summary>
    /// 点击间隔计算器。
    /// 这个类不负责真正点击，只负责根据“基础间隔 + 随机扰动开关”算出这一次该等多久。
    /// </summary>
    public class ClickTimingService
    {
        /// <summary>
        /// 随机源。
        /// 之所以注入 Random，是为了测试时可以传固定种子，保证结果可复现。
        /// </summary>
        private readonly Random _random;

        /// <summary>
        /// 创建间隔计算服务。
        /// </summary>
        /// <param name="random">可选随机源；不传则内部自己创建。</param>
        public ClickTimingService(Random random = null)
        {
            _random = random ?? new Random();
        }

        /// <summary>
        /// 计算本次点击的实际等待时间。
        /// </summary>
        /// <param name="baseIntervalMs">基础间隔，单位毫秒。</param>
        /// <param name="perturbationEnabled">是否启用随机扰动。</param>
        /// <returns>最终等待时间，最小为 1 毫秒。</returns>
        public int GetDelay(int baseIntervalMs, bool perturbationEnabled)
        {
            if (baseIntervalMs < 1)
            {
                return 1;
            }

            if (!perturbationEnabled)
            {
                return baseIntervalMs;
            }

            var ratio = 0.9 + (_random.NextDouble() * 0.2);
            return Math.Max(1, (int)Math.Round(baseIntervalMs * ratio));
        }
    }
}
