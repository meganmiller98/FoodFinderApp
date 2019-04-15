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
    public class PhotosFragment : Android.Support.V4.App.Fragment
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.Adapter mAdapter;
        private List<Photos> mPhotos;
        string getID;
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
                getPhotos(getID);
            }
            else
            {
                Toast.MakeText(Context as Activity, "Nothing again", ToastLength.Short).Show();
            }

            return view;
        }

        async void getPhotos(string getID)
        {
            mPhotos = new List<Photos>();
            //myIp
            //string uri = "htp://192.168.0.20:45455/api/Photos/";

            //uni IP
            string uri = "http://10.201.37.145:45455/api/Photos/";

            //string uri = "htp://192.168.1.70:45455/api/Photos/";
            //string uri = "htps://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/Photos/";

            string otherhalf = "getPhotos?ID= " + getID;

            Uri result = null;

            if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
            {
                var httpClient = new HttpClient();
                var refineResult = (await httpClient.GetStringAsync(result));
                mPhotos = JsonConvert.DeserializeObject<List<Photos>>(refineResult);

                mLayoutManager = new GridLayoutManager(Context as Activity, 2);
                mRecyclerView.SetLayoutManager(mLayoutManager);
                mAdapter = new PhotosRecyclerAdapter(mPhotos, mRecyclerView, Context as Activity);
                mRecyclerView.SetAdapter(mAdapter);
            }
        }
    }

    public class PhotosRecyclerAdapter : RecyclerView.Adapter
    {
        private List<Photos> mPhotos;
        private RecyclerView mRecyclerView;
        private Context mcontext;

        public PhotosRecyclerAdapter(List<Photos> photos, RecyclerView recyclerView, Context context)
        {
            mPhotos = photos;
            mRecyclerView = recyclerView;
            mcontext = context;
        }

        public class MenuTypeView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mIDPhotos { get; set; }
            public TextView mRestaurantID { get; set; }
            public ImageView mPhotoFilePath { get; set; }


            public MenuTypeView(View view) : base(view)
            {
                mMainView = view;

            }
        }
        public override int ItemCount
        {
            get { return mPhotos.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MenuTypeView myHolder = holder as MenuTypeView;
            Console.Write(mPhotos[position].PhotoFilePath);
            myHolder.mPhotoFilePath.SetImageBitmap(ImageHelper.GetImageBitmapFromUrl(mPhotos[position].PhotoFilePath));

            myHolder.mMainView.Click += mMainView_Click;
        }

        private void mMainView_Click(object sender, EventArgs e)
        {

            int position = mRecyclerView.GetChildAdapterPosition((View)sender);
            //int indexPosition = (mMenuType.Count) - position;
            //string namewoo = (mMenuType[position].Menu);
            Console.WriteLine(mPhotos[position].PhotoFilePath);
            Intent intent = new Intent(mcontext, typeof(PhotoActivity));
            intent.PutExtra("PhotoFilePath", mPhotos[position].PhotoFilePath);
            intent.PutExtra("PhotoID", mPhotos[position].IDPhotos);
            mcontext.StartActivity(intent);

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.PhotoLayout, parent, false);

            ImageView image = row.FindViewById<ImageView>(Resource.Id.imageView1);
            
            MenuTypeView view = new MenuTypeView(row)
            {
                mPhotoFilePath = image

            };
            return view;
        }

    }
}