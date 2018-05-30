//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Converters
{
    using ImprezGarage.Infrastructure.Model;
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class VehicleTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch(((VehicleType)value).Id)
            {
                case 1:
                    return (parameter?.ToString() == "Car") ? Visibility.Visible : Visibility.Collapsed;
                case 2:
                    return (parameter?.ToString() == "Motorbike") ? Visibility.Visible : Visibility.Collapsed;
                case 3:
                    return (parameter?.ToString() == "Bicycle") ? Visibility.Visible : Visibility.Collapsed;
                default:
                    return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}   //ImprezGarage.Infrastructure.Converters namespace 