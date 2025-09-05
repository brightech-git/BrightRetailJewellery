using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;

namespace CallApi
{
    public class Offline
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

            _TranDtls.SupTyp = "B2B";
            if (Convert.ToDecimal(dtItem.Rows[0]["IGST"].ToString()) > 0)
                _TranDtls.IgstOnIntra = "N";
            else
                _TranDtls.IgstOnIntra = "N";
            _TranDtls.RegRev = "N";
            _TranDtls.EcmGstin = null;
            inj.TranDtls = _TranDtls;



            DocDtls _DocDtls = new DocDtls();
            if (dtItem.Rows[0]["TRANTYPE"].ToString() == "SR" || dtItem.Rows[0]["TRANTYPE"].ToString() == "CN")
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
            else if (dtItem.Rows[0]["TRANTYPE"].ToString() == "IPU" || dtItem.Rows[0]["TRANTYPE"].ToString() == "DN")
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

            _ValDtls.AssVal = Convert.ToDecimal(dtItem.Compute("SUM(AMOUNT)", null));
            _ValDtls.IgstVal = Convert.ToDecimal(dtItem.Compute("SUM(IGST)", null));
            _ValDtls.CgstVal = Convert.ToDecimal(dtItem.Compute("SUM(CGST)", null));
            _ValDtls.SgstVal = Convert.ToDecimal(dtItem.Compute("SUM(SGST)", null));
            _ValDtls.CesVal = Convert.ToDecimal(dtItem.Compute("SUM(CESS)", null));
            _ValDtls.StCesVal = 0;
            _ValDtls.Discount = 0;
            _ValDtls.OthChrg = 0;
            _ValDtls.RndOffAmt = 0;
            _ValDtls.TotInvVal = Convert.ToDecimal(dtItem.Compute("SUM(TOTALAMT)", null));

            inj.ValDtls = _ValDtls;

