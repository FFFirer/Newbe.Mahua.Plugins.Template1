using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.IO;
using System.Configuration;
using Dapper;
using System.Data.SQLite;
using System.Diagnostics;

namespace Newbe.Mahua.Plugins.Template1.Services.Impl
{
    internal class SqliteDbHelper : IDbHelper
    {
        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <returns></returns>
        public Task InitDbAsync()
        {
            return Task.Run(() => CreateBdIfnotExists());
        }


        /// <summary>
        /// 创建数据库链接
        /// </summary>
        /// <returns></returns>
        public Task<DbConnection> CreateDbConnectionAsync()
        {
            return Task.Run(() => CreateDbConnectionCore());
        }

        private static DbConnection CreateDbConnectionCore()
        {
            var dbf = DbProviderFactories.GetFactory("System.Data.SQLite");
            var conn = dbf.CreateConnection();
            Debug.Assert(conn != null, nameof(conn) + "!= null");
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            return conn;
        }

        private static void CreateBdIfnotExists()
        {
            var dbDirectory = (string)AppDomain.CurrentDomain.GetData("DataDirectory");
            if (!Directory.Exists(dbDirectory)){
                Directory.CreateDirectory(dbDirectory);
            }
            var dbfile = Path.Combine(dbDirectory, "mydb.db");
            if (!File.Exists(dbfile))
            {
                SQLiteConnection.CreateFile(dbfile);
                using(var conn = CreateDbConnectionCore())
                {
                    conn.Execute(@"CREATE TABLE QUANS(
                        ID TEXT PRIMARY KEY,
                        QUNID TEXT NOT NULL,
                        INFO TEXT NOT NULL)");
                    conn.Execute(@"CREATE TABLE INVITES(
                        ID TEXT PRIMARY KEY,
                        QUNID TEXT NOT NULL,
                        INVITER TEXT,
                        JOINER TEXT)");
                }
            }
        }
    }
}
