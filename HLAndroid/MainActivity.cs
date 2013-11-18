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

			if (!HLDatabase.DbExisted)
			{
				var dl = ReadLines(() => Assets.Open("CropData.txt"), Encoding.ASCII);
				//var istream = Assets.Open("CropData.txt");
				//var dataLines = File.ReadLines();
				HLDatabase.CreateDummyData(dl);
			}

			SetupEditTextFields();

		}

		void SetupEditTextFields()
		{
			var cutWidth = FindViewById<EditText>(Resource.Id.editCutWidth);
			var sieveWidth = FindViewById<EditText>(Resource.Id.editSieveWidth);
			var collectingArea = FindViewById<EditText>(Resource.Id.editCollectingArea);
			var expectedYield = FindViewById<EditText>(Resource.Id.editExpectedYield);
			var price = FindViewById<EditText>(Resource.Id.editPrice);
			var seedLoss = FindViewById<EditText>(Resource.Id.editSeedLoss);

			cutWidth.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
			{
				_inputParas.CurrCutWidth = ParseEditText((EditText)sender);
				RefreshResult();
			};
			sieveWidth.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
			{
				_inputParas.CurrSieveWidth = ParseEditText((EditText)sender);
				RefreshResult();
			};
			collectingArea.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
			{
				_inputParas.CurrCollectingAreasqft = ParseEditText((EditText)sender);
				RefreshResult();
			};
			expectedYield.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
			{
				_inputParas.CurrExpectedYield = ParseEditText((EditText)sender);
				RefreshResult();
			};
			price.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
			{
				_inputParas.CurrPrice = ParseEditText((EditText)sender);
				RefreshResult();
			};
			seedLoss.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
			{
				_inputParas.CurrSeedLossInput = ParseEditText((EditText)sender);
				RefreshResult();
			};
		}

		double ParseEditText(EditText et)
		{
			if (string.IsNullOrWhiteSpace(et.Text))
			{
				return -1;
			}
			var pValue = 0.0;
			if (!double.TryParse(et.Text, out pValue))
			{
				et.Text = null;
				return -1;
			}
			return pValue;
		}

		public IEnumerable<string> ReadLines(Func<Stream> streamProvider,
		                                     Encoding encoding)
		{
			using (var stream = streamProvider())
			using (var reader = new StreamReader(stream, encoding))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					yield return line;
				}
			}
		}

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

		public void SelectedItemChanged(int position, string label)
		{
			// TODO: handle menu item changed.
			switch (label)
			{
				case "Home":
					{
//						var calcFrag = new CalcFragment();
//						var transaction = SupportFragmentManager.BeginTransaction();
//						transaction.Replace(Resource.Id.fragment_container, calcFrag, CalcFragTag);
//						transaction.Commit();
//						SlidingMenu.ShowContent();
					}
					break;
				case "Help":
					{
//						Toast.MakeText(this.SupportActionBar.ThemedContext, "help clicked", ToastLength.Short).Show();
					}
					break;
				case "About":
					{
//						var aFrag = new AboutFragment();
//						var transaction = SupportFragmentManager.BeginTransaction();
//						transaction.Replace(Resource.Id.fragment_container, aFrag);
//						transaction.Commit();
//						SlidingMenu.ShowContent();
					}
					break;
				default:
					throw new InvalidOperationException();
			}
		}

		[Export]
		public void MethodClicked(View v)
		{
			const string wStr = "Weight";
			const string vStr = "Volume";
			const string cStr = "Count";
			var l = new List<string> { wStr, vStr, cStr };
			CreateSelectDialog(l, x => x, "Select method", selected =>
			{
				switch (selected)
				{
					case wStr:
						_inputParas.CurrChoice = MethodChoice.Weight;
						break;
					case vStr:
						_inputParas.CurrChoice = MethodChoice.Volume;
						break;
					case cStr:
						_inputParas.CurrChoice = MethodChoice.Count;
						break;
					default:
						throw new InvalidDataException();
				}

				((Button)v).Text = selected;
				RefreshResult();
			});
		}

		[Export]
		public void CropClicked(View v)
		{
			var l = HLDatabase.GetTable<Crop>();
			CreateSelectDialog(l, x => x.ToString(), "Select method", selected =>
			{
				_inputParas.CurrCrop = selected;
				((Button)v).Text = selected.ToString();
				RefreshResult();
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

		InputParas _inputParas = new InputParas();

		void RefreshResult()
		{
			var res = Helper.Calc(_inputParas);
			if (res != null)
			{
				var resLbs = FindViewById<TextView>(Resource.Id.resLbs);
				var resBu = FindViewById<TextView>(Resource.Id.resBu);
				var resLossPercent = FindViewById<TextView>(Resource.Id.resLossPercent);
				var resLossValue = FindViewById<TextView>(Resource.Id.resLossValue);

				resLbs.Text = resLbs.Text.Substring(0, resLbs.Text.IndexOf(':') + 1) + res.LpaLbs;
				resBu.Text = resBu.Text.Substring(0, resBu.Text.IndexOf(':') + 1) + res.LpaBu;
				resLossPercent.Text = resLossPercent.Text.Substring(0, resLossPercent.Text.IndexOf(':') + 1) + res.PercentLoss;
				resLossValue.Text = resLossValue.Text.Substring(0, resLossValue.Text.IndexOf(':') + 1) + res.LossValue;
			}
		}
	}
}


