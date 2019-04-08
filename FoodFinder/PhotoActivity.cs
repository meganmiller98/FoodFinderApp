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
    [Activity(Label = "PhotoActivity")]
    public class PhotoActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PhotoActivityLayout);
            // Create your application here

            string photoFilePath = Intent.GetStringExtra("PhotoFilePath");
            string photoID = Intent.GetStringExtra("PhotoID");

            Console.WriteLine(photoID + " " + photoFilePath);

            ImageView image = FindViewById<ImageView>(Resource.Id.imageView1);

            image.SetImageBitmap(ImageHelper.GetImageBitmapFromUrl(photoFilePath));
        }
    }
}