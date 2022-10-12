using System;

namespace ET
{
    /// <summary>
    ///  谁的朋友属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class FriendOfAttribute : Attribute
    {
        public Type Type;

        public FriendOfAttribute(Type type)
        {
            this.Type = type;
        }
    }
}