using System;

namespace Harvest_Loss
{
    public class CalcVM : NotifyingClass
    {
        public CalcVM()
        {
        }

        double _cutWidth;

        public double CutWidth
        {
            get { return _cutWidth; }
            set { SetProperty(ref _cutWidth, value); }
        }
    }
}

