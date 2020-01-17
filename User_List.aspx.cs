using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace MPOP_Dashboard_CMS
{
    public partial class User_List : System.Web.UI.Page
    {
        private mpop_uatEntities db = new mpop_uatEntities();
        //private string username;

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

            DataTable dt = userTable();

            gvUser.DataSource = dt;
            gvUser.DataBind();
        }

        private DataTable userTable()
        {
            DataTable dt = new DataTable("UserTable");
            dt.Columns.Add("UserID");
            dt.Columns.Add("UserName");
            dt.Columns.Add("FirstName");
            dt.Columns.Add("LastName");
            dt.Columns.Add("Role");

            int i = 0;

            var result = db.sp_MPOP__Dashboard_User_Select().ToList();
            foreach (var user in result)
            {
                dt.Rows.Add();
                dt.Rows[i]["UserID"] = user.UserID;
                dt.Rows[i]["UserName"] = user.UserName;
                dt.Rows[i]["FirstName"] = user.FirstName;
                dt.Rows[i]["LastName"] = user.LastName;
                var role = db.sp_MPOP__Dashboard_Role_SelectedByID(user.RoleID).FirstOrDefault();
                dt.Rows[i]["Role"] = role.RoleName;
                i++;
            }
            return dt;
        }

        protected void lbtnEdit_Command(object sender, CommandEventArgs e)
        {
            if (isValidPage() == false)
                lblMessage.Visible = true;
            else
                Response.Redirect("User_Entry.aspx?userid=" + e.CommandArgument.ToString() + "&isdelete=false");
        }

        protected void lbtnDelete_Command(object sender, CommandEventArgs e)
        {
            if (isValidPage() == false)
                lblMessage.Visible = true;
            else
                Response.Redirect("User_Entry.aspx?userid=" + e.CommandArgument.ToString() + "&isdelete=true");
        }

        protected void lbtnChangePassword_Command(object sender, CommandEventArgs e)
        {
            if (isValidPage() == false)
                lblMessage.Visible = true;
            else
                Response.Redirect("UserPasswordChange.aspx?userid=" + e.CommandArgument.ToString());
        }

        protected void lbtnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("Dashboard.aspx?username=" + Session["username"]);
        }

        private bool isValidPage()
        {
            string thispageid = db.T_Dashboard_CMS_Page.Where(x => x.PageName == "User_Entry").Select(x => x.PageID).FirstOrDefault();
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