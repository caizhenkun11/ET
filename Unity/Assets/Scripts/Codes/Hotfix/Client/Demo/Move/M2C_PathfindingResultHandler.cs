

namespace ET.Client
{
    /// <summary>
    /// 路径查找结果处理
    /// </summary>
    [MessageHandler(SceneType.Client)]
    public class M2C_PathfindingResultHandler : AMHandler<M2C_PathfindingResult>
    {
        protected override async ETTask Run(Session session, M2C_PathfindingResult message)
        {
            Unit unit = session.DomainScene().CurrentScene().GetComponent<UnitComponent>().Get(message.Id);

            float speed = unit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed);

            await unit.GetComponent<MoveComponent>().MoveToAsync(message.Points, speed);
        }
    }
}
