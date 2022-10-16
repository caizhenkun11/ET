﻿using System;

namespace ET.Server
{
    /// <summary>
    /// 对象解锁请求处理
    /// </summary>
    [ActorMessageHandler(SceneType.Location)]
    public class ObjectUnLockRequestHandler : AMActorRpcHandler<Scene, ObjectUnLockRequest, ObjectUnLockResponse>
    {
        protected override async ETTask Run(Scene scene, ObjectUnLockRequest request, ObjectUnLockResponse response, Action reply)
        {
            scene.GetComponent<LocationComponent>().UnLock(request.Key, request.OldInstanceId, request.InstanceId);

            reply();

            await ETTask.CompletedTask;
        }
    }
}