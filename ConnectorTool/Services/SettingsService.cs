using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace ConnectorTool.Services
{
    /// <summary>
    /// 配置持久化服务。
    /// 负责把 <see cref="AppSettings"/> 保存成 JSON 文件，并在启动时读回来。
    /// </summary>
    public class SettingsService
    {
        /// <summary>
        /// settings.json 的完整路径。
        /// </summary>
        private readonly string _settingsFilePath;

        /// <summary>
        /// 创建配置服务。
        /// </summary>
        /// <param name="appDataRoot">可选根目录。测试时会传临时目录；正常运行时使用 AppData。</param>
        public SettingsService(string appDataRoot = null)
        {
            var root = appDataRoot ??
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ConnectorTool");

            Directory.CreateDirectory(root);
            _settingsFilePath = Path.Combine(root, "settings.json");
        }

        /// <summary>
        /// 读取配置。
        /// 如果文件不存在，则返回一份默认配置。
        /// </summary>
        public AppSettings Load()
        {
            if (!File.Exists(_settingsFilePath))
            {
                return new AppSettings();
            }

            using (var stream = File.OpenRead(_settingsFilePath))
            {
                var serializer = new DataContractJsonSerializer(typeof(AppSettings));
                return (AppSettings)serializer.ReadObject(stream);
            }
        }

        /// <summary>
        /// 保存配置到磁盘。
        /// </summary>
        /// <param name="settings">要保存的配置对象。</param>
        public void Save(AppSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            using (var stream = File.Create(_settingsFilePath))
            {
                var serializer = new DataContractJsonSerializer(typeof(AppSettings));
                serializer.WriteObject(stream, settings);
            }
        }
    }
}
