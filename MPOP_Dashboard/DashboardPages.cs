using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPOP_Dashboard_CMS.MPOP_Dashboard
{
    public class DashboardPages
    {
        public string PageID { get; set; }
        public string PageName { get; set; }
        public string PageDescription { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }

        public int? Flag { get; set; }
    }
}