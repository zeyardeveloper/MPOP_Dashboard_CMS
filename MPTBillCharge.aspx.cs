using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using MPOP_Dashboard_CMS.MPOP_Dashboard;

namespace MPOP_Dashboard_CMS
{
    public partial class MPTBillCharge : System.Web.UI.Page
    {
        private mpop_uatEntities db = new mpop_uatEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }

            if (strPreviousPage == "")
            {
                Response.Redirect("~/LogIn.aspx");
            }

            LinkButton lbtn = Master.FindControl("lbtnMasterLogOut") as LinkButton;
            lbtn.Text = "Log Out";
        }

        protected void btnSync_Click(object sender, EventArgs e)
        {
            if (txtMSISDNNo.Text.Trim() == "" && txtAmount.Text.Trim() == "")
            {
                lblMessage.Text = ConfigurationManager.AppSettings["RequiredField"];
            }
            else
            {
                Dictionary<string, string> postData = new Dictionary<string, string>(2);
                postData.Add("msisdn", txtMSISDNNo.Text.Trim());
                postData.Add("amount", txtAmount.Text.Trim());
                postData.Add("referenceCode", RandomDigits(13));
                postData.Add("clientCorrelator", RandomDigits(20));
                postData.Add("appkey", ConfigurationManager.AppSettings["mptappkey"]);

                string url = "http://54.251.130.27/MPTChargingandSMS/api/ChargingandSms/MPTBillCharge";

                _ = PostFormUrlEncodedAsync(url, postData);
            }
        }

        public async Task PostFormUrlEncodedAsync(string url, IEnumerable<KeyValuePair<string, string>> postData)
        {
            try
            {
                HttpResponseMessage response = null;
                using (var httpClient = new HttpClient(new HttpClientHandler { UseProxy = false }))
                {
                    using (var content = new FormUrlEncodedContent(postData))
                    {
                        content.Headers.Clear();
                        content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                        response = httpClient.PostAsync(url, content).Result;
                        txtResult.Text = await response.Content.ReadAsStringAsync();

                        lblMessage.Text = ConfigurationManager.AppSettings["Successful"];

                        MPOP_Dashboard.MPTBillCharge mptbillcharge = new MPOP_Dashboard.MPTBillCharge();
                        DataExtract(mptbillcharge);
                        db.sp_MPOP_Dashboard_MPTBillCharge_CRUD(mptbillcharge.MPTBillChargeID, mptbillcharge.MSISDN, mptbillcharge.Amount, mptbillcharge.Result, mptbillcharge.CreatedDate, mptbillcharge.CreatedBy);
                    }
                }
            }
            catch
            {
                lblMessage.Text = ConfigurationManager.AppSettings["Unsuccessful"];
            }
        }

        public string RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());

            return s;
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            Session["username"] = String.Empty;
            Response.Redirect("LogIn.aspx?");
        }

        private void DataExtract(MPOP_Dashboard.MPTBillCharge mptbillcharge)
        {
            try
            {
                mptbillcharge.MPTBillChargeID = null;
                mptbillcharge.MSISDN = txtMSISDNNo.Text.Trim();
                mptbillcharge.Amount = int.Parse(txtAmount.Text);
                mptbillcharge.Result = txtResult.Text.Trim();
                mptbillcharge.CreatedDate = null;
                mptbillcharge.CreatedBy = Session["username"].ToString();
            }
            catch (Exception ex)
            {
            }
        }
    }
}