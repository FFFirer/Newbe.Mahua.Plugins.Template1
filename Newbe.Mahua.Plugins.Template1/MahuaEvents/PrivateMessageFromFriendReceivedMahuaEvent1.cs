using Newbe.Mahua.MahuaEvents;
using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Web;
using Newbe.Mahua.Plugins.Template1.RobotRemoteService;
using System.Net;
using Newbe.Mahua.Plugins.Template1.Services;
using System.Collections.Generic;

namespace Newbe.Mahua.Plugins.Template1.MahuaEvents
{
    /// <summary>
    /// 来自好友的私聊消息接收事件
    /// </summary>
    public class PrivateMessageFromFriendReceivedMahuaEvent1
        : IPrivateMessageFromFriendReceivedMahuaEvent
    {
        private readonly IMahuaApi _mahuaApi;   //我也不知道为什么要加这个 2018年7月28日22点28分
        private readonly IPublishQuan _publishquan;
        private readonly IAdminControl _admincontrol;
        private List<String> AdminList;

        public PrivateMessageFromFriendReceivedMahuaEvent1(IMahuaApi mahuaApi, IPublishQuan publishQuan, IAdminControl adminControl)
        {
            _mahuaApi = mahuaApi;
            //_publishquan = publishQuan;
            _admincontrol = adminControl;
            AdminList.Add("609936294");
            AdminList.Add("2432880190");
            AdminList.Add("2653254193");
        }

        public void ProcessFriendMessage(PrivateMessageFromFriendReceivedContext context)
        {
            //判断发送的QQ是否来自管理员
            if(_admincontrol.IsAdmin(context.FromQq, context.Message))
            {
                string CallbackMes = _admincontrol.DoAdmin(context.Message);
                _mahuaApi.SendPrivateMessage(context.FromQq, CallbackMes);
            }
            //if(AdminList.Contains(context.FromQq))
            //{
            //    switch (context.Message)
            //    {
            //        case "开始发券":
            //            _publishquan.StartAsync().GetAwaiter().GetResult();
            //            _mahuaApi.SendPrivateMessage(context.FromQq, "已开启发券");
            //            break;
            //        case "停止发券":
            //            _publishquan.StartAsync().GetAwaiter().GetResult();
            //            _mahuaApi.SendPrivateMessage(context.FromQq, "已停止发券");
            //            break;
            //        default:
            //            //非机器人管理指令，正常处理
            //            GiveQuan(context);
            //            break;
            //    }
            //}
            else
            {
                GiveQuan(context);
            }

            // 不要忘记在MahuaModule中注册
        }

        public static void GiveQuan(PrivateMessageFromFriendReceivedContext context)
        {
            Task.Factory.StartNew(() =>
            {
                RobotRemoteService.RobotRemoteService RrService = new RobotRemoteService.RobotRemoteService();
                using (var robotSession = MahuaRobotManager.Instance.CreateSession())
                {
                    string ReturnMessage = string.Empty;
                    var api = robotSession.MahuaApi;
                    string Message = context.Message;
                    string keyword = GetKeyWord(Message);
                    string resultUrl = @"http://52lequan.cn/index.php?r=l&kw=" + System.Web.HttpUtility.UrlEncode(keyword, System.Text.Encoding.UTF8);
                    if (keyword != "NoKey")
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
        /// 将网络图片下载，并保存在本地路径，将网络地址替换为本地相对路径
        /// </summary>
        /// <param name="input"></param>
        public static string TransferImage(string input)
        {
            string imgPattern = @"file=(.*?)]";
            Match M_Img = Regex.Match(input, imgPattern);
            string url = M_Img.Groups[1].Value;
            string img_name = System.IO.Path.GetFileName(url);
            string save_path = Environment.CurrentDirectory + @"\data\image\";
            //图片下载
            DownloadImage(url, save_path + img_name);
            string output = input.Replace(url, img_name);
            return output;
        }
        /// <summary>
        /// 从网络下载图片
        /// </summary>
        /// <param name="weburl"></param>
        public static void DownloadImage(string weburl, string image_name)
        {
            Uri download_url = new Uri(weburl);
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(download_url, image_name);
            }
        }
        /// <summary>
        /// 提取关键词
        /// </summary>
        /// <param name="message"></param>
        /// <returns>"NoKey"</returns>
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
