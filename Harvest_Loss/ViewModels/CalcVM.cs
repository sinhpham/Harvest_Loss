using System;

namespace Harvest_Loss
{
    public class CalcVM : NotifyingClass
    {
        public CalcVM()
        {
            LossPerAcreLbs = 1;
            LossPercent = 10;
            LossValue = 50;
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
    }
}

