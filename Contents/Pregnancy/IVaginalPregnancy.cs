namespace MonoCoC.Contents.Pregnancy
{
    /// <summary>
    /// 原版 VaginalPregnancy.hx：阴道妊娠的表现接口。
    /// 实现类至少要提供分娩场景，并在分娩后清理妊娠状态。
    /// </summary>
    public interface IVaginalPregnancy
    {
        /// <summary>原版 updateVaginalPregnancy()：推进妊娠；返回 true 表示需要刷新显示。</summary>
        bool UpdateVaginalPregnancy();

        /// <summary>原版 vaginalBirth()：分娩并清理妊娠。</summary>
        void VaginalBirth();
    }
}