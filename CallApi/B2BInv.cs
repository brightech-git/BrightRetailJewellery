using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CallApi
{
    public class B2BInv
    {

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }


        public class CancelReq
        {
            public string Irn { get; set; }
            public string CnlRsn { get; set; }
            public string CnlRem { get; set; }
        }

        public class CancelRes
        {
            public string status { get; set; }
            public string data { get; set; }
            public object ErrorDetails { get; set; }
            public string InfoDtls { get; set; }
        }


        public class CancelData
        {
            public CREDENTIALS _CREDENTIALS = new CREDENTIALS();
            public CancelReq _CancelReq = new CancelReq();
        }




        public class CREDENTIALS
        {
            public string GSTNO { get; set; }
            public string USERNAME { get; set; }
            public string PASSWORD { get; set; }
            public string SECERET { get; set; }
            public string CLIENTID { get; set; }
            public string PublicKey { get; set; }
        }


        public class Para
        {
            public CREDENTIALS _CREDENTIALS = new CREDENTIALS();
            public COMPANY _COMPANY = new COMPANY();
            public List<ITEM> _ITEM = new List<ITEM>();
            public BUYER _BUYER = new BUYER();
        }

        public class COMPANY
        {
            
            public string COMPANYNAME { get; set; }

            public string ADDRESS1 { get; set; }

            public string ADDRESS2 { get; set; }

            public string ADDRESS3 { get; set; }

            public string ADDRESS4 { get; set; }

            public string PHONE { get; set; }

            public string EMAIL { get; set; }

            public string GSTNO { get; set; }

            public string AREACODE { get; set; }

            public string STATECODE { get; set; }

        }

        public class ITEM
        {
            public long? SNO { get; set; }

            public string ITEMNAME { get; set; }

            public string TAGNO { get; set; }

            public string HSN { get; set; }

            public int? PCS { get; set; }

            public decimal? GRSWT { get; set; }

            public string SALEMODE { get; set; }

            public decimal? TOTALAMT { get; set; }

            public decimal? AMOUNT { get; set; }

            public decimal? TAX { get; set; }

            public decimal? DISCOUNT { get; set; }

            public decimal? SALESTAX { get; set; }

            public decimal? CESSTAX { get; set; }

            public decimal CGST { get; set; }

            public decimal SGST { get; set; }

            public decimal IGST { get; set; }

            public decimal CESS { get; set; }

            public string TRANNO { get; set; }

            public string TRANDATE { get; set; }

            public decimal? RATE { get; set; }

            public string BATCHNO { get; set; }

            public string TRANTYPE { get; set; }

            public string ISSERVICE { get; set; }
        }
        public class BUYER
        {
            public string PNAME { get; set; }

            public string ADDRESS1 { get; set; }

            public string ADDRESS2 { get; set; }

            public string ADDRESS3 { get; set; }

            public string AREA { get; set; }

            public string PHONERES { get; set; }

            public string EMAIL { get; set; }

            public string GSTNO { get; set; }

            public string PINCODE { get; set; }

            public string STATECODE { get; set; }

            public string EINVTYPE { get; set; }

        }

    }
}
