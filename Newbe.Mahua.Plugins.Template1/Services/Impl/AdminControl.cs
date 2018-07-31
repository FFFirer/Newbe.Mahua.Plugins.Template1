using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Newbe.Mahua.Plugins.Template1.Services.Impl
{
    public class AdminControl : IAdminControl
    {
        private static List<string> AdminList = new List<string>
        {
            "609936294",
            "2432880190",
            "2653254193",
        };

        string FaSomeQuan = @"/群([0-9]*?)发(.*?)券";
        string NotFaSomeQuan = @"/群([0-9]*?)不发(.*?)券";
        string FaWhatQuan = @"/群([0-9]*?)发的什么券";
        string FaAllQuan = @"/群([0-9]*?)发全品类券";
        string StartFaQuan = @"/开始发券";
        string StopFaQuan = @"/停止发券";
        string SomeoneInviteWho = @"/查看([0-9]*?)邀请信息";
        private static List<string> CommandList = new List<string>
        {
            @"/群([0-9]*?)发(.*?)券",
            @"/群([0-9]*?)不发(.*?)券",
            @"/群([0-9]*?)发的什么券",
            @"/群([0-9]*?)发全品类券",
            @"/开始发券",
            @"/停止发券",
            @"/查看([0-9]*?)邀请信息",
        };

        public bool IsAdmin(string QQID, string Message)
        {
            if (AdminList.Contains(QQID))
            {
                foreach(string command in CommandList)
                {
                    if (Regex.IsMatch(Message, command))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }


        public string Admin2do(string Message)
        {
            //获取命令参数，执行的方法，然后分别调用不同的方法
            
        }
    }
}
