using System;
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
using ActionbarSherlock.App;
using HLIOSCore;
using System.IO;

namespace HLAndroid
{
	public class AboutFrag : SherlockFragment
	{
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return inflater.Inflate(Resource.Layout.AboutLayout, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			var tv = Activity.FindViewById<TextView>(Resource.Id.textView1);
			using (var s = Activity.Assets.Open("About.txt"))
			{
				using (var reader = new StreamReader(s))
				{
					var contents = reader.ReadToEnd();
					tv.Text = contents;
				}
			}

			base.OnViewCreated(view, savedInstanceState);
		}
	}
}

