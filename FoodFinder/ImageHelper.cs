﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FoodFinder
{
    //www.youtube.com/watch?v=6cKkS2rZa4E got from
    //used in myRestaurantListViewAdapter to display restaurant's main image
    class ImageHelper
    {
        public static Bitmap GetImageBitmapFromUrl (string url)
        {
            Bitmap imageBitmap = null;
            using (var webClient = new WebClient())
            {
                byte[] imageBytes = webClient.DownloadData(url);
                if(imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }
            return imageBitmap;
        }
    }
}