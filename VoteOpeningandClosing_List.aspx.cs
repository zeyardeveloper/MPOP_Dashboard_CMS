using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MPOP_Dashboard_CMS
{
    public partial class VoteOpeningandClosing_List : System.Web.UI.Page
    {
        private mpop_uatEntities db = new mpop_uatEntities();

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

            DataTable dt = vocTable();
            gvVoteOpeningandClosing.DataSource = dt;
            gvVoteOpeningandClosing.DataBind();
        }

        protected void lbtnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("Dashboard.aspx?username=" + Session["username"]);
        }

        private DataTable vocTable()
        {
            DataTable dt = new DataTable("VOCTable");
            dt.Columns.Add("VotingTimeId");
            dt.Columns.Add("StartDate");
            dt.Columns.Add("EndDate");
            dt.Columns.Add("Title");

            int i = 0;

            var result = db.sp_MPOP__Dashboard_VotingOpeningandClosing_Select().ToList();
            foreach (var voc in result)
            {
                dt.Rows.Add();
                dt.Rows[i]["VotingTimeId"] = voc.VotingTimeId;
                DateTime dtStart = DateTime.Parse(voc.StartDate.ToString());
                //dt.Rows[i]["StartDate"] = dtStart.ToShortDateString();
                dt.Rows[i]["StartDate"] = voc.StartDate.ToString();
                DateTime dtEnd = DateTime.Parse(voc.EndDate.ToString());
                //dt.Rows[i]["EndDate"] = dtEnd.ToShortDateString();
                dt.Rows[i]["EndDate"] = voc.EndDate.ToString();
                dt.Rows[i]["Title"] = voc.Title;

                i++;
            }
            return dt;
        }

        protected void lbtnEdit_Command(object sender, CommandEventArgs e)
        {
            if (isValidPage() == false)
                lblMessage.Visible = true;
            else
                Response.Redirect("VoteOpeningandClosing_Entry?votingtimeid=" + e.CommandArgument.ToString() + "&isdeleted=false");
        }

        protected void lbtnDelete_Command(object sender, CommandEventArgs e)
        {
            if (isValidPage() == false)
                lblMessage.Visible = true;
            else
                Response.Redirect("VoteOpeningandClosing_Entry?votingtimeid=" + e.CommandArgument.ToString() + "&isdeleted=true");
        }

        private bool isValidPage()
        {
            string thispageid = db.T_Dashboard_CMS_Page.Where(x => x.PageName == "VoteOpeningandClosing_Entry").Select(x => x.PageID).FirstOrDefault();
            string username = Session["username"].ToString();
            string roleid = db.T_Dashboard_CMS_User.Where(x => x.UserName == username).Select(x => x.RoleID).FirstOrDefault();
            int isvalidpage = db.T_Dashboard_CMS_RolePermission.Where(x => x.RoleID == roleid && x.PageID == thispageid).Select(x => x.RolePermissionID).Count();

            if (isvalidpage == 0)
                return false;
            else
                return true;
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            Session["username"] = String.Empty;
            Response.Redirect("LogIn.aspx?");
        }
    }
}