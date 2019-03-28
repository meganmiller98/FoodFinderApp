using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Essentials;

namespace FoodFinder
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, WindowSoftInputMode = SoftInput.AdjustPan)]
    
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener 
    {
        TextView textMessage;
       //public ListView mListView;
        HomePage homePage = new HomePage();
        VoucherPage voucherPage = new VoucherPage();
        ProfilePage profilePage = new ProfilePage();
        SearchFragment searchFragment = new SearchFragment();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            setFragment(homePage);
            textMessage = FindViewById<TextView>(Resource.Id.message);
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    setFragment(homePage);
                    return true;
                case Resource.Id.navigation_dashboard:
                    setFragment(voucherPage);
                    return true;
                case Resource.Id.navigation_notifications:
                    setFragment(profilePage);
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
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

