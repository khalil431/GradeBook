using Student.Database.Relational;
using System;

namespace Student.Database
{
    /// <summary>
    /// Defines the details of a student and its interactivity with the database
    /// </summary>
    public class StudentListItemViewModel : BaseViewModel
    {

        #region Private Members

        private StudentDataStore mStudent;

        #endregion

        #region Public Properties

        /// <summary>
        /// The ID of the student
        /// </summary>
        public string ID { get { return mStudent.ID; } set { mStudent.ID = value; OnPropertyChanged(nameof(ID)); } }

        /// <summary>
        /// The first name of the student
        /// </summary>
        public string FirstName { get { return mStudent.FirstName; } set { mStudent.FirstName = value; OnPropertyChanged(nameof(FirstName)); } }

        /// <summary>
        /// The last name of the student
        /// </summary>
        public string LastName { get { return mStudent.LastName; } set { mStudent.LastName = value; OnPropertyChanged(nameof(LastName)); } }

        /// <summary>
        /// The email of the student
        /// </summary>
        public string Email { get { return mStudent.Email; } set { mStudent.Email = value; OnPropertyChanged(nameof(Email)); } }

        /// <summary>
        /// The mark of the student
        /// </summary>
        public string Mark { get { return mStudent.Mark; } set { mStudent.Mark = value; OnPropertyChanged(nameof(Mark)); } }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor that creates a new student
        /// </summary>
        public StudentListItemViewModel()
        {
            if (StudentListViewModel.Student == null)
                mStudent = new StudentDataStore { ID = null, Mark = null };      
            else
            {
                mStudent = new StudentDataStore
                {
                    ID = StudentListViewModel.Student.ID,
                    FirstName = StudentListViewModel.Student.FirstName,
                    LastName = StudentListViewModel.Student.LastName,
                    Email = StudentListViewModel.Student.Email,
                    Mark = StudentListViewModel.Student.Mark
                };
            }
        }

        #endregion

    }
}
