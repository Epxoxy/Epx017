using Microsoft.Expression.Interactivity.Layout;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Example
{
    public class SmoothScrollViewer : ScrollViewer
    {
        public SmoothScrollViewer()
        {
            initFrame = new DiscreteDoubleKeyFrame() { KeyTime = TimeSpan.Zero };
            beginFrame = new LinearDoubleKeyFrame() { KeyTime = TimeSpan.FromMilliseconds(60) };
            endFrame = new LinearDoubleKeyFrame() { KeyTime = TimeSpan.FromMilliseconds(150) };
            scrollFrames = new DoubleAnimationUsingKeyFrames();
            scrollFrames.KeyFrames.Add(initFrame);
            scrollFrames.KeyFrames.Add(beginFrame);
            scrollFrames.KeyFrames.Add(endFrame);

            scrollStoryboard = new Storyboard();
            scrollStoryboard.Children.Add(scrollFrames);
            Storyboard.SetTarget(scrollFrames, this);
            Storyboard.SetTargetProperty(scrollFrames, new PropertyPath(SmoothScrollViewer.OffsetProperty));
            scrollStoryboard.Completed += OnStoryboardCompleted;
        }

        private void OnStoryboardCompleted(object sender, EventArgs e)
        {
            if(overlay != null)
                overlay.Visibility = Visibility.Collapsed;
            System.Diagnostics.Debug.WriteLine("StoryboardCompleted");
            isScrollBegan = false;
        }

        private DoubleKeyFrame initFrame;
        private DoubleKeyFrame beginFrame;
        private DoubleKeyFrame endFrame;
        private DoubleAnimationUsingKeyFrames scrollFrames;
        private Storyboard scrollStoryboard;
        private bool isScrollBegan = false;

        public static readonly DependencyProperty OffsetProperty = 
            DependencyProperty.Register("Offset", typeof(double), 
                typeof(SmoothScrollViewer), 
                new PropertyMetadata(OnOffsetChanged));
        public double Offset
        {
            get { return (double)GetValue(OffsetProperty); }
            set { SetValue(OffsetProperty, value); }
        }

        public static readonly DependencyProperty DurationProperty = 
            DependencyProperty.Register("Duration", typeof(TimeSpan), 
                typeof(SmoothScrollViewer),
                new PropertyMetadata(OnDurationChanged));
        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public static readonly DependencyProperty SlowdownDurationProperty = 
            DependencyProperty.Register("SlowdownDuration", typeof(TimeSpan),
                typeof(SmoothScrollViewer), 
                new PropertyMetadata(OnSlowdownDurationChanged));
        public TimeSpan SlowdownDuration
        {
            get { return (TimeSpan)GetValue(SlowdownDurationProperty); }
            set { SetValue(SlowdownDurationProperty, value); }
        }

        public static readonly DependencyProperty MouseWheelDeltaDividerProperty = 
            DependencyProperty.Register("MouseWheelDeltaDivider", typeof(double), 
                typeof(SmoothScrollViewer), 
                new PropertyMetadata(1.0, OnMouseWheelDeltaDividerChanged));
        public double MouseWheelDeltaDivider
        {
            get { return (double)GetValue(MouseWheelDeltaDividerProperty); }
            set { SetValue(MouseWheelDeltaDividerProperty, value); }
        }

        public static readonly DependencyProperty IsSmoothScrollingEnabledProperty = 
            DependencyProperty.Register("IsSmoothScrollingEnabled", typeof(bool),
                typeof(SmoothScrollViewer), 
                new PropertyMetadata(true, OnIsSmoothScrollingEnabledChanged));
        public bool IsSmoothScrollingEnabled
        {
            get { return (bool)GetValue(IsSmoothScrollingEnabledProperty); }
            set { SetValue(IsSmoothScrollingEnabledProperty, value); }
        }

        private static void OnMouseWheelDeltaDividerChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var viewer = sender as SmoothScrollViewer;
            if (viewer != null)
                viewer.deltaDividerCache = (double)e.NewValue;
        }
        private static void OnIsSmoothScrollingEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var viewer = sender as SmoothScrollViewer;
            if(viewer != null)
                viewer.smoothEnabledCache = (bool)e.NewValue;
        }
        private static void OnOffsetChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var viewer = sender as SmoothScrollViewer;
            viewer.ScrollToVerticalOffset((double)e.NewValue);
        }
        private static void OnDurationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var viewer = sender as SmoothScrollViewer;
            if(viewer != null)
                viewer.scrollFrames.Duration = new Duration((TimeSpan)e.NewValue);
        }
        private static void OnSlowdownDurationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var viewer = sender as SmoothScrollViewer;
            viewer?.CalculateNewKeyTime(((TimeSpan)e.NewValue).TotalMilliseconds);
        }

        private void CalculateNewKeyTime(double endMs)
        {
            if (endMs < 0) endMs = 0;
            double beginMs = scrollFrames.Duration.TimeSpan.TotalMilliseconds - endFrame.KeyTime.TimeSpan.TotalMilliseconds;
            double totalMs = beginMs + endMs;
            endFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(endMs));
            scrollFrames.Duration = new Duration(TimeSpan.FromMilliseconds(totalMs));
        }

        private int lastTime = 0;
        private double deltaDividerCache = 1.0;
        private bool smoothEnabledCache = true;
        private System.Windows.Shapes.Rectangle overlay;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            overlay = GetTemplateChild("PART_Overlay") as System.Windows.Shapes.Rectangle;
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            if (!smoothEnabledCache)
            {
                base.OnPreviewMouseWheel(e);
            }else
            {
                System.Diagnostics.Debug.WriteLine("Last -> " + (e.Timestamp - lastTime));
                lastTime = e.Timestamp;
                AnimationScroll(e.Delta, deltaDividerCache);
                e.Handled = true;
            }
        }

        private void AnimationScroll(int eventDelta, double deltaDevider)
        {
            double delta = -eventDelta / deltaDevider;
            if ((delta > 0 && this.VerticalOffset >= this.ScrollableHeight)
                || (delta < 0 && this.VerticalOffset <= 0))
                return;

            TimeSpan storyboardTime = (isScrollBegan ? scrollStoryboard.GetCurrentTime() : TimeSpan.Zero);
            System.Diagnostics.Debug.WriteLine($"\nOffset->{this.VerticalOffset}\nDelta->{delta}\nNowTime->{storyboardTime}");
            //System.Diagnostics.Debug.WriteLine(Duration + SlowdownDuration);

            if (overlay != null)
                overlay.Visibility = Visibility.Visible;
            if (storyboardTime >= TimeSpan.Zero)
            {
                scrollStoryboard.Stop();

                delta += (double)endFrame.Value - this.VerticalOffset;

                double newOffset = this.VerticalOffset + delta;
                if (newOffset < 0)
                    newOffset = 0;
                if (newOffset > this.ScrollableHeight)
                    newOffset = this.ScrollableHeight;

                initFrame.Value = this.VerticalOffset;
                beginFrame.Value = newOffset * 0.8 + this.VerticalOffset * 0.2;
                endFrame.Value = newOffset;
                System.Diagnostics.Debug.WriteLine("initFrame -> " + initFrame.Value);
                System.Diagnostics.Debug.WriteLine("beginFrame -> " + beginFrame.Value);
                System.Diagnostics.Debug.WriteLine("endFrame -> " + endFrame.Value);
                isScrollBegan = true;
                scrollStoryboard.Begin();
            }
            else
            {
                scrollStoryboard.Stop();

                double newOffset = this.VerticalOffset + delta;
                if (newOffset < 0)
                    newOffset = 0;
                if (newOffset > this.ScrollableHeight)
                    newOffset = this.ScrollableHeight;

                initFrame.Value = this.VerticalOffset;
                beginFrame.Value = newOffset * 0.8 + this.VerticalOffset * 0.2;
                endFrame.Value = newOffset;
                isScrollBegan = true;
                scrollStoryboard.Begin();
                System.Diagnostics.Debug.WriteLine("stop begin");
            }
            System.Diagnostics.Debug.WriteLine("Target -> " + endFrame.Value);
        }
    }

}
