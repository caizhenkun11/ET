using System.Collections;
using System.Diagnostics;

namespace ET.Server
{
    /// <summary>
    /// 坚定者组件系统
    /// </summary>
    [FriendOf(typeof(WatcherComponent))]
    public static class WatcherComponentSystem
    {
        public class WatcherComponentAwakeSystem : AwakeSystem<WatcherComponent>
        {
            protected override void Awake(WatcherComponent self)
            {
                WatcherComponent.Instance = self;
            }
        }

        public class WatcherComponentDestroySystem : DestroySystem<WatcherComponent>
        {
            protected override void Destroy(WatcherComponent self)
            {
                WatcherComponent.Instance = null;
            }
        }
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="self"></param>
        /// <param name="createScenes"></param>
        public static void Start(this WatcherComponent self, int createScenes = 0)
        {
            string[] localIP = NetworkHelper.GetAddressIPs();
            var processConfigs = StartProcessConfigCategory.Instance.GetAll();
            foreach (StartProcessConfig startProcessConfig in processConfigs.Values)
            {
                if (!WatcherHelper.IsThisMachine(startProcessConfig.InnerIP, localIP))
                {
                    continue;
                }
                Process process = WatcherHelper.StartProcess(startProcessConfig.Id, createScenes);
                self.Processes.Add(startProcessConfig.Id, process);
            }
        }
    }
}