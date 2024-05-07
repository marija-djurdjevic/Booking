using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp
{
    public class GlobalVariables : DependencyObject
    {
        public static readonly DependencyProperty ShowTooltipsProperty =
            DependencyProperty.Register("ShowTooltips", typeof(bool), typeof(GlobalVariables), new PropertyMetadata(true));
        public GlobalVariables(bool value)
        {
            ShowTooltips = value;
        }

        public GlobalVariables()
        { }
        public bool ShowTooltips
        {
            get { return (bool)GetValue(ShowTooltipsProperty); }
            set { SetValue(ShowTooltipsProperty, value); }
        }
    }
}
