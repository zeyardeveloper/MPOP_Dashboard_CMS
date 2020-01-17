using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MPOP_Dashboard_CMS
{
    public partial class Contestant_List : System.Web.UI.Page
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

            DataTable dt = contestantTable();
            gvContestant.DataSource = dt;
            gvContestant.DataBind();
        }

        private DataTable contestantTable()
        {
            DataTable dt = new DataTable("ContestantTable");
            dt.Columns.Add("ContestantID");
            dt.Columns.Add("Name");
            dt.Columns.Add("VoteCount");
            dt.Columns.Add("Remark");
            dt.Columns.Add("ContestantNo");

            int i = 0;

            var contestantResult = db.sp_MPOP__Dashboard_Contestant_Select().ToList();
            foreach (var c in contestantResult)
            {
                dt.Rows.Add();
                dt.Rows[i]["ContestantID"] = c.ContestantID;
                dt.Rows[i]["Name"] = c.Name;
                dt.Rows[i]["VoteCount"] = c.VoteCount;
                dt.Rows[i]["Remark"] = c.Remark;
                dt.Rows[i]["ContestantNo"] = c.ContestantNo;

                i++;
            }
            return dt;
        }

        protected void lbtnEdit_Command(object sender, CommandEventArgs e)
        {
            if (isValidPage() == false)
                lblMessage.Visible = true;
            else
                Response.Redirect("Contestant_Entry.aspx?contestantid=" + e.CommandArgument.ToString() + "&isdeleted=false");
        }

        protected void lbtnDelete_Command(object sender, CommandEventArgs e)
        {
            if (isValidPage() == false)
                lblMessage.Visible = true;
            else
                Response.Redirect("Contestant_Entry.aspx?contestantid=" + e.CommandArgument.ToString() + "&isdeleted=true");
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            Session["username"] = String.Empty;
            Response.Redirect("LogIn.aspx?");
        }

        private bool isValidPage()
        {
            string thispageid = db.T_Dashboard_CMS_Page.Where(x => x.PageName == "Contestant_Entry").Select(x => x.PageID).FirstOrDefault();
            string username = Session["username"].ToString();
            string roleid = db.T_Dashboard_CMS_User.Where(x => x.UserName == username).Select(x => x.RoleID).FirstOrDefault();
            int isvalidpage = db.T_Dashboard_CMS_RolePermission.Where(x => x.RoleID == roleid && x.PageID == thispageid).Select(x => x.RolePermissionID).Count();

            if (isvalidpage == 0)
                return false;
            else
                return true;
        }
    }
}