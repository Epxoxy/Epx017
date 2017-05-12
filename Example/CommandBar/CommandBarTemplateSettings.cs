using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Example
{
    public sealed class CommandBarTemplateSettings : DependencyObject
    {
        public double DeviationToOpenLength
        {
            get { return (double)GetValue(DeviationToOpenLengthProperty); }
            private set { SetValue(DeviationToOpenLengthProperty, value); }
        }
        public static readonly DependencyProperty DeviationToOpenLengthProperty =
            DependencyProperty.Register("DeviationToOpenLength", typeof(double), typeof(CommandBarTemplateSettings), 
                new PropertyMetadata(0d));
        
        public double DeviationToMinimalLength
        {
            get { return (double)GetValue(DeviationToMinimalLengthProperty); }
            set { SetValue(DeviationToMinimalLengthProperty, value); }
        }
        public static readonly DependencyProperty DeviationToMinimalLengthProperty =
            DependencyProperty.Register("DeviationToMinimalLength", typeof(double), typeof(CommandBarTemplateSettings), new PropertyMetadata(0d));
        
        public double OpenLength
        {
            get { return (double)GetValue(OpenLengthProperty); }
            private set { SetValue(OpenLengthProperty, value); }
        }
        public static readonly DependencyProperty OpenLengthProperty =
            DependencyProperty.Register("OpenLength", typeof(double), typeof(CommandBarTemplateSettings), 
                new PropertyMetadata(0d));
        
        public double MinimalLength
        {
            get { return (double)GetValue(MinimalLengthProperty); }
            private set { SetValue(MinimalLengthProperty, value); }
        }
        public static readonly DependencyProperty MinimalLengthProperty =
            DependencyProperty.Register("MinimalLength", typeof(double), typeof(CommandBarTemplateSettings), 
                new PropertyMetadata(20d));
        
        public double CompactLength
        {
            get { return (double)GetValue(CompactLengthProperty); }
            private set { SetValue(CompactLengthProperty, value); }
        }
        public static readonly DependencyProperty CompactLengthProperty =
            DependencyProperty.Register("CompactLength", typeof(double), typeof(CommandBarTemplateSettings), 
                new PropertyMetadata(40d));

        public double NegativeDeviationToOpenLength
        {
            get { return (double)GetValue(NegativeDeviationToOpenLengthProperty); }
            private set { SetValue(NegativeDeviationToOpenLengthProperty, value); }
        }
        public static readonly DependencyProperty NegativeDeviationToOpenLengthProperty =
            DependencyProperty.Register("NegativeDeviationToOpenLength", typeof(double), typeof(CommandBarTemplateSettings),
                new PropertyMetadata(0d));

        private double minOpenLength = 60d;
        public double MinOpenLength => minOpenLength;

        public static readonly DependencyPropertyKey MinOpenLengthPropertyKey =
            DependencyProperty.RegisterReadOnly("MinOpenLength", typeof(double), 
                typeof(CommandBarTemplateSettings), 
                new PropertyMetadata(80d));
        public static readonly DependencyProperty MinOpenLengthProperty = MinOpenLengthPropertyKey.DependencyProperty;
        
        public double VisibleHeight
        {
            get { return (double)GetValue(VisibleHeightProperty); }
            set { SetValue(VisibleHeightProperty, value); }
        }
        public static readonly DependencyProperty VisibleHeightProperty =
            DependencyProperty.Register("VisibleHeight", typeof(double), typeof(CommandBarTemplateSettings), new PropertyMetadata(0d));
        
        internal CommandBarTemplateSettings(CommandBar owner)
        {
            this.Owner = owner;
            this.Update();
        }

        internal CommandBar Owner { get; }

        internal void Update()
        {
            double height = this.Owner.Height;
            double actualHeight = this.Owner.ActualHeight;
            double targetLength = MinOpenLength;
            if (double.IsNaN(height) || double.IsInfinity(height)) height = 0d;
            else
            {
                double max = Math.Max(height, actualHeight);
                if (max > targetLength) targetLength = max;
            }

            if (this.OpenLength != targetLength)
            {
                this.OpenLength = targetLength;
            }
            this.DeviationToOpenLength = this.OpenLength - actualHeight;
            this.DeviationToMinimalLength = actualHeight - this.MinimalLength;
            this.NegativeDeviationToOpenLength = -this.DeviationToOpenLength;
            this.VisibleHeight = actualHeight;
        }
    }
}
