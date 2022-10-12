﻿using System;

namespace ET.Server
{
    /// <summary>
    /// 对象锁请求处理
    /// </summary>
    [ActorMessageHandler(SceneType.Location)]
    public class ObjectLockRequestHandler : AMActorRpcHandler<Scene, ObjectLockRequest, ObjectLockResponse>
    {
        protected override async ETTask Run(Scene scene, ObjectLockRequest request, ObjectLockResponse response, Action reply)
        {
            scene.GetComponent<LocationComponent>().Lock(request.Key, request.InstanceId, request.Time).Coroutine();

            reply();

            await ETTask.CompletedTask;
        }
    }
}