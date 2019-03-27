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
    public class FragmentTest : Fragment
    {
        TextView text;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public FragmentTest()
        {

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.testLayoutFragment, container, false);

            text = view.FindViewById<TextView>(Resource.Id.textView1);
            Button button = view.FindViewById<Button>(Resource.Id.button1);

            button.Click += button_click;

            return view;
        }
        public void button_click(object sender, EventArgs e)
        {
            text.Text = "WHATEVER";
        }
    }
}