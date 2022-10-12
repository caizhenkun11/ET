using System;

namespace ET
{
    /// <summary>
    /// 系统类型接口
    /// </summary>
    public interface ISystemType
    {
        Type Type();
        Type SystemType();
        InstanceQueueIndex GetInstanceQueueIndex();
    }
}