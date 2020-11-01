
﻿using System;
using System.Collections.Generic;

﻿using Common.Entity;

using System.Collections.Generic;
using System.IO;


namespace DataAccessLayer
{
    public interface IDAO
    {
        int SaveUser(int userId);

        List<ContentMaster> GetContentData(bool IsClause, int ContentId = 0);
        int SaveUserTransaction(int id, int UserId, int Templateid, string LastVersion, string CurrentVersion, DateTime ModifiedDate);

        BlobStorageDetail GetDataLakeStorageDetails();
        Dictionary<string, string> GetResourcesConfigurations();
        List<UserMaster> GetAllUsers(string UserId = "");
        List<TemplateMaster> GetAllTemplate(int TemplateId = 0);
        List<UserTransactiondata> GetAllUserTransaction();
        List<UserTemplateMapping> GetAllUserTemplateMapping(int userid = 0);
        string SaveUserTemplateMapping(UserTemplateMapping objUserTemplateMapping);
        List<string> GetAllVersionByTeplateId(int TemplateId);
        List<TemplateMaster> GetAllTemplateByUserId(int UserId = 0);

    }
}
