using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public bool IsMaximized
        {
            get { return (bool)GetValue(IsMaximizedProperty); }
            set { SetValue(IsMaximizedProperty, value); }
        }
        public object TitleBar
        {
            get { return (object)GetValue(TitleBarProperty); }
            set { SetValue(TitleBarProperty, value); }
        }

        public static readonly DependencyProperty TitleBarHeightProperty =
            DependencyProperty.Register("TitleBarHeight", typeof(double), typeof(MainWindow), new PropertyMetadata(30d));
        public static readonly DependencyProperty IgnoreTaskbarOnMaximizeProperty =
          DependencyProperty.Register("IgnoreTaskbarOnMaximize", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));
        public static readonly DependencyProperty IsMaximizedProperty =
            DependencyProperty.Register("IsMaximized", typeof(bool), typeof(MainWindow), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty TitleBarProperty =
            DependencyProperty.Register("TitleBar", typeof(object), typeof(MainWindow), new PropertyMetadata(null));

        [DllImport("User32.dll", CharSet= CharSet.Auto)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public MainWindow()
        {
            Debug.WriteLine("Start : "+getCurrentTime());
            App.LoadWindowSetting();
            InitializeComponent();
            Epxoxy.Controls.WindowExt.SetWindowBackground(this);
            RenderOptions.ProcessRenderMode = RenderMode.Default;
            int tier = RenderCapability.Tier >> 16;
            Title = "EPX " + tier;
            Epxoxy.Controls.WindowHelper.SetShowIcon(this, false);
            Loaded += MainWindowLoaded;
        }

        private async void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= MainWindowLoaded;
            EventHandler activatedHandler = null;
            EventHandler deactivatedHandler = null;
            activatedHandler = (obj, args) =>
            {
                //TitleTB.IsEnabled = true;
            };
            deactivatedHandler = (obj, args) =>
            {
                //TitleTB.IsEnabled = false;
            };
            RoutedEventHandler unloadedHandler = null;
            unloadedHandler = (obj, args) =>
            {
                this.Unloaded -= unloadedHandler;
                Application.Current.Deactivated -= activatedHandler;
                Application.Current.Activated -= deactivatedHandler;
            };
            Application.Current.Deactivated += deactivatedHandler;
            Application.Current.Activated += activatedHandler;
            this.Unloaded += unloadedHandler;
            
            SplashView view = new SplashView();
            Debug.WriteLine("Navigate StartView : " + getCurrentTime());
            using (var overlay = Epxoxy.Controls.OverlayAdorner<UIElement>.Overlay(rootgrid, view))
            {
                view.StartStoryBoard();
                Debug.WriteLine("Create ExamplePage Instance complete : " + getCurrentTime());
                await Task.Delay(500);
                ExamplePage epage = new ExamplePage();
                await Task.Delay(500);
                mainFrame.Navigate(epage);
                Debug.WriteLine("Navigate ExamplePage : " + getCurrentTime());
            }
        }

        #region HostVisual test

        private HostVisual createElementOnWorkerThread()
        {
            HostVisual hostVisual = new HostVisual();
            Thread thread = new Thread(new ParameterizedThreadStart(elementWorkerThread));
            thread.ApartmentState = ApartmentState.STA;
            thread.IsBackground = true;
            thread.Start(hostVisual);
            s_event.WaitOne();

            return hostVisual;
        }

        private void elementWorkerThread(object arg)
        {
            HostVisual hostVisual = (HostVisual)arg;
            VisualTargetPresentationSource visualTargetps = new VisualTargetPresentationSource(hostVisual);

            s_event.Set();

            visualTargetps.RootVisual = createElement();

            System.Windows.Threading.Dispatcher.Run();
        }

        private FrameworkElement createElement()
        {
            return new ExamplePage();
        }

        #endregion

        private void MainFrameNavigating(object sender, NavigatingCancelEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(getCurrentTime());
            if(e.NavigationMode == NavigationMode.Back) { e.Cancel = true; }
        }
        
        private long getCurrentTime()
        {
            return (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
        }

        private static AutoResetEvent s_event = new AutoResetEvent(false);

        private bool isStart;
        private double progress = 0d;
        private async void ProgressState_Click(object sender, EventArgs e)
        {
            if (!isStart)
            {
                isStart = true;
                this.Dispatcher.Invoke(() =>
                {
                    TaskBar.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Normal;
                });
                while (isStart)
                {
                    await Task.Delay(100);
                    progress += 0.01;
                    this.Dispatcher.Invoke(() =>
                    {
                        if (progress >= 1)
                        {
                            progress = 0d;
                        }
                        TaskBar.ProgressValue = progress;
                    });
                }
            }
            else isStart = false;
        }
    }
}
