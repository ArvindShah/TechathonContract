using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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

    }
}
