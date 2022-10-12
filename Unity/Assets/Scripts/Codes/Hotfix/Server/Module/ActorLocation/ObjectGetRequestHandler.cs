using System;

namespace ET.Server
{
    /// <summary>
    /// 对象获取请求处理
    /// </summary>
    [ActorMessageHandler(SceneType.Location)]
    public class ObjectGetRequestHandler : AMActorRpcHandler<Scene, ObjectGetRequest, ObjectGetResponse>
    {
        protected override async ETTask Run(Scene scene, ObjectGetRequest request, ObjectGetResponse response, Action reply)
        {
            long instanceId = await scene.GetComponent<LocationComponent>().Get(request.Key);
            response.InstanceId = instanceId;
            reply();
        }
    }
}