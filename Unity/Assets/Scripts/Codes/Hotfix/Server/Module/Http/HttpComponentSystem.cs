using System;
using System.Collections.Generic;
using System.Net;

namespace ET.Server
{
    /// <summary>
    /// http组件系统
    /// </summary>
    [FriendOf(typeof(HttpComponent))]
    public static class HttpComponentSystem
    {
        /// <summary>
        /// http组件激活系统
        /// </summary>
        public class HttpComponentAwakeSystem : AwakeSystem<HttpComponent, string>
        {
            protected override void Awake(HttpComponent self, string address)
            {
                try
                {
                    self.Load();

                    self.Listener = new HttpListener();

                    foreach (string s in address.Split(';'))
                    {
                        if (s.Trim() == "")
                        {
                            continue;
                        }
                        self.Listener.Prefixes.Add(s);
                    }

                    self.Listener.Start();

                    self.Accept().Coroutine();
                }
                catch (HttpListenerException e)
                {
                    throw new Exception($"请现在cmd中运行: netsh http add urlacl url=http://*:你的address中的端口/ user=Everyone, address: {address}", e);
                }
            }
        }
        /// <summary>
        /// http组件加载系统
        /// </summary>
        [ObjectSystem]
        public class HttpComponentLoadSystem : LoadSystem<HttpComponent>
        {
            protected override void Load(HttpComponent self)
            {
                self.Load();
            }
        }
        /// <summary>
        /// http组件销毁系统
        /// </summary>
        [ObjectSystem]
        public class HttpComponentDestroySystem : DestroySystem<HttpComponent>
        {
            protected override void Destroy(HttpComponent self)
            {
                self.Listener.Stop();
                self.Listener.Close();
            }
        }
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="self"></param>
        public static void Load(this HttpComponent self)
        {
            self.dispatcher = new Dictionary<string, IHttpHandler>();

            HashSet<Type> types = EventSystem.Instance.GetTypes(typeof(HttpHandlerAttribute));

            SceneType sceneType = self.GetParent<Scene>().SceneType;

            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(HttpHandlerAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                HttpHandlerAttribute httpHandlerAttribute = (HttpHandlerAttribute)attrs[0];

                if (httpHandlerAttribute.SceneType != sceneType)
                {
                    continue;
                }

                object obj = Activator.CreateInstance(type);

                IHttpHandler ihttpHandler = obj as IHttpHandler;
                if (ihttpHandler == null)
                {
                    throw new Exception($"HttpHandler handler not inherit IHttpHandler class: {obj.GetType().FullName}");
                }
                self.dispatcher.Add(httpHandlerAttribute.Path, ihttpHandler);
            }
        }
        /// <summary>
        /// 接受
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static async ETTask Accept(this HttpComponent self)
        {
            long instanceId = self.InstanceId;
            while (self.InstanceId == instanceId)
            {
                try
                {
                    HttpListenerContext context = await self.Listener.GetContextAsync();
                    self.Handle(context).Coroutine();
                }
                catch (ObjectDisposedException)
                {
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="self"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async ETTask Handle(this HttpComponent self, HttpListenerContext context)
        {
            try
            {
                IHttpHandler handler;
                if (self.dispatcher.TryGetValue(context.Request.Url.AbsolutePath, out handler))
                {
                    await handler.Handle(self.Domain, context);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            context.Request.InputStream.Dispose();
            context.Response.OutputStream.Dispose();
        }
    }
}