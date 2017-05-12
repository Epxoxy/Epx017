using System.Text;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Input;

namespace Example
{
    /// <summary>
    /// Interaction logic for Example.xaml
    /// </summary>
    public partial class ExamplePage : Page
    {

        public ExamplePage()
        {
            InitializeComponent();

            Loaded += ExamplePage_Loaded;

            //PreviewKeyUp += Window_PreviewKeyUp;
            //System.Threading.Thread.Sleep(4000);//Test for Async create instance of page, testing , not complete.

        }

        private PersonPage personPage;
        private void ExamplePage_Loaded(object sender, RoutedEventArgs e)
        {
            personPage = new PersonPage();
            //personPage.ItemLayer.ItemsSource = toast.UnReadList;
            //dialog.ShowAsync();
        }

        private void PersonBtnClick(object sender, RoutedEventArgs e)
        {
            navigationFrame.Navigate(personPage);
        }
    }
}
