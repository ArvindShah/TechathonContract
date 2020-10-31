using GEP.SMART.Storage.AzureSQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Helper
{
    public class SQLHelper
    {
        protected ReliableSqlDatabase GetDBConnection(int type)
        {
            try
            {
                string connStr = string.Empty;
                if (type == 1)
                {
                    string dbName = "dev_cst_Report_DW";
                    connStr = "Data Source=hx67tx2ygu.database.windows.net,1433;Initial Catalog=" + dbName + ";User Id=gep_sql_admin;Password=Password@123;MultipleActiveResultSets=True;ConnectRetryCount=3;ConnectRetryInterval=10;Connection Timeout=30;";
                }
                else if(type == 2)
                {
                    connStr = "Data Source=tcp:smartgepdev.database.windows.net,1433;Initial Catalog=DEVSmartConfiguration;User ID=Dev_Config_User;Password=DcU$@123;MultipleActiveResultSets=True;Trusted_Connection=False;Encrypt=True;ConnectRetryCount=3;ConnectRetryInterval=10;Connection Timeout=30;";
                }
                else
                {
                    connStr = "Data Source=tcp:smartgepdev.database.windows.net,1433;Initial Catalog=Dev_SidleyAustin;User ID=DEV_Smart_User;Password=D7xvxRq9;MultipleActiveResultSets=True;Trusted_Connection=False;Encrypt=True;ConnectRetryCount=3;ConnectRetryInterval=10;Connection Timeout=30;";

                }
                return new ReliableSqlDatabase(connStr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
