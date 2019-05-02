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
    //Displaying Voucher Code information
    class VoucherCodeDialogFragment : DialogFragment
    {
        string getVoucherCode;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.voucherCodeDialog, container, false);

            ImageButton Button = view.FindViewById<ImageButton>(Resource.Id.closeButton);
            TextView heading = view.FindViewById<TextView>(Resource.Id.voucherHeading);
            TextView voucherCode = view.FindViewById<TextView>(Resource.Id.voucherCode);
            Button done = view.FindViewById<Button>(Resource.Id.button1);

            if (Arguments != null)
            {
                if (Arguments.GetString("voucherCode") != null)
                {
                    getVoucherCode = Arguments.GetString("voucherCode");
                    voucherCode.Text = getVoucherCode;
                }
                else
                {
                    voucherCode.Text = "VOUCHER101";
                }
            }

            Button.Click += button_Click;
            done.Click += done_Click;
            return view;

        }

        public void button_Click(object sender, EventArgs e)
        {
            Dismiss();
        }

        public void done_Click(object sender, EventArgs e)
        {
            Dismiss();
        }
    }
}