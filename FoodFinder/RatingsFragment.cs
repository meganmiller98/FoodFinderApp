﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace FoodFinder
{
    public class RatingsFragment : Android.Support.V4.App.Fragment
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.Adapter mAdapter;
        private List<Ratings> mRatings;
        string getID;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.InfoTab, container, false);
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView1);

            getID = RestaurantProfileActivity.sendData();
            if (getID != null)
            {
                Toast.MakeText(Context as Activity, getID, ToastLength.Short).Show();
                getRatings(getID);
            }
            else
            {
                Toast.MakeText(Context as Activity, "Nothing again", ToastLength.Short).Show();
            }

            return view;
        }
        async void getRatings(string getID)
        {
            mRatings = new List<Ratings>();
            //myIp
            //string uri = "htp://192.168.0.20:45455/api/Ratings/";

            //uni IP
            //string uri = "htp://10.201.37.145:45455/api/mainmenu/";

            string uri = "http://192.168.1.70:45455/api/Ratings/";

            string otherhalf = "getRatings?ID= " + getID;

            Uri result = null;

            if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
            {
                var httpClient = new HttpClient();
                var refineResult = (await httpClient.GetStringAsync(result));
                mRatings = JsonConvert.DeserializeObject<List<Ratings>>(refineResult);

                mLayoutManager = new LinearLayoutManager(Context as Activity);
                mRecyclerView.SetLayoutManager(mLayoutManager);
                mAdapter = new RatingsRecyclerAdapter(mRatings);
                mRecyclerView.SetAdapter(mAdapter);

            }

        }
    }
    public class RatingsRecyclerAdapter : RecyclerView.Adapter
    {
        /*private List<Email> mEmails;
        public RecyclerAdapter(List<Email> emails)
        {
            mEmails = emails;
        }*/
        private List<Ratings> mRatings;
        public RatingBar ratingBar;
        float x;

        public RatingsRecyclerAdapter(List<Ratings> ratings)
        {
            mRatings = ratings;
        }

        public class MyRatingView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mRating { get; set; }
            public TextView mRestaurantID { get; set; }
            public TextView mName { get; set; }
            public TextView mUserID { get; set; }
            

            public MyRatingView(View view) : base(view)
            {
                mMainView = view;

            }
        }
        public override int ItemCount
        {
            get { return mRatings.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyRatingView myHolder = holder as MyRatingView;

            x = float.Parse(mRatings[position].rating, CultureInfo.InvariantCulture.NumberFormat);
            myHolder.mName.Text = mRatings[position].name;
            ratingBar.Rating = x;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            
            View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.RatingsLayout, parent, false);

            TextView textName = row.FindViewById<TextView>(Resource.Id.Name);
            ratingBar = row.FindViewById<RatingBar>(Resource.Id.ratingBar1);
            ratingBar.Clickable = false;
            ratingBar.IsIndicator = true;
            //mratingBar.Rating = x;


            MyRatingView view = new MyRatingView(row)
            {
                mName = textName,
                //ratingBar = mratingBar
                //ratingBar = mratingBar
                
            };
            return view;
        }

    }
}