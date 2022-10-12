using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ET.Client
{
    /// <summary>
    /// 路由器地址组件系统
    /// </summary>
    [FriendOf(typeof(RouterAddressComponent))]
    public static class RouterAddressComponentSystem
    {
        /// <summary>
        /// 路由器地址组件激活系统
        /// </summary>
        public class RouterAddressComponentAwakeSystem : AwakeSystem<RouterAddressComponent, string, int>
        {
            protected override void Awake(RouterAddressComponent self, string address, int port)
            {
                self.RouterManagerHost = address;
                self.RouterManagerPort = port;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static async ETTask Init(this RouterAddressComponent self)
        {
            self.RouterManagerIPAddress = NetworkHelper.GetHostAddress(self.RouterManagerHost);
            await self.GetAllRouter();
        }
        /// <summary>
        /// 获取所有路由器
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        private static async ETTask GetAllRouter(this RouterAddressComponent self)
        {
            string url = $"http://{self.RouterManagerHost}:{self.RouterManagerPort}/get_router?v={RandomGenerator.RandUInt32()}";
            Log.Debug($"start get router info: {url}");
            string routerInfo = await HttpClientHelper.Get(url);
            Log.Debug($"recv router info: {routerInfo}");
            HttpGetRouterResponse httpGetRouterResponse = JsonHelper.FromJson<HttpGetRouterResponse>(routerInfo);
            self.Info = httpGetRouterResponse;
            Log.Debug($"start get router info finish: {JsonHelper.ToJson(httpGetRouterResponse)}");

            // 打乱顺序
            RandomGenerator.BreakRank(self.Info.Routers);

            self.WaitTenMinGetAllRouter().Coroutine();
        }

        // 等10分钟再获取一次
        public static async ETTask WaitTenMinGetAllRouter(this RouterAddressComponent self)
        {
            await TimerComponent.Instance.WaitAsync(5 * 60 * 1000);
            if (self.IsDisposed)
            {
                return;
            }
            await self.GetAllRouter();
        }
        /// <summary>
        /// 获取地址
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IPEndPoint GetAddress(this RouterAddressComponent self)
        {
            if (self.Info.Routers.Count == 0)
            {
                return null;
            }

            string address = self.Info.Routers[self.RouterIndex++ % self.Info.Routers.Count];
            string[] ss = address.Split(':');
            IPAddress ipAddress = IPAddress.Parse(ss[0]);
            if (self.RouterManagerIPAddress.AddressFamily == AddressFamily.InterNetworkV6)
            {
                ipAddress = ipAddress.MapToIPv6();
            }
            return new IPEndPoint(ipAddress, int.Parse(ss[1]));
        }
        /// <summary>
        /// 获取真正地址
        /// </summary>
        /// <param name="self"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static IPEndPoint GetRealmAddress(this RouterAddressComponent self, string account)
        {
            int v = account.Mode(self.Info.Realms.Count);
            string address = self.Info.Realms[v];
            string[] ss = address.Split(':');
            IPAddress ipAddress = IPAddress.Parse(ss[0]);
            //if (self.IPAddress.AddressFamily == AddressFamily.InterNetworkV6)
            //{ 
            //    ipAddress = ipAddress.MapToIPv6();
            //}
            return new IPEndPoint(ipAddress, int.Parse(ss[1]));
        }
    }
}