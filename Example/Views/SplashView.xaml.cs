using System.Windows.Controls;
namespace Example
{
    /// <summary>
    /// Interaction logic for SplashView.xaml
    /// </summary>
    public partial class SplashView : UserControl
    {
        public SplashView()
        {
            InitializeComponent();
        }

        public void StartStoryBoard()
        {
            var sb = FindResource("loadedStoryboard") as System.Windows.Media.Animation.Storyboard;
            if (sb != null) sb.Begin();
        }
    }
}
