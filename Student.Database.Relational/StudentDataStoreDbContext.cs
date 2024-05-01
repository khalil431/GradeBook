using Microsoft.EntityFrameworkCore;

namespace Student.Database.Relational
{
    /// <summary>
    /// The database context for the student data store
    /// </summary>
    public class StudentDataStoreDbContext : DbContext
    {
        /// <summary>
        /// The table named "Students" for data storage from the data base
        /// </summary>
        public DbSet<StudentDataStore> Students { get; set; }

        #region Database Configuration

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Creates a database named "StudentDB.db"
            optionsBuilder.UseSqlite("Data Source = StudentDB.db");
            base.OnConfiguring(optionsBuilder);
        }

        #endregion
    }
}
