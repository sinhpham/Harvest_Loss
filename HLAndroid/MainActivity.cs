using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Com.Slidingmenu.Lib.App;
using System.Collections.Generic;
using System.Linq;
using Java.Interop;
using HLIOSCore;
using System.IO;
using Android.Content.Res;
using System.Text;

namespace HLAndroid
{
	[Activity(Label = "HLAndroid", MainLauncher = true)]
	public class MainActivity : SlidingFragmentActivity, MenuFragment.ISlidingMenuAct
	{
		public override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.MainFragContainer);
			SetBehindContentView(Resource.Layout.Menu);

			SupportFragmentManager.BeginTransaction()
				.Add(Resource.Id.fragment_container, new CalcFragment(), CalcFragTag)
				.Commit();

			SupportFragmentManager.BeginTransaction()
                .Add(Resource.Id.menu_fragment_container, new MenuFragment())
                    .Commit();

			// Setup action bar
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);

			// Setup the sliding menu
			SlidingMenu.SetShadowWidth(50);
			SlidingMenu.SetShadowDrawable(Resource.Drawable.SlidingMenuShadow);
			SlidingMenu.SetBehindWidth(500);
			SlidingMenu.SetFadeDegree(0.35f);
			SlidingMenu.TouchModeAbove = Com.Slidingmenu.Lib.SlidingMenu.TouchmodeFullscreen;

			if (!HLDatabase.DbExisted)
			{
				var dl = Helper.ReadLines(() => Assets.Open("CropData.txt"), Encoding.ASCII);
				HLDatabase.CreateDummyData(dl);
			}
		}

		const string CalcFragTag = "calfragtag";



		public override bool OnOptionsItemSelected(ActionbarSherlock.View.IMenuItem p0)
		{
			var text = p0.TitleFormatted.ToString();
			if (text == Title)
			{
				SlidingMenu.ShowMenu();
				return true;
			}
			return base.OnOptionsItemSelected(p0);
		}

		[Export]
		public void MethodClicked(View v)
		{
			var f = SupportFragmentManager.FindFragmentByTag (CalcFragTag);
			if (f != null) {
				((CalcFragment)f).MethodClicked(v);
			}
		}

		[Export]
		public void CropClicked(View v)
		{
			var f = SupportFragmentManager.FindFragmentByTag (CalcFragTag);
			if (f != null) {
				((CalcFragment)f).CropClicked(v);
			}
		}

		public void SelectedItemChanged(int position, string label)
		{
			// TODO: handle menu item changed.
			switch (label)
			{
				case "Home":
					{
						var calcFrag = new CalcFragment();
						var transaction = SupportFragmentManager.BeginTransaction();
						transaction.Replace(Resource.Id.fragment_container, calcFrag, CalcFragTag);
						transaction.Commit();
						SlidingMenu.ShowContent();
					}
					break;
				case "Help":
					{
						var hFrag = new HelpFrag();
						var transaction = SupportFragmentManager.BeginTransaction();
						transaction.Replace(Resource.Id.fragment_container, hFrag);
						transaction.Commit();
						SlidingMenu.ShowContent();
					}
					break;
				case "About":
					{
						var aFrag = new AboutFrag();
						var transaction = SupportFragmentManager.BeginTransaction();
						transaction.Replace(Resource.Id.fragment_container, aFrag);
						transaction.Commit();
						SlidingMenu.ShowContent();
					}
					break;
				default:
					throw new InvalidOperationException();
			}
		}
	}
}


