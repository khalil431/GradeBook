using System.Windows;

namespace Student.Database
{
    /// <summary>
    /// Interaction logic for DialogueWindow.xaml
    /// </summary>
    public partial class DialogueWindow : Window
    {
        public DialogueWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(true, false, true, this);
        }
    }
}
