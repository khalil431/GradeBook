using System.Windows.Controls;

namespace Student.Database
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(false, true, false);
        }
    }
}
