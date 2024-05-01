using System.Windows;

namespace Student.Database
{
    /// <summary>
    /// Binds all view models and combines them
    /// </summary>
    public class MainViewModel : BaseViewModel
    {

        #region Private Members

        private WindowViewModel mWindowViewModel;

        private ApplicationViewModel mApplicationViewModel;

        private StudentListItemViewModel mStudentListItemViewModel;

        private StudentListViewModel mStudentListViewModel;

        private DialogueWindowViewModel mDialogueWindowViewModel;

        #endregion

        #region Public Properties

        public WindowViewModel WindowViewModel { get { return mWindowViewModel; } }

        public ApplicationViewModel ApplicationViewModel { get { return mApplicationViewModel; } }

        public StudentListItemViewModel StudentListItemViewModel { get { return mStudentListItemViewModel; } }

        public StudentListViewModel StudentListViewModel { get { return mStudentListViewModel; } }

        public DialogueWindowViewModel DialogueWindowViewModel { get { return mDialogueWindowViewModel; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor that binds all view models 
        /// </summary>
        /// <param name="designingFunctionality">if the page is a main window</param>
        /// <param name="databaseFunctionality">if the page is related to databases</param>
        /// <param name="dialogueFunctionality">if the page is a dialogue</param>
        /// <param name="window">the window to be passed in for creating a window view model</param>
        public MainViewModel(bool designingFunctionality, bool databaseFunctionality, bool dialogueFunctionality, Window window = null)
        {  
            // Creates new application view model for every page for navigation
            mApplicationViewModel = new ApplicationViewModel();

            // Creates window view model if it is a main window
            if (designingFunctionality)
                mWindowViewModel = new WindowViewModel(window);

            // Creates student list view model if the page is related to databases
            if (databaseFunctionality)
                mStudentListViewModel = new StudentListViewModel();

            // Creates a dialogue window view model if it is a message box
            if (dialogueFunctionality)
                mDialogueWindowViewModel = new DialogueWindowViewModel(window);
        }

        #endregion

    }
}
