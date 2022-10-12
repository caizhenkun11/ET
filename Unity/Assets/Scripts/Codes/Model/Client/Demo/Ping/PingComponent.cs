namespace ET.Client
{
    /// <summary>
    /// ping组件
    /// </summary>
    [ComponentOf(typeof(Session))]
    public class PingComponent : Entity, IAwake, IDestroy
    {
        public long Ping; //延迟值
    }
}