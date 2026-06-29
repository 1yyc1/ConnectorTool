using System;
using System.ComponentModel;
using System.Windows.Forms;
using ConnectorTool.Interop;

namespace ConnectorTool.Services
{
    /// <summary>
    /// 全局热键服务。
    /// 负责把开始/停止热键注册到 Windows，并在关闭或切换时注销。
    /// </summary>
    public class HotkeyService : IDisposable
    {
        /// <summary>
        /// 开始热键对应的内部 ID。
        /// </summary>
        public const int StartHotkeyId = 0x1001;
        /// <summary>
        /// 停止热键对应的内部 ID。
        /// </summary>
        public const int StopHotkeyId = 0x1002;

        /// <summary>
        /// 注册两个全局热键。
        /// 如果其中一个失败，会抛出异常给 UI 提示。
        /// </summary>
        public void Register(IntPtr handle, Keys startHotkey, Keys stopHotkey)
        {
            Unregister(handle);

            if (!NativeMethods.RegisterHotKey(handle, StartHotkeyId, 0, (int)startHotkey))
            {
                throw new Win32Exception("开始热键注册失败，可能已被其他程序占用。");
            }

            if (!NativeMethods.RegisterHotKey(handle, StopHotkeyId, 0, (int)stopHotkey))
            {
                NativeMethods.UnregisterHotKey(handle, StartHotkeyId);
                throw new Win32Exception("停止热键注册失败，可能已被其他程序占用。");
            }
        }

        /// <summary>
        /// 注销之前注册过的全局热键。
        /// </summary>
        public void Unregister(IntPtr handle)
        {
            NativeMethods.UnregisterHotKey(handle, StartHotkeyId);
            NativeMethods.UnregisterHotKey(handle, StopHotkeyId);
        }

        /// <summary>
        /// 标准释放方法。
        /// 当前没有托管资源，保留接口是为了以后扩展更方便。
        /// </summary>
        public void Dispose()
        {
        }
    }
}
