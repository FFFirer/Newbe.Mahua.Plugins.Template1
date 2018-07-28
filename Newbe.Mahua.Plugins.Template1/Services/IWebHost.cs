using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Newbe.Mahua.Plugins.Template1.Services
{
    public interface IWebHost
    {
        /// <summary>
        /// 启动web服务
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="lifetimeScope"></param>
        /// <returns></returns>
        Task StartAsync(string baseUrl, ILifetimeScope lifetimeScope);

        /// <summary>
        /// 停止
        /// </summary>
        /// <returns></returns>
        Task StopAsync();
    }
}
