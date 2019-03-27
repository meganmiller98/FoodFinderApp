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
    class SearchFragment : Fragment
    {
        //public ListView mListView;
        public TextView test;
        
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here

        }

        public SearchFragment()
        {

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //View view = inflater.Inflate(Resource.Layout.searchPage, container, false);
            View view = inflater.Inflate(Resource.Layout.searchPage, container, false);

            Android.Support.V7.Widget.Toolbar toolbar = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar1);
            Button locationButton = view.FindViewById<Button>(Resource.Id.locationButton);
            Android.Support.V7.Widget.Toolbar toolbar3 = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar2);
            TextView text1 = view.FindViewById<TextView>(Resource.Id.textView1);
            //Android.Widget.SearchView searchView = view.FindViewById<Android.Widget.SearchView>(Resource.Id.searchView);
            //Android.Widget.SearchView searchView = view.FindViewById<Android.Widget.SearchView>(Resource.Id.searchView1);
            //searchView.OnActionViewExpanded();

            //searchView.Click += search_Click;

            //ListView listview = (ListView)view.FindViewById(Resource.Id.myListView);
            test = view.FindViewById<TextView>(Resource.Id.test);
            return view;

        }

        /*edittext.KeyPress += (object sender, View.KeyEventArgs e) => {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    categoryTextView.Text = "chicken";
                    dishesSearchOption.Text = "hello";
                    Toast.MakeText(Context as Activity, edittext.Text, ToastLength.Short).Show();
                    e.Handled = true;
                }
            };*/

        /* bool  TextView.IOnEditorActionListener.OnEditorAction(TextView v, ImeAction actionId, KeyEvent e)
        {
            bool handled = false;
            if (actionId == Android.Views.InputMethods.ImeAction.Done)
            {
                string result = edittext.Text.ToString();
                categoryTextView.Text = result;
                handled = true;
            }
            return handled;
        }*/
    }
}