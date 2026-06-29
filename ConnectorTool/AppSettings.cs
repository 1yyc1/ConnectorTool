using System;
using System.IO;
using System.Windows.Forms;

namespace ConnectorTool
{
    /// <summary>
    /// 程序配置模型。
    /// 这里保存的是需要跨启动持久化的用户设置，比如点击间隔、热键、目标坐标、日志目录等。
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// 点击间隔，单位是毫秒。
        /// 默认值 1000 表示每秒点击一次。
        /// </summary>
        public int ClickIntervalMs { get; set; } = 1000;

        /// <summary>
        /// 开始连点的全局热键。
        /// 默认使用 F1。
        /// </summary>
        public Keys StartHotkey { get; set; } = Keys.F1;

        /// <summary>
        /// 停止连点的全局热键。
        /// 默认使用 F2。
        /// </summary>
        public Keys StopHotkey { get; set; } = Keys.F2;

        /// <summary>
        /// 目标点击坐标 X。
        /// 只有在 <see cref="HasTargetPosition"/> 为 true 时才有效。
        /// </summary>
        public int TargetX { get; set; }

        /// <summary>
        /// 目标点击坐标 Y。
        /// 只有在 <see cref="HasTargetPosition"/> 为 true 时才有效。
        /// </summary>
        public int TargetY { get; set; }

        /// <summary>
        /// 是否已经选择过目标坐标。
        /// 用来防止在未选点时直接启动连点。
        /// </summary>
        public bool HasTargetPosition { get; set; }

        /// <summary>
        /// 是否启用随机扰动。
        /// 启用后，只会对点击间隔做轻微浮动，不会改变坐标。
        /// </summary>
        public bool RandomPerturbationEnabled { get; set; } = true;

        /// <summary>
        /// 是否启用日志。
        /// 如果为 false，程序不会写日志文件。
        /// </summary>
        public bool LoggingEnabled { get; set; }

        /// <summary>
        /// 日志目录。
        /// 默认放在程序目录下的 Logs 文件夹中。
        /// </summary>
        public string LogDirectory { get; set; } =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
    }
}
