using System;
using System.Reflection;
using System.Collections.Generic;

namespace Harvest_Loss
{
    public class CalcVM : NotifyingClass
    {
        public CalcVM()
        {
            LossPerAcreLbs = 1;
            LossPercent = 10;
            LossValue = 50;

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
        }

        Method _currMethod;

        public Method CurrMethod
        {
            get { return _currMethod; }
            set { SetProperty(ref _currMethod, value); }
        }

        List<Crop> _crops;

        Crop _currCrop;

        public Crop CurrCrop
        {
            get { return _currCrop; }
            set { SetProperty(ref _currCrop, value); }
        }

        double _cutWidth;

        public double CutWidth
        {
            get { return _cutWidth; }
            set { SetProperty(ref _cutWidth, value); }
        }

        double _sieveWidth;

        public double SieveWidth
        {
            get { return _sieveWidth; }
            set { SetProperty(ref _sieveWidth, value); }
        }

        double _collectingArea;

        public double CollectingArea
        {
            get { return _collectingArea; }
            set { SetProperty(ref _collectingArea, value); }
        }

        double _expectedYield;

        public double ExpectedYield
        {
            get { return _expectedYield; }
            set { SetProperty(ref _expectedYield, value); }
        }

        double _price;

        public double Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }

        double _seedLoss;

        public double SeedLoss
        {
            get { return _seedLoss; }
            set { SetProperty(ref _seedLoss, value); }
        }

        double _lossPerAcreLbs;

        public double LossPerAcreLbs
        {
            get { return _lossPerAcreLbs; }
            set { SetProperty(ref _lossPerAcreLbs, value); }
        }

        double _lossPercent;

        public double LossPercent
        {
            get { return _lossPercent; }
            set { SetProperty(ref _lossPercent, value); }
        }

        double _lossValue;

        public double LossValue
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
    }
}

