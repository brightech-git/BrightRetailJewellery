using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CallApi
{
    public class BounzInv
    {
        public class BOUNZ_TRANACTION
        {
            public string USERNAME { get; set; }
            public string PASSWORD { get; set; }
            public string PublicKey { get; set; }
        }
        public class Para
        {
            //public BOUNZ_TRANACTION BOUNZ_TRANACTION = new BOUNZ_TRANACTION();
            //public List<transactions> transactions = new List<transactions>();
            public LockPoint Lockpoint = new LockPoint();
            public ReleasaeLockPoint Releasaelockpoint = new ReleasaeLockPoint();
            public Redeem redeem = new Redeem();
            public Profile profile = new Profile();

        }
        public class Paralist
        {
            public List<transactions> transactions = new List<transactions>();
        }
        public class transactions
        {
            public string bill_number { get; set; }
            public DateTime bill_date { get; set; }
            public string store_code { get; set; }
            public string partner_code { get; set; }
            public string loyalty_id { get; set; }
            public string mobile_number { get; set; }
            public string country_code { get; set; }
            public decimal total_bill_amount { get; set; }
            public decimal total_discount { get; set; }
            public decimal tax_amount { get; set; }
            public string charity_amount { get; set; }
            public string tender_type { get; set; }
            public string transaction_type { get; set; }
            public string customer_type { get; set; }
            public string excise_duty { get; set; }
            public string action { get; set; }
            public string category_code { get; set; }
            public string tentative_date { get; set; }
            public string currency { get; set; }
            public string channel_code { get; set; }
            public string til_no { get; set; }
            public int e_id { get; set; }

        }
        public class LockPoint
        {
            public string loyalty_id { get; set; }
            public string mobile_number { get; set; }
            public string country_code { get; set; }
            public string partner_id { get; set; }
            public string store_id { get; set; }
            public string points { get; set; }
            public string amount { get; set; }
            public string activity_code { get; set; }
        }
        public class ReleasaeLockPoint
        {
            public string loyalty_id { get; set; }
            public string partner_id { get; set; }
            public string store_id { get; set; }
            public string lock_id { get; set; }
        }
        public class Redeem
        {
            public string loyalty_id { get; set; }
            public string partner_id { get; set; }
            public string store_id { get; set; }
            public string invoice_number { get; set; }
            public string points { get; set; }
            public string amount { get; set; }
            public string lock_id { get; set; }
            public string activity_code { get; set; }
            public string transaction_id { get; set; }
        }
        public class Profile
        {
            public string loyalty_id { get; set; }
            public string mobile_number { get; set; }
            public string country_code { get; set; }
        }
        public class data
        {
            public bool status { get; set; }
            public string message { get; set; }
            public string status_code { get; set; }
            public Values values { get; set; }
        }

        public class BOUNZRESULT
        {
            public bool status { get; set; }
            public string message { get; set; }
            public string status_code { get; set; }
            public Values values { get; set; }
        }
        public class BOUNZLOCKRESULT
        {
            public bool status { get; set; }
            public string message { get; set; }
            public string status_code { get; set; }
            public lockValues values { get; set; }

        }
        public class BOUNZPROFILE
        {
            public bool status { get; set; }
            public string message { get; set; }
            public string status_code { get; set; }
            public List<ProfileValues> values { get; set; }
        }
        public class Values
        {
            public long batch_id { get; set; }
        }
        public class lockValues
        {
            public long lock_point_id { get; set; }
            public int debit_points { get; set; }
            public string transaction_label { get; set; }
            public string description { get; set; }
            public int activity_id { get; set; }
            public string activity_name { get; set; }
            public string transaction_id { get; set; }
        }
        public class ProfileValues
        {
            public int id { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string loyalty_id { get; set; }
            public string country_code { get; set; }
            public string mobile_number { get; set; }
            public string email { get; set; }
            public object partner_id { get; set; }
            public object store_id { get; set; }
            public object e_id { get; set; }
            public string account_status { get; set; }
            public int point_balance { get; set; }
            public int tentative_points { get; set; }
        }

    }
}