using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FoodFinder
{
    //Use this. It has to be a class or will not work
    class FragmentDialogClass : DialogFragment
    {
        //interface for refine options to be passed to homepage fragment
        public interface OnInputSelected { void sendInput(String input); }

        TextView sortby;
        CheckBox distanceBox;
        CheckBox ratingBox;
        CheckBox mostPopBox;
        CheckBox lowPriceBox;
        CheckBox highPriceBox;

        TextView dietaryRequirements;
        CheckBox vegetarianBox;
        CheckBox veganBox;
        CheckBox glutenFreeBox;
        CheckBox noDietary;

        Switch OpenNowSwitch;

        Button applyButton;

        string sort;
        string dietary;
        //string dietary;
        string openNow;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.FragmentDialog, container, false);
            this.Dialog.SetTitle("Refine");

            sortby = view.FindViewById<TextView>(Resource.Id.textView1);
            distanceBox = view.FindViewById<CheckBox>(Resource.Id.distanceCheckBox);
            ratingBox = view.FindViewById<CheckBox>(Resource.Id.ratingCheckBox);
            mostPopBox = view.FindViewById<CheckBox>(Resource.Id.mostPopularCheckBox);
            lowPriceBox = view.FindViewById<CheckBox>(Resource.Id.lowPriceCheckbox);
            highPriceBox = view.FindViewById<CheckBox>(Resource.Id.highPriceCheckbox);

            dietaryRequirements = view.FindViewById<TextView>(Resource.Id.textView2);
            vegetarianBox = view.FindViewById<CheckBox>(Resource.Id.vegetarianCheckbox);
            veganBox = view.FindViewById<CheckBox>(Resource.Id.veganCheckbox);
            glutenFreeBox = view.FindViewById<CheckBox>(Resource.Id.glutenFreeCheckbox);
            noDietary = view.FindViewById<CheckBox>(Resource.Id.noneDietaryCheckbox);

            OpenNowSwitch = view.FindViewById<Switch>(Resource.Id.openNowSwitch);


            applyButton = view.FindViewById<Button>(Resource.Id.applyButton);
            applyButton.Click += button_Click_Apply;

           
            //setting the refinements to the previous state
            if (Arguments != null)
            {
                if (Arguments.GetString("sort") != null)
                {
                    if (Arguments.GetString("sort") == "distance")
                    {
                        distanceBox.Checked = true;
                    }
                    else if (Arguments.GetString("sort") == "rating")
                    {
                        ratingBox.Checked = true;
                        veganBox.Checked = true;
                    }
                    else if (Arguments.GetString("sort") == "popular")
                    {
                        mostPopBox.Checked = true;
                    }
                    else if (Arguments.GetString("sort") == "low")
                    {
                        lowPriceBox.Checked = true;
                    }
                    else if (Arguments.GetString("sort") == "high")
                    {
                        highPriceBox.Checked = true;
                    }
                }
                if (Arguments.GetString("dietary") != null)
                {
                    if (Arguments.GetString("dietary") == "vegan")
                    {
                        veganBox.Checked = true;
                    }
                    else if (Arguments.GetString("dietary") == "vegetarian")
                    {
                        vegetarianBox.Checked = true;
                    }
                    else if (Arguments.GetString("dietary") == "glutenfree")
                    {
                        glutenFreeBox.Checked = true;
                    }
                    else if (Arguments.GetString("dietary") == "none")
                    {
                        noDietary.Checked = true;
                    }
                }
                if (Arguments.GetString("openNow") != null)
                {
                    if (Arguments.GetString("openNow") == "yes")
                    {
                        OpenNowSwitch.Checked = true;
                    }
                    else
                    {
                        OpenNowSwitch.Checked = false;
                    }
                }
                if (Arguments.GetString("sort") == null && Arguments.GetString("dietary") == null && Arguments.GetString("openNow") == null)
                {
                    distanceBox.Checked = true;
                    noDietary.Checked = true;
                }
            }

            //Making sure only one item per category is selected at any time.
            distanceBox.Click += (o, e) =>
            {
                if (distanceBox.Checked)
                {
                    //Toast.MakeText(this.Context as Activity, "Selected", ToastLength.Short).Show();
                    ratingBox.Checked = false;
                    mostPopBox.Checked = false;
                    lowPriceBox.Checked = false;
                    highPriceBox.Checked = false;
                }
                else
                {
                    //Toast.MakeText(this.Context as Activity, "Unselected", ToastLength.Short).Show();
                }
            };
            ratingBox.Click += (o, e) =>
            {
                if (ratingBox.Checked)
                {
                    //Toast.MakeText(this.Context as Activity, "Selected", ToastLength.Short).Show();
                    distanceBox.Checked = false;
                    mostPopBox.Checked = false;
                    lowPriceBox.Checked = false;
                    highPriceBox.Checked = false;
                }
                else
                {
                    //Toast.MakeText(this.Context as Activity, "Unselected", ToastLength.Short).Show();
                }
            };
            mostPopBox.Click += (o, e) =>
            {
                if (mostPopBox.Checked)
                {
                    //Toast.MakeText(this.Context as Activity, "Selected", ToastLength.Short).Show();
                    ratingBox.Checked = false;
                    distanceBox.Checked = false;
                    lowPriceBox.Checked = false;
                    highPriceBox.Checked = false;
                }
                else
                {
                    //Toast.MakeText(this.Context as Activity, "Unselected", ToastLength.Short).Show();
                }
            };
            lowPriceBox.Click += (o, e) =>
            {
                if (lowPriceBox.Checked)
                {
                    //Toast.MakeText(this.Context as Activity, "Selected", ToastLength.Short).Show();
                    ratingBox.Checked = false;
                    distanceBox.Checked = false;
                    mostPopBox.Checked = false;
                    highPriceBox.Checked = false;
                }
                else
                {
                    //Toast.MakeText(this.Context as Activity, "Unselected", ToastLength.Short).Show();
                }
            };
            highPriceBox.Click += (o, e) =>
            {
                if (highPriceBox.Checked)
                {
                    //Toast.MakeText(this.Context as Activity, "Selected", ToastLength.Short).Show();
                    ratingBox.Checked = false;
                    distanceBox.Checked = false;
                    lowPriceBox.Checked = false;
                    mostPopBox.Checked = false;
                }
                else
                {
                    //Toast.MakeText(this.Context as Activity, "Unselected", ToastLength.Short).Show();
                }
            };
            noDietary.Click += (o, e) =>
            {
                if (noDietary.Checked)
                {
                    //Toast.MakeText(this.Context as Activity, "Selected", ToastLength.Short).Show();
                    vegetarianBox.Checked = false;
                    veganBox.Checked = false;
                    glutenFreeBox.Checked = false;
                }
                else
                {
                    //Toast.MakeText(this.Context as Activity, "Unselected", ToastLength.Short).Show();
                }
            };
            vegetarianBox.Click += (o, e) =>
            {
                if (vegetarianBox.Checked)
                {
                    //Toast.MakeText(this.Context as Activity, "Selected", ToastLength.Short).Show();
                    noDietary.Checked = false;
                    veganBox.Checked = false;
                    glutenFreeBox.Checked = false;
                }
                else
                {
                    //Toast.MakeText(this.Context as Activity, "Unselected", ToastLength.Short).Show();
                }
            };
            veganBox.Click += (o, e) =>
            {
                if (veganBox.Checked)
                {
                    //Toast.MakeText(this.Context as Activity, "Selected", ToastLength.Short).Show();
                    noDietary.Checked = false;
                    vegetarianBox.Checked = false;
                    glutenFreeBox.Checked = false;
                }
                else
                {
                    //Toast.MakeText(this.Context as Activity, "Unselected", ToastLength.Short).Show();
                }
            };
            glutenFreeBox.Click += (o, e) =>
            {
                if (glutenFreeBox.Checked)
                {
                    //Toast.MakeText(this.Context as Activity, "Selected", ToastLength.Short).Show();
                    noDietary.Checked = false;
                    vegetarianBox.Checked = false;
                    veganBox.Checked = false;
                }
                else
                {
                    //Toast.MakeText(this.Context as Activity, "Unselected", ToastLength.Short).Show();
                }
            };
            OpenNowSwitch.Click += (o, e) =>
            {
                if (OpenNowSwitch.Checked)
                {
                    Toast.MakeText(this.Context as Activity, "Selected", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this.Context as Activity, "Unselected", ToastLength.Short).Show();
                }
            };
            return view;
        }

        //set sort, dietary and open now variables
        void button_Click_Apply(object sender, EventArgs e)
        {
            if (distanceBox.Checked)
            {
                sort = "distance";
            }
            else if (ratingBox.Checked)
            {
                sort = "rating";
            }
            else if (mostPopBox.Checked)
            {
                sort = "popular";
            }
            else if(lowPriceBox.Checked)
            {
                sort = "low";
            }
            else if(highPriceBox.Checked)
            {
                sort = "high";
            }
            else
            {
                sort = "distance";
            }

            if (noDietary.Checked)
            {
                dietary = "none"; 
            }
            else if (veganBox.Checked)
            {
                dietary = "vegan";
            }
            else if (vegetarianBox.Checked)
            {
                dietary = "vegetarian";
            }
            else if (glutenFreeBox.Checked)
            {
                dietary = "glutenfree";
            }
            else
            {
                dietary = "none";
            }

            if(OpenNowSwitch.Checked)
            {
                openNow = "yes";
            }
            else
            {
                openNow = "no";
            }

            //Passing the arguments to the home page to be sent as parameters to the API
            HomePage fragment = new HomePage();
            Bundle args = new Bundle();
            args.PutString("sort", sort);
            args.PutString("dietary", dietary);
            args.PutString("openNow", openNow);
            fragment.Arguments = args;

            var fragmentTransaction = FragmentManager.BeginTransaction();
            fragmentTransaction.Replace(Resource.Id.frame, fragment);
            fragmentTransaction.Commit();


        }

    }
}