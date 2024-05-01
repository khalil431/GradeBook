using System.Windows.Controls;

namespace Student.Database
{
    /// <summary>
    /// Interaction logic for InsertPage.xaml
    /// </summary>
    public partial class InsertPage : Page
    {
        public InsertPage()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(false, true, false);
        }
    }
}
