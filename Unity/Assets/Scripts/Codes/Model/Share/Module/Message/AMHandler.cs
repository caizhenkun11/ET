using System;

namespace ET
{
    /// <summary>
    /// 一个消息处理
    /// </summary>
    /// <typeparam name="Message"></typeparam>
    public abstract class AMHandler<Message> : IMHandler where Message : class
    {
        protected abstract ETTask Run(Session session, Message message);

        public void Handle(Session session, object msg)
        {
            Message message = msg as Message;
            if (message == null)
            {
                Log.Error($"消息类型转换错误: {msg.GetType().Name} to {typeof(Message).Name}");
                return;
            }

            if (session.IsDisposed)
            {
                Log.Error($"session disconnect {msg}");
                return;
            }

            this.Run(session, message).Coroutine();
        }

        public Type GetMessageType()
        {
            return typeof(Message);
        }

        public Type GetResponseType()
        {
            return null;
        }
    }
}