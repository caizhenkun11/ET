﻿namespace ET.Client
{
    /// <summary>
    /// 网络客户端组件监听读取事件
    /// </summary>
    [Event(SceneType.Process)]
    public class NetClientComponentOnReadEvent : AEvent<NetClientComponentOnRead>
    {
        protected override async ETTask Run(Scene scene, NetClientComponentOnRead args)
        {
            Session session = args.Session;
            object message = args.Message;
            if (message is IResponse response)
            {
                session.OnResponse(response);
                return;
            }

            // 普通消息或者是Rpc请求消息
            MessageDispatcherComponent.Instance.Handle(session, message);
            await ETTask.CompletedTask;
        }
    }
}