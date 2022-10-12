using System.Net;
using System.Net.Sockets;

namespace ET.Server
{
    /// <summary>
    /// 网络内部组件系统
    /// </summary>
    [FriendOf(typeof(NetInnerComponent))]
    public static class NetInnerComponentSystem
    {
        /// <summary>
        /// 网络内部组件激活系统
        /// </summary>
        [ObjectSystem]
        public class NetInnerComponentAwakeSystem : AwakeSystem<NetInnerComponent>
        {
            protected override void Awake(NetInnerComponent self)
            {
                NetInnerComponent.Instance = self;

                switch (self.InnerProtocol)
                {
                    case NetworkProtocol.TCP:
                        {
                            self.ServiceId = NetServices.Instance.AddService(new TService(AddressFamily.InterNetwork, ServiceType.Inner));
                            break;
                        }
                    case NetworkProtocol.KCP:
                        {
                            self.ServiceId = NetServices.Instance.AddService(new KService(AddressFamily.InterNetwork, ServiceType.Inner));
                            break;
                        }
                }

                NetServices.Instance.RegisterReadCallback(self.ServiceId, self.OnRead);
                NetServices.Instance.RegisterErrorCallback(self.ServiceId, self.OnError);
            }
        }
        /// <summary>
        /// 网络内部组件激活1系统
        /// </summary>
        [ObjectSystem]
        public class NetInnerComponentAwake1System : AwakeSystem<NetInnerComponent, IPEndPoint>
        {
            protected override void Awake(NetInnerComponent self, IPEndPoint address)
            {
                NetInnerComponent.Instance = self;

                switch (self.InnerProtocol)
                {
                    case NetworkProtocol.TCP:
                        {
                            self.ServiceId = NetServices.Instance.AddService(new TService(address, ServiceType.Inner));
                            break;
                        }
                    case NetworkProtocol.KCP:
                        {
                            self.ServiceId = NetServices.Instance.AddService(new KService(address, ServiceType.Inner));
                            break;
                        }
                }

                NetServices.Instance.RegisterAcceptCallback(self.ServiceId, self.OnAccept);
                NetServices.Instance.RegisterReadCallback(self.ServiceId, self.OnRead);
                NetServices.Instance.RegisterErrorCallback(self.ServiceId, self.OnError);
            }
        }
        /// <summary>
        /// 网络内部组件销毁系统
        /// </summary>
        [ObjectSystem]
        public class NetInnerComponentDestroySystem : DestroySystem<NetInnerComponent>
        {
            protected override void Destroy(NetInnerComponent self)
            {
                NetServices.Instance.RemoveService(self.ServiceId);
            }
        }
        /// <summary>
        /// 监听读取
        /// </summary>
        /// <param name="self"></param>
        /// <param name="channelId"></param>
        /// <param name="actorId"></param>
        /// <param name="message"></param>
        private static void OnRead(this NetInnerComponent self, long channelId, long actorId, object message)
        {
            Session session = self.GetChild<Session>(channelId);
            if (session == null)
            {
                return;
            }

            session.LastRecvTime = TimeHelper.ClientFrameTime();

            self.HandleMessage(actorId, message);
        }
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="self"></param>
        /// <param name="actorId"></param>
        /// <param name="message"></param>
        public static void HandleMessage(this NetInnerComponent self, long actorId, object message)
        {
            EventSystem.Instance.Publish(Root.Instance.Scene, new NetInnerComponentOnRead() { ActorId = actorId, Message = message });
        }
        /// <summary>
        /// 监听错误
        /// </summary>
        /// <param name="self"></param>
        /// <param name="channelId"></param>
        /// <param name="error"></param>
        private static void OnError(this NetInnerComponent self, long channelId, int error)
        {
            Session session = self.GetChild<Session>(channelId);
            if (session == null)
            {
                return;
            }

            session.Error = error;
            session.Dispose();
        }

        // 这个channelId是由CreateAcceptChannelId生成的
        /// <summary>
        /// 监听接受
        /// </summary>
        /// <param name="self"></param>
        /// <param name="channelId"></param>
        /// <param name="ipEndPoint"></param>
        private static void OnAccept(this NetInnerComponent self, long channelId, IPEndPoint ipEndPoint)
        {
            Session session = self.AddChildWithId<Session, int>(channelId, self.ServiceId);
            session.RemoteAddress = ipEndPoint;
            //session.AddComponent<SessionIdleCheckerComponent, int, int, int>(NetThreadComponent.checkInteral, NetThreadComponent.recvMaxIdleTime, NetThreadComponent.sendMaxIdleTime);
        }
        /// <summary>
        /// 创建内部会话
        /// </summary>
        /// <param name="self"></param>
        /// <param name="channelId"></param>
        /// <param name="ipEndPoint"></param>
        /// <returns></returns>
        private static Session CreateInner(this NetInnerComponent self, long channelId, IPEndPoint ipEndPoint)
        {
            Session session = self.AddChildWithId<Session, int>(channelId, self.ServiceId);
            session.RemoteAddress = ipEndPoint;
            NetServices.Instance.CreateChannel(self.ServiceId, channelId, ipEndPoint);

            //session.AddComponent<InnerPingComponent>();
            //session.AddComponent<SessionIdleCheckerComponent, int, int, int>(NetThreadComponent.checkInteral, NetThreadComponent.recvMaxIdleTime, NetThreadComponent.sendMaxIdleTime);

            return session;
        }

        // 内网actor session，channelId是进程号
        public static Session Get(this NetInnerComponent self, long channelId)
        {
            Session session = self.GetChild<Session>(channelId);
            if (session != null)
            {
                return session;
            }

            IPEndPoint ipEndPoint = StartProcessConfigCategory.Instance.Get((int)channelId).InnerIPPort;
            session = self.CreateInner(channelId, ipEndPoint);
            return session;
        }
    }
}