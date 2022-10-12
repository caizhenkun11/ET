using System.Net;
using System.Text;

namespace ET.Server
{
    /// <summary>
    /// http助手
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// 响应
        /// </summary>
        /// <param name="context"></param>
        /// <param name="response"></param>
        public static void Response(HttpListenerContext context, object response)
        {
            byte[] bytes = JsonHelper.ToJson(response).ToUtf8();
            context.Response.StatusCode = 200;
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.ContentLength64 = bytes.Length;
            context.Response.OutputStream.Write(bytes, 0, bytes.Length);
        }
    }
}