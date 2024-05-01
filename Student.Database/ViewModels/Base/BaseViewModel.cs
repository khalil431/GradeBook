using System.ComponentModel;

namespace Student.Database
{
    /// <summary>
    /// The base view model that implements INotifyPropertyChanged
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {

        /* IMPORTANT NOTE:
         * FodyWeaver NUGET package has been installed to detect for changes in the properties in all of the view models
         * that inherit from this base view model.
         */


        /// <summary>
        /// The event that is fired when any child property changes its value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => {};

        /// <summary>
        /// Call this to fire a <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="name">The name of the property being changed</param>
        public void OnPropertyChanged (string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
