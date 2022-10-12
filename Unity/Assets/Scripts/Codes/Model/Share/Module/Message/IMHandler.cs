using System;

namespace ET
{
    /// <summary>
    /// 消息处理接口
    /// </summary>
    public interface IMHandler
    {
        void Handle(Session session, object message);
        Type GetMessageType();

        Type GetResponseType();
    }
}