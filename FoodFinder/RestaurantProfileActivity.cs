using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace FoodFinder
{
    [Activity(Label = "RestaurantProfileActivity")]
    public class RestaurantProfileActivity : AppCompatActivity
    {
        public static string ID;
        ImageButton saveButton;
        List<favedRestaurants> mFavedRestaurants;
        //private string mID;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.RestaurantProfileLayout);
            // Create your application here
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            Android.Support.V7.Widget.Toolbar toolbarNav = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarNav);
            ImageView imageView = FindViewById<ImageView>(Resource.Id.imageView);
            TextView textview1 = FindViewById<TextView>(Resource.Id.textView1);
            TextView textview2 = FindViewById<TextView>(Resource.Id.textView2);
            TextView textview3 = FindViewById<TextView>(Resource.Id.textView3);
            RatingBar ratingBar = FindViewById<RatingBar>(Resource.Id.ratingBar1);
            Android.Support.Design.Widget.TabLayout tabLayout = FindViewById<Android.Support.Design.Widget.TabLayout>(Resource.Id.tabLayout);
            saveButton = FindViewById<ImageButton>(Resource.Id.imageButton1);
            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);

            var restaurantInfo = JsonConvert.DeserializeObject<Post>(Intent.GetStringExtra("RestaurantInfo"));

            var imageBitmap = ImageHelper.GetImageBitmapFromUrl(restaurantInfo.MainPhoto1);
            imageView.SetImageBitmap(imageBitmap);

            saveButton.Click += saveButtonClick;

            textview1.Text = restaurantInfo.RestaurantName;
            if (restaurantInfo.Cost == "1")
            {
                textview2.Text = "£";
            }
            else if (restaurantInfo.Cost == "2")
            {
                textview2.Text = "££";
            }
            else if (restaurantInfo.Cost == "3")
            {
                textview2.Text = "£££";
            }

            if (restaurantInfo.Cuisines == null && restaurantInfo.Categories == null)
            {
                textview3.Text = "";
            }
            else if (restaurantInfo.Cuisines != null && restaurantInfo.Categories == null)
            {
                textview3.Text = restaurantInfo.Cuisines;
            }
            else if (restaurantInfo.Cuisines == null && restaurantInfo.Categories != null)
            {
                textview3.Text = restaurantInfo.Categories;
            }
            else if (restaurantInfo.Cuisines != null && restaurantInfo.Categories != null)
            {
                textview3.Text = restaurantInfo.Cuisines + " " + restaurantInfo.Categories;
            }

            ID = restaurantInfo.ID;

            SetSupportActionBar(toolbarNav);
            SetupViewPager(viewPager);
            
            tabLayout.SetupWithViewPager(viewPager);
            //viewPager.Adapter = new MyFragmentAdapter(SupportFragmentManager, 4);

            viewPager.AddOnPageChangeListener(new TabLayout.TabLayoutOnPageChangeListener(tabLayout));
            checkIfUserHasSavedRestaurant(ID);

        }
        async void checkIfUserHasSavedRestaurant(string ID)
        {
            ISharedPreferences prefs = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string userID = prefs.GetString("userID", null);

            if (userID == null)
            {
                Toast.MakeText(this, "User Not Logged In", ToastLength.Short).Show();
            }
            else
            {
                mFavedRestaurants = new List<favedRestaurants>();
                //myIp
                //string uri = "htp://192.168.0.20:45455/api/Ratings/";

                //uni IP
                //string uri = "htp://10.201.37.145:45455/api/mainmenu/";

                string uri = "http://192.168.1.70:45455/api/favouriteRestaurants/";

                string otherhalf = "checkIfSaved?userID=" + userID + "&restaurantID=" + ID;
                //string otherhalf = "checkIfSaved?userID=1&restaurantID=1";
                Uri result = null;

                if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
                {
                    var httpClient = new HttpClient();
                    var refineResult = (await httpClient.GetStringAsync(result));
                    mFavedRestaurants = JsonConvert.DeserializeObject<List<favedRestaurants>>(refineResult);

                    if(mFavedRestaurants.Count > 0)
                    {
                        saveButton.Selected = true;
                    }
                    else
                    {
                        saveButton.Selected = false;
                    }
                }
            }
           
        }
        void SetupViewPager(ViewPager viewPage)
        {
            var adapter = new Adapter(SupportFragmentManager);
            adapter.AddFragment(new InfoTabFragment(), "Info");
            adapter.AddFragment(new MenuTabFragment(), "Menu");
            adapter.AddFragment(new PhotosFragment(), "Photos");
            adapter.AddFragment(new RatingsFragment(), "Ratings");
            viewPage.Adapter = adapter;
            //viewPager.Adapter.NotifyDataSetChanged();
        }
        public static string sendData()
        {
            return ID;
        }
        void saveButtonClick(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Click", ToastLength.Short).Show();
            ISharedPreferences prefs = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string userID = prefs.GetString("userID", null);
            if (saveButton.Selected == true)
            {
                saveButton.Selected = false;
                Toast.MakeText(this, "true to false", ToastLength.Short).Show();
                

                if (userID == null)
                {
                    Toast.MakeText(this, "User Not Logged In", ToastLength.Short).Show();
                }
                else
                {
                    deleteSavedRestaurant(userID);
                }
            }
            else
            {
                saveButton.Selected = true;
                Toast.MakeText(this, "false to true", ToastLength.Short).Show();
                if (userID == null)
                {
                    Toast.MakeText(this, "User Not Logged In", ToastLength.Short).Show();
                }
                else
                {
                    saveRestaurant(userID);
                }
            }
        }
        async void deleteSavedRestaurant(string userID)
        {
            string uri = "http://192.168.1.70:45455/api/favouriteRestaurants/";

            string otherhalf = "deleteSaved?userID=" + userID + "&restaurantID=" + ID;
            //string otherhalf = "checkIfSaved?userID=1&restaurantID=1";
            Uri result = null;

            if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
            {
                var httpClient = new HttpClient();
                var refineResult = (await httpClient.DeleteAsync(result));
                if (refineResult.IsSuccessStatusCode)
                {
                    Console.WriteLine("successfully deleted.");
                    Toast.MakeText(this, "Removed from favourites", ToastLength.Short).Show();
                }
            }
        }

        async void saveRestaurant(string userID)
        {
            favedRestaurants fav = new favedRestaurants(userID, ID, null);
            
            string uri = "http://192.168.1.70:45455/api/favouriteRestaurants/Save";

            Uri result = new Uri(uri);
            Console.WriteLine(result);
            var httpClient = new HttpClient();

            var content = new StringContent(JsonConvert.SerializeObject(fav));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage refineResult = (await httpClient.PostAsync(result, content));

            //await HandleResponse(refineResult);
            string serialized = await refineResult.Content.ReadAsStringAsync();
            Console.WriteLine(serialized);
            if (refineResult.IsSuccessStatusCode)
            {
                Toast.MakeText(this, "Added to favourites", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Something went wrong", ToastLength.Short).Show();
            }
        }
    }

    public class Adapter : FragmentPagerAdapter
    {
        List<Android.Support.V4.App.Fragment> fragments = new List<Android.Support.V4.App.Fragment>();
        List<string> fragmentTitles = new List<string>();
        public Adapter(Android.Support.V4.App.FragmentManager fm) : base(fm) { }
        public void AddFragment(Android.Support.V4.App.Fragment fragment, String title)
        {
            fragments.Add(fragment);
            fragmentTitles.Add(title);
        }
        public override int Count { get { return fragments.Count; } }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return fragments[position];
        }
        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(fragmentTitles[position]);
        }
        
    }
}