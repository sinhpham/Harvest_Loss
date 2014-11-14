using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Harvest_Loss
{
    public class MenuPage : ContentPage
    {
        public MenuPage()
        {
            menuItems = new List<MenuItem>();
            menuItems.Add(new MenuItem("Main", () => new ContentPage()));
            menuItems.Add(new MenuItem("About", () => new ContentPage()));
            menuItems.Add(new MenuItem("Help", () => new ContentPage()));

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

            var layout = new StackLayout { Spacing = 0, VerticalOptions = LayoutOptions.FillAndExpand };

            var menuList = new ListView
            {
                ItemsSource = menuItems,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Transparent,
            };

            var cell = new DataTemplate(typeof(TextCell));
            cell.SetBinding(TextCell.TextProperty, "MenuTitle");
            cell.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);

            menuList.ItemTemplate = cell;

            menuList.ItemSelected += (object sender, SelectedItemChangedEventArgs e) =>
            {
                if (e.SelectedItem != null)
                {
                    var mi = (MenuItem)e.SelectedItem;
                    menuList.SelectedItem = null;
                    _chosenItemCmd.Execute(mi);
                }
            };

            layout.Children.Add(menuList);

            Content = layout;
        }

        List<MenuItem> menuItems;

        public Page DefaultPage
        {
            get
            {
                return menuItems[0].NaviPage;
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

