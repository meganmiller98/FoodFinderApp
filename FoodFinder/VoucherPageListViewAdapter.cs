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
    class VoucherPageListViewAdapter : BaseAdapter<Vouchers>
    {
        private List<Vouchers> mItems;
        private Context mContext;
        ImageView image;

        public VoucherPageListViewAdapter(Context context, List<Vouchers> items)
        {
            mItems = items;
            mContext = context;
        }
        public override int Count
        {
            get { return mItems.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Vouchers this[int position]
        {
            get { return mItems[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View voucher = convertView;
            if (voucher == null)
            {
                voucher = LayoutInflater.From(mContext).Inflate(Resource.Layout.listview_vouchers, null, false);
            }

            //Using ImageHelper class
            //MainPhoto1 in post
            ImageView voucherImg = voucher.FindViewById<ImageView>(Resource.Id.VoucherImage);
            var imageBitmap = ImageHelper.GetImageBitmapFromUrl(mItems[position].voucherImage);
            voucherImg.SetImageBitmap(imageBitmap);

            TextView restaurantName = voucher.FindViewById<TextView>(Resource.Id.RestaurantName);
            restaurantName.Text = mItems[position].restName;

            TextView deal = voucher.FindViewById<TextView>(Resource.Id.Deal);
            deal.Text = mItems[position].deal;

            TextView expiryDate = voucher.FindViewById<TextView>(Resource.Id.ExpiryDate);
            expiryDate.Text = mItems[position].expiryDate.ToString();

            ImageButton button = voucher.FindViewById<ImageButton>(Resource.Id.imageButton1);
            //button.Click += buttonClick;

            image = voucher.FindViewById<ImageView>(Resource.Id.imageView1);
            /*image.Click += imageClick;
            voucher.image.Click += delegate {
                btnOneClick();
            };*/
            return voucher;
        }
        
       /* void buttonClick(object sender, EventArgs e)
        {
            Toast.MakeText(mContext, "Clicked", ToastLength.Short).Show();
        }
        void imageClick (object sender, EventArgs e)
        {
            Toast.MakeText(mContext, "Clicked", ToastLength.Short).Show();
            if (image.Selected == true)
            {
                image.Selected = false;
               
            }
            else if (image.Selected == false)
            {
                image.Selected = true;
            }
            

        }*/
    }
}