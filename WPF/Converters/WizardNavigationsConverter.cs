using BookingApp.Aplication.UseCases;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace BookingApp.WPF.Converters
{
    public class WizardNavigationsConverter : MarkupExtension, IValueConverter
    {
        public WizardNavigationsConverter()
        { }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null && value != null)
            {
                var image = value as string;

                string ImageNameFirstNumber = image.Split(".")[0].Split("\\").Last();
                var parametar = parameter as string;

                if (int.TryParse(ImageNameFirstNumber, out var number))
                {
                    if (number == 0)
                    {
                        if(parametar == "back")
                        {
                            return Visibility.Hidden;
                        }
                        else if (parametar == "next")
                        {
                            return Visibility.Visible;
                        }
                        else if(parametar== "finish")
                        {
                            return Visibility.Hidden;
                        }
                    }
                    else if (number ==5)
                    {
                        if (parametar == "back")
                        {
                            return Visibility.Visible;
                        }
                        else if (parametar == "next")
                        {
                            return Visibility.Hidden;
                        }
                        else if (parametar == "finish")
                        {
                            return Visibility.Visible;
                        }
                    }
                    else
                    {
                        if (parametar == "back")
                        {
                            return Visibility.Visible;
                        }
                        else if (parametar == "next")
                        {
                            return Visibility.Visible;
                        }
                        else if (parametar == "finish")
                        {
                            return Visibility.Hidden;
                        }
                    }
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}