using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPOP_Dashboard_CMS.MPOP_Dashboard;

namespace MPOP_Dashboard_CMS
{
    public partial class UserPasswordChange : System.Web.UI.Page
    {
        private mpop_uatEntities db = new mpop_uatEntities();
        private MPOP_Dashboard.MPOP_Dashoard_Class mpop_dashboard_class = new MPOP_Dashoard_Class();
        private string EncryptionKey;
        private string EncryptedPassword;

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
                string userid = Request.QueryString["userid"];
                var result = db.T_Dashboard_CMS_User.Where(x => x.UserID == userid).Select(x => new { value = x.Password, x.EncryptedKey }).FirstOrDefault();
                hfOldPassword.Value = mpop_dashboard_class.Decrypt(result.value, result.EncryptedKey);
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            MPOP_Dashboard.MPOP_Dashoard_Class mpop_dashboard_class = new MPOP_Dashoard_Class();

            if (hfOldPassword.Value == txtOldPassword.Text.Trim())
            {
                EncryptionKey = mpop_dashboard_class.GenerateEncryptionKey();
                EncryptedPassword = mpop_dashboard_class.Encrypt(txtNewPassword.Text.Trim(), EncryptionKey);

                db.sp_MPOP__Dashboard_User_ChangePassword(Request.QueryString["UserID"], EncryptedPassword, EncryptionKey);

                Response.Redirect("User_List.aspx?");
            }
            else
            {
                lblMessage.Visible = true;
            }
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            Session["username"] = String.Empty;
            Response.Redirect("LogIn.aspx?");
        }
    }
}