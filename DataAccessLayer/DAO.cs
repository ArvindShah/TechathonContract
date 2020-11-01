using System;
using System.Collections.Generic;
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
        public List<ContentMaster> GetContentData(bool IsClause,int ContentId = 0)
        {
            var sqlHelper = GetDBConnection();
            var dataSet = sqlHelper.ExecuteDataSet(Constants.TECH_GETALLCONTENTDATA, IsClause, ContentId);
            var dt = dataSet.Tables[0];
            List<ContentMaster> contentList = new List<ContentMaster>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ContentMaster content = new ContentMaster();
                content.ContentId = Convert.ToInt32(dt.Rows[i]["ContentId"]);
                content.ContentName = dt.Rows[i]["ContentName"].ToString();
                content.ContentDescription = dt.Rows[i]["Description"].ToString();
                content.isClause = Convert.ToBoolean(dt.Rows[i]["Isclause"].ToString());
                content.IsEditable = Convert.ToBoolean(dt.Rows[i]["IsEditable"].ToString());
                content.Comment = dt.Rows[i]["Comment"].ToString();
                contentList.Add(content);
            }
            return contentList;
        }
        public int SaveUserTransaction(int id, int UserId, int Templateid, int LastVersion, int CurrentVersion, DateTime ModifiedDate)
        {
            var sqlHelper = GetDBConnection();
            var dataSet = sqlHelper.ExecuteDataSet(Constants.Tech_SaveUserTransaction, id,UserId, Templateid, LastVersion, CurrentVersion, ModifiedDate);

            return 0;
        }

    }
}
