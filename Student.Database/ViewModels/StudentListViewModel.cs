using Student.Database.Relational;
using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;

namespace Student.Database
{
    /// <summary>
    /// Defines the properties of the list view for binding and interactivity with the database
    /// </summary>
    public class StudentListViewModel : BaseViewModel
    {

        #region Private Members

        /* To store the ID of the student before it is modified in the edit page so that the student can be updated in the database
         * Also to make sure that the ID of the student while editing is not set to that of another student
         */
        private static ulong mStudentEditingID;

        private StudentDataStore mSelectedStudent;

        private int mSelectedIndex;

        private ObservableCollection<StudentDataStore> mStudentList = new ObservableCollection<StudentDataStore>();

        private static StudentListItemViewModel mStudent = new StudentListItemViewModel();

        #endregion

        #region Public Properties

        /// <summary>
        /// The source of the items for the list view. It gets the data from the database and stores it as an observable collection. The ItemsSource property of the list view will bind to this
        /// </summary>
        public ObservableCollection<StudentDataStore> StudentList { get { return mStudentList; } set { mStudentList = value; } }

        /// <summary>
        /// This is what the text in the text boxes in the insert page will bind to. The attributes such as ID and first name will be modified there, so that <see cref="mStudent"/> will have data and can be inserted into a database
        /// </summary>
        public static StudentListItemViewModel Student { get { return mStudent; } set { mStudent = value; } } 

        /// <summary>
        /// This is what the selected item in the list view will bind to. It will allow us to edit or delete the student
        /// </summary>
        public StudentDataStore SelectedStudent { get { return mSelectedStudent; } set { mSelectedStudent = value; } }

