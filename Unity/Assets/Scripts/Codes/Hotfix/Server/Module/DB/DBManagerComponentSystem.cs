using System;

namespace ET.Server
{
    /// <summary>
    /// 数据库管理组件系统
    /// </summary>
    [FriendOf(typeof(DBManagerComponent))]
    public static class DBManagerComponentSystem
    {
        /// <summary>
        /// 数据库管理组件激活系统
        /// </summary>
        [ObjectSystem]
        public class DBManagerComponentAwakeSystem : AwakeSystem<DBManagerComponent>
        {
            protected override void Awake(DBManagerComponent self)
            {
                DBManagerComponent.Instance = self;
            }
        }
        /// <summary>
        /// 数据库管理组件销毁系统
        /// </summary>
        [ObjectSystem]
        public class DBManagerComponentDestroySystem : DestroySystem<DBManagerComponent>
        {
            protected override void Destroy(DBManagerComponent self)
            {
                DBManagerComponent.Instance = null;
            }
        }
        /// <summary>
        /// 获取区域数据库
        /// </summary>
        /// <param name="self"></param>
        /// <param name="zone"></param>
        /// <returns></returns>
        public static DBComponent GetZoneDB(this DBManagerComponent self, int zone)
        {
            DBComponent dbComponent = self.DBComponents[zone];
            if (dbComponent != null)
            {
                return dbComponent;
            }

            StartZoneConfig startZoneConfig = StartZoneConfigCategory.Instance.Get(zone);
            if (startZoneConfig.DBConnection == "")
            {
                throw new Exception($"zone: {zone} not found mongo connect string");
            }

            dbComponent = self.AddChild<DBComponent, string, string, int>(startZoneConfig.DBConnection, startZoneConfig.DBName, zone);
            self.DBComponents[zone] = dbComponent;
            return dbComponent;
        }
    }
}