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
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.MenuTab, container, false);
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView1);

            //get ID of restaurant 
            getID = RestaurantProfileActivity.sendData();
            if (getID != null)
            {
                getPhotos(getID);
            }
            else
            {
                Toast.MakeText(Context as Activity, "Nothing again", ToastLength.Short).Show();
            }

            return view;
        }

        //getting all the photos for the restaurant
        async void getPhotos(string getID)
        {
            mPhotos = new List<Photos>();
            string uri = "https://zeno.computing.dundee.ac.uk/2018-projects/foodfinder/api/Photos/";

            string otherhalf = "getPhotos?ID= " + getID;

            Uri result = null;

            if (Uri.TryCreate(new Uri(uri), otherhalf, out result))
            {
                var httpClient = new HttpClient();
                var refineResult = (await httpClient.GetStringAsync(result));
                mPhotos = JsonConvert.DeserializeObject<List<Photos>>(refineResult);

                //displaying photos in a grid layout of two columns
                mLayoutManager = new GridLayoutManager(Context as Activity, 2);
                mRecyclerView.SetLayoutManager(mLayoutManager);
                mAdapter = new PhotosRecyclerAdapter(mPhotos, mRecyclerView, Context as Activity);
                mRecyclerView.SetAdapter(mAdapter);
            }
        }
    }

    //Binding photo content to views in recycler view
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
            //calling ImageHelper class to display image
            myHolder.mPhotoFilePath.SetImageBitmap(ImageHelper.GetImageBitmapFromUrl(mPhotos[position].PhotoFilePath));

            //photo is clicked
            myHolder.mMainView.Click += mMainView_Click;
        }

        private void mMainView_Click(object sender, EventArgs e)
        {
            //getting the index of the clicked photo and sending its info to PhotoActivity so it can be displayed full size
            int position = mRecyclerView.GetChildAdapterPosition((View)sender);
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