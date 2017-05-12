using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Example
{
    public class BarOverflowPanel : Panel
    {


        public double WrapWidth
        {
            get { return (double)GetValue(WrapWidthProperty); }
            set { SetValue(WrapWidthProperty, value); }
        }
        public static readonly DependencyProperty WrapWidthProperty =
            DependencyProperty.Register("WrapWidth", typeof(double), typeof(BarOverflowPanel), 
                new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure),
                new ValidateValueCallback(IsWrapWidthValid));

        private static bool IsWrapWidthValid(object value)
        {
            double v = (double)value;
            return double.IsNaN(v) && !double.IsPositiveInfinity(v);
        }

        double _wrapWidth;
        protected override Size MeasureOverride(Size availableSize)
        {
            _wrapWidth = double.IsNaN(WrapWidth) ? availableSize.Width : WrapWidth;
            UIElementCollection children = InternalChildren;
            int childrenCount = children.Count;

            return base.MeasureOverride(availableSize);
        }
        
    }
}
