using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace Newbe.Mahua.Plugins.Template1.Services
{
    public interface IDbHelper
    {
        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <returns></returns>
        Task InitDbAsync();

        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <returns></returns>
        Task<DbConnection> CreateDbConnectionAsync();
    }
}
