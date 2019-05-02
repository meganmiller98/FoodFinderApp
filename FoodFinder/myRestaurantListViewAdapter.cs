using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FoodFinder
{ 
    //displaying the layout for each item in the restaurant (POST) list
    class myRestaurantListViewAdapter : BaseAdapter<Post>
    {
        private List<Post> mItems;
        private Context mContext;

        public myRestaurantListViewAdapter(Context context, List<Post> items)
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

        public override Post this[int position]
        {
            get { return mItems[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View restaurant = convertView;
            if (restaurant == null)
            {
                restaurant = LayoutInflater.From(mContext).Inflate(Resource.Layout.listview_restaurant, null, false);
            }

            ImageView restaurantImg = restaurant.FindViewById<ImageView>(Resource.Id.RestaurantImage);
            var imageBitmap = ImageHelper.GetImageBitmapFromUrl(mItems[position].MainPhoto1);
            restaurantImg.SetImageBitmap(imageBitmap);

            TextView restaurantName = restaurant.FindViewById<TextView>(Resource.Id.RestaurantName);
            restaurantName.Text = mItems[position].RestaurantName;

            TextView category = restaurant.FindViewById<TextView>(Resource.Id.Category);
            category.Text = mItems[position].Categories;

            TextView times = restaurant.FindViewById<TextView>(Resource.Id.Time);
            string open = mItems[position].Opentimes.ToString();
            open = open.Remove(5);
            string close = mItems[position].CloseTimes.ToString();
            close = close.Remove(5);
            times.Text = open + " - " + close;

            TextView rating = restaurant.FindViewById<TextView>(Resource.Id.Rating);
            rating.Text = mItems[position].Rating + "/5";

            return restaurant;
        }
    }
}