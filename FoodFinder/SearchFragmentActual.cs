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
using Android.Views.InputMethods;
using Android.Widget;
using static Android.Resource;
using static Android.Views.View;

namespace FoodFinder
{
    public class SearchFragmentActual : Fragment 
    {
        EditText edittext;
        TextView dishesSearchOption;
        TextView categorySearchOption;
        TextView cuisineSearchOption;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            

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
            categorySearchOption = view.FindViewById<TextView>(Resource.Id.categorySearchOption);

            TextView cuisineTextView = view.FindViewById<TextView>(Resource.Id.cuisineTextView);
            cuisineSearchOption = view.FindViewById<TextView>(Resource.Id.cuisineSearchOption);

            TextView dishesTextView = view.FindViewById<TextView>(Resource.Id.dishesTextView);
            dishesSearchOption = view.FindViewById<TextView>(Resource.Id.dishesSearchOption);

            TextView infoText = view.FindViewById<TextView>(Resource.Id.infoText);
           
            //putting the search icon to the left of the edit text widget
            edittext.SetCompoundDrawablesWithIntrinsicBounds(Resource.Drawable.baseline_search_24, 0, 0, 0);

            //if keyboard enter button is pressed put searched items under each category heading
            edittext.KeyPress += (object sender, View.KeyEventArgs e) => {
               e.Handled = false;
               if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
               {
                    infoText.Text = "Select one of the options below :";
                   categorySearchOption.Text = edittext.Text;
                   cuisineSearchOption.Text = edittext.Text;
                   dishesSearchOption.Text = edittext.Text;
                   e.Handled = true;
               }
           };

            categorySearchOption.Click += category_click;
            cuisineSearchOption.Click += cuisine_click;
            dishesSearchOption.Click += dishes_click;
          
            return view;
        }

        //setting variables for search by category executed
       void category_click(object sender, EventArgs e)
       {
            string option = "category";
            string searchedstring = categorySearchOption.Text;
            createResultsFragment(option, searchedstring);
       }
        //setting variables for search by cuisine executed
        void cuisine_click(object sender, EventArgs e)
        {
            string option = "cuisine";
            string searchedstring = cuisineSearchOption.Text;
            createResultsFragment(option, searchedstring);
        }

        //setting variables for search by dish executed
        void dishes_click(object sender, EventArgs e)
        {
            string option = "dishes";
            string searchedstring = dishesSearchOption.Text;
            createResultsFragment(option, searchedstring);
        }

        //send info to the  SearchFragmentActual fragment to execute the search queries
        public void createResultsFragment(string option, string searchedString)
        {
            
            searchResultsPage resultsPage = new searchResultsPage();

            Bundle args = new Bundle();
            args.PutString("option", option);
            args.PutString("searchedString", searchedString);
            resultsPage.Arguments = args;

            FragmentTransaction fragmentTransaction = this.Activity.FragmentManager.BeginTransaction();
            fragmentTransaction.Replace(Resource.Id.frame, resultsPage);
            fragmentTransaction.AddToBackStack("SearchFragmentActual");
            fragmentTransaction.Commit();

        }

    }
}