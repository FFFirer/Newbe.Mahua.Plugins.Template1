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


        //获取发券详情
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

        //删除发券详情
        public async Task RemoveFaQuanInfoAsync(FaQuanInfo info)
        {
            using(var conn = await _dbHelper.CreateDbConnectionAsync())
            {
                await conn.ExecuteAsync("delete from QUANS where QUNID='" + info.QunID + "' and INFO='" + info.Info + "'");
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
                var Invites = await conn.ExecuteReaderAsync("select * from INVITES where='" + qun + "'");
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
}
