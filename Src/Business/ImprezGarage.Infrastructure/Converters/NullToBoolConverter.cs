//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                if (parameter?.ToString() == "Inverse")
                {
                    return true;
                }
                return false;
            }

            if (parameter?.ToString() == "Inverse")
            {
                return false;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}   //ImprezGarage.Infrastructure.Converters namespace 