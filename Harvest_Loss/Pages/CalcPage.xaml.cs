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

            _cropPicker.Clicked += (object sender, EventArgs e) =>
            {
                Navigation.PushAsync(new CropList());
            };

        }
    }
}

