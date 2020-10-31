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
    }
}
