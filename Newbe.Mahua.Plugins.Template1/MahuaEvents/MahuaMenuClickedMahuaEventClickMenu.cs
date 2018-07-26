using Newbe.Mahua.MahuaEvents;
using System;
using System.Diagnostics;

namespace Newbe.Mahua.Plugins.Template1.MahuaEvents
{
    /// <summary>
    /// 菜单点击事件
    /// </summary>
    public class MahuaMenuClickedMahuaEventClickMenu
        : IMahuaMenuClickedMahuaEvent
    {
        private readonly IMahuaApi _mahuaApi;

        public MahuaMenuClickedMahuaEventClickMenu(
            IMahuaApi mahuaApi)
        {
            _mahuaApi = mahuaApi;
        }

        public void ProcessManhuaMenuClicked(MahuaMenuClickedContext context)
        {
            // todo 填充处理逻辑
            if(context.Menu.Id == "menu1")
            {
                ShowNewbe();
            }
            

            // 不要忘记在MahuaModule中注册
        }

        public static void ShowNewbe()
        {
            Process.Start("http://www.newbe.pro");
        }
    }
}
