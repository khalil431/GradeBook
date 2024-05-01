using Student.Database.Relational;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace Student.Database
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            using (var db = new StudentDataStoreDbContext())
            {
                // Sets up database at the start of the application
                db.Database.Migrate();
            }
        }
    }
}
