using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPOP_Dashboard_CMS.MPOP_Dashboard;

namespace MPOP_Dashboard_CMS
{
    public partial class _Default : Page
    {
        private mpop_uatEntities db = new mpop_uatEntities();
        private MPOP_Dashboard.MPOP_Dashoard_Class mpop_dashboard_class = new MPOP_Dashoard_Class();
        private string EncryptionKey;
        private string EncryptedPassword;

        //private bool isSelectRoleID = false;
        private string DecryptedPassword;

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
                ddlRole_Bind();
                if (Request.QueryString["UserID"] != null)
                {
                    var userResult = db.sp_MPOP__Dashboard_User_SelectedByID(Request.QueryString["UserID"]).FirstOrDefault();
                    hfUserID.Value = userResult.UserID;
                    txtUserName.Text = userResult.UserName;

                    DecryptedPassword = mpop_dashboard_class.Decrypt(userResult.Password, userResult.EncryptedKey);
                    hfPassword.Value = DecryptedPassword;

                    txtFirstName.Text = userResult.FirstName;
                    txtLastName.Text = userResult.LastName;
                    ddlRole.Items.Insert(0, "--Select Role--");

                    var role = db.sp_MPOP__Dashboard_Role_SelectedByID(userResult.RoleID).FirstOrDefault();
                    hfRoleID.Value = role.RoleID;
                    ddlRole.SelectedItem.Text = role.RoleName;
                    ddlRole.DataValueField = "RoleID";
                    ddlRole.DataTextField = "RoleName";

                    if (Request.QueryString["isdelete"] == "false")
                    {
                        btnSave.Text = "Update";
                        txtUserName.ReadOnly = true;
                    }
                    else
                    {
                        btnSave.Text = "Delete";
                        txtFirstName.ReadOnly = true;
                        txtLastName.ReadOnly = true;
                        txtUserName.ReadOnly = true;
                        txtPassword.ReadOnly = true;
                        ddlRole.Enabled = false;
                    }

                    lblPassword.Visible = false;
                    txtPassword.Visible = false;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int count = db.T_Dashboard_CMS_User.Where(x => x.UserName == txtUserName.Text.ToLower().Trim()).Select(x => x.UserName).Count();
            MPOP_Dashboard.MPOP_Dashoard_Class mpop_dashboard_class = new MPOP_Dashoard_Class();
            MPOP_Dashboard.User_Registration ur = new MPOP_Dashboard.User_Registration();

            EncryptionKey = mpop_dashboard_class.GenerateEncryptionKey();

            if (btnSave.Text == "Save")
                EncryptedPassword = mpop_dashboard_class.Encrypt(txtPassword.Text.Trim(), EncryptionKey);
            else
                EncryptedPassword = mpop_dashboard_class.Encrypt(hfPassword.Value, EncryptionKey);

            DataExtract(ur);
            bool checkddl = IsGuid(ddlRole.SelectedValue);

            if (btnSave.Text == "Save" && count == 1)
            {
                if (count != 0 && checkddl == false)
                    lblMessage.Text = "User is already exist!";
                else
                    lblMessage.Text = "Please fill data correctly!";
            }
            else
            {
                if (checkddl == false && hfRoleID.Value == "")
                {
                    lblMessage.Text = "Please fill data correctly!";
                }
                else
                {
                    db.sp_MPOP_Dashboard_User_CRUD(
                          ur.UserID,
                          ur.UserName,
                          ur.Password,
                          ur.EncryptedKey,
                          ur.FirstName,
                          ur.LastName,
                          ur.RoleID,
                          ur.IsDeleted,
                          ur.CreatedDate,
                          ur.CreatedBy,
                          ur.UpdatedDate,
                          ur.UpdatedBy,
                           ur.DeletedDate,
                          ur.DeletedBy,
                          ur.Flag
                          );

                    //var userid_roleid = db.T_Dashboard_CMS_User.OrderByDescending(x => x.CreatedDate).Select(x => new { value = x.UserID, x.RoleID }).FirstOrDefault();
                    //db.sp_MPOP_Dashboard_UserRole_Mapping_CRUD(null, userid_roleid.value, userid_roleid.RoleID, null, Session["username"].ToString(), null, null, null, null, (int)MPOP_Dashboard_Enum.StoredProcedureType.Insert);

                    PageClear();
                    Response.Redirect("User_List.aspx?");
                }
            }
        }

        private void DataExtract(User_Registration ur)
        {
            ur.UserID = hfUserID.Value;
            ur.UserName = txtUserName.Text.ToLower().Trim();
            ur.Password = EncryptedPassword;
            ur.EncryptedKey = EncryptionKey;
            ur.FirstName = txtFirstName.Text.Trim();
            ur.LastName = txtLastName.Text.Trim();

            bool checkddl = IsGuid(ddlRole.SelectedValue);

            if (checkddl == true)
                ur.RoleID = ddlRole.SelectedValue;
            else
                ur.RoleID = hfRoleID.Value;

            ur.IsDeleted = false;
            ur.CreatedDate = null;
            ur.CreatedBy = Session["username"].ToString();
            ur.UpdatedDate = null;
            if (btnSave.Text == "Update")
                ur.UpdatedBy = Session["username"].ToString();
            else
                ur.UpdatedBy = null;

            ur.DeletedDate = null;

            if (btnSave.Text == "Delete")
                ur.DeletedBy = Session["username"].ToString();
            else
                ur.DeletedBy = null;

            if (btnSave.Text == "Save")
                ur.Flag = (int)MPOP_Dashboard_Enum.StoredProcedureType.Insert;
            else if (btnSave.Text == "Update")
                ur.Flag = (int)MPOP_Dashboard_Enum.StoredProcedureType.Update;
            else
                ur.Flag = (int)MPOP_Dashboard_Enum.StoredProcedureType.Delete;
        }

        private void PageClear()
        {
            hfUserID.Value = null;
            txtUserName.Text = String.Empty;
            txtPassword.Text = String.Empty;
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
        }

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            //isSelectRoleID = true;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            db.sp_MPOP__Dashboard_User_DeletedByID(Request.QueryString["UserID"]);
            Response.Redirect("User_List.aspx?");
        }

        protected void lbtnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("Dashboard.aspx?username=" + Session["username"]);
        }

        private void ddlRole_Bind()
        {
            #region ddlRole Bind

            ddlRole.Items.Clear();
            ddlRole.Items.Insert(0, "--Select Role--");
            ddlRole.DataSource = db.sp_MPOP__Dashboard_Role_Select().ToList();
            ddlRole.DataValueField = "RoleID";
            ddlRole.DataTextField = "RoleName";
            ddlRole.DataBind();

            #endregion ddlRole Bind
        }

        public static bool IsGuid(string guidString)
        {
            bool bResult = false;
            try
            {
                Guid g = new Guid(guidString);
                bResult = true;
            }
            catch
            {
                bResult = false;
            }

            return bResult;
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            Session["username"] = String.Empty;
            Response.Redirect("LogIn.aspx?");
        }
    }
}