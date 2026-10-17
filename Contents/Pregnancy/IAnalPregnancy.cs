namespace MonoCoC.Contents.Pregnancy
{
    /// <summary>
    /// 原版 AnalPregnancy.hx：肛门妊娠的表现接口。
    /// 实现类至少要提供分娩场景。
    /// </summary>
    public interface IAnalPregnancy
    {
        /// <summary>原版 updateAnalPregnancy()：推进妊娠；返回 true 表示需要刷新显示。</summary>
        bool UpdateAnalPregnancy();

        /// <summary>原版 analBirth()：分娩并输出场景。</summary>
        void AnalBirth();
    }
}