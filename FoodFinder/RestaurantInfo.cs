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
    public class RestaurantInfo
    {
            public string ID { get; set; }
            public string RestaurantName { get; set; }
            public string OwnerID { get; set; }
            public string Address { get; set; }
            public string Longitude { get; set; }
            public string Latitude { get; set; }
            public string Cost { get; set; }
            public string VoucherID { get; set; }
            public string Category { get; set; }
            public string Cuisine { get; set; }
            public string MainPhoto1 { get; set; }
            public string MainPhoto2 { get; set; }
            public string MainPhoto3 { get; set; }
            public string MainPhoto4 { get; set; }
            public string MainPhoto5 { get; set; }
            public string Description { get; set; }
            public string ContactTelephone { get; set; }
            public string ContactEmail { get; set; }
            public string Website { get; set; }
            public string OpenTimesMonday { get; set; }
            public string OpenTimesTuesday { get; set; }
            public string OpenTimesWednesday { get; set; }
            public string OpenTimesThursday { get; set; }
            public string OpenTimesFriday { get; set; }
            public string OpenTimesSaturday { get; set; }
            public string OpenTimesSunday { get; set; }
            public string CloseTimesMonday { get; set; }
            public string CloseTimesTuesday { get; set; }
            public string CloseTimesWednesday { get; set; }
            public string CloseTimesThursday { get; set; }
            public string CloseTimesFriday { get; set; }
            public string CloseTimesSaturday { get; set; }
            public string CloseTimesSunday { get; set; }
            public string Rating { get; set; }


            public string error { get; set; }

        public RestaurantInfo(string ID, string RestaurantName, string OwnerID, string Address, string Longitude, string Latitude, string Cost, string VoucherID, string Category,
            string Cuisine, string MainPhoto1, string MainPhoto2, string MainPhoto3, string MainPhoto4, string MainPhoto5, string Description,
            string ContactTelephone, string ContactEmail, string Website, string OpenTimesMonday, string OpenTimesTuesday, string OpenTimesWednesday, string OpenTimesThursday,
            string OpenTimesFriday, string OpenTimesSaturday, string OpenTimesSunday, string CloseTimesMonday, string CloseTimesTuesday,
            string CloseTimesWednesday, string CloseTimesThursday, string CloseTimesFriday, string CloseTimesSaturday, string CloseTimesSunday, string Rating, string error)
        {
            this.ID = ID;
            this.RestaurantName = RestaurantName;
            this.OwnerID = OwnerID;
            this.Address = Address;
            this.Longitude = Longitude;
            this.Latitude = Latitude;
            this.Cost = Cost;
            this.VoucherID = VoucherID;
            this.Category = Category;
            this.Cuisine = Cuisine;
            this.MainPhoto1 = MainPhoto1;
            this.MainPhoto2 = MainPhoto2;
            this.MainPhoto3 = MainPhoto3;
            this.MainPhoto4 = MainPhoto4;
            this.MainPhoto5 = MainPhoto5;
            this.Description = Description;
            this.ContactTelephone = ContactTelephone;
            this.ContactEmail = ContactEmail;
            this.Website = Website;
            this.OpenTimesMonday = OpenTimesMonday;
            this.OpenTimesTuesday = OpenTimesTuesday;
            this.OpenTimesWednesday = OpenTimesWednesday;
            this.OpenTimesThursday = OpenTimesThursday;
            this.OpenTimesFriday = OpenTimesFriday;
            this.OpenTimesSaturday = OpenTimesSaturday;
            this.OpenTimesSunday = OpenTimesSunday;
            this.CloseTimesMonday = CloseTimesMonday;
            this.CloseTimesTuesday = CloseTimesTuesday;
            this.CloseTimesWednesday = CloseTimesWednesday;
            this.CloseTimesThursday = CloseTimesThursday;
            this.CloseTimesFriday = CloseTimesFriday;
            this.CloseTimesSaturday = CloseTimesSaturday;
            this.CloseTimesSunday = CloseTimesSunday;
            this.Rating = Rating;
            this.error = error;
        }
    }
}