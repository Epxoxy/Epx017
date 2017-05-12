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
    public class OverflowPanel : Panel
    {
        public static bool GetIsOverflow(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsOverflowProperty);
        }
        public static void SetIsOverflow(DependencyObject obj, bool value)
        {
            obj.SetValue(IsOverflowProperty, value);
        }
        public static readonly DependencyProperty IsOverflowProperty =
            DependencyProperty.RegisterAttached("IsOverflow", typeof(bool), typeof(OverflowPanel),
                new PropertyMetadata(false, OnIsOverflowChanged));

        private static void OnIsOverflowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Visual child = d as Visual;
            if(child != null)
            {
                OverflowPanel overflowPanel = VisualTreeHelper.GetParent(child) as OverflowPanel;
                if(overflowPanel != null)
                {

                }
            }
        }



        public ItemCollection OverflowItems
        {
            get { return (ItemCollection)GetValue(OverflowItemsProperty); }
            set { SetValue(OverflowItemsProperty, value); }
        }

        public static readonly DependencyProperty OverflowItemsProperty =
            DependencyProperty.Register("OverflowItems", typeof(ItemCollection), typeof(OverflowPanel), new PropertyMetadata(null));




        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            int count = (this.InternalChildren != null) ? InternalChildren.Count : 0;
            if(count > 0)
            {
                System.Collections.Generic.List<Int64> stableKeyValues;
                stableKeyValues = new List<Int64>(count);
                int i = 1;
                do
                {
                } while (++i < count);
            }
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size desiredSize = new Size();
            UIElementCollection children = InternalChildren;

            for (int i = 0, count = children.Count; i < count; ++i)
            {
                UIElement child = children[i];
                if (child != null)
                {
                    child.Measure(availableSize);
                    desiredSize.Width = Math.Max(desiredSize.Width, child.DesiredSize.Width);
                    desiredSize.Height = Math.Max(desiredSize.Height, child.DesiredSize.Height);
                }
            }
            return base.MeasureOverride(availableSize);
        }

    }
}
