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
                RobotRemoteService.RobotRemoteService RrService = new RobotRemoteService.RobotRemoteService();
                using (var robotSession = MahuaRobotManager.Instance.CreateSession())
                {
                    string ReturnMessage = string.Empty;
                    var api = robotSession.MahuaApi;
                    string Message = context.Message;
                    ReturnMessage = RrService.GetQuan(Message);
                    if(Regex.IsMatch(ReturnMessage, @"^error"))
                    {
                        api.SendPrivateMessage("609936294", ReturnMessage + "\n" + "原始消息：" + Message);
                        api.SendPrivateMessage(context.FromQq, "出了一点小故障，请再试一次");
                    }
                    else if(Regex.IsMatch(ReturnMessage, @"file=.*?]"))
                    {
                        //ReturnMessage = TransferImage(ReturnMessage);
                        //api.SendPrivateMessage(context.FromQq, ReturnMessage);
                        api.SendPrivateMessage(context.FromQq, "【小学数学公式定律手册 彩色版 教材全解应用题思路点拨一二三四五六年级上下册思维训练教辅知识大全 1-3-6小学数学公式单位换算书】\n[CQ:image,file=D:\\CQP-xiaoi\\酷Q Pro\\data\\image\\TB2HiWDXzfguuRjSszcXXbb7FXa_!!2982001514-0-item_pic.jpg]\n现价：12.90\n券后价：7.9\n【下单链接】\nhttps://uland.taobao.com/coupon/edetail?e=f7SVlr4inEwGQASttHIRqaDrIGMQgR%2FaIWya0l4DLrTyi1aSyqbpmoc4cdtwQZqclyl4SqF4eKkeg%2BiFujJjkC3kw205C36xDfqEFBOhTcxR0dc5vyZ8pFzX6p6qRdAPIJsfjzznD0qwkuPSKAb8M%2B1LRo38GBz3ugJmHzuXNrct%2FzOJQMDvl0gfTscre%2FnACJMVxbzoieAJlVWeIP9CzA%3D%3D&traceId=0bb6493215326093881857422e\n——————————\n复制这条信息，￥0Ku2b0Bsgeo￥,打开【手机淘宝】即可查看\n更多优惠请点击http://52lequan.cn/index.php?r=l&kw=%e6%95%99%e8%be%85");
                    }
                    else
                    {
                        api.SendPrivateMessage(context.FromQq, ReturnMessage);
                    }
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
            string save_path = Environment.CurrentDirectory + @"\data\image\\";
            //图片下载
            DownloadImage(url, save_path + img_name);
            string output = input.Replace(url, @"D:\CQP-xiaoi\酷Q Pro\data\image\" + img_name);
            return output;
        }
        /// <summary>
        /// 从网络下载图片
        /// </summary>
        /// <param name="weburl"></param>
        public static void DownloadImage(string weburl, string image_name)
        {
            Uri download_url = new Uri(weburl);
            using(WebClient client = new WebClient())
            {
                client.DownloadFile(download_url, image_name);
            }
        }
    }
}
