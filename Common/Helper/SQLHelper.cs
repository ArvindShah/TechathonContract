using GEP.SMART.Storage.AzureSQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Helper
{
    public class SQLHelper
    {
        protected ReliableSqlDatabase GetDBConnection()
        {
            try
            {
                string dbName = "Dev_OGE";
                string connStr = "Data Source=hx67tx2ygu.database.windows.net,1433;Initial Catalog=" + dbName + ";User Id=gep_sql_admin;Password=Password@123;MultipleActiveResultSets=True;ConnectRetryCount=3;ConnectRetryInterval=10;Connection Timeout=30;";
                return new ReliableSqlDatabase(connStr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
