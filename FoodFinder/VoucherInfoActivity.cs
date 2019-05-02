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
    //Voucher Profile Display
    [Activity(Label = "VoucherInfoActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class VoucherInfoActivity : Activity
    {
        ImageButton saveButton;
        string voucherID;
        string voucherCode;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.VoucherInfoPage);

            //get voucher info from the voucher results page
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
            voucherCode = voucherInfo.voucherCode;

            checkIfUserHasSavedVoucher(voucherID);

            saveButton.Click += saveButtonClick;
            useNowButton.Click += useVoucherClick;

            
        }

        async void checkIfUserHasSavedVoucher(string voucherID)
        {
            ISharedPreferences prefs = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string userID = prefs.GetString("userID", null);

            if (userID == null)
            {
                Console.WriteLine("user not logged in");
            }
            else
            {
                List<savedVouchers> mVouchers = new List<savedVouchers>();

                string uri = "https://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/savedVouchers/";
                

                string otherhalf = "checkIfSaved?userID=" + userID + "&voucherID=" + voucherID;
                
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
            ISharedPreferences prefs = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string userID = prefs.GetString("userID", null);
            if (saveButton.Selected == true)
            {
                saveButton.Selected = false;

                if (userID == null)
                {
                    Console.WriteLine("User Not Logged In");
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
            string uri = "https://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/savedVouchers/";

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

        //create a savedVouchers object then send to API to insert into database
        async void saveVoucher(string userID)
        {
            savedVouchers save = new savedVouchers(voucherID, userID, null);

            string uri = "https://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/savedVouchers/Save";

            Uri result = new Uri(uri);
            Console.WriteLine(result);
            var httpClient = new HttpClient();

            var content = new StringContent(JsonConvert.SerializeObject(save));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage refineResult = (await httpClient.PostAsync(result, content));

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

        //Display voucher code dialog fragment
        public void useVoucherClick(object sender, EventArgs e)
        {
            VoucherCodeDialogFragment voucherCodeDisplay = new VoucherCodeDialogFragment();
            Bundle args = new Bundle();
            args.PutString("voucherCode", voucherCode);
            voucherCodeDisplay.Arguments = args;
            FragmentTransaction transcation = FragmentManager.BeginTransaction();
            voucherCodeDisplay.Show(transcation, "voucherCodeDialog");
        }

        //If coming from user profile, return to user profile when the back button is pressed
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