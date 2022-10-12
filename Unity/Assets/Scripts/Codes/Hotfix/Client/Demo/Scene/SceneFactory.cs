using System.Net.Sockets;

namespace ET.Client
{
    /// <summary>
    /// 场景工厂
    /// </summary>
    public static class SceneFactory
    {
        /// <summary>
        /// 创建客户端场景
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static async ETTask<Scene> CreateClientScene(int zone, string name)
        {
            await ETTask.CompletedTask;

            Scene clientScene = EntitySceneFactory.CreateScene(zone, SceneType.Client, name, ClientSceneManagerComponent.Instance);
            clientScene.AddComponent<CurrentScenesComponent>();
            clientScene.AddComponent<ObjectWait>();
            clientScene.AddComponent<PlayerComponent>();

            EventSystem.Instance.Publish(clientScene, new EventType.AfterCreateClientScene());
            return clientScene;
        }
        /// <summary>
        /// 创建当前场景
        /// </summary>
        /// <param name="id"></param>
        /// <param name="zone"></param>
        /// <param name="name"></param>
        /// <param name="currentScenesComponent"></param>
        /// <returns></returns>
        public static Scene CreateCurrentScene(long id, int zone, string name, CurrentScenesComponent currentScenesComponent)
        {
            Scene currentScene = EntitySceneFactory.CreateScene(id, IdGenerater.Instance.GenerateInstanceId(), zone, SceneType.Current, name, currentScenesComponent);
            currentScenesComponent.Scene = currentScene;

            EventSystem.Instance.Publish(currentScene, new EventType.AfterCreateCurrentScene());
            return currentScene;
        }


    }
}