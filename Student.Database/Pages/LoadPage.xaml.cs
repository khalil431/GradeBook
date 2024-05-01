using System.Windows.Controls;

namespace Student.Database
{
    /// <summary>
    /// Interaction logic for LoadPage.xaml
    /// </summary>
    public partial class LoadPage : Page
    {
        public LoadPage()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(false, true, false);
        }
    }
}
