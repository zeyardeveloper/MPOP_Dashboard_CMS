using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPOP_Dashboard_CMS.MPOP_Dashboard
{
    public static class MPOP_Dashboard_Enum
    {
        public enum StoredProcedureType : int
        {
            Select = 0,
            Insert = 1,
            Update = 2,
            Delete = 3
        }
    }
}