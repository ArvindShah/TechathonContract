using Common.Entity;
using Common.Helper.DataLakeHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace spend.selfserve.businessLayer.Helper.DataLakeHelper
{
    public class DataLakeHelper
    {
        private const string TenantId = "TenantID";
        private const string ApplicationId = "ApplicationId";
        private const string AuthenticationKey = "AuthenticationKey";
        private const string OutputDataLakeSg2Path = "OutputDataLakeSG2Path";
        private string _applicationName;

        public DataLakeHelper()
        {
            _applicationName = "DataLakeHelper";
        }

        public static async Task<bool> MoveFileToDataLakeWithRetry(string storageName, string folderPath, string fileName, Stream fileStream, Dictionary<string, string> keyValuePairs, int retryAttempts = 1)
        {
            string fileSystemValue, tenantIDValue, applicationIdValue, authenticationKeyValue;
            OperationResult fileCreationResult = null;

            if (fileStream == null)
            {
                //SpendLoggerHelper.LogInfo("MoveFileToDataLakeWithRetrySSDL Method File Content/stream is null", "MoveFileToDataLake", partnerCode, partnerCode, "MoveFileToDataLake");
                throw new Exception("MoveFileToDataLakeWithRetrySSDL Method File Content/stream is null");
            }
            keyValuePairs.TryGetValue(OutputDataLakeSg2Path, out fileSystemValue);
            keyValuePairs.TryGetValue(TenantId, out tenantIDValue);
            keyValuePairs.TryGetValue(ApplicationId, out applicationIdValue);
            keyValuePairs.TryGetValue(AuthenticationKey, out authenticationKeyValue);

            if (string.IsNullOrEmpty(fileSystemValue) || string.IsNullOrEmpty(tenantIDValue) || string.IsNullOrEmpty(applicationIdValue) || string.IsNullOrEmpty(authenticationKeyValue))
            {
                //SpendLoggerHelper.LogInfo("MoveFileToDataLakeWithRetrySSDL Method Datalake Configuration missing", "MoveFileToDataLake", partnerCode, partnerCode, "MoveFileToDataLake");
                throw new Exception("MoveFileToDataLakeWithRetrySSDL Method Datalake Configuration missing");
            }

            try
            {
                DLStorageManagementClient.CreateClient(applicationIdValue, authenticationKeyValue, tenantIDValue, storageName);
                var datalakeStorageClient = new DLStorageManagementClient();

                for (int i = 0; i < retryAttempts; i++)
                {
                    fileCreationResult = await datalakeStorageClient.CreateFileAsync(fileSystemValue, folderPath, fileName, fileStream);

                    if (fileCreationResult.IsSuccessStatusCode)
                        return fileCreationResult.IsSuccessStatusCode;
                }
                if (!fileCreationResult.IsSuccessStatusCode)
                {
                    //SpendLoggerHelper.LogInfo("MoveFileToDataLakeWithRetrySSDL Method Create file Failed as -" + fileCreationResult.StatusMessage, "MoveFileToDataLake", partnerCode, partnerCode, "MoveFileToDataLakeWithRetrySSDL");
                    throw new Exception("MoveFileToDataLakeWithRetrySSDL Method Create file Failed as -" + fileCreationResult.StatusMessage);
                }
            }
            catch (Exception ex)
            {
                //SpendLoggerHelper.LogError(ex, "MoveFileToDataLakeWithRetrySSDL", partnerCode, partnerCode, "Common");
                throw;
            }
            return fileCreationResult.IsSuccessStatusCode;
        }


        public bool DeleteFileWithRetry(string storageName, string filePath, Dictionary<string, string> keyValuePairs, int retryAttempts = 1)
        {
            string fileSystemValue, tenantIDValue, applicationIdValue, authenticationKeyValue;
            OperationResult fileCreationResult = null;

            keyValuePairs.TryGetValue(OutputDataLakeSg2Path, out fileSystemValue);
            keyValuePairs.TryGetValue(TenantId, out tenantIDValue);
            keyValuePairs.TryGetValue(ApplicationId, out applicationIdValue);
            keyValuePairs.TryGetValue(AuthenticationKey, out authenticationKeyValue);

            if (string.IsNullOrEmpty(fileSystemValue) || string.IsNullOrEmpty(tenantIDValue) || string.IsNullOrEmpty(applicationIdValue) || string.IsNullOrEmpty(authenticationKeyValue))
            {
                //SpendLoggerHelper.LogInfo("DeleteFileWithRetry Method Datalake Configuration missing", "DeleteFileWithRetry", partnerCode, contactCode, _applicationName);
                throw new Exception("DeleteFileWithRetry Method Datalake Configuration missing");
            }

            try
            {
                DLStorageManagementClient.CreateClient(applicationIdValue, authenticationKeyValue, tenantIDValue, storageName);
                var datalakeStorageClient = new DLStorageManagementClient();

                for (int i = 0; i < retryAttempts; i++)
                {
                    fileCreationResult = datalakeStorageClient.DeleteFileOrDirectory(fileSystemValue, filePath);

                    if (fileCreationResult.IsSuccessStatusCode)
                        return fileCreationResult.IsSuccessStatusCode;
                }
                if (!fileCreationResult.IsSuccessStatusCode)
                {
                    //SpendLoggerHelper.LogInfo("DeleteFileWithRetry Method Create file Failed as -" + fileCreationResult.StatusMessage, "DeleteFileWithRetry", partnerCode, contactCode, _applicationName);
                    throw new Exception("DeleteFileWithRetry Method Create file Failed as -" + fileCreationResult.StatusMessage);
                }
            }
            catch (Exception ex)
            {
                //SpendLoggerHelper.LogError(ex, "DeleteFileWithRetry", partnerCode, contactCode, _applicationName);
                throw;
            }
            return fileCreationResult.IsSuccessStatusCode;
        }

        public static async Task<Stream> GetFileStreamWithStorageBlob(StorageBlob storageBlob, string reportFilePath)
        {
            try
            {
                // string filePath1 = @"https://devspendssdatalake.blob.core.windows.net/spendssintegration/351/Spend/10/3300/1/adbfiles/FieldWiseSummary/TestSpendErrorLogger.csv";

                byte[] templateBytesArrayAspose = await storageBlob.DownloadBlobAsByteArray(reportFilePath);

                Stream fileStream = new MemoryStream(templateBytesArrayAspose);

                return fileStream;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
