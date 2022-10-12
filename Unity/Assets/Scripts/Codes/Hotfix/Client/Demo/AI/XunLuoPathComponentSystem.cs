using Unity.Mathematics;

namespace ET.Client
{
    /// <summary>
    /// 巡逻路线组件系统
    /// </summary>
    [FriendOf(typeof(XunLuoPathComponent))]
    public static class XunLuoPathComponentSystem
    {
        /// <summary>
        /// 获取当前路径
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static float3 GetCurrent(this XunLuoPathComponent self)
        {
            return self.path[self.Index];
        }
        /// <summary>
        /// 设置移动到下一个路径序号
        /// </summary>
        /// <param name="self"></param>
        public static void MoveNext(this XunLuoPathComponent self)
        {
            self.Index = ++self.Index % self.path.Length;
        }
    }
}