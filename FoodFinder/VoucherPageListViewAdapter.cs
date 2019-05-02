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
    //Sets up Listview adapter for vouchers
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

            ImageView voucherImg = voucher.FindViewById<ImageView>(Resource.Id.VoucherImage);
            var imageBitmap = ImageHelper.GetImageBitmapFromUrl(mItems[position].voucherImage);
            voucherImg.SetImageBitmap(imageBitmap);

            TextView restaurantName = voucher.FindViewById<TextView>(Resource.Id.RestaurantName);
            restaurantName.Text = mItems[position].restName;

            TextView deal = voucher.FindViewById<TextView>(Resource.Id.Deal);
            deal.Text = mItems[position].deal;

            TextView expiryDate = voucher.FindViewById<TextView>(Resource.Id.ExpiryDate);
            //expiryDate.Text = mItems[position].expiryDate.ToString();
            string date = mItems[position].expiryDate.ToString();

            if (date.Contains("00:00:00"))
            {
                string modifiedDate = date.Remove(date.IndexOf(" 00:00:00"), " 00:00:00".Length);
                expiryDate.Text = "Expires: " + modifiedDate;
            }
            else
            {
                expiryDate.Text = date;
            }

            ImageButton button = voucher.FindViewById<ImageButton>(Resource.Id.imageButton1);

            image = voucher.FindViewById<ImageView>(Resource.Id.imageView1);

            return voucher;
        }
        
    }
}