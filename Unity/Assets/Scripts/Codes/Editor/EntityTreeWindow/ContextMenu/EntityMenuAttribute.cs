using System;

namespace ET
{
    /// <summary>
    ///  实体菜单属性
    /// </summary>
    public class EntityMenuAttribute : Attribute
    {
        public readonly Type bind_to;
        public readonly string menu_name;

        public EntityMenuAttribute(Type bind_to, string menu_name)
        {
            this.bind_to = bind_to;
            this.menu_name = menu_name;
        }
    }
}