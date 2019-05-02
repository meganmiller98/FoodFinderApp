using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Essentials;

namespace FoodFinder
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false, WindowSoftInputMode = SoftInput.AdjustPan, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener 
    {
        TextView textMessage;
        HomePage homePage = new HomePage();
        VoucherPage voucherPage = new VoucherPage();
        ProfilePage profilePage = new ProfilePage();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);

            updateRatings();

            deleteExpiredVoucher();

            //On activity refresh check if the profile page should be displayed
            if (Intent.GetStringExtra("frgToLoad") != null)
            {
                navigation.Menu.GetItem(2).SetChecked(true);
                setFragment(profilePage);
            }
            else
            {
                setFragment(homePage);
            }

            textMessage = FindViewById<TextView>(Resource.Id.message);
            
            navigation.SetOnNavigationItemSelectedListener(this);

           

        }

        //changing fragments on nav bar clicks
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    setFragment(homePage);
                    Console.WriteLine("This is the item " + item);

                    return true;
                case Resource.Id.navigation_dashboard:
                    setFragment(voucherPage);
                    Console.WriteLine("This is the item " + item);
                    return true;
                case Resource.Id.navigation_notifications:
                    setFragment(profilePage);
                    Console.WriteLine("This is the item " + item);
                    return true;
            }
            return false;
        }

        //From Xamarin Fragment info page
        private void setFragment(Fragment fragment)
        {
            FragmentTransaction fragmentTransaction = this.FragmentManager.BeginTransaction();

            // The fragment will have the ID of Resource.Id.fragment_container.
            fragmentTransaction.Replace(Resource.Id.frame, fragment);

            // Commit the transaction.
            fragmentTransaction.Commit();
        }

        //Found in https docs.microsoft.com/en-gb/xamarin/essentials/get-started?tabs=windows%2Candroid
        //grant app permission to use device's location
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        //update ratings of restaurants when application starts
        async void updateRatings()
        {
            Post updateRatings = new Post(null, null, null, null, null, null, null, null, null, null, null);
            
            string uri = "https://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/mainmenu/ratingsTest";
           
            Uri result = new Uri(uri);
            Console.WriteLine(result);
            var httpClient = new HttpClient();

            var content = new StringContent(JsonConvert.SerializeObject(updateRatings));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage refineResult = (await httpClient.PostAsync(result, content));

            string serialized = await refineResult.Content.ReadAsStringAsync();
            Console.WriteLine(serialized);
            if (refineResult.IsSuccessStatusCode)
            {
                Console.WriteLine("ratings updated");
            }
            else
            {
                Console.WriteLine("ratings not updated");
            }
        }

        //Delete any vouchers that have expired
        async void deleteExpiredVoucher()
        {
            
            string uri = "https://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/Voucher/deleteExpiredVouchers";
            Uri result = new Uri(uri);

            var httpClient = new HttpClient();
            var refineResult = (await httpClient.DeleteAsync(result));
            if (refineResult.IsSuccessStatusCode)
            {
                Console.WriteLine("successfully deleted.");
            }
            else
            {
                Console.WriteLine("Something went wrong");
            }
            
        }
    }
}

