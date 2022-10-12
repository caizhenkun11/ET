namespace ET
{
    /// <summary>
    /// 单元组件
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public class UnitComponent : Entity, IAwake, IDestroy
    {
    }
}