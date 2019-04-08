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
    public class Photos
    {
        public string IDPhotos { get; set; }
        public string RestaurantID { get; set; }
        public string PhotoFilePath { get; set; }

        public string error { get; set; }

        public Photos(string IDPhotos, string RestaurantID, string PhotoFilePath, string error)
        {
            this.IDPhotos = IDPhotos;
            this.RestaurantID = RestaurantID;
            this.PhotoFilePath = PhotoFilePath;

            this.error = error;
        }
    }
}