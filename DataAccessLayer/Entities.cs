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
        public string LastVersion { get; set; }
        public string CurrentVersion { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
    public class UserMaster
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public bool IsAdmin { get; set; }


    }
    public class TemplateMaster
    {
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public int TemplateTypeId { get; set; }

        public string DataLacPath { get; set; }
        public string Version { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string TemplateTypeName { get; set; }
    }

    public class UserTransactiondata
    {
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string LastVersion { get; set; }
        public string CurrentVersion { get; set; }
        public DateTime ModifiedDate { get; set; }
       
    }
    public class UserTemplateMapping
    {
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
     public bool isWrite { get; set; }

    }
    public class UserAdmin
    {
        public int UserId { get; set; }
      
        public bool isadmin { get; set; }

    }
}
