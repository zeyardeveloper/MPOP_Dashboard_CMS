using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPOP_Dashboard_CMS.MPOP_Dashboard
{
    public class VoteOpeningandClosingEntry
    {
        public string VotingTimeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Title { get; set; }

        public int? Flag { get; set; }
    }
}