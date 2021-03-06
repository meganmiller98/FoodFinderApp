﻿using System;
using System.Collections.Generic;
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
    public class ProfilePage : Fragment
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.Adapter mAdapter;
        private List<savedRestaurants> mRestaurants;
        TextView nothingSavedMessage;
        TextView noVouchers;

        private RecyclerView nRecyclerView;
        private RecyclerView.LayoutManager nLayoutManager;
        private RecyclerView.Adapter nAdapter;
        private List<Vouchers> nSavedVouchers;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public ProfilePage()
        {

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View view = inflater.Inflate(Resource.Layout.ProfilePage, container, false);

            //check if user is logged
            ISharedPreferences prefs = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string name = prefs.GetString("name", null);
            string userID = prefs.GetString("userID", null);
            string userName = prefs.GetString("username", null);
            string password = prefs.GetString("password", null);

            if (userID == null)
            {
                Intent intent = new Intent(Context as Activity, typeof(LogInActivity));
                intent.PutExtra("isProfile", "from profile");
                StartActivity(intent);
            }
            else
            {
                nRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerViewVouchers);
                mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerViewRestaurants);
                TextView nameText = view.FindViewById<TextView>(Resource.Id.welcomeText);
                nothingSavedMessage = view.FindViewById<TextView>(Resource.Id.textSaveMessage);
                noVouchers = view.FindViewById<TextView>(Resource.Id.textSaveMessage2);
                nameText.Text = name;

                Button logOut = view.FindViewById<Button>(Resource.Id.logOutButton);

                logOut.Click += logout_Click;

                getSavedVouchers(userID);
                getSavedRestaurants(userID);
                

            }
            return view;
        }
        void logout_Click(object sender, EventArgs e)
        {
            ISharedPreferences prefs = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            ISharedPreferencesEditor editer = prefs.Edit();
            editer.Clear();
            editer.Apply();

            Intent intent = new Intent(Context as Activity, typeof(MainActivity));
            StartActivity(intent);

        }
        async void getSavedRestaurants(string userID)
        {
            Console.WriteLine("in getSaved Method");
            mRestaurants = new List<savedRestaurants>();

            string uri = "https://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/SavedRestaurant/";

            string otherhalf = "getSaved?userID= " + userID;

            Uri result = null;

            if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
            {
                var httpClient = new HttpClient();
                var refineResult = (await httpClient.GetStringAsync(result));
                mRestaurants = JsonConvert.DeserializeObject<List<savedRestaurants>>(refineResult);

                if (mRestaurants.Count > 0)
                {
                    nothingSavedMessage.Visibility = Android.Views.ViewStates.Gone;
                    //positions items in recycler view
                    mLayoutManager = new LinearLayoutManager(Context as Activity, LinearLayoutManager.Horizontal, false);
                    mRecyclerView.SetLayoutManager(mLayoutManager);
                    //Binds the mRestaurants data with the view items in recycler view
                    mAdapter = new SavedRestaurantsRecyclerAdapter(mRestaurants, mRecyclerView, Context as Activity);
                    mRecyclerView.SetAdapter(mAdapter);
                }
                else
                {
                    nothingSavedMessage.Visibility = Android.Views.ViewStates.Visible;
                }


            }

        }
        async void getSavedVouchers(string userID)
        {
            Console.WriteLine("In the get saved vouchers method");
            nSavedVouchers = new List<Vouchers>();

            string uri = "https://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/Voucher/";

            string otherhalf = "getUsersSavedVouchers?userID= " + userID;

            Uri result = null;

            if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
            {
                var httpClient = new HttpClient();
                var refineResult = (await httpClient.GetStringAsync(result));
                nSavedVouchers = JsonConvert.DeserializeObject<List<Vouchers>>(refineResult);

                if (nSavedVouchers.Count > 0)
                {
                    noVouchers.Visibility = Android.Views.ViewStates.Gone;
                    Console.WriteLine("In the saved Vouchers page");
                    nLayoutManager = new LinearLayoutManager(Context as Activity, LinearLayoutManager.Horizontal, false);
                    nRecyclerView.SetLayoutManager(nLayoutManager);
                    nAdapter = new SavedVouchersRecyclerAdapter(nSavedVouchers, nRecyclerView, Context as Activity);
                    nRecyclerView.SetAdapter(nAdapter);
                }
                else
                {
                    noVouchers.Visibility = Android.Views.ViewStates.Visible;
                }


            }
        }
    }

    public class SavedRestaurantsRecyclerAdapter : RecyclerView.Adapter
    {
        private List<savedRestaurants> mRestaurants;
        private RecyclerView mRecyclerView;
        private Context mcontext;

        public SavedRestaurantsRecyclerAdapter(List<savedRestaurants> restaurants, RecyclerView recyclerView, Context context)
        {
            mRestaurants = restaurants;
            mRecyclerView = recyclerView;
            mcontext = context;
        }

        public class savedRestaurantsView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mUserID { get; set; }
            public TextView mRestaurantID { get; set; }
            public TextView mRestaurantName { get; set; }
            public ImageView mMainPhoto { get; set; }


            public savedRestaurantsView(View view) : base(view)
            {
                mMainView = view;

            }
        }
        public override int ItemCount
        {
            get { return mRestaurants.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            Console.WriteLine("In the saved restaurant adpater on bind");
            savedRestaurantsView myHolder = holder as savedRestaurantsView;
            Console.Write(mRestaurants[position].MainPhoto);
            myHolder.mMainPhoto.SetImageBitmap(ImageHelper.GetImageBitmapFromUrl(mRestaurants[position].MainPhoto));
            myHolder.mRestaurantName.Text = mRestaurants[position].RestaurantName;

            myHolder.mMainView.Click += mMainView_Click;
        }

        private void mMainView_Click(object sender, EventArgs e)
        {

            int position = mRecyclerView.GetChildAdapterPosition((View)sender);
            
            getInfoForRestaurantProfile(position);
           

        }
        async void getInfoForRestaurantProfile(int position)
        {
            string uri = "https://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/mainmenu/";

            string otherhalf = "getInfoForProfile?ID=" + mRestaurants[position].RestaurantID.ToString();
            
            Uri result = null;

            if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
            {
                var httpClient = new HttpClient();
                var refineResult = (await httpClient.GetStringAsync(result));
                List<Post> RestaurantList = JsonConvert.DeserializeObject<List<Post>>(refineResult);
                if (RestaurantList.Count == 0)
                {
                    Console.Write("Something went wrong with retrieving restaurant info");
                }
                else
                {

                    Intent intent = new Intent(mcontext, typeof(RestaurantProfileActivity));
                    intent.PutExtra("RestaurantInfo", JsonConvert.SerializeObject(RestaurantList[0]));
                    intent.PutExtra("profile", "yes");
                    mcontext.StartActivity(intent);

                }

            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            Console.WriteLine("In the view holder restaurant adpater");
            View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.profileSavedRestuarants, parent, false);

            ImageView image = row.FindViewById<ImageView>(Resource.Id.restaurantImage);
            TextView restaurantName = row.FindViewById<TextView>(Resource.Id.restaurantName);

            savedRestaurantsView view = new savedRestaurantsView(row)
            {
                mMainPhoto = image,
                mRestaurantName = restaurantName

            };
            return view;
        }

    }


    public class SavedVouchersRecyclerAdapter : RecyclerView.Adapter
    {
        private List<Vouchers> nsavedVouchers;
        private RecyclerView nRecyclerView;
        private Context ncontext;

        public SavedVouchersRecyclerAdapter(List<Vouchers> savedVouchers, RecyclerView recyclerView, Context context)
        {
            nsavedVouchers = savedVouchers;
            nRecyclerView = recyclerView;
            ncontext = context;
        }

        public class savedVouchersView : RecyclerView.ViewHolder
        {
            public View nMainView { get; set; }
            public TextView nVoucherID { get; set; }
            public TextView nRestaurantID { get; set; }
            public TextView nexpiryDate { get; set; }
            public TextView ndeal { get; set; }
            public TextView ntermsOfConditions { get; set; }
            public ImageView nVoucherImage { get; set; }
            public TextView nVoucherCode { get; set; }
            public TextView nNumber { get; set; }
            public TextView nrestName { get; set; }



            public savedVouchersView(View view) : base(view)
            {
                nMainView = view;

            }
        }
        public override int ItemCount
        {
            get { return nsavedVouchers.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            savedVouchersView Holder = holder as savedVouchersView;

            Console.Write(nsavedVouchers[position].voucherImage);

            Holder.nVoucherImage.SetImageBitmap(ImageHelper.GetImageBitmapFromUrl(nsavedVouchers[position].voucherImage));
            Holder.nrestName.Text = nsavedVouchers[position].restName;

            Holder.nMainView.Click += mMainView_Click;
        }

        private void mMainView_Click(object sender, EventArgs e)
        {

            int position = nRecyclerView.GetChildAdapterPosition((View)sender);
            
            Intent intent = new Intent(ncontext, typeof(VoucherInfoActivity));
            intent.PutExtra("VoucherInfo", JsonConvert.SerializeObject(nsavedVouchers[position]));
            intent.PutExtra("profile", "yes");
            ncontext.StartActivity(intent);
            

        }
    
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.profileSavedVouchers, parent, false);

            ImageView image = row.FindViewById<ImageView>(Resource.Id.voucherImage);
            TextView restaurantName = row.FindViewById<TextView>(Resource.Id.restaurantVoucherName);

            savedVouchersView view = new savedVouchersView(row)
            {
                nVoucherImage = image,
                nrestName = restaurantName

            };
            return view;
        }
    }
}