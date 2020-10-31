using System;
using System.IO;

namespace BusinessLayer
{
    public interface IBAO
    {
        int SaveUser(int userId);
        void UploadFileToDatalake(Stream fileStream, string fileName, int documentTypeId, int documentTemplateId);
    }
}
