﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using static Android.Resource;
using static Android.Views.View;

namespace FoodFinder
{
    public class SearchFragmentActual : Fragment 
    {
        //TextView categoryTextView;
        EditText edittext;
        TextView dishesSearchOption;
        TextView text;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here

        }

        public SearchFragmentActual()
        {

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.searchPage, container, false);

            Android.Support.V7.Widget.Toolbar toolbar = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar1);
            Button locationButton = view.FindViewById<Button>(Resource.Id.locationButton);

            Android.Support.V7.Widget.Toolbar toolbar2 = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar2);
            edittext = view.FindViewById<EditText>(Resource.Id.edittext);

            TextView categoryTextView = view.FindViewById<TextView>(Resource.Id.categoryTextView);
            TextView categorySearchOption = view.FindViewById<TextView>(Resource.Id.categorySearchOption);

            TextView cuisineTextView = view.FindViewById<TextView>(Resource.Id.cuisineTextView);
            TextView cuisineSearchOption = view.FindViewById<TextView>(Resource.Id.cuisineSearchOption);

            TextView dishesTextView = view.FindViewById<TextView>(Resource.Id.dishesTextView);
            dishesSearchOption = view.FindViewById<TextView>(Resource.Id.dishesSearchOption);

            TextView infoText = view.FindViewById<TextView>(Resource.Id.infoText);
           
            edittext.SetCompoundDrawablesWithIntrinsicBounds(Resource.Drawable.baseline_search_24, 0, 0, 0);

            edittext.KeyPress += (object sender, View.KeyEventArgs e) => {
               e.Handled = false;
               if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
               {
                    infoText.Text = "Select one of the options below :";
                   categorySearchOption.Text = edittext.Text;
                   cuisineSearchOption.Text = edittext.Text;
                   dishesSearchOption.Text = edittext.Text;
                   Toast.MakeText(Context as Activity, edittext.Text, ToastLength.Short).Show();
                   e.Handled = true;
               }
           };

            categorySearchOption.Click += category_click;

            return view;
        }

       void category_click(object sender, EventArgs e)
       {
            dishesSearchOption.Text = "hellooooo";
       }

    }
}