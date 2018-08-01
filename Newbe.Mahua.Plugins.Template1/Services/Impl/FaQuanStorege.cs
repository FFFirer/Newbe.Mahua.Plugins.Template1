using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Newbe.Mahua.Plugins.Template1.Services.Impl
{
    public class FaQuanStorege : IFaQuanStorege
    {
        private readonly IDbHelper _dbHelper;
        public FaQuanStorege(IDbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }


        /// <summary>
        /// 获取发券详情
        /// </summary>
        /// <returns></returns>
        public async Task<List<FaQuanInfo>> GetAllFaQuanInfoAsync()
        {
            using (var conn = await _dbHelper.CreateDbConnectionAsync())
            {
                var QReader = await conn.ExecuteReaderAsync("select * from QUANS");
                List<FaQuanInfo> faQuanInfos = new List<FaQuanInfo>();
                while (QReader.Read())
                {
                    faQuanInfos.Add(new FaQuanInfo
                    {
                        QunID = QReader["QUNID"].ToString(),
                        Info = QReader["INFO"].ToString(),
                    });
                }
                return faQuanInfos;
            }
        }

        /// <summary>
        /// 获取特定群的发券信息
        /// </summary>
        /// <param name="qun"></param>
        /// <returns></returns>
        public async Task<List<FaQuanInfo>> GetFaQuanInfoAsync(string qun)
        {
            using(var conn = await _dbHelper.CreateDbConnectionAsync())
            {
                var QReader = await conn.ExecuteReaderAsync("select * from QUANS where QUNID='" + qun +"'");
                List<FaQuanInfo> faQuanInfos = new List<FaQuanInfo>();
                while (QReader.Read())
                {
                    faQuanInfos.Add(new FaQuanInfo
                    {
                        QunID = QReader["QUNID"].ToString(),
                        Info = QReader["INFO"].ToString(),
                    });
                }
                return faQuanInfos;
            }
        }

        /// <summary>
        /// 插入发券信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task InsertFaQuanInfoAsync(FaQuanInfo info)
        {
            using(var conn = await _dbHelper.CreateDbConnectionAsync())
            {
                await conn.InsertAsync(new InfoEntity
                {
                    id = Guid.NewGuid().ToString(),
                    Qunid = info.QunID,
                    Info = info.Info
                });
            }
        }

        /// <summary>
        /// 删除发券消息（特定品类）
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task RemoveFaQuanInfoAsync(FaQuanInfo info)
        {
            using(var conn = await _dbHelper.CreateDbConnectionAsync())
            {
                await conn.ExecuteAsync("delete from QUANS where QUNID='" + info.QunID + "' and INFO='" + info.Info + "'");
            }
        }

        /// <summary>
        /// 删除发券信息（指定群）
        /// </summary>
        /// <param name="qunId"></param>
        /// <returns></returns>
        public async Task RemoveFaQuanInfoOnQunAsync(string qunId)
        {
            using(var conn = await _dbHelper.CreateDbConnectionAsync())
            {
                await conn.ExecuteAsync("delete from QUANS where QUNID='" + qunId + "'");
            }
        }

        //插入邀请进群信息
        public async Task InsertInviteInfoAsync(InviteInfo Info)
        {
            using (var conn = await _dbHelper.CreateDbConnectionAsync())
            {
                await conn.InsertAsync(new InviteEntity
                {
                    id = Guid.NewGuid().ToString(),
                    QunId = Info.QunID,
                    Inviter = Info.Inviter,
                    Joiner = Info.Joiner
                });
            }
        }

        //查询邀请进群信息
        public async Task<List<InviteInfo>> GetInviteInfo(string qun)
        {
            using (var conn = await _dbHelper.CreateDbConnectionAsync())
            {
                List<InviteInfo> invites = new List<InviteInfo>();
                var Invites = await conn.ExecuteReaderAsync("select * from INVITES where QUNID='" + qun + "'");
                while (Invites.Read())
                {
                    invites.Add(new InviteInfo
                    {
                        QunID = Invites["QUNID"].ToString(),
                        Inviter = Invites["INVITER"].ToString(),
                        Joiner = Invites["JOINER"].ToString()
                    });
                }
                return invites;
            }
        }

        //查询今天第几页了
        public async Task<int> GetNowPagesize()
        {
            using(var conn = await _dbHelper.CreateDbConnectionAsync())
            {
                var res = await conn.QueryAsync("select PAGENO from JISHU where ID='PageNo'");
                int r = res.FirstOrDefault();
                return r;
            }
        }
        //更新今天的页数
        public async Task UpdateNowPageSize(FaQuanJiShu jishu)
        {
            using (var conn = await _dbHelper.CreateDbConnectionAsync())
            {
                await conn.UpdateAsync(new JiShuEntity { id=jishu.Id, pageno=jishu.PageNo});
            }
        }

    }

    [Table("QUANS")]
    public class InfoEntity
    {
        [Key] public string id { get; set; }
        public string Qunid { get; set; }
        public string Info { get; set; }
    }

    [Table("INVITES")]
    public class InviteEntity
    {
        [Key] public string id { get; set; }
        public string QunId { get; set; }
        public string Inviter { get; set; }
        public string Joiner { get; set; }
    }

    [Table("JISHU")]
    public class JiShuEntity
    {
        [Key] public string id { get; set; }
        public int pageno { get; set; }
    }
}
