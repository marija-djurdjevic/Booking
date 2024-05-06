using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Controls;

namespace BookingApp.WPF.Converters
{

    public class EmptySelectionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.All(v => v != null && !string.IsNullOrWhiteSpace(v.ToString())))
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "Morate izabrati jezik.");
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
