using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MPOP_Dashboard_CMS
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private mpop_uatEntities db = new mpop_uatEntities();
        private string username;
        private string roleid;

        protected void Page_Load(object sender, EventArgs e)
        {
            LinkButton lbtn = Master.FindControl("lbtnMasterLogOut") as LinkButton;
            lbtn.Text = "Log Out";

            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/LogIn.aspx");
            }

            if (!IsPostBack)
            {
                DataTable dt = pageTable();
                gvPages.DataSource = dt;
                gvPages.DataBind();
            }
        }

        private DataTable pageTable()
        {
            username = Session["username"].ToString();
            if (username == "")
                Response.Redirect("LogIn.aspx");

            roleid = db.T_Dashboard_CMS_User.Where(x => x.UserName == username).Select(x => x.RoleID).FirstOrDefault();

            var resultPage = db.T_Dashboard_CMS_RolePermission.Where(x => x.RoleID == roleid).Select(x => x.PageID).ToList();

            DataTable dt = new DataTable("PagesTable");
            dt.Columns.Add("PageID");
            dt.Columns.Add("PageName");
            dt.Columns.Add("PageDescription");

            int i = 0;

            var result = db.sp_MPOP__Dashboard_Page_Select().ToList();

            foreach (var page in result)
            {
                foreach (var rp in resultPage)
                {
                    if (rp == page.PageID)
                    {
                        dt.Rows.Add();
                        dt.Rows[i]["PageID"] = page.PageID;
                        dt.Rows[i]["PageName"] = page.PageName;
                        dt.Rows[i]["PageDescription"] = page.PageDescription;
                        i++;
                    }
                }
            }
            return dt;
        }

        protected void lbtnPageLink_Command(object sender, CommandEventArgs e)
        {
            string pageurl = e.CommandArgument.ToString() + ".aspx?";
            Response.Redirect(e.CommandArgument.ToString());
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            Session["username"] = String.Empty;
            Response.Redirect("LogIn.aspx?");
        }
    }
}