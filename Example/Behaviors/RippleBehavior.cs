using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Example
{
    public class RippleBehavior : Behavior<Button>
    {

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Click += OnAssociatedObjectMouseDown;
        }

        private void OnAssociatedObjectMouseDown(object sender, RoutedEventArgs e)
        {
            var max = Math.Max(this.AssociatedObject.ActualHeight, this.AssociatedObject.ActualWidth);
            Ellipse ellipse = new Ellipse()
            {
                Fill = Brushes.White,
                Opacity = 0.2,
                Height = max,
                Width = max,
                RenderTransformOrigin = new Point(0.5, 0.5)
            };
            Rectangle rect = new Rectangle()
            {
                Height = this.AssociatedObject.ActualHeight,
                Width = this.AssociatedObject.ActualWidth,
                IsHitTestVisible = false
            };
            rect.Fill = new VisualBrush()
            {
                Visual = ellipse,
                Stretch = Stretch.None
            };
            ellipse.RenderTransform = new ScaleTransform() { ScaleX = 0, ScaleY = 0};
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.Click -= OnAssociatedObjectMouseDown;
        }
    }
}
