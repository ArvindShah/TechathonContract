
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
        int SaveUserTransaction(int id, int UserId, int Templateid, string LastVersion, string CurrentVersion, DateTime ModifiedDate);

        void UploadFileToDatalake(Stream fileStream, string fileName, int documentTypeId, int documentTemplateId);
        List<UserMaster> GetAllUsers(int UserId = 0);
        List<TemplateMaster> GetAllTemplate(int TemplateId = 0);
        List<UserTransactiondata> GetAllUserTransaction();
        List<UserTemplateMapping> GetAllUserTemplateMapping(int userid = 0);
        string SaveUserTemplateMapping(UserTemplateMapping objUserTemplateMapping);
        List<string> GetAllVersionByTeplateId(int TemplateId);

    }
}
