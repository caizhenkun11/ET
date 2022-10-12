namespace ET.Server
{
    /// <summary>
    /// 锁信息激活系统
    /// </summary>
    [ObjectSystem]
    public class LockInfoAwakeSystem : AwakeSystem<LockInfo, long, CoroutineLock>
    {
        protected override void Awake(LockInfo self, long lockInstanceId, CoroutineLock coroutineLock)
        {
            self.CoroutineLock = coroutineLock;
            self.LockInstanceId = lockInstanceId;
        }
    }
    /// <summary>
    /// 锁信息销毁系统
    /// </summary>
    [ObjectSystem]
    public class LockInfoDestroySystem : DestroySystem<LockInfo>
    {
        protected override void Destroy(LockInfo self)
        {
            self.CoroutineLock.Dispose();
            self.LockInstanceId = 0;
        }
    }
    /// <summary>
    /// 定位组件系统
    /// </summary>
    [FriendOf(typeof(LocationComponent))]
    [FriendOf(typeof(LockInfo))]
    public static class LocationComponentSystem
    {
        public static async ETTask Add(this LocationComponent self, long key, long instanceId)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Location, key))
            {
                self.locations[key] = instanceId;
                Log.Info($"location add key: {key} instanceId: {instanceId}");
            }
        }

        public static async ETTask Remove(this LocationComponent self, long key)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Location, key))
            {
                self.locations.Remove(key);
                Log.Info($"location remove key: {key}");
            }
        }
        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="self"></param>
        /// <param name="key"></param>
        /// <param name="instanceId"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static async ETTask Lock(this LocationComponent self, long key, long instanceId, int time = 0)
        {
            CoroutineLock coroutineLock = await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Location, key);

            LockInfo lockInfo = self.AddChild<LockInfo, long, CoroutineLock>(instanceId, coroutineLock);
            self.lockInfos.Add(key, lockInfo);

            Log.Info($"location lock key: {key} instanceId: {instanceId}");

            if (time > 0)
            {
                long lockInfoInstanceId = lockInfo.InstanceId;
                await TimerComponent.Instance.WaitAsync(time);
                if (lockInfo.InstanceId != lockInfoInstanceId)
                {
                    return;
                }

                self.UnLock(key, instanceId, instanceId);
            }
        }
        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="self"></param>
        /// <param name="key"></param>
        /// <param name="oldInstanceId"></param>
        /// <param name="newInstanceId"></param>
        public static void UnLock(this LocationComponent self, long key, long oldInstanceId, long newInstanceId)
        {
            if (!self.lockInfos.TryGetValue(key, out LockInfo lockInfo))
            {
                Log.Error($"location unlock not found key: {key} {oldInstanceId}");
                return;
            }

            if (oldInstanceId != lockInfo.LockInstanceId)
            {
                Log.Error($"location unlock oldInstanceId is different: {key} {oldInstanceId}");
                return;
            }

            Log.Info($"location unlock key: {key} instanceId: {oldInstanceId} newInstanceId: {newInstanceId}");

            self.locations[key] = newInstanceId;

            self.lockInfos.Remove(key);

            // 解锁
            lockInfo.Dispose();
        }

        public static async ETTask<long> Get(this LocationComponent self, long key)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Location, key))
            {
                self.locations.TryGetValue(key, out long instanceId);
                Log.Info($"location get key: {key} instanceId: {instanceId}");
                return instanceId;
            }
        }
    }
}