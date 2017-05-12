using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Example
{
    public class TryPanel : StackPanel
    {
        public TryPanel()
        {
        }

        public new void LineUp()
        {
            var first = this.Children[0];
            this.Children.Remove(first);
            this.Children.Add(first);
        }

        public new void LineDown()
        {
            var last = this.Children[this.Children.Count - 1];
            this.Children.Remove(last);
            this.Children.Insert(0, last);
        }

        public new void MouseWheelUp()
        {
            var last = this.Children[this.Children.Count - 1];
            this.Children.Remove(last);
            this.Children.Insert(0, last);
            this.InvalidateMeasure();
        }

        public new void MouseWheelDown()
        {
            var first = this.Children[0];
            this.Children.Remove(first);
            this.Children.Add(first);
            this.InvalidateMeasure();
        }
    }
}
