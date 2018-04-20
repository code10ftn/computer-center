using Data.Model;
using System.Data.Entity;

namespace Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=ComputerCenterDatabaseContext")
        {
            Database.SetInitializer(new ComputerCenterDatabaseInitializer());
        }

        public virtual DbSet<Classroom> Classrooms { get; set; }

        public virtual DbSet<Course> Courses { get; set; }

        public virtual DbSet<Schedule> Schedules { get; set; }

        public virtual DbSet<Session> Sessions { get; set; }

        public virtual DbSet<Software> Softwares { get; set; }

        public virtual DbSet<Subject> Subjects { get; set; }
    }
}