using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.SlideoutNavigation;
using MonoTouch.Dialog;

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
                new StringElement("Weight", () =>
                {
//                    BFCIOSGlobal.ClearAll();
//                    NavigationController.PushViewController(new CalcDialogController(), true);
                }),
                new StringElement("Volume", () =>
                {
//                    BFCIOSGlobal.ClearAll();
//                    NavigationController.PushViewController(new CalcAerialDialog(), true);
                }),
                new StringElement("Count", () =>
                {
//                    BFCIOSGlobal.ClearAll();
//                    NavigationController.PushViewController(new CalcOrchardDialog(), true);
                }),
                new StringElement("Help", () =>
                {
//                    NavigationController.PushViewController(new HelpViewController(), true);
                })
            });
        }
    }
}

