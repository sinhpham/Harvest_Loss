using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using HLIOSCore;
using System.Diagnostics;

namespace HLIOS
{
    public partial class CalcDVC : DialogViewController
    {
        public CalcDVC() : base (UITableViewStyle.Grouped, null)
        {
            Root = new RootElement("Harvest Loss");

            var sec = new Section("Input")
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
                                   () => HLDatabase.GetTable<Crop>(),
                                   selected =>
                {
                    _currCrop = selected;
                    RefreshResult();
                }),
                CreateActionEntryElement("Cut width(ft)", (sender, arg) =>
                {
                    if (!double.TryParse(((EntryElement)sender).Value, out _currCutWidth))
                    {
                        _currCutWidth = -1;
                    }
                    RefreshResult();
                }),
                CreateActionEntryElement("Sieve width(in)", (sender, arg) =>
                {
                    if (!double.TryParse(((EntryElement)sender).Value, out _currSieveWidth))
                    {
                        _currSieveWidth = -1;
                    }
                    RefreshResult();
                }),
                CreateActionEntryElement("Collecting area (sq ft)", (sender, arg) =>
                {
                    if (!double.TryParse(((EntryElement)sender).Value, out _currCollectingAreasqft))
                    {
                        _currCollectingAreasqft = -1;
                    }
                    RefreshResult();
                }),
                CreateActionEntryElement("Expected yield (bu/acre)", (sender, arg) =>
                {
                    if (!double.TryParse(((EntryElement)sender).Value, out _currExpectedYield))
                    {
                        _currExpectedYield = -1;
                    }
                    RefreshResult();
                }),
                CreateActionEntryElement("Price ($/bu)", (sender, arg) =>
                {
                    if (!double.TryParse(((EntryElement)sender).Value, out _currPrice))
                    {
                        _currPrice = -1;
                    }
                    RefreshResult();
                }),
                CreateActionEntryElement("Seed loss (g)", (sender, arg) =>
                {
                    if (!double.TryParse(((EntryElement)sender).Value, out _currSeedLossg))
                    {
                        _currSeedLossg = -1;
                    }
                    RefreshResult();
                }),
            };

            //_lph = new StringElement("Loss per ha (kg)");
            _lpaLbs = new StringElement("Loss per acre (lbs)");
            _lpaBu = new StringElement("Loss per acre (bu)");
            _percentLoss = new StringElement("Loss (%)");
            _lossValue = new StringElement("Loss value ($/acre)");

            _resSec = new Section("Results")
            {
                _lpaLbs, _lpaBu, _percentLoss, _lossValue
            };

            Root.Add(new List<Section> { sec, _resSec });
        }

        Section _resSec;
        StringElement _lpaLbs;
        StringElement _lpaBu;
        StringElement _percentLoss;
        StringElement _lossValue;
        double _currCutWidth = -1;
        double _currSieveWidth = -1;
        double _currCollectingAreasqft = -1;
        double _currExpectedYield = -1;
        double _currPrice = -1;
        double _currSeedLossg = -1;
        Crop _currCrop;

        void RefreshResult()
        {
            if (_currCutWidth == -1 || _currSieveWidth == -1 || _currSeedLossg == -1 ||
                _currCollectingAreasqft == -1)
            {
                return;
            }
            var concenFactor = _currCutWidth / InchToFeet(_currSieveWidth);

            var collectingAreasi = SquareFeetToMeters(_currCollectingAreasqft);

            var lph = 10 * _currSeedLossg / concenFactor / collectingAreasi;
            var lpalbs = KgPHaToLbsPAcre(lph);

            Debug.WriteLine("Loss per ha: {0}", lph);
            Debug.WriteLine("Loss per acre lbs: {0}", lpalbs);

            _lpaLbs.Value = lpalbs.ToString("F");

            if (_currCrop != null)
            {
                var lpabu = lpalbs / _currCrop.LbsPBushel;
                Debug.WriteLine("Loss per acre bu: {0}", lpabu);
                _lpaBu.Value = lpabu.ToString("F");
                if (_currExpectedYield != -1)
                {
                    var percentLoss = lpabu / _currExpectedYield;
                    Debug.WriteLine("Percent loss: {0}", percentLoss);
                    _percentLoss.Value = percentLoss.ToString("P");
                }
                if (_currPrice != -1)
                {
                    var lossValue = _currPrice * lpabu;
                    Debug.WriteLine("Loss value: {0}", lossValue);
                    _lossValue.Value = lossValue.ToString("C");
                }
            }
            Root.Reload(_resSec, UITableViewRowAnimation.None);
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

        static public EntryElement CreateActionEntryElement(string caption, EventHandler act)
        {
            var ret = new EntryElement(caption, null, null);
            ret.TextAlignment = UITextAlignment.Right;
            ret.Changed += act;
            return ret;
        }

        static public double InchToFeet(double valueInInch)
        {
            return valueInInch / 12;
        }

        static public double SquareFeetToMeters(double valueInSF)
        {
            return valueInSF * 0.09290304;
        }

        static public double KgPHaToLbsPAcre(double valueInKgPHa)
        {
            return valueInKgPHa / 1.12;
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
