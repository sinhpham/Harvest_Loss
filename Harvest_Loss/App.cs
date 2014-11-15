using System;
using Xamarin.Forms;

namespace Harvest_Loss
{
    public class App
    {
        static Page _rootPage;

        public static Page GetMainPage()
        {	
            if (_rootPage == null)
            {
                _rootPage = new RootPage();
            }
            return _rootPage;
        }

        static CalcVM _calcVM;

        public static CalcVM CalcVM
        {
            get
            {
                if (_calcVM == null)
                {
                    _calcVM = new CalcVM();
                }
                return _calcVM;
            }
        }
    }
}

