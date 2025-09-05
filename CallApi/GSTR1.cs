using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//using System.IO;
//using Newtonsoft.Json;

namespace CallApi
{


    public static class NumHelp
    {
        public static double Val(string expression)
        {
            if (expression == null)
                return 0;

            //try the entire string, then progressively smaller substrings to replicate the behavior of VB's 'Val', which ignores trailing characters after a recognizable value:
            for (int size = expression.Length; size > 0; size--)
            {
                double testDouble;
                if (double.TryParse(expression.Substring(0, size), out testDouble))
                    return testDouble;
            }

            //no value is recognized, so return 0:
            return 0;
        }

        public static double Val(object expression)
        {
            if (expression == null)
                return 0;

            double testDouble;
            if (double.TryParse(expression.ToString(), out testDouble))
                return testDouble;

            //VB's 'Val' function returns -1 for 'true':
            bool testBool;
            if (bool.TryParse(expression.ToString(), out testBool))
                return testBool ? -1 : 0;

            //VB's 'Val' function returns the day of the month for dates:
            DateTime testDate;
            if (DateTime.TryParse(expression.ToString(), out testDate))
                return testDate.Day;

            //no value is recognized, so return 0:
            return 0;
        }

        public static int Val(char expression)
        {
            int testInt;
            if (int.TryParse(expression.ToString(), out testInt))
                return testInt;
            else
                return 0;
        }
    }

    public class CreGSTR1
    {
        public void FuncGSTR1(string GSTIN, string FP, DataSet ds)
        {
            GSTR1 r1 = new GSTR1();
            r1.gstin = GSTIN;
            r1.fp = FP;
            r1.gt = 0;
            r1.cur_gt = 0;
            //r1.b2b =ds.Tables["B2B"].AsEnumerable().Select()

            //r1.b2b = (from DataRow dr in ds.Tables["B2B"].Rows
            //          select new B2b()
            //          {
            //              ctin = dr["GSTNO"].ToString()                          
            //          }).ToList();

            List<B2b> _B2b = new List<B2b>();
            List<B2cs> _B2cs = new List<B2cs>();
            List<Exp> _Exp = new List<Exp>();
            Hsn _Hsn = new Hsn();
            //B2b STARTS
            foreach (DataRow dr in ds.Tables["B2B"].Select("RESULT=5"))
            {
                B2b bb = new B2b();
                bb.ctin = dr["GSTNO"].ToString();



                Itm itm = new Itm();
                itm.num = 1;

                ItmDet det = new ItmDet();
                det.txval = NumHelp.Val(dr["AMOUNT"].ToString());
                det.rt = (int)NumHelp.Val(dr["RATE"].ToString());
                det.camt = NumHelp.Val(dr["CGST"].ToString());
                det.samt = NumHelp.Val(dr["SGST"].ToString());
                det.csamt = NumHelp.Val(dr["CESS"].ToString());


                itm.itm_det = det;

                Inv inv = new Inv();

                List<Itm> ii = new List<Itm>();
                ii.Add(itm);
                inv.itms = ii;

                inv.inum = dr["TRANNO"].ToString();
                DateTime dtm = DateTime.Parse(dr["TRANDATE"].ToString());
                inv.idt = dtm.ToString("dd-MM-yyyy");
                inv.val = Convert.ToDouble(dr["AMOUNT"].ToString());
                inv.pos = dr["STATE"].ToString().Substring(0, 2);
                inv.rchrg = "N";
                inv.inv_typ = "R";

                List<Inv> _Inv = new List<Inv>();
                _Inv.Add(inv);

                bb.inv = _Inv;
                _B2b.Add(bb);
            }
            r1.b2b = _B2b;
            //B2b ENDS

            //B2CS STARTS

            foreach (DataRow dr in ds.Tables["B2CS"].Select("RESULT=5"))
            {
                B2cs b2Cs = new B2cs();
                b2Cs.rt = NumHelp.Val(dr["RATE"].ToString());
                string state = dr["STATE"].ToString();
                if (state == "")
                    state = "33";
                else
                    state = dr["STATE"].ToString().Substring(0, 2);
                b2Cs.pos = state.Substring(0, 2);
                b2Cs.typ = "OE";
                b2Cs.txval = NumHelp.Val(dr["TAX"].ToString());
                b2Cs.csamt = NumHelp.Val(dr["CESS"].ToString());
                if (state == "33")
                {
                    b2Cs.sply_ty = "INTRA";
                    b2Cs.samt = NumHelp.Val(dr["CGST"].ToString());
                    b2Cs.camt = NumHelp.Val(dr["SGST"].ToString());
                }
                else
                {
                    b2Cs.sply_ty = "INTER";
                    b2Cs.iamt = NumHelp.Val(dr["IGST"].ToString());
                }
                b2Cs.rt = NumHelp.Val(dr["RATE"].ToString());
                _B2cs.Add(b2Cs);
            }
            r1.b2cs = _B2cs;
            //B2CS ENDS

            //HSN STARTS
            List<Datum> dtum = new List<Datum>();

            foreach (DataRow dr in ds.Tables["HSN"].Select("RESULT=5.0"))
            {
                Datum dm = new Datum();
                dm.num = Convert.ToInt32(NumHelp.Val(dr["KEYNO"].ToString())-5);
                dm.hsn_sc = dr["HSN"].ToString();
                dm.desc = dr["DESCRPITION"].ToString();
                dm.uqc = dr["UQC"].ToString().Substring(0,3);
                dm.qty = NumHelp.Val(dr["QTY"].ToString());
                dm.val = NumHelp.Val(dr["AMOUNT"].ToString());
                dm.txval = NumHelp.Val(dr["TAX"].ToString());
                dm.iamt = NumHelp.Val(dr["IGST"].ToString());
                dm.camt = NumHelp.Val(dr["CGST"].ToString());
                dm.samt = NumHelp.Val(dr["SGST"].ToString());
                dm.csamt = NumHelp.Val(dr["CESS"].ToString());
                dtum.Add(dm);
            }
            _Hsn.data = dtum;
            r1.hsn = _Hsn;


            //HSN ENDS
            string pth = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+"\\GSTR1"+ DateTime.Now.ToString("ddMMyyHHmmss")+ ".json";
            string JSONresult = Newtonsoft.Json.JsonConvert.SerializeObject(r1);
            string fname = DateTime.Now.ToString("ddMMyyHHmmss");
            string path = @""+ pth +"" + fname + ".json";
            using (var tw = new System.IO.StreamWriter(pth, true))
            {
                tw.WriteLine(JSONresult.ToString());
                tw.Close();
            }

        }


    }



