using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Globalization;
using System.Text;
using System.IO;

namespace Harvest_Loss
{
    public class Helpers
    {
        static public IEnumerable<string> ReadLines(Stream stream, Encoding encoding)
        {
            using (var reader = new StreamReader(stream, encoding))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }

    public class EnumExt<T>
    {
        static EnumExt()
        {
            _toStr = new Dictionary<T, string>();

            foreach (var v in Enum.GetValues(typeof(T)))
            {
                _toStr.Add((T)v, Enum.GetName(typeof(T), v));
            }
        }

        protected static Dictionary<T, string> _toStr;

        public string GetDescription(T curr)
        {
            return _toStr[curr];
        }

        public IList<string> DescList()
        {
            return _toStr.Values.ToList();
        }

        public T FromDesc(string desc)
        {
            var ret = _toStr.First(x => string.CompareOrdinal(x.Value, desc) == 0);
            return ret.Key;
        }
    }

    public class EnumToPickerIdxCov<T, TEx> : IValueConverter where TEx : EnumExt<T>, new()
    {
        static EnumToPickerIdxCov()
        {
            _extObj = new TEx();
            Names = new List<string>(_extObj.DescList());
        }

        static EnumExt<T> _extObj;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var desc = _extObj.GetDescription((T)value);
            var idx = Names.IndexOf(desc);
            return idx;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var idx = (int)value;
            if (idx < 0 || idx >= Names.Count)
            {
                return null;
            }
            var name = Names[idx];

            var ret = _extObj.FromDesc(name);
            return ret;
        }

        public static readonly List<string> Names;
    }

    public class MethodExt : EnumExt<CalcVM.Method>
    {
        static MethodExt()
        {
            _toStr = new Dictionary<CalcVM.Method, string>();
            _toStr.Add(CalcVM.Method.Weight, "Weight");
            _toStr.Add(CalcVM.Method.Volume, "Volume");
            _toStr.Add(CalcVM.Method.Count, "Count");
        }
    }

    public class MethodToPickerIdxCov : EnumToPickerIdxCov<CalcVM.Method, MethodExt>
    {

    }


}

