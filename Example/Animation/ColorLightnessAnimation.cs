using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Example
{
    public class ColorLightnessAnimation : ColorAnimationBase
    {
        private Double[] _keyvalues;
        private bool _isAnimationFunctionValid;
        static ColorLightnessAnimation()
        {
            PropertyChangedCallback propCallback = new PropertyChangedCallback(AnimationFunctionChanged);
            OriginColorProperty = DependencyProperty.Register("OriginColor", typeof(Color), typeof(ColorLightnessAnimation), new PropertyMetadata(null));
            ToProperty = DependencyProperty.Register("To", typeof(double), typeof(ColorLightnessAnimation), new PropertyMetadata(0d, propCallback));
            FromProperty = DependencyProperty.Register("From", typeof(double), typeof(ColorLightnessAnimation), new PropertyMetadata(0d, propCallback));
            ByProperty = DependencyProperty.Register("By", typeof(double), typeof(ColorLightnessAnimation), new PropertyMetadata(0d, propCallback));
            EasingFunctionProperty = DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(ColorLightnessAnimation), new PropertyMetadata(null));
        }

        private static void AnimationFunctionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorLightnessAnimation a = (ColorLightnessAnimation)d;
            a._isAnimationFunctionValid = false;
        }

        #region Constructors

        public ColorLightnessAnimation() : base()
        {
        }
        public ColorLightnessAnimation(float toValue, Duration duration) : this()
        {
            To = toValue;
            Duration = duration;
        }
        public ColorLightnessAnimation(float toValue, Duration duration, FillBehavior fillBehavior) : this()
        {
            To = toValue;
            Duration = duration;
            FillBehavior = fillBehavior;
        }
        public ColorLightnessAnimation(float fromValue, float toValue, Duration duration) : this()
        {
            From = fromValue;
            To = toValue;
            Duration = duration;
        }
        public ColorLightnessAnimation(float fromValue, float toValue, Duration duration, FillBehavior fillBehavior) : this()
        {
            From = fromValue;
            To = toValue;
            Duration = duration;
            FillBehavior = fillBehavior;
        }

        #endregion

        #region Freezable

        public new ColorLightnessAnimation Clone()
        {
            return (ColorLightnessAnimation)base.Clone();
        }

        protected override Freezable CreateInstanceCore()
        {
            return new ColorLightnessAnimation();
        }

        #endregion

        protected override Color GetCurrentValueCore(Color defaultOriginValue, Color defaultDestinationValue, AnimationClock animationClock)
        {
            System.Diagnostics.Debug.Assert(animationClock.CurrentState != ClockState.Stopped);
            if (!_isAnimationFunctionValid) ValidateAnimationFunction();
            double progress = animationClock.CurrentProgress.Value;
            IEasingFunction easingFunction = EasingFunction;
            
            if (easingFunction != null)
            {
                progress = easingFunction.Ease(progress);
            }
            double fromVal = _keyvalues[0];
            double toVal = _keyvalues[1];
            double target;
            if(fromVal > toVal)
            {
                target = (1 - progress) *(fromVal - toVal) + toVal;
            }
            else
            {
                target = (toVal - fromVal) * progress + fromVal;
            }
            Color originColor = OriginColor;
            if (target == 0) return originColor;
            return ColorEx.ChangeColorBrightness(originColor, target);
        }

        private void ValidateAnimationFunction()
        {
            _keyvalues = new Double[2];
            //Validate From
            double fromVal = From;
            if (fromVal > 100) _keyvalues[0] = 100;
            else if (fromVal < -100) _keyvalues[0] = -100;
            else _keyvalues[0] = fromVal;
            //Validate TO
            double toVal = To;
            if (toVal > 100) _keyvalues[1] = 100;
            else if (toVal < -100) _keyvalues[1] = -100;
            else _keyvalues[1] = toVal;
            _isAnimationFunctionValid = true;
        }
        
        public Color OriginColor
        {
            get { return (Color)GetValue(OriginColorProperty); }
            set { SetValue(OriginColorProperty, value); }
        }
        public static readonly DependencyProperty OriginColorProperty;
        
        public double To
        {
            get { return (double)GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }
        public static readonly DependencyProperty ToProperty;

        public double From
        {
            get { return (double)GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }
        public static readonly DependencyProperty FromProperty;

        public double By
        {
            get { return (double)GetValue(ByProperty); }
            set { SetValue(ByProperty, value); }
        }
        public static readonly DependencyProperty ByProperty;

        public IEasingFunction EasingFunction
        {
            get
            {
                return (IEasingFunction)GetValue(EasingFunctionProperty);
            }
            set
            {
                SetValue(EasingFunctionProperty, value);
            }
        }
        public static readonly DependencyProperty EasingFunctionProperty;

    }

    public static class ColorEx
    {
        public static Color LightToTransparent(Color baseColor, float percLighter)
        {
            if (percLighter == 0.0f)
            {
                return Colors.Transparent;
            }
            else if (percLighter == 1.0f)
            {
                return baseColor;
            }
            else
            {
                Color lightLight = Colors.White;

                int dr = baseColor.R - lightLight.R;
                int dg = baseColor.G - lightLight.G;
                int db = baseColor.B - lightLight.B;

                return Color.FromRgb((byte)(baseColor.R - (byte)(dr * percLighter)),
                                      (byte)(baseColor.G - (byte)(dg * percLighter)),
                                      (byte)(baseColor.B - (byte)(db * percLighter)));
            }
        }
        public static Color DarkToBlack(Color baseColor, float percDarker)
        {
            if (percDarker == 0.0f)
            {
                return baseColor;
            }
            else if (percDarker == 1.0f)
            {
                return Colors.Black;
            }
            else
            {
                Color darkDark = Colors.Black;

                int dr = baseColor.R - darkDark.R;
                int dg = baseColor.G - darkDark.G;
                int db = baseColor.B - darkDark.B;

                return Color.FromRgb((byte)(baseColor.R - (byte)(dr * percDarker)),
                                      (byte)(baseColor.G - (byte)(dg * percDarker)),
                                      (byte)(baseColor.B - (byte)(db * percDarker)));
            }
        }
        
        public static Color DarkenBy(Color baseColor, double percent)
        {
            return ChangeColorBrightness(baseColor, (float)percent / 100.0f);
        }
        public static Color LightenBy(Color baseColor, double percent)
        {
            return ChangeColorBrightness(baseColor, -1f * ((float)percent / 100.0f));
        }
        public static Color ChangeColorBrightness(Color baseColor, double percent)
        {
            return ChangeColorBrightness(baseColor, (float)percent / 100.0f);
        }
        public static Color ChangeColorBrightness(Color baseColor, float correctionFactor)
        {
            float red = (255 - baseColor.R) * correctionFactor + baseColor.R;
            float green = (255 - baseColor.G) * correctionFactor + baseColor.G;
            float blue = (255 - baseColor.B) * correctionFactor + baseColor.B;
            return Color.FromArgb(baseColor.A, (byte)red, (byte)green, (byte)blue);
        }
    }
    
}
