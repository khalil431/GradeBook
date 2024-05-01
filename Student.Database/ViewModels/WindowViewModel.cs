using System.Windows;
using System.Windows.Input;

namespace Student.Database
{
    /// <summary>
    /// Defines the interactivity of the window and its behavior
    /// </summary>
    public class WindowViewModel : BaseViewModel
    {

        #region Private Members

        private Window mWindow;

        private int mOuterMarginSize= 10;

        private int mWindowRadius = 10;

        private WindowDockPosition mDockPosition = WindowDockPosition.Undocked;

        #endregion

        #region Public Properties

        /// <summary>
        /// The minimum height of the window
        /// </summary>
        public int WindowMinimumHeight { get; set; } = 500;

        /// <summary>
        /// The maximum height of the window
        /// </summary>
        public int WindowMinimumWidth { get; set; } = 500;

        /// <summary>
        /// Decides if the window is borderless based on window state and dock position
        /// </summary>
        public bool Borderless { get { return mWindow.WindowState == WindowState.Maximized || mDockPosition != WindowDockPosition.Undocked; } }

        /// <summary>
        /// Decides the resize border integer based on if the window is borderless. If the window is borderless, there is no resize border.
        /// </summary>
        public int ResizeBorder { get { return Borderless ? 0 : 6; } }

        /// <summary>
        /// The thickness of the <see cref="ResizeBorder"/>
        /// </summary>
        public Thickness ResizeBorderThickness { get { return new Thickness(ResizeBorder + OuterMarginSize); } }
        
        /// <summary>
        /// The padding of the content in the window
        /// </summary>
        public Thickness InnerContentPadding { get { return new Thickness(ResizeBorder); } }       

        /// <summary>
        /// The size of the outer margin for drop shadow
        /// </summary>
        public int OuterMarginSize
        {
            get
            {
                return Borderless ? 0 : mOuterMarginSize;
            }
            set
            {
                mOuterMarginSize = value;
            }
        }

        /// <summary>
        /// The thickness of the <see cref="OuterMarginSize"/>
        /// </summary>
        public Thickness OuterMarginSizeThickness { get { return new Thickness(OuterMarginSize); } }

        /// <summary>
        /// The corner radius of the <see cref="WindowRadius"/>
        /// </summary>
        public CornerRadius WindowCornerRadius { get { return new CornerRadius(WindowRadius); } }

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public int WindowRadius {
            get
            {
                // If it is maximized or docked, no border
                return Borderless ? 0 : mWindowRadius;
            }
            set
            {
                mWindowRadius = value;
            }
        }

        /// <summary>
        /// The height of the title bar
        /// </summary>
        public int TitleHeight { get; set; } = 42;

        /// <summary>
        /// The grid length of the <see cref="TitleHeight"/>
        /// </summary>
        public GridLength TitleHeightGridLength { get { return new GridLength(TitleHeight + ResizeBorder); } }

        #endregion

        #region Commands

        /// <summary>
        /// The command to minimize the window
        /// </summary>
        public ICommand MinimizeCommand { get; set; }

        /// <summary>
        /// The command to maximize the window
        /// </summary>
        public ICommand MaximizeCommand { get; set; }

        /// <summary>
        /// The command to close the window
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// The command to open the system menu when the window icon is clicked
        /// </summary>
        public ICommand MenuCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="window">the window that is passed in for listeing out for window resizing and fixing resize issue</param>
        public WindowViewModel(Window window)
        {
            mWindow = window;

            // Listen out for window resizing
            mWindow.StateChanged += (sender, e) =>
            {
                // Fire off events for all properties that are affected by a resize
                WindowResized();
            };

            // Create commands
            MinimizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => mWindow.Close());
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(mWindow, GetMousePosition()));

            // Fix window resize issue
            var resizer = new WindowResizer(mWindow);

            // Listen out for dock changes
            resizer.WindowDockChanged += (dock) =>
            {
                // Store last position
                mDockPosition = dock;

                // Fire off resize events
                WindowResized();
            };
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Gets the mouse position when the window icon is clicked for determining where the system menu will open
        /// </summary>
        /// <returns>returns the position of the mouse cursor</returns>
        private Point GetMousePosition()
        {
            // Gets the position of the window
            var position = Mouse.GetPosition(mWindow);

            // Gets the point where the cursor was
            return new Point(position.X + mWindow.Left, position.Y + mWindow.Top);
        }

        /// <summary>
        /// If the window resizes to a special position (docked or maximized)
        /// this will update all required property change events to set the borders and radius values
        /// </summary>
        private void WindowResized()
        {
            // Fire off events for all properties that are affected by a resize
            OnPropertyChanged(nameof(Borderless));
            OnPropertyChanged(nameof(ResizeBorderThickness));
            OnPropertyChanged(nameof(WindowRadius));
            OnPropertyChanged(nameof(WindowCornerRadius));
            OnPropertyChanged(nameof(OuterMarginSize));
            OnPropertyChanged(nameof(OuterMarginSizeThickness));            
        }

        #endregion

    }
}
