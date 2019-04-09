using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    [Activity(Label = "LogInActivity")]
    public class LogInActivity : Activity
    {
        EditText mUsername;
        EditText mPassword;
        Button mButton;
        LinearLayout mLinearLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layoutLogIn);
            // Create your application here

            mLinearLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            mUsername = FindViewById<EditText>(Resource.Id.userNameInput);
            mPassword = FindViewById<EditText>(Resource.Id.passwordInput);
            mButton = FindViewById<Button>(Resource.Id.button1);

            Android.Widget.Toolbar toolbar = FindViewById<Android.Widget.Toolbar>(Resource.Id.toolbar1);
            mButton.Click += login_Click;

            mUsername.KeyPress += (object sender, View.KeyEventArgs e) => {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    if (mUsername != null && mPassword != null)
                    {
                        validateUsers();
                    }
                    else
                    {
                        Toast.MakeText(this, "empty", ToastLength.Short).Show();
                    }
                    e.Handled = true;
                }
            };
            mPassword.KeyPress += (object sender, View.KeyEventArgs e) => {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    if (mUsername != null && mPassword != null)
                    {
                        validateUsers();
                    }
                    else
                    {
                        Toast.MakeText(this, "empty", ToastLength.Short).Show();
                    }
                    e.Handled = true;
                }
            };
        }

        void login_Click(object sender, EventArgs e)
        {
            if (mUsername != null && mPassword != null)
            {
                validateUsers();
            }

        }
        async void validateUsers()
        {
            //myIp
            //string uri = "htp://192.168.0.20:45455/api/mainmenu/";

            //uni IP
            //string uri = "htp://10.201.37.145:45455/api/mainmenu/";

            string uri = "http://192.168.1.70:45455/api/Users/";
            string otherhalf = "validateUser?email=" + mUsername.Text + "&password=" + mPassword.Text;

            Uri result = null;

            if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
            {
                var httpClient = new HttpClient();
                var refineResult = (await httpClient.GetStringAsync(result));
                List<User> users = JsonConvert.DeserializeObject<List<User>>(refineResult);
                if (users.Count != 0)
                {
                    string mID = users[0].UserID;

                    // code help from stacktips.com/tutorials/xamarin/shared-preferences-example-in-xamarin-android
                    ISharedPreferences prefs = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
                    ISharedPreferencesEditor editor = prefs.Edit();
                    editor.PutString("userID", mID);
                    editor.PutString("username", mUsername.Text);
                    editor.PutString("password", mPassword.Text);
                    editor.Apply();

                    Toast.MakeText(this, mUsername.Text, ToastLength.Short).Show();
                    help();

                }
                else
                {
                    Toast.MakeText(this, "no users with those credentials", ToastLength.Short).Show();
                }

            }
        }

        public void help()
        {
            ISharedPreferences prefs = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string userID = prefs.GetString("userID", null);
            string userName = prefs.GetString("username", null);
            string password = prefs.GetString("password", null);

            if (userName == null || password == null)
            {
                Toast.MakeText(this, "not stored", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, userID + " " + userName + " " + password, ToastLength.Short).Show();
            }

            if (Intent.GetStringExtra("RestaurantInfo") != null)
            {
                Intent intent = new Intent(this, typeof(RestaurantProfileActivity));
                //intent.PutExtra("RestaurantInfo", JsonConvert.SerializeObject(restaurantInfo);
                intent.PutExtra("RestaurantInfo", Intent.GetStringExtra("RestaurantInfo"));
                StartActivity(intent);
                Finish();
            }
            else
            {
                Finish();
            }
        }
    }

}