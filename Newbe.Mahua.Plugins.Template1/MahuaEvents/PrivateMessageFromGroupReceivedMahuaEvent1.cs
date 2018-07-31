using Newbe.Mahua.MahuaEvents;
using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Web;
using Newbe.Mahua.Plugins.Template1.RobotRemoteService;
using System.Net;
using Newbe.Mahua.Plugins.Template1.Services;

namespace Newbe.Mahua.Plugins.Template1.MahuaEvents
{
    /// <summary>
    /// 来自群成员的私聊消息接收事件
    /// </summary>
    public class PrivateMessageFromGroupReceivedMahuaEvent1
        : IPrivateMessageFromGroupReceivedMahuaEvent
    {
        private readonly IMahuaApi _mahuaApi;

        public PrivateMessageFromGroupReceivedMahuaEvent1(
            IMahuaApi mahuaApi)
        {
            _mahuaApi = mahuaApi;
        }

        public void ProcessGroupMessage(PrivateMessageFromGroupReceivedContext context)
        {
            //异步发送消息
            Task.Factory.StartNew(() =>
            {
                using (var robotSession = MahuaRobotManager.Instance.CreateSession())
                {
                    string ReturnMessage = string.Empty;
                    var api = robotSession.MahuaApi;
                    string Message = context.Message;
                    string keyword = GetKeyWord(Message);
                    string resultUrl = @"http://52lequan.cn/index.php?r=l&kw=" + System.Web.HttpUtility.UrlEncode(keyword, System.Text.Encoding.UTF8);
                    if(keyword != "NoKey")
                    {
                        ReturnMessage = string.Format("关键词：{0}\n链接：{1}\n复制链接在浏览器中打开\n如果没有你想要的结果，点击右侧搜索全网", keyword, resultUrl);
                    }
                    else
                    {
                        ReturnMessage = "没有识别出您想要的商品，请再输入\n我想要+商品名称\n进行搜索，如需人工服务，请联系所在群管理员";
                    }
                    api.SendPrivateMessage(context.FromQq, ReturnMessage);
                }
            });
        }

        
        /// <summary>
        /// 提取关键词
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string GetKeyWord(string RawString)
        {
            string KeyPattern = @"([我有].*?[有要买])([\w]+)[\W]?";
            if (Regex.IsMatch(RawString, KeyPattern))
            {
                Match match = Regex.Match(RawString, KeyPattern);
                string keyword = match.Groups[2].Value;
                return keyword;
            }
            else
            {
                return "NoKey";
            }
        }
    }
}
