﻿using System.Collections.Generic;
using MongoDB.Bson;

namespace ET.Server
{
    /// <summary>
    /// 转移助手
    /// </summary>
    public static class TransferHelper
    {
        public static async ETTask Transfer(Unit unit, long sceneInstanceId, string sceneName)
        {
            // location加锁
            long unitId = unit.Id;
            long unitInstanceId = unit.InstanceId;

            M2M_UnitTransferRequest request = new M2M_UnitTransferRequest() { Entitys = new List<byte[]>() };
            request.OldInstanceId = unitInstanceId;
            request.Unit = unit.ToBson();
            foreach (Entity entity in unit.Components.Values)
            {
                if (entity is ITransfer)
                {
                    request.Entitys.Add(entity.ToBson());
                }
            }
            unit.Dispose();

            await LocationProxyComponent.Instance.Lock(unitId, unitInstanceId);
            await ActorMessageSenderComponent.Instance.Call(sceneInstanceId, request);
        }
    }
}