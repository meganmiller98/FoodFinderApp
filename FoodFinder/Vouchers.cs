using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FoodFinder
{
    public class Vouchers
    {
        public string voucherID { get; set; }
        public string restaurantID { get; set; }
        public string expiryDate { get; set; }
        public string deal { get; set; }
        public string termsOfConditions { get; set; }
        public string voucherImage { get; set; }
        public string voucherCode { get; set; }
        public string number { get; set; }
        public string restName { get; set; }

        public string error { get; set; }
        public Vouchers(string voucherID, string restaurantID, string expiryDate, string deal, string termsOfConditions, string voucherImage, string voucherCode, string number, string restName, string error)
        {
            this.voucherID = voucherID;
            this.restaurantID = restaurantID;
            this.expiryDate = expiryDate;
            this.deal = deal;
            this.termsOfConditions = termsOfConditions;
            this.voucherImage = voucherImage;
            this.voucherCode = voucherCode;
            this.number = number;
            this.restName = restName;
            this.error = error;
        }
    }
}