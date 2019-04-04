using System;
using System.Collections.Generic;
using System.Linq;
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
            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);

            

            var restaurantInfo = JsonConvert.DeserializeObject<Post>(Intent.GetStringExtra("RestaurantInfo"));

            var imageBitmap = ImageHelper.GetImageBitmapFromUrl(restaurantInfo.MainPhoto1);
            imageView.SetImageBitmap(imageBitmap);

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

            ID = "1";
            
            SetSupportActionBar(toolbarNav);
            SetupViewPager(viewPager);
            
            tabLayout.SetupWithViewPager(viewPager);
            //viewPager.Adapter = new MyFragmentAdapter(SupportFragmentManager, 4);

            viewPager.AddOnPageChangeListener(new TabLayout.TabLayoutOnPageChangeListener(tabLayout));
            
            
        }
        void SetupViewPager(ViewPager viewPage)
        {
            var adapter = new Adapter(SupportFragmentManager);
            adapter.AddFragment(new MenuTabFragment(), "MenuTab");
            adapter.AddFragment(new InfoTabFragment(), "InfoTab");
            viewPage.Adapter = adapter;
            //viewPager.Adapter.NotifyDataSetChanged();
        }
        public static string sendData()
        {
            return ID;
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