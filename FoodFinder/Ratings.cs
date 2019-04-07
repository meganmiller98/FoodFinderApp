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
    public class Ratings
    {
        public string rating { get; set; }
        public string restaurantID { get; set; }
        public string name { get; set; }
        public string userId { get; set; }

        public string error { get; set; }

        public Ratings(string rating, string restaurantID, string name, string userId, string error)
        {
            this.rating = rating;
            this.restaurantID = restaurantID;
            this.name = name;
            this.userId = userId;

            this.error = error;
        }
    }
}