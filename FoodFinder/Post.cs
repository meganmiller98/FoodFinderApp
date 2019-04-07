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
using Newtonsoft.Json;

namespace FoodFinder
{
    
        public class Post
        {
            public string ID { get; set; }
            public string MainPhoto1 { get; set; }
            public string RestaurantName { get; set; }
            public string Categories { get; set; }
            public string Cuisines { get; set; }
            public string Address { get; set; }
            public string Opentimes { get; set; }
            public string CloseTimes { get; set; }
            public string Rating { get; set; }
            public string Cost { get; set; }

            public string error { get; set; }

            public Post (string ID, string MainPhoto1, string RestaurantName, string Categories, string Cuisines, string Address, string Opentimes, string CloseTimes, string Rating, string Cost, string error)
            {
                this.ID = ID;
                this.MainPhoto1 = MainPhoto1;
                this.RestaurantName = RestaurantName;
                this.Categories = Categories;
                this.Cuisines = Cuisines;
                this.Address = Address;
                this.Opentimes = Opentimes;
                this.CloseTimes = CloseTimes;
                this.Rating = Rating;
                this.Cost = Cost;
                this.error = error;
            }
        /*public override string ToString()
        {
            return string.Format(
                "Post: {0}, \n {1}, \n {2}, \n {3}, \n  {4}, \n  {5}, \n  {6}, \n  {7}, \n  {8}, \n  {9},",
                MainPhoto1, RestaurantName, Categories, Cuisines, Address, Opentimes, CloseTimes, Rating, Cost, error);
        }*/
    }
    
}