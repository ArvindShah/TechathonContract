using System;
using Common;
using Common.Helper;

namespace DataAccessLayer
{
    public class DAO : SQLHelper, IDAO
    {
        public int SaveUser(int userId)
        {
            var sqlHelper = GetDBConnection();
            var dataSet = sqlHelper.ExecuteDataSet(Constants.PROC_SPEND_SSDL_Check_Valid_JobID, 7094);

            return 0;
        }
    }
}
