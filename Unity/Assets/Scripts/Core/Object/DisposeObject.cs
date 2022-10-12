using System;
using System.ComponentModel;

namespace ET
{
    /// <summary>
    /// 销毁对象
    /// </summary>
    public abstract class DisposeObject : Object, IDisposable, ISupportInitialize
    {
        public virtual void Dispose()
        {
        }

        public virtual void BeginInit()
        {
        }

        public virtual void EndInit()
        {
        }
    }
}