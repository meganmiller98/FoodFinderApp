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
    [Activity(Label = "VoucherInfoActivity")]
    public class VoucherInfoActivity : Activity
    {
        ImageButton saveButton;
        string voucherID;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.VoucherInfoPage);

            var voucherInfo = JsonConvert.DeserializeObject<Vouchers>(Intent.GetStringExtra("VoucherInfo"));

            ImageView voucherImage = FindViewById<ImageView>(Resource.Id.VoucherImage);
            TextView restName = FindViewById<TextView>(Resource.Id.RestaurantName);
            TextView deal = FindViewById<TextView>(Resource.Id.Deal);
            TextView expiryDate= FindViewById<TextView>(Resource.Id.ExpiryDate);
            Button useNowButton = FindViewById<Button>(Resource.Id.useNowButton);
            saveButton = FindViewById<ImageButton>(Resource.Id.saveButton);
            ImageView phoneIcon = FindViewById<ImageView>(Resource.Id.phoneImage);
            TextView contactNumber = FindViewById<TextView>(Resource.Id.ContactNumber);
            TextView termsOfUse = FindViewById<TextView>(Resource.Id.TermsOfUse);
            TextView conditions = FindViewById<TextView>(Resource.Id.Conditions);

            var imageBitmap = ImageHelper.GetImageBitmapFromUrl(voucherInfo.voucherImage);
            voucherImage.SetImageBitmap(imageBitmap);


            if (voucherInfo.expiryDate.Contains("00:00:00"))
            {
                string date = voucherInfo.expiryDate.Remove(voucherInfo.expiryDate.IndexOf(" 00:00:00"), " 00:00:00".Length);
                expiryDate.Text = date;
            }
            else
            {
                expiryDate.Text = voucherInfo.expiryDate;
            }


            voucherID = voucherInfo.voucherID;
            restName.Text = voucherInfo.restName;
            deal.Text = voucherInfo.deal;
            contactNumber.Text = voucherInfo.number;
            conditions.Text = voucherInfo.termsOfConditions;

            checkIfUserHasSavedVoucher(voucherID);

            saveButton.Click += saveButtonClick;

            
        }

        async void checkIfUserHasSavedVoucher(string voucherID)
        {
            ISharedPreferences prefs = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string userID = prefs.GetString("userID", null);

            if (userID == null)
            {
                //Toast.MakeText(this, "User Not Logged In", ToastLength.Short).Show();
                Console.WriteLine("user not logged in");
            }
            else
            {
                List<savedVouchers> mVouchers = new List<savedVouchers>();
                //myIp
                //string uri = "htp://192.168.0.20:45455/api/savedVouchers/";

                //uni IP
                string uri = "http://10.201.37.145:45455/api/savedVouchers/";

                //string uri = "htps://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/savedVouchers/";
                //string uri = "htp://192.168.1.70:45455/api/favouriteRestaurants/";

                string otherhalf = "checkIfSaved?userID=" + userID + "&voucherID=" + voucherID;
                //string otherhalf = "checkIfSaved?userID=1&restaurantID=1";
                Uri result = null;

                if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
                {
                    var httpClient = new HttpClient();
                    var refineResult = (await httpClient.GetStringAsync(result));
                    mVouchers = JsonConvert.DeserializeObject<List<savedVouchers>>(refineResult);

                    if (mVouchers.Count > 0)
                    {
                        saveButton.Selected = true;
                    }
                    else
                    {
                        saveButton.Selected = false;
                    }
                }
            }
        }
        void saveButtonClick(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Click", ToastLength.Short).Show();
            ISharedPreferences prefs = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string userID = prefs.GetString("userID", null);
            if (saveButton.Selected == true)
            {
                saveButton.Selected = false;
                //Toast.MakeText(this, "true to false", ToastLength.Short).Show();


                if (userID == null)
                {
                    Toast.MakeText(this, "User Not Logged In", ToastLength.Short).Show();
                }
                else
                {
                    deleteSavedVoucher(userID);
                }
            }
            else
            {
                if (userID == null)
                {
                    Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                    alert.SetTitle("Log In");
                    alert.SetMessage("You need to log in to add vouchers to your favourites");
                    alert.SetPositiveButton("Log In", (senderAlert, args) => {
                        Intent intent = new Intent(this, typeof(LogInActivity));
                        intent.PutExtra("VoucherInfo", Intent.GetStringExtra("VoucherInfo"));
                        StartActivity(intent);
                        Finish();
                    });
                    alert.SetNegativeButton("Cancel", (senderAlert, args) => {

                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
                else
                {
                    saveButton.Selected = true;
                    //Toast.MakeText(this, "false to true", ToastLength.Short).Show();
                    saveVoucher(userID);
                }
            }
         }
        async void deleteSavedVoucher(string userID)
        {
            //string uri = "htp://192.168.1.70:45455/api/favouriteRestaurants/";

            //uni
            //string uri = "htp://10.201.37.145:45455/api/savedVouchers/";
            string uri = "https://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/savedVouchers/";

            //myIp
            //string uri = "htp://192.168.0.20:45455/api/savedVouchers/";

            string otherhalf = "deleteSaved?userID=" + userID + "&voucherID=" + voucherID;
            Uri result = null;

            if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
            {
                var httpClient = new HttpClient();
                var refineResult = (await httpClient.DeleteAsync(result));
                if (refineResult.IsSuccessStatusCode)
                {
                    Console.WriteLine("successfully deleted.");
                    Toast.MakeText(this, "Removed from favourites", ToastLength.Short).Show();
                }
            }
        }
        async void saveVoucher(string userID)
        {
            savedVouchers save = new savedVouchers(voucherID, userID, null);

            //string uri = "htp://192.168.1.70:45455/api/favouriteRestaurants/Save";

            //uni
            string uri = "http://10.201.37.145:45455/api/savedVouchers/Save";
            //string uri = "htps://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/savedVouchers/Save";
            //myIp
            //string uri = "htp://192.168.0.20:45455/api/savedVouchers/Save";

            Uri result = new Uri(uri);
            Console.WriteLine(result);
            var httpClient = new HttpClient();

            var content = new StringContent(JsonConvert.SerializeObject(save));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage refineResult = (await httpClient.PostAsync(result, content));

            //await HandleResponse(refineResult);
            string serialized = await refineResult.Content.ReadAsStringAsync();
            Console.WriteLine(serialized);
            if (refineResult.IsSuccessStatusCode)
            {
                Toast.MakeText(this, "Added to favourites", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Something went wrong", ToastLength.Short).Show();
            }
        }
        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {

                if (Intent.GetStringExtra("profile") != null)
                {
                    Intent intent = new Intent(this, typeof(MainActivity));
                    intent.PutExtra("frgToLoad", "profilePage");
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