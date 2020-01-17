using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPOP_Dashboard_CMS.MPOP_Dashboard
{
    public class RolePermission
    {
        public string RolePermissionID { get; set; }
        public string RoleID { get; set; }
        public string PageID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }

        public int? Flag { get; set; }
    }
}