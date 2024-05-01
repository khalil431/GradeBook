using System.Windows;

namespace Student.Database
{
    /// <summary>
    /// The View Model for the custom flat window
    /// </summary>
    public class DialogueWindowViewModel : WindowViewModel
    {
        #region Public Properties

        /// <summary>
        /// The title of this dialogue window
        /// </summary>
        public static string Title { get; set; }

        /// <summary>
        /// The content to host inside the dialogue
        /// </summary>
        public static string Content { get; set; }

        /// <summary>
        /// The maximum width of the window
        /// </summary>
        public int WindowMaximumWidth { get; set; } = 400;

        /// <summary>
        /// The maximum height of the window
        /// </summary>
        public int WindowMaximumHeight { get; set; } = 300;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public DialogueWindowViewModel(Window window) : base(window)
        {
            // Make minimum size smaller
            WindowMinimumWidth = 250;
            WindowMinimumHeight = 100;

            // Make title bar smaller
            TitleHeight = 30;
        }

        #endregion
    }
}
