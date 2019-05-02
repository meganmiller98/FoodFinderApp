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

            //sets the layout
            SetContentView(Resource.Layout.layoutLogIn);

            mLinearLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            mUsername = FindViewById<EditText>(Resource.Id.userNameInput);
            mPassword = FindViewById<EditText>(Resource.Id.passwordInput);
            mButton = FindViewById<Button>(Resource.Id.button1);
            Button signup = FindViewById<Button>(Resource.Id.signUpButton);

            Android.Widget.Toolbar toolbar = FindViewById<Android.Widget.Toolbar>(Resource.Id.toolbar1);
            SetActionBar(toolbar);
            ActionBar.Title = "Log In";

            mButton.Click += login_Click;
            signup.Click += signup_Click;

            //input validation
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
        //open up sign-up page
        void signup_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(SignUpActivity));
            StartActivity(intent);
            Finish();
        }
        void login_Click(object sender, EventArgs e)
        {
            if (mUsername != null && mPassword != null)
            {
                validateUsers();
            }

        }

        //check if user exists
        async void validateUsers()
        {
            string uri = "https://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/Users/";
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
                    string name = users[0].Name;

                    // code help from stacktips.com/tutorials/xamarin/shared-preferences-example-in-xamarin-android
                    //stores credentials to phone but they are not accessible to any other apps
                    ISharedPreferences prefs = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
                    ISharedPreferencesEditor editor = prefs.Edit();
                    editor.PutString("name", name);
                    editor.PutString("userID", mID);
                    editor.PutString("username", mUsername.Text);
                    editor.PutString("password", mPassword.Text);
                    editor.Apply();

                    openCorrectPage();

                }
                else
                {
                    Toast.MakeText(this, "no users with those credentials", ToastLength.Short).Show();
                }

            }
        }

        //open correct page up
        public void openCorrectPage()
        {

            if (Intent.GetStringExtra("RestaurantInfo") != null)
            {
                Intent intent = new Intent(this, typeof(RestaurantProfileActivity));
                //intent.PutExtra("RestaurantInfo", JsonConvert.SerializeObject(restaurantInfo);
                intent.PutExtra("RestaurantInfo", Intent.GetStringExtra("RestaurantInfo"));
                StartActivity(intent);
                Finish();
            }
            else if(Intent.GetStringExtra("VoucherInfo") != null)
            {
                Intent intent = new Intent(this, typeof(VoucherInfoActivity));
                    //intent.PutExtra("RestaurantInfo", JsonConvert.SerializeObject(restaurantInfo);
                intent.PutExtra("VoucherInfo", Intent.GetStringExtra("VoucherInfo"));
                StartActivity(intent);
                Finish();
            }
            else
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                intent.PutExtra("frgToLoad", "profilePage");
                StartActivity(intent);
                Finish();
            }
        }

        //override back button.
        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                //can't open profile page if user is not logged in so return user to Restaurants Page
                if (Intent.GetStringExtra("isProfile") != null)
                {
                    Intent intent = new Intent(this, typeof(MainActivity));
                    StartActivity(intent);
                    Finish();
                    return false;
                }
                else
                {
                    Finish();
                    return false;
                }
            }
            return false;

        }
    }

}