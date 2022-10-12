using System;

namespace ET
{
    /// <summary>
    /// 销毁接口
    /// </summary>
    public interface IDestroy
    {

    }
    /// <summary>
    /// 销毁系统接口
    /// </summary>
    public interface IDestroySystem : ISystemType
    {
        void Run(object o);
    }
    /// <summary>
    /// 销毁系统
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ObjectSystem]
    public abstract class DestroySystem<T> : IDestroySystem where T : IDestroy
    {
        public void Run(object o)
        {
            this.Destroy((T)o);
        }

        public Type SystemType()
        {
            return typeof(IDestroySystem);
        }

        public InstanceQueueIndex GetInstanceQueueIndex()
        {
            return InstanceQueueIndex.None;
        }

        public Type Type()
        {
            return typeof(T);
        }

        protected abstract void Destroy(T self);
    }
}
