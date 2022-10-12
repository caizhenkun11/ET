namespace ET.Client
{
    /// <summary>
    /// 会话组件销毁系统
    /// </summary>
    public class SessionComponentDestroySystem : DestroySystem<SessionComponent>
    {
        protected override void Destroy(SessionComponent self)
        {
            self.Session.Dispose();
        }
    }
}
