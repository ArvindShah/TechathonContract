
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



    }
}
