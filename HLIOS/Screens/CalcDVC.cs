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

            var slEle = CreateActionEntryElement("Seed loss (g)", (sender, arg) =>
            {
                if (!double.TryParse(((EntryElement)sender).Value, out _currSeedLossInput))
                {
                    _currSeedLossInput = -1;
                    ((EntryElement)sender).Value = null;
                }
                RefreshResult();
            });

            var selectMethodEle = new RootElement("Method", new RadioGroup("m", 0))
            {
                new Section()
                {
                    new MyRadioElement(MethodChoice.Weight.ToString(), (sender, arg) =>
                    {
                        NavigationController.PopViewControllerAnimated(true);
                        slEle.Caption = "Seed loss (g)";
                        _currChoice = MethodChoice.Weight;
                        RefreshResult();
                    }),
                    new MyRadioElement(MethodChoice.Volume.ToString(), (sender, arg) =>
                    {
                        NavigationController.PopViewControllerAnimated(true);
                        slEle.Caption = "Seed loss (ml)";
                        _currChoice = MethodChoice.Volume;
                        RefreshResult();
                    }),
                    new MyRadioElement(MethodChoice.Count.ToString(), (sender, arg) =>
                    {
                        NavigationController.PopViewControllerAnimated(true);
                        slEle.Caption = "Seed loss (number)";
                        _currChoice = MethodChoice.Count;
                        RefreshResult();
                    }),
                }
            };

            var sec = new Section("Input")
            {
                selectMethodEle,
                CreateRadioRootEle("Crop", "c",
                                   () => HLDatabase.GetTable<Crop>(),
                                   selected =>
                {
                    _currCrop = selected;
                    RefreshResult();
                }),

                CreateActionEntryElement("Cut width (ft)", (sender, arg) =>
                {
                    if (!double.TryParse(((EntryElement)sender).Value, out _currCutWidth))
                    {
                        _currCutWidth = -1;
                        ((EntryElement)sender).Value = null;
                    }
                    RefreshResult();
                }),
                CreateActionEntryElement("Sieve width (in)", (sender, arg) =>
                {
                    if (!double.TryParse(((EntryElement)sender).Value, out _currSieveWidth))
                    {
                        _currSieveWidth = -1;
                        ((EntryElement)sender).Value = null;
                    }
                    RefreshResult();
                }),
                CreateActionEntryElement("Collecting area (sq ft)", (sender, arg) =>
                {
                    if (!double.TryParse(((EntryElement)sender).Value, out _currCollectingAreasqft))
                    {
                        _currCollectingAreasqft = -1;
                        ((EntryElement)sender).Value = null;
                    }
                    RefreshResult();
                }),
                CreateActionEntryElement("Expected yield (bu/acre)", (sender, arg) =>
                {
                    if (!double.TryParse(((EntryElement)sender).Value, out _currExpectedYield))
                    {
                        _currExpectedYield = -1;
                        ((EntryElement)sender).Value = null;
                    }
                    RefreshResult();
                }),
                CreateActionEntryElement("Price ($/bu)", (sender, arg) =>
                {
                    if (!double.TryParse(((EntryElement)sender).Value, out _currPrice))
                    {
                        _currPrice = -1;
                        ((EntryElement)sender).Value = null;
                    }
                    RefreshResult();
                }),
                slEle,
            };

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

        enum MethodChoice
        {
            Weight,
            Volume,
            Count
        }

        MethodChoice _currChoice = MethodChoice.Weight;
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
        double _currSeedLossInput = -1;
        Crop _currCrop;

        void RefreshResult()
        {
            // Clear all values first.
            _lpaLbs.Value = null;
            _lpaBu.Value = null;
            _percentLoss.Value = null;
            _lossValue.Value = null;
            Root.Reload(_resSec, UITableViewRowAnimation.None);

            if (_currCutWidth == -1 || _currSieveWidth == -1 || _currSeedLossInput == -1 ||
                _currCollectingAreasqft == -1 || _currCrop == null)
            {
                return;
            }
            var _currSeedLossInG = _currSeedLossInput;
            if (_currChoice == MethodChoice.Count)
            {
                _currSeedLossInG = _currSeedLossInput * _currCrop.KernelWeight / 1000;
            }
            else if (_currChoice == MethodChoice.Volume)
            {
                _currSeedLossInG = Helper.MlToG(_currSeedLossInput, _currCrop.KgPBushel);
            }


            var concenFactor = _currCutWidth / Helper.InchToFeet(_currSieveWidth);

            var collectingAreasi = Helper.SquareFeetToMeters(_currCollectingAreasqft);

            var lph = 10 * _currSeedLossInG / concenFactor / collectingAreasi;
            var lpalbs = Helper.KgPHaToLbsPAcre(lph);

            Debug.WriteLine("Loss per ha: {0}", lph);
            Debug.WriteLine("Loss per acre lbs: {0}", lpalbs);

            _lpaLbs.Value = lpalbs.ToString("F");


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
            ret.KeyboardType = UIKeyboardType.NumbersAndPunctuation;
            ret.Changed += act;
            return ret;
        }
    }

    public class MyRadioElement : RadioElement
    {
        public MyRadioElement(string caption):base(caption)
        {
        }

        public MyRadioElement(string caption, EventHandler<EventArgs> onSelectedAct):base(caption)
        {
            OnSelected += onSelectedAct;
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
