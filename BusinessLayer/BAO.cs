using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DataAccessLayer;
using spend.selfserve.businessLayer.Helper.DataLakeHelper;

namespace BusinessLayer
{
    public class BAO : IBAO
    {
        private readonly IDAO _DAO;
        public BAO(IDAO DAO)
        {
            _DAO = DAO;
        }

        public int SaveUser(int userId)
        {
            return _DAO.SaveUser(userId);
        }

        public async void UploadFileToDatalake(Stream fileStream, string fileName, int documentTypeId, int documentTemplateId )
        {
            try
            {
                string fileType = fileName.Substring(fileName.Length - 4);
                string newFileName = fileName.Remove(fileName.Length - 5);
                int lastIndex = newFileName.LastIndexOf(".");
                if(lastIndex < 0)
                {
                    newFileName = newFileName + "_v1.0.0.0";
                    lastIndex = newFileName.LastIndexOf(".");
                }
                string lastNumber = newFileName.Substring(lastIndex + 1);
                string increment = (int.Parse(lastNumber) + 1).ToString();
                string result = string.Concat(newFileName.Substring(0, lastIndex + 1), increment) + "." + fileType;

                var folderPath = $"{"Techathon20"}/{documentTypeId}/{documentTemplateId}";
                var blobStorageDetail = _DAO.GetDataLakeStorageDetails();
                var resourceConfigurations = _DAO.GetResourcesConfigurations();
                bool isFileMoved = await DataLakeHelper.MoveFileToDataLakeWithRetry(blobStorageDetail.StorageName, folderPath, result, fileStream, resourceConfigurations);
                if (!isFileMoved)
                    throw new ApplicationException("Error: File could not moved due to unknown problem at blob storage");
            }
            catch
            {
                throw;
            }
        }
    }
}
