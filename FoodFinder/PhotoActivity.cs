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
    //displaying the photo clicked on in the restaurant profile page full sized
    [Activity(Label = "PhotoActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class PhotoActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PhotoActivityLayout);

            string photoFilePath = Intent.GetStringExtra("PhotoFilePath");
            string photoID = Intent.GetStringExtra("PhotoID");

            Console.WriteLine(photoID + " " + photoFilePath);

            ImageView image = FindViewById<ImageView>(Resource.Id.imageView1);

            image.SetImageBitmap(ImageHelper.GetImageBitmapFromUrl(photoFilePath));
        }
    }
}