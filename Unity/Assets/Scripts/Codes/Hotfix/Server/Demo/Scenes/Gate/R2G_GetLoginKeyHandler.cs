using System;


namespace ET.Server
{
    /// <summary>
    /// 获取登入密钥处理
    /// </summary>
    [ActorMessageHandler(SceneType.Gate)]
    public class R2G_GetLoginKeyHandler : AMActorRpcHandler<Scene, R2G_GetLoginKey, G2R_GetLoginKey>
    {
        protected override async ETTask Run(Scene scene, R2G_GetLoginKey request, G2R_GetLoginKey response, Action reply)
        {
            long key = RandomGenerator.RandInt64();
            scene.GetComponent<GateSessionKeyComponent>().Add(key, request.Account);
            response.Key = key;
            response.GateId = scene.Id;
            reply();
            await ETTask.CompletedTask;
        }
    }
}