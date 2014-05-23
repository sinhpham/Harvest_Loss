using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.IO;

namespace HLIOS
{
    public partial class AboutDVC : DialogViewController
    {
        public AboutDVC() : base(UITableViewStyle.Plain, null)
        {
            Root = new RootElement("");
        }

        public override void ViewDidLoad()
        {
            var text = File.ReadAllText("Data/About.txt");
            var frameSize = UIScreen.MainScreen.Bounds;
            frameSize.Height -= UIApplication.SharedApplication.StatusBarFrame.Height + this.NavigationController.NavigationBar.Frame.Height;

            var tv = new UITextView(frameSize);

            tv.Text = text;
            tv.Editable = false;
            tv.DataDetectorTypes = UIDataDetectorType.Link;


            Root = new RootElement("About")
            {
                new Section()
                {
                    new UIViewElement("", tv, false)
                }
            };

            base.ViewDidLoad();
        }
    }
}
