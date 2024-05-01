using System;

namespace Student.Database.Relational
{
    /// <summary>
    /// The columns to be stored in the table for students
    /// </summary>
    public class StudentDataStore
    {
        #region Public Properties

        /// <summary>
        /// The ID of the student
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The first name of the student
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the student
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The email of the student
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The mark of the student
        /// </summary>
        public string Mark { get; set; }

        #endregion
    }
}
