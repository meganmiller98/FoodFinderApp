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

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.MenuTab, container, false);
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView1);

            getID = RestaurantProfileActivity.sendData();
            if (getID != null)
            {
                getMenuTypes(getID);
            }
            else
            {
                Console.WriteLine("can't retrieve restaurant ID");
            }

            return view;
        }

        //get the list of menus for the restaurant
        async void getMenuTypes(string getID)
        {
            mMenuType = new List<MenuType>();

            string uri = "https://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/MenuType/";

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

    //Binding menu type data to view items in recyclerview
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
        }

        private void mMainView_Click(object sender, EventArgs e)
        {
            //get the index of the menu clicked
            int position = mRecyclerView.GetChildAdapterPosition((View)sender);

            //show the menu items for that menu in a new page
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