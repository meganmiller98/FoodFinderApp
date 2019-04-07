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
    public class MenuType
    {
        public string Menu { get; set; }
        public string RestaurantID { get; set; }

        public string error { get; set; }

        public MenuType(string Menu, string RestaurantID, string error)
        {
            this.Menu = Menu;
            this.RestaurantID = RestaurantID;

            this.error = error;
        }
    }
}