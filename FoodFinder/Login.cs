using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace FoodFinder
{
    public class Login : Fragment
    {
        EditText mUsername;
        EditText mPassword;
        Button mButton;
        LinearLayout mLinearLayout;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.layoutLogIn, container, false);

            mLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            mUsername = view.FindViewById<EditText>(Resource.Id.userNameInput);
            mPassword = view.FindViewById<EditText>(Resource.Id.passwordInput);
            mButton = view.FindViewById<Button>(Resource.Id.button1);

            Android.Widget.Toolbar toolbar = view.FindViewById<Android.Widget.Toolbar>(Resource.Id.toolbar1);
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
                       Toast.MakeText(Context as Activity, "empty", ToastLength.Short).Show();
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
                        Toast.MakeText(Context as Activity, "empty", ToastLength.Short).Show();
                    }
                    e.Handled = true;
                }
            };
            return view;
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

                    Toast.MakeText(Context as Activity, mUsername.Text, ToastLength.Short).Show();
                    help();
                }
                else
                {
                    Toast.MakeText(Context as Activity, "no users with those credentials", ToastLength.Short).Show();
                }

            }
        }

        public void help ()
        {
            ISharedPreferences prefs = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string userID = prefs.GetString("userID", null);
            string userName = prefs.GetString("username", null);
            string password = prefs.GetString("password", null);

            if (userName == null || password == null)
            {
                Toast.MakeText(Context as Activity, "not stored", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(Context as Activity, userID + " " + userName + " " + password, ToastLength.Short).Show();
            }
        }

    }
}