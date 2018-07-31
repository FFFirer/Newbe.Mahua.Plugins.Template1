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
        private readonly IFaQuanStorege _faQuanStorege;
        private readonly IPublishQuan _publishQuan;

        public AdminControl(IFaQuanStorege faQuanStorege,
            IPublishQuan publishQuan)
        {
            _faQuanStorege = faQuanStorege;
            _publishQuan = publishQuan;
        }
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
            @"/查看群([0-9]*?)邀请信息",
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
            //发特定种类券
            if(Regex.IsMatch(Message, FaSomeQuan))
            {
                Match match = Regex.Match(Message, FaSomeQuan);
                string QqQun = match.Groups[1].Value;
                string Q = match.Groups[2].Value;
                _faQuanStorege.InsertFaQuanInfoAsync(new FaQuanInfo
                {
                    QunID = QqQun,
                    Info = Q
                }).GetAwaiter().GetResult();
                return "操作成功";
            }
            //不发特定种类券
            if(Regex.IsMatch(Message, NotFaSomeQuan))
            {
                Match match = Regex.Match(Message, NotFaSomeQuan);
                string QqQun = match.Groups[1].Value;
                string Q = match.Groups[2].Value;
                _faQuanStorege.RemoveFaQuanInfoAsync(new FaQuanInfo
                {
                    QunID = QqQun,
                    Info = Q
                }).GetAwaiter().GetResult();
                return "操作成功";
            }
            //发了什么券
            if(Regex.IsMatch(Message, FaWhatQuan))
            {
                Match match = Regex.Match(Message, FaWhatQuan);
                string QqQun = match.Groups[1].Value;
                List<FaQuanInfo> infos = _faQuanStorege.GetFaQuanInfoAsync(QqQun).GetAwaiter().GetResult();
                string message = string.Empty;
                message += string.Format("群{0}发券种类：\n", QqQun);
                foreach(var i in infos)
                {
                    message += string.Format("{0}  ", i.Info);
                }
                return message;
            }
            //发全品类券
            if(Regex.IsMatch(Message, FaAllQuan))
            {
                Match match = Regex.Match(Message, FaWhatQuan);
                string QqQun = match.Groups[1].Value;
                _faQuanStorege.InsertFaQuanInfoAsync(new FaQuanInfo
                {
                    QunID = QqQun,
                    Info = "all"
                }).GetAwaiter().GetResult();

                return "操作成功";
            }
            //开始发券
            if(Regex.IsMatch(Message, StartFaQuan))
            {
                _publishQuan.StartAsync().GetAwaiter().GetResult();
                return "START";
            }
            //停止发券
            if(Regex.IsMatch(Message, StopFaQuan))
            {
                _publishQuan.StopAsync().GetAwaiter().GetResult();
                return "STOP";
            }
            //谁邀请了谁
            if(Regex.IsMatch(Message, SomeoneInviteWho))
            {
                Match match = Regex.Match(Message, FaWhatQuan);
                string QqQun = match.Groups[1].Value;
                List<InviteInfo> invites = _faQuanStorege.GetInviteInfo(QqQun).GetAwaiter().GetResult();
                List<string> QQs = invites.Select(p => p.Inviter).Distinct().ToList();
                string returnMessage = string.Empty;
                foreach (string qq in QQs)
                {
                    int Count = invites.Where(p => p.Inviter.Equals(qq)).Count();
                    returnMessage += string.Format("{0}【{1}】", qq, Count);
                }
                return returnMessage;
            }
        }
    }
}
