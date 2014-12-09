using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using MonoTouch.UIKit;
using System.Drawing;
using Harvest_Loss;
using Harvest_Loss_iOS;

//[assembly: ExportRenderer(typeof(DoneEntryCell), typeof(DoneEntryCellRenderer))]
//namespace Harvest_Loss_iOS
//{
//    public class DoneEntryCellRenderer : EntryCellRenderer
//    {
//        public override UITableViewCell GetCell(Cell item, UITableView tv)
//        {
//            var cell = base.GetCell(item, tv);
//            if (cell != null)
//            {
//                var textField = (UITextField)cell.ContentView.Subviews[0];
//                AddDoneButton(textField);
//            }
//            return cell;
//        }
//
//        /// <summary>
//        /// Add toolbar with Done button
//        /// </summary>
//        protected void AddDoneButton(UITextField uiTextField)
//        {
//            var toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, 50.0f, 44.0f));
//
//            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate
//            {
//                uiTextField.ResignFirstResponder();
//            });
//
//            toolbar.Items = new UIBarButtonItem[]
//            {
//                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
//                doneButton
//            };
//
//            uiTextField.InputAccessoryView = toolbar;
//        }
//    }
//}