    public class GSTR1
    {
        public string gstin { get; set; }
        public string fp { get; set; }
        public double gt { get; set; }
        public double cur_gt { get; set; }
        public List<B2b> b2b { get; set; }
        public List<B2cs> b2cs { get; set; }
        public List<Exp> exp { get; set; }
        public Hsn hsn { get; set; }

    }

    public class ItmDet
    {
        public double txval { get; set; }
        public int rt { get; set; }
        public double camt { get; set; }
        public double samt { get; set; }
        public double csamt { get; set; }
        public double? iamt { get; set; }

    }

    public class Itm
    {
        public int num { get; set; }
        public ItmDet itm_det { get; set; }

    }

    public class Inv
    {
        public string inum { get; set; }
        public string idt { get; set; }
        public double val { get; set; }
        public string pos { get; set; }
        public string rchrg { get; set; }
        public List<Itm> itms { get; set; }
        public string inv_typ { get; set; }

    }

    public class B2b
    {
        public string ctin { get; set; }
        public List<Inv> inv { get; set; }

    }

    public class B2cs
    {
        public double rt { get; set; }
        public string sply_ty { get; set; }
        public string pos { get; set; }
        public string typ { get; set; }
        public double txval { get; set; }
        public double iamt { get; set; }
        public double csamt { get; set; }
        public double? camt { get; set; }
        public double? samt { get; set; }

    }

    public class Itm2
    {
        public double txval { get; set; }
        public int rt { get; set; }
        public double iamt { get; set; }
        public double csamt { get; set; }

    }

    public class Inv2
    {
        public string inum { get; set; }
        public string idt { get; set; }
        public double val { get; set; }
        public List<Itm2> itms { get; set; }

    }

    public class Exp
    {
        public string exp_typ { get; set; }
        public List<Inv2> inv { get; set; }

    }

    public class Datum
    {
        public int num { get; set; }
        public string hsn_sc { get; set; }
        public string desc { get; set; }
        public string uqc { get; set; }
        public double qty { get; set; }
        public double val { get; set; }
        public double txval { get; set; }
        public double iamt { get; set; }
        public double camt { get; set; }
        public double samt { get; set; }
        public double csamt { get; set; }

    }

    public class Hsn
    {
        public List<Datum> data { get; set; }

    }



}
