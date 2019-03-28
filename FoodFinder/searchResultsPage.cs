using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace FoodFinder
{
    public class searchResultsPage : Fragment
    {
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

            TextView test = view.FindViewById<TextView>(Resource.Id.test);

            if (Arguments != null)
            {
                String value = Arguments.GetString("option");
                String value2 = Arguments.GetString("searchedString");
                //String value3 = Arguments.GetString("openNow");
                test.Text = value + " " + value2 ;
            }

            return view; 
        }
    }
}