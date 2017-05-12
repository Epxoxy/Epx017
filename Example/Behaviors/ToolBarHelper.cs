using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Example
{
    public static class ToolBarHelper
    {
        public static readonly DependencyPropertyKey IsInOverflowPanelKey =
            DependencyProperty.RegisterAttachedReadOnly("IsInOverflowPanel", typeof(bool), typeof(ToolBarHelper), new PropertyMetadata(false));

        public static readonly DependencyProperty IsInOverflowPanelProperty = IsInOverflowPanelKey.DependencyProperty;

        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetIsInOverflowPanel(UIElement target)
        {
            return (bool)target.GetValue(IsInOverflowPanelProperty);
        }

        public static readonly DependencyProperty TrackParentPanelProperty =
            DependencyProperty.RegisterAttached("TrackParentPanel", typeof(bool), typeof(ToolBarHelper),
                                                 new PropertyMetadata(false, OnTrackParentPanelPropertyChanged));

        public static void SetTrackParentPanel(DependencyObject d, bool value)
        {
            d.SetValue(TrackParentPanelProperty, value);
        }

        public static bool GetTrackParentPanel(DependencyObject d)
        {
            return (bool)d.GetValue(TrackParentPanelProperty);
        }

        private static void OnTrackParentPanelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as UIElement;
            if (element != null)
            {
                bool newValue = (bool)e.NewValue;
                if (newValue)
                {
                    element.LayoutUpdated += (s, arg) => OnControlLayoutUpdated(element);
                }
            }
        }
        private static void OnControlLayoutUpdated(UIElement element)
        {
            var isInOverflow = TreeHelper.FindParent<ToolBarOverflowPanel>(element) != null;
            element.SetValue(IsInOverflowPanelKey, isInOverflow);
        }
    }

    public static class TreeHelper
    {
        public static T FindParent<T>(this DependencyObject obj) where T : DependencyObject
        {
            return obj.GetAncestors().OfType<T>().FirstOrDefault();
        }

        public static IEnumerable<DependencyObject> GetAncestors(this DependencyObject element)
        {
            do
            {
                yield return element;
                element = VisualTreeHelper.GetParent(element);
            } while (element != null);
        }
    }
}
