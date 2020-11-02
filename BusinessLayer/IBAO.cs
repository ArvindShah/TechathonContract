
﻿using DataAccessLayer;
using System;
using System.Collections.Generic;

using System.IO;
using System.Text;

namespace BusinessLayer
{
    public interface IBAO
    {
        int SaveUser(int userId);

        List<ContentMaster> GetContentData(bool IsClause,int ContentId = 0);
        int SaveUserTransaction(int id, int UserId, int Templateid, string LastVersion, string CurrentVersion, DateTime ModifiedDate);

        void UploadFileToDatalake(Stream fileStream, string fileName, int documentTypeId, int documentTemplateId);
        List<UserMaster> GetAllUsers(String UserId = "");
        List<TemplateMaster> GetAllTemplate(int TemplateId = 0);
        List<UserTransactiondata> GetAllUserTransaction();
        List<UserTemplateMapping> GetAllUserTemplateMapping(int userid = 0);
        string SaveUserTemplateMapping(UserTemplateMapping objUserTemplateMapping);
        List<string> GetAllVersionByTeplateId(int TemplateId);
        List<TemplateMaster> GetAllTemplateByUserId(int UserId = 0);
        string UpdateUserToAdmin(UserAdmin objUserAdmin);

        string DeleteTemplateUserMapping( DelUserTemp delObj );

       // List<UserTemplateMapping> GetAllUserTemplateMapping();
        StringBuilder getWordDoc(string docName, string version);
        void SaveDOCX(string v1, string v2, bool isLandScape = false, double rMargin = 1, double lMargin = 1, double bMargin = 1, double tMargin = 1);
        List<ContentType> GetContentType();
    }
}
