﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace FoodFinder
{
    public class PhotosFragment : Android.Support.V4.App.Fragment
    {
        //private RecyclerView mRecyclerView;
        //private RecyclerView.LayoutManager mLayoutManager;
        //private RecyclerView.Adapter mAdapter;
        //private List<MenuType> mMenuType;
        //string getID;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.PhotosTab, container, false);


            return view; ;
        }
    }
}