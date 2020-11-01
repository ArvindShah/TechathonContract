using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public class ContentMaster
    {
        public int ContentId { get; set; }
        public string ContentName { get; set; }
        public string ContentDescription { get; set; }
        public bool isClause { get; set; }
        public bool IsEditable { get; set; }
        public string Comment { get; set; }

    }
    public class UserTransaction
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public int Templateid { get; set; }
        public int LastVersion { get; set; }
        public int CurrentVersion { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
