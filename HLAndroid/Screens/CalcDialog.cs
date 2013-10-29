using System;
using CrossUI.Droid.Dialog;
using CrossUI.Droid.Dialog.Elements;
using Android.App;

namespace HLAndroid
{
    [Activity]
    public class CalcDialog : DialogActivity
    {
     
        public override Android.Views.View OnCreateView(string name, Android.Content.Context context, Android.Util.IAttributeSet attrs)
        {
            Root = new RootElement("Test");
            return base.OnCreateView(name, context, attrs);
        }
    }
}

