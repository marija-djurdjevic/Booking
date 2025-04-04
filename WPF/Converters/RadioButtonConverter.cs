﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace BookingApp.WPF.Converters
{
    public class RadioButtonConverter : MarkupExtension, IValueConverter
    {
        [ConstructorArgument("value")]
        public int Value { get; set; }

        public RadioButtonConverter(int value)
        {
            Value = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Value == (int)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Value : Binding.DoNothing;
        }
    }
}