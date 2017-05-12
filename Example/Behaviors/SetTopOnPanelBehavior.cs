using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Example
{
    public class SetTopOnPanelBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            var parent = this.AssociatedObject.Parent;
            Binding binding = null;
            var panel = parent as System.Windows.Controls.Panel;
            if(panel != null)
            {
                binding = new Binding()
                {
                    Source = panel,
                    Path = new PropertyPath("Children.Count"),
                    Mode = BindingMode.OneWay
                };
            }
            else
            {
                var itemscontrol = parent as System.Windows.Controls.ItemsControl;
                if (itemscontrol != null)
                {
                    binding = new Binding()
                    {
                        Source = panel,
                        Path = new PropertyPath("Items.Count"),
                        Mode = BindingMode.OneWay
                    };
                }
            }
            if(binding != null)
                this.AssociatedObject.SetBinding(System.Windows.Controls.Panel.ZIndexProperty, binding);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}
