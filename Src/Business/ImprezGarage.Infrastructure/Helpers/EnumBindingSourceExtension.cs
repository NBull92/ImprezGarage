using System;
using System.Windows.Markup;

namespace ImprezGarage.Modules.Logger.Helpers
{
    /// <summary>
    /// This class is used to access the values of an enum and allow for us to then bind those values to a UI Element like a ComboBox
    /// </summary>
    public class EnumBindingSourceExtension : MarkupExtension
    {
        public Type EnumType { get; private set; }  

        public EnumBindingSourceExtension(Type enumType)
        {
            if(enumType is null || !enumType.IsEnum)
                throw new Exception("EnumType should be of Type Enum.");

            EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(EnumType);
        }
    }
}