            List<ItemList> ilst = new List<ItemList>();
            foreach (DataRow dr in dtItem.Rows)
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
                    IL.Qty = Convert.ToDecimal(dr["GRSWT"].ToString());
                    IL.Unit = "GMS";
                    IL.UnitPrice = Convert.ToDecimal(dr["RATE"].ToString());
                }
                else
                {
                    IL.Qty = Convert.ToDecimal(dr["PCS"].ToString());
                    IL.Unit = "PCS";
                    IL.UnitPrice = Convert.ToDecimal(dr["AMOUNT"].ToString());
                }
                IL.TotAmt = Convert.ToDecimal(dr["AMOUNT"].ToString());
                IL.Discount = 0;
                IL.PreTaxVal = 0;
                IL.AssAmt = Convert.ToDecimal(dr["AMOUNT"].ToString());
                IL.GstRt = Convert.ToDecimal(dr["SALESTAX"].ToString());
                IL.IgstAmt = Convert.ToDecimal(dr["IGST"].ToString());
                IL.CgstAmt = Convert.ToDecimal(dr["CGST"].ToString());
                IL.SgstAmt = Convert.ToDecimal(dr["SGST"].ToString());
                IL.CesRt = Convert.ToDecimal(dr["CESSTAX"].ToString());
                IL.CesAmt = Convert.ToDecimal(dr["CESS"].ToString());
                IL.CesNonAdvlAmt = 0;
                IL.StateCesRt = 0;
                IL.StateCesAmt = 0;
                IL.StateCesNonAdvlAmt = 0;
                IL.OthChrg = 0;
                IL.TotItemVal = Convert.ToDecimal(dr["TOTALAMT"].ToString());
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

        public class GovtResponse
        {
            public string Success { get; set; }
            public long AckNo { get; set; }
            public string AckDt { get; set; }
            public string Irn { get; set; }
            public string SignedInvoice { get; set; }
            public string SignedQRCode { get; set; }
            public string Status { get; set; }
            public long EwbNo { get; set; }
            public string EwbDt { get; set; }
            public string EwbValidTill { get; set; }
            public List<ErrorDetail> ErrorDetails { get; set; }
        }

        public class GovtRes
        {
            public string custom_fields { get; set; }
            public string document_status { get; set; }
            public GovtResponse govt_response { get; set; }
        }

        public class ClearTax
        {
            public Transaction transaction { get; set; }
        }

        public GovtRes clsJonClearTax(List<InputClass> inputs, string OwnerId, string PortalUrl, string AuthToken)
        {
            List<ClearTax> lst = new List<ClearTax>();
            ClearTax clearTax = new ClearTax();

            List<GovtRes> govt = new List<GovtRes>();
            //List<Transaction> lst = new List<Transaction>();

            foreach (InputClass input in inputs)
            {
                Transaction inj = new Transaction();
                inj.Version = "1.1";
                TranDtls _TranDtls = new TranDtls();

                _TranDtls.SupTyp = "B2B";
                if (Convert.ToDecimal(input.dtItem.Rows[0]["IGST"].ToString()) > 0)
                    _TranDtls.IgstOnIntra = "N";
                else
                    _TranDtls.IgstOnIntra = "N";
                _TranDtls.RegRev = "N";
                _TranDtls.EcmGstin = null;
                _TranDtls.TaxSch = "GST";
                inj.TranDtls = _TranDtls;



                DocDtls _DocDtls = new DocDtls();
                if (input.dtItem.Rows[0]["TRANTYPE"].ToString() == "SR" || input.dtItem.Rows[0]["TRANTYPE"].ToString() == "CN")
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
                else if (input.dtItem.Rows[0]["TRANTYPE"].ToString() == "IPU" || input.dtItem.Rows[0]["TRANTYPE"].ToString() == "DN")
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

                _ValDtls.AssVal = Convert.ToDecimal(input.dtItem.Compute("SUM(AMOUNT)", null));
                _ValDtls.IgstVal = Convert.ToDecimal(input.dtItem.Compute("SUM(IGST)", null));
                _ValDtls.CgstVal = Convert.ToDecimal(input.dtItem.Compute("SUM(CGST)", null));
                _ValDtls.SgstVal = Convert.ToDecimal(input.dtItem.Compute("SUM(SGST)", null));
                _ValDtls.CesVal = Convert.ToDecimal(input.dtItem.Compute("SUM(CESS)", null));
                _ValDtls.StCesVal = 0;
                _ValDtls.Discount = 0;
                _ValDtls.OthChrg = (Decimal)input.dtItem.Rows[0]["TCS"];
                _ValDtls.RndOffAmt = 0;
                _ValDtls.TotInvVal = Convert.ToDecimal(Convert.ToDecimal(input.dtItem.Compute("SUM(TOTALAMT)", null)) + (Decimal)input.dtItem.Rows[0]["TCS"]);

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
                    IL.IsServc = "N";
                    IL.HsnCd = dr["HSN"].ToString();
                    if (Convert.ToDecimal(dr["GRSWT"].ToString()) > 0)
                    {
                        IL.Qty = Convert.ToDecimal(dr["GRSWT"].ToString());
                        IL.Unit = "GMS";
                        IL.UnitPrice = Convert.ToDecimal(dr["RATE"].ToString());
                    }
                    else
                    {
                        IL.Qty = Convert.ToDecimal(dr["PCS"].ToString());
                        IL.Unit = "PCS";
                        IL.UnitPrice = Convert.ToDecimal(dr["AMOUNT"].ToString());
                    }

                    IL.Discount = 0;
                    IL.PreTaxVal = 0;
                    IL.AssAmt = Convert.ToDecimal(dr["AMOUNT"].ToString());
                    IL.GstRt = Convert.ToDecimal(dr["SALESTAX"].ToString());
                    IL.IgstAmt = Convert.ToDecimal(dr["IGST"].ToString());
                    IL.CgstAmt = Convert.ToDecimal(dr["CGST"].ToString());
                    IL.SgstAmt = Convert.ToDecimal(dr["SGST"].ToString());
                    IL.CesRt = Convert.ToDecimal(dr["CESSTAX"].ToString());
                    IL.CesAmt = Convert.ToDecimal(dr["CESS"].ToString());
                    IL.CesNonAdvlAmt = 0;
                    IL.StateCesRt = 0;
                    IL.StateCesAmt = 0;
                    IL.StateCesNonAdvlAmt = 0;
                    if (cnt == 0)
                    {
                        //IL.TotAmt = Convert.ToDecimal(Convert.ToDecimal(dr["AMOUNT"].ToString()) + Convert.ToDecimal(dr["TCS"].ToString()));                        
                        //IL.OthChrg = Convert.ToDecimal(dr["TCS"].ToString());
                        //IL.TotItemVal = Convert.ToDecimal(Convert.ToDecimal(dr["TOTALAMT"].ToString()) + Convert.ToDecimal(dr["TCS"].ToString()));
                        IL.TotAmt = Convert.ToDecimal(dr["AMOUNT"].ToString());
                        IL.OthChrg = 0;
                        IL.TotItemVal = Convert.ToDecimal(Convert.ToDecimal(dr["TOTALAMT"].ToString()));
                    }
                    else
                    {
                        IL.TotAmt = Convert.ToDecimal(dr["AMOUNT"].ToString());
                        IL.OthChrg = 0;
                        IL.TotItemVal = Convert.ToDecimal(Convert.ToDecimal(dr["TOTALAMT"].ToString()));
                    }
                    ilst.Add(IL);
                }
                inj.ItemList = ilst;
                clearTax.transaction = inj;
                lst.Add(clearTax);
                cnt += 1;
            }



            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(PortalUrl);
            //client.DefaultRequestHeaders.Add("x-cleartax-auth-token", "1.8d7102fc-dcd5-4028-afe1-1a546578f91d_c02d56f711426edc678c0b5435b82c661a1789703fe15661ef0334a3af5a85d7");
            client.DefaultRequestHeaders.Add("x-cleartax-auth-token", AuthToken);
            client.DefaultRequestHeaders.Add("x-cleartax-product", "EInvoice");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("owner_id", OwnerId);            
            client.DefaultRequestHeaders.Add("gstin", lst[0].transaction.SellerDtls.Gstin.ToString());
            string TJson = JArray.FromObject(lst).ToString();



            //var res = client.PutAsJsonAsync("v2/eInvoice/generate", lst).Result;

            var json = JsonConvert.SerializeObject(lst);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var res = client.PutAsync("v2/eInvoice/generate", stringContent).Result;
            dynamic test = new JObject();
            if (res.IsSuccessStatusCode)
            {
                var cnt = res.Content.ReadAsStringAsync().Result;
                govt = JsonConvert.DeserializeObject<List<GovtRes>>(cnt);
                //o.FirstOrDefault().SelectToken("govt_response").ToString() ;
                //govt.document_status = o.FirstOrDefault().SelectToken("document_status").ToString();
                //test.Data = res.Content.ReadAsStringAsync().Result.ToString();
                //var cnt = JObject.Parse(res.Content.ReadAsStringAsync().Result.ToString());
                //govt = JsonConvert.DeserializeObject<GovtRes>(res.Content.ReadAsStringAsync().Result.ToString());
            }

            //((JValue)test.Data).Value
            if (govt.Count > 0)
            {
                return govt[0];
            }
            else
            {
                return null;
            }
        }

        public class ClearTaxCancel
        {
            public string irn { get; set; }
            public string CnlRsn { get; set; }
            public string CnlRem { get; set; }
        }

        public GovtRes clsJonClearTax_Cancel(string irn, string CnlRsn, string CnlRem, string GSTIN, string OwnerId, string PortalUrl,string token)
        {
            List<GovtRes> govt = new List<GovtRes>();
            ClearTaxCancel clearTaxCancel = new ClearTaxCancel();
            clearTaxCancel.irn = irn;
            clearTaxCancel.CnlRsn = CnlRsn;
            clearTaxCancel.CnlRem = CnlRem;
            List<ClearTaxCancel> clearTaxCancels = new List<ClearTaxCancel>();
            clearTaxCancels.Add(clearTaxCancel);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(PortalUrl);
            client.DefaultRequestHeaders.Add("x-cleartax-auth-token", token);
            client.DefaultRequestHeaders.Add("x-cleartax-product", "EInvoice");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("owner_id", OwnerId);
            client.DefaultRequestHeaders.Add("gstin", GSTIN);
            string TJson = JArray.FromObject(clearTaxCancels).ToString();

            //var res = client.PutAsJsonAsync("v2/eInvoice/cancel", clearTaxCancels).Result;

            var json = JsonConvert.SerializeObject(clearTaxCancels);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var res = client.PutAsync("v2/eInvoice/cancel", stringContent).Result;
            dynamic test = new JObject();
            if (res.IsSuccessStatusCode)
            {
                var cnt = res.Content.ReadAsStringAsync().Result;
                govt = JsonConvert.DeserializeObject<List<GovtRes>>(cnt);
            }


            if (govt.Count > 0)
            {
                return govt[0];
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
                if (input.dtItem.Rows[0]["TRANTYPE"].ToString() == "SR" || input.dtItem.Rows[0]["TRANTYPE"].ToString() == "CN")
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
                else if (input.dtItem.Rows[0]["TRANTYPE"].ToString() == "IPU" || input.dtItem.Rows[0]["TRANTYPE"].ToString() == "DN")
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

                _ValDtls.AssVal = Convert.ToDecimal(input.dtItem.Compute("SUM(AMOUNT)", null));
                _ValDtls.IgstVal = Convert.ToDecimal(input.dtItem.Compute("SUM(IGST)", null));
                _ValDtls.CgstVal = Convert.ToDecimal(input.dtItem.Compute("SUM(CGST)", null));
                _ValDtls.SgstVal = Convert.ToDecimal(input.dtItem.Compute("SUM(SGST)", null));
                _ValDtls.CesVal = Convert.ToDecimal(input.dtItem.Compute("SUM(CESS)", null));
                _ValDtls.StCesVal = 0;
                _ValDtls.Discount = 0;
                _ValDtls.OthChrg = Convert.ToDecimal(input.dtItem.Compute("SUM(TCS)", null));
                _ValDtls.RndOffAmt = 0;
                _ValDtls.TotInvVal = Convert.ToDecimal(input.dtItem.Compute("SUM(TOTALAMT)", null)) + Convert.ToDecimal(input.dtItem.Compute("SUM(TCS)", null));

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
                        IL.Qty = Convert.ToDecimal(dr["GRSWT"].ToString());
                        IL.Unit = "GMS";
                        IL.UnitPrice = Convert.ToDecimal(dr["RATE"].ToString());
                    }
                    else
                    {
                        IL.Qty = Convert.ToDecimal(dr["PCS"].ToString());
                        IL.Unit = "PCS";
                        IL.UnitPrice = Convert.ToDecimal(dr["AMOUNT"].ToString());
                    }
                    IL.TotAmt = Convert.ToDecimal(dr["AMOUNT"].ToString());
                    IL.Discount = 0;
                    IL.PreTaxVal = 0;
                    IL.AssAmt = Convert.ToDecimal(dr["AMOUNT"].ToString());
                    IL.GstRt = Convert.ToDecimal(dr["SALESTAX"].ToString());
                    IL.IgstAmt = Convert.ToDecimal(dr["IGST"].ToString());
                    IL.CgstAmt = Convert.ToDecimal(dr["CGST"].ToString());
                    IL.SgstAmt = Convert.ToDecimal(dr["SGST"].ToString());
                    IL.CesRt = Convert.ToDecimal(dr["CESSTAX"].ToString());
                    IL.CesAmt = Convert.ToDecimal(dr["CESS"].ToString());
                    IL.CesNonAdvlAmt = 0;
                    IL.StateCesRt = 0;
                    IL.StateCesAmt = 0;
                    IL.StateCesNonAdvlAmt = 0;
                    IL.OthChrg = 0;
                    IL.TotItemVal = Convert.ToDecimal(dr["TOTALAMT"].ToString());
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

        public class Invoice_Json
        {
            //[StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\", MinLength = 4, MaxLength = 10)]
            public string Version { get; set; }
            //public string SupTyp { get; set; }
            //public string IgstOnIntra { get; set; }
            //public string RegRev { get; set; }
            //public string EcmGstin { get; set; }


            public TranDtls TranDtls { get; set; }
            public DocDtls DocDtls { get; set; }
            public SellerDtls SellerDtls { get; set; }
            public BuyerDtls BuyerDtls { get; set; }
            public DispDtls DispDtls { get; set; }
            public ShipDtls ShipDtls { get; set; }
            public List<ItemList> ItemList { get; set; }
            public PayDtls PayDtls { get; set; }
            public RefDtls RefDtls { get; set; }

            public AddlDocDtls[] AddlDocDtls { get; set; }
            public ExpDtls ExpDtls { get; set; }
            public EwbDtls EwbDtls { get; set; }

            public ValDtls ValDtls { get; set; }
        }


        public class TranDtls
        {
            //[StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\", MinLength = 3, MaxLength = 10)]
            public string TaxSch { get; set; }

            //[StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\", MinLength = 3, MaxLength = 10)]
            public string SupTyp { get; set; }
            public string RegRev { get; set; } //Y- whether the tax liability is payable under reverse charge
            public string EcmGstin { get; set; }//GSTIN of e-Commerce operator
            public string IgstOnIntra { get; set; }// Y- indicates the supply is intra state but chargeable to IGST
        }
        public class DocDtls
        {
            //[StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\", MinLength = 3, MaxLength = 11)]
            public string Typ { get; set; }

            //[StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 16)]
            public string No { get; set; }

            //[StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{};'|", MinLength = 10, MaxLength = 10)]
            public string Dt { get; set; }
        }
        public class SellerDtls
        {
            [Required]
            public string Gstin { get; set; }
            [Required]
            public string LglNm { get; set; }
            public string TrdNm { get; set; }
            [Required]
            public string Addr1 { get; set; }
            [Required]
            public string Addr2 { get; set; }
            [Required]
            public string Loc { get; set; }
            [Required]
            public Int32 Pin { get; set; }
            public string Stcd { get; set; }
            [Required]
            public string Ph { get; set; }
            [Required]
            public string Em { get; set; }

        }
        public class BuyerDtls
        {
            [Required]
            public string Gstin { get; set; }
            [Required]
            public string LglNm { get; set; }
            public string TrdNm { get; set; }
            [Required]
            public string Pos { get; set; }
            [Required]
            public string Addr1 { get; set; }
            [Required]
            public string Addr2 { get; set; }
            [Required]
            public string Loc { get; set; }
            [Required]
            public Int32 Pin { get; set; }
            public string Stcd { get; set; }
            [Required]
            public string Ph { get; set; }
            [Required]
            public string Em { get; set; }

        }
        public class DispDtls
        {
            public string Nm { get; set; }
            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Loc { get; set; }
            public Int32 Pin { get; set; }
            public string Stcd { get; set; }

        }
        public class ShipDtls
        {
            public string Gstin { get; set; }
            public string LglNm { get; set; }
            public string TrdNm { get; set; }
            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Loc { get; set; }
            public Int32 Pin { get; set; }
            public string Stcd { get; set; }

        }
        public class ItemList
        {
            public string SlNo { get; set; }
            public string PrdDesc { get; set; }
            public string IsServc { get; set; }
            public string Barcde { get; set; }
            public string HsnCd { get; set; }
            public decimal Qty { get; set; }
            public decimal FreeQty { get; set; }
            public string Unit { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal TotAmt { get; set; }
            public decimal Discount { get; set; }
            public decimal PreTaxVal { get; set; }
            public decimal AssAmt { get; set; }
            public decimal GstRt { get; set; }
            public decimal SgstAmt { get; set; }
            public decimal IgstAmt { get; set; }
            public decimal CgstAmt { get; set; }
            public decimal CesRt { get; set; }
            public decimal CesAmt { get; set; }
            public decimal CesNonAdvlAmt { get; set; }
            public decimal StateCesRt { get; set; }
            public decimal StateCesAmt { get; set; }
            public decimal StateCesNonAdvlAmt { get; set; }
            public decimal OthChrg { get; set; }
            public decimal TotItemVal { get; set; }
            public string OrdLineRef { get; set; }//Order line reference
            public string OrgCntry { get; set; } //Origin Country
            public string PrdSlNo { get; set; } //Serial number in case of each item having a unique number.
            public BchDtls BchDtls { get; set; }
            public List<Attribute> AttribDtls { get; set; }

        }
        public class Attribute
        {
            public string Nm { get; set; }
            public string Val { get; set; }
        }
        public class ValDtls
        {
            public decimal AssVal { get; set; } // Total Assessable value of all items
            public decimal CgstVal { get; set; } // Total CGST value of all items
            public decimal SgstVal { get; set; } // Total SGST value of all items
            public decimal IgstVal { get; set; } // Total IGST value of all items
            public decimal CesVal { get; set; } // Total CESS value of all items
            public decimal StCesVal { get; set; } // Total State CESS value of all items
            public decimal RndOffAmt { get; set; } // Rounded off amount
            public decimal Discount { get; set; } // Discount
            public decimal OthChrg { get; set; } // Other Charges
            public decimal TotInvVal { get; set; } // Final Invoice value
            public decimal TotInvValFc { get; set; } //Final Invoice value in Additional Currency
        }
        public class PayDtls
        {
            public string Nm { get; set; } // Payee Name    
            public string AccDet { get; set; } // Bank account number of payee
            public string Mode { get; set; } // Mode of Payment: Cash, Credit, Direct Transfer
            public string FinInsBr { get; set; } // Branch or IFSC code
            public string PayTerm { get; set; } // Terms of Payment
            public string PayInstr { get; set; } // Payment Instruction
            public string CrTrn { get; set; } // Credit Transfer
            public string DirDr { get; set; } // Direct Debit
            public int CrDay { get; set; } // Credit Days
            public decimal PaidAmt { get; set; } // The sum of amount which have been paid in advance.
            public decimal PaymtDue { get; set; } //Outstanding amount that is required to be paid.
        }
        public class DocPerdDtls
        {
            public string InvStDt { get; set; } // Invoice Period Start Date
            public string InvEndDt { get; set; } // Invoice Period End Date
        }
        public class RefDtls
        {
            public string InvRm { get; set; } // Remarks/Note   
            public DocPerdDtls DocPerdDtls { get; set; }
            private PrecDocDtls[] PrecDocDtls { get; set; }
            public ContrDtls[] ContrDtls { get; set; }

        }
        public class PrecDocDtls
        {
            public string InvNo { get; set; } // Reference of original invoice, if any
            public string InvDt { get; set; } // Date of preceding invoice
            public string OthRefNo { get; set; } // Other Reference
        }
        public class ContrDtls
        {
            public string RecAdvRefr { get; set; } //Receipt Advice No.
            public string RecAdvDt { get; set; } //Date of receipt advice
            public string TendRefr { get; set; } //Lot/Batch Reference No.
            public string ContrRefr { get; set; } //Contract Reference Number
            public string ExtRefr { get; set; } //Contract Reference Number
            public string ProjRefr { get; set; } //Project Reference Number
            public string PORefr { get; set; } //Vendor PO Reference Number
            public string PORefDt { get; set; } //Vendor PO Reference date
        }
        public class AddlDocDtls
        {
            public string Url { get; set; }
            public string Docs { get; set; }
            public string Info { get; set; }
        }
        public class ExpDtls
        {
            public string ShipBNo { get; set; }
            public string ShipBDt { get; set; }
            public string Port { get; set; }
            public string RefClm { get; set; }
            public string ForCur { get; set; }
            public string CntCode { get; set; }
            public string ExpDuty { get; set; }
        }
        public class EwbDtls
        {
            public string TransId { get; set; }  //Transin/GSTIN
            public string TransName { get; set; } //Name of the transporter
            public string TransMode { get; set; } //Mode of transport (Road-1, Rail-2, Air-3, Ship-4)
            public int Distance { get; set; } //Distance between source and destination PIN codes
            public string TransDocNo { get; set; } //Tranport Document Number
            public string TransDocDt { get; set; } //Transport Document Date
            public string VehNo { get; set; } //Vehicle Number
            public string VehType { get; set; } //Whether O-ODC or R-Regular
        }
        public class BchDtls
        {
            public string Nm { get; set; }
            public string ExpDt { get; set; }
            public string WrDt { get; set; }
        }

        public class CustomFields
        {
            public string customfieldLable1 { get; set; }
            public string customfieldLable2 { get; set; }
            public string customfieldLable3 { get; set; }
        }

        public class AddlDocDtl
        {
            public string Url { get; set; }
            public string Docs { get; set; }
            public string Info { get; set; }
        }

        public class Transaction
        {
            public string Version { get; set; }
            public TranDtls TranDtls { get; set; }
            public DocDtls DocDtls { get; set; }
            public SellerDtls SellerDtls { get; set; }
            public BuyerDtls BuyerDtls { get; set; }
            public DispDtls DispDtls { get; set; }
            public ShipDtls ShipDtls { get; set; }
            public List<ItemList> ItemList { get; set; }
            public ValDtls ValDtls { get; set; }
            public PayDtls PayDtls { get; set; }
            public RefDtls RefDtls { get; set; }
            public List<AddlDocDtl> AddlDocDtls { get; set; }
            public ExpDtls ExpDtls { get; set; }
            public EwbDtls EwbDtls { get; set; }
            public CustomFields custom_fields { get; set; }
        }

    }
}
