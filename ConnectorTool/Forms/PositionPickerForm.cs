using System.Drawing;
using System.Windows.Forms;

namespace ConnectorTool.Forms
{
    /// <summary>
    /// 全屏拾取坐标层。
    /// 用户在这里单击一次，就能把屏幕坐标返回给主窗体。
    /// </summary>
    public partial class PositionPickerForm : Form
    {
        /// <summary>
        /// 构造拾取层。
        /// 它会被设置成无边框、置顶、半透明，让用户更容易把注意力放在目标位置上。
        /// </summary>
        public PositionPickerForm()
        {
            InitializeComponent();
            KeyPreview = true;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            TopMost = true;
            ShowInTaskbar = false;
            BackColor = Color.Black;
            Opacity = 0.35;
        }

        /// <summary>
        /// 用户选中的坐标。
        /// 如果用户按 Esc 取消，则保持为 null。
        /// </summary>
        public Point? SelectedPosition { get; private set; }

        /// <summary>
        /// 处理键盘命令。
        /// 我们只关心 Esc，用来取消拾取。
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// 处理鼠标按下。
        /// 只接受左键一次点击，记录当前光标位置。
        /// </summary>
        private void HandlePickerMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            SelectedPosition = Cursor.Position;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
