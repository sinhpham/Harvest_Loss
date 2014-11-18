using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Harvest_Loss
{
    public partial class CropList : ContentPage
    {
        public CropList()
        {
            InitializeComponent();

            BindingContext = App.CalcVM;

            _listView.ItemTapped += (s, arg) =>
            {
                App.CalcVM.CurrCrop = (Crop)_listView.SelectedItem;
                _listView.SelectedItem = null;

                Navigation.PopAsync();
            };
        }
    }
}

