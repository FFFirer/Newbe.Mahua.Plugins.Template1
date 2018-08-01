using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newbe.Mahua.Plugins.Template1.Services
{
    public interface IFaQuanStorege
    {
        //获取发券详情
        Task<List<FaQuanInfo>> GetAllFaQuanInfoAsync();

        //获取发券详情（特定群）
        Task<List<FaQuanInfo>> GetFaQuanInfoAsync(string qun);

        //插入发券详情
        Task InsertFaQuanInfoAsync(FaQuanInfo info);

        //删除发券详情
        Task RemoveFaQuanInfoAsync(FaQuanInfo info);
        //删除一个群的发券信息
        Task RemoveFaQuanInfoOnQunAsync(string qunId);

        //插入邀请进群信息
        Task InsertInviteInfoAsync(InviteInfo Info);

        //查询邀请进群信息
        Task<List<InviteInfo>> GetInviteInfo(string qun);

        //查询今天第几页了
        Task<int> GetNowPagesize();
        //更新今天的页数
        Task UpdateNowPageSize(FaQuanJiShu js);
    }

    
    public class FaQuanInfo
    {
        //群号
        public string QunID { get; set; }
        //关键词信息
        public string Info { get; set; }
    }

    public class InviteInfo
    {
        //群号
        public string QunID { get; set; }
        //邀请人qq
        public string Inviter { get; set; }
        //进群者qq
        public string Joiner { get; set; }
    }

    public class FaQuanJiShu
    {
        public string Id { get; set; }
        public int PageNo { get; set; }
    }
}
