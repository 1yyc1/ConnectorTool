using System;
using System.Drawing;
using System.IO;

namespace ConnectorTool.Services
{
    /// <summary>
    /// 应用图标读取服务。
    /// 统一从 image/秒表.ico 读取图标，避免标题栏图标和窗口图标各用各的。
    /// </summary>
    public static class AppIconService
    {
        /// <summary>
        /// 获取默认 ico 路径。
        /// </summary>
        public static string GetDefaultIconPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image", "秒表.ico");
        }

        /// <summary>
        /// 尝试加载程序图标。
        /// 如果文件不存在则返回 null，方便程序继续启动。
        /// </summary>
        public static Icon LoadIconOrNull()
        {
            var iconPath = GetDefaultIconPath();
            if (!File.Exists(iconPath))
            {
                return null;
            }

            using (var stream = File.OpenRead(iconPath))
            {
                return new Icon(stream);
            }
        }
    }
}
