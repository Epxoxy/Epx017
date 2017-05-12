using System;
using System.Windows;

namespace Example
{
    public class AppBarButton : System.Windows.Controls.Button, ICommandBarElement
    {
        static AppBarButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AppBarButton), new FrameworkPropertyMetadata(typeof(AppBarButton)));
            IsCompactProperty = DependencyProperty.Register("IsCompact", typeof(bool), typeof(AppBarButton), new PropertyMetadata(false));
            LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(AppBarButton), new PropertyMetadata(string.Empty));
            IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(AppBarButton), new PropertyMetadata(null));
            CompactModeProperty = DependencyProperty.Register("CompactMode", typeof(CompactMode), typeof(AppBarButton), new PropertyMetadata(CompactMode.Label));
        }

        public bool IsCompact
        {
            get { return (bool)GetValue(IsCompactProperty); }
            set { SetValue(IsCompactProperty, value); }
        }
        public static readonly DependencyProperty IsCompactProperty;
        
        bool ICommandBarElement.IsCompact
        {
            get { return IsCompact; }
            set { IsCompact = value; }
        }

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        public static readonly DependencyProperty LabelProperty;
        
        public object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty;
        
        public CompactMode CompactMode
        {
            get { return (CompactMode)GetValue(CompactModeProperty); }
            set { SetValue(CompactModeProperty, value); }
        }

        public static readonly DependencyProperty CompactModeProperty;
        
    }

    public class AppBarToggleButton : System.Windows.Controls.Primitives.ToggleButton, ICommandBarElement
    {
        static AppBarToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AppBarToggleButton), new FrameworkPropertyMetadata(typeof(AppBarToggleButton)));
            IsCompactProperty = DependencyProperty.Register("IsCompact", typeof(bool), typeof(AppBarToggleButton), new PropertyMetadata(false));
            LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(AppBarToggleButton), new PropertyMetadata(string.Empty));
            IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(AppBarToggleButton), new PropertyMetadata(null));
            CompactModeProperty = DependencyProperty.Register("CompactMode", typeof(CompactMode), typeof(AppBarToggleButton), new PropertyMetadata(CompactMode.Label));
        }

        public bool IsCompact
        {
            get { return (bool)GetValue(IsCompactProperty); }
            set { SetValue(IsCompactProperty, value); }
        }
        public static readonly DependencyProperty IsCompactProperty;

        bool ICommandBarElement.IsCompact
        {
            get { return IsCompact; }
            set { IsCompact = value; }
        }

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        public static readonly DependencyProperty LabelProperty;

        public object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty;
        
        public CompactMode CompactMode
        {
            get { return (CompactMode)GetValue(CompactModeProperty); }
            set { SetValue(CompactModeProperty, value); }
        }
        public static readonly DependencyProperty CompactModeProperty;
        
    }

    public class AppBarContent: System.Windows.Controls.ContentControl, ICommandBarElement
    {
        static AppBarContent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AppBarContent), new FrameworkPropertyMetadata(typeof(AppBarContent)));
            IsCompactProperty = DependencyProperty.Register("IsCompact", typeof(bool), typeof(AppBarContent), new PropertyMetadata(false));
        }
        
        public bool IsCompact
        {
            get { return (bool)GetValue(IsCompactProperty); }
            set { SetValue(IsCompactProperty, value); }
        }

        public CompactMode CompactMode { get; set; }

        public static readonly DependencyProperty IsCompactProperty =
            DependencyProperty.Register("IsCompact", typeof(bool), typeof(AppBarContent), new PropertyMetadata(false));
        
    }

    public interface ICommandBarElement
    {
        bool IsCompact { get; set; }
        CompactMode CompactMode { get; set; }
    }

    public enum CompactMode
    {
        None,
        Icon,
        Label
    }

}
