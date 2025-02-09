﻿using System;

namespace ET.Server
{
    /// <summary>
    /// 基准处理
    /// </summary>
    [MessageHandler(SceneType.BenchmarkServer)]
    public class C2G_BenchmarkHandler : AMRpcHandler<C2G_Benchmark, G2C_Benchmark>
    {
        protected override async ETTask Run(Session session, C2G_Benchmark request, G2C_Benchmark response, Action reply)
        {
            BenchmarkServerComponent benchmarkServerComponent = session.DomainScene().GetComponent<BenchmarkServerComponent>();
            if (benchmarkServerComponent.Count++ % 1000000 == 0)
            {
                Log.Debug($"benchmark count: {benchmarkServerComponent.Count} {TimeHelper.ClientNow()}");
            }
            reply();
            await ETTask.CompletedTask;
        }
    }
}