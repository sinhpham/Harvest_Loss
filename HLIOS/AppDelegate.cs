using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.SlideoutNavigation;
using MonoTouch.Dialog;
using HLIOSCore;
using System.IO;

namespace HLIOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        UIWindow window;

        public SlideoutNavigationController MainScreen { get; private set; }
        //
        // This method is invoked when the application has loaded and is ready to run. In this
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // create a new window instance based on the screen size
            window = new UIWindow(UIScreen.MainScreen.Bounds);

            if (!HLDatabase.DbExisted)
            {
                var dataLines = File.ReadLines("Data/CropData.txt");
                HLDatabase.CreateDummyData(dataLines);
            }

            MainScreen = new SlideoutNavigationController();
            MainScreen.TopView = new CalcDVC();
            MainScreen.MenuView = new SlideoutMenuDVC();
			
            // If you have defined a root view controller, set it here:
            window.RootViewController = MainScreen;
			
            // make the window visible
            window.MakeKeyAndVisible();
			
            return true;
        }
    }

    public class SlideoutMenuDVC : DialogViewController
    {
        public SlideoutMenuDVC() 
            : base(UITableViewStyle.Plain,new RootElement(""))
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Root.Add(new Section()
            {
                new StringElement("Main", () =>
                {
                    NavigationController.PopToRootViewController(false);
                    NavigationController.PushViewController(new CalcDVC(), true);
                }),

                new StringElement("Help", () =>
                {
                    NavigationController.PopToRootViewController(false);
                    NavigationController.PushViewController(new HelpDVC(), true);

                }),
                new StringElement("About", () =>
                {
                    NavigationController.PopToRootViewController(false);
                    NavigationController.PushViewController(new AboutDVC(), true);
                })
            });
        }
    }
}

