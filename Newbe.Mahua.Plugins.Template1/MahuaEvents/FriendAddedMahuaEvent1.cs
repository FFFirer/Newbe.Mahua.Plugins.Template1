using Newbe.Mahua.MahuaEvents;
using System;
using System.Threading.Tasks;

namespace Newbe.Mahua.Plugins.Template1.MahuaEvents
{
    /// <summary>
    /// 已添加新好友事件
    /// </summary>
    public class FriendAddedMahuaEvent1
        : IFriendAddedMahuaEvent
    {
        private readonly IMahuaApi _mahuaApi;

        public FriendAddedMahuaEvent1(
            IMahuaApi mahuaApi)
        {
            _mahuaApi = mahuaApi;
        }

        public void ProcessFriendsAdded(FriendAddedMahuaEventContext context)
        {
            // todo 填充处理逻辑
            Task.Factory.StartNew(() =>
            {
                using (var robotSession = MahuaRobotManager.Instance.CreateSession())
                {
                    var api = robotSession.MahuaApi;
                    api.SendPrivateMessage(context.FromQq, "欢迎使用小盒子人工助理服务！输入\n 我想要+关键词 \n搜索优惠券\n或在浏览器打开以下链接寻找更多你喜欢的额内容");
                }
            });

            // 不要忘记在MahuaModule中注册
        }
    }
}
