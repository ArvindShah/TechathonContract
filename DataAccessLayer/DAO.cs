using System;
using System.Collections.Generic;

using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Common;
using Common.Entity;
using Common.Helper;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace DataAccessLayer
{
    public class DAO : SQLHelper, IDAO
    {
        public int SaveUser(int userId)
        {
            var sqlHelper = GetDBConnection(1);
            var ds = sqlHelper.ExecuteDataSet(Constants.PROC_SPEND_SSDL_Check_Valid_JobID, 7094);
            return 0;
        }

        public List<ContentMaster> GetContentData(bool IsClause,int ContentId = 0)
        {
            var sqlHelper = GetDBConnection(1);
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
        public int SaveUserTransaction(int id, int UserId, int Templateid, string LastVersion, string CurrentVersion, DateTime ModifiedDate)
        {
            var sqlHelper = GetDBConnection(1);
            var dataSet = sqlHelper.ExecuteDataSet(Constants.Tech_SaveUserTransaction, id,UserId, Templateid, LastVersion, CurrentVersion, ModifiedDate);

            return 0;
      }

        public Dictionary<string, string> GetResourcesConfigurations()
        {
            RefCountingDataReader objRefCountingDataReader = null;
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            try
            {
                var sqlHelper = GetDBConnection(3);

                objRefCountingDataReader = (RefCountingDataReader)sqlHelper.ExecuteReader("SPEND_GET_RESOURCES");
                if (objRefCountingDataReader != null)
                {
                    var sqlDr = (SqlDataReader)objRefCountingDataReader.InnerReader;

                    while (sqlDr.Read())
                    {
                        keyValuePairs.Add((string)sqlDr["ConfigKey"], (string)sqlDr["ConfigValue"]);                      
                    }
                }
            }
            catch (Exception ex)
            {
                //SpendLoggerHelper.LogError(ex, "GetResourcesConfigurations", partnerCode, partnerCode, "QAWorkbenchDAO");
                throw ex;
            }
            return keyValuePairs;
        }

        public BlobStorageDetail GetDataLakeStorageDetails()
        {
            BlobStorageDetail objBlobStorageDetail = new BlobStorageDetail();
            try
            {
                var sqlHelper = GetDBConnection(2);

                DataSet ds = sqlHelper.ExecuteDataSet(Constants.PROC_GetFileStorageDetail, 245020, true, 2200);

                if (ds.Tables != null || ds.Tables.Count > 0)
                {
                    DataTable table = ds.Tables[0];
                    if (table != null || table.Rows.Count > 0)
                    {
                        var containerName = table.Rows[0]["FSContainerName"];
                        if (IsDBNull(containerName))
                            throw new ApplicationException("FSContainerName is blank");

                        objBlobStorageDetail.FSContainerName = containerName.ToString();

                        var storageKey = table.Rows[0]["STORAGE_KEY"];
                        if (IsDBNull(storageKey))
                            throw new ApplicationException("STORAGE_KEY is blank");

                        objBlobStorageDetail.StorageKey = storageKey.ToString();

                        var storageName = table.Rows[0]["STORAGE_NAME"];
                        if (IsDBNull(storageName))
                            throw new ApplicationException("STORAGE_NAME is blank");
                        objBlobStorageDetail.StorageName = storageName.ToString();

                        var projectName = table.Rows[0]["Project_Name"];
                        if (IsDBNull(projectName))
                            throw new ApplicationException("Project_Name is blank");
                        objBlobStorageDetail.ProjectName = projectName.ToString();
                    }
                }
                return objBlobStorageDetail;
            }
            catch (SqlException ex)
            {
                //SpendLoggerHelper.LogError(ex, "GetDataLakeStorageDetails", partnerCode, partnerCode, applicationName);
                throw new ApplicationException("Error in data operation: ", ex);
            }
            catch (Exception ex)
            {
                //SpendLoggerHelper.LogError(ex, "GetDataLakeStorageDetails", partnerCode, partnerCode, applicationName);
                throw;
            }
        }

        public bool IsDBNull(object value)
        {
            return value == null || value == DBNull.Value;

        }

        public List<UserMaster> GetAllUsers(String UserId ="")
        {
            var sqlHelper = GetDBConnection(1);
            var dataSet = sqlHelper.ExecuteDataSet(Constants.Tech_GetAllUsers, UserId);
            var dt = dataSet.Tables[0];
            List<UserMaster> userList = new List<UserMaster>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                UserMaster user = new UserMaster();
                user.UserId = Convert.ToInt32(dt.Rows[i]["USERID"]);
                user.UserName = dt.Rows[i]["USERNAME"].ToString();
                user.UserEmail = dt.Rows[i]["USEREMAIL"].ToString();
                user.IsAdmin = Convert.ToBoolean(dt.Rows[i]["isAdmin"]);
                userList.Add(user);
            }
            return userList;
        }
        public List<TemplateMaster> GetAllTemplate(int TemplateId = 0)
        {
            var sqlHelper = GetDBConnection(1);
            var dataSet = sqlHelper.ExecuteDataSet(Constants.Tech_GetAllTemplates, TemplateId);
            var dt = dataSet.Tables[0];
            List<TemplateMaster> TemplateList = new List<TemplateMaster>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TemplateMaster temp = new TemplateMaster();
                temp.TemplateId = Convert.ToInt32(dt.Rows[i]["TemplateId"]);
                temp.TemplateName = dt.Rows[i]["TemplateName"].ToString();
                temp.TemplateTypeId = Convert.ToInt32(dt.Rows[i]["TemplateTypeId"].ToString());

                temp.DataLacPath = Convert.ToString(dt.Rows[i]["DataLacPath"]);

                temp.Version = Convert.ToString(dt.Rows[i]["Version"]);
                temp.LastModifiedDate = String.IsNullOrEmpty(dt.Rows[i]["LastModifiedDate"].ToString()) ? null : (DateTime?)dt.Rows[i]["LastModifiedDate"];                temp.TemplateTypeName = dt.Rows[i]["TemplateTypeName"].ToString();

                TemplateList.Add(temp);
            }
            return TemplateList;
        }
        public List<UserTransactiondata> GetAllUserTransaction()
        {
            var sqlHelper = GetDBConnection(1);
            var dataSet = sqlHelper.ExecuteDataSet(Constants.Tech_GetAllUserTransaction,0);
            var dt = dataSet.Tables[0];
            List<UserTransactiondata> UserTransactionList = new List<UserTransactiondata>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                UserTransactiondata usertr = new UserTransactiondata();
                usertr.UserId = Convert.ToInt32(dt.Rows[i]["Userid"]);
                usertr.TemplateId = Convert.ToInt32(dt.Rows[i]["TemplateId"].ToString());
                usertr.TemplateName = Convert.ToString(dt.Rows[i]["TemplateName"].ToString());

                usertr.UserName = Convert.ToString(dt.Rows[i]["UserName"]);
                usertr.LastVersion = Convert.ToString(dt.Rows[i]["LastVersion"].ToString());
                usertr.CurrentVersion = Convert.ToString(dt.Rows[i]["CurrentVersion"].ToString());
                usertr.ModifiedDate = Convert.ToDateTime(dt.Rows[i]["ModifiedDate"].ToString());

                UserTransactionList.Add(usertr);
            }
            
            return UserTransactionList;
        }
        public List<string> GetAllVersionByTeplateId(int TemplateId)
        {
            var List = GetAllUserTransaction();
            List<string> versionlist = List.Where(c => c.TemplateId == TemplateId).Select(x => x.CurrentVersion).Distinct().ToList();
            return versionlist;
        }
        public List<UserTemplateMapping> GetAllUserTemplateMapping(int userid = 0)
        {
            var sqlHelper = GetDBConnection(1);
            var dataSet = sqlHelper.ExecuteDataSet(Constants.Tech_GetAllUserTemplateMapping, userid);
            var dt = dataSet.Tables[0];
            List<UserTemplateMapping> usertemplist = new List<UserTemplateMapping>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                UserTemplateMapping objum = new UserTemplateMapping();
                objum.UserId = Convert.ToInt32(dt.Rows[i]["Userid"]);
                objum.TemplateId = Convert.ToInt32(dt.Rows[i]["TemplateId"].ToString());
                objum.TemplateName = Convert.ToString(dt.Rows[i]["TemplateName"].ToString());
                objum.isWrite = Convert.ToBoolean(dt.Rows[i]["isWrite"].ToString());
                objum.UserName = Convert.ToString(dt.Rows[i]["UserName"]);
              

                usertemplist.Add(objum);
            }
            return usertemplist;
        }
        public string SaveUserTemplateMapping(UserTemplateMapping objUserTemplateMapping)
        {
            var sqlHelper = GetDBConnection(1);
            var ds = sqlHelper.ExecuteDataSet(Constants.Tech_SaveUserTemplateMapping, objUserTemplateMapping.UserId, objUserTemplateMapping.TemplateId, objUserTemplateMapping.isWrite);
            //var dt = ds.Tables[0];
            //var op = Convert.ToString(dt.Rows[0]["outputt"].ToString());
            return "";
        }
        public List<TemplateMaster> GetAllTemplateByUserId(int UserId = 0)
        {
            var sqlHelper = GetDBConnection(1);
            var dataSet = sqlHelper.ExecuteDataSet(Constants.Tech_GetAllTemplatesByUserId, UserId);
            var dt = dataSet.Tables[0];
            List<TemplateMaster> TemplateList = new List<TemplateMaster>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TemplateMaster temp = new TemplateMaster();
                temp.TemplateId = Convert.ToInt32(dt.Rows[i]["TemplateId"]);
                temp.TemplateName = dt.Rows[i]["TemplateName"].ToString();
                temp.TemplateTypeId = Convert.ToInt32(dt.Rows[i]["TemplateTypeId"].ToString());

                temp.DataLacPath = Convert.ToString(dt.Rows[i]["DataLacPath"]);

                temp.Version = Convert.ToString(dt.Rows[i]["Version"]);
                temp.LastModifiedDate = String.IsNullOrEmpty(dt.Rows[i]["LastModifiedDate"].ToString()) ? null : (DateTime?)dt.Rows[i]["LastModifiedDate"]; temp.TemplateTypeName = dt.Rows[i]["TemplateTypeName"].ToString();

                TemplateList.Add(temp);
            }
            return TemplateList;
        }
        public string UpdateUserToAdmin(UserAdmin objUserAdmin)
        {
            var sqlHelper = GetDBConnection(1);
            var ds = sqlHelper.ExecuteDataSet(Constants.Tech_UpdateUserAdmin, objUserAdmin.UserId, objUserAdmin.isadmin);
            return "true";
        }

    }
}
