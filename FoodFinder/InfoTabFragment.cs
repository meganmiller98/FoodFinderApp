using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace FoodFinder
{
    public class InfoTabFragment : Android.Support.V4.App.Fragment
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.Adapter mAdapter;
        private List<Email> mEmails;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public InfoTabFragment()
        {

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.InfoTab, container, false);
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView1);

            mEmails = new List<Email>();
            mEmails.Add(new Email() { Name = "Tom", Subject = "Wanna hang?", Message = "See you tomorrow!" });
            mEmails.Add(new Email() { Name = "Tom", Subject = "Wanna hang?", Message = "See you tomorrow!" });
            mEmails.Add(new Email() { Name = "Tom", Subject = "Wanna hang?", Message = "See you tomorrow!" });
            mEmails.Add(new Email() { Name = "Tom", Subject = "Wanna hang?", Message = "See you tomorrow!" });
            mEmails.Add(new Email() { Name = "Tom", Subject = "Wanna hang?", Message = "See you tomorrow!" });
            mEmails.Add(new Email() { Name = "Tom", Subject = "Wanna hang?", Message = "See you tomorrow!" });
            mEmails.Add(new Email() { Name = "Tom", Subject = "Wanna hang?", Message = "See you tomorrow!" });
            mEmails.Add(new Email() { Name = "Tom", Subject = "Wanna hang?", Message = "See you tomorrow!" });
            mEmails.Add(new Email() { Name = "Tom", Subject = "Wanna hang?", Message = "See you tomorrow!" });
            mEmails.Add(new Email() { Name = "Tom", Subject = "Wanna hang?", Message = "See you tomorrow!" });
            mEmails.Add(new Email() { Name = "Tom", Subject = "Wanna hang?", Message = "See you tomorrow!" });
            mEmails.Add(new Email() { Name = "Tom", Subject = "Wanna hang?", Message = "See you tomorrow!" });
            mEmails.Add(new Email() { Name = "Tom", Subject = "Wanna hang?", Message = "See you tomorrow!" });
            mEmails.Add(new Email() { Name = "Tom", Subject = "Wanna hang?", Message = "See you tomorrow!" });
            mEmails.Add(new Email() { Name = "Tom", Subject = "Wanna hang?", Message = "See you tomorrow!" });
            mEmails.Add(new Email() { Name = "Tom", Subject = "Wanna hang?", Message = "See you tomorrow!" });
            mEmails.Add(new Email() { Name = "Tom", Subject = "Wanna hang?", Message = "See you tomorrow!" });
            //Create our layout manager
            mLayoutManager = new LinearLayoutManager(Context as Activity);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mAdapter = new RecyclerAdapter(mEmails);
            mRecyclerView.SetAdapter(mAdapter);

            string getData = RestaurantProfileActivity.sendData();
            if(getData != null)
            {
                Toast.MakeText(Context as Activity, getData, ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(Context as Activity, "Nothing again", ToastLength.Short).Show();
            }

            return view;
        }

        /* async void refineList(ListView listview, string value, string value2, string value3)
         {
             //myIp
             string uri = "http://192.168.0.20:45455/api/Restaurant/";

             //uni IP
             //string uri = "htp://10.201.37.145:45455/api/mainmenu/";

             //string uri = "htp://192.168.1.70:45455/api/mainmenu/";

             string otherhalf = "refinements2?lat=" + lat + "&lon=" + lon + "&sort=" + value + "&dietary=" + value2 + "&openNow=" + value3;

             Uri result = null;

             if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
             {
                 var httpClient = new HttpClient();
                 var refineResult = (await httpClient.GetStringAsync(result));
                 List<Post> RestaurantList = JsonConvert.DeserializeObject<List<Post>>(refineResult);
                 myRestaurantListViewAdapter adapter = new myRestaurantListViewAdapter(this.Context as Activity, RestaurantList);
                 listview.Adapter = adapter;

                 //test.Text = result.AbsoluteUri;

             }

         }*/
    }
    public class RecyclerAdapter : RecyclerView.Adapter
    {
        private List<Email> mEmails;
        public RecyclerAdapter(List<Email> emails)
        {
            mEmails = emails;
        }

        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mName { get; set; }
            public TextView mSubject { get; set; }
            public TextView mMessage { get; set; }

            public MyView(View view) : base(view)
            {
                mMainView = view;
            }
        }
        public override int ItemCount
        {
            get { return mEmails.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;
            myHolder.mName.Text = mEmails[position].Name;
            myHolder.mSubject.Text = mEmails[position].Subject;
            myHolder.mMessage.Text = mEmails[position].Message;

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.infoLayout, parent, false);
            TextView textName = row.FindViewById<TextView>(Resource.Id.textView1);
            TextView textSubject = row.FindViewById<TextView>(Resource.Id.textView2);
            TextView textMessage = row.FindViewById<TextView>(Resource.Id.textView3);

            MyView view = new MyView(row) { mName = textName, mSubject = textSubject, mMessage = textMessage };
            return view;
        }

    }
}