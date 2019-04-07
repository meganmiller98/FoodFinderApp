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
    public class MenuItems
    {
        public string RestaurantID { get; set; }
        public string Item { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }

        public string error { get; set; }

        public MenuItems(string RestaurantID, string Item, string Price, string Description, string error)
        {
            this.RestaurantID = RestaurantID;
            this.Item = Item;
            this.Price = Price;
            this.Description = Description;

            this.error = error;
        }
    }
}