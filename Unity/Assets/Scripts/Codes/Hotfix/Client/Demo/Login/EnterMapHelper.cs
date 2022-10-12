using System;


namespace ET.Client
{
    /// <summary>
    /// 进入地图助手
    /// </summary>
    public static class EnterMapHelper
    {
        /// <summary>
        /// 进入地图异步
        /// </summary>
        /// <param name="clientScene"></param>
        /// <returns></returns>
        public static async ETTask EnterMapAsync(Scene clientScene)
        {
            try
            {
                G2C_EnterMap g2CEnterMap = await clientScene.GetComponent<SessionComponent>().Session.Call(new C2G_EnterMap()) as G2C_EnterMap;
                clientScene.GetComponent<PlayerComponent>().MyId = g2CEnterMap.MyId;

                // 等待场景切换完成
                await clientScene.GetComponent<ObjectWait>().Wait<Wait_SceneChangeFinish>();

                EventSystem.Instance.Publish(clientScene, new EventType.EnterMapFinish());
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}