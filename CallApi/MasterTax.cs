using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CallApi
{
    public class MasterTax
    {

        public sealed class GenericValidator<T>
        {
            /// <summary>
            /// Validates the specified entity.
            /// </summary>
            /// <param name="entity">The entity.</param>
            /// <returns></returns>
            public IList<ValidationResult> Validate(T entity)
            {
                var results = new List<ValidationResult>();
                var context = new ValidationContext(entity, null, null);
                Validator.TryValidateObject(entity, context, results, true);
                return results;
            }
        }

        public Boolean checkIsValid<T>(T chkCls)
        {
            GenericValidator<T> target = new GenericValidator<T>();
            if (target.Validate(chkCls).Count > 0)
            {
                IList<ValidationResult> Results = target.Validate(chkCls);
                String _Res = "";
                foreach (ValidationResult Result in Results)
                {
                    _Res += Environment.NewLine + Result.ErrorMessage;
                }
                MessageBox.Show(chkCls.GetType().Name.ToString() + Environment.NewLine + _Res);
                return false;
            }
            return true;
        }

        public Boolean ChkValidClass<T>(T _cls)
        {
            if (checkIsValid<T>(_cls) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<Invoice_Json> clsJon(DataRow drComp, DataRow drBuyer, DataTable dtItem)
        {
            List<Invoice_Json> lst = new List<Invoice_Json>();
            Invoice_Json inj = new Invoice_Json();
            inj.Version = "1.1";
            TranDtls _TranDtls = new TranDtls();
            if (drBuyer["EINVTYPE"].ToString() != "")
                _TranDtls.SupTyp = drBuyer["EINVTYPE"].ToString();
            else
                _TranDtls.SupTyp = "B2B";
            if (Convert.ToDecimal(dtItem.Rows[0]["IGST"].ToString()) > 0)
                _TranDtls.IgstOnIntra = "N";
            else
                _TranDtls.IgstOnIntra = "N";
            _TranDtls.RegRev = "N";
            _TranDtls.EcmGstin = null;
            inj.TranDtls = _TranDtls;



            DocDtls _DocDtls = new DocDtls();
            if (dtItem.Rows[0]["TRANTYPE"].ToString() == "SR")
            {
                _DocDtls.Typ = "CRN";
            }
            else if (dtItem.Rows[0]["TRANTYPE"].ToString() == "SA" || dtItem.Rows[0]["TRANTYPE"].ToString() == "OD" || dtItem.Rows[0]["TRANTYPE"].ToString() == "RD")
            {
                _DocDtls.Typ = "INV";
            }
            else if (dtItem.Rows[0]["TRANTYPE"].ToString() == "IIN")
            {
                _DocDtls.Typ = "INV";
            }
            else if (dtItem.Rows[0]["TRANTYPE"].ToString() == "IPU")
            {
                _DocDtls.Typ = "DBN";
            }

            _DocDtls.No = dtItem.Rows[0]["TRANNO"].ToString();
            _DocDtls.Dt = dtItem.Rows[0]["TRANDATE"].ToString();
            inj.DocDtls = _DocDtls;

            SellerDtls _SellerDtls = new SellerDtls();

            _SellerDtls.Gstin = drComp["GSTNO"].ToString();
            _SellerDtls.LglNm = drComp["COMPANYNAME"].ToString();
            _SellerDtls.TrdNm = null;
            _SellerDtls.Addr1 = drComp["ADDRESS1"].ToString();
            _SellerDtls.Addr2 = drComp["ADDRESS2"].ToString();
            _SellerDtls.Loc = drComp["ADDRESS3"].ToString();
            if (string.IsNullOrEmpty(drComp["AREACODE"].ToString()))
                _SellerDtls.Pin = Convert.ToInt32("0");
            else
                _SellerDtls.Pin = Convert.ToInt32(drComp["AREACODE"].ToString());
            _SellerDtls.Stcd = drComp["STATECODE"].ToString();
            _SellerDtls.Ph = drComp["PHONE"].ToString();
            _SellerDtls.Em = drComp["EMAIL"].ToString();
            if (checkIsValid<SellerDtls>(_SellerDtls) == false)
            {
                return lst;
            }
            inj.SellerDtls = _SellerDtls;

            BuyerDtls _BuyerDtls = new BuyerDtls();

            _BuyerDtls.Gstin = drBuyer["GSTNO"].ToString();
            _BuyerDtls.LglNm = drBuyer["PNAME"].ToString();
            _BuyerDtls.TrdNm = null;
            _BuyerDtls.Pos = drBuyer["STATECODE"].ToString();
            _BuyerDtls.Addr1 = drBuyer["ADDRESS1"].ToString();
            _BuyerDtls.Addr2 = drBuyer["ADDRESS2"].ToString();
            _BuyerDtls.Loc = drBuyer["ADDRESS3"].ToString();
            if (string.IsNullOrEmpty(drBuyer["PINCODE"].ToString()))
                _BuyerDtls.Pin = Convert.ToInt32("0");
            else
                _BuyerDtls.Pin = Convert.ToInt32(drBuyer["PINCODE"].ToString());
            _BuyerDtls.Stcd = drBuyer["STATECODE"].ToString();
            _BuyerDtls.Ph = drBuyer["PHONERES"].ToString();
            _BuyerDtls.Em = drBuyer["EMAIL"].ToString();
            if (checkIsValid<BuyerDtls>(_BuyerDtls) == false)
            {
                return lst;
            }
            inj.BuyerDtls = _BuyerDtls;

            inj.ShipDtls = null;

            ValDtls _ValDtls = new ValDtls();

            _ValDtls.AssVal = Convert.ToDouble(dtItem.Compute("SUM(AMOUNT)", null));
            _ValDtls.IgstVal = Convert.ToDouble(dtItem.Compute("SUM(IGST)", null));
            _ValDtls.CgstVal = Convert.ToDouble(dtItem.Compute("SUM(CGST)", null));
            _ValDtls.SgstVal = Convert.ToDouble(dtItem.Compute("SUM(SGST)", null));
            _ValDtls.CesVal = Convert.ToDouble(dtItem.Compute("SUM(CESS)", null));
            _ValDtls.StCesVal = 0;
            _ValDtls.Discount = 0;
            _ValDtls.OthChrg = 0;
            _ValDtls.RndOffAmt = 0;
            _ValDtls.TotInvVal = Convert.ToDouble(dtItem.Compute("SUM(TOTALAMT)", null));

            inj.ValDtls = _ValDtls;

            List<ItemList> ilst = new List<ItemList>();
            foreach (DataRow dr in dtItem.Rows)
            {
                ItemList IL = new ItemList();
                IL.SlNo = dr["SNO"].ToString();
                IL.PrdDesc = dr["ITEMNAME"].ToString();
                IL.IsServc = "N";
                IL.HsnCd = dr["HSN"].ToString();
                if (Convert.ToDecimal(dr["GRSWT"].ToString()) > 0)
                {
                    IL.Qty = Convert.ToDouble(dr["GRSWT"].ToString());
                    IL.Unit = "GMS";
                    IL.UnitPrice = Convert.ToDouble(dr["RATE"].ToString());
                }
                else
                {
                    IL.Qty = Convert.ToDouble(dr["PCS"].ToString());
                    IL.Unit = "PCS";
                    IL.UnitPrice = Convert.ToDouble(dr["AMOUNT"].ToString());
                }
                IL.TotAmt = Convert.ToDouble(dr["AMOUNT"].ToString());
                IL.Discount = 0;
                IL.PreTaxVal = 0;
                IL.AssAmt = Convert.ToDouble(dr["AMOUNT"].ToString());
                IL.GstRt = Convert.ToDouble(dr["SALESTAX"].ToString());
                IL.IgstAmt = Convert.ToDouble(dr["IGST"].ToString());
                IL.CgstAmt = Convert.ToDouble(dr["CGST"].ToString());
                IL.SgstAmt = Convert.ToDouble(dr["SGST"].ToString());
                IL.CesRt = Convert.ToDouble(dr["CESSTAX"].ToString());
                IL.CesAmt = Convert.ToDouble(dr["CESS"].ToString());
                IL.CesNonAdvlAmt = 0;
                IL.StateCesRt = 0;
                IL.StateCesAmt = 0;
                IL.StateCesNonAdvlAmt = 0;
                IL.OthChrg = 0;
                IL.TotItemVal = Convert.ToDouble(dr["TOTALAMT"].ToString());
                ilst.Add(IL);
            }
            inj.ItemList = ilst;
            lst.Add(inj);

            string pth = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\GST\\" + dtItem.Rows[0]["TRANNO"].ToString().Replace("/", "") + ".json";
            string JSONresult = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
            string fname = "\\" + dtItem.Rows[0]["TRANNO"].ToString().Replace("/", "");
            string path = @"" + pth;//' + "" + fname + ".json";
            using (var tw = new System.IO.StreamWriter(pth, false))
            {
                tw.WriteLine(JSONresult.ToString());
                tw.Close();
            }

            return lst;
        }


        public class InputClass
        {
            public DataRow drComp { get; set; }
            public DataRow drBuyer { get; set; }
            public DataTable dtItem { get; set; }
        }


        public class ErrorDetail
        {
            public string error_code { get; set; }
            public string error_message { get; set; }
            public string error_source { get; set; }
        }

        //public class GovtResponse
        //{
        //    public string Success { get; set; }
        //    public long AckNo { get; set; }
        //    public string AckDt { get; set; }
        //    public string Irn { get; set; }
        //    public string SignedInvoice { get; set; }
        //    public string SignedQRCode { get; set; }
        //    public string Status { get; set; }
        //    public long EwbNo { get; set; }
        //    public string EwbDt { get; set; }
        //    public string EwbValidTill { get; set; }
        //    public List<ErrorDetail> ErrorDetails { get; set; }
        //}


        public class data
        {
            public long AckNo { get; set; }
            public string AckDt { get; set; }
            public string Irn { get; set; }
            public string SignedInvoice { get; set; }
            public string SignedQRCode { get; set; }
            public string Status { get; set; }
            public object EwbNo { get; set; }
            public object EwbDt { get; set; }
            public object EwbValidTill { get; set; }
            public object Remarks { get; set; }
        }

        public class GovtRes
        {
            public data data { get; set; }
            public string status_cd { get; set; }
            public string status_desc { get; set; }
        }

        //public class GovtRes
        //{
        //    public string custom_fields { get; set; }
        //    public string document_status { get; set; }
        //    public GovtResponse govt_response { get; set; }
        //}

        public class masterTax
        {
            public Transaction transaction { get; set; }
        }

        public GovtRes clsJonMasterTax(List<InputClass> inputs, string PortalUrl, string usrname, string usrpassword, string clientid, string client_secret, string client_Ip)
        {
            List<masterTax> lst = new List<masterTax>();
            masterTax masterTax = new masterTax();

            GovtRes govt = new GovtRes();
            //List<Transaction> lst = new List<Transaction>();

            foreach (InputClass input in inputs)
            {
                Transaction inj = new Transaction();
                inj.Version = "1.1";
                TranDtls _TranDtls = new TranDtls();
                if (input.drBuyer["EINVTYPE"].ToString() != "")
                    _TranDtls.SupTyp = input.drBuyer["EINVTYPE"].ToString();
                else
                    _TranDtls.SupTyp = "B2B";
                ////_TranDtls.SupTyp = "B2B";
                if (Convert.ToDecimal(input.dtItem.Rows[0]["IGST"].ToString()) > 0)
                    _TranDtls.IgstOnIntra = "N";
                else
                    _TranDtls.IgstOnIntra = "N";
                _TranDtls.RegRev = "N";
                _TranDtls.EcmGstin = null;
                _TranDtls.TaxSch = "GST";
                inj.TranDtls = _TranDtls;


                DocDtls _DocDtls = new DocDtls();
                if (input.dtItem.Rows[0]["TRANTYPE"].ToString() == "SR")
                {
                    _DocDtls.Typ = "CRN";
                }
                else if (input.dtItem.Rows[0]["TRANTYPE"].ToString() == "SA" || input.dtItem.Rows[0]["TRANTYPE"].ToString() == "OD" || input.dtItem.Rows[0]["TRANTYPE"].ToString() == "RD")
                {
                    _DocDtls.Typ = "INV";
                }
                else if (input.dtItem.Rows[0]["TRANTYPE"].ToString() == "IIN")
                {
                    _DocDtls.Typ = "INV";
                }
                else if (input.dtItem.Rows[0]["TRANTYPE"].ToString() == "IPU")
                {
                    _DocDtls.Typ = "DBN";
                }

                _DocDtls.No = input.dtItem.Rows[0]["TRANNO"].ToString();
                _DocDtls.Dt = input.dtItem.Rows[0]["TRANDATE"].ToString();
                inj.DocDtls = _DocDtls;

                SellerDtls _SellerDtls = new SellerDtls();

                _SellerDtls.Gstin = input.drComp["GSTNO"].ToString();
                _SellerDtls.LglNm = input.drComp["COMPANYNAME"].ToString();
                _SellerDtls.TrdNm = null;
                _SellerDtls.Addr1 = input.drComp["ADDRESS1"].ToString();
                _SellerDtls.Addr2 = input.drComp["ADDRESS2"].ToString();
                _SellerDtls.Loc = input.drComp["ADDRESS3"].ToString();
                if (string.IsNullOrEmpty(input.drComp["AREACODE"].ToString()))
                    _SellerDtls.Pin = Convert.ToInt32("0");
                else
                    _SellerDtls.Pin = Convert.ToInt32(input.drComp["AREACODE"].ToString());
                _SellerDtls.Stcd = input.drComp["STATECODE"].ToString();
                _SellerDtls.Ph = input.drComp["PHONE"].ToString();
                _SellerDtls.Em = input.drComp["EMAIL"].ToString();
                if (checkIsValid<SellerDtls>(_SellerDtls) == false)
                {
                    return null;
                }
                inj.SellerDtls = _SellerDtls;

                BuyerDtls _BuyerDtls = new BuyerDtls();

                _BuyerDtls.Gstin = input.drBuyer["GSTNO"].ToString();
                _BuyerDtls.LglNm = input.drBuyer["PNAME"].ToString();
                _BuyerDtls.TrdNm = null;
                _BuyerDtls.Pos = input.drBuyer["STATECODE"].ToString();
                _BuyerDtls.Addr1 = input.drBuyer["ADDRESS1"].ToString();
                _BuyerDtls.Addr2 = input.drBuyer["ADDRESS2"].ToString();
                _BuyerDtls.Loc = input.drBuyer["ADDRESS3"].ToString();
                if (string.IsNullOrEmpty(input.drBuyer["PINCODE"].ToString()))
                    _BuyerDtls.Pin = Convert.ToInt32("0");
                else
                    _BuyerDtls.Pin = Convert.ToInt32(input.drBuyer["PINCODE"].ToString());
                _BuyerDtls.Stcd = input.drBuyer["STATECODE"].ToString();
                _BuyerDtls.Ph = input.drBuyer["PHONERES"].ToString();
                _BuyerDtls.Em = input.drBuyer["EMAIL"].ToString();
                if (checkIsValid<BuyerDtls>(_BuyerDtls) == false)
                {
                    return null;
                }
                inj.BuyerDtls = _BuyerDtls;

                inj.ShipDtls = null;

                ValDtls _ValDtls = new ValDtls();

                _ValDtls.AssVal = Convert.ToDouble(input.dtItem.Compute("SUM(AMOUNT)", null));
                _ValDtls.IgstVal = Convert.ToDouble(input.dtItem.Compute("SUM(IGST)", null));
                _ValDtls.CgstVal = Convert.ToDouble(input.dtItem.Compute("SUM(CGST)", null));
                _ValDtls.SgstVal = Convert.ToDouble(input.dtItem.Compute("SUM(SGST)", null));
                _ValDtls.CesVal = Convert.ToDouble(input.dtItem.Compute("SUM(CESS)", null));
                _ValDtls.StCesVal = 0;
                _ValDtls.Discount = 0;
                _ValDtls.OthChrg = Convert.ToDouble(input.dtItem.Rows[0]["TCS"], null); //(Double)input.dtItem.Rows[0]["TCS"];
                _ValDtls.RndOffAmt = 0;
                _ValDtls.TotInvVal = Convert.ToDouble(Convert.ToDecimal(input.dtItem.Compute("SUM(TOTALAMT)", null)) + (Decimal)input.dtItem.Rows[0]["TCS"]);

                inj.ValDtls = _ValDtls;

                CustomFields customFields = new CustomFields();
                customFields.customfieldLable1 = input.dtItem.Rows[0]["BATCHNO"].ToString();
                customFields.customfieldLable2 = input.dtItem.Rows[0]["TRANTYPE"].ToString();
                customFields.customfieldLable3 = input.dtItem.Rows[0]["TRANNO"].ToString();

                inj.custom_fields = customFields;

                int cnt = 0;
                List<ItemList> ilst = new List<ItemList>();
                foreach (DataRow dr in input.dtItem.Rows)
                {
                    ItemList IL = new ItemList();
                    IL.SlNo = dr["SNO"].ToString();
                    IL.PrdDesc = dr["ITEMNAME"].ToString();
                    if (dr["ISSERVICE"].ToString() == "Y")
                        IL.IsServc = "Y";
                    else
                        IL.IsServc = "N";
                    //IL.IsServc = "N";
                    IL.HsnCd = dr["HSN"].ToString();
                    if (Convert.ToDecimal(dr["GRSWT"].ToString()) > 0)
                    {
                        IL.Qty = Convert.ToDouble(dr["GRSWT"].ToString());
                        IL.Unit = "GMS";
                        IL.UnitPrice = Convert.ToDouble(dr["RATE"].ToString());
                    }
                    else
                    {
                        IL.Qty = Convert.ToDouble(dr["PCS"].ToString());
                        IL.Unit = "PCS";
                        IL.UnitPrice = Convert.ToDouble(dr["AMOUNT"].ToString());
                    }

                    IL.Discount = 0;
                    IL.PreTaxVal = 0;
                    IL.AssAmt = Convert.ToDouble(dr["AMOUNT"].ToString());
                    IL.GstRt = Convert.ToDouble(dr["SALESTAX"].ToString());
                    IL.IgstAmt = Convert.ToDouble(dr["IGST"].ToString());
                    IL.CgstAmt = Convert.ToDouble(dr["CGST"].ToString());
                    IL.SgstAmt = Convert.ToDouble(dr["SGST"].ToString());
                    IL.CesRt = Convert.ToDouble(dr["CESSTAX"].ToString());
                    IL.CesAmt = Convert.ToDouble(dr["CESS"].ToString());
                    IL.CesNonAdvlAmt = 0;
                    IL.StateCesRt = 0;
                    IL.StateCesAmt = 0;
                    IL.StateCesNonAdvlAmt = 0;
                    if (cnt == 0)
                    {
                        //IL.TotAmt = Convert.ToDecimal(Convert.ToDecimal(dr["AMOUNT"].ToString()) + Convert.ToDecimal(dr["TCS"].ToString()));                        
                        //IL.OthChrg = Convert.ToDecimal(dr["TCS"].ToString());
                        //IL.TotItemVal = Convert.ToDecimal(Convert.ToDecimal(dr["TOTALAMT"].ToString()) + Convert.ToDecimal(dr["TCS"].ToString()));
                        IL.TotAmt = Convert.ToDouble(dr["AMOUNT"].ToString());
                        IL.OthChrg = 0;
                        IL.TotItemVal = Convert.ToDouble(Convert.ToDecimal(dr["TOTALAMT"].ToString()));
                    }
                    else
                    {
                        IL.TotAmt = Convert.ToDouble(dr["AMOUNT"].ToString());
                        IL.OthChrg = 0;
                        IL.TotItemVal = Convert.ToDouble(Convert.ToDecimal(dr["TOTALAMT"].ToString()));
                    }
                    ilst.Add(IL);
                }
                inj.ItemList = ilst;
                masterTax.transaction = inj;
                lst.Add(masterTax);
                cnt += 1;
            }

            //auth token
            HttpClient client1 = new HttpClient();
            client1.DefaultRequestHeaders.Add("username", usrname);
            client1.DefaultRequestHeaders.Add("password", usrpassword);
            client1.DefaultRequestHeaders.Add("ip_address", client_Ip);
            client1.DefaultRequestHeaders.Add("client_id", clientid);
            client1.DefaultRequestHeaders.Add("client_secret", client_secret);
            client1.DefaultRequestHeaders.Add("gstin", lst[0].transaction.SellerDtls.Gstin.ToString());

            //response = await client1.GetAsync(PortalUrl + "einvoice/authenticate");
            string AuthToken = string.Empty;
            var res1 = client1.GetAsync(PortalUrl + "einvoice/authenticate?email=" + lst[0].transaction.SellerDtls.Em.ToString()).Result;
            if (res1.IsSuccessStatusCode)
            {
                var cnt = res1.Content.ReadAsStringAsync().Result;
                var auth = JObject.Parse(cnt);
                if (auth["status_cd"].ToString().ToUpper() == "SUCESS" || auth["status_cd"].ToString() == "1")
                {
                    AuthToken = auth["data"]["AuthToken"].ToString();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            //END auth token

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("username", usrname);
            client.DefaultRequestHeaders.Add("ip_address", client_Ip);
            client.DefaultRequestHeaders.Add("client_id", clientid);
            client.DefaultRequestHeaders.Add("client_secret", client_secret);
            client.DefaultRequestHeaders.Add("auth-token", AuthToken);
            client.DefaultRequestHeaders.Add("gstin", lst[0].transaction.SellerDtls.Gstin.ToString());
            string TJson = JArray.FromObject(lst).ToString();

            Transaction tran = new Transaction();
            tran = lst[0].transaction;
            var json = JsonConvert.SerializeObject(tran);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var res = client.PostAsync(PortalUrl + "einvoice/type/GENERATE/version/V1_03?email=" + lst[0].transaction.SellerDtls.Em.ToString(), stringContent).Result;
            dynamic test = new JObject();
            if (res.IsSuccessStatusCode)
            {
                var cnt = res.Content.ReadAsStringAsync().Result;
                var auth = JObject.Parse(cnt);
                String result1 = Convert.ToString(cnt);
                if (auth["status_cd"].ToString().ToUpper() == "SUCESS" || auth["status_cd"].ToString() == "1")
                {
                    govt = JsonConvert.DeserializeObject<GovtRes>(result1);
                }
                else
                {
                    if (auth["status_cd"].ToString() == "0")
                    {
                        govt = JsonConvert.DeserializeObject<GovtRes>(result1);
                    }
                    else
                    {
                        return null;
                    }
                }

            }
            if (govt != null)
            {
                return govt;
            }
            else
            {
                return null;
            }
        }

        public class masterTaxCancel
        {
            public string irn { get; set; }
            public string CnlRsn { get; set; }
            public string CnlRem { get; set; }
        }

        public GovtRes clsJonmasterTax_Cancel(string irn, string CnlRsn, string CnlRem, string GSTIN, string PortalUrl, string usrname, string usrpassword, string client_Ip, string clientid, string client_secret, string compmailid)
        {
            GovtRes govt = new GovtRes();
            masterTaxCancel masterTaxCancel = new masterTaxCancel();
            masterTaxCancel.irn = irn;
            masterTaxCancel.CnlRsn = CnlRsn;
            masterTaxCancel.CnlRem = CnlRem;
            List<masterTaxCancel> masterTaxCancels = new List<masterTaxCancel>();
            masterTaxCancels.Add(masterTaxCancel);


            //auth token
            HttpClient client1 = new HttpClient();
            client1.DefaultRequestHeaders.Add("username", usrname);
            client1.DefaultRequestHeaders.Add("password", usrpassword);
            client1.DefaultRequestHeaders.Add("ip_address", client_Ip);
            client1.DefaultRequestHeaders.Add("client_id", clientid);
            client1.DefaultRequestHeaders.Add("client_secret", client_secret);
            client1.DefaultRequestHeaders.Add("gstin", GSTIN);
            string AuthToken = string.Empty;
            var res1 = client1.GetAsync(PortalUrl + "einvoice/authenticate?email=" + compmailid).Result;
            if (res1.IsSuccessStatusCode)
            {
                var cnt = res1.Content.ReadAsStringAsync().Result;
                var auth = JObject.Parse(cnt);
                //AuthToken = auth["data"]["AuthToken"].ToString();
                if (auth["status_cd"].ToString().ToUpper() == "SUCESS" || auth["status_cd"].ToString() == "1")
                {
                    AuthToken = auth["data"]["AuthToken"].ToString();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            //END auth token

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("username", usrname);
            client.DefaultRequestHeaders.Add("ip_address", client_Ip);
            client.DefaultRequestHeaders.Add("client_id", clientid);
            client.DefaultRequestHeaders.Add("client_secret", client_secret);
            client.DefaultRequestHeaders.Add("auth-token", AuthToken);
            client.DefaultRequestHeaders.Add("gstin", GSTIN);
            string TJson = JArray.FromObject(masterTaxCancels).ToString();

            var json = JsonConvert.SerializeObject(masterTaxCancels[0]);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var res = client.PostAsync(PortalUrl + "einvoice/type/CANCEL/version/V1_03?email=" + compmailid, stringContent).Result;
            dynamic test = new JObject();
            if (res.IsSuccessStatusCode)
            {
                //var cnt = res.Content.ReadAsStringAsync().Result;
                //govt = JsonConvert.DeserializeObject<List<GovtRes>>(cnt);
                var cnt = res.Content.ReadAsStringAsync().Result;
                var auth = JObject.Parse(cnt);
                String result1 = Convert.ToString(cnt);
                if (auth["status_cd"].ToString().ToUpper() == "SUCESS" || auth["status_cd"].ToString() == "1")
                {
                    govt = JsonConvert.DeserializeObject<GovtRes>(result1);
                }
                else
                {
                    if (auth["status_cd"].ToString() == "0")
                    {
                        govt = JsonConvert.DeserializeObject<GovtRes>(result1);
                    }
                    else
                    {
                        return null;
                    }
                }
            }


            if (govt != null)
            {
                return govt;
            }
            else
            {
                return null;
            }
        }


        public List<Invoice_Json> clsJonMulti(List<InputClass> inputs)
        {
            List<Invoice_Json> lst = new List<Invoice_Json>();

            foreach (InputClass input in inputs)
            {
                Invoice_Json inj = new Invoice_Json();
                inj.Version = "1.1";
                TranDtls _TranDtls = new TranDtls();

                _TranDtls.SupTyp = "B2B";
                if (Convert.ToDecimal(input.dtItem.Rows[0]["IGST"].ToString()) > 0)
                    _TranDtls.IgstOnIntra = "N";
                else
                    _TranDtls.IgstOnIntra = "N";
                _TranDtls.RegRev = "N";
                _TranDtls.EcmGstin = null;
                inj.TranDtls = _TranDtls;



                DocDtls _DocDtls = new DocDtls();
                if (input.dtItem.Rows[0]["TRANTYPE"].ToString() == "SR")
                {
                    _DocDtls.Typ = "CRN";
                }
                else if (input.dtItem.Rows[0]["TRANTYPE"].ToString() == "SA" || input.dtItem.Rows[0]["TRANTYPE"].ToString() == "OD" || input.dtItem.Rows[0]["TRANTYPE"].ToString() == "RD")
                {
                    _DocDtls.Typ = "INV";
                }
                else if (input.dtItem.Rows[0]["TRANTYPE"].ToString() == "IIN")
                {
                    _DocDtls.Typ = "INV";
                }
                else if (input.dtItem.Rows[0]["TRANTYPE"].ToString() == "IPU")
                {
                    _DocDtls.Typ = "DBN";
                }

                _DocDtls.No = input.dtItem.Rows[0]["TRANNO"].ToString();
                _DocDtls.Dt = input.dtItem.Rows[0]["TRANDATE"].ToString();
                inj.DocDtls = _DocDtls;

                SellerDtls _SellerDtls = new SellerDtls();

                _SellerDtls.Gstin = input.drComp["GSTNO"].ToString();
                _SellerDtls.LglNm = input.drComp["COMPANYNAME"].ToString();
                _SellerDtls.TrdNm = null;
                _SellerDtls.Addr1 = input.drComp["ADDRESS1"].ToString();
                _SellerDtls.Addr2 = input.drComp["ADDRESS2"].ToString();
                _SellerDtls.Loc = input.drComp["ADDRESS3"].ToString();
                if (string.IsNullOrEmpty(input.drComp["AREACODE"].ToString()))
                    _SellerDtls.Pin = Convert.ToInt32("0");
                else
                    _SellerDtls.Pin = Convert.ToInt32(input.drComp["AREACODE"].ToString());
                _SellerDtls.Stcd = input.drComp["STATECODE"].ToString();
                _SellerDtls.Ph = input.drComp["PHONE"].ToString();
                _SellerDtls.Em = input.drComp["EMAIL"].ToString();
                if (checkIsValid<SellerDtls>(_SellerDtls) == false)
                {
                    return lst;
                }
                inj.SellerDtls = _SellerDtls;

                BuyerDtls _BuyerDtls = new BuyerDtls();

                _BuyerDtls.Gstin = input.drBuyer["GSTNO"].ToString();
                _BuyerDtls.LglNm = input.drBuyer["PNAME"].ToString();
                _BuyerDtls.TrdNm = null;
                _BuyerDtls.Pos = input.drBuyer["STATECODE"].ToString();
                _BuyerDtls.Addr1 = input.drBuyer["ADDRESS1"].ToString();
                _BuyerDtls.Addr2 = input.drBuyer["ADDRESS2"].ToString();
                _BuyerDtls.Loc = input.drBuyer["ADDRESS3"].ToString();
                if (string.IsNullOrEmpty(input.drBuyer["PINCODE"].ToString()))
                    _BuyerDtls.Pin = Convert.ToInt32("0");
                else
                    _BuyerDtls.Pin = Convert.ToInt32(input.drBuyer["PINCODE"].ToString());
                _BuyerDtls.Stcd = input.drBuyer["STATECODE"].ToString();
                _BuyerDtls.Ph = input.drBuyer["PHONERES"].ToString();
                _BuyerDtls.Em = input.drBuyer["EMAIL"].ToString();
                if (checkIsValid<BuyerDtls>(_BuyerDtls) == false)
                {
                    return lst;
                }
                inj.BuyerDtls = _BuyerDtls;

                inj.ShipDtls = null;

                ValDtls _ValDtls = new ValDtls();

                _ValDtls.AssVal = Convert.ToDouble(input.dtItem.Compute("SUM(AMOUNT)", null));
                _ValDtls.IgstVal = Convert.ToDouble(input.dtItem.Compute("SUM(IGST)", null));
                _ValDtls.CgstVal = Convert.ToDouble(input.dtItem.Compute("SUM(CGST)", null));
                _ValDtls.SgstVal = Convert.ToDouble(input.dtItem.Compute("SUM(SGST)", null));
                _ValDtls.CesVal = Convert.ToDouble(input.dtItem.Compute("SUM(CESS)", null));
                _ValDtls.StCesVal = 0;
                _ValDtls.Discount = 0;
                _ValDtls.OthChrg = Convert.ToDouble(input.dtItem.Compute("SUM(TCS)", null));
                _ValDtls.RndOffAmt = 0;
                _ValDtls.TotInvVal = Convert.ToDouble(input.dtItem.Compute("SUM(TOTALAMT)", null)) + Convert.ToDouble(input.dtItem.Compute("SUM(TCS)", null));

                inj.ValDtls = _ValDtls;

                List<ItemList> ilst = new List<ItemList>();
                foreach (DataRow dr in input.dtItem.Rows)
                {
                    ItemList IL = new ItemList();
                    IL.SlNo = dr["SNO"].ToString();
                    IL.PrdDesc = dr["ITEMNAME"].ToString();
                    if (dr["ISSERVICE"].ToString() == "Y")
                        IL.IsServc = "Y";
                    else
                        IL.IsServc = "N";
                    //IL.IsServc = "N";
                    IL.HsnCd = dr["HSN"].ToString();
                    if (Convert.ToDecimal(dr["GRSWT"].ToString()) > 0)
                    {
                        IL.Qty = Convert.ToDouble(dr["GRSWT"].ToString());
                        IL.Unit = "GMS";
                        IL.UnitPrice = Convert.ToDouble(dr["RATE"].ToString());
                    }
                    else
                    {
                        IL.Qty = Convert.ToDouble(dr["PCS"].ToString());
                        IL.Unit = "PCS";
                        IL.UnitPrice = Convert.ToDouble(dr["AMOUNT"].ToString());
                    }
                    IL.TotAmt = Convert.ToDouble(dr["AMOUNT"].ToString());
                    IL.Discount = 0;
                    IL.PreTaxVal = 0;
                    IL.AssAmt = Convert.ToDouble(dr["AMOUNT"].ToString());
                    IL.GstRt = Convert.ToDouble(dr["SALESTAX"].ToString());
                    IL.IgstAmt = Convert.ToDouble(dr["IGST"].ToString());
                    IL.CgstAmt = Convert.ToDouble(dr["CGST"].ToString());
                    IL.SgstAmt = Convert.ToDouble(dr["SGST"].ToString());
                    IL.CesRt = Convert.ToDouble(dr["CESSTAX"].ToString());
                    IL.CesAmt = Convert.ToDouble(dr["CESS"].ToString());
                    IL.CesNonAdvlAmt = 0;
                    IL.StateCesRt = 0;
                    IL.StateCesAmt = 0;
                    IL.StateCesNonAdvlAmt = 0;
                    IL.OthChrg = 0;
                    IL.TotItemVal = Convert.ToDouble(dr["TOTALAMT"].ToString());
                    ilst.Add(IL);
                }
                inj.ItemList = ilst;
                lst.Add(inj);
            }

            string strName = DateTime.Now.ToString("yyyyMMddHHmmss");
            string pth = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\GST\\" + strName.ToString().Replace("/", "") + ".json";
            string JSONresult = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
            string fname = "\\" + strName.ToString().Replace("/", "");
            string path = @"" + pth;//' + "" + fname + ".json";
            using (var tw = new System.IO.StreamWriter(pth, false))
            {
                tw.WriteLine(JSONresult.ToString());
                tw.Close();
            }
            return lst;
        }


        public class AddlDocDtl
        {
            public string Docs { get; set; }
            public string Info { get; set; }
            public string Url { get; set; }
        }

        public class BuyerDtls
        {
            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Em { get; set; }
            public string Gstin { get; set; }
            public string LglNm { get; set; }
            public string Loc { get; set; }
            public string Ph { get; set; }
            public int Pin { get; set; }
            public string Pos { get; set; }
            public string Stcd { get; set; }
            public string TrdNm { get; set; }
        }

        public class DispDtls
        {
            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Loc { get; set; }
            public string Nm { get; set; }
            public int Pin { get; set; }
            public string Stcd { get; set; }
        }

        public class DocDtls
        {
            public string Dt { get; set; }
            public string No { get; set; }
            public string Typ { get; set; }
        }

        public class EwbDtls
        {
            public int Distance { get; set; }
            public string TransdocDt { get; set; }
            public string Transdocno { get; set; }
            public string Transid { get; set; }
            public string TransMode { get; set; }
            public string Transname { get; set; }
            public string Vehno { get; set; }
            public string Vehtype { get; set; }
        }

        public class ExpDtls
        {
            public string CntCode { get; set; }
            public string ForCur { get; set; }
            public string Port { get; set; }
            public string RefClm { get; set; }
            public string ShipBDt { get; set; }
            public string ShipBNo { get; set; }
        }

        public class AttribDtl
        {
            public string Nm { get; set; }
            public string Val { get; set; }
        }

        public class BchDtls
        {
            public string Expdt { get; set; }
            public string Nm { get; set; }
            public string wrDt { get; set; }
        }

        public class ItemList
        {
            public double AssAmt { get; set; }
            public List<AttribDtl> AttribDtls { get; set; }
            public string Barcde { get; set; }
            public BchDtls BchDtls { get; set; }
            public double CesAmt { get; set; }
            public int CesNonAdvlAmt { get; set; }
            public double CesRt { get; set; }
            public double CgstAmt { get; set; }
            public int Discount { get; set; }
            public int FreeQty { get; set; }
            public double GstRt { get; set; }
            public string HsnCd { get; set; }
            public double IgstAmt { get; set; }
            public string IsServc { get; set; }
            public string OrdLineRef { get; set; }
            public string OrgCntry { get; set; }
            public int OthChrg { get; set; }
            public string PrdDesc { get; set; }
            public string PrdSlNo { get; set; }
            public int PreTaxVal { get; set; }
            public double Qty { get; set; }
            public double SgstAmt { get; set; }
            public string SlNo { get; set; }
            public double StateCesAmt { get; set; }
            public int StateCesNonAdvlAmt { get; set; }
            public int StateCesRt { get; set; }
            public double TotAmt { get; set; }
            public double TotItemVal { get; set; }
            public string Unit { get; set; }
            public double UnitPrice { get; set; }
        }

        public class PayDtls
        {
            public string Accdet { get; set; }
            public int Crday { get; set; }
            public string Crtrn { get; set; }
            public string Dirdr { get; set; }
            public string Fininsbr { get; set; }
            public string Mode { get; set; }
            public string Nm { get; set; }
            public int Paidamt { get; set; }
            public string Payinstr { get; set; }
            public int Paymtdue { get; set; }
            public string Payterm { get; set; }
        }

        public class ContrDtl
        {
            public string Contrrefr { get; set; }
            public string Extrefr { get; set; }
            public string PoRefDt { get; set; }
            public string Porefr { get; set; }
            public string Projrefr { get; set; }
            public string RecAdvDt { get; set; }
            public string RecAdvRefr { get; set; }
            public string Tendrefr { get; set; }
        }

        public class DocPerdDtls
        {
            public string InvEndDt { get; set; }
            public string InvStDt { get; set; }
        }

        public class PrecDocDtl
        {
            public string InvDt { get; set; }
            public string InvNo { get; set; }
            public string OthRefNo { get; set; }
        }

        public class RefDtls
        {
            public List<ContrDtl> ContrDtls { get; set; }
            public DocPerdDtls DocPerdDtls { get; set; }
            public string InvRm { get; set; }
            public List<PrecDocDtl> PrecDocDtls { get; set; }
        }

        public class SellerDtls
        {
            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Em { get; set; }
            public string Gstin { get; set; }
            public string LglNm { get; set; }
            public string Loc { get; set; }
            public string Ph { get; set; }
            public int Pin { get; set; }
            public string Stcd { get; set; }
            public string TrdNm { get; set; }
        }

        public class ShipDtls
        {
            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Gstin { get; set; }
            public string LglNm { get; set; }
            public string Loc { get; set; }
            public int Pin { get; set; }
            public string Stcd { get; set; }
            public string TrdNm { get; set; }
        }

        public class TranDtls
        {
            public object EcmGstin { get; set; }
            public string IgstOnIntra { get; set; }
            public string RegRev { get; set; }
            public string SupTyp { get; set; }
            public string TaxSch { get; set; }
        }

        public class ValDtls
        {
            public double AssVal { get; set; }
            public double CesVal { get; set; }
            public double CgstVal { get; set; }
            public double Discount { get; set; }
            public double IgstVal { get; set; }
            public double OthChrg { get; set; }
            public double RndOffAmt { get; set; }
            public double SgstVal { get; set; }
            public double StCesVal { get; set; }
            public double TotInvVal { get; set; }
            public double TotInvValFc { get; set; }
        }

        public class CustomFields
        {
            public string customfieldLable1 { get; set; }
            public string customfieldLable2 { get; set; }
            public string customfieldLable3 { get; set; }
        }

        public class Invoice_Json
        {
            public List<AddlDocDtl> AddlDocDtls { get; set; }
            public BuyerDtls BuyerDtls { get; set; }
            public DispDtls DispDtls { get; set; }
            public DocDtls DocDtls { get; set; }
            public EwbDtls EwbDtls { get; set; }
            public ExpDtls ExpDtls { get; set; }
            public List<ItemList> ItemList { get; set; }
            public PayDtls PayDtls { get; set; }
            public RefDtls RefDtls { get; set; }
            public SellerDtls SellerDtls { get; set; }
            public ShipDtls ShipDtls { get; set; }
            public TranDtls TranDtls { get; set; }
            public ValDtls ValDtls { get; set; }
            public string Version { get; set; }
        }

        public class Transaction
        {
            public List<AddlDocDtl> AddlDocDtls { get; set; }
            public BuyerDtls BuyerDtls { get; set; }
            public DispDtls DispDtls { get; set; }
            public DocDtls DocDtls { get; set; }
            public EwbDtls EwbDtls { get; set; }
            public ExpDtls ExpDtls { get; set; }
            public List<ItemList> ItemList { get; set; }
            public PayDtls PayDtls { get; set; }
            public RefDtls RefDtls { get; set; }
            public SellerDtls SellerDtls { get; set; }
            public ShipDtls ShipDtls { get; set; }
            public TranDtls TranDtls { get; set; }
            public ValDtls ValDtls { get; set; }
            public string Version { get; set; }

            public CustomFields custom_fields { get; set; }
        }


    }
}
