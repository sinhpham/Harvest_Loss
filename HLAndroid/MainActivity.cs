using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Com.Slidingmenu.Lib.App;

namespace HLAndroid
{
    [Activity(Label = "HLAndroid", MainLauncher = true)]
    public class MainActivity : SlidingFragmentActivity, MenuFragment.ISlidingMenuAct
    {
        int count = 1;

        public override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            SetBehindContentView(Resource.Layout.Menu);



            SupportFragmentManager.BeginTransaction()
                .Add(Resource.Id.menu_fragment_container, new MenuFragment())
                    .Commit();

            // Setup action bar
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            // Setup the sliding menu
            SlidingMenu.SetShadowWidth(50);
            SlidingMenu.SetShadowDrawable (Resource.Drawable.SlidingMenuShadow);
            SlidingMenu.SetBehindWidth(500);
            SlidingMenu.SetFadeDegree(0.35f);
            SlidingMenu.TouchModeAbove = Com.Slidingmenu.Lib.SlidingMenu.TouchmodeFullscreen;

        }

        public void SelectedItemChanged(int position, string label)
        {
            Console.WriteLine(label);
        }
    }
}


