using System;
using System.IO;
using System.Text;

namespace ConnectorTool.Services
{
    /// <summary>
    /// 简单文本日志服务。
    /// 只记录高价值事件，不记录每一次点击，避免刷屏和性能浪费。
    /// </summary>
    public class LogService
    {
        /// <summary>
        /// 日志文件所在目录。
        /// </summary>
        private readonly string _directory;

        /// <summary>
        /// 创建日志服务。
        /// 注意：这里不立即创建目录，避免你在界面里输入路径时就创建一堆半成品文件夹。
        /// 目录会在第一次真正写日志时再创建。
        /// </summary>
        public LogService(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentException("Log directory is required.", nameof(directory));
            }

            _directory = directory;
        }

        /// <summary>
        /// 写一条 Info 级别日志。
        /// </summary>
        public void WriteInfo(string message)
        {
            Write("Info", message);
        }

        /// <summary>
        /// 写一条 Warning 级别日志。
        /// </summary>
        public void WriteWarning(string message)
        {
            Write("Warning", message);
        }

        /// <summary>
        /// 写一条 Error 级别日志。
        /// </summary>
        public void WriteError(string message)
        {
            Write("Error", message);
        }

        /// <summary>
        /// 真正的写文件逻辑。
        /// 这里会按天生成日志文件，名字形如 ConnectorTool-2026-06-29.log。
        /// </summary>
        private void Write(string level, string message)
        {
            Directory.CreateDirectory(_directory);
            var filePath = Path.Combine(_directory, $"ConnectorTool-{DateTime.Now:yyyy-MM-dd}.log");
            var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}{Environment.NewLine}";
            File.AppendAllText(filePath, line, new UTF8Encoding(false));
        }
    }
}
