using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MPOP_Dashboard_CMS.MPOP_Dashboard;

namespace MPOP_Dashboard_CMS
{
    public partial class VoteOpeningandClosing_Entry : System.Web.UI.Page
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
                var votingOCResult = db.sp_MPOP__Dashboard_VotingOpeningandClosing_SelectedByID(Request.QueryString["votingtimeid"]).FirstOrDefault();

                if (votingOCResult != null)
                {
                    hfVotingTimeId.Value = votingOCResult.VotingTimeID;
                    //txtStartDate.Text = Convert.ToDateTime(votingOCResult.StartDate).ToString("dd/MM/yyyy");

                    cldStartDate.SelectedDate = (DateTime)votingOCResult.StartDate;
                    txtStartDate.Text = cldStartDate.SelectedDate.ToShortDateString();
                    Session["StartDate"] = votingOCResult.StartDate;

                    //txtEndDate.Text = Convert.ToDateTime(votingOCResult.EndDate).ToString("dd/MM/yyyy");

                    cldEndDate.SelectedDate = (DateTime)votingOCResult.EndDate;
                    txtEndDate.Text = cldEndDate.SelectedDate.ToShortDateString();
                    Session["EndDate"] = votingOCResult.EndDate;

                    txtTitle.Text = votingOCResult.Title;
                    if (Request.QueryString["isdeleted"] == "false")
                    {
                        btnSave.Text = "Update";
                    }
                    else
                    {
                        btnSave.Text = "Delete";
                        txtTitle.ReadOnly = true;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (cldStartDate.SelectedDate != DateTime.Parse("1/1/0001 12:00:00 AM") && cldEndDate.SelectedDate != DateTime.Parse("1/1/0001 12:00:00 AM"))
            {
                MPOP_Dashboard.MPOP_Dashoard_Class mpop_dashboard_class = new MPOP_Dashoard_Class();
                MPOP_Dashboard.VoteOpeningandClosingEntry voc = new MPOP_Dashboard.VoteOpeningandClosingEntry();

                DataExtract(voc);

                db.sp_MPOP_Dashboard_VotingOpeningandClosing_CRUD(
                      voc.VotingTimeId,
                      voc.StartDate,
                      voc.EndDate,
                      voc.Title,
                      voc.Flag
                      );

                //PageClear();
                Response.Redirect("VoteOpeningandClosing_List.aspx?");
            }
            else
            {
                lblMessage.Text = "Please choose start date and end date!";
            }
        }

        private void DataExtract(MPOP_Dashboard.VoteOpeningandClosingEntry voc)
        {
            voc.VotingTimeId = hfVotingTimeId.Value;
            //voc.StartDate = DateTime.ParseExact(txtStartDate.Text, "dd/MM/yyyy", null);//DateTime.Parse(txtStartDate.Text);

            //if (Session["StartDate"] != null)
            voc.StartDate = DateTime.Parse(Session["StartDate"].ToString());//cldStartDate.SelectedDate;
                                                                            //else
                                                                            //voc.StartDate = DateTime.ParseExact(txtStartDate.Text, "dd/MM/yyyy", null);

            //voc.EndDate = DateTime.ParseExact(txtEndDate.Text, "dd/MM/yyyy", null);//DateTime.Parse(txtEndDate.Text);

            voc.EndDate = DateTime.Parse(Session["EndDate"].ToString());//cldEndDate.SelectedDate;
            voc.Title = txtTitle.Text;

            if (btnSave.Text == "Save")
                voc.Flag = (int)MPOP_Dashboard_Enum.StoredProcedureType.Insert;
            else if (btnSave.Text == "Update")
                voc.Flag = (int)MPOP_Dashboard_Enum.StoredProcedureType.Update;
            else if (btnSave.Text == "Delete")
                voc.Flag = (int)MPOP_Dashboard_Enum.StoredProcedureType.Delete;
        }

        protected void lbtnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("Dashboard.aspx?username=" + Session["username"]);
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            Session["username"] = String.Empty;
            Response.Redirect("LogIn.aspx?");
        }

        protected void cldStartDate_SelectionChanged(object sender, EventArgs e)
        {
            Calendar cldEndDate = (Calendar)sender;
            //txtStartDate.Text = cldStartDate.SelectedDate.ToShortDateString();
            string sdate = cldStartDate.SelectedDate.ToShortDateString();
            //string stime = DateTime.Now.ToShortTimeString();
            DateTime time = DateTime.Now;
            string h = time.Hour.ToString();
            string m = time.Minute.ToString();
            string s = time.Second.ToString();
            string stime = h + ":" + m + ":" + s;
            string datetime = sdate + " " + stime;
            txtStartDate.Text = datetime;
            DateTime startdt = DateTime.Parse(datetime);
            Session["StartDate"] = startdt;
        }

        protected void cldEndDate_SelectionChanged(object sender, EventArgs e)
        {
            Calendar cldEndDate = (Calendar)sender;
            //txtEndDate.Text = cldEndDate.SelectedDate.ToShortDateString();
            string edate = cldEndDate.SelectedDate.ToShortDateString();
            //string etime = DateTime.Now.ToShortTimeString();
            DateTime time = DateTime.Now;
            string h = time.Hour.ToString();
            string m = time.Minute.ToString();
            string s = time.Second.ToString();
            string etime = h + ":" + m + ":" + s;
            string datetime = edate + " " + etime;
            txtEndDate.Text = datetime;
            DateTime enddt = DateTime.Parse(datetime);
            Session["EndDate"] = enddt;
        }
    }
}