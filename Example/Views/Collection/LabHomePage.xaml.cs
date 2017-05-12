using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Example
{
    /// <summary>
    /// Interaction logic for LabHomePage.xaml
    /// </summary>
    public partial class LabHomePage : Page
    {
        private List<string> TestListBox { get; set; }
        public LabHomePage()
        {
            InitializeComponent();
            this.Loaded += LabHomePageLoaded;
        }

        private async void LabHomePageLoaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= LabHomePageLoaded;
            //Build Test string with big number.
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < 100; ++i)
            {
                stringBuilder.Append(Properties.Resources.testString + "\n");
            }
            tb.Text = stringBuilder.ToString();
            var brushes = from property in typeof(Brushes).GetProperties()
                          let value = property.GetValue(null)
                          select value;
            panelTesting.ItemsSource = brushes.Take(100).ToArray();

            //Nested scroll init
            translate = header.RenderTransform as TranslateTransform;
            if (translate == null)
                header.RenderTransform = translate = new TranslateTransform();
            innerTranslate = innerheader.RenderTransform as TranslateTransform;
            if(innerTranslate == null)
                innerheader.RenderTransform = innerTranslate = new TranslateTransform();
            content.ScrollChanged += OnScrollChanged;

            //Load Listbox items
            await Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);
                TestListBox = new List<string>();
                for (int i = 0; i < 100; i++)
                {
                    TestListBox.Add("Item " + i.ToString());
                }
            });
            //Init ItemsSource
            listbox.ItemsSource = TestListBox;
            list.ItemsSource = TestListBox;
            ItemsControl.ItemsSource = Enumerable.Range(0, 53);
        }

        private TranslateTransform translate;
        private TranslateTransform innerTranslate;
        private double factor = 0.5;
        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalOffset >= 150)
            {
                translate.Y = -150;
                innerTranslate.Y = 75;
            }
            else
            {
                translate.Y = -e.VerticalOffset;
                innerTranslate.Y = e.VerticalOffset * factor;
            }
        }

        private string LoadTemplate(Control ctl)
        {
            var builder = new StringBuilder();
            using (var writer = new System.IO.StringWriter(builder))
                System.Windows.Markup.XamlWriter.Save(ctl.Template, writer);
            return builder.ToString();
        }

        #region Listbox

        private void JumpItemClick(object sender, RoutedEventArgs e)
        {
            string text = toItemBox.Text;
            if (string.IsNullOrWhiteSpace(text)) return;
            int index;
            if (int.TryParse(text, out index) && index < list.Items.Count)
            {
                list.ScrollIntoView(list.Items[index]);
            }
        }

        #endregion

        #region Animation Test

        #region Animation

        private Random random = new Random();

        private void start()
        {
            double height = cvs.ActualHeight, width = cvs.ActualWidth;
            double time = Math.Sqrt(2 * height / 9.8);
            Ellipse e = new Ellipse();
            Panel.SetZIndex(e, 3);
            cvs.Children.Add(e);

            var leftani = new DoubleAnimation(0, width, TimeSpan.FromSeconds(time));
            Timeline.SetDesiredFrameRate(leftani, (int)(60));
            EventHandler completedHandler = null;
            completedHandler = (sender, args) =>
            {
                leftani.Completed -= completedHandler;
                if (cvs.Children.Count > 0) cvs.Children.RemoveAt(0);
            };
            leftani.Completed += completedHandler;

            random = new Random((int)DateTime.Now.Ticks);
            e.Height = e.Width = random.Next(3, 20);

            e.Fill = new SolidColorBrush(new Color()
            {
                A = (byte)random.Next(0, 255),
                R = (byte)random.Next(0, 255),
                G = (byte)random.Next(0, 255),
                B = (byte)random.Next(0, 255),
            });

            double startheight = random.Next(0, (int)height);
            var topani = new DoubleAnimationUsingKeyFrames()
            {
                DecelerationRatio = 1,
                AutoReverse = true
            };
            Timeline.SetDesiredFrameRate(topani, 60);
            topani.KeyFrames.Add(new DiscreteDoubleKeyFrame(startheight, TimeSpan.FromSeconds(0d)));
            topani.KeyFrames.Add(new SplineDoubleKeyFrame(height, TimeSpan.FromSeconds(time), new KeySpline(0.5, 0, 1, 1)));


            e.BeginAnimation(Canvas.LeftProperty, leftani);
            e.BeginAnimation(Canvas.TopProperty, topani);
        }

        private Action action;
        private bool flagStopAni;
        private void startToggle_Click(object sender, RoutedEventArgs e)
        {
            if (startToggle.IsChecked == true)
            {
                action = new Action(runAnimation);
                flagStopAni = true;
                Application.Current.Dispatcher.BeginInvoke(action);
            }
            else
            {
                flagStopAni = false;
            }
        }

        private async void runAnimation()
        {
            do
            {
                start();
                await Task.Delay(100);
            } while (flagStopAni);
        }

        #endregion

        private int count;
        private System.Timers.Timer processTimer;
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            updateState();
        }

        private void updateState()
        {
            if (sprocketcontrol.IsIndeterminate == true)
            {
                if (processTimer == null)
                {
                    processTimer = new System.Timers.Timer(200);
                    processTimer.Elapsed += onProcessTimerElapsed;
                }
                processTimer.Start();
            }
            else
            {
                if (processTimer != null)
                {
                    processTimer.Stop();
                    processTimer.Elapsed -= onProcessTimerElapsed;
                    processTimer = null;
                    count = 0;
                    processTb.Text = "0";
                }
            }
        }

        private void onProcessTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => {
                processTb.Text = (++count).ToString();
                if (count > 99)
                {
                    sprocketcontrol.IsIndeterminate = false;
                    updateState();
                }
            });
        }

        #endregion

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems == null || e.AddedItems.Count == 0) return;
            ToastUtils.Show("Now what you are selected is : "+ e.AddedItems[0]);
        }

        private int density = 350;
        private int ewidth = 10;
        private void drawCirclrBtnClick(object sender, RoutedEventArgs e)
        {
            if(genColor == null)
                ensureGenColor();
            Canvas container = cvs;
            container.Children.Clear();
            Random r = new Random();

            int width = (int)container.ActualWidth;
            int height = (int)container.ActualHeight;
            int num = width * height / density;
            System.Diagnostics.Debug.WriteLine("Countor " + num);

            for (int i = 0; i < num; i++)
            {
                var circle = new Ellipse();
                circle.Height = circle.Width = r.Next(ewidth);
                circle.Fill = new SolidColorBrush(genColor(r));
                Canvas.SetLeft(circle, r.Next(width));
                Canvas.SetTop(circle, r.Next(height));
                container.Children.Add(circle);
            }
        }

        private void ensureGenColor()
        {
            if (colorfulToggle.IsChecked == true)
                genColor = newColorful;
            else
                genColor = newBlackWhite;
        }

        private void colorfulToggleClick(object sender, RoutedEventArgs e)
        {
            ensureGenColor();
            Canvas container = cvs;
            int width = (int)container.ActualWidth;
            int height = (int)container.ActualHeight;
            Random r = new Random();
            VisualTreeHelper.HitTest(container, null, f =>
            {
                var circle = f.VisualHit as Ellipse;
                if (circle != null)
                    circle.Fill = new SolidColorBrush(genColor(r));
                return HitTestResultBehavior.Continue;
            }, new GeometryHitTestParameters(new RectangleGeometry(new Rect(0, 0, width, height))));
        }

        private Func<Random, Color> genColor = null;
        private Color newBlackWhite(Random r)
        {
            return new Color
            {
                A = (byte)r.Next(255),
                R = 0,
                G = 0,
                B = 0
            };
        }

        private Color newColorful(Random r)
        {
            return new Color
            {
                A = (byte)r.Next(255),
                R = (byte)r.Next(255),
                G = (byte)r.Next(255),
                B = (byte)r.Next(255)
            };
        }
    }
}
