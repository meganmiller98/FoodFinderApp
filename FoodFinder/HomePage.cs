using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
 
using Android.Support.V7.Widget; 
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace FoodFinder
{
    public class HomePage : Fragment
    {
        public ListView mListView;
        public TextView test;
        public string mParam1;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
            
        }

        public HomePage()
        {

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.HomePage, container, false);
          
            Android.Support.V7.Widget.Toolbar toolbar = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar1);
            Button locationButton = view.FindViewById<Button>(Resource.Id.locationButton);
            Android.Support.V7.Widget.Toolbar toolbar3 = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar2);
            ImageButton refineButton = view.FindViewById<ImageButton>(Resource.Id.refineSearchButton);
            ImageButton searchButton = view.FindViewById<ImageButton>(Resource.Id.searchButton);
            
            //Android.Widget.SearchView searchView = view.FindViewById<Android.Widget.SearchView>(Resource.Id.searchView1);
            //searchView.OnActionViewExpanded();
            
            searchButton.Click += search_Click;

            ListView listview = (ListView)view.FindViewById(Resource.Id.myListView);
            test = view.FindViewById<TextView>(Resource.Id.test);

            if (Arguments != null)
            {
                String value = Arguments.GetString("sort");
                String value2 = Arguments.GetString("dietary");
                String value3 = Arguments.GetString("openNow");
                test.Text = value + " " + value2 + " " + value3;
                refineList(listview, value, value2, value3);
            }
            else
            {
                ExampleMethodAsync(listview);
            }
            refineButton.Click += button_Click;
            return view;
            
        }


        async void ExampleMethodAsync(ListView listview)
        {
            var httpClient = new HttpClient();
            //uni IP
            //var result = (await httpClient.GetStringAsync("htp://10.201.37.145:45455/api/mainmenu/dayOfWeek?lon=56.456388&lat=-2.982268"));
            //my flat IP
            var result = (await httpClient.GetStringAsync("http://192.168.0.20:45455/api/mainmenu/dayOfWeek?lon=56.456388&lat=-2.982268"));
            //Katy's IP
            //var result = (await httpClient.GetStringAsync("htp://192.168.1.70:45455/api/mainmenu/dayOfWeek?lon=56.456388&lat=-2.982268"));
            List<Post> RestaurantList = JsonConvert.DeserializeObject<List<Post>>(result);
            myRestaurantListViewAdapter adapter = new myRestaurantListViewAdapter(this.Context as Activity, RestaurantList);
            listview.Adapter = adapter;

        }

        async void refineList(ListView listview, string value, string value2, string value3)
        {
            //myIp
            string uri = "htp://192.168.0.20:45455/api/mainmenu/";

            //uni IP
            //string uri = "htp://10.201.37.145:45455/api/mainmenu/";

            //string uri = "http://192.168.1.70:45455/api/mainmenu/";
            string otherhalf = "refinements?sort=" + value + "&dietary=" + value2 + "&openNow=" + value3;

            Uri result = null;

            if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
            {
                var httpClient = new HttpClient();
                var refineResult = (await httpClient.GetStringAsync(result));
                List<Post> RestaurantList = JsonConvert.DeserializeObject<List<Post>>(refineResult);
                myRestaurantListViewAdapter adapter = new myRestaurantListViewAdapter(this.Context as Activity, RestaurantList);
                listview.Adapter = adapter;

                //test.Text = result.AbsoluteUri;
            }

        }
        void button_Click(object sender, EventArgs e)
        {
            //show fragment
            FragmentTransaction transcation = FragmentManager.BeginTransaction();
            FragmentDialogClass refineDialog = new FragmentDialogClass();
            refineDialog.Show(transcation, "FragmentDialog");

        }

       void search_Click(object sender, EventArgs e)
        {
            FragmentTransaction fragmentTransaction = this.Activity.FragmentManager.BeginTransaction();

            //SearchFragment prof = new SearchFragment();
            //FragmentTest prof = new FragmentTest();
            SearchFragmentActual prof = new SearchFragmentActual();

            // The fragment will have the ID of Resource.Id.fragment_container.
            fragmentTransaction.Replace(Resource.Id.frame, prof);
            fragmentTransaction.AddToBackStack("HomePage");

            // Commit the transaction.
            fragmentTransaction.Commit();
            
        }
      
    }
}