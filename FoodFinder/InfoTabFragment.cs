using System;
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
    public class InfoTabFragment : Android.Support.V4.App.Fragment
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.Adapter mAdapter;
        private List<RestaurantInfo> mRestaurantInfo;
        string getID;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }
        public InfoTabFragment()
        {

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.InfoTab, container, false);
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView1);

            //getting the ID of the current restaurant displayed
            getID = RestaurantProfileActivity.sendData();
            if(getID != null)
            {
                restaurantInfo(getID);
            }
            else
            {
                Toast.MakeText(Context as Activity, "Something went wrong", ToastLength.Short).Show();
            }

            return view;
        }

        //getting the current restaurants general information
        async void restaurantInfo(string getID)
         {
            mRestaurantInfo = new List<RestaurantInfo>();

            string uri = "https://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/Restaurant/";

             string otherhalf = "HomeResults?ID= "+ getID ;

             Uri result = null;

             if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
             {
                 var httpClient = new HttpClient();
                 var refineResult = (await httpClient.GetStringAsync(result));
                 mRestaurantInfo = JsonConvert.DeserializeObject<List<RestaurantInfo>>(refineResult);

                //setting up the layout so the information can be displayed
                mLayoutManager = new LinearLayoutManager(Context as Activity);
                mRecyclerView.SetLayoutManager(mLayoutManager);
                
                //passing the restaurant's information to the adapter
                mAdapter = new RecyclerAdapter(mRestaurantInfo);
                mRecyclerView.SetAdapter(mAdapter); 

             }

         }
    }

    //Binding restaurant info with item views in recyclerview
    public class RecyclerAdapter : RecyclerView.Adapter
    {
        private List<RestaurantInfo> mRestaurantInfo;
        public RecyclerAdapter (List<RestaurantInfo> restaurantInfo)
        {
            mRestaurantInfo = restaurantInfo;
        }

        public class MyView : RecyclerView.ViewHolder
        {
            //getting and setting the restaurant information
            public View mMainView { get; set; }
            public TextView mDescription { get; set; }
            public TextView mOpenTimesMonday { get; set; }
            public TextView mCloseTimesMonday { get; set; }
            public TextView mOpenTimesTuesday { get; set; }
            public TextView mCloseTimesTuesday { get; set; }
            public TextView mOpenTimesWednesday { get; set; }
            public TextView mCloseTimesWednesday { get; set; }
            public TextView mOpenTimesThursday { get; set; }
            public TextView mCloseTimesThursday { get; set; }
            public TextView mOpenTimesFriday { get; set; }
            public TextView mCloseTimesFriday { get; set; }
            public TextView mOpenTimesSaturday { get; set; }
            public TextView mCloseTimesSaturday { get; set; }
            public TextView mOpenTimesSunday { get; set; }
            public TextView mCloseTimesSunday { get; set; }
            public TextView mContactNumber { get; set; }
            public TextView mAddress { get; set; }

            public MyView(View view) : base(view)
            {
                mMainView = view;
            }
        }
        public override int ItemCount
        {
            get { return mRestaurantInfo.Count; }
        }

        //Binding the data so that the information can be displayed in the recycler view within the view holder
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;
            myHolder.mDescription.Text = mRestaurantInfo[position].Description;

            myHolder.mOpenTimesMonday.Text = mRestaurantInfo[position].OpenTimesMonday;
            myHolder.mCloseTimesMonday.Text = mRestaurantInfo[position].CloseTimesMonday;
            myHolder.mOpenTimesTuesday.Text = mRestaurantInfo[position].OpenTimesTuesday;
            myHolder.mCloseTimesTuesday.Text = mRestaurantInfo[position].CloseTimesTuesday;
            myHolder.mOpenTimesWednesday.Text = mRestaurantInfo[position].OpenTimesWednesday;
            myHolder.mCloseTimesWednesday.Text = mRestaurantInfo[position].CloseTimesWednesday;
            myHolder.mOpenTimesThursday.Text = mRestaurantInfo[position].OpenTimesThursday;
            myHolder.mCloseTimesThursday.Text = mRestaurantInfo[position].CloseTimesThursday;
            myHolder.mOpenTimesFriday.Text = mRestaurantInfo[position].OpenTimesFriday;
            myHolder.mCloseTimesFriday.Text = mRestaurantInfo[position].CloseTimesFriday;
            myHolder.mOpenTimesSaturday.Text = mRestaurantInfo[position].OpenTimesSaturday;
            myHolder.mCloseTimesSaturday.Text = mRestaurantInfo[position].CloseTimesSaturday;
            myHolder.mOpenTimesSunday.Text = mRestaurantInfo[position].OpenTimesSunday;
            myHolder.mCloseTimesSunday.Text = mRestaurantInfo[position].CloseTimesSunday;

            myHolder.mContactNumber.Text = mRestaurantInfo[position].ContactTelephone;
            myHolder.mAddress.Text = mRestaurantInfo[position].Address;

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.infoLayout, parent, false);
            TextView textDescription = row.FindViewById<TextView>(Resource.Id.description);
            TextView textOpenTimesMonday = row.FindViewById<TextView>(Resource.Id.MondayOpen);
            TextView textCloseTimesMonday = row.FindViewById<TextView>(Resource.Id.MondayClose);
            TextView textOpenTimesTuesday = row.FindViewById<TextView>(Resource.Id.TuesdayOpen);
            TextView textCloseTimesTuesday = row.FindViewById<TextView>(Resource.Id.TuesdayClose);
            TextView textOpenTimesWednesday = row.FindViewById<TextView>(Resource.Id.WednesdayOpen);
            TextView textCloseTimesWednesday = row.FindViewById<TextView>(Resource.Id.WednesdayClose);
            TextView textOpenTimesThursday = row.FindViewById<TextView>(Resource.Id.ThursdayOpen);
            TextView textCloseTimesThursday = row.FindViewById<TextView>(Resource.Id.ThursdayClose);
            TextView textOpenTimesFriday = row.FindViewById<TextView>(Resource.Id.FridayOpen);
            TextView textCloseTimesFriday = row.FindViewById<TextView>(Resource.Id.FridayClose);
            TextView textOpenTimesSaturday = row.FindViewById<TextView>(Resource.Id.SaturdayOpen);
            TextView textCloseTimesSaturday = row.FindViewById<TextView>(Resource.Id.SaturdayClose);
            TextView textOpenTimesSunday = row.FindViewById<TextView>(Resource.Id.SundayOpen);
            TextView textCloseTimesSunday = row.FindViewById<TextView>(Resource.Id.SundayClose);
            TextView textContactNumber = row.FindViewById<TextView>(Resource.Id.ContactNumber);
            TextView textAddress = row.FindViewById<TextView>(Resource.Id.Address);

            //sets the restaurant information to the layout elements specified in the restaurant profile xaml file so all the info can be displayed
            MyView view = new MyView(row)
            {
                mDescription = textDescription,
                mOpenTimesMonday = textOpenTimesMonday,
                mCloseTimesMonday = textCloseTimesMonday,
                mOpenTimesTuesday = textOpenTimesTuesday,
                mCloseTimesTuesday = textCloseTimesTuesday,
                mOpenTimesWednesday = textOpenTimesWednesday,
                mCloseTimesWednesday = textCloseTimesWednesday,
                mOpenTimesThursday = textOpenTimesThursday,
                mCloseTimesThursday = textCloseTimesThursday,
                mOpenTimesFriday = textOpenTimesFriday,
                mCloseTimesFriday = textCloseTimesFriday,
                mOpenTimesSaturday = textOpenTimesSaturday,
                mCloseTimesSaturday = textCloseTimesSaturday,
                mOpenTimesSunday = textOpenTimesSunday,
                mCloseTimesSunday = textCloseTimesSunday,
                mContactNumber = textContactNumber,
                mAddress = textAddress
            };
            return view;
        }

    }
}