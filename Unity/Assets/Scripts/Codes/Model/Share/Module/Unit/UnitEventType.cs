using Unity.Mathematics;

namespace ET
{
    /// <summary>
    /// 事件类型
    /// </summary>
    namespace EventType
    {
        public struct ChangePosition
        {
            public Unit Unit;
            public float3 OldPos;
        }

        public struct ChangeRotation
        {
            public Unit Unit;
        }
    }
}