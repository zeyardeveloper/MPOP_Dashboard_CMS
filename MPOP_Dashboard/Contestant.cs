using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPOP_Dashboard_CMS.MPOP_Dashboard
{
    public class Contestant
    {
        public string ContestantID { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedData { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public int VoteCount { get; set; }

        public string Remark { get; set; }

        public string ContestantNo { get; set; }

        public int? Flag { get; set; }
    }
}