using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

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
    public class searchResultsPage : Fragment
    {
        string option;
        string searchedString;
        TextView test;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.searchResultsPage, container, false);

            Android.Support.V7.Widget.Toolbar toolbar3 = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar2);
            ImageButton refineButton = view.FindViewById<ImageButton>(Resource.Id.refineSearchButton);

            ListView listview = (ListView)view.FindViewById(Resource.Id.myListView);

            test = view.FindViewById<TextView>(Resource.Id.test);

            if (Arguments != null)
            {
                option = Arguments.GetString("option");
                searchedString = Arguments.GetString("searchedString");
                //String value3 = Arguments.GetString("openNow");
                test.Text = option + " " + searchedString ;

                if (option == "category")
                {
                    ShowCategoryResults(searchedString, listview);
                }
                else if(option == "cuisine")
                {

                }
                else if (option == "dishes")
                {

                }
            }
            else
            {
                test.Text = "No Results!";
            }
            return view; 
        }

        async void ShowCategoryResults(string searchedString, ListView listview)
        {
            string value = "56.456388";
            string value2 = "-2.982268";
            //myIp
            string uri = "http://192.168.0.20:45455/api/mainmenu/";

            //uni IP
            //string uri = "htp://10.201.37.145:45455/api/mainmenu/";

            //string uri = "htp://192.168.1.70:45455/api/mainmenu/";
            string otherhalf = "GetRestaurantsAccordingToCategories?lon=" + value + "&lat=" + value2 + "&category=" + searchedString;

            Uri result = null;

            if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
            {
                var httpClient = new HttpClient();
                var refineResult = (await httpClient.GetStringAsync(result));
                List<Post> RestaurantList = JsonConvert.DeserializeObject<List<Post>>(refineResult);
                if (RestaurantList.Count == 0)
                {
                    test.Text = "Sorry, no restaurants nearby with category '" + searchedString + "'";
                }
                else
                {
                    test.Text = "Restaurants nearby with the category '" + searchedString + "'";
                    myRestaurantListViewAdapter adapter = new myRestaurantListViewAdapter(this.Context as Activity, RestaurantList);
                    listview.Adapter = adapter;
                }

                //test.Text = result.AbsoluteUri;
            }

        }
    }
    
}