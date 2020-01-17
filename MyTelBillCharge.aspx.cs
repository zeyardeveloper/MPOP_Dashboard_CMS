using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using MPOP_Dashboard_CMS.MPOP_Dashboard;

namespace MPOP_Dashboard_CMS
{
    public partial class MyTelBillCharge : System.Web.UI.Page
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
                postData.Add("mpopRefTranId", "12345678910");
                postData.Add("msIsdn", txtMSISDNNo.Text.Trim());
                postData.Add("amount", txtAmount.Text.Trim());

                string url = "https://mpopapis.mytel.com.mm/mpop/modifyBalanceMPop";

                _ = PostFormUrlEncodedAsync(url, postData);
            }
        }

        public async Task PostFormUrlEncodedAsync(string url, IEnumerable<KeyValuePair<string, string>> postData)
        {
            try
            {
                using (var httpClient = new HttpClient(new HttpClientHandler { UseProxy = true }))
                {
                    using (var content = new FormUrlEncodedContent(postData))
                    {
                        content.Headers.Clear();

                        content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                        //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "apismpop:u6Q6qwe%EAu%nV8u");

                        var plainTextBytes = Encoding.ASCII.GetBytes("apismpop:u6Q6qwe%EAu%nV8u");
                        string val = System.Convert.ToBase64String(plainTextBytes);
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + val);

                        var method = new HttpMethod("POST");

                        var json = JsonConvert.SerializeObject(postData, Formatting.Indented);
                        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                        var rp = httpClient.PostAsync(url, stringContent).Result;
                        txtResult.Text = await rp.Content.ReadAsStringAsync();

                        lblMessage.Text = ConfigurationManager.AppSettings["Successful"];

                        MPOP_Dashboard.MyTelBillCharge mytelbillcharge = new MPOP_Dashboard.MyTelBillCharge();
                        DataExtract(mytelbillcharge);
                        db.sp_MPOP_Dashboard_MyTelBillCharge_CRUD(mytelbillcharge.MyTelBillChargeID, mytelbillcharge.MSISDN, mytelbillcharge.Amount, mytelbillcharge.Result, mytelbillcharge.CreatedDate, mytelbillcharge.CreatedBy);
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

        private void DataExtract(MPOP_Dashboard.MyTelBillCharge mytelbillcharge)
        {
            try
            {
                mytelbillcharge.MyTelBillChargeID = null;
                mytelbillcharge.MSISDN = txtMSISDNNo.Text.Trim();
                mytelbillcharge.Amount = int.Parse(txtAmount.Text);
                mytelbillcharge.Result = txtResult.Text.Trim();
                mytelbillcharge.CreatedDate = null;
                mytelbillcharge.CreatedBy = Session["username"].ToString();
            }
            catch (Exception ex)
            {
            }
        }
    }
}