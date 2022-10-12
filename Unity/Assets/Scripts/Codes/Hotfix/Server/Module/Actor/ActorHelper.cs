using System;
using System.IO;

namespace ET.Server
{
    /// <summary>
    /// 演员助手
    /// </summary>
    public static class ActorHelper
    {
        /// <summary>
        /// 创建响应
        /// </summary>
        /// <param name="iActorRequest"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static IActorResponse CreateResponse(IActorRequest iActorRequest, int error)
        {
            Type responseType = OpcodeTypeComponent.Instance.GetResponseType(iActorRequest.GetType());
            IActorResponse response = (IActorResponse)Activator.CreateInstance(responseType);
            response.Error = error;
            response.RpcId = iActorRequest.RpcId;
            return response;
        }
    }
}