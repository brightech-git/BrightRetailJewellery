using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
//using System.Net.Http;
//using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Data;
using Newtonsoft.Json;
using static CallApi.BounzInv;


namespace CallApi
{
    public class PushData
    {
        //HttpClient client;
        //HttpResponseMessage response;
        public string apiurl = "";
        public string tpkey = "";
        public string username = "";
        public string password = "";

        public string Callapi(string json)
        {
            var request = (HttpWebRequest)WebRequest.Create(apiurl);
            var data = Encoding.ASCII.GetBytes(json);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            else
            {
                return "Not Connect";
            }

        }


        public List<string> Callapijson(string json)
        {
            List<string> STR = new List<string>();
            var request = (HttpWebRequest)WebRequest.Create(apiurl);
            var data = Encoding.ASCII.GetBytes(json);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //var responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();
                //STR = new List<string>(responseString);

                var responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();
                List<string> ss = new List<string>();
                STR = JsonConvert.DeserializeObject<List<string>>(responseString);
                return STR;
            }
            else
            {
                STR.Add("Not Saved");
                STR.Add("Error from GST Portal");
                STR.Add("SERVER NOT CONNECT");
                return STR;
            }

        }



        public string CallapiGet(string json)
        {
            var request = (HttpWebRequest)WebRequest.Create(apiurl);
            var data = Encoding.ASCII.GetBytes(json);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            else
            {
                return "Not Connect";
            }

        }

        public string MISAPI(DataTable dtBill, DataTable dtAddress)
        {

            string json = "";
            var request = (HttpWebRequest)WebRequest.Create(apiurl);
            var data = Encoding.ASCII.GetBytes(json);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            else
            {
                return "Not Connect";
            }

        }

        public BOUNZRESULT CallapiBounz(string json)
        {
            BOUNZRESULT STR = new BOUNZRESULT();
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);

            var request = (HttpWebRequest)WebRequest.Create(apiurl);
            var data = Encoding.ASCII.GetBytes(json);
            request.Method = "POST";
            string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
            request.Headers.Add("Authorization", "Basic " + svcCredentials);
            request.Headers.Add("TP_APPLICATION_KEY", tpkey);
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();
                STR = JsonConvert.DeserializeObject<BOUNZRESULT>(responseString);

            }
            else
            {
                STR.status = false;
                STR.status_code = response.StatusCode.ToString();
                STR.message = response.StatusDescription.ToString();
            }

            return STR;
        }
        public BOUNZLOCKRESULT CallapiBounz1(string json)
        {

            BOUNZLOCKRESULT STR = new BOUNZLOCKRESULT();
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);

            var request = (HttpWebRequest)WebRequest.Create(apiurl);
            var data = Encoding.ASCII.GetBytes(json);
            request.Method = "POST";
            string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
            request.Headers.Add("Authorization", "Basic " + svcCredentials);
            request.Headers.Add("TP_APPLICATION_KEY", tpkey);
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();
                STR = JsonConvert.DeserializeObject<BOUNZLOCKRESULT>(responseString);

            }
            else
            {
                STR.status = false;
                STR.status_code = response.StatusCode.ToString();
                STR.message = response.StatusDescription.ToString();
            }

            return STR;
        }
        public BOUNZPROFILE CallapiBounz2(string json)
        {

            BOUNZPROFILE STR = new BOUNZPROFILE();
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);

            var request = (HttpWebRequest)WebRequest.Create(apiurl);
            var data = Encoding.ASCII.GetBytes(json);
            request.Method = "POST";
            string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
            request.Headers.Add("Authorization", "Basic " + svcCredentials);
            request.Headers.Add("TP_APPLICATION_KEY", tpkey);
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();
                STR = JsonConvert.DeserializeObject<BOUNZPROFILE>(responseString);

            }
            else
            {
                STR.status = false;
                STR.status_code = response.StatusCode.ToString();
                STR.message = response.StatusDescription.ToString();
            }

            return STR;
        }      
    }
}
