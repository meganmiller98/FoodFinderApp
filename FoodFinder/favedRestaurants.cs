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
    public class favedRestaurants
    {
        public string userID { get; set; }
        public string RestaurantID { get; set; }

        public string error { get; set; }

        public favedRestaurants(string userID, string RestaurantID, string error)
        {
            this.RestaurantID = RestaurantID;
            this.userID = userID;
            this.error = error;
        }
    }
}