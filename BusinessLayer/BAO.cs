using System;
using System.Collections.Generic;
using System.Text;
using DataAccessLayer;
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
        public List<ContentMaster> GetContentData(bool IsClause, int ContentId = 0)
        {
            return _DAO.GetContentData(IsClause);
        }
        public int SaveUserTransaction(int id, int UserId, int Templateid, int LastVersion, int CurrentVersion, DateTime ModifiedDate)
        {
            return _DAO.SaveUserTransaction(id,UserId, Templateid, LastVersion, CurrentVersion, ModifiedDate);
        }
    }
}
