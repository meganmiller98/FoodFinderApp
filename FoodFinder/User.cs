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
    public class User
    {
        public string UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string error { get; set; }

        public User(string UserID, string Name, string Email, string Password, string error)
        {
            this.UserID = UserID;
            this.Name = Name;
            this.Email = Email;
            this.Password = Password;

            this.error = error;
        }
    }
}