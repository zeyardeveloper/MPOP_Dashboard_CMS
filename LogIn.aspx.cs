using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPOP_Dashboard_CMS.MPOP_Dashboard;

namespace MPOP_Dashboard_CMS
{
    public partial class LogIn : System.Web.UI.Page
    {
        private mpop_uatEntities db = new mpop_uatEntities();
        private string DecryptedPassword;

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["username"] = string.Empty;
            txtLoginName.Focus();
        }

        protected void btnLogIn_Click(object sender, EventArgs e)
        {
            MPOP_Dashboard.MPOP_Dashoard_Class mpop_dashboard_class = new MPOP_Dashoard_Class();
            var result = db.T_Dashboard_CMS_User.Where(x => x.UserName == txtLoginName.Text.ToLower().Trim()).Select(x => new { value = x.Password, x.EncryptedKey }).FirstOrDefault();
            var isdeleted = db.T_Dashboard_CMS_User.Where(x => x.UserName == txtLoginName.Text.ToLower().Trim()).Select(x => x.IsDeleted).FirstOrDefault();
            if (result != null)
            {
                DecryptedPassword = mpop_dashboard_class.Decrypt(result.value, result.EncryptedKey);

                if (txtPassword.Text.Trim() == DecryptedPassword && isdeleted == false)
                {
                    Session["username"] = txtLoginName.Text.ToLower().Trim();
                    Response.Redirect("Dashboard.aspx?");
                }
                else
                {
                    lblMessage.Text = "Wrond UserName and Password!";
                }
            }
        }
    }
}