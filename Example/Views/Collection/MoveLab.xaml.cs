using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Example
{
    /// <summary>
    /// Interaction logic for MoveLab.xaml
    /// </summary>
    public partial class MoveLab : Page
    {
        public MoveLab()
        {
            InitializeComponent();
            PreviewKeyDown += Canvas_KeyDown;
        }


        DoubleAnimation anileft;
        DoubleAnimation anitop;
        Storyboard sbleft;
        Storyboard sbtop;
        private bool isLeftPause = false;
        private bool isTopPause = false;
        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {/**
            if (xcountor >= 0)
            {
                if (e.Key == Key.Left && xcountor >= 10)
                {
                    xcountor -= 10;
                }
                if (e.Key == Key.Right && xcountor < canvas.ActualWidth - 10)
                {
                    xcountor += 10;
                }
                Canvas.SetLeft(ellipse, xcountor);
            }
            if (ycountor >= 0)
            {
                if (e.Key == Key.Up && ycountor >= 10)
                {
                    ycountor -= 10;
                }
                if (e.Key == Key.Down && ycountor < canvas.ActualHeight - 10)
                {
                    ycountor += 10;
                }
                Canvas.SetTop(ellipse, ycountor);
            }**/
            if (e.Key == Key.Right)
            {
                if (sbleft == null)
                {
                    sbleft = new Storyboard();
                    anileft = new DoubleAnimation();
                    anileft.Duration = TimeSpan.FromSeconds(0.5 * canvas.ActualWidth / 10);
                    anileft.From = Canvas.GetLeft(ellipse);
                    anileft.To = canvas.ActualWidth;
                    Storyboard.SetTarget(anileft, ellipse);
                    Storyboard.SetTargetProperty(anileft, new PropertyPath("(Canvas.Left)"));
                    sbleft.Children.Add(anileft);
                    sbleft.Begin();
                }
                else if (isLeftPause)
                {
                    sbleft.Resume();
                    isLeftPause = false;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Move to right");
                }
            }
            if (e.Key == Key.Down)
            {
                if (sbtop == null)
                {
                    sbtop = new Storyboard();
                    anitop = new DoubleAnimation();
                    anitop.Duration = TimeSpan.FromSeconds(0.5 * canvas.ActualHeight / 10);
                    anitop.From = Canvas.GetTop(ellipse);
                    anitop.To = canvas.ActualHeight;
                    Storyboard.SetTarget(anitop, ellipse);
                    Storyboard.SetTargetProperty(anitop, new PropertyPath("(Canvas.Top)"));
                    sbtop.Children.Add(anitop);
                    sbtop.Begin();
                }
                else if (isTopPause)
                {
                    sbtop.Resume();
                    isTopPause = false;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Move to down");
                }
            }
        }

        private void ellipse_GotFocus(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Got focus");
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right && sbleft != null)
            {
                sbleft.Pause();
                System.Diagnostics.Debug.WriteLine("Set anileft to null");
                isLeftPause = true;
            }
            if (e.Key == Key.Down && sbtop != null)
            {
                sbtop.Pause();
                System.Diagnostics.Debug.WriteLine("Set anitop to null");
                isTopPause = true;
            }
        }

    }
}
