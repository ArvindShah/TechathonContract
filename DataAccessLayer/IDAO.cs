
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
        int SaveUserTransaction(int id, int UserId, int Templateid, int LastVersion, int CurrentVersion, DateTime ModifiedDate);

        BlobStorageDetail GetDataLakeStorageDetails();
        Dictionary<string, string> GetResourcesConfigurations();
        List<UserMaster> GetAllUsers(int UserId = 0);
        List<TemplateMaster> GetAllTemplate(int TemplateId = 0);
        List<UserTransactiondata> GetAllUserTransaction();
        List<UserTemplateMapping> GetAllUserTemplateMapping();
        string SaveUserTemplateMapping(UserTemplateMapping objUserTemplateMapping);


    }
}
