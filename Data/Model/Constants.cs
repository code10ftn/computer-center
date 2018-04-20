namespace Data.Model
{
    public class Constants
    {
        public enum Platform
        {
            Windows,
            Linux,
            Both,
            Any
        }

        public enum ApplicationState
        {
            Application,
            Courses,
            CoursesAdd,
            CoursesView,
            Softwares,
            SoftwaresAdd,
            SoftwaresView,
            Subjects,
            SubjectsAdd,
            SubjectsView,
            Classrooms,
            ClassroomsAdd,
            ClassroomsView,
            Schedule,
            ScheduleAdd,
            ScheduleOpen,
            ScheduleGlobal,
            SessionDetails
        }
    }
}