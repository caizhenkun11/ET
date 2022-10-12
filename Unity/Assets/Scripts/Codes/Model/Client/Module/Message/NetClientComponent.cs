using System.Net.Sockets;

namespace ET.Client
{
    /// <summary>
    /// 网络客户端组件监听读取
    /// </summary>
    public struct NetClientComponentOnRead
    {
        public Session Session;
        public object Message;
    }
    /// <summary>
    /// 网络客户端组件
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public class NetClientComponent : Entity, IAwake<AddressFamily>, IDestroy
    {
        public int ServiceId;
    }
}