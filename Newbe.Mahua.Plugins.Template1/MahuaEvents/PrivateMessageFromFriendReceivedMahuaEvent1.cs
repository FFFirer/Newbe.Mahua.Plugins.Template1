using Newbe.Mahua.MahuaEvents;
using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Web;
using Newbe.Mahua.Plugins.Template1.RobotRemoteService;
using System.Net;

namespace Newbe.Mahua.Plugins.Template1.MahuaEvents
{
    /// <summary>
    /// 来自好友的私聊消息接收事件
    /// </summary>
    public class PrivateMessageFromFriendReceivedMahuaEvent1
        : IPrivateMessageFromFriendReceivedMahuaEvent
    {
        private readonly IMahuaApi _mahuaApi;

        public PrivateMessageFromFriendReceivedMahuaEvent1(
            IMahuaApi mahuaApi)
        {
            _mahuaApi = mahuaApi;
        }

        public void ProcessFriendMessage(PrivateMessageFromFriendReceivedContext context)
        {

            //异步发送消息
            Task.Factory.StartNew(() =>
            {
                RobotRemoteService.RobotRemoteService RrService = new RobotRemoteService.RobotRemoteService();
                using (var robotSession = MahuaRobotManager.Instance.CreateSession())
                {
                    string ReturnMessage = string.Empty;
                    var api = robotSession.MahuaApi;
                    string Message = context.Message;
                    ReturnMessage = RrService.GetQuan(Message);
                    if (Regex.IsMatch(ReturnMessage, @"^error"))
                    {
                        api.SendPrivateMessage("609936294", ReturnMessage + "\n" + "原始消息：" + Message);
                        api.SendPrivateMessage(context.FromQq, "出了一点小故障，请再试一次");
                    }
                    else if (Regex.IsMatch(ReturnMessage, @"file=.*?]"))
                    {
                        ReturnMessage = TransferImage(ReturnMessage);
                        api.SendPrivateMessage(context.FromQq, ReturnMessage);
                    }
                    else
                    {
                        api.SendPrivateMessage(context.FromQq, ReturnMessage);
                    }
                }
            });

            // 不要忘记在MahuaModule中注册
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
            string output = input.Replace(url, save_path + img_name);
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
    }
}
