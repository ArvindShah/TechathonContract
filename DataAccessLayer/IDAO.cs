using Common.Entity;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataAccessLayer
{
    public interface IDAO
    {
        int SaveUser(int userId);
        BlobStorageDetail GetDataLakeStorageDetails();
        Dictionary<string, string> GetResourcesConfigurations();


    }
}
