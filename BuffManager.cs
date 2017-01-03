using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using TShockAPI;
using TShockAPI.DB;

namespace Koishi.BanBuff
{
    public class BuffManager
    {
        private IDbConnection database;

        public List<BuffBan> BuffBans = new List<BuffBan>();

        public BuffManager(IDbConnection db)
        {
            IQueryBuilder sqliteQueryCreator;
            this.database = db;
            SqlColumn[] sqlColumn = new SqlColumn[2];
            SqlColumn sqlColumn1 = new SqlColumn("BuffID", MySqlDbType.Int32, new int?(50));
            sqlColumn1.Primary = true;
            sqlColumn[0] = sqlColumn1;
            sqlColumn[1] = new SqlColumn("AllowedGroups", MySqlDbType.Text);
            SqlTable sqlTable = new SqlTable("BuffBans", sqlColumn);
            IDbConnection dbConnection = db;
            if (DbExt.GetSqlType(db) == SqlType.Sqlite)
            {
                sqliteQueryCreator = new SqliteQueryCreator();
            }
            else
            {
                sqliteQueryCreator = new MysqlQueryCreator();
            }
            (new SqlTableCreator(dbConnection, sqliteQueryCreator)).EnsureTableStructure(sqlTable);
            this.UpdateBuffBans();
        }

        public void AddNewBan(int BuffID = 0)
        {
            try
            {
                IDbConnection dbConnection = this.database;
                object[] buffID = new object[] { BuffID, "" };
                DbExt.Query(dbConnection, "INSERT INTO BuffBans (BuffID, AllowedGroups) VALUES (@0, @1);", buffID);
                if (!this.BuffIsBanned(BuffID, null))
                {
                    this.BuffBans.Add(new BuffBan(BuffID));
                }
            }
            catch (Exception exception)
            {
                TShock.Log.Error(exception.ToString());
            }
        }

        public bool AllowGroup(int id, string name)
        {
            bool flag;
            string str = "";
            BuffBan buffBanById = Koishi.BanBuff.Utils.GetBuffBanById(id);
            if (buffBanById != null)
            {
                try
                {
                    str = string.Join(",", buffBanById.AllowedGroups);
                    if (str.Length > 0)
                    {
                        str = string.Concat(str, ",");
                    }
                    str = string.Concat(str, name);
                    buffBanById.SetAllowedGroups(str);
                    IDbConnection dbConnection = this.database;
                    object[] objArray = new object[] { str, id };
                    flag = DbExt.Query(dbConnection, "UPDATE BuffBans SET AllowedGroups=@0 WHERE BuffID=@1", objArray) > 0;
                }
                catch (Exception exception)
                {
                    TShock.Log.Error(exception.ToString());
                    return false;
                }
                return flag;
            }
            return false;
        }

        public bool BuffIsBanned(int id)
        {
            if (this.BuffBans.Contains(new BuffBan(id)))
            {
                return true;
            }
            return false;
        }

        public bool BuffIsBanned(int id, TSPlayer ply)
        {
            BuffBan buffBanById = Koishi.BanBuff.Utils.GetBuffBanById(id);
            if (buffBanById == null)
            {
                return false;
            }
            return !buffBanById.HasPermissionToUseBuff(ply);
        }

        public void RemoveBan(int BuffID)
        {
            if (!this.BuffIsBanned(BuffID, null))
            {
                return;
            }
            try
            {
                IDbConnection dbConnection = this.database;
                object[] buffID = new object[] { BuffID };
                DbExt.Query(dbConnection, "DELETE FROM BuffBans WHERE BuffID=@0;", buffID);
                this.BuffBans.Remove(new BuffBan(BuffID));
            }
            catch (Exception exception)
            {
                TShock.Log.Error(exception.ToString());
            }
        }

        public bool RemoveGroup(int id, string group)
        {
            bool flag;
            BuffBan buffBanById = Koishi.BanBuff.Utils.GetBuffBanById(id);
            if (buffBanById != null)
            {
                try
                {
                    buffBanById.RemoveGroup(group);
                    string str = string.Join(",", buffBanById.AllowedGroups);
                    IDbConnection dbConnection = this.database;
                    object[] objArray = new object[] { str, id };
                    if (DbExt.Query(dbConnection, "UPDATE BuffBans SET AllowedGroups=@0 WHERE BuffID=@1", objArray) <= 0)
                    {
                        return false;
                    }
                    else
                    {
                        flag = true;
                    }
                }
                catch (Exception exception)
                {
                    TShock.Log.Error(exception.ToString());
                    return false;
                }
                return flag;
            }
            return false;
        }

        public void UpdateBuffBans()
        {
            this.BuffBans.Clear();
            using (QueryResult queryResult = DbExt.QueryReader(this.database, "SELECT * FROM BuffBans", new object[0]))
            {
                while (queryResult != null && queryResult.Read())
                {
                    BuffBan buffBan = new BuffBan(queryResult.Get<int>("BuffID"));
                    buffBan.SetAllowedGroups(queryResult.Get<string>("AllowedGroups"));
                    this.BuffBans.Add(buffBan);
                }
            }
        }
    }
}