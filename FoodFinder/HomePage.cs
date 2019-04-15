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
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace FoodFinder
{
    public class HomePage : Fragment
    {
        public ListView mListView;
        public TextView test;
        public string mParam1;
        string lon;
        string lat;
        string sort;
        string dietary;
        string openNow;
        //private List<Post> RestaurantList;

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
            getLocation();
            getLastKnownLocation();
            //test.Text = lat + lon;
            if (Arguments != null)
            {
                sort = Arguments.GetString("sort");
                dietary = Arguments.GetString("dietary");
                openNow = Arguments.GetString("openNow");
                //test.Text = sort + " " + dietary + " " + openNow;
                refineList(listview, sort, dietary, openNow);
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
            //my
            //string uri = "htp://192.168.0.20:45455/api/mainmenu/";
            //uni
            string uri = "http://10.201.37.145:45455/api/mainmenu/";
            //Katy
            //string uri = "htp://192.168.1.70:45455/api/mainmenu/";
            //string uri = "htps://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/mainmenu/";
            string otherhalf = "HomeResults?lat="+lat+"&lon="+lon;
            //test.Text = otherhalf;
            Uri result = null;

            if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
            {
                var httpClient = new HttpClient();
                var refineResult = (await httpClient.GetStringAsync(result));
                List<Post> RestaurantList = JsonConvert.DeserializeObject<List<Post>>(refineResult);
                if (RestaurantList.Count == 0)
                {
                    test.Text = "Sorry, no restaurants nearby";
                }
                else
                {
                    myRestaurantListViewAdapter adapter = new myRestaurantListViewAdapter(this.Context as Activity, RestaurantList);
                    listview.Adapter = adapter;
                    //test.Text = result.AbsoluteUri;

                    listview.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
                    {
                        string restName = RestaurantList[e.Position].RestaurantName;
                        Toast.MakeText(Context as Activity, restName, ToastLength.Short).Show();
                        Intent intent = new Intent(Context as Activity, typeof(RestaurantProfileActivity));
                        intent.PutExtra("RestaurantInfo", JsonConvert.SerializeObject(RestaurantList[e.Position]));
                        StartActivity(intent);
                    };
                }

            }
        }


        async void refineList(ListView listview, string value, string value2, string value3)
        {
            //myIp
            //string uri = "htp://192.168.0.20:45455/api/mainmenu/";

            //uni IP
           string uri = "http://10.201.37.145:45455/api/mainmenu/";

            //string uri = "htp://192.168.1.70:45455/api/mainmenu/";
            //string uri = "htps://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/mainmenu/";

            string otherhalf = "refinements2?lat=" + lat + "&lon="+ lon + "&sort=" + value + "&dietary=" + value2 + "&openNow=" + value3;

            Uri result = null;

            if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
            {
                var httpClient = new HttpClient();
                var refineResult = (await httpClient.GetStringAsync(result));
                List<Post> RestaurantList = JsonConvert.DeserializeObject<List<Post>>(refineResult);
                if (RestaurantList.Count == 0)
                {
                    test.Text = "Sorry, no results";
                }
                else
                {
                    myRestaurantListViewAdapter adapter = new myRestaurantListViewAdapter(this.Context as Activity, RestaurantList);
                    listview.Adapter = adapter;

                    listview.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
                    {
                        string restName = RestaurantList[e.Position].RestaurantName;
                        Toast.MakeText(Context as Activity, restName, ToastLength.Short).Show();
                        Intent intent = new Intent(Context as Activity, typeof(RestaurantProfileActivity));
                        intent.PutExtra("RestaurantInfo", JsonConvert.SerializeObject(RestaurantList[e.Position]));
                        StartActivity(intent);
                    };
                }

            }

        }
        void button_Click(object sender, EventArgs e)
        {
            //show fragment
            FragmentDialogClass refineDialog = new FragmentDialogClass();
            Bundle args = new Bundle();
            args.PutString("sort", sort);
            args.PutString("dietary", dietary);
            args.PutString("openNow", openNow);
            refineDialog.Arguments = args;
            FragmentTransaction transcation = FragmentManager.BeginTransaction();
            refineDialog.Show(transcation, "FragmentDialog");
        }

       void search_Click(object sender, EventArgs e)
        {
            FragmentTransaction fragmentTransaction = this.Activity.FragmentManager.BeginTransaction();
            SearchFragmentActual prof = new SearchFragmentActual();
            fragmentTransaction.Replace(Resource.Id.frame, prof);
            fragmentTransaction.AddToBackStack("HomePage");
            fragmentTransaction.Commit();
            
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
                    test.Text = "Can't find location";
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

        private Task AlertDialog(string v1, string message, string v2)
        {
            throw new NotImplementedException();
        }

        async void getLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {

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