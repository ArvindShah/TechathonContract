using DataAccessLayer;
using System;
using System.Collections.Generic;

namespace BusinessLayer
{
    public interface IBAO
    {
        int SaveUser(int userId);
        List<ContentMaster> GetContentData(bool IsClause,int ContentId = 0);
        int SaveUserTransaction(int id, int UserId, int Templateid, int LastVersion, int CurrentVersion, DateTime ModifiedDate);
    }
}
