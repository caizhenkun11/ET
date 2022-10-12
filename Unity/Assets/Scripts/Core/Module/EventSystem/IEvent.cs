using System;

namespace ET
{
    /// <summary>
    /// 事件接口
    /// </summary>
    public interface IEvent
    {
        Type Type { get; }
    }
    /// <summary>
    /// 事件
    /// </summary>
    /// <typeparam name="A"></typeparam>
    public abstract class AEvent<A> : IEvent where A : struct
    {
        public Type Type
        {
            get
            {
                return typeof(A);
            }
        }

        protected abstract ETTask Run(Scene scene, A a);

        public async ETTask Handle(Scene scene, A a)
        {
            try
            {
                await Run(scene, a);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}