using System;
using System.Net;

namespace ET.Client
{
    /// <summary>
    /// 路由器检查组件激活系统
    /// </summary>
    [ObjectSystem]
    public class RouterCheckComponentAwakeSystem : AwakeSystem<RouterCheckComponent>
    {
        protected override void Awake(RouterCheckComponent self)
        {
            CheckAsync(self).Coroutine();
        }
        /// <summary>
        /// 检查异步
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        private static async ETTask CheckAsync(RouterCheckComponent self)
        {
            Session session = self.GetParent<Session>();
            long instanceId = self.InstanceId;

            while (true)
            {
                if (self.InstanceId != instanceId)
                {
                    return;
                }

                await TimerComponent.Instance.WaitAsync(1000);

                if (self.InstanceId != instanceId)
                {
                    return;
                }

                long time = TimeHelper.ClientFrameTime();

                if (time - session.LastRecvTime < 7 * 1000)
                {
                    continue;
                }

                try
                {
                    long sessionId = session.Id;

                    (uint localConn, uint remoteConn) = await NetServices.Instance.GetKChannelConn(session.ServiceId, sessionId);

                    IPEndPoint realAddress = self.GetParent<Session>().RemoteAddress;
                    Log.Info($"get recvLocalConn start: {self.ClientScene().Id} {realAddress} {localConn} {remoteConn}");

                    (uint recvLocalConn, IPEndPoint routerAddress) = await RouterHelper.GetRouterAddress(self.ClientScene(), realAddress, localConn, remoteConn);
                    if (recvLocalConn == 0)
                    {
                        Log.Error($"get recvLocalConn fail: {self.ClientScene().Id} {routerAddress} {realAddress} {localConn} {remoteConn}");
                        continue;
                    }

                    Log.Info($"get recvLocalConn ok: {self.ClientScene().Id} {routerAddress} {realAddress} {recvLocalConn} {localConn} {remoteConn}");

                    session.LastRecvTime = TimeHelper.ClientNow();

                    NetServices.Instance.ChangeAddress(session.ServiceId, sessionId, routerAddress);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}