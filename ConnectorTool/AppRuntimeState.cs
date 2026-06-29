namespace ConnectorTool
{
    /// <summary>
    /// 主界面的运行状态。
    /// 用枚举集中表达状态，可以避免在 UI 代码里到处散落多个 bool，后续维护时更不容易互相打架。
    /// </summary>
    public enum AppRuntimeState
    {
        /// <summary>
        /// 空闲状态。
        /// 通常表示还没有选择目标坐标，不能直接开始连点。
        /// </summary>
        Idle,

        /// <summary>
        /// 已准备状态。
        /// 表示已经有目标坐标，可以通过按钮或热键启动连点。
        /// </summary>
        Ready,

        /// <summary>
        /// 连点运行中。
        /// 此时后台点击循环正在执行，停止按钮和停止热键应该可用。
        /// </summary>
        Running,

        /// <summary>
        /// 正在选择坐标。
        /// 用户点击“选择位置”后进入这个状态，下一次在屏幕上的点击会被记录为目标位置。
        /// </summary>
        PickingPosition
    }
}
