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
using Xamarin.Essentials;

namespace FoodFinder
{
    public class searchResultsPage : Fragment
    {
        string option;
        string searchedString;
        string lon;
        string lat;
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

            getLastKnownLocation();

            if (Arguments != null)
            {
                option = Arguments.GetString("option");
                searchedString = Arguments.GetString("searchedString");
                //String value3 = Arguments.GetString("openNow");
                test.Text = option + " " + searchedString;

                if(Arguments.GetString("sort") != null && Arguments.GetString("dietary") != null && Arguments.GetString("openNow") != null && option == "category")
                {
                    test.Text = "sort stuff please";
                }
                else if (option == "category")
                {
                    ShowCategoryResults(searchedString, listview);
                }
                else if (option == "cuisine")
                {
                    ShowCuisineResults(searchedString, listview);
                }
                else if (option == "dishes")
                {
                    ShowDishesResults(searchedString, listview);
                }
            }
            else
            {
                test.Text = "No Results!";
            }
            refineButton.Click += button_Click; 
            return view;
        }

        async void ShowCategoryResults(string searchedString, ListView listview)
        {
            //myIp
            string uri = "http://192.168.0.20:45455/api/mainmenu/";

            //uni IP
            //string uri = "htp://10.201.37.145:45455/api/mainmenu/";

            //string uri = "htp://192.168.1.70:45455/api/mainmenu/";
            string otherhalf = "GetRestaurantsAccordingToCategories?lon=" + lon + "&lat=" + lat + "&category=" + searchedString;

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


        async void ShowCuisineResults(string searchedString, ListView listview)
        {
            //myIp
            string uri = "http://192.168.0.20:45455/api/mainmenu/";

            //uni IP
            //string uri = "htp://10.201.37.145:45455/api/mainmenu/";

            //string uri = "htp://192.168.1.70:45455/api/mainmenu/";
            string otherhalf = "GetRestaurantsAccordingToCuisines?lon=" + lon + "&lat=" + lat + "&cuisine=" + searchedString;

            Uri result = null;

            if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
            {
                var httpClient = new HttpClient();
                var refineResult = (await httpClient.GetStringAsync(result));
                List<Post> RestaurantList = JsonConvert.DeserializeObject<List<Post>>(refineResult);
                if (RestaurantList.Count == 0)
                {
                    test.Text = "Sorry, no restaurants nearby with cuisine '" + searchedString + "'";
                }
                else
                {
                    test.Text = "Restaurants nearby with '" + searchedString + "' cuisine";
                    myRestaurantListViewAdapter adapter = new myRestaurantListViewAdapter(this.Context as Activity, RestaurantList);
                    listview.Adapter = adapter;
                }

                //test.Text = result.AbsoluteUri;
            }
        }

        async void ShowDishesResults(string searchedString, ListView listview)
        {
            //myIp
            string uri = "http://192.168.0.20:45455/api/mainmenu/";

            //uni IP
            //string uri = "htp://10.201.37.145:45455/api/mainmenu/";

            //string uri = "htp://192.168.1.70:45455/api/mainmenu/";
            string otherhalf = "GetRestaurantsAccordingToDish?lon=" + lon + "&lat=" + lat + "&dish=" + searchedString;

            Uri result = null;

            if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
            {
                var httpClient = new HttpClient();
                var refineResult = (await httpClient.GetStringAsync(result));
                List<Post> RestaurantList = JsonConvert.DeserializeObject<List<Post>>(refineResult);
                if (RestaurantList.Count == 0)
                {
                    test.Text = "Sorry, no restaurants nearby with dish '" + searchedString + "'";
                }
                else
                {
                    test.Text = "Restaurants nearby with dish '" + searchedString + "'";
                    myRestaurantListViewAdapter adapter = new myRestaurantListViewAdapter(this.Context as Activity, RestaurantList);
                    listview.Adapter = adapter;
                }

                //test.Text = result.AbsoluteUri;
            }
        }

        void button_Click(object sender, EventArgs e)
        {
            //show fragment
            searchRefineDialog refineDialog = new searchRefineDialog();
            Bundle args = new Bundle();
            args.PutString("option", option);
            args.PutString("searchedString", searchedString);
            refineDialog.Arguments = args;
            FragmentTransaction transcation = FragmentManager.BeginTransaction();
            refineDialog.Show(transcation, "searchRefineDialog");

        }

        async void getLastKnownLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    lat = location.Latitude.ToString();
                    lon = location.Longitude.ToString();
                    //test.Text = location.Latitude.ToString() + " " + location.Longitude.ToString();
                }
                else
                {
                    test.Text = "nothing to locate";
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                AlertDialog.Builder alert = new AlertDialog.Builder(Context as Activity);
                alert.SetTitle("Failed");
                alert.SetMessage(fnsEx.ToString());
                alert.SetPositiveButton("Okay", (senderAlert, args) => {
                    Toast.MakeText(Context as Activity, "Okay", ToastLength.Short).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();

            }
            catch (FeatureNotEnabledException fneEx)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(Context as Activity);
                alert.SetTitle("Failed");
                alert.SetMessage(fneEx.ToString());
                alert.SetPositiveButton("Okay", (senderAlert, args) => {
                    Toast.MakeText(Context as Activity, "Okay", ToastLength.Short).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
            catch (PermissionException pEx)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(Context as Activity);
                alert.SetTitle("Failed");
                alert.SetMessage(pEx.ToString());
                alert.SetPositiveButton("Okay", (senderAlert, args) => {
                    Toast.MakeText(Context as Activity, "Okay", ToastLength.Short).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
            catch (Exception ex)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(Context as Activity);
                alert.SetTitle("Failed");
                alert.SetMessage(ex.ToString());
                alert.SetPositiveButton("Okay", (senderAlert, args) => {
                    Toast.MakeText(Context as Activity, "Okay", ToastLength.Short).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
        }
    }
}