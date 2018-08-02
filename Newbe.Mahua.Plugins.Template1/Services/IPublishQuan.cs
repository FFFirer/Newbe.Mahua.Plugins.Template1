using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newbe.Mahua.Plugins.Template1.Services
{
    public interface IPublishQuan
    {
        /// <summary>
        /// 启动
        /// </summary>
        /// <returns></returns>
        Task StartAsync();

        /// <summary>
        /// 停止
        /// </summary>
        /// <returns></returns>
        Task StopAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task FaOnceNow();
    }
}
