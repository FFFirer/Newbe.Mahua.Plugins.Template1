using Newbe.Mahua.MahuaEvents;
using System;
using Newbe.Mahua.Plugins.Template1.Services;

namespace Newbe.Mahua.Plugins.Template1.MahuaEvents
{
    /// <summary>
    /// 插件初始化事件
    /// </summary>
    public class InitializationMahuaEvent1
        : IInitializationMahuaEvent
    {
        private readonly IMahuaApi _mahuaApi;
        private readonly IWebHost _webhost;

        public InitializationMahuaEvent1(
            IMahuaApi mahuaApi, 
            IWebHost webHost)
        {
            _mahuaApi = mahuaApi;
            _webhost = webHost;
        }

        public void Initialized(InitializedContext context)
        {
            // 在本地地址上启动Web服务，可以根据需求改变端口
            _webhost.StartAsync("http://localhost:65238", _mahuaApi.GetSourceContainer());

            // 不要忘记在MahuaModule中注册
        }
    }
}
