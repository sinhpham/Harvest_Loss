using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Reflection;
using System.IO;

namespace Harvest_Loss
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            var assembly = typeof(CalcVM).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream("Harvest_Loss.Data.About.txt"))
            {
                using (var sr = new StreamReader(stream))
                {
                    var text = sr.ReadToEnd();
                    _label.Text = text;
                }
            }
        }
    }
}

