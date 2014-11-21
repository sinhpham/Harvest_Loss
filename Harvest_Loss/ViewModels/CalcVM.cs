using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace Harvest_Loss
{
    public class CalcVM : NotifyingClass
    {
        public CalcVM()
        {
            _crops = new List<Crop>();

            var assembly = typeof(CalcVM).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream("Harvest_Loss.Data.CropData.txt"))
            {
                var lines = Helpers.ReadLines(stream, System.Text.Encoding.UTF8);
                foreach (var line in lines)
                {
                    var str = line.Split(new char[] { ',' });

                    var currCrop = new Crop
                    {
                        Name = str[0],
                        LbsPBushel = double.Parse(str[1]),
                        BushelPTonne = double.Parse(str[2]),
                        KgPBushel = double.Parse(str[3]),
                        KernelWeight = double.Parse(str[4])
                    };

                    _crops.Add(currCrop);
                }
            }

            CurrMethod = Method.Weight;
        }

        // Input.
        Method _currMethod;

        public Method CurrMethod
        {
            get { return _currMethod; }
            set
            {
                SetProperty(ref _currMethod, value);
                RefreshResult();
            }
        }

        List<Crop> _crops;

        public List<Crop> Crops
        {
            get { return _crops; }
        }

        Crop _currCrop;

        public Crop CurrCrop
        {
            get { return _currCrop; }
            set
            {
                SetProperty(ref _currCrop, value);
                RefreshResult();
            }
        }

        double? _cutWidth;

        public double? CutWidth
        {
            get { return _cutWidth; }
            set
            {
                SetProperty(ref _cutWidth, value);
                RefreshResult();
            }
        }

        double? _sieveWidth;

        public double? SieveWidth
        {
            get { return _sieveWidth; }
            set
            {
                SetProperty(ref _sieveWidth, value);
                RefreshResult();
            }
        }

        double? _collectingArea;

        public double? CollectingArea
        {
            get { return _collectingArea; }
            set
            {
                SetProperty(ref _collectingArea, value);
                RefreshResult();
            }
        }

        double? _expectedYield;

        public double? ExpectedYield
        {
            get { return _expectedYield; }
            set
            {
                SetProperty(ref _expectedYield, value);
                RefreshResult();
            }
        }

        double? _price;

        public double? Price
        {
            get { return _price; }
            set
            {
                SetProperty(ref _price, value);
                RefreshResult();
            }
        }

        double? _seedLoss;

        public double? SeedLoss
        {
            get { return _seedLoss; }
            set
            {
                SetProperty(ref _seedLoss, value);
                RefreshResult();
            }
        }

        // Results.
        double? _lossPerAcreLbs;

        public double? LossPerAcreLbs
        {
            get { return _lossPerAcreLbs; }
            set { SetProperty(ref _lossPerAcreLbs, value); }
        }

        double? _lossPercent;

        public double? LossPercent
        {
            get { return _lossPercent; }
            set { SetProperty(ref _lossPercent, value); }
        }

        double? _lossValue;

        public double? LossValue
        {
            get { return _lossValue; }
            set { SetProperty(ref _lossValue, value); }
        }

        public enum Method
        {
            Weight,
            Volume,
            Count
        }

        void RefreshResult()
        {
            // Clear all values first.
            LossPerAcreLbs = null;
            LossPercent = null;
            LossValue = null;

            if (CutWidth == null || SieveWidth == null || SeedLoss == null ||
                CollectingArea == null || CurrCrop == null)
            {
                return;
            }
            var currSeedLossInG = SeedLoss.Value;
            if (CurrMethod == Method.Count)
            {
                currSeedLossInG = SeedLoss.Value * _currCrop.KernelWeight / 1000;
            }
            else if (CurrMethod == Method.Volume)
            {
                currSeedLossInG = Helpers.MlToG(SeedLoss.Value, _currCrop.KgPBushel);
            }


            var concenFactor = CutWidth.Value / Helpers.InchToFeet(SieveWidth.Value);

            var collectingAreasi = Helpers.SquareFeetToMeters(CollectingArea.Value);

            var lph = 10 * currSeedLossInG / concenFactor / collectingAreasi;
            var lpalbs = Helpers.KgPHaToLbsPAcre(lph);

            Debug.WriteLine("Loss per ha: {0}", lph);
            Debug.WriteLine("Loss per acre lbs: {0}", lpalbs);

            LossPerAcreLbs = lpalbs;

            var lpabu = lpalbs / CurrCrop.LbsPBushel;
//            Debug.WriteLine("Loss per acre bu: {0}", lpabu);
//            _lpaBu.Value = lpabu.ToString("F");

            if (ExpectedYield.HasValue)
            {
                LossPercent = lpabu / ExpectedYield.Value;
                Debug.WriteLine("Percent loss: {0}", LossPercent);
            }


            if (Price.HasValue)
            {
                LossValue = Price.Value * lpabu;
                Debug.WriteLine("Loss value: {0}", LossPercent);
            }
        }
    }

    public class CurrMethodToUnitCov : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var m = (CalcVM.Method)value;
            switch (m)
            {
                case CalcVM.Method.Weight:
                    return "g";
                case CalcVM.Method.Volume:
                    return "ml";
                case CalcVM.Method.Count:
                    return "number";
            }
            throw new InvalidOperationException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

