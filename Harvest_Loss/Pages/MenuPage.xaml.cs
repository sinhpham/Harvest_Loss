using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Harvest_Loss
{
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            _chosenItemCmd = new Command(obj =>
            {
                var handler = MenuItemChanged;
                if (handler != null)
                {
                    handler(this, new MenuItemChangedEventArg()
                    {
                        SelectedMenuItem = obj as MenuItem,
                    });
                }
            });

            _menuItems = new List<MenuItem>();
            _menuItems.Add(new MenuItem("Main", () => new CalcPage()));
            _menuItems.Add(new MenuItem("About", () => new AboutPage()));
            _menuItems.Add(new MenuItem("Help", () => new HelpPage()));

            InitializeComponent();

            _menuList.ItemSelected += (object sender, SelectedItemChangedEventArgs e) =>
            {
                if (e.SelectedItem != null)
                {
                    var mi = (MenuItem)e.SelectedItem;
                    _menuList.SelectedItem = null;
                    _chosenItemCmd.Execute(mi);
                }
            };


        }

        List<MenuItem> _menuItems;

        public List<MenuItem> MenuItems
        {
            get { return _menuItems; }
        }

        public Page DefaultPage
        {
            get
            {
                return MenuItems[0].NaviPage;
            }
        }

        public event EventHandler<MenuItemChangedEventArg> MenuItemChanged;

        Command _chosenItemCmd;
    }

    public class MenuItemChangedEventArg : EventArgs
    {
        public MenuItem SelectedMenuItem { get; set; }
    }

    public class MenuItem
    {
        public MenuItem(string title, Func<Page> initRootPage)
        {
            MenuTitle = title;

            _initRootPage = initRootPage;
        }

        Func<Page> _initRootPage;

        public string MenuTitle { get; set; }

        NavigationPage _naviPage;

        public NavigationPage NaviPage
        {
            get
            {
                // Delay initiation of a page until requested.
                if (_naviPage == null)
                {
                    _naviPage = new NavigationPage(RootPage);
                }
                return _naviPage;
            }
        }

        Page _rootPage;

        public Page RootPage
        {
            get
            {
                if (_rootPage == null)
                {
                    _rootPage = _initRootPage();
                    _rootPage.Title = MenuTitle;
                    _initRootPage = null;
                }
                return _rootPage;
            }
        }
    }
}

