using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Example
{
    /// <summary>
    /// Interaction logic for CubicBezier.xaml
    /// </summary>
    public partial class CubicBezier : Page
    {
        public CubicBezier()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= OnLoaded;
            onEllipseOneDragging(null, null);
            onEllipseTwoDragging(null, null);
        }
        
        #region Cubic
        private Point Point1 { get; set; }
        private Point Point2 { get; set; }
        private double Length { get; set; } = 300;
        private bool isBack = false;

        const string format = "{0:0.00},{1:0.00}";
        private void onEllipseOneDragging(object sender, MouseEventArgs e)
        {
            double x = (p1Line.X2 - 10);
            double y = (p1Line.Y2 - 10);
            Point1 = new Point(x < 0 ? 0 : x, y < 0 ? 0: y);
            bezier.Point1 = Point1;
            p1Run.Text = string.Format(format, Math.Round(Point1.X / Length, 3), Math.Round(Point1.Y / Length, 3));
        }

        private void onEllipseTwoDragging(object sender, MouseEventArgs e)
        {
            double x = (p2Line.X2 - 10);
            double y = (p2Line.Y2 - 10);
            Point2 = new Point(x < 0 ? 0 : x, y < 0 ? 0 : y);
            bezier.Point2 = Point2;
            p2Run.Text = string.Format(format, Math.Round(Point2.X / Length, 3), Math.Round(Point2.Y / Length, 3));
        }
        
        private void beginBezier(FrameworkElement target, double value, KeyTime keytime)
        {
            if (target != null)
            {
                Point p1 = new Point(Math.Round(Point1.X / Length, 3), Math.Round(Point1.Y / Length, 3));
                Point p2 = new Point(Math.Round(Point2.X / Length, 3), Math.Round(Point2.Y / Length, 3));
                SplineDoubleKeyFrame sdkf = new SplineDoubleKeyFrame(value, keytime);
                try
                {
                    sdkf.KeySpline = new KeySpline(p1, p2);
                }catch(Exception ex)
                {
                    MessageBox.Show("Error data : " + ex.Message, "Exception");
                    return;
                }

                DoubleAnimationUsingKeyFrames daukf = new DoubleAnimationUsingKeyFrames();
                daukf.KeyFrames.Add(sdkf);

                var board = new Storyboard();
                Storyboard.SetTarget(daukf, target);
                Storyboard.SetTargetProperty(daukf, new PropertyPath("(FrameworkElement.RenderTransform).(TranslateTransform.X)"));
                board.Children.Add(daukf);
                board.Begin();
            }
        }

        private void beginEase(FrameworkElement target, double value, KeyTime keytime, IEasingFunction easingFunc)
        {
            if (target != null)
            {
                var edkf = new EasingDoubleKeyFrame(value, keytime);
                edkf.EasingFunction = easingFunc;

                DoubleAnimationUsingKeyFrames daukf = new DoubleAnimationUsingKeyFrames();
                daukf.KeyFrames.Add(edkf);

                var board = new Storyboard();
                Storyboard.SetTarget(daukf, target);
                Storyboard.SetTargetProperty(daukf, new PropertyPath("(FrameworkElement.RenderTransform).(TranslateTransform.X)"));
                board.Children.Add(daukf);
                board.Begin();
            }
        }

        private void goBtnClick(object sender, RoutedEventArgs e)
        {
            var easingFunc = EaseCombobox.SelectedItem as EasingFunctionBase;
            if(easingFunc != null)
            {
                easingFunc.EasingMode = (EasingMode)modeCombobox.SelectedItem;
            }
            var time = Math.Round(slider.Value, 3);
            double targetX = 0d;
            if (!isBack) targetX = preLayer.ActualWidth;
            beginBezier(rect1, targetX, TimeSpan.FromSeconds(time));
            beginEase(rect2, targetX, TimeSpan.FromSeconds(time), easingFunc);
            isBack = !isBack;
        }

        #endregion

        private void resetBtnClick(object sender, RoutedEventArgs e)
        {
            p1Ellipse.RenderTransform = new TranslateTransform();
            p2Ellipse.RenderTransform = new TranslateTransform(300,300);
            onEllipseOneDragging(null, null);
            onEllipseTwoDragging(null, null);
        }
    }

    static class DrawingUtility
    {
        static void solvexy(double a, double b,double c , double d ,double e, double f, out double i, out double j)
        {
            j = (c - a / d * f) / (b - a * e / d);
            i = (c - (b * j)) / a;
        }

        static double b0 (double t) { return Math.Pow(1 - t, 3); }
        static double b1(double t) { return t * ( 1 - t) * ( 1 - t) * 3; }
        static double b2(double t) { return (1 - t) * t *  t * 3; }
        static double b3(double t) { return Math.Pow(t, 3); }

        static void bez4pts1(double x0, double y0, double x4, double y4, double x5, double y5, double x3, double y3, out double x1, out double y1, out double x2, out double y2)
        {
            double c1 = Math.Sqrt((x4 - x0) * (x4 - x0) + (y4 - y0) * (y4 - y0));
            double c2 = Math.Sqrt((x5 - x4) * (x5 - x4) + (y5 - y4) * (y5 - y4));
            double c3 = Math.Sqrt((x3 - x5) * (x3 - x5) + (y3 - y5) * (y3 - y5));
            double t1 = c1 / (c1 + c2 + c3);
            double t2 = (c1 + c2) / (c1 + c2 + c3);
            solvexy(b1(t1), b2(t1), x4 - (x0 * b0(t1)) - (x3 * b3(t1)), b1(t2), b2(t2), x5 - (x0 * b0(t2)) - x3 * b3(t2), out x1, out x2);
            solvexy(b1(t1), b2(t1), y4 - (y0 * b0(t1)) - (y3 * b3(t1)), b1(t2), b2(t2), y5 - (y0 * b0(t2)) - y3 * b3(t2), out y1, out y2);
        }
        static public PathFigure BezierFromIntersection(Point start, Point ctl1, Point ctl2, Point end)
        {
            double x1, y1, x2, y2;
            bez4pts1(start.X, start.Y, ctl1.X, ctl1.Y, ctl2.X, ctl2.Y, end.X, end.Y, out x1, out y1, out x2, out y2);
            PathFigure p = new PathFigure { StartPoint = start };
            p.Segments.Add(new BezierSegment { Point1 = new Point(x1, y1), Point2 = new Point(x2, y2), Point3 = end });
            return p;
        }
    }
}
