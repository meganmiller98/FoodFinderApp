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
    public class savedRestaurants
    {
        public string userID { get; set; }
        public string RestaurantID { get; set; }
        public string RestaurantName { get; set; }
        public string MainPhoto { get; set; }

        public string error { get; set; }

        public savedRestaurants(string userID, string RestaurantID, string RestaurantName, string MainPhoto, string error)
        {
            this.RestaurantID = RestaurantID;
            this.userID = userID;
            this.RestaurantName = RestaurantName;
            this.MainPhoto = MainPhoto;
            this.error = error;
        }
    }
}