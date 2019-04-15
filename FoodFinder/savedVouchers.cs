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
    public class savedVouchers
    {
        public string voucherID { get; set; }
        public string userId { get; set; }

        public string error { get; set; }

        public savedVouchers(string voucherID, string userId, string error)
        {
            this.voucherID = voucherID;
            this.userId = userId;
            this.error = error;
        }
    }
}