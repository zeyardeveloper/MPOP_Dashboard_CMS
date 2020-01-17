using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPOP_Dashboard_CMS.MPOP_Dashboard
{
    public class MyTelBillCharge
    {
        public string MyTelBillChargeID { get; set; }
        public string MSISDN { get; set; }

        public decimal Amount { get; set; }
        public string Result { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}