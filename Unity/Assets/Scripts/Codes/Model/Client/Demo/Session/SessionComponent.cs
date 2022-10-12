namespace ET.Client
{
    /// <summary>
    /// 会话组件
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public class SessionComponent : Entity, IAwake, IDestroy
    {
        public Session Session { get; set; }
    }
}
