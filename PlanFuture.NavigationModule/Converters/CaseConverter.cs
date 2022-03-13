using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PlanFuture.Modules.NavigationModule.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public partial class CaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                culture = culture ?? CultureInfo.CurrentCulture;

                switch (this.TargetCasing)
                {
                    case CharacterCasing.Lower:
                        return str.ToLower(culture);
                    case CharacterCasing.Upper:
                        return str.ToUpper(culture);
                    case CharacterCasing.Normal:
                        return str;
                }
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                culture = culture ?? CultureInfo.CurrentCulture;

                switch (this.SourceCasing)
                {
                    case CharacterCasing.Lower:
                        return str.ToLower(culture);
                    case CharacterCasing.Upper:
                        return str.ToUpper(culture);
                    case CharacterCasing.Normal:
                        return str;
                }
            }

            return DependencyProperty.UnsetValue;
        }
    }
}
