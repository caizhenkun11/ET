﻿

using System.Collections.Generic;
using System.IO;

namespace ET.Server
{
    /// <summary>
    /// 消息助手
    /// </summary>
    public static class MessageHelper
    {
        /// <summary>
        /// 通知单元增加
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="sendUnit"></param>
        public static void NoticeUnitAdd(Unit unit, Unit sendUnit)
        {
            M2C_CreateUnits createUnits = new M2C_CreateUnits();
            createUnits.Units.Add(UnitHelper.CreateUnitInfo(sendUnit));
            MessageHelper.SendToClient(unit, createUnits);
        }
        /// <summary>
        /// 通知单元移除
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="sendUnit"></param>
        public static void NoticeUnitRemove(Unit unit, Unit sendUnit)
        {
            M2C_RemoveUnits removeUnits = new M2C_RemoveUnits();
            removeUnits.Units.Add(sendUnit.Id);
            MessageHelper.SendToClient(unit, removeUnits);
        }
        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="message"></param>
        public static void Broadcast(Unit unit, IActorMessage message)
        {
            Dictionary<long, AOIEntity> dict = unit.GetBeSeePlayers();
            // 网络底层做了优化，同一个消息不会多次序列化
            foreach (AOIEntity u in dict.Values)
            {
                ActorMessageSenderComponent.Instance.Send(u.Unit.GetComponent<UnitGateComponent>().GateSessionActorId, message);
            }
        }
        /// <summary>
        /// 发送给客户端
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="message"></param>
        public static void SendToClient(Unit unit, IActorMessage message)
        {
            SendActor(unit.GetComponent<UnitGateComponent>().GateSessionActorId, message);
        }


        /// <summary>
        /// 发送协议给ActorLocation
        /// </summary>
        /// <param name="id">注册Actor的Id</param>
        /// <param name="message"></param>
        public static void SendToLocationActor(long id, IActorLocationMessage message)
        {
            ActorLocationSenderComponent.Instance.Send(id, message);
        }

        /// <summary>
        /// 发送协议给Actor
        /// </summary>
        /// <param name="actorId">注册Actor的InstanceId</param>
        /// <param name="message"></param>
        public static void SendActor(long actorId, IActorMessage message)
        {
            ActorMessageSenderComponent.Instance.Send(actorId, message);
        }

        /// <summary>
        /// 发送RPC协议给Actor
        /// </summary>
        /// <param name="actorId">注册Actor的InstanceId</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async ETTask<IActorResponse> CallActor(long actorId, IActorRequest message)
        {
            return await ActorMessageSenderComponent.Instance.Call(actorId, message);
        }

        /// <summary>
        /// 发送RPC协议给ActorLocation
        /// </summary>
        /// <param name="id">注册Actor的Id</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async ETTask<IActorResponse> CallLocationActor(long id, IActorLocationRequest message)
        {
            return await ActorLocationSenderComponent.Instance.Call(id, message);
        }
    }
}