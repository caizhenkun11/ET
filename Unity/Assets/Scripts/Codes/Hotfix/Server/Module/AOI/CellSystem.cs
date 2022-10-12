using System.Collections.Generic;
using System.Text;

namespace ET.Server
{
    /// <summary>
    /// 单元系统
    /// </summary>
    [FriendOf(typeof(Cell))]
    public static class CellSystem
    {
        /// <summary>
        /// 单元销毁系统
        /// </summary>
        [ObjectSystem]
        public class CellDestroySystem : DestroySystem<Cell>
        {
            protected override void Destroy(Cell self)
            {
                self.AOIUnits.Clear();

                self.SubsEnterEntities.Clear();

                self.SubsLeaveEntities.Clear();
            }
        }

        public static void Add(this Cell self, AOIEntity aoiEntity)
        {
            self.AOIUnits.Add(aoiEntity.Id, aoiEntity);
        }

        public static void Remove(this Cell self, AOIEntity aoiEntity)
        {
            self.AOIUnits.Remove(aoiEntity.Id);
        }
        /// <summary>
        /// 单元id2字符串
        /// </summary>
        /// <param name="cellId"></param>
        /// <returns></returns>
        public static string CellIdToString(this long cellId)
        {
            int y = (int)(cellId & 0xffffffff);
            int x = (int)((ulong)cellId >> 32);
            return $"{x}:{y}";
        }
        /// <summary>
        /// 单元id2字符串
        /// </summary>
        /// <param name="cellIds"></param>
        /// <returns></returns>
        public static string CellIdToString(this HashSet<long> cellIds)
        {
            StringBuilder sb = new StringBuilder();
            foreach (long cellId in cellIds)
            {
                sb.Append(cellId.CellIdToString());
                sb.Append(",");
            }

            return sb.ToString();
        }
    }
}