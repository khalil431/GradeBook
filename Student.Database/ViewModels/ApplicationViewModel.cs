using System.Windows;
using System.Windows.Input;

namespace Student.Database
{
    /// <summary>
    /// The application state as a view model
    /// </summary>
    public class ApplicationViewModel : BaseViewModel
    {

        #region Public Properties

        /// <summary>
        /// The current page of the application
        /// </summary>
        public ApplicationPage CurrentPage { get; set; } = ApplicationPage.Dashboard;

        /// <summary>
        /// The view model to use for the current page when the CurrentPage changes
        /// NOTE: This is not a live up-to-date view model of the current page
        ///       it is simply used to set the view model of the current page 
        ///       at the time it changes
        /// </summary>
        public BaseViewModel CurrentPageViewModel { get; set; }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Navigates to the specified page
        /// </summary>
        /// <param name="page">The page to go to</param>
        /// <param name="viewModel">The view model, if any, to set explicitly to the new page</param>
        private void GoToPage(ApplicationPage page, BaseViewModel viewModel = null)
        {

            // Set the view model
            CurrentPageViewModel = viewModel;

            // Set the current page
            ((MainViewModel)((MainWindow)Application.Current.MainWindow).DataContext).ApplicationViewModel.CurrentPage = page;

            // Fire off a CurrentPage changed event
            OnPropertyChanged(nameof(CurrentPage));

        }

        #endregion

        #region Public Commands

        /// <summary>
        /// Takes the user to the page for inserting data
        /// </summary>
        public ICommand InsertPageCommand { get; set; }

        /// <summary>
        /// Takes the user to the page for reading data from the database in the form of a list view
        /// </summary>
        public ICommand LoadPageCommand { get; set; }

        /// <summary>
        /// Takes the user to the main page or dashboard for navigating to other pages
        /// </summary>
        public ICommand DashboardPageCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor that sets up the commands
        /// </summary>
        public ApplicationViewModel()
        {
            // Shows insert page
            InsertPageCommand = new RelayCommand(() => GoToPage(ApplicationPage.Insert));

            // Shows load page for reading data from database in a list view
            LoadPageCommand = new RelayCommand(() => GoToPage(ApplicationPage.Load));

            // Shows dashboard page or main page for navigation
            DashboardPageCommand = new RelayCommand(() => GoToPage(ApplicationPage.Dashboard));           
        }

        #endregion

    }
}
