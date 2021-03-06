﻿using System;
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
        string sort;
        string dietary;
        string openNow;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.searchResultsPage, container, false);

            Android.Support.V7.Widget.Toolbar toolbar3 = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar2);
            ImageButton refineButton = view.FindViewById<ImageButton>(Resource.Id.refineSearchButton);

            ListView listview = (ListView)view.FindViewById(Resource.Id.myListView);

            test = view.FindViewById<TextView>(Resource.Id.test);

            getLastKnownLocation();

            //check if their are any additional refinements to the search then execute query according to chosen search by category.
            if (Arguments != null)
            {
                option = Arguments.GetString("option");
                searchedString = Arguments.GetString("searchedString");
                test.Text = option + " " + searchedString;

                if(Arguments.GetString("sort") != null && Arguments.GetString("dietary") != null && Arguments.GetString("openNow") != null && option == "category")
                {
                    sort = Arguments.GetString("sort");
                    dietary = Arguments.GetString("dietary");
                    openNow = Arguments.GetString("openNow");

                    refineCategoryResults(listview);

                }
                else if(Arguments.GetString("sort") != null && Arguments.GetString("dietary") != null && Arguments.GetString("openNow") != null && option == "cuisine")
                {
                    sort = Arguments.GetString("sort");
                    dietary = Arguments.GetString("dietary");
                    openNow = Arguments.GetString("openNow");

                    refineCuisineResults(listview);

                }
                else if (Arguments.GetString("sort") != null && Arguments.GetString("dietary") != null && Arguments.GetString("openNow") != null && option == "dishes")
                {
                    sort = Arguments.GetString("sort");
                    dietary = Arguments.GetString("dietary");
                    openNow = Arguments.GetString("openNow");

                    refineDishResults(listview);

                }
                else if  (Arguments.GetString("sort") == null && option == "category")
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

            string uri = "https://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/mainmenu/";
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
                    test.Text = "Restaurants nearby with the category '" + searchedString + " "+sort +"'";
                    myRestaurantListViewAdapter adapter = new myRestaurantListViewAdapter(this.Context as Activity, RestaurantList);
                    listview.Adapter = adapter;
                    listview.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
                    {
                        string restName = RestaurantList[e.Position].RestaurantName;
                        Intent intent = new Intent(Context as Activity, typeof(RestaurantProfileActivity));
                        intent.PutExtra("RestaurantInfo", JsonConvert.SerializeObject(RestaurantList[e.Position]));
                        StartActivity(intent);
                    };
                }

            }
        }


        async void ShowCuisineResults(string searchedString, ListView listview)
        {
            string uri = "https://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/mainmenu/";
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
                    listview.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
                    {
                        string restName = RestaurantList[e.Position].RestaurantName;
                        //Toast.MakeText(Context as Activity, restName, ToastLength.Short).Show();
                        Intent intent = new Intent(Context as Activity, typeof(RestaurantProfileActivity));
                        intent.PutExtra("RestaurantInfo", JsonConvert.SerializeObject(RestaurantList[e.Position]));
                        StartActivity(intent);
                    };
                }

            }
        }

        async void ShowDishesResults(string searchedString, ListView listview)
        {
            string uri = "https://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/mainmenu/";
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
                    listview.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
                    {
                        string restName = RestaurantList[e.Position].RestaurantName;
                       // Toast.MakeText(Context as Activity, restName, ToastLength.Short).Show();
                        Intent intent = new Intent(Context as Activity, typeof(RestaurantProfileActivity));
                        intent.PutExtra("RestaurantInfo", JsonConvert.SerializeObject(RestaurantList[e.Position]));
                        StartActivity(intent);
                    };
                }

            }
        }

        async void refineCategoryResults(ListView listview)
        {

            string uri = "https://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/mainmenu/";
            string otherhalf = "CategoryRefinements?lat=" + lat + "&lon=" + lon + "&category=" + searchedString + "&sort=" +sort+ "&dietary=" +dietary+ "&openNow=" +openNow;

            Uri result = null;

            if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
            {
                var httpClient = new HttpClient();
                var refineResult = (await httpClient.GetStringAsync(result));
                List<Post> RestaurantList = JsonConvert.DeserializeObject<List<Post>>(refineResult);
                if (RestaurantList.Count == 0)
                {
                    test.Text = "Sorry, no restaurants nearby with those refinements";
                }
                else
                {
                    test.Text = "Restaurants nearby with the category '" + searchedString +"'";
                    myRestaurantListViewAdapter adapter = new myRestaurantListViewAdapter(this.Context as Activity, RestaurantList);
                    listview.Adapter = adapter;
                    listview.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
                    {
                        string restName = RestaurantList[e.Position].RestaurantName;
                        //Toast.MakeText(Context as Activity, restName, ToastLength.Short).Show();
                        Intent intent = new Intent(Context as Activity, typeof(RestaurantProfileActivity));
                        intent.PutExtra("RestaurantInfo", JsonConvert.SerializeObject(RestaurantList[e.Position]));
                        StartActivity(intent);
                    };
                }
            }
        }

        async void refineCuisineResults(ListView listview)
        {
            string uri = "https://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/mainmenu/";

            string otherhalf = "CuisineRefinements?lat=" + lat + "&lon=" + lon + "&cuisine=" + searchedString + "&sort=" + sort + "&dietary=" + dietary + "&openNow=" + openNow;

            Uri result = null;

            if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
            {
                var httpClient = new HttpClient();
                var refineResult = (await httpClient.GetStringAsync(result));
                List<Post> RestaurantList = JsonConvert.DeserializeObject<List<Post>>(refineResult);
                if (RestaurantList.Count == 0)
                {
                    test.Text = "Sorry, no restaurants nearby with those refinements";
                }
                else
                {
                    test.Text = "Restaurants nearby with the cuisine '" + searchedString + "'";
                    myRestaurantListViewAdapter adapter = new myRestaurantListViewAdapter(this.Context as Activity, RestaurantList);
                    listview.Adapter = adapter;
                    listview.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
                    {
                        string restName = RestaurantList[e.Position].RestaurantName;
                        //Toast.MakeText(Context as Activity, restName, ToastLength.Short).Show();
                        Intent intent = new Intent(Context as Activity, typeof(RestaurantProfileActivity));
                        intent.PutExtra("RestaurantInfo", JsonConvert.SerializeObject(RestaurantList[e.Position]));
                        StartActivity(intent);
                    };
                }

                //test.Text = result.AbsoluteUri;
            }
        }

        async void refineDishResults(ListView listview)
        {
            string uri = "https://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/mainmenu/";
            string otherhalf = "DishRefinements?lat=" + lat + "&lon=" + lon + "&dish=" + searchedString + "&sort=" + sort + "&dietary=" + dietary + "&openNow=" + openNow;

            Uri result = null;

            if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
            {
                var httpClient = new HttpClient();
                var refineResult = (await httpClient.GetStringAsync(result));
                List<Post> RestaurantList = JsonConvert.DeserializeObject<List<Post>>(refineResult);
                if (RestaurantList.Count == 0)
                {
                    test.Text = "Sorry, no restaurants nearby with those refinements";
                }
                else
                {
                    test.Text = "Restaurants nearby with the dish refine '" + searchedString + "'";
                    myRestaurantListViewAdapter adapter = new myRestaurantListViewAdapter(this.Context as Activity, RestaurantList);
                    listview.Adapter = adapter;
                    listview.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
                    {
                        string restName = RestaurantList[e.Position].RestaurantName;
                        //Toast.MakeText(Context as Activity, restName, ToastLength.Short).Show();
                        Intent intent = new Intent(Context as Activity, typeof(RestaurantProfileActivity));
                        intent.PutExtra("RestaurantInfo", JsonConvert.SerializeObject(RestaurantList[e.Position]));
                        StartActivity(intent);
                    };
                }
                
            }
        }

        void button_Click(object sender, EventArgs e)
        {
            //show refinemenet dialog fragment
            searchRefineDialog refineDialog = new searchRefineDialog();
            Bundle args = new Bundle();
            args.PutString("sort", sort);
            args.PutString("dietary", dietary);
            args.PutString("openNow", openNow);
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