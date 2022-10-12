using System;

namespace ET.Server
{
    /// <summary>
    /// 对象移除请求处理
    /// </summary>
    [ActorMessageHandler(SceneType.Location)]
    public class ObjectRemoveRequestHandler : AMActorRpcHandler<Scene, ObjectRemoveRequest, ObjectRemoveResponse>
    {
        protected override async ETTask Run(Scene scene, ObjectRemoveRequest request, ObjectRemoveResponse response, Action reply)
        {
            await scene.GetComponent<LocationComponent>().Remove(request.Key);

            reply();
        }
    }
}