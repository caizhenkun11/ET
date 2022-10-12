﻿namespace ET.Client
{
    /// <summary>
    /// 创建自己单元处理
    /// </summary>
    [MessageHandler(SceneType.Client)]
    public class M2C_CreateMyUnitHandler : AMHandler<M2C_CreateMyUnit>
    {
        protected override async ETTask Run(Session session, M2C_CreateMyUnit message)
        {
            // 通知场景切换协程继续往下走
            session.DomainScene().GetComponent<ObjectWait>().Notify(new Wait_CreateMyUnit() { Message = message });
            await ETTask.CompletedTask;
        }
    }
}
