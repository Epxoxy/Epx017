using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Example
{
    public enum AdaptType
    {
        Height,
        Width,
        Both
    }
    public class AdaptPanel : Panel
    {
        public AdaptType AdaptType
        {
            get { return (AdaptType)GetValue(AdaptTypeProperty); }
            set { SetValue(AdaptTypeProperty, value); }
        }
        public static readonly DependencyProperty AdaptTypeProperty =
            DependencyProperty.Register("AdaptType", typeof(AdaptType), typeof(AdaptPanel), new PropertyMetadata(AdaptType.Height));
        
        protected override Size MeasureOverride(Size availableSize)
        {
            Size childConstraint = new Size(Double.PositiveInfinity, Double.PositiveInfinity);

            foreach (UIElement child in InternalChildren)
            {
                if (child == null) { continue; }
                child.Measure(childConstraint);
            }
            return new Size();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (double.IsInfinity(finalSize.Height) || double.IsInfinity(finalSize.Width))
            {
                throw new InvalidOperationException("Infinity height or width.");
            }
            if (Children.Count > 0)
            {
                for (int cindex = 0; cindex < InternalChildren.Count; cindex++)
                {
                    if (InternalChildren[cindex] == null) { continue; }
                    var rect = new Rect(0, 0, finalSize.Width, InternalChildren[cindex].DesiredSize.Height);
                    InternalChildren[cindex].Arrange(rect);
                }
            }
            return finalSize;
        }

        /// <summary>
        /// Override of <seealso cref="UIElement.GetLayoutClip"/>.
        /// </summary>
        /// <returns>Geometry to use as additional clip if LayoutConstrained=true</returns>
        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            //Canvas only clips to bounds if ClipToBounds is set, 
            //  no automatic clipping
            if (ClipToBounds)
                return new RectangleGeometry(new Rect(RenderSize));
            else
                return null;
        }

        private void Generat(Size finalSize)
        {
            double desiredHeight = 0;
            double desiredWidth = 0;
            switch (AdaptType)
            {
                case AdaptType.Height:
                    desiredHeight = finalSize.Height;
                    break;
                case AdaptType.Width:
                    desiredWidth = finalSize.Width;
                    break;
                default:
                    desiredHeight = finalSize.Height;
                    desiredWidth = finalSize.Width;
                    break;
            }
        }
    }
}
