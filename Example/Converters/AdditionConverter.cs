using System;
using System.Globalization;
using System.Windows.Data;

namespace Example
{
    [ValueConversion(typeof(double), typeof(double))]
    public class AdditionConverter : IValueConverter
    {
        private Type type;
        public Type Type
        {
            get { return type; }
            set
            {
                type = value;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0;
            if (parameter == null) return value;
            var source = (double)value;
            var param = (double)parameter;
            return source + param;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0;
            if (parameter == null) return value;
            var source = (double)value;
            var param = (double)parameter;
            return source - param;
        }
    }

}
