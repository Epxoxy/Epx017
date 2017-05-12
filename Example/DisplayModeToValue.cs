using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Example
{
    public class DisplayModeToValue : IValueConverter
    {
        public object Narrow { get; set; }
        public object Wide { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Epxoxy.Controls.SplitViewDisplayMode)
            {
                switch ((Epxoxy.Controls.SplitViewDisplayMode)value)
                {
                    case Epxoxy.Controls.SplitViewDisplayMode.Inline:
                    case Epxoxy.Controls.SplitViewDisplayMode.Overlay:
                        return Wide;
                    case Epxoxy.Controls.SplitViewDisplayMode.CompactInline:
                    case Epxoxy.Controls.SplitViewDisplayMode.CompactOverlay:
                        return Narrow;
                }
            }
            return Narrow;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
