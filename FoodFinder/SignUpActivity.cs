using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
    [Activity(Label = "SignUpActivity")]
    public class SignUpActivity : Activity
    {
        EditText name;
        EditText email;
        EditText password1;
        EditText password2;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.signup);
            // Create your application here
            Android.Widget.Toolbar toolbar = FindViewById<Android.Widget.Toolbar>(Resource.Id.toolbar1);
            SetActionBar(toolbar);
            ActionBar.Title = "Sign Up";

            TextView textName = FindViewById<TextView>(Resource.Id.textName);
            TextView textEmail = FindViewById<TextView>(Resource.Id.textUserName);
            TextView textPassword = FindViewById<TextView>(Resource.Id.password);
            TextView textPassword2 = FindViewById<TextView>(Resource.Id.password2);

            name = FindViewById<EditText>(Resource.Id.Name);
            email = FindViewById<EditText>(Resource.Id.userNameInput);
            password1 = FindViewById<EditText>(Resource.Id.passwordInput);
            password2 = FindViewById<EditText>(Resource.Id.passwordInput2);

            Button confirm = FindViewById<Button>(Resource.Id.confirm);

            confirm.Click += confirmedClick;


        }
        void confirmedClick(object sender, EventArgs e)
        {
            if (name.Text.Trim() != "" && email.Text.Trim() != "" && password1.Text.Trim() != "" && password1.Text.Trim() == password2.Text.Trim())
            {
                signUpUser(name.Text, email.Text, password1.Text);
            }
            else if (password1.Text.Trim() != password2.Text.Trim())
            {
                Toast.MakeText(this, "passwords do not match", ToastLength.Short).Show();
            }
            else if (name.Text.Trim() == "" || email.Text.Trim() == "" || password1.Text.Trim() == "" || password2.Text.Trim() == "")
            {
                Toast.MakeText(this, "Missing values", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "please check your credentials", ToastLength.Short).Show();
            }
        }
        async void signUpUser(string name, string email, string password)
        {
            User user = new User(null, name, email, password, null);

            //myIp
            //string uri = "htp://192.168.0.20:45455/api/Users/addUser";
            //uni
            string uri = "http://10.201.37.145:45455/api/Users/addUser";
            //string uri = "htp://192.168.1.70:45455/api/Users/addUser";
            //string uri = "htps://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/Users/addUser";

            Uri result = new Uri(uri);
            Console.WriteLine(result);
            var httpClient = new HttpClient();

            var content = new StringContent(JsonConvert.SerializeObject(user));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage refineResult = (await httpClient.PostAsync(result, content));

            //await HandleResponse(refineResult);
            string serialized = await refineResult.Content.ReadAsStringAsync();
            Console.WriteLine(serialized);
            if (refineResult.IsSuccessStatusCode)
            {
                Toast.MakeText(this, "Success", ToastLength.Short).Show();
                validateUsers(email, password, name);
            }
            else
            {
                Toast.MakeText(this, "Something went wrong", ToastLength.Short).Show();
            }
        }
        async void validateUsers(string email, string password, string name)
        {
            //myIp
            //string uri = "htp://192.168.0.20:45455/api/Users/";

            //uni IP
            string uri = "http://10.201.37.145:45455/api/Users/";
            //string uri = "htps://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/Users/";

            //string uri = "htp://192.168.1.70:45455/api/Users/";
            string otherhalf = "validateUser?email=" + email + "&password=" + password;

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
                    editor.PutString("name", name);
                    editor.PutString("userID", mID);
                    editor.PutString("username", email);
                    editor.PutString("password", password);
                    editor.Apply();

                    Toast.MakeText(this, email, ToastLength.Short).Show();
                    Finish();
                    Intent intent = new Intent(this, typeof(MainActivity));
                    intent.PutExtra("frgToLoad", "profilePage");
                    StartActivity(intent);
                    Finish();

                }
                else
                {
                    Toast.MakeText(this, "no users with those credentials", ToastLength.Short).Show();
                }

            }
        }
    }
}