using System;
using System.Collections;
using System.Diagnostics;

namespace ET.Server
{
    /// <summary>
    /// 监听助手
    /// </summary>
    public static class WatcherHelper
    {
        /// <summary>
        /// 获取本机配置
        /// </summary>
        /// <returns></returns>
        public static StartMachineConfig GetThisMachineConfig()
        {
            string[] localIP = NetworkHelper.GetAddressIPs();
            StartMachineConfig startMachineConfig = null;
            foreach (StartMachineConfig config in StartMachineConfigCategory.Instance.GetAll().Values)
            {
                if (!WatcherHelper.IsThisMachine(config.InnerIP, localIP))
                {
                    continue;
                }
                startMachineConfig = config;
                break;
            }

            if (startMachineConfig == null)
            {
                throw new Exception("not found this machine ip config!");
            }

            return startMachineConfig;
        }
        /// <summary>
        /// 是否本机
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="localIPs"></param>
        /// <returns></returns>
        public static bool IsThisMachine(string ip, string[] localIPs)
        {
            if (ip != "127.0.0.1" && ip != "0.0.0.0" && !((IList)localIPs).Contains(ip))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 开始处理
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="createScenes"></param>
        /// <returns></returns>
        public static Process StartProcess(int processId, int createScenes = 0)
        {
            StartProcessConfig startProcessConfig = StartProcessConfigCategory.Instance.Get(processId);
            const string exe = "dotnet";
            string arguments = $"App.dll" +
                    $" --Process={startProcessConfig.Id}" +
                    $" --AppType=Server" +
                    $" --StartConfig={Options.Instance.StartConfig}" +
                    $" --Develop={Options.Instance.Develop}" +
                    $" --CreateScenes={createScenes}" +
                    $" --LogLevel={Options.Instance.LogLevel}" +
                    $" --Console={Options.Instance.Console}";
            Log.Debug($"{exe} {arguments}");
            Process process = ProcessHelper.Run(exe, arguments);
            return process;
        }
    }
}