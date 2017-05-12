using System;
using System.Windows.Media;

namespace Example
{
    public class BrushMember : NotificationObject
    {
        public string Name { get; set; }
        private SolidColorBrush brush;
        public SolidColorBrush Brush
        {
            get
            {
                if (brush == null) brush = new SolidColorBrush();
                return brush;
            }
            set
            {
                brush = value;
                RaisePropertyChanged(nameof(Brush));
            }
        }
        public Color Color
        {
            get
            {
                return Brush.Color;
            }
            set
            {
                if(brush.Color != value)
                {
                    Brush.Color = value;
                    RaisePropertyChanged(nameof(Color));
                }
            }
        }
        private string rgb;
        public string RGB
        {
            get
            {
                return rgb;
            }
            set
            {
                if (rgb != value)
                {
                    if (isInnerchange)
                    {
                        rgb = value;
                        RaisePropertyChanged(nameof(RGB));
                    }
                    else updateFromRGBString(value);
                }
            }
        }
        private string stringValue;
        public string StringValue
        {
            get
            {
                return stringValue;
            }
            set
            {
                if (stringValue != value)
                {
                    if (isInnerchange)
                    {
                        stringValue = value;
                        RaisePropertyChanged(nameof(StringValue));
                    }
                    else updateFromString(value);
                }
            }
        }
        public override string ToString()
        {
            return Name;
        }
        
        public void updateAll()
        {
            if(Color != null)
            {
                updateFromColor(Color);
            }
        }

        private bool updateFromRGBString(string value)
        {
            if (isInnerchange) return false;
            isInnerchange = true;
            string[] colorParams = value.ToString().Split(',');
            if (colorParams.Length > 4 || colorParams.Length < 1)
            {
                isInnerchange = false;
                return false;
            }
            byte[] bytes = new byte[4];
            for (int i = 0; i < bytes.Length; ++i)
            {
                bytes[i] = 255;
            }
            int bytesindex = colorParams.Length > 3 ? 0 : 1;
            for (int i = 0; i < colorParams.Length && i < bytes.Length; ++i, ++bytesindex)
            {
                if (string.IsNullOrWhiteSpace(colorParams[i])) bytes[bytesindex] = 0;
                else bytes[bytesindex] = Convert.ToByte(colorParams[i]);
            }
            this.Color = new Color() { A = bytes[0], R = bytes[1], G = bytes[2], B = bytes[3] };
            this.StringValue = this.Color.ToString();
            this.RGB = value;
            isInnerchange = false;
            return true;
        }

        private bool updateFromString(string value)
        {
            if (isInnerchange) return false;
            isInnerchange = true;
            try
            {
                if (!value.Contains("#")) value = value.Insert(0, "#");
                var color = (Color)ColorConverter.ConvertFromString(value);
                this.Color = color;
                this.StringValue = value;
                this.RGB = color.A + "," + color.R + "," + color.G + "," + color.B;
            }
            catch(Exception e)
            {
                isInnerchange = false;
                System.Diagnostics.Debug.WriteLine("Color convert fail " + e.Message);
                return false;
            }
            finally { }
            isInnerchange = false;
            return true;
        }

        private void updateFromColor(Color color)
        {
            if (isInnerchange) return;
            isInnerchange = true;

            this.Color = color;
            this.StringValue = color.ToString();
            this.RGB = color.A + "," + color.R + "," + color.G + "," + color.B;
            
            isInnerchange = false;
        }

        private bool isInnerchange;
    }
}
