using System;
using System.IO;
using System.Net.Http;

namespace ET.Client
{
    /// <summary>
    /// http客户端助手
    /// </summary>
    public static class HttpClientHelper
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public static async ETTask<string> Get(string link)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(link);
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"http request fail: {link.Substring(0, link.IndexOf('?'))}\n{e}");
            }
        }
    }
}