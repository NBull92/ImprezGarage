//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Logger.Helpers
{
    using System;
    using System.Collections;

    /// <summary>
    /// Use this class in an ObjectDataProvider, to provide a list of enum value names for an enum type, 
    /// modified by the attributes of the enum type values, above their enum value declarations, giving them user facing properties.
    /// </summary>
    public class EnumAttributeGetValues
    {
        public static Array GetValues(Type enumType)
        {
            var attributeEnumValues = new ArrayList();

            if (!enumType.IsEnum)
                return attributeEnumValues.ToArray();

            foreach (object enumTypeValue in Enum.GetValues(enumType))
            {
                var enumTypeValueName = enumTypeValue.ToString();

                // Hide an enum value from the user, if its browsable attribute has been specified as false, 
                // using [Browsable(false)] above its enum value declaration.
                var enumTypeValueIsBrowsable = EnumAttributeHelpers.GetIsEnumValueBrowsable(enumTypeValue, enumType);
                if (!enumTypeValueIsBrowsable)
                {
                    continue;
                }

                // Give an enum value a user facing pretty name, if its description attribute has been specified, 
                // using [Description("User Facing Pretty Name")] above its enum value declaration.
                var enumTypeValueDescription = EnumAttributeHelpers.GetEnumValueDescription(enumTypeValue, enumType);
                if (!string.IsNullOrWhiteSpace(enumTypeValueDescription))
                {
                    attributeEnumValues.Add(enumTypeValueDescription);
                }
                else
                {
                    attributeEnumValues.Add(enumTypeValueName);
                }
            }

            return attributeEnumValues.ToArray();
        }
    }
}   //ImprezGarage.Modules.Logger.Helpers namespace 