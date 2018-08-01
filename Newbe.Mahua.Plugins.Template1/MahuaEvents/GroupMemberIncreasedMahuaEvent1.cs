using Newbe.Mahua.MahuaEvents;
using System;
using Newbe.Mahua.Plugins.Template1.Services;

namespace Newbe.Mahua.Plugins.Template1.MahuaEvents
{
    /// <summary>
    /// 群成员增多事件
    /// </summary>
    public class GroupMemberIncreasedMahuaEvent1
        : IGroupMemberIncreasedMahuaEvent
    {
        private readonly IMahuaApi _mahuaApi;
        private readonly IFaQuanStorege _faQuanStorege;

        public GroupMemberIncreasedMahuaEvent1(
            IMahuaApi mahuaApi,
            IFaQuanStorege faQuanStorege)
        {
            _mahuaApi = mahuaApi;
            _faQuanStorege = faQuanStorege;
        }

        public void ProcessGroupMemberIncreased(GroupMemberIncreasedContext context)
        {
            // todo 填充处理逻辑
            _faQuanStorege.InsertInviteInfoAsync(new InviteInfo
            {
                QunID = context.FromGroup,
                Inviter = context.InvitatorOrAdmin,
                Joiner = context.JoinedQq
            }).GetAwaiter().GetResult();
        }
    }
}
