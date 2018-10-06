//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------


namespace ImprezGarage.Modules.Logger.Converters
{
    using Helpers;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Data;

    public class EnumValueDescriptionConverter : IValueConverter
    {
        private readonly Dictionary<string, object> _enumValueDescriptionsCache = new Dictionary<string, object>();

        /// <summary>
        /// Convert an enum value into its enum value description.
        /// </summary>
        public object Convert(object enumValue, Type stringType, object parameter, CultureInfo culture)
        {
            var enumValueDescription = string.Empty;

            if (enumValue == null)
                return enumValueDescription;

            var enumType = enumValue.GetType();
            if (!enumType.IsEnum)
                return enumValueDescription;

            enumValueDescription = EnumAttributeHelpers.GetEnumValueDescription(enumValue, enumType);

            if (!_enumValueDescriptionsCache.ContainsKey(enumValueDescription))
                _enumValueDescriptionsCache.Add(enumValueDescription, enumValue);

            return enumValueDescription;
        }

        /// <summary>
        /// Convert back an enum value description into its enum value.
        /// </summary>
        public object ConvertBack(object enumValueDescription, Type enumType, object parameter, CultureInfo culture)
        {
            if (enumValueDescription == null)
                return null;

            var stringType = enumValueDescription.GetType();
            if (stringType != typeof(string))
                return null;

            object enumValue;

            var enumValueDescriptionString = (string)enumValueDescription;

            if (_enumValueDescriptionsCache.ContainsKey(enumValueDescriptionString))
            {
                enumValue = _enumValueDescriptionsCache[enumValueDescriptionString];
            }
            else
            {
                enumValue = EnumAttributeHelpers.GetEnumValueFromDescription(enumValueDescriptionString, enumType);

                if (enumValue != null)
                {
                    _enumValueDescriptionsCache.Add(enumValueDescriptionString, enumValue);
                }
            }

            return enumValue;
        }
    }
}   //ImprezGarage.Modules.Logger.Converters namespace 