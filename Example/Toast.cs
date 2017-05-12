using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Example
{
    public class ToastUtils
    {
        struct ToastItem
        {
            public string Text { get; set; }
            public double Time { get; set; }
            public ToastItem(string text, double time)
            {
                this.Text = text;
                this.Time = time;
            }
        }
        private static Queue<ToastItem> toastQueue = new Queue<ToastItem>();
        private static bool isShowing;
        public static void Show(string text, double time = 2000)
        {
            toastQueue.Enqueue(new ToastItem(text, time));
            if (toastQueue.Count == 1)
                EnsureToast();
        }
        public static void Show(string text, Window win, double time = 2000)
        {
            if (win == null) return;
            Show(text, win.Content as UIElement, time);
        }
        private static void EnsureToast()
        {
            if (!isShowing && toastQueue.Count > 0)
            {
                var item = toastQueue.Dequeue();
                Show(item.Text, Application.Current.MainWindow, item.Time);
            }
        }
        public static async void Show(string text, UIElement element, double time = 2000)
        {
            if (element == null) return;
            var origin = Color.FromArgb(0, 84, 84, 84);
            var bg = Color.FromArgb(240, 84, 84, 84);
            var root = new Border
            {
                CornerRadius = new CornerRadius(5),
                SnapsToDevicePixels = true,
                Background = new SolidColorBrush(bg),
                MinHeight = 100,
                MinWidth = 200,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Child = new TextBlock
                {
                    Text = text,
                    Foreground = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(20)
                }
            };
            var fadeIn = new ColorAnimation(origin, bg, TimeSpan.FromMilliseconds(100));
            var fadeOut = new ColorAnimation(bg, origin, TimeSpan.FromMilliseconds(200));
            using (var overlay = Epxoxy.Controls.OverlayAdorner<Border>.Overlay(element, root))
            {
                isShowing = true;
                root.Background.BeginAnimation(SolidColorBrush.ColorProperty, fadeIn);
                await Task.Delay(TimeSpan.FromMilliseconds(time));
                root.Background.BeginAnimation(SolidColorBrush.ColorProperty, fadeOut);
                await Task.Delay(TimeSpan.FromMilliseconds(200));
                isShowing = false;
                EnsureToast();
            }
        }
    }
}
