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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Example
{
    /// <summary>
    /// Interaction logic for InkCanvasTest.xaml
    /// </summary>
    public partial class InkCanvasTest : Page
    {
        private List<BrushMember> Colors { get; set; }
        public InkCanvasTest()
        {
            InitializeComponent();
            this.Loaded += onLoaded;
        }

        private async void onLoaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => {
                Colors = new List<BrushMember>();
                Type brushesType = typeof(Brushes);
                var brushesProperties = brushesType.GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                foreach (var prop in brushesProperties)
                {
                    string name = prop.Name;
                    Colors.Add(new BrushMember() { Name = name, Brush = ((SolidColorBrush)prop.GetValue(null, null)) });
                }
            });
            colorCombobox.ItemsSource = Colors;
        }
    }
}
