using Novea2._0.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Novea2._0.ViewModel.Customer
{
    public class StoreIdToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string storeId = (string)value;
            var store = DataProvider.Ins.DB.CUAHANGs.FirstOrDefault(s => s.MACH == storeId);
            if (store != null)
            {
                return store.TENCH;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
