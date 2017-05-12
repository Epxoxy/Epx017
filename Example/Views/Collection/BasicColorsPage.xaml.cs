using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Example
{
    /// <summary>
    /// Interaction logic for SystemColors.xaml
    /// </summary>
    public partial class BasicColorsPage : Page
    {
        private List<BrushMember> SystemColorsList{ get; set; }
        private List<BrushMember> DefBrushMembers { get; set; }
        public BrushMember CacheBrush { get; set; }
        public BasicColorsPage()
        {
            InitializeComponent();
            this.Loaded += onLoaded;
        }

        private async void onLoaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= onLoaded;
            await Task.Run(() =>
            {
                SystemColorsList = new List<BrushMember>();
                var systemColorsProperties = typeof(SystemColors).GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                foreach (var prop in systemColorsProperties)
                {
                    if (prop.GetValue(null, null) is SolidColorBrush)
                    {
                        string name = prop.Name;
                        var member = new BrushMember()
                        {
                            Name = name,
                            Brush = (SolidColorBrush)prop.GetValue(null, null),
                        };
                        member.updateAll();
                        SystemColorsList.Add(member);
                    }
                }
                DefBrushMembers = new List<BrushMember>();
                var brushesProperties = typeof(Brushes).GetProperties();
                foreach(var prop in brushesProperties)
                {
                    string name = prop.Name;
                    var member = new BrushMember()
                    {
                        Name = name,
                        Brush = (SolidColorBrush)prop.GetValue(null, null),
                    };
                    member.updateAll();
                    DefBrushMembers.Add(member);
                }
            });

            CacheBrush = new BrushMember();
            ConverterRoot.DataContext = CacheBrush;
            SysColorsDataGrid.ItemsSource = SystemColorsList;
            BrushesDataGrid.ItemsSource = DefBrushMembers;
        }
    }
}
