using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using System.Diagnostics;
using CronExpressionDescriptor;
using Newbe.Mahua.Plugins.Template1.RobotRemoteService;
using System.Text.RegularExpressions;
using System.Net;

namespace Newbe.Mahua.Plugins.Template1.Services.Impl
{
    public class PublishQuan : IPublishQuan
    {
        //每天零点触发
        private static readonly string JobId = "jobid";
        private static readonly List<string> TimeList = new List<string> { "7", "9", "12", "18", "20" };
        private readonly IMahuaApi _mahuaApi;
        private readonly IFaQuanStorege _faquanstorege;

        public PublishQuan(IMahuaApi mahuaApi, IFaQuanStorege faQuanStorege)
        {
            _mahuaApi = mahuaApi;
            _faquanstorege = faQuanStorege;
        }

        public Task StartAsync()
        {
            //添加定时任务
            //每隔一段时间触发
            RecurringJob.AddOrUpdate(JobId, () => Tasks2Do(), () => Cron.MinuteInterval(5));

            //使用浏览器打开定时任务的网址
            Process.Start("http://localHost:65238/hangfire/recurring");
            return Task.FromResult(0);
        }

        public Task StopAsync()
        {
            //移除定时任务
            RecurringJob.RemoveIfExists(JobId);
            return Task.FromResult(0);
        }

        public void SendMeesage(string message)
        {
            //调用发送群消息的方法
            string NowStamp = DateTime.UtcNow.ToLongTimeString();
            _mahuaApi.SendGroupMessage("826427383", NowStamp + "\n" + message);

        }
        public void Tasks2Do()
        {
            //获取当前的时间
            string Hour = DateTime.Now.Hour.ToString();
            //检查是否要开始发券
            if (!TimeList.Contains(Hour))
            {
                int PageNo = _faquanstorege.GetNowPagesize().GetAwaiter().GetResult();
                RobotRemoteService.RobotRemoteService service = new RobotRemoteService.RobotRemoteService();
                //获取每个群的券关键词
                List<FaQuanInfo> all_info = _faquanstorege.GetAllFaQuanInfoAsync().GetAwaiter().GetResult();
                List<string> quns = all_info.Select(x => x.QunID).Distinct().ToList();
                foreach(string id in quns)
                {
                    Task.Factory.StartNew(() =>
                    {
                        List<string> result = new List<string>();
                        List<string> keys = all_info.Where(p => p.QunID.Equals(id)).Select(x => x.Info).ToList();
                        foreach (string key in keys)
                        {
                            List<string> res = new List<string>();
                            if (key.Equals("全品类"))
                            {
                                res = service.PostQuans("", PageNo).ToList();
                            }
                            else
                            {
                                res = service.PostQuans(key, PageNo).ToList();
                            }
                            //判断页数的增减
                            if (Hour.Equals("24"))
                            {
                                PageNo = 1;
                            }
                            else
                            {
                                PageNo++;
                            }
                            _faquanstorege.UpdateNowPageSize(new FaQuanJiShu { Id = "PageNo", PageNo = PageNo}).GetAwaiter().GetResult();
                            result.AddRange(res);
                        }
                        List<string> r = result.OrderBy(p => p.Length).Take(15).ToList();
                        //异步发送券信息
                        using(var robotSession = MahuaRobotManager.Instance.CreateSession())
                        {
                            var api = robotSession.MahuaApi;
                            foreach (var l in r)
                            {
                                string o = TransferImage(l);
                                api.SendGroupMessage(id, o);
                            }
                        }
                    });
                }
            }

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
    }
}
