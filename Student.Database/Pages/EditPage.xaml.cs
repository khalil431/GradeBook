using System.Windows.Controls;

namespace Student.Database
{
    /// <summary>
    /// Interaction logic for EditPage.xaml
    /// </summary>
    public partial class EditPage : Page
    {
        public EditPage()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(false, true, false);
        }
    }
}
