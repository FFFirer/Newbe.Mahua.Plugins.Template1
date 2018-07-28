using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using System.Diagnostics;

namespace Newbe.Mahua.Plugins.Template1.Services.Impl
{
    public class PublishQuan : IPublishQuan
    {
        //每天零点触发
        private static readonly string JobId = "jobid";

        private readonly IMahuaApi _mahuaApi;

        public PublishQuan(IMahuaApi mahuaApi)
        {
            _mahuaApi = mahuaApi;
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
            SendMeesage("定时任务：间隔五分钟触发");
            BackgroundJob.Schedule(() => SendMeesage("延迟任务：延迟一分钟触发"), TimeSpan.FromMinutes(1));
            BackgroundJob.Schedule(() => SendMeesage("延迟任务：延迟两分钟触发"), TimeSpan.FromMinutes(2));
        }
    }
}
