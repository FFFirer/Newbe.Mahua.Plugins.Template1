using Newbe.Mahua.MahuaEvents;
using System;
using System.Threading.Tasks;
using Newbe.Mahua.Plugins.Template1.RobotRemoteService;

namespace Newbe.Mahua.Plugins.Template1.MahuaEvents
{
    /// <summary>
    /// 好友申请接受事件
    /// </summary>
    public class FriendAddingRequestMahuaEvent1
        : IFriendAddingRequestMahuaEvent
    {
        private readonly IMahuaApi _mahuaApi;

        public FriendAddingRequestMahuaEvent1(
            IMahuaApi mahuaApi)
        {
            _mahuaApi = mahuaApi;
        }

        public void ProcessAddingFriendRequest(FriendAddingRequestContext context)
        {
            // todo 填充处理逻辑
            //_mahuaApi.AcceptFriendAddingRequest(context.AddingFriendRequestId, context.FromQq, string.Empty);
            //RobotRemoteService rrService = new RobotRemoteService();
            Task.Factory.StartNew(() =>
            {
                using (var robotSession = MahuaRobotManager.Instance.CreateSession())
                {
                    //string response = rrService.HelloWorld();
                    var api = robotSession.MahuaApi;
                    api.SendPrivateMessage(context.FromQq, "已收到请求，在审核中\n"+context.AddingFriendRequestId+"\n"+context.FromQq);
                    api.AcceptFriendAddingRequest(context.AddingFriendRequestId, context.FromQq, context.Message);
                    api.SendPrivateMessage(context.FromQq, "已接受");

                }
            });
            // 不要忘记在MahuaModule中注册
        }
    }
}
