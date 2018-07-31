using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newbe.Mahua.Plugins.Template1.Services
{
    public interface IAdminControl
    {
        //判断是否是管理员的管理命令
        bool IsAdmin(string QQID,string message);

        //进行管理员的操作
        string Admin2do(string message);

    }
}
