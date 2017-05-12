using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Example
{
    /// <summary>
    /// Interaction logic for Epxoxy.xaml
    /// </summary>
    public partial class EpxoxyEx : Page
    {
        public EpxoxyEx()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var flyout = new Epxoxy.Controls.Flyout();
            var rect = new Rectangle() { Fill = Brushes.SkyBlue, MinWidth = 200, MinHeight = 300 };
            flyout.Content = rect;
            using (var overlay = Epxoxy.Controls.OverlayAdorner<UIElement>.Overlay(this, flyout))
            {
                await flyout.WaitForCloseAsync();
            }
        }

        private void colorPickerBtnClick(object sender, RoutedEventArgs e)
        {
            var picker = new Epxoxy.Controls.ColorPickerDialog();
            picker.Owner = Application.Current.MainWindow;
            picker.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            var successed = picker.ShowDialog();
            if (successed == true)
            {
                System.Diagnostics.Debug.WriteLine("Color selected : " + picker.SelectedColor);
            }
        }

        private void messageDialogBtnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Epxoxy.Controls.MessageDialog
            {
                Content = "This is a message"
            };
            if (dialog.ShowDialog() == true)
            {
            }
            System.Diagnostics.Debug.WriteLine(dialog.DialogResult);
        }

        private async void asyncContentDialogBtnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Epxoxy.Controls.ContentDialog() { Content = "Messagebox test" };
            var result = await dialog.ShowAsync();
        }

        private async void asyncPickerDialogBtnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Epxoxy.Controls.ColorPickerContentDialog();
            var result = await dialog.ShowAsync();
        }
    }
}
