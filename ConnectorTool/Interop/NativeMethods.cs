using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ConnectorTool.Interop
{
    /// <summary>
    /// Win32 API 封装。
    /// 这里集中放系统级调用，避免零散散落在窗体代码里。
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// WM_HOTKEY 消息。
        /// 当全局热键触发时，窗口会收到这个消息。
        /// </summary>
        public const int WmHotKey = 0x0312;
        /// <summary>
        /// 非客户区鼠标按下消息。
        /// 用于拖动无边框窗体的自定义标题栏。
        /// </summary>
        public const int WmNclButtonDown = 0x00A1;
        /// <summary>
        /// 标题栏命中值。
        /// 配合 WM_NCLBUTTONDOWN 使用，告诉系统把点击当成标题栏拖动处理。
        /// </summary>
        public const int HtCaption = 0x0002;

        private const uint InputMouse = 0;
        private const uint MouseEventfLeftDown = 0x0002;
        private const uint MouseEventfLeftUp = 0x0004;

        /// <summary>
        /// 注册全局热键。
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, int vk);

        /// <summary>
        /// 注销全局热键。
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// 直接设置鼠标光标位置。
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetCursorPos(int x, int y);

        /// <summary>
        /// 释放当前鼠标捕获。
        /// 用于让无边框标题栏可以像系统标题栏一样拖动。
        /// </summary>
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        /// <summary>
        /// 向窗口发送消息。
        /// </summary>
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        /// <summary>
        /// 发送输入事件。
        /// 当前只用于鼠标点击。
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        /// <summary>
        /// 在指定坐标执行一次左键点击。
        /// </summary>
        public static void ClickLeftAt(int x, int y)
        {
            if (!SetCursorPos(x, y))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "无法移动鼠标到目标坐标。");
            }

            var inputs = new[]
            {
                new INPUT
                {
                    type = InputMouse,
                    mi = new MOUSEINPUT
                    {
                        dwFlags = MouseEventfLeftDown
                    }
                },
                new INPUT
                {
                    type = InputMouse,
                    mi = new MOUSEINPUT
                    {
                        dwFlags = MouseEventfLeftUp
                    }
                }
            };

            var sent = SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
            if (sent != inputs.Length)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "无法发送鼠标点击事件。");
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public uint type;
            public MOUSEINPUT mi;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
    }
}
