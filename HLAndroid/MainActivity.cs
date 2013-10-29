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

namespace HLAndroid
{
    [Activity(Label = "HLAndroid", MainLauncher = true)]
    public class MainActivity : SlidingFragmentActivity, MenuFragment.ISlidingMenuAct
    {
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
            SlidingMenu.SetShadowDrawable(Resource.Drawable.SlidingMenuShadow);
            SlidingMenu.SetBehindWidth(500);
            SlidingMenu.SetFadeDegree(0.35f);
            SlidingMenu.TouchModeAbove = Com.Slidingmenu.Lib.SlidingMenu.TouchmodeFullscreen;



            //HLDatabase.CreateDummyData(dataLines);
        }

        public void SelectedItemChanged(int position, string label)
        {
            // TODO: handle menu item changed.
            Console.WriteLine(label);
        }

        [Export]
        public void MethodClicked(View v)
        {
            var l = new List<string> { "Weight", "Volume", "Count" };
            CreateSelectDialog(l, x => x, "Select method", selected =>
            {

            });
        }

        [Export]
        public void CropClicked(View v)
        {
            var l = new List<string>();
            for(var i = 0; i < 100; ++i) {
                l.Add(i.ToString());
            }
            CreateSelectDialog(l, x => x, "Select method", selected =>
            {

            });
        }

        private void CreateSelectDialog<T>(IList<T> list, Func<T, string> display, string title, Action<T> selectedAction)
        {
            // Return selected index
            var displayList = list.Select(display).ToArray();
            var adap = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, displayList);

            new AlertDialog.Builder(this)
                .SetTitle(title)
                    .SetSingleChoiceItems(adap, -1, (s, arg) =>
            {
                var ad = (AlertDialog)s;
                var pos = ad.ListView.CheckedItemPosition;
                ad.Dismiss();
                selectedAction(list[pos]);
            }).Create().Show();
        }
    }
}


