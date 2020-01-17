using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPOP_Dashboard_CMS.MPOP_Dashboard;

namespace MPOP_Dashboard_CMS
{
    public partial class RolePermission : System.Web.UI.Page
    {
        private mpop_uatEntities db = new mpop_uatEntities();
        private MPOP_Dashboard.MPOP_Dashoard_Class mpop_dashboard_class = new MPOP_Dashoard_Class();

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
                gvPageList_Bind();
            }
        }

        private void gvPageList_Bind()
        {
            DataTable dt = pagelistTable();
            gvPageList.DataSource = dt;
            gvPageList.DataBind();
        }

        private DataTable pagelistTable()
        {
            DataTable dt = new DataTable("PageListTable");
            dt.Columns.Add("PageID");
            dt.Columns.Add("PageName");
            dt.Columns.Add("PageDescription");
            dt.Columns.Add("PageCheck");

            int i = 0;

            var result = db.sp_MPOP__Dashboard_Page_Select().ToList();

            foreach (var page in result)
            {
                if (page.PageName != ConfigurationManager.AppSettings["Page1"] && page.PageName != ConfigurationManager.AppSettings["Page2"])
                {
                    dt.Rows.Add();
                    dt.Rows[i]["PageID"] = page.PageID;
                    dt.Rows[i]["PageName"] = page.PageName;
                    dt.Rows[i]["PageDescription"] = page.PageDescription;

                    i++;
                }
            }
            return dt;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlRole.Text != "--Select Role--")
            {
                MPOP_Dashboard.MPOP_Dashoard_Class mpop_dashboard_class = new MPOP_Dashoard_Class();
                MPOP_Dashboard.RolePermission rp = new MPOP_Dashboard.RolePermission();

                db.sp_MPOP_Dashboard_RolePermission_DeletedByRoleID(ddlRole.SelectedValue);

                int i = 0;

                foreach (GridViewRow row in gvPageList.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkPageCheck = (row.Cells[0].FindControl("cbPageCheck") as CheckBox);
                        if (chkPageCheck.Checked)
                        {
                            rp.PageID = gvPageList.Rows[i].Cells[0].Text;

                            db.sp_MPOP_Dashboard_RolePermission_CRUD(null, ddlRole.SelectedValue, gvPageList.Rows[i].Cells[0].Text, null, null, null, null, null, null, (int)MPOP_Dashboard_Enum.StoredProcedureType.Insert);
                        }
                    }
                    i++;
                }
                PageClear();
            }
            else
            {
                lblMessage.Text = "Please choose role!";
            }
        }

        private void PageClear()
        {
            lblMessage.Text = "Successful";
            ddlRole_Bind();
            gvPageList_Bind();
        }

        private void pagelistTableByUserRoleID(string roleid)
        {
            gvPageList_Bind();

            int i = 0;

            foreach (GridViewRow row in gvPageList.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int result = db.sp_MPOP__Dashboard_RolePermission_SelectedByRoleIDandPageID(roleid, gvPageList.Rows[i].Cells[0].Text).Count();

                    if (result != 0)
                    {
                        CheckBox checkBox = row.FindControl("cbPageCheck") as CheckBox;
                        checkBox.Checked = true;
                    }
                }
                i++;
            }
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

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            pagelistTableByUserRoleID(ddlRole.SelectedValue);
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            Session["username"] = String.Empty;
            Response.Redirect("LogIn.aspx?");
        }
    }
}