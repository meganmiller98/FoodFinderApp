using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace FoodFinder
{
    public class MenuTabFragment : Android.Support.V4.App.Fragment
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.Adapter mAdapter;
        private List<MenuType> mMenuType;
        string getID;
        //public string namewoo;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.MenuTab, container, false);
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView1);

            getID = RestaurantProfileActivity.sendData();
            if (getID != null)
            {
                Toast.MakeText(Context as Activity, getID, ToastLength.Short).Show();
                getMenuTypes(getID);
            }
            else
            {
                Toast.MakeText(Context as Activity, "Nothing again", ToastLength.Short).Show();
            }

            return view;
        }

        async void getMenuTypes(string getID)
        {
            mMenuType = new List<MenuType>();
            //myIp
            //string uri = "htp://192.168.0.20:45455/api/MenuType/";

            //uni IP
            string uri = "http://10.201.37.145:45455/api/MenuType/";

            //string uri = "htp://192.168.1.70:45455/api/MenuType/";

            //string uri = "htps://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/MenuType";

            string otherhalf = "getMenuTypes?ID= " + getID;

            Uri result = null;

            if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
            {
                var httpClient = new HttpClient();
                var refineResult = (await httpClient.GetStringAsync(result));
                mMenuType = JsonConvert.DeserializeObject<List<MenuType>>(refineResult);

                mLayoutManager = new LinearLayoutManager(Context as Activity);
                mRecyclerView.SetLayoutManager(mLayoutManager);
                mAdapter = new MenuTypeRecyclerAdapter(mMenuType, mRecyclerView, Context as Activity);
                mRecyclerView.SetAdapter(mAdapter);
            }
        }
    }

    public class MenuTypeRecyclerAdapter : RecyclerView.Adapter
    {
        private List<MenuType> mMenuType;
        private RecyclerView mRecyclerView;
        private Context mcontext;

        public MenuTypeRecyclerAdapter(List<MenuType> menuType, RecyclerView recyclerView, Context context)
        {
            mMenuType = menuType;
            mRecyclerView = recyclerView;
            mcontext = context;
        }

        public class MenuTypeView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mMenu { get; set; }
            public TextView mRestaurantID { get; set; }


            public MenuTypeView(View view) : base(view)
            {
                mMainView = view;

            }
        }
        public override int ItemCount
        {
            get { return mMenuType.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MenuTypeView myHolder = holder as MenuTypeView;

            myHolder.mMenu.Text = mMenuType[position].Menu;
            myHolder.mMainView.Click += mMainView_Click;

            //myHolder.mMainView.Click += delegate {
                
               // myHolder.mMainView.Context.StartActivity(typeof(ClientLogin));
            //};
            //myHolder.mRestaurantID.Text = mMenuType[position].RestaurantID;


        }

        private void mMainView_Click(object sender, EventArgs e)
        {

            int position = mRecyclerView.GetChildAdapterPosition((View)sender);
            //int indexPosition = (mMenuType.Count) - position;
            //string namewoo = (mMenuType[position].Menu);
            Console.WriteLine(mMenuType[position].Menu);
            Intent intent = new Intent(mcontext, typeof(MenuDisplayActivity));
            intent.PutExtra("MenuType", mMenuType[position].Menu);
            intent.PutExtra("RestaurantID", mMenuType[position].RestaurantID);
            mcontext.StartActivity(intent);

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.MenuTypeLayout, parent, false);

            TextView textMenu = row.FindViewById<TextView>(Resource.Id.menuType);

            MenuTypeView view = new MenuTypeView(row)
            {
                mMenu = textMenu

            };
            return view;
        }

    }
}