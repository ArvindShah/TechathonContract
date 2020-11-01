
﻿using DataAccessLayer;
using System;
using System.Collections.Generic;

using System.IO;


namespace BusinessLayer
{
    public interface IBAO
    {
        int SaveUser(int userId);

        List<ContentMaster> GetContentData(bool IsClause,int ContentId = 0);
        int SaveUserTransaction(int id, int UserId, int Templateid, int LastVersion, int CurrentVersion, DateTime ModifiedDate);

        void UploadFileToDatalake(Stream fileStream, string fileName, int documentTypeId, int documentTemplateId);
        List<UserMaster> GetAllUsers(String UserId = "");
        List<TemplateMaster> GetAllTemplate(int TemplateId = 0);
        List<UserTransactiondata> GetAllUserTransaction();
        List<UserTemplateMapping> GetAllUserTemplateMapping();

    }
}
