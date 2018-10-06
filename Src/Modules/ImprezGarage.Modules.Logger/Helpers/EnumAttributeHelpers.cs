//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Logger.Helpers
{
    using System;
    using System.ComponentModel;

    public static class EnumAttributeHelpers
    {
        /// <summary>
        /// Determine if an enum value is browsable, allowing its visibility to the user to be specified, 
        /// using [Browsable(true/false)] above its enum value declaration. Defaults to true and is visible to the user, 
        /// if the browsable attribute has not been specified.
        /// </summary>
        public static bool GetIsEnumValueBrowsable(object enumValue, Type enumType)
        {
            if (!enumType.IsEnum)
                return true;

            var enumValueName = enumValue.ToString();
            var enumValueFieldInfo = enumType.GetField(enumValueName);
            if (enumValueFieldInfo == null)
                return true;

            var isEnumValueBrowsable = true;

            var enumValueBrowsables = enumValueFieldInfo.GetCustomAttributes(typeof(BrowsableAttribute), true) as BrowsableAttribute[];
            if (enumValueBrowsables.Length > 0)
            {
                isEnumValueBrowsable = enumValueBrowsables[0].Browsable;
            }

            return isEnumValueBrowsable;
        }

        /// <summary>
        /// Get the description of an enum value, which should be a user facing pretty name, 
        /// using [Description("User Facing Pretty Name")] above its enum value declaration. If the description attribute has not been specified, 
        /// it figures out a user facing pretty name from the enum value name.
        /// </summary>
        public static string GetEnumValueDescription(object enumValue, Type enumType)
        {
            var enumValueDescription = string.Empty;

            if (!enumType.IsEnum)
                return enumValueDescription;

            var enumValueName = enumValue.ToString();
            var enumValueFieldInfo = enumType.GetField(enumValueName);
            if (enumValueFieldInfo == null)
                return enumValueDescription;

            if (enumValueFieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true) is DescriptionAttribute[] enumValueDescriptions && enumValueDescriptions.Length > 0)
            {
                enumValueDescription = enumValueDescriptions[0].Description;
            }
            else
            {
                enumValueDescription = Variable.NameToLabel(enumValueName);
            }

            return enumValueDescription;
        }

        /// <summary>
        /// Get an enum value from a description of it, which should be a user facing pretty name, 
        /// using [Description("User Facing Pretty Name")] above its enum value declaration. If the description attribute has not been specified, 
        /// it figures out a user facing pretty name from the enum value name.
        /// </summary>
        public static object GetEnumValueFromDescription(string enumValueDescription, Type enumType)
        {
            object enumValue = null;

            if (!enumType.IsEnum)
                return enumValue;

            foreach (object enumTypeValue in Enum.GetValues(enumType))
            {
                string enumTypeValueDescription = GetEnumValueDescription(enumTypeValue, enumType);

                if (enumTypeValueDescription == enumValueDescription)
                {
                    enumValue = enumTypeValue;

                    break;
                }
            }

            return enumValue;
        }
    }
}   //ImprezGarage.Modules.Logger.Helpers namespace 