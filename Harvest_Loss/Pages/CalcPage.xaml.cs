using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Harvest_Loss
{
    public partial class CalcPage : ContentPage
    {
        public CalcPage()
        {
            InitializeComponent();

            BindingContext = App.CalcVM;


            foreach (var item in MethodToPickerIdxCov.Names)
            {
                _methodPicker.Items.Add(item);
            }

            _methodPicker.SelectedIndex = 0;

            _cropPicker.Clicked += (object sender, EventArgs e) =>
            {
                Navigation.PushAsync(new CropList());
            };

            if (Device.OS == TargetPlatform.Android)
            {
                var a = (Grid)_mainGrid;

                foreach (var v in a.Children)
                {
                    var lbl = v as Label;
                    if (lbl != null)
                    {
                        lbl.Font = Font.SystemFontOfSize(NamedSize.Large);
                        lbl.TextColor = Color.White;
                        if (lbl.Text == "Inputs" || lbl.Text == "Results")
                        {
                            lbl.Font = Font.SystemFontOfSize(NamedSize.Large, FontAttributes.Bold);
                        }
                    }
                }
            }
        }
    }

    public class DoneEntry : Entry
    {
    }
}

