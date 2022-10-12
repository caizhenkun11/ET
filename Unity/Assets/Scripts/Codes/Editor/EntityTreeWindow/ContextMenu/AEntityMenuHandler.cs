namespace ET
{
    /// <summary>
    /// 实体菜单控制
    /// </summary>
    public abstract class AEntityMenuHandler
    {
        internal string menuName;

        public abstract void OnClick(Entity entity);
    }
}