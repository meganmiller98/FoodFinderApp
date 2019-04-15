using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace FoodFinder
{
    public class VoucherPage : Fragment
    {
        string lon;
        string lat;
        TextView resultInfo;
        ListView voucherList;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public VoucherPage()
        {

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.VoucherPage, container, false);
            Toolbar toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar1);
            TextView toolbarText = view.FindViewById<TextView>(Resource.Id.textView1);
            resultInfo = view.FindViewById<TextView>(Resource.Id.textView2);
            voucherList = view.FindViewById<ListView>(Resource.Id.listView1);

            getLastKnownLocation();
            return view;
        }

        async void getVouchers(ListView listview)
        {
            //my
            //string uri = "htp://192.168.0.20:45455/api/Voucher/";
            //uni
            string uri = "http://10.201.37.145:45455/api/Voucher/";
            //Katy
            //string uri = "htp://192.168.1.70:45455/api/Voucher/";

            //string uri = "htps://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/Voucher/";
            string otherhalf = "getVouchers?lat=" + lat + "&lon=" + lon;
            //test.Text = otherhalf;
            Uri result = null;

            if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
            {
                var httpClient = new HttpClient();
                var refineResult = (await httpClient.GetStringAsync(result));
                List<Vouchers> VoucherList = JsonConvert.DeserializeObject<List<Vouchers>>(refineResult);
                if (VoucherList.Count == 0)
                {
                    resultInfo.Text = "Sorry, no restaurants with vouchers available nearby";
                }
                else
                {
                    VoucherPageListViewAdapter adapter = new VoucherPageListViewAdapter(this.Context as Activity, VoucherList);
                    listview.Adapter = adapter;

                    listview.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
                    {
                        string restName = VoucherList[e.Position].restName;
                        Toast.MakeText(Context as Activity, restName, ToastLength.Short).Show();
                        //Toast.MakeText(Context as Activity, restName, ToastLength.Short).Show();
                        Intent intent = new Intent(Context as Activity, typeof(VoucherInfoActivity));
                        intent.PutExtra("VoucherInfo", JsonConvert.SerializeObject(VoucherList[e.Position]));
                        StartActivity(intent);
                    };
                }

            }
        }

        async void getLastKnownLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    lat = location.Latitude.ToString();
                    lon = location.Longitude.ToString();
                    getVouchers(voucherList);
                }
                else
                {
                    resultInfo.Text = "Can't find location";
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                AlertDialog.Builder alert = new AlertDialog.Builder(Context as Activity);
                alert.SetTitle("Failed");
                alert.SetMessage(fnsEx.ToString());
                alert.SetPositiveButton("Okay", (senderAlert, args) => {
                    Toast.MakeText(Context as Activity, "Okay", ToastLength.Short).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();

            }
            catch (FeatureNotEnabledException fneEx)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(Context as Activity);
                alert.SetTitle("Failed");
                alert.SetMessage(fneEx.ToString());
                alert.SetPositiveButton("Okay", (senderAlert, args) => {
                    Toast.MakeText(Context as Activity, "Okay", ToastLength.Short).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
            catch (PermissionException pEx)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(Context as Activity);
                alert.SetTitle("Failed");
                alert.SetMessage(pEx.ToString());
                alert.SetPositiveButton("Okay", (senderAlert, args) => {
                    Toast.MakeText(Context as Activity, "Okay", ToastLength.Short).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
            catch (Exception ex)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(Context as Activity);
                alert.SetTitle("Failed");
                alert.SetMessage(ex.ToString());
                alert.SetPositiveButton("Okay", (senderAlert, args) => {
                    Toast.MakeText(Context as Activity, "Okay", ToastLength.Short).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
        }
    }
}