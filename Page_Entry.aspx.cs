using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPOP_Dashboard_CMS.MPOP_Dashboard;

namespace MPOP_Dashboard_CMS
{
    public partial class Page_Entry : System.Web.UI.Page
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

            gvPages_Bind();
        }

        private void gvPages_Bind()
        {
            DataTable dt = pagesTable();
            gvPages.DataSource = dt;
            gvPages.DataBind();
        }

        private DataTable pagesTable()
        {
            DataTable dt = new DataTable("PagesTable");
            dt.Columns.Add("PageID");
            dt.Columns.Add("PageName");
            dt.Columns.Add("PageDescription");

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
            if (txtPageName.Text.Trim() == "")
            {
                lblMessagae.Visible = true;
            }
            else
            {
                MPOP_Dashboard.DashboardPages dp = new MPOP_Dashboard.DashboardPages();

                DataExtract(dp);

                db.sp_MPOP_Dashboard_Page_CRUD(
                      dp.PageID,
                      dp.PageName,
                      dp.PageDescription,
                      dp.IsDeleted,
                      dp.CreatedDate,
                      dp.CreatedBy,
                      dp.UpdatedDate,
                      dp.UpdatedBy,
                      dp.DeletedDate,
                      dp.DeletedBy,
                      dp.Flag
                      );

                gvPages_Bind();
                PageClear();
            }
        }

        private void PageClear()
        {
            lblMessage.Text = "Successful";
            hfPageID.Value = null;
            txtPageName.Text = String.Empty;
            txtPageDescription.Text = String.Empty;
            btnSave.Text = "Save";
        }

        private void DataExtract(MPOP_Dashboard.DashboardPages dp)
        {
            dp.PageID = hfPageID.Value;
            dp.PageName = txtPageName.Text.Trim();
            dp.PageDescription = txtPageDescription.Text.Trim();

            if (btnSave.Text == "Save")
                dp.IsDeleted = false;
            else if (btnSave.Text == "Update")
                dp.IsDeleted = false;
            else
                dp.IsDeleted = true;

            dp.CreatedDate = null;
            dp.CreatedBy = null;
            dp.UpdatedDate = null;
            if (btnSave.Text == "Update")
                dp.UpdatedBy = Session["username"].ToString();
            else
                dp.UpdatedBy = null;

            dp.DeletedDate = null;

            if (btnSave.Text == "Delete")
                dp.DeletedBy = Session["username"].ToString();
            else
                dp.DeletedBy = null;

            if (btnSave.Text == "Save")
                dp.Flag = (int)MPOP_Dashboard_Enum.StoredProcedureType.Insert;
            else if (btnSave.Text == "Update")
                dp.Flag = (int)MPOP_Dashboard_Enum.StoredProcedureType.Update;
            else
                dp.Flag = (int)MPOP_Dashboard_Enum.StoredProcedureType.Delete;
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            Session["username"] = String.Empty;
            Response.Redirect("LogIn.aspx?");
        }

        protected void chkIsDefault_CheckedChanged(object sender, EventArgs e)
        {
        }

        protected void lbtnEdit_Command(object sender, CommandEventArgs e)
        {
            hfPageID.Value = e.CommandArgument.ToString();
            var result = db.sp_MPOP__Dashboard_Page_SelectedByID(e.CommandArgument.ToString()).FirstOrDefault();
            txtPageName.Text = result.PageName;
            txtPageDescription.Text = result.PageDescription;
            btnSave.Text = "Update";
        }

        protected void lbtnDelete_Command(object sender, CommandEventArgs e)
        {
            hfPageID.Value = e.CommandArgument.ToString();
            var result = db.sp_MPOP__Dashboard_Page_SelectedByID(e.CommandArgument.ToString()).FirstOrDefault();
            txtPageName.Text = result.PageName;
            txtPageDescription.Text = result.PageDescription;
            btnSave.Text = "Delete";
        }
    }
}