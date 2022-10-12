namespace ET.Client
{
    /// <summary>
    /// 开始场景改变处理
    /// </summary>
    [MessageHandler(SceneType.Client)]
    public class M2C_StartSceneChangeHandler : AMHandler<M2C_StartSceneChange>
    {
        protected override async ETTask Run(Session session, M2C_StartSceneChange message)
        {
            await SceneChangeHelper.SceneChangeTo(session.ClientScene(), message.SceneName, message.SceneInstanceId);
        }
    }
}
