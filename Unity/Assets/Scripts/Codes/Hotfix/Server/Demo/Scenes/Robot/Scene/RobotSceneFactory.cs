namespace ET.Server
{
    /// <summary>
    /// 机器人场景工厂
    /// </summary>
    public static class RobotSceneFactory
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="id"></param>
        /// <param name="instanceId"></param>
        /// <param name="zone"></param>
        /// <param name="name"></param>
        /// <param name="sceneType"></param>
        /// <param name="startSceneConfig"></param>
        /// <returns></returns>
        public static async ETTask<Scene> Create(
            Entity parent,
            long id,
            long instanceId,
            int zone,
            string name,
            SceneType sceneType,
            StartSceneConfig startSceneConfig = null
        )
        {
            await ETTask.CompletedTask;
            Log.Info($"create scene: {sceneType} {name} {zone}");
            Scene scene = EntitySceneFactory.CreateScene(id, instanceId, zone, sceneType, name, parent);

            scene.AddComponent<MailBoxComponent, MailboxType>(MailboxType.UnOrderMessageDispatcher);

            switch (scene.SceneType)
            {
                case SceneType.Robot:
                    scene.AddComponent<RobotManagerComponent>();
                    break;
            }

            return scene;
        }
    }
}