        /// <summary>
        /// This is what the selected index in the list view will bind to. It will allow us to delete the student
        /// </summary>
        public int SelectedIndex { get { return mSelectedIndex; } set { mSelectedIndex = value; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor that creates commands and loads data from database
        /// </summary>
        public StudentListViewModel()
        {
            // Creating new instance of StudentDataStoreDbContext, to access the table (db.Students)
            using (var db = new StudentDataStoreDbContext())
            {
                // Load the data from the database into the StudentList property, and effectively list view through binding
                mStudentList = new ObservableCollection<StudentDataStore>(db.Students);
            }

            // Set up delete command
            DeleteCommand = new RelayCommand(() => DeleteItem());

            // Set up enter command
            EnterCommand = new RelayCommand(() => InsertItem());

            // Set up edit command
            EditCommand = new RelayCommand(() => EditItem());

            // Set up done command
            DoneCommand = new RelayCommand(() => Done());
           
            // Set up export command
            ExportCommand = new RelayCommand(() => 
            {
                // Show cursor as wait to indicate that an operation is going to happen
                Mouse.OverrideCursor = Cursors.Wait;

                // Execute export method
                Export();
            });
            

        }

        #endregion

        #region Public Commands

        /// <summary>
        /// This command will allow us to delete the selected item from the list view
        /// </summary>
        public ICommand DeleteCommand { get; set; }

        /// <summary>
        /// This command will allow us to create a student and insert them into the database
        /// </summary>
        public ICommand EnterCommand { get; set; }

        /// <summary>
        /// This command will allow us to edit a student from the list view and the database
        /// </summary>
        public ICommand EditCommand { get; set; }

        /// <summary>
        /// This command updates the student's details after the done button is pressed in the edit page
        /// </summary>
        public ICommand DoneCommand { get; set; }

        /// <summary>
        /// This command exports the database or the list view into an excel file that the user can edit or save
        /// </summary>
        public ICommand ExportCommand { get; set; }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Helper method that deletes item from list view
        /// </summary>
        private void DeleteItem()
        {
            try
            {
                // If there is no selected student, then show message and terminate method
                if (mSelectedStudent == null)
                {
                    // Show message
                    ShowMessage("Select a record first.");

                    // Terminate method
                    return;
                }

                // Creating new instance of StudentDataStoreDbContext, to access the table (db.Students)
                using (var db = new StudentDataStoreDbContext())
                {
                    // Declares key variable
                    int key;

                    // Puts "SelectedStudent" property that was bound to by list view into a variable
                    var selectedItem = mSelectedStudent;

                    // Puts the ID of the selected student into "key" variable
                    key = System.Convert.ToInt32(selectedItem.ID);

                    // Creating new connection that is associated with the data source that gives the database
                    using (SqliteConnection conn = new SqliteConnection(@"Data Source = StudentDB.db"))
                    {
                        // Opens the connection
                        conn.Open();

                        // Creates sql command that deletes student from the database that has the same ID as the selected student
                        SqliteCommand command = new SqliteCommand("Delete from Students where ID='" + key + "'", conn);

                        // Reads results, and executes command
                        SqliteDataReader reader = command.ExecuteReader();

                        // Closes reader
                        reader.Close();
                    }

                    // Removes the item from the list view
                    mStudentList.RemoveAt(mSelectedIndex);
                }
            }
            catch 
            { ShowMessage("An error occured. Please try again."); }           
        }

        /// <summary>
        /// Helper method that inserts item into database
        /// </summary>
        private void InsertItem()
        {           
            try
            {
                // Declaring variables
                ulong result;
                ulong result1;

                // See if ID or Mark can be parsed into an integer. If they cannot, then they are not valid data. 
                bool isValid = ulong.TryParse(mStudent.ID, out result);
                bool isValid1 = ulong.TryParse(mStudent.Mark, out result1);

                // Checks if any data typed is of incorrect format
                if (!isValid || !isValid1 || !Regex.IsMatch(mStudent.FirstName, @"^[a-zA-Z '-]+$") || !Regex.IsMatch(mStudent.LastName, @"^[a-zA-Z '-]+$") || !IsValidEmail(mStudent.Email))
                {
                    // Shows message
                    ShowMessage("Data entered is in incorrect format, or numbers are too large. Type appropriate data.");

                    // Terminates method
                    return;
                }

                // Creating new connection that is associated with the data source that gives the database
                using (SqliteConnection conn = new SqliteConnection(@"Data Source = StudentDB.db"))
                {
                    // Opens the connection
                    conn.Open();

                    // Creates new command that checks if there is another student with the ID typed in the text box
                    SqliteCommand command = new SqliteCommand("Select * from Students where ID='" + mStudent.ID + "'", conn);

                    // Reads results, and executes command
                    SqliteDataReader reader = command.ExecuteReader();

                    // If a record exists, then show a message and terminate the method
                    if (reader.Read() == true)
                    {
                        // Show a message 
                        ShowMessage("Another student exists with the ID you typed.");

                        // Closes reader
                        reader.Close();

                        // Terminate method
                        return;
                    }

                    // Closes reader
                    reader.Close();
                }

                // Creating new instance of StudentDataStoreDbContext, to access the table (db.Students)
                using (var db = new StudentDataStoreDbContext())
                {
                    // Creates the student from the "Student" property and its attributes
                    StudentDataStore item = new StudentDataStore
                    {
                        ID = mStudent.ID,
                        FirstName = mStudent.FirstName,
                        LastName = mStudent.LastName,
                        Email = mStudent.Email,
                        Mark = mStudent.Mark
                    };

                    // Adds item to database
                    db.Students.Add(item);

                    // Saves changes
                    db.SaveChanges();
                }

                // Setting properties null to empty text boxes
                mStudent.ID = null;
                mStudent.FirstName = null;
                mStudent.LastName = null;
                mStudent.Email = null;
                mStudent.Mark = null;
            }
            catch 
            { ShowMessage("An error occured. Please try again."); }            
        }

        /// <summary>
        /// Helper method for <see cref="EditCommand"/> that saves data of the selected item from the list view before page navigation
        /// </summary>
        private void EditItem()
        {
            try
            {
                // If there is no selected student, then show message and terminate method
                if (mSelectedStudent == null)
                {
                    // Show message
                    ShowMessage("Select a record first.");

                    // Terminate method
                    return;
                }

                // Puts selected student from the list view in a variable
                var selectedItem = mSelectedStudent;

                // Assigns the properties of the selected student from the list view to this class's properties again, since they were set to null after insertion
                mStudent.ID = selectedItem.ID;
                mStudentEditingID = System.Convert.ToUInt64(selectedItem.ID);
                mStudent.FirstName = selectedItem.FirstName;
                mStudent.LastName = selectedItem.LastName;
                mStudent.Email = selectedItem.Email;
                mStudent.Mark = selectedItem.Mark;

                // Navigate to edit page
                ((MainViewModel)((MainWindow)Application.Current.MainWindow).DataContext).ApplicationViewModel.CurrentPage = ApplicationPage.Edit;
            }
            catch 
            { ShowMessage("An error occured. Please try again."); }                        
        }

        /// <summary>
        /// Helper method for <see cref="DoneCommand"/> that creates an sql command for updating the student's details from the database
        /// </summary>
        private void Done()
        {
            try
            {
                // Declaring variables
                ulong result;
                ulong result1;

                // See if ID or Mark can be parsed into an integer. If they cannot, then they are not valid data. 
                bool isValid = ulong.TryParse(mStudent.ID, out result);
                bool isValid1 = ulong.TryParse(mStudent.Mark, out result1);

                // Checks if any data typed is of incorrect format
                if (!isValid || !isValid1 || !Regex.IsMatch(mStudent.FirstName, @"^[a-zA-Z '-]+$") || !Regex.IsMatch(mStudent.LastName, @"^[a-zA-Z '-]+$") || !IsValidEmail(mStudent.Email))
                {
                    // Shows message
                    ShowMessage("Data entered is in incorrect format, or numbers are too large. Type appropriate data.");

                    // Terminates method
                    return;
                }

                // Creating new connection that is associated with the data source that gives the database
                using (SqliteConnection conn = new SqliteConnection(@"Data Source = StudentDB.db"))
                {
                    // Opens the connection
                    conn.Open();

                    // Creates new command that checks if there is another student with the ID typed in the text box
                    SqliteCommand command = new SqliteCommand("Select * from Students where ID='" + mStudent.ID + "'", conn);

                    // Reads results, and executes command
                    SqliteDataReader reader = command.ExecuteReader();

                    // If a record exists, but with an ID that was not the same as the original ID, then show a message and terminate the method
                    // In other words, if the student in the database has an ID that is different from mStudentEditingID or the original ID, then the ID has been changed while editing,
                    // and therefore an attempt has been made to set the ID of the student to that of another student in the database.
                    if (reader.Read() == true)
                    {
                        /* Checks if the record has an ID that was same as the original ID. If they are not the same, then ID has been changed while editing and 
                         * so the record searched from the query is another different record, and therefore an attempt has been made to set the ID of the student to that of another student
                         */
                        if (System.Convert.ToUInt64(reader.GetValue(reader.GetOrdinal("ID"))) != mStudentEditingID)
                        {
                            // Show a message 
                            ShowMessage("Another student exists with the ID you typed.");

                            // Closes reader
                            reader.Close();

                            // Terminate method
                            return;
                        }

                    }

                    // Closes reader
                    reader.Close();
                }

                // Modifying first names and last names so that a single appostrophe will be replaced with double appostrophe (a single appostrophe is interpreted differently in SQL)
                mStudent.FirstName = mStudent.FirstName.Replace("'", "''");
                mStudent.LastName = mStudent.LastName.Replace("'", "''");

                // Creating new connection that is associated with the data source that gives the database
                using (SqliteConnection conn = new SqliteConnection(@"Data Source = StudentDB.db"))
                {
                    // Opens the connection
                    conn.Open();

                    // Creates new command that updates the student according to the data typed into the textboxes
                    SqliteCommand command = new SqliteCommand("Update Students set ID='" + mStudent.ID + "', FirstName='" + mStudent.FirstName + "', LastName='" + mStudent.LastName + "', Email='" + mStudent.Email + "', Mark='" + mStudent.Mark + "' where ID='" + mStudentEditingID + "'", conn);

                    // Reads results, and executes command
                    SqliteDataReader reader = command.ExecuteReader();

                    // Closes reader
                    reader.Close();
                }

                // Sets properties to null again so that insertion page will not have data in the text boxes
                mStudent.ID = null;
                mStudent.FirstName = null;
                mStudent.LastName = null;
                mStudent.Email = null;
                mStudent.Mark = null;

                // Navigate to load page
                ((MainViewModel)((MainWindow)Application.Current.MainWindow).DataContext).ApplicationViewModel.CurrentPage = ApplicationPage.Load;
            }
            catch
            { ShowMessage("An error occured. Please try again."); }                                  
        }

        /// <summary>
        /// Helper method for <see cref="ExportCommand"/> that exports the data in the list view or database into an excel file that can be edited or saved by the user
        /// </summary>
        private void Export()
        {
            // Creating new connection that is associated with the data source that gives the database
            using (SqliteConnection conn = new SqliteConnection(@"Data Source = StudentDB.db"))
            {
                // Opens the connection
                conn.Open();

                // Creates new command that gets all records from table
                SqliteCommand command = new SqliteCommand("SELECT * FROM Students", conn);

                // Reads results, and executes command
                SqliteDataReader reader = command.ExecuteReader();                

                // If there is no record, show a message and terminate the method
                if (reader.Read() == false)
                {
                    // Delay thread so that waiting cursor can show if there is no record
                    Thread.Sleep(125);

                    // Sets overriding of cursor to null before message can be shown if there is no record
                    Mouse.OverrideCursor = null;

                    // Show a message box
                    ShowMessage("Database is empty. Insert some records first.");

                    // Closes reader
                    reader.Close();

                    // Terminate method
                    return;
                }

                // Closes reader
                reader.Close();
            }
            try
            {
                // Since this is a long operation, use another thread to execute operation and keep UI thread alive
                Task.Run(() =>
                {
                    // Create an Excel application instance
                    Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = true;
                    // Create an Excel workbook instance 
                    Microsoft.Office.Interop.Excel.Workbook excelWorkBook = excelApp.Workbooks.Add(1);


                    // Add a new worksheet to workbook with the table name
                    Microsoft.Office.Interop.Excel.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();
                    excelWorkSheet.Name = "Students";

                    // Setting up labels that are bold
                    excelWorkSheet.Cells[1, 1] = "ID";
                    excelWorkSheet.Cells[1, 1].Font.Bold = true;

                    excelWorkSheet.Cells[1, 2] = "First Name";
                    excelWorkSheet.Cells[1, 2].Font.Bold = true;

                    excelWorkSheet.Cells[1, 3] = "Last Name";
                    excelWorkSheet.Cells[1, 3].Font.Bold = true;

                    excelWorkSheet.Cells[1, 4] = "Email";
                    excelWorkSheet.Cells[1, 4].Font.Bold = true;

                    excelWorkSheet.Cells[1, 5] = "Mark";
                    excelWorkSheet.Cells[1, 5].Font.Bold = true;

                    // Setting the column widths
                    excelWorkSheet.Columns[1].ColumnWidth = 15;
                    excelWorkSheet.Columns[2].ColumnWidth = 15;
                    excelWorkSheet.Columns[3].ColumnWidth = 15;
                    excelWorkSheet.Columns[4].ColumnWidth = 15;
                    excelWorkSheet.Columns[5].ColumnWidth = 15;

                    // Represents row number two so that data can be entered in a row that does not have labels
                    int i = 2;

                    // Creating new instance of StudentDataStoreDbContext, to access the table (db.Students)
                    using (var db = new StudentDataStoreDbContext())
                    {
                        // For each student in the table, add a student in the row number (i) with data for the properties in different columns
                        foreach (StudentDataStore student in db.Students)
                        {
                            excelWorkSheet.Cells[i, 1] = student.ID;
                            excelWorkSheet.Cells[i, 2] = student.FirstName;
                            excelWorkSheet.Cells[i, 3] = student.LastName;
                            excelWorkSheet.Cells[i, 4] = student.Email;
                            excelWorkSheet.Cells[i, 5] = student.Mark;

                            // Increment row number (i) so that the next student will be input in another row
                            i++;
                        }
                    }
                });

                // Delay the thread so that waiting cursor can show if there is a record
                Thread.Sleep(1000);

                // Set overriding back to null
                Mouse.OverrideCursor = null;

            }
            catch 
            { ShowMessage("An error occured. Please try again."); }
            

            //   excelWorkBook.Save();
            //   excelWorkBook.Close();
            //   excelApp.Quit();
        }

        /// <summary>
        /// Shows a custom message box that has a message
        /// </summary>
        /// <param name="message">message that is going to be shown in the window</param>
        private void ShowMessage(string message)
        {
            // Puts a new dialogue window in a messageBox variable
            var messageBox = new DialogueWindow();

            // Sets the content of the message box as the message parameter
            DialogueWindowViewModel.Content = message;

            // Sets the title of the message box
            DialogueWindowViewModel.Title = "Error";

            // Shows the message box as a dialogue
            messageBox.ShowDialog();
        }

        /// <summary>
        /// Checks if the email is of a valid format
        /// </summary>
        /// <param name="email">the email to be checked</param>
        /// <returns>returns true if the email is valid or false if the email is not valid</returns>
        private bool IsValidEmail(string email)
        {
            try
            {
                // Creates a new instance of MailAddress class from the email
                var addr = new System.Net.Mail.MailAddress(email);

                // If the address property of the instance is same as the email, then we know that the email is in correct format, so return true
                // Otherwise, return false
                return addr.Address == email;
            }
            catch
            {
                // If an error occured, then return false
                return false;
            }
        }

        #endregion

    }
}
