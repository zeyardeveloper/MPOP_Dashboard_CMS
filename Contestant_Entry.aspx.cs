using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPOP_Dashboard_CMS.MPOP_Dashboard;

namespace MPOP_Dashboard_CMS
{
    public partial class Contestant_Entry : System.Web.UI.Page
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
                string contestantid = Request.QueryString["contestantid"];
                var contestantResult = db.T_Mpop_Contestant.Where(x => x.ContestantID == contestantid).FirstOrDefault();

                if (contestantResult != null)
                {
                    hfContestantID.Value = contestantResult.ContestantID;

                    txtContestantName.Text = contestantResult.Name;
                    txtVoteCount.Text = contestantResult.VoteCount.ToString();
                    txtRemark.Text = contestantResult.Remark.ToString();
                    txtContestantNo.Text = contestantResult.ContestantNo;

                    if (Request.QueryString["isdeleted"] == "false")
                    {
                        btnSave.Text = "Update";
                    }
                    else
                    {
                        btnSave.Text = "Delete";

                        txtContestantName.ReadOnly = true;
                        txtVoteCount.ReadOnly = true;
                        txtRemark.ReadOnly = true;
                        txtContestantNo.ReadOnly = true;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            MPOP_Dashboard.MPOP_Dashoard_Class mpop_dashboard_class = new MPOP_Dashoard_Class();
            MPOP_Dashboard.Contestant contestant = new MPOP_Dashboard.Contestant();

            DataExtract(contestant);

            db.sp_MPOP_Dashboard_Contestant_CRUD(
                  contestant.ContestantID,
                  contestant.Name,
                  contestant.CreatedData,
                  contestant.CreatedBy,
                  contestant.UpdatedDate,
                  contestant.UpdatedBy,
                  contestant.VoteCount,
                  contestant.Remark,
                  contestant.ContestantNo,
                  contestant.Flag
                  );
            Response.Redirect("Contestant_List.aspx?");
        }

        private void DataExtract(MPOP_Dashboard.Contestant contestant)
        {
            contestant.ContestantID = hfContestantID.Value;
            contestant.Name = txtContestantName.Text.Trim();
            contestant.CreatedData = null;
            contestant.CreatedBy = Session["username"].ToString();
            contestant.UpdatedDate = null;
            contestant.UpdatedBy = Session["username"].ToString();
            contestant.VoteCount = int.Parse(txtVoteCount.Text);
            contestant.Remark = txtRemark.Text;
            contestant.ContestantNo = txtContestantNo.Text;

            if (btnSave.Text == "Save")
                contestant.Flag = (int)MPOP_Dashboard_Enum.StoredProcedureType.Insert;
            else if (btnSave.Text == "Update")
                contestant.Flag = (int)MPOP_Dashboard_Enum.StoredProcedureType.Update;
            else if (btnSave.Text == "Delete")
                contestant.Flag = (int)MPOP_Dashboard_Enum.StoredProcedureType.Delete;
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            Session["username"] = String.Empty;
            Response.Redirect("LogIn.aspx?");
        }
    }
}