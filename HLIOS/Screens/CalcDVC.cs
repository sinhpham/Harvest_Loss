using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using HLIOSCore;

namespace HLIOS
{
    public partial class CalcDVC : DialogViewController
    {
        public CalcDVC() : base (UITableViewStyle.Grouped, null)
        {
            Root = new RootElement("Harvest Loss");

            var sec = new Section()
            {
                CreateRadioRootEle("Method", "m",
                                           () =>
                {
                    return new List<string>() { "Weight", "Volume", "Count" };
                },
                                           str =>
                {
                }),
                CreateRadioRootEle("Crop", "c",
                                           () =>
                                           {
                    return HLDatabase.GetTable<Crop>();
                },
                str =>
                {
                }),
                new EntryElement("Cut width", null, null),
                new EntryElement("Sieve width", null, null),
                new EntryElement("Collecting area", null, null),
                new EntryElement("Expected yield", null, null),
                new EntryElement("Price", null, null),
                new EntryElement("Seed loss", null, null),
            };

            var resSec = new Section("Results")
            {
                new StringElement("Loss per acre(lbs)"),
                new StringElement("Loss per acre(bu)"),
                new StringElement("Loss (%)"),
                new StringElement("Loss Value($/acre)"),
            };


            Root.Add(new List<Section>{sec, resSec});

        }

        static public RootElement CreateRadioRootEle<T>(string caption, string groupKey, Func<IList<T>> listFunc, Action<T> selectedAct)
        {
            var rootEle = new RootElement(caption, new RadioGroup(groupKey, 0));
            rootEle.createOnSelected = new Func<RootElement, UIViewController>(arg =>
                                                                               {
                var reVc = new DialogViewController(arg, true);

                var list = listFunc();
                selectedAct(list[arg.RadioSelected]);

                var sec = new Section();

                foreach (var ele in list)
                {
                    var mre = new MyRadioElement(ele.ToString());
                    mre.OnSelected += (object sender, EventArgs e) =>
                    {
                        var si = list[rootEle.RadioSelected];
                        selectedAct(si);
                        reVc.NavigationController.PopViewControllerAnimated(true);
                    };
                    sec.Add(mre);
                }
                arg.Clear();
                arg.Add(sec);
                return reVc;
            });

            return rootEle;
        }
    }

    public class MyRadioElement : RadioElement
    {
        public MyRadioElement(string caption):base(caption)
        {

        }

        public override void Selected(DialogViewController dvc, MonoTouch.UIKit.UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
        {
            base.Selected(dvc, tableView, indexPath);

            var selected = OnSelected;
            if (selected != null)
                selected(this, EventArgs.Empty);
        }

        public event EventHandler<EventArgs> OnSelected;
    }
}
