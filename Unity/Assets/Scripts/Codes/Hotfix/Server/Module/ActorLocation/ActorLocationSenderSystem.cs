using System;

namespace ET.Server
{
    /// <summary>
    /// 演员定位发送激活系统
    /// </summary>
    [ObjectSystem]
    public class ActorLocationSenderAwakeSystem : AwakeSystem<ActorLocationSender>
    {
        protected override void Awake(ActorLocationSender self)
        {
            self.LastSendOrRecvTime = TimeHelper.ServerNow();
            self.ActorId = 0;
            self.Error = 0;
        }
    }

    /// <summary>
    /// 演员定位发送销毁系统
    /// </summary>
    [ObjectSystem]
    public class ActorLocationSenderDestroySystem : DestroySystem<ActorLocationSender>
    {
        protected override void Destroy(ActorLocationSender self)
        {
            Log.Debug($"actor location remove: {self.Id}");
            self.LastSendOrRecvTime = 0;
            self.ActorId = 0;
            self.Error = 0;
        }
    }
}