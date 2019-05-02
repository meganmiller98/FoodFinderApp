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
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace FoodFinder
{
    [Activity(Label = "MenuDisplayActivity")]
    public class MenuDisplayActivity : Activity
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.Adapter mAdapter;
        private List<MenuItems> mMenuItems;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MenuDisplayLayout);

            string ID = Intent.GetStringExtra("RestaurantID");
            string menuType = Intent.GetStringExtra("MenuType");

            Console.WriteLine(ID + " " + menuType);

            Android.Widget.Toolbar toolbar = FindViewById<Android.Widget.Toolbar>(Resource.Id.toolbar1);
            SetActionBar(toolbar);
            ActionBar.Title = menuType;

            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);

            getMenuItems(ID, menuType);
        }

        //get Menu items for restaurant
        async void getMenuItems(string ID, string menuType)
        {
            mMenuItems = new List<MenuItems>();
            
            string uri = "https://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/MenuItems/";
            string otherhalf = "getMenuItems?ID=" + ID + "&menutype=" + menuType;

            Uri result = null;

            if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
            {
                var httpClient = new HttpClient();
                var refineResult = (await httpClient.GetStringAsync(result));
                mMenuItems = JsonConvert.DeserializeObject<List<MenuItems>>(refineResult);

                mLayoutManager = new LinearLayoutManager(this);
                mRecyclerView.SetLayoutManager(mLayoutManager);
                mAdapter = new MenuItemsRecyclerAdapter(mMenuItems, mRecyclerView, this);
                mRecyclerView.SetAdapter(mAdapter);
            }
        }
    }

    //Binding menu items data to items view in recycler view
    public class MenuItemsRecyclerAdapter : RecyclerView.Adapter
    {
        private List<MenuItems> mMenuItems;
        private RecyclerView mRecyclerView;
        private Context mcontext;

        public MenuItemsRecyclerAdapter(List<MenuItems> menuItems, RecyclerView recyclerView, Context context)
        {
            mMenuItems = menuItems;
            mRecyclerView = recyclerView;
            mcontext = context;
        }

        public class MenuTypeView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mRestaurantID { get; set; }
            public TextView mItem { get; set; }
            public TextView mPrice { get; set; }
            public TextView mDescription { get; set; }


            public MenuTypeView(View view) : base(view)
            {
                mMainView = view;

            }
        }
        public override int ItemCount
        {
            get { return mMenuItems.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MenuTypeView myHolder = holder as MenuTypeView;

            myHolder.mItem.Text = mMenuItems[position].Item;
            myHolder.mPrice.Text = "£" + mMenuItems[position].Price;
            myHolder.mDescription.Text = mMenuItems[position].Description;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.MenuItemLayout, parent, false);

            TextView textName = row.FindViewById<TextView>(Resource.Id.dishName);
            TextView textPrice = row.FindViewById<TextView>(Resource.Id.dishPrice);
            TextView textDescription = row.FindViewById<TextView>(Resource.Id.dishDescription);

            MenuTypeView view = new MenuTypeView(row)
            {
                mItem = textName,
                mPrice = textPrice,
                mDescription = textDescription
            };
            return view;
        }

    }
}