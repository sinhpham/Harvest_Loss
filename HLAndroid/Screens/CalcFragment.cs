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
	public class CalcFragment : SherlockFragment
    {
		public override void OnCreate(Bundle savedInstanceState)
		{
			SetHasOptionsMenu(true);

			base.OnCreate(savedInstanceState);
		}


		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return inflater.Inflate(Resource.Layout.Main, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			SetupEditTextFields();
			base.OnViewCreated(view, savedInstanceState);
		}

		void SetupEditTextFields()
		{
			var cutWidth = Activity.FindViewById<EditText>(Resource.Id.editCutWidth);
			var sieveWidth = Activity.FindViewById<EditText>(Resource.Id.editSieveWidth);
			var collectingArea = Activity.FindViewById<EditText>(Resource.Id.editCollectingArea);
			var expectedYield = Activity.FindViewById<EditText>(Resource.Id.editExpectedYield);
			var price = Activity.FindViewById<EditText>(Resource.Id.editPrice);
			var seedLoss = Activity.FindViewById<EditText>(Resource.Id.editSeedLoss);

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

		public void MethodClicked(View v)
		{
			const string wStr = "Weight";
			const string vStr = "Volume";
			const string cStr = "Count";
			var l = new List<string> { wStr, vStr, cStr };

			var tvsl = Activity.FindViewById<TextView>(Resource.Id.tvSeedLoss);
			CreateSelectDialog(l, x => x, "Select method", selected =>
				{
					switch (selected)
					{
						case wStr:
							_inputParas.CurrChoice = MethodChoice.Weight;
							tvsl.Text = "Seed loss (g)";
							break;
						case vStr:
							_inputParas.CurrChoice = MethodChoice.Volume;
							tvsl.Text = "Seed loss (ml)";
							break;
						case cStr:
							_inputParas.CurrChoice = MethodChoice.Count;
							tvsl.Text = "Seed loss (number)";
							break;
						default:
							throw new InvalidDataException();
					}

					((Button)v).Text = selected;
					RefreshResult();
				});
		}

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
			var adap = new ArrayAdapter(this.Activity, Android.Resource.Layout.SimpleSpinnerDropDownItem, displayList);

			new AlertDialog.Builder(this.Activity)
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
				var resLbs = Activity.FindViewById<TextView>(Resource.Id.resLbs);
				var resBu = Activity.FindViewById<TextView>(Resource.Id.resBu);
				var resLossPercent = Activity.FindViewById<TextView>(Resource.Id.resLossPercent);
				var resLossValue = Activity.FindViewById<TextView>(Resource.Id.resLossValue);

				resLbs.Text = resLbs.Text.Substring(0, resLbs.Text.IndexOf(':') + 1) + " " + res.LpaLbs;
				resBu.Text = resBu.Text.Substring(0, resBu.Text.IndexOf(':') + 1) + " " + res.LpaBu;
				resLossPercent.Text = resLossPercent.Text.Substring(0, resLossPercent.Text.IndexOf(':') + 1) + " " + res.PercentLoss;
				resLossValue.Text = resLossValue.Text.Substring(0, resLossValue.Text.IndexOf(':') + 1) + " " + res.LossValue;
			}
		}
    }
}

