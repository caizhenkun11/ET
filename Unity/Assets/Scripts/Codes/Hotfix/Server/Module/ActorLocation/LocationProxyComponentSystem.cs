using System;

namespace ET.Server
{
    /// <summary>
    /// 定位代理组件激活系统
    /// </summary>
    [ObjectSystem]
    public class LocationProxyComponentAwakeSystem : AwakeSystem<LocationProxyComponent>
    {
        protected override void Awake(LocationProxyComponent self)
        {
            LocationProxyComponent.Instance = self;
        }
    }
    /// <summary>
    /// 定位代理组件销毁系统
    /// </summary>
    [ObjectSystem]
    public class LocationProxyComponentDestroySystem : DestroySystem<LocationProxyComponent>
    {
        protected override void Destroy(LocationProxyComponent self)
        {
            LocationProxyComponent.Instance = null;
        }
    }
    /// <summary>
    /// 定位代理组件系统
    /// </summary>
    public static class LocationProxyComponentSystem
    {
        /// <summary>
        /// 获取定位场景id
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static long GetLocationSceneId(long key)
        {
            return StartSceneConfigCategory.Instance.LocationConfig.InstanceId;
        }

        public static async ETTask Add(this LocationProxyComponent self, long key, long instanceId)
        {
            Log.Info($"location proxy add {key}, {instanceId} {TimeHelper.ServerNow()}");
            await ActorMessageSenderComponent.Instance.Call(GetLocationSceneId(key),
                new ObjectAddRequest() { Key = key, InstanceId = instanceId });
        }

        public static async ETTask Lock(this LocationProxyComponent self, long key, long instanceId, int time = 60000)
        {
            Log.Info($"location proxy lock {key}, {instanceId} {TimeHelper.ServerNow()}");
            await ActorMessageSenderComponent.Instance.Call(GetLocationSceneId(key),
                new ObjectLockRequest() { Key = key, InstanceId = instanceId, Time = time });
        }

        public static async ETTask UnLock(this LocationProxyComponent self, long key, long oldInstanceId, long instanceId)
        {
            Log.Info($"location proxy unlock {key}, {instanceId} {TimeHelper.ServerNow()}");
            await ActorMessageSenderComponent.Instance.Call(GetLocationSceneId(key),
                new ObjectUnLockRequest() { Key = key, OldInstanceId = oldInstanceId, InstanceId = instanceId });
        }

        public static async ETTask Remove(this LocationProxyComponent self, long key)
        {
            Log.Info($"location proxy add {key}, {TimeHelper.ServerNow()}");
            await ActorMessageSenderComponent.Instance.Call(GetLocationSceneId(key),
                new ObjectRemoveRequest() { Key = key });
        }

        public static async ETTask<long> Get(this LocationProxyComponent self, long key)
        {
            if (key == 0)
            {
                throw new Exception($"get location key 0");
            }

            // location server配置到共享区，一个大战区可以配置N多个location server,这里暂时为1
            ObjectGetResponse response =
                    (ObjectGetResponse)await ActorMessageSenderComponent.Instance.Call(GetLocationSceneId(key),
                        new ObjectGetRequest() { Key = key });
            return response.InstanceId;
        }

        /// <summary>
        /// 添加定位
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static async ETTask AddLocation(this Entity self)
        {
            await LocationProxyComponent.Instance.Add(self.Id, self.InstanceId);
        }
        /// <summary>
        /// 移除定位
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static async ETTask RemoveLocation(this Entity self)
        {
            await LocationProxyComponent.Instance.Remove(self.Id);
        }
    }
}