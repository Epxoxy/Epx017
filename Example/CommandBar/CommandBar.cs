using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Example
{
    //[StyleTypedProperty(Property =nameof(ItemContainerStyle), StyleTargetType =typeof(ICommandBarElement))]
    public class CommandBar : ContentControl
    {
        #region ------------- Dependency Property -----------

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        public static readonly DependencyProperty IsOpenProperty;

        public bool IsSticky
        {
            get { return (bool)GetValue(IsStickyProperty); }
            set { SetValue(IsStickyProperty, value); }
        }
        public static readonly DependencyProperty IsStickyProperty;
        
        public CommandPlace Place
        {
            get { return (CommandPlace)GetValue(PlaceProperty); }
            internal set { SetValue(PlaceProperty, value); }
        }
        public static readonly DependencyProperty PlaceProperty;
        
        public BarClosedDisplayMode ClosedDisplayMode
        {
            get { return (BarClosedDisplayMode)GetValue(ClosedDisplayModeProperty); }
            set { SetValue(ClosedDisplayModeProperty, value); }
        }
        public static readonly DependencyProperty ClosedDisplayModeProperty;

        public CommandBarTemplateSettings TemplateSettings
        {
            get { return (CommandBarTemplateSettings)GetValue(TemplateSettingsProperty); }
            private set { SetValue(TemplateSettingsProperty, value); }
        }
        public static readonly DependencyProperty TemplateSettingsProperty;

        #endregion
        
        private ObservableCollection<ICommandBarElement> primaryCommands;
        public ObservableCollection<ICommandBarElement> PrimaryCommands
        {
            get { return primaryCommands; }
        }

        private ObservableCollection<ICommandBarElement> secondaryCommands;
        public ObservableCollection<ICommandBarElement> SecondaryCommands
        {
            get { return secondaryCommands; }
        }

        #region ------------- Property Changed Callback -------------

        //IsOpen property changed callback
        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommandBar commandbar = d as CommandBar;
            if(commandbar != null)
            {
                commandbar.UpdateOpenStatesAsync();
            }
        }

        //ClsoeDisplayMode property changed callback
        private static void OnCloseDisplayModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommandBar commandbar = d as CommandBar;
            if (commandbar != null && e.OldValue != e.NewValue)
            {
                commandbar.UpdateCloseDisplayMode((BarClosedDisplayMode)e.OldValue,(BarClosedDisplayMode)e.NewValue);
            }
        }

        private static void OnCompactLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommandBar bar = d as CommandBar;
            if(bar != null)
            {
            }
        }

        private static void OnMetricsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommandBar bar = d as CommandBar;
            if (bar != null)
            {

            }
        }

        private static void OnPlaceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommandBar bar = d as CommandBar;
            if (bar != null)
            {
                bar.UpdateClipRectangleGeometry();
            }
        }

        #endregion

        #region ------------- Constructor -------------

        static CommandBar()
        {
            IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(CommandBar),
                new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                FrameworkPropertyMetadataOptions.AffectsRender, OnIsOpenChanged));
            IsStickyProperty = DependencyProperty.Register("IsSticky", typeof(bool), typeof(CommandBar),
                new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
            ClosedDisplayModeProperty = DependencyProperty.Register("ClosedDisplayMode", typeof(BarClosedDisplayMode), typeof(CommandBar),
                new FrameworkPropertyMetadata(BarClosedDisplayMode.Compact,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                FrameworkPropertyMetadataOptions.AffectsRender, OnCloseDisplayModeChanged));
            TemplateSettingsProperty = DependencyProperty.Register("TemplateSettings", typeof(CommandBarTemplateSettings), 
                typeof(CommandBar), new PropertyMetadata(null));
            PlaceProperty = DependencyProperty.Register("Place", typeof(CommandPlace), typeof(CommandBar),
                new FrameworkPropertyMetadata(CommandPlace.Bottom,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnPlaceChanged));
            HeightProperty.OverrideMetadata(typeof(CommandBar), new FrameworkPropertyMetadata(40d,
                FrameworkPropertyMetadataOptions.AffectsRender));

            FocusManager.IsFocusScopeProperty.OverrideMetadata(typeof(CommandBar), new FrameworkPropertyMetadata(true));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(CommandBar), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));

            DefaultStyleKeyProperty.OverrideMetadata(typeof(CommandBar), new FrameworkPropertyMetadata(typeof(CommandBar)));
        }

        public CommandBar()
        {
            primaryCommands = new ObservableCollection<ICommandBarElement>();
            secondaryCommands = new ObservableCollection<ICommandBarElement>();
            primaryCommands.CollectionChanged += OnPrimaryCommandsCollectionChanged;
            secondaryCommands.CollectionChanged += OnSecondaryCommandsCollectionChanged;
            this.TemplateSettings = new CommandBarTemplateSettings(this);
            this.Unloaded += OnUnloaded;
        }

        #endregion

        #region ------------- Event handler -------------

        /// <summary>
        /// UnSub event handler on unloaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= OnUnloaded;
            primaryCommands.CollectionChanged -= OnPrimaryCommandsCollectionChanged;
            secondaryCommands.CollectionChanged -= OnSecondaryCommandsCollectionChanged;
        }

        private void OnPrimaryCommandsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            bool isClose = !IsOpen;
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (ICommandBarElement item in e.NewItems)
                {
                    item.IsCompact = isClose;
                }
            }
        }

        private void OnSecondaryCommandsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (ICommandBarElement item in e.NewItems)
                {
                    item.CompactMode = CompactMode.Icon;
                    item.IsCompact = true;
                }
            }
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (IsSticky && IsOpen)
            {
                if ((e.OriginalSource as ICommandBarElement) != null) IsOpen = false;
            }
            System.Diagnostics.Debug.WriteLine("OnPreviewMouseLeftButtonUp " + e.OriginalSource);
            base.OnPreviewMouseLeftButtonUp(e);
        }

        private void OnMouseDownOutsideElement(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("OnMouseDownOutsideElement " + sender);
            Mouse.RemovePreviewMouseUpOutsideCapturedElementHandler(this, OnMouseDownOutsideElement);
            ReleaseMouseCapture();
            if (IsOpen) IsOpen = false;
        }

        private void OnContentRootMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ClosedDisplayMode == BarClosedDisplayMode.Minimal && IsOpen == false)
                IsOpen = true;
        }

        internal virtual void OnOpening()
        {
        }

        internal virtual void OnClosing()
        {
        }

        #endregion

        #region ------------- Update Method -------------

        private void UpdateCloseDisplayMode(BarClosedDisplayMode oldMode, BarClosedDisplayMode newMode)
        {
            System.Diagnostics.Debug.WriteLine("UpdateCloseDisplayMode");
            if (oldMode == BarClosedDisplayMode.Minimal && contentRoot != null)
            {
                contentRoot.MouseDown -= OnContentRootMouseDown;
            }
            switch (newMode)
            {
                case BarClosedDisplayMode.Compact:
                    this.Height = this.TemplateSettings.CompactLength;
                    break;
                case BarClosedDisplayMode.Minimal:
                    this.Height = this.TemplateSettings.MinimalLength;
                    if(contentRoot != null)
                    {
                        contentRoot.MouseDown -= OnContentRootMouseDown;
                        contentRoot.MouseDown += OnContentRootMouseDown;
                    }
                    break;
                case BarClosedDisplayMode.Hidden:
                    this.Height = 0d;
                    break;
            }
            this.GotoCloseState();
        }

        /// <summary>
        /// Update on IsOpen changed
        /// </summary>
        private void UpdateOpenStatesAsync()
        {
            bool targetState = !IsOpen;
            var frdCommands = PrimaryCommands;
            var secCommands = SecondaryCommands;
            for (int i = 0; i < frdCommands.Count; ++i)
            {
                frdCommands[i].IsCompact = targetState;
            }
            //Update state
            if (IsOpen)
            {
                if (IsSticky == false)
                {
                    Mouse.Capture(this, CaptureMode.SubTree);
                    Mouse.AddPreviewMouseUpOutsideCapturedElementHandler(this, OnMouseDownOutsideElement);
                }
                VisualStateManager.GoToState(this, States.OPEN, false);
                Keyboard.Focus(this);
            }
            else
            {
                GotoCloseState();
                Keyboard.ClearFocus();
            }
        }

        /// <summary>
        /// Goto special close state
        /// </summary>
        private void GotoCloseState()
        {
            if (IsOpen) return;
            string targetStateName = string.Empty;
            switch (ClosedDisplayMode)
            {
                case BarClosedDisplayMode.Compact:
                    targetStateName = States.COMPACT;
                    break;
                case BarClosedDisplayMode.Minimal:
                    targetStateName = States.MINIMAL;
                    break;
                case BarClosedDisplayMode.Hidden:
                    targetStateName = States.HIDDEN;
                    break;
                default: return;
            }
            System.Diagnostics.Debug.WriteLine("GotoCloseState " + targetStateName);
            VisualStateManager.GoToState(this, targetStateName, false);
        }

        #endregion

        #region ------------- Override -------------

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            clipRectangleGeometry = GetTemplateChild("PART_ClipRectangle") as RectangleGeometry;
            contentRoot = GetTemplateChild("ContentRoot") as UIElement;
            System.Windows.Data.Binding binding = null;
            var panel = this.Parent as Panel;
            if (panel != null)
            {
                binding = new System.Windows.Data.Binding()
                {
                    Source = panel,
                    Path = new PropertyPath("Children.Count"),
                    Mode = System.Windows.Data.BindingMode.OneWay
                };
            }
            else
            {
                var itemscontrol = this.Parent as ItemsControl;
                if (itemscontrol != null)
                {
                    binding = new System.Windows.Data.Binding()
                    {
                        Source = itemscontrol,
                        Path = new PropertyPath("Items.Count"),
                        Mode = System.Windows.Data.BindingMode.OneWay
                    };
                }
            }
            if (binding != null)
                this.SetBinding(Panel.ZIndexProperty, binding);
            this.UpdateClipRectangleGeometry();
            if (Place == CommandPlace.Bottom)
            {
                this.GotoCloseState();
            }
            else
            {
                this.UpdateClipTopTranform();
            }
        }

        private void UpdateClipTopTranform()
        {
            if (clipRectangleGeometry == null) return;
            var transform = clipRectangleGeometry.Transform as TranslateTransform;
            if (transform == null) return;
            switch (ClosedDisplayMode)
            {
                case BarClosedDisplayMode.Compact:
                    transform.Y = TemplateSettings.CompactLength;
                    break;
                case BarClosedDisplayMode.Minimal:
                    transform.Y = TemplateSettings.MinimalLength;
                    break;
            }
        }
        
        private void UpdateClipRectangleGeometry()
        {
            if (clipRectangleGeometry != null)
            {
                double openHeight = TemplateSettings.OpenLength;
                double targetY = Place == CommandPlace.Top ? -openHeight : -TemplateSettings.DeviationToOpenLength;
                this.clipRectangleGeometry.Rect = new Rect(0, targetY, (double)this.ActualWidth, openHeight);
                System.Diagnostics.Debug.WriteLine("OnRenderSizeChanged of " + targetY + ","+ openHeight );
            }
        }

        //Override method so that to clip overflow size
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            TemplateSettings.Update();
            UpdateClipRectangleGeometry();
        }
        #endregion

        //Visual states name collection
        class States
        {
            public const string OPEN = "Open";
            public const string COMPACT = "Compact";
            public const string HIDDEN = "Hidden";
            public const string MINIMAL = "Minimal";
        }

        #region ------------- Members -------------

        //Private members
        private RectangleGeometry clipRectangleGeometry;
        //No in use members
        private UIElement contentRoot;

        #endregion
    }

    public enum CommandPlace
    {
        Top,
        Bottom
    }
}